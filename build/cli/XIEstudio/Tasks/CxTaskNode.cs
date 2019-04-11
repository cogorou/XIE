/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using XIE;

namespace XIEstudio
{
	/// <summary>
	/// AUX ツリーノード (タスクフロー)
	/// </summary>
	public class CxTaskNode : TreeNode
		, IDisposable
		, ICloneable
		, XIE.Tasks.IxAuxNode
		, XIE.Tasks.IxAuxNodeImageOut
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskNode()
		{
			this.Info = new XIE.Tasks.CxTaskUnitInfo();
			this.Taskflow = null;
			this.FileTime = DateTime.Now;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="info">タスクユニット情報</param>
		/// <param name="taskflow">タスクフロー</param>
		public CxTaskNode(XIE.Tasks.CxTaskUnitInfo info, XIE.Tasks.CxTaskflow taskflow)
		{
			this.Info = info;
			this.Taskflow = taskflow;
			this.FileTime = DateTime.Now;
		}

		#endregion

		#region 接続と切断:

		/// <summary>
		/// タスクフロー: タスクユニットの接続.
		/// </summary>
		/// <param name="config">外部機器情報</param>
		/// <returns>
		///		生成したノードを配列に格納して返します。
		///		生成に失敗した要素は null が格納されています。
		/// </returns>
		public static CxTaskNode[] Connect(XIE.Tasks.CxAuxInfo config)
		{
			var aux = (XIE.Tasks.IxAuxInfoTasks)config;
			int count = aux.Infos.Length;
			var nodes = new CxTaskNode[count];

			#region 初期化.
			for (int i = 0; i < count; i++)
			{
				try
				{
					var info = aux.Infos[i];
					var task = aux.Tasks[i] as XIE.Tasks.CxTaskflow;
					var node = new CxTaskNode(info, task);
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
				var aux = (XIE.Tasks.IxAuxInfoTasks)config;
				aux.Remove(this.Info);
			}
		}

		#endregion

		#region 初期化:

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Setup()
		{
			if (this.IsSetuped == false)
			{
				#region ファイル情報:
				string filename = this.Info.FileName;
				if (string.IsNullOrEmpty(filename) == false &&
					System.IO.File.Exists(filename))
				{
					// ファイルに関連する項目の更新.
					this.FileTime = System.IO.File.GetLastWriteTime(filename);
					string name = System.IO.Path.GetFileNameWithoutExtension(filename);
					if (this.Taskflow != null)
						this.Taskflow.Name = name;
					this.Document.DocumentName = name;
				}
				#endregion

				// 印刷設定の既定値.
				this.Document.DefaultPageSettings.Landscape = true;

				// 依存関係の復元:
				this.LoadDependency();

				#region タスクの初期化:
				if (this.Taskflow != null)
				{
					var args = new XIE.Tasks.CxTaskSetupEventArgs(
						this.Taskflow,
						this.AuxInfo,
						this.GetBaseDir(),
						this.Name
						);

					foreach (var task in this.Taskflow.TaskUnits)
					{
						try
						{
							task.Setup(this, args, this.Taskflow);
						}
						catch (System.Exception ex)
						{
							XIE.Log.Api.Error("{0}: {1}.Setup: {2}", task.Name, task.GetType().FullName, ex.Message);
						}
					}
				}
				#endregion
			}

			#region 表示名.
			if (this.Taskflow != null)
			{
				this.Name = this.Taskflow.Name;
				this.Text = this.Taskflow.Name;
				this.ImageKey = this.Taskflow.IconKey;
				this.SelectedImageKey = this.Taskflow.IconKey;

				// 描画属性の復元.
				this.Visualizer.SheetType = this.Taskflow.SheetType;
				this.Visualizer.BlockType = this.Taskflow.BlockType;
				this.Visualizer.ColorType = this.Taskflow.ColorType;
				this.Visualizer.LineType = this.Taskflow.LineType;
				this.Visualizer.Setup();
			}
			else
			{
				var name = this.Info.FileName;
				if (string.IsNullOrEmpty(name) == false)
					name = System.IO.Path.GetFileNameWithoutExtension(name);
				this.Name = name;
				this.Text = name;
			}
			#endregion

			this.IsSetuped = true;
		}
		private bool IsSetuped = false;

		#endregion

		#region 初期化: (依存関係)

		/// <summary>
		/// 依存関係の復元
		/// </summary>
		protected void LoadDependency()
		{
			if (this.Taskflow != null)
			{
				// 依存関係の復元:
				var taskflow = (XIE.Tasks.CxTaskflow)this.Taskflow;
				var task_list = taskflow.GetTasks(true);

				// 描画属性の復元:
				this.Visualizer.SheetType = taskflow.SheetType;
				this.Visualizer.BlockType = taskflow.BlockType;
				this.Visualizer.ColorType = taskflow.ColorType;
				this.Visualizer.LineType = taskflow.LineType;
				this.Visualizer.Setup();

				// アイコンの初期化.
				var icons = (XIE.Tasks.IxAuxImageList16)CxAuxInfoForm.AuxInfo;
				try
				{
					taskflow.IconImage = icons.ImageList.Images[taskflow.IconKey];
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.StackTrace);
				}
				foreach (var task in task_list)
				{
					try
					{
						task.IconImage = icons.ImageList.Images[task.IconKey];
					}
					catch (System.Exception ex)
					{
						XIE.Log.Api.Error(ex.StackTrace);
					}
				}

				// タスク参照の復元:
				foreach (var task in task_list)
				{
					if (task is XIE.Tasks.CxTaskReference)
					{
						var task_ref = (XIE.Tasks.CxTaskReference)task;
						if (string.IsNullOrWhiteSpace(task_ref.DependencyTaskID) == false)
						{
							try
							{
								task_ref.DependencyTask = task_list.Find((item) => { return item.UniqueID == task_ref.DependencyTaskID; });
							}
							catch (System.Exception ex)
							{
								XIE.Log.Api.Error(ex.StackTrace);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 依存関係の保存
		/// </summary>
		protected void SaveDependency()
		{
			if (this.Taskflow != null)
			{
				// 依存関係の保存:
				var taskflow = (XIE.Tasks.CxTaskflow)this.Taskflow;
				var task_list = taskflow.GetTasks(true);

				// 描画属性の保存:
				taskflow.SheetType = this.Visualizer.SheetType;
				taskflow.BlockType = this.Visualizer.BlockType;
				taskflow.ColorType = this.Visualizer.ColorType;
				taskflow.LineType = this.Visualizer.LineType;

				// タスクのユニーク ID の付加:
				foreach (var task in task_list)
				{
					if (string.IsNullOrWhiteSpace(task.UniqueID))
					{
						task.UniqueID = GenerateUniqueID();
					}
				}

				// タスク参照の保存:
				foreach (var task in task_list)
				{
					if (task is XIE.Tasks.CxTaskReference)
					{
						var task_ref = (XIE.Tasks.CxTaskReference)task;
						try
						{
							var dependency_task = task_list.Find((item) => { return ReferenceEquals(item, task_ref.DependencyTask); });
							if (dependency_task != null)
								task_ref.DependencyTaskID = dependency_task.UniqueID;
						}
						catch (System.Exception ex)
						{
							XIE.Log.Api.Error(ex.StackTrace);
						}
					}
				}
			}
		}

		/// <summary>
		/// ユニーク IDの生成
		/// </summary>
		/// <returns></returns>
		private string GenerateUniqueID()
		{
			var timestamp = DateTime.Now;
			string suffix = string.Format("{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}_{6:000}_{7:X4}",
				timestamp.Year, timestamp.Month, timestamp.Day,
				timestamp.Hour, timestamp.Minute, timestamp.Second,
				timestamp.Millisecond, SeqNo++);
			return suffix;
		}
		private static ushort SeqNo = 0;

		#endregion

		#region IDisposable の実装:

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			if (this.PropertyDialog != null)
				this.PropertyDialog.Close();
			this.PropertyDialog = null;

			this.Taskflow = null;
			this.FileTime = DateTime.Now;

			IsSetuped = false;
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// クローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public override object Clone()
		{
			Type type = this.GetType();
			Assembly asm = Assembly.GetAssembly(type);
			var clone = (CxTaskNode)asm.CreateInstance(type.FullName);
			return clone;
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
			this.OpenPropertyDialog(null);
		}

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender">呼び出し元</param>
		/// <param name="e">イベント引数</param>
		public virtual void Tick(object sender, EventArgs e)
		{
			#region タスクとの同期.
			if (this.Taskflow != null)
			{
				if (this.Name != this.Taskflow.Name)
					this.Name = this.Taskflow.Name;

				// ファイル更新状態の識別:
				if (this.UnsavedDataExists)
				{
					var text = string.Format("{0} *", this.Taskflow.Name);
					if (this.Text != text)
						this.Text = text;

					var color = Color.Red;
					if (this.ForeColor != color)
						this.ForeColor = color;
				}
				else
				{
					var text = this.Taskflow.Name;
					if (this.Text != text)
						this.Text = text;

					var color = Color.FromKnownColor(KnownColor.WindowText);
					if (this.ForeColor != color)
						this.ForeColor = color;
				}
			}
			#endregion

			#region アイコンの更新.
			string key = "Service-Connect";

			if (this.Taskflow == null)
				key = "Service-Disconnect";
			else if (this.IsRunning == true)
				key = "Service-Run";
			else
			{
				var task = this.Taskflow;
				int ci = task.DataIn.Length + task.DataParam.Length;
				int co = task.DataOut.Length;
				if (ci > 0 && co > 0)
					key = "Service-Port";
				else if (ci > 0)
					key = "Service-PortIn";
				else if (co > 0)
					key = "Service-PortOut";
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
			if (this.Taskflow is XIE.Tasks.IxAuxNodeImageOut)
			{
				var task = (XIE.Tasks.IxAuxNodeImageOut)this.Taskflow;
				task.Draw(view);
			}
		}

		/// <summary>
		/// 描画イメージの解除
		/// </summary>
		/// <param name="view">描画先</param>
		public virtual void Reset(XIE.GDI.CxImageView view)
		{
			if (this.Taskflow is XIE.Tasks.IxAuxNodeImageOut)
			{
				var task = (XIE.Tasks.IxAuxNodeImageOut)this.Taskflow;
				task.Reset(view);
			}
		}

		/// <summary>
		/// 描画処理 (描画中の処理)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			if (this.Taskflow is XIE.Tasks.IxAuxNodeImageOut)
			{
				var task = (XIE.Tasks.IxAuxNodeImageOut)this.Taskflow;
				task.Rendering(sender, e);
			}
		}

		/// <summary>
		/// 描画処理 (描画完了時の処理)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Rendered(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			if (this.Taskflow is XIE.Tasks.IxAuxNodeImageOut)
			{
				var task = (XIE.Tasks.IxAuxNodeImageOut)this.Taskflow;
				task.Rendered(sender, e);
			}
		}

		/// <summary>
		/// 図形操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Handling(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			if (this.Taskflow is XIE.Tasks.IxAuxNodeImageOut)
			{
				var task = (XIE.Tasks.IxAuxNodeImageOut)this.Taskflow;
				task.Handling(sender, e);
			}
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
		/// タスクユニット情報
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxTaskNode.Info")]
		public virtual XIE.Tasks.CxTaskUnitInfo Info
		{
			get { return m_Info; }
			set { m_Info = value; }
		}
		private XIE.Tasks.CxTaskUnitInfo m_Info = null;

		/// <summary>
		/// タスクフロー
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxTaskNode.Taskflow")]
		public XIE.Tasks.CxTaskflow Taskflow
		{
			get { return m_Taskflow; }
			set { m_Taskflow = value; }
		}
		private XIE.Tasks.CxTaskflow m_Taskflow = null;

		/// <summary>
		/// 編集対象のタスクフロー
		/// </summary>
		[ReadOnly(true)]
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxTaskNode.CurrentTaskflow")]
		public XIE.Tasks.CxTaskflow CurrentTaskflow
		{
			get
			{
				if (this.Taskflow == null)
					return null;
				return this.Taskflow.GetCurrentTaskflow();
			}
		}

		/// <summary>
		/// 指定されたタスクの１つ上のタスクフローを検索します。
		/// </summary>
		/// <param name="target"></param>
		/// <returns>
		///		見つかったタスクフローを返します。
		///		見つからなければ null を返します。
		/// </returns>
		public XIE.Tasks.CxTaskflow FindParentTaskflow(XIE.Tasks.CxTaskUnit target)
		{
			return FindParentTaskflow(this.Taskflow, target);
		}

		/// <summary>
		/// 指定されたタスクの１つ上のタスクフローを検索します。
		/// </summary>
		/// <param name="taskflow"></param>
		/// <param name="target"></param>
		/// <returns>
		///		見つかったタスクフローを返します。
		///		見つからなければ null を返します。
		/// </returns>
		private XIE.Tasks.CxTaskflow FindParentTaskflow(XIE.Tasks.CxTaskflow taskflow, XIE.Tasks.CxTaskUnit target)
		{
			foreach (var child in taskflow.TaskUnits)
			{
				if (ReferenceEquals(child, target))
					return taskflow;
				if (child is XIE.Tasks.CxTaskflow)
				{
					var parent = FindParentTaskflow((XIE.Tasks.CxTaskflow)child, target);
					if (parent != null)
						return parent;
				}
			}
			return null;
		}

		#endregion

		#region プロパティ: (状態)

		/// <summary>
		/// 実行状態
		/// </summary>
		[XIE.CxCategory("Status")]
		[XIE.CxDescription("P:XIEstudio.CxTaskNode.IsRunning")]
		public virtual bool IsRunning
		{
			get { return m_IsRunning; }
			set { m_IsRunning = value; }
		}
		private bool m_IsRunning = false;

		#endregion

		#region プロパティ: (TimeStamp)

		/// <summary>
		/// ファイル日時
		/// </summary>
		public DateTime FileTime
		{
			get { return m_FileTime; }
			set
			{
				m_FileTime = value;
				UpdatedTime = value;
			}
		}
		private DateTime m_FileTime = DateTime.Now;

		/// <summary>
		/// 更新日時
		/// </summary>
		public DateTime UpdatedTime
		{
			get { return m_UpdatedTime; }
			set
			{
				m_UpdatedTime = value;
				ExecutedTime = value;
			}
		}
		private DateTime m_UpdatedTime = DateTime.Now;

		/// <summary>
		/// 実行日時
		/// </summary>
		public DateTime ExecutedTime
		{
			get { return m_ExecutedTime; }
			set
			{
				m_ExecutedTime = value;
			}
		}
		private DateTime m_ExecutedTime = DateTime.Now;

		/// <summary>
		/// 未保存のデータの存在の有無を取得します。
		/// </summary>
		public virtual bool UnsavedDataExists
		{
			get
			{
				if (this.Taskflow == null) return false;
				if (string.IsNullOrEmpty(this.Info.FileName) == true) return true;
				if (System.IO.File.Exists(this.Info.FileName) == false) return true;
				DateTime filetime = System.IO.File.GetLastWriteTime(this.Info.FileName);
				return (filetime < this.UpdatedTime);
			}
		}

		#endregion

		#region プロパティ: (描画/印刷関連)

		/// <summary>
		/// タスクユニット描画クラス
		/// </summary>
		[Browsable(false)]
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxTaskNode.Visualizer")]
		public CxTaskVisualizer Visualizer
		{
			get { return m_Visualizer; }
			private set { m_Visualizer = value; }
		}
		private CxTaskVisualizer m_Visualizer = new CxTaskVisualizer();

		/// <summary>
		/// 印刷ドキュメント
		/// </summary>
		[Browsable(false)]
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxTaskNode.Document")]
		public PrintDocument Document
		{
			get { return m_Document; }
			set { m_Document = value; }
		}
		private PrintDocument m_Document = new PrintDocument();

		#endregion

		#region プロパティ: (タスクフロー編集フォーム関連)

		/// <summary>
		/// 左辺パネルの可視属性
		/// </summary>
		public bool IsVisibleLeftPanel
		{
			get { return m_IsVisibleLeftPanel; }
			set { m_IsVisibleLeftPanel = value; }
		}
		private bool m_IsVisibleLeftPanel = true;

		/// <summary>
		/// 下辺パネルの可視属性
		/// </summary>
		public bool IsVisibleBottomPanel
		{
			get { return m_IsVisibleBottomPanel; }
			set { m_IsVisibleBottomPanel = value; }
		}
		private bool m_IsVisibleBottomPanel = true;

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
			if (PropertyDialog == null)
			{
				if (this.Taskflow == null)
				{
					MessageBox.Show(Form.ActiveForm, "Taskflow is null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return null;
				}

				PropertyDialog = new CxTaskflowForm(this);
				PropertyDialog.Owner = owner;
				PropertyDialog.StartPosition = FormStartPosition.Manual;
				PropertyDialog.Location = ApiHelper.GetNeighborPosition(PropertyDialog.Size);
				PropertyDialog.FormClosed += delegate(object _sender, FormClosedEventArgs _e)
					{
						PropertyDialog = null;
					};
				PropertyDialog.Show();
			}
			else if (PropertyDialog.WindowState == FormWindowState.Minimized)
			{
				PropertyDialog.WindowState = FormWindowState.Normal;
			}
			else
			{
				PropertyDialog.Focus();
			}
			return PropertyDialog;
		}

		#endregion

		#region ファイルアクセス:

		/// <summary>
		/// ソースファイルの読み込み
		/// </summary>
		public virtual void Load()
		{
			string filename = this.Info.FileName;

			#region ファイル読み込み:
			if (string.IsNullOrEmpty(filename) == false &&
				System.IO.File.Exists(filename))
			{
				// ファイルの復元.
				this.Taskflow = (XIE.Tasks.CxTaskflow)this.Info.CreateTaskUnit();

				// ファイルに関連する項目の更新.
				this.FileTime = System.IO.File.GetLastWriteTime(filename);
				this.UpdatedTime = this.FileTime;
				this.ExecutedTime = this.FileTime;

				var name = System.IO.Path.GetFileNameWithoutExtension(filename);
				this.Taskflow.Name = name;
				this.Name = name;
				this.Text = name;
				this.Document.DocumentName = name;

				// 依存関係の復元:
				this.LoadDependency();
			}
			#endregion
		}

		/// <summary>
		/// ソースファイルへの保存
		/// </summary>
		public virtual void Save()
		{
			string filename = this.Info.FileName;

			#region ファイル保存:
			if (string.IsNullOrEmpty(filename) == false)
			{
				// ファイルに関連する項目の更新.
				var name = System.IO.Path.GetFileNameWithoutExtension(filename);
				this.Taskflow.Name = name;
				this.Name = name;
				this.Text = name;
				this.Document.DocumentName = this.Name;

				// 依存関係の保存:
				this.SaveDependency();

				// ファイルへの保存.
				XIE.Axi.WriteAsBinary(filename, this.Taskflow);

				// ファイルに関連する項目の更新.
				this.FileTime = System.IO.File.GetLastWriteTime(filename);
				this.UpdatedTime = this.FileTime;
			}
			#endregion
		}

		/// <summary>
		/// ダイアログを表示して保存処理を行います。
		/// </summary>
		/// <param name="owner">オーナーフォーム</param>
		/// <returns>
		///		ユーザーの応答を返します。
		/// </returns>
		public DialogResult OpenSaveDialog(Form owner)
		{
			var node = this;

			#region SaveFileDialog
			var dlg = new SaveFileDialog();
			{
				if (string.IsNullOrEmpty(node.Info.FileName))
				{
					var name = ApiHelper.MakeValidFileName(node.Name, "_");
					node.Info.FileName = string.Format("{0}.xtf", name);
				}

				dlg.Filter = "Taskflow files |*.xtf";
				dlg.OverwritePrompt = true;
				dlg.AddExtension = true;
				dlg.FileName = System.IO.Path.GetFileName(node.Info.FileName);

				string dir = System.IO.Path.GetDirectoryName(node.Info.FileName);
				if (string.IsNullOrWhiteSpace(dir) == false)
					dlg.InitialDirectory = dir;
				else if (string.IsNullOrWhiteSpace(CxAuxInfoForm.AppSettings.TaskflowFileDirectory) == false)
					dlg.InitialDirectory = CxAuxInfoForm.AppSettings.TaskflowFileDirectory;
				else if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
					dlg.InitialDirectory = XIE.Tasks.SharedData.ProjectDir;

				if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
					dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);
				if (string.IsNullOrWhiteSpace(CxAuxInfoForm.AppSettings.TaskflowFileDirectory) == false)
					dlg.CustomPlaces.Add(CxAuxInfoForm.AppSettings.TaskflowFileDirectory);
			}
			#endregion

			var ans = dlg.ShowDialog(owner);
			if (ans == DialogResult.OK)
			{
				node.Info.FileName = dlg.FileName;
				node.Save();
			}

			return ans;
		}

		#endregion

		#region 編集履歴:

		/// <summary>
		/// 編集履歴: Undo Buffer
		/// </summary>
		private List<XIE.Tasks.CxTaskflow> HistoryUndos = new List<XIE.Tasks.CxTaskflow>();

		/// <summary>
		/// 編集履歴: Redo Buffer
		/// </summary>
		private List<XIE.Tasks.CxTaskflow> HistoryRedos = new List<XIE.Tasks.CxTaskflow>();

		/// <summary>
		/// 編集履歴: 追加
		/// </summary>
		/// <param name="update_time">タイムスタンプを更新するか否か</param>
		public void AddHistory(bool update_time = true)
		{
			var task = (XIE.Tasks.CxTaskflow)this.Taskflow.Clone();
			this.HistoryUndos.Add(task);
			this.HistoryRedos.Clear();
			if (update_time)
				this.UpdatedTime = DateTime.Now;
		}

		/// <summary>
		/// 編集履歴: 戻す事が可能か否か
		/// </summary>
		public bool CanUndo()
		{
			return (0 < this.HistoryUndos.Count);
		}

		/// <summary>
		/// 編集履歴: 進む事が可能か否か
		/// </summary>
		public bool CanRedo()
		{
			return (0 < this.HistoryRedos.Count);
		}

		/// <summary>
		/// 編集履歴: 戻す
		/// </summary>
		public void Undo()
		{
			if (this.HistoryUndos.Count == 0) return;
			this.HistoryRedos.Add(this.Taskflow);
			this.Taskflow = this.HistoryUndos[this.HistoryUndos.Count - 1];
			this.HistoryUndos.Remove(this.Taskflow);
			this.Visualizer.HandlingInfo.Reset();
		}

		/// <summary>
		/// 編集履歴: 進む
		/// </summary>
		public void Redo()
		{
			if (this.HistoryRedos.Count == 0) return;
			this.HistoryUndos.Add(this.Taskflow);
			this.Taskflow = this.HistoryRedos[this.HistoryRedos.Count - 1];
			this.HistoryRedos.Remove(this.Taskflow);
			this.Visualizer.HandlingInfo.Reset();
		}

		#endregion

		#region 印刷処理関連:

		/// <summary>
		/// 印刷対象のタスクフローのページ番号 [0~]
		/// </summary>
		public int Document_PageNo = 0;

		/// <summary>
		/// 印刷対象のタスクフローのコレクション [1~]
		/// </summary>
		public List<XIE.Tasks.CxTaskflow> Document_Taskflows = new List<XIE.Tasks.CxTaskflow>();

		/// <summary>
		/// 印刷対象のタスクフローのページタイトルのコレクション
		/// </summary>
		public List<string> Document_Titles = new List<string>();

		/// <summary>
		/// 印刷対象のタスクフローのコレクションの初期化
		/// </summary>
		public void Document_Setup()
		{
			Document_PageNo = 0;
			Document_Taskflows.Clear();
			Document_Titles.Clear();

			// 最上位のタスクフローを追加する.
			{
				Document_Taskflows.Add(this.Taskflow);
				Document_Titles.Add(this.Taskflow.Name);
			}

			// 子タスクにタスクフローがあれば追加する.
			var tasks = this.Taskflow.GetTasks(true);
			foreach (var task in tasks)
			{
				if (task is XIE.Tasks.CxTaskflow)
				{
					var taskflow = (XIE.Tasks.CxTaskflow)task;
					var title = Document_GetPageTitle(taskflow);
					Document_Taskflows.Add(taskflow);
					Document_Titles.Add(title);
				}
			}
		}

		/// <summary>
		/// 指定のタスクフローのページタイトルを取得します。
		/// </summary>
		/// <param name="target"></param>
		/// <returns>
		///		ページタイトルを返します。
		/// </returns>
		private string Document_GetPageTitle(XIE.Tasks.CxTaskflow target)
		{
			var path = new List<XIE.Tasks.CxTaskflow>();
			Document_GetPath(target, path);
			string title = "";
			for (int i = 0; i < path.Count; i++)
			{
				if (i == 0)
					title = path[i].Name;
				else
					title += string.Format(" / {0}", path[i].Name);
			}
			return title;
		}

		/// <summary>
		/// 指定のタスクフローまでのパスを取得します。
		/// </summary>
		/// <param name="target"></param>
		/// <param name="path"></param>
		private void Document_GetPath(XIE.Tasks.CxTaskflow target, List<XIE.Tasks.CxTaskflow> path)
		{
			var parent = this.FindParentTaskflow(target);
			if (parent != null)
			{
				Document_GetPath(parent, path);
			}
			path.Add(target);
		}

		#endregion

		#region メソッド: (情報)

		/// <summary>
		/// 基準ディレクトリの取得
		/// </summary>
		/// <returns>
		///		現在設定されている Info.AssemblyName の位置を返します。
		///		未設定の場合は TaskflowFileDirectory を返します。
		///		何れも未設定の場合は CurrentDirectory を返します。
		/// </returns>
		public virtual string GetBaseDir()
		{
			var baseDir = CxAuxInfoForm.AppSettings.TaskflowFileDirectory;
			if (string.IsNullOrEmpty(this.Info.FileName) == false)
				baseDir = System.IO.Path.GetDirectoryName(this.Info.FileName);
			if (string.IsNullOrEmpty(baseDir))
				baseDir = System.IO.Directory.GetCurrentDirectory();
			return baseDir;
		}

		#endregion

		#region メソッド: (描画用)

		/// <summary>
		/// 画像ビューに描画する画像
		/// </summary>
		private XIE.CxImage m_BgImage = null;

		/// <summary>
		/// 画像ビューに描画する画像の取得
		/// </summary>
		/// <param name="task">対象のタスクユニット</param>
		/// <returns>
		///		指定のタスクのデータ出力ポートから画像を検索して返します。
		///		表示可能な画像が無ければ null を返します。
		/// </returns>
		public XIE.CxImage GetBgImage(XIE.Tasks.CxTaskUnit task)
		{
			foreach (var item in task.DataOut)
			{
				if (item.Data is XIE.CxImage)
				{
					var image = (XIE.CxImage)item.Data;
					if (m_BgImage == null)
						m_BgImage = new XIE.CxImage();
					m_BgImage.CopyFrom(image);
					if (m_BgImage.IsValid && m_BgImage.Depth == 0)
					{
						m_BgImage.Depth = m_BgImage.CalcDepth(-1);
						if (1 < m_BgImage.Depth && m_BgImage.Depth < 8)
							m_BgImage.Depth = 8;
					}
					return m_BgImage;
				}
			}
			return null;
		}

		/// <summary>
		/// 画像ビューに描画する画像の取得
		/// </summary>
		/// <param name="port">対象のデータ出力ポート</param>
		/// <returns>
		///		指定のタスクのデータ出力ポートから画像を取得して返します。
		///		表示可能な画像が無ければ null を返します。
		/// </returns>
		public XIE.CxImage GetBgImage(XIE.Tasks.CxTaskPortOut port)
		{
			if (port.Data is XIE.CxImage)
			{
				var image = (XIE.CxImage)port.Data;
				if (m_BgImage == null)
					m_BgImage = new XIE.CxImage();
				m_BgImage.CopyFrom(image);
				if (m_BgImage.IsValid && m_BgImage.Depth == 0)
				{
					m_BgImage.Depth = m_BgImage.CalcDepth(-1);
					if (1 < m_BgImage.Depth && m_BgImage.Depth < 8)
						m_BgImage.Depth = 8;
				}
				return m_BgImage;
			}
			return null;
		}

		/// <summary>
		/// 画像ビューの背景に表示する画像の取得
		/// </summary>
		/// <param name="task">対象のタスクユニット</param>
		/// <param name="pos">クリックされた位置</param>
		/// <returns>
		///		画像オブジェクトへの参照を返します。
		/// </returns>
		public XIE.CxImage GetBgImage(XIE.Tasks.CxTaskUnit task, TxTaskUnitPosition pos)
		{
			if (task == null) return null;

			#region クリックされた位置によって表示する画像を切り替える.
			switch (pos.Type)
			{
				default:
				case ExTaskUnitPositionType.None:
					break;
				case ExTaskUnitPositionType.Body:
					{
						return GetBgImage(task);
					}
				case ExTaskUnitPositionType.DataIn:
					{
						var item = task.DataIn[pos.Index];
						if (item.IsConnected)
							return GetBgImage(item.ReferencePort);
					}
					break;
				case ExTaskUnitPositionType.DataParam:
					{
						var item = task.DataParam[pos.Index];
						if (item.IsConnected)
							return GetBgImage(item.ReferencePort);
					}
					break;
				case ExTaskUnitPositionType.DataOut:
					return GetBgImage(task.DataOut[pos.Index]);
			}
			#endregion

			return null;
		}

		#endregion

		#region メソッド: (ソースコード生成)

		/// <summary>
		/// コンパイル
		/// </summary>
		/// <param name="language_type">言語種別</param>
		/// <param name="statement">構文</param>
		/// <returns>
		///		コンパイル結果を返します。
		/// </returns>
		public virtual CompilerResults Compile(XIE.Tasks.ExLanguageType language_type, string statement)
		{
			var provider = this.GetProvider(language_type);
			var cc_param = new CompilerParameters();
			var refasm = this.GetReferencedAssemblies(XIE.Tasks.SharedData.References.Values);

			#region コンパイラパラメータの設定:
			{
				cc_param.GenerateExecutable = false;
				cc_param.GenerateInMemory = true;
				cc_param.WarningLevel = 4;
				cc_param.TreatWarningsAsErrors = false;
				cc_param.ReferencedAssemblies.AddRange(refasm);

				#region DebugMode
				if (this.AuxInfo != null &&
					this.AuxInfo.DebugMode &&
					string.IsNullOrEmpty(this.AuxInfo.ProjectDir) == false)
				{
					// スクリプトをデバッグする為の設定です。
					// ビルドの度に一時ファイルが生成されることに注意してください。
					// 一時ファイルは下記ディレクトリ配下に生成されます。
					// Dir="%USERPROFILE%\Documents\XIE-Studio-1.0\TempFiles"

					cc_param.IncludeDebugInformation = true;
					string tempDir = System.IO.Path.Combine(this.AuxInfo.ProjectDir, "TempFiles");
					if (System.IO.Directory.Exists(tempDir) == false)
						System.IO.Directory.CreateDirectory(tempDir);
					cc_param.TempFiles = new TempFileCollection(tempDir, true);
				}
				else
				{
					cc_param.IncludeDebugInformation = false;
				}
				#endregion

				#region CompilerOptions
				cc_param.CompilerOptions = "/t:library /optimize";
				switch (language_type)
				{
					case XIE.Tasks.ExLanguageType.CSharp:
						cc_param.CompilerOptions += " /unsafe+";
						break;
					case XIE.Tasks.ExLanguageType.VisualBasic:
						break;
					default:
						throw new XIE.CxException(XIE.ExStatus.InvalidParam);
				}
				#endregion
			}
			#endregion

			// (!) 2015.02.20(Sat):
			// VisualBasic の場合はこの方法は使えません。
			// 代わりに CompileAssemblyFromDom を使用してください。
			var results = provider.CompileAssemblyFromSource(cc_param, statement);

			return results;
		}

		/// <summary>
		/// コンパイル
		/// </summary>
		/// <param name="language_type">言語種別</param>
		/// <param name="cc_unit">コードユニット</param>
		/// <returns>
		///		コンパイル結果を返します。
		/// </returns>
		public virtual CompilerResults Compile(XIE.Tasks.ExLanguageType language_type, CodeCompileUnit cc_unit)
		{
			var provider = this.GetProvider(language_type);
			var cc_param = new CompilerParameters();
			var refasm = this.GetReferencedAssemblies(XIE.Tasks.SharedData.References.Values);

			#region コンパイラパラメータの設定:
			{
				cc_param.GenerateExecutable = false;
				cc_param.GenerateInMemory = true;
				cc_param.WarningLevel = 4;
				cc_param.TreatWarningsAsErrors = false;
				cc_param.ReferencedAssemblies.AddRange(refasm);

				#region DebugMode
				if (this.AuxInfo != null &&
					this.AuxInfo.DebugMode &&
					string.IsNullOrEmpty(this.AuxInfo.ProjectDir) == false)
				{
					// スクリプトをデバッグする為の設定です。
					// ビルドの度に一時ファイルが生成されることに注意してください。
					// 一時ファイルは下記ディレクトリ配下に生成されます。
					// Dir="%USERPROFILE%\Documents\XIE-Studio-1.0\TempFiles"

					cc_param.IncludeDebugInformation = true;
					string tempDir = System.IO.Path.Combine(this.AuxInfo.ProjectDir, "TempFiles");
					if (System.IO.Directory.Exists(tempDir) == false)
						System.IO.Directory.CreateDirectory(tempDir);
					cc_param.TempFiles = new TempFileCollection(tempDir, true);
				}
				else
				{
					cc_param.IncludeDebugInformation = false;
				}
				#endregion

				#region CompilerOptions
				cc_param.CompilerOptions = "/t:library /optimize";
				switch (language_type)
				{
					case XIE.Tasks.ExLanguageType.CSharp:
						cc_param.CompilerOptions += " /unsafe+";
						break;
					case XIE.Tasks.ExLanguageType.VisualBasic:
						break;
					default:
						throw new XIE.CxException(XIE.ExStatus.InvalidParam);
				}
				#endregion
			}
			#endregion

			var results = provider.CompileAssemblyFromDom(cc_param, cc_unit);

			return results;
		}

		/// <summary>
		/// ソースコード生成
		/// </summary>
		/// <param name="language_type">言語種別</param>
		/// <param name="entry_point">エントリポイントを追加するか否か</param>
		/// <returns>
		///		生成したコードユニットを返します。
		/// </returns>
		public CodeCompileUnit GenerateCode(XIE.Tasks.ExLanguageType language_type, bool entry_point = false)
		{
			if (this.Taskflow == null)
				throw new CxException(XIE.ExStatus.InvalidObject, "Taskflow is null.");

			string target_ns = "Users";
			string target_type = "UserTask";

			var cc_unit = new CodeCompileUnit();

			// 参照設定:
			var refasm = this.GetReferencedAssemblies(XIE.Tasks.SharedData.References.Values);
			cc_unit.ReferencedAssemblies.AddRange(refasm);

			#region コンパイルユニットの生成:
			{
				var code_ns = new CodeNamespace(target_ns);
				var used_imports = new Dictionary<string, string>();

				#region タスクフローのクラス名の割り当て:
				// 全てのタスクフローにクラス名を割り当てる.
				var classNames = new Dictionary<XIE.Tasks.CxTaskUnit, string>();
				{
					int index = 0;

					// 最上位.
					classNames[this.Taskflow] = string.Format("{0}_{1}", target_type, index++);

					// 子タスク.
					var whole = this.Taskflow.GetTasks(true);
					foreach (var item in whole)
					{
						if (item is XIE.Tasks.Syntax_Class)
							classNames[item] = string.Format("{0}_{1}", target_type, index++);
						else if (item is XIE.Tasks.CxScriptEx)
							classNames[item] = string.Format("{0}_{1}", target_type, index++);
					}
				}
				#endregion

				#region エントリポイントの生成:
				if (entry_point)
				{
					var code_ep = this.GenerateEntryPoint(target_ns, classNames[this.Taskflow]);
					code_ns.Types.Add(code_ep);
				}
				#endregion

				#region クラスの生成:
				foreach (var item in classNames)
				{
					var args = new XIE.Tasks.CxGenerateCodeArgs(language_type, 0, code_ns, null, null);
					args.ClassNames = classNames;

					// コード生成.
					if (item.Key is XIE.Tasks.Syntax_Class)
						((XIE.Tasks.Syntax_Class)item.Key).GenerateTypeCode(this, args);
					else if (item.Key is XIE.Tasks.CxScriptEx)
					{
						var task = (XIE.Tasks.CxScriptEx)item.Key;
						task.GenerateTypeCode(this, args);

						foreach (var import in task.Imports)
						{
							string name = import.Trim();
							if (name.Length > 0)
								used_imports[import] = name;
						}
					}
				}
				#endregion

				#region 名前空間:
				if (used_imports.Count == 0)
				{
					var base_imports = ApiHelper.CreateImports();
					foreach (var item in base_imports)
						code_ns.Imports.Add(new CodeNamespaceImport(item));
				}
				else
				{
					foreach (var item in used_imports)
					{
						string name = item.Key.Trim();
						if (name.Length > 0)
							code_ns.Imports.Add(new CodeNamespaceImport(name));
					}
				}
				#endregion

				cc_unit.Namespaces.Add(code_ns);
			}
			#endregion

			return cc_unit;
		}

		/// <summary>
		/// エントリポイントの生成
		/// </summary>
		/// <param name="target_ns">名前空間</param>
		/// <param name="target_type">最初に呼び出すクラス</param>
		/// <returns>
		///		生成した CodeTypeDeclaration を返します。
		/// </returns>
		private CodeTypeDeclaration GenerateEntryPoint(string target_ns, string target_type)
		{
			var program = new CodeTypeDeclaration("Program");

			#region エントリポイント: (Program)
			{
				program.Comments.Add(new CodeCommentStatement("<summary>", true));
				program.Comments.Add(new CodeCommentStatement("EntryPoint", true));
				program.Comments.Add(new CodeCommentStatement("</summary>", true));
				program.IsPartial = true;
				program.Attributes = MemberAttributes.Static | MemberAttributes.Private;
			}
			#endregion

			#region エントリポイント: (Main)
			{
				var member = new CodeEntryPointMethod();
				member.Comments.Add(new CodeCommentStatement("<summary>", true));
				member.Comments.Add(new CodeCommentStatement("EntryPoint", true));
				member.Comments.Add(new CodeCommentStatement("</summary>", true));

				#region 共通:
				var sender_type = typeof(object);
				var sender_name = "sender";

				var baseDir_type = typeof(string);
				var baseDir_name = "baseDir";

				var baseName_type = typeof(string);
				var baseName_name = "baseName";

				var loop_type = typeof(int);
				var loop_name = "loop";

				var aux_type = typeof(XIE.Tasks.CxAuxInfo);
				var aux_name = "auxInfo";

				var task_type = string.Format("{0}.{1}", target_ns, target_type);
				var task_name = "task";
				#endregion

				#region sender:
				{
					// object sender = null;
					member.Statements.Add(
						new CodeVariableDeclarationStatement(
							sender_type,
							sender_name,
							new CodePrimitiveExpression(null)
						));
				}
				#endregion

				#region baseDir:
				{
					// var baseDir = System.IO.Directory.GetCurrentDirectory();
					member.Statements.Add(
						new CodeVariableDeclarationStatement(
							baseDir_type,
							baseDir_name,
							new CodeMethodInvokeExpression(
								new CodeTypeReferenceExpression(typeof(System.IO.Directory)),
								"GetCurrentDirectory"
							)
						));
				}
				#endregion

				#region baseName:
				{
					// var baseName = "yyyymmddHHMMSS";
					member.Statements.Add(
						new CodeVariableDeclarationStatement(
							baseName_type,
							baseName_name,
							new CodePrimitiveExpression(this.Taskflow.Name)
						));
				}
				#endregion

				#region loop:
				{
					// var loop = 1;
					member.Statements.Add(
						new CodeVariableDeclarationStatement(
							loop_type,
							loop_name,
							new CodePrimitiveExpression(1)
						));
				}
				#endregion

				#region AuxInfo:
				{
					// var auxInfo = new XIE.Tasks.CxAuxInfo();
					member.Statements.Add(
						new CodeVariableDeclarationStatement(
							aux_type,
							aux_name,
							new CodeObjectCreateExpression(aux_type)
						));
				}
				#endregion

				#region AuxInfo: (Load)
				{
					// filename
					var filename = System.IO.Path.Combine(XIE.Tasks.SharedData.ProjectDir, CxAuxInfoForm.AuxInfoFileName);

					// auxInfo.Load(filename);
					member.Statements.Add(
						new CodeMethodInvokeExpression(
							new CodeVariableReferenceExpression(aux_name),
							"Load",
							new CodeExpression[]
								{
									new CodePrimitiveExpression(filename)
								}
						));
				}
				#endregion

				#region AuxInfo: (Setup)
				{
					// auxInfo.Setup();
					member.Statements.Add(
						new CodeMethodInvokeExpression(
							new CodeVariableReferenceExpression(aux_name),
							"Setup"
						));
				}
				#endregion

				#region UserTask:
				{
					// var task = new Users.UserTask();
					member.Statements.Add(
						new CodeVariableDeclarationStatement(
							task_type,
							task_name,
							new CodeObjectCreateExpression(task_type)
						));
				}
				#endregion

				#region UserTask: (Setup)
				{
					// task.Setup(sender, e, parent);
					member.Statements.Add(
						new CodeMethodInvokeExpression(
							new CodeVariableReferenceExpression(task_name),
							"Setup",
							new CodeExpression[]
								{
									// sender
									new CodeVariableReferenceExpression(sender_name),
									// e
									new CodeObjectCreateExpression(
										typeof(XIE.Tasks.CxTaskSetupEventArgs),
										new CodeExpression[]
										{
											CodeLiteral.Null,
											new CodeVariableReferenceExpression(aux_name),
											new CodeVariableReferenceExpression(baseDir_name),
											new CodeVariableReferenceExpression(baseName_name),
										}
									),
									// parent
									CodeLiteral.Null,
								}
						));
				}
				#endregion

				#region UserTask: (Prepare)
				{
					// task.Prepare(sender, e);
					member.Statements.Add(
						new CodeMethodInvokeExpression(
							new CodeVariableReferenceExpression(task_name),
							"Prepare",
							new CodeExpression[]
								{
									// sender
									new CodeVariableReferenceExpression(sender_name),
									// e
									new CodeObjectCreateExpression(
										typeof(XIE.Tasks.CxTaskExecuteEventArgs),
										new CodeExpression[]
										{
											CodeLiteral.Null,
											new CodeVariableReferenceExpression(aux_name),
											new CodeVariableReferenceExpression(baseDir_name),
											new CodeVariableReferenceExpression(baseName_name),
											new CodePrimitiveExpression(0),
											new CodeVariableReferenceExpression(loop_name),
										}
									),
								}
						));
				}
				#endregion

				#region UserTask: (Execute)
				{
					var initial_type = typeof(int);
					var initial_name = "i";

					#region for: arguments
					var initial_statement = new CodeVariableDeclarationStatement(
							initial_type,
							initial_name,
							new CodePrimitiveExpression(1)
						);

					var limit_expression = new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression(initial_name),
							CodeBinaryOperatorType.LessThanOrEqual,
							new CodeVariableReferenceExpression(loop_name)
						);

					var step_statement = new CodeAssignStatement(
							new CodeVariableReferenceExpression(initial_name),
							new CodeBinaryOperatorExpression(
								new CodeVariableReferenceExpression(initial_name),
								CodeBinaryOperatorType.Add,
								new CodePrimitiveExpression(1)
							)
						);
					#endregion

					#region for: body_statements
					CodeStatement[] body_statements;
					{
						var this_scope = new CodeStatementCollection();

						// task.Execute(sender, e);
						this_scope.Add(
							new CodeMethodInvokeExpression(
								new CodeVariableReferenceExpression(task_name),
								"Execute",
								new CodeExpression[]
									{
										// sender
										new CodeVariableReferenceExpression(sender_name),
										// e
										new CodeObjectCreateExpression(
											typeof(XIE.Tasks.CxTaskExecuteEventArgs),
											new CodeExpression[]
											{
												CodeLiteral.Null,
												new CodeVariableReferenceExpression(aux_name),
												new CodeVariableReferenceExpression(baseDir_name),
												new CodeVariableReferenceExpression(baseName_name),
												new CodeVariableReferenceExpression(initial_name),
												new CodeVariableReferenceExpression(loop_name),
											}
										),
									}
							));

						body_statements = new CodeStatement[this_scope.Count];
						if (this_scope.Count > 0)
							this_scope.CopyTo(body_statements, 0);
					}
					#endregion

					member.Statements.Add(new CodeIterationStatement(initial_statement, limit_expression, step_statement, body_statements));
				}
				#endregion

				#region UserTask: (Restore)
				{
					// task.Restore(sender, e);
					member.Statements.Add(
						new CodeMethodInvokeExpression(
							new CodeVariableReferenceExpression(task_name),
							"Restore",
							new CodeExpression[]
								{
									// sender
									new CodeVariableReferenceExpression(sender_name),
									// e
									new CodeObjectCreateExpression(
										typeof(XIE.Tasks.CxTaskExecuteEventArgs),
										new CodeExpression[]
										{
											CodeLiteral.Null,
											new CodeVariableReferenceExpression(aux_name),
											new CodeVariableReferenceExpression(baseDir_name),
											new CodeVariableReferenceExpression(baseName_name),
											new CodePrimitiveExpression(0),
											new CodeVariableReferenceExpression(loop_name),
										}
									),
								}
						));
				}
				#endregion

				program.Members.Add(member);
			}
			#endregion

			return program;
		}

		/// <summary>
		/// 指定のキーに該当する Summary を抽出します。
		/// </summary>
		/// <param name="key"></param>
		/// <returns>
		///		抽出した文字列を返します。
		///		該当する Summary がなければ空白を返します。
		/// </returns>
		private string GetSummary(string key)
		{
			try
			{
				return XIE.AxiTextStorage.Texts[key];
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.Message);
				return "";
			}
		}

		/// <summary>
		/// 言語種別に対応するプロバイダを取得します。
		/// </summary>
		/// <param name="language_type">言語種別</param>
		/// <returns>
		///		プロバイダを返します。
		/// </returns>
		public CodeDomProvider GetProvider(XIE.Tasks.ExLanguageType language_type)
		{
			var options = new Dictionary<string, string>();

			#region ProviderOptions
			try
			{
				// (!) var を使用したいだけなので .NET 4.0 以降では指定不要です.
				if (Environment.Version.Major == 2)
				{
					Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5", false);
					object value = regkey.GetValue("Install");
					if (value is int && ((int)value) == 1)
					{
						options["CompilerVersion"] = "v3.5";
					}
				}
			}
			catch (System.Exception)
			{
			}
			#endregion

			switch (language_type)
			{
				case XIE.Tasks.ExLanguageType.CSharp:
					return new Microsoft.CSharp.CSharpCodeProvider(options);
				case XIE.Tasks.ExLanguageType.VisualBasic:
					return new Microsoft.VisualBasic.VBCodeProvider(options);
				default:
					throw new XIE.CxException(XIE.ExStatus.InvalidParam);
			}
		}

		/// <summary>
		/// 参照設定を取得します。
		/// </summary>
		/// <param name="references">参照設定</param>
		/// <returns>
		///		参照するアセンブリ名の一覧を返します。
		/// </returns>
		private string[] GetReferencedAssemblies(IEnumerable<XIE.Tasks.CxReferencedAssembly> references)
		{
			var result = new List<string>();

			foreach (var item in references)
			{
				Assembly asm = null;

				#region 厳密名があれば優先的に使用する.
				if (string.IsNullOrWhiteSpace(item.FullName) == false)
				{
					try
					{
						asm = Assembly.Load(item.FullName);

						// フルパスで参照する形式.
						result.Add(asm.Location);
					}
					catch (System.Exception)
					{
						// 一時的に GAC 登録解除している可能性を考慮する.
					}
				}
				#endregion

				#region ファイル名でロードを試みる.
				if (asm == null &&
					string.IsNullOrWhiteSpace(item.HintPath) == false &&
					System.IO.File.Exists(item.HintPath))
				{
					try
					{
						asm = Assembly.LoadFrom(item.HintPath);

						// ファイル名で参照する形式.
						result.Add(item.HintPath);
					}
					catch (System.Exception ex)
					{
						XIE.Log.Api.Error(ex.StackTrace);
					}
				}
				#endregion
			}

			return result.ToArray();
		}

		#endregion

		#region メソッド: (コントロールパネル管理)

		/// <summary>
		/// コントロールパネル管理オブジェクト
		/// </summary>
		private Dictionary<Form, XIE.Tasks.CxTaskUnit> ControlPanelManagement = new Dictionary<Form, XIE.Tasks.CxTaskUnit>();

		/// <summary>
		/// コントロールパネル管理オブジェクトへ追加します。
		/// </summary>
		/// <param name="form">コントロールパネルフォーム</param>
		/// <param name="task">コントロールパネルフォームを生成したタスク</param>
		public void ControlPanelManagement_Add(Form form, XIE.Tasks.CxTaskUnit task)
		{
			ControlPanelManagement[form] = task;
			form.FormClosed += (_sender, _e) =>
				{
					ControlPanelManagement.Remove(form);
				};
		}

		/// <summary>
		/// コントロールパネル管理オブジェクトに格納された項目を全て除去します。
		/// </summary>
		public void ControlPanelManagement_CloseAll()
		{
			foreach (var item in ControlPanelManagement)
			{
				item.Key.Close();
			}
			ControlPanelManagement.Clear();
		}

		/// <summary>
		/// コントロールパネル管理オブジェクトに格納された項目がタスクフローになければ除去します。
		/// </summary>
		public void ControlPanelManagement_Purge()
		{
			var removed_items = new List<Form>();
			var tasks = this.Taskflow.GetTasks(true);
			foreach (var item in ControlPanelManagement)
			{
				int index = tasks.FindIndex((task) => { return ReferenceEquals(task, item.Value); });
				if (index < 0)
				{
					removed_items.Add(item.Key);
				}
			}
			foreach (var item in removed_items)
			{
				item.Close();
			}
		}

		#endregion
	}
}
