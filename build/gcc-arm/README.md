gcc-arm
===

## ビルド方法

**Linux**  
1. XIE/build/gcc-arm/build.sh  


## 説明

本ディレクトリ配下には XIE の RaspberryPi(ARM) 向けクロスコンパイル用の Makefile を格納しています。  

XIE の全てを RaspberryPi 上でビルドすると 6~8 時間程度を要します。  

XIE の下位部分（xie_core）をx86環境でクロスコンパイルしておき、
その生成物を Raspberry Pi へ手動で配置した後、
上位部分（xie_high）を RaspberryPi 上でビルドすることで時間短縮を行えます。  

下位部分のビルドは Intel Core i7 2.40GHz 環境で 6 分程度掛かります。

これらのビルドは Linux 環境で行ってください。  
実行するには、RaspberryPi(ARM) 用クロスコンパイル環境が別途必要です。  

クロスコンパイル環境 (x86):  

    /  
    ├ home  
    │├ (your account)
    ││├ XIE  
    │││├ build  
    ││││├ gcc-arm  
    │││││├ xie_core
    ││││││├ build.sh         … 下記 (1) (2) を必要とします。  
    ││││││├ libxie_core.so   … (3) 生成物(ARM 版 シェアドオブジェクト)  
    ││││││└ libxie_cored.so  …     〃
    │││├ include             … (1) ヘッダーファイル  
    │││└ source              … (2) ソースファイル（XIE）  


Raspberry Pi 環境:  

    /  
    ├ home  
    │├ (your account)
    ││├ XIE  
    │││├ build  
    ││││├ gcc  
    │││││└ raspi_build.sh  … 下記 (3) を必要とします。  
    │││├ lib
    ││││├ libxie_core.so   … (3) 上記の生成物。(ここに手動で配置する)
    ││││├ libxie_cored.so  …     〃  
    ││││├ libxie_high.so   … (4) Raspberry Pi 上でビルドした生成物。
    ││││└ libxie_highd.so  …     〃  
    │││├ include  
    │││└ source


## RaspberryPi(ARM) 用クロスコンパイル環境の構築

下記リポジトリよりクロスコンパイル環境を取得して適当なディレクトリに展開します。  
[https://github.com/raspberrypi/tools.git](https://github.com/raspberrypi/tools.git)  

ここではホームディレクトリ配下の raspi というディレクトリに展開する例を示します。  

    $ cd  
    $ mkdir raspi  
    $ cd raspi  
    $ git clone https://github.com/raspberrypi/tools.git --depth 1  
    (暫く待つ)  

下記ディレクトリの配下に必要なファイルがあることを確認してください。  

    $ cd ~/raspi/tools/arm-bcm2708  
    $ ls  
    arm-bcm2708-linux-gnueabi  
    arm-bcm2708hardfp-linux-gnueabi  
    gcc-linaro-arm-linux-gnueabihf-raspbian  

環境変数を設定して反映します。  

    $ cd  
    $ vi .bashrc  
    export PATH=$PATH:$HOME/raspi/tools/arm-bcm2708/gcc-linaro-arm-linux-gnueabihf-raspbian/bin  
    $ source .bashrc  

環境変数を反映されたことを確認します。  

    $ arm-linux-gnueabihf-gcc -v  
