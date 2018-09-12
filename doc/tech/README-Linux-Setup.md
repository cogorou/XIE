XIE 手動セットアップ方法（Linux 版）
====

## インストール

ldconfig の設定が必要です。  
下記のように XIE の .so があるディレクトリへのパスが書かれたテキストファイルを  
/etc/ld.so.conf.d に配置して ldconfig を実行してください。

XIE.conf

	/home/(your account)/XIE/lib

下記は操作手順の例です。  

	$ cd /home/(your account)/XIE/lib
	$ su  
	# echo `pwd` > /etc/ld.so.conf.d/XIE.conf  
	# ldconfig  


## アンインストール

ldconfig の設定解除を行い、XIE 関連ファイルを削除してください。  
下記は操作手順の例です。  

	$ su  
	# cd /etc/ld.so.conf.d  
	# rm XIE.conf  
	# exit
	$ cd
	$ rm -rf ~/XIE
