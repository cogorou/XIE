/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XIEstudio
{
	/// <summary>
	/// 画像プレビューフォーム
	/// </summary>
	public partial class CxImagePreviewForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="view">監視対象のビュー</param>
		public CxImagePreviewForm(XIE.GDI.CxImageView view)
		{
			InitializeComponent();
			this.TargetView = view;
			this.Preview.Dock = DockStyle.Fill;
			this.panelView.Controls.Add(this.Preview);

			this.VisibleRectFigure.PenColor = new XIE.TxRGB8x4(0xFF, 0x00, 0x00, 0xFF);	// 枠線:赤(不透明)
			this.VisibleRectFigure.PenWidth = 0;
			this.VisibleRectFigure.PenStyle = XIE.GDI.ExGdiPenStyle.Dot;
			this.VisibleRectFigure.Brush = new XIE.GDI.TxGdiBrush(new XIE.TxRGB8x4(0xFF, 0x00, 0x00, 0x3F));	// 背景:赤(更に透明)
			this.VisibleRectShadow.PenColor = new XIE.TxRGB8x4(0x00, 0x00, 0x00, 0xFF);	// 影線:黒(不透明)
			this.VisibleRectShadow.PenWidth = 0;
			this.VisibleRectShadow.PenStyle = XIE.GDI.ExGdiPenStyle.Solid;
		}

		#endregion

		#region オブジェクト:

		/// <summary>
		/// 監視対象の画像ビュー
		/// </summary>
		private XIE.GDI.CxImageView TargetView = null;

		/// <summary>
		/// プレビュー
		/// </summary>
		private XIE.GDI.CxImageView Preview = new XIE.GDI.CxImageView();

		/// <summary>
		/// 描画用図形 (可視範囲)
		/// </summary>
		private XIE.GDI.CxGdiRectangle VisibleRectFigure = XIE.GDI.CxGdiRectangle.Default;

		/// <summary>
		/// 描画用図形 (可視範囲) の影
		/// </summary>
		private XIE.GDI.CxGdiRectangle VisibleRectShadow = XIE.GDI.CxGdiRectangle.Default;

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxImagePreviewForm_Load(object sender, EventArgs e)
		{
			PreviewForm_Setup();
			Preview_Update();

			if (TargetView != null)
				TargetView.Rendered += TargetView_Rendered;

			toolViewHalftone.Checked = this.Preview.Halftone;
			toolDock.Checked = IsDocking;

			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxImagePreviewForm_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		#endregion

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			statusImageInfo_Update();
		}

		#region プロパティ:

		/// <summary>
		/// オーナーフォームへの追従モード
		/// </summary>
		public bool IsDocking
		{
			get { return m_IsDocking; }
			set { m_IsDocking = value; }
		}
		private bool m_IsDocking = true;

		#endregion

		#region ツールバー:

		/// <summary>
		/// ビューの Halftone モードの切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolViewHalftone_Click(object sender, EventArgs e)
		{
			this.Preview.Halftone = toolViewHalftone.Checked;
			this.Preview.Refresh();
		}

		/// <summary>
		/// フォームの追従モードの切り替え
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolDock_Click(object sender, EventArgs e)
		{
			IsDocking = toolDock.Checked;
		}

		#endregion

		#region statusbar:

		/// <summary>
		/// 画像情報の更新
		/// </summary>
		private void statusImageInfo_Update()
		{
			//XIE.GDI.CxImageView view = this.Preview;
			//XIE.CxImage image = view.Image;
			//if (image == null)
			//{
			//	statusCursorInfo.Text = "";
			//}
			//else
			//{
			//	// CursorInfo
			//	if (view.Canvas.Magnification < 2.0)
			//	{
			//		XIE.TxPointD dp = view.PointToClient(Control.MousePosition);
			//		XIE.TxPointD ip = view.Canvas.DPtoIP(dp, XIE.GDI.ExGdiScalingMode.TopLeft);
			//		statusCursorInfo.Text = string.Format("dp={0:F0},{1:F0} ip={2:F0},{3:F0}",
			//			dp.X, dp.Y,
			//			ip.X, ip.Y
			//		);
			//	}
			//	else
			//	{
			//		XIE.TxPointD dp = view.PointToClient(Control.MousePosition);
			//		XIE.TxPointD ip = view.Canvas.DPtoIP(dp, XIE.GDI.ExGdiScalingMode.Center);
			//		statusCursorInfo.Text = string.Format("dp={0:F0},{1:F0} ip={2:F1},{3:F1}",
			//			dp.X, dp.Y,
			//			ip.X, ip.Y
			//		);
			//	}
			//}
		}

		#endregion

		#region プレビュー: (初期化)

		/// <summary>
		/// プレビュー初期化
		/// </summary>
		private void PreviewForm_Setup()
		{
			Preview.Rendering += Preview_Rendering;
			Preview.MouseDown += Preview_MouseDown;
			Preview.MouseMove += Preview_MouseMove;
			Preview.PreviewKeyDown += Preview_PreviewKeyDown;
		}

		/// <summary>
		/// プレビュー解放
		/// </summary>
		private void PreviewForm_TearDown()
		{
			Preview.Rendering -= Preview_Rendering;
			Preview.MouseDown -= Preview_MouseDown;
			Preview.MouseMove -= Preview_MouseMove;
			Preview.PreviewKeyDown -= Preview_PreviewKeyDown;
		}

		/// <summary>
		/// プレビューの表示更新
		/// </summary>
		private void Preview_Update()
		{
			XIE.CxImage image = this.TargetView.Image;

			// 画像設定.
			this.Preview.Image = image;
			if (this.Preview.Image != null)
				this.Preview.FitImageSize(0);

			// 可視範囲の更新.
			XIE.TxRectangleD vis = this.TargetView.VisibleRect();
			VisibleRectFigure.Shape = vis;
			VisibleRectShadow.Shape = vis;

			// 表示更新.
			this.Preview.FitImageSize(0);
			this.Preview.Refresh();
		}

		#endregion

		#region プレビュー: (描画イベント)

		/// <summary>
		/// プレビューの描画
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Preview_Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			lock (this)
			{
				try
				{
					var image = this.TargetView.Image;
					var vis = this.TargetView.VisibleRect();

					if (image != null && (vis.Width < image.Width || vis.Height < image.Height))
					{
						XIE.GDI.IxGdi2d[] figures = 
								{
									VisibleRectShadow,
									VisibleRectFigure
								};

						e.Canvas.DrawOverlay(figures, XIE.GDI.ExGdiScalingMode.TopLeft);
					}
				}
				catch (System.Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
					System.Diagnostics.Debug.WriteLine(ex.StackTrace);
				}
				finally
				{
				}
			}
		}

		#endregion

		#region プレビュー: (マウス操作イベント)

		/// <summary>
		/// プレビューがクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Preview_MouseClick(object sender, MouseEventArgs e)
		{
		}

		/// <summary>
		/// プレビュー用のピクチャボックスでマウスが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Preview_MouseDown(object sender, MouseEventArgs e)
		{
			if (TargetView != null)
			{
				TargetView.ViewPoint = Preview.DPtoIP(e.Location, XIE.GDI.ExGdiScalingMode.TopLeft);
				TargetView.Refresh();
			}
		}

		/// <summary>
		/// プレビュー用のピクチャボックスでマウスが移動しているとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Preview_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (TargetView != null)
				{
					TargetView.ViewPoint = Preview.DPtoIP((XIE.TxPointD)e.Location, XIE.GDI.ExGdiScalingMode.TopLeft);
					TargetView.Refresh();
				}
			}
		}

		#endregion

		#region 自信のフォーム: (コントロールイベント)

		/// <summary>
		/// 自信のフォームのサイズが変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxImagePreviewForm_Resize(object sender, EventArgs e)
		{
			Preview.FitImageSize(0);
			Preview.Refresh();
		}

		#endregion

		#region 自信のビュー: (コントロールイベント)

		/// <summary>
		/// Preview: キーボード押下
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Preview_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			#region [HOME]
			if (e.KeyCode == Keys.Home && !e.Alt && !e.Control && !e.Shift)
			{
				if (Owner is CxAuxInfoForm)
				{
					var form = (CxAuxInfoForm)Owner;
					form.ProfileOverlay_MoveToCursorPosition();
				}
			}
			#endregion

			#region [END]
			if (e.KeyCode == Keys.End && !e.Alt && !e.Control && !e.Shift)
			{
				if (Owner is CxAuxInfoForm)
				{
					var form = (CxAuxInfoForm)Owner;
					form.ProfileOverlay_BackToPreviousPosition();
				}
			}
			#endregion
		}

		#endregion

		#region 監視対象: (コントロールイベント)

		/// <summary>
		/// 監視対象の描画完了イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TargetView_Rendered(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			// プレビューの表示更新.
			Preview_Update();
		}

		#endregion
	}
}
