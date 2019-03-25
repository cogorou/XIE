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
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using XIE;

namespace XIEstudio
{
	/// <summary>
	/// ROIOverlay フォーム
	/// </summary>
	public partial class CxROIForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="view">監視対象の画像ビュー</param>
		/// <param name="roi">編集対象の処理範囲オーバレイ</param>
		/// <param name="node">編集対象の画像ノード (無ければ null を指定してください。)</param>
		public CxROIForm(XIE.GDI.CxImageView view, XIE.GDI.CxOverlayROI roi, XIE.Tasks.IxAuxNodeImageOut node)
		{
			InitializeComponent();
			this.ImageView = view;
			this.ROIOverlay = roi;
			this.Node = node;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 監視対象の画像ビュー
		/// </summary>
		public XIE.GDI.CxImageView ImageView
		{
			get { return m_ImageView; }
			set { m_ImageView = value; }
		}
		private XIE.GDI.CxImageView m_ImageView = null;

		/// <summary>
		/// 編集対象の処理範囲オーバレイ
		/// </summary>
		public XIE.GDI.CxOverlayROI ROIOverlay
		{
			get { return m_ROIOverlay; }
			set { m_ROIOverlay = value; }
		}
		private XIE.GDI.CxOverlayROI m_ROIOverlay = null;

		/// <summary>
		/// 編集対象の画像ノード
		/// </summary>
		public XIE.Tasks.IxAuxNodeImageOut Node
		{
			get { return m_Node; }
			set { m_Node = value; }
		}
		private XIE.Tasks.IxAuxNodeImageOut m_Node = null;

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxROIForm_Load(object sender, EventArgs e)
		{
			if (Owner != null)
			{
				Owner.Move += OwnerForm_Move;
				this.RelatedLocation = new Point(
					this.Location.X - Owner.Location.X,
					this.Location.Y - Owner.Location.Y
					);
			}

			if (this.ImageView != null)
				this.ImageView.Rendered += ImageView_Rendered;

			numericUpDown_Update();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxROIForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.ImageView != null)
				this.ImageView.Rendered -= ImageView_Rendered;
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 情報更新
		/// </summary>
		public void UpdateInfo()
		{
			if (this.ImageView != null)
				this.ImageView.Refresh();
		}

		#endregion

		#region コントロールイベント:

		/// <summary>
		/// フォームがアクティブになったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxROIForm_Activated(object sender, EventArgs e)
		{
			this.Opacity = 1.0;
		}

		/// <summary>
		/// フォームが非アクティブになったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxROIForm_Deactivate(object sender, EventArgs e)
		{
			this.Opacity = 0.9;
		}

		/// <summary>
		/// 自信のフォームが移動したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxROIForm_Move(object sender, EventArgs e)
		{
			if (Owner != null)
			{
				this.RelatedLocation = new Point(
					this.Location.X - Owner.Location.X,
					this.Location.Y - Owner.Location.Y
					);
			}
		}
		private Point RelatedLocation = new Point();

		#endregion

		#region コントロールイベント: (Owner)

		/// <summary>
		/// オーナーフォームが移動したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OwnerForm_Move(object sender, EventArgs e)
		{
			if (Owner != null)
			{
				this.Location = new Point(this.RelatedLocation.X + Owner.Location.X, this.RelatedLocation.Y + Owner.Location.Y);
			}
		}

		/// <summary>
		/// 画像ビューの表示更新イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageView_Rendered(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			numericUpDown_Update();
		}

		#endregion

		#region コントロールイベント: (数値入力)

		/// <summary>
		/// 数値入力コントロールの更新
		/// </summary>
		private void numericUpDown_Update()
		{
			if (this.ImageView != null &&
				this.ImageView.Image != null &&
				this.ROIOverlay != null)
			{
				try
				{
					Ignore = true;

					var image = this.ImageView.Image;
					var roi = (XIE.TxRectangleI)this.ROIOverlay.Shape;
					if (this.Node != null)
						this.Node.ROI = roi;

					numX.Enabled = true;
					numY.Enabled = true;
					numWidth.Enabled = true;
					numHeight.Enabled = true;
					textWindow.Enabled = true;

					numX.Minimum = 0;
					numX.Maximum = image.Width;
					if (numX.Minimum <= roi.X && roi.X <= numX.Maximum)
						numX.Value = roi.X;

					numY.Minimum = 0;
					numY.Maximum = image.Height;
					if (numY.Minimum <= roi.Y && roi.Y <= numY.Maximum)
						numY.Value = roi.Y;

					numWidth.Minimum = 0;
					numWidth.Maximum = image.Width;
					if (numWidth.Minimum <= roi.Width && roi.Width <= numWidth.Maximum)
						numWidth.Value = roi.Width;

					numHeight.Minimum = 0;
					numHeight.Maximum = image.Height;
					if (numHeight.Minimum <= roi.Height && roi.Height <= numHeight.Maximum)
						numHeight.Value = roi.Height;

					textWindow.Text = string.Format(
						"{0},{1},{2},{3}",
						numX.Value,
						numY.Value,
						numWidth.Value,
						numHeight.Value
						);
				}
				finally
				{
					Ignore = false;
				}
			}
			else
			{
				numX.Enabled = false;
				numY.Enabled = false;
				numWidth.Enabled = false;
				numHeight.Enabled = false;
				textWindow.Enabled = false;
			}
		}
		private bool Ignore = false;

		/// <summary>
		/// 数値入力コントロールの値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (Ignore) return;

			try
			{
				if (this.ImageView != null &&
					this.ImageView.Image != null &&
					this.ROIOverlay != null)
				{
					var image_size = this.ImageView.Image.ImageSize;

					int sx = (int)(numX.Value);
					int sy = (int)(numY.Value);
					if (sx > image_size.Width - 1)
						sx = image_size.Width - 1;
					if (sx < 0)
						sx = 0;
					if (sy > image_size.Height - 1)
						sy = image_size.Height - 1;
					if (sy < 0)
						sy = 0;

					int ex = (int)(sx + numWidth.Value - 1);
					int ey = (int)(sy + numHeight.Value - 1);
					if (ReferenceEquals(sender, numX) || ReferenceEquals(sender, numY))
					{
						if (ex > image_size.Width - 1)
							ex = image_size.Width - 1;
						if (ey > image_size.Height - 1)
							ey = image_size.Height - 1;
					}
					else
					{
						if (ex > image_size.Width - 1)
						{
							sx -= (ex - (image_size.Width - 1));
							if (sx < 0)
								sx = 0;
							ex = image_size.Width - 1;
						}
						if (ey > image_size.Height - 1)
						{
							sy -= (ey - (image_size.Height - 1));
							if (sy < 0)
								sy = 0;
							ey = image_size.Height - 1;
						}
					}
					if (ex < 0)
						ex = 0;
					if (ey < 0)
						ey = 0;

					var roi = new XIE.TxRectangleI(sx, sy, (ex - sx + 1), (ey - sy + 1));

					#region アスペクト比:
					if (this.ROIOverlay.AspectRatio.Width > 0 && this.ROIOverlay.AspectRatio.Height > 0)
					{
						if (ReferenceEquals(sender, numX) ||
							ReferenceEquals(sender, numWidth))
						{
							var height = (int)System.Math.Round(roi.Width * this.ROIOverlay.AspectRatio.Height / this.ROIOverlay.AspectRatio.Width);
							if (roi.Y + height <= image_size.Height)
							{
								roi.Height = height;
							}
							else
							{
								roi.Height = image_size.Height - roi.Y;
								roi.Width = (int)System.Math.Round(roi.Height * this.ROIOverlay.AspectRatio.Width / this.ROIOverlay.AspectRatio.Height);
							}
						}
						else if (ReferenceEquals(sender, numY) ||
								ReferenceEquals(sender, numHeight))
						{
							var width = (int)System.Math.Round(roi.Height * this.ROIOverlay.AspectRatio.Width / this.ROIOverlay.AspectRatio.Height);
							if (roi.X + width <= image_size.Width)
							{
								roi.Width = width;
							}
							else
							{
								roi.Width = image_size.Width - roi.X;
								roi.Height = (int)System.Math.Round(roi.Width * this.ROIOverlay.AspectRatio.Height / this.ROIOverlay.AspectRatio.Width);
							}
						}
					}
					#endregion

					this.ROIOverlay.Shape = roi;
					this.ImageView.Refresh();
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// 数値入力コントロールがフォーカスを得たとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericUpDown_Enter(object sender, EventArgs e)
		{
			var ctrl = sender as NumericUpDown;
			if (ctrl != null)
			{
				ctrl.Select(0, ctrl.Text.Length);
			}
		}

		/// <summary>
		/// テキストボックスの値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textWindow_TextChanged(object sender, EventArgs e)
		{
			if (Ignore) return;

			try
			{
				if (this.ImageView != null &&
					this.ImageView.Image != null &&
					this.ROIOverlay != null)
				{
					string[] items = textWindow.Text.Split(new char[] { ',' });

					var roi = (XIE.TxRectangleI)this.ROIOverlay.Shape;

					if (items.Length > 0) roi.X = Convert.ToInt32(items[0]);
					if (items.Length > 1) roi.Y = Convert.ToInt32(items[1]);
					if (items.Length > 2) roi.Width = Convert.ToInt32(items[2]);
					if (items.Length > 3) roi.Height = Convert.ToInt32(items[3]);

					var image_size = this.ImageView.Image.ImageSize;

					if (0 <= roi.X && roi.X < image_size.Width &&
						0 <= roi.Y && roi.Y < image_size.Height &&
						roi.X + roi.Width <= image_size.Width &&
						roi.Y + roi.Height <= image_size.Height)
					{
						#region アスペクト比:
						if (this.ROIOverlay.AspectRatio.Width > 0 && this.ROIOverlay.AspectRatio.Height > 0)
						{
							var width = (int)System.Math.Round(roi.Height * this.ROIOverlay.AspectRatio.Width / this.ROIOverlay.AspectRatio.Height);
							var height = (int)System.Math.Round(roi.Width * this.ROIOverlay.AspectRatio.Height / this.ROIOverlay.AspectRatio.Width);
							if (this.ROIOverlay.AspectRatio.Width > this.ROIOverlay.AspectRatio.Height)
							{
								if (roi.Y + height <= image_size.Height)
								{
									roi.Height = height;
								}
								else if (roi.X + width > image_size.Width)
								{
									roi.Width = width;
								}
							}
							else
							{
								if (roi.X + width > image_size.Width)
								{
									roi.Width = width;
								}
								else if (roi.Y + height <= image_size.Height)
								{
									roi.Height = height;
								}
							}
						}
						#endregion

						this.ROIOverlay.Shape = roi;
					}
					this.ImageView.Refresh();
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// テキストボックスがフォーカスを得たとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textWindow_Enter(object sender, EventArgs e)
		{
			var ctrl = sender as TextBox;
			if (ctrl != null)
			{
				ctrl.Select(0, ctrl.Text.Length);
			}
		}

		#endregion

		#region コントロールイベント: (ボタン)

		private void buttonGrid_Click(object sender, EventArgs e)
		{
		}

		private void buttonFill_Click(object sender, EventArgs e)
		{
		}

		private void buttonSnap_Click(object sender, EventArgs e)
		{
		}

		private void buttonReset_Click(object sender, EventArgs e)
		{
		}

		#endregion

		#region toolbar:

		/// <summary>
		/// PenColor の基本色
		/// </summary>
		private XIE.TxRGB8x4[] PenColors = new XIE.TxRGB8x4[]
			{
				new XIE.TxRGB8x4(0x00, 0x00, 0x00),	// Bk
				new XIE.TxRGB8x4(0xFF, 0xFF, 0xFF),	// Wh
				new XIE.TxRGB8x4(0xFF, 0x00, 0x00),	// R
				new XIE.TxRGB8x4(0x00, 0xFF, 0x00),	// G
				new XIE.TxRGB8x4(0x00, 0x00, 0xFF),	// B
				new XIE.TxRGB8x4(0xFF, 0x00, 0xFF),	// M
				new XIE.TxRGB8x4(0xFF, 0xFF, 0x00),	// Y
				new XIE.TxRGB8x4(0x00, 0xFF, 0xFF),	// C
			};

		/// <summary>
		/// PenColor のカスタム色
		/// </summary>
		private int[] CustomColors = new int[0];

		/// <summary>
		/// PenColor メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPenColor_DropDownOpened(object sender, EventArgs e)
		{
			var buttons = new ToolStripMenuItem[]
			{
				menuPenColor0,
				menuPenColor1,
				menuPenColor2,
				menuPenColor3,
				menuPenColor4,
				menuPenColor5,
				menuPenColor6,
				menuPenColor7,
			};

			for (int i = 0; i < PenColors.Length; i++)
			{
				buttons[i].Checked = (PenColors[i] == this.ROIOverlay.PenColor);
			}
		}

		/// <summary>
		/// PenColor が押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPenColor_Click(object sender, EventArgs e)
		{
			if (this.ImageView == null) return;
			if (this.ROIOverlay != null)
			{
				var button = sender as ToolStripMenuItem;
				if (button != null)
				{
					var index = Convert.ToInt32(button.Tag);

					if (0 <= index && index < this.PenColors.Length)
					{
						this.ROIOverlay.PenColor = this.PenColors[index];
						this.ImageView.Refresh();
					}
					else
					{
						if (this.CustomColors.Length == 0)
						{
							this.CustomColors = new int[this.PenColors.Length];
							for(int i=0 ; i<this.CustomColors.Length ; i++)
							{
								this.CustomColors[i] = this.PenColors[i].B << 16 | this.PenColors[i].G << 8 | this.PenColors[i].R;
							}
						}

						var dlg = new ColorDialog();
						//dlg.AnyColor = true;
						//dlg.AllowFullOpen = true;
						//dlg.SolidColorOnly = true;
						dlg.CustomColors = this.CustomColors;
						dlg.Color = this.ROIOverlay.PenColor;
						if (dlg.ShowDialog(this) == DialogResult.OK)
						{
							this.ROIOverlay.PenColor = dlg.Color;
							this.ImageView.Refresh();
						}
					}

					CxAuxInfoForm.AppSettings.ROIPenColor = this.ROIOverlay.PenColor;
				}
			}
		}

		/// <summary>
		/// Grid が押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolGrid_ButtonClick(object sender, EventArgs e)
		{
			if (this.ImageView == null) return;

			if (this.ROIOverlay != null)
			{
				this.ROIOverlay.Grid = !this.ROIOverlay.Grid;
				CxAuxInfoForm.AppSettings.ROIGrid = this.ROIOverlay.Grid;
				this.ImageView.Refresh();
			}
		}

		/// <summary>
		///  Grid メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolGrid_DropDownOpened(object sender, EventArgs e)
		{
			var type = CxAuxInfoForm.AppSettings.ROIGridType;
			toolGridType0.Checked = (type == 0);
			toolGridType1.Checked = (type == 1);
		}

		/// <summary>
		/// GridType メニューが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolGridType_Click(object sender, EventArgs e)
		{
			var item = (ToolStripMenuItem)sender;
			int type = Convert.ToInt32(item.Tag);
			CxAuxInfoForm.AppSettings.ROIGridType = type;

			if (this.ROIOverlay != null)
			{
				this.ROIOverlay.GridType = type;
				this.ImageView.Refresh();
			}
		}

		/// <summary>
		/// Fill が押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFill_Click(object sender, EventArgs e)
		{
			if (this.ImageView == null) return;

			if (this.ROIOverlay != null)
			{
				if (this.ROIOverlay.BrushStyle != XIE.GDI.ExGdiBrushStyle.None)
					this.ROIOverlay.BrushStyle = XIE.GDI.ExGdiBrushStyle.None;
				else
					this.ROIOverlay.BrushStyle = XIE.GDI.ExGdiBrushStyle.Solid;

				CxAuxInfoForm.AppSettings.ROIBkEnable = !(this.ROIOverlay.BrushStyle == XIE.GDI.ExGdiBrushStyle.None);

				this.ImageView.Refresh();
			}
		}

		#endregion

		#region toolbar: (Reset)

		/// <summary>
		/// Reset ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolReset_ButtonClick(object sender, EventArgs e)
		{
			if (this.ImageView == null) return;

			if (this.ROIOverlay != null)
			{
				var roi = new XIE.TxRectangleI();

				#region リセット:
				var view = this.ImageView;
				var image = this.ImageView.Image;
				var vis = view.VisibleRectI(true);
				if (vis.Width != image.Width ||
					vis.Height != image.Height)
				{
					vis.X += vis.Width / 4;
					vis.Y += vis.Height / 4;
					vis.Width = vis.Width / 2;
					vis.Height = vis.Height / 2;
				}
				roi = vis;
				#endregion

				this.ROIOverlay.Shape = roi;

				this.ImageView.Refresh();
			}
		}

		/// <summary>
		/// 比率指定 の Reset メニューが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolResetRatio_Click(object sender, EventArgs e)
		{
			if (this.ImageView == null) return;

			if (this.ROIOverlay != null)
			{
				var menu = sender as ToolStripMenuItem;
				if (menu == null) return;

				var percent = Convert.ToInt32(menu.Tag);
				if (!(0 < percent && percent <= 100)) return;

				var size = this.ImageView.Image.Size;
				var vis = this.ImageView.VisibleRect();
				var roi_w = size.Width * percent / 100;
				var roi_h = size.Height * percent / 100;
				var roi_x = (int)(vis.X + (vis.Width - roi_w) / 2);
				var roi_y = (int)(vis.Y + (vis.Height - roi_h) / 2);
				if (roi_x + roi_w > size.Width)
					roi_x = size.Width - roi_w;
				if (roi_y + roi_h > size.Height)
					roi_y = size.Height - roi_h;
				if (roi_x < 0)
					roi_x = 0;
				if (roi_y < 0)
					roi_y = 0;
				var roi = new XIE.TxRectangleI(roi_x, roi_y, roi_w, roi_h);

				this.ROIOverlay.Shape = roi;

				this.ImageView.Refresh();
			}
		}

		#endregion

		#region toolbar: (Aspect)

		/// <summary>
		/// アスペクト比
		/// </summary>
		private TxSizeI AspectRatio = new TxSizeI();

		/// <summary>
		/// Aspect ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolAspect_ButtonClick(object sender, EventArgs e)
		{
			var ax = AspectRatio.Width;
			var ay = AspectRatio.Height;

			var size = this.ImageView.Image.Size;
			var roi = ROIOverlay.Shape;
			if (ax == 0 || ay == 0)
			{
				ax = size.Width;
				ay = size.Height;
			}

			if (ax > ay)
				roi.Height = (int)(roi.Width * ay / ax);
			else
				roi.Width = (int)(roi.Height * ax / ay);

			if (roi.X + roi.Width > size.Width)
				roi.X = size.Width - roi.Width;
			if (roi.Y + roi.Height > size.Height)
				roi.Y = size.Height - roi.Height;
			if (roi.X < 0)
				roi.X = 0;
			if (roi.Y < 0)
				roi.Y = 0;

			ROIOverlay.Shape = roi;

			this.ImageView.Refresh();
		}

		/// <summary>
		/// Aspect メニューが開いたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolAspect_DropDownOpened(object sender, EventArgs e)
		{
			var menus = new ToolStripMenuItem[]
			{
				toolAspect0,
				toolAspect0302,
				toolAspect0403,
				toolAspect1609,
				toolAspect0203,
				toolAspect0304,
				toolAspect0916,
			};
			foreach (var item in menus)
			{
				var tag = item.Tag;
				if (tag == null) continue;
				var ratio = Convert.ToInt32(tag);
				item.Checked = (ratio == (AspectRatio.Width * 100 + AspectRatio.Height));
			}
		}

		/// <summary>
		/// Aspect メニューが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolAspect_Click(object sender, EventArgs e)
		{
			var tag = ((ToolStripMenuItem)sender).Tag;
			if (tag == null) return;
			var val = Convert.ToInt32(tag);
			AspectRatio = new TxSizeI(val / 100, val % 100);
			if (ReferenceEquals(sender, toolAspect0) == false)
			{
				toolAspect_ButtonClick(sender, e);
			}
			ROIOverlay.AspectRatio = AspectRatio;
		}

		#endregion

		#region toolbar: (Snap)

		/// <summary>
		/// Snap ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolSnap_Click(object sender, EventArgs e)
		{
			if (this.ROIOverlay != null &&
				this.ROIOverlay.CheckValidity(this.ImageView.Image))
			{
				try
				{
					var src = ImageView.Image;
					var roi = (XIE.TxRectangleD)this.ROIOverlay.Shape;
					using (var part = src.Child(roi))
					{
						var data = (CxImage)part.Clone();
						data.ExifCopy(src.Exif());
						if (this.Node is TreeNode)
						{
							var prefix = ApiHelper.GetFileNameWithoutExtensionForImage(((TreeNode)this.Node).Name);
							var name = prefix + "-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
							var args = new XIE.Tasks.CxAuxNotifyEventArgs_AddImage(data, name);
							CxAuxInfoForm.AuxInfo.SendRequested(this.Node, args);
						}
						else
						{
							var name = "ROI-" + ApiHelper.MakeFileNameSuffix(DateTime.Now, true);
							var args = new XIE.Tasks.CxAuxNotifyEventArgs_AddImage(data, name);
							CxAuxInfoForm.AuxInfo.SendRequested(this, args);
						}
					}
				}
				catch (System.Exception)
				{
				}
			}
		}

		#endregion
	}
}
