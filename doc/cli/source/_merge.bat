@echo off

@rem ==================================================
@rem check argument

if {%1} == {} (
	echo ERROR: Specify a prefix of the assembry.
	goto End
)

set PREFIX=%1

@rem ==================================================
@rem check files

if exist %PREFIX%.xml	del %PREFIX%.xml

@rem ==================================================
@rem merge xml files

@rem ------------------------------
@rem 1. Create header.

if exist %PREFIX%\__header.txt (
	type %PREFIX%\__header.txt > %PREFIX%\_temp.txt
) else (
	type nul > %PREFIX%\_temp.txt
)

@rem ------------------------------
@rem 2. merge xml files

if exist %PREFIX% (
	pushd %PREFIX%
	for /R %%i in (*.xml) do type %%i >> _temp.txt
	popd
)

@rem ------------------------------
@rem 3. Add footer.

if exist %PREFIX%\__footer.txt (
	type %PREFIX%\__footer.txt >> %PREFIX%\_temp.txt
)

@rem ------------------------------
@rem E. rename temporary file.

if exist %PREFIX%\_temp.txt (
	move %PREFIX%\_temp.txt %PREFIX%.xml
)

:End
