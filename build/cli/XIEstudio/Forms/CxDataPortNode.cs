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
	/// AUX ツリーノード (データ入出力ポート)
	/// </summary>
	class CxDataPortNode : TreeNode
		, IDisposable
		, XIE.Tasks.IxAuxNode
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
		public CxDataPortNode()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		public CxDataPortNode(XIE.Tasks.CxDataPortInfo info, XIE.Tasks.CxDataPortThread controller)
		{
			InitializeComponent();
			this.Info = info;
			this.Controller = controller;
		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxDataPortNode()
		{
			Dispose();
		}

		#endregion

		#region 接続と切断:

		/// <summary>
		/// イメージグラバーの接続.(スレッドとの関連付け)
		/// </summary>
		/// <param name="config">外部機器情報</param>
		/// <returns>
		///		生成したノードを配列に格納して返します。
		///		生成に失敗した要素は null が格納されています。
		/// </returns>
		public static CxDataPortNode[] Connect(XIE.Tasks.CxAuxInfo config)
		{
			var aux = (XIE.Tasks.IxAuxInfoDataPorts)config;
			int count = aux.Infos.Length;
			var nodes = new CxDataPortNode[count];

			#region 初期化.
			for (int i = 0; i < count; i++)
			{
				try
				{
					var node = new CxDataPortNode(aux.Infos[i], aux.Controllers[i]);
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
				var aux = (XIE.Tasks.IxAuxInfoDataPorts)config;
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
						name = this.Controller.GetName();
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

			#region データ入力ポート:
			try
			{
				if (this.Controller is XIE.Tasks.IxDataPortIn)
				{
					var controller = (XIE.Tasks.IxDataPortIn)this.Controller;
					var length = controller.Length;
					var model = XIE.ModelOf.From(controller.DataType);
					if (model == XIE.TxModel.Default)
						XIE.Log.Api.Error("{0}: {1} is not supported.", this.Name, controller.DataType);
					else
						this.DataPortIn.Resize(length, model);
				}
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error("{0}: {1}", this.Name, ex.Message);
			}
			#endregion

			#region データ出力ポート:
			try
			{
				if (this.Controller is XIE.Tasks.IxDataPortOut)
				{
					var controller = (XIE.Tasks.IxDataPortOut)this.Controller;
					var length = controller.Length;
					var model = XIE.ModelOf.From(controller.DataType);
					if (model == XIE.TxModel.Default)
						XIE.Log.Api.Error("{0}: {1} is not supported.", this.Name, controller.DataType);
					else
						this.DataPortOut.Resize(length, model);
				}
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error("{0}: {1}", this.Name, ex.Message);
			}
			#endregion

			this.Tick(this, EventArgs.Empty);
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
			if (PropertyDialog != null)
				PropertyDialog.Close();
			PropertyDialog = null;
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
			this.OpenPropertyDialog(sender as Form);
		}

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">イベント引数</param>
		public virtual void Tick(object sender, EventArgs e)
		{
			#region アイコンの更新.
			string key = "IO-Connect";

			if (this.Controller == null)
				key = "IO-Disconnect";
			else if (this.Controller.IsRunning == false)
				key = "IO-Stop";

			if (this.ImageKey != key)
				this.ImageKey = key;
			if (this.SelectedImageKey != key)
				this.SelectedImageKey = key;
			#endregion
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// デバイス情報
		/// </summary>
		public virtual XIE.Tasks.CxDataPortInfo Info
		{
			get { return m_Info; }
			set { m_Info = value; }
		}
		private XIE.Tasks.CxDataPortInfo m_Info = new XIE.Tasks.CxDataPortInfo();

		/// <summary>
		/// コントローラ
		/// </summary>
		public virtual XIE.Tasks.CxDataPortThread Controller
		{
			get { return m_Controller; }
			set { m_Controller = value; }
		}
		private XIE.Tasks.CxDataPortThread m_Controller = null;

		#endregion

		#region イベントハンドラ:

		/// <summary>
		/// 通知イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void DataPort_Notify(object sender, XIE.Tasks.CxDataPortEventArgs e)
		{
			#region データ入力ポート:
			try
			{
				if (e is XIE.Tasks.IxDataPortIn)
				{
					lock (this.DataPortIn)
					{
						var buf = this.DataPortIn;
						var args = (XIE.Tasks.IxDataPortIn)e;
						if (buf.Model == XIE.ModelOf.From(args.DataType))
						{
							if (0 <= e.Index && 0 < e.Count && (e.Index + e.Count) <= buf.Length)
							{
								#region U16 (UInt16)
								if (buf.Model == XIE.TxModel.U16(1))
								{
									var addr = (XIE.Ptr.UInt16Ptr)buf.Address();
									var port = (XIE.Tasks.IxDataPortIn<UInt16>)e;
									for (int i = e.Index; i < e.Count; i++)
									{
										addr[i] = port.GetData(i);
									}
								}
								#endregion

								#region U32 (UInt32)
								if (buf.Model == XIE.TxModel.U32(1))
								{
									var addr = (XIE.Ptr.UInt32Ptr)buf.Address();
									var port = (XIE.Tasks.IxDataPortIn<UInt32>)e;
									for (int i = e.Index; i < e.Count; i++)
									{
										addr[i] = port.GetData(i);
									}
								}
								#endregion
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error("{0}: {1}", this.Name, ex.Message);
			}
			#endregion

			#region データ出力ポート:
			try
			{
				if (e is XIE.Tasks.IxDataPortOut)
				{
					lock (this.DataPortOut)
					{
						var buf = this.DataPortOut;
						var args = (XIE.Tasks.IxDataPortOut)e;
						if (buf.Model == XIE.ModelOf.From(args.DataType))
						{
							if (0 <= e.Index && 0 < e.Count && (e.Index + e.Count) <= buf.Length)
							{
								#region U16 (UInt16)
								if (buf.Model == XIE.TxModel.U16(1))
								{
									var addr = (XIE.Ptr.UInt16Ptr)buf.Address();
									var port = (XIE.Tasks.IxDataPortOut<UInt16>)e;
									for (int i = e.Index; i < e.Count; i++)
									{
										addr[i] = port.GetData(i);
									}
								}
								#endregion

								#region U32 (UInt32)
								if (buf.Model == XIE.TxModel.U32(1))
								{
									var addr = (XIE.Ptr.UInt32Ptr)buf.Address();
									var port = (XIE.Tasks.IxDataPortOut<UInt32>)e;
									for (int i = e.Index; i < e.Count; i++)
									{
										addr[i] = port.GetData(i);
									}
								}
								#endregion
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error("{0}: {1}", this.Name, ex.Message);
			}
			#endregion

			this.TimeStampRecv = DateTime.Now;
		}

		/// <summary>
		/// データ入力ポートバッファ
		/// </summary>
		private XIE.CxArray DataPortIn = new XIE.CxArray();

		/// <summary>
		/// データ出力ポートバッファ
		/// </summary>
		private XIE.CxArray DataPortOut = new XIE.CxArray();

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
		}

		/// <summary>
		/// 中断
		/// </summary>
		public virtual void Abort()
		{
			this.Controller.Stop();
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
				Form form = new CxDataPortSelectionForm(this.Info, typeof(XIE.Tasks.CxDataPortThread));
				form.StartPosition = FormStartPosition.Manual;
				form.Location = ApiHelper.GetNeighborPosition(form.Size);
				result = form.ShowDialog(owner);
				if (result == DialogResult.OK)
				{
					var aux = (XIE.Tasks.IxAuxInfoDataPorts)aux_info;

					this.Dispose();

					// 生成.
					var controller = (XIE.Tasks.CxDataPortThread)this.Info.CreateInstance();
					controller.Setup();

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

			if (PropertyDialog == null)
			{
				PropertyDialog = new CxDataPortPropertyForm(Controller);
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
