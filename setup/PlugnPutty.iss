#define ApplicationName 'PlugnPutty'
#define ApplicationFile 'PlugnPutty.exe'
#define ApplicationVersion GetStringFileInfo('..\PlugnPutty\bin\Release\PlugnPutty.exe', PRODUCT_VERSION)

[Files]
; Applicazione
Source: "..\PlugnPutty\bin\Release\{#ApplicationFile}"; DestDir: {app}; Flags: ignoreversion replacesameversion replacesameversion restartreplace; Permissions: everyone-full
Source: "framework\NDP451-KB2859818-Web.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall; AfterInstall: InstallFramework; Check: FrameworkIsNotInstalled

[Dirs]
Name: "{app}"; Permissions: everyone-full

[Setup]
AppName={#ApplicationName}
AppVersion={#ApplicationVersion}
AppVerName={#ApplicationName} {#ApplicationVersion}
OutputBaseFilename={#ApplicationName}_Setup_{#ApplicationVersion}
AppPublisher=Alessandro Barbieri
AppPublisherURL=https://github.com/undici77/PlugnPutty.git
DefaultDirName={pf}\Undici77\{#ApplicationName}
DefaultGroupName=Undici77\{#ApplicationName}
Compression=lzma
SolidCompression=yes
MinVersion=6.1.7600
PrivilegesRequired=admin
AppCopyright=Copyright (C) 2019 Alessandro Barbieri
SetupIconFile=..\PlugnPutty\res\PlugnPuttyEnable.ico
UninstallDisplayIcon=yes
OutputDir=output\{#ApplicationVersion}
LicenseFile=license\license.rtf

[Run]

[Icons]
Name: "{group}\{#ApplicationName}"; Filename: "{app}\{#ApplicationFile}"
Name: "{group}\Uninstall {#ApplicationName}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#ApplicationName}"; Filename: "{app}\{#ApplicationFile}";

[Code]
function FrameworkIsNotInstalled: Boolean;
begin
  Result := not RegKeyExists(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\.NETFramework\policy\v4.0');
end;

procedure InstallFramework;
var
  StatusText: string;
  ResultCode: Integer;
begin
  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := 'Installing .NET framework...';
  WizardForm.ProgressGauge.Style := npbstMarquee;
  try
    if not Exec(ExpandConstant('{tmp}\NDP451-KB2859818-Web.exe'), '/q /noreboot', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
    begin
       // you can interact with the user that the installation failed
       MsgBox('.NET installation failed with code: ' + IntToStr(ResultCode) + '.',
         mbError, MB_OK);
    end;
  finally
    WizardForm.StatusLabel.Caption := StatusText;
    WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;