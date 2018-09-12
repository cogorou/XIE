#!/bin/bash

#export CROSS_COMPILE=
export CROSS_COMPILE=arm-linux-gnueabihf-

mode=install
if [ "$#" -ne 0 ]; then
	mode=$1
fi
echo "make $1"

cd xie_core
sh build.sh $mode
cd ..

# cd xie_high
# sh build.sh $mode
# cd ..

#cd xie_gpu
#sh build.sh $mode
#cd ..

