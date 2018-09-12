@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

pushd XIE.Core
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

pushd XIE.Tasks
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

pushd XIEversion
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

pushd XIEcapture
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

pushd XIEprompt
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

pushd XIEscript
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

pushd XIEstudio
call _build.bat
if ERRORLEVEL 1 goto :EOF
popd

@rem ==================================================
@rem common

