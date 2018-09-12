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
	/// AUX ツリーノード (TCP/IP サーバー)
	/// </summary>
	class CxTcpServerNode : TreeNode
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
		public CxTcpServerNode()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		public CxTcpServerNode(XIE.Tasks.CxTcpServerInfo info, XIE.Net.CxTcpServer controller)
		{
			InitializeComponent();
			this.Info = info;
			this.Controller = controller;
		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxTcpServerNode()
		{
			Dispose();
		}

		#endregion

		#region 接続と切断:

		/// <summary>
		/// TCP/IP Server: デバイスの接続.(スレッドとの関連付け)
		/// </summary>
		/// <param name="config">外部機器情報</param>
		/// <returns>
		///		生成したノードを配列に格納して返します。
		///		生成に失敗した要素は null が格納されています。
		/// </returns>
		public static CxTcpServerNode[] Connect(XIE.Tasks.CxAuxInfo config)
		{
			XIE.Tasks.IxAuxInfoTcpServers aux = (XIE.Tasks.IxAuxInfoTcpServers)config;
			int count = aux.Infos.Length;
			CxTcpServerNode[] nodes = new CxTcpServerNode[count];

			#region 初期化.
			for (int i = 0; i < count; i++)
			{
				try
				{
					CxTcpServerNode node = new CxTcpServerNode(aux.Infos[i], aux.Controllers[i]);
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
				XIE.Tasks.IxAuxInfoTcpServers aux = (XIE.Tasks.IxAuxInfoTcpServers)config;
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
				string name = string.Format("{0}:{1}", this.Info.Hostname, this.Info.Port);
				this.Name = name;
				this.Text = name;
				this.ToolTipText = name;
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
			string key = "Net-Connect";

			if (this.Controller == null)
				key = "Net-Disconnect";
			else if (this.Controller.IsRunning == false)
				key = "Net-Stop";

			if (this.ImageKey != key)
				this.ImageKey = key;
			if (this.SelectedImageKey != key)
				this.SelectedImageKey = key;
			#endregion
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// TCP/IP サーバー情報
		/// </summary>
		public virtual XIE.Tasks.CxTcpServerInfo Info
		{
			get { return m_Info; }
			set { m_Info = value; }
		}
		private XIE.Tasks.CxTcpServerInfo m_Info = new XIE.Tasks.CxTcpServerInfo();

		/// <summary>
		/// スレッド
		/// </summary>
		public virtual XIE.Net.CxTcpServer Controller
		{
			get { return m_Thread; }
			set { m_Thread = value; }
		}
		private XIE.Net.CxTcpServer m_Thread = null;

		#endregion

		#region プロパティ:(送受信スレッド)

		/// <summary>
		/// 送信用バッファサイズ (byte)
		/// </summary>
		[Category("Parameter")]
		[XIE.CxDescription("P:XIEstudio.CxTcpServerNode.SendBufferSize")]
		public virtual int SendBufferSize
		{
			get { return m_SendBufferSize; }
			set { m_SendBufferSize = value; }
		}
		private int m_SendBufferSize = 32000;

		/// <summary>
		/// 送信用タイムアウト (ms)
		/// </summary>
		[Category("Parameter")]
		[XIE.CxDescription("P:XIEstudio.CxTcpServerNode.SendTimeout")]
		public virtual int SendTimeout
		{
			get { return m_SendTimeout; }
			set { m_SendTimeout = value; }
		}
		private int m_SendTimeout = 0;

		/// <summary>
		/// 受信用バッファサイズ (byte)
		/// </summary>
		[Category("Parameter")]
		[XIE.CxDescription("P:XIEstudio.CxTcpServerNode.ReceiveBufferSize")]
		public virtual int ReceiveBufferSize
		{
			get { return m_ReceiveBufferSize; }
			set { m_ReceiveBufferSize = value; }
		}
		private int m_ReceiveBufferSize = 32000;

		/// <summary>
		/// 受信用タイムアウト (ms)
		/// </summary>
		[Category("Parameter")]
		[XIE.CxDescription("P:XIEstudio.CxTcpServerNode.ReceiveTimeout")]
		public virtual int ReceiveTimeout
		{
			get { return m_ReceiveTimeout; }
			set { m_ReceiveTimeout = value; }
		}
		private int m_ReceiveTimeout = 0;

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

			#region デバイスオープン.
			var info = new XIE.Tasks.CxTcpServerInfo();
			info.CopyFrom(this.Info);

			Form form = new CxTcpServerSelectForm(info);
			form.StartPosition = FormStartPosition.Manual;
			form.Location = ApiHelper.GetNeighborPosition(form.Size);
			result = form.ShowDialog(owner);
			if (result == DialogResult.OK)
			{
				var aux = (XIE.Tasks.IxAuxInfoTcpServers)aux_info;
				int index = aux.Find(this.Info);

				if (index < 0)
				{
					this.Dispose();

					// 生成.
					XIE.Net.CxTcpServer controller = info.CreateController();
					controller.Setup();
					controller.Start();
					aux.Add(info, controller);

					this.Info = info;
					this.Controller = controller;
					this.Setup();
				}
				else
				{
					this.Dispose();
					if (aux.Controllers[index] != null)
						aux.Controllers[index].Dispose();

					// 生成.
					XIE.Net.CxTcpServer controller = info.CreateController();
					controller.Setup();
					controller.Start();
					aux.Infos[index] = info;
					aux.Controllers[index] = controller;

					this.Info = info;
					this.Controller = controller;
					this.Setup();
				}

				#region スレッド起動.
				try
				{
					if (this.Controller != null && this.Controller.IsRunning == false)
					{
						this.Controller.Start();
					}
				}
				catch (System.Exception ex)
				{
					Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
				}
				#endregion
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
				throw new System.InvalidOperationException("TcpServerThread is null");

			if (PropertyDialog == null)
			{
				PropertyDialog = new CxTcpServerPropertyForm(this);
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
