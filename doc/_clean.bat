@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

pushd cpp
call _clean.bat
popd

pushd cpp
call project_clean.bat
popd

pushd cpp\customhhc
call _clean.bat
popd

pushd cpp\samples\vc\tutorial01
call _clean.bat
popd

pushd cpp\samples\vc\tutorial02
call _clean.bat
popd

pushd cli
call _clean.bat
popd

pushd cli
call project_clean.bat
popd

pushd cli\customhhc
call _clean.bat
popd

pushd cli\samples\cs\tutorial01
call _clean.bat
popd

pushd cli\samples\cs\tutorial02
call _clean.bat
popd

pushd icons
call _clean.bat
popd

pushd wix
call _clean.bat
popd

