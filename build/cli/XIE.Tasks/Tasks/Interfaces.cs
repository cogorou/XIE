/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Reflection;

namespace XIE.Tasks
{
	#region IxAuxNode

	/// <summary>
	/// 外部機器ツリーノードのインターフェース
	/// </summary>
	public interface IxAuxNode
	{
		/// <summary>
		/// 外部機器情報 (通常は CxAuxInfo のインスタンスが設定されます)
		/// </summary>
		CxAuxInfo AuxInfo
		{
			get;
			set;
		}

		/// <summary>
		/// ダブルクリックされたとき
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">イベント引数</param>
		void DoubleClick(object sender, EventArgs e);

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">イベント引数</param>
		void Tick(object sender, EventArgs e);
	}

	#endregion

	#region IxAuxNodeImageOut

	/// <summary>
	/// 画像オブジェクトを出力するツリーノードのインターフェース
	/// </summary>
	public interface IxAuxNodeImageOut
	{
		/// <summary>
		/// 描画処理
		/// </summary>
		/// <param name="view">描画先</param>
		void Draw(XIE.GDI.CxImageView view);

		/// <summary>
		/// 描画イメージの解除
		/// </summary>
		/// <param name="view">描画先</param>
		void Reset(XIE.GDI.CxImageView view);

		/// <summary>
		/// 描画処理 (描画中の処理)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Rendering(object sender, XIE.GDI.CxRenderingEventArgs e);

		/// <summary>
		/// 描画処理 (描画完了時の処理)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Rendered(object sender, XIE.GDI.CxRenderingEventArgs e);

		/// <summary>
		/// 図形操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Handling(object sender, XIE.GDI.CxHandlingEventArgs e);

		/// <summary>
		/// 処理範囲
		/// </summary>
		XIE.TxRectangleI ROI { get; set; }
	}

	#endregion

	#region IxAuxImageList16

	/// <summary>
	/// 16x16 イメージリストインターフェース
	/// </summary>
	public interface IxAuxImageList16
	{
		#region プロパティ:

		/// <summary>
		/// イメージリスト
		/// </summary>
		ImageList ImageList
		{
			get;
		}

		#endregion
	}

	#endregion

	// AuxInfo

	#region IxAuxInfoAudioInputs

	/// <summary>
	/// 音声入力デバイスコレクションインターフェース
	/// </summary>
	public interface IxAuxInfoAudioInputs
	{
		#region プロパティ:

		/// <summary>
		/// デバイス情報コレクション
		/// </summary>
		XIE.Media.CxDeviceParam[] Infos
		{
			get;
		}

		#endregion

		#region 追加と削除.

		/// <summary>
		/// 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		void Add(XIE.Media.CxDeviceParam info);

		/// <summary>
		/// 全て削除.
		/// </summary>
		void RemoveAll();

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void RemoveAt(int index);

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="src">デバイス情報</param>
		void Remove(object src);

		/// <summary>
		/// 検索.
		/// </summary>
		/// <param name="src">デバイス情報</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int Find(object src);

		#endregion
	}

	#endregion

	#region IxAuxInfoAudioOutputs

	/// <summary>
	/// 音声出力デバイスコレクションインターフェース
	/// </summary>
	public interface IxAuxInfoAudioOutputs
	{
		#region プロパティ:

		/// <summary>
		/// デバイス情報コレクション
		/// </summary>
		XIE.Media.CxDeviceParam[] Infos
		{
			get;
		}

		#endregion

		#region 追加と削除.

		/// <summary>
		/// 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		void Add(XIE.Media.CxDeviceParam info);

		/// <summary>
		/// 全て削除.
		/// </summary>
		void RemoveAll();

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void RemoveAt(int index);

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="src">デバイス情報</param>
		void Remove(object src);

		/// <summary>
		/// 検索.
		/// </summary>
		/// <param name="src">デバイス情報</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int Find(object src);

		#endregion
	}

	#endregion

	#region IxAuxInfoCameras

	/// <summary>
	/// カメラデバイスコレクションインターフェース
	/// </summary>
	public interface IxAuxInfoCameras
	{
		#region プロパティ:

		/// <summary>
		/// デバイス情報コレクション
		/// </summary>
		XIE.Media.CxDeviceParam[] Infos
		{
			get;
		}

		/// <summary>
		/// コントローラコレクション
		/// </summary>
		XIE.Media.CxCamera[] Controllers
		{
			get;
		}

		#endregion

		#region 追加と削除.

		/// <summary>
		/// 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		void Add(XIE.Media.CxDeviceParam info, XIE.Media.CxCamera controller);

		/// <summary>
		/// 全て削除.
		/// </summary>
		void RemoveAll();

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void RemoveAt(int index);

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		void Remove(object src);

		/// <summary>
		/// 検索.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int Find(object src);

		#endregion
	}

	#endregion

	#region IxAuxInfoGrabbers

	/// <summary>
	/// イメージグラバーコレクションインターフェース
	/// </summary>
	public interface IxAuxInfoGrabbers
	{
		#region プロパティ:

		/// <summary>
		/// デバイス情報コレクション
		/// </summary>
		CxGrabberInfo[] Infos
		{
			get;
		}

		/// <summary>
		/// コントローラコレクション
		/// </summary>
		CxGrabberThread[] Controllers
		{
			get;
		}

		#endregion

		#region 追加と削除.

		/// <summary>
		/// 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		void Add(XIE.Tasks.CxGrabberInfo info, CxGrabberThread controller);

		/// <summary>
		/// 全て削除.
		/// </summary>
		void RemoveAll();

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void RemoveAt(int index);

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		void Remove(object src);

		/// <summary>
		/// 検索.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int Find(object src);

		#endregion
	}

	#endregion

	#region IxAuxInfoDataPorts

	/// <summary>
	/// データ入出力ポートコレクションインターフェース
	/// </summary>
	public interface IxAuxInfoDataPorts
	{
		#region プロパティ:

		/// <summary>
		/// デバイス情報コレクション
		/// </summary>
		CxDataPortInfo[] Infos
		{
			get;
		}

		/// <summary>
		/// コントローラコレクション
		/// </summary>
		CxDataPortThread[] Controllers
		{
			get;
		}

		#endregion

		#region 追加と削除.

		/// <summary>
		/// 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		void Add(XIE.Tasks.CxDataPortInfo info, CxDataPortThread controller);

		/// <summary>
		/// 全て削除.
		/// </summary>
		void RemoveAll();

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void RemoveAt(int index);

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		void Remove(object src);

		/// <summary>
		/// 検索.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int Find(object src);

		#endregion
	}

	#endregion

	#region IxAuxInfoSerialPorts

	/// <summary>
	/// シリアル通信ポートコレクションインターフェース
	/// </summary>
	public interface IxAuxInfoSerialPorts
	{
		#region プロパティ:

		/// <summary>
		/// デバイス情報コレクション
		/// </summary>
		XIE.Tasks.CxSerialPortInfo[] Infos
		{
			get;
		}

		/// <summary>
		/// コントローラコレクション
		/// </summary>
		XIE.IO.CxSerialPort[] Controllers
		{
			get;
		}

		#endregion

		#region 追加と削除.

		/// <summary>
		/// 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		void Add(XIE.Tasks.CxSerialPortInfo info, XIE.IO.CxSerialPort controller);

		/// <summary>
		/// 全て削除.
		/// </summary>
		void RemoveAll();

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void RemoveAt(int index);

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		void Remove(object src);

		/// <summary>
		/// 検索.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int Find(object src);

		#endregion
	}

	#endregion

	#region IxAuxInfoTcpClients

	/// <summary>
	/// TCP/IP 通信クライアントコレクションインターフェース
	/// </summary>
	public interface IxAuxInfoTcpClients
	{
		#region プロパティ:

		/// <summary>
		/// デバイス情報コレクション
		/// </summary>
		XIE.Tasks.CxTcpClientInfo[] Infos
		{
			get;
		}

		/// <summary>
		/// コントローラコレクション
		/// </summary>
		XIE.Net.CxTcpClient[] Controllers
		{
			get;
		}

		#endregion

		#region 追加と削除.

		/// <summary>
		/// 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		void Add(XIE.Tasks.CxTcpClientInfo info, XIE.Net.CxTcpClient controller);

		/// <summary>
		/// 全て削除.
		/// </summary>
		void RemoveAll();

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void RemoveAt(int index);

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		void Remove(object src);

		/// <summary>
		/// 検索.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int Find(object src);

		#endregion
	}

	#endregion

	#region IxAuxInfoTcpServers

	/// <summary>
	/// TCP/IP 通信サーバーコレクションインターフェース
	/// </summary>
	public interface IxAuxInfoTcpServers
	{
		#region プロパティ:

		/// <summary>
		/// デバイス情報コレクション
		/// </summary>
		XIE.Tasks.CxTcpServerInfo[] Infos
		{
			get;
		}

		/// <summary>
		/// コントローラコレクション
		/// </summary>
		XIE.Net.CxTcpServer[] Controllers
		{
			get;
		}

		#endregion

		#region 追加と削除.

		/// <summary>
		/// 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		void Add(XIE.Tasks.CxTcpServerInfo info, XIE.Net.CxTcpServer controller);

		/// <summary>
		/// 全て削除.
		/// </summary>
		void RemoveAll();

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void RemoveAt(int index);

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		void Remove(object src);

		/// <summary>
		/// 検索.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int Find(object src);

		#endregion
	}

	#endregion

	#region IxAuxInfoMedias

	/// <summary>
	/// メディアプレイヤーコレクションインターフェース
	/// </summary>
	public interface IxAuxInfoMedias
	{
		#region プロパティ:

		/// <summary>
		/// 動画ファイル情報コレクション
		/// </summary>
		XIE.Tasks.CxMediaInfo[] Infos
		{
			get;
		}

		/// <summary>
		/// メディアプレイヤーコレクション
		/// </summary>
		XIE.Media.CxMediaPlayer[] Players
		{
			get;
		}

		#endregion

		#region 追加と削除.

		/// <summary>
		/// 追加.
		/// </summary>
		/// <param name="info">動画ファイル情報</param>
		/// <param name="player">メディアプレイヤー</param>
		void Add(XIE.Tasks.CxMediaInfo info, XIE.Media.CxMediaPlayer player);

		/// <summary>
		/// 全て削除.
		/// </summary>
		void RemoveAll();

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void RemoveAt(int index);

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="src">動画ファイル情報またはメディアプレイヤー</param>
		void Remove(object src);

		/// <summary>
		/// 検索.
		/// </summary>
		/// <param name="src">動画ファイル情報またはメディアプレイヤー</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int Find(object src);

		#endregion
	}

	#endregion

	#region IxAuxInfoImages

	/// <summary>
	/// 画像オブジェクトコレクションインターフェース
	/// </summary>
	public interface IxAuxInfoImages
	{
		#region プロパティ:

		/// <summary>
		/// データ情報コレクション
		/// </summary>
		XIE.Tasks.CxImageInfo[] Infos
		{
			get;
		}

		/// <summary>
		/// データオブジェクトコレクション
		/// </summary>
		XIE.CxImage[] Datas
		{
			get;
		}

		#endregion

		#region 追加と削除.

		/// <summary>
		/// 追加.
		/// </summary>
		/// <param name="info">データ情報</param>
		/// <param name="data">データオブジェクト</param>
		void Add(XIE.Tasks.CxImageInfo info, XIE.CxImage data);

		/// <summary>
		/// 全て削除.
		/// </summary>
		void RemoveAll();

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void RemoveAt(int index);

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="src">データ情報またはデータオブジェクト</param>
		void Remove(object src);

		/// <summary>
		/// 検索.
		/// </summary>
		/// <param name="src">データ情報またはデータオブジェクト</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int Find(object src);

		#endregion
	}

	#endregion

	#region IxAuxInfoTasks

	/// <summary>
	/// タスクユニットコレクションインターフェース
	/// </summary>
	public interface IxAuxInfoTasks
	{
		#region プロパティ:

		/// <summary>
		/// タスクユニット情報コレクション
		/// </summary>
		XIE.Tasks.CxTaskUnitInfo[] Infos
		{
			get;
		}

		/// <summary>
		/// タスクユニットコレクション
		/// </summary>
		XIE.Tasks.CxTaskUnit[] Tasks
		{
			get;
		}

		#endregion

		#region 追加と削除.

		/// <summary>
		/// 追加.
		/// </summary>
		/// <param name="info">タスクユニット情報</param>
		/// <param name="task">タスクユニット</param>
		void Add(XIE.Tasks.CxTaskUnitInfo info, XIE.Tasks.CxTaskUnit task);

		/// <summary>
		/// 全て削除.
		/// </summary>
		void RemoveAll();

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void RemoveAt(int index);

		/// <summary>
		/// 削除.
		/// </summary>
		/// <param name="src">タスクユニット情報またはタスクユニット</param>
		void Remove(object src);

		/// <summary>
		/// 検索.
		/// </summary>
		/// <param name="src">タスクユニット情報またはタスクユニット</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int Find(object src);

		#endregion
	}

	#endregion

	// Tasks

	#region IxTaskControlPanel

	/// <summary>
	/// コントロールパネルを持つタスクのインターフェース
	/// </summary>
	public interface IxTaskControlPanel
	{
		/// <summary>
		/// コントロールパネルの生成
		/// </summary>
		/// <param name="args">実行時引数</param>
		/// <param name="options">オプション</param>
		/// <returns>
		///		モードレスダイアログの場合は生成したコントロールパネルを返します。
		///		モーダルダイアログの場合は内部で表示して完了後に null を返します。
		/// </returns>
		Form Create(CxTaskExecuteEventArgs args, params object[] options);

		/// <summary>
		/// コントロールパネルの表示状態
		/// </summary>
		/// <remarks>
		///		モードレスダイアログの表示中は true 、未表示の場合は false を返します。
		///		モーダルダイアログの場合は常に false を返します。
		/// </remarks>
		bool IsOpened { get; }
	}

	#endregion

	#region IxTaskOutputImage

	/// <summary>
	/// 画像ビューへ画像表示を行うタスクのインターフェース
	/// </summary>
	public interface IxTaskOutputImage
	{
		/// <summary>
		/// 画像表示
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">画像更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void OutputImage(XIE.GDI.CxImageView view, bool update);
	}

	#endregion

	#region IxTaskOutputReport

	/// <summary>
	/// データグリッドへレポート出力を行うタスクのインターフェース
	/// </summary>
	public interface IxTaskOutputReport
	{
		/// <summary>
		/// レポート出力
		/// </summary>
		/// <param name="view">出力先</param>
		/// <param name="update">データセット更新の指示 (true の場合は再生成します。false の場合は前回の結果を維持します。)</param>
		void OutputReport(DataGridView view, bool update);

		/// <summary>
		/// レポート出力の有効属性
		/// </summary>
		bool EnableReport { get; set; }

		/// <summary>
		/// レポート出力プロパティフォームを保有しているか否か
		/// </summary>
		bool HasReportPropertyForm { get; }

		/// <summary>
		/// レポート出力プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したレポート出力プロパティフォームを返します。
		///		対応していない場合は null を返します。
		/// </returns>
		Form OpenReportPropertyForm(params object[] options);
	}

	#endregion

	#region IxTaskOverlayRendering

	/// <summary>
	/// 画像ビューへオーバレイ表示を行うタスクのインターフェース
	/// </summary>
	public interface IxTaskOverlayRendering
	{
		/// <summary>
		/// オーバレイ描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Rendering(object sender, XIE.GDI.CxRenderingEventArgs e);

		/// <summary>
		/// オーバレイ描画の有効属性
		/// </summary>
		bool EnableRendering { get; set; }

		/// <summary>
		/// オーバレイ描画プロパティフォームを保有しているか否か
		/// </summary>
		bool HasRenderingPropertyForm { get; }

		/// <summary>
		/// オーバレイ描画プロパティフォームの生成
		/// </summary>
		/// <param name="options">オプション</param>
		/// <returns>
		///		生成したオーバレイ描画プロパティフォームを返します。
		///		対応していない場合は null を返します。
		/// </returns>
		Form OpenRenderingPropertyForm(params object[] options);
	}

	#endregion

	#region IxTaskOverlayRendered

	/// <summary>
	/// 画像ビューへオーバレイ表示を行うタスクのインターフェース
	/// </summary>
	public interface IxTaskOverlayRendered
	{
		/// <summary>
		/// オーバレイ描画処理 (描画完了後)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Rendered(object sender, XIE.GDI.CxRenderingEventArgs e);
	}

	#endregion

	#region IxTaskOverlayHandling

	/// <summary>
	/// 画像ビューのオーバレイ操作を行うタスクのインターフェース
	/// </summary>
	public interface IxTaskOverlayHandling
	{
		/// <summary>
		/// オーバレイ操作処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Handling(object sender, XIE.GDI.CxHandlingEventArgs e);

		/// <summary>
		/// オーバレイ操作の有効属性
		/// </summary>
		bool EnableHandling { get; set; }
	}

	#endregion
}
