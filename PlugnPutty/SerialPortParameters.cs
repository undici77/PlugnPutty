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
using System.IO.Ports;

class SerialPortParameters
{
	enum PARAMETERS
	{
		BAUDRATE,
		DATA_BITS,
		PARITY,
		STOP_BITS,
		HANDSHAKE
	}

	private string    _Port;
	private int       _Baudrate;
	private Parity    _Parity;
	private int       _Data_Bits;
	private StopBits  _Stop_Bits;
	private Handshake _Handshake;

	/// @brief Constructor
	///
	public SerialPortParameters()
	{
		_Port      = "";
		_Baudrate  = 0;
		_Parity    = Parity.None;
		_Data_Bits = 0;
		_Stop_Bits = StopBits.None;
		_Handshake = Handshake.None;
	}

	/// @brief Constructor
	///
	/// @param port porn name
	/// @param settings	port settings string
	public SerialPortParameters(string port, string settings)
	{
		_Port    = port;
		Settings = settings;
	}

	/// @brief Get or set port settings from string
	public string Settings
	{
		get
		{
			string settings;

			settings = string.Format("{0},{1},{2},{3},{4}", _Baudrate, _Data_Bits, _Parity.ToString(), _Stop_Bits.ToString(), _Handshake.ToString());

			return (settings);
		}

		set
		{
			string[] parameters;

			parameters = value.Split(',');
			if (parameters.Length < 5)
			{
				throw new Exception("Serail parameters number < 5");
			}

			_Baudrate  = int.Parse(parameters[(int)PARAMETERS.BAUDRATE]);
			_Data_Bits = int.Parse(parameters[(int)PARAMETERS.DATA_BITS]);
			_Parity    = (Parity)Enum.Parse(typeof(Parity), parameters[(int)PARAMETERS.PARITY]);
			_Stop_Bits = (StopBits)Enum.Parse(typeof(StopBits), parameters[(int)PARAMETERS.STOP_BITS]);
			_Handshake = (Handshake)Enum.Parse(typeof(Handshake), parameters[(int)PARAMETERS.HANDSHAKE]);
		}
	}

	/// @brief Get or set port name
	public string Port
	{
		get
		{
			return (_Port);
		}
		set
		{
			_Port = value;
		}
	}

	/// @brief Get or set baudrate
	public int Baudrate
	{
		get
		{
			return (_Baudrate);
		}
		set
		{
			_Baudrate = value;
		}
	}

	/// @brief Get or set parity
	public Parity Parity
	{
		get
		{
			return (_Parity);
		}
		set
		{
			_Parity = value;
		}
	}

	/// @brief Get or set data bits
	public int DataBits
	{
		get
		{
			return (_Data_Bits);
		}
		set
		{
			_Data_Bits = value;
		}
	}

	/// @brief Get or set stop bits
	public StopBits StopBits
	{
		get
		{
			return (_Stop_Bits);
		}
		set
		{
			_Stop_Bits = value;
		}
	}

	/// @brief Get or set handshake
	public Handshake Handshake
	{
		get
		{
			return (_Handshake);
		}
		set
		{
			_Handshake = value;
		}
	}
}
