@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

set LINUX=

@rem ==================================================
@rem JOB
:JOB

pushd xie_core
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

pushd xie_ds
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

pushd xie_high
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

@rem ==================================================
@rem common

