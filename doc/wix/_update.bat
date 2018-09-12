@echo on

@rem =================================================================
@rem JOB
@rem 

@if not exist Archives					mkdir Archives
@if not exist Archives\doc				mkdir Archives\doc

call :doc

exit /b

@rem =================================================================
@rem doc
@rem 
:doc

echo [[[doc]]]

@set SRCDIR=..
@set DSTDIR=.\Archives\doc

copy %SRCDIR%\cli\XIE-cli.chm			%DSTDIR%
copy %SRCDIR%\cpp\XIE-cpp.chm			%DSTDIR%

echo. > %DSTDIR%\XIE-cli.chw
echo. > %DSTDIR%\XIE-cpp.chw

exit /b
