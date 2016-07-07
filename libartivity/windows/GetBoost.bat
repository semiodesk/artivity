Powershell.exe -executionpolicy remotesigned -File DownloadBoost.ps1
cd boost_1_61_0
call bootstrap.bat
b2.exe toolset=msvc-12.0 address-model=64 msvc
b2.exe toolset=msvc-12.0 address-model=64 msvc runtime-debugging=on variant=debug