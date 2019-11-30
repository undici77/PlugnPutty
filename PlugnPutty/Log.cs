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
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;
using PlugnPutty;

public class Log
{
	private static Log _Instance;

	public const int INFO_LEVEL  = 1;
	public const int DEBUG_LEVEL = 2;
	public const int CATCH_LEVEL = 4;

	public const int MAX_LEVEL = INFO_LEVEL + DEBUG_LEVEL + CATCH_LEVEL;

	private struct FIELD
	{
		public string message_fild;
		public string log_field;
	};

	public bool                       _Info;
	public bool                       _Debug;
	public bool                       _Catch;
	private BlockingCollection<FIELD> _Queue;
	private Thread                    _Thread;
	private bool                      _To_File;
	private string                    _Path;
	private object                    _Lock;
	private APPEND_LOG                _Append_Log;

	public delegate void APPEND_LOG(string message);

	/// @brief Constructor
	///
	private Log()
	{
		_Lock = new object();
		_Queue = new BlockingCollection<FIELD>();
	}

	/// @brief Format array of bytes to hex string
	///
	/// @param byte_array array of bytes to format
	/// @retval formatted string
	private string ByteArrayToString(byte[] byte_array)
	{
		StringBuilder hex;

		hex = new StringBuilder(byte_array.Length * 2);

		foreach (byte b in byte_array)
		{
			hex.AppendFormat("{0:x2}", b);
		}

		return (hex.ToString());
	}

	/// @brief Singleton pattern
	///
	public static Log Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = new Log();
			}

			return (_Instance);
		}
	}

	/// @brief Initialize log thread/procedure
	///
	/// @param log_path path to place log files
	/// @param append_log callback to notify new log to append to UI
	public void Init(string log_path, APPEND_LOG append_log)
	{
		_Append_Log = new APPEND_LOG(append_log);
		_Path       = log_path;
		_Thread     = new Thread(LogThread);

		_Thread.Start();
	}

	/// @brief End log thread/procedure
	///
	public void End()
	{
		_Thread.Interrupt();
		_Thread.Join();
	}

	/// @brief Get/Set level of log filters level
	///
	public int Level
	{
		get
		{
			int level;

			level = ((_Info ? INFO_LEVEL : 0) + (_Debug ? DEBUG_LEVEL : 0) + (_Catch ? CATCH_LEVEL : 0));

			return (level);
		}

		set
		{
			_Info  = (((value & INFO_LEVEL)  != 0) ? true : false);
			_Debug = (((value & DEBUG_LEVEL) != 0) ? true : false);
			_Catch = (((value & CATCH_LEVEL) != 0) ? true : false);
		}
	}

	/// @brief Get/Set to write log to file
	///
	public bool ToFile
	{
		get
		{
			return (_To_File);
		}

		set
		{
			_To_File = value;
		}
	}

	/// @brief Format ERROR log
	///
	/// @param message message to format
	public void Error(string message)
	{
		FIELD field;

		try
		{
			message = message.Replace("\r", "\\r");
			message = message.Replace("\n", "\\n");
			message = message.Trim();

			field.message_fild = message;
			field.log_field    = String.Format("{0:HH:mm:ss.ffffff} [ERROR] ", DateTime.Now) + message;

			lock (_Lock)
			{
				_Queue.Add(field);
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.StackTrace);
		}
	}

	/// @brief Get/Set log level INFORMATION filter
	///
	public bool LevelInfo
	{
		get
		{
			return (_Info);
		}

		set
		{
			_Info = value;
		}
	}

	/// @brief Format INFORMATION log
	///
	/// @param message message to format
	public void Info(string message)
	{
		FIELD field;

		if (!_Info)
		{
			return;
		}

		try
		{
			message = message.Replace("\r", "\\r");
			message = message.Replace("\n", "\\n");
			message = message.Trim();

			field.message_fild = message;
			field.log_field    = String.Format("{0:HH:mm:ss.ffffff} [INFO]  ", DateTime.Now) + message;

			lock (_Lock)
			{
				_Queue.Add(field);
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.StackTrace);
		}
	}

	/// @brief Get/Set log level DEBUG filter
	///
	public bool LevelDebug
	{
		get
		{
			return (_Debug);
		}

		set
		{
			_Debug = value;
		}
	}

	/// @brief Format DEBUG log for protocol payload
	///
	/// @param command command description string
	/// @param data payload data
	public void Debug(string command, byte[] data)
	{
		FIELD      field;
		string     message;

		if (!_Debug)
		{
			return;
		}

		try
		{
			command = command.Replace("\r", "\\r");
			command = command.Replace("\n", "\\n");
			command = command.Trim();

			message = ByteArrayToString(data);
			message = message.Trim();

			field.message_fild = command + "(" + message + ")";
			field.log_field    = String.Format("{0:HH:mm:ss.ffffff} [DEBUG] ", DateTime.Now) + command + "(" + message + ")";

			lock (_Lock)
			{
				_Queue.Add(field);
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.StackTrace);
		}
	}

	/// @brief Format DEBUG log for protocol string message
	///
	/// @param command command description string
	/// @param message message to format
	public void Debug(string command, string message)
	{
		FIELD field;

		if (!_Debug)
		{
			return;
		}

		try
		{
			command = command.Replace("\r", "\\r");
			command = command.Replace("\n", "\\n");
			command = command.Trim();

			message = message.Replace("\r", "\\r");
			message = message.Replace("\n", "\\n");
			message = message.Trim();

			field.message_fild = command + "(" + message + ")";
			field.log_field    = String.Format("{0:HH:mm:ss.ffffff} [DEBUG] ", DateTime.Now) + command + "(" + message + ")";

			lock (_Lock)
			{
				_Queue.Add(field);
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.StackTrace);
		}
	}

	/// @brief Format DEBUG log
	///
	/// @param message message to format
	public void Debug(string message)
	{
		FIELD field;

		if (!_Debug)
		{
			return;
		}

		try
		{
			message = message.Replace("\r", "\\r");
			message = message.Replace("\n", "\\n");
			message = message.Trim();

			field.message_fild = message;
			field.log_field    = String.Format("{0:HH:mm:ss.ffffff} [DEBUG] ", DateTime.Now) + message;

			lock (_Lock)
			{
				_Queue.Add(field);
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.StackTrace);
		}
	}

	/// @brief Get/Set log level CATCH filter
	///
	public bool LevelCatch
	{
		get
		{
			return (_Catch);
		}
		set
		{
			_Catch = value;
		}
	}

	/// @brief Format CATCH log
	///
	/// @param message message to format
	public void Catch(string message)
	{
		FIELD      field;
		StackFrame stack_frame;

		if (!_Catch)
		{
			return;
		}

		try
		{
			message = message.Replace("\r\n", " ");
			message = message.Trim();

			stack_frame = new StackFrame(1, true);

			field.message_fild = "Exception " + Path.GetFileName(stack_frame.GetFileName()) + " line:" + stack_frame.GetFileLineNumber() + " " + stack_frame.GetMethod() + ")";
			field.log_field    = String.Format("{0:HH:mm:ss.ffffff} [CATCH] ", DateTime.Now) + message +
			                     " (" + Path.GetFileName(stack_frame.GetFileName()) + " line:" + stack_frame.GetFileLineNumber() + " " +
			                     stack_frame.GetMethod() + ")";

			lock (_Lock)
			{
				_Queue.Add(field);
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.StackTrace);
		}
	}

	/// @brief Thread of writing log to UI/File
	///
	private void LogThread()
	{
		FIELD        field;
		string       file_name;
		StreamWriter stream;
		string       buffer;

		try
		{
			while (true)
			{
				field = _Queue.Take();

				Trace.WriteLine(field.message_fild);

				try
				{
					buffer = field.message_fild;
					if (buffer.Length > 512)
					{
						buffer = buffer.Substring(0, 512) + "...";
					}

					App.Instance.BeginInvoke(_Append_Log, buffer);
				}
				catch (Exception ex)
				{
					Trace.WriteLine(ex.StackTrace);
				}

				try
				{
					if (_To_File)
					{
						if (!Directory.Exists(_Path))
						{
							Directory.CreateDirectory(_Path);
						}

						file_name = _Path + "\\" + String.Format("{0}_{1:yyyyMMdd}", App.Name, System.DateTime.Now) + ".txt";

						stream = System.IO.File.AppendText(file_name);
						stream.WriteLine(field.log_field);
						stream.Close();
					}
				}
				catch (Exception ex)
				{
					Trace.WriteLine(ex.StackTrace);
				}
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.StackTrace);
		}
	}
}
