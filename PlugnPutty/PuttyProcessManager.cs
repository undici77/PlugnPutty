/*
 * This file is part of the PlugnPutty distribution (https://github.com/undici77/PlugnPutty.git).
 * Copyright (c) 2019 Alessandro Barbieri.
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using PlugnPutty;

class PuttyProcessManager
{
	private Point _WindowsDisplacement;
	[DllImport("USER32.DLL", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool SetForegroundWindow(IntPtr hwnd);

	[DllImport("USER32.DLL", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool BringWindowToTop(IntPtr hwnd);

	[DllImport("USER32.DLL", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool MoveWindow(IntPtr hwnd, int x, int y, int width, int height, bool repaint);

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	[DllImport("USER32.DLL", SetLastError = true)]
	static extern bool GetWindowRect(IntPtr hwnd, out RECT Rect);
	private const int GET_WINDOWS_RECT_POLLING_TIME    = 10;
	private const int GET_WINDOWS_RECT_POLLING_TIMEOUT = 10000;
	private const int WAIT_ON_EXIT_TIME				   = 10;

	private const string USB_DESCRIPTOR_REGEX = "^([a-zA-Z0-9\\\\_\\&]*)\\|([a-zA-Z0-9,]*)[\\;]?";
	private const string PROCESS_NAME         = "PuTTY";

	private UsbSerialListener                      _Usb_Serial_Listener;
	private List<UsbSerialListener.USB_DESCRIPTOR> _Usb_Descriptor_List;

	private struct PROCESS_DATA
	{
		public Process process;
		public string  profile_name;
		public string  windows_title;
	};

	private Dictionary<UsbSerialListener.USB_DEVICE, PROCESS_DATA> _Process_Dictionary;

	private string _Putty_Path;
	private bool   _Enable;
	private bool   _Pause;
	private bool   _Autoclose;
	private bool   _Reopen;
	private int    _Polling_Time;
	private int    _Start_Delay;
	private int    _Term_Width;
	private int	   _Term_Height;
	private Thread _Thread;
	private object _Thread_Lock;

	/// @brief Constructor
	///
	/// @param polling_time	time of polling to check if a process is alive
	/// @param start_delay delay to create a new process
	/// @param width width of putty window
	/// @param height height of putty window
	/// @param putty_path path of putty exe
	/// @param usd_descriptor usb descriptor string ([vid_0&pid_0]|[settings];[vid_1&pid_1]|[settings])
	public PuttyProcessManager(int polling_time, int start_delay, int width, int height, string putty_path, string usd_descriptor)
	{
		UsbSerialListener.USB_DESCRIPTOR descriptor;
		Regex                            regex;
		Match                            match;

		try
		{
			_Thread_Lock         = new object();
			_Thread              = null;
			_Usb_Serial_Listener = null;

			_Usb_Descriptor_List = new List<UsbSerialListener.USB_DESCRIPTOR>();
			_Process_Dictionary  = new Dictionary<UsbSerialListener.USB_DEVICE, PROCESS_DATA>();
			_WindowsDisplacement = new Point(0, 0);

			_Putty_Path   = putty_path;
			_Enable       = false;
			_Pause        = true;
			_Autoclose    = false;
			_Reopen       = false;
			_Term_Width   = ((width  < 0) ? 0 : width);
			_Term_Height  = ((height < 0) ? 0 : height);

			_Start_Delay = start_delay;
			if (_Start_Delay < 1)
			{
				_Start_Delay = 1;
			}

			_Polling_Time = polling_time;
			if (_Polling_Time < 1)
			{
				_Polling_Time = 1;
			}

			regex = new Regex(USB_DESCRIPTOR_REGEX);
			match = regex.Match(usd_descriptor);
			do
			{
				descriptor.vid_pid  = match.Groups[1].ToString();
				descriptor.settings = match.Groups[2].ToString();

				_Usb_Descriptor_List.Add(descriptor);

				usd_descriptor = usd_descriptor.Replace(match.Groups[0].ToString(), "");

				match = regex.Match(usd_descriptor);
			}
			while (match.Success);
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
		}
	}

	/// @brief Start of the procedure
	///
	private void Start()
	{
		lock (_Thread_Lock)
		{
			if (_Usb_Serial_Listener == null)
			{
				_Usb_Serial_Listener = new UsbSerialListener(_Polling_Time, _Usb_Descriptor_List, OnAdded, OnRemoved);
			}

			if (_Thread == null)
			{
				_Thread = new Thread(ProcessPollingThread);
				_Thread.Start();
			}
		}
	}

	/// @brief Stop of the procedure
	///
	public void Stop()
	{
		lock (_Thread_Lock)
		{
			if (_Thread != null)
			{
				_Thread.Interrupt();
				_Thread.Join();
				_Thread = null;
			}

			if (_Usb_Serial_Listener != null)
			{
				_Usb_Serial_Listener.Stop();
				_Usb_Serial_Listener = null;
			}
		}
	}

	/// @brief Polling procedure of putty process
	///
	private void ProcessPollingThread()
	{
		Dictionary<UsbSerialListener.USB_DEVICE, PROCESS_DATA> dictionary;

		Log.Instance.Debug("PuttyProcessManager Start");

		try
		{
			while (true)
			{
				lock (_Process_Dictionary)
				{
					dictionary = new Dictionary<UsbSerialListener.USB_DEVICE, PROCESS_DATA>(_Process_Dictionary);
				}

				foreach (KeyValuePair<UsbSerialListener.USB_DEVICE, PROCESS_DATA> pair in dictionary)
				{
					try
					{
						if (pair.Value.process.WaitForExit(WAIT_ON_EXIT_TIME))
						{
							Log.Instance.Info(pair.Value.windows_title + " - Closed");
							if (_Reopen)
							{
								_Usb_Serial_Listener.Remove(pair.Key);
							}

							pair.Value.process.Close();
						}
					}
					catch (ThreadInterruptedException ex)
					{
						throw (ex);
					}
					catch
					{
						try
						{
							if (_Reopen)
							{
								_Usb_Serial_Listener.Remove(pair.Key);
							}
						}
						catch
						{
						}
					}
				}

				Thread.Sleep(_Polling_Time);
			}
		}
		catch (ThreadInterruptedException)
		{
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
		}

		Log.Instance.Debug("PuttyProcessManager Stop");
	}

	/// @brief Event of new device detected
	///
	/// @param usb_device usb device descriptor
	/// @retval	true device added successfully, false device added error
	private bool OnAdded(UsbSerialListener.USB_DEVICE usb_device)
	{
		SerialPortParameters parameters;
		PROCESS_DATA         process_data;

		try
		{
			parameters = new SerialPortParameters(usb_device.name, usb_device.descriptor.settings);

			process_data.profile_name = "PlugnPutty" + parameters.Port;
			process_data.windows_title = parameters.Port + " - " + PROCESS_NAME;
			process_data.process       = FindProcess(PROCESS_NAME, process_data.windows_title);

			if (process_data.process == null)
			{
				process_data.process = CreateProccess(process_data.profile_name, parameters);
				if (process_data.process == null)
				{
					Log.Instance.Info(process_data.windows_title + " - Creation error");
				}
				else
				{
					Log.Instance.Info(process_data.windows_title + " - Created");
				}
			}
			else
			{
				Log.Instance.Info(process_data.windows_title + " - Added");
			}

			try
			{
				lock (_Process_Dictionary)
				{
					_Process_Dictionary.Add(usb_device, process_data);
				}
			}
			catch (Exception)
			{
			}

			parameters = null;

			return (true);
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
		}

		return (false);
	}

	/// @brief Event of device removed
	///
	/// @param usb_device usb device descriptor
	private void OnRemoved(UsbSerialListener.USB_DEVICE usb_device)
	{
		Process process;

		try
		{
			if (_Autoclose)
			{
				if (_Process_Dictionary.ContainsKey(usb_device))
				{
					process = _Process_Dictionary[usb_device].process;

					Log.Instance.Info(_Process_Dictionary[usb_device].windows_title + " - Killed");

					try
					{
						process.Kill();
					}
					catch
					{
					}

					try
					{
						lock (_Process_Dictionary)
						{
							_Process_Dictionary.Remove(usb_device);
						}
					}
					catch
					{
					}
				}
			}
		}
		catch
		{
			try
			{
				lock (_Process_Dictionary)
				{
					_Process_Dictionary.Remove(usb_device);
				}
			}
			catch
			{
			}
		}
	}

	/// @brief Create a putty process
	///
	/// @param profile_name name of putty profile
	/// @param parameters serial port parameters
	/// @retval reference of creared process or null
	private Process CreateProccess(string profile_name, SerialPortParameters parameters)
	{
		ProcessStartInfo start_info;
		Process          process;
		PuttyProfile     profile;
		int              counts;
		RECT             rect;
		int              width;
		int              height;
		int              x;
		int              y;
		Rectangle        top_left_bounds;
		Rectangle        top_right_bounds;
		Rectangle        central_bounds;
		Rectangle        bottom_left_bounds;
		Rectangle        bottom_right_bounds;
		Rectangle        bounds;

		try
		{
			profile                    = new PuttyProfile(profile_name);

			profile.LogFileName        = App.LogPath + @"Putty_&Y&M&D_&T.txt";
			profile.TermWidth          = (uint)_Term_Width;
			profile.TermHeight         = (uint)_Term_Height;
			profile.SerialLine         = parameters.Port;
			profile.SerialSpeed        = (uint)parameters.Baudrate;
			profile.SerialDataBits     = (uint)parameters.DataBits;
			profile.SerialStopHalfbits = ((uint)parameters.StopBits * 2);
			profile.SerialParity       = (uint)parameters.Parity;

			start_info = new ProcessStartInfo(_Putty_Path);
			start_info.Arguments = " -load " + profile_name;

			Thread.Sleep(_Start_Delay);

			process = Process.Start(start_info);
			if (process != null)
			{
				counts = 0;
				while (!GetWindowRect(process.MainWindowHandle, out rect) && (counts < GET_WINDOWS_RECT_POLLING_TIMEOUT))
				{
					Thread.Sleep(GET_WINDOWS_RECT_POLLING_TIME);
					counts += GET_WINDOWS_RECT_POLLING_TIME;
				}

				if (!GetWindowRect(process.MainWindowHandle, out rect))
				{
					return (null);
				}

				width = (rect.right - rect.left);
				height = (rect.bottom - rect.top);

				x = Cursor.Position.X;
				y = Cursor.Position.Y;

				top_left_bounds     = new Rectangle(x, y, width, height);
				top_right_bounds    = new Rectangle(x, y - height, width, height);
				central_bounds      = new Rectangle(x - (width / 2), y - (height / 2), width, height);
				bottom_left_bounds  = new Rectangle(x - width, y, width, height);
				bottom_right_bounds = new Rectangle(x - width, y - height, width, height);

				bounds = new Rectangle(0, 0, width, height);

				foreach (Screen scrn in Screen.AllScreens)
				{
					if (scrn.Bounds.Contains(central_bounds))
					{
						bounds = central_bounds;

						bounds.X += _WindowsDisplacement.X;
						bounds.Y += _WindowsDisplacement.Y;

						_WindowsDisplacement.X += 20;
						if (_WindowsDisplacement.X > (width / 3))
						{
							_WindowsDisplacement.X = 0;
						}

						_WindowsDisplacement.Y += 20;
						if (_WindowsDisplacement.Y > (height / 3))
						{
							_WindowsDisplacement.Y = 0;
						}
						break;
					}
					else if (scrn.Bounds.Contains(top_left_bounds))
					{
						bounds = top_left_bounds;
						break;
					}
					else if (scrn.Bounds.Contains(bottom_right_bounds))
					{
						bounds = bottom_right_bounds;
						break;
					}
					else if (scrn.Bounds.Contains(bottom_left_bounds))
					{
						bounds = bottom_left_bounds;
						break;
					}
					else if (scrn.Bounds.Contains(top_right_bounds))
					{
						bounds = top_right_bounds;
						break;
					}
				}

				MoveWindow(process.MainWindowHandle, bounds.X, bounds.Y, width, height, true);

				SetForegroundWindow(process.MainWindowHandle);
				BringWindowToTop(process.MainWindowHandle);
			}

			start_info = null;

			return (process);
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
			return (null);
		}
	}

	/// @brief Search a process
	///
	/// @param process_name name of the process
	/// @param windows_title windows title
	/// @retval reference of found process or null
	private Process FindProcess(string process_name, string windows_title)
	{
		Process[] process_list;

		try
		{
			process_list = Process.GetProcessesByName("PuTTY");
			foreach (Process process in process_list)
			{
				if (process.MainWindowTitle.Contains(windows_title))
				{
					return (process);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
		}

		return (null);
	}

	/// @brief Get or set if procedure is enable
	///
	public bool Enable
	{
		get
		{
			return (_Enable);
		}

		set
		{
			_Enable = value;

			if (_Enable && !_Pause)
			{
				Start();
			}
			else
			{
				Stop();
			}
		}
	}

	/// @brief Get or set if procedure is in puase
	///
	public bool Pause
	{
		get
		{
			return (_Pause);
		}

		set
		{
			_Pause = value;

			if (_Enable && !_Pause)
			{
				Start();
			}
			else
			{
				Stop();
			}
		}
	}

	/// @brief Get or set if when usb is unplugged putty process is close automatically
	///
	public bool Autoclose
	{
		get
		{
			return (_Autoclose);
		}

		set
		{
			_Autoclose = value;
		}
	}

	/// @brief Get or set if when a putty process is closed but  usb is aready plugged the process will reopen
	///
	public bool Reopen
	{
		get
		{
			return (_Reopen);
		}

		set
		{
			_Reopen = value;
		}
	}
}
