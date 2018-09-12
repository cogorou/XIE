XIE 手動セットアップ方法（Windows 版）
====

インストーラ (XIE.msi) を使用せず手動で行う場合の手順を示します。  

## インストール

この作業は XIEversion.exe を使用すると容易に行えます。  

GUI から行う場合、XIEversion.exe を起動して Register ボタンを押下してください。  
コマンドラインから行う場合は下記のように引数を指定して実行してください。  

	c:> cd XIE/bin  
	c:> XIEversion /i  

その後、起動中のエクスプローラや Visual Studio を再起動すると反映されます。  

XIEversion.exe は下記の作業を一括して行います。

- 環境変数生成と編集
- AssemblyFoldersEx 設定
- GAC 登録
- COM 登録

**環境変数**

|環境変数|値|  
|--------|--|  
|PATH|...(snip)...;**%XIE100BIN%;**|  
|XIE100ASM |(your development directory)\XIE\bin|  
|XIE100BIN |(your development directory)\XIE\bin|  
|XIE100ROOT|(your development directory)\XIE|  

**AssemblyFoldersEx**

|O/S|キー|  
|---|----|  
|32bit|HKLM\SOFTWARE\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\XIE 1.0|  
|64bit|HKLM\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\XIE 1.0|  

**GAC 登録**

|アセンブリ|ディレクトリ|  
|----------|------------|  
|XIE.Core|C:\Windows\Microsoft.NET\assembly\GAC_MSIL\XIE.Core|  
|XIE.Tasks|C:\Windows\Microsoft.NET\assembly\GAC_MSIL\XIE.Tasks|  

**COM 登録**

|項目|値|  
|----|--|  
|FriendlyName|CxDSScreenCaptureFilter|  
|CLSID|74EB5751-A444-4798-AC1A-05175E2CCE8F|  


## アンインストール

この作業は XIEversion.exe を使用すると容易に行えます。  
下記のように引数を指定して実行してください。  

	c:> cd XIE/bin  
	c:> XIEversion /u  

その後、起動中のエクスプローラや Visual Studio を再起動すると反映されます。  

XIEversion.exe は下記の作業を一括して行います。

- 環境変数削除と編集
- AssemblyFoldersEx 設定解除
- GAC 登録解除
- COM 登録解除

作業完了後、関連ファイルを削除してください。  
