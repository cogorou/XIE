xie_high
===

これは、上位機能のプロジェクトです。  
下記の機能を実装しています。  
- 表示機能 … OpenGL と WindowsGDI を使用した表示機能です。
- ファイル入出力 … jpeg, png に対応しています。
- UVC 関連 … USB 2.0 カメラを接続して画像を取り込みます。
- 通信 … SerialPort, UDP, TCP/IP


## ビルド

ターミナルを起動し、build.sh を実行してください。  

	$ sh build.sh


## クリーン

ビルド結果を消去するには clean.sh を実行してください。  

	$ sh clean.sh

これにより、後述の「構成」の (1)～(3) が消去されます。  


## 構成

**ディレクトリ構成**  

(1) 中間ファイルの格納場所  
(2) ビルド結果のログ  
(3) 生成物  
(4) 生成物の複製 (ldconfig の対象)  

	XIE  
	├ build  
	│├ gcc  
	││├ xie_high  
	│││├ obj                … (1)  
	│││├ objd               … (1)  
	│││├ make-release.log   … (2)  
	│││├ make-debug.log     … (2)  
	│││├ libxie_high.so     … (3)  
	│││├ libxie_highd.so    … (3)  
	├ lib  
	│├ libxie_high.so       … (4)  
	│├ libxie_highd.so      … (4)  
	│  
	├ include               … ヘッダー  
	│├ GDI                  …
	│├ IO                   …
	│├ Net                  …
	│├ UVC                  …
	├ source                … ソースコード  
	│├ xie_high             …
