xie_core
====

これは、基本機能のプロジェクトです。

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
	│├ xie_core_x64_100.dll      … (1) 64 bit (Release) 版
	│└ xie_core_x86_100.dll      … (1) 32 bit (Release) 版
	├ bin-debug
	│├ xie_core_x64_100.dll      … (1) 64 bit (Debug) 版
	│└ xie_core_x86_100.dll      … (1) 32 bit (Debug) 版
	│
	├ build
	│├ vc
	││├ xie_core
	│││├ Win32                   … (2) 32 bit 版
	││││├ Debug
	│││││└ xie_core_x86_100.dll  … (3) Debug 版
	││││├ Release
	│││││└ xie_core_x86_100.dll  … (3) Release 版
	││││
	│││├ x64                     … (2) 64 bit 版
	││││├ Debug
	│││││└ xie_core_x64_100.dll  … (3) Debug 版
	││││├ Release
	│││││└ xie_core_x64_100.dll  … (3) Release 版
	││││
	│││└ xie_core.log            … (4)
	│
	├ include                    … ヘッダー
	│├ Core                      …
	├ source                     … ソースコード
	│├ xie_core                  …


**依存関係**

◇ 64bit版

	xie_core_x64_100.dll
	├ xie_high_x64_100.dll      … ファイル入出力プラグイン

◇ 32bit版

	xie_core_x86_100.dll
	├ xie_high_x86_100.dll      … ファイル入出力プラグイン

※1) 後述、「プラグイン」の項の説明をご参照ください。

## プラグイン
**CPU 版のデータオブジェクトのファイル入出力**  
データオブジェクトのファイル入出力はプラグインになっています。
bmp,raw は内部実装されていますが、
png,jpeg はプラグインをロードした後、使用可能になります。
下記 (1) のように初期化関数を実行した後、
下記 (2) (3) のような操作が可能になります。

初期化の例)
	#include "xie_core.h"
	#include "xie_high.h"

	int main(int argc, char** argv)
	{
		xie_core_setup();
		xie_high_setup();             // (1)

		xie::CxImage src1;
		xie::CxImage src2;

		src1.Load("testfile.png");    // (2)
		src2.Load("testfile.jpg");    // (2)

		src1.Save("result.png");      // (3)
		src2.Save("result.jpg");      // (3)

		xie_high_teardown();
		xie_core_teardown();

		return 0;
	}
