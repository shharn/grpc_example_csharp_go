set NUGET_PATH=%UserProfile%\.nuget\packages
set TOOLS_PATH=%NUGET_PATH%\Grpc.Tools\1.22.0\tools\windows_x64

%TOOLS_PATH%\protoc.exe -I %~dp0/proto game.proto --go_out=plugins=grpc:client\service
