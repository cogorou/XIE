XIEprompt
====

これは、.NET/mono 版の評価アプリケーションのプロジェクトです。  
主に XIE-Studio で生成したソースコードを実行することを目的にしています。  

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

**プロジェクト構成**  

このプロジェクトにはエントリポイントのみが含まれており、
アプリケーション本体（フォーム等）は XIE.Tasks に含まれます。
このような構成にした理由は Azuki テキストエディタを１箇所に配置したかった為です。
Azuki テキストエディタを複数の機能から使用していますが、
XIE.Tasks がグローバルアセンブリである為、
Azuki テキストエディタの干渉を避ける為 private にしています。
その為、フォーム等の多くの処理を XIE.Tasks アセンブリ内に配置しています。

**ディレクトリ構成**  

(1) 生成物の複製  
(2) 生成物  
(3) 中間ファイルの格納場所  
(4) ビルド結果のログ  

	XIE
	├ bin
	│├ XIEprompt.exe               … (1) AnyCPU (Release) 版
	│├ XIEprompt.exe.config        …     アプリケーション構成ファイル
	│└ XIEprompt.XML               …     Intellisence ファイル
	├ bin-debug
	│├ XIEprompt.exe               … (1) AnyCPU (Debug) 版
	│├ XIEprompt.exe.config        …     アプリケーション構成ファイル
	│└ XIEprompt.XML               …     Intelisence ファイル
	│
	├ build
	│├ cli
	││├ XIEprompt
	│││├ bin                     … (2) AnyCPU 版
	││││├ Debug
	││││├ XIEprompt.exe         … (2) Debug 版
	││││├ XIEprompt.exe.config  …     アプリケーション構成ファイル
	││││└ XIEprompt.XML         …     Intellisence ファイル
	││││├ Release
	││││├ XIEprompt.exe         … (2) Release 版
	││││├ XIEprompt.exe.config  …     アプリケーション構成ファイル
	││││└ XIEprompt.XML         …     Intelisence ファイル
	││││
	│││├ obj                     … (3)
	││││├ Debug
	││││├ Release
	││││
	│││└ XIEprompt.log           … (4)


**依存関係**

XIEprompt 及び依存するアセンブリは AnyCPU ですので実行時に x64/x86 を分岐しています。

◇ 64bit版

	XIEprompt.exe
	├ XIE.Core.dll
	│├ xie_core_x64_100.dll     … 基本機能
	│├ xie_high_x64_100.dll     … 上位機能
	├ XIE.Tasks.dll

◇ 32bit版

	XIEprompt.exe
	├ XIE.Core.dll
	│├ xie_core_x86_100.dll     … 基本機能
	│├ xie_high_x86_100.dll     … 上位機能
	├ XIE.Tasks.dll
