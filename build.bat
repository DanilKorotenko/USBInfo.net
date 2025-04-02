@REM @echo off

for /f "tokens=*" %%i in ('git rev-list --count HEAD') do set buildNumber=%%i

set buildDir=usbInfo-%buildNumber%

set buildZip=%buildDir%.zip

echo %buildZip%

dotnet publish -r win-x64 --self-contained -o publish

powershell Compress-Archive -Path "publish\**" -DestinationPath %buildZip% -Force
