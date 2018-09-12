@echo off
@rem ==================================================
@rem ドキュメント生成バッチ
@rem 
@rem ※ 編集する際は doxyfile も参照してください。
@rem 

@set ProgramFilesAny=%ProgramFiles%

@if not "%ProgramFiles(x86)%"=="" @set ProgramFilesAny=%ProgramFiles(x86)%

set HTMLDIR=html
set CUSTOMHHC=.\bin\customhhc.exe
set NKFCMD=.\util\nkf.exe
set DOXYCMD=.\util\doxygen.exe
set DOXYFILE=project.txt
set LOGFILE=_build.log
set WARNFILE=_build_warning.log

set CHMBUILDER="%ProgramFilesAny%\Sandcastle\ProductionTools\ChmBuilder.exe"
set DBCSFIX="%ProgramFilesAny%\Sandcastle\ProductionTools\DBCSFix.exe"
set HHCCMD="%ProgramFilesAny%\HTML Help Workshop\hhc.exe"

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

echo [[[ CHM ]]]
%HHCCMD% %HTMLDIR%\index.hhp
move /y %HTMLDIR%\*.chm .

exit /b
