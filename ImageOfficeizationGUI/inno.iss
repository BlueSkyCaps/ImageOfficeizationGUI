; �ű��� Inno Setup �ű��� ���ɣ�
; �йش��� Inno Setup �ű��ļ�����ϸ��������İ����ĵ���

#define MyAppName "Image Officeization"
#define MyAppVersion "2.4"
#define MyAppPublisher "BlueSkyCaps"
#define MyAppURL "https://www.reminisce.com/"
#define MyAppExeName "ImageOfficeizationGUI.exe"

[Setup]
; ע: AppId��ֵΪ������ʶ��Ӧ�ó���
; ��ҪΪ������װ����ʹ����ͬ��AppIdֵ��
; (��Ҫ�����µ� GUID�����ڲ˵��е�� "����|���� GUID"��)
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
; ������ȡ��ע�ͣ����ڷǹ���װģʽ�����У���Ϊ��ǰ�û���װ����
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

; ע��: ��Ҫ���κι���ϵͳ�ļ���ʹ�á�Flags: ignoreversion��

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}";  IconFilename: "{app}\Resources\edit-main.ico"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon;IconFilename: "{app}\Resources\edit-main.ico"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

