LabRecorder
===

上位ライブラリ(xie_high) に実装された機能のデモです。  

このデモでは、Media 名前空間の配下のスクリーンキャプチャ機能を使用します。  
Windows でのみ動作します。  
Linux では使用できません。  

## ビルド

プロジェクトを起動してビルドするか、\_build.bat を実行してください。  

	C:> _build.bat

ビルド結果や実行結果を消去するには \_clean.bat を実行してください。  

	C:> _clean.bat


## 実行

コマンドプロンプトを起動し、$(TargetDir) に移動して実行してください。  

	C:> cd bin\Release  
	C:> demo.exe  


実行するとウィンドウの一覧を表示します。

	test01
	 0: 00000000: Desktop                                  [0,0-1920,1080]
	 1: 002005A6: 管理者: C:\Windows\System32\cmd.exe - d  [79,346-757,432]
	 2: 000E05B4: 日付と時刻                               [1947,31-465,553]
	 5: 00010180: Program Manager                          [0,0-3840,1080]
	 6: exit
	input number ＞ 

キャプチャするウィンドウの番号を入力して enter キーを押下すると開始します。

	input number ＞ 2
	000E05B4: 日付と時刻                               [1947,31-465,553]
	10 fps

10 秒経過すると自動的に停止し、下記のように統計値を表示して終了します。

	Count   =       101
	Mean    =    98.607
	Sigma   =    11.760
	Min     =    44.044
	Max     =   116.708
	Elapsed = 10063.573

キャプチャした結果は Results ディレクトリ配下に格納されています。

	$(TargetDir)
	├ demo.exe
	├ Results
	│├ (処理結果の動画や画像)


## 構成

通常は依存する .NET アセンブリ (XIE.Core.dll) が GAC に登録されており、
それが依存する Win32 アセンブリが配置されたディレクトリが PATH 環境変数に登録されています。
.NET アセンブリが GAC に登録されていない場合はアプリケーションに隣接する位置に配置することで動作可能ですが、
Win32 アセンブリをバインドする為に PATH 環境変数の登録は必須です。
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
