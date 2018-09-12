#!/bin/bash

export CROSS_COMPILE=
#export CROSS_COMPILE=arm-linux-gnueabihf-

mode=install
if [ "$#" -ne 0 ]; then
	mode=$1
fi
echo "make $1"

make DEBUG=1 $mode 2>&1 | tee make-debug.log
make DEBUG=0 $mode 2>&1 | tee make-release.log

