@echo off

set PREFIX=XIEprompt
set PROJECT=%PREFIX%.csproj

@rem ==================================================
@rem ENVIRONMENT VARIABLE

set MSBUILD_EXE=%WinDir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

@rem ==================================================
@rem JOB
:JOB

call :MAKE clean AnyCPU Debug
call :MAKE clean AnyCPU Release

if exist %PREFIX%.log del %PREFIX%.log

call :MAKE build AnyCPU Debug
if ERRORLEVEL 1 goto :EOF

call :MAKE build AnyCPU Release
if ERRORLEVEL 1 goto :EOF

echo [%time%]

exit /b

@rem ==================================================
@rem MAKE
:MAKE

set COMMAND=%1
set PLATFORM=%2
set CONFIGURATION=%3

echo [%time%] %PROJECT% - %COMMAND% %PLATFORM% %CONFIGURATION%
%MSBUILD_EXE% %PROJECT% /t:%COMMAND% /p:Platform=%PLATFORM% /p:Configuration=%CONFIGURATION% >> %PREFIX%.log 2>&1
if ERRORLEVEL 1 (
	echo error occured.
	pause
	goto :EOF
)

exit /b
