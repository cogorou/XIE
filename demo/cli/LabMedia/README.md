LabMedia
===

基本ライブラリ(XIE.Core) に実装された機能のデモです。  

このデモでは、Media 名前空間の配下の機能を使用します。  
Windows では DirectShow、Linux では V4L2 に依存します。  

## ビルド

プロジェクトを起動してビルドするか、\_build.bat を実行してください。  

	C:> _build.bat

ビルド結果や実行結果を消去するには \_clean.bat を実行してください。  

	C:> _clean.bat


## 実行

コマンドプロンプトを起動し、$(TargetDir) に移動して実行してください。  

	C:> cd bin\Release  
	C:> demo.exe  


処理の経過を標準出力に表示します。  
出力結果の確認が必要であればリダイレクトを使用してテキストファイルに保存してください。  

	C:> cd bin\Release  
	C:> demo.exe > result.txt  

処理結果の動画や画像は Results ディレクトリに保存されています。  
一部のサンプルは GRF ファイルを実行ディレクトリに保存しています。  

	$(TargetDir)
	├ demo.exe
	├ Results
	│├ (処理結果の動画や画像)
	├ (GRF ファイル)


## 概要

※ Linux 版では下記の test01, test11 のみ動作します。

TOPIC:  

- test01: 接続されているデバイスの一覧
- test11: カメラから画像を取り込む処理
- test12: カメラから画像を取り込み、動画ファイルへ保存する処理
- test13: カメラ制御とビデオ品質制御
- test21: 動画ファイルを読み込む処理
- test22: 動画ファイルを読み込み、別の動画ファイルに保存する処理
- test31: DeskTop をキャプチャして動画ファイルに保存する処理
- test32: CxCanvas をキャプチャして動画ファイルに保存する処理

----

## 構成

通常は依存する .NET アセンブリ (XIE.Core.dll) が GAC に登録されており、
それが依存する Win32 アセンブリが配置されたディレクトリが PATH 環境変数に設定されています。
.NET アセンブリが GAC に登録されていない場合はアプリケーションに隣接する位置に配置することで動作可能ですが、
Win32 アセンブリをバインドする為に PATH 環境変数の設定は必要です。
PATH 環境変数の設定については、このリポジトリの README.md をご参照ください。  

(1) 当アプリケーション  
(2) 依存する .NET アセンブリ  
(3) 依存する Win32 アセンブリ  
(4) 依存する DirectShow フィルタ (Windows 版のみ)  

**64bit 環境**  

	XIE  
	├ bin  
	│├ xie_core_x64_100.dll … (3)  
	│├ xie_high_x64_100.dll … (3)  
	│├ xie_ds_x64_100.dll   … (4)  
	├ demo  
	│├ cli  
	││├ $(ProjectDir)  
	│││├ bin  
	││││├ Release        … $(TargetDir)
	│││││├ demo.exe      … (1)  
	│││││├ XIE.Core.dll  … (2)  

**32bit 環境**  

	XIE  
	├ bin  
	│├ xie_core_x86_100.dll … (3)  
	│├ xie_high_x86_100.dll … (3)  
	│├ xie_ds_x86_100.dll   … (4)  
	├ demo  
	│├ cli  
	││├ $(ProjectDir)  
	│││├ bin  
	││││├ Release        … $(TargetDir)
	│││││├ demo.exe      … (1)  
	│││││├ XIE.Core.dll  … (2)  
