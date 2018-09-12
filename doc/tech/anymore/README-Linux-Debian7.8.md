XIE 開発メモ（Linux 版）
====

ここでは XIE の Linux 版の開発に関する説明を記載します。

## 構成

作業環境のディレクトリ構成は下図のようになっています。

	$HOME
	├ XIE      … 本体

## 実行環境

**OS**

- Debian GNU/Linux 7.8 [amd64/i686]
- Debian GNU/Linux raspberrypi 3.10.25+ [armv6l]

**必要なソフトウェア**

- libstdc++  
  package: libstdc++6  

- Mono 3.12.1  
  package: mono-runtime  
  
- X11  
  package: libx11-6  

- glew (The OpenGL Extension Wrangler Library)  
  package: libglew1.7  
  
- libpng [1.2.x]  
  package: libpng12-0  
  
- libtiff [4.0.3]  
  package: libtiff4  
  
- libjpeg [v8]  
  package: libjpeg8  
  

- zlib [1.2.8]  
  package: zlib1g  


## 開発環境

**OS**

- Debian GNU/Linux 7.8 [amd64]  
  64bit 版の開発に必要です。  
  
- Debian GNU/Linux 7.8 [i686]  
  32bit 版の開発に必要です。  
  また、ARM 版のクロスコンパイルでも使用します。  

- Debian GNU/Linux raspberrypi 3.10.25+ [armv6l]  
  ARM 版の開発に必要です。  
  

**IDE**

- build-essential  
  package: build-essential  
  
- gcc 4.7.2  
  package: gcc gcc-4.7  
  
- g++ 4.7.2  
  package: g++ g++-4.7  
  
- Mono 3.12.1  
  package: mono-complete  
  ※ 2.10.8.1 から upgrade する必要があります。（ページ下の参考の覧をご参照ください。）  
  
- MonoDevelop 5.7  
  package: monodevelop  
  ※ 3.0.3.2 から upgrade する必要があります。（ページ下の参考の覧をご参照ください。）  
  

**開発言語**

- GNU C++  
  C++ 11 以降の仕様に依存します。  
  
- C#  
  C# 4.0 以降の仕様に依存します。  
  

**関連ソフトウェア**

- libstdc++  
  package: libstdc++6-4.7-dev  

- X11  
  package: libx11-6  

- glew (The OpenGL Extension Wrangler Library)  
  package: libglew-dev  
  [http://glew.sourceforge.net/](http://glew.sourceforge.net/)  
  
- libpng [1.2.x]  
  package: libpng12-dev  
  [http://www.libpng.org/pub/png/libpng.html](http://www.libpng.org/pub/png/libpng.html)  
  
- libtiff [4.0.3]  
  package: libtiff4-dev  
  [http://www.remotesensing.org/libtiff/](http://www.remotesensing.org/libtiff/)  
  
- libjpeg [v8]  
  package: libjpeg8-dev  
  [http://www.ijg.org/](http://www.ijg.org/)  
  

- zlib [1.2.8]  
  package: zlib1g-dev  
  [http://www.zlib.net/](http://www.zlib.net/)  
  
- NVIDIA CUDA 7.0  
  [https://developer.nvidia.com/cuda-downloads](https://developer.nvidia.com/cuda-downloads)  
  GPU データオブジェクトを使用したアプリケーション開発を行う場合にのみ必要です。  
  

## 参考）  

- mono 3.12.1  
  [http://www.mono-project.com/download/#download-lin](http://www.mono-project.com/download/#download-lin)  
  Debian 7.8 amd64 で mono-complete と monodevelop を apt-get すると、  
  mono 2.10.8.1、monodevelop 3.0.3.2 がインストールされます。  
  これらを upgrade する必要があります。  
  方法は上記のサイトに記載されています。  
  
