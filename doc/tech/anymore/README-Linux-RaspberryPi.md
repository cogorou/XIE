XIE 開発メモ（Linux 版）-Raspberry Pi 向けのクロスコンパイル環境構築-
====

Debian GNU/Linux 7.8 [i686] で Raspberry Pi 向けのクロスコンパイル環境を構築した際のメモです。

※注）64bit OS 上ではビルドできません。(執筆時点)  

## 手順

### ソフトウェアの取得

下記リポジトリから取得します。  
[https://github.com/raspberrypi](https://github.com/raspberrypi)  

	$ cd
	$ mkdir raspi
	$ cd raspi
	$ git clone https://github.com/raspberrypi/tools.git --depth 1
	(暫く時間が掛かります)

作業環境のディレクトリ構成は下図のようになっています。

	$HOME
	├ raspi
	│├ tools
	││├ arm-bcm2708

下記のソフトウェアがあれば正常です。

	$ cd ~/raspi/tools/arm-bcm2708
	$ ls
	  arm-bcm2708-linux-gnueabi
	  arm-bcm2708hardfp-linux-gnueabi
	  gcc-linaro-arm-linux-gnueabihf-raspbian

----
### 環境設定

コンパイラへの PATH を設定します。

	$ cd
	$ vi .bashrc
	export PATH=$PATH:$HOME/raspi/tools/arm-bcm2708/gcc-linaro-arm-linux-gnueabihf-raspbian/bin

ログインしなおすか、下記のように入力して変更を反映します。

	$ source .bashrc

下記のように入力して実行できれば正常です。

	$ arm-linux-gnueabihf-gcc -v
	Using built-in specs.
	COLLECT_GCC=arm-linux-gnueabihf-gcc
	COLLECT_LTO_WRAPPER=/home/(YourAccount)/raspi/tools/arm-bcm2708/gcc-linaro-arm-linux-gnueabihf-raspbian/bin/../libexec/gcc/arm-linux-gnueabihf/4.8.3/lto-wrapper
	Target: arm-linux-gnueabihf
	Configured with:
	(中略)
	Thread model: posix
	gcc version 4.8.3 20140106 (prerelease) (crosstool-NG linaro-1.13.1-4.8-2014.01 - Linaro GCC 2013.11)

