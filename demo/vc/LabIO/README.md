LabIO
===

上位ライブラリ(xie_high) に実装された機能のデモです。  

このデモでは、シリアル通信、UDP通信、TCP/IP通信を使用します。  

## ビルド

プロジェクトを起動してビルドするか、\_build.bat を実行してください。  

	C:> _build.bat

ビルド結果や実行結果を消去するには \_clean.bat を実行してください。  

	C:> _clean.bat


## 実行

コマンドプロンプトを起動し、$(TargetDir) に移動して実行してください。  
※１）test01（シリアル通信）は COM1 と COM11 が接続されている必要があります。  
※２）test02（UDP 通信）は 127.0.0.1:5001、5002 を使用します。  
※３）test03（TCP/IP 通信）は 127.0.0.1:5000 を使用します。  

参考）同一PC内でシリアル通信を行うので com0com 等のエミュレータがあると便利です。  

下図のように処理結果が表示されると正常です。  

	C:> cd Win32\Release  
	C:> demo.exe  
	test01
	[1] port1.Write()
	[1] port1.Write returned 5. [hello],     0.011 msec
	[2] port2.Read()
	[2] port2.Read returned 5. [hello],     0.007 msec
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
	[5] Server.Connections() = 0
	[5] Server.Connections() = 1
	[5] Server.Connections() = 2
	[6] Server.Write()
	[6] stream.Write returned 8. [Hello 1!]
	[6] stream.Write returned 8. [Hello 2!]
	[7] Client1.Read()
	[7] stream.Read returned 8. [Hello 1!]
	[8] Client2.Read()
	[8] stream.Read returned 8. [Hello 2!]
	[9] Client1.Write()
	[9] stream.Write returned 16. [This is Client1.]
	[10] Client2.Write()
	[10] stream.Write returned 16. [This is Client2.]
	[11] Server.Read()
	[11] stream.Read returned 16. [This is Client1.]
	[11] stream.Read returned 16. [This is Client2.]


## 構成

当アプリケーションが依存する Win32 アセンブリをバインドする為に PATH 環境変数が設定されている必要があります。
PATH 環境変数の設定については、このリポジトリの README.md をご参照ください。  

(1) 当アプリケーション  
(2) 依存する Win32 アセンブリ  

**32bit 環境**  

	XIE  
	├ bin  
	│├ xie_core_x86_100.dll … (2)  
	│├ xie_high_x86_100.dll … (2)  
	├ demo  
	│├ vc  
	││├ $(ProjectDir)  
	│││├ Win32  
	││││├ Release        … $(TargetDir)
	│││││├ demo.exe     … (1)  
