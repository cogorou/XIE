@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

pushd XIE-Documents
call _clean.bat
popd

del /q /s *.log
rmdir /s /q Archives
rmdir /s /q lib
rmdir /s /q .vs
