@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

pushd XIEenvlib
call _clean.bat
popd

pushd XIE
call _clean.bat
popd

del /q /s *.log
rmdir /s /q Archives
rmdir /s /q lib
rmdir /s /q .vs
