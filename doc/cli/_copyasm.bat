@echo off

set BINDIR=..\..\bin

if not exist assembly mkdir assembly

copy /y %BINDIR%\XIE.*.dll assembly\.
copy /y %BINDIR%\XIE.*.xml assembly\.
