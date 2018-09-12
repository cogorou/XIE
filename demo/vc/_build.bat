@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

set PROJECT_DIR=LabCore
echo %PROJECT_DIR%
pushd %PROJECT_DIR%
call _build.bat
popd

set PROJECT_DIR=LabExif
echo %PROJECT_DIR%
pushd %PROJECT_DIR%
call _build.bat
popd

set PROJECT_DIR=LabGDI
echo %PROJECT_DIR%
pushd %PROJECT_DIR%
call _build.bat
popd

set PROJECT_DIR=LabIO
echo %PROJECT_DIR%
pushd %PROJECT_DIR%
call _build.bat
popd

set PROJECT_DIR=LabMedia
echo %PROJECT_DIR%
pushd %PROJECT_DIR%
call _build.bat
popd

set PROJECT_DIR=LabRecorder
echo %PROJECT_DIR%
pushd %PROJECT_DIR%
call _build.bat
popd
