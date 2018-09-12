XIE 開発メモ（Windows 版）
====

ここでは XIE の Windows 版の開発に関する説明を記載します。

## 構成

作業環境のディレクトリ構成は下図のようになっています。

	(Drive)
	├ EggsRepos
	│├ XIE_1.0.0
	││├ XIE      … 本体

## 実行環境

**OS**

- Microsoft Windows 7   [32bit/64bit]  (SP1)
- Microsoft Windows 10   [32bit/64bit]  

**必要なソフトウェア**

- .NET Framework 4.0  
  
- Visual C++ v12.0 ランタイム  
  [https://www.microsoft.com/ja-jp/download/details.aspx?id=40784](https://www.microsoft.com/ja-jp/download/details.aspx?id=40784)  
  

## 開発環境

**OS**

- Microsoft Windows 7   [32bit/64bit]  (SP1)
- Microsoft Windows 10   [32bit/64bit]  

**IDE**

- Microsoft Visual Studio 2013  
  Community または Professional 以上を対象とします。  

**開発言語**

- Visual C++ 12.0  
  C++ 11 以降の仕様に依存します。  
  
- Visual C#  
  C# 4.0 以降の仕様に依存します。  
  

**関連ソフトウェア**

- Visual Studio 2013 のマルチバイト MFC ライブラリ  
  [http://www.microsoft.com/ja-jp/download/details.aspx?id=40770](http://www.microsoft.com/ja-jp/download/details.aspx?id=40770)  
  このライブラリは、MFC を使用したアプリケーション開発を行う場合にのみ必要です。  


## リファレンスマニュアル生成環境

- doxygen [1.8.7]  
  [http://www.stack.nl/~dimitri/doxygen/download.html](http://www.stack.nl/~dimitri/doxygen/download.html)  
  
- HTML Help Workshop  
  [https://msdn.microsoft.com/ja-jp/library/windows/desktop/ms669985%28v=vs.85%29.aspx](https://msdn.microsoft.com/ja-jp/library/windows/desktop/ms669985%28v=vs.85%29.aspx)  
  
- Sandcastle - June 2010 [2.6.1062.1]  
  [http://sandcastle.codeplex.com/](http://sandcastle.codeplex.com/)  
  
- Sandcastle Help File Builder [1.9.8.0]  
  [https://shfb.codeplex.com/](https://shfb.codeplex.com/)  
  

## インストーラ生成環境

- Windows Installer XML (WiX) toolset [3.10]  
  [http://wix.sourceforge.net/](http://wix.sourceforge.net/)  
  file) wix38.msi  
  
- WiX Edit [0.7.5]  
  [http://wixedit.sourceforge.net/](http://wixedit.sourceforge.net/)  
  file) wixedit-0.7.5.msi  
  

## 参考）  

- 開発ツール対応 OS 一覧  
  [http://www.microsoft.com/japan/msdn/vstudio/support/tools.aspx](http://www.microsoft.com/japan/msdn/vstudio/support/tools.aspx)  
  
- Memory Limits for Windows and Windows Server Releases  
  [http://msdn.microsoft.com/en-us/library/aa366778(v=vs.85).aspx](http://msdn.microsoft.com/en-us/library/aa366778(v=vs.85).aspx)  
