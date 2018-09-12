#!/bin/bash

cd XIE.Core
sh build.sh
cd ..

cd XIE.Tasks
sh build.sh
cd ..

cd XIEprompt
sh build.sh
cd ..

cd XIEscript
sh build.sh
cd ..

cd XIEstudio
sh build.sh
cd ..

