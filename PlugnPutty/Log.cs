/*
 * This file is part of the CsLog distribution (https://github.com/undici77/CsLog.git).
 * Copyright (c) 2020 Alessandro Barbieri.
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
using System.Windows.Forms;

public class Log
{
	private static Log _Instance;

	public const int ERROR_LEVEL = 0;
	public const int INFO_LEVEL  = 1;
	public const int DEBUG_LEVEL = 2;
	public const int CATCH_LEVEL = 4;

	public const int MAX_LEVEL  = INFO_LEVEL + DEBUG_LEVEL + CATCH_LEVEL;
	public const int MAX_LENGTH = 512;

	public const int TRY_TO_TAKE_TIME = 1000;

	private struct FIELD
	{
		public int      log_level;
		public string   message;
		public DateTime date_time;
	};

	public bool                       _Info;
	public bool                       _Debug;
	public bool                       _Catch;
	private BlockingCollection<FIELD> _Queue;
	private Thread                    _Thread;
	private bool                      _To_File;
	private string                    _Path;
	private string                    _Name;
	private APPEND_LOG                _Append_Log;

	public delegate void APPEND_LOG(string message);

	/// @brief Constructor
	///
	private Log()
	{
		_Queue = new BlockingCollection<FIELD>(4096);
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
	/// @param path path to place log files
	/// @param name log file name prefix
	/// @param append_log callback to notify new log to append to UI
	public void Init(string path, string name, APPEND_LOG append_log)
	{
		if (append_log == null)
		{
			_Append_Log = null;
		}
		else
		{
			_Append_Log = new APPEND_LOG(append_log);
		}

		_Path       = path;
		_Name       = name;
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

			field.log_level = ERROR_LEVEL;
			field.message   = message;
			field.date_time = DateTime.Now;

			_Queue.Add(field);
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

			field.log_level = INFO_LEVEL;
			field.message   = message;
			field.date_time = DateTime.Now;

			_Queue.Add(field);
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

			field.log_level = DEBUG_LEVEL;
			field.message   = message;
			field.date_time = DateTime.Now;

			_Queue.Add(field);
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
			stack_frame = new StackFrame(1, true);

			message = message.Replace("\r\n", " ");
			message = message.Trim();

			field.log_level = CATCH_LEVEL;
			field.message   = message + " (" + Path.GetFileName(stack_frame.GetFileName()) + " line:" + stack_frame.GetFileLineNumber() + " " + stack_frame.GetMethod() + ")";
			field.date_time = DateTime.Now;

			_Queue.Add(field);
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
		string       file_name;
		StreamWriter stream;
		string       buffer;
		string       log;
		DateTime	 file_date;
		FIELD        field;

		file_date = DateTime.Now.Date;
		stream    = null;

		try
		{
			while (true)
			{
				// Try to get field from queue "on fly"
				if (!_Queue.TryTake(out field, TRY_TO_TAKE_TIME))
				{
					// No field, so let's close stream (if opened)
					if (stream != null)
					{
						stream.Close();
						stream = null;
					}

					// waiting a field
					field = _Queue.Take();
				}

				try
				{
					if (_To_File)
					{
						// Checking if date is changed
						if (file_date != field.date_time.Date)
						{
							// If stream open, let's close it, so file name will changed
							if (stream != null)
							{
								stream.Close();
								stream = null;
							}
						}

						// If stream stream is closed, let's open it!
						if (stream == null)
						{
							if (!Directory.Exists(_Path))
							{
								Directory.CreateDirectory(_Path);
							}

							file_date = field.date_time.Date;
							file_name = _Path + "\\" + String.Format("{0}_{1:yyyyMMdd}", _Name, field.date_time) + ".txt";
							stream = System.IO.File.AppendText(file_name);
						}
					}
					else if (stream != null)
					{
						stream.Close();
						stream = null;
					}

					// Formatting log
					switch (field.log_level)
					{
						case ERROR_LEVEL:
							log = String.Format("{0:HH:mm:ss.ffffff} [ERROR] ", field.date_time) + field.message;
							break;

						case INFO_LEVEL:
							log = String.Format("{0:HH:mm:ss.ffffff} [INFO] ", field.date_time) + field.message;
							break;

						case DEBUG_LEVEL:
							log = String.Format("{0:HH:mm:ss.ffffff} [DEBUG] ", field.date_time) + field.message;
							break;

						case CATCH_LEVEL:
							log = String.Format("{0:HH:mm:ss.ffffff} [CATCH] ", field.date_time) + field.message;
							break;

						default:
							Trace.WriteLine("Log level error");
							log = null;
							break;

					}

					if (log != null)
					{
						try
						{
							// Writing log to Trace
							Trace.WriteLine(log);

							// Writing log to File
							if (stream != null)
							{
								stream.WriteLine(log);
							}
						}
						catch (Exception ex)
						{
							Trace.WriteLine(ex.StackTrace);
						}

						try
						{
							if (_Append_Log != null)
							{
								buffer = log;
								if (buffer.Length > MAX_LENGTH)
								{
									buffer = buffer.Substring(0, MAX_LENGTH) + "...";
								}

								_Append_Log(buffer);
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
		catch (ThreadInterruptedException )
		{
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.StackTrace);
		}

		if (stream != null)
		{
			stream.Close();
			stream = null;
		}
	}
}
