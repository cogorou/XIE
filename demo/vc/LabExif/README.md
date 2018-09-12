LabExif
===

Exif 関連のデモです。  

## ビルド

プロジェクトを起動してビルドするか、\_build.bat を実行してください。  

	C:> _build.bat

ビルド結果や実行結果を消去するには \_clean.bat を実行してください。  

	C:> _clean.bat


## 実行

コマンドプロンプトを起動し、$(TargetDir) に移動して実行してください。  

	C:> cd Win32\Release  
	C:> demo.exe  

また、処理結果画像が Results ディレクトリに保存されています。  

	$(TargetDir)
	├ demo.exe
	├ Results
	│├ (処理結果画像)


## 構成

当アプリケーション が依存する Win32 アセンブリをバインドする為に PATH 環境変数が設定されている必要があります。
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
