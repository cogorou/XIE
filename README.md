XIE
===

XIE は Eggs Imaging Elements の略称です。  
画像処理アプリケーション開発を補助することを目的としたライブラリです。  
データ構造、画像表示、画像取り込み、通信等を実装しています。  

本リポジトリには XIE の中枢部分である C++ 版及び .NET 版を格納しています。  

注意）  
現在、バージョン 1.0 を開発中です。  
機能の追加や変更等により互換性が断たれる可能性があります。  
開発が収束まではバージョン更新を行いませんのでご注意ください。  

## 動作環境と開発環境

下記の説明を参考にしてください。  

- [Windows 版](doc/tech/README-Windows.md)  
- [Linux 版](doc/tech/README-Linux.md)  

## ビルド方法

- [XIE ビルド方法（ライブラリとアプリケーション）](build/README.md)
- [XIE ビルド方法（リファレンスマニュアル）](doc/README.md)

## セットアップ

Windows 版の場合は、インストーラが使用できます。  
Linux 版の場合は、手動セットアップのみです。  

手動セットアップ方法については下記をご参照ください。  

- [XIE 手動セットアップ方法（Windows 版）](doc/tech/README-Windows-Setup.md)
- [XIE 手動セットアップ方法（Linux 版）](doc/tech/README-Linux-Setup.md)

## ユーティリティの説明
- [XIE-Capture](doc/app/XIEcapture/README.md)  
- [XIE-Prompt](doc/app/XIEprompt/README.md)  
- [XIE-Script](doc/app/XIEscript/README.md)  
- [XIE-Version](doc/app/XIEversion/README.md)  

## デモ
- [XIE デモプロジェクト](demo/README.md)

## バージョン表記

|セグメント|範囲|備考|  
|----------|----|----|  
|major|1~255|ソース互換が断たれた場合に更新します。|  
|minor|0~99|バイナリ互換が断たれた場合に更新します。その他、機能追加を行った場合にも更新します。major が更新された場合に 0 リセットされます。|  
|build|0~|正規版の場合は常に 0 です。暫定版を特定ユーザーに配布する際に使用します。major、minor が更新された場合に 0 リセットされます。|  
|revision|0~|リファクタリングやバグフィックスを行った場合に更新します。major、minor、build が更新された場合に 0 リセットされます。|  


## 著作者

[cogorou](https://github.com/cogorou)

Eggs Imaging Laboratory
