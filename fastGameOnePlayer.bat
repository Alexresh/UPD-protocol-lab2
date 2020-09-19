echo off
start UDP-Lab/UDP-Server/bin/Debug/netcoreapp3.1/UDP-Server.exe 1
TIMEOUT /T 1 /NOBREAK
start UDP-Lab/UDP-Client/bin/Debug/netcoreapp3.1/UDP-Client.exe