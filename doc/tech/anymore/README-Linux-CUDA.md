XIE 開発メモ（Linux 版）-NVIDIA CUDA のビルド-
====

Debian GNU/Linux 8.2 [amd64] で NVIDIA CUDA 7.5 のビルドを行った際のメモです。  

※注）ビルドは 64bit OS 上でのみ対応しています。(執筆時点)  


## 手順

### ソフトウェアの取得

下記サイトから runfile を取得します。  
[https://developer.nvidia.com/cuda-downloads](https://developer.nvidia.com/cuda-downloads)  
   
条件）  
- OperatingSystem: Linux  
- Architecture: x86_64  
- Distribution: Ubuntu  
- Version: 14.04  
- Installer Type: runfile (local)

**Download Target Installer for Linux Ubuntu 14.04 x86_64**  
cuda_7.5.18_linux.run (md5sum: 4b3bcecf0dfc35928a0898793cf3e4c6) 

----
### runfile の実行

作業環境のディレクトリ構成は下図のようになっています。

	$HOME
	├ cuda
	│├ cuda_7.5.18_linux.run

作業ディレクトリに入り、runfile を実行します。

	$ cd cuda
	$ su
	# sh cuda_7.5.18_linux.run

実行すると下記のように EULA が表示されます。

	Logging to /tmp/cuda_install_2205.log
	Using more to view the EULA.
	End User License Agreement
	--------------------------
	(中略)
	-- More --

何かキーを押下するとページが進みます。  
Q キーを押下すると EULA が中断し、インストールメニューが開始します。  
下記を参考に進めてください。  

	Do you accept the previously read EULA? (accept/decline/quit): accept
	You are attempting to install on an unsupported configuration. Do you wish to continue? ((y)es/(n)o) [ default is no ]: y
	Install NVIDIA Accelerated Graphics Driver for Linux-x86_64 352.39? ((y)es/(n)o/(q)uit): n
	Install the CUDA 7.5 Toolkit? ((y)es/(n)o/(q)uit): y
	Enter Toolkit Location [ default is /usr/local/cuda-7.5 ]:
	Do you want to install a symbolic link at /usr/local/cuda? ((y)es/(n)o/(q)uit): y
	Install the CUDA 7.5 Samples? ((y)es/(n)o/(q)uit): n
	Installing the CUDA Toolkit in /usr/local/cuda-7.5 ...
	(暫く時間が掛かります)

※1) CUDA 対応の GPU が実装されていれば <u>Driver</u> をインストールできます。  
※2) <u>Samples</u> は必要に応じてインストールしてください。  


終了すると下記のようにメッセージが表示されます。  

	===========
	= Summary =
	===========

	Driver:   Not Selected
	Toolkit:  Installed in /usr/local/cuda-7.5
	Samples:  Not Selected

	Please make sure that
	 -   PATH includes /usr/local/cuda-7.5/bin
	 -   LD_LIBRARY_PATH includes /usr/local/cuda-7.5/lib64, or, add /usr/local/cuda-7.5/lib64 to /etc/ld.so.conf and run ldconfig as root

	To uninstall the CUDA Toolkit, run the uninstall script in /usr/local/cuda-7.5/bin
	To uninstall the NVIDIA Driver, run nvidia-uninstall

	Please see CUDA_Installation_Guide_Linux.pdf in /usr/local/cuda-7.5/doc/pdf for detailed information on setting up CUDA.

	***WARNING: Incomplete installation! This installation did not install the CUDA Driver. A driver of version at least 352.00 is required for CUDA 7.5 functionality to work.
	To install the driver using this installer, run the following command, replacing <CudaInstaller> with the name of this run file:
	    sudo <CudaInstaller>.run -silent -driver

	Logfile is /tmp/cuda_install_2205.log
	#

下記のように入力して実行できれば正常にインストールされています。  

	$ /usr/local/cuda/bin/nvcc --version
	nvcc: NVIDIA (R) Cuda compiler driver
	Copyright (c) 2005-2015 NVIDIA Corporation
	Built on Tue_Aug_11_14:27:32_CDT_2015
	Cuda compilation tools, release 7.5, V7.5.17

