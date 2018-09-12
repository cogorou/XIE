xie_ds
====

これは、DirectShow Filter のプロジェクトです。

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
	│├ xie_ds_x64_100.dll      … (1) 64 bit (Release) 版
	│└ xie_ds_x86_100.dll      … (1) 32 bit (Release) 版
	├ bin-debug
	│├ xie_ds_x64_100.dll      … (1) 64 bit (Debug) 版
	│└ xie_ds_x86_100.dll      … (1) 32 bit (Debug) 版
	│
	├ build
	│├ vc
	││├ xie_ds
	│││├ Win32                   … (2) 32 bit 版
	││││├ Debug
	│││││└ xie_ds_x86_100.dll  … (3) Debug 版
	││││├ Release
	│││││└ xie_ds_x86_100.dll  … (3) Release 版
	││││
	│││├ x64                     … (2) 64 bit 版
	││││├ Debug
	│││││└ xie_ds_x64_100.dll  … (3) Debug 版
	││││├ Release
	│││││└ xie_ds_x64_100.dll  … (3) Release 版
	││││
	│││└ xie_ds.log              … (4)
	│
	├ include                   … ヘッダー
	│└ xie_ds.h
	│
	├ source                    … ソースファイル
	│└ xie_ds


## レジストリ登録と解除

このライブラリに実装された機能を DirectShow から使用するには、レジストリに登録する必要があります。  
通常は XIE-Version (`XIEversion_100.exe`) を使用してください。  
手動で登録及び登録解除を行う場合は下記のように `regsvr32` を使用してください。  

登録するには、`bin` (Debug 版は `bin-debug`) ディレクトリに移動し、下記のように実行してください。

	$ regsvr32 xie_ds_x86_100.dll
	$ regsvr32 xie_ds_x64_100.dll


登録を解除する場合は、下記のように実行してください。

	$ regsvr32 /u xie_ds_x86_100.dll
	$ regsvr32 /u xie_ds_x64_100.dll

### 関連

- DirectShow フィルタの登録方法  
	https://msdn.microsoft.com/ja-jp/library/cc354748.aspx?f=255&MSPPError=-2147217396
- 実装
	- dllmain.cpp [build/vc/xie_ds]
	- xie_ds.h [include]
	- CxDSScreenCaptureFilter.h [source/xie_ds]
	- CxDSScreenCapturePin.h [source/xie_ds]

## ソースコードの頒布元

本プロジェクトの `source/xie_ds/baseclasses` ディレクトリ配下に在るソースコードは、  
Windows 7 SDK に含まれる DirectShow サンプルの複製です。  

DirectShow サンプル:  
%ProgramFiles%\Microsoft SDKs\Windows\v7.1\Samples\multimedia\directshow  

頒布元:  
https://www.microsoft.com/en-us/download/details.aspx?id=8279  
file: GRMSDK_EN_DVD.iso  


