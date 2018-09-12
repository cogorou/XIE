xie_core
===

これは、基本機能のプロジェクトです。  

## ビルド

ターミナルを起動し、build.sh を実行してください。  

	$ sh build.sh


## クリーン

ビルド結果を消去するには clean.sh を実行してください。  

	$ sh clean.sh

これにより、後述の「構成」の (1)～(3) が消去されます。  


## 構成

**ディレクトリ構成**  

(1) 中間ファイルの格納場所  
(2) ビルド結果のログ  
(3) 生成物  
(4) 生成物の複製 (ldconfig の対象)  

	XIE  
	├ build  
	│├ gcc  
	││├ xie_core  
	│││├ obj              … (1)  
	│││├ objd             … (1)  
	│││├ make-release.log … (2)  
	│││├ make-debug.log   … (2)  
	│││├ libxie_core.so   … (3)  
	│││├ libxie_cored.so  … (3)  
	├ lib  
	│├ libxie_core.so     … (4)  
	│├ libxie_cored.so    … (4)  
	│  
	├ include             … ヘッダー  
	│├ Core               …
	├ source              … ソースコード  
	│├ xie_core           …


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
		xie_high_setup();           // (1)

		xie::CxImage src1;
		xie::CxImage src2;

		src1.Load("testfile.png");  // (2)
		src2.Load("testfile.jpg");  // (2)

		src1.Save("result.png");    // (3)
		src2.Save("result.jpg");    // (3)

		xie_high_teardown();
		xie_core_teardown();

		return 0;
	}
