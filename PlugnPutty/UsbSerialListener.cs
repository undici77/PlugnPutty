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
using System.Management;
using System.Threading;

public class UsbSerialListener
{
	public struct USB_DESCRIPTOR
	{
		public string vid_pid;
		public string settings;
	};

	public struct USB_DEVICE
	{
		public string         name;
		public USB_DESCRIPTOR descriptor;
	};

	public delegate bool ON_ADDED(USB_DEVICE usb_device);
	public delegate void ON_REMOVED(USB_DEVICE usb_device);

	private event ON_ADDED   _On_Added;
	private event ON_REMOVED _On_Removed;

	private List<USB_DESCRIPTOR> _Usb_Descriptor_List;
	private Thread               _Thread;
	private List<USB_DEVICE>     _Current_Device_List;

	private int                  _Polling_Time;

	/// @brief Constructor
	///
	/// @param polling_time	time of polling to check if new usb is plugged or unplugged
	/// @param usb_descriptor_list list of USB_DESCRIPTOR
	/// @param on_add delegate of add event
	/// @param on_remove delegate of remove event
	public UsbSerialListener(int polling_time, List<USB_DESCRIPTOR> usb_descriptor_list, ON_ADDED on_add, ON_REMOVED on_remove)
	{
		_Usb_Descriptor_List = usb_descriptor_list;
		_Current_Device_List = new List<USB_DEVICE>();

		_Polling_Time = polling_time;
		if (_Polling_Time < 1)
		{
			_Polling_Time = 1;
		}

		_On_Added   += on_add;
		_On_Removed += on_remove;

		_Thread = new Thread(PollingThread);
		_Thread.Start();
	}

	/// @brief Stop polling procedure
	///
	public void Stop()
	{
		_Thread.Interrupt();
		_Thread.Join();
		_Thread = null;
	}

	/// @brief Polling procedure of lug in / plug out detection
	///
	private void PollingThread()
	{
		int              id;
		USB_DEVICE       device;
		List<USB_DEVICE> added_device_list;
		List<USB_DEVICE> device_list;

		Log.Instance.Debug("UsbSerialListener Start");

		try
		{
			while (true)
			{
				device_list       = GetComPort();
				added_device_list = device_list;

				lock (_Current_Device_List)
				{
					for (id = 0; id < device_list.Count; id++)
					{
						device = device_list[id];
						if (!_Current_Device_List.Contains(device))
						{
							if (!_On_Added(device))
							{
								added_device_list.Remove(device);
							}
						}
					}

					for (id = 0; id < _Current_Device_List.Count; id++)
					{
						device = _Current_Device_List[id];
						if (!device_list.Contains(device))
						{
							_On_Removed(device);
						}
					}

					_Current_Device_List = added_device_list;
				}

				device_list       = null;
				added_device_list = null;

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

		Log.Instance.Debug("UsbSerialListener Stop");
	}

	/// @brief Get list of available COM port
	///
	/// @retval	lisi of available device
	private List<USB_DEVICE> GetComPort()
	{
		try
		{
			ManagementScope          scope;
			SelectQuery              query;
			ManagementObjectSearcher searcher;
			HashSet<string>          hash_set;
			USB_DEVICE               device;
			List<USB_DEVICE>         device_list;
			string                   vid_pid;
			string                   com_port;

			scope       = new ManagementScope();
			query       = new SelectQuery("SELECT * FROM Win32_SerialPort");
			searcher    = new ManagementObjectSearcher(scope, query);
			hash_set    = new HashSet<string>();
			device_list = new List<USB_DEVICE>();

			foreach (ManagementObject item in searcher.Get())
			{
				vid_pid  = item["PNPDeviceID"].ToString();
				com_port = item["DeviceID"].ToString();

				foreach (USB_DESCRIPTOR descriptor in _Usb_Descriptor_List)
				{
					if (vid_pid.Contains(descriptor.vid_pid))
					{
						device.name       = com_port;
						device.descriptor = descriptor;

						device_list.Add(device);
					}
				}
			}

			scope    = null;
			query    = null;
			searcher = null;
			hash_set = null;

			return (device_list);
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
		}

		return (null);
	}

	/// @brief Remove a device from list
	///
	/// @param usb_device usb device descriptor
	public void Remove(USB_DEVICE usb_device)
	{
		try
		{
			lock (_Current_Device_List)
			{
				_On_Removed(usb_device);
				_Current_Device_List.Remove(usb_device);
			}
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
		}
	}
}
