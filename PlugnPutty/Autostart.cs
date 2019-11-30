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

using Microsoft.Win32;
using System;

class Autostart
{
	private string _Name;
	private string _Path;

	/// @brief Contructor
	///
	/// @param name	application name
	/// @param path	application binary full path
	public Autostart(string name, string path)
	{
		_Name = name;
		_Path = path;
	}

	/// @brief Getter and Setter of feature enable
	///
	public bool Enable
	{
		get
		{
			RegistryKey reg_key;
			object      path;

			try
			{
				reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
				path = reg_key.GetValue(_Name);

				if (path == null)
				{
					return (false);
				}

				return (path.ToString() == _Path);
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}

			return (false);
		}

		set
		{
			RegistryKey reg_key;

			try
			{
				reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
				if (value)
				{
					reg_key.SetValue(_Name, _Path);
				}
				else
				{
					reg_key.DeleteValue(_Name);
				}
			}
			catch (Exception ex)
			{
				Log.Instance.Catch(ex.Message);
			}
		}
	}
}
