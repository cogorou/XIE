LabIO
===

上位ライブラリ(xie_high) に実装された機能のデモです。  

このデモでは、シリアル通信、UDP通信、TCP/IP通信を使用します。  

## ビルド

ターミナルを起動し、make を実行してください。

	$ make  

ビルド結果や実行結果を消去するには make clean を実行してください。  

	$ make clean


## 実行

ターミナルを起動し、demo を実行してください。  
※１）test01（シリアル通信）は ttyS1 と ttyS2 が接続されている必要があります。  
※２）test02（UDP 通信）は 127.0.0.1:5001、5002 を使用します。  
※３）test03（TCP/IP 通信）は 127.0.0.1:5000 を使用します。  

参考）同一PC内でシリアル通信を行うので com0com 等のエミュレータがあると便利です。  

下図のように処理結果が表示されると正常です。  

	$ ./demo  
	test01
	[1] port1.Write()
	[1] port1.Write returned 5. [hello],     0.228 msec
	[2] port2.Read()
	[2] port2.Read returned 5. [hello],     6.855 msec
	test02
	[1] client1.Write()
	[1] client1.Write returned 12. [Hola, mundo!]
	[2] client2.Read()
	[2] client2.Read returned 12. [Hola, mundo!] (Port=5001)
	test03
	[1] Server
	[2] Server.IsValid
	[3] Client1
	[4] Client2
	[5] Server.Connections() = 1
	[5] Server.Connections() = 2
	[6] Server.Write()
	[6] stream.Write returned 8. [Hello 1!]
	[6] stream.Write returned 8. [Hello 2!]
	[7] Client1.Read()
	[7] stream.Read returned 8. [Hello 2!]
	[8] Client2.Read()
	[8] stream.Read returned 8. [Hello 1!]
	[9] Client1.Write()
	[9] stream.Write returned 16. [This is Client1.]
	[10] Client2.Write()
	[10] stream.Write returned 16. [This is Client2.]
	[11] Server.Read()
	[11] stream.Read returned 16. [This is Client2.]
	[11] stream.Read returned 16. [This is Client1.]


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
	│├ libxie_core.so … (2)  
	│├ libxie_high.so … (2)  


## デバッグ

デバッグを行う場合は下記のように引数を指定して make を実行してください。  

	$ make DEBUG=1


下記のように環境変数を設定してから make を実行しても構いません。  

	$ export DEBUG=1
	$ make

この場合、デバッグの解除を行うには環境変数を削除する必要があります。  

	$ unset DEBUG
	$ make

実行するには gdb を使用します。  

	$ gdb demo
