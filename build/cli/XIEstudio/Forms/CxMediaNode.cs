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
using System.Threading;
using System.Runtime.InteropServices;

namespace XIEstudio
{
	/// <summary>
	/// DirectShow MediaPlayer
	/// </summary>
	public class CxMediaNode : TreeNode
		, IDisposable
		, XIE.Tasks.IxAuxNode
		, XIE.Tasks.IxAuxNodeImageOut
		, XIE.IxRunnable
		, XIE.IxParam
	{
		#region コンストラクタ/デストラクタ.

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxMediaNode()
		{
			this.Info = new XIE.Tasks.CxMediaInfo();
			this.Player = null;
			this.FileTime = DateTime.Now;
			this.UpdatedTime = this.FileTime;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="info">動画ファイル情報</param>
		/// <param name="player">メディアプレイヤー</param>
		public CxMediaNode(XIE.Tasks.CxMediaInfo info, XIE.Media.CxMediaPlayer player)
		{
			this.Info = info;
			this.Player = player;

			if (string.IsNullOrEmpty(info.FileName) == false &&
				System.IO.File.Exists(info.FileName))
			{
				this.FileTime = System.IO.File.GetLastWriteTime(info.FileName);
			}
		}

		#endregion

		#region 接続と切断:

		/// <summary>
		/// メディアプレイヤーの接続.
		/// </summary>
		/// <param name="config">外部機器情報</param>
		/// <returns>
		///		生成したノードを配列に格納して返します。
		///		生成に失敗した要素は null が格納されています。
		/// </returns>
		public static CxMediaNode[] Connect(XIE.Tasks.CxAuxInfo config)
		{
			var aux = (XIE.Tasks.IxAuxInfoMedias)config;
			int count = aux.Infos.Length;
			var nodes = new CxMediaNode[count];

			#region 初期化.
			for (int i = 0; i < count; i++)
			{
				try
				{
					var node = new CxMediaNode(aux.Infos[i], aux.Players[i]);
					nodes[i] = node;
					node.AuxInfo = config;
					node.Setup();
				}
				catch (System.Exception ex)
				{
					Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion

			return nodes;
		}

		/// <summary>
		/// 切断
		/// </summary>
		/// <param name="config">外部機器情報</param>
		public virtual void Disconnect(XIE.Tasks.CxAuxInfo config)
		{
			this.Dispose();
			this.Remove();

			// AuxInfo からの除去.
			{
				var aux = (XIE.Tasks.IxAuxInfoMedias)config;
				aux.Remove(this.Info);
			}
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Setup()
		{
			if (this.IsSetuped == true) return;
			this.IsSetuped = true;

			if (string.IsNullOrEmpty(this.Info.FileName) == false)
			{
				string name = System.IO.Path.GetFileName(this.Info.FileName);
				this.Name = name;
				this.Text = name;
				this.ToolTipText = this.Info.FileName;
			}

			if (Player != null)
			{
				XIE.TxImageSize size = Player.GetFrameSize();
				for (int i = 0; i < this.Images.Length; i++)
				{
					if (this.Images[i] == null)
						this.Images[i] = new XIE.CxImage(size);
					else
						this.Images[i].Resize(size);
				}

				VideoGrabber = Player.CreateGrabber(XIE.Media.ExMediaType.Video);
				VideoGrabber.Notify += VideoGrabber_Notify;
				VideoGrabber.Start();
			}
		}

		/// <summary>
		/// 初期化済みフラグ
		/// </summary>
		public virtual bool IsSetuped
		{
			get { return m_IsSetuped; }
			protected set { m_IsSetuped = value; }
		}
		private bool m_IsSetuped = false;

		/// <summary>
		/// IDisposable: 解放
		/// </summary>
		public virtual void Dispose()
		{
			if (VideoGrabber != null)
				VideoGrabber.Dispose();
			VideoGrabber = null;
			Player = null;
			//for (int i = 0; i < this.Images.Length; i++)
			//{
			//	if (this.Images[i] != null)
			//		this.Images[i].Dispose();
			//	this.Images[i] = null;
			//}
			IsSetuped = false;
		}

		#endregion

		#region IxAuxNode の実装:

		/// <summary>
		/// 外部機器情報 (通常は CxAuxInfo のインスタンスが設定されます)
		/// </summary>
		public virtual XIE.Tasks.CxAuxInfo AuxInfo
		{
			get;
			set;
		}

		/// <summary>
		/// ダブルクリックされたとき
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">イベント引数</param>
		public virtual void DoubleClick(object sender, EventArgs e)
		{
			try
			{
				if (Player == null)
					return;
				else if (Player.IsPaused)
					this.Start();
				else if (Player.IsRunning)
					this.Stop();
				else
					this.Start();
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(MethodBase.GetCurrentMethod().Name + ":" + ex.Message);
			}
		}

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">イベント引数</param>
		public virtual void Tick(object sender, EventArgs e)
		{
			#region アイコンの更新.
			string key = "Media-Connect";

			try
			{
				if (Player == null)
					key = "Media-Disconnect";
				else if (Player.IsPaused)
					key = "Media-Pause";
				else if (Player.IsRunning)
					key = "Media-Connect";
				else
					key = "Media-Stop";
			}
			catch (Exception)
			{
			}

			if (this.ImageKey != key)
				this.ImageKey = key;
			if (this.SelectedImageKey != key)
				this.SelectedImageKey = key;
			#endregion
		}

		#endregion

		#region IxAuxNodeImageOut の実装:

		/// <summary>
		/// 描画処理
		/// </summary>
		/// <param name="view">描画先</param>
		public virtual void Draw(XIE.GDI.CxImageView view)
		{
			if (view.Image != this.Data)
			{
				view.Image = this.Data;
				view.Refresh();
			}
		}

		/// <summary>
		/// 描画イメージの解除
		/// </summary>
		/// <param name="view">描画先</param>
		public virtual void Reset(XIE.GDI.CxImageView view)
		{
			if (ReferenceEquals(view.Image, this.Data))
				view.Image = null;
		}

		/// <summary>
		/// 描画処理 (描画中の処理)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
		}

		/// <summary>
		/// 描画処理 (描画完了時の処理)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Rendered(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
		}

		/// <summary>
		/// 図形操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Handling(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
		}

		/// <summary>
		/// 処理範囲
		/// </summary>
		public virtual XIE.TxRectangleI ROI
		{
			get { return m_ROI; }
			set { m_ROI = value; }
		}
		private XIE.TxRectangleI m_ROI = new XIE.TxRectangleI();

		#endregion

		// ////////////////////////////////////////////////////////////
		// イベント.
		//

		#region イベント.

		/// <summary>
		/// 映像捕獲用イベントレシーバ
		/// </summary>
		private XIE.Media.CxGrabber VideoGrabber = null;

		/// <summary>
		/// 周期処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void VideoGrabber_Notify(object sender, XIE.Media.CxGrabberArgs e)
		{
			try
			{
				#region 画像取り込み.
				int index = this.FrameCounter % this.Images.Length;
				XIE.CxImage image = this.Images[index];
				e.CopyTo(image);
				this.Data = image;

				this.FrameCounter++;
				#endregion
			}
			catch (System.Exception)
			{
			}
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// DirectShow 関連.
		//

		#region プロパティ:

		/// <summary>
		/// 動画ファイル情報
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxMediaNode.Info")]
		public virtual XIE.Tasks.CxMediaInfo Info
		{
			get { return m_Info; }
			set { m_Info = value; }
		}
		private XIE.Tasks.CxMediaInfo m_Info = null;

		/// <summary>
		/// メディアプレイヤー
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxMediaNode.Player")]
		public XIE.Media.CxMediaPlayer Player
		{
			get { return m_Player; }
			set { m_Player = value; }
		}
		private XIE.Media.CxMediaPlayer m_Player = null;

		#endregion

		#region プロパティ: (TimeStamp)

		/// <summary>
		/// ファイル日時
		/// </summary>
		public DateTime FileTime
		{
			get;
			set;
		}

		/// <summary>
		/// 更新日時
		/// </summary>
		public DateTime UpdatedTime
		{
			get;
			set;
		}

		#endregion

		#region オブジェクト.

		/// <summary>
		/// 画像取り込み用バッファ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[Browsable(false)]
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxMediaNode.Images")]
		public virtual XIE.CxImage[] Images
		{
			get { return m_Images; }
		}
		private XIE.CxImage[] m_Images = new XIE.CxImage[30];

		/// <summary>
		/// データオブジェクト
		/// </summary>
		[XmlIgnore]
		public virtual XIE.CxImage Data
		{
			get { return m_Data; }
			set { m_Data = value; }
		}
		private XIE.CxImage m_Data = null;

		/// <summary>
		/// フレームカウンター [初期値:0、範囲:0~]
		/// </summary>
		[XmlIgnore]
		public virtual int FrameCounter
		{
			get { return m_FrameCounter; }
			set { m_FrameCounter = value; }
		}
		private int m_FrameCounter = 0;

		#endregion

		#region IxRunnable の実装:

		/// <summary>
		/// リセット
		/// </summary>
		public virtual void Reset()
		{
			Player.Reset();
		}

		/// <summary>
		/// 開始
		/// </summary>
		public virtual void Start()
		{
			if (this.IsRunning) return;
			FrameCounter = 0;
			Player.Start();
		}

		/// <summary>
		/// 停止
		/// </summary>
		public virtual void Stop()
		{
			Player.Stop();
			Player.SetStartPosition(0);
		}

		/// <summary>
		/// 一時停止
		/// </summary>
		public virtual void Pause()
		{
			if (Player.IsPaused)
				Player.Start();
			else
				Player.Pause();
		}

		/// <summary>
		/// 待機
		/// </summary>
		/// <param name="timeout">タイムアウト (msec) [-1,0~]</param>
		/// <returns>
		///		停止を検知すると true を返します。
		///		タイムアウトすると false を返します。
		/// </returns>
		public bool Wait(int timeout)
		{
			return Player.Wait(timeout);
		}

		/// <summary>
		/// 動作状態
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		public virtual bool IsRunning
		{
			get { return Player.IsRunning; }
		}

		#endregion

		#region IxParam の実装:

		/// <summary>
		/// パラメータの取得
		/// </summary>
		/// <param name="name">パラメータ名称</param>
		/// <returns>
		///		取得した値を返します。
		///	</returns>
		public object GetParam(string name)
		{
			return Player.GetParam(name);
		}

		/// <summary>
		/// パラメータの設定
		/// </summary>
		/// <param name="name">パラメータ名称</param>
		/// <param name="value">設定値</param>
		public void SetParam(string name, object value)
		{
			Player.SetParam(name, value);
		}

		#endregion

		// 
		// UI 関連
		// 

		#region 選択ダイアログ:

		/// <summary>
		/// 選択ダイアログの表示
		/// </summary>
		/// <param name="owner">ダイアログのオーナーフォーム</param>
		/// <param name="aux_info">外部機器情報</param>
		/// <param name="options">追加の引数 (通常は指定不要です。)</param>
		/// <returns>
		///		OK または Cancel を返します。
		/// </returns>
		public virtual DialogResult OpenSelectDialog(Form owner, XIE.Tasks.CxAuxInfo aux_info, params object[] options)
		{
			DialogResult result = DialogResult.Cancel;

			return result;
		}

		#endregion

		#region プロパティダイアログ:

		/// <summary>
		/// プロパティダイアログ
		/// </summary>
		public virtual Form PropertyDialog
		{
			get { return m_PropertyDialog; }
			set { m_PropertyDialog = value; }
		}
		private Form m_PropertyDialog = null;

		/// <summary>
		/// プロパティダイアログの生成
		/// </summary>
		/// <param name="owner">ダイアログのオーナーフォーム</param>
		/// <param name="options">追加の引数 (通常は指定不要です。)</param>
		/// <returns>
		///		生成されたフォームを返します。
		/// </returns>
		public virtual Form OpenPropertyDialog(Form owner, params object[] options)
		{
			if (Player == null)
				throw new System.InvalidOperationException("Player is null");

			if (PropertyDialog == null)
			{
				PropertyDialog = new CxMediaPropertyForm(Player);
				if (PropertyDialog == null)
				{
					MessageBox.Show(owner, XIE.AxiTextStorage.GetValue("F:XIE.ExStatus.Unsupported"), "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return null;
				}

				PropertyDialog.Owner = owner;
				PropertyDialog.StartPosition = FormStartPosition.Manual;
				PropertyDialog.Location = ApiHelper.GetNeighborPosition(PropertyDialog.Size);
				PropertyDialog.FormClosed += PropertyDialog_FormClosed;
				PropertyDialog.Show();
			}
			else if (PropertyDialog.WindowState == FormWindowState.Minimized)
			{
				PropertyDialog.WindowState = FormWindowState.Normal;
			}

			return PropertyDialog;
		}

		/// <summary>
		/// プロパティダイアログが閉じたとき
		/// </summary>
		protected virtual void PropertyDialog_FormClosed(object sender, EventArgs e)
		{
			PropertyDialog = null;
		}

		#endregion
	}
}
