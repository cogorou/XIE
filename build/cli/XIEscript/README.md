XIEscript
====

これは、.NET/mono 版のスクリプト実行ユーティリティのプロジェクトです。  
言語は C# と VisualBasic に対応しています。  


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
	│├ XIEscript.exe             … (1) AnyCPU (Release) 版
	│└ XIEscript.exe.config      …     アプリケーション構成ファイル
	├ bin-debug
	│├ XIEscript.exe             … (1) AnyCPU (Debug) 版
	│└ XIEscript.exe.config      …     アプリケーション構成ファイル
	│
	├ build
	│├ cli
	││├ XIE.Core
	│││├ bin
	││││├ Debug
	││││├ XIEscript.exe          … (2) Debug 版
	││││└ XIEscript.exe.config   …     アプリケーション構成ファイル
	││││├ Release
	││││├ XIEscript.exe          … (2) Release 版
	││││└ XIEscript.exe.config   …     アプリケーション構成ファイル
	││││
	│││├ obj                     … (3)
	││││├ Debug
	││││├ Release
	││││
	│││└ XIEscript.log           … (4)


**依存関係**

現在のところ BCL にのみ依存し、XIE には依存しない構成にしています。  
将来は、XIE.Core に依存して XIE に特化した機能を実装する予定です。  


## 使用方法

コマンドプロンプト（Linux ではターミナル）を起動して XIEscript.exe を実行します。  
XIEscript.exe が有るディレクトリに PATH が通っていなければ
XIEscript.exe があるディレクトリへ移動してください。  

例）  
Windows 環境  

	＞ cd c:\Eggs\XIE-1.0\bin
	＞ XIEscript.exe

Linux 環境  

	$ cd ~/XIE/bin
	$ XIEscript.exe


引数を指定せずに実行すると使用方法が表示されますので参考にしてください。  

	Usage:
	   XIEscript [command]
	   XIEscript [switch] [arguments]

	command)
	   /? ... show help.
	   /v ... show version.
	   /gen:(file) ... generate a Program.Main.

	switch)
	   /t:(files) ... specify a target files. [necessary]
	   /e:(method) ... specify an entry point method name.
	   /r:(assemblies) ... specify reference assembly.

	language)
	   Visual C# ... The file extension must be .cs
	   Visual Basic ... The file extension must be .vb

	1) When generate a Program.Main .
	   XIEscript /gen:Program.cs
	             ~~~~~~~~~~~~~~~
	2) When has a Program.Main .
	   XIEscript /t:Program.cs
	             ~~~~~~~~~~~~~
	3) When does not have a Program.Main .
	   XIEscript /t:Program.cs /e:User.Hello.Start
	                           ~~~~~~~~~~~~~~~~~~~
	4) If you want to specify more than one file.
	   XIEscript /t:Program.cs,Class1.cs,Class2.cs
	                          ~~~~~~~~~~~~~~~~~~~~
	5) If you want to specify reference assembly.
	   XIEscript /t:Program.cs /r:User1.dll,User2.dll
	                           ~~~~~~~~~~~~~~~~~~~~~~
	6) If you want to specify arguments.
	   XIEscript /t:Program.cs arg1 arg2 arg3
	                           ~~~~~~~~~~~~~~

最も単純な方法は下記手順です。

- 雛形を生成する。
- 雛形を編集して目的の処理を記述する。
- スクリプトを実行する。

**◇ 雛形を生成する。**

下記のように引数に `/gen:（ファイル名）` を指定すると雛形を生成します。

	＞ XIEscript.exe /gen:Program.cs

生成される雛形は下記です。  

	namespace User
	{
		using System;
		using System.Collections.Generic;
		using System.Windows.Forms;
		using System.Reflection;

		static class Program
		{
			public static void Main(string[] args)
			{
				XIE.Axi.Setup();

				XIE.CxImage src = new XIE.CxImage();
				XIE.CxImage dst = new XIE.CxImage();

				try
				{
				}
				finally
				{
					src.Dispose();
					dst.Dispose();
				}
			}
		}
	}

**◇ 雛形を編集して目的の処理を記述する。**

テキストエディタで雛形を編集して目的の処理を記述します。  
下記は test.jpg を読み込んで濃淡化して
test-dst.png に保存する例です。

	namespace User
	{
		using System;
		using System.Collections.Generic;
		using System.Windows.Forms;
		using System.Reflection;

		static class Program
		{
			public static void Main(string[] args)
			{
				XIE.Axi.Setup();

				XIE.CxImage src = new XIE.CxImage("test.jpg");
				XIE.CxImage dst = new XIE.CxImage();

				try
				{
					dst.Filter().RgbToGray(src);
					dst.Save("test-dst.png");
				}
				finally
				{
					src.Dispose();
					dst.Dispose();
				}
			}
		}
	}

**◇ スクリプトを実行する。**

下記のように引数に `/t:（ファイル名）` を指定するとスクリプトを実行します。

	＞ XIEscript.exe /t:Program.cs

ファイルが複数有る場合はカンマで区切って指定してください。

	＞ XIEscript.exe /t:Program.cs,sub1.cs,sub2.cs

参照アセンブリを追加する場合は、引数に `/r:（ファイル名）` を指定してください。

	＞ XIEscript.exe /t:Program.cs /r:OpenCvSharp.dll

オプション以外の引数はスクリプトに渡されます。  
例えば、入力画像と出力画像をスクリプトに渡す場合は下記のように記述します。  

	＞ XIEscript.exe /t:Program.cs test.jpg test-dst.png

スクリプト側は、下記のように引数を受け取れます。  

	namespace User
	{
		using System;
		using System.Collections.Generic;
		using System.Windows.Forms;
		using System.Reflection;

		static class Program
		{
			public static void Main(string[] args)
			{
				string srcfile = args[0];	// 入力画像.
				string dstfile = args[1];	// 出力画像.

				XIE.Axi.Setup();

				XIE.CxImage src = new XIE.CxImage(srcfile);
				XIE.CxImage dst = new XIE.CxImage();

				try
				{
					dst.Filter().RgbToGray(src);
					dst.Save(dstfile);
				}
				finally
				{
					src.Dispose();
					dst.Dispose();
				}
			}
		}
	}


## トラブルシューティング

**Linux Mono 環境で実行すると例外が発生する。(1)**  
ビルドのソリューション構成が ReleaseLinux (または DebugLinux) を選択しているか調べる。  
これらのソリューションでは "条件付きソリューション構成" に "LINUX" を指定している。  
