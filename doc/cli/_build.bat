@echo on

set BINDIR=..\..\bin
set SRCDIR=source

@rem ==================================================
@rem CLEAN

call _clean.bat

if {%1} == {clean} goto EOF

@rem ==================================================
@rem Merge XML documents

@rem pushd %SRCDIR%
@rem call _merge.bat XIE.Core
@rem popd

@rem ==================================================
@rem Copy assemblies and XML documents

@rem call _copyasm.bat

@rem if not exist assembly mkdir assembly
@rem for %%i in (%BINDIR%\XIE.*.dll)    do copy /y %%i assembly\%%~nxi
@rem for %%i in (%SRCDIR%\XIE.*.xml)    do move    %%i assembly\.

@rem ==================================================
@rem Copy figure images

@rem xcopy /y /s /e /i /q .\images   .\images
@rem xcopy /y /s /e /i /q .\examples .\images\examples

@rem ==================================================
@rem Build

@set ProgramFilesAny=%ProgramFiles%

@if not "%ProgramFiles(x86)%"=="" @set ProgramFilesAny=%ProgramFiles(x86)%

@set MSBUILD="%WinDir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
@set HHCCMD="%ProgramFilesAny%\HTML Help Workshop\hhc.exe"
@set MREFBUILDER="%DXROOT%\ProductionTools\MRefBuilder.exe"
@set XSLTRANSFORM="%DXROOT%\ProductionTools\XslTransform.exe"
@set BUILDASSEMBLER="%DXROOT%\ProductionTools\BuildAssembler.exe"
@set CHMBUILDER="%DXROOT%\ProductionTools\ChmBuilder.exe"
@set DBCSFIX="%ProgramFilesAny%\Sandcastle\ProductionTools\DBCSFix.exe"

@echo MSBUILD=%MSBUILD%
@echo HHCCMD=%HHCCMD%
@echo MREFBUILDER=%MREFBUILDER%
@echo SHFBROOT=%SHFBROOT%
@echo CHMBUILDER=%CHMBUILDER%
@echo DBCSFIX=%DBCSFIX%

@rem ---------------------------------------------
@rem Build
@rem 

%MSBUILD% project.shfbproj
@move /y .\Output\*.chm .

@rem ---------------------------------------------
@rem convert UTF8 to Shift_JIS
@rem 

%DBCSFIX% /d:".\Working\Output" /l:1041

@rem ---------------------------------------------
@rem compile html
@rem 

@copy /y .\Working\*.hhp .\Working\Output\HtmlHelp1\.
@copy /y .\Working\*.hhc .\Working\Output\HtmlHelp1\.
@copy /y .\Working\*.hhk .\Working\Output\HtmlHelp1\.

@pushd .\Working\Output\HtmlHelp1\.
%HHCCMD% Help1x.hhp
@move /y *.chm ..\..\..\.
@popd

@rem ==================================================
@rem Intellisense

@rem copy /y .\Output\*.xml %BINDIR%\.

:END

