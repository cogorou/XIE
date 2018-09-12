@echo on

@rem =================================================================
@rem JOB
@rem 

@if not exist Archives					mkdir Archives
@if not exist Archives\Copyright		mkdir Archives\Copyright
@if not exist Archives\bin				mkdir Archives\bin
@if not exist Archives\bin\x64			mkdir Archives\bin\x64
@if not exist Archives\bin\x86			mkdir Archives\bin\x86
@if not exist Archives\include			mkdir Archives\include

call :TOP
call :Copyright
call :bin
call :include

exit /b

@rem =================================================================
@rem TOP
@rem 
:TOP

echo [[[TOP]]]

@set SRCDIR=..\..
@set DSTDIR=.\Archives

type XIE\README.txt > %DSTDIR%\README.txt

echo.>> %DSTDIR%\README.txt
echo Last updated: %date%>> %DSTDIR%\README.txt

exit /b

@rem =================================================================
@rem Copyright
@rem 
:Copyright

echo [[[Copyright]]]

@set SRCDIR=..\..\thirdparty
@set DSTDIR=.\Archives\Copyright

copy %SRCDIR%\libjpeg\README			%DSTDIR%\libjpeg-README.txt
copy %SRCDIR%\libpng\LICENSE			%DSTDIR%\libpng-LICENSE.txt
copy %SRCDIR%\libtiff\COPYRIGHT			%DSTDIR%\libtiff-COPYRIGHT.txt
copy %SRCDIR%\libz\README				%DSTDIR%\zlib-README.txt

@rem Azuki
copy ..\..\build\cli\XIE.Tasks\Azuki\license.txt %DSTDIR%\Azuki-license.txt

exit /b

@rem =================================================================
@rem bin
@rem 
:bin

echo [[[bin]]]

@set SRCDIR=..\..\bin
@set DSTDIR=.\Archives\bin

copy %SRCDIR%\*.dll						%DSTDIR%
copy %SRCDIR%\*.dll.plugin				%DSTDIR%
copy %SRCDIR%\*.lib						%DSTDIR%
copy %SRCDIR%\*.exe						%DSTDIR%
copy %SRCDIR%\*.exe.config				%DSTDIR%
copy %SRCDIR%\*.xml						%DSTDIR%

exit /b

@rem =================================================================
@rem include
@rem 
:include

echo [[[include]]]

@set SRCDIR=..\..\include
@set DSTDIR=.\Archives\include

xcopy /s /y /i %SRCDIR%\*.h				%DSTDIR%\.

exit /b
