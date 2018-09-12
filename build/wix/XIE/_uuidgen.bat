@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

set COMMAND=%1
set OPTION=%2

if not "%ProgramFiles(x86)%" == "" (goto _X64) else (goto _X86)

:_X86
@if NOT DEFINED DEVENV_DIR (
@rem	set UUIDGEN="%ProgramFiles%\Microsoft Visual Studio 8\Common7\Tools\uuidgen.exe"
@rem	set UUIDGEN="%ProgramFiles%\Microsoft Visual Studio 9.0\Common7\Tools\uuidgen.exe"
		set UUIDGEN="%ProgramFiles%\Microsoft SDKs\Windows\v7.1A\Bin\uuidgen.exe"
)
call :JOB
goto :EOF

:_X64
@if NOT DEFINED DEVENV_DIR (
@rem	set UUIDGEN="%ProgramFiles(x86)%\Microsoft Visual Studio 8\Common7\Tools\uuidgen.exe"
@rem	set UUIDGEN="%ProgramFiles(x86)%\Microsoft Visual Studio 9.0\Common7\Tools\uuidgen.exe"
		set UUIDGEN="%ProgramFiles(x86)%\Microsoft SDKs\Windows\v7.1A\Bin\uuidgen.exe"
)
call :JOB
goto :EOF

@rem ==================================================
@rem USAGE
:USAGE

echo USAGE:
echo _uuidgen count [option]
echo Ex1:
echo _uuidgen 3
echo.
echo Ex2:
echo _uuidgen 3 -s   : Output UUID as an initialized C struct
echo _uuidgen 3 -i   : Output UUID in an IDL interface template
echo.
echo Microsoft UUID Generator v1.01 Copyright (c) Microsoft Corporation. All rights reserved.
echo.
%UUIDGEN% /?
echo.
@if "%COMMAND%" == "" pause
goto :EOF

@rem ==================================================
@rem START
:JOB

@if "%COMMAND%" == "" goto USAGE
@if "%COMMAND%" == "/?" goto USAGE

echo uuidgen.exe -n%COMMAND% -c %OPTION%
echo.

%UUIDGEN% -n%COMMAND% -c %OPTION%

exit /b
