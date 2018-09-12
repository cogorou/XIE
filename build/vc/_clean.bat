@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

pushd xie
call _clean.bat
popd

pushd xie_core
call _clean.bat
popd

pushd xie_ds
call _clean.bat
popd

pushd xie_high
call _clean.bat
popd

@rem ==================================================
@rem common
