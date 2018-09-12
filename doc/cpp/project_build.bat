@echo off

set PREFIX=project
set PROJECT=%PREFIX%.vcxproj
set VisualStudioVersion=12.0

@rem ==================================================
@rem ENVIRONMENT VARIABLE

set MSBUILD_EXE=%WinDir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

@rem ==================================================
@rem JOB

if exist %PREFIX%.log del %PREFIX%.log

call :MAKE Rebuild Win32 Debug
if ERRORLEVEL 1 goto :EOF

call :MAKE Rebuild Win32 Release
if ERRORLEVEL 1 goto :EOF

exit /b

@rem ==================================================
@rem MAKE
:MAKE

set COMMAND=%1
set PLATFORM=%2
set CONFIGURATION=%3

@if "%PLATFORM%" == "x86" set PLATFORM=Win32

echo %PROJECT% - %COMMAND% %PLATFORM% %CONFIGURATION%
%MSBUILD_EXE% %PROJECT% /t:%COMMAND% /p:Platform=%PLATFORM% /p:Configuration=%CONFIGURATION% > %PREFIX%.log 2>&1
if ERRORLEVEL 1 (
	echo error occured.
	pause
	goto :EOF
)

exit /b
