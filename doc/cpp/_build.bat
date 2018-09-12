@echo off
@rem ==================================================
@rem ドキュメント生成バッチ
@rem 
@rem ※ 編集する際は doxyfile も参照してください。
@rem 

@set ProgramFilesAny=%ProgramFiles%

@if not "%ProgramFiles(x86)%"=="" @set ProgramFilesAny=%ProgramFiles(x86)%

set HTMLDIR=html
set CUSTOMHHC=.\util\customhhc.exe
set NKFCMD=.\util\nkf.exe
set DOXYCMD=.\util\doxygen.exe
set DOXYFILE=project-doxyfile.txt
set LOGFILE=_build.log
set WARNFILE=_build_warning.log

set ENCODING=1
set DBCSFIX=%ProgramFilesAny%\Sandcastle\ProductionTools\DBCSFix.exe
set HHCCMD=%ProgramFilesAny%\HTML Help Workshop\hhc.exe

@rem ==================================================
@rem BUILD
@rem 

echo [[[ BUILD ]]]
if exist %LOGFILE%  del /q /s /Q %LOGFILE%
if exist %WARNFILE% del /q /s /Q %WARNFILE%
if exist %HTMLDIR%  rmdir /q /s %HTMLDIR%

if exist %DOXYCMD% (
	%DOXYCMD% %DOXYFILE%  >  %LOGFILE% 2>&1
) else (
	doxygen %DOXYFILE%  >  %LOGFILE% 2>&1
)

if exist "%CUSTOMHHC%" (
	echo [[[ custom hhc ]]]
	%CUSTOMHHC%   %HTMLDIR%\index.hhc
)

if "%ENCODING%"=="1" (
	if exist "%DBCSFIX%" (
		echo [[[ convert hhc UTF8 to Shift_JIS ]]]
		"%DBCSFIX%" /d:%HTMLDIR% /l:1041

		if exist "%NKFCMD%" (
			echo [[[ convert hhk UTF8 to Shift_JIS ]]]
			copy /y       %HTMLDIR%\index.hhk       %HTMLDIR%\index.hhk.org
			%NKFCMD% -s   %HTMLDIR%\index.hhk.org > %HTMLDIR%\index.hhk
		)
	)
)

echo [[[ CHM ]]]
"%HHCCMD%" %HTMLDIR%\index.hhp
move /y %HTMLDIR%\*.chm .

exit /b
