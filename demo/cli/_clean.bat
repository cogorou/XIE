@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

pushd LabCore
call _clean.bat
popd

pushd LabExif
call _clean.bat
popd

pushd LabGDI
call _clean.bat
popd

pushd LabIO
call _clean.bat
popd

pushd LabMedia
call _clean.bat
popd

pushd LabRecorder
call _clean.bat
popd
