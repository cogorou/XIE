@echo off

if exist html        rmdir /Q /S html
if exist latex       rmdir /Q /S latex

del /q /s *.user
del /q /s *.ncb
del /q /s *.aps
del /q /s *.log
del /q /s *.suo
del /q /s *.sdf
del /AH /q /s *.suo
del /q /s BuildLog.htm
del /q /s *.tmp
rmdir /s /q Template
rmdir /s /q Win32
rmdir /s /q x86
rmdir /s /q x64
rmdir /s /q ipch
rmdir /s /q obj
rmdir /s /q Results
rmdir /s /q .vs
