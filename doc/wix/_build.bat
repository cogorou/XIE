@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

call _update.bat > _update.log 2>&1

pushd XIE-Documents
call _build.bat
popd
