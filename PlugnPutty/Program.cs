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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace PlugnPutty
{
	static class App
	{
		[DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
		private static extern uint TimeBeginPeriod(uint uMilliseconds);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetForegroundWindow(IntPtr hWnd);

		private const int SW_SHOW_NORMAL    = 1;
		private const int SW_SHOW_MAXIMIZED = 3;
		private const int SW_RESTORE       = 9;

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		static MainForm _Main_Window;
		/// @brief Get the instance of the main window
		static public MainForm Instance
		{
			get
			{
				return (_Main_Window);
			}
		}

		static string _Version;
		/// @brief Get the version of the software
		static public string Version
		{
			get
			{
				return (_Version);
			}
		}

		static string _Name;
		/// @brief Get the name of the software
		static public string Name
		{
			get
			{
				return (_Name);
			}
		}

		static string _Path;
		/// @brief Get the path of exe file
		static public string Path
		{
			get
			{
				return (_Path);
			}
		}

		/// @brief Get the path where log are saved
		static public string LogPath
		{
			get
			{
				return (_Path + "Log\\" + _Name + "\\");
			}
		}

		[STAThread]
		/// @brief Entry point
		///
		/// @param args arguments
		static void Main(string[] args)
		{
			bool            create_new;
			string          process_name;
			Assembly        assembly;
			FileVersionInfo file_version_info;
			Process         current;

			// Set process timers granularity to 1ms
			TimeBeginPeriod(1);

			// Get process exe path
			process_name = System.IO.Directory.GetCurrentDirectory() + " - " + Application.ProductName;
			process_name = process_name.Replace("\\", "");
			process_name = process_name.Replace(".", "");
			process_name = process_name.Replace(":", "");
			for (int i = 0; i < args.Length; i++)
			{
				process_name += " - " + args[i];
			}

			create_new = true;
			// Check if the process is already in execution
			using (Mutex mutex = new Mutex(true, process_name, out create_new))
			{
				if (create_new)
				{
					// Create new process
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);

					assembly = Assembly.GetExecutingAssembly();
					file_version_info = FileVersionInfo.GetVersionInfo(assembly.Location);
					_Version = file_version_info.FileVersion;
					_Name = file_version_info.ProductName;
					_Path = System.IO.Path.GetDirectoryName(file_version_info.FileName) + "\\";

					_Main_Window = new MainForm();
					Application.Run(_Main_Window);
					_Main_Window = null;
				}
				else
				{
					// Show process already created
					current = Process.GetCurrentProcess();
					foreach (Process process in Process.GetProcessesByName(current.ProcessName))
					{
						if (process.Id != current.Id)
						{
							ShowWindow(process.MainWindowHandle, SW_RESTORE);
							SetForegroundWindow(process.MainWindowHandle);

							return;
						}
					}
				}
			}
		}
	}
}
