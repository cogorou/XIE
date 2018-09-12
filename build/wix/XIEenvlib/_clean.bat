@echo off

@rem ==================================================
@rem ENVIRONMENT VARIABLE

@rem ==================================================
@rem JOB
:JOB

@rem ==================================================
@rem common

del /q /s *.user
del /q /s *.ncb
del /q /s *.aps
del /q /s *.log
del /q /s *.suo
del /q /s *.sdf
del /AH /q /s *.suo
del /q /s BuildLog.htm
rmdir /s /q Template
rmdir /s /q Win32
rmdir /s /q x86
rmdir /s /q x64
rmdir /s /q ipch
rmdir /s /q bin
rmdir /s /q obj
rmdir /s /q .vs
