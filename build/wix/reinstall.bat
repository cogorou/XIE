@echo off

@set INSTALLER=XIE\XIE.msi
@if exist %INSTALLER% (
	msiexec /x %INSTALLER% /qb- /L* reinstall_1.log
	msiexec /i %INSTALLER% /qb- /L* reinstall_2.log
) else (
	@echo %INSTALLER% is not found.
	@pause
)
