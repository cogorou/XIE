XIE.Core
====

これは、.NET/mono 版の中枢のプロジェクトです。

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
	│├ XIE.Core.dll                … (1) AnyCPU (Release) 版
	│└ XIE.Core.XML                …     Intellisence ファイル
	├ bin-debug
	│├ XIE.Core.dll                … (1) AnyCPU (Debug) 版
	│└ XIE.Core.XML                …     Intellisence ファイル
	│
	├ build
	│├ cli
	││├ XIE.Core
	│││├ bin                     … (2) AnyCPU 版
	││││├ Debug
	│││││├ XIE.Core.dll        … (2) Debug 版
	│││││└ XIE.Core.XML        …     Intellisence ファイル
	││││├ Release
	│││││├ XIE.Core.dll        … (2) Release 版
	│││││└ XIE.Core.XML        …     Intellisence ファイル
	││││
	│││├ obj                     … (3)
	││││├ Debug
	││││├ Release
	││││
	│││└ XIE.Core.log           … (4)


**依存関係**

XIE.Core は AnyCPU ですので実行時に x64/x86 を分岐しています。

◇ 64bit版

	XIE.Core.dll
	├ xie_core_x64_100.dll      … 基本機能
	├ xie_high_x64_100.dll      … 上位機能

◇ 32bit版

	XIE.Core.dll
	├ xie_core_x86_100.dll      … 基本機能
	├ xie_high_x86_100.dll      … 上位機能
