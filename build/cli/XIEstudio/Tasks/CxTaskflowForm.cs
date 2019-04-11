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
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using System.CodeDom;
using System.CodeDom.Compiler;
using XIE;

namespace XIEstudio
{
	/// <summary>
	/// タスクフロー編集フォーム
	/// </summary>
	public partial class CxTaskflowForm : Form
	{
		#region 共有データ:

		/// <summary>
		/// タスクユニットのコレクション
		/// </summary>
		public static Dictionary<XIE.Tasks.CxTaskUnit, string> TaskUnits = new Dictionary<XIE.Tasks.CxTaskUnit, string>();

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskflowForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="node">タスクノード</param>
		public CxTaskflowForm(CxTaskNode node)
		{
			InitializeComponent();
			this.TaskNode = node;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// タスクノード
		/// </summary>
		public CxTaskNode TaskNode
		{
			get { return m_TaskNode; }
			set { m_TaskNode = value; }
		}
		private CxTaskNode m_TaskNode = new CxTaskNode();

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowForm_Load(object sender, EventArgs e)
		{
			#region アイコン設定.
			// 
			// 詳細は XIEstudio に記載するコメントを参照されたい.
			// 現象)
			//   デザイナでアイコンを指定した場合、Linux Mono 環境では下記の例外が発生する.
			//   System.ArgumentException: A null reference or invalid value was found [GDI+ status: InvalidParameter]
			// 
			switch (System.Environment.OSVersion.Platform)
			{
				case PlatformID.Unix:
					// 
					// 上記理由によりアイコンの設定は行わない.
					// 
					break;
				default:
					{
						var aux = (XIE.Tasks.IxAuxImageList16)CxAuxInfoForm.AuxInfo;
						var image = (Bitmap)aux.ImageList.Images[this.TaskNode.ImageKey];
						this.Icon = Icon.FromHandle(image.GetHicon());
					}
					break;
			}
			#endregion

			panelPageTaskflow.Visible = true;
			panelPageImageView.Visible = false;
			panelPageTimes.Visible = false;
			panelPageTaskflow.Dock = DockStyle.Fill;
			panelPageImageView.Dock = DockStyle.Fill;
			panelPageTimes.Dock = DockStyle.Fill;
			panelTaskflow.BackColor = CxAuxInfoForm.AppSettings.BkColor;

			this.TaskNode.Document.PrintPage += printTaskflow_PrintPage;
			toolTaskLoopMax.SelectedIndex = 0;
			pictureTaskflow.AllowDrop = true;
			toolTaskControlPanel.Enabled = false;
			toolRenderingProperty.Enabled = false;
			toolReportProperty.Enabled = false;

			Toolbox_Setup();
			Outline_Setup();
			Taskflow_Setup();
			TaskflowHelpForm_Setup();
			TaskflowPreviewForm_Setup();

			TabTimes_Setup();
		//	Console_Setup();
			ImageView_Setup();
			ImagePreviewForm_Setup();

			splitMain.Panel1Collapsed = !this.TaskNode.IsVisibleLeftPanel;
			splitView.Panel2Collapsed = !this.TaskNode.IsVisibleBottomPanel;

			CxAuxInfoForm.AuxInfo.Updated += AuxInfo_Updated;
			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();
			CxAuxInfoForm.AuxInfo.Updated -= AuxInfo_Updated;

			this.TaskNode.Document.PrintPage -= printTaskflow_PrintPage;

			this.TaskNode.IsVisibleLeftPanel = !splitMain.Panel1Collapsed;
			this.TaskNode.IsVisibleBottomPanel = !splitView.Panel2Collapsed;

			TabTimes_Setup();
		//	Console_TearDown();
			ImageView_TearDown();
			ImagePreviewForm_TearDown();

			Outline_TearDown();
			Toolbox_TearDown();
			TaskflowHelpForm_TearDown();
			TaskflowPreviewForm_TearDown();
			Taskflow_TearDown();
		}

		#endregion

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			#region タイトル表示:
			if (this.TaskNode != null)
			{
				if (this.Text != this.TaskNode.Text)
					this.Text = this.TaskNode.Text;
			}
			#endregion

			#region toolbar の表示更新:
			{
				bool is_running = this.TaskNode.IsRunning;

				// toolbarMain
				toolPageTaskflow.Checked = (panelPageTaskflow.Visible);
				toolPageImageView.Checked = (panelPageImageView.Visible);
				toolImagePreview.Checked = (this.ImagePreviewForm != null && this.ImagePreviewForm.Visible);
				toolTaskflowPreview.Checked = (this.TaskflowPreviewForm != null && this.TaskflowPreviewForm.Visible);
				toolTaskHelp.Checked = (this.TaskflowHelpForm != null && this.TaskflowHelpForm.Visible);
				toolTaskLoopMax.Enabled = (is_running == false && this.IsTaskRepeat == false);
				toolTaskRepeat.Checked = (this.IsTaskRepeat == true);
				toolTaskRepeat.Enabled = (is_running == false);
				toolTaskStart.Enabled = (is_running == false || (is_running && this.IsWaitingInBreakpoint));
				toolTaskStop.Enabled = (is_running == true);

				// toolbarTaskflow
				if (is_running == false)
				{
					toolTaskflowEditUndo.Enabled = this.TaskNode.CanUndo();
					toolTaskflowEditRedo.Enabled = this.TaskNode.CanRedo();

					var tasks = this.TaskNode.CurrentTaskflow.TaskUnits.FindAll(item => item.Selected);
					toolTaskflowEditCut.Enabled = (tasks.Count > 0);
					toolTaskflowEditCopy.Enabled = (tasks.Count > 0);
					toolTaskflowEditPaste.Enabled = (Clipboard.ContainsData("XIE.Tasks.CxTaskUnit"));
				}
				else
				{
					toolTaskflowEditUndo.Enabled = false;
					toolTaskflowEditRedo.Enabled = false;
					toolTaskflowEditCut.Enabled = false;
					toolTaskflowEditCopy.Enabled = false;
					toolTaskflowEditPaste.Enabled = false;
				}

				// toolbarView
				toolViewHalftone.Checked = this.ImageView.Halftone;
				toolViewUnpack.Checked = (this.ImageView.Unpack);
				toolViewChannelNo.Text = this.ImageView.ChannelNo.ToString();
				toolViewProfile.Checked = this.ProfileOverlay.Visible;
				toolViewGrid.Checked = this.GridOverlay.Visible;
			}
			#endregion

			#region statusbar 表示更新:
			{
				statusbar_Update();
			}
			#endregion

			#region ユニットの移動 または ポートの接続:
			{
				var info = this.TaskNode.Visualizer.HandlingInfo;
				if (info.GripPosition.Type == ExTaskUnitPositionType.Body ||
					info.GripPosition.Type == ExTaskUnitPositionType.DataOut ||
					info.GripPosition.Type == ExTaskUnitPositionType.ControlOut)
				{
					if (info.Button == MouseButtons.Left)
					{
						bool is_changed = pictureTaskflow_AutoScroll();

						// 移動先の候補(赤色の矩形)または接続線の候補(橙色の線分)を描画するため、表示更新する必要がある.
						if (is_changed == false)
							pictureTaskflow.Refresh();
					}
				}
			}
			#endregion

			#region ヘルプの表示更新:
			if (TaskflowHelpForm != null)
			{
				if (treeToolbox.Focused)
				{
					var node = treeToolbox.SelectedNode;
					if (node is CxToolboxNode)
					{
						var task = ((CxToolboxNode)node).Task;
						if (!ReferenceEquals(TaskflowHelpForm.Task, task))
						{
							TaskflowHelpForm.Update(task);
						}
					}
					else
					{
						TaskflowHelpForm.Update(null);
					}
				}
				else if (treeOutline.Focused)
				{
					var node = treeOutline.SelectedNode;
					if (node is CxToolboxNode)
					{
						var task = ((CxToolboxNode)node).Task;
						if (!ReferenceEquals(TaskflowHelpForm.Task, task))
						{
							TaskflowHelpForm.Update(task);
						}
					}
					else
					{
						TaskflowHelpForm.Update(null);
					}
				}
				else if (pictureTaskflow.Focused || propertyTask.Focused)
				{
					var focued_unit = this.TaskNode.Visualizer.HandlingInfo.FocusedUnit;
					if (!ReferenceEquals(TaskflowHelpForm.Task, focued_unit))
					{
						TaskflowHelpForm.Update(focued_unit);
					}
				}
			}
			#endregion
		}

		// //////////////////////////////////////////////////
		// 外部機器情報
		// //////////////////////////////////////////////////

		#region 外部機器情報: (イベントハンドラ)

		/// <summary>
		/// 外部機器情報通知イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void AuxInfo_Updated(object sender, XIE.Tasks.CxAuxNotifyEventArgs e)
		{
			#region CxTaskUnit からの通知:
			if (sender is XIE.Tasks.CxTaskUnit)
			{
				var action = new Action(() =>
					{
						var task = (XIE.Tasks.CxTaskUnit)sender;

						ImageView_DrawImage();
						ImageView_Refresh();

						propertyTask.Refresh();
						pictureTaskflow.Refresh();

						gridReport_Update(sender, e);
					});
				if (this.InvokeRequired)
					this.Invoke(action);
				else
					action();
			}
			#endregion
		}

		#endregion

		// //////////////////////////////////////////////////
		// toolbar / statusbar
		// //////////////////////////////////////////////////

		#region toolbarMain: (ページ切り替え)

		/// <summary>
		/// toolbarMain: Taskflow の表示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPageTaskflow_Click(object sender, EventArgs e)
		{
			if (panelPageTaskflow.Visible == false)
			{
				panelPageTaskflow.Visible = true;
				panelPageImageView.Visible = false;
				panelPageTimes.Visible = false;
			}
		}

		/// <summary>
		/// toolbarMain: ImageView の表示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPageImageView_Click(object sender, EventArgs e)
		{
			if (panelPageImageView.Visible == false)
			{
				panelPageTaskflow.Visible = false;
				panelPageImageView.Visible = true;
				panelPageTimes.Visible = false;
			}
		}

		/// <summary>
		/// toolbarMain: Times の表示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPageTimes_Click(object sender, EventArgs e)
		{
			if (panelPageTimes.Visible == false)
			{
				panelPageTaskflow.Visible = false;
				panelPageImageView.Visible = false;
				panelPageTimes.Visible = true;
			}
		}

		#endregion

		#region toolbarMain: (タスクプロパティ)

		/// <summary>
		/// タスクプロパティの編集
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskProperty_Click(object sender, EventArgs e)
		{
			menuTaskflowProperty_Click(sender, e);
		}

		#endregion

		#region toolbarMain: (ファイル操作)

		/// <summary>
		/// ファイル保存
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFileSave_Click(object sender, EventArgs e)
		{
			menuTaskflowFileSave_Click(sender, e);
		}

		/// <summary>
		/// 印刷
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFilePrint_Click(object sender, EventArgs e)
		{
			menuTaskflowFilePrint_Click(sender, e);
		}

		/// <summary>
		/// 印刷プレビュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFilePrintPreview_Click(object sender, EventArgs e)
		{
			menuTaskflowFilePrintPreview_Click(sender, e);
		}

		#endregion

		#region toolbarMain: (ソースコード生成)

		/// <summary>
		/// ソースコード生成
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFileGenerateCode_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			if (this.TaskNode.ExecutedTime <= this.TaskNode.UpdatedTime)
			{
				string message = "You need to execute in advance.";
				message += "\n" + "Do you want to continue without running?";
				var ans = MessageBox.Show(this, message, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (ans != DialogResult.Yes)
					return;
			}

			try
			{
				XIE.Tasks.ExLanguageType type;
				{
					var menu = sender as ToolStripMenuItem;
					if (menu == null) return;
					int id = Convert.ToInt32(menu.Tag);
					switch (id)
					{
						default:
							return;
						case 1:
							type = XIE.Tasks.ExLanguageType.CSharp;
							break;
						case 2:
							type = XIE.Tasks.ExLanguageType.VisualBasic;
							break;
					}
				}

				var dlg = new CxTaskflowSourceForm(this.TaskNode, type);
				dlg.StartPosition = FormStartPosition.Manual;
				dlg.Location = ApiHelper.GetNeighborPosition(dlg.Size);
				dlg.ShowDialog(this);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region toolbarMain: (フォーム切り替え)

		/// <summary>
		/// ヘルプフォームの表示/非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskHelp_Click(object sender, EventArgs e)
		{
			if (TaskflowHelpForm == null)
			{
				var task = this.TaskNode.Visualizer.HandlingInfo.FocusedUnit;
				TaskflowHelpForm = new CxTaskflowHelpForm(task);
				TaskflowHelpForm.StartPosition = FormStartPosition.Manual;
				TaskflowHelpForm.Location = TaskflowHelpForm_GetTypicalLocation();
				TaskflowHelpForm.FormClosing += TaskflowHelpForm_FormClosing;
				TaskflowHelpForm.Move += TaskflowHelpForm_Move;
				TaskflowHelpForm.Resize += TaskflowHelpForm_Resize;
				TaskflowHelpForm_UpdateOffset();
				TaskflowHelpForm.Show(this);
			}
			else if (TaskflowHelpForm.Visible == false)
			{
				if (TaskflowHelpForm.IsDocking)
				{
					#region ヘルプフォームが可視範囲外に出たら再調整する.
					Point st = new Point(0, 0);
					Point ed = new Point(0, 0);
					foreach (Screen screen in Screen.AllScreens)
					{
						if (st.X > screen.WorkingArea.Left)
							st.X = screen.WorkingArea.Left;
						if (st.Y > screen.WorkingArea.Top)
							st.Y = screen.WorkingArea.Top;
						if (ed.X < screen.WorkingArea.Right)
							ed.X = screen.WorkingArea.Right;
						if (ed.Y < screen.WorkingArea.Bottom)
							ed.Y = screen.WorkingArea.Bottom;
					}
					if (TaskflowHelpForm.Location.X < st.X ||
						TaskflowHelpForm.Location.Y < st.Y ||
						TaskflowHelpForm.Location.X > ed.X ||
						TaskflowHelpForm.Location.Y > ed.Y)
					{
						TaskflowHelpForm.Location = TaskflowHelpForm_GetTypicalLocation();
					}
					#endregion
				}
				TaskflowHelpForm.Visible = true;
			}
			else
			{
				TaskflowHelpForm.Visible = false;
				TaskflowHelpForm_UpdateOffset();
			}
		}

		/// <summary>
		/// タスクフロープレビューフォームの表示切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskflowPreview_Click(object sender, EventArgs e)
		{
			if (TaskflowPreviewForm == null)
			{
				TaskflowPreviewForm = new CxTaskflowPreviewForm(panelTaskflow, pictureTaskflow, this.TaskNode);
				TaskflowPreviewForm.Owner = this;
				TaskflowPreviewForm.StartPosition = FormStartPosition.Manual;
				TaskflowPreviewForm.Location = TaskflowPreviewForm_GetTypicalLocation();
				TaskflowPreviewForm.FormClosing += TaskflowPreviewForm_FormClosing;
				TaskflowPreviewForm.Move += TaskflowPreviewForm_Move;
				TaskflowPreviewForm.Resize += TaskflowPreviewForm_Resize;
				TaskflowPreviewForm_UpdateOffset();
				TaskflowPreviewForm.Show();
			}
			else if (TaskflowPreviewForm.Visible == false)
			{
				if (TaskflowPreviewForm.IsDocking)
				{
					#region タスクフロープレビューフォームが可視範囲外に出たら再調整する.
					Point st = new Point(0, 0);
					Point ed = new Point(0, 0);
					foreach (Screen screen in Screen.AllScreens)
					{
						if (st.X > screen.WorkingArea.Left)
							st.X = screen.WorkingArea.Left;
						if (st.Y > screen.WorkingArea.Top)
							st.Y = screen.WorkingArea.Top;
						if (ed.X < screen.WorkingArea.Right)
							ed.X = screen.WorkingArea.Right;
						if (ed.Y < screen.WorkingArea.Bottom)
							ed.Y = screen.WorkingArea.Bottom;
					}
					if (TaskflowPreviewForm.Location.X < st.X ||
						TaskflowPreviewForm.Location.Y < st.Y ||
						TaskflowPreviewForm.Location.X > ed.X ||
						TaskflowPreviewForm.Location.Y > ed.Y)
					{
						TaskflowPreviewForm.Location = TaskflowPreviewForm_GetTypicalLocation();
					}
					#endregion
				}
				TaskflowPreviewForm.Visible = true;
			}
			else
			{
				TaskflowPreviewForm.Visible = false;
				TaskflowPreviewForm_UpdateOffset();
			}
		}

		/// <summary>
		/// 画像プレビューフォームの表示切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolImagePreview_Click(object sender, EventArgs e)
		{
			if (ImagePreviewForm == null)
			{
				ImagePreviewForm = new CxTaskflowImageForm(this.TaskNode, this.ImageView);
				ImagePreviewForm.Owner = this;
				ImagePreviewForm.StartPosition = FormStartPosition.Manual;
				ImagePreviewForm.Location = ImagePreviewForm_GetTypicalLocation();
				ImagePreviewForm.EnableVisibleRect = panelPageImageView.Visible;
				ImagePreviewForm.FormClosing += ImagePreviewForm_FormClosing;
				ImagePreviewForm.Move += ImagePreviewForm_Move;
				ImagePreviewForm.Resize += ImagePreviewForm_Resize;
				ImagePreviewForm_UpdateOffset();
				ImagePreviewForm.Show();
			}
			else if (ImagePreviewForm.Visible == false)
			{
				if (ImagePreviewForm.IsDocking)
				{
					#region 画像プレビューフォームが可視範囲外に出たら再調整する.
					Point st = new Point(0, 0);
					Point ed = new Point(0, 0);
					foreach (Screen screen in Screen.AllScreens)
					{
						if (st.X > screen.WorkingArea.Left)
							st.X = screen.WorkingArea.Left;
						if (st.Y > screen.WorkingArea.Top)
							st.Y = screen.WorkingArea.Top;
						if (ed.X < screen.WorkingArea.Right)
							ed.X = screen.WorkingArea.Right;
						if (ed.Y < screen.WorkingArea.Bottom)
							ed.Y = screen.WorkingArea.Bottom;
					}
					if (ImagePreviewForm.Location.X < st.X ||
						ImagePreviewForm.Location.Y < st.Y ||
						ImagePreviewForm.Location.X > ed.X ||
						ImagePreviewForm.Location.Y > ed.Y)
					{
						ImagePreviewForm.Location = ImagePreviewForm_GetTypicalLocation();
					}
					#endregion
				}

				ImagePreviewForm.Visible = true;
				ImagePreviewForm.EnableVisibleRect = panelPageImageView.Visible;
			}
			else
			{
				ImagePreviewForm.Visible = false;
				ImagePreviewForm_UpdateOffset();
			}
		}

		#endregion

		#region toolbarMain: (ウィンドウ操作)

		/// <summary>
		/// ページ切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPageToggle_Click(object sender, EventArgs e)
		{
			panelPageTaskflow.Visible = !panelPageTaskflow.Visible;
			panelPageImageView.Visible = !panelPageImageView.Visible;
		}

		/// <summary>
		/// 右辺パネルの開閉
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPanelR_Click(object sender, EventArgs e)
		{
			splitMain.Panel2Collapsed = !splitMain.Panel2Collapsed;
		}

		/// <summary>
		/// 下辺パネルの開閉
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPanelB_Click(object sender, EventArgs e)
		{
			splitView.Panel2Collapsed = !splitView.Panel2Collapsed;
		}

		/// <summary>
		/// 折り畳み
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolCollapse_Click(object sender, EventArgs e)
		{
			if (splitMain.Panel2Collapsed && splitView.Panel2Collapsed)
			{
				splitMain.Panel2Collapsed = splitMain_Panel2Collapsed;
				splitView.Panel2Collapsed = splitView_Panel2Collapsed;
			}
			else
			{
				splitMain_Panel2Collapsed = splitMain.Panel2Collapsed;
				splitView_Panel2Collapsed = splitView.Panel2Collapsed;
				splitMain.Panel2Collapsed = true;
				splitView.Panel2Collapsed = true;
			}
		}

		private bool splitMain_Panel2Collapsed = false;
		private bool splitView_Panel2Collapsed = false;

		#endregion

		#region toolbarTaskflow: (Scale)

		/// <summary>
		/// toolbarTaskflow: Toolbox の開閉
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolToolbox_Click(object sender, EventArgs e)
		{
			splitTaskflow.Panel1Collapsed = !splitTaskflow.Panel1Collapsed;
		}

		/// <summary>
		/// toolbarTaskflow: 表示倍率ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskflowViewScale_ButtonClick(object sender, EventArgs e)
		{
			var scale = 100;
			this.TaskNode.Visualizer.Scale = scale;
			pictureTaskflow_AdjustSize(scale);
			this.pictureTaskflow.Refresh();
		}

		/// <summary>
		/// toolbarTaskflow: 表示倍率メニューが選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskflowViewScale_Click(object sender, EventArgs e)
		{
			var menu = (ToolStripMenuItem)sender;
			if (menu.Tag != null)
			{
				var scale = Convert.ToInt32(menu.Tag);
				this.TaskNode.Visualizer.Scale = scale;
				pictureTaskflow_AdjustSize(scale);
				this.pictureTaskflow.Refresh();
			}
		}

		/// <summary>
		/// toolbarTaskflow: 表示倍率ダウンボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskflowViewScaleDown_Click(object sender, EventArgs e)
		{
			var scale = pictureTaskflow_CalcScale(-1);
			pictureTaskflow_AdjustSize(scale);
			this.pictureTaskflow.Refresh();
		}

		/// <summary>
		/// toolbarTaskflow: 表示倍率アップボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskflowViewScaleUp_Click(object sender, EventArgs e)
		{
			var scale = pictureTaskflow_CalcScale(+1);
			pictureTaskflow_AdjustSize(scale);
			this.pictureTaskflow.Refresh();
		}

		#endregion

		#region toolbarView: (ディスプレイ)

		/// <summary>
		/// toolbarView: 画像全体が View に表示されるように表示倍率を調整します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewFitImageSize_ButtonClick(object sender, EventArgs e)
		{
			this.ImageView.FitImageSize(0);
			this.ImageView_Refresh();
		}

		/// <summary>
		/// toolbarView: 画像の幅を View の幅に合わせます。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewFitImageWidth_Click(object sender, EventArgs e)
		{
			this.ImageView.FitImageSize(1);
			this.ImageView_Refresh();
		}

		/// <summary>
		/// toolbarView: 画像の高さを View の幅に合わせます。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewFitImageHeight_Click(object sender, EventArgs e)
		{
			this.ImageView.FitImageSize(2);
			this.ImageView_Refresh();
		}


		/// <summary>
		/// toolbarView: 表示倍率を既定値に設定します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewScale_ButtonClick(object sender, EventArgs e)
		{
			int scale = 0;
			PropertyInfo prop = sender.GetType().GetProperty("Tag");
			if (prop != null)
				scale = Convert.ToInt32(prop.GetValue(sender, null));

			if (scale > 0)
			{
				this.ImageView.Magnification = scale * 0.01;
				this.ImageView_Refresh();
			}
			else
			{
				this.ImageView.AdjustScale(0);
				this.ImageView_Refresh();
			}
		}

		/// <summary>
		/// toolbarView: 表示倍率メニューが開いたときの表示更新処理。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewScale_DropDownOpened(object sender, EventArgs e)
		{
			ToolStripMenuItem[] items = 
			{
				toolViewScale0010,
				toolViewScale0025,
				toolViewScale0050,
				toolViewScale0075,
				toolViewScale0100,
				toolViewScale0200,
				toolViewScale0400,
				toolViewScale0800,
				toolViewScale1600,
				toolViewScale3200,
			};

			foreach (ToolStripMenuItem item in items)
			{
				PropertyInfo prop = item.GetType().GetProperty("Tag");
				if (prop != null)
				{
					double mag = this.ImageView.Magnification;
					int tag = Convert.ToInt32(prop.GetValue(item, null));
					item.Checked = (tag * 0.01 == mag);
				}
			}
		}

		/// <summary>
		/// toolbarView: 表示倍率を縮小します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewScaleDown_Click(object sender, EventArgs e)
		{
			this.ImageView.AdjustScale(-1);
			this.ImageView_Refresh();
		}

		/// <summary>
		/// toolbarView: 表示倍率を拡大します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewScaleUp_Click(object sender, EventArgs e)
		{
			this.ImageView.AdjustScale(+1);
			this.ImageView_Refresh();
		}

		/// <summary>
		/// toolbarView: 伸縮時の濃度補間方法の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewHalftone_Click(object sender, EventArgs e)
		{
			this.ImageView.Halftone = !this.ImageView.Halftone;
			this.ImageView_Refresh();
		}

		/// <summary>
		/// toolbarView: ビット深度の調整
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewDepth_ButtonClick(object sender, EventArgs e)
		{
			var image = this.ImageView.Image;
			if (image != null && image.IsValid)
			{
				int target_channel = -1;
				if (this.ImageView.Unpack)
				{
					int index = this.ImageView.ChannelNo;
					int count = image.Channels * image.Model.Pack;
					if (0 <= index && index < count)
						target_channel = index;
				}
				image.Depth = image.CalcDepth(target_channel);
				ImageView_Refresh();
			}
		}

		/// <summary>
		/// toolbarView: ビット深度の調整 (既定値)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewDepthDefault_Click(object sender, EventArgs e)
		{
			var image = this.ImageView.Image;
			if (image != null && image.IsValid)
			{
				image.Depth = 0;
				ImageView_Refresh();
			}
		}

		/// <summary>
		/// toolbarView: パッキング/アンパッキングの切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewUnpack_Click(object sender, EventArgs e)
		{
			this.ImageView.Unpack = !this.ImageView.Unpack;
			this.ImageView_Refresh();
		}

		/// <summary>
		/// toolbarView: 表示チャネル指標の切り替え (前へ)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewChannelPrev_Click(object sender, EventArgs e)
		{
			if (this.ImageView.Image == null) return;
			int ch = this.ImageView.ChannelNo - 1;
			if (ch < 0)
				ch = 0;
			this.ImageView.ChannelNo = ch;
			this.ImageView_Refresh();
		}

		/// <summary>
		/// toolbarView: 表示チャネル指標の切り替え (次へ)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewChannelNext_Click(object sender, EventArgs e)
		{
			if (this.ImageView.Image == null) return;
			int ch = this.ImageView.ChannelNo + 1;
			if (ch > XIE.Defs.XIE_IMAGE_CHANNELS_MAX - 1)
				ch = XIE.Defs.XIE_IMAGE_CHANNELS_MAX - 1;
			this.ImageView.ChannelNo = ch;
			this.ImageView_Refresh();
		}

		#endregion

		#region toolbarView: (オーバレイ)

		/// <summary>
		/// toolbarView: Profile の表示/非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewProfile_Click(object sender, EventArgs e)
		{
			ProfileOverlay.Visible = !ProfileOverlay.Visible;
			ImageView_Refresh();
		}

		/// <summary>
		/// toolbarView: Grid の表示/非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewGrid_Click(object sender, EventArgs e)
		{
			GridOverlay.Visible = !GridOverlay.Visible;
			ImageView_Refresh();
		}

		/// <summary>
		/// toolbarView: Graph の表示/非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewGraph_Click(object sender, EventArgs e)
		{
		}

		#endregion

		#region toolbarView: (スナップショット)

		/// <summary>
		/// toolbarView: スナップショットボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewSnapshot_ButtonClick(object sender, EventArgs e)
		{
			if (ImageView.Image != null)
			{
				try
				{
					var data = (CxImage)ImageView.Image.Clone();
					data.ExifCopy(ImageView.Image.Exif());

					var name = "Snap-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
					var args = new XIE.Tasks.CxAuxNotifyEventArgs_AddImage(data, name);
					this.TaskNode.AuxInfo.SendRequested(this, args);
				}
				catch (System.Exception)
				{
				}
			}
		}

		/// <summary>
		/// toolbarView: スナップショットのメニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewSnapshot_DropDownOpened(object sender, EventArgs e)
		{
			var is_valid = (ImageView.Image != null);
			toolViewSnapshotOverlayMode0.Enabled = is_valid;
			toolViewSnapshotOverlayMode1.Enabled = is_valid;
			toolViewSnapshotOverlayMode2.Enabled = is_valid;
		}

		/// <summary>
		/// toolbarView: オーバレイ付きスナップショットのモード切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewSnapshotOverlayMode_Click(object sender, EventArgs e)
		{
			var item = sender as ToolStripMenuItem;
			if (ImageView.Image != null && item != null)
			{
				try
				{
					XIE.CxImage data;
					var mode = Convert.ToInt32(item.Tag);
					switch (mode)
					{
						default:
						case 0:
							{
								var display_size = ImageView.Image.Size;
								var view_point = ImageView.ViewPoint;
								var mag = 1;
								data = ImageView.Snapshot(display_size, view_point, mag);
							}
							break;
						case 1:
							{
								var view_point = ImageView.ViewPoint;
								var mag = ImageView.Magnification;
								var vis = ImageView.VisibleRect();
								var display_size = new XIE.TxSizeI(
										(int)System.Math.Ceiling(vis.Width * mag),
										(int)System.Math.Ceiling(vis.Height * mag)
									);
								data = ImageView.Snapshot(display_size, view_point, mag);
							}
							break;
						case 2:
							{
								data = ImageView.Snapshot();
							}
							break;
					}
					if (ImageView.Image != null)
						data.ExifCopy(ImageView.Image.Exif());

					var name = "Overlay-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
					var args = new XIE.Tasks.CxAuxNotifyEventArgs_AddImage(data, name);
					this.TaskNode.AuxInfo.SendRequested(this, args);
				}
				catch (System.Exception)
				{
				}
			}
		}

		#endregion

		#region statusbar:

		/// <summary>
		/// ステータスバー表示更新:
		/// </summary>
		private void statusbar_Update()
		{
			if (panelPageTaskflow.Visible)
				statusbar_Update_Taskflow();
			if (panelPageImageView.Visible)
				statusbar_Update_ImageView();
		}

		/// <summary>
		/// ステータスバー表示更新: (Taskflow)
		/// </summary>
		private void statusbar_Update_Taskflow()
		{
			var container = this.panelTaskflow;
			var picturebox = this.pictureTaskflow;
			var location = picturebox.Location;

			#region マウス位置情報:
			{
				var dp = pictureTaskflow.PointToClient(Control.MousePosition);
				statusMouseInfo.Text = string.Format("{0},{1}", dp.X, dp.Y);
			}
			#endregion

			#region 位置補正:
			if (container.Width < picturebox.Width)
			{
				if (location.X < -(picturebox.Width - container.Width))
					location.X = -(picturebox.Width - container.Width);
				if (location.X > 0)
					location.X = 0;
			}
			else
			{
				location.X = 0;
			}

			if (container.Height < picturebox.Height)
			{
				if (location.Y < -(picturebox.Height - container.Height))
					location.Y = -(picturebox.Height - container.Height);
				if (location.Y > 0)
					location.Y = 0;
			}
			else
			{
				location.Y = 0;
			}

			if (picturebox.Location.X != location.X ||
				picturebox.Location.Y != location.Y)
			{
				picturebox.Location = location;
			}
			#endregion

			#region シートの始点と終点:
			{
				var st = new Point(
					-picturebox.Location.X,
					-picturebox.Location.Y
					);
				var ed = new Point(
					st.X + picturebox.Width,
					st.Y + picturebox.Height
					);
				var scale = this.TaskNode.Visualizer.Scale;
				statusViewInfo.Text = string.Format("[ {0},{1} - {2},{3} ] ({4}%)", st.X, st.Y, ed.X, ed.Y, scale);
			}
			#endregion

			#region シート規格の表示:
			{
				var spec = this.TaskNode.Visualizer.GetSheetSpec();
				var size = this.TaskNode.Visualizer.GetSheetSize();
				if (statusbar_PrevSheetSize.Width != size.Width ||
					statusbar_PrevSheetSize.Height != size.Height)
				{
					statusSheetInfo.Text = string.Format("{0}: {1},{2}", spec, size.Width, size.Height);
				}
			}
			#endregion
		}
		private Size statusbar_PrevSheetSize = new Size();

		/// <summary>
		/// ステータスバー表示更新: (ImageView)
		/// </summary>
		private void statusbar_Update_ImageView()
		{
			var view = this.ImageView;
			var image = view.Image;
			if (image == null)
			{
				statusViewInfo.Text = "";
				statusMouseInfo.Text = "";
			}
			else
			{
				#region ImageInfo
				statusViewInfo.Text = string.Format("{0}x{1} {2}ch {3}x{4} {5}",
					image.Width,
					image.Height,
					image.Channels,
					image.Model.Type,
					image.Model.Pack,
					((image.Depth == 0) ? "" : string.Format("({0}bits)", image.Depth))
					);
				#endregion

				#region MouseInfo
				{
					XIE.TxPointD dp = view.PointToClient(Control.MousePosition);
					XIE.TxPointD ip = (view.Magnification < 2.0)
						? view.DPtoIP(dp, XIE.GDI.ExGdiScalingMode.TopLeft)
						: view.DPtoIP(dp, XIE.GDI.ExGdiScalingMode.Center);
					double mag =  view.Magnification;
					statusMouseInfo.Text = string.Format("dp={0:F0},{1:F0} ip={2:F0},{3:F0} (x{4})",
						dp.X, dp.Y,
						ip.X, ip.Y,
						(mag < 0.01) ? mag.ToString("F3") : mag.ToString("F2")
					);
				}
				#endregion
			}
		}

		#endregion

		// //////////////////////////////////////////////////
		// CxTaskflowForm
		// //////////////////////////////////////////////////

		#region CxTaskflowForm: (コントロールイベント)

		/// <summary>
		/// CxTaskflowForm: フォームがアクティブになった時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowForm_Activated(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// CxTaskflowForm: フォームが非アクティブになった時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowForm_Deactivate(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// CxAuxInfoForm: フォームが移動した時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowForm_Move(object sender, EventArgs e)
		{
			ImagePreviewForm_Follow();
			TaskflowPreviewForm_Follow();
			TaskflowHelpForm_Follow();
		}

		/// <summary>
		/// CxAuxInfoForm: フォームがサイズ変更した時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowForm_Resize(object sender, EventArgs e)
		{
			ImagePreviewForm_Follow();
			TaskflowPreviewForm_Follow();
			TaskflowHelpForm_Follow();
		}

		#endregion

		#region CxTaskflowForm: (キーボードイベント)

		/// <summary>
		/// CxTaskflowForm: キーボード押下
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowForm_KeyDown(object sender, KeyEventArgs e)
		{
			#region File - Save: [Ctrl+S]
			if (e.KeyCode == Keys.S && !e.Alt && e.Control && !e.Shift)
			{
				e.Handled = true;
				menuTaskflowFileSave_Click(sender, e);
				return;
			}
			#endregion

			#region File - Print: [Ctrl+P]
			if (e.KeyCode == Keys.P && !e.Alt && e.Control && !e.Shift)
			{
				e.Handled = true;
				menuTaskflowFilePrint_Click(sender, e);
				return;
			}
			#endregion

			#region Edit - Undo: [Ctrl+Z]
			if (e.KeyCode == Keys.Z && !e.Alt && e.Control && !e.Shift)
			{
				e.Handled = true;
				menuTaskflowEditUndo_Click(sender, e);
				return;
			}
			#endregion

			#region Edit - Redo: [Ctrl+Y]
			if (e.KeyCode == Keys.Y && !e.Alt && e.Control && !e.Shift)
			{
				e.Handled = true;
				menuTaskflowEditRedo_Click(sender, e);
				return;
			}
			#endregion

			#region Task - Start: [F5]
			if (e.KeyCode == Keys.F5 && !e.Alt && !e.Control && !e.Shift)
			{
				e.Handled = true;
				toolTaskStart_Click(sender, e);
				return;
			}
			#endregion

			#region Task - Delete All Breakpoints: [Ctrl+Shift+F9]
			if (e.KeyCode == Keys.F9 && !e.Alt && e.Control && e.Shift)
			{
				e.Handled = true;
				menuTaskflowDeleteAllBreakpoints_Click(sender, e);
				return;
			}
			#endregion

			if (pictureTaskflow.Focused)
			{
				#region Edit - Cut: [Ctrl+X]
				if (e.KeyCode == Keys.X && !e.Alt && e.Control && !e.Shift)
				{
					e.Handled = true;
					menuTaskflowEditCut_Click(sender, e);
					return;
				}
				#endregion

				#region Edit - Copy: [Ctrl+C]
				if (e.KeyCode == Keys.C && !e.Alt && e.Control && !e.Shift)
				{
					e.Handled = true;
					menuTaskflowEditCopy_Click(sender, e);
					return;
				}
				#endregion

				#region Edit - Paste: [Ctrl+V]
				if (e.KeyCode == Keys.V && !e.Alt && e.Control && !e.Shift)
				{
					e.Handled = true;
					menuTaskflowEditPaste_Click(sender, e);
					return;
				}
				#endregion

				#region Edit - Select All: [Ctrl+A]
				if (e.KeyCode == Keys.A && !e.Alt && e.Control && !e.Shift)
				{
					e.Handled = true;
					menuTaskflowEditSelectAll_Click(sender, e);
					return;
				}
				#endregion

				#region Task - Toggle Breakpoint: [F9]
				if (e.KeyCode == Keys.F9 && !e.Alt && !e.Control && !e.Shift)
				{
					e.Handled = true;
					menuTaskBodyBreakpoint_Click(sender, e);
					return;
				}
				#endregion

				#region Task - Delete Unit: [Del]
				if (e.KeyCode == Keys.Delete && !e.Alt && !e.Control && !e.Shift)
				{
					e.Handled = true;
					menuTaskBodyDelete_Click(sender, e);
					return;
				}
				#endregion

				#region Task - Enter Sheet: [Ctrl+E]
				if (e.KeyCode == Keys.E && !e.Alt && e.Control && !e.Shift)
				{
					e.Handled = true;
					menuTaskBodyEnter_Click(sender, e);
					return;
				}
				#endregion

				#region Task - Quit Sheet: [Ctrl+Q]
				if (e.KeyCode == Keys.Q && !e.Alt && e.Control && !e.Shift)
				{
					e.Handled = true;
					menuTaskflowQuit_Click(sender, e);
					return;
				}
				#endregion

				#region Task - [Home]
				if (e.KeyCode == Keys.Home && !e.Alt && !e.Control && !e.Shift)
				{
					e.Handled = true;

					var location = this.pictureTaskflow.Location;
					location.X = 0;
					this.pictureTaskflow.Location = location;
					return;
				}
				#endregion

				#region Task - [End]
				if (e.KeyCode == Keys.End && !e.Alt && !e.Control && !e.Shift)
				{
					e.Handled = true;

					var location = this.pictureTaskflow.Location;
					location.X = -(this.pictureTaskflow.Width - this.panelTaskflow.Width);
					this.pictureTaskflow.Location = location;
					return;
				}
				#endregion

				#region Task - [Ctrl+Home]
				if (e.KeyCode == Keys.Home && !e.Alt && e.Control && !e.Shift)
				{
					e.Handled = true;

					var location = this.pictureTaskflow.Location;
					location.X = 0;
					location.Y = 0;
					this.pictureTaskflow.Location = location;
					return;
				}
				#endregion

				#region Task - [Ctrl+End]
				if (e.KeyCode == Keys.End && !e.Alt && e.Control && !e.Shift)
				{
					e.Handled = true;

					var location = this.pictureTaskflow.Location;
					location.X = -(this.pictureTaskflow.Width - this.panelTaskflow.Width);
					location.Y = -(this.pictureTaskflow.Height - this.panelTaskflow.Height);
					this.pictureTaskflow.Location = location;
					return;
				}
				#endregion
			}
		}

		/// <summary>
		/// CxTaskflowForm: キーボード押下
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
		}

		#endregion

		// //////////////////////////////////////////////////
		// panelTaskflow
		// //////////////////////////////////////////////////

		#region panelTaskflow: (コントロールイベント)

		/// <summary>
		/// panelTaskflow: サイズが変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panelTaskflow_Resize(object sender, EventArgs e)
		{
			ImagePreviewForm_Follow();
			TaskflowPreviewForm_Follow();
			TaskflowHelpForm_Follow();

			var container = this.panelTaskflow;
			var picturebox = this.pictureTaskflow;

			{
				var st = picturebox.Location;
				st.X = picturebox.Location.X;
				st.Y = picturebox.Location.Y;

				var ed = new Point();
				ed.X = -(picturebox.Location.X - container.Size.Width);
				ed.Y = -(picturebox.Location.Y - container.Size.Height);

				if (st.X < -(picturebox.Width - container.Width))
					st.X = -(picturebox.Width - container.Width);
				if (st.X > 0)
					st.X = 0;

				if (st.Y < -(picturebox.Height - container.Height))
					st.Y = -(picturebox.Height - container.Height);
				if (st.Y > 0)
					st.Y = 0;

				if (picturebox.Location.X != st.X ||
					picturebox.Location.Y != st.Y)
				{
					picturebox.Location = st;
				}
			}
		}

		#endregion

		#region pictureTaskflow: (コントロールイベント)

		/// <summary>
		/// pictureTaskflow: 位置が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureTaskflow_LocationChanged(object sender, EventArgs e)
		{
			if (this.TaskNode.CurrentTaskflow != null)
				this.TaskNode.CurrentTaskflow.SheetLocation = pictureTaskflow.Location;
		}

		#endregion

		// //////////////////////////////////////////////////
		// Toolbox
		// //////////////////////////////////////////////////

		#region Toolbox: (初期化と解放)

		/// <summary>
		/// Toolbox: 初期化
		/// </summary>
		private void Toolbox_Setup()
		{
			#region イメージリストの初期化:
			{
				var aux = (XIE.Tasks.IxAuxImageList16)CxAuxInfoForm.AuxInfo;
				treeToolbox.ImageList = aux.ImageList;
			}
			#endregion

			#region ツリーノードの初期化:
			foreach (var task in CxTaskflowForm.TaskUnits)
			{
				InitializeToolbox(treeToolbox.Nodes, task);
			}
			#endregion

			#region ツリーノードの初期化: (タスクフロー)
			// 注1) AuxInfo/Tasks は AuxPlugin で設定され、上記の処理で生成される.
			{
				var current = this.TaskNode.Taskflow;
				var nodes = treeToolbox.Nodes["AuxInfo"].Nodes["Tasks"].Nodes;
				var aux = (XIE.Tasks.IxAuxInfoTasks)CxAuxInfoForm.AuxInfo;
				foreach (var task in aux.Tasks)
				{
					if (task == null) continue;
					if (ReferenceEquals(task, current)) continue;
					//int ci = task.DataIn.Length + task.DataParam.Length;
					//int co = task.DataOut.Length;
					//if (ci == 0 && co == 0) continue;

					var node = new CxToolboxNode(task, task.Name);
					node.Setup();
					nodes.Add(node);
				}
			}
			#endregion

			foreach (TreeNode node in treeToolbox.Nodes)
				node.Expand();

			if (treeToolbox.Nodes.Count > 0)
				treeToolbox.Nodes[0].EnsureVisible();
		}

		/// <summary>
		/// Toolbox: 解放
		/// </summary>
		private void Toolbox_TearDown()
		{
		}

		#endregion

		#region Toolbox: (ツリーの初期化)

		/// <summary>
		/// Toolbox: ツリーの初期化
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="task"></param>
		private void InitializeToolbox(TreeNodeCollection nodes, KeyValuePair<XIE.Tasks.CxTaskUnit, string> task)
		{
			#region タスクユニットのアイコンを初期化する.
			try
			{
				task.Key.IconImage = treeToolbox.ImageList.Images[task.Key.IconKey];
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.StackTrace);
			}
			#endregion

			if (task.Key is XIE.Tasks.CxTaskFolder)
			{
				var folder = (XIE.Tasks.CxTaskFolder)task.Key;
				if (folder.DefaultIndex < 0)
				{
					#region 通常のフォルダ:
					var node = new TreeNode();
					node.Name = task.Key.Name;
					node.Text = task.Key.Name;
					node.ImageKey = task.Key.IconKey;
					node.SelectedImageKey = task.Key.IconKey;
					nodes.Add(node);
					foreach (var child in folder.Tasks)
					{
						InitializeToolbox(node.Nodes, child);
					}
					#endregion
				}
				else
				{
					#region 既定のタスクユニットを持つフォルダ:
					var node = new CxToolboxNode(task.Key, task.Value);
					node.Setup();
					nodes.Add(node);
					foreach (var child in folder.Tasks)
					{
						InitializeToolbox(node.Nodes, child);
					}
					#endregion
				}
			}
			else if ((task.Key is XIE.Tasks.CxTaskflow) && !(task.Key is XIE.Tasks.Syntax_Class))
			{
				#region CxTaskflow の派生: (※ Syntax_Class は含みません。)
				var flow = (XIE.Tasks.CxTaskflow)task.Key;
				var node = new CxToolboxNode(task.Key, task.Value);
				node.Setup();
				nodes.Add(node);
				foreach (var child in flow.TaskUnits)
				{
					InitializeToolbox(node.Nodes, new KeyValuePair<XIE.Tasks.CxTaskUnit, string>(child, ""));
				}
				#endregion
			}
			else
			{
				#region 単一のタスクユニット:
				var node = new CxToolboxNode(task.Key, task.Value);
				node.Setup();
				nodes.Add(node);
				#endregion
			}
		}

		#endregion

		#region Toolbox: (コントロールイベント)

		/// <summary>
		///  treeToolbox: マウスが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeToolbox_MouseDown(object sender, MouseEventArgs e)
		{

		}

		#endregion

		#region Toolbox: (ドラッグ＆ドロップ)

		/// <summary>
		/// treeToolbox: 項目のドラッグが開始されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeToolbox_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (e.Item is CxToolboxNode)
			{
				if (this.TaskNode.IsRunning == false)
				{
					this.DragTask = null;
					var ctrl = ((Control.ModifierKeys & Keys.Control) == Keys.Control);
					var shift = ((Control.ModifierKeys & Keys.Shift) == Keys.Shift);
					var alt = ((Control.ModifierKeys & Keys.Alt) == Keys.Alt);

					if (!ctrl && !shift && !alt && e.Button == MouseButtons.Left)
					{
						DragDropEffects effect = treeToolbox.DoDragDrop(new DataObject("treeToolbox.CxToolboxNode", e.Item), DragDropEffects.All);
					}
				}
			}
		}

		#endregion

		// //////////////////////////////////////////////////
		// Outline
		// //////////////////////////////////////////////////

		#region Outline: (初期化と解放)

		/// <summary>
		/// Outline: 初期化
		/// </summary>
		private void Outline_Setup()
		{
			var aux = (XIE.Tasks.IxAuxImageList16)CxAuxInfoForm.AuxInfo;
			treeOutline.ImageList = aux.ImageList;
			treeOutline.PaintEx += treeOutline_PaintEx;

			Outline_Update();
		}

		/// <summary>
		/// Outline: 解放
		/// </summary>
		private void Outline_TearDown()
		{
			treeOutline.PaintEx -= treeOutline_PaintEx;
		}

		#endregion

		#region Outline: (更新)

		/// <summary>
		/// Outline: 更新
		/// </summary>
		private void Outline_Update()
		{
			var info = this.TaskNode.Visualizer.HandlingInfo;
			var focused_unit = info.FocusedUnit;

			treeOutline.Nodes.Clear();

			var taskflow = (XIE.Tasks.CxTaskflow)this.TaskNode.Taskflow;
			foreach (var child in taskflow.TaskUnits)
			{
				Outline_Update(treeOutline.Nodes, child);
			}

			#region フォーカスが当たっているタスクに該当するノードの選択.
			if (focused_unit != null)
			{
				var nodes = treeOutline.GetAllNodes();
				foreach (var item in nodes)
				{
					if (item is CxToolboxNode)
					{
						var node = (CxToolboxNode)item;
						if (ReferenceEquals(node.Task, focused_unit))
						{
							treeOutline.SelectedNode = node;
							break;
						}
					}
				}
			}
			#endregion
		}

		/// <summary>
		/// Outline: ツリーの更新
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="task"></param>
		private void Outline_Update(TreeNodeCollection nodes, XIE.Tasks.CxTaskUnit task)
		{
			var node = new CxToolboxNode(task);
			nodes.Add(node);

			if (task is XIE.Tasks.CxTaskflow)
			{
				var taskflow = (XIE.Tasks.CxTaskflow)task;
				foreach (var child in taskflow.TaskUnits)
				{
					Outline_Update(node.Nodes, child);
				}
			}

			node.Expand();
		}

		#endregion

		#region Outline: (移動)

		/// <summary>
		/// Outline: 現在のタスクノードを範囲内に表示します。
		/// </summary>
		private void Outline_EnsureVisible()
		{
			var taskflow = this.TaskNode.CurrentTaskflow;
			if (taskflow != this.TaskNode.Taskflow)
			{
				var nodes = treeOutline.GetAllNodes();
				foreach (var node in nodes)
				{
					if (node is CxToolboxNode)
					{
						var parent = (CxToolboxNode)node;
						if (parent.Task == taskflow)
						{
							var children = treeOutline.GetChildNodes(parent);
							foreach (var child in children)
							{
								child.EnsureVisible();
							}
							node.EnsureVisible();
							break;
						}
					}
				}
			}
		}

		#endregion

		#region Outline: (ドラッグ＆ドロップ)

		/// <summary>
		/// treeOutline: 項目のドラッグが開始されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeOutline_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (e.Item is CxToolboxNode)
			{
				if (this.TaskNode.IsRunning == false)
				{
					this.DragTask = null;
					var ctrl = ((Control.ModifierKeys & Keys.Control) == Keys.Control);
					var shift = ((Control.ModifierKeys & Keys.Shift) == Keys.Shift);
					var alt = ((Control.ModifierKeys & Keys.Alt) == Keys.Alt);

					if (ctrl && !shift && !alt && e.Button == MouseButtons.Left)
					{
						var node = (CxToolboxNode)e.Item;
						if (node.Task is XIE.Tasks.CxTaskflow)
						{
							// (!) 参照禁止.
						}
						else if (node.Task is XIE.Tasks.CxTaskReference)
						{
							// (!) 参照禁止.
						}
						else
						{
							DragDropEffects effect = treeOutline.DoDragDrop(new DataObject("treeOutline.CxToolboxNode", e.Item), DragDropEffects.All);
						}
					}
				}
			}
		}

		#endregion

		#region Outline: (コントロールイベント)

		/// <summary>
		/// Outline: ノードが選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeOutline_AfterSelect(object sender, TreeViewEventArgs e)
		{
			var node = (CxToolboxNode)e.Node;

			var ctrl = ((Control.ModifierKeys & Keys.Control) == Keys.Control);
			var shift = ((Control.ModifierKeys & Keys.Shift) == Keys.Shift);
			var alt = ((Control.ModifierKeys & Keys.Alt) == Keys.Alt);

			#region Left: 選択されたノードのタスク位置へフォーカスを移動する:
			if (!ctrl && !shift && !alt)
			{
				if (treeOutline.Focused)
					pictureTaskflow_EnsureVisible(node.Task);
				return;
			}
			#endregion
		}

		/// <summary>
		/// Outline: マウスが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeOutline_MouseDown(object sender, MouseEventArgs e)
		{
			var tvi = treeOutline.HitTest(e.Location);
			if (tvi == null) return;
			var node = (CxToolboxNode)tvi.Node;

			var ctrl = ((Control.ModifierKeys & Keys.Control) == Keys.Control);
			var shift = ((Control.ModifierKeys & Keys.Shift) == Keys.Shift);
			var alt = ((Control.ModifierKeys & Keys.Alt) == Keys.Alt);

			#region Left:
			if (!ctrl && !shift && !alt && e.Button == MouseButtons.Left)
			{
				// 選択済みの場合は明示的に関数を呼び出す.
				// それ以外は AfterSelect で行う.
				if (node != null)
				{
					if (node == treeOutline.SelectedNode)
					{
						if (treeOutline.Focused)
							pictureTaskflow_EnsureVisible(node.Task);
					}
					else
						treeOutline.SelectedNode = node;
				}
				return;
			}
			#endregion

			#region Right:
			if (!ctrl && !shift && !alt && e.Button == MouseButtons.Right)
			{
				treeOutline.SelectedNode = node;
				return;
			}
			#endregion

			#region Ctrl+Left:
			if (ctrl && !shift && !alt && e.Button == MouseButtons.Left)
			{
				treeOutline.SelectedNode = node;
				return;
			}
			#endregion
		}

		#endregion

		#region Outline: (カスタム描画)

		/// <summary>
		/// Outline: 再描画イベントが発生したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeOutline_PaintEx(object sender, PaintEventArgs e)
		{
			var nodes = treeOutline.GetAllNodes();

			#region Category の描画:
			{
				// ノードの右端の統計.
				var stat = new XIE.TxStatistics();
				foreach (var item in nodes)
				{
					if (item is CxToolboxNode)
					{
						var node = (CxToolboxNode)item;
						stat += node.Bounds.Right;
					}
				}

				// Category 描画領域の左端.
				var left = (float)(8 + stat.Max);

				// 背景塗り潰し.
				using (var brush = new SolidBrush(treeOutline.BackColor))
				{
					e.Graphics.FillRectangle(brush, left, 0, (treeOutline.Width - left), treeOutline.Height);
				}

				// 各ノードの Category 描画.
				using (var font = new Font(treeOutline.Font.FontFamily, treeOutline.Font.Size - 1))
				using (var brush = new SolidBrush(treeOutline.ForeColor))
				{
					foreach (var item in nodes)
					{
						if (item.IsVisible)
						{
							if (item is CxToolboxNode)
							{
								var node = (CxToolboxNode)item;
								var location = new PointF(left, node.Bounds.Y + 0.5f);

								e.Graphics.DrawString(node.Task.Category, font, brush, location);
							}
						}
					}
				}
			}
			#endregion

			#region CurrentTaskflow を示す矩形の描画:
			if (this.TaskNode.Taskflow != this.TaskNode.CurrentTaskflow)
			{
				var taskflow = this.TaskNode.CurrentTaskflow;
				
				foreach (var node in nodes)
				{
					if (node is CxToolboxNode)
					{
						var parent = (CxToolboxNode)node;
						if (parent.Task == taskflow)
						{
							var st = new Point(parent.Bounds.Left, parent.Bounds.Top);
							var ed = new Point(parent.Bounds.Right, parent.Bounds.Bottom);

							st.X -= 24;
							st.Y -= 1;

							var children = treeOutline.GetChildNodes(parent);
							foreach (var child in children)
							{
								if (ed.X < child.Bounds.Right)
									ed.X = child.Bounds.Right;
								if (ed.Y < child.Bounds.Bottom)
									ed.Y = child.Bounds.Bottom;
							}

							ed.X += 1;
							ed.Y += 1;

							using (var pen = new Pen(Color.Red))
							{
								var bounds = new Rectangle(st, new Size(ed.X - st.X, ed.Y - st.Y));
								e.Graphics.DrawRectangle(pen, bounds);
							}

							break;
						}
					}
				}
			}
			#endregion
		}

		#endregion

		// //////////////////////////////////////////////////
		// TaskflowHelpForm
		// //////////////////////////////////////////////////

		#region TaskflowHelpForm: (初期化と解放)

		/// <summary>
		/// ヘルプフォーム: 初期化
		/// </summary>
		private void TaskflowHelpForm_Setup()
		{
		}

		/// <summary>
		/// ヘルプフォーム: 解放
		/// </summary>
		private void TaskflowHelpForm_TearDown()
		{
			if (this.TaskflowHelpForm != null)
				this.TaskflowHelpForm.Close();
			this.TaskflowHelpForm = null;
		}

		#endregion

		#region TaskflowHelpForm: (表示)

		/// <summary>
		/// ヘルプフォーム
		/// </summary>
		private CxTaskflowHelpForm TaskflowHelpForm = null;

		/// <summary>
		/// ヘルプフォームのコンテナ
		/// </summary>
		private Control TaskflowHelpForm_Container
		{
			get { return splitView.Panel1; }
		}

		/// <summary>
		/// ヘルプフォームとコンテナの相対位置
		/// </summary>
		private Point TaskflowHelpForm_LocationOffset = new Point();

		/// <summary>
		/// ヘルプフォームとコンテナの相対位置の更新
		/// </summary>
		private void TaskflowHelpForm_UpdateOffset()
		{
			if (this.TaskflowHelpForm != null)
			{
				var form = TaskflowHelpForm;
				var ctrl = TaskflowHelpForm_Container;
				var location = ctrl.PointToClient(form.Location);
				var offset = new Point(
					location.X - (ctrl.Location.X + ctrl.Width),
					location.Y - (ctrl.Location.Y)
					);
				TaskflowHelpForm_LocationOffset = offset;
			}
		}

		/// <summary>
		/// ヘルプフォームの移動
		/// </summary>
		private void TaskflowHelpForm_Follow()
		{
			if (this.TaskflowHelpForm != null)
			{
				if (this.TaskflowHelpForm.IsDocking)
				{
					var form = TaskflowHelpForm;
					var ctrl = TaskflowHelpForm_Container;
					var location = new Point(
						TaskflowHelpForm_LocationOffset.X + (ctrl.Location.X + ctrl.Width),
						TaskflowHelpForm_LocationOffset.Y + (ctrl.Location.Y)
						);
					form.Location = ctrl.PointToScreen(location);
				}
				else
				{
					TaskflowHelpForm_UpdateOffset();
				}
			}
		}

		/// <summary>
		/// ヘルプフォームの推奨位置の取得
		/// </summary>
		/// <returns>
		///		ヘルプフォームの推奨位置を計算して返します。
		/// </returns>
		private Point TaskflowHelpForm_GetTypicalLocation()
		{
			var form = TaskflowHelpForm;
			var ctrl = TaskflowHelpForm_Container;
			int x = ctrl.Width - form.Width - 4;
			int y = 4 + 36;
			return ctrl.PointToScreen(new Point(x, y));
		}

		/// <summary>
		/// ヘルプフォームが閉じられるとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TaskflowHelpForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				TaskflowHelpForm.Visible = false;
				TaskflowHelpForm_UpdateOffset();
				e.Cancel = true;
			}
			else
			{
				TaskflowHelpForm = null;
			}
		}

		/// <summary>
		/// ヘルプフォームが移動したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TaskflowHelpForm_Move(object sender, EventArgs e)
		{
			TaskflowHelpForm_UpdateOffset();
		}

		/// <summary>
		/// ヘルプフォームがサイズ変更したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TaskflowHelpForm_Resize(object sender, EventArgs e)
		{
			TaskflowHelpForm_UpdateOffset();
		}

		#endregion

		// //////////////////////////////////////////////////
		// TaskflowPreviewForm
		// //////////////////////////////////////////////////

		#region TaskflowPreviewForm: (初期化と解放)

		/// <summary>
		/// タスクフロープレビューフォーム: 初期化
		/// </summary>
		private void TaskflowPreviewForm_Setup()
		{
		}

		/// <summary>
		/// タスクフロープレビューフォーム: 解放
		/// </summary>
		private void TaskflowPreviewForm_TearDown()
		{
			if (this.TaskflowPreviewForm != null)
				this.TaskflowPreviewForm.Close();
			this.TaskflowPreviewForm = null;
		}

		#endregion

		#region TaskflowPreviewForm: (表示)

		/// <summary>
		/// タスクフロープレビューフォーム
		/// </summary>
		private CxTaskflowPreviewForm TaskflowPreviewForm = null;

		/// <summary>
		/// タスクフロープレビューフォームのコンテナ
		/// </summary>
		private Control TaskflowPreviewForm_Container
		{
			get { return splitView.Panel1; }
		}

		/// <summary>
		/// タスクフロープレビューフォームとコンテナの相対位置
		/// </summary>
		private Point TaskflowPreviewForm_LocationOffset = new Point();

		/// <summary>
		/// タスクフロープレビューフォームとコンテナの相対位置の更新
		/// </summary>
		private void TaskflowPreviewForm_UpdateOffset()
		{
			if (this.TaskflowPreviewForm != null)
			{
				var form = TaskflowPreviewForm;
				var ctrl = TaskflowPreviewForm_Container;
				var location = ctrl.PointToClient(form.Location);
				var offset = new Point(
					location.X - (ctrl.Location.X + ctrl.Width),
					location.Y - (ctrl.Location.Y)
					);
				TaskflowPreviewForm_LocationOffset = offset;
			}
		}

		/// <summary>
		/// タスクフロープレビューフォームの移動
		/// </summary>
		private void TaskflowPreviewForm_Follow()
		{
			if (this.TaskflowPreviewForm != null)
			{
				if (this.TaskflowPreviewForm.IsDocking)
				{
					var form = TaskflowPreviewForm;
					var ctrl = TaskflowPreviewForm_Container;
					var location = new Point(
						TaskflowPreviewForm_LocationOffset.X + (ctrl.Location.X + ctrl.Width),
						TaskflowPreviewForm_LocationOffset.Y + (ctrl.Location.Y)
					);
					form.Location = ctrl.PointToScreen(location);
				}
				else
				{
					TaskflowPreviewForm_UpdateOffset();
				}
			}
		}

		/// <summary>
		/// タスクフロープレビューフォームの推奨位置の取得
		/// </summary>
		/// <returns>
		///		プレビューの推奨位置を計算して返します。
		/// </returns>
		private Point TaskflowPreviewForm_GetTypicalLocation()
		{
			var form = TaskflowPreviewForm;
			var ctrl = TaskflowPreviewForm_Container;
			int x = ctrl.Width - form.Width - 4;
			int y = 4 + 36;
			return ctrl.PointToScreen(new Point(x, y));
		}

		/// <summary>
		/// タスクフロープレビューフォームが閉じられるとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TaskflowPreviewForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				TaskflowPreviewForm.Visible = false;
				TaskflowPreviewForm_UpdateOffset();
				e.Cancel = true;
			}
			else
			{
				TaskflowPreviewForm = null;
			}
		}

		/// <summary>
		/// タスクフロープレビューフォームが移動したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TaskflowPreviewForm_Move(object sender, EventArgs e)
		{
			TaskflowPreviewForm_UpdateOffset();
		}

		/// <summary>
		/// タスクフロープレビューフォームがサイズ変更したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TaskflowPreviewForm_Resize(object sender, EventArgs e)
		{
			TaskflowPreviewForm_UpdateOffset();
		}

		#endregion

		// //////////////////////////////////////////////////
		// ImagePreviewForm
		// //////////////////////////////////////////////////

		#region ImagePreviewForm: (初期化と解放)

		/// <summary>
		/// 画像プレビューフォーム: 初期化
		/// </summary>
		private void ImagePreviewForm_Setup()
		{
		}

		/// <summary>
		/// 画像プレビューフォーム: 解放
		/// </summary>
		private void ImagePreviewForm_TearDown()
		{
			if (this.ImagePreviewForm != null)
				this.ImagePreviewForm.Close();
			this.ImagePreviewForm = null;
		}

		#endregion

		#region ImagePreviewForm: (表示)

		/// <summary>
		/// 画像プレビューフォーム
		/// </summary>
		private CxTaskflowImageForm ImagePreviewForm = null;

		/// <summary>
		/// 画像プレビューフォームのコンテナ
		/// </summary>
		private Control ImagePreviewForm_Container
		{
			get { return splitView.Panel1; }
		}

		/// <summary>
		/// 画像プレビューフォームとコンテナの相対位置
		/// </summary>
		private Point ImagePreviewForm_LocationOffset = new Point();

		/// <summary>
		/// 画像プレビューフォームとコンテナの相対位置の更新
		/// </summary>
		private void ImagePreviewForm_UpdateOffset()
		{
			if (this.ImagePreviewForm != null)
			{
				var form = ImagePreviewForm;
				var ctrl = ImagePreviewForm_Container;
				var location = ctrl.PointToClient(form.Location);
				var offset = new Point(
					location.X - (ctrl.Location.X + ctrl.Width),
					location.Y - (ctrl.Location.Y)
					);
				ImagePreviewForm_LocationOffset = offset;
			}
		}

		/// <summary>
		/// 画像プレビューフォームの移動
		/// </summary>
		private void ImagePreviewForm_Follow()
		{
			if (this.ImagePreviewForm != null)
			{
				if (this.ImagePreviewForm.IsDocking)
				{
					var form = ImagePreviewForm;
					var ctrl = ImagePreviewForm_Container;
					var location = new Point(
						ImagePreviewForm_LocationOffset.X + (ctrl.Location.X + ctrl.Width),
						ImagePreviewForm_LocationOffset.Y + (ctrl.Location.Y)
					);
					form.Location = ctrl.PointToScreen(location);
				}
				else
				{
					ImagePreviewForm_UpdateOffset();
				}
			}
		}

		/// <summary>
		/// 画像プレビューフォームの推奨位置の取得
		/// </summary>
		/// <returns>
		///		画像プレビューフォームの推奨位置を計算して返します。
		/// </returns>
		private Point ImagePreviewForm_GetTypicalLocation()
		{
			var form = ImagePreviewForm;
			var ctrl = ImagePreviewForm_Container;
			int x = ctrl.Width - form.Width - 4;
			int y = 4 + 36;
			return ctrl.PointToScreen(new Point(x, y));
		}

		/// <summary>
		/// 画像プレビューフォームが閉じられるとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImagePreviewForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				ImagePreviewForm.Visible = false;
				ImagePreviewForm_UpdateOffset();
				e.Cancel = true;
			}
			else
			{
				ImagePreviewForm = null;
			}
		}

		/// <summary>
		/// 画像プレビューフォームが移動したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImagePreviewForm_Move(object sender, EventArgs e)
		{
			ImagePreviewForm_UpdateOffset();
		}

		/// <summary>
		/// 画像プレビューフォームがサイズ変更したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImagePreviewForm_Resize(object sender, EventArgs e)
		{
			ImagePreviewForm_UpdateOffset();
		}

		#endregion

		// //////////////////////////////////////////////////
		// ImageView
		// //////////////////////////////////////////////////

		#region ImageView: (オブジェクト)

		/// <summary>
		/// 画像ビュー
		/// </summary>
		private XIE.GDI.CxImageView ImageView
		{
			get { return m_ImageView; }
		}
		private XIE.GDI.CxImageView m_ImageView = new XIE.GDI.CxImageView();

		/// <summary>
		/// オーバレイ (プロファイル表示)
		/// </summary>
		private XIE.GDI.CxOverlayProfile ProfileOverlay = new XIE.GDI.CxOverlayProfile();

		/// <summary>
		/// オーバレイ (グリッド表示)
		/// </summary>
		private XIE.GDI.CxOverlayGrid GridOverlay = new XIE.GDI.CxOverlayGrid();

		#endregion

		#region ImageView: (初期化と解放)

		/// <summary>
		/// ImageView: 初期化
		/// </summary>
		private void ImageView_Setup()
		{
			this.panelImageView.Controls.Add(this.ImageView);
			this.ImageView.Dock = DockStyle.Fill;
			this.ImageView.BackgroundBrush = new XIE.GDI.TxGdiBrush(CxAuxInfoForm.AppSettings.BkColor);

			this.ImageView.Resize += ImageView_Resize;
			this.ImageView.PreviewKeyDown += ImageView_PreviewKeyDown;
			this.ImageView.MouseDoubleClick += ImageView_MouseDoubleClick;

			this.ImageView.Rendering += GridOverlay.Rendering;

			this.ImageView.Rendering += ImageView_Rendering;
			this.ImageView.Rendered += ImageView_Rendered;
			this.ImageView.Handling += ImageView_Handling;

			this.ImageView.Rendering += ProfileOverlay.Rendering;
			this.ImageView.Handling += ProfileOverlay.Handling;
		}

		/// <summary>
		/// ImageView: 解放
		/// </summary>
		private void ImageView_TearDown()
		{
			this.ImageView.Resize -= ImageView_Resize;
			this.ImageView.PreviewKeyDown -= ImageView_PreviewKeyDown;
			this.ImageView.MouseDoubleClick -= ImageView_MouseDoubleClick;

			this.ImageView.Rendering -= GridOverlay.Rendering;

			this.ImageView.Rendering -= ImageView_Rendering;
			this.ImageView.Rendered -= ImageView_Rendered;
			this.ImageView.Handling -= ImageView_Handling;

			this.ImageView.Rendering -= ProfileOverlay.Rendering;
			this.ImageView.Handling -= ProfileOverlay.Handling;

			this.ImageView.Dispose();
		}

		#endregion

		#region ImageView: (描画/操作)

		/// <summary>
		/// ImageView: 表示更新
		/// </summary>
		/// <remarks>
		///		画像ビュータブ (tabPageImageView) が選択されていない場合は、
		///		ImageView と SmallView に対して Refresh を行います。
		///		それ以外は、ImageView のみ Refresh を行います。
		///		その場合、ImageView の Rendered イベントの後に
		///		ImagePreviewForm に対して Refresh が行われます。
		/// </remarks>
		private void ImageView_Refresh()
		{
			ImageView.Refresh();

			if (ImageView_IsVisible() == false)
			{
				if (ImagePreviewForm != null)
					ImagePreviewForm.Refresh();
			}
		}

		/// <summary>
		/// ImageView: 可視属性
		/// </summary>
		/// <returns>
		///		画像ビュータブ (tabPageImageView) が開いている場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		private bool ImageView_IsVisible()
		{
			return (panelPageImageView.Visible);
		}

		/// <summary>
		/// ImageView: 背景画像の更新
		/// </summary>
		private void ImageView_DrawImage()
		{
			var info = this.TaskNode.Visualizer.HandlingInfo;

			#region 既定の画像: (全て探索して最後に見つかった画像を残す。)
			if (ImageView.Image == null && this.TaskSchedule != null)
			{
				foreach (var task in this.TaskSchedule)
				{
					if (task is XIE.Tasks.IxTaskOutputImage)
					{
						((XIE.Tasks.IxTaskOutputImage)task).OutputImage(ImageView, false);
					}
				}
			}
			#endregion

			#region IxTaskOutputImage: (背景画像の更新)
			if (info.FocusedUnit is XIE.Tasks.IxTaskOutputImage)
			{
				((XIE.Tasks.IxTaskOutputImage)info.FocusedUnit).OutputImage(ImageView, true);
			}
			else if (info.FocusedUnit is XIE.Tasks.CxScript)
			{
				var image = this.TaskNode.GetBgImage(info.FocusedUnit);
				if (image != null)
					ImageView.Image = image;
			}
			else if (this.TaskSchedule != null)
			{
				var taskflow = this.TaskNode.Taskflow;
				var index = taskflow.DefaultImageIndex;

				if (0 <= index && index < this.TaskSchedule.Count)
				{
					#region 既定の画像: (DefaultImageIndex で指定されたタスクのみ実行する。)
					var task = this.TaskSchedule[index];
					if (task is XIE.Tasks.IxTaskOutputImage)
					{
						((XIE.Tasks.IxTaskOutputImage)task).OutputImage(ImageView, true);
					}
					else if (task is XIE.Tasks.CxScript)
					{
						var image = this.TaskNode.GetBgImage(task);
						if (image != null)
							ImageView.Image = image;
					}
					#endregion
				}
			}
			#endregion

			if (this.ImagePreviewForm != null)
				this.ImagePreviewForm.Refresh();
		}

		/// <summary>
		/// ImageView: 描画イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			try
			{
				var info = this.TaskNode.Visualizer.HandlingInfo;

				#region IxTaskOverlayRendering:
				if (info.FocusedUnit is XIE.Tasks.IxTaskOverlayRendering)
				{
					var task = (XIE.Tasks.IxTaskOverlayRendering)info.FocusedUnit;
					if (task.EnableRendering)
						task.Rendering(sender, e);
				}
				#endregion
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// ImageView: 描画完了イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_Rendered(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var info = this.TaskNode.Visualizer.HandlingInfo;

			#region IxTaskOverlayRendered:
			if (info.FocusedUnit is XIE.Tasks.IxTaskOverlayRendered)
			{
				((XIE.Tasks.IxTaskOverlayRendered)info.FocusedUnit).Rendered(sender, e);
			}
			#endregion

			if (this.ImagePreviewForm != null)
				this.ImagePreviewForm.Refresh();
		}

		/// <summary>
		/// ImageView: 操作イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_Handling(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			var info = this.TaskNode.Visualizer.HandlingInfo;

			#region IxTaskOverlayHandling:
			if (info.FocusedUnit is XIE.Tasks.IxTaskOverlayHandling)
			{
				((XIE.Tasks.IxTaskOverlayHandling)info.FocusedUnit).Handling(sender, e);
			}
			#endregion
		}

		#endregion

		#region ImageView: (コントロールイベント)

		/// <summary>
		/// ImageView: サイズ変更
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_Resize(object sender, EventArgs e)
		{
			ImageView_Refresh();
		}

		/// <summary>
		/// ImageView: キーボード押下
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			#region [A] 原寸.
			if (e.KeyCode == Keys.A && !e.Alt && !e.Control && !e.Shift)
			{
				ImageView.AdjustScale(0);
				ImageView_Refresh();
			}
			#endregion

			#region [-] 縮小.
			if (e.KeyCode == Keys.OemMinus && !e.Alt && !e.Control && !e.Shift)
			{
				ImageView.AdjustScale(-1);
				ImageView_Refresh();
			}
			#endregion

			#region [+] 拡大.
			if (e.KeyCode == Keys.Oemplus && !e.Alt && !e.Control && !e.Shift)
			{
				ImageView.AdjustScale(+1);
				ImageView_Refresh();
			}
			#endregion

			#region [F] フィット.
			if (e.KeyCode == Keys.F && !e.Alt && !e.Control && !e.Shift)
			{
				ImageView.FitImageSize(0);
				ImageView_Refresh();
			}
			#endregion

			#region [HOME]
			if (e.KeyCode == Keys.Home && !e.Alt && !e.Control && !e.Shift)
			{
				ProfileOverlay_MoveToCursorPosition();
			}
			#endregion

			#region [END]
			if (e.KeyCode == Keys.End && !e.Alt && !e.Control && !e.Shift)
			{
				ProfileOverlay_BackToPreviousPosition();
			}
			#endregion
		}

		/// <summary>
		/// ImageView: マウスでダブルクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.ProfileOverlay.Visible)
			{
				this.ProfileOverlay.IsFixed = !this.ProfileOverlay.IsFixed;
				this.ImageView_Refresh();
			}
		}

		#endregion

		#region ImageView: (Profile 関連)

		/// <summary>
		/// ImageView: Profile のカーソル位置へ始点を移動します。
		/// </summary>
		public void ProfileOverlay_MoveToCursorPosition()
		{
			if (this.ProfileOverlay.Visible && this.ProfileOverlay.IsFixed)
			{
				this.ProfileOverlay_PreviousPosition = this.ImageView.ViewPoint;
				this.ImageView.ViewPoint = this.ProfileOverlay.CursorPosition;
				this.ImageView_Refresh();
			}
		}

		/// <summary>
		/// ImageView: Profile のカーソル位置へ移動する前の位置へ始点を移動します。
		/// </summary>
		public void ProfileOverlay_BackToPreviousPosition()
		{
			if (this.ProfileOverlay.Visible && this.ProfileOverlay.IsFixed)
			{
				this.ImageView.ViewPoint = this.ProfileOverlay_PreviousPosition;
				this.ImageView_Refresh();
			}
		}
		XIE.TxPointD ProfileOverlay_PreviousPosition = new XIE.TxPointD();

		#endregion

		#region ImageView: (Panel 関連)

		/// <summary>
		/// ImageView のコンテナの可視属性が変更したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panelPageImageView_VisibleChanged(object sender, EventArgs e)
		{
			if (this.ImagePreviewForm != null  && this.ImagePreviewForm.Visible)
			{
				this.ImagePreviewForm.EnableVisibleRect = panelPageImageView.Visible;
				this.ImagePreviewForm.Refresh();
			}
		}

		#endregion

		// //////////////////////////////////////////////////
		// TabView
		// //////////////////////////////////////////////////

		#region TabView: (コントロールイベント)

		/// <summary>
		/// TabView: タブが切り替わったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TabView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.ImagePreviewForm != null)
			{
				this.ImagePreviewForm.EnableVisibleRect = ImageView_IsVisible();
				this.ImagePreviewForm.Refresh();
			}
		}

		#endregion

		// //////////////////////////////////////////////////
		// TabTimes
		// //////////////////////////////////////////////////

		#region TabTimes: (初期化と解放)

		/// <summary>
		/// TabTimes: 初期化
		/// </summary>
		private void TabTimes_Setup()
		{
			#region リストビューの初期化:
			{
				var aux = (XIE.Tasks.IxAuxImageList16)CxAuxInfoForm.AuxInfo;
				listTimes.SmallImageList = aux.ImageList;
				listTimes.Columns.Clear();
				var columns = new ColumnHeader[]
				{
					new ColumnHeader() { Text = "Name", Width = 200},
					new ColumnHeader() { Text = "Category", Width = 200},
					new ColumnHeader() { Text = "Count", Width = 50, TextAlign = HorizontalAlignment.Center},
					new ColumnHeader() { Text = "Mean", Width = 100, TextAlign = HorizontalAlignment.Right},
					new ColumnHeader() { Text = "Sigma", Width = 100, TextAlign = HorizontalAlignment.Right},
					new ColumnHeader() { Text = "Min", Width = 100, TextAlign = HorizontalAlignment.Right},
					new ColumnHeader() { Text = "Max", Width = 100, TextAlign = HorizontalAlignment.Right},
					new ColumnHeader() { Text = "Sum", Width = 100, TextAlign = HorizontalAlignment.Right},
					new ColumnHeader() { Text = "Exception", Width = -2, TextAlign = HorizontalAlignment.Left},
				};
				foreach (var column in columns)
					listTimes.Columns.Add(column);
			}
			#endregion
		}

		/// <summary>
		/// TabTimes: 解放
		/// </summary>
		private void TabTimes_TearDown()
		{
		}

		#endregion

		#region TabTimes: (toolbarTimes)

		/// <summary>
		/// toolbarTimes: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTimesSave_Click(object sender, EventArgs e)
		{
			#region ファイル保存.
			try
			{
				#region SaveFileDialog
				var dlg = new SaveFileDialog();
				{
					string filename = ApiHelper.MakeFileNameSuffix(DateTime.Now, false) + ".csv";

					dlg.Filter = "CSV files|*.csv";
					dlg.OverwritePrompt = true;
					dlg.AddExtension = true;
					dlg.FileName = filename;

					if (string.IsNullOrWhiteSpace(CxAuxInfoForm.AppSettings.TaskflowFileDirectory) == false)
						dlg.InitialDirectory = CxAuxInfoForm.AppSettings.TaskflowFileDirectory;
					else if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
						dlg.InitialDirectory = XIE.Tasks.SharedData.ProjectDir;

					if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
						dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);
					if (string.IsNullOrWhiteSpace(CxAuxInfoForm.AppSettings.TaskflowFileDirectory) == false)
						dlg.CustomPlaces.Add(CxAuxInfoForm.AppSettings.TaskflowFileDirectory);
				}
				#endregion

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion
		}

		/// <summary>
		/// toolbarTimes: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTimesAdjust_Click(object sender, EventArgs e)
		{
			listTimes.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
			listTimes.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
			listTimes.Columns[8].Width = -2;
		}

		#endregion

		#region TabTimes: (listTimes)

		/// <summary>
		/// listTimes: 表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listTimes_Update(object sender, EventArgs e)
		{
			if (this.TaskSchedule == null) return;

			double total = 0;

			#region リストビューの更新.
			for (int i = 0; i < this.TaskSchedule.Count; i++)
			{
				var task = this.TaskSchedule[i];
				var laps = this.TaskLaps[task];
				var exception = this.TaskException[task];
				var stat = XIE.TxStatistics.From(laps);
				if (laps.Count == 0)
				{
					stat.Min = 0;
					stat.Max = 0;
				}

				var lvitem = listTimes.Items[i];
				lvitem.SubItems[2].Text = string.Format("{0}", (int)stat.Count);
				lvitem.SubItems[3].Text = string.Format("{0:N3}", stat.Mean);
				lvitem.SubItems[4].Text = string.Format("{0:N3}", stat.Sigma);
				lvitem.SubItems[5].Text = string.Format("{0:N3}", stat.Min);
				lvitem.SubItems[6].Text = string.Format("{0:N3}", stat.Max);
				lvitem.SubItems[7].Text = string.Format("{0:N3}", stat.Sum1);
				lvitem.SubItems[8].Text = (exception == null) ? "" : exception.Message;

				total += stat.Mean;
			}
			#endregion

			toolTimesAdjust_Click(sender, e);

			statusLabelMessage.Text = string.Format("Execute: Lap={0:N3} msec", total);
		}

		/// <summary>
		/// listTimes: 項目の選択状態が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listTimes_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			if (this.TaskSchedule == null) return;

			var info = this.TaskNode.Visualizer.HandlingInfo;

			#region フォーカスタスクを変更して表示更新する.
			if (0 <= e.ItemIndex && e.ItemIndex < this.TaskSchedule.Count)
			{
				info.FocusedUnit = this.TaskSchedule[e.ItemIndex];

				propertyTask.SelectedObject = info.FocusedUnit;
				propertyTask.Refresh();
				ImageView_DrawImage();
				ImageView_Refresh();
				gridReport_Update(sender, e);
			}
			#endregion
		}

		/// <summary>
		/// listTimes: 選択項目指標が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listTimes_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		#endregion

		// //////////////////////////////////////////////////
		// TabReport
		// //////////////////////////////////////////////////

		#region TabReport: (toolbarReport)

		/// <summary>
		/// toolbarReport: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolReportSave_Click(object sender, EventArgs e)
		{
			#region ファイル保存.
			try
			{
				#region SaveFileDialog
				var dlg = new SaveFileDialog();
				{
					string filename = ApiHelper.MakeFileNameSuffix(DateTime.Now, false) + ".csv";

					dlg.Filter = "CSV files|*.csv";
					dlg.OverwritePrompt = true;
					dlg.AddExtension = true;
					dlg.FileName = filename;

					if (string.IsNullOrWhiteSpace(CxAuxInfoForm.AppSettings.TaskflowFileDirectory) == false)
						dlg.InitialDirectory = CxAuxInfoForm.AppSettings.TaskflowFileDirectory;
					else if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
						dlg.InitialDirectory = XIE.Tasks.SharedData.ProjectDir;

					if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
						dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);
					if (string.IsNullOrWhiteSpace(CxAuxInfoForm.AppSettings.TaskflowFileDirectory) == false)
						dlg.CustomPlaces.Add(CxAuxInfoForm.AppSettings.TaskflowFileDirectory);
				}
				#endregion

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion
		}

		/// <summary>
		/// toolbarReport: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolReportAdjust_Click(object sender, EventArgs e)
		{
			if (gridReport.AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.None)
			{
				gridReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
				toolReportAdjust.Checked = true;
			}
			else
			{
				gridReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
				toolReportAdjust.Checked = false;
			}
		}

		/// <summary>
		/// toolbarReport: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboReport_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (gridReport.DataSource is DataSet)
			{
				var ds = (DataSet)gridReport.DataSource;

				// カラムが意図せず入れ替る挙動の対策.
				this.gridReport.DataSource = null;
				this.gridReport.DataSource = ds;

				// データグリッドの表示対象テーブル設定.
				if (0 <= comboReport.SelectedIndex && comboReport.SelectedIndex < ds.Tables.Count)
					this.gridReport.DataMember = ds.Tables[comboReport.SelectedIndex].TableName;

				this.gridReport.Refresh();
			}
		}

		/// <summary>
		/// toolbarReport: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolReportPrev_Click(object sender, EventArgs e)
		{
			if (comboReport.Items.Count == 0) return;

			int index = comboReport.SelectedIndex - 1;
			if (index < 0)
				index = 0;
			comboReport.SelectedIndex = index;
		}

		/// <summary>
		/// toolbarReport: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolReportNext_Click(object sender, EventArgs e)
		{
			if (comboReport.Items.Count == 0) return;

			int index = comboReport.SelectedIndex + 1;
			if (index > comboReport.Items.Count - 1)
				index = comboReport.Items.Count - 1;
			comboReport.SelectedIndex = index;
		}

		/// <summary>
		/// toolbarReport: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolReportProperty_Click(object sender, EventArgs e)
		{
			if (propertyTask.SelectedObject is XIE.Tasks.IxTaskOutputReport)
			{
				try
				{
					var task = (XIE.Tasks.IxTaskOutputReport)propertyTask.SelectedObject;
					var dlg = task.OpenReportPropertyForm();
					if (dlg != null)
					{
						try
						{
							dlg.StartPosition = FormStartPosition.Manual;
							dlg.Location = ApiHelper.GetNeighborPosition(dlg.Size, 1);
							var ans = dlg.ShowDialog(this);

							#region 表示更新:
							{
								ImageView_DrawImage();
								ImageView_Refresh();

								propertyTask.Refresh();
								pictureTaskflow.Refresh();

								gridReport_Update(sender, e);
							}
							#endregion
						}
						finally
						{
							dlg.Dispose();
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		#endregion

		#region TabReport: (gridReport)

		/// <summary>
		/// gridReport: 現在選択されているタスクが IxTaskOutputReport を実装しているとき、DataGrid を表示更新します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gridReport_Update(object sender, EventArgs e)
		{
			var info = this.TaskNode.Visualizer.HandlingInfo;

			#region DataGrid への出力:
			try
			{
				gridReport.DataSource = null;

				if (info.FocusedUnit is XIE.Tasks.IxTaskOutputReport)
				{
					var unit = (XIE.Tasks.IxTaskOutputReport)info.FocusedUnit;
					if (unit.EnableReport)
						unit.OutputReport(gridReport, true);
				}

				object result = gridReport.DataSource;
				comboReport.Enabled = (result != null);
				toolReportPrev.Enabled = (result != null);
				toolReportNext.Enabled = (result != null);
				if (result is DataSet)
				{
					var ds = (DataSet)result;

					// ドロップダウンリストの初期化.
					if (comboReport.Items.Count != ds.Tables.Count)
					{
						comboReport.Items.Clear();
						for (int i = 0; i < ds.Tables.Count; i++)
							comboReport.Items.Add(ds.Tables[i].TableName);
						if (comboReport.Items.Count > 0)
							comboReport.SelectedIndex = 0;
					}
					else
					{
						for (int i = 0; i < ds.Tables.Count; i++)
							comboReport.Items[i] = ds.Tables[i].TableName;
					}

					// データグリッドの表示対象テーブル設定.
					if (0 <= comboReport.SelectedIndex && comboReport.SelectedIndex < ds.Tables.Count)
						this.gridReport.DataMember = ds.Tables[comboReport.SelectedIndex].TableName;

					toolReportPrev.Enabled = (comboReport.Items.Count > 1);
					toolReportNext.Enabled = (comboReport.Items.Count > 1);
				}
				else
				{
					comboReport.Items.Clear();
				}

				gridReport.Refresh();
			}
			catch (Exception ex)
			{
				XIE.Log.Api.Error(ex.StackTrace);
			}
			#endregion
		}

		#endregion

		// //////////////////////////////////////////////////
		// TabConsole
		// //////////////////////////////////////////////////

		#region TabConsole: (初期化と解放)

		private System.IO.TextWriter StandardOutput = null;
		private System.IO.StringWriter SpecialOutputStream = null;
		private StringBuilder SpecialOutputString = null;

		/// <summary>
		/// Console: 初期化
		/// </summary>
		private void Console_Setup()
		{
			if (SpecialOutputString != null) return;
			if (SpecialOutputStream != null) return;

			#region コンソール出力設定:
			StandardOutput = System.Console.Out;
			SpecialOutputString = new StringBuilder();
			SpecialOutputStream = new System.IO.StringWriter(SpecialOutputString);
			System.Console.SetOut(SpecialOutputStream);
			#endregion
		}

		/// <summary>
		/// Console: 解放
		/// </summary>
		private void Console_TearDown()
		{
			#region コンソール出力設定: (復元)
			System.Console.SetOut(StandardOutput);
			if (SpecialOutputStream != null)
				SpecialOutputStream.Dispose();
			SpecialOutputStream = null;
			if (SpecialOutputString != null)
				SpecialOutputString.Length = 0;
			SpecialOutputString = null;
			#endregion
		}

		#endregion

		#region TabConsole: (toolbarConsole)

		/// <summary>
		/// toolbarConsole: クリアボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolConsoleClear_Click(object sender, EventArgs e)
		{
			textConsole.Clear();
		}

		#endregion

		// //////////////////////////////////////////////////
		// propertyTask
		// //////////////////////////////////////////////////

		#region propertyTask: (コントロールイベント)

		/// <summary>
		/// propertyTask: 値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void propertyTask_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			#region 単一のタスクを実行する:
			if (propertyTask.SelectedObject is XIE.Tasks.CxTaskUnit)
			{
				var task = (XIE.Tasks.CxTaskUnit)propertyTask.SelectedObject;
				var args = new XIE.Tasks.CxTaskValueChangedEventArgs(
					this.TaskNode.Taskflow,
					this.TaskNode.AuxInfo,
					this.TaskNode.GetBaseDir(),
					this.TaskNode.Name,
					e.ChangedItem,
					e.OldValue
					);

				try
				{
					var watch = new XIE.CxStopwatch();

					task.Prepare(sender, args);			// 準備.

					watch.Start();
					task.ValueChanged(sender, args);	// 実行.
					watch.Stop();

					task.Restore(sender, args);			// 復旧.

					statusLabelMessage.Text = string.Format("ValueChanged: Elapsed={0:N3} msec", watch.Elapsed);
				}
				catch (System.Exception ex)
				{
					statusLabelMessage.Text = string.Format("ValueChanged: {0}", ex.Message);
				}

				ImageView_DrawImage();
				ImageView_Refresh();
				gridReport_Update(sender, e);

				pictureTaskflow.Refresh();
			}
			#endregion
		}

		/// <summary>
		/// propertyTask: オブジェクトが変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void propertyTask_SelectedObjectsChanged(object sender, EventArgs e)
		{
			// コントロールパネル.
			if (propertyTask.SelectedObject is XIE.Tasks.IxTaskControlPanel)
			{
				var task = (XIE.Tasks.IxTaskControlPanel)propertyTask.SelectedObject;
				toolTaskControlPanel.Enabled = !task.IsOpened;
			}
			else
			{
				toolTaskControlPanel.Enabled = false;
			}

			// レンダリング.
			if (propertyTask.SelectedObject is XIE.Tasks.IxTaskOverlayRendering)
			{
				var task = (XIE.Tasks.IxTaskOverlayRendering)propertyTask.SelectedObject;
				toolRenderingProperty.Enabled = task.HasRenderingPropertyForm;
			}
			else
			{
				toolRenderingProperty.Enabled = false;
			}

			// レポート出力.
			if (propertyTask.SelectedObject is XIE.Tasks.IxTaskOutputReport)
			{
				var task = (XIE.Tasks.IxTaskOutputReport)propertyTask.SelectedObject;
				toolReportProperty.Enabled = task.HasReportPropertyForm;
			}
			else
			{
				toolReportProperty.Enabled = false;
			}
		}

		#endregion

		#region propertyTask: (ツールバー)

		/// <summary>
		/// toolbarProperty: TaskEnableFlags メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskEnableFlags_DropDownOpened(object sender, EventArgs e)
		{
			if (propertyTask.SelectedObject is XIE.Tasks.IxTaskOverlayRendering)
			{
				var task = (XIE.Tasks.IxTaskOverlayRendering)propertyTask.SelectedObject;
				toolTaskEnableRendering.Enabled = true;
				toolTaskEnableRendering.Checked = task.EnableRendering;
			}
			else
			{
				toolTaskEnableRendering.Enabled = false;
				toolTaskEnableRendering.Checked = false;
			}

			if (propertyTask.SelectedObject is XIE.Tasks.IxTaskOutputReport)
			{
				var task = (XIE.Tasks.IxTaskOutputReport)propertyTask.SelectedObject;
				toolTaskEnableReport.Enabled = true;
				toolTaskEnableReport.Checked = task.EnableReport;
			}
			else
			{
				toolTaskEnableReport.Enabled = false;
				toolTaskEnableReport.Checked = false;
			}
		}

		/// <summary>
		/// toolbarProperty: EnableRendering が選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskEnableRendering_Click(object sender, EventArgs e)
		{
			if (propertyTask.SelectedObject is XIE.Tasks.IxTaskOverlayRendering)
			{
				var task = (XIE.Tasks.IxTaskOverlayRendering)propertyTask.SelectedObject;
				task.EnableRendering = !task.EnableRendering;
				ImageView_Refresh();
			}
		}

		/// <summary>
		/// toolbarProperty: EnableReport が選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskEnableReport_Click(object sender, EventArgs e)
		{
			if (propertyTask.SelectedObject is XIE.Tasks.IxTaskOutputReport)
			{
				var task = (XIE.Tasks.IxTaskOutputReport)propertyTask.SelectedObject;
				task.EnableReport = !task.EnableReport;
				gridReport_Update(sender, e);
			}
		}

		/// <summary>
		/// toolbarProperty: Rendering Property が選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolRenderingProperty_Click(object sender, EventArgs e)
		{
			if (propertyTask.SelectedObject is XIE.Tasks.IxTaskOverlayRendering)
			{
				try
				{
					var task = (XIE.Tasks.IxTaskOverlayRendering)propertyTask.SelectedObject;
					var dlg = task.OpenRenderingPropertyForm();
					if (dlg != null)
					{
						try
						{
							dlg.StartPosition = FormStartPosition.Manual;
							dlg.Location = ApiHelper.GetNeighborPosition(dlg.Size, 1);
							var ans = dlg.ShowDialog(this);

							#region 表示更新:
							{
								ImageView_DrawImage();
								ImageView_Refresh();

								propertyTask.Refresh();
								pictureTaskflow.Refresh();

								gridReport_Update(sender, e);
							}
							#endregion
						}
						finally
						{
							dlg.Dispose();
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		/// <summary>
		/// toolbarProperty: Control Panel が選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskControlPanel_Click(object sender, EventArgs e)
		{
			menuTaskBodyControlPanel_Click(sender, e);
		}

		#endregion

		// //////////////////////////////////////////////////
		// Taskflow
		// //////////////////////////////////////////////////

		#region Taskflow: (初期化と解放)

		/// <summary>
		/// Taskflow: 初期化
		/// </summary>
		private void Taskflow_Setup()
		{
			#region タスクフロービューの初期化:
			{
				pictureTaskflow.Location = this.TaskNode.CurrentTaskflow.SheetLocation;
				pictureTaskflow.Size = this.TaskNode.Visualizer.GetSheetSize();
				pictureTaskflow.BackColor = this.TaskNode.Visualizer.SheetColor;
				pictureTaskflow.MouseWheel += pictureTaskflow_MouseWheel;
			}
			#endregion
		}

		/// <summary>
		/// Taskflow: 解放
		/// </summary>
		private void Taskflow_TearDown()
		{
			#region タスクフロービューの解放:
			{
				pictureTaskflow.MouseWheel -= pictureTaskflow_MouseWheel;
			}
			#endregion

			this.TaskNode.ControlPanelManagement_CloseAll();
		}

		#endregion

		#region Taskflow: (ドラッグ＆ドロップ)

		/// <summary>
		/// ドラッグ中のタスクユニット
		/// </summary>
		private XIE.Tasks.CxTaskUnit DragTask = null;

		/// <summary>
		/// Taskflow: ドラッグ項目が領域内に入ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureTaskflow_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("treeToolbox.CxToolboxNode", true))
			{
				if (this.DragTask == null)
				{
					#region タスクユニットを取得する.
					var src_node = (CxToolboxNode)e.Data.GetData("treeToolbox.CxToolboxNode");
					if (src_node.Task is XIE.Tasks.CxTaskFolder)
					{
						var folder = (XIE.Tasks.CxTaskFolder)src_node.Task;
						var task = folder.GetDefaultTask();
						if (task == null) return;
						this.DragTask = (XIE.Tasks.CxTaskUnit)task.Clone();
					}
					else
					{
						this.DragTask = (XIE.Tasks.CxTaskUnit)src_node.Task.Clone();
					}
					#endregion
				}

				e.Effect = DragDropEffects.All;
				return;
			}
			if (e.Data.GetDataPresent("treeOutline.CxToolboxNode", true))
			{
				if (this.DragTask == null)
				{
					#region タスクユニットを取得する.
					var src_node = (CxToolboxNode)e.Data.GetData("treeOutline.CxToolboxNode");
					{
						this.DragTask = new XIE.Tasks.CxTaskReference(src_node.Task);
					}
					#endregion
				}

				e.Effect = DragDropEffects.All;
				return;
			}
		}

		/// <summary>
		/// Taskflow: ドラッグ項目が領域外に出たとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureTaskflow_DragLeave(object sender, EventArgs e)
		{
			this.DragTask = null;
			pictureTaskflow.Refresh();
		}

		/// <summary>
		/// Taskflow: ドラッグ項目が領域上で待機しているとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureTaskflow_DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("treeToolbox.CxToolboxNode", true))
			{
				if (this.DragTask != null)
				{
					// ノードの基準位置を設定する.
					var sp = new Point(e.X, e.Y);
					var cp = pictureTaskflow.PointToClient(sp);
					var ap = this.TaskNode.Visualizer.GetActualPoint(cp);
					this.DragTask.Location = ap;
				}

				if (pictureTaskflow_AutoScroll() == false)
					pictureTaskflow.Refresh();
			}
			if (e.Data.GetDataPresent("treeOutline.CxToolboxNode", true))
			{
				if (this.DragTask != null)
				{
					// ノードの基準位置を設定する.
					var sp = new Point(e.X, e.Y);
					var cp = pictureTaskflow.PointToClient(sp);
					var ap = this.TaskNode.Visualizer.GetActualPoint(cp);
					this.DragTask.Location = ap;
				}

				if (pictureTaskflow_AutoScroll() == false)
					pictureTaskflow.Refresh();
			}
		}

		/// <summary>
		/// Taskflow: ドラッグ項目が領域内にドロップされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureTaskflow_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("treeToolbox.CxToolboxNode", true))
			{
				if (this.DragTask != null)
				{
					// ドロップされたデータを取得する.
					var src_task = this.DragTask;
					this.DragTask = null;

					this.TaskNode.AddHistory();
					this.TaskNode.CurrentTaskflow.TaskUnits.Add(src_task);
					this.TaskNode.CurrentTaskflow.TaskUnits.ForEach(item => item.Selected = false);

					#region タスクの初期化:
					try
					{
						// ノードの基準位置を設定する.
						var sp = new Point(e.X, e.Y);
						var cp = pictureTaskflow.PointToClient(sp);
						var ap = this.TaskNode.Visualizer.GetActualPoint(cp);

						var args = new XIE.Tasks.CxTaskSetupEventArgs(
							this.TaskNode.Taskflow,
							this.TaskNode.AuxInfo,
							this.TaskNode.GetBaseDir(),
							this.TaskNode.Name
							);

						src_task.Location = ap;
						src_task.Setup(sender, args, this.TaskNode.CurrentTaskflow);
						statusLabelMessage.Text = "";
					}
					catch (System.Exception ex)
					{
						statusLabelMessage.Text = string.Format("Setup: {0}", ex.Message);
					}
					#endregion

					this.TaskNode.Visualizer.HandlingInfo.Update(src_task, new TxTaskUnitPosition(), MouseButtons.None, src_task.Location);
					this.propertyTask.SelectedObject = src_task;
					this.pictureTaskflow.Refresh();
					this.Outline_Update();
				}
				return;
			}
			if (e.Data.GetDataPresent("treeOutline.CxToolboxNode", true))
			{
				if (this.DragTask != null)
				{
					// ドロップされたデータを取得する.
					var src_task = this.DragTask;
					this.DragTask = null;

					this.TaskNode.AddHistory();
					this.TaskNode.CurrentTaskflow.TaskUnits.Add(src_task);
					this.TaskNode.CurrentTaskflow.TaskUnits.ForEach(item => item.Selected = false);

					#region タスクの初期化:
					try
					{
						// ノードの基準位置を設定する.
						var sp = new Point(e.X, e.Y);
						var cp = pictureTaskflow.PointToClient(sp);
						var ap = this.TaskNode.Visualizer.GetActualPoint(cp);

						var args = new XIE.Tasks.CxTaskSetupEventArgs(
							this.TaskNode.Taskflow,
							this.TaskNode.AuxInfo,
							this.TaskNode.GetBaseDir(),
							this.TaskNode.Name
							);

						src_task.Location = ap;
						src_task.Setup(sender, args, this.TaskNode.CurrentTaskflow);
						statusLabelMessage.Text = "";
					}
					catch (System.Exception ex)
					{
						statusLabelMessage.Text = string.Format("Setup: {0}", ex.Message);
					}
					#endregion

					this.TaskNode.Visualizer.HandlingInfo.Update(src_task, new TxTaskUnitPosition(), MouseButtons.None, src_task.Location);
					this.propertyTask.SelectedObject = src_task;
					this.pictureTaskflow.Refresh();
					this.Outline_Update();
				}
				return;
			}
		}

		#endregion

		#region Taskflow: (描画処理)

		/// <summary>
		/// Taskflow: 再描画イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureTaskflow_Paint(object sender, PaintEventArgs e)
		{
			if (this.pictureTaskflow.BackColor != this.TaskNode.Visualizer.SheetColor)
				this.pictureTaskflow.BackColor = this.TaskNode.Visualizer.SheetColor;

			var graphics = e.Graphics;

			var mag = (float)this.TaskNode.Visualizer.Scale / 100;
			graphics.ResetTransform();
			graphics.ScaleTransform(mag, mag);

			#region グリッド:
			if (mag > 0.5f)
			{
				Pen pen = new Pen(this.TaskNode.Visualizer.GridColor);
				pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
				int height = (int)(pictureTaskflow.Height / mag);
				int width = (int)(pictureTaskflow.Width / mag);
				Point st = new Point();
				Point ed = new Point();
				for (int y = 0; y < height; y += 32)
				{
					st.X = 0;
					st.Y = y;
					ed.X = width;
					ed.Y = y;
					graphics.DrawLine(pen, st, ed);
				}
				for (int x = 0; x < width; x += 32)
				{
					st.X = x;
					st.Y = 0;
					ed.X = x;
					ed.Y = height;
					graphics.DrawLine(pen, st, ed);
				}
			}
			#endregion

			#region タスクユニットの描画:
			{
				foreach (var task in this.TaskNode.CurrentTaskflow.TaskUnits)
				{
					try
					{
						this.TaskNode.Visualizer.Render(graphics, task);
					}
					catch (Exception ex)
					{
						XIE.Log.Api.Error(ex.Message);
					}
				}
			}
			#endregion

			#region 移動先の候補の描画: (赤色の点線矩形)
			if (this.TaskNode.Visualizer.HandlingInfo.GripPosition.Type == ExTaskUnitPositionType.Body)
			{
				if (this.TaskNode.Visualizer.HandlingInfo.Button == MouseButtons.Left)
				{
					using (Pen pen = new Pen(Color.Red, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot })
					{
						foreach (var task in this.TaskNode.CurrentTaskflow.TaskUnits)
						{
							if (task == null) continue;
							if (task.Selected)
							{
								var mouse_position = pictureTaskflow.PointToClient(Control.MousePosition);
								var figure_position = this.TaskNode.Visualizer.HandlingInfo.Location;

								var diff = new Point();
								diff.X = (int)((mouse_position.X - figure_position.X) / mag);
								diff.Y = (int)((mouse_position.Y - figure_position.Y) / mag);

								var bounds = this.TaskNode.Visualizer.GetBounds(task);
								bounds.X += diff.X;
								bounds.Y += diff.Y;

								graphics.DrawRectangle(pen, bounds);
							}
						}
					}
				}
			}
			#endregion

			#region 接続線の候補の描画: (橙色の線分)
			if (this.TaskNode.Visualizer.HandlingInfo.GripPosition.Type == ExTaskUnitPositionType.ControlOut)
			{
				if (this.TaskNode.Visualizer.HandlingInfo.Button == MouseButtons.Left)
				{
					var mouse_position = pictureTaskflow.PointToClient(MousePosition);

					#region 始点: 出力ポート.
					var src_unit = this.TaskNode.Visualizer.HandlingInfo.FocusedUnit;
					var src_grip = new TxTaskUnitPosition(
						this.TaskNode.Visualizer.HandlingInfo.GripPosition.Type,
						this.TaskNode.Visualizer.HandlingInfo.GripPosition.Index
						);
					var src_rects = this.TaskNode.Visualizer.GetPortRects(src_unit, src_grip.Type);
					var src_rect = src_rects[src_grip.Index];
					#endregion

					#region 終点: 入力ポート.
					var dst_grip = new TxTaskUnitPosition();
					var dst_unit = this.TaskNode.CurrentTaskflow.TaskUnits.Find(
						delegate(XIE.Tasks.CxTaskUnit task)
						{
							dst_grip = this.TaskNode.Visualizer.HitTest(task, mouse_position);
							return (dst_grip.Type != ExTaskUnitPositionType.None);
						});
					switch (dst_grip.Type)
					{
						default:
							using (Pen pen = new Pen(Color.Orange, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
							{
								var st = new Point((src_rect.Left + src_rect.Right) / 2, (src_rect.Top + src_rect.Bottom) / 2);
								var ed = this.TaskNode.Visualizer.GetActualPoint(mouse_position);
								graphics.DrawLine(pen, st, ed);

								var w = this.TaskNode.Visualizer.BlockSize.Width - (this.TaskNode.Visualizer.PortSize.Width / 2);
								var h = this.TaskNode.Visualizer.BlockSize.Height - (this.TaskNode.Visualizer.PortSize.Height / 2);
								var er = (float)System.Math.Sqrt(w * w + h * h);
								var ellipse = new RectangleF(ed.X - er, ed.Y - er, er * 2, er * 2);
								graphics.DrawEllipse(pen, ellipse);
							}
							break;
						case ExTaskUnitPositionType.ControlIn:
							using (Pen pen = new Pen(this.TaskNode.Visualizer.ControlInColor, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
							{
								var dst_rects = this.TaskNode.Visualizer.GetPortRects(dst_unit, dst_grip.Type);
								var dst_rect = dst_rects[dst_grip.Index];
								var st = new Point((src_rect.Left + src_rect.Right) / 2, (src_rect.Top + src_rect.Bottom) / 2);
								var ed = new Point((dst_rect.Left + dst_rect.Right) / 2, (dst_rect.Top + dst_rect.Bottom) / 2);
								graphics.DrawLine(pen, st, ed);
							}
							break;
					}
					#endregion
				}
			}
			else if (this.TaskNode.Visualizer.HandlingInfo.GripPosition.Type == ExTaskUnitPositionType.DataOut)
			{
				if (this.TaskNode.Visualizer.HandlingInfo.Button == MouseButtons.Left)
				{
					var mouse_position = pictureTaskflow.PointToClient(MousePosition);

					#region 始点: 出力ポート.
					var src_unit = this.TaskNode.Visualizer.HandlingInfo.FocusedUnit;
					var src_grip = new TxTaskUnitPosition(
						this.TaskNode.Visualizer.HandlingInfo.GripPosition.Type,
						this.TaskNode.Visualizer.HandlingInfo.GripPosition.Index
						);
					var src_rects = this.TaskNode.Visualizer.GetPortRects(src_unit, src_grip.Type);
					var src_rect = src_rects[src_grip.Index];
					#endregion

					#region 終点: 入力ポート.
					var dst_grip = new TxTaskUnitPosition();
					var dst_unit = this.TaskNode.CurrentTaskflow.TaskUnits.Find(
						delegate(XIE.Tasks.CxTaskUnit task)
						{
							dst_grip = this.TaskNode.Visualizer.HitTest(task, mouse_position);
							return (dst_grip.Type != ExTaskUnitPositionType.None);
						});
					switch (dst_grip.Type)
					{
						default:
							using (Pen pen = new Pen(Color.Orange, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
							{
								var st = new Point((src_rect.Left + src_rect.Right) / 2, (src_rect.Top + src_rect.Bottom) / 2);
								var ed = this.TaskNode.Visualizer.GetActualPoint(mouse_position);
								graphics.DrawLine(pen, st, ed);

								var w = this.TaskNode.Visualizer.BlockSize.Width - (this.TaskNode.Visualizer.PortSize.Width / 2);
								var h = this.TaskNode.Visualizer.BlockSize.Height - (this.TaskNode.Visualizer.PortSize.Height / 2);
								var er = (float)System.Math.Sqrt(w * w + h * h);
								var ellipse = new RectangleF(ed.X - er, ed.Y - er, er * 2, er * 2);
								graphics.DrawEllipse(pen, ellipse);
							}
							break;
						case ExTaskUnitPositionType.DataIn:
							using (Pen pen = new Pen(this.TaskNode.Visualizer.DataInColor, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
							{
								var dst_rects = this.TaskNode.Visualizer.GetPortRects(dst_unit, dst_grip.Type);
								var dst_rect = dst_rects[dst_grip.Index];
								var st = new Point((src_rect.Left + src_rect.Right) / 2, (src_rect.Top + src_rect.Bottom) / 2);
								var ed = new Point((dst_rect.Left + dst_rect.Right) / 2, (dst_rect.Top + dst_rect.Bottom) / 2);
								graphics.DrawLine(pen, st, ed);
							}
							break;
						case ExTaskUnitPositionType.DataParam:
							using (Pen pen = new Pen(this.TaskNode.Visualizer.DataParamColor, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
							{
								var dst_rects = this.TaskNode.Visualizer.GetPortRects(dst_unit, dst_grip.Type);
								var dst_rect = dst_rects[dst_grip.Index];
								var st = new Point((src_rect.Left + src_rect.Right) / 2, (src_rect.Top + src_rect.Bottom) / 2);
								var ed = new Point((dst_rect.Left + dst_rect.Right) / 2, (dst_rect.Top + dst_rect.Bottom) / 2);
								graphics.DrawLine(pen, st, ed);
							}
							break;
					}
					#endregion
				}
			}
			#endregion

			#region ドラッグ中のタスクユニットの描画:
			if (this.DragTask != null)
			{
				try
				{
					this.TaskNode.Visualizer.Render(graphics, this.DragTask);
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.StackTrace);
				}
			}
			#endregion
		}

		#endregion

		#region Taskflow: (印刷処理)

		/// <summary>
		/// Taskflow: 印刷イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void printTaskflow_PrintPage(object sender, PrintPageEventArgs e)
		{
			var graphics = e.Graphics;

			int page_no = this.TaskNode.Document_PageNo;
			int page_num = this.TaskNode.Document_Taskflows.Count;
			if (0 <= page_no && page_no < page_num)
			{
				var taskflow = this.TaskNode.Document_Taskflows[page_no];

				var visualizer = new CxTaskVisualizer();
				visualizer.SetupForPrinting(this.TaskNode.Visualizer);

				#region 表示倍率の調整:
				{
					// 注) 実際に印刷するまで graphics に設定が反映されないらしい.
					//     graphics.VisibleClipBounds は e.PageBounds と一致しない時がある.
					//

					var bounds = e.PageBounds;
					var scale = 100 / this.TaskNode.Visualizer.Scale;

					float mag_x = (float)bounds.Width / (this.pictureTaskflow.Width * scale);
					float mag_y = (float)bounds.Height / (this.pictureTaskflow.Height * scale);
					float mag = System.Math.Min(mag_x, mag_y);
					if (mag > 1)
						mag = 1;

					graphics.ResetTransform();
					graphics.ScaleTransform(mag, mag);
				}
				#endregion

				#region タスクユニットの描画:
				foreach (var task in taskflow.TaskUnits)
				{
					try
					{
						visualizer.Render(graphics, task);
					}
					catch (Exception ex)
					{
						XIE.Log.Api.Error(ex.StackTrace);
					}
				}
				#endregion

				#region ヘッダーの描画:
				using (var font = new Font(visualizer.FontName, 12, FontStyle.Regular))
				using (var brush = new SolidBrush(visualizer.TextColor))
				{
					var header = string.Format("[{0}/{1}] {2}",
						page_no + 1,
						page_num,
						this.TaskNode.Document_Titles[page_no]
						);

					graphics.DrawString(header, font, brush, new PointF(0, 0));
				}
				#endregion

				page_no++;

				if (page_no < page_num)
				{
					this.TaskNode.Document_PageNo = page_no;
					e.HasMorePages = true;	// 継続.
					return;
				}
			}

			e.HasMorePages = false;	// ページ終端.
		}


		#endregion

		#region Taskflow: (指定タスク位置への移動)

		/// <summary>
		/// 指定されたタスクを可視範囲内に表示します。
		/// </summary>
		/// <param name="task">表示対象のタスク</param>
		private void pictureTaskflow_EnsureVisible(XIE.Tasks.CxTaskUnit task)
		{
			var info = this.TaskNode.Visualizer.HandlingInfo;

			// 指定されたタスクの親を探す.
			var parent = this.TaskNode.FindParentTaskflow(task);
			if (parent == null) return;

			#region 親が現在のシートでなければ切り替える.
			if (!ReferenceEquals(parent, this.TaskNode.CurrentTaskflow))
			{
				this.TaskNode.AddHistory(false);

				this.TaskNode.Taskflow.SetCurrentTaskflow(parent);
				this.pictureTaskflow.Location = parent.SheetLocation;
			}
			#endregion

			#region 指定されたタスクの位置へ移動する.
			int index = parent.TaskUnits.FindIndex(item => ReferenceEquals(item, task));
			if (0 <= index)
			{
				var scale = this.TaskNode.Visualizer.Scale;
				var location = task.Location;
				location.X = location.X * scale / 100;
				location.Y = location.Y * scale / 100;
				var container = this.panelTaskflow;
				var picturebox = this.pictureTaskflow;

				if (container.Width < picturebox.Width)
				{
					int tx = location.X - (container.Width / 2);
					if (tx > (picturebox.Width - container.Width))
						tx = (picturebox.Width - container.Width);
					if (tx < 0)
						tx = 0;
					location.X = -tx;
				}
				else
				{
					location.X = 0;
				}

				if (container.Height < picturebox.Height)
				{
					int ty = location.Y - (container.Height / 2);
					if (ty > (picturebox.Height - container.Height))
						ty = (picturebox.Height - container.Height);
					if (ty < 0)
						ty = 0;
					location.Y = -ty;
				}
				else
				{
					location.Y = 0;
				}

				picturebox.Location = location;
			}
			#endregion

			#region 指定されたタスクにフォーカスを移動する.
			info.FocusedUnit = task;
			propertyTask.SelectedObject = info.FocusedUnit;
			propertyTask.Refresh();
			ImageView_DrawImage();
			ImageView_Refresh();
			gridReport_Update(this, EventArgs.Empty);
			#endregion

			pictureTaskflow.Refresh();
			treeOutline.Refresh();
		}

		#endregion

		#region Taskflow: (自動スクロール)

		/// <summary>
		/// 前回オートスクロールした時刻
		/// </summary>
		private DateTime AutoScroll_PreviousTime = DateTime.Now;

		/// <summary>
		/// マウスが画面端で待機している場合に自動的にスクロールする処理
		/// </summary>
		/// <returns>
		///		移動した場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		private bool pictureTaskflow_AutoScroll()
		{
			if (AutoScroll_PreviousTime < this.TaskNode.Visualizer.HandlingInfo.TimeStamp)
			{
				AutoScroll_PreviousTime = this.TaskNode.Visualizer.HandlingInfo.TimeStamp;
				return false;
			}

			var now = DateTime.Now;
			var span = now - AutoScroll_PreviousTime;
			if (span.TotalMilliseconds < 100) return false;

			try
			{
				var scale = this.TaskNode.Visualizer.Scale;
				var margin = (72 * scale / 100);
				var movement = (16 * scale / 100);
				var container = this.panelTaskflow;
				var picturebox = this.pictureTaskflow;

				int tx = picturebox.Location.X;
				int ty = picturebox.Location.Y;
				{
					var st = new Point();
					st.X = -(picturebox.Location.X);
					st.Y = -(picturebox.Location.Y);

					var ed = new Point();
					ed.X = -(picturebox.Location.X - container.Size.Width);
					ed.Y = -(picturebox.Location.Y - container.Size.Height);

					var mp = picturebox.PointToClient(Control.MousePosition);

					if (mp.X < (st.X + margin))
						tx += movement;
					if (mp.X > (ed.X - margin))
						tx -= movement;
					if (tx < -(picturebox.Width - container.Width))
						tx = -(picturebox.Width - container.Width);
					if (tx > 0)
						tx = 0;

					if (mp.Y < (st.Y + margin))
						ty += movement;
					if (mp.Y > (ed.Y - margin))
						ty -= movement;
					if (ty < -(picturebox.Height - container.Height))
						ty = -(picturebox.Height - container.Height);
					if (ty > 0)
						ty = 0;
				}

				if (picturebox.Location.X != tx ||
					picturebox.Location.Y != ty)
				{
					AutoScroll_PreviousTime = DateTime.Now;
					picturebox.Location = new Point(tx, ty);
					return true;
				}
			}
			catch (System.Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			return false;
		}

		#endregion

		#region Taskflow: (マウス操作)

		/// <summary>
		/// Taskflow: マウスがダブルクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureTaskflow_DoubleClick(object sender, EventArgs e)
		{
			var ctrl = (Control.ModifierKeys & Keys.Control) == Keys.Control;
			var shift = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
			var alt = (Control.ModifierKeys & Keys.Alt) == Keys.Alt;

			pictureTaskflow.Focus();

			if (!ctrl && !shift && !alt)
			{
				#region Enter / Exit:
				{
					var mouse_position = pictureTaskflow.PointToClient(Control.MousePosition);
					var grip_pos = new TxTaskUnitPosition();
					var focused_unit = this.TaskNode.CurrentTaskflow.TaskUnits.Find(
						delegate(XIE.Tasks.CxTaskUnit task)
						{
							grip_pos = this.TaskNode.Visualizer.HitTest(task, mouse_position);
							return (grip_pos.Type != ExTaskUnitPositionType.None);
						});

					var info = this.TaskNode.Visualizer.HandlingInfo;
					switch (grip_pos.Type)
					{
						case ExTaskUnitPositionType.ControlIn:
						case ExTaskUnitPositionType.ControlOut:
						case ExTaskUnitPositionType.DataIn:
						case ExTaskUnitPositionType.DataParam:
						case ExTaskUnitPositionType.DataOut:
							break;
						case ExTaskUnitPositionType.Body:
							{
								menuTaskBodyEnter_Click(sender, e);
							}
							break;
						default:
						case ExTaskUnitPositionType.None:
							{
								menuTaskflowQuit_Click(sender, e);
							}
							break;
					}
					return;
				}
				#endregion
			}
		}

		/// <summary>
		/// Taskflow: マウスが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureTaskflow_MouseDown(object sender, MouseEventArgs e)
		{
			var ctrl = (Control.ModifierKeys & Keys.Control) == Keys.Control;
			var shift = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
			var alt = (Control.ModifierKeys & Keys.Alt) == Keys.Alt;

			pictureTaskflow.Focus();

			if (!ctrl && !shift && !alt && e.Button == MouseButtons.Left)
			{
				#region ユニットの移動 または ポートの接続:
				{
					var grip_pos = new TxTaskUnitPosition();
					var focused_unit = this.TaskNode.CurrentTaskflow.TaskUnits.FindLast(
						delegate(XIE.Tasks.CxTaskUnit task)
						{
							grip_pos = this.TaskNode.Visualizer.HitTest(task, e.Location);
							return (grip_pos.Type != ExTaskUnitPositionType.None);
						});

					var info = this.TaskNode.Visualizer.HandlingInfo;
					switch (grip_pos.Type)
					{
						default:
						case ExTaskUnitPositionType.None:
						case ExTaskUnitPositionType.ControlIn:
						case ExTaskUnitPositionType.ControlOut:
						case ExTaskUnitPositionType.DataIn:
						case ExTaskUnitPositionType.DataParam:
						case ExTaskUnitPositionType.DataOut:
							{
								this.TaskNode.CurrentTaskflow.TaskUnits.ForEach(item => item.Selected = false);
								info.Update(focused_unit, grip_pos, e.Button, e.Location);

								propertyTask.SelectedObject = focused_unit;

								var image = this.TaskNode.GetBgImage(focused_unit, grip_pos);
								if (image != null)
								{
									this.ImageView.Image = image;
									this.ImageView.FitImageSize(0);
								}
								else
								{
									ImageView_DrawImage();
								}

								gridReport_Update(sender, e);
							}
							break;
						case ExTaskUnitPositionType.Body:
							{
								if (focused_unit.Selected == false)
									this.TaskNode.CurrentTaskflow.TaskUnits.ForEach(item => item.Selected = false);
								info.Update(focused_unit, grip_pos, e.Button, e.Location);
								focused_unit.Selected = true;

								propertyTask.SelectedObject = focused_unit;
								ImageView_DrawImage();
								gridReport_Update(sender, e);
							}
							break;
					}

					propertyTask.Refresh();
					ImageView_Refresh();
					pictureTaskflow.Refresh();

					return;
				}
				#endregion
			}

			if (ctrl && !shift && !alt && e.Button == MouseButtons.Left)
			{
				#region ユニットの複数選択:
				{
					var grip_pos = new TxTaskUnitPosition();
					var focus_unit = this.TaskNode.CurrentTaskflow.TaskUnits.FindLast(
						delegate(XIE.Tasks.CxTaskUnit task)
						{
							grip_pos = this.TaskNode.Visualizer.HitTest(task, e.Location);
							return (grip_pos.Type != ExTaskUnitPositionType.None);
						});

					var info = this.TaskNode.Visualizer.HandlingInfo;
					switch (grip_pos.Type)
					{
						default:
						case ExTaskUnitPositionType.None:
						case ExTaskUnitPositionType.ControlIn:
						case ExTaskUnitPositionType.ControlOut:
						case ExTaskUnitPositionType.DataIn:
						case ExTaskUnitPositionType.DataParam:
						case ExTaskUnitPositionType.DataOut:
							{
								// 何もせず画像ビューやプロパティグリッドの状態を維持する.
							}
							break;
						case ExTaskUnitPositionType.Body:
							{
								info.Update(focus_unit, grip_pos, e.Button, e.Location);
								focus_unit.Selected = true;

								propertyTask.SelectedObject = focus_unit;
								ImageView_DrawImage();
								gridReport_Update(sender, e);
							}
							break;
					}

					propertyTask.Refresh();
					ImageView_Refresh();
					pictureTaskflow.Refresh();

					return;
				}
				#endregion
			}

			if (!ctrl && !shift && !alt && e.Button == MouseButtons.Right)
			{
				#region コンテキストメニュー表示:
				{
					var location = pictureTaskflow.PointToScreen(e.Location);
					var grip_pos = new TxTaskUnitPosition();
					var focus_unit = this.TaskNode.CurrentTaskflow.TaskUnits.Find(
						delegate(XIE.Tasks.CxTaskUnit task)
						{
							grip_pos = this.TaskNode.Visualizer.HitTest(task, e.Location);
							return (grip_pos.Type != ExTaskUnitPositionType.None);
						});

					var info = this.TaskNode.Visualizer.HandlingInfo;
					switch (grip_pos.Type)
					{
						default:
						case ExTaskUnitPositionType.None:
							{
								info.Update(focus_unit, grip_pos, e.Button, e.Location);
								menuTaskflow.Show(location.X, location.Y);
							}
							break;
						case ExTaskUnitPositionType.ControlIn:
							{
								info.Update(focus_unit, grip_pos, e.Button, e.Location);
								menuTaskPortIn.Show(location.X, location.Y);
							}
							break;
						case ExTaskUnitPositionType.ControlOut:
							{
								info.Update(focus_unit, grip_pos, e.Button, e.Location);
								menuTaskPortOut.Show(location.X, location.Y);
							}
							break;
						case ExTaskUnitPositionType.DataIn:
							{
								info.Update(focus_unit, grip_pos, e.Button, e.Location);
								menuTaskPortIn.Show(location.X, location.Y);
							}
							break;
						case ExTaskUnitPositionType.DataParam:
							{
								info.Update(focus_unit, grip_pos, e.Button, e.Location);
								menuTaskPortIn.Show(location.X, location.Y);
							}
							break;
						case ExTaskUnitPositionType.DataOut:
							{
								info.Update(focus_unit, grip_pos, e.Button, e.Location);
								menuTaskPortOut.Show(location.X, location.Y);
							}
							break;
						case ExTaskUnitPositionType.Body:
							{
								info.Update(focus_unit, grip_pos, e.Button, e.Location);
								focus_unit.Selected = true;
								menuTaskBody.Show(location.X, location.Y);
							}
							break;
					}

					propertyTask.SelectedObject = focus_unit;
					propertyTask.Refresh();
					ImageView_DrawImage();
					ImageView_Refresh();
					gridReport_Update(sender, e);

					return;
				}
				#endregion
			}
		}

		/// <summary>
		/// Taskflow: マウスが移動したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureTaskflow_MouseMove(object sender, MouseEventArgs e)
		{
			var info = this.TaskNode.Visualizer.HandlingInfo;

			if (info.GripPosition.Type == ExTaskUnitPositionType.None)
			{
				#region 背景のスクロール:
				if (info.Button == MouseButtons.Left)
				{
					var container = this.panelTaskflow;
					var picturebox = this.pictureTaskflow;
					var location = picturebox.Location;

					if (container.Width < picturebox.Width)
					{
						int tx = location.X + (e.X - info.Location.X);
						if (tx < -(picturebox.Width - container.Width))
							tx = -(picturebox.Width - container.Width);
						if (tx > 0)
							tx = 0;
						location.X = tx;
					}
					else
					{
						location.X = 0;
					}

					if (container.Height < picturebox.Height)
					{
						int ty = location.Y + (e.Y - info.Location.Y);
						if (ty < -(picturebox.Height - container.Height))
							ty = -(picturebox.Height - container.Height);
						if (ty > 0)
							ty = 0;
						location.Y = ty;
					}
					else
					{
						location.Y = 0;
					}

					this.TaskNode.CurrentTaskflow.SheetLocation = location;
					picturebox.Location = location;
					picturebox.Refresh();
				}
				#endregion
			}
		}

		/// <summary>
		/// Taskflow: マウスのクリックが解除されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureTaskflow_MouseUp(object sender, MouseEventArgs e)
		{
			var ctrl = (Control.ModifierKeys & Keys.Control) == Keys.Control;
			var shift = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
			var alt = (Control.ModifierKeys & Keys.Alt) == Keys.Alt;

			var info = this.TaskNode.Visualizer.HandlingInfo;

			#region 本体がグリップされているとき.
			if (info.GripPosition.Type == ExTaskUnitPositionType.Body)
			{
				if (info.Button == MouseButtons.Left)
				{
					var diff = new Point();
					diff.X = e.X - info.Location.X;
					diff.Y = e.Y - info.Location.Y;

					if (info.FocusedUnit != null && 
						info.SelectedPrev &&
						diff.X == 0 &&
						diff.Y == 0)
					{
						#region 選択済みを CTRL+Down した時、移動量 0,0 ならば選択を解除する:
						if (ctrl && !shift && !alt)
						{
							info.FocusedUnit.Selected = false;
							info.FocusedUnit = null;
							info.SelectedPrev = false;
						}
						#endregion
					}
					else if (diff.X != 0 || diff.Y != 0)
					{
						#region 選択されたタスクユニットを移動する: (実行中は不可)
						if (this.TaskNode.IsRunning == false)
						{
							this.TaskNode.AddHistory();

							foreach (var task in this.TaskNode.CurrentTaskflow.TaskUnits)
							{
								if (task.Selected)
								{
									var mag = (double)this.TaskNode.Visualizer.Scale / 100;
									var location = task.Location;
									location.X += (int)(diff.X / mag);
									location.Y += (int)(diff.Y / mag);
									task.Location = location;
								}
							}
						}
						#endregion
					}
				}
			}
			#endregion

			#region コントロール出力がグリップされているとき.
			if (info.GripPosition.Type == ExTaskUnitPositionType.ControlOut)
			{
				if (info.Button == MouseButtons.Left)
				{
					#region ポートの接続: (実行中は不可)
					if (this.TaskNode.IsRunning == false)
					{
						this.TaskNode.AddHistory();

						var src_unit = info.FocusedUnit;
						foreach (var dst_unit in this.TaskNode.CurrentTaskflow.TaskUnits)
						{
							TxTaskUnitPosition pos = this.TaskNode.Visualizer.HitTest(dst_unit, e.Location);
							if (pos.Type == ExTaskUnitPositionType.ControlIn)
							{
								//if (dst_unit.ControlIn.CanConnect(src_unit, ExOutputPortType.ControlOut, 0))
								{
									dst_unit.ControlIn.Connect(src_unit, XIE.Tasks.ExOutputPortType.ControlOut, 0);
									break;
								}
							}
						}
					}
					#endregion
				}
			}
			#endregion

			#region データ出力がグリップされているとき.
			if (info.GripPosition.Type == ExTaskUnitPositionType.DataOut)
			{
				if (info.Button == MouseButtons.Left)
				{
					#region ポートの接続: (実行中は不可)
					if (this.TaskNode.IsRunning == false)
					{
						this.TaskNode.AddHistory();

						var src_unit = info.FocusedUnit;
						foreach (var dst_unit in this.TaskNode.CurrentTaskflow.TaskUnits)
						{
							TxTaskUnitPosition pos = this.TaskNode.Visualizer.HitTest(dst_unit, e.Location);
							if (pos.Type == ExTaskUnitPositionType.DataIn)
							{
								//if (dst_unit.DataIn[pos.Index].CanConnect(src_unit, ExOutputPortType.DataOut, info.GripPosition.Index))
								{
									dst_unit.DataIn[pos.Index].Connect(src_unit, XIE.Tasks.ExOutputPortType.DataOut, info.GripPosition.Index);
									break;
								}
							}
							if (pos.Type == ExTaskUnitPositionType.DataParam)
							{
								//if (dst_unit.DataParam[pos.Index].CanConnect(src_unit, ExOutputPortType.DataOut, info.GripPosition.Index))
								{
									dst_unit.DataParam[pos.Index].Connect(src_unit, XIE.Tasks.ExOutputPortType.DataOut, info.GripPosition.Index);
									break;
								}
							}
						}
					}
					#endregion
				}
			}
			#endregion

			this.TaskNode.Visualizer.HandlingInfo.Button = MouseButtons.None;
			pictureTaskflow.Refresh();
		}

		/// <summary>
		/// Taskflow: マウスポインタが領域外に出たとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureTaskflow_MouseLeave(object sender, EventArgs e)
		{
			this.TaskNode.Visualizer.HandlingInfo.Button = MouseButtons.None;
		}

		/// <summary>
		/// Taskflow: マウスホイールが操作されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureTaskflow_MouseWheel(object sender, MouseEventArgs e)
		{
			var ctrl = (Control.ModifierKeys & Keys.Control) == Keys.Control;
			var shift = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
			var alt = (Control.ModifierKeys & Keys.Alt) == Keys.Alt;

			#region 背景の垂直スクロール:
			if (!ctrl && !shift && !alt)
			{
				var container = this.panelTaskflow;
				var picturebox = this.pictureTaskflow;
				var location = picturebox.Location;

				if (container.Height < picturebox.Height)
				{
					int ty = location.Y + e.Delta;
					if (ty < -(picturebox.Height - container.Height))
						ty = -(picturebox.Height - container.Height);
					if (ty > 0)
						ty = 0;
					location.Y = ty;
				}

				picturebox.Location = location;
			}
			#endregion

			#region 背景の水平スクロール:
			if (!ctrl && shift && !alt)
			{
				var container = this.panelTaskflow;
				var picturebox = this.pictureTaskflow;
				var location = picturebox.Location;

				if (container.Width < picturebox.Width)
				{
					int tx = location.X + e.Delta;
					if (tx < -(picturebox.Width - container.Width))
						tx = -(picturebox.Width - container.Width);
					if (tx > 0)
						tx = 0;
					location.X = tx;
				}

				picturebox.Location = location;
			}
			#endregion

			#region 縮小/拡大:
			if (ctrl && !shift && !alt)
			{
				var scale = pictureTaskflow_CalcScale((e.Delta < 0) ? -1 : +1);
				pictureTaskflow_AdjustSize(scale);
				this.pictureTaskflow.Refresh();
			}
			#endregion
		}

		/// <summary>
		/// 表示倍率調整。(縮小または拡大します。)
		/// </summary>
		/// <param name="mode">0:原寸、-1:縮小、+1:拡大</param>
		/// <returns>
		/// </returns>
		private int pictureTaskflow_CalcScale(int mode)
		{
			var scale = this.TaskNode.Visualizer.Scale;
			switch (mode)
			{
				case 0:
					scale = 100;
					break;
				case -1:
					scale -= 10;
					if (scale < 10)
						scale = 10;
					break;
				case +1:
					scale += 10;
					if (scale > 100)
						scale = 100;
					break;
			}
			return scale;
		}

		/// <summary>
		/// 表示倍率設定とピクチャボックスの位置調整
		/// </summary>
		/// <param name="scale">表示倍率 [0 より大きい値 (10 刻み)]</param>
		private void pictureTaskflow_AdjustSize(int scale)
		{
			var container = this.panelTaskflow;
			var picturebox = this.pictureTaskflow;
			var location = picturebox.Location;

			this.TaskNode.Visualizer.Scale = scale;
			var size = this.TaskNode.Visualizer.GetSheetSize();

			if (container.Width < size.Width)
			{
				int tx = location.X;
				if (tx < -(size.Width - container.Width))
					tx = -(size.Width - container.Width);
				if (tx > 0)
					tx = 0;
				location.X = tx;
			}
			else
			{
				location.X = 0;
			}

			if (container.Height < size.Height)
			{
				int ty = location.Y;
				if (ty < -(size.Height - container.Height))
					ty = -(size.Height - container.Height);
				if (ty > 0)
					ty = 0;
				location.Y = ty;
			}
			else
			{
				location.Y = 0;
			}

			picturebox.Location = location;
			picturebox.Size = size;
		}

		#endregion

		#region Taskflow: (開始と停止)

		/// <summary>
		/// タスクの処理回数の選択
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskLoopMax_TextChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(toolTaskLoopMax.Text) == false)
			{
				int value = Convert.ToInt32(toolTaskLoopMax.Text);
				if (value > 0)
					this.LoopMax = value;
			}
		}

		/// <summary>
		/// タスクの繰り返し処理の指示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskRepeat_Click(object sender, EventArgs e)
		{
			this.IsTaskRepeat = true;
			toolTaskStart_Click(sender, e);
		}

		/// <summary>
		/// タスクの開始
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskStart_Click(object sender, EventArgs e)
		{
			menuTaskflowStart_Click(sender, e);
		}

		/// <summary>
		/// タスクの停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskStop_Click(object sender, EventArgs e)
		{
			menuTaskflowStop_Click(sender, e);
		}

		#endregion

		#region Taskflow: (準備と復旧)

		/// <summary>
		/// 非同期処理開始前の準備
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void workerTask_Prepare(object sender, EventArgs e)
		{
			#region 操作禁止:
			{
				bool enable = false;
				toolFileGenerateCode.Enabled = enable;
				toolFileSave.Enabled = enable;
				toolFilePrint.Enabled = enable;
				toolFilePrintPreview.Enabled = enable;
				toolTaskLoopMax.Enabled = enable;
			}
			#endregion

			#region TaskSchedule の更新:
			this.TaskSchedule.Clear();
			this.TaskNode.Taskflow.SortTaskSequence(true);
			this.TaskSchedule = this.TaskNode.Taskflow.GetTasks(true);
			#endregion

			#region Outline の更新:
			Outline_Update();
			#endregion

			#region リストビューの初期化:
			listTimes.Items.Clear();
			foreach (var task in this.TaskSchedule)
			{
				var lvitem = new ListViewItem(task.Name, task.IconKey);
				lvitem.SubItems.Add(new ListViewItem.ListViewSubItem(lvitem, task.Category));
				for (int i = 2; i < listTimes.Columns.Count; i++)
					lvitem.SubItems.Add(new ListViewItem.ListViewSubItem(lvitem, ""));
				listTimes.Items.Add(lvitem);
			}
			#endregion

			#region コンソール出力設定:
			Console_Setup();
			#endregion

			#region ディレクトリ移動:
			if (string.IsNullOrWhiteSpace(this.TaskNode.Info.FileName) == false &&
				System.IO.File.Exists(this.TaskNode.Info.FileName))
			{
				var dir = System.IO.Path.GetDirectoryName(this.TaskNode.Info.FileName);
				System.IO.Directory.SetCurrentDirectory(dir);
			}
			else if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false &&
				System.IO.Directory.Exists(XIE.Tasks.SharedData.ProjectDir))
			{
				System.IO.Directory.SetCurrentDirectory(XIE.Tasks.SharedData.ProjectDir);
			}
			#endregion

			#region 処理開始前の準備:
			{
				var watch = new XIE.CxStopwatch();
				watch.Start();
				{
					var args = new XIE.Tasks.CxTaskExecuteEventArgs(
						this.TaskNode.Taskflow,
						this.TaskNode.AuxInfo,
						this.TaskNode.GetBaseDir(),
						this.TaskNode.Name,
						0,
						this.LoopMax
						);
					foreach (var task in this.TaskSchedule)
					{
						try
						{
							task.Prepare(sender, args);
						}
						catch (System.Exception ex)
						{
							XIE.Log.Api.Error("Prepare: {0}", ex.Message);
						}
					}
				}
				watch.Stop();
				XIE.Log.Api.Trace("Prepare: Elapsed={0:N3} msec", watch.Elapsed);
			}
			#endregion

			this.TaskNode.IsRunning = true;
			this.TaskNode.ExecutedTime = DateTime.Now;
			this.PreviousTaskflow = this.TaskNode.CurrentTaskflow;
		}

		/// <summary>
		/// 非同期処理終了後の復旧
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void workerTask_Restore(object sender, EventArgs e)
		{
			this.TaskNode.IsRunning = false;

			#region 現在のシートが実行前のシートでなければ切り替える.
			if (this.PreviousTaskflow != null &&
				!ReferenceEquals(this.TaskNode.CurrentTaskflow, this.PreviousTaskflow))
			{
				this.TaskNode.Taskflow.SetCurrentTaskflow(this.PreviousTaskflow);
				this.pictureTaskflow.Location = this.PreviousTaskflow.SheetLocation;
			}
			#endregion

			#region コンソール出力設定: (復元)
			Console_TearDown();
			#endregion

			#region ディレクトリ移動:
			if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false &&
				System.IO.Directory.Exists(XIE.Tasks.SharedData.ProjectDir))
			{
				System.IO.Directory.SetCurrentDirectory(XIE.Tasks.SharedData.ProjectDir);
			}
			#endregion

			#region 処理終了後の復旧:
			if (this.TaskSchedule != null)
			{
				var watch = new XIE.CxStopwatch();
				watch.Start();
				{
					var args = new XIE.Tasks.CxTaskExecuteEventArgs(
						this.TaskNode.Taskflow,
						this.TaskNode.AuxInfo,
						this.TaskNode.GetBaseDir(),
						this.TaskNode.Name,
						0,
						this.LoopMax
						);
					foreach (var task in this.TaskSchedule)
					{
						try
						{
							task.Restore(sender, args);
						}
						catch (System.Exception ex)
						{
							XIE.Log.Api.Error("Restore: {0}", ex.Message);
						}
					}
				}
				watch.Stop();
				XIE.Log.Api.Trace("Restore: Elapsed={0:N3} msec", watch.Elapsed);
			}
			#endregion

			#region 操作禁止解除:
			{
				bool enable = true;
				toolFileGenerateCode.Enabled = enable;
				toolFileSave.Enabled = enable;
				toolFilePrint.Enabled = enable;
				toolFilePrintPreview.Enabled = enable;
				toolTaskLoopMax.Enabled = enable;
			}
			#endregion
		}

		#endregion

		#region Taskflow: (スレッド参照用オブジェクト)

		/// <summary>
		/// タスク実行要求
		/// </summary>
		private enum TaskOrder
		{
			/// <summary>
			/// 通常
			/// </summary>
			Normal,

			/// <summary>
			/// 試行 (ソースコード生成前に一回実行します。指定の処理回数は無視します。)
			/// </summary>
			Trial,
		}

		/// <summary>
		/// 処理回数 [0,1~]
		/// </summary>
		private int LoopMax = 1;

		/// <summary>
		/// タスクの繰り返し処理の指示
		/// </summary>
		private bool IsTaskRepeat = false;

		/// <summary>
		/// ブレークポイントでの待機中を示すフラグ
		/// </summary>
		private bool IsWaitingInBreakpoint = false;

		/// <summary>
		/// タスク実行前のタスクフロー
		/// </summary>
		private XIE.Tasks.CxTaskflow PreviousTaskflow = null;

		/// <summary>
		/// タスクの実行順序
		/// </summary>
		private List<XIE.Tasks.CxTaskUnit> TaskSchedule = new List<XIE.Tasks.CxTaskUnit>();

		/// <summary>
		/// タスクの処理時間のコレクション
		/// </summary>
		private Dictionary<XIE.Tasks.CxTaskUnit, List<double>> TaskLaps = null;

		/// <summary>
		/// タスクの実行時に発生した例外
		/// </summary>
		private Dictionary<XIE.Tasks.CxTaskUnit, Exception> TaskException = null;

		#endregion

		#region Taskflow: (スレッド)

		/// <summary>
		/// スレッド: 非同期処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void workerTask_DoWork(object sender, DoWorkEventArgs e)
		{
			if (this.TaskSchedule == null) return;

			XIE.Log.Api.Trace("Execute:");

			var stime = DateTime.Now;
			var repeat = this.IsTaskRepeat;
			int loop_num = 0;
			int loop_max = (repeat) ? 0 : this.LoopMax;

			var order = TaskOrder.Normal;
			if (e.Argument is TaskOrder)
				order = (TaskOrder)e.Argument;
			if (order == TaskOrder.Trial)
			{
				loop_max = 1;
				repeat = false;
			}

			#region タスクの処理時間のコレクションの初期化:
			{
				this.TaskLaps = new Dictionary<XIE.Tasks.CxTaskUnit, List<double>>();
				this.TaskException = new Dictionary<XIE.Tasks.CxTaskUnit, Exception>();
				foreach (var task in this.TaskSchedule)
				{
					this.TaskLaps[task] = new List<double>();
					this.TaskException[task] = null;
				}
			}
			#endregion

			#region タスクの実行:
			try
			{
				var taskflow = this.TaskNode.Taskflow;
				if (taskflow != null)
				{
					while (workerTask.CancellationPending == false)
					{
						var args = new XIE.Tasks.CxTaskExecuteEventArgs();

						// DoWork/ProgressChanged 間の排他制御:
						lock (this.TaskSchedule)
						{
							loop_num++;
							if (repeat == false)
							{
								if (loop_num > loop_max) break;
							}

							// タスクユニットに渡す引数.
							args.Taskflow = this.TaskNode.Taskflow;
							args.AuxInfo = this.TaskNode.AuxInfo;
							args.BaseDir = this.TaskNode.GetBaseDir();
							args.BaseName = this.TaskNode.Name;
							args.LoopCount = loop_num;
							args.LoopMax = loop_max;

							args.Callback = delegate(object _sender, XIE.Tasks.CxTaskExecuteEventArgs _e)
							#region タスクフローからコールバックされる関数.
							{
								if (workerTask.CancellationPending)
									throw new OperationCanceledException();

								var task = (XIE.Tasks.CxTaskUnit)_sender;

								try
								{
									var watch = new XIE.CxStopwatch();

									// タスクユニットの実行.
									watch.Start();
									task.Execute(sender, args);
									watch.Stop();

									this.TaskLaps[task].Add(watch.Lap);
									this.TaskException[task] = null;

									// ブレークポイントの処理.
									if (order == TaskOrder.Normal)
									{
										if (task.Breakpoint)
										{
											workerTask_Breakpoint(task, args);
											this.IsWaitingInBreakpoint = true;
											while (workerTask.CancellationPending == false)
											{
												if (this.IsWaitingInBreakpoint == false) break;
												System.Threading.Thread.Sleep(10);
											}
										}
									}
								}
								catch (System.Exception ex)
								{
									this.TaskLaps[task].Add(0);
									this.TaskException[task] = ex;
								}
							};
							#endregion

							// 最上位のタスクフローの実行.
							taskflow.Execute(sender, args);
						}

						// 経過通知.
						workerTask.ReportProgress(loop_num);

						#region 制御構文:
						if (args.ControlSyntax == XIE.Tasks.ExControlSyntax.Continue)
						{
							throw new XIE.CxException(ExStatus.Unsupported, XIE.AxiTextStorage.GetValue("F:XIE.Tasks.Syntax_Continue.Warnings.Unsupported"));
						}
						if (args.ControlSyntax == XIE.Tasks.ExControlSyntax.Break)
						{
							throw new XIE.CxException(ExStatus.Unsupported, XIE.AxiTextStorage.GetValue("F:XIE.Tasks.Syntax_Break.Warnings.Unsupported"));
						}
						if (args.ControlSyntax == XIE.Tasks.ExControlSyntax.Return)
						{
							break;
						}
						#endregion

						System.Threading.Thread.Sleep(10);
					}
				}
			}
			catch (System.Exception ex)
			{
				e.Result = ex;
			}
			#endregion

			#region トレース:
			var etime = DateTime.Now;
			var timeSpan = etime - stime;
			XIE.Log.Api.Trace("Execute: Elapsed={0:N3} msec (loop={1}) {2}~{3}"
				, timeSpan.TotalMilliseconds
				, loop_num
				, stime.ToString("hh:mm:ss")
				, etime.ToString("hh:mm:ss")
				);
			#endregion
		}

		/// <summary>
		/// スレッド: ブレークポイントの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void workerTask_Breakpoint(object sender, XIE.Tasks.CxTaskExecuteEventArgs e)
		{
			this.Invoke((MethodInvoker)delegate()
			{
				var task = (XIE.Tasks.CxTaskUnit)sender;
				var info = this.TaskNode.Visualizer.HandlingInfo;
				info.Reset();
				info.FocusedUnit = task;

				// 現在のタスクを可視範囲内に表示する.
				pictureTaskflow_EnsureVisible(task);

				ImageView_DrawImage();
				ImageView_Refresh();

				propertyTask.SelectedObject = task;
				propertyTask.Refresh();
				pictureTaskflow.Refresh();

				toolTimesAdjust_Click(sender, e);
				gridReport_Update(sender, e);
			});
		}

		/// <summary>
		/// スレッド: 経過処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void workerTask_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			// DoWork/ProgressChanged 間の排他制御:
			lock (this.TaskSchedule)
			{
				// プロパティグリッドの表示更新:
				propertyTask.Refresh();

				// リストビューの更新:
				listTimes_Update(sender, e);

				#region コンソール出力:
				try
				{
					string text = SpecialOutputString.ToString();
					SpecialOutputString.Length = 0;
					long sum_length = textConsole.TextLength;
					sum_length += text.Length;
					if (sum_length >= (long)textConsole.MaxLength)
						textConsole.Clear();

					// 追加して最後の行までスクロールする.
					textConsole.AppendText(text);
					textConsole.SelectionStart = textConsole.Text.Length;
					textConsole.Focus();
					textConsole.ScrollToCaret();
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message);
				}
				#endregion
			}
		}

		/// <summary>
		/// スレッド: 完了時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void workerTask_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			workerTask_Restore(sender, e);
			this.IsTaskRepeat = false;

			ImageView_DrawImage();
			ImageView_Refresh();
			propertyTask.Refresh();
			pictureTaskflow.Refresh();

			toolTimesAdjust_Click(sender, e);
			gridReport_Update(sender, e);

			if (e.Result is Exception)
			{
				var ex = (Exception)e.Result;
				statusLabelMessage.Text = ex.Message;
			}
		}

		#endregion

		#region Taskflow: (menuTaskPortIn)

		/// <summary>
		/// menuTaskPortIn: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskPortIn_Opened(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning == false)
			{
				menuTaskPortInDisconnect.Enabled = true;
			}
			else
			{
				menuTaskPortInDisconnect.Enabled = false;
			}
		}

		/// <summary>
		/// menuTaskPortIn: Disconnect
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskPortInDisconnect_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			#region 指定された入力ポートの接続を解除します:
			{
				var info = this.TaskNode.Visualizer.HandlingInfo;
				if (info.FocusedUnit != null)
				{
					var unit = info.FocusedUnit;
					var type = info.GripPosition.Type;
					var index = info.GripPosition.Index;

					switch (type)
					{
						case ExTaskUnitPositionType.ControlIn:
							{
								this.TaskNode.AddHistory();

								unit.ControlIn.Disconnect();
							}
							break;
						case ExTaskUnitPositionType.DataIn:
							{
								this.TaskNode.AddHistory();

								if (0 <= index && index < unit.DataIn.Length)
									unit.DataIn[index].Disconnect();
							}
							break;
						case ExTaskUnitPositionType.DataParam:
							{
								this.TaskNode.AddHistory();

								if (0 <= index && index < unit.DataParam.Length)
									unit.DataParam[index].Disconnect();
							}
							break;
					}

					pictureTaskflow.Refresh();
				}
			}
			#endregion
		}

		#endregion

		#region Taskflow: (menuTaskPortOut)

		/// <summary>
		/// menuTaskPortOut: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskPortOut_Opened(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning == false)
			{
				menuTaskPortOutDisconnect.Enabled = true;
			}
			else
			{
				menuTaskPortOutDisconnect.Enabled = false;
			}
		}

		/// <summary>
		/// menuTaskPortOut: Disconnect
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskPortOutDisconnect_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			#region 指定の出力ポートへの接続を解除します:
			{
				var info = this.TaskNode.Visualizer.HandlingInfo;
				if (info.FocusedUnit != null)
				{
					var unit = info.FocusedUnit;
					var type = info.GripPosition.Type;
					var index = info.GripPosition.Index;

					switch (type)
					{
						case ExTaskUnitPositionType.ControlOut:
							{
								this.TaskNode.AddHistory();

								unit.ControlOut.Disconnect(this.TaskNode.CurrentTaskflow.TaskUnits);
							}
							break;
						case ExTaskUnitPositionType.DataOut:
							{
								this.TaskNode.AddHistory();

								if (0 <= index && index < unit.DataOut.Length)
								{
									unit.DataOut[index].Disconnect(this.TaskNode.CurrentTaskflow.TaskUnits);
								}
							}
							break;
					}

					pictureTaskflow.Refresh();
				}
			}
			#endregion
		}

		#endregion

		#region Taskflow: (menuTaskBody)

		/// <summary>
		/// menuTaskBody: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskBody_Opened(object sender, EventArgs e)
		{
			var info = this.TaskNode.Visualizer.HandlingInfo;
			menuTaskBodyControlPanel.Enabled = (info.FocusedUnit is XIE.Tasks.IxTaskControlPanel);
			menuTaskBodyEnter.Enabled = (info.FocusedUnit is XIE.Tasks.CxTaskflow);
			menuTaskBodyBreakpoint.Enabled = true;

			if (this.TaskNode.IsRunning == false)
			{
				var tasks = this.TaskNode.CurrentTaskflow.TaskUnits.FindAll(item => item.Selected);
				menuTaskBodyCut.Enabled = (tasks.Count > 0);
				menuTaskBodyCopy.Enabled = (tasks.Count > 0);
				menuTaskBodyPaste.Enabled = (Clipboard.ContainsData("XIE.Tasks.CxTaskUnit"));
				menuTaskBodyDisconnect.Enabled = true;
				menuTaskBodyDelete.Enabled = true;
			}
			else
			{
				menuTaskBodyCut.Enabled = false;
				menuTaskBodyCopy.Enabled = false;
				menuTaskBodyPaste.Enabled = false;
				menuTaskBodyDisconnect.Enabled = false;
				menuTaskBodyDelete.Enabled = false;
			}
		}

		/// <summary>
		/// menuTaskBody: Control Panel が選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskBodyControlPanel_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning == false)
			{
				var info = this.TaskNode.Visualizer.HandlingInfo;
				if (info.FocusedUnit is XIE.Tasks.IxTaskControlPanel)
				{
					var task = (XIE.Tasks.IxTaskControlPanel)info.FocusedUnit;
					if (task.IsOpened == false)
					{
						try
						{
							var args = new XIE.Tasks.CxTaskExecuteEventArgs(
								this.TaskNode.Taskflow,
								this.TaskNode.AuxInfo,
								this.TaskNode.GetBaseDir(),
								this.TaskNode.Name,
								0,
								this.LoopMax
								);
							var dlg = task.Create(args);
							if (dlg == null)
							{
								#region 表示更新:
								{
									ImageView_DrawImage();
									ImageView_Refresh();

									propertyTask.Refresh();
									pictureTaskflow.Refresh();

									gridReport_Update(sender, e);
								}
								#endregion
							}
							else
							{
								dlg.StartPosition = FormStartPosition.Manual;
								dlg.Location = ApiHelper.GetNeighborPosition(dlg.Size, 1);
								dlg.FormClosed += (_sender, _e) =>
									{
										#region 表示更新:
										{
											ImageView_DrawImage();
											ImageView_Refresh();

											propertyTask.Refresh();
											pictureTaskflow.Refresh();

											gridReport_Update(sender, e);
										}
										#endregion
									};
								dlg.Show(this);

								this.TaskNode.ControlPanelManagement_Add(dlg, (XIE.Tasks.CxTaskUnit)task);
							}
						}
						catch (Exception ex)
						{
							MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}
			}
		}

		/// <summary>
		/// menuTaskBody: Breakpoint
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskBodyBreakpoint_Click(object sender, EventArgs e)
		{
			#region 選択されたタスクユニットのブレークポイントを ON/OFF します:
			{
				this.TaskNode.AddHistory();

				foreach (var task in this.TaskNode.CurrentTaskflow.TaskUnits)
				{
					if (task.Selected)
						task.Breakpoint = !task.Breakpoint;
				}

				pictureTaskflow.Refresh();
			}
			#endregion
		}

		/// <summary>
		/// menuTaskBody: Disconnect
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskBodyDisconnect_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			#region 選択されたタスクユニットの入出力ポートを全て接続解除します:
			{
				this.TaskNode.AddHistory();

				var info = this.TaskNode.Visualizer.HandlingInfo;
				var tasks = this.TaskNode.CurrentTaskflow.GetTasks(false).FindAll(item => item.Selected);

				foreach (var task in tasks)
				{
					// 各タスクユニットの入力側のポートを切断します.
					task.ControlIn.Disconnect();
					foreach (var item in task.DataIn)
						item.Disconnect();
					foreach (var item in task.DataParam)
						item.Disconnect();

					// 各タスクユニットの出力側のポートを切断します.
					task.ControlOut.Disconnect(this.TaskNode.CurrentTaskflow.TaskUnits);
					foreach (var port in task.DataOut)
						port.Disconnect(this.TaskNode.CurrentTaskflow.TaskUnits);
				}

				pictureTaskflow.Refresh();
			}
			#endregion
		}

		/// <summary>
		/// menuTaskBody: Delete
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskBodyDelete_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			#region 選択されたタスクユニットを削除します:
			{
				this.TaskNode.AddHistory();

				var info = this.TaskNode.Visualizer.HandlingInfo;
				var tasks = this.TaskNode.CurrentTaskflow.GetTasks(false).FindAll(item => item.Selected);

				// 選択されているタスクユニットを除去します.
				foreach (var task in tasks)
				{
					this.TaskNode.CurrentTaskflow.TaskUnits.Remove(task);
				}
				// 除去したタスクユニットの出力側のポートを切断します.
				foreach (var task in tasks)
				{
					task.ControlOut.Disconnect(this.TaskNode.CurrentTaskflow.TaskUnits);
					foreach (var port in task.DataOut)
						port.Disconnect(this.TaskNode.CurrentTaskflow.TaskUnits);
				}
				info.Reset();

				this.TaskNode.ControlPanelManagement_Purge();
				this.pictureTaskflow.Refresh();
				this.Outline_Update();
			}
			#endregion
		}

		/// <summary>
		/// menuTaskBody: Enter が選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskBodyEnter_Click(object sender, EventArgs e)
		{
			var info = this.TaskNode.Visualizer.HandlingInfo;
			if (info.FocusedUnit is XIE.Tasks.CxTaskflow)
			{
				if (this.TaskNode.IsRunning == false)
					this.TaskNode.AddHistory(false);

				var taskflow = (XIE.Tasks.CxTaskflow)info.FocusedUnit;
				if (this.TaskNode.Taskflow.SetCurrentTaskflow(taskflow))
				{
					pictureTaskflow.Location = taskflow.SheetLocation;
				}
				pictureTaskflow.Refresh();
				Outline_EnsureVisible();
				treeOutline.Refresh();
			}
		}

		#endregion

		#region Taskflow: (menuTaskflow)

		/// <summary>
		/// menuTaskflow: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflow_Opened(object sender, EventArgs e)
		{
			bool is_running = this.TaskNode.IsRunning;
			menuTaskflowView.Enabled = (!is_running);
			menuTaskflowQuit.Enabled = (this.TaskNode.CurrentTaskflow != this.TaskNode.Taskflow);
			menuTaskflowStart.Enabled = (workerTask.IsBusy == false || (workerTask.IsBusy && this.IsWaitingInBreakpoint));
			menuTaskflowStop.Enabled = (workerTask.IsBusy == true);
		}

		#endregion

		#region Taskflow: (menuTaskflow) [Start/Stop]

		/// <summary>
		/// menuTaskflow: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowStart_Click(object sender, EventArgs e)
		{
			#region 開始:
			if (workerTask.IsBusy)
			{
				if (this.IsWaitingInBreakpoint)
					this.IsWaitingInBreakpoint = false;
				else
					workerTask.CancelAsync();
			}
			else
			{
				try
				{
					workerTask_Prepare(sender, e);
					workerTask.RunWorkerAsync(TaskOrder.Normal);
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			#endregion
		}

		/// <summary>
		/// menuTaskflow: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowStop_Click(object sender, EventArgs e)
		{
			if (workerTask.IsBusy)
				workerTask.CancelAsync();
		}

		/// <summary>
		/// menuTaskflow: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowDeleteAllBreakpoints_Click(object sender, EventArgs e)
		{
			#region 全てのタスクユニットのブレークポイントを解除します:
			{
				this.TaskNode.AddHistory();

				var taskflow = (XIE.Tasks.CxTaskflow)this.TaskNode.Taskflow;
				var tasks = taskflow.GetTasks(true);
				tasks.ForEach(item => item.Breakpoint = false);

				pictureTaskflow.Refresh();
			}
			#endregion
		}

		#endregion

		#region Taskflow: (menuTaskflow) [Property]

		/// <summary>
		/// menuTaskflow: タスクプロパティの編集
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowProperty_Click(object sender, EventArgs e)
		{
			var node = this.TaskNode;
			var taskflow = this.TaskNode.Taskflow;
			var current = (this.TaskNode.CurrentTaskflow is XIE.Tasks.Syntax_Class)
				? this.TaskNode.CurrentTaskflow
				: this.TaskNode.Taskflow;

			var dlg = new XIE.Tasks.CxTaskPortEditForm(current);
			dlg.StartPosition = FormStartPosition.Manual;
			dlg.Location = ApiHelper.GetNeighborPosition(dlg.Size, 1);
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				#region タスクユニットのアイコンを初期化する.
				try
				{
					int ci = current.DataIn.Length + current.DataParam.Length;
					int co = current.DataOut.Length;

					string icon = current.IconKey;
					if (ci > 0 && co > 0)
						icon = "Service-Port";
					else if (ci > 0)
						icon = "Service-PortIn";
					else if (co > 0)
						icon = "Service-PortOut";
					else
						icon = "Service-Connect";
					current.IconKey = icon;
					current.IconImage = treeToolbox.ImageList.Images[icon];
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.StackTrace);
				}
				#endregion

				#region アイコン設定.
				if (ReferenceEquals(taskflow, current))
				{
					node.ImageKey = current.IconKey;
					node.SelectedImageKey = current.IconKey;

					// 詳細は XIEstudio に記載するコメントを参照されたい.
					// 現象)
					//   デザイナでアイコンを指定した場合、Linux Mono 環境では下記の例外が発生する.
					//   System.ArgumentException: A null reference or invalid value was found [GDI+ status: InvalidParameter]
					// 
					switch (System.Environment.OSVersion.Platform)
					{
						case PlatformID.Unix:
							// 
							// 上記理由によりアイコンの設定は行わない.
							// 
							break;
						default:
							{
								var aux = (XIE.Tasks.IxAuxImageList16)CxAuxInfoForm.AuxInfo;
								var image = (Bitmap)aux.ImageList.Images[this.TaskNode.ImageKey];
								this.Icon = Icon.FromHandle(image.GetHicon());
							}
							break;
					}
				}
				#endregion
			}
		}

		#endregion

		#region Taskflow: (menuTaskflow) [File]

		/// <summary>
		/// menuTaskflow: ファイルメニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowFile_DropDownOpened(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning)
			{
				menuTaskflowFileSave.Enabled = false;
				menuTaskflowFilePrint.Enabled = false;
				menuTaskflowFilePrintPreview.Enabled = false;
			}
			else
			{
				menuTaskflowFileSave.Enabled = true;
				menuTaskflowFilePrint.Enabled = true;
				menuTaskflowFilePrintPreview.Enabled = true;
			}
		}

		/// <summary>
		/// menuTaskflow: ファイル保存
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowFileSave_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			#region ファイル保存.
			try
			{
				var node = this.TaskNode;

				#region SaveFileDialog
				var dlg = new SaveFileDialog();
				{
					string filename = node.Info.FileName;
					if (string.IsNullOrEmpty(filename))
						filename = node.Name + ".xtf";

					dlg.Filter = "Taskflow files|*.xtf";
					dlg.OverwritePrompt = true;
					dlg.AddExtension = true;
					dlg.FileName = System.IO.Path.GetFileName(filename);

					string dir = System.IO.Path.GetDirectoryName(filename);
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

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					CxAuxInfoForm.AppSettings.TaskflowFileDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);
					node.Info.FileName = dlg.FileName;
					node.Save();
					node.Setup();
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion
		}

		/// <summary>
		/// menuTaskflow: 印刷
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowFilePrint_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			#region 印刷:
			try
			{
				this.TaskNode.Document_Setup();
				this.printDlg.Document = this.TaskNode.Document;
				if (this.printDlg.ShowDialog(this) == DialogResult.OK)
				{
					this.TaskNode.Document.Print();
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion
		}

		/// <summary>
		/// menuTaskflow: 印刷プレビュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowFilePrintPreview_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			#region 印刷プレビュー:
			try
			{
				this.TaskNode.Document_Setup();
				var dlg = new CxTaskflowPrintPreviewForm(this.TaskNode);
				dlg.StartPosition = FormStartPosition.Manual;
				dlg.Location = new Point(this.Location.X + 16, this.Location.Y + 16);
				dlg.ShowDialog(this);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion
		}

		#endregion

		#region Taskflow: (menuTaskflow) [Edit]

		/// <summary>
		/// menuTaskflow: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowEdit_DropDownOpened(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning == false)
			{
				menuTaskflowEditUndo.Enabled = this.TaskNode.CanUndo();
				menuTaskflowEditRedo.Enabled = this.TaskNode.CanRedo();

				var tasks = this.TaskNode.CurrentTaskflow.TaskUnits.FindAll(item => item.Selected);
				menuTaskflowEditCut.Enabled = (tasks.Count > 0);
				menuTaskflowEditCopy.Enabled = (tasks.Count > 0);
				menuTaskflowEditPaste.Enabled = (Clipboard.ContainsData("XIE.Tasks.CxTaskUnit"));
				menuTaskflowEditSelectAll.Enabled = (this.TaskNode.CurrentTaskflow.TaskUnits.Count > 0);
			}
			else
			{
				menuTaskflowEditUndo.Enabled = false;
				menuTaskflowEditRedo.Enabled = false;
				menuTaskflowEditCut.Enabled = false;
				menuTaskflowEditCopy.Enabled = false;
				menuTaskflowEditPaste.Enabled = false;
				menuTaskflowEditSelectAll.Enabled = (this.TaskNode.CurrentTaskflow.TaskUnits.Count > 0);
			}
		}

		/// <summary>
		/// menuTaskflow: Undo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowEditUndo_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			this.TaskNode.Undo();
			this.propertyTask.SelectedObject = null;
			this.ImageView.Image = null;
			this.ImageView_Refresh();
			this.propertyTask.Refresh();
			this.pictureTaskflow.Refresh();
		}

		/// <summary>
		/// menuTaskflow: Redo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowEditRedo_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			this.TaskNode.Redo();
			this.propertyTask.SelectedObject = null;
			this.ImageView.Image = null;
			this.ImageView_Refresh();
			this.propertyTask.Refresh();
			this.pictureTaskflow.Refresh();
		}

		/// <summary>
		/// menuTaskflow: Cut
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowEditCut_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			var tasks = this.TaskNode.CurrentTaskflow.TaskUnits.FindAll(item => item.Selected);
			if (tasks.Count > 0)
			{
				this.TaskNode.AddHistory();

				foreach (var task in tasks)
				{
					task.Selected = false;

					this.TaskNode.CurrentTaskflow.TaskUnits.Remove(task);

					// 基準位置:
					task.Location = new Point(
						task.Location.X + pictureTaskflow.Location.X,
						task.Location.Y + pictureTaskflow.Location.Y
						);

					// 切断:
					{
						// タスクユニットの入力側のポートを切断します.
						task.ControlIn.Disconnect();
						foreach (var item in task.DataIn)
							item.Disconnect();
						foreach (var item in task.DataParam)
							item.Disconnect();

						// タスクユニットの出力側のポートを切断します.
						task.ControlOut.Disconnect(this.TaskNode.CurrentTaskflow.TaskUnits);
						foreach (var port in task.DataOut)
							port.Disconnect(this.TaskNode.CurrentTaskflow.TaskUnits);
					}
				}

				Clipboard.SetData("XIE.Tasks.CxTaskUnit", tasks);

				this.TaskNode.ControlPanelManagement_Purge();
				this.TaskNode.Visualizer.HandlingInfo.FocusedUnit = null;
				this.propertyTask.SelectedObject = null;
				this.ImageView.Image = null;
				this.ImageView_Refresh();
				this.propertyTask.Refresh();
				this.pictureTaskflow.Refresh();
			}
		}

		/// <summary>
		/// menuTaskflow: Copy
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowEditCopy_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			var tasks = this.TaskNode.CurrentTaskflow.TaskUnits.FindAll(item => item.Selected);
			if (tasks.Count > 0)
			{
				this.TaskNode.AddHistory(false);

				for (int i = 0; i < tasks.Count; i++)
				{
					tasks[i].Selected = false;

					var task = (XIE.Tasks.CxTaskUnit)tasks[i].Clone();
					tasks[i] = task;

					// 基準位置:
					task.Location = new Point(
						task.Location.X + pictureTaskflow.Location.X,
						task.Location.Y + pictureTaskflow.Location.Y
						);

					// 切断:
					{
						// タスクユニットの入力側のポートを切断します.
						task.ControlIn.Disconnect();
						foreach (var item in task.DataIn)
							item.Disconnect();
						foreach (var item in task.DataParam)
							item.Disconnect();

						// タスクユニットの出力側のポートを切断します.
						task.ControlOut.Disconnect(this.TaskNode.CurrentTaskflow.TaskUnits);
						foreach (var port in task.DataOut)
							port.Disconnect(this.TaskNode.CurrentTaskflow.TaskUnits);
					}
				}

				Clipboard.SetData("XIE.Tasks.CxTaskUnit", tasks);

				this.TaskNode.Visualizer.HandlingInfo.FocusedUnit = null;
				this.propertyTask.SelectedObject = null;
				this.ImageView.Image = null;
				this.ImageView_Refresh();
				this.propertyTask.Refresh();
				this.pictureTaskflow.Refresh();
			}
		}

		/// <summary>
		/// menuTaskflow: Paste
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowEditPaste_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.IsRunning) return;

			if (Clipboard.ContainsData("XIE.Tasks.CxTaskUnit"))
			{
				var tasks = (List<XIE.Tasks.CxTaskUnit>)Clipboard.GetData("XIE.Tasks.CxTaskUnit");

				this.TaskNode.AddHistory();

				foreach (var task in tasks)
				{
					this.TaskNode.CurrentTaskflow.TaskUnits.Add(task);

					#region タスクの初期化:
					try
					{
						var icons = (XIE.Tasks.IxAuxImageList16)this.TaskNode.AuxInfo;

						var args = new XIE.Tasks.CxTaskSetupEventArgs(
							this.TaskNode.Taskflow,
							this.TaskNode.AuxInfo,
							this.TaskNode.GetBaseDir(),
							this.TaskNode.Name
							);

						// 基準位置を設定する.
						var cp = new Point(
							task.Location.X - pictureTaskflow.Location.X,
							task.Location.Y - pictureTaskflow.Location.Y
							);

						task.Location = cp;
						task.Selected = true;
						task.IconImage = icons.ImageList.Images[task.IconKey];
						task.Setup(sender, args, this.TaskNode.CurrentTaskflow);

						#region 子タスクの初期化:
						if (task is XIE.Tasks.CxTaskflow)
						{
							var taskflow = (XIE.Tasks.CxTaskflow)task;

							var children = taskflow.GetTasks(true);
							foreach (var child in children)
							{
								try
								{
									child.IconImage = icons.ImageList.Images[child.IconKey];
								}
								catch (System.Exception)
								{
								}
							}
						}
						#endregion
					}
					catch (System.Exception)
					{
					}
					#endregion
				}

				this.Outline_Update();
				this.ImageView_Refresh();
				this.propertyTask.Refresh();
				this.pictureTaskflow.Refresh();
			}
		}

		/// <summary>
		/// menuTaskflow: Select All
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowEditSelectAll_Click(object sender, EventArgs e)
		{
			var tasks = this.TaskNode.CurrentTaskflow.TaskUnits;
			if (tasks.Count > 0)
			{
				this.TaskNode.AddHistory(false);

				foreach (var task in tasks)
				{
					task.Selected = true;
				}

				this.TaskNode.Visualizer.HandlingInfo.FocusedUnit = null;
				this.propertyTask.SelectedObject = null;
				this.ImageView.Image = null;
				this.ImageView_Refresh();
				this.propertyTask.Refresh();
				this.pictureTaskflow.Refresh();
			}
		}

		#endregion

		#region Taskflow: (menuTaskflow) [View]

		/// <summary>
		/// menuTaskflow: 表示倍率メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewScale_DropDownOpened(object sender, EventArgs e)
		{
			var scale = this.TaskNode.Visualizer.Scale;
			foreach (ToolStripMenuItem item in menuTaskflowViewScale.DropDownItems)
			{
				int tag = Convert.ToInt32(item.Tag);
				item.Checked = (tag == scale);
			}
		}

		/// <summary>
		/// menuTaskflow: 表示倍率メニューが選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewScale_Click(object sender, EventArgs e)
		{
			var menu = (ToolStripMenuItem)sender;
			if (menu.Tag != null)
			{
				var scale = Convert.ToInt32(menu.Tag);
				this.TaskNode.Visualizer.Scale = scale;
				pictureTaskflow_AdjustSize(scale);
				this.pictureTaskflow.Refresh();
			}
		}

		/// <summary>
		/// menuTaskflow: SheetType
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewSheetType_DropDownOpened(object sender, EventArgs e)
		{
			int src_type = this.TaskNode.Visualizer.SheetType;
			foreach (ToolStripMenuItem item in menuTaskflowViewSheetType.DropDownItems)
			{
				int dst_type = Convert.ToInt32(item.Tag);
				item.Checked = (dst_type == src_type);
			}
		}

		/// <summary>
		/// menuTaskflow: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewSheetType_Click(object sender, EventArgs e)
		{
			var item = (ToolStripMenuItem)sender;
			int sheet_type = Convert.ToInt32(item.Tag);
			this.TaskNode.Visualizer.SheetType = sheet_type;
			pictureTaskflow_AdjustSize(this.TaskNode.Visualizer.Scale);
			this.pictureTaskflow.Refresh();
		}

		/// <summary>
		/// menuTaskflow: ColorType
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewColorType_DropDownOpened(object sender, EventArgs e)
		{
			menuTaskflowViewColorType0.Checked = (this.TaskNode.Visualizer.ColorType == 0);
			menuTaskflowViewColorType1.Checked = (this.TaskNode.Visualizer.ColorType == 1);
		}

		/// <summary>
		/// menuTaskflow: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewColorType0_Click(object sender, EventArgs e)
		{
			this.TaskNode.Visualizer.ColorType = 0;
			this.TaskNode.Visualizer.Setup();
			pictureTaskflow.Refresh();
		}

		/// <summary>
		/// menuTaskflow: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewColorType1_Click(object sender, EventArgs e)
		{
			this.TaskNode.Visualizer.ColorType = 1;
			this.TaskNode.Visualizer.Setup();
			pictureTaskflow.Refresh();
		}

		/// <summary>
		/// menuTaskflow: LineType
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewLineType_DropDownOpened(object sender, EventArgs e)
		{
			menuTaskflowViewLineType0.Checked = (this.TaskNode.Visualizer.LineType == 0);
			menuTaskflowViewLineType1.Checked = (this.TaskNode.Visualizer.LineType == 1);
		}

		/// <summary>
		/// menuTaskflow: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewLineType0_Click(object sender, EventArgs e)
		{
			this.TaskNode.Visualizer.LineType = 0;
			this.TaskNode.Visualizer.Setup();
			pictureTaskflow.Refresh();
		}

		/// <summary>
		/// menuTaskflow: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewLineType1_Click(object sender, EventArgs e)
		{
			this.TaskNode.Visualizer.LineType = 1;
			this.TaskNode.Visualizer.Setup();
			pictureTaskflow.Refresh();
		}

		/// <summary>
		/// menuTaskflow: BlockType
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewBlockType_DropDownOpened(object sender, EventArgs e)
		{
			menuTaskflowViewBlockType0.Checked = (this.TaskNode.Visualizer.BlockType == 0);
			menuTaskflowViewBlockType1.Checked = (this.TaskNode.Visualizer.BlockType == 1);
		}

		/// <summary>
		/// menuTaskflow: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewBlockType0_Click(object sender, EventArgs e)
		{
			this.TaskNode.Visualizer.BlockType = 0;
			this.TaskNode.Visualizer.Setup();
			pictureTaskflow.Refresh();
		}

		/// <summary>
		/// menuTaskflow: 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowViewBlockType1_Click(object sender, EventArgs e)
		{
			this.TaskNode.Visualizer.BlockType = 1;
			this.TaskNode.Visualizer.Setup();
			pictureTaskflow.Refresh();
		}

		#endregion

		#region Taskflow: (menuTaskflow) [Quit]

		/// <summary>
		/// menuTaskflow: Quit が選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskflowQuit_Click(object sender, EventArgs e)
		{
			if (this.TaskNode.CurrentTaskflow == this.TaskNode.Taskflow) return;

			var taskflow = this.TaskNode.FindParentTaskflow(this.TaskNode.CurrentTaskflow);
			if (taskflow != null)
			{
				if (this.TaskNode.IsRunning == false)
					this.TaskNode.AddHistory(false);

				this.TaskNode.Taskflow.SetCurrentTaskflow(taskflow);
				pictureTaskflow.Location = taskflow.SheetLocation;
				pictureTaskflow.Refresh();
				Outline_EnsureVisible();
				treeOutline.Refresh();
			}
		}

		#endregion
	}
}
