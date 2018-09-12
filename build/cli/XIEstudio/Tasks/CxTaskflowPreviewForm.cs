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
using System.Diagnostics;

namespace XIEstudio
{
	/// <summary>
	/// タスクフロープレビューフォーム
	/// </summary>
	public partial class CxTaskflowPreviewForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="container">監視対象のビューのコンテナ</param>
		/// <param name="view">監視対象のビュー</param>
		/// <param name="taskflow">監視対象のタスクフロー</param>
		public CxTaskflowPreviewForm(Panel container, PictureBox view, CxTaskNode taskflow)
		{
			InitializeComponent();
			this.TargetContainer = container;
			this.TargetView = view;
			this.TaskNode = taskflow;
			this.Preview.Dock = DockStyle.Fill;
			this.Preview.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
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
		/// 監視対象のビューのコンテナ
		/// </summary>
		private Panel TargetContainer = null;

		/// <summary>
		/// 監視対象のビュー
		/// </summary>
		private PictureBox TargetView = null;

		/// <summary>
		/// プレビュー
		/// </summary>
		private PictureBox Preview = new PictureBox();

		/// <summary>
		/// 濃度補間モード
		/// </summary>
		private System.Drawing.Drawing2D.InterpolationMode InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;

		/// <summary>
		/// タスクノード
		/// </summary>
		private CxTaskNode TaskNode = new CxTaskNode();

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
		private void CxTaskflowPreviewForm_Load(object sender, EventArgs e)
		{
			PreviewForm_Setup();

			if (TargetView != null)
				TargetView.Paint += TargetView_Paint;

			toolDock.Checked = IsDocking;
			toolViewHalftone.Checked = (this.InterpolationMode == System.Drawing.Drawing2D.InterpolationMode.High);

			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowPreviewForm_FormClosing(object sender, FormClosingEventArgs e)
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
			statusbar_Update();
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
			this.InterpolationMode = toolViewHalftone.Checked
				? System.Drawing.Drawing2D.InterpolationMode.High
				: System.Drawing.Drawing2D.InterpolationMode.Default;
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
		private void statusbar_Update()
		{
		}

		#endregion

		#region プレビュー: (初期化)

		/// <summary>
		/// プレビュー初期化
		/// </summary>
		private void PreviewForm_Setup()
		{
			Preview.Paint += Preview_Paint;
			Preview.MouseDown += Preview_MouseDown;
			Preview.MouseMove += Preview_MouseMove;
		}

		/// <summary>
		/// プレビュー解放
		/// </summary>
		private void PreviewForm_TearDown()
		{
			Preview.Paint -= Preview_Paint;
			Preview.MouseDown -= Preview_MouseDown;
			Preview.MouseMove -= Preview_MouseMove;
		}

		#endregion

		#region プレビュー: (描画イベント)

		/// <summary>
		/// プレビューの描画
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Preview_Paint(object sender, PaintEventArgs e)
		{
			lock (this)
			{
				try
				{
					var graphics = e.Graphics;

					var mag = (float)this.TaskNode.Visualizer.Scale / 100;

					var preview_size = this.Preview.Size;
					var target_bounds = new Rectangle(this.TargetView.Location, this.TargetView.Size);
					var container_size = this.TargetContainer.Size;

					#region 表示倍率の調整.
					{
						float mag_x = (float)preview_size.Width / target_bounds.Width;
						float mag_y = (float)preview_size.Height / target_bounds.Height;
						float mag_min = System.Math.Min(mag_x, mag_y);

						if (mag_min < 1)
						{
							graphics.ResetTransform();
							graphics.ScaleTransform(mag_min, mag_min);
							graphics.InterpolationMode = this.InterpolationMode;
						}
						else
						{
							graphics.ResetTransform();
						}
					}
					#endregion

					#region 背景:
					using (var brush = new SolidBrush(this.TaskNode.Visualizer.SheetColor))
					{
						var rect = new Rectangle(
							0, 0,
							target_bounds.Width,
							target_bounds.Height
							);
						graphics.FillRectangle(brush, rect);
					}
					#endregion

					#region 可視範囲:
					if (this.TargetView != null && this.TargetContainer != null)
					{
						var rect = new Rectangle(
							-target_bounds.X,
							-target_bounds.Y,
							container_size.Width,
							container_size.Height
							);
						Pen pen = new Pen(Color.FromArgb(255, 0, 0));
						pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
						graphics.DrawRectangle(pen, rect);
					}
					#endregion

					#region タスクユニットの描画:
					graphics.ScaleTransform(mag, mag);
					var scope = (XIE.Tasks.CxTaskflow)this.TaskNode.CurrentTaskflow;
					if (scope != null)
					{
						foreach (var task in scope.TaskUnits)
						{
							try
							{
								this.TaskNode.Visualizer.Render(graphics, task);
							}
							catch (Exception ex)
							{
								XIE.Log.Api.Error(ex.StackTrace);
							}
						}
					}
					#endregion
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message);
				}
				finally
				{
				}
			}
		}

		#endregion

		#region プレビュー: (マウス操作イベント)

		/// <summary>
		/// プレビュー上のクリック位置に対応する視点へ移動する
		/// </summary>
		/// <param name="location">クリック位置</param>
		private void Preview_MoveViewPoint(Point location)
		{
			var container = this.TargetContainer;
			var picturebox = this.TargetView;

			if (container != null && picturebox != null)
			{
				var dst = picturebox.Location;

				float mag_x = (float)picturebox.Width / this.Preview.Width;
				float mag_y = (float)picturebox.Height / this.Preview.Height;
				float mag = System.Math.Max(mag_x, mag_y);

				if (container.Width < picturebox.Width)
				{
					int tx = (int)System.Math.Round(location.X * mag - (container.Width / 2));
					if (tx + container.Width > picturebox.Width)
						tx = picturebox.Width - container.Width;
					if (tx < 0)
						tx = 0;
					dst.X = -tx;
				}

				if (container.Height < picturebox.Height)
				{
					int ty = (int)System.Math.Round(location.Y * mag - (container.Height / 2));
					if (ty + container.Height > picturebox.Height)
						ty = picturebox.Height - container.Height;
					if (ty < 0)
						ty = 0;
					dst.Y = -ty;
				}

				picturebox.Location = dst;
				picturebox.Refresh();
			}
		}

		/// <summary>
		/// プレビュー用のピクチャボックスでマウスが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Preview_MouseDown(object sender, MouseEventArgs e)
		{
			Preview_MoveViewPoint(e.Location);
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
				Preview_MoveViewPoint(e.Location);
			}
		}

		#endregion

		#region 自信のフォーム: (コントロールイベント)


		/// <summary>
		/// 自信のフォームのサイズが変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowPreviewForm_Resize(object sender, EventArgs e)
		{
			Preview.Refresh();
		}

		#endregion

		#region 監視対象: (コントロールイベント)

		/// <summary>
		/// 監視対象の描画完了イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TargetView_Paint(object sender, PaintEventArgs e)
		{
			// プレビューの表示更新.
			this.Preview.Refresh();
		}

		#endregion
	}
}
