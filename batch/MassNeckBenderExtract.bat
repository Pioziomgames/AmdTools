@echo off
IF "%~1" == "" GOTO Info

for %%f in (%1\*.epl*) do "%~d0%~p0NeckBenderEditor.exe" "%%f"
exit /b

:Info
echo Drag and drop a folder to unpack all Neck_Bender files in it
echo:
set /p input="Press Enter to Quit"