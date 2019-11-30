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

using PlugnPutty;
using System;
using System.Diagnostics;
using System.Threading;

class ExternalProcessMonitor
{
	private bool           _Found;
	private Thread         _Thread;
	private int            _Polling_Time;
	private string[]       _Process_Name_List;
	private STATUS_CHANGED _Status_Changed;

	public delegate void STATUS_CHANGED(bool found);

	/// @brief Constructor
	///
	/// @param polling_time time of polling
	/// @param process_name_list list of process to monitor
	/// @param status_changed callback to notify process monitor status changed
	public ExternalProcessMonitor(int polling_time, string[] process_name_list, STATUS_CHANGED status_changed)
	{
		_Found             = true;
		_Process_Name_List = process_name_list;
		_Status_Changed    = new STATUS_CHANGED(status_changed);
		_Polling_Time      = polling_time;
		_Thread            = new Thread(ProcessPollingThread);

		_Thread.Start();
	}

	/// @brief Polling of process to monitor
	///
	private void ProcessPollingThread()
	{
		bool found;

		Log.Instance.Debug("ExternalProcessMonitor Start");

		try
		{
			App.Instance.BeginInvoke(_Status_Changed, _Found);
			while (true)
			{
				found = FindProcess();
				if (_Found != found)
				{
					_Found = found;
					App.Instance.BeginInvoke(_Status_Changed, _Found);
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

		Log.Instance.Debug("ExternalProcessMonitor Stop");
	}

	/// @brief Search if at least one process is in execution
	///
	/// @retval true at least one process is in execution, false no process are in execution
	private bool FindProcess()
	{
		Process[] process_list;

		try
		{
			foreach (string process_name in _Process_Name_List)
			{
				process_list = Process.GetProcessesByName(process_name);
				if (process_list.Length > 0)
				{
					return (true);
				}
			}

			return (false);
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
		}

		return (false);
	}

	/// @brief Stop thread execution
	///
	public void Stop()
	{
		if (_Thread != null)
		{
			_Thread.Interrupt();
			_Thread = null;
		}
	}
}
