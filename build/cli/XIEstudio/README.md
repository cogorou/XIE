XIEstudio
====

これは、.NET/mono 版の評価アプリケーションのプロジェクトです。


## ビルド

ソリューションを立ち上げてバッチビルドを行うか、
コマンドプロンプトを起動し、\_build.bat を実行してください。  

	$ ＿build.bat

バッチビルドを行う際は、Configuration に注意してください。
Windows 版と Linux 版を分けています。
Windows 環境に於いては Debug/Release のみ選択してビルドしてください。
Linux 版は Linux 環境で mono を使用してビルドしてください。

- Windows 版
	- Debug
	- Release
- Linux 版
	- DebugLinux
	- ReleaseLinux

## クリーン

ビルド結果を消去するには \_clean.bat を実行してください。  

	$ ＿clean.bat

これにより、後述の「構成」の (2)～(4) が消去されます。  

## 構成

**ディレクトリ構成**  

(1) 生成物の複製  
(2) 生成物  
(3) 中間ファイルの格納場所  
(4) ビルド結果のログ  

	XIE
	├ bin
	│├ XIEstudio.exe               … (1) AnyCPU (Release) 版
	│├ XIEstudio.exe.config        …     アプリケーション構成ファイル
	│└ XIEstudio.XML               …     Intellisence ファイル
	├ bin-debug
	│├ XIEstudio.exe               … (1) AnyCPU (Debug) 版
	│├ XIEstudio.exe.config        …     アプリケーション構成ファイル
	│└ XIEstudio.XML               …     Intelisence ファイル
	│
	├ build
	│├ cli
	││├ XIEstudio
	│││├ bin                     … (2) AnyCPU 版
	││││├ Debug
	││││├ XIEstudio.exe         … (2) Debug 版
	││││├ XIEstudio.exe.config  …     アプリケーション構成ファイル
	││││└ XIEstudio.XML         …     Intellisence ファイル
	││││├ Release
	││││├ XIEstudio.exe         … (2) Release 版
	││││├ XIEstudio.exe.config  …     アプリケーション構成ファイル
	││││└ XIEstudio.XML         …     Intelisence ファイル
	││││
	│││├ obj                     … (3)
	││││├ Debug
	││││├ Release
	││││
	│││└ XIEstudio.log           … (4)


**依存関係**

XIEstudio 及び依存するアセンブリは AnyCPU ですので実行時に x64/x86 を分岐しています。

◇ 64bit版

	XIEstudio.exe
	├ XIE.Core.dll
	│├ xie_core_x64_100.dll     … 基本機能
	│├ xie_high_x64_100.dll     … 上位機能
	├ XIE.Tasks.dll

◇ 32bit版

	XIEstudio.exe
	├ XIE.Core.dll
	│├ xie_core_x86_100.dll     … 基本機能
	│├ xie_high_x86_100.dll     … 上位機能
	├ XIE.Tasks.dll


## トラブルシューティング

**Linux Mono 環境で実行すると例外が発生する。(1)**  
ビルドのソリューション構成が ReleaseLinux (または DebugLinux) を選択しているか調べる。  
これらのソリューションでは "条件付きソリューション構成" に "LINUX" を指定している。  

**Linux Mono 環境で実行すると例外が発生する。(2)**  
フォームの InitializeComponent 内に ISupportInitialize が無いか調べる。  
例）  

	((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();  
	((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();  

**Linux Mono 環境で実行すると例外が発生する。(3)**  
フォームにアイコンを設定していると InitializeComponent で例外が発生する。  
関連) Program.cs 内の "アイコン設定" の処理  
