XIE ビルド方法
===

## リファレンスマニュアル

リファレンスマニュアルの生成は Windows 環境でのみ可能です。  

1. XIE/doc/cli/\_build.bat  
2. XIE/doc/cpp/\_build.bat  

### インストーラの生成

Windows 版の場合はインストーラを生成できます。  
この作業は Windows 環境で行ってください。  
※ 生成するには WiX が必要です。  

下記は操作手順の例です。  
完了すると XIE/doc/wix/XIE-Documents の配下に XIE-Documents.msi が生成されます。  

	c:> cd XIE/doc/wix
	c:> _build.bat
