@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

pushd cpp
call _build.bat
popd

pushd cli
call _build.bat
popd

pushd icons
call _build.bat
popd
