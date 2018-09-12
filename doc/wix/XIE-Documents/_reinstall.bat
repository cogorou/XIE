@echo off

set INSTALLER=XIE-Documents.msi

if exist %INSTALLER% (
	msiexec /x %INSTALLER% /qb- /L* _reinstall_1.log
	msiexec /i %INSTALLER% /qb- /L* _reinstall_2.log
) else (
	echo %INSTALLER% is not found.
	pause
)

