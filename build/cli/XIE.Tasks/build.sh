#!/bin/bash

PREFIX=XIE.Tasks
PROJECT=$PREFIX.csproj

# ==================================================
# ENVIRONMENT VARIABLE

MSBUILD_EXE=xbuild

# ==================================================
# MAKE
MAKE()
{
	COMMAND=$1
	PLATFORM=$2
	SOLUTION=$3

	echo [`date "+%T"`] $PROJECT - $COMMAND $PLATFORM $SOLUTION
	$MSBUILD_EXE $PROJECT /t:$COMMAND /p:Platform=$PLATFORM /p:Configuration=$SOLUTION >> $PREFIX.log 2>&1
	if [ $? != 0 ]; then
		echo error occured.
		return 1
	fi
	return 0
}

# ==================================================
# JOB
JOB()
{
	FRAMEWORK=$1

	echo FrameworkVersion=$FRAMEWORK

	MAKE clean AnyCPU DebugLinux
	MAKE clean AnyCPU ReleaseLinux

	if [ -e $PREFIX.log ]; then
		rm $PREFIX.log
	fi

	MAKE build AnyCPU DebugLinux
	MAKE build AnyCPU ReleaseLinux

	echo [`date "+%T"`]

	return 0
}

# ==================================================
# START

JOB v4.0

#mdtool build -c:DebugLinux
#mdtool build -c:ReleaseLinux

exit

