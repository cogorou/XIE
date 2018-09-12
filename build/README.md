XIE ビルド方法
===

## .NET アセンブリの署名

本リポジトリにはアセンブリのキーファイル (.snk) は格納されていません。  
この状態で後述のビルドを実行すると、厳密名を持たないアセンブリが生成されます。  

下記のアセンブリを GAC に登録する必要が有る場合は厳密名を署名しなければなりません。  

- XIE/build/cli/XIE.Core
- XIE/build/cli/XIE.Tasks

もし、アセンブリに厳密名を署名する必要があれば、下記のように開発環境の任意の位置に新しく生成したキーファイルを配置し、配置された場所を示す環境変数を生成してください。

**キーファイルの配置の例**

	C:\
	├ dev
	│├ snk
	││├ XIE.snk

**環境変数の例**

|環境変数|値|  
|--------|--|  
|XIE_SNK|C:\dev\snk\XIE.snk|  


前述のアセンブリのプロジェクトは **XIE_SNK** 環境変数に示された位置のキーファイルを参照しています。
存在しなければ署名が無いアセンブリを生成します。

## ライブラリとアプリケーション

下記の手順でビルドしてください。  

1. XIE C++ 版  
2. XIE .NET 版  

**Windows**  
Windows 環境では下記のバッチファイルでビルドできます。  
1. XIE/build/vc/\_build.bat  
2. XIE/build/cli/\_build.bat  

**Linux**  
Linux 環境では下記のシェルスクリプトでビルドできます。  
1. XIE/build/gcc/build.sh  
2. XIE/build/cli/build.sh  

### インストーラの生成

Windows 版の場合はインストーラを生成できます。  
この作業は Windows 環境で行ってください。  
※ 生成するには WiX が必要です。  

下記は操作手順の例です。  
完了すると XIE/build/wix/XIE の配下に XIE.msi が生成されます。  

	c:> cd XIE/build/wix
	c:> _build.bat
