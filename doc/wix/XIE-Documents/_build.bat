@echo off

set PREFIX=XIE-Documents
set PROJECT=%PREFIX%.wixproj
set VisualStudioVersion=12.0

@rem ==================================================
@rem ENVIRONMENT VARIABLE

set MSBUILD_EXE=%WinDir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

@rem ==================================================
@rem JOB

if not exist ..\Archives (
	echo ..\Archives is not found.
	echo Please execute the ..\_build.bat before this work.
	pause
	exit /b
)

if exist %PREFIX%.log del %PREFIX%.log

@rem call :MAKE Rebuild x86 Debug
@rem if ERRORLEVEL 1 goto :EOF

call :MAKE Rebuild x86 Release
if ERRORLEVEL 1 goto :EOF

exit /b

@rem ==================================================
@rem MAKE
:MAKE

set COMMAND=%1
set PLATFORM=%2
set CONFIGURATION=%3

echo %PROJECT% - %COMMAND% %PLATFORM% %CONFIGURATION%
%MSBUILD_EXE% %PROJECT% /t:%COMMAND% /p:Platform=%PLATFORM% /p:Configuration=%CONFIGURATION% > %PREFIX%.log 2>&1
if ERRORLEVEL 1 (
	echo error occured.
	pause
	goto :EOF
)

exit /b
