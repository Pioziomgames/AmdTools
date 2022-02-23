@echo off
IF "%~1" == "" GOTO Info

for %%f in (%1\*.amd*) do "%~d0%~p0AmdEditor.exe" "%%f"
exit /b

:Info
echo Drag and drop a folder to unpack all amd files in it
echo:
set /p input="Press Enter to Quit"