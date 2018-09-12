/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.CodeDom;
using System.Globalization;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using System.Xml.Serialization;

namespace XIEstudio
{
	/// <summary>
	/// AUX ツリーノード (カメラデバイス)
	/// </summary>
	class CxCameraNode : TreeNode
		, IDisposable
		, XIE.Tasks.IxAuxNode
		, XIE.Tasks.IxAuxNodeImageOut
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ補助関数
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxCameraNode()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		public CxCameraNode(XIE.Media.CxDeviceParam info, XIE.Media.CxCamera controller)
		{
			InitializeComponent();
			this.Info = info;
			this.Controller = controller;
		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxCameraNode()
		{
			Dispose();
		}

		#endregion

		#region 接続と切断:

		/// <summary>
		/// カメラデバイス: カメラの接続.(スレッドとの関連付け)
		/// </summary>
		/// <param name="config">外部機器情報</param>
		/// <returns>
		///		生成したノードを配列に格納して返します。
		///		生成に失敗した要素は null が格納されています。
		/// </returns>
		public static CxCameraNode[] Connect(XIE.Tasks.CxAuxInfo config)
		{
			var aux = (XIE.Tasks.IxAuxInfoCameras)config;
			int count = aux.Infos.Length;
			var nodes = new CxCameraNode[count];

			#region 初期化.
			for (int i = 0; i < count; i++)
			{
				try
				{
					var node = new CxCameraNode(aux.Infos[i], aux.Controllers[i]);
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
				var aux = (XIE.Tasks.IxAuxInfoCameras)config;
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

			#region 表示名:
			{
				string name = "Unknown";
				if (this.Controller != null)
				{
					try
					{
						var product = (string)this.Controller.GetParam("ProductName");
						var size = this.Controller.GetFrameSize();
						name = string.Format("{0} {1}x{2}", product, size.Width, size.Height);
					}
					catch (System.Exception)
					{
						// ignore
					}
				}
				this.Name = name;
				this.Text = name;
			}
			#endregion

			for (int i = 0; i < Images.Length; i++)
			{
				if (Images[i] == null)
					Images[i] = new XIE.CxImage();
			}

			this.Tick(this, EventArgs.Empty);

			if (this.Controller != null)
			{
				this.VideoGrabber = this.Controller.CreateGrabber();
				this.VideoGrabber.Notify += Grabber_Notify;
				this.VideoGrabber.Start();
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
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			this.Image = null;
			if (PropertyDialog != null)
				PropertyDialog.Close();
			PropertyDialog = null;
			if (VideoGrabber != null)
				VideoGrabber.Dispose();
			VideoGrabber = null;
			//foreach (var item in Images)
			//{
			//	if (item != null)
			//		item.Dispose();
			//}
			Controller = null;
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
			if (this.IsRunning)
				this.Stop();
			else
				this.Start();
		}

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">イベント引数</param>
		public virtual void Tick(object sender, EventArgs e)
		{
			#region アイコンの更新.
			string key = "Camera-Connect";

			if (this.Controller == null)
				key = "Camera-Disconnect";
			else if (this.Controller.IsRunning == false)
				key = "Camera-Stop";
			else if (IsRecording)
				key = "Camera-Record";

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
			if (view.Image != this.Image || this.TimeStampDraw < this.TimeStampRecv)
			{
				this.TimeStampDraw = DateTime.Now;
				view.Image = this.Image;
				view.Refresh();
			}
		}

		/// <summary>
		/// 描画イメージの解除
		/// </summary>
		/// <param name="view">描画先</param>
		public virtual void Reset(XIE.GDI.CxImageView view)
		{
			if (ReferenceEquals(view.Image, this.Image))
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

		#region プロパティ:

		/// <summary>
		/// デバイス情報
		/// </summary>
		public virtual XIE.Media.CxDeviceParam Info
		{
			get { return m_Info; }
			set { m_Info = value; }
		}
		private XIE.Media.CxDeviceParam m_Info = new XIE.Media.CxDeviceParam();

		/// <summary>
		/// コントローラ
		/// </summary>
		public virtual XIE.Media.CxCamera Controller
		{
			get { return m_Controller; }
			set { m_Controller = value; }
		}
		private XIE.Media.CxCamera m_Controller = null;

		/// <summary>
		/// 映像捕獲用イベントレシーバ
		/// </summary>
		private XIE.Media.CxGrabber VideoGrabber = null;

		#endregion

		#region イベントハンドラ:

		/// <summary>
		/// 通知イベント (取り込み完了)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void Grabber_Notify(object sender, XIE.Media.CxGrabberArgs e)
		{
			this.BufferIndex = e.Index % this.Images.Length;
			var image = this.Images[this.BufferIndex];
			e.CopyTo(image);

			this.Image = image;
			this.TimeStampRecv = DateTime.Now;

			#region フレームレート計算.
			{
				var dt = XIE.TxDateTime.FromBinary(e.TimeStamp, true);
				this.TimeStamps.Add(dt);
				int count = this.TimeStamps.Count;
				if (count > 30)
				{
					double sum = 0.0;
					for (int i = 1; i < count; i++)
					{
						TimeSpan diff = this.TimeStamps[i].Subtract(this.TimeStamps[i - 1]);
						sum += diff.TotalSeconds;
					}
					if (sum != 0)
						this.FrameRate = count / sum;

					this.TimeStamps.Clear();
				}
			}
			#endregion
		}

		/// <summary>
		/// 画像取り込み用バッファ指標
		/// </summary>
		private int BufferIndex = 0;

		/// <summary>
		/// 画像取り込み用バッファ
		/// </summary>
		private XIE.CxImage[] Images = new XIE.CxImage[5];

		/// <summary>
		/// 画像描画用
		/// </summary>
		private XIE.CxImage Image = null;

		/// <summary>
		/// タイムスタンプのコレクション
		/// </summary>
		private List<DateTime> TimeStamps = new List<DateTime>();

		/// <summary>
		/// タイムスタンプ (受信時)
		/// </summary>
		private DateTime TimeStampRecv = DateTime.Now;

		/// <summary>
		/// タイムスタンプ (描画時)
		/// </summary>
		private DateTime TimeStampDraw = DateTime.Now;

		#endregion

		#region スレッド: (開始/停止)

		/// <summary>
		/// 録画準備
		/// </summary>
		/// <param name="enable"></param>
		public virtual void SetupRecording(bool enable)
		{
			IsRecording = enable;

			if (this.Controller is XIE.Media.CxCamera)
			{
				var controller = (XIE.Media.CxCamera)this.Controller;

				#region カメラ停止待機:
				for (int i = 0; i < 50; i++)
				{
					if (controller.IsRunning == false) break;
					System.Threading.Thread.Sleep(100);
				}
				#endregion

				#region 初期化:
				if (enable)
				{
					// 音声入力デバイス.
					var audioInput = new XIE.Media.CxDeviceParam();
					var audioInputs = (XIE.Tasks.IxAuxInfoAudioInputs)this.AuxInfo;
					if (audioInputs.Infos.Length != 0)
						audioInput.CopyFrom(audioInputs.Infos[0]);

					// 出力先.
					var now = DateTime.Now;
					string nickname = ApiHelper.MakeValidFileName(this.Name, "-");
					string suffix = ApiHelper.MakeFileNameSuffix(now, false);
					string extension;
					switch (CxAuxInfoForm.AppSettings.VideoFileType)
					{
						default: extension = "wmv";  break;
						case XIE.Tasks.ExVideoFileType.Avi: extension = "avi"; break;
						case XIE.Tasks.ExVideoFileType.Asf: extension = "asf"; break;
						case XIE.Tasks.ExVideoFileType.Wmv: extension = "wmv"; break;
					}
					if (!audioInput.IsValid)
						extension = "avi";
					string filename = string.Format("{0}-{1}.{2}", nickname, suffix, extension);
					string filepath = System.IO.Path.Combine(XIE.Tasks.SharedData.ProjectDir, filename);
					var output_file = new XIE.CxStringW(filepath);

					// 初期化.
					controller.Setup(this.Info, audioInput, output_file);
				}
				else
				{
					controller.Setup(this.Info, null, null);
				}
				#endregion
			}
		}

		/// <summary>
		/// 開始
		/// </summary>
		public virtual void Start()
		{
			this.Controller.Start();
		}

		/// <summary>
		/// 停止
		/// </summary>
		public virtual void Stop()
		{
			this.Controller.Stop();

			if (IsRecording)
				SetupRecording(false);
		}

		/// <summary>
		/// 中断
		/// </summary>
		public virtual void Abort()
		{
			this.Controller.Abort();
		}

		/// <summary>
		/// 動作状態
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		public virtual bool IsRunning
		{
			get { return this.Controller.IsRunning; }
		}

		/// <summary>
		/// 録画指示
		/// </summary>
		public virtual bool IsRecording
		{
			get { return m_IsRecording; }
			set { m_IsRecording = value; }
		}
		private bool m_IsRecording = false;

		/// <summary>
		/// フレームレート (fps)
		/// </summary>
		public virtual double FrameRate
		{
			get { return m_FrameRate; }
			set { m_FrameRate = value; }
		}
		private double m_FrameRate = 0.0;

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

			#region デバイスのオープン.
			{
				Form form = new XIE.Media.CxCameraSelectionForm(this.Info);
				form.StartPosition = FormStartPosition.Manual;
				form.Location = ApiHelper.GetNeighborPosition(form.Size);
				result = form.ShowDialog(owner);
				if (result == DialogResult.OK)
				{
					var aux = (XIE.Tasks.IxAuxInfoCameras)aux_info;

					this.Dispose();

					// 生成.
					var controller = new XIE.Media.CxCamera();
					controller.Setup(this.Info, null, null);
					this.VideoGrabber = controller.CreateGrabber();
					this.VideoGrabber.Notify += Grabber_Notify;
					this.VideoGrabber.Start();

					int index = aux.Find(this.Info);
					if (index < 0)
					{
						aux.Add(this.Info, controller);
					}
					else
					{
						aux.Controllers[index] = controller;
					}

					this.Controller = controller;
					this.Setup();
				}
			}
			#endregion

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
			if (Controller == null)
				throw new System.InvalidOperationException("Controller is null");

			if (options != null && options.Length > 0)
			{
				#region PropertyPage
				if (options[0] is XIE.Media.ExMediaType)
				{
					try
					{
						var type = (XIE.Media.ExMediaType)options[0];
						this.Controller.OpenPropertyDialog(owner, type);
						return null;
					}
					catch (System.Exception ex)
					{
						MessageBox.Show(owner, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return null;
					}
				}

				MessageBox.Show(owner, XIE.AxiTextStorage.GetValue("F:XIE.ExStatus.Unsupported"), "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return null;
				#endregion
			}
			else if (PropertyDialog == null)
			{
				PropertyDialog = new CxCameraPropertyForm(Controller);
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
