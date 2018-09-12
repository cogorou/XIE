@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

set PROJECT_DIR=tutorial01
echo %PROJECT_DIR%
pushd %PROJECT_DIR%
call _build.bat
popd

set PROJECT_DIR=tutorial02
echo %PROJECT_DIR%
pushd %PROJECT_DIR%
call _build.bat
popd
