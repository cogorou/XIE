@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

pushd XIE.Core
call _clean.bat
popd

pushd XIE.Tasks
call _clean.bat
popd

pushd XIEversion
call _clean.bat
popd

pushd XIEcapture
call _clean.bat
popd

pushd XIEprompt
call _clean.bat
popd

pushd XIEscript
call _clean.bat
popd

pushd XIEstudio
call _clean.bat
popd

@rem ==================================================
@rem common
