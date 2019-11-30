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
using Microsoft.Win32;

class PuttyProfile
{
	private readonly string SUB_KEY;

	/// @brief Constructor
	///
	/// @param name profile name
	public PuttyProfile(string name)
	{
		try
		{
			SUB_KEY = @"Software\SimonTatham\PuTTY\Sessions\" + name;

			if (Registry.CurrentUser.OpenSubKey(SUB_KEY) == null)
			{
				Registry.CurrentUser.CreateSubKey(SUB_KEY);
				HostName = @"";
				LogFileName = @"putty.log";
				Protocol = @"serial";
				TerminalType = @"xterm";
				TerminalSpeed = @"38400,38400";
				TerminalModes =
				    @"CS7=A,CS8=A,DISCARD=A,DSUSP=A,ECHO=A,ECHOCTL=A,ECHOE=A,ECHOK=A,ECHOKE=A,ECHONL=A,EOF=A,EOL=A,EOL2=A,ERASE=A,FLUSH=A,ICANON=A,ICRNL=A,IEXTEN=A,IGNCR=A,IGNPAR=A,IMAXBEL=A,INLCR=A,INPCK=A,INTR=A,ISIG=A,ISTRIP=A,IUCLC=A,IUTF8=A,IXANY=A,IXOFF=A,IXON=A,KILL=A,LNEXT=A,NOFLSH=A,OCRNL=A,OLCUC=A,ONLCR=A,ONLRET=A,ONOCR=A,OPOST=A,PARENB=A,PARMRK=A,PARODD=A,PENDIN=A,QUIT=A,REPRINT=A,START=A,STATUS=A,STOP=A,SUSP=A,SWTCH=A,TOSTOP=A,WERASE=A,XCASE=A";
				ProxyExcludeList = @"";
				ProxyHost = @"proxy";
				ProxyUsername = @"";
				ProxyPassword = @"";
				ProxyTelnetCommand = @"connect % host % port\\n";
				Environment = @"";
				UserName = @"";
				LocalUserName = @"";
				Cipher = @"aes,chacha20,3des,WARN,des,blowfish,arcfour";
				KEX = @"ecdh,dh-gex-sha1,dh-group14-sha1,rsa,WARN,dh-group1-sha1";
				HostKey = @"ed25519,ecdsa,rsa,dsa,WARN";
				RekeyBytes = @"1G";
				GSSLibs = @"gssapi32,sspi,custom";
				GSSCustom = @"";
				LogHost = @"";
				PublicKeyFile = @"";
				RemoteCommand = @"";
				Answerback = @"PuTTY";
				BellWaveFile = @"";
				WinTitle = @"";
				Font = @"Courier New";
				Colour0 = @"187,187,187";
				Colour1 = @"255,255,255";
				Colour2 = @"0,0,0";
				Colour3 = @"85,85,85";
				Colour4 = @"0,0,0";
				Colour5 = @"0,255,0";
				Colour6 = @"0,0,0";
				Colour7 = @"85,85,85";
				Colour8 = @"187,0,0";
				Colour9 = @"255,85,85";
				Colour10 = @"0,187,0";
				Colour11 = @"85,255,85";
				Colour12 = @"187,187,0";
				Colour13 = @"255,255,85";
				Colour14 = @"0,0,187";
				Colour15 = @"85,85,255";
				Colour16 = @"187,0,187";
				Colour17 = @"255,85,255";
				Colour18 = @"0,187,187";
				Colour19 = @"85,255,255";
				Colour20 = @"187,187,187";
				Colour21 = @"255,255,255";
				Wordness0 = @"0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
				Wordness32 = @"0,1,2,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1";
				Wordness64 = @"1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,2";
				Wordness96 = @"1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1";
				Wordness128 = @"1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1";
				Wordness160 = @"1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1";
				Wordness192 = @"2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2";
				Wordness224 = @"2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2";
				MousePaste = @"explicit";
				CtrlShiftIns = @"explicit";
				CtrlShiftCV = @"none";
				LineCodePage = @"UTF-8";
				Printer = @"";
				X11Display = @"";
				X11AuthFile = @"";
				BoldFont = @"";
				WideFont = @"";
				WideBoldFont = @"";
				SerialLine = @"";
				WindowClass = @"";
				SSHManualHostKeys = @"";
				PortForwardings = @"";
				Present = 0x00000001;
				LogType = 0x00000000;
				LogFileClash = 0x0000000;
				LogFlush = 0x00000001;
				LogHeader = 0x00000001;
				SSHLogOmitPasswords = 0x00000001;
				SSHLogOmitData = 0x00000000;
				PortNumber = 0x00000000;
				CloseOnExit = 0x00000001;
				WarnOnClose = 0x00000001;
				PingInterval = 0x00000000;
				PingIntervalSecs = 0x00000000;
				TCPNoDelay = 0x00000001;
				TCPKeepalives = 0x00000000;
				AddressFamily = 0x00000000;
				ProxyDNS = 0x00000001;
				ProxyLocalhost = 0x00000000;
				ProxyMethod = 0x00000000;
				ProxyPort = 0x00000050;
				ProxyLogToTerm = 0x00000001;
				UserNameFromEnvironment = 0x00000000;
				NoPTY = 0x00000000;
				Compression = 0x00000000;
				TryAgent = 0x00000001;
				AgentFwd = 0x00000000;
				GssapiFwd = 0x00000000;
				ChangeUsername = 0x00000000;
				RekeyTime = 0x0000003c;
				GssapiRekey = 0x00000002;
				SshNoAuth = 0x00000000;
				SshBanner = 0x00000001;
				AuthTIS = 0x00000000;
				AuthKI = 0x00000001;
				AuthGSSAPI = 0x00000001;
				AuthGSSAPIKEX = 0x00000001;
				SshNoShell = 0x00000000;
				SshProt = 0x00000003;
				SSH2DES = 0x00000000;
				RFCEnviron = 0x00000000;
				PassiveTelnet = 0x00000000;
				BackspaceIsDelete = 0x00000000;
				RXVTHomeEnd = 0x00000000;
				LinuxFunctionKeys = 0x00000004;
				NoApplicationKeys = 0x00000000;
				NoApplicationCursors = 0x00000000;
				NoMouseReporting = 0x00000000;
				NoRemoteResize = 0x00000000;
				NoAltScreen = 0x00000000;
				NoRemoteWinTitle = 0x00000000;
				NoRemoteClearScroll = 0x00000000;
				RemoteQTitleAction = 0x00000001;
				NoDBackspace = 0x00000000;
				NoRemoteCharset = 0x00000000;
				ApplicationCursorKeys = 0x00000000;
				ApplicationKeypad = 0x00000000;
				NetHackKeypad = 0x00000000;
				AltF4 = 0x00000001;
				AltSpace = 0x00000000;
				AltOnly = 0x00000000;
				ComposeKey = 0x00000000;
				CtrlAltKeys = 0x00000001;
				TelnetKey = 0x00000000;
				TelnetRet = 0x00000001;
				LocalEcho = 0x00000002;
				LocalEdit = 0x00000002;
				AlwaysOnTop = 0x00000000;
				FullScreenOnAltEnter = 0x00000000;
				HideMousePtr = 0x00000000;
				SunkenEdge = 0x00000000;
				WindowBorder = 0x00000001;
				CurType = 0x00000000;
				BlinkCur = 0x00000000;
				Beep = 0x00000001;
				BeepInd = 0x00000000;
				BellOverload = 0x00000001;
				BellOverloadN = 0x00000005;
				BellOverloadT = 0x000007d0;
				BellOverloadS = 0x00001388;
				ScrollbackLines = 0x000007d0;
				DECOriginMode = 0x00000000;
				AutoWrapMode = 0x00000001;
				LFImpliesCR = 0x00000000;
				CRImpliesLF = 0x00000000;
				DisableArabicShaping = 0x00000000;
				DisableBidi = 0x00000000;
				WinNameAlways = 0x00000001;
				TermWidth = 0x00000056;
				TermHeight = 0x00000015;
				FontIsBold = 0x00000000;
				FontCharSet = 0x00000000;
				FontHeight = 0x0000000a;
				FontQuality = 0x00000000;
				FontVTMode = 0x00000004;
				UseSystemColours = 0x00000000;
				TryPalette = 0x00000000;
				ANSIColour = 0x00000001;
				Xterm256Colour = 0x00000001;
				TrueColour = 0x00000001;
				BoldAsColour = 0x00000001;
				RawCNP = 0x00000000;
				UTF8linedraw = 0x00000000;
				PasteRTF = 0x00000000;
				MouseIsXterm = 0x00000000;
				RectSelect = 0x00000000;
				PasteControls = 0x00000000;
				MouseOverride = 0x00000001;
				MouseAutocopy = 0x00000001;
				CJKAmbigWide = 0x00000000;
				UTF8Override = 0x00000001;
				CapsLockCyr = 0x00000000;
				ScrollBar = 0x00000001;
				ScrollBarFullScreen = 0x00000000;
				ScrollOnKey = 0x00000000;
				ScrollOnDisp = 0x00000001;
				EraseToScrollback = 0x00000001;
				LockSize = 0x00000000;
				BCE = 0x00000001;
				BlinkText = 0x00000000;
				X11Forward = 0x00000000;
				X11AuthType = 0x00000001;
				LocalPortAcceptAll = 0x00000000;
				RemotePortAcceptAll = 0x00000000;
				BugIgnore1 = 0x00000000;
				BugPlainPW1 = 0x00000000;
				BugRSA1 = 0x00000000;
				BugIgnore2 = 0x00000000;
				BugHMAC2 = 0x00000000;
				BugDeriveKey2 = 0x00000000;
				BugRSAPad2 = 0x00000000;
				BugPKSessID2 = 0x00000000;
				BugRekey2 = 0x00000000;
				BugMaxPkt2 = 0x00000000;
				BugOldGex2 = 0x00000000;
				BugWinadj = 0x00000000;
				BugChanReq = 0x00000000;
				StampUtmp = 0x00000001;
				LoginShell = 0x00000001;
				ScrollbarOnLeft = 0x00000000;
				BoldFontIsBold = 0x00000000;
				BoldFontCharSet = 0x00000000;
				BoldFontHeight = 0x00000000;
				WideFontIsBold = 0x00000000;
				WideFontCharSet = 0x00000000;
				WideFontHeight = 0x00000000;
				WideBoldFontIsBold = 0x00000000;
				WideBoldFontCharSet = 0x00000000;
				WideBoldFontHeight = 0x00000000;
				ShadowBold = 0x00000000;
				ShadowBoldOffset = 0x00000001;
				SerialSpeed = 0x00000000;
				SerialDataBits = 0x00000008;
				SerialStopHalfbits = 0x00000002;
				SerialParity = 0x00000000;
				SerialFlowControl = 0x00000000;
				ConnectionSharing = 0x00000000;
				ConnectionSharingUpstream = 0x00000001;
				ConnectionSharingDownstream = 0x00000001;
			}
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
		}
	}

	/// @brief Read string value from window register
	///
	/// @param key key name
	/// @retval string content
	private string ReadString(string key)
	{
		try
		{
			return (Registry.CurrentUser.OpenSubKey(SUB_KEY).GetValue(key).ToString());
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
		}

		return ("");
	}

	/// @brief Write string value from window register
	///
	/// @param key key name
	/// @param value string to write
	private void WriteString(string key, string value)
	{
		try
		{
			Registry.CurrentUser.CreateSubKey(SUB_KEY).SetValue(key, value, RegistryValueKind.String);
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
		}
	}

	/// @brief Read int value from window register
	///
	/// @param key key name
	/// @retval int value
	private uint ReadUInt(string key)
	{
		try
		{
			return (uint.Parse(Registry.CurrentUser.OpenSubKey(SUB_KEY).GetValue(key).ToString()));
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
		}

		return (0);
	}

	/// @brief Write int value from window register
	///
	/// @param key key name
	/// @param value int value
	private void WriteUInt(string key, uint value)
	{
		try
		{
			Registry.CurrentUser.CreateSubKey(SUB_KEY).SetValue(key, value, RegistryValueKind.DWord);
		}
		catch (Exception ex)
		{
			Log.Instance.Catch(ex.Message);
		}
	}

	//////////////////////////////////////////////////////////////////////////
	// Key defined by PuTTy project
	//////////////////////////////////////////////////////////////////////////

	public string HostName
	{
		get
		{
			return (ReadString("HostName"));
		}
		set
		{
			WriteString("HostName", value);
		}
	}
	public string LogFileName
	{
		get
		{
			return (ReadString("LogFileName"));
		}
		set
		{
			WriteString("LogFileName", value);
		}
	}
	public string Protocol
	{
		get
		{
			return (ReadString("Protocol"));
		}
		set
		{
			WriteString("Protocol", value);
		}
	}
	public string TerminalType
	{
		get
		{
			return (ReadString("TerminalType"));
		}
		set
		{
			WriteString("TerminalType", value);
		}
	}
	public string TerminalSpeed
	{
		get
		{
			return (ReadString("TerminalSpeed"));
		}
		set
		{
			WriteString("TerminalSpeed", value);
		}
	}

	public string TerminalModes
	{
		get
		{
			return (ReadString("TerminalModes"));
		}
		set
		{
			WriteString("TerminalModes", value);
		}
	}
	public string ProxyExcludeList
	{
		get
		{
			return (ReadString("ProxyExcludeList"));
		}
		set
		{
			WriteString("ProxyExcludeList", value);
		}
	}
	public string ProxyHost
	{
		get
		{
			return (ReadString("ProxyHost"));
		}
		set
		{
			WriteString("ProxyHost", value);
		}
	}
	public string ProxyUsername
	{
		get
		{
			return (ReadString("ProxyUsername"));
		}
		set
		{
			WriteString("ProxyUsername", value);
		}
	}
	public string ProxyPassword
	{
		get
		{
			return (ReadString("ProxyPassword"));
		}
		set
		{
			WriteString("ProxyPassword", value);
		}
	}
	public string ProxyTelnetCommand
	{
		get
		{
			return (ReadString("TerminalModes"));
		}
		set
		{
			WriteString("", value);
		}
	}
	public string Environment
	{
		get
		{
			return (ReadString("Environment"));
		}
		set
		{
			WriteString("Environment", value);
		}
	}
	public string UserName
	{
		get
		{
			return (ReadString("UserName"));
		}
		set
		{
			WriteString("UserName", value);
		}
	}
	public string LocalUserName
	{
		get
		{
			return (ReadString("LocalUserName"));
		}
		set
		{
			WriteString("LocalUserName", value);
		}
	}
	public string Cipher
	{
		get
		{
			return (ReadString("Cipher"));
		}
		set
		{
			WriteString("Cipher", value);
		}
	}
	public string KEX
	{
		get
		{
			return (ReadString("KEX"));
		}
		set
		{
			WriteString("KEX", value);
		}
	}
	public string HostKey
	{
		get
		{
			return (ReadString("HostKey"));
		}
		set
		{
			WriteString("HostKey", value);
		}
	}
	public string RekeyBytes
	{
		get
		{
			return (ReadString("RekeyBytes"));
		}
		set
		{
			WriteString("RekeyBytes", value);
		}
	}
	public string GSSLibs
	{
		get
		{
			return (ReadString("GSSLibs"));
		}
		set
		{
			WriteString("GSSLibs", value);
		}
	}
	public string GSSCustom
	{
		get
		{
			return (ReadString("GSSCustom"));
		}
		set
		{
			WriteString("GSSCustom", value);
		}
	}
	public string LogHost
	{
		get
		{
			return (ReadString("LogHost"));
		}
		set
		{
			WriteString("LogHost", value);
		}
	}
	public string PublicKeyFile
	{
		get
		{
			return (ReadString("PublicKeyFile"));
		}
		set
		{
			WriteString("PublicKeyFile", value);
		}
	}
	public string RemoteCommand
	{
		get
		{
			return (ReadString("RemoteCommand"));
		}
		set
		{
			WriteString("RemoteCommand", value);
		}
	}
	public string Answerback
	{
		get
		{
			return (ReadString("Answerback"));
		}
		set
		{
			WriteString("Answerback", value);
		}
	}
	public string BellWaveFile
	{
		get
		{
			return (ReadString("BellWaveFile"));
		}
		set
		{
			WriteString("BellWaveFile", value);
		}
	}
	public string WinTitle
	{
		get
		{
			return (ReadString("WinTitle"));
		}
		set
		{
			WriteString("WinTitle", value);
		}
	}
	public string Font
	{
		get
		{
			return (ReadString("Font"));
		}
		set
		{
			WriteString("Font", value);
		}
	}
	public string Colour0
	{
		get
		{
			return (ReadString("Colour0"));
		}
		set
		{
			WriteString("Colour0", value);
		}
	}
	public string Colour1
	{
		get
		{
			return (ReadString("Colour1"));
		}
		set
		{
			WriteString("Colour1", value);
		}
	}
	public string Colour2
	{
		get
		{
			return (ReadString("Colour2"));
		}
		set
		{
			WriteString("Colour2", value);
		}
	}
	public string Colour3
	{
		get
		{
			return (ReadString("Colour3"));
		}
		set
		{
			WriteString("Colour3", value);
		}
	}
	public string Colour4
	{
		get
		{
			return (ReadString("Colour4"));
		}
		set
		{
			WriteString("Colour4", value);
		}
	}
	public string Colour5
	{
		get
		{
			return (ReadString("Colour5"));
		}
		set
		{
			WriteString("Colour5", value);
		}
	}
	public string Colour6
	{
		get
		{
			return (ReadString("Colour6"));
		}
		set
		{
			WriteString("Colour6", value);
		}
	}
	public string Colour7
	{
		get
		{
			return (ReadString("Colour7"));
		}
		set
		{
			WriteString("Colour7", value);
		}
	}
	public string Colour8
	{
		get
		{
			return (ReadString("Colour8"));
		}
		set
		{
			WriteString("Colour8", value);
		}
	}
	public string Colour9
	{
		get
		{
			return (ReadString("Colour9"));
		}
		set
		{
			WriteString("Colour9", value);
		}
	}
	public string Colour10
	{
		get
		{
			return (ReadString("Colour10"));
		}
		set
		{
			WriteString("Colour10", value);
		}
	}
	public string Colour11
	{
		get
		{
			return (ReadString("Colour11"));
		}
		set
		{
			WriteString("Colour11", value);
		}
	}
	public string Colour12
	{
		get
		{
			return (ReadString("Colour12"));
		}
		set
		{
			WriteString("Colour12", value);
		}
	}
	public string Colour13
	{
		get
		{
			return (ReadString("Colour13"));
		}
		set
		{
			WriteString("Colour13", value);
		}
	}
	public string Colour14
	{
		get
		{
			return (ReadString("Colour14"));
		}
		set
		{
			WriteString("Colour14", value);
		}
	}
	public string Colour15
	{
		get
		{
			return (ReadString("Colour15"));
		}
		set
		{
			WriteString("Colour15", value);
		}
	}
	public string Colour16
	{
		get
		{
			return (ReadString("Colour16"));
		}
		set
		{
			WriteString("Colour16", value);
		}
	}
	public string Colour17
	{
		get
		{
			return (ReadString("Colour17"));
		}
		set
		{
			WriteString("Colour17", value);
		}
	}
	public string Colour18
	{
		get
		{
			return (ReadString("Colour18"));
		}
		set
		{
			WriteString("Colour18", value);
		}
	}
	public string Colour19
	{
		get
		{
			return (ReadString("Colour19"));
		}
		set
		{
			WriteString("Colour19", value);
		}
	}
	public string Colour20
	{
		get
		{
			return (ReadString("Colour20"));
		}
		set
		{
			WriteString("Colour20", value);
		}
	}
	public string Colour21
	{
		get
		{
			return (ReadString("Colour21"));
		}
		set
		{
			WriteString("Colour21", value);
		}
	}
	public string Wordness0
	{
		get
		{
			return (ReadString("Wordness0"));
		}
		set
		{
			WriteString("Wordness0", value);
		}
	}
	public string Wordness32
	{
		get
		{
			return (ReadString("Wordness32"));
		}
		set
		{
			WriteString("Wordness32", value);
		}
	}
	public string Wordness64
	{
		get
		{
			return (ReadString("Wordness64"));
		}
		set
		{
			WriteString("Wordness64", value);
		}
	}
	public string Wordness96
	{
		get
		{
			return (ReadString("Wordness96"));
		}
		set
		{
			WriteString("Wordness96", value);
		}
	}
	public string Wordness128
	{
		get
		{
			return (ReadString("Wordness128"));
		}
		set
		{
			WriteString("Wordness128", value);
		}
	}
	public string Wordness160
	{
		get
		{
			return (ReadString("Wordness160"));
		}
		set
		{
			WriteString("Wordness160", value);
		}
	}
	public string Wordness192
	{
		get
		{
			return (ReadString("Wordness192"));
		}
		set
		{
			WriteString("Wordness192", value);
		}
	}
	public string Wordness224
	{
		get
		{
			return (ReadString("Wordness224"));
		}
		set
		{
			WriteString("Wordness224", value);
		}
	}
	public string MousePaste
	{
		get
		{
			return (ReadString("MousePaste"));
		}
		set
		{
			WriteString("MousePaste", value);
		}
	}
	public string CtrlShiftIns
	{
		get
		{
			return (ReadString("CtrlShiftIns"));
		}
		set
		{
			WriteString("CtrlShiftIns", value);
		}
	}
	public string CtrlShiftCV
	{
		get
		{
			return (ReadString("CtrlShiftCV"));
		}
		set
		{
			WriteString("CtrlShiftCV", value);
		}
	}
	public string LineCodePage
	{
		get
		{
			return (ReadString("LineCodePage"));
		}
		set
		{
			WriteString("LineCodePage", value);
		}
	}
	public string Printer
	{
		get
		{
			return (ReadString("Printer"));
		}
		set
		{
			WriteString("Printer", value);
		}
	}
	public string X11Display
	{
		get
		{
			return (ReadString("X11Display"));
		}
		set
		{
			WriteString("X11Display", value);
		}
	}
	public string X11AuthFile
	{
		get
		{
			return (ReadString("X11AuthFile"));
		}
		set
		{
			WriteString("X11AuthFile", value);
		}
	}
	public string BoldFont
	{
		get
		{
			return (ReadString("BoldFont"));
		}
		set
		{
			WriteString("BoldFont", value);
		}
	}
	public string WideFont
	{
		get
		{
			return (ReadString("WideFont"));
		}
		set
		{
			WriteString("WideFont", value);
		}
	}
	public string WideBoldFont
	{
		get
		{
			return (ReadString("WideBoldFont"));
		}
		set
		{
			WriteString("WideBoldFont", value);
		}
	}
	public string SerialLine
	{
		get
		{
			return (ReadString("SerialLine"));
		}
		set
		{
			WriteString("SerialLine", value);
		}
	}
	public string WindowClass
	{
		get
		{
			return (ReadString("WindowClass"));
		}
		set
		{
			WriteString("WindowClass", value);
		}
	}
	public string SSHManualHostKeys
	{
		get
		{
			return (ReadString("SSHManualHostKeys"));
		}
		set
		{
			WriteString("SSHManualHostKeys", value);
		}
	}
	public string PortForwardings
	{
		get
		{
			return (ReadString("PortForwardings"));
		}
		set
		{
			WriteString("PortForwardings", value);
		}
	}

	public uint Present
	{
		get
		{
			return (ReadUInt("Present"));
		}
		set
		{
			WriteUInt("Present", value);
		}
	}
	public uint LogType
	{
		get
		{
			return (ReadUInt("LogType"));
		}
		set
		{
			WriteUInt("LogType", value);
		}
	}
	public uint LogFileClash
	{
		get
		{
			return (ReadUInt("LogFileClash"));
		}
		set
		{
			WriteUInt("LogFileClash", value);
		}
	}
	public uint LogFlush
	{
		get
		{
			return (ReadUInt("LogFlush"));
		}
		set
		{
			WriteUInt("LogFlush", value);
		}
	}
	public uint LogHeader
	{
		get
		{
			return (ReadUInt("LogHeader"));
		}
		set
		{
			WriteUInt("LogHeader", value);
		}
	}
	public uint SSHLogOmitPasswords
	{
		get
		{
			return (ReadUInt("SSHLogOmitPasswords"));
		}
		set
		{
			WriteUInt("SSHLogOmitPasswords", value);
		}
	}
	public uint SSHLogOmitData
	{
		get
		{
			return (ReadUInt("SSHLogOmitData"));
		}
		set
		{
			WriteUInt("SSHLogOmitData", value);
		}
	}
	public uint PortNumber
	{
		get
		{
			return (ReadUInt("PortNumber"));
		}
		set
		{
			WriteUInt("PortNumber", value);
		}
	}
	public uint CloseOnExit
	{
		get
		{
			return (ReadUInt("CloseOnExit"));
		}
		set
		{
			WriteUInt("CloseOnExit", value);
		}
	}
	public uint WarnOnClose
	{
		get
		{
			return (ReadUInt("WarnOnClose"));
		}
		set
		{
			WriteUInt("WarnOnClose", value);
		}
	}
	public uint PingInterval
	{
		get
		{
			return (ReadUInt("PingInterval"));
		}
		set
		{
			WriteUInt("PingInterval", value);
		}
	}
	public uint PingIntervalSecs
	{
		get
		{
			return (ReadUInt("PingIntervalSecs"));
		}
		set
		{
			WriteUInt("PingIntervalSecs", value);
		}
	}
	public uint TCPNoDelay
	{
		get
		{
			return (ReadUInt("TCPNoDelay"));
		}
		set
		{
			WriteUInt("TCPNoDelay", value);
		}
	}
	public uint TCPKeepalives
	{
		get
		{
			return (ReadUInt("TCPKeepalives"));
		}
		set
		{
			WriteUInt("TCPKeepalives", value);
		}
	}
	public uint AddressFamily
	{
		get
		{
			return (ReadUInt("AddressFamily"));
		}
		set
		{
			WriteUInt("AddressFamily", value);
		}
	}
	public uint ProxyDNS
	{
		get
		{
			return (ReadUInt("ProxyDNS"));
		}
		set
		{
			WriteUInt("ProxyDNS", value);
		}
	}
	public uint ProxyLocalhost
	{
		get
		{
			return (ReadUInt("ProxyLocalhost"));
		}
		set
		{
			WriteUInt("ProxyLocalhost", value);
		}
	}
	public uint ProxyMethod
	{
		get
		{
			return (ReadUInt("ProxyMethod"));
		}
		set
		{
			WriteUInt("ProxyMethod", value);
		}
	}
	public uint ProxyPort
	{
		get
		{
			return (ReadUInt("ProxyPort"));
		}
		set
		{
			WriteUInt("ProxyPort", value);
		}
	}
	public uint ProxyLogToTerm
	{
		get
		{
			return (ReadUInt("ProxyLogToTerm"));
		}
		set
		{
			WriteUInt("ProxyLogToTerm", value);
		}
	}
	public uint UserNameFromEnvironment
	{
		get
		{
			return (ReadUInt("UserNameFromEnvironment"));
		}
		set
		{
			WriteUInt("UserNameFromEnvironment", value);
		}
	}
	public uint NoPTY
	{
		get
		{
			return (ReadUInt("NoPTY"));
		}
		set
		{
			WriteUInt("NoPTY", value);
		}
	}
	public uint Compression
	{
		get
		{
			return (ReadUInt("Compression"));
		}
		set
		{
			WriteUInt("Compression", value);
		}
	}
	public uint TryAgent
	{
		get
		{
			return (ReadUInt("TryAgent"));
		}
		set
		{
			WriteUInt("TryAgent", value);
		}
	}
	public uint AgentFwd
	{
		get
		{
			return (ReadUInt("AgentFwd"));
		}
		set
		{
			WriteUInt("AgentFwd", value);
		}
	}
	public uint GssapiFwd
	{
		get
		{
			return (ReadUInt("GssapiFwd"));
		}
		set
		{
			WriteUInt("GssapiFwd", value);
		}
	}
	public uint ChangeUsername
	{
		get
		{
			return (ReadUInt("ChangeUsername"));
		}
		set
		{
			WriteUInt("ChangeUsername", value);
		}
	}
	public uint RekeyTime
	{
		get
		{
			return (ReadUInt("RekeyTime"));
		}
		set
		{
			WriteUInt("RekeyTime", value);
		}
	}
	public uint GssapiRekey
	{
		get
		{
			return (ReadUInt("GssapiRekey"));
		}
		set
		{
			WriteUInt("GssapiRekey", value);
		}
	}
	public uint SshNoAuth
	{
		get
		{
			return (ReadUInt("SshNoAuth"));
		}
		set
		{
			WriteUInt("SshNoAuth", value);
		}
	}
	public uint SshBanner
	{
		get
		{
			return (ReadUInt("SshBanner"));
		}
		set
		{
			WriteUInt("SshBanner", value);
		}
	}
	public uint AuthTIS
	{
		get
		{
			return (ReadUInt("AuthTIS"));
		}
		set
		{
			WriteUInt("AuthTIS", value);
		}
	}
	public uint AuthKI
	{
		get
		{
			return (ReadUInt("AuthKI"));
		}
		set
		{
			WriteUInt("AuthKI", value);
		}
	}
	public uint AuthGSSAPI
	{
		get
		{
			return (ReadUInt("AuthGSSAPI"));
		}
		set
		{
			WriteUInt("AuthGSSAPI", value);
		}
	}
	public uint AuthGSSAPIKEX
	{
		get
		{
			return (ReadUInt("AuthGSSAPIKEX"));
		}
		set
		{
			WriteUInt("AuthGSSAPIKEX", value);
		}
	}
	public uint SshNoShell
	{
		get
		{
			return (ReadUInt("SshNoShell"));
		}
		set
		{
			WriteUInt("SshNoShell", value);
		}
	}
	public uint SshProt
	{
		get
		{
			return (ReadUInt("SshProt"));
		}
		set
		{
			WriteUInt("SshProt", value);
		}
	}
	public uint SSH2DES
	{
		get
		{
			return (ReadUInt("SSH2DES"));
		}
		set
		{
			WriteUInt("SSH2DES", value);
		}
	}
	public uint RFCEnviron
	{
		get
		{
			return (ReadUInt("RFCEnviron"));
		}
		set
		{
			WriteUInt("RFCEnviron", value);
		}
	}
	public uint PassiveTelnet
	{
		get
		{
			return (ReadUInt("PassiveTelnet"));
		}
		set
		{
			WriteUInt("PassiveTelnet", value);
		}
	}
	public uint BackspaceIsDelete
	{
		get
		{
			return (ReadUInt("BackspaceIsDelete"));
		}
		set
		{
			WriteUInt("BackspaceIsDelete", value);
		}
	}
	public uint RXVTHomeEnd
	{
		get
		{
			return (ReadUInt("RXVTHomeEnd"));
		}
		set
		{
			WriteUInt("RXVTHomeEnd", value);
		}
	}
	public uint LinuxFunctionKeys
	{
		get
		{
			return (ReadUInt("LinuxFunctionKeys"));
		}
		set
		{
			WriteUInt("LinuxFunctionKeys", value);
		}
	}
	public uint NoApplicationKeys
	{
		get
		{
			return (ReadUInt("NoApplicationKeys"));
		}
		set
		{
			WriteUInt("NoApplicationKeys", value);
		}
	}
	public uint NoApplicationCursors
	{
		get
		{
			return (ReadUInt("NoApplicationCursors"));
		}
		set
		{
			WriteUInt("NoApplicationCursors", value);
		}
	}
	public uint NoMouseReporting
	{
		get
		{
			return (ReadUInt("NoMouseReporting"));
		}
		set
		{
			WriteUInt("NoMouseReporting", value);
		}
	}
	public uint NoRemoteResize
	{
		get
		{
			return (ReadUInt("NoRemoteResize"));
		}
		set
		{
			WriteUInt("NoRemoteResize", value);
		}
	}
	public uint NoAltScreen
	{
		get
		{
			return (ReadUInt("NoAltScreen"));
		}
		set
		{
			WriteUInt("NoAltScreen", value);
		}
	}
	public uint NoRemoteWinTitle
	{
		get
		{
			return (ReadUInt("NoRemoteWinTitle"));
		}
		set
		{
			WriteUInt("NoRemoteWinTitle", value);
		}
	}
	public uint NoRemoteClearScroll
	{
		get
		{
			return (ReadUInt("NoRemoteClearScroll"));
		}
		set
		{
			WriteUInt("NoRemoteClearScroll", value);
		}
	}
	public uint RemoteQTitleAction
	{
		get
		{
			return (ReadUInt("RemoteQTitleAction"));
		}
		set
		{
			WriteUInt("RemoteQTitleAction", value);
		}
	}
	public uint NoDBackspace
	{
		get
		{
			return (ReadUInt("NoDBackspace"));
		}
		set
		{
			WriteUInt("NoDBackspace", value);
		}
	}
	public uint NoRemoteCharset
	{
		get
		{
			return (ReadUInt("NoRemoteCharset"));
		}
		set
		{
			WriteUInt("NoRemoteCharset", value);
		}
	}
	public uint ApplicationCursorKeys
	{
		get
		{
			return (ReadUInt("ApplicationCursorKeys"));
		}
		set
		{
			WriteUInt("ApplicationCursorKeys", value);
		}
	}
	public uint ApplicationKeypad
	{
		get
		{
			return (ReadUInt("ApplicationKeypad"));
		}
		set
		{
			WriteUInt("ApplicationKeypad", value);
		}
	}
	public uint NetHackKeypad
	{
		get
		{
			return (ReadUInt("NetHackKeypad"));
		}
		set
		{
			WriteUInt("NetHackKeypad", value);
		}
	}
	public uint AltF4
	{
		get
		{
			return (ReadUInt("AltF4"));
		}
		set
		{
			WriteUInt("AltF4", value);
		}
	}
	public uint AltSpace
	{
		get
		{
			return (ReadUInt("AltSpace"));
		}
		set
		{
			WriteUInt("AltSpace", value);
		}
	}
	public uint AltOnly
	{
		get
		{
			return (ReadUInt("AltOnly"));
		}
		set
		{
			WriteUInt("AltOnly", value);
		}
	}
	public uint ComposeKey
	{
		get
		{
			return (ReadUInt("ComposeKey"));
		}
		set
		{
			WriteUInt("ComposeKey", value);
		}
	}
	public uint CtrlAltKeys
	{
		get
		{
			return (ReadUInt("CtrlAltKeys"));
		}
		set
		{
			WriteUInt("CtrlAltKeys", value);
		}
	}
	public uint TelnetKey
	{
		get
		{
			return (ReadUInt("TelnetKey"));
		}
		set
		{
			WriteUInt("TelnetKey", value);
		}
	}
	public uint TelnetRet
	{
		get
		{
			return (ReadUInt("TelnetRet"));
		}
		set
		{
			WriteUInt("TelnetRet", value);
		}
	}
	public uint LocalEcho
	{
		get
		{
			return (ReadUInt("LocalEcho"));
		}
		set
		{
			WriteUInt("LocalEcho", value);
		}
	}
	public uint LocalEdit
	{
		get
		{
			return (ReadUInt("LocalEdit"));
		}
		set
		{
			WriteUInt("LocalEdit", value);
		}
	}
	public uint AlwaysOnTop
	{
		get
		{
			return (ReadUInt("AlwaysOnTop"));
		}
		set
		{
			WriteUInt("AlwaysOnTop", value);
		}
	}
	public uint FullScreenOnAltEnter
	{
		get
		{
			return (ReadUInt("FullScreenOnAltEnter"));
		}
		set
		{
			WriteUInt("FullScreenOnAltEnter", value);
		}
	}
	public uint HideMousePtr
	{
		get
		{
			return (ReadUInt("HideMousePtr"));
		}
		set
		{
			WriteUInt("HideMousePtr", value);
		}
	}
	public uint SunkenEdge
	{
		get
		{
			return (ReadUInt("SunkenEdge"));
		}
		set
		{
			WriteUInt("SunkenEdge", value);
		}
	}
	public uint WindowBorder
	{
		get
		{
			return (ReadUInt("WindowBorder"));
		}
		set
		{
			WriteUInt("WindowBorder", value);
		}
	}
	public uint CurType
	{
		get
		{
			return (ReadUInt("CurType"));
		}
		set
		{
			WriteUInt("CurType", value);
		}
	}
	public uint BlinkCur
	{
		get
		{
			return (ReadUInt("BlinkCur"));
		}
		set
		{
			WriteUInt("BlinkCur", value);
		}
	}
	public uint Beep
	{
		get
		{
			return (ReadUInt("Beep"));
		}
		set
		{
			WriteUInt("Beep", value);
		}
	}
	public uint BeepInd
	{
		get
		{
			return (ReadUInt("BeepInd"));
		}
		set
		{
			WriteUInt("BeepInd", value);
		}
	}
	public uint BellOverload
	{
		get
		{
			return (ReadUInt("BellOverload"));
		}
		set
		{
			WriteUInt("BellOverload", value);
		}
	}
	public uint BellOverloadN
	{
		get
		{
			return (ReadUInt("BellOverloadN"));
		}
		set
		{
			WriteUInt("BellOverloadN", value);
		}
	}
	public uint BellOverloadT
	{
		get
		{
			return (ReadUInt("BellOverloadT"));
		}
		set
		{
			WriteUInt("BellOverloadT", value);
		}
	}
	public uint BellOverloadS
	{
		get
		{
			return (ReadUInt("BellOverloadS"));
		}
		set
		{
			WriteUInt("BellOverloadS", value);
		}
	}
	public uint ScrollbackLines
	{
		get
		{
			return (ReadUInt("ScrollbackLines"));
		}
		set
		{
			WriteUInt("ScrollbackLines", value);
		}
	}
	public uint DECOriginMode
	{
		get
		{
			return (ReadUInt("DECOriginMode"));
		}
		set
		{
			WriteUInt("DECOriginMode", value);
		}
	}
	public uint AutoWrapMode
	{
		get
		{
			return (ReadUInt("AutoWrapMode"));
		}
		set
		{
			WriteUInt("AutoWrapMode", value);
		}
	}
	public uint LFImpliesCR
	{
		get
		{
			return (ReadUInt("LFImpliesCR"));
		}
		set
		{
			WriteUInt("LFImpliesCR", value);
		}
	}
	public uint CRImpliesLF
	{
		get
		{
			return (ReadUInt("CRImpliesLF"));
		}
		set
		{
			WriteUInt("CRImpliesLF", value);
		}
	}
	public uint DisableArabicShaping
	{
		get
		{
			return (ReadUInt("DisableArabicShaping"));
		}
		set
		{
			WriteUInt("DisableArabicShaping", value);
		}
	}
	public uint DisableBidi
	{
		get
		{
			return (ReadUInt("DisableBidi"));
		}
		set
		{
			WriteUInt("DisableBidi", value);
		}
	}
	public uint WinNameAlways
	{
		get
		{
			return (ReadUInt("WinNameAlways"));
		}
		set
		{
			WriteUInt("WinNameAlways", value);
		}
	}
	public uint TermWidth
	{
		get
		{
			return (ReadUInt("TermWidth"));
		}
		set
		{
			WriteUInt("TermWidth", value);
		}
	}
	public uint TermHeight
	{
		get
		{
			return (ReadUInt("TermHeight"));
		}
		set
		{
			WriteUInt("TermHeight", value);
		}
	}
	public uint FontIsBold
	{
		get
		{
			return (ReadUInt("FontIsBold"));
		}
		set
		{
			WriteUInt("FontIsBold", value);
		}
	}
	public uint FontCharSet
	{
		get
		{
			return (ReadUInt("FontCharSet"));
		}
		set
		{
			WriteUInt("FontCharSet", value);
		}
	}
	public uint FontHeight
	{
		get
		{
			return (ReadUInt("FontHeight"));
		}
		set
		{
			WriteUInt("FontHeight", value);
		}
	}
	public uint FontQuality
	{
		get
		{
			return (ReadUInt("FontQuality"));
		}
		set
		{
			WriteUInt("FontQuality", value);
		}
	}
	public uint FontVTMode
	{
		get
		{
			return (ReadUInt("FontVTMode"));
		}
		set
		{
			WriteUInt("FontVTMode", value);
		}
	}
	public uint UseSystemColours
	{
		get
		{
			return (ReadUInt("UseSystemColours"));
		}
		set
		{
			WriteUInt("UseSystemColours", value);
		}
	}
	public uint TryPalette
	{
		get
		{
			return (ReadUInt("TryPalette"));
		}
		set
		{
			WriteUInt("TryPalette", value);
		}
	}
	public uint ANSIColour
	{
		get
		{
			return (ReadUInt("ANSIColour"));
		}
		set
		{
			WriteUInt("ANSIColour", value);
		}
	}
	public uint Xterm256Colour
	{
		get
		{
			return (ReadUInt("Xterm256Colour"));
		}
		set
		{
			WriteUInt("Xterm256Colour", value);
		}
	}
	public uint TrueColour
	{
		get
		{
			return (ReadUInt("TrueColour"));
		}
		set
		{
			WriteUInt("TrueColour", value);
		}
	}
	public uint BoldAsColour
	{
		get
		{
			return (ReadUInt("BoldAsColour"));
		}
		set
		{
			WriteUInt("BoldAsColour", value);
		}
	}
	public uint RawCNP
	{
		get
		{
			return (ReadUInt("RawCNP"));
		}
		set
		{
			WriteUInt("RawCNP", value);
		}
	}
	public uint UTF8linedraw
	{
		get
		{
			return (ReadUInt("UTF8linedraw"));
		}
		set
		{
			WriteUInt("UTF8linedraw", value);
		}
	}
	public uint PasteRTF
	{
		get
		{
			return (ReadUInt("PasteRTF"));
		}
		set
		{
			WriteUInt("PasteRTF", value);
		}
	}
	public uint MouseIsXterm
	{
		get
		{
			return (ReadUInt("MouseIsXterm"));
		}
		set
		{
			WriteUInt("MouseIsXterm", value);
		}
	}
	public uint RectSelect
	{
		get
		{
			return (ReadUInt("RectSelect"));
		}
		set
		{
			WriteUInt("RectSelect", value);
		}
	}
	public uint PasteControls
	{
		get
		{
			return (ReadUInt("PasteControls"));
		}
		set
		{
			WriteUInt("PasteControls", value);
		}
	}
	public uint MouseOverride
	{
		get
		{
			return (ReadUInt("MouseOverride"));
		}
		set
		{
			WriteUInt("MouseOverride", value);
		}
	}
	public uint MouseAutocopy
	{
		get
		{
			return (ReadUInt("MouseAutocopy"));
		}
		set
		{
			WriteUInt("MouseAutocopy", value);
		}
	}
	public uint CJKAmbigWide
	{
		get
		{
			return (ReadUInt("CJKAmbigWide"));
		}
		set
		{
			WriteUInt("CJKAmbigWide", value);
		}
	}
	public uint UTF8Override
	{
		get
		{
			return (ReadUInt("UTF8Override"));
		}
		set
		{
			WriteUInt("UTF8Override", value);
		}
	}
	public uint CapsLockCyr
	{
		get
		{
			return (ReadUInt("CapsLockCyr"));
		}
		set
		{
			WriteUInt("CapsLockCyr", value);
		}
	}
	public uint ScrollBar
	{
		get
		{
			return (ReadUInt("ScrollBar"));
		}
		set
		{
			WriteUInt("ScrollBar", value);
		}
	}
	public uint ScrollBarFullScreen
	{
		get
		{
			return (ReadUInt("ScrollBarFullScreen"));
		}
		set
		{
			WriteUInt("ScrollBarFullScreen", value);
		}
	}
	public uint ScrollOnKey
	{
		get
		{
			return (ReadUInt("ScrollOnKey"));
		}
		set
		{
			WriteUInt("ScrollOnKey", value);
		}
	}
	public uint ScrollOnDisp
	{
		get
		{
			return (ReadUInt("ScrollOnDisp"));
		}
		set
		{
			WriteUInt("ScrollOnDisp", value);
		}
	}
	public uint EraseToScrollback
	{
		get
		{
			return (ReadUInt("EraseToScrollback"));
		}
		set
		{
			WriteUInt("EraseToScrollback", value);
		}
	}
	public uint LockSize
	{
		get
		{
			return (ReadUInt("LockSize"));
		}
		set
		{
			WriteUInt("LockSize", value);
		}
	}
	public uint BCE
	{
		get
		{
			return (ReadUInt("BCE"));
		}
		set
		{
			WriteUInt("BCE", value);
		}
	}
	public uint BlinkText
	{
		get
		{
			return (ReadUInt("BlinkText"));
		}
		set
		{
			WriteUInt("BlinkText", value);
		}
	}
	public uint X11Forward
	{
		get
		{
			return (ReadUInt("X11Forward"));
		}
		set
		{
			WriteUInt("X11Forward", value);
		}
	}
	public uint X11AuthType
	{
		get
		{
			return (ReadUInt("X11AuthType"));
		}
		set
		{
			WriteUInt("X11AuthType", value);
		}
	}
	public uint LocalPortAcceptAll
	{
		get
		{
			return (ReadUInt("LocalPortAcceptAll"));
		}
		set
		{
			WriteUInt("LocalPortAcceptAll", value);
		}
	}
	public uint RemotePortAcceptAll
	{
		get
		{
			return (ReadUInt("RemotePortAcceptAll"));
		}
		set
		{
			WriteUInt("RemotePortAcceptAll", value);
		}
	}
	public uint BugIgnore1
	{
		get
		{
			return (ReadUInt("BugIgnore1"));
		}
		set
		{
			WriteUInt("BugIgnore1", value);
		}
	}
	public uint BugPlainPW1
	{
		get
		{
			return (ReadUInt("BugPlainPW1"));
		}
		set
		{
			WriteUInt("BugPlainPW1", value);
		}
	}
	public uint BugRSA1
	{
		get
		{
			return (ReadUInt("BugRSA1"));
		}
		set
		{
			WriteUInt("BugRSA1", value);
		}
	}
	public uint BugIgnore2
	{
		get
		{
			return (ReadUInt("BugIgnore2"));
		}
		set
		{
			WriteUInt("BugIgnore2", value);
		}
	}
	public uint BugHMAC2
	{
		get
		{
			return (ReadUInt("BugHMAC2"));
		}
		set
		{
			WriteUInt("BugHMAC2", value);
		}
	}
	public uint BugDeriveKey2
	{
		get
		{
			return (ReadUInt("BugDeriveKey2"));
		}
		set
		{
			WriteUInt("BugDeriveKey2", value);
		}
	}
	public uint BugRSAPad2
	{
		get
		{
			return (ReadUInt("BugRSAPad2"));
		}
		set
		{
			WriteUInt("BugRSAPad2", value);
		}
	}
	public uint BugPKSessID2
	{
		get
		{
			return (ReadUInt("BugPKSessID2"));
		}
		set
		{
			WriteUInt("BugPKSessID2", value);
		}
	}
	public uint BugRekey2
	{
		get
		{
			return (ReadUInt("BugRekey2"));
		}
		set
		{
			WriteUInt("BugRekey2", value);
		}
	}
	public uint BugMaxPkt2
	{
		get
		{
			return (ReadUInt("BugMaxPkt2"));
		}
		set
		{
			WriteUInt("BugMaxPkt2", value);
		}
	}
	public uint BugOldGex2
	{
		get
		{
			return (ReadUInt("BugOldGex2"));
		}
		set
		{
			WriteUInt("BugOldGex2", value);
		}
	}
	public uint BugWinadj
	{
		get
		{
			return (ReadUInt("BugWinadj"));
		}
		set
		{
			WriteUInt("BugWinadj", value);
		}
	}
	public uint BugChanReq
	{
		get
		{
			return (ReadUInt("BugChanReq"));
		}
		set
		{
			WriteUInt("BugChanReq", value);
		}
	}
	public uint StampUtmp
	{
		get
		{
			return (ReadUInt("StampUtmp"));
		}
		set
		{
			WriteUInt("StampUtmp", value);
		}
	}
	public uint LoginShell
	{
		get
		{
			return (ReadUInt("LoginShell"));
		}
		set
		{
			WriteUInt("LoginShell", value);
		}
	}
	public uint ScrollbarOnLeft
	{
		get
		{
			return (ReadUInt("ScrollbarOnLeft"));
		}
		set
		{
			WriteUInt("ScrollbarOnLeft", value);
		}
	}
	public uint BoldFontIsBold
	{
		get
		{
			return (ReadUInt("BoldFontIsBold"));
		}
		set
		{
			WriteUInt("BoldFontIsBold", value);
		}
	}
	public uint BoldFontCharSet
	{
		get
		{
			return (ReadUInt("BoldFontCharSet"));
		}
		set
		{
			WriteUInt("BoldFontCharSet", value);
		}
	}
	public uint BoldFontHeight
	{
		get
		{
			return (ReadUInt("BoldFontHeight"));
		}
		set
		{
			WriteUInt("BoldFontHeight", value);
		}
	}
	public uint WideFontIsBold
	{
		get
		{
			return (ReadUInt("WideFontIsBold"));
		}
		set
		{
			WriteUInt("WideFontIsBold", value);
		}
	}
	public uint WideFontCharSet
	{
		get
		{
			return (ReadUInt("WideFontCharSet"));
		}
		set
		{
			WriteUInt("WideFontCharSet", value);
		}
	}
	public uint WideFontHeight
	{
		get
		{
			return (ReadUInt("WideFontHeight"));
		}
		set
		{
			WriteUInt("WideFontHeight", value);
		}
	}
	public uint WideBoldFontIsBold
	{
		get
		{
			return (ReadUInt("WideBoldFontIsBold"));
		}
		set
		{
			WriteUInt("WideBoldFontIsBold", value);
		}
	}
	public uint WideBoldFontCharSet
	{
		get
		{
			return (ReadUInt("WideBoldFontCharSet"));
		}
		set
		{
			WriteUInt("WideBoldFontCharSet", value);
		}
	}
	public uint WideBoldFontHeight
	{
		get
		{
			return (ReadUInt("WideBoldFontHeight"));
		}
		set
		{
			WriteUInt("WideBoldFontHeight", value);
		}
	}
	public uint ShadowBold
	{
		get
		{
			return (ReadUInt("ShadowBold"));
		}
		set
		{
			WriteUInt("ShadowBold", value);
		}
	}
	public uint ShadowBoldOffset
	{
		get
		{
			return (ReadUInt("ShadowBoldOffset"));
		}
		set
		{
			WriteUInt("ShadowBoldOffset", value);
		}
	}
	public uint SerialSpeed
	{
		get
		{
			return (ReadUInt("SerialSpeed"));
		}
		set
		{
			WriteUInt("SerialSpeed", value);
		}
	}
	public uint SerialDataBits
	{
		get
		{
			return (ReadUInt("SerialDataBits"));
		}
		set
		{
			WriteUInt("SerialDataBits", value);
		}
	}
	public uint SerialStopHalfbits
	{
		get
		{
			return (ReadUInt("SerialStopHalfbits"));
		}
		set
		{
			WriteUInt("SerialStopHalfbits", value);
		}
	}
	public uint SerialParity
	{
		get
		{
			return (ReadUInt("SerialParity"));
		}
		set
		{
			WriteUInt("SerialParity", value);
		}
	}
	public uint SerialFlowControl
	{
		get
		{
			return (ReadUInt("SerialFlowControl"));
		}
		set
		{
			WriteUInt("SerialFlowControl", value);
		}
	}
	public uint ConnectionSharing
	{
		get
		{
			return (ReadUInt("ConnectionSharing"));
		}
		set
		{
			WriteUInt("ConnectionSharing", value);
		}
	}
	public uint ConnectionSharingUpstream
	{
		get
		{
			return (ReadUInt("ConnectionSharingUpstream"));
		}
		set
		{
			WriteUInt("ConnectionSharingUpstream", value);
		}
	}
	public uint ConnectionSharingDownstream
	{
		get
		{
			return (ReadUInt("ConnectionSharingDownstream"));
		}
		set
		{
			WriteUInt("ConnectionSharingDownstream", value);
		}
	}
}
