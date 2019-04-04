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
	/// 画像編集フォーム
	/// </summary>
	public partial class CxImageEditorForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageEditorForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="node">画像ノード</param>
		public CxImageEditorForm(CxImageNode node)
		{
			InitializeComponent();
			this.ImageNode = node;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 画像ノード
		/// </summary>
		public CxImageNode ImageNode
		{
			get { return m_ImageNode; }
			set { m_ImageNode = value; }
		}
		private CxImageNode m_ImageNode = new CxImageNode();

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxImageEditorForm_Load(object sender, EventArgs e)
		{
			// フォームのサイズ.
			this.PrevFormSize = this.Size;

			#region フォーム初期化.
			{
				this.Text = this.ImageNode.Name;
			}
			#endregion

			ImageView_Setup();
			PreviewForm_Setup();

			this.ImageView.Image = this.ImageNode.Data;

			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられる時の解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxImageEditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();

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
			#region toolbarView の表示更新:
			{
				toolPreview.Checked = (this.PreviewForm != null && this.PreviewForm.Visible);

				toolEditUndo.Enabled = this.ImageNode.CanUndo();
				toolEditRedo.Enabled = this.ImageNode.CanRedo();

				toolViewCut.Enabled = (this.ImageView.Image != null);
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
			}
			#endregion

			#region toolbarOverlay の表示更新:
			{
				this.toolFigureHandling.Checked = (EditMode == ExEditMode.Figure);
				this.toolPaintBrushMode.Checked = (EditMode == ExEditMode.Brush);
				this.toolPaintDropMode.Checked = (EditMode == ExEditMode.Drop);
				this.toolFigureAlign.Enabled = (this.ImageNode.Overlay.SelectedFigures.IsEmpty == false);
				this.toolFigureClone.Enabled = (this.ImageNode.Overlay.SelectedFigures.IsEmpty == false);
				this.toolFigureDraw.Enabled = (this.ImageNode.Overlay.Figures.Count > 0);
				this.toolFigureClear.Enabled = (this.ImageNode.Overlay.Figures.Count > 0);
			}
			#endregion

			#region statusbar の表示更新:
			statusbar_Update();
			#endregion
		}

		// //////////////////////////////////////////////////
		// CxImageEditorForm
		// //////////////////////////////////////////////////

		#region CxImageEditorForm: (コントロールイベント)

		/// <summary>
		/// CxImageEditorForm: フォームが移動した時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxImageEditorForm_Move(object sender, EventArgs e)
		{
			PreviewForm_Follow();
		}

		/// <summary>
		/// CxImageEditorForm: フォームがサイズ変更した時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxImageEditorForm_Resize(object sender, EventArgs e)
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

			PreviewForm_Follow();
		}

		#endregion

		#region CxImageEditorForm: (キーボードイベント)

		/// <summary>
		/// CxImageEditorForm: キーボード押下
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxImageEditorForm_KeyDown(object sender, KeyEventArgs e)
		{
			#region Edit - Undo: [Ctrl+Z]
			if (e.KeyCode == Keys.Z && !e.Alt && e.Control && !e.Shift)
			{
				e.Handled = true;
				toolEditUndo_Click(sender, e);
				return;
			}
			#endregion

			#region Edit - Redo: [Ctrl+Y]
			if (e.KeyCode == Keys.Y && !e.Alt && e.Control && !e.Shift)
			{
				e.Handled = true;
				toolEditRedo_Click(sender, e);
				return;
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
		/// CxImageEditorForm: キーボード押下
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxImageEditorForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
		}

		#endregion

		// //////////////////////////////////////////////////
		// toolbar
		// //////////////////////////////////////////////////

		#region toolbarView: (Edit)

		/// <summary>
		/// toolbarView: Undo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditUndo_Click(object sender, EventArgs e)
		{
			this.ImageNode.Undo();
			this.ImageView.Refresh();

			CxAuxInfoForm.AuxInfo.SendRequested(this.ImageNode, new XIE.Tasks.CxAuxNotifyEventArgs_Refresh());
		}

		/// <summary>
		/// toolbarView: Redo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditRedo_Click(object sender, EventArgs e)
		{
			this.ImageNode.Redo();
			this.ImageView.Refresh();

			CxAuxInfoForm.AuxInfo.SendRequested(this.ImageNode, new XIE.Tasks.CxAuxNotifyEventArgs_Refresh());
		}

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
				using (var image = new XIE.CxImage())
				{
					var roi = new XIE.TxRectangleI();

					#region ROI 指定:
					if (this.ROIOverlay.Visible &&
						this.ROIOverlay.CheckValidity(this.ImageView.Image))
					{
						roi = (XIE.TxRectangleI)this.ROIOverlay.Shape;
					}
					#endregion

					image.Attach(ImageView.Image, roi);

					XIE.AxiClipboard.CopyFrom(image);
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// toolbarView: 切り取り
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewCut_Click(object sender, EventArgs e)
		{
			if (ImageView.Image == null) return;

			try
			{
				this.ImageNode.AddHistory(0, true);	// 編集履歴:

				using (var image = new XIE.CxImage())
				{
					var roi = new XIE.TxRectangleI();

					#region ROI 指定:
					if (this.ROIOverlay.Visible &&
						this.ROIOverlay.CheckValidity(this.ImageView.Image))
					{
						roi = (XIE.TxRectangleI)this.ROIOverlay.Shape;
					}
					#endregion

					image.Attach(ImageView.Image, roi);

					XIE.AxiClipboard.CopyFrom(image);

					image.Clear(0);
					ImageView.Refresh();

					CxAuxInfoForm.AuxInfo.SendRequested(this.ImageNode, new XIE.Tasks.CxAuxNotifyEventArgs_Refresh());
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
			PasteImageToView(false);
		}

		/// <summary>
		/// toolbarView: 貼り付け (アルファフィールドを使用する)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewPasteUseAlpha_Click(object sender, EventArgs e)
		{
			PasteImageToView(true);
		}

		/// <summary>
		/// 画像ビューへ画像を貼り付ける
		/// </summary>
		/// <param name="use_alpha">32bpp の場合、アルファフィールドを使用するか否か</param>
		private void PasteImageToView(bool use_alpha)
		{
			try
			{
				var idata = Clipboard.GetDataObject();
				if (idata == null) return;

				this.ImageNode.AddHistory(1, false);

				XIE.GDI.IxGdi2d added_figure = null;

				var vis = this.ImageView.VisibleRect();
				var mag = this.ImageView.Magnification;

				#region 図形の新規生成:
				using (var image = XIE.AxiClipboard.ToImage())
				{
					if (image != null)
					{
						var center = vis.Location + vis.Size / 2;
						var figure = new XIE.GDI.CxGdiImage();
						figure.Location = center;
						figure.Bitmap = image.ToBitmap(use_alpha);
						added_figure = figure;
					}
				}
				#endregion

				if (added_figure != null)
				{
					if (this.EditMode != ExEditMode.Figure)
						this.EditMode = ExEditMode.Figure;
					if (this.ImageNode.Overlay.Visible != true)
						this.ImageNode.Overlay.Visible = true;

					this.ImageNode.Overlay.Figures.Add(added_figure);
					this.propertyOverlay.SelectedObject = added_figure;
					this.propertyOverlay.Refresh();
					this.ImageView.Refresh();

					CxAuxInfoForm.AuxInfo.SendRequested(this.ImageNode, new XIE.Tasks.CxAuxNotifyEventArgs_Refresh());
				}
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

				var node = this.ImageNode;
				var roi = this.ImageNode.ROI;

				#region 画像オブジェクトの roi が未初期化の場合は、現状を複製するかリセットする:
				if (roi.X < 0 || roi.Y < 0 || roi.Width <= 0 || roi.Height <= 0)
				{
					roi = (TxRectangleI)this.ROIOverlay.Shape;

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

				var node = this.ImageNode;
				var roi = this.ImageNode.ROI;

				#region 画像オブジェクトの roi が未初期化の場合は、現状を複製するかリセットする:
				if (this.ImageView.Image != null)
				{
					if (roi.X < 0 || roi.Y < 0 || roi.Width <= 0 || roi.Height <= 0)
					{
						roi = (TxRectangleI)this.ROIForm.ROIOverlay.Shape;

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
					var data = (CxImage)this.ImageNode.Data.Clone();
					data.ExifCopy(this.ImageNode.Data.Exif());

					var prefix = ApiHelper.GetFileNameWithoutExtensionForImage(this.ImageNode.Name);
					var name = prefix + "-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
					var args = new XIE.Tasks.CxAuxNotifyEventArgs_AddImage(data, name);
					this.ImageNode.AuxInfo.SendRequested(this, args);
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
							try
							{
								this.ImageNode.Overlay.VisibleSelectionMark = false;
								var display_size = ImageView.Image.Size;
								var view_point = ImageView.ViewPoint;
								var mag = 1;
								data = ImageView.Snapshot(display_size, view_point, mag);
							}
							finally
							{
								this.ImageNode.Overlay.VisibleSelectionMark = true;
							}
							break;
						case 1:
							try
							{
								this.ImageNode.Overlay.VisibleSelectionMark = false;
								var view_point = ImageView.ViewPoint;
								var mag = ImageView.Magnification;
								var vis = ImageView.VisibleRect();
								var display_size = new XIE.TxSizeI(
										(int)System.Math.Ceiling(vis.Width * mag),
										(int)System.Math.Ceiling(vis.Height * mag)
									);
								data = ImageView.Snapshot(display_size, view_point, mag);
							}
							finally
							{
								this.ImageNode.Overlay.VisibleSelectionMark = true;
							}
							break;
						case 2:
							try
							{
								this.ImageNode.Overlay.VisibleSelectionMark = false;
								data = ImageView.Snapshot();
							}
							finally
							{
								this.ImageNode.Overlay.VisibleSelectionMark = true;
							}
							break;
					}
					data.ExifCopy(this.ImageNode.Data.Exif());
					var prefix = ApiHelper.GetFileNameWithoutExtensionForImage(this.ImageNode.Name);
					var name = prefix + "-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
					var args = new XIE.Tasks.CxAuxNotifyEventArgs_AddImage(data, name);
					this.ImageNode.AuxInfo.SendRequested(this, args);
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		#endregion

		#region toolbarView: (Filter)

		/// <summary>
		/// toolFilter: メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFilter_DropDownOpened(object sender, EventArgs e)
		{
			var is_valid = (this.ImageView.Image != null);

			menuFilterGrayScale.Enabled = is_valid;
			menuFilterColor.Enabled = is_valid;
			menuFilterOperation.Enabled = is_valid;
			menuFilterRotate.Enabled = is_valid;
			menuFilterScale.Enabled = is_valid;
			menuFilterMirror.Enabled = is_valid;
			menuEffector.Enabled = is_valid;
		}

		/// <summary>
		/// toolFilter: メニューが選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuFilter_Click(object sender, EventArgs e)
		{
			try
			{
				XIE.CxImage src = this.ImageNode.Data;
				if (src == null) return;
				if (src.IsValid == false) return;

				using (var dst = new XIE.CxImage())
				{
					do
					{
						#region GrayScale:
						{
							var dst_type = XIE.ExType.None;
							var dst_pack = 1;
							var dst_channels = 1;

							#region 出力画像の型の決定:
							if (ReferenceEquals(sender, menuFilterGrayScale1))
								dst_type = XIE.ExType.U8;
							if (ReferenceEquals(sender, menuFilterGrayScale2))
								dst_type = XIE.ExType.U16;
							if (ReferenceEquals(sender, menuFilterGrayScale3))
								dst_type = XIE.ExType.S16;
							if (ReferenceEquals(sender, menuFilterGrayScale4))
								dst_type = XIE.ExType.F64;
							#endregion

							// --------------------------------------------------
							if (dst_type != XIE.ExType.None)
							{
								var dst_model = new XIE.TxModel(dst_type, dst_pack);
								dst.Resize(src.Width, src.Height, dst_model, dst_channels);

								var scale = XIE.Axi.CalcScale(src.Model.Type, src.Depth, dst.Model.Type, dst.Depth);
								if (src.Model.Type != XIE.ExType.F64 &&
									src.Model.Type != XIE.ExType.F32)
								{
									// 出力側の要素が大きければスケーリングしない.
									if (scale > 1.0)
										scale = 1.0;
								}
								else
								{
									// 出力側の要素が大きければスケーリングしない.
									if (dst.Model.Type == XIE.ExType.F64 ||
										dst.Model.Type == XIE.ExType.F32)
									{
										var src_depth = XIE.Axi.CalcDepth(src.Model.Type);
										var dst_depth = XIE.Axi.CalcDepth(dst.Model.Type);
										if (src_depth <= dst_depth)
										{
											if (scale > 1.0)
												scale = 1.0;
										}
									}
								}

								// 変換実行:
								dst.Filter().Copy(src, scale);

								var depth = dst.CalcDepth(-1);
								if (1 < depth && depth < 8)
									dst.Depth = 8;
								else
									dst.Depth = depth;

								break;
							}
						}
						#endregion

						#region Color:
						{
							var dst_type = XIE.ExType.None;
							var dst_pack = 0;
							var dst_channels = 0;

							#region 出力画像の型の決定:
							if (ReferenceEquals(sender, menuFilterColor1))
							{
								dst_type = XIE.ExType.U8;
								dst_pack = 3;
								dst_channels = 1;
							}
							if (ReferenceEquals(sender, menuFilterColor2))
							{
								dst_type = XIE.ExType.U8;
								dst_pack = 4;
								dst_channels = 1;
							}
							if (ReferenceEquals(sender, menuFilterColor3))
							{
								dst_type = XIE.ExType.U8;
								dst_pack = 1;
								dst_channels = 3;
							}
							if (ReferenceEquals(sender, menuFilterColor4))
							{
								dst_type = XIE.ExType.U16;
								dst_pack = 1;
								dst_channels = 3;
							}
							if (ReferenceEquals(sender, menuFilterColor5))
							{
								dst_type = XIE.ExType.S16;
								dst_pack = 1;
								dst_channels = 3;
							}
							if (ReferenceEquals(sender, menuFilterColor6))
							{
								dst_type = XIE.ExType.F64;
								dst_pack = 1;
								dst_channels = 3;
							}
							#endregion

							// --------------------------------------------------
							if (dst_type != XIE.ExType.None)
							{
								var dst_model = new XIE.TxModel(dst_type, dst_pack);
								dst.Resize(src.Width, src.Height, dst_model, dst_channels);

								var scale = XIE.Axi.CalcScale(src.Model.Type, src.Depth, dst.Model.Type, dst.Depth);
								if (src.Model.Type != XIE.ExType.F64 &&
									src.Model.Type != XIE.ExType.F32)
								{
									// 出力側の要素が大きければスケーリングしない.
									if (scale > 1.0)
										scale = 1.0;
								}
								else
								{
									// 出力側の要素が大きければスケーリングしない.
									if (dst.Model.Type == XIE.ExType.F64 ||
										dst.Model.Type == XIE.ExType.F32)
									{
										var src_depth = XIE.Axi.CalcDepth(src.Model.Type);
										var dst_depth = XIE.Axi.CalcDepth(dst.Model.Type);
										if (src_depth <= dst_depth)
										{
											if (scale > 1.0)
												scale = 1.0;
										}
									}
								}

								// 変換実行:
								dst.Filter().Copy(src, scale);

								var depth = dst.CalcDepth(-1);
								if (1 < depth && depth < 8)
									dst.Depth = 8;
								else
									dst.Depth = depth;

								break;
							}
						}
						#endregion

						#region Color: (Segmentation)
						// --------------------------------------------------
						// RgbToBgr
						if (ReferenceEquals(sender, menuFilterColor7))
						{
							dst.Resize(src.Width, src.Height, src.Model, src.Channels);
							dst.Filter().RgbToBgr(src, 0);
							dst.Depth = src.Depth;
							break;
						}
						#endregion

						#region Operation:
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterOperation1))
						{
							dst.Filter().Not(src);
							dst.Depth = src.Depth;
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterOperation2))
						{
							dst.Filter().Math(src, XIE.ExMath.Abs);
							dst.Depth = src.Depth;
							break;
						}
						#endregion

						#region Rotate:
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterRotateFree))
						{
							var dlg = new CxRotateForm();
							dlg.StartPosition = FormStartPosition.CenterParent;
							dlg.SrcSize = src.Size;
							dlg.Interpolation = (this.FilterScaleInterpolation != 0);
							if (dlg.ShowDialog(this) == DialogResult.OK)
							{
								this.FilterScaleInterpolation = dlg.Interpolation ? 1 : 0;

								var angle = dlg.Angle;
								var axis = dlg.Axis;
								using (var matrix = XIE.CxMatrix.PresetRotate(angle, axis.X, axis.Y))
								{
									dst.Resize(src.ImageSize);
									dst.Reset();
									int mode = (this.FilterScaleInterpolation == 0) ? 0 : 1;
									dst.Filter().Affine(src, matrix, mode);
									dst.Depth = src.Depth;
								}
							}
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterRotate1))
						{
							dst.Filter().Rotate(src, -1);
							dst.Depth = src.Depth;
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterRotate2))
						{
							dst.Filter().Rotate(src, +1);
							dst.Depth = src.Depth;
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterRotate3))
						{
							dst.Filter().Rotate(src, 2);
							dst.Depth = src.Depth;
							break;
						}
						#endregion

						#region Scale:
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterScaleFree))
						{
							var dlg = new CxScaleForm();
							dlg.StartPosition = FormStartPosition.CenterParent;
							dlg.SrcSize = src.Size;
							dlg.Interpolation = (this.FilterScaleInterpolation != 0);
							if (dlg.ShowDialog(this) == DialogResult.OK)
							{
								this.FilterScaleInterpolation = dlg.Interpolation ? 1 : 0;

								var mag_x = (double)dlg.DstSize.Width / dlg.SrcSize.Width;
								var mag_y = (double)dlg.DstSize.Height / dlg.SrcSize.Height;
								int mode = (this.FilterScaleInterpolation == 0) ? 0 : ((mag_x <= 0.5 && mag_y <= 0.5) ? 2 : 1);
								//int mode = (this.FilterScaleInterpolation == 0) ? 0 : 1;
								dst.Filter().Scale(src, mag_x, mag_y, mode);
								dst.Depth = src.Depth;
							}
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterScale1))
						{
							var mag = 0.1;
							var mode = (this.FilterScaleInterpolation == 0) ? 0 : 2;
							dst.Filter().Scale(src, mag, mag, mode);
							dst.Depth = src.Depth;
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterScale2))
						{
							var mag = 0.25;
							var mode = (this.FilterScaleInterpolation == 0) ? 0 : 2;
							dst.Filter().Scale(src, mag, mag, mode);
							dst.Depth = src.Depth;
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterScale3))
						{
							var mag = 0.5;
							var mode = (this.FilterScaleInterpolation == 0) ? 0 : 2;
							dst.Filter().Scale(src, mag, mag, mode);
							dst.Depth = src.Depth;
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterScale4))
						{
							var mag = 0.75;
							var mode = (this.FilterScaleInterpolation == 0) ? 0 : 1;
							dst.Filter().Scale(src, mag, mag, mode);
							dst.Depth = src.Depth;
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterScale5))
						{
							var mag = 1.5;
							var mode = (this.FilterScaleInterpolation == 0) ? 0 : 1;
							dst.Filter().Scale(src, mag, mag, mode);
							dst.Depth = src.Depth;
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterScale6))
						{
							var mag = 2.0;
							var mode = (this.FilterScaleInterpolation == 0) ? 0 : 1;
							dst.Filter().Scale(src, mag, mag, mode);
							dst.Depth = src.Depth;
							break;
						}
						#endregion

						#region Mirror:
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterMirror1))
						{
							dst.Filter().Mirror(src, 1);
							dst.Depth = src.Depth;
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterMirror2))
						{
							dst.Filter().Mirror(src, 2);
							dst.Depth = src.Depth;
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterMirror3))
						{
							dst.Filter().Mirror(src, 3);
							dst.Depth = src.Depth;
							break;
						}
						// --------------------------------------------------
						if (ReferenceEquals(sender, menuFilterTranspose))
						{
							dst.Filter().Transpose(src);
							dst.Depth = src.Depth;
							break;
						}
						#endregion

						#region Effector - Gamma Converter
						if (ReferenceEquals(sender, menuEffectorGammaConverter))
						{
							var dlg = new CxGammaConverterForm();
							dlg.SrcImage = src;
							dlg.StartPosition = FormStartPosition.CenterParent;
							if (dlg.ShowDialog(this) == DialogResult.OK)
							{
								dst.CopyFrom(dlg.DstImage);
							}
							break;
						}
						#endregion

						#region Effector - HSV Converter
						if (ReferenceEquals(sender, menuEffectorHsvConverter))
						{
							var dlg = new CxHsvConverterForm();
							dlg.SrcImage = src;
							dlg.StartPosition = FormStartPosition.CenterParent;
							if (dlg.ShowDialog(this) == DialogResult.OK)
							{
								dst.CopyFrom(dlg.DstImage);
							}
							break;
						}
						#endregion

						#region Effector - RGB Converter
						if (ReferenceEquals(sender, menuEffectorRgbConverter))
						{
							var dlg = new CxRgbConverterForm();
							dlg.SrcImage = src;
							dlg.StartPosition = FormStartPosition.CenterParent;
							if (dlg.ShowDialog(this) == DialogResult.OK)
							{
								dst.CopyFrom(dlg.DstImage);
							}
							break;
						}
						#endregion

						#region Effector - Monochrome
						if (ReferenceEquals(sender, menuEffectorMonochrome))
						{
							var dlg = new CxMonochromeForm();
							dlg.SrcImage = src;
							dlg.StartPosition = FormStartPosition.CenterParent;
							if (dlg.ShowDialog(this) == DialogResult.OK)
							{
								dst.CopyFrom(dlg.DstImage);
							}
							break;
						}
						#endregion

						#region Effector - PartColor
						if (ReferenceEquals(sender, menuEffectorPartColor))
						{
							var dlg = new CxPartColorForm();
							dlg.SrcImage = src;
							dlg.StartPosition = FormStartPosition.CenterParent;
							if (dlg.ShowDialog(this) == DialogResult.OK)
							{
								dst.CopyFrom(dlg.DstImage);
							}
							break;
						}
						#endregion
					}
					while (false);

					if (dst.IsValid)
					{
						this.ImageNode.AddHistory(0, true);
						dst.ExifCopy(src.Exif());
						src.CopyFrom(dst);
						this.ImageView.Refresh();

						CxAuxInfoForm.AuxInfo.SendRequested(this.ImageNode, new XIE.Tasks.CxAuxNotifyEventArgs_Refresh());
					}
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// toolFilterSplit: メニューが選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuFilterSplit_Click(object sender, EventArgs e)
		{
			var timestamp = DateTime.Now;
			XIE.CxImage src = this.ImageNode.Data;
			if (src == null) return;

			// --------------------------------------------------
			// Split
			if (ReferenceEquals(sender, menuFilterSplit))
			{
				var prefix = ApiHelper.GetFileNameWithoutExtensionForImage(this.ImageNode.Name);
				var suffix = ApiHelper.MakeFileNameSuffix(timestamp, true);
				for (int ch = 0; ch < src.Channels; ch++)
				{
					using (var src_child = src.Child(ch))
					{
						var packs = src_child.Model.Pack;
						for (int k = 0; k < packs; k++)
						{
							var dst = new XIE.CxImage(src.Width, src.Height, new XIE.TxModel(src.Model.Type, 1), 1);
							dst.Filter().CopyEx(src_child, k, 1);
							dst.Depth = src.Depth;
							dst.ExifCopy(src.Exif());

							var name = string.Format("{0}-{1}-{2}", prefix, suffix, ch * packs + k);
							var args = new XIE.Tasks.CxAuxNotifyEventArgs_AddImage(dst, name);
							this.ImageNode.AuxInfo.SendRequested(this, args);
						}
					}
				}
				return;
			}
		}

		/// <summary>
		/// toolFilter: サイズ変更時の濃度補間モードのメニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuFilterScaleInterpolation_DropDownOpened(object sender, EventArgs e)
		{
			var menus = new ToolStripMenuItem[]
			{
				menuFilterScaleInterpolation0,
				menuFilterScaleInterpolation1,
			};
			foreach (var menu in menus)
			{
				if (menu.Tag == null) continue;
				var mode = Convert.ToInt32(menu.Tag);
				menu.Checked = (this.FilterScaleInterpolation == mode);
			}
		}

		/// <summary>
		/// toolFilter: サイズ変更時の濃度補間モードのメニューが選択されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuFilterScaleInterpolation_Click(object sender, EventArgs e)
		{
			var menu = sender as ToolStripMenuItem;
			if (menu == null) return;
			if (menu.Tag == null) return;

			this.FilterScaleInterpolation = Convert.ToInt32(menu.Tag);

			// 再表示:
			toolFilter.ShowDropDown();
			menuFilterScale.ShowDropDown();
			menuFilterScaleInterpolation.ShowDropDown();
		}

		/// <summary>
		/// サイズ変更時の濃度補間モード [0:しない、1:する]
		/// </summary>
		private int FilterScaleInterpolation = 1;

		#endregion

		#region toolbarView: (Preview)

		/// <summary>
		/// toolbarView: 画像プレビューフォームの表示切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolImagePreview_Click(object sender, EventArgs e)
		{
			if (PreviewForm == null)
			{
				PreviewForm = new CxImagePreviewForm(this.ImageView);
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
		/// フルスクリーン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFullscreen_Click(object sender, EventArgs e)
		{
			ChangeFullscreenMode(!this.IsFullscreen);
		}

		#endregion

		#region toolbarOverlay: (オーバレイ)

		/// <summary>
		/// オーバレイ編集モードの定数
		/// </summary>
		public enum ExEditMode
		{
			/// <summary>
			/// なし
			/// </summary>
			None,
			/// <summary>
			/// 図形操作モード
			/// </summary>
			Figure,
			/// <summary>
			/// ペイントモード (ブラシ)
			/// </summary>
			Brush,
			/// <summary>
			/// ペイントモード (水滴)
			/// </summary>
			Drop,
		}

		/// <summary>
		/// オーバレイ編集モード
		/// </summary>
		private ExEditMode EditMode = ExEditMode.None;

		/// <summary>
		/// toolbarOverlay: プロパティグリッドの表示切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolProperty_Click(object sender, EventArgs e)
		{
			this.splitView.Panel2Collapsed = !this.splitView.Panel2Collapsed;
		}

		/// <summary>
		/// スケーリングモード選択メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolScalingMode_DropDownOpened(object sender, EventArgs e)
		{
			ToolStripMenuItem[] items = 
			{
				toolScalingModeNone,
				toolScalingModeTopLeft,
				toolScalingModeCenter,
			};

			foreach (ToolStripMenuItem item in items)
			{
				PropertyInfo prop = item.GetType().GetProperty("Tag");
				if (prop != null)
				{
					var mode1 = this.ImageNode.Overlay.ScalingMode;
					var mode2 = (XIE.GDI.ExGdiScalingMode)Enum.Parse(typeof(XIE.GDI.ExGdiScalingMode), (string)prop.GetValue(item, null), true);
					item.Checked = (mode1 == mode2);
				}
			}
		}

		/// <summary>
		/// スケーリングモードの切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolScalingMode_Click(object sender, EventArgs e)
		{
			PropertyInfo prop = sender.GetType().GetProperty("Tag");
			if (prop != null)
			{
				var mode = (XIE.GDI.ExGdiScalingMode)Enum.Parse(typeof(XIE.GDI.ExGdiScalingMode), (string)prop.GetValue(sender, null), true);
				this.ImageNode.Overlay.ScalingMode = mode;
				if (propertyOverlay.SelectedObject == this.ImageNode.Overlay)
				{
					this.propertyOverlay.Refresh();
				}
				this.ImageView.Refresh();
			}
		}

		/// <summary>
		/// toolbarOverlay: 図形操作の有効/無効の切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFigureActive_Click(object sender, EventArgs e)
		{
			if (this.EditMode != ExEditMode.Figure)
			{
				this.EditMode = ExEditMode.Figure;
				this.ImageNode.Overlay.Visible = true;
				this.propertyOverlay.SelectedObject = this.ImageNode.Overlay;
				this.propertyOverlay.Refresh();
			}
			else
			{
				this.EditMode = ExEditMode.None;
				this.propertyOverlay.SelectedObject = null;
				this.propertyOverlay.Refresh();
			}
			this.ImageView.Refresh();
		}

		/// <summary>
		/// toolbarOverlay: ペイントモード（ブラシ）
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPaintBrushMode_Click(object sender, EventArgs e)
		{
			if (this.EditMode != ExEditMode.Brush)
			{
				this.EditMode = ExEditMode.Brush;
				this.propertyOverlay.SelectedObject = this.ImageNode.PaintBrush;
				this.propertyOverlay.Refresh();
			}
			else
			{
				this.EditMode = ExEditMode.None;
				this.propertyOverlay.SelectedObject = null;
				this.propertyOverlay.Refresh();
			}
			this.ImageView.Refresh();
		}

		/// <summary>
		/// toolbarOverlay: ペイントモード (水滴)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPaintDropMode_Click(object sender, EventArgs e)
		{
			if (this.EditMode != ExEditMode.Drop)
			{
				this.EditMode = ExEditMode.Drop;
				this.propertyOverlay.SelectedObject = this.ImageNode.PaintDrop;
				this.propertyOverlay.Refresh();
			}
			else
			{
				this.EditMode = ExEditMode.None;
				this.propertyOverlay.SelectedObject = null;
				this.propertyOverlay.Refresh();
			}
			this.ImageView.Refresh();
		}

		#endregion

		#region toolbarOverlay: (図形)

		/// <summary>
		/// toolbarOverlay: 図形の追加
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFigureAdd_Click(object sender, EventArgs e)
		{
			this.ImageNode.AddHistory(1, false);

			XIE.GDI.IxGdi2d added_figure = null;

			var vis = this.ImageView.VisibleRect();
			var mag = this.ImageView.Magnification;
			var scaling_mode = this.ImageNode.Overlay.ScalingMode;

			#region 図形の新規生成:
			do
			{
				if (ReferenceEquals(sender, menuFigureAddPoint))
				{
					var center = vis.Location + vis.Size / 2;
					var size = new XIE.TxSizeD(10, 10);
					var figure = new XIE.GDI.CxGdiPoint(center);
					figure.PenColor = Color.Red;
					figure.AnchorSize = (mag < 1) ? size / mag : size;
					added_figure = figure;
					break;
				}
				if (ReferenceEquals(sender, menuFigureAddLineSegment))
				{
					var center = vis.Location + vis.Size / 2;
					var size = vis.Size / 8;
					var st = center - size;
					var ed = center + size;
					var figure = new XIE.GDI.CxGdiLineSegment(st, ed);
					figure.PenColor = Color.Red;
					added_figure = figure;
					break;
				}
				if (ReferenceEquals(sender, menuFigureAddLine))
				{
					var figure = new XIE.GDI.CxGdiLine(-0.75, 1, 0);
					figure.PenColor = Color.Red;
					added_figure = figure;
					break;
				}
				if (ReferenceEquals(sender, menuFigureAddCircle))
				{
					var center = vis.Location + vis.Size / 2;
					var radius = System.Math.Min(vis.Size.Width, vis.Size.Height) / 8;
					var figure = new XIE.GDI.CxGdiCircle(center, radius);
					figure.PenColor = Color.Red;
					added_figure = figure;
					break;
				}
				if (ReferenceEquals(sender, menuFigureAddCircleArc))
				{
					var center = vis.Location + vis.Size / 2;
					var radius = System.Math.Min(vis.Size.Width, vis.Size.Height) / 8;
					var figure = new XIE.GDI.CxGdiCircleArc(center, radius, 0, 270);
					figure.PenColor = Color.Red;
					added_figure = figure;
					break;
				}
				if (ReferenceEquals(sender, menuFigureAddEllipse))
				{
					var center = vis.Location + vis.Size / 2;
					var radius = vis.Size / 8;
					var figure = new XIE.GDI.CxGdiEllipse(center, radius.Width, radius.Height);
					figure.PenColor = Color.Red;
					added_figure = figure;
					break;
				}
				if (ReferenceEquals(sender, menuFigureAddEllipseArc))
				{
					var center = vis.Location + vis.Size / 2;
					var radius = vis.Size / 8;
					var figure = new XIE.GDI.CxGdiEllipseArc(center, radius.Width, radius.Height, 0, 270);
					figure.PenColor = Color.Red;
					added_figure = figure;
					break;
				}
				if (ReferenceEquals(sender, menuFigureAddRectangle))
				{
					var center = vis.Location + vis.Size / 2;
					var size = vis.Size / 8;
					var figure = new XIE.GDI.CxGdiRectangle(center - size, size * 2);
					figure.PenColor = Color.Red;
					added_figure = figure;
					break;
				}
				if (ReferenceEquals(sender, menuFigureAddTrapezoid))
				{
					var center = vis.Location + vis.Size / 2;
					var size1 = vis.Size / 8;
					var size2 = vis.Size / 6;
					var p1 = center + new XIE.TxSizeD(-size1.Width, -size1.Height);
					var p2 = center + new XIE.TxSizeD(+size1.Width, -size1.Height);
					var p3 = center + new XIE.TxSizeD(+size2.Width, +size1.Height);
					var p4 = center + new XIE.TxSizeD(-size2.Width, +size1.Height);
					var figure = new XIE.GDI.CxGdiTrapezoid(p1, p2, p3, p4);
					figure.PenColor = Color.Red;
					added_figure = figure;
					break;
				}
				if (ReferenceEquals(sender, menuFigureAddPolyline))
				{
					var center = vis.Location + vis.Size / 2;
					var size1 = vis.Size / 8;
					var size2 = vis.Size / 16;
					var points = new XIE.TxPointD[]
					{
						center + new XIE.TxSizeD(-size1.Width, -size1.Height),
						center + new XIE.TxSizeD(-size2.Width, +size1.Height),
						center,
						center + new XIE.TxSizeD(+size2.Width, +size1.Height),
						center + new XIE.TxSizeD(+size1.Width, -size1.Height),
					};
					var figure = new XIE.GDI.CxGdiPolyline(points);
					figure.PenColor = Color.Red;
					added_figure = figure;
					break;
				}
				if (ReferenceEquals(sender, menuFigureAddString))
				{
					var figure = new XIE.GDI.CxGdiString();
					var center = vis.Location + vis.Size / 2;
					figure.Location = center;
					figure.PenColor = Color.Red;
					var now = DateTime.Now;
					figure.Text = string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}.{6:000}",
						now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
					added_figure = figure;
					break;
				}
			} while (false);
			#endregion

			if (added_figure != null)
			{
				if (this.EditMode != ExEditMode.Figure)
					this.EditMode = ExEditMode.Figure;
				if (this.ImageNode.Overlay.Visible != true)
					this.ImageNode.Overlay.Visible = true;

				this.ImageNode.Overlay.Figures.Add(added_figure);
				this.ImageNode.Overlay.SelectedFigures.Dispose();
				this.ImageNode.Overlay.SelectedFigures.Add(added_figure);
				this.ImageNode.Overlay.SelectedFigure = added_figure;
				this.propertyOverlay.SelectedObject = added_figure;
				this.propertyOverlay.Refresh();
				this.ImageView.Refresh();
			}
		}

		/// <summary>
		/// toolbarOverlay: 図形の整列
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFigureAlign_Click(object sender, EventArgs e)
		{
			if (this.ImageNode.Overlay.SelectedFigures.IsEmpty == false)
			{
				this.ImageNode.AddHistory(1, false);	// 編集履歴:

				foreach (var item in this.ImageNode.Overlay.SelectedFigures.Figures)
				{
					item.Key.Location = new TxPointD(
						System.Math.Truncate(item.Key.Location.X),
						System.Math.Truncate(item.Key.Location.Y)
						);
				}
				this.ImageView.Refresh();
			}
		}

		/// <summary>
		/// toolbarOverlay: 図形の複製
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFigureClone_Click(object sender, EventArgs e)
		{
			if (this.ImageNode.Overlay.SelectedFigures.IsEmpty == false)
			{
				this.ImageNode.AddHistory(1, false);	// 編集履歴:

				var clones = new List<XIE.GDI.IxGdi2d>();
				foreach (var item in this.ImageNode.Overlay.SelectedFigures.Figures)
				{
					if (item.Key is ICloneable)
						clones.Add((XIE.GDI.IxGdi2d)((ICloneable)item.Key).Clone());
				}
				this.ImageNode.Overlay.Figures.AddRange(clones);
				this.ImageView.Refresh();
			}
		}

		/// <summary>
		/// toolbarOverlay: 図形のラスター変換
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFigureDraw_Click(object sender, EventArgs e)
		{
			this.ImageNode.AddHistory(0, true);	// 編集履歴:
			{
				this.ImageNode.Overlay.SelectedFigures.Dispose();
				this.ImageNode.Overlay.SelectedFigure = null;

				var display_size = ImageView.Image.Size;
				var view_point = ImageView.ViewPoint;
				var mag = 1;
				using (var data = ImageView.Snapshot(display_size, view_point, mag))
				{
					this.ImageNode.Data.CopyFrom(data);
				}
			}

			this.ImageNode.AddHistory(1, false);	// 編集履歴:
			this.ImageNode.Overlay.Dispose();

			this.propertyOverlay.SelectedObject = null;
			this.propertyOverlay.Refresh();
			this.ImageView.Refresh();

			CxAuxInfoForm.AuxInfo.SendRequested(this.ImageNode, new XIE.Tasks.CxAuxNotifyEventArgs_Refresh());
		}

		/// <summary>
		/// toolbarOverlay: 図形の消去
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFigureClear_Click(object sender, EventArgs e)
		{
			this.ImageNode.AddHistory(1, false);	// 編集履歴:

			if (this.ImageNode.Overlay.SelectedFigures.IsEmpty)
			{
				this.ImageNode.Overlay.Dispose();
				this.propertyOverlay.SelectedObject = null;
				this.propertyOverlay.Refresh();
				this.ImageView.Refresh();
			}
			else
			{
				var figures = new List<XIE.GDI.IxGdi2d>(this.ImageNode.Overlay.SelectedFigures.Figures.Keys);
				this.ImageNode.Overlay.SelectedFigures.Dispose();
				this.ImageNode.Overlay.SelectedFigure = null;
				foreach (var figure in figures)
				{
					this.ImageNode.Overlay.Figures.Remove(figure);
					if (figure is IDisposable)
						((IDisposable)figure).Dispose();
				}
				this.propertyOverlay.SelectedObject = null;
				this.propertyOverlay.Refresh();
				this.ImageView.Refresh();
			}
		}

		#endregion

		#region statusbar:

		/// <summary>
		/// ステータスバー表示更新:
		/// </summary>
		private void statusbar_Update()
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
					double mag = view.Magnification;
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
		// propertyOverlay
		// //////////////////////////////////////////////////

		#region propertyOverlay: (コントロールイベント)

		/// <summary>
		/// propertyOverlay: 値が変更されたとき
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private void propertyOverlay_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			if (propertyOverlay.SelectedObject is XIE.GDI.IxGdi2d)
			{
				var figure = (XIE.GDI.IxGdi2d)propertyOverlay.SelectedObject;
				if (this.ImageNode.Overlay.SelectedFigures.Exists(figure))
				{
					this.ImageNode.Overlay.SelectedFigures.Remove(figure);
					this.ImageNode.Overlay.SelectedFigures.Add(figure);
				}
			}

			this.ImageView.Refresh();

			if (this.propertyOverlay_Focused == true)
				this.propertyOverlay.Refresh();
		}

		/// <summary>
		/// propertyOverlay: フォーカス状態
		/// </summary>
		private bool propertyOverlay_Focused = false;

		/// <summary>
		/// propertyOverlay: フォーカスが当たったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void propertyOverlay_Enter(object sender, EventArgs e)
		{
			this.propertyOverlay_Focused = true;
		}

		/// <summary>
		/// propertyOverlay: フォーカスが外れたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void propertyOverlay_Leave(object sender, EventArgs e)
		{
			this.propertyOverlay_Focused = false;
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
			get { return splitView.Panel1; }
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
					form.Location = ctrl.PointToScreen(location);
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

		#region ImageView: (描画/操作)

		/// <summary>
		/// ImageView: 描画イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			lock(this.ImageNode.Overlay)
			{
				this.ImageNode.Overlay.Rendering(sender, e);

				switch (this.EditMode)
				{
					case ExEditMode.None:
						break;
					case ExEditMode.Figure:
						break;
					case ExEditMode.Brush:
						this.ImageNode.PaintBrush.Rendering(sender, e);
						break;
					case ExEditMode.Drop:
						this.ImageNode.PaintDrop.Rendering(sender, e);
						break;
				}
			}
		}

		/// <summary>
		/// ImageView: 描画完了イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_Rendered(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			if (this.PreviewForm != null)
				this.PreviewForm.Refresh();
			if (this.propertyOverlay_Focused == false)
				this.propertyOverlay.Refresh();
		}

		/// <summary>
		/// ImageView: 操作イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_Handling(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			lock (this.ImageNode.Overlay)
			{
				switch (this.EditMode)
				{
					case ExEditMode.None:
						break;
					case ExEditMode.Figure:
						try
						{
							this.ImageNode.Overlay.BeginHandling = (object _sender, XIE.GDI.CxHandlingEventArgs _e) =>
								{
									this.ImageNode.AddHistory(1, false);
								};

							this.ImageNode.Overlay.Handling(sender, e);

							if (this.propertyOverlay.SelectedObject != this.ImageNode.Overlay.SelectedFigure)
							{
								this.propertyOverlay.SelectedObject = this.ImageNode.Overlay.SelectedFigure;
								this.propertyOverlay.Refresh();
							}
						}
						finally
						{
							this.ImageNode.Overlay.BeginHandling = null;
						}
						break;
					case ExEditMode.Brush:
						try
						{
							this.ImageNode.PaintBrush.BeginHandling = (object _sender, XIE.GDI.CxHandlingEventArgs _e) =>
							{
								this.ImageNode.AddHistory(0, true);
							};

							var is_updated = e.IsUpdated;
							this.ImageNode.PaintBrush.Handling(sender, e);

							if (!is_updated && e.IsUpdated)
							{
								CxAuxInfoForm.AuxInfo.SendRequested(this.ImageNode, new XIE.Tasks.CxAuxNotifyEventArgs_Refresh());
							}
						}
						finally
						{
							this.ImageNode.PaintBrush.BeginHandling = null;
						}
						break;
					case ExEditMode.Drop:
						try
						{
							this.ImageNode.PaintDrop.BeginHandling = (object _sender, XIE.GDI.CxHandlingEventArgs _e) =>
							{
								this.ImageNode.AddHistory(0, true);
							};

							var is_updated = e.IsUpdated;
							this.ImageNode.PaintDrop.Handling(sender, e);

							if (!is_updated && e.IsUpdated)
							{
								CxAuxInfoForm.AuxInfo.SendRequested(this.ImageNode, new XIE.Tasks.CxAuxNotifyEventArgs_Refresh());
							}
						}
						finally
						{
							this.ImageNode.PaintDrop.BeginHandling = null;
						}
						break;
				}
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

			#region CTRL+[X] カット.
			if (e.KeyCode == Keys.X && !e.Alt && e.Control && !e.Shift)
			{
				toolViewCut_Click(sender, e);
			}
			#endregion

			#region CTRL+[V] ペースト.
			if (e.KeyCode == Keys.V && !e.Alt && e.Control && !e.Shift)
			{
				toolViewPaste_Click(sender, e);
			}
			#endregion

			#region CTRL+ALT+[V] ペースト.(アルファフィールドを使用する)
			if (e.KeyCode == Keys.V && e.Alt && e.Control && !e.Shift)
			{
				toolViewPasteUseAlpha_Click(sender, e);
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
						PanelTreeView_Collapsed = splitView.Panel2Collapsed;

						// パネル非表示.
						splitView.Panel2Collapsed = true;

						// ツールバー、ステータスバーを非表示にする.
						toolbarView.Visible = false;
						toolbarOverlay.Visible = false;
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
						splitView.Panel2Collapsed = PanelTreeView_Collapsed;

						// ツールバー、ステータスバーを復元する.
						toolbarView.Visible = true;
						toolbarOverlay.Visible = true;
						statusbar.Visible = true;

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

			menuEditUndo.Enabled = this.ImageNode.CanUndo();
			menuEditRedo.Enabled = this.ImageNode.CanRedo();

			menuViewCut.Enabled = (this.ImageView.Image != null);
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
				menuViewPaste.Enabled = clipboard_exist;
			}

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
			toolImagePreview_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: Undo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuEditUndo_Click(object sender, EventArgs e)
		{
			toolEditUndo_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: Redo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuEditRedo_Click(object sender, EventArgs e)
		{
			toolEditRedo_Click(sender, e);
		}

		/// <summary>
		/// ImageViewMenu: 切り取り
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewCut_Click(object sender, EventArgs e)
		{
			toolViewCut_Click(sender, e);
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
		/// ImageViewMenu: 貼り付け (アルファフィールドを使用する)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuViewPasteUseAlpha_Click(object sender, EventArgs e)
		{
			toolViewPasteUseAlpha_Click(sender, e);
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
	}
}
