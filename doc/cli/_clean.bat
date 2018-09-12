@echo off

if exist Working		rmdir /s /q Working
if exist Output			rmdir /s /q Output
if exist Intellisense	rmdir /s /q Intellisense
if exist assembly		rmdir /s /q assembly
if exist comments		rmdir /s /q comments
if exist images			rmdir /s /q images

if exist manifest.xml	del manifest.xml
if exist reflection.org	del reflection.org
if exist reflection.xml	del reflection.xml
if exist toc.xml		del toc.xml

del /q *.shfbproj_*

@rem if exist mainpage\html	rmdir /s /q mainpage\html

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
rmdir /s /q obj
rmdir /s /q bin
rmdir /s /q .vs
