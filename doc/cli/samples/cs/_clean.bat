@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

pushd tutorial01
call _clean.bat
popd

pushd tutorial02
call _clean.bat
popd
