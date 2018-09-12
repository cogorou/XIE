LabMedia
===

上位ライブラリ(xie_high) に実装された機能のデモです。  

このデモでは、Media 名前空間の配下の機能を使用します。  
Windows では DirectShow、Linux では V4L2 に依存します。  

## ビルド

ターミナルを起動し、make を実行してください。

	$ make  

ビルド結果や実行結果を消去するには make clean を実行してください。  

	$ make clean


## 実行

ターミナルを起動し、demo を実行してください。  

	$ ./demo  


## 概要

※ Linux 版では下記の test01, test11 のみ動作します。

TOPIC:  

- test01: 接続されているデバイスの一覧
- test11: カメラから画像を取り込む処理

## 構成

当アプリケーションが依存するシェアドオブジェクトをバインドする為に ldconfig が実行されている必要があります。  
ldconfig の方法については、このリポジトリの README.md をご参照ください。  

(1) 当アプリケーション  
(2) 依存するシェアドオブジェクト  

**環境**  

	XIE  
	├ demo  
	│├ gcc  
	││├ $(ProjectDir)  
	│││├ demo  … (1)  
	├ lib  
	│├ libxie_core.so  … (2)  
	│├ libxie_high.so  … (2)  
