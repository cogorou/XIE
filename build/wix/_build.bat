@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

pushd XIEenvlib
call _build.bat
popd

call _update.bat > _update.log 2>&1

pushd XIE
call _build.bat
popd
