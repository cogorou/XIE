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
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Management;
using System.Configuration;
using XIE;

namespace XIEstudio
{
	/// <summary>
	/// 外部機器情報編集フォーム
	/// </summary>
	public partial class CxAuxInfoForm : Form
	{
		#region 共有データ:

		/// <summary>
		/// アプリケーション設定
		/// </summary>
		public static CxAppSettings AppSettings = null;

		/// <summary>
		/// アプリケーション設定ファイルパス
		/// </summary>
		public static string AppSettingsFileName = "";

		/// <summary>
		/// 外部機器情報
		/// </summary>
		public static XIE.Tasks.CxAuxInfo AuxInfo = null;

		/// <summary>
		/// 外部機器情報 (変更前の状態)
		/// </summary>
		private XIE.Tasks.CxAuxInfo AuxInfoPrev = null;

		/// <summary>
		/// 外部機器情報ファイル名
		/// </summary>
		public static string AuxInfoFileName = "";

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAuxInfoForm()
		{
			#region ClipboardObserver.
			switch (System.Environment.OSVersion.Platform)
			{
				case PlatformID.Unix:
					break;
				default:
					ClipboardObserver = new XIE.Forms.CxClipboardObserver(this);
					ClipboardObserver.Notify += new EventHandler(ClipboardObserver_Notify);
					break;
			}
			#endregion

			InitializeComponent();
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxAuxInfoForm_Load(object sender, EventArgs e)
		{
			// フォームのサイズ.
			this.PrevFormSize = this.Size;

			Setup();

			// 外部機器情報 (変更前の状態):
			this.AuxInfoPrev = (XIE.Tasks.CxAuxInfo)AuxInfo.Clone();

			CxAuxInfoForm.AuxInfo.Requested += AuxInfo_Requested;
			CxAuxInfoForm.AuxInfo.Updated += AuxInfo_Updated;
			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられる時の解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxAuxInfoForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			#region ファイル保存済みか否かを検証します.
			{
				var images = new List<CxImageNode>();
				var tasks = new List<CxTaskNode>();

				#region Image.
				{
					var folder = FolderNodes["Image"];
					foreach (var item in folder.Nodes)
					{
						if (item is CxImageNode)
						{
							var node = (CxImageNode)item;
							if (node.UnsavedDataExists)
								images.Add(node);
						}
					}
				}
				#endregion

				#region Tasks.
				{
					var folder = FolderNodes["Tasks"];
					foreach (var item in folder.Nodes)
					{
						if (item is CxTaskNode)
						{
							var node = (CxTaskNode)item;
							if (node.UnsavedDataExists)
								tasks.Add(node);
						}
					}
				}
				#endregion

				if (images.Count > 0 || tasks.Count > 0)
				{
					var ans = MessageBox.Show(
						"Unsaved data exists. Do you want to save?",
						"Question",
						MessageBoxButtons.YesNoCancel,
						MessageBoxIcon.Question);

					switch (ans)
					{
						default:
						case DialogResult.Cancel:
							e.Cancel = true;
							return;
						case DialogResult.No:
							break;
						case DialogResult.Yes:

							#region 保存処理: images
							foreach (var node in images)
							{
								node.OpenSaveDialog(this);
							}
							#endregion

							#region 保存処理: tasks
							foreach (var node in tasks)
							{
								node.OpenSaveDialog(this);
							}
							#endregion

							break;
					}
				}
			}
			#endregion

			CxAuxInfoForm.AuxInfo.Purge();

			#region 外部機器情報 (保存):
			if (this.AuxInfoPrev != null &&
				this.AuxInfoPrev.ContentEquals(AuxInfo) == false)
			{
				if (System.IO.Directory.Exists(XIE.Tasks.SharedData.ProjectDir))
				{
					#region 外部機器情報の保存:
					if (CxAuxInfoForm.AuxInfo != null &&
						string.IsNullOrEmpty(XIE.Tasks.SharedData.ProjectDir) == false &&
						string.IsNullOrEmpty(CxAuxInfoForm.AuxInfoFileName) == false &&
						System.IO.Directory.Exists(XIE.Tasks.SharedData.ProjectDir))
					{
						string filename = System.IO.Path.Combine(XIE.Tasks.SharedData.ProjectDir, CxAuxInfoForm.AuxInfoFileName);
						CxAuxInfoForm.AuxInfo.Save(filename);
					}
					#endregion
				}

				this.AuxInfoPrev = null;
			}
			#endregion

			timerUpdateUI.Stop();
			CxAuxInfoForm.AuxInfo.Requested -= AuxInfo_Requested;
			CxAuxInfoForm.AuxInfo.Updated -= AuxInfo_Updated;

			TearDown();
		}

		#endregion

		#region 初期化と解放:(外部機器情報)

		/// <summary>
		/// フォルダーノードコレクション
		/// </summary>
		private Dictionary<string, TreeNode> FolderNodes = new Dictionary<string, TreeNode>();

		/// <summary>
		/// 初期化
		/// </summary>
		void Setup()
		{
			ImageView_Setup();
			PreviewForm_Setup();
			ExifForm_Setup();

			treeAux_Setup();
			CameraNode_Setup();
			SerialPortNode_Setup();
			TcpServerNode_Setup();
			TcpClientNode_Setup();
			Media_Setup();
			Image_Setup();
			TaskNode_Setup();
		}

		/// <summary>
		/// 解放
		/// </summary>
		void TearDown()
		{
			treeAux_Dispose();
			CameraNode_Dispose();
			SerialPortNode_Dispose();
			TcpServerNode_Dispose();
			TcpClientNode_Dispose();
			Media_Dispose();
			Image_Dispose();
			TaskNode_Dispose();

			ExifForm_TearDown();
			PreviewForm_TearDown();
			ImageView_TearDown();
		}

		#endregion

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			#region treeAuxInfo の表示更新.
			foreach (KeyValuePair<string, TreeNode> pair in FolderNodes)
			{
				foreach (TreeNode node in pair.Value.Nodes)
				{
					try
					{
						if (node is XIE.Tasks.IxAuxNode)
							((XIE.Tasks.IxAuxNode)node).Tick(sender, e);
					}
					catch (System.Exception)
					{
					}
					finally
					{
					}
				}
			}
			#endregion

			#region CxCameraNode の表示更新.
			if (treeAux.SelectedNode is CxCameraNode)
			{
				var node = (CxCameraNode)treeAux.SelectedNode;
				node.Draw(this.ImageView);
			}
			#endregion

			#region CxMediaNode の表示更新.
			if (treeAux.SelectedNode is CxMediaNode)
			{
				var node = (CxMediaNode)treeAux.SelectedNode;
				node.Draw(this.ImageView);
			}
			#endregion

			#region toolbarView の表示更新.
			{
				toolLogForm.Checked = (this.LogForm != null && this.LogForm.Visible);
				toolPreview.Checked = (this.PreviewForm != null && this.PreviewForm.Visible);

				toolViewClipboardObserver.Checked = (ClipboardObserver != null && ClipboardObserver.Enabled);

				{
					bool clipboard_exist = false;
					var idata = Clipboard.GetDataObject();
					if (idata != null)
					{
						if (idata.GetDataPresent(DataFormats.Dib))
							clipboard_exist = true;
						else if (idata.GetDataPresent(DataFormats.Bitmap))
							clipboard_exist = true;
						else if (idata.GetDataPresent(typeof(XIE.CxImage).FullName))
							clipboard_exist = true;
					}
					toolViewPaste.Enabled = clipboard_exist;
				}

				toolViewHalftone.Checked = this.ImageView.Halftone;
				toolViewUnpack.Checked = (this.ImageView.Unpack);
				toolViewChannelNo.Text = this.ImageView.ChannelNo.ToString();
				toolViewProfile.Checked = this.ProfileOverlay.Visible;
				toolViewGrid.Checked = this.GridOverlay.Visible;
				toolViewROI.Checked = this.ROIOverlay.Visible;
				toolViewExif.Checked = (this.ExifForm != null && this.ExifForm.Visible);
			}
			#endregion

			#region statusbar の表示更新.
			statusbar_Update();
			#endregion
		}

		// //////////////////////////////////////////////////
		// 外部機器情報
		// //////////////////////////////////////////////////

		#region 外部機器情報: (イベントハンドラ)

		/// <summary>
		/// 外部機器情報通知イベント (変更要求)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void AuxInfo_Requested(object sender, XIE.Tasks.CxAuxNotifyEventArgs e)
		{
			#region 表示更新:
			if (e is XIE.Tasks.CxAuxNotifyEventArgs_Refresh)
			{
				try
				{
					var action = new Action(() =>
					{
						var args = (XIE.Tasks.CxAuxNotifyEventArgs_Refresh)e;

						this.ImageView.Refresh();
					});
					if (this.InvokeRequired)
						this.Invoke(action);
					else
						action();
				}
				catch (System.Exception)
				{
				}
			}
			#endregion

			#region 画像ノード追加:
			if (e is XIE.Tasks.CxAuxNotifyEventArgs_AddImage)
			{
				try
				{
					var action = new Action(() =>
					{
						var args = (XIE.Tasks.CxAuxNotifyEventArgs_AddImage)e;
						var info = new XIE.Tasks.CxImageInfo();
						var node = new CxImageNode(info, args.Data);
						node.AuxInfo = CxAuxInfoForm.AuxInfo;
						node.ContextMenuStrip = DataMenu;
						node.Setup();
						node.Name = args.Name;
						node.Text = args.Name;

						var aux = (XIE.Tasks.IxAuxInfoImages)CxAuxInfoForm.AuxInfo;
						aux.Add(info, args.Data);
						treeAux.Nodes["Data"].Nodes["Image"].Nodes.Add(node);
						treeAux.Nodes["Data"].Nodes["Image"].Expand();

						if (args.Selected)
							treeAux.SelectedNode = node;
					});
					if (this.InvokeRequired)
						this.Invoke(action);
					else
						action();
				}
				catch (System.Exception)
				{
				}
			}
			#endregion
		}

		/// <summary>
		/// 外部機器情報通知イベント (更新通知)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void AuxInfo_Updated(object sender, XIE.Tasks.CxAuxNotifyEventArgs e)
		{
		}

		#endregion

		// //////////////////////////////////////////////////
		// toolbar
		// //////////////////////////////////////////////////

		#region toolbarAuxInfo: (Log)

		/// <summary>
		/// ログフォームの表示/非表示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolLogForm_Click(object sender, EventArgs e)
		{
			if (this.LogForm == null)
			{
				this.LogForm = new XIE.Log.CxLogForm();
				this.LogForm.Text = "Log";
				this.LogForm.Owner = this;
				this.LogForm.StartPosition = FormStartPosition.Manual;
				this.LogForm.Location = ApiHelper.GetNeighborPosition(this.LogForm.Size);
				this.LogForm.FormClosed += delegate(object _sender, FormClosedEventArgs _e)
				{
					this.LogForm = null;
				};
				this.LogForm.Show();
			}
			else
			{
				this.LogForm.Visible = !this.LogForm.Visible;
			}
		}

		private XIE.Log.CxLogForm LogForm = null;

		#endregion

		#region toolbarAuxInfo: (File)

		/// <summary>
		/// プロジェクトディレクトリ。(空白の場合は既定値になります。)
		/// </summary>
		public string ProjectDir = "";

		/// <summary>
		/// ファイル読み込み
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFileOpen_Click(object sender, EventArgs e)
		{
			#region OpenFileDialog
			var dlg = new OpenFileDialog();
			{
				dlg.AddExtension = true;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = true;
				dlg.Filter = "Supported files |*.avi;*.asf;*.wmv;*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw;";
				dlg.Filter += "|Media files |*.avi;*.asf;*.wmv";
				dlg.Filter += "|Image files |*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw";
				dlg.Filter += "|All files (*.*)|*.*";

				if (string.IsNullOrWhiteSpace(this.ProjectDir) == false)
					dlg.InitialDirectory = this.ProjectDir;
				else if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
					dlg.InitialDirectory = XIE.Tasks.SharedData.ProjectDir;

				if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
					dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);
			}
			#endregion

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				this.ProjectDir = System.IO.Path.GetDirectoryName(dlg.FileName);

				try
				{
					TreeNode firstnode = LoadDataFiles(dlg.FileNames);
					if (firstnode != null)
						treeAux.SelectedNode = firstnode;
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		#endregion

		#region toolbarAuxInfo: (Audio)

		/// <summary>
		/// 音声入力デバイスの選択
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolAudioInput_Click(object sender, EventArgs e)
		{
			var param = new XIE.Media.CxDeviceParam();
			var aux = (XIE.Tasks.IxAuxInfoAudioInputs)CxAuxInfoForm.AuxInfo;
			if (aux.Infos.Length > 0)
				param.CopyFrom(aux.Infos[0]);

			var dlg = new XIE.Media.CxDeviceSelectionForm(
				XIE.Media.ExMediaType.Audio,
				XIE.Media.ExMediaDir.Input,
				param);
			dlg.StartPosition = FormStartPosition.Manual;
			dlg.Location = ApiHelper.GetNeighborPosition(new Size(500, 203));
			dlg.Text = string.Format("Audio Input");
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				if (string.IsNullOrEmpty(param.Name))
					aux.RemoveAll();
				else if (aux.Infos.Length == 0)
					aux.Add(param);
				else
					aux.Infos[0] = param;
			}
		}

		/// <summary>
		/// 音声出力デバイスの選択
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolAudioOutput_Click(object sender, EventArgs e)
		{
			var param = new XIE.Media.CxDeviceParam();
			var aux = (XIE.Tasks.IxAuxInfoAudioOutputs)CxAuxInfoForm.AuxInfo;
			if (aux.Infos.Length > 0)
				param.CopyFrom(aux.Infos[0]);

			var dlg = new XIE.Media.CxDeviceSelectionForm(
				XIE.Media.ExMediaType.Audio,
				XIE.Media.ExMediaDir.Output,
				param);
			dlg.StartPosition = FormStartPosition.Manual;
			dlg.Location = ApiHelper.GetNeighborPosition(new Size(500, 203));
			dlg.Text = string.Format("Audio Output");
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				if (string.IsNullOrEmpty(param.Name))
					aux.RemoveAll();
				else if (aux.Infos.Length == 0)
					aux.Add(param);
				else
					aux.Infos[0] = param;
			}
		}

		#endregion

		#region toolbarView: (ClipboardObserver)

		/// <summary>
		/// toolbarView: クリップボード監視の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewClipboardObserver_Click(object sender, EventArgs e)
		{
			if (ClipboardObserver == null) return;
			ClipboardObserver.Enabled = !ClipboardObserver.Enabled;
		}

		/// <summary>
		/// ClipboardObserver: オブジェクト
		/// </summary>
		private XIE.Forms.CxClipboardObserver ClipboardObserver = null;

		/// <summary>
		/// ClipboardObserver: 通知イベント 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClipboardObserver_Notify(object sender, EventArgs e)
		{
			var args = (XIE.Forms.CxClipboardObserverEventArgs)e;
			var idata = Clipboard.GetDataObject();

			if (ApiHelper.IgnoreClipboardObserverNotifyEvent == false)
			{
#if false
				if (idata != null && idata.GetDataPresent(DataFormats.Dib))
				{
					XIE.Log.Api.Trace("ClipboardObserver_Notify: Dib [{0},{1},{2},{3}]", args.Msg.HWnd, args.Msg.WParam, args.Msg.LParam, this.Handle);

					#region クリップボードからの取得.
					try
					{
						{
							var data = XIE.AxiClipboard.ToImage();
							var info = new XIE.Tasks.CxImageInfo();
							var node = new CxImageNode(info, data);
							node.AuxInfo = CxAuxInfoForm.AuxInfo;
							node.ContextMenuStrip = DataMenu;
							node.Setup();
							string name = "Observer-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
							node.Name = name;
							node.Text = name;

							var aux = (XIE.Tasks.IxAuxInfoImages)CxAuxInfoForm.AuxInfo;
							aux.Add(info, data);
							treeAux.Nodes["Data"].Nodes["Image"].Nodes.Add(node);
							treeAux.Nodes["Data"].Nodes["Image"].Expand();
							treeAux.SelectedNode = node;
						}
					}
					catch (System.Exception ex)
					{
						XIE.Log.Api.Error(ex.StackTrace);
					}
					#endregion
				}
#endif

				if (idata != null && idata.GetDataPresent(DataFormats.Bitmap))
				{
					XIE.Log.Api.Trace("ClipboardObserver_Notify: Bitmap [{0},{1},{2},{3}]", args.Msg.HWnd, args.Msg.WParam, args.Msg.LParam, this.Handle);

					#region クリップボードからの取得.
					try
					{
						using (var bitmap = (Bitmap)Clipboard.GetData(DataFormats.Bitmap))
						{
							var data = (XIE.CxImage)bitmap;
							var info = new XIE.Tasks.CxImageInfo();
							var node = new CxImageNode(info, data);
							node.AuxInfo = CxAuxInfoForm.AuxInfo;
							node.ContextMenuStrip = DataMenu;
							node.Setup();
							string name = "Observer-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
							node.Name = name;
							node.Text = name;

							var aux = (XIE.Tasks.IxAuxInfoImages)CxAuxInfoForm.AuxInfo;
							aux.Add(info, data);
							treeAux.Nodes["Data"].Nodes["Image"].Nodes.Add(node);
							treeAux.Nodes["Data"].Nodes["Image"].Expand();
							treeAux.SelectedNode = node;
						}
					}
					catch (System.Exception ex)
					{
						XIE.Log.Api.Error(ex.StackTrace);
					}
					#endregion
				}
				else
				{
					XIE.Log.Api.Trace("ClipboardObserver_Notify: Etc [{0},{1},{2},{3}]", args.Msg.HWnd, args.Msg.WParam, args.Msg.LParam, this.Handle);
				}
			}
		}

		#endregion

		#region toolbarView: (Edit)

		/// <summary>
		/// toolbarView: コピー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewCopy_Click(object sender, EventArgs e)
		{
			if (ImageView.Image == null) return;

			try
			{
				var roi = new XIE.TxRectangleI();

				#region ROI 指定:
				if (this.ROIOverlay.Visible &&
					this.ROIOverlay.CheckValidity(this.ImageView.Image))
				{
					roi = (XIE.TxRectangleD)this.ROIOverlay.Shape;
				}
				#endregion

				try
				{
					ApiHelper.IgnoreClipboardObserverNotifyEvent = true;	// クリップボード監視通知イベントを無視する.

					using (var image = new XIE.CxImage())
					{
						image.Attach(ImageView.Image, roi);

						XIE.AxiClipboard.CopyFrom(image);
					}
				}
				finally
				{
					ApiHelper.IgnoreClipboardObserverNotifyEvent = false;
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// toolbarView: 貼り付け
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewPaste_Click(object sender, EventArgs e)
		{
			try
			{
				XIE.CxImage data = XIE.AxiClipboard.ToImage();

				#region ツリーノード追加:
				if (data != null)
				{
					var info = new XIE.Tasks.CxImageInfo();
					var node = new CxImageNode(info, data);
					node.AuxInfo = CxAuxInfoForm.AuxInfo;
					node.ContextMenuStrip = DataMenu;
					node.Setup();
					string name = "Paste-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
					node.Name = name;
					node.Text = name;

					var aux = (XIE.Tasks.IxAuxInfoImages)CxAuxInfoForm.AuxInfo;
					aux.Add(info, data);
					treeAux.Nodes["Data"].Nodes["Image"].Nodes.Add(node);
					treeAux.Nodes["Data"].Nodes["Image"].Expand();
					treeAux.SelectedNode = node;
				}
				#endregion
			}
			catch (System.Exception)
			{
			}
		}

		#endregion

		#region toolbarView: (ROI)

		/// <summary>
		/// toolbarView: ROI の表示/非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewROI_Click(object sender, EventArgs e)
		{
			ROIOverlay.Visible = !ROIOverlay.Visible;
			ROIOverlay_VisibleChanged(this, EventArgs.Empty);
			ImageView.Refresh();
		}

		/// <summary>
		/// ROI: 表示/非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ROIOverlay_VisibleChanged(object sender, EventArgs e)
		{
			if (this.ROIOverlay.Visible)
			{
				if (this.ImageView.Image == null) return;

				var roi = new XIE.TxRectangleI();

				var node = treeAux.SelectedNode as XIE.Tasks.IxAuxNodeImageOut;
				if (node != null)
					roi = node.ROI;

				#region 画像オブジェクトの roi が未初期化の場合は、現状を複製するかリセットする:
				if (roi.X < 0 || roi.Y < 0 || roi.Width <= 0 || roi.Height <= 0)
				{
					roi = (XIE.TxRectangleI)this.ROIOverlay.Shape;

					var size = this.ImageView.Image.Size;
					var uninitialized = (roi.X < 0 || roi.Y < 0 || roi.Width <= 0 || roi.Height <= 0);
					var invalid = !(0 <= roi.X && 0 <= roi.Y && roi.Width < size.Width && roi.Height < size.Height);

					if (uninitialized || invalid)
					{
						var view = this.ImageView;
						var vis = view.VisibleRectI(true);

						if (vis.Width == size.Width && vis.Height == size.Height)
						{
							roi = vis;
						}
						else
						{
							roi.X = vis.X + vis.Width / 4;
							roi.Y = vis.Y + vis.Height / 4;
							roi.Width = vis.Width / 2;
							roi.Height = vis.Height / 2;
						}
					}
				}
				#endregion

				this.ROIOverlay.Shape = roi;

				this.ROIOverlay.Grid = CxAuxInfoForm.AppSettings.ROIGrid;
				this.ROIOverlay.GridType = CxAuxInfoForm.AppSettings.ROIGridType;
				this.ROIOverlay.PenColor = CxAuxInfoForm.AppSettings.ROIPenColor;
				this.ROIOverlay.Brush = new XIE.GDI.TxGdiBrush(CxAuxInfoForm.AppSettings.ROIBkColor);
				if (CxAuxInfoForm.AppSettings.ROIBkEnable == false)
					this.ROIOverlay.BrushStyle = XIE.GDI.ExGdiBrushStyle.None;

				#region ダイアログ表示:
				this.ROIForm = new CxROIForm(this.ImageView, this.ROIOverlay, node);
				this.ROIForm.FormClosed += new FormClosedEventHandler(ROIForm_FormClosed);
				this.ROIForm.StartPosition = FormStartPosition.Manual;
				this.ROIForm.Location = this.ImageView.PointToScreen(new Point(7, 7));
				this.ROIForm.Show(this);
				#endregion
			}
			else
			{
				#region ダイアログ非表示:
				if (this.ROIForm != null)
					this.ROIForm.Close();
				#endregion
			}
		}

		/// <summary>
		/// ROI: 編集フォーム
		/// </summary>
		private CxROIForm ROIForm = null;

		/// <summary>
		/// ROI: 編集フォームが閉じたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ROIForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.ROIForm = null;
			this.ROIOverlay.Visible = false;
			this.ImageView.Refresh();
		}

		/// <summary>
		/// ROI: フォーム表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ROIForm_Update(object sender, EventArgs e)
		{
			if (this.ROIForm != null)
			{
				if (this.ROIForm.ROIOverlay == null)
					this.ROIForm.ROIOverlay = this.ROIOverlay;

				this.ROIOverlay.Grid = CxAuxInfoForm.AppSettings.ROIGrid;
				this.ROIOverlay.PenColor = CxAuxInfoForm.AppSettings.ROIPenColor;
				this.ROIOverlay.Brush = new XIE.GDI.TxGdiBrush(CxAuxInfoForm.AppSettings.ROIBkColor);
				if (CxAuxInfoForm.AppSettings.ROIBkEnable == false)
					this.ROIOverlay.BrushStyle = XIE.GDI.ExGdiBrushStyle.None;

				var roi = new XIE.TxRectangleI();

				var node = treeAux.SelectedNode as XIE.Tasks.IxAuxNodeImageOut;
				if (node != null)
					roi = node.ROI;

				#region 画像オブジェクトの roi が未初期化の場合は、現状を複製するかリセットする:
				if (this.ImageView.Image != null)
				{
					if (roi.X < 0 || roi.Y < 0 || roi.Width <= 0 || roi.Height <= 0)
					{
						roi = (XIE.TxRectangleI)this.ROIForm.ROIOverlay.Shape;

						var size = this.ImageView.Image.Size;
						var uninitialized = (roi.X < 0 || roi.Y < 0 || roi.Width <= 0 || roi.Height <= 0);
						var invalid = !(0 <= roi.X && 0 <= roi.Y && roi.Width < size.Width && roi.Height < size.Height);

						if (uninitialized || invalid)
						{
							var view = this.ImageView;
							var vis = view.VisibleRectI(true);

							if (vis.Width == size.Width && vis.Height == size.Height)
							{
								roi = vis;
							}
							else
							{
								roi.X = vis.X + vis.Width / 4;
								roi.Y = vis.Y + vis.Height / 4;
								roi.Width = vis.Width / 2;
								roi.Height = vis.Height / 2;
							}
						}
					}
				}
				#endregion

				this.ROIForm.ROIOverlay.Shape = roi;
				this.ROIForm.Node = node;
				this.ROIForm.UpdateInfo();
			}
		}

		#endregion

		#region toolbarView: (Exif)

		/// <summary>
		/// Exif: フォーム表示切替
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewExif_Click(object sender, EventArgs e)
		{
			if (ExifForm == null)
			{
				XIE.CxImage image = null;
				var node = treeAux.SelectedNode as CxImageNode;
				if (node != null)
					image = node.Data;

				ExifForm = new CxExifForm(image);
				ExifForm.Owner = this;
				ExifForm.StartPosition = FormStartPosition.Manual;
				ExifForm.Location = ExifForm_GetTypicalLocation();
				ExifForm.FormClosing += ExifForm_FormClosing;
				ExifForm.Move += ExifForm_Move;
				ExifForm.Resize += ExifForm_Resize;
				ExifForm_UpdateOffset();
				ExifForm.Show();
			}
			else if (ExifForm.Visible == false)
			{
				if (ExifForm.IsDocking)
				{
					#region プレビューフォームが可視範囲外に出たら再調整する.
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
					if (ExifForm.Location.X < st.X ||
						ExifForm.Location.Y < st.Y ||
						ExifForm.Location.X > ed.X ||
						ExifForm.Location.Y > ed.Y)
					{
						ExifForm.Location = ExifForm_GetTypicalLocation();
					}
					#endregion
				}
				ExifForm.Visible = true;
			}
			else
			{
				ExifForm.Visible = false;
				ExifForm_UpdateOffset();
			}
		}

		#endregion

		#region toolbarView: (Canvas)

		/// <summary>
		/// toolbarView: 画像全体が View に表示されるように表示倍率を調整します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewFitImageSize_ButtonClick(object sender, EventArgs e)
		{
			this.ImageView.FitImageSize(0);
			this.ImageView.Refresh();
		}

		/// <summary>
		/// toolbarView: 画像の幅を View の幅に合わせます。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewFitImageWidth_Click(object sender, EventArgs e)
		{
			this.ImageView.FitImageSize(1);
			this.ImageView.Refresh();
		}

		/// <summary>
		/// toolbarView: 画像の高さを View の幅に合わせます。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewFitImageHeight_Click(object sender, EventArgs e)
		{
			this.ImageView.FitImageSize(2);
			this.ImageView.Refresh();
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
				this.ImageView.Refresh();
			}
			else
			{
				this.ImageView.AdjustScale(0);
				this.ImageView.Refresh();
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
			this.ImageView.Refresh();
		}

		/// <summary>
		/// toolbarView: 表示倍率を拡大します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewScaleUp_Click(object sender, EventArgs e)
		{
			this.ImageView.AdjustScale(+1);
			this.ImageView.Refresh();
		}

		/// <summary>
		/// toolbarView: 伸縮時の濃度補間方法の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewHalftone_Click(object sender, EventArgs e)
		{
			this.ImageView.Halftone = !this.ImageView.Halftone;
			this.ImageView.Refresh();
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
				ImageView.Refresh();
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
				ImageView.Refresh();
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
			this.ImageView.Refresh();
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
			this.ImageView.Refresh();
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
			this.ImageView.Refresh();
		}

		#endregion

		#region toolbarView: (Overlay)

		/// <summary>
		/// toolbarView: Profile の表示/非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewProfile_Click(object sender, EventArgs e)
		{
			ProfileOverlay.Visible = !ProfileOverlay.Visible;
			ImageView.Refresh();
		}

		/// <summary>
		/// toolbarView: Grid の表示/非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewGrid_Click(object sender, EventArgs e)
		{
			GridOverlay.Visible = !GridOverlay.Visible;
			ImageView.Refresh();
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

		#region toolbarView: (Snapshot)

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
					var data = (XIE.CxImage)ImageView.Image.Clone();
					data.ExifCopy(ImageView.Image.Exif());

					var info = new XIE.Tasks.CxImageInfo();
					var node = new CxImageNode(info, data);
					node.AuxInfo = CxAuxInfoForm.AuxInfo;
					node.ContextMenuStrip = DataMenu;
					node.Setup();
					string name = "";
					if (treeAux.SelectedNode is CxCameraNode ||
						treeAux.SelectedNode is CxMediaNode ||
						treeAux.SelectedNode is CxImageNode)
					{
						var prefix = ApiHelper.GetFileNameWithoutExtensionForImage(treeAux.SelectedNode.Name);
						name = prefix + "-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
					}
					if (string.IsNullOrWhiteSpace(name))
						name = "Snap-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
					node.Name = name;
					node.Text = name;

					var aux = (XIE.Tasks.IxAuxInfoImages)CxAuxInfoForm.AuxInfo;
					aux.Add(info, data);
					treeAux.Nodes["Data"].Nodes["Image"].Nodes.Add(node);
					treeAux.Nodes["Data"].Nodes["Image"].Expand();
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
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
					var info = new XIE.Tasks.CxImageInfo();
					var node = new CxImageNode(info, data);
					node.AuxInfo = CxAuxInfoForm.AuxInfo;
					node.ContextMenuStrip = DataMenu;
					node.Setup();
					string name = "";
					if (treeAux.SelectedNode is CxCameraNode ||
						treeAux.SelectedNode is CxMediaNode ||
						treeAux.SelectedNode is CxImageNode)
					{
						var prefix = ApiHelper.GetFileNameWithoutExtensionForImage(treeAux.SelectedNode.Name);
						name = prefix + "-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
					}
					if (string.IsNullOrWhiteSpace(name))
						name = "Overlay-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
					node.Name = name;
					node.Text = name;

					var aux = (XIE.Tasks.IxAuxInfoImages)CxAuxInfoForm.AuxInfo;
					aux.Add(info, data);
					treeAux.Nodes["Data"].Nodes["Image"].Nodes.Add(node);
					treeAux.Nodes["Data"].Nodes["Image"].Expand();
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		#endregion

		#region toolbarView: (Preview)

		/// <summary>
		/// プレビューウィンドウの表示切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPreview_Click(object sender, EventArgs e)
		{
			if (PreviewForm == null)
			{
				PreviewForm = new CxImagePreviewForm(ImageView);
				PreviewForm.Owner = this;
				PreviewForm.StartPosition = FormStartPosition.Manual;
				PreviewForm.Location = PreviewForm_GetTypicalLocation();
				PreviewForm.FormClosing += PreviewForm_FormClosing;
				PreviewForm.Move += PreviewForm_Move;
				PreviewForm.Resize += PreviewForm_Resize;
				PreviewForm_UpdateOffset();
				PreviewForm.Show();
			}
			else if (PreviewForm.Visible == false)
			{
				if (PreviewForm.IsDocking)
				{
					#region プレビューフォームが可視範囲外に出たら再調整する.
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
					if (PreviewForm.Location.X < st.X ||
						PreviewForm.Location.Y < st.Y ||
						PreviewForm.Location.X > ed.X ||
						PreviewForm.Location.Y > ed.Y)
					{
						PreviewForm.Location = PreviewForm_GetTypicalLocation();
					}
					#endregion
				}
				PreviewForm.Visible = true;
			}
			else
			{
				PreviewForm.Visible = false;
				PreviewForm_UpdateOffset();
			}
		}

		#endregion

		#region toolbarView: (Fullscreen)

		/// <summary>
		/// toolbarView: フルスクリーン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFullscreen_Click(object sender, EventArgs e)
		{
			ChangeFullscreenMode(!this.IsFullscreen);
		}

		#endregion

		// //////////////////////////////////////////////////
		// statusbar
		// //////////////////////////////////////////////////

		#region statusbar: (表示更新)

		/// <summary>
		/// ステータスバー表示更新:
		/// </summary>
		private void statusbar_Update()
		{
			statusbar_Update_WorkingSet();
			statusbar_Update_ImageView();
			statusbar_Update_ImageCursor();
		}

		/// <summary>
		/// ワーキングセットの更新
		/// </summary>
		private void statusbar_Update_WorkingSet()
		{
			var now = DateTime.Now;
			bool update_each_second = (this.PreviousSecond != now.Second);
			if (update_each_second)
				this.PreviousSecond = now.Second;

			#region プロセス情報.
			if (update_each_second)
			{
				Process proc = Process.GetCurrentProcess();
				long lnPF = proc.WorkingSet64 / 1024;		// プロセスの使用量 (KB)
				statusWorkingSet.Text = string.Format("({0:N0} KB)", lnPF);
			}
			#endregion
		}
		private int PreviousSecond = 0;

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

		/// <summary>
		/// ステータスバー表示更新: (ImageCursor)
		/// </summary>
		private void statusbar_Update_ImageCursor()
		{
			var selected_node = treeAux.SelectedNode;
			if (selected_node is XIE.Tasks.IxAuxNodeImageOut)
			{
				toolImageName.Visible = true;
				toolImagePrev.Visible = true;
				toolImageNext.Visible = true;

				if (toolImageName.Text != selected_node.Text)
					toolImageName.Text = selected_node.Text;
			}
			else
			{
				toolImageName.Visible = false;
				toolImagePrev.Visible = false;
				toolImageNext.Visible = false;
			}
		}

		/// <summary>
		/// ImageCursor: 前のノード
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolImagePrev_Click(object sender, EventArgs e)
		{
			var selected_node = treeAux.SelectedNode;
			if (selected_node is XIE.Tasks.IxAuxNodeImageOut)
			{
				var prev_node = selected_node.PrevNode;
				if (prev_node != null && prev_node.Level == selected_node.Level)
				{
					treeAux.SelectedNode = prev_node;
				}
			}
		}

		/// <summary>
		/// ImageCursor: 次のノード
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolImageNext_Click(object sender, EventArgs e)
		{
			var selected_node = treeAux.SelectedNode;
			if (selected_node is XIE.Tasks.IxAuxNodeImageOut)
			{
				var next_node = selected_node.NextNode;
				if (next_node != null && next_node.Level == selected_node.Level)
				{
					treeAux.SelectedNode = next_node;
				}
			}
		}

		#endregion

		#region statusbar: (コントロールイベント)

		/// <summary>
		/// ツリービューの表示・非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void statusTreeView_Click(object sender, EventArgs e)
		{
			splitVert.Panel1Collapsed = !splitVert.Panel1Collapsed;
		}

		/// <summary>
		/// リストビューの表示・非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void statusListView_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// GC.Collect の実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void statusGCcollect_Click(object sender, EventArgs e)
		{
			GC.Collect();
		}

		#endregion

		// //////////////////////////////////////////////////
		// CxAuxInfoForm
		// //////////////////////////////////////////////////

		#region CxAuxInfoForm: (ドラッグ＆ドロップ)

		/// <summary>
		/// CxAuxInfoForm: ドラッグされた項目が Form 内に入ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxAuxInfoForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
				return;
			}
		}

		/// <summary>
		/// CxAuxInfoForm: ドラッグされた項目が Form 上にドロップされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxAuxInfoForm_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				#region ファイルがドロップされた時.
				try
				{
					var drops = (string[])e.Data.GetData(DataFormats.FileDrop, false);
					var firstnode  = LoadDataFiles(drops);
					if (firstnode != null)
						treeAux.SelectedNode = firstnode;
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				#endregion
			}
		}

		#endregion

		#region CxAuxInfoForm: (コントロールイベント)

		/// <summary>
		/// CxAuxInfoForm: フォームがアクティブになった時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxAuxInfoForm_Activated(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// CxAuxInfoForm: フォームが非アクティブになった時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxAuxInfoForm_Deactivate(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// CxAuxInfoForm: フォームが移動した時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxAuxInfoForm_Move(object sender, EventArgs e)
		{
			PreviewForm_Follow();
			ExifForm_Follow();
		}

		/// <summary>
		/// CxAuxInfoForm: フォームがサイズ変更した時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxAuxInfoForm_Resize(object sender, EventArgs e)
		{
			if (this.IsFullscreen == false)
			{
				if (this.Visible)
				{
					if (this.WindowState == FormWindowState.Normal)
						this.PrevFormSize = this.Size;
					else
						this.PrevFormSize = this.RestoreBounds.Size;
				}
			}
		}

		#endregion

		#region CxAuxInfoForm: (キーボードイベント)

		/// <summary>
		/// CxAuxInfoForm: キーボード押下
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxAuxInfoForm_KeyDown(object sender, KeyEventArgs e)
		{
			#region 保存: [CTRL+S]
			if (e.KeyCode == Keys.S && !e.Alt && e.Control && !e.Shift)
			{
				if (treeAux.SelectedNode is CxImageNode)
				{
					e.Handled = true;
					menuDataSave_Click(sender, e);
					return;
				}
				if (treeAux.SelectedNode is CxTaskNode)
				{
					e.Handled = true;
					menuTaskSave_Click(sender, e);
					return;
				}
			}
			#endregion

			#region 画像切り替え: [CTRL+Tab]
			if (treeAux.SelectedNode is CxImageNode)
			{
				// 次: Ctrl+[Tab]
				if (e.KeyCode == Keys.Tab && !e.Alt && e.Control && !e.Shift)
				{
					e.Handled = true;
					var node = treeAux.SelectedNode.NextNode;
					if (node == null)
						node = treeAux.SelectedNode.Parent.FirstNode;
					if (node != null)
						treeAux.SelectedNode = node;
					return;
				}
				// 前: Ctrl+Shift+[Tab]
				if (e.KeyCode == Keys.Tab && !e.Alt && e.Control && e.Shift)
				{
					e.Handled = true;
					var node = treeAux.SelectedNode.PrevNode;
					if (node == null)
						node = treeAux.SelectedNode.Parent.LastNode;
					if (node != null)
						treeAux.SelectedNode = node;
					return;
				}
			}
			#endregion

			#region [F11] フルスクリーン:
			if (e.KeyCode == Keys.F11 && !e.Alt && !e.Control && !e.Shift)
			{
				e.Handled = true;
				ChangeFullscreenMode(!this.IsFullscreen);
				return;
			}
			#endregion

			#region [ESC] フルスクリーン解除:
			if (e.KeyCode == Keys.Escape && !e.Alt && !e.Control && !e.Shift)
			{
				e.Handled = true;
				if (this.IsFullscreen)
					ChangeFullscreenMode(false);
				return;
			}
			#endregion
		}

		/// <summary>
		/// CxAuxInfoForm: キーボード押下
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxAuxInfoForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
		}

		#endregion

		// //////////////////////////////////////////////////
		// treeAux
		// //////////////////////////////////////////////////

		#region treeAux: (初期化と解放)

		/// <summary>
		/// treeAux: 初期化
		/// </summary>
		private void treeAux_Setup()
		{
			var icons = (XIE.Tasks.IxAuxImageList16)CxAuxInfoForm.AuxInfo;
			treeAux.ImageList = icons.ImageList;
			treeAux.PaintEx += treeAux_PaintEx;

			#region ツリービューの初期化: (Device)
			{
				string category = "Device";
				var folder = new TreeNode();
				folder.Name = category;
				folder.Text = category;
				folder.ImageKey = "Node-Book-Closed";
				folder.SelectedImageKey = "Node-Book-Opened";
				treeAux.Nodes.Add(folder);
				FolderNodes[category] = folder;

				KeyValuePair<string, ContextMenuStrip>[] items = 
				{
					new KeyValuePair<string, ContextMenuStrip>("Camera", CameraMenu),
					new KeyValuePair<string, ContextMenuStrip>("SerialPort", SerialPortMenu),
					new KeyValuePair<string, ContextMenuStrip>("TcpServer", NetworkMenu),
					new KeyValuePair<string, ContextMenuStrip>("TcpClient", NetworkMenu),
				};

				foreach (var item in items)
				{
					var node = new TreeNode();
					node.Name = item.Key;
					node.Text = item.Key;
					node.ImageKey = "Node-Book-Closed";
					node.SelectedImageKey = "Node-Book-Opened";
					node.ContextMenuStrip = item.Value;
					folder.Nodes.Add(node);
					FolderNodes[item.Key] = node;
				}
				folder.Expand();
			}
			#endregion

			#region ツリービューの初期化: (Data)
			{
				string category = "Data";
				var folder = new TreeNode();
				folder.Name = category;
				folder.Text = category;
				folder.ImageKey = "Node-Book-Closed";
				folder.SelectedImageKey = "Node-Book-Opened";
				folder.ContextMenuStrip = DataMenu;
				treeAux.Nodes.Add(folder);
				FolderNodes[category] = folder;

				KeyValuePair<string, ContextMenuStrip>[] items = 
				{
					new KeyValuePair<string, ContextMenuStrip>("Media", DataMenu),
					new KeyValuePair<string, ContextMenuStrip>("Image", DataMenu),
				};

				foreach (var item in items)
				{
					var node = new TreeNode();
					node.Name = item.Key;
					node.Text = item.Key;
					node.ImageKey = "Node-Book-Closed";
					node.SelectedImageKey = "Node-Book-Opened";
					node.ContextMenuStrip = item.Value;
					folder.Nodes.Add(node);
					FolderNodes[item.Key] = node;
				}
				folder.Expand();
			}
			#endregion

			#region ツリービューの初期化: (Tasks)
			{
				string category = "Tasks";
				var folder = new TreeNode();
				folder.Name = category;
				folder.Text = category;
				folder.ImageKey = "Node-Book-Closed";
				folder.SelectedImageKey = "Node-Book-Opened";
				folder.ContextMenuStrip = TaskMenu;
				treeAux.Nodes.Add(folder);
				FolderNodes[category] = folder;
				folder.Expand();
			}
			#endregion

			if (treeAux.Nodes.Count > 1)
				treeAux.SelectedNode = treeAux.Nodes[0];
		}

		/// <summary>
		/// treeAux: 解放
		/// </summary>
		private void treeAux_Dispose()
		{
			treeAux.PaintEx -= treeAux_PaintEx;
		}

		#endregion

		#region treeAux: (カスタム描画)

		/// <summary>
		/// treeAux: 再描画イベントが発生したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeAux_PaintEx(object sender, PaintEventArgs e)
		{
			int margin = 0;

			var nodes = treeAux.GetAllNodes();
			using (var pen = new Pen(treeAux.BackColor))
			{
				foreach (var node in nodes)
				{
					var bounds = node.Bounds;
					bounds.X -= margin;
					bounds.Y -= margin;
					bounds.Width += margin * 2;
					bounds.Height += margin * 2;
					e.Graphics.DrawRectangle(pen, bounds);
				}
			}

			var selected_node = treeAux.SelectedNode;
			if (selected_node != null)
			{
				using (var pen = new Pen(Color.Red))
				{
					var bounds = selected_node.Bounds;
					bounds.X -= margin;
					bounds.Y -= margin;
					bounds.Width += margin * 2;
					bounds.Height += margin * 2;
					e.Graphics.DrawRectangle(pen, bounds);
				}
			}
		}

		#endregion

		#region treeAux: (コントロールイベント)

		/// <summary>
		/// treeAux: ノードが選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeAux_AfterSelect(object sender, TreeViewEventArgs e)
		{
			#region ImageView の表示更新.
			if (treeAux.SelectedNode is XIE.Tasks.IxAuxNodeImageOut)
			{
				var node = (XIE.Tasks.IxAuxNodeImageOut)treeAux.SelectedNode;
				node.Draw(this.ImageView);
			}
			#endregion

			#region ROI 表示更新:
			if (this.ROIForm != null)
			{
				ROIForm_Update(sender, e);
			}
			#endregion

			#region Exif 表示更新:
			if (this.ExifForm != null)
			{
				ExifForm_Update(sender, e);
			}
			#endregion
		}

		/// <summary>
		/// treeAux: ノードがマウスで選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeAux_MouseDown(object sender, MouseEventArgs e)
		{
			TreeViewHitTestInfo info = treeAux.HitTest(e.Location);
			if (info != null && info.Node != null)
			{
				treeAux.SelectedNode = info.Node;

				#region 右クリック時、コンテキストメニュー表示.
				// ※ここで右クリックされた項目を選択したいので、treeAux.ContextMenu に割り当てていません.
				if (e.Button == MouseButtons.Right)
				{
					if (info.Node != null && info.Node.ContextMenuStrip != null)
					{
						Point neighbor = treeAux.PointToScreen(e.Location);
						info.Node.ContextMenuStrip.Show(neighbor);
					}
				}
				#endregion
			}
			else
			{
				treeAux.SelectedNode = null;
			}
		}

		/// <summary>
		/// treeAux: ノードがダブルクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeAux_DoubleClick(object sender, EventArgs e)
		{
			TreeViewHitTestInfo info = treeAux.HitTest(treeAux.PointToClient(Cursor.Position));
			if (info != null && info.Node != null)
			{
				treeAux.SelectedNode = info.Node;

				#region ダブルクリック時は既定の動作を行います.
				try
				{
					if (info.Node is XIE.Tasks.IxAuxNode)
					{
						var node = (XIE.Tasks.IxAuxNode)info.Node;
						node.DoubleClick(this, e);
					}
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				#endregion
			}
		}

		/// <summary>
		/// treeAux: キーボードが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeAux_KeyDown(object sender, KeyEventArgs e)
		{
			#region 保存: [CTRL+S]
			if (e.KeyCode == Keys.S && !e.Alt && e.Control && !e.Shift)
			{
				if (treeAux.SelectedNode is CxImageNode)
				{
					e.Handled = true;
					menuDataSave_Click(sender, e);
					return;
				}
			}
			#endregion

			#region 削除: [Delete]
			if (e.KeyCode == Keys.Delete && !e.Alt && !e.Control && !e.Shift)
			{
				e.Handled = true;
				treeAux_RemoveNode();
				return;
			}
			#endregion

			#region CTRL+[E] Exif 表示切替:
			if (e.KeyCode == Keys.E && !e.Alt && e.Control && !e.Shift)
			{
				toolViewExif_Click(sender, e);
			}
			#endregion
		}

		/// <summary>
		/// treeAux: キーボードが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeAux_KeyPress(object sender, KeyPressEventArgs e)
		{
			// BEEP 音の対策.
			// TreeView のノードにフォーカスがある場合は、
			// KeyDown ハンドラで e.Handled = true にしても効果がない.
			e.Handled = true;
		}

		#endregion

		#region treeAux: (ノード追加) ※ 一括ファイル読み込み.

		/// <summary>
		/// treeAux: ファイル読み込み
		/// </summary>
		/// <param name="filenames">読み込むファイルのコレクション</param>
		/// <returns>
		///		最初に追加されたノードを返します。
		/// </returns>
		private TreeNode LoadDataFiles(IEnumerable<string> filenames)
		{
			var errors = new List<string>();
			TreeNode firstnode = null;
			foreach (string filename in filenames)
			{
				try
				{
					if (filename.EndsWith(".avi", StringComparison.InvariantCultureIgnoreCase) ||
						filename.EndsWith(".asf", StringComparison.InvariantCultureIgnoreCase) ||
						filename.EndsWith(".wmv", StringComparison.InvariantCultureIgnoreCase) ||
						filename.EndsWith(".mp4", StringComparison.InvariantCultureIgnoreCase))
					{
						#region 新規ノード.
						if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
						{
							XIE.Media.CxDeviceParam audioOutput = null;
							var audioOutputs = (XIE.Tasks.IxAuxInfoAudioOutputs)CxAuxInfoForm.AuxInfo;
							if (audioOutputs.Infos.Length != 0)
								audioOutput = audioOutputs.Infos[0];

							var info = new XIE.Tasks.CxMediaInfo(filename);
							var player = info.CreateMedia(audioOutput);
							var node = new CxMediaNode(info, player);
							node.ContextMenuStrip = MediaMenu;
							node.Setup();

							var aux = (XIE.Tasks.IxAuxInfoMedias)CxAuxInfoForm.AuxInfo;
							aux.Add(info, player);
							FolderNodes["Media"].Nodes.Add(node);
							FolderNodes["Media"].Expand();

							if (firstnode == null)
								firstnode = node;
						}
						else
						{
							throw new System.NotSupportedException("The Media can only be used with the Windows version.");
						}
						#endregion
					}
					if (filename.EndsWith(".bmp", StringComparison.InvariantCultureIgnoreCase) ||
						filename.EndsWith(".dib", StringComparison.InvariantCultureIgnoreCase) ||
						filename.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase) ||
						filename.EndsWith(".jpeg", StringComparison.InvariantCultureIgnoreCase) ||
						filename.EndsWith(".tif", StringComparison.InvariantCultureIgnoreCase) ||
						filename.EndsWith(".tiff", StringComparison.InvariantCultureIgnoreCase) ||
						filename.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
					{
						var data = new XIE.CxImage(filename);
						var info = new XIE.Tasks.CxImageInfo(filename);
						var node = new CxImageNode(info, data);
						node.AuxInfo = CxAuxInfoForm.AuxInfo;
						node.ContextMenuStrip = DataMenu;
						node.Setup();

						var aux = (XIE.Tasks.IxAuxInfoImages)CxAuxInfoForm.AuxInfo;
						aux.Add(info, data);
						treeAux.Nodes["Data"].Nodes["Image"].Nodes.Add(node);
						treeAux.Nodes["Data"].Nodes["Image"].Expand();

						if (firstnode == null)
							firstnode = node;
					}
					if (filename.EndsWith(".raw", StringComparison.InvariantCultureIgnoreCase))
					{
						// TODO: 本来は XIE.Axi.CheckRaw で種類を確認しなければならない.
						var header = XIE.Axi.CheckRaw(filename);
						var clsname = Encoding.ASCII.GetString(header.ClassName).Trim('\0');
						if (clsname == "CxImage")
						{
							var data = new XIE.CxImage(filename);
							var info = new XIE.Tasks.CxImageInfo(filename);
							var node = new CxImageNode(info, data);
							node.AuxInfo = CxAuxInfoForm.AuxInfo;
							node.ContextMenuStrip = DataMenu;
							node.Setup();

							var aux = (XIE.Tasks.IxAuxInfoImages)CxAuxInfoForm.AuxInfo;
							aux.Add(info, data);
							treeAux.Nodes["Data"].Nodes["Image"].Nodes.Add(node);
							treeAux.Nodes["Data"].Nodes["Image"].Expand();

							if (firstnode == null)
								firstnode = node;
						}
					}
					if (filename.EndsWith(".xtf", StringComparison.InvariantCultureIgnoreCase))
					{
						var info = new XIE.Tasks.CxTaskUnitInfo(filename);
						var task = (XIE.Tasks.CxTaskflow)info.CreateTaskUnit();
						var node = new CxTaskNode(info, task);
						node.AuxInfo = CxAuxInfoForm.AuxInfo;
						node.ContextMenuStrip = TaskMenu;
						node.Setup();

						var aux = (XIE.Tasks.IxAuxInfoTasks)CxAuxInfoForm.AuxInfo;
						aux.Add(info, task);
						treeAux.Nodes["Tasks"].Nodes.Add(node);
						node.EnsureVisible();
					}
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.StackTrace);

					var filename_name = System.IO.Path.GetFileName(filename);
					errors.Add(string.Format("{0}\n  {1}\n\n", filename_name, ex.Message));
				}
			}

			if (errors.Count > 0)
			{
				var msg = new StringBuilder();
				try
				{
					foreach (var item in errors)
						msg.Append(item);

					MessageBox.Show(this, msg.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					msg.Clear();
				}
			}

			return firstnode;
		}

		#endregion

		#region treeAux: (ノード削除)

		/// <summary>
		/// treeAux: ノード配下の削除
		/// </summary>
		private void treeAux_RemoveNode()
		{
			if (ReferenceEquals(treeAux.SelectedNode, treeAux.Nodes["Data"]))
			{
				treeAux_RemoveNode(FolderNodes["Media"]);
				treeAux_RemoveNode(FolderNodes["Image"]);
			}
			else
			{
				treeAux_RemoveNode(treeAux.SelectedNode);
			}
		}

		/// <summary>
		/// treeAux: ノード配下の削除
		/// </summary>
		/// <param name="target_node"></param>
		private void treeAux_RemoveNode(TreeNode target_node)
		{
			// -----
			if (ReferenceEquals(target_node, FolderNodes["Camera"]))
			{
				#region Camera 配下の全ての子ノード.
				{
					var nodes = new List<CxCameraNode>();
					foreach (var node in target_node.Nodes)
					{
						if (node is CxCameraNode)
							nodes.Add((CxCameraNode)node);
					}
					foreach (CxCameraNode node in nodes)
					{
						node.Reset(this.ImageView);
						node.Disconnect(CxAuxInfoForm.AuxInfo);
					}
					nodes.Clear();
					this.ImageView.Refresh();
					return;
				}
				#endregion
			}
			if (target_node is CxCameraNode)
			{
				#region Camera 配下の選択されたノードのみ.
				{
					var node = target_node as CxCameraNode;
					if (node != null)
					{
						target_node = null;
						node.Reset(this.ImageView);
						node.Disconnect(CxAuxInfoForm.AuxInfo);
					}
					this.ImageView.Refresh();
					return;
				}
				#endregion
			}
			if (target_node is CxDataPortNode)
			{
				#region DataPort 配下の選択されたノードのみ.
				{
					var node = target_node as CxDataPortNode;
					if (node != null)
						node.Disconnect(CxAuxInfoForm.AuxInfo);
					if (ReferenceEquals(node, target_node))
						target_node = null;
					return;
				}
				#endregion
			}
			// -----
			if (ReferenceEquals(target_node, FolderNodes["SerialPort"]))
			{
				#region SerialPort 配下の全ての子ノード.
				{
					var nodes = new List<CxSerialPortNode>();
					foreach (var node in FolderNodes["SerialPort"].Nodes)
					{
						if (node is CxSerialPortNode)
							nodes.Add((CxSerialPortNode)node);
					}
					foreach (CxSerialPortNode node in nodes)
					{
						node.Disconnect(CxAuxInfoForm.AuxInfo);
						if (ReferenceEquals(node, target_node))
							target_node = null;
					}
					nodes.Clear();
					return;
				}
				#endregion
			}
			if (target_node is CxSerialPortNode)
			{
				#region SerialPort 配下の選択されたノードのみ.
				{
					var node = target_node as CxSerialPortNode;
					if (node != null)
						node.Disconnect(CxAuxInfoForm.AuxInfo);
					if (ReferenceEquals(node, target_node))
						target_node = null;
					return;
				}
				#endregion
			}
			// -----
			if (ReferenceEquals(target_node, FolderNodes["TcpServer"]))
			{
				#region TcpServer 配下の全ての子ノード.
				{
					var nodes = new List<CxTcpServerNode>();
					foreach (CxTcpServerNode node in FolderNodes["TcpServer"].Nodes)
						nodes.Add(node);
					foreach (CxTcpServerNode node in nodes)
					{
						node.Disconnect(CxAuxInfoForm.AuxInfo);
						if (ReferenceEquals(node, target_node))
							target_node = null;
					}
					nodes.Clear();
					return;
				}
				#endregion
			}
			if (target_node is CxTcpServerNode)
			{
				#region TcpServer 配下の選択されたノードのみ.
				{
					var node = target_node as CxTcpServerNode;
					node.Disconnect(CxAuxInfoForm.AuxInfo);
					if (ReferenceEquals(node, target_node))
						target_node = null;
					return;
				}
				#endregion
			}
			// -----
			if (ReferenceEquals(target_node, FolderNodes["TcpClient"]))
			{
				#region TcpClient 配下の全ての子ノード.
				{
					var nodes = new List<CxTcpClientNode>();
					foreach (CxTcpClientNode node in FolderNodes["TcpClient"].Nodes)
						nodes.Add(node);
					foreach (CxTcpClientNode node in nodes)
					{
						node.Disconnect(CxAuxInfoForm.AuxInfo);
						if (ReferenceEquals(node, target_node))
							target_node = null;
					}
					nodes.Clear();
					return;
				}
				#endregion
			}
			if (target_node is CxTcpClientNode)
			{
				#region TcpClient 配下の選択されたノードのみ.
				{
					var node = target_node as CxTcpClientNode;
					node.Disconnect(CxAuxInfoForm.AuxInfo);
					if (ReferenceEquals(node, target_node))
						target_node = null;
					return;
				}
				#endregion
			}
			// -----
			if (ReferenceEquals(target_node, FolderNodes["Media"]))
			{
				#region Media 配下の全ての子ノード.
				{
					var nodes = new List<CxMediaNode>();
					foreach (var node in target_node.Nodes)
						nodes.Add((CxMediaNode)node);
					foreach (var node in nodes)
					{
						node.Reset(this.ImageView);
						node.Disconnect(CxAuxInfoForm.AuxInfo);
					}
					this.ImageView.Refresh();
					return;
				}
				#endregion
			}
			if (target_node is CxMediaNode)
			{
				#region Media 配下の選択されたノードのみ.
				{
					var node = target_node as CxMediaNode;
					node.Reset(this.ImageView);
					node.Disconnect(CxAuxInfoForm.AuxInfo);
					this.ImageView.Refresh();
					return;
				}
				#endregion
			}
			// -----
			if (ReferenceEquals(target_node, FolderNodes["Image"]))
			{
				#region ファイル保存済みか否かを検証します.
				{
					var nodes = new List<CxImageNode>();

					var folder = target_node;
					foreach (var item in folder.Nodes)
					{
						if (item is CxImageNode)
						{
							var node = (CxImageNode)item;
							if (node.UnsavedDataExists)
								nodes.Add(node);
						}
					}

					if (nodes.Count > 0)
					{
						var ans = MessageBox.Show(
							"Unsaved data exists. Do you want to save?",
							"Question",
							MessageBoxButtons.YesNoCancel,
							MessageBoxIcon.Question);

						switch (ans)
						{
							default:
							case DialogResult.Cancel:
								return;
							case DialogResult.No:
								break;
							case DialogResult.Yes:
								foreach (var node in nodes)
								{
									node.OpenSaveDialog(this);
								}
								break;
						}
					}
				}
				#endregion

				#region Image 配下の全ての子ノード.
				{
					var nodes = new List<CxImageNode>();
					foreach (var node in target_node.Nodes)
						nodes.Add((CxImageNode)node);
					foreach (var node in nodes)
					{
						node.Reset(this.ImageView);
						node.Disconnect(CxAuxInfoForm.AuxInfo);
					}
					this.ImageView.Refresh();
					return;
				}
				#endregion
			}
			if (target_node is CxImageNode)
			{
				#region ファイル保存済みか否かを検証します.
				{
					var node = target_node as CxImageNode;
					if (node.UnsavedDataExists)
					{
						var ans = MessageBox.Show(
							"Data is not saved. Do you want to save?",
							"Question",
							MessageBoxButtons.YesNoCancel,
							MessageBoxIcon.Question);

						switch (ans)
						{
							default:
							case DialogResult.Cancel:
								return;
							case DialogResult.No:
								break;
							case DialogResult.Yes:
								node.OpenSaveDialog(this);
								break;
						}
					}
				}
				#endregion

				#region Image 配下の選択されたノードのみ.
				{
					var node = target_node as CxImageNode;
					node.Reset(this.ImageView);
					node.Disconnect(CxAuxInfoForm.AuxInfo);
					this.ImageView.Refresh();
					return;
				}
				#endregion
			}
			// -----
			if (ReferenceEquals(target_node, FolderNodes["Tasks"]))
			{
				#region ファイル保存済みか否かを検証します.
				{
					var nodes = new List<CxTaskNode>();

					var folder = target_node;
					foreach (var item in folder.Nodes)
					{
						if (item is CxTaskNode)
						{
							var node = (CxTaskNode)item;
							if (node.UnsavedDataExists)
								nodes.Add(node);
						}
					}

					if (nodes.Count > 0)
					{
						var ans = MessageBox.Show(
							"Unsaved data exists. Do you want to save?",
							"Question",
							MessageBoxButtons.YesNoCancel,
							MessageBoxIcon.Question);

						switch (ans)
						{
							default:
							case DialogResult.Cancel:
								return;
							case DialogResult.No:
								break;
							case DialogResult.Yes:
								foreach (var node in nodes)
								{
									node.OpenSaveDialog(this);
								}
								break;
						}
					}
				}
				#endregion

				#region Tasks 配下の全ての子ノード.
				{
					this.ImageView.Image = null;
					var nodes = new List<CxTaskNode>();
					foreach (var node in target_node.Nodes)
						nodes.Add((CxTaskNode)node);
					foreach (var node in nodes)
					{
						node.Reset(this.ImageView);
						node.Disconnect(CxAuxInfoForm.AuxInfo);
					}
					this.ImageView.Refresh();
					return;
				}
				#endregion
			}
			if (target_node is CxTaskNode)
			{
				#region ファイル保存済みか否かを検証します.
				{
					var node = target_node as CxTaskNode;
					if (node.UnsavedDataExists)
					{
						var ans = MessageBox.Show(
							"Data is not saved. Do you want to save?",
							"Question",
							MessageBoxButtons.YesNoCancel,
							MessageBoxIcon.Question);

						switch (ans)
						{
							default:
							case DialogResult.Cancel:
								return;
							case DialogResult.No:
								break;
							case DialogResult.Yes:
								node.OpenSaveDialog(this);
								break;
						}
					}
				}
				#endregion

				#region Tasks 配下の選択されたノードのみ.
				{
					var node = target_node as CxTaskNode;
					node.Reset(this.ImageView);
					node.Disconnect(CxAuxInfoForm.AuxInfo);
					this.ImageView.Refresh();
					return;
				}
				#endregion
			}
		}

		#endregion

		// //////////////////////////////////////////////////
		// カメラデバイス
		// //////////////////////////////////////////////////

		#region カメラデバイス: (初期化と解放)

		/// <summary>
		/// カメラデバイス: ツリーノード初期化.
		/// </summary>
		private void CameraNode_Setup()
		{
			var nodes = CxCameraNode.Connect(CxAuxInfoForm.AuxInfo);
			var folder = FolderNodes["Camera"];
			foreach (var node in nodes)
			{
				node.ContextMenuStrip = CameraMenu;
				folder.Nodes.Add(node);
			}
			folder.Expand();
		}

		/// <summary>
		/// カメラデバイス: ツリーノード解放.
		/// </summary>
		private void CameraNode_Dispose()
		{
			var folder = FolderNodes["Camera"];
			foreach (var item in folder.Nodes)
			{
				if (item is IDisposable)
					((IDisposable)item).Dispose();
			}
			folder.Nodes.Clear();
		}

		#endregion

		#region カメラデバイス: (CameraMenu)

		/// <summary>
		/// CameraMenu: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CameraMenu_Opened(object sender, EventArgs e)
		{
			#region メニューの表示更新.
			CxCameraNode node = treeAux.SelectedNode as CxCameraNode;
			bool enable = (node != null && node.Controller != null);
			bool is_running = (enable && node.IsRunning);
			bool is_win32nt = (System.Environment.OSVersion.Platform == PlatformID.Win32NT);
			bool audio_connected = false;
			try
			{
				if (enable)
					audio_connected = (bool)node.Controller.GetParam("Audio.Connected");
			}
			catch (System.Exception)
			{
			}
	
			{
				menuCameraSaveGRF.Enabled = (enable && is_running == false && is_win32nt);
				menuCameraRecord.Enabled = (enable && is_running == false && is_win32nt);
				menuCameraStart.Enabled = (enable && is_running == false);
				menuCameraStop.Enabled = (enable && is_running == true);
				menuCameraProperty.Enabled = (enable);
				menuCameraPropertyAudio.Enabled = (enable && is_win32nt && audio_connected);
				menuCameraPropertyVideo.Enabled = (enable && is_win32nt);
			}
			#endregion
		}

		/// <summary>
		/// CameraMenu: デバイスのオープン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuCameraOpen_Click(object sender, EventArgs e)
		{
			bool is_parent = ReferenceEquals(treeAux.SelectedNode, FolderNodes["Camera"]);
			if (is_parent)
			{
				#region デバイスのオープン.
				try
				{
					var node = new CxCameraNode();
					node.AuxInfo = CxAuxInfoForm.AuxInfo;
					node.ContextMenuStrip = CameraMenu;
					if (node.OpenSelectDialog(this, CxAuxInfoForm.AuxInfo) == DialogResult.OK)
					{
						FolderNodes["Camera"].Nodes.Add(node);
						FolderNodes["Camera"].Expand();
					}
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				#endregion
			}
			else
			{
				#region 選択されたノードの更新.
				if (treeAux.SelectedNode is CxCameraNode)
				{
					try
					{
						CxCameraNode node = treeAux.SelectedNode as CxCameraNode;
						bool is_running = node.IsRunning;
						if (is_running)
							node.Stop();
						if (node.OpenSelectDialog(this, CxAuxInfoForm.AuxInfo) == DialogResult.OK)
						{
						}
						if (is_running)
							node.Start();
					}
					catch (System.Exception ex)
					{
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				#endregion
			}
		}

		/// <summary>
		/// CameraMenu: GRF ファイルの保存
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuCameraSaveGRF_Click(object sender, EventArgs e)
		{
			var node = treeAux.SelectedNode as CxCameraNode;
			if (node != null && node.Controller != null)
			{
				#region SaveFileDialog
				var dlg = new SaveFileDialog();
				{
					var now = DateTime.Now;
					var node_name = ApiHelper.MakeValidFileName(node.Name, "_");
					var timestamp = ApiHelper.MakeFileNameSuffix(now, false);
					var filename = string.Format("{0}-{1}.GRF", node_name, timestamp);

					dlg.Filter = "GRF files |*.GRF";
					dlg.OverwritePrompt = true;
					dlg.AddExtension = true;
					dlg.FileName = filename;
					if (XIE.Tasks.SharedData.ProjectDir != "")
						dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);
				}
				#endregion

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					node.Controller.SetParam("SaveGraphFile", dlg.FileName);
				}
			}
		}

		/// <summary>
		/// CameraMenu: カメラの切断
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuCameraClose_Click(object sender, EventArgs e)
		{
			treeAux_RemoveNode();
		}

		#endregion

		#region カメラデバイス: (Record/Start/Stop/Abort)

		/// <summary>
		/// カメラデバイス: Record
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuCameraRecord_Click(object sender, EventArgs e)
		{
			CxCameraNode node = treeAux.SelectedNode as CxCameraNode;
			if (node != null)
			{
				if (node.Controller.IsRunning)
					node.Controller.Stop();
				else
					node.SetupRecording(!node.IsRecording);

				if (node.IsRecording)
					node.Controller.Start();
			}
		}

		/// <summary>
		/// カメラデバイス: Start
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuCameraStart_Click(object sender, EventArgs e)
		{
			CxCameraNode node = treeAux.SelectedNode as CxCameraNode;
			if (node != null)
			{
				if (node.IsRunning)
					node.Stop();
				else
					node.Start();
			}
		}

		/// <summary>
		/// カメラデバイス: Abort
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuCameraStop_Click(object sender, EventArgs e)
		{
			CxCameraNode node = treeAux.SelectedNode as CxCameraNode;
			if (node != null)
			{
				if (node.IsRunning)
					node.Stop();
			}
		}

		#endregion

		#region カメラデバイス: (Property)

		/// <summary>
		/// カメラデバイス: Property
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuCameraProperty_Click(object sender, EventArgs e)
		{
			// プロパティダイアログの表示.
			var node = treeAux.SelectedNode as CxCameraNode;
			if (node != null)
			{
				node.OpenPropertyDialog(this);
			}
		}

		/// <summary>
		/// カメラデバイス: Property (Audio)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuCameraPropertyAudio_Click(object sender, EventArgs e)
		{
			var node = treeAux.SelectedNode as CxCameraNode;
			if (node != null)
			{
				node.OpenPropertyDialog(this, new object[] { XIE.Media.ExMediaType.Audio });
			}
		}

		/// <summary>
		/// カメラデバイス: Property (Video)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuCameraPropertyVideo_Click(object sender, EventArgs e)
		{
			var node = treeAux.SelectedNode as CxCameraNode;
			if (node != null)
			{
				node.OpenPropertyDialog(this, new object[] { XIE.Media.ExMediaType.Video });
			}
		}

		#endregion

		// //////////////////////////////////////////////////
		// シリアル通信ポート
		// //////////////////////////////////////////////////

		#region シリアル通信ポート: (初期化と解放)

		/// <summary>
		/// シリアル通信ポート: ツリーノード初期化.
		/// </summary>
		private void SerialPortNode_Setup()
		{
			var nodes = CxSerialPortNode.Connect(CxAuxInfoForm.AuxInfo);
			var folder = FolderNodes["SerialPort"];
			foreach (var node in nodes)
			{
				node.ContextMenuStrip = SerialPortMenu;
				folder.Nodes.Add(node);
			}
			folder.Expand();
		}

		/// <summary>
		/// シリアル通信ポート: ツリーノード解放.
		/// </summary>
		private void SerialPortNode_Dispose()
		{
			var folder = FolderNodes["SerialPort"];
			foreach (var item in folder.Nodes)
			{
				if (item is IDisposable)
					((IDisposable)item).Dispose();
			}
			folder.Nodes.Clear();
		}

		#endregion

		#region シリアル通信ポート: (SerialPortMenu)

		/// <summary>
		/// シリアル通信ポート: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SerialPortMenu_Opened(object sender, EventArgs e)
		{
			#region メニューの更新.

			menuSerialPortOpen.Enabled = true;
			menuSerialPortClose.Enabled = true;

			#endregion
		}

		/// <summary>
		/// シリアル通信ポート: オープンがクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolSerialPortOpen_Click(object sender, EventArgs e)
		{
			bool is_parent = ReferenceEquals(treeAux.SelectedNode, FolderNodes["SerialPort"]);
			if (is_parent)
			{
				#region デバイスのオープン.
				try
				{
					var node = new CxSerialPortNode();
					node.AuxInfo = CxAuxInfoForm.AuxInfo;
					node.ContextMenuStrip = SerialPortMenu;
					if (node.OpenSelectDialog(this, CxAuxInfoForm.AuxInfo) == DialogResult.OK)
					{
						FolderNodes["SerialPort"].Nodes.Add(node);
						FolderNodes["SerialPort"].Expand();
					}
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				#endregion
			}
			else
			{
				#region プロパティダイアログの表示.
				if (treeAux.SelectedNode is CxSerialPortNode)
				{
					try
					{
						CxSerialPortNode node = treeAux.SelectedNode as CxSerialPortNode;
						node.OpenPropertyDialog(this);
					}
					catch (System.Exception ex)
					{
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				#endregion
			}
		}

		/// <summary>
		/// シリアル通信ポート: クローズがクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolSerialPortClose_Click(object sender, EventArgs e)
		{
			treeAux_RemoveNode();
		}

		#endregion

		// //////////////////////////////////////////////////
		// ネットワーク
		// //////////////////////////////////////////////////

		#region ネットワーク: (NetworkMenu)

		/// <summary>
		/// ネットワーク: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NetworkMenu_Opened(object sender, EventArgs e)
		{
			#region 選択されたノードの判定.
			//bool is_parent = false;
			//bool is_busy = false;
			bool is_opened = false;
			TreeNode node = null;

			if (ReferenceEquals(treeAux.SelectedNode, FolderNodes["TcpServer"]) ||
				ReferenceEquals(treeAux.SelectedNode, FolderNodes["TcpClient"]))
			{
				// 親ディレクトリ.
			}
			else if (treeAux.SelectedNode is CxTcpServerNode)
			{
				CxTcpServerNode item = treeAux.SelectedNode as CxTcpServerNode;
				//is_busy = (item.Controller != null && item.Controller.IsRunning);
				is_opened = (item.PropertyDialog != null);
				node = item;
			}
			else if (treeAux.SelectedNode is CxTcpClientNode)
			{
				CxTcpClientNode item = treeAux.SelectedNode as CxTcpClientNode;
				//is_busy = (item.Controller != null && item.Controller.IsRunning);
				is_opened = (item.PropertyDialog != null);
				node = item;
			}
			#endregion

			#region メニューの更新.
			menuNetworkProperty.Enabled = (node != null && is_opened == false);
			#endregion
		}

		/// <summary>
		/// ネットワーク: デバイスのオープン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuNetworkOpen_Click(object sender, EventArgs e)
		{
			#region 選択されたノードの判定.
			int parent_id = 0;

			if (ReferenceEquals(treeAux.SelectedNode, FolderNodes["TcpServer"]))
				parent_id = 1;
			else if (ReferenceEquals(treeAux.SelectedNode, FolderNodes["TcpClient"]))
				parent_id = 2;
			#endregion

			if (parent_id != 0)
			{
				#region デバイスのオープン.
				if (parent_id == 1)
				{
					#region TCP/IP Server
					try
					{
						var node = new CxTcpServerNode();
						node.AuxInfo = CxAuxInfoForm.AuxInfo;
						node.ContextMenuStrip = NetworkMenu;
						if (node.OpenSelectDialog(this, CxAuxInfoForm.AuxInfo) == DialogResult.OK)
						{
							FolderNodes["TcpServer"].Nodes.Add(node);
							FolderNodes["TcpServer"].Expand();
						}
					}
					catch (System.Exception ex)
					{
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					#endregion
				}
				else if (parent_id == 2)
				{
					#region TCP/IP Client
					try
					{
						var node = new CxTcpClientNode();
						node.AuxInfo = CxAuxInfoForm.AuxInfo;
						node.ContextMenuStrip = NetworkMenu;
						if (node.OpenSelectDialog(this, CxAuxInfoForm.AuxInfo) == DialogResult.OK)
						{
							FolderNodes["TcpClient"].Nodes.Add(node);
							FolderNodes["TcpClient"].Expand();
						}
					}
					catch (System.Exception ex)
					{
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					#endregion
				}
				#endregion
			}
			else
			{
				#region 選択されたノードの更新.
				if (treeAux.SelectedNode is CxTcpServerNode)
				{
					#region TCP/IP Server
					try
					{
						CxTcpServerNode node = treeAux.SelectedNode as CxTcpServerNode;
						if (node.OpenSelectDialog(this, CxAuxInfoForm.AuxInfo) == DialogResult.OK)
						{
						}
					}
					catch (System.Exception ex)
					{
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					#endregion
				}
				else if (treeAux.SelectedNode is CxTcpClientNode)
				{
					#region TCP/IP Client
					try
					{
						CxTcpClientNode node = treeAux.SelectedNode as CxTcpClientNode;
						if (node.OpenSelectDialog(this, CxAuxInfoForm.AuxInfo) == DialogResult.OK)
						{
						}
					}
					catch (System.Exception ex)
					{
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					#endregion
				}
				#endregion
			}
		}

		/// <summary>
		/// ネットワーク: デバイスのクローズ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuNetworkClose_Click(object sender, EventArgs e)
		{
			treeAux_RemoveNode();
		}

		/// <summary>
		/// ネットワーク: プロパティダイアログのオープン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuNetworkSetting_Click(object sender, EventArgs e)
		{
			if (treeAux.SelectedNode is CxTcpServerNode)
			{
				((CxTcpServerNode)treeAux.SelectedNode).OpenPropertyDialog(this);
			}
			else if (treeAux.SelectedNode is CxTcpClientNode)
			{
				((CxTcpClientNode)treeAux.SelectedNode).OpenPropertyDialog(this);
			}
		}

		#endregion

		#region TCP/IP Server: (初期化と解放)

		/// <summary>
		/// TCP/IP Server: ツリーノード初期化.
		/// </summary>
		private void TcpServerNode_Setup()
		{
			var nodes = CxTcpServerNode.Connect(CxAuxInfoForm.AuxInfo);
			var folder = FolderNodes["TcpServer"];
			foreach (var node in nodes)
			{
				node.ContextMenuStrip = NetworkMenu;
				folder.Nodes.Add(node);
			}
			folder.Expand();
		}

		/// <summary>
		/// TCP/IP Server: ツリーノード解放.
		/// </summary>
		private void TcpServerNode_Dispose()
		{
			var folder = FolderNodes["TcpServer"];
			foreach (var item in folder.Nodes)
			{
				if (item is IDisposable)
					((IDisposable)item).Dispose();
			}
			folder.Nodes.Clear();
		}

		#endregion

		#region TCP/IP Client: (初期化と解放)

		/// <summary>
		/// TCP/IP Client: ツリーノード初期化.
		/// </summary>
		private void TcpClientNode_Setup()
		{
			var nodes = CxTcpClientNode.Connect(CxAuxInfoForm.AuxInfo);
			var folder = FolderNodes["TcpClient"];
			foreach (var node in nodes)
			{
				node.ContextMenuStrip = NetworkMenu;
				folder.Nodes.Add(node);
			}
			folder.Expand();
		}

		/// <summary>
		/// TCP/IP Client: ツリーノード解放.
		/// </summary>
		private void TcpClientNode_Dispose()
		{
			var folder = FolderNodes["TcpClient"];
			foreach (var item in folder.Nodes)
			{
				if (item is IDisposable)
					((IDisposable)item).Dispose();
			}
			folder.Nodes.Clear();
		}

		#endregion

		// //////////////////////////////////////////////////
		// Media
		// //////////////////////////////////////////////////

		#region Media: (初期化と解放)

		/// <summary>
		/// Media: ツリーノード初期化.
		/// </summary>
		private void Media_Setup()
		{
			var nodes = CxMediaNode.Connect(CxAuxInfoForm.AuxInfo);
			var folder = FolderNodes["Media"];
			foreach (var node in nodes)
			{
				node.ContextMenuStrip = MediaMenu;
				folder.Nodes.Add(node);
			}
			folder.Expand();
		}

		/// <summary>
		/// Media: ツリーノード解放.
		/// </summary>
		private void Media_Dispose()
		{
			var folder = FolderNodes["Media"];
			foreach (var item in folder.Nodes)
			{
				if (item is IDisposable)
					((IDisposable)item).Dispose();
			}
			folder.Nodes.Clear();
		}

		#endregion

		#region Media: (MediaMenu)

		/// <summary>
		/// MediaMenu: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MediaMenu_Opened(object sender, EventArgs e)
		{
			var node = treeAux.SelectedNode as CxMediaNode;
			if (node != null)
			{
				bool enable = (node != null && node.Player != null);
				bool is_running = (enable && node.Player.IsRunning);
				bool is_paused = (enable && node.Player.IsPaused);
				bool is_win32nt = (System.Environment.OSVersion.Platform == PlatformID.Win32NT);

				menuMediaStart.Enabled = (enable && is_running == false);
				menuMediaPause.Enabled = (enable && is_running == true);
				menuMediaPause.Checked = (enable && is_paused == true);
				menuMediaStop.Enabled = (enable && is_running == true);
				menuMediaSaveGRF.Enabled = (enable && is_running == false && is_win32nt);
				menuMediaProperty.Enabled = (enable);
			}
			else
			{
				menuMediaStart.Enabled = false;
				menuMediaPause.Enabled = false;
				menuMediaStop.Enabled = false;
				menuMediaProperty.Enabled = false;
			}
		}

		/// <summary>
		/// MediaMenu: メディアの再生
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuMediaStart_Click(object sender, EventArgs e)
		{
			var node = treeAux.SelectedNode as CxMediaNode;
			if (node != null)
			{
				node.Start();
			}
		}

		/// <summary>
		/// MediaMenu: メディアの一時停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuMediaPause_Click(object sender, EventArgs e)
		{
			var node = treeAux.SelectedNode as CxMediaNode;
			if (node != null)
			{
				node.Pause();
			}
		}

		/// <summary>
		/// MediaMenu: メディアの停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuMediaStop_Click(object sender, EventArgs e)
		{
			var node = treeAux.SelectedNode as CxMediaNode;
			if (node != null)
			{
				node.Stop();
			}
		}

		/// <summary>
		/// MediaMenu: GRF ファイルの保存
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuMediaSaveGRF_Click(object sender, EventArgs e)
		{
			var node = treeAux.SelectedNode as CxMediaNode;
			if (node != null && node.Player != null)
			{
				#region SaveFileDialog
				var dlg = new SaveFileDialog();
				{
					var now = DateTime.Now;
					var node_name = ApiHelper.MakeValidFileName(node.Name, "_");
					var timestamp = ApiHelper.MakeFileNameSuffix(now, false);
					var filename = string.Format("{0}-{1}.GRF", node_name, timestamp);

					dlg.Filter = "GRF files |*.GRF";
					dlg.OverwritePrompt = true;
					dlg.AddExtension = true;
					dlg.FileName = filename;
					if (XIE.Tasks.SharedData.ProjectDir != "")
						dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);
				}
				#endregion

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					node.Player.SetParam("SaveGraphFile", dlg.FileName);
				}
			}
		}

		/// <summary>
		/// MediaMenu: メディアを閉じる
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuMediaClose_Click(object sender, EventArgs e)
		{
			treeAux_RemoveNode();
		}

		/// <summary>
		/// MediaMenu: メディアのプロパティダイアログを表示する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuMediaProperty_Click(object sender, EventArgs e)
		{
			// プロパティダイアログの表示.
			var node = treeAux.SelectedNode as CxMediaNode;
			if (node != null)
			{
				node.OpenPropertyDialog(this);
			}
		}

		#endregion

		// //////////////////////////////////////////////////
		// Data
		// //////////////////////////////////////////////////

		#region Image: (初期化と解放)

		/// <summary>
		/// Image: ツリーノード初期化.
		/// </summary>
		private void Image_Setup()
		{
			var nodes = CxImageNode.Connect(CxAuxInfoForm.AuxInfo);
			var folder = FolderNodes["Image"];
			foreach (var node in nodes)
			{
				node.ContextMenuStrip = DataMenu;
				folder.Nodes.Add(node);
			}
			folder.Expand();
		}

		/// <summary>
		/// Image: ツリーノード解放.
		/// </summary>
		private void Image_Dispose()
		{
			var folder = FolderNodes["Image"];
			foreach (var item in folder.Nodes)
			{
				if (item is IDisposable)
					((IDisposable)item).Dispose();
			}
			folder.Nodes.Clear();
		}

		#endregion

		#region Data: (DataMenu)

		/// <summary>
		/// Data: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DataMenu_Opened(object sender, EventArgs e)
		{
			#region 選択されたノードの判定.
			var selected_node = treeAux.SelectedNode;
			if (ReferenceEquals(selected_node, treeAux.Nodes["Data"]))
			{
				menuDataOpen.Enabled = true;
				menuDataSave.Enabled = false;
				menuDataClose.Enabled = true;
			}
			else if (
				ReferenceEquals(selected_node, FolderNodes["Media"]) ||
				ReferenceEquals(selected_node, FolderNodes["Image"]))
			{
				menuDataOpen.Enabled = true;
				menuDataSave.Enabled = false;
				menuDataClose.Enabled = true;

				if (ReferenceEquals(selected_node, FolderNodes["Image"]))
					menuDataThumbnail.Enabled = true;
				else
					menuDataThumbnail.Enabled = false;
			}
			else if (selected_node is CxImageNode)
			{
				menuDataOpen.Enabled = false;
				menuDataSave.Enabled = true;
				menuDataClose.Enabled = true;
				menuDataThumbnail.Enabled = false;
			}
			#endregion
		}

		/// <summary>
		/// Data: オープンがクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuDataOpen_Click(object sender, EventArgs e)
		{
			#region OpenFileDialog
			var selected_node = treeAux.SelectedNode;
			var dlg = new OpenFileDialog();
			{
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = true;

				if (string.IsNullOrWhiteSpace(CxAuxInfoForm.AppSettings.ImageFileDirectory) == false)
					dlg.InitialDirectory = CxAuxInfoForm.AppSettings.ImageFileDirectory;

				if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
					dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);
			}
			if (ReferenceEquals(selected_node, treeAux.Nodes["Data"]))
			{
				dlg.Filter = "Data files|*.bmp;*.dib;*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.raw;*.avi;*.asf;*.wmv;*.mp4";
				dlg.Filter += "|Image files|*.bmp;*.dib;*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.raw";
				dlg.Filter += "|Media files|*.avi;*.asf;*.wmv;*.mp4";
				dlg.Filter += "|Bitmap files|*.bmp;*.dib";
				dlg.Filter += "|Jpeg files|*.jpg;*.jpeg";
				dlg.Filter += "|Png files|*.png";
				dlg.Filter += "|TIff files|*.tif;*.tiff";
				dlg.Filter += "|Raw files|*.raw";
				dlg.Filter += "|Avi files|*.avi";
				dlg.Filter += "|Asf files|*.asf";
				dlg.Filter += "|Wmv files|*.wmv";
				dlg.Filter += "|MP4 files|*.mp4";
			}
			if (ReferenceEquals(selected_node, FolderNodes["Media"]))
			{
				dlg.Filter = "Media files|*.avi;*.asf;*.wmv;*.mp4";
				dlg.Filter += "|Avi files|*.avi";
				dlg.Filter += "|Asf files|*.asf";
				dlg.Filter += "|Wmv files|*.wmv";
				dlg.Filter += "|MP4 files|*.mp4";
			}
			if (ReferenceEquals(selected_node, FolderNodes["Image"]))
			{
				dlg.Filter = "Image files|*.bmp;*.dib;*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.raw";
				dlg.Filter += "|Bitmap files|*.bmp;*.dib";
				dlg.Filter += "|Jpeg files|*.jpg;*.jpeg";
				dlg.Filter += "|Png files|*.png";
				dlg.Filter += "|TIff files|*.tif;*.tiff";
				dlg.Filter += "|Raw files|*.raw";
			}
			#endregion

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				CxAuxInfoForm.AppSettings.ImageFileDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);
				TreeNode firstnode = LoadDataFiles(dlg.FileNames);
				if (firstnode != null)
					treeAux.SelectedNode = firstnode;
			}
		}

		/// <summary>
		/// Data: 保存がクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuDataSave_Click(object sender, EventArgs e)
		{
			if (treeAux.SelectedNode is CxImageNode)
			{
				var node = (CxImageNode)treeAux.SelectedNode;
				node.OpenSaveDialog(this);
			}
		}

		/// <summary>
		/// Data: クローズがクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuDataClose_Click(object sender, EventArgs e)
		{
			treeAux_RemoveNode();
		}

		/// <summary>
		/// Data: サムネイルの表示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuDataThumbnail_Click(object sender, EventArgs e)
		{
			var selected_node = treeAux.SelectedNode;

			#region 画像ノード:
			if (ReferenceEquals(selected_node, FolderNodes["Image"]))
			{
				var nodes = new List<CxImageNode>();
				foreach (var node in FolderNodes["Image"].Nodes)
					nodes.Add((CxImageNode)node);

				var dlg = new CxImageThumbnailForm(nodes);
				dlg.StartPosition = FormStartPosition.CenterParent;
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					foreach (var item in dlg.ImageNodes)
					{
						var node = item.Key;
						if ((item.Value & CxImageThumbnailForm.ExActionType.Save) == CxImageThumbnailForm.ExActionType.Save)
						{
							node.OpenSaveDialog(this);
						}
						if ((item.Value & CxImageThumbnailForm.ExActionType.Close) == CxImageThumbnailForm.ExActionType.Close)
						{
							treeAux_RemoveNode(node);
						}
					}
				}
				return;
			}
			#endregion
		}

		/// <summary>
		/// Data: プロパティダイアログの表示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuDataProperty_Click(object sender, EventArgs e)
		{
			if (treeAux.SelectedNode is CxImageNode)
			{
				var node = (CxImageNode)treeAux.SelectedNode;
				node.OpenPropertyDialog(null);
			}
		}

		#endregion

		// //////////////////////////////////////////////////
		// Tasks
		// //////////////////////////////////////////////////

		#region Tasks: (初期化と解放)

		/// <summary>
		/// Tasks: ツリーノード初期化.
		/// </summary>
		private void TaskNode_Setup()
		{
			var nodes = CxTaskNode.Connect(CxAuxInfoForm.AuxInfo);
			var folder = FolderNodes["Tasks"];
			foreach (var node in nodes)
			{
				node.ContextMenuStrip = TaskMenu;
				folder.Nodes.Add(node);
			}
			folder.Expand();
		}

		/// <summary>
		/// Tasks: ツリーノード解放.
		/// </summary>
		private void TaskNode_Dispose()
		{
			var folder = FolderNodes["Tasks"];
			foreach (var item in folder.Nodes)
			{
				if (item is IDisposable)
					((IDisposable)item).Dispose();
			}
			folder.Nodes.Clear();
		}

		#endregion

		#region Tasks: (TaskMenu)

		/// <summary>
		/// TaskMenu: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TaskMenu_Opened(object sender, EventArgs e)
		{
			if (treeAux.SelectedNode == treeAux.Nodes["Tasks"])
			{
				menuTaskNew.Enabled = true;
				menuTaskOpen.Enabled = true;
				menuTaskSave.Enabled = false;
				menuTaskClose.Enabled = true;
				menuTaskDelete.Enabled = true;
				menuTaskThumbnail.Visible = true;
				menuTaskProperty.Visible = false;
			}
			else if (treeAux.SelectedNode is CxTaskNode)
			{
				var node = (CxTaskNode)treeAux.SelectedNode;

				menuTaskNew.Enabled = false;
				menuTaskOpen.Enabled = false;
				menuTaskSave.Enabled = (node.Taskflow != null);
				menuTaskClose.Enabled = true;
				menuTaskDelete.Enabled = true;
				menuTaskThumbnail.Visible = false;
				menuTaskProperty.Visible = true;
				menuTaskProperty.Enabled = (node.Taskflow != null);
			}
		}

		/// <summary>
		/// TaskMenu: 新規作成
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskNew_Click(object sender, EventArgs e)
		{
			var name = ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
			var info = new XIE.Tasks.CxTaskUnitInfo();
			var task = new XIE.Tasks.Syntax_Class(name, "Service-Connect");
			var node = new CxTaskNode(info, task);
			node.AuxInfo = CxAuxInfoForm.AuxInfo;
			node.ContextMenuStrip = TaskMenu;
			node.Setup();

			var aux = (XIE.Tasks.IxAuxInfoTasks)CxAuxInfoForm.AuxInfo;
			aux.Add(info, task);
			treeAux.Nodes["Tasks"].Nodes.Add(node);
			node.EnsureVisible();
		}

		/// <summary>
		/// TaskMenu: ファイル読み込み
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskOpen_Click(object sender, EventArgs e)
		{
			#region OpenFileDialog
			var dlg = new OpenFileDialog();
			{
				dlg.AddExtension = true;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = true;
				dlg.Filter = "Taskflow files |*.xtf";

				if (string.IsNullOrWhiteSpace(CxAuxInfoForm.AppSettings.TaskflowFileDirectory) == false)
					dlg.InitialDirectory = CxAuxInfoForm.AppSettings.TaskflowFileDirectory;
				else if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
					dlg.InitialDirectory = XIE.Tasks.SharedData.ProjectDir;

				if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
					dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);
			}
			#endregion

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				foreach (var filename in dlg.FileNames)
				{
					this.ProjectDir = System.IO.Path.GetDirectoryName(filename);
					var info = new XIE.Tasks.CxTaskUnitInfo(filename);
					var task = (XIE.Tasks.CxTaskflow)info.CreateTaskUnit();
					var node = new CxTaskNode(info, task);
					node.AuxInfo = CxAuxInfoForm.AuxInfo;
					node.ContextMenuStrip = TaskMenu;
					node.Setup();

					var aux = (XIE.Tasks.IxAuxInfoTasks)CxAuxInfoForm.AuxInfo;
					aux.Add(info, task);
					treeAux.Nodes["Tasks"].Nodes.Add(node);
					node.EnsureVisible();
				}
			}
		}

		/// <summary>
		/// TaskMenu: ファイル保存
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskSave_Click(object sender, EventArgs e)
		{
			var node = treeAux.SelectedNode as CxTaskNode;
			if (node != null && node.Taskflow != null)
			{
				node.OpenSaveDialog(this);
			}
		}

		/// <summary>
		/// TaskMenu: 閉じる
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskClose_Click(object sender, EventArgs e)
		{
			treeAux_RemoveNode();
		}

		/// <summary>
		/// TaskMenu: 削除
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskDelete_Click(object sender, EventArgs e)
		{
			if (treeAux.SelectedNode == treeAux.Nodes["Tasks"])
			{
				var msg = "The all nodes and related files will be deleted.\nDo you want to continue ?";
				var ans = MessageBox.Show(this, msg, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (ans == DialogResult.Yes)
				{
					#region Tasks 配下の全ての子ノード.
					{
						this.ImageView.Image = null;
						var files = new List<string>();
						var nodes = new List<CxTaskNode>();
						var target_node = FolderNodes["Tasks"];
						foreach (var item in target_node.Nodes)
						{
							var node = (CxTaskNode)item;
							nodes.Add(node);
							files.Add(node.Info.FileName);
						}
						// ノード削除.
						foreach (var node in nodes)
						{
							node.Reset(this.ImageView);
							node.Disconnect(CxAuxInfoForm.AuxInfo);
						}
						// ファイル削除.
						foreach (var file in files)
						{
							if (!string.IsNullOrWhiteSpace(file) && System.IO.File.Exists(file))
							{
								try
								{
									System.IO.File.Delete(file);
								}
								catch (Exception ex)
								{
									XIE.Log.Api.Error("{0} ({1})", ex.Message, file);
								}
							}
						}
						this.ImageView.Refresh();
						return;
					}
					#endregion
				}
			}
			else if (treeAux.SelectedNode is CxTaskNode)
			{
				var msg = "The selected node and related file will be deleted.\nDo you want to continue ?";
				var ans = MessageBox.Show(this, msg, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (ans == DialogResult.Yes)
				{
					#region Tasks 配下の選択されたノードのみ.
					{
						var node = (CxTaskNode)treeAux.SelectedNode;
						var file = node.Info.FileName;
						node.Reset(this.ImageView);
						node.Disconnect(CxAuxInfoForm.AuxInfo);
						if (!string.IsNullOrWhiteSpace(file) && System.IO.File.Exists(file))
						{
							try
							{
								System.IO.File.Delete(file);
							}
							catch (Exception ex)
							{
								XIE.Log.Api.Error("{0} ({1})", ex.Message, file);
							}
						}
						this.ImageView.Refresh();
						return;
					}
					#endregion
				}
			}
		}

		/// <summary>
		/// TaskMenu: サムネイルの表示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskThumbnail_Click(object sender, EventArgs e)
		{
			var nodes = new List<CxTaskNode>();
			foreach (var node in treeAux.Nodes["Tasks"].Nodes)
				nodes.Add((CxTaskNode)node);

			var dlg = new CxTaskflowThumbnailForm(nodes);
			dlg.StartPosition = FormStartPosition.CenterParent;
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				int index = dlg.SelectedIndex;
				if (0 <= index && index < nodes.Count)
				{
					var node = nodes[index];
					if (node != null)
					{
						if (node.Taskflow == null)
						{
							MessageBox.Show(this, "Taskflow is null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						else
						{
							node.OpenPropertyDialog(null);
						}
					}
				}
			}
		}

		/// <summary>
		/// TaskMenu: プロパティダイアログの表示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuTaskProperty_Click(object sender, EventArgs e)
		{
			var node = treeAux.SelectedNode as CxTaskNode;
			if (node != null && node.Taskflow != null)
			{
				node.OpenPropertyDialog(null);
			}
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

		/// <summary>
		/// オーバレイ (ROI 表示/操作)
		/// </summary>
		private XIE.GDI.CxOverlayROI ROIOverlay = new XIE.GDI.CxOverlayROI();

		#endregion

		#region ImageView: (初期化と解放)

		/// <summary>
		/// ImageView: 初期化
		/// </summary>
		private void ImageView_Setup()
		{
			this.splitVert.Panel2.Controls.Add(this.ImageView);

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
			this.ImageView.Rendering += ROIOverlay.Rendering;
			this.ImageView.Handling += ROIOverlay.Handling;

			this.ImageView.ContextMenuStrip = ImageViewMenu;
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
			this.ImageView.Rendering -= ROIOverlay.Rendering;
			this.ImageView.Handling -= ROIOverlay.Handling;

			this.ImageView.Dispose();
		}

		#endregion

		#region ImageView: (描画)

		/// <summary>
		/// 描画イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			if (treeAux.SelectedNode is XIE.Tasks.IxAuxNodeImageOut)
			{
				var node = (XIE.Tasks.IxAuxNodeImageOut)treeAux.SelectedNode;
				node.Rendering(sender, e);
			}
		}

		/// <summary>
		/// 描画完了イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_Rendered(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			if (treeAux.SelectedNode is XIE.Tasks.IxAuxNodeImageOut)
			{
				var node = (XIE.Tasks.IxAuxNodeImageOut)treeAux.SelectedNode;
				node.Rendered(sender, e);
			}
			ImageView_Rendered_TimeStamp = DateTime.Now;
		}
		private DateTime ImageView_Rendered_TimeStamp = DateTime.Now;

		#endregion

		#region ImageView: (操作)

		/// <summary>
		/// 操作イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_Handling(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			if (treeAux.SelectedNode is XIE.Tasks.IxAuxNodeImageOut)
			{
				var node = (XIE.Tasks.IxAuxNodeImageOut)treeAux.SelectedNode;
				node.Handling(sender, e);
			}
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
			PreviewForm_Follow();
			ExifForm_Follow();

			ImageView.Refresh();
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
				this.ImageView.Refresh();
			}
		}

		#endregion

		#region ImageView: (キーボードイベント)

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
				ImageView.Refresh();
			}
			#endregion

			#region [-] 縮小.
			if (e.KeyCode == Keys.OemMinus && !e.Alt && !e.Control && !e.Shift)
			{
				ImageView.AdjustScale(-1);
				ImageView.Refresh();
			}
			#endregion

			#region [+] 拡大.
			if (e.KeyCode == Keys.Oemplus && !e.Alt && !e.Control && !e.Shift)
			{
				ImageView.AdjustScale(+1);
				ImageView.Refresh();
			}
			#endregion

			#region [F] フィット.
			if (e.KeyCode == Keys.F && !e.Alt && !e.Control && !e.Shift)
			{
				ImageView.FitImageSize(0);
				ImageView.Refresh();
			}
			#endregion

			#region CTRL+[C] コピー.
			if (e.KeyCode == Keys.C && !e.Alt && e.Control && !e.Shift)
			{
				toolViewCopy_Click(sender, e);
			}
			#endregion

			#region CTRL+[V] ペースト.
			if (e.KeyCode == Keys.V && !e.Alt && e.Control && !e.Shift)
			{
				toolViewPaste_Click(sender, e);
			}
			#endregion

			#region CTRL+[E] Exif 表示切替:
			if (e.KeyCode == Keys.E && !e.Alt && e.Control && !e.Shift)
			{
				toolViewExif_Click(sender, e);
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
				this.ImageView.Refresh();
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
				this.ImageView.Refresh();
			}
		}
		XIE.TxPointD ProfileOverlay_PreviousPosition = new XIE.TxPointD();

		#endregion

		#region ImageView: (フルスクリーン)

		/// <summary>
		/// フルスクリーンモード切り替え
		/// </summary>
		/// <param name="enable"></param>
		private void ChangeFullscreenMode(bool enable)
		{
			if (this.IsFullscreen != enable)
			{
				this.IsFullscreen = enable;

				try
				{
					if (enable)
					{
						#region 通常→フル.
						this.Visible = false;

						// 状態保存.
						// --- ウィンドウステータス.
						PrevFormState = this.WindowState;		// ウィンドウの状態.
						if (this.WindowState == FormWindowState.Maximized)
							this.WindowState = FormWindowState.Normal;
						PrevFormStyle = this.FormBorderStyle;	// 境界線スタイル.
						//PrevFormSize = this.Size;				// フォームのサイズ.

						// --- パネル.
						PanelTreeView_Collapsed = splitVert.Panel1Collapsed;

						// パネル非表示.
						splitVert.Panel1Collapsed = true;

						// ツールバー、ステータスバーを非表示にする.
						toolbarView.Visible = false;
						statusbar.Visible = false;

						// 1. フォームの境界線スタイルを「None」にする.
						this.FormBorderStyle = FormBorderStyle.None;
						// 2. フォームのウィンドウ状態を「最大化」する.
						this.WindowState = FormWindowState.Maximized;

						this.Visible = true;

						#endregion
					}
					else
					{
						#region フル→通常.

						// 復元.
						// --- ウィンドウステータス.
						// フォームのサイズを元に戻す.
						this.Size = PrevFormSize;

						// 1. フォームの境界線スタイルを元に戻す.
						this.FormBorderStyle = PrevFormStyle;
						// 2. フォームのウィンドウ状態を元に戻す.
						this.WindowState = PrevFormState;

						//  --- パネル表示／非表示.
						splitVert.Panel1Collapsed = PanelTreeView_Collapsed;

						// ツールバー、ステータスバーを復元する.
						toolbarView.Visible = true;
						statusbar.Visible = true;

						#region アイコン設定:
						// 現象)
						//   デザイナでアイコンを指定した場合、Linux Mono 環境では下記の例外が発生する.
						//   System.ArgumentException: A null reference or invalid value was found [GDI+ status: InvalidParameter]
						// 原因)
						//   CxAuxInfoForm 初期化処理の途中(下記)でリソースの取得に失敗している模様。
						//   this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
						// 対策)
						//   Linux Mono 環境ではフォームのアイコンを設定しない。
						//   実行時に判定する為、Application のリソースに埋め込まれたアイコンを利用する。
						//                                     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
						//   ※注1) app.ico のビルドアクションを "埋め込まれたリソース" にしなければ GetManifestResourceStream で例外が発生する。
						//   ※注2) GetManifestResourceStream に渡す引数には "既定の名前空間" と "アイコンファイル名" を指定する。
						// 
						switch (System.Environment.OSVersion.Platform)
						{
							case PlatformID.Unix:
								// 
								// 上記の理由によりアイコンの設定は行わない.
								// 
								break;
							default:
								{
									Assembly asm = Assembly.GetExecutingAssembly();
									using (Stream stream = asm.GetManifestResourceStream("XIEstudio.app.ico"))
									{
										this.Icon = new System.Drawing.Icon(stream);
									}
								}
								break;
						}
						#endregion

						#endregion
					}
				}
				finally
				{
				}
			}
		}

		/// <summary>
		/// フルスクリーン状態.
		/// </summary>
		private bool IsFullscreen = false;

		/// <summary>
		/// フルスクリーン表示前のフォームサイズ.
		/// </summary>
		private Size PrevFormSize = new Size(640, 480);

		/// <summary>
		/// フルスクリーン表示前のフォームのウィンドウ状態.
		/// </summary>
		private FormWindowState PrevFormState = FormWindowState.Normal;

		/// <summary>
		/// フルスクリーン表示前のフォームの境界線スタイル.
		/// </summary>
		private FormBorderStyle PrevFormStyle = FormBorderStyle.Sizable;

		/// <summary>
		/// フルスクリーン表示前のツリービューの表示／非表示状態
		/// </summary>
		private bool PanelTreeView_Collapsed = false;

		#endregion

		#region ImageViewMenu: (コントロールイベント)

		/// <summary>
		/// ImageViewMenu: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageViewMenu_Opened(object sender, EventArgs e)
		{
			menuFullscreen.Checked = this.IsFullscreen;
			menuPreview.Checked = (this.PreviewForm != null && this.PreviewForm.Visible);

			double mag = this.ImageView.Magnification;
			menuViewScale.Text = string.Format("Scale (x{0})",
				(mag < 0.01) ? mag.ToString("F3") : mag.ToString("F2")
			);

			menuViewUnpack.Text = this.ImageView.Unpack ?
				string.Format("Unpacked (CH:{0})", this.ImageView.ChannelNo) :
				string.Format("Unpack");
			menuViewUnpack.Checked = (this.ImageView.Unpack);

			menuViewHalftone.Checked = this.ImageView.Halftone;
			menuViewProfile.Checked = this.ProfileOverlay.Visible;
			menuViewGrid.Checked = this.GridOverlay.Visible;
			menuViewROI.Checked = this.ROIOverlay.Visible;
			menuViewExif.Checked = (this.ExifForm != null && this.ExifForm.Visible);
		}

		/// <summary>
		/// ImageViewMenu: フルスクリーン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuFullscreen_Click(object sender, EventArgs e)
		{
			toolFullscreen_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: 画像プレビューフォームの表示切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuPreview_Click(object sender, EventArgs e)
		{
			toolPreview_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: コピー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewCopy_Click(object sender, EventArgs e)
		{
			toolViewCopy_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: 貼り付け
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewPaste_Click(object sender, EventArgs e)
		{
			toolViewPaste_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: ROI の表示/非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewROI_Click(object sender, EventArgs e)
		{
			toolViewROI_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: Exif フォーム表示切替
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewExif_Click(object sender, EventArgs e)
		{
			toolViewExif_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: 画像全体が View に表示されるように表示倍率を調整します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewFitImageSizeBoth_Click(object sender, EventArgs e)
		{
			toolViewFitImageSize_ButtonClick(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: 画像の幅を View の幅に合わせます。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewFitImageSizeWidth_Click(object sender, EventArgs e)
		{
			toolViewFitImageWidth_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: 画像の高さを View の幅に合わせます。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewFitImageSizeHeight_Click(object sender, EventArgs e)
		{
			toolViewFitImageHeight_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: 表示倍率メニューが開いたときの表示更新処理。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewScale_DropDownOpened(object sender, EventArgs e)
		{
			ToolStripMenuItem[] items = 
			{
				menuViewScale0010,
				menuViewScale0025,
				menuViewScale0050,
				menuViewScale0075,
				menuViewScale0100,
				menuViewScale0200,
				menuViewScale0400,
				menuViewScale0800,
				menuViewScale1600,
				menuViewScale3200,
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
		/// ImageViewMenu: 表示倍率を指定値に設定します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewScale_Click(object sender, EventArgs e)
		{
			int scale = 0;
			PropertyInfo prop = sender.GetType().GetProperty("Tag");
			if (prop != null)
				scale = Convert.ToInt32(prop.GetValue(sender, null));

			if (scale > 0)
			{
				this.ImageView.Magnification = scale * 0.01;
				this.ImageView.Refresh();
			}
		}

		/// <summary>
		/// ImageViewMenu: 表示倍率を既定値に設定します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewScaleReset_Click(object sender, EventArgs e)
		{
			this.ImageView.AdjustScale(0);
			this.ImageView.Refresh();
		}

		/// <summary>
		/// ImageViewMenu: 表示倍率を縮小します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewScaleDown_Click(object sender, EventArgs e)
		{
			this.ImageView.AdjustScale(-1);
			this.ImageView.Refresh();
		}

		/// <summary>
		/// ImageViewMenu: 表示倍率を拡大します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewScaleUp_Click(object sender, EventArgs e)
		{
			this.ImageView.AdjustScale(+1);
			this.ImageView.Refresh();
		}

		/// <summary>
		/// ImageViewMenu: 伸縮時の濃度補間方法の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewHalftone_Click(object sender, EventArgs e)
		{
			toolViewHalftone_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: ビット深度の調整
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewDepthFit_Click(object sender, EventArgs e)
		{
			toolViewDepth_ButtonClick(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: ビット深度の調整 (既定値)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewDepthDefault_Click(object sender, EventArgs e)
		{
			toolViewDepthDefault_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: パッキング/アンパッキングの切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewUnpack_Click(object sender, EventArgs e)
		{
			toolViewUnpack_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: 表示チャネル指標の切り替え (前へ)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewChannelPrev_Click(object sender, EventArgs e)
		{
			toolViewChannelPrev_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: 表示チャネル指標の切り替え (次へ)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewChannelNext_Click(object sender, EventArgs e)
		{
			toolViewChannelNext_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: Profile の表示/非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewProfile_Click(object sender, EventArgs e)
		{
			toolViewProfile_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: Graph の表示/非表示の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewGrid_Click(object sender, EventArgs e)
		{
			toolViewGrid_Click(sender, e);
		}

		#endregion

		// //////////////////////////////////////////////////
		// PreviewForm
		// //////////////////////////////////////////////////

		#region PreviewForm: (初期化と解放)

		/// <summary>
		/// 画像プレビューフォーム: 初期化
		/// </summary>
		private void PreviewForm_Setup()
		{
		}

		/// <summary>
		/// 画像プレビューフォーム: 解放
		/// </summary>
		private void PreviewForm_TearDown()
		{
			if (this.PreviewForm != null)
				this.PreviewForm.Close();
			this.PreviewForm = null;
		}

		#endregion

		#region PreviewForm: (表示)

		/// <summary>
		/// 画像プレビューフォーム
		/// </summary>
		private CxImagePreviewForm PreviewForm = null;

		/// <summary>
		/// 画像プレビューフォームのコンテナ
		/// </summary>
		private Control PreviewForm_Container
		{
			get { return ImageView; }
		}

		/// <summary>
		/// 画像プレビューフォームとコンテナの相対位置
		/// </summary>
		private Point PreviewForm_LocationOffset = new Point();

		/// <summary>
		/// 画像プレビューフォームとコンテナの相対位置の更新
		/// </summary>
		private void PreviewForm_UpdateOffset()
		{
			if (this.PreviewForm != null)
			{
				var form = PreviewForm;
				var ctrl = PreviewForm_Container;
				var location = ctrl.PointToClient(form.Location);
				var offset = new Point(
					location.X - (ctrl.Location.X + ctrl.Width),
					location.Y - (ctrl.Location.Y)
					);
				PreviewForm_LocationOffset = offset;
			}
		}

		/// <summary>
		/// 画像プレビューフォームの移動
		/// </summary>
		private void PreviewForm_Follow()
		{
			if (this.PreviewForm != null)
			{
				if (this.PreviewForm.IsDocking)
				{
					var form = PreviewForm;
					var ctrl = PreviewForm_Container;
					var location = new Point(
						PreviewForm_LocationOffset.X + (ctrl.Location.X + ctrl.Width),
						PreviewForm_LocationOffset.Y + (ctrl.Location.Y)
					);
					var form_location = ctrl.PointToScreen(location);
					form.Location = form_location;
				}
				else
				{
					PreviewForm_UpdateOffset();
				}
			}
		}

		/// <summary>
		/// 画像プレビューフォームの推奨位置の取得
		/// </summary>
		/// <returns>
		///		画像プレビューフォームの推奨位置を計算して返します。
		/// </returns>
		private Point PreviewForm_GetTypicalLocation()
		{
			var form = PreviewForm;
			var ctrl = PreviewForm_Container;
			int x = ctrl.Width - form.Width - 4;
			int y = 4;
			return ctrl.PointToScreen(new Point(x, y));
		}

		/// <summary>
		/// 画像プレビューフォームが閉じられるとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PreviewForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				PreviewForm.Visible = false;
				PreviewForm_UpdateOffset();
				e.Cancel = true;
			}
			else
			{
				PreviewForm = null;
			}
		}

		/// <summary>
		/// 画像プレビューフォームが移動したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PreviewForm_Move(object sender, EventArgs e)
		{
			PreviewForm_UpdateOffset();
		}

		/// <summary>
		/// 画像プレビューフォームがサイズ変更したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PreviewForm_Resize(object sender, EventArgs e)
		{
			PreviewForm_UpdateOffset();
		}

		#endregion

		// //////////////////////////////////////////////////
		// ExifForm
		// //////////////////////////////////////////////////

		#region ExifForm: (初期化と解放)

		/// <summary>
		/// Exif フォーム: 初期化
		/// </summary>
		private void ExifForm_Setup()
		{
		}

		/// <summary>
		/// Exif フォーム: 解放
		/// </summary>
		private void ExifForm_TearDown()
		{
			if (this.ExifForm != null)
				this.ExifForm.Close();
			this.ExifForm = null;
		}

		#endregion

		#region ExifForm: (表示)

		/// <summary>
		/// Exif フォーム
		/// </summary>
		private CxExifForm ExifForm = null;

		/// <summary>
		/// Exif フォームのコンテナ
		/// </summary>
		private Control ExifForm_Container
		{
			get { return ImageView; }
		}

		/// <summary>
		/// Exif フォームとコンテナの相対位置
		/// </summary>
		private Point ExifForm_LocationOffset = new Point();

		/// <summary>
		/// Exif フォームとコンテナの相対位置の更新
		/// </summary>
		private void ExifForm_UpdateOffset()
		{
			if (this.ExifForm != null)
			{
				var form = ExifForm;
				var ctrl = ExifForm_Container;
				var location = ctrl.PointToClient(form.Location);
				var offset = new Point(
					location.X - (ctrl.Location.X),
					location.Y - (ctrl.Location.Y)
					);
				ExifForm_LocationOffset = offset;
			}
		}

		/// <summary>
		/// Exif フォームの移動
		/// </summary>
		private void ExifForm_Follow()
		{
			if (this.ExifForm != null)
			{
				if (this.ExifForm.IsDocking)
				{
					var form = ExifForm;
					var ctrl = ExifForm_Container;
					var location = new Point(
						ExifForm_LocationOffset.X + (ctrl.Location.X),
						ExifForm_LocationOffset.Y + (ctrl.Location.Y)
					);
					var form_location = ctrl.PointToScreen(location);
					form.Location = form_location;
				}
				else
				{
					ExifForm_UpdateOffset();
				}
			}
		}

		/// <summary>
		/// Exif フォームの推奨位置の取得
		/// </summary>
		/// <returns>
		///		Exif フォームの推奨位置を計算して返します。
		/// </returns>
		private Point ExifForm_GetTypicalLocation()
		{
			var form = ExifForm;
			var ctrl = ExifForm_Container;
			int x = 4;
			int y = 4;
			return ctrl.PointToScreen(new Point(x, y));
		}

		/// <summary>
		/// Exif フォームが閉じられるとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExifForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				ExifForm.Visible = false;
				ExifForm_UpdateOffset();
				e.Cancel = true;
			}
			else
			{
				ExifForm = null;
			}
		}

		/// <summary>
		/// Exif フォームが移動したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExifForm_Move(object sender, EventArgs e)
		{
			ExifForm_UpdateOffset();
		}

		/// <summary>
		/// Exif フォームがサイズ変更したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExifForm_Resize(object sender, EventArgs e)
		{
			ExifForm_UpdateOffset();
		}

		#endregion

		#region ExifForm: (表示更新)

		/// <summary>
		/// Exif フォーム: 表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExifForm_Update(object sender, EventArgs e)
		{
			if (this.ExifForm != null)
			{
				var node = treeAux.SelectedNode as CxImageNode;
				if (node != null)
					this.ExifForm.Image = node.Data;
				else
					this.ExifForm.Image = null;
				this.ExifForm.UpdateInfo();
			}
		}

		#endregion

	}
}
