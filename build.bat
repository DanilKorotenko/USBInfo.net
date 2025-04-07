@REM @echo off

for /f "tokens=*" %%i in ('git rev-list --count HEAD') do set buildNumber=%%i

set buildDir=usbInfo-%buildNumber%

set buildZip=%buildDir%.zip

echo %buildZip%

dotnet publish -r win-x64 --self-contained -o publish

cd Installer

set installerName=USBInfo-%buildNumber%

set installerFileName=%installerName%.msi

echo %installerFileName%

@REM  dotnet tool install --global wix

wix build -arch x64 -d BuildNumber=%buildNumber% -culture uk-ua -loc lang/uk-ua.wxl .\Product.wxs -out %installerFileName%


@REM powershell Compress-Archive -Path "publish\**" -DestinationPath %buildZip% -Force
