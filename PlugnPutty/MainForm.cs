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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.IO;

namespace PlugnPutty
{
	public partial class MainForm : Form
	{
		private const int              LOG_LIST_BOX_MAX_ITEM = 1000;

		private IniFile                _Ini_File;
		private string                 _Ini_File_Path;
		private string                 _Putty_Path;
		private Icon                   _Enable_Icon;
		private Icon                   _Disable_Icon;
		private bool                   _Start_Minimized;
		private bool                   _Force_Closing;
		private object                 _Lock;

		private ContextMenu            _Tray_Icon_Context_Menu;
		private MenuItem               _Menu_Item_Enable;
		private MenuItem               _Menu_Item_Autoclose;
		private MenuItem               _Menu_Item_Reopen;

		private PuttyProcessManager    _Putty_Process_Manager;
		private ExternalProcessMonitor _External_Process_Monitor;
		private Autostart              _Autostart;

		/// @brief Form constructor
		///
		public MainForm()
		{
			InitializeComponent();

			Log.Instance.Init(App.LogPath, AppendLog);
			_Autostart = new Autostart(App.Name,  Application.ExecutablePath.ToString());

			_Ini_File_Path = App.Path + App.Name + ".ini";

			_Lock = new object();
		}

		/// @brief Form load event
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void MainWindow_Load(object sender, EventArgs e)
		{
			string                           buffer;
			ContextMenuStrip                 context_menu;
			ToolStripItem                    item;
			string                           usb_descriptor;
			int                              polling_time;
			int                              start_delay;
			int                              log_level;
			int                              putty_height;
			int                              putty_width;

			// Form title
			Text = App.Name + " " + App.Version;

			// Mouse right click context menu
			context_menu = new ContextMenuStrip();
			item = context_menu.Items.Add("Copy");
			item.Click += new EventHandler(CopyToClipboard_Event);
			item = context_menu.Items.Add("List usb devices");
			item.Click += new EventHandler(PrintListDevice_Event);

			ContextMenuStrip = context_menu;

			// Ini file init
			_Ini_File = new IniFile();
			_Ini_File.Load(_Ini_File_Path);

			// Minimize dialog if needed
			buffer = _Ini_File.GetKeyValue("General", "StartMinimized");
			if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer))
			{
				buffer = "1";
				_Ini_File.SetKeyValue("General", "StartMinimized", buffer);
			}
			_Start_Minimized = (buffer == "1");

			if (_Start_Minimized)
			{
				WindowState = FormWindowState.Minimized;
			}

			// Setup Log parameters
			buffer = _Ini_File.GetKeyValue("Log", "Level");
			if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer) || !int.TryParse(buffer, out log_level))
			{
				log_level = Log.INFO_LEVEL;
				_Ini_File.SetKeyValue("Log", "Level", log_level.ToString());
			}
			Log.Instance.Level = log_level;

			buffer = _Ini_File.GetKeyValue("Log", "ToFile");
			if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer))
			{
				buffer = "0";
				_Ini_File.SetKeyValue("Log", "ToFile", buffer);
			}
			Log.Instance.ToFile = (buffer == "1");

			// Setup PuttyProcessManager parameters
			_Putty_Path = _Ini_File.GetKeyValue("Putty", "Exe");
			if ((_Putty_Path == null) || (_Putty_Path == ""))
			{
				_Putty_Path = @"putty.exe";

				_Ini_File.SetKeyValue("Putty", "Exe", _Putty_Path);
			}

			// if exe not exist, open a FileDialog
			if (!File.Exists(_Putty_Path))
			{
				OpenFileDialog dialog;

				dialog = new OpenFileDialog();

				dialog.Title = "Putty EXE file";
				dialog.InitialDirectory = _Putty_Path;

				dialog.Filter = "profile files (*.exe)|*.exe";
				if (dialog.ShowDialog(this) != DialogResult.OK)
				{
					Application.Exit();
					Log.Instance.End();

					return;
				}

				_Putty_Path = dialog.FileName;

				_Ini_File.SetKeyValue("Putty", "Exe", _Putty_Path);
			}

			buffer = _Ini_File.GetKeyValue("Putty", "StartDelay");
			if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer) || !int.TryParse(buffer, out start_delay))
			{
				start_delay = 10;
				_Ini_File.SetKeyValue("Putty", "StartDelay", start_delay.ToString());
			}

			buffer = _Ini_File.GetKeyValue("Putty", "TermHeight");
			if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer) || !int.TryParse(buffer, out putty_height))
			{
				putty_height = 21;
				_Ini_File.SetKeyValue("Putty", "TermHeight", putty_height.ToString());
			}

			buffer = _Ini_File.GetKeyValue("Putty", "TermWidth");
			if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer) || !int.TryParse(buffer, out putty_width))
			{
				putty_width = 86;
				_Ini_File.SetKeyValue("Putty", "TermWidth", putty_width.ToString());
			}

			usb_descriptor = _Ini_File.GetKeyValue("Usb", "Descriptor");
			if (string.IsNullOrEmpty(usb_descriptor) || string.IsNullOrWhiteSpace(usb_descriptor))
			{
				usb_descriptor = @"USB\VID_0483&PID_5740|115200,8,None,1,None";
				_Ini_File.SetKeyValue("Usb", "Descriptor", usb_descriptor);
			}

			buffer = _Ini_File.GetKeyValue("Usb", "PollingTime");
			if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer) || !int.TryParse(buffer, out polling_time))
			{
				polling_time = 10;
				_Ini_File.SetKeyValue("Usb", "PollingTime", polling_time.ToString());
			}

			_Putty_Process_Manager = new PuttyProcessManager(polling_time, start_delay, putty_width, putty_height, _Putty_Path, usb_descriptor);

			// Setup ExternalProcessMonitor parameters
			buffer = _Ini_File.GetKeyValue("ExternalProcessMonitor", "PollingTime");
			if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer) || !int.TryParse(buffer, out polling_time))
			{
				polling_time = 1000;
				_Ini_File.SetKeyValue("ExternalProcessMonitor", "PollingTime", polling_time.ToString());
			}

			buffer = _Ini_File.GetKeyValue("ExternalProcessMonitor", "ProcessName");
			if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer))
			{
				buffer = "PComm Terminal Emulator";
				_Ini_File.SetKeyValue("ExternalProcessMonitor", "ProcessName", buffer);
			}

			_External_Process_Monitor = new ExternalProcessMonitor(polling_time, buffer.Split(';'), ExternalProcessMonitor_Changed);

			// Setup Application parameters
			buffer = _Ini_File.GetKeyValue("General", "Autoclose");
			if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer))
			{
				buffer = "1";

				_Ini_File.SetKeyValue("General", "Autoclose", buffer);
			}
			_Putty_Process_Manager.Autoclose = (buffer == "1");

			buffer = _Ini_File.GetKeyValue("General", "Reopen");
			if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer))
			{
				buffer = "0";
				_Ini_File.SetKeyValue("General", "Reopen", buffer);
			}
			_Putty_Process_Manager.Reopen = (buffer == "1");

			buffer = _Ini_File.GetKeyValue("General", "Enable");
			if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer))
			{
				buffer = "1";

				_Ini_File.SetKeyValue("General", "Enable", buffer);
			}
			_Putty_Process_Manager.Enable = (buffer == "1");

			_Tray_Icon_Context_Menu = new ContextMenu();

			AutostartCheckBox.Checked = _Autostart.Enable;

			// Setup TrayIcon objects
			_Menu_Item_Enable    = new MenuItem("Enable", TrayIconEnable_Changed);
			_Menu_Item_Autoclose = new MenuItem("Autoclose", TrayIconAutoclose_Changed);
			_Menu_Item_Reopen    = new MenuItem("Reopen", TrayIconReopen_Changed);

			_Tray_Icon_Context_Menu.MenuItems.Add(_Menu_Item_Enable);
			_Tray_Icon_Context_Menu.MenuItems.Add(_Menu_Item_Autoclose);
			_Tray_Icon_Context_Menu.MenuItems.Add(_Menu_Item_Reopen);
			_Tray_Icon_Context_Menu.MenuItems.Add("Exit", TrayIconExit_Changed);

			TrayNotify.Visible     = true;
			TrayNotify.ContextMenu = _Tray_Icon_Context_Menu;

			_Force_Closing = false;

			// Save ini file
			_Ini_File.Save(_Ini_File_Path);

			_Enable_Icon = Icon.FromHandle(Resource.PlugnPuttyEnable.GetHicon());
			_Disable_Icon = Icon.FromHandle(Resource.PlugnPuttyDisable.GetHicon());

			UpdateControls();
		}

		/// @brief Update all controls contained in form, tray icon
		///
		private void UpdateControls()
		{
			Icon icon;

			try
			{
				icon = (_Putty_Process_Manager.Enable && !_Putty_Process_Manager.Pause) ? _Enable_Icon : _Disable_Icon;

				TrayNotify.Icon = icon;
				Icon            = icon;

				TrayNotify.Text           = App.Name + " " + App.Version + (_Putty_Process_Manager.Enable ? " - Enable" : " - Disable");
				TrayNotify.BalloonTipText = App.Name + " " + App.Version + (_Putty_Process_Manager.Enable ? " - Enable" : " - Disable");
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}

			try
			{
				EnableCheckBox.Checked         = _Putty_Process_Manager.Enable;
				AutocloseCheckBox.Enabled      = _Putty_Process_Manager.Enable;
				StartMinimizedCheckBox.Enabled = _Putty_Process_Manager.Enable;
				ReopenCheckBox.Enabled         = _Putty_Process_Manager.Enable;

				AutocloseCheckBox.Checked      = _Putty_Process_Manager.Autoclose;
				StartMinimizedCheckBox.Checked = _Start_Minimized;
				ReopenCheckBox.Checked         = _Putty_Process_Manager.Reopen;

				_Menu_Item_Autoclose.Enabled   = _Putty_Process_Manager.Enable;
				_Menu_Item_Reopen.Enabled      = _Putty_Process_Manager.Enable;

				_Menu_Item_Autoclose.Checked   = _Putty_Process_Manager.Autoclose;
				_Menu_Item_Reopen.Checked      = _Putty_Process_Manager.Reopen;
				_Menu_Item_Enable.Checked      = _Putty_Process_Manager.Enable;
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief Form closing event
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if ((e.CloseReason == CloseReason.UserClosing) && !_Force_Closing)
				{
					e.Cancel    = true;
					WindowState = FormWindowState.Minimized;
				}
				else
				{
					_Putty_Process_Manager.Stop();
					_External_Process_Monitor.Stop();

					Log.Instance.End();
				}
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief Form resize event
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void MainWindow_Resize(object sender, EventArgs e)
		{
			try
			{
				if (WindowState == FormWindowState.Minimized)
				{
					Hide();

					ShowInTaskbar = false;
					Visible       = false;
				}
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief Form double click event
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void TrayNotify_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				Show();
				WindowState = FormWindowState.Normal;

				ShowInTaskbar = true;
				Visible       = true;
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief Externa monitor notification status changed
		///
		/// @param found true if at least one process is found, false if no process is found
		private void ExternalProcessMonitor_Changed(bool found)
		{
			try
			{
				_Putty_Process_Manager.Pause = found;

				UpdateControls();
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief EnableCheckBox value changed
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void EnableCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				_Putty_Process_Manager.Enable = EnableCheckBox.Checked;

				_Ini_File.SetKeyValue("General", "Enable", _Putty_Process_Manager.Enable ? "1" : "0");

				_Ini_File.Save(_Ini_File_Path);

				UpdateControls();
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief AutocloseCheckBox value changed
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void AutocloseCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				_Putty_Process_Manager.Autoclose = AutocloseCheckBox.Checked;

				_Ini_File.SetKeyValue("General", "Autoclose", _Putty_Process_Manager.Autoclose ? "1" : "0");

				_Ini_File.Save(_Ini_File_Path);

				UpdateControls();
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief StartMinimizedCheckBox value changed
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void StartMinimizedCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				_Start_Minimized = StartMinimizedCheckBox.Checked;

				_Ini_File.SetKeyValue("General", "StartMinimized", _Start_Minimized ? "1" : "0");

				_Ini_File.Save(_Ini_File_Path);

				UpdateControls();
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief ReopenCheckBox value changed
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void ReopenCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				_Putty_Process_Manager.Reopen = ReopenCheckBox.Checked;

				_Ini_File.SetKeyValue("General", "Reopen", _Putty_Process_Manager.Reopen ? "1" : "0");

				_Ini_File.Save(_Ini_File_Path);

				UpdateControls();
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief AutostartCheckBox value changed
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void AutostartCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				_Autostart.Enable = AutostartCheckBox.Checked;
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief TrayIconEnable value changed
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void TrayIconEnable_Changed(object sender, EventArgs e)
		{
			MenuItem item;

			try
			{
				item = ((MenuItem)sender);
				item.Checked = !item.Checked;

				_Putty_Process_Manager.Enable = item.Checked;

				UpdateControls();
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief TrayIconAutoclose value changed
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void TrayIconAutoclose_Changed(object sender, EventArgs e)
		{
			MenuItem item;

			try
			{
				item = ((MenuItem)sender);
				item.Checked = !item.Checked;

				_Putty_Process_Manager.Autoclose = item.Checked;

				UpdateControls();
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief TrayIconReopen value changed
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void TrayIconReopen_Changed(object sender, EventArgs e)
		{
			MenuItem item;

			try
			{
				item = ((MenuItem)sender);
				item.Checked = !item.Checked;

				_Putty_Process_Manager.Reopen = item.Checked;

				UpdateControls();
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief TrayIconExit value changed
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void TrayIconExit_Changed(object sender, EventArgs e)
		{
			try
			{
				_Force_Closing = true;

				Close();
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief PrintListDevice event
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void PrintListDevice_Event(object sender, EventArgs e)
		{
			ManagementScope          connection_scope;
			SelectQuery              serial_query;
			ManagementObjectSearcher searcher;
			HashSet<string>          list;

			try
			{
				connection_scope = new ManagementScope();
				serial_query     = new SelectQuery("SELECT * FROM Win32_SerialPort");
				searcher         = new ManagementObjectSearcher(connection_scope, serial_query);
				list             = new HashSet<string>();

				Log.Instance.Info("----------------------------------------------------------");
				foreach (ManagementObject item in searcher.Get())
				{
					string vid_pid = item["PNPDeviceID"].ToString();
					string com_port = item["DeviceID"].ToString();

					Log.Instance.Info(com_port + " - " + vid_pid);
				}
				Log.Instance.Info("----------------------------------------------------------");
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief CopyToClipboard event
		///
		/// @param sender object sender of event
		/// @param e args of event
		private void CopyToClipboard_Event(object sender, EventArgs e)
		{
			try
			{
				if (LogListBox.SelectedItems.Count > 0)
				{
					StringBuilder builder = new StringBuilder();

					foreach (int id in LogListBox.SelectedIndices)
					{
						builder.AppendLine(LogListBox.Items[id].ToString().Trim());
					}

					Clipboard.SetText(builder.ToString());
				}
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}

		/// @brief Add message to LogListBox
		///
		/// @param message messge to add
		private void AppendLog(string message)
		{
			try
			{
				if (LogListBox.Items.Count > LOG_LIST_BOX_MAX_ITEM)
				{
					LogListBox.Items.RemoveAt(0);
				}

				LogListBox.Items.Add(message);
				LogListBox.SelectedIndex = (LogListBox.Items.Count - 1);
			}
			catch
			{
			}
		}
	}
}
