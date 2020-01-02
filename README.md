# PlugnPutty
Open and close PuTTY session automatically when you plug and unplug USB CDC device.

## How it works
- In this example I use a STM32 demo board and a VT100 serial terminal.

![Image 1](https://github.com/undici77/PlugnPutty/blob/master/images/image1.png)


- Power on the board

![Image 2](https://github.com/undici77/PlugnPutty/blob/master/images/image2.png)


- Plug CDC USB and Putty appear automatically

![Image 3](https://github.com/undici77/PlugnPutty/blob/master/images/image3.png)



## Cretae Setup
- Install Visual Studio 2015 or later
- Install InnoSetup
- Open visual studio and compile compile project
- Open setup/PlugnPutty.iss
- Generate setup.exe
- Install setup.exe
	
## PlugnPutty.ini setup
&nbsp;&nbsp;&nbsp;&nbsp;**[General]**<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**StartMinimized** = If 1 start minimazed else start normal<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**Autoclose**  = If 1 autoclose putty on usb plug out, else leave it opened<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**Reopen** = If 1 reopen putty if is closed and usb is plugged in, else leave it closed<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**Enable** = If 1 software is enable, else disable<br />
<br />
&nbsp;&nbsp;&nbsp;&nbsp;**[Log]**<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**Level** = Bitmask of log verbose: INFO_LEVEL->bit 0, DEBUG_LEVEL->bit 1, CATCH_LEVEL->bit 2<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**ToFile** = If 1 log is saved to file<br />

&nbsp;&nbsp;&nbsp;&nbsp;**[Putty]**<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**Exe** = Path of putty.exe<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**StartDelay** = Delay before opening new PuTTY session<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**TermHeight** = Height of PuTTY window<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**TermWidth** = Width of PuTTY window<br />

&nbsp;&nbsp;&nbsp;&nbsp;**[Usb]**<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**Descriptor** = List of USB [descriptor]|[serial_parameters];[descriptor]|[serial_parameters]<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**PollingTime** = Polling time of usb plugin / plugout<br />

&nbsp;&nbsp;&nbsp;&nbsp;**[ExternalProcessMonitor]**<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**PollingTime** = Polling time of external process monitor<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**ProcessName** = Name of process that put PlugnPutty in pause<br />


