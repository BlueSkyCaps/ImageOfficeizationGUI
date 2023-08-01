; 脚本由 Inno Setup 脚本向导 生成！
; 有关创建 Inno Setup 脚本文件的详细资料请查阅帮助文档！

#define MyAppName "Image Officeization"
#define MyAppVersion "2.4"
#define MyAppPublisher "BlueSkyCaps"
#define MyAppURL "https://www.reminisce.com/"
#define MyAppExeName "ImageOfficeizationGUI.exe"

[Setup]
; 注: AppId的值为单独标识该应用程序。
; 不要为其他安装程序使用相同的AppId值。
; (若要生成新的 GUID，可在菜单中点击 "工具|生成 GUID"。)
AppId={{4A453F0E-BD71-4333-B946-4BAE1818B5CE}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
InfoBeforeFile=D:\work_space\baidu_uploading\projects\ImageOfficeizationGUI\ImageOfficeizationGUI\Before Install.txt
InfoAfterFile=D:\work_space\baidu_uploading\projects\ImageOfficeizationGUI\ImageOfficeizationGUI\After Install.txt
; 以下行取消注释，以在非管理安装模式下运行（仅为当前用户安装）。
;PrivilegesRequired=lowest
OutputDir=C:\Users\BlueSkyCarry\Desktop
OutputBaseFilename=image-officeization-installer
SetupIconFile=D:\work_space\baidu_uploading\projects\ImageOfficeizationGUI\ImageOfficeizationGUI\Resources\edit-main.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
UsePreviousAppDir=true

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "D:\work_space\baidu_uploading\projects\ImageOfficeizationGUI\ImageOfficeizationGUI\bin\Release\net6.0-windows\win-x64\publish\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\work_space\baidu_uploading\projects\ImageOfficeizationGUI\ImageOfficeizationGUI\bin\Release\net6.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

; 注意: 不要在任何共享系统文件上使用“Flags: ignoreversion”

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}";  IconFilename: "{app}\Resources\edit-main.ico"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon;IconFilename: "{app}\Resources\edit-main.ico"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

