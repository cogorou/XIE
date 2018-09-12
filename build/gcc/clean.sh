#!/bin/bash

export CROSS_COMPILE=
#export CROSS_COMPILE=arm-linux-gnueabihf-

cd xie_core
sh clean.sh
cd ..

cd xie_high
sh clean.sh
cd ..

