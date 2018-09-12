XIE.Tasks
====

これは、.NET/mono 版のタスク関連プロジェクトです。

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
	│├ XIE.Tasks.dll               … (1) AnyCPU (Release) 版
	│└ XIE.Tasks.XML               …     Intellisence ファイル
	├ bin-debug
	│├ XIE.Tasks.dll               … (1) AnyCPU (Debug) 版
	│└ XIE.Tasks.XML               …     Intellisence ファイル
	│
	├ build
	│├ cli
	││├ XIE.Tasks
	│││├ bin                     … (2) AnyCPU 版
	││││├ Debug
	│││││├ XIE.Tasks.dll       … (2) Debug 版
	│││││└ XIE.Tasks.XML       …     Intellisence ファイル
	││││├ Release
	│││││├ XIE.Tasks.dll       … (2) Release 版
	│││││└ XIE.Tasks.XML       …     Intellisence ファイル
	││││
	│││├ obj                     … (3)
	││││├ Debug
	││││├ Release
	││││
	│││└ XIE.Tasks.log           … (4)


**依存関係**

XIE.Tasks は AnyCPU ですので実行時に x64/x86 を分岐しています。

◇ 64bit版

	XIE.Tasks.dll
	├ XIE.Core.dll
	│├ xie_core_x64_100.dll      … 基本機能
	│├ xie_high_x64_100.dll      … 上位機能

◇ 32bit版

	XIE.Tasks.dll
	├ XIE.Core.dll
	│├ xie_core_x86_100.dll      … 基本機能
	│├ xie_high_x86_100.dll      … 上位機能

## トラブルシューティング

**Linux Mono 環境でビルドするとコンパイルエラーが発生する。**  
Visual Studio でフォームを追加した際に大文字/小文字が一致しない条件になっている場合がある。  
例えば CxTestForm を追加した場合、下記の３ファイルが生成される。  

	CxTestForm.cs
	CxTestForm.Designer.cs
	CxTestForm.resx

この時、XIE.Tasks.csproj では CxTestForm.designer.cs となっている場合がある。  
この場合は XIE.Tasks.csproj  をテキストエディタで開いて編集する。  


