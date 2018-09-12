XIE 開発メモ（Linux 版）
====

ここでは XIE の Linux 版の開発に関する説明を記載します。

## 構成

作業環境のディレクトリ構成は下図のようになっています。

	$HOME
	├ XIE      … 本体

## 実行環境

**OS**

- Linux Mint 17.3 MATE 64-bit

**必要なソフトウェア**

- libstdc++  
  package: libstdc++6  

- Mono 4.8.4  
  package: mono-runtime  
  
- X11  
  package: libx11-6  
  
- glew (The OpenGL Extension Wrangler Library)  
  package: libglew1.10  
  
- libpng [1.2.x]  
  package: libpng12-0  
  
- libtiff [5.2.0]  
  package: libtiff5  
  
- libjpeg [8.0.2]  
  package: libjpeg8  
  
- zlib [1.2.8]  
  package: zlib1g  


## 開発環境

**OS**

- Linux Mint 17.3 MATE 64-bit  
  64bit 版の開発に必要です。  


**IDE**

- build-essential  
  package: build-essential  
  
- gcc 4.8.4  
  package: gcc gcc-4.8  
  
- g++ 4.8.4  
  package: g++ g++-4.8  
  
- Mono 4.8.1  
  package: mono-complete  
  
- MonoDevelop 5.10  
  package: monodevelop  
  

**開発言語**

- GNU C++  
  C++ 11 以降の仕様に依存します。  
  
- C#  
  C# 4.0 以降の仕様に依存します。  
  

**関連ソフトウェア**

- libstdc++  
  package: libstdc++-4.8-dev  

- X11  
  package: libx11-6  

- glew (The OpenGL Extension Wrangler Library)  
  package: libglew-dev  
  [http://glew.sourceforge.net/](http://glew.sourceforge.net/)  
  
- libpng [1.2.x]  
  package: libpng12-dev  
  [http://www.libpng.org/pub/png/libpng.html](http://www.libpng.org/pub/png/libpng.html)  
  
- libtiff [5.2.0]  
  package: libtiff5-dev  
  [http://www.libtiff.org/](http://www.libtiff.org/)  
  
- libjpeg [v8]  
  package: libjpeg8-dev  
  [http://www.ijg.org/](http://www.ijg.org/)  
  
- zlib [1.2.8]  
  package: zlib1g-dev  
  [http://www.zlib.net/](http://www.zlib.net/)  
  
