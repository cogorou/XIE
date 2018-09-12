#!/bin/bash

export CROSS_COMPILE=
#export CROSS_COMPILE=arm-linux-gnueabihf-

mode=all
if [ "$#" -ne 0 ]; then
	mode=$1
fi
echo "make $1"

cd LabCore
make $mode
cd ..

cd LabGDI
make $mode
cd ..

cd LabIO
make $mode
cd ..

cd LabMedia
make $mode
cd ..

