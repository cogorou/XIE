xie_high
====

これは、上位機能のプロジェクトです。  
下記の機能を実装しています。  
- 表示機能 … OpenGL と WindowsGDI を使用した表示機能です。
- ファイル入出力 … jpeg, png, tiff に対応しています。
- UVC 関連 … USB 2.0 カメラを接続して画像を取り込みます。
- 通信 … SerialPort, UDP, TCP/IP

## ビルド

ソリューションを立ち上げてバッチビルドを行うか、
コマンドプロンプトを起動し、\_build.bat を実行してください。  

	$ ＿build.bat

## クリーン

ビルド結果を消去するには \_clean.bat を実行してください。  

	$ ＿clean.bat

これにより、後述の「構成」の (2)～(4) が消去されます。  

## 構成

**ディレクトリ構成**  

(1) 生成物の複製  
(2) 中間ファイルの格納場所  
(3) 生成物  
(4) ビルド結果のログ  

	XIE
	├ bin
	│├ xie_high_x64_100.dll      … (1) 64 bit (Release) 版
	│└ xie_high_x86_100.dll      … (1) 32 bit (Release) 版
	├ bin-debug
	│├ xie_high_x64_100.dll      … (1) 64 bit (Debug) 版
	│└ xie_high_x86_100.dll      … (1) 32 bit (Debug) 版
	│
	├ build
	│├ vc
	││├ xie_high
	│││├ Win32                   … (2) 32 bit 版
	││││├ Debug
	│││││└ xie_high_x86_100.dll  … (3) Debug 版
	││││├ Release
	│││││└ xie_high_x86_100.dll  … (3) Release 版
	││││
	│││├ x64                     … (2) 64 bit 版
	││││├ Debug
	│││││└ xie_high_x64_100.dll  … (3) Debug 版
	││││├ Release
	│││││└ xie_high_x64_100.dll  … (3) Release 版
	││││
	│││└ xie_high.log            … (4)
	│
	├ include                    … ヘッダー
	│├ GDI                       …
	│├ IO                        …
	│├ Net                       …
	│├ UVC                       …
	├ source                     … ソースコード
	│├ xie_high                  …


**依存関係**

◇ 64bit版

	xie_high_x64_100.dll
	├ xie_core_x64_100.dll      … XIE 基本ライブラリ

◇ 32bit版

	xie_high_x86_100.dll
	├ xie_core_x86_100.dll      … XIE 基本ライブラリ
