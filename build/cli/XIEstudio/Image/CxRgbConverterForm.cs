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
	/// RGB 変換フォーム
	/// </summary>
	public partial class CxRgbConverterForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxRgbConverterForm()
		{
			InitializeComponent();

			numericRedRatio.TextChanged += numericRedRatio_TextChanged;
			numericGreenRatio.TextChanged += numericGreenRatio_TextChanged;
			numericBlueRatio.TextChanged += numericBlueRatio_TextChanged;
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxRgbConverterForm_Load(object sender, EventArgs e)
		{
			if (SrcImage == null)
				throw new CxException(ExStatus.InvalidObject);
			if (DstImage == null)
				DstImage = new CxImage();

			numericRedRatio.Value = trackRedRatio.Value = (int)(Param.RedRatio * 100);
			numericGreenRatio.Value = trackGreenRatio.Value = (int)(Param.GreenRatio * 100);
			numericBlueRatio.Value = trackBlueRatio.Value = (int)(Param.BlueRatio * 100);

			#region 画像ビューの初期化:
			{
				var view = new XIE.GDI.CxImageView();
				ImageView = view;
				view.Image = this.SrcImage;
				view.Dock = DockStyle.Fill;
				view.BackgroundBrush = new XIE.GDI.TxGdiBrush(Color.FromArgb(64, 64, 64));
				view.Rendering += ImageView_Rendering;
				view.Resize += ImageView_Resize;
				panelView.Controls.Add(view);
				ImageView_Update(false);
			}
			#endregion

			Updated = true;

			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxRgbConverterForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (workerExecute.IsBusy)
			{
				e.Cancel = true;
				return;
			}

			timerUpdateUI.Stop();

			if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
			{
				Param.RedRatio = ((double)numericRedRatio.Value * 0.01);
				Param.GreenRatio = ((double)numericGreenRatio.Value * 0.01);
				Param.BlueRatio = ((double)numericBlueRatio.Value * 0.01);
			}
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 入力画像
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxRgbConverterForm.SrcImage")]
		public XIE.CxImage SrcImage
		{
			get { return m_SrcImage; }
			set { m_SrcImage = value; }
		}
		private XIE.CxImage m_SrcImage = null;

		/// <summary>
		/// 出力画像
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxRgbConverterForm.DstImage")]
		public XIE.CxImage DstImage
		{
			get { return m_DstImage; }
			set { m_DstImage = value; }
		}
		private XIE.CxImage m_DstImage = null;

		/// <summary>
		/// パラメータ
		/// </summary>
		public static XIE.Effectors.CxRgbConverter Param = new XIE.Effectors.CxRgbConverter();

		#endregion

		#region 実行:

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			if (Updated)
			{
				if (workerExecute.IsBusy == false)
				{
					Updated = false;
					Effector.RedRatio = ((double)numericRedRatio.Value * 0.01);
					Effector.GreenRatio = ((double)numericGreenRatio.Value * 0.01);
					Effector.BlueRatio = ((double)numericBlueRatio.Value * 0.01);
					workerExecute.RunWorkerAsync();
				}
			}
		}

		/// <summary>
		/// 更新の有無
		/// </summary>
		private bool Updated = false;

		/// <summary>
		/// イフェクタ
		/// </summary>
		private XIE.Effectors.CxRgbConverter Effector = new XIE.Effectors.CxRgbConverter();

		/// <summary>
		/// スレッド: 実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void workerExecute_DoWork(object sender, DoWorkEventArgs e)
		{
			if (SrcImage.ImageSize != DstImage.ImageSize)
				DstImage.Dispose();

			Effector.Execute(SrcImage, DstImage);
		}

		/// <summary>
		/// スレッド: 完了
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void workerExecute_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			ImageView.Image = this.DstImage;
			ImageView_Update(true);
		}

		#endregion

		#region ImageView:

		/// <summary>
		/// 画像ビュー
		/// </summary>
		private XIE.GDI.CxImageView ImageView = null;

		/// <summary>
		/// サイズ調整を行うか否か
		/// </summary>
		private bool IsFitImageSize = true;

		/// <summary>
		/// 画像ビューの画像更新
		/// </summary>
		/// <param name="refresh">表示更新するか否か</param>
		private void ImageView_Update(bool refresh)
		{
			var view = ImageView;
			if (IsFitImageSize)
				view.FitImageSize(0, 90);
			if (refresh)
				view.Refresh();
		}

		/// <summary>
		/// 画像ビューの描画イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ImageView_Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			var view = (XIE.GDI.CxImageView)sender;
		}

		/// <summary>
		/// 画像ビューがサイズ変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ImageView_Resize(object sender, EventArgs e)
		{
			var view = (XIE.GDI.CxImageView)sender;
			view.FitImageSize(0, 90);
			view.Refresh();
		}

		#endregion

		#region RedRatio:

		/// <summary>
		/// RedRatio: フォーカスが移ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericRedRatio_Enter(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			ctrl.Select(0, ctrl.Value.ToString().Length);
		}

		/// <summary>
		/// RedRatio: テキストが変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericRedRatio_TextChanged(object sender, EventArgs e)
		{
			numericRedRatio_ValueChanged(sender, e);
		}

		/// <summary>
		/// RedRatio: 値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericRedRatio_ValueChanged(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			if (ctrl.Focused)
			{
				if (ctrl.Minimum <= ctrl.Value && ctrl.Value <= ctrl.Maximum)
				{
					trackRedRatio.Value = (int)ctrl.Value;
					Updated = true;
				}
			}
		}

		/// <summary>
		/// RedRatio: スライダーが動かされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackRedRatio_Scroll(object sender, EventArgs e)
		{
			var ctrl = (TrackBar)sender;
			if (ctrl.Focused)
			{
				numericRedRatio.Value = ctrl.Value;
				Updated = true;
			}
		}

		#endregion

		#region GreenRatio:

		/// <summary>
		/// GreenRatio: フォーカスが移ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericGreenRatio_Enter(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			ctrl.Select(0, ctrl.Value.ToString().Length);
		}

		/// <summary>
		/// GreenRatio: テキストが変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericGreenRatio_TextChanged(object sender, EventArgs e)
		{
			numericGreenRatio_ValueChanged(sender, e);
		}

		/// <summary>
		/// GreenRatio: 値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericGreenRatio_ValueChanged(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			if (ctrl.Focused)
			{
				if (ctrl.Minimum <= ctrl.Value && ctrl.Value <= ctrl.Maximum)
				{
					trackGreenRatio.Value = (int)ctrl.Value;
					Updated = true;
				}
			}
		}

		/// <summary>
		/// GreenRatio: スライダーが動かされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackGreenRatio_Scroll(object sender, EventArgs e)
		{
			var ctrl = (TrackBar)sender;
			if (ctrl.Focused)
			{
				numericGreenRatio.Value = ctrl.Value;
				Updated = true;
			}
		}

		#endregion

		#region BlueRatio:

		/// <summary>
		/// BlueRatio: フォーカスが移ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericBlueRatio_Enter(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			ctrl.Select(0, ctrl.Value.ToString().Length);
		}

		/// <summary>
		/// BlueRatio: テキストが変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericBlueRatio_TextChanged(object sender, EventArgs e)
		{
			numericBlueRatio_ValueChanged(sender, e);
		}

		/// <summary>
		/// BlueRatio: 値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericBlueRatio_ValueChanged(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			if (ctrl.Focused)
			{
				if (ctrl.Minimum <= ctrl.Value && ctrl.Value <= ctrl.Maximum)
				{
					trackBlueRatio.Value = (int)ctrl.Value;
					Updated = true;
				}
			}
		}

		/// <summary>
		/// BlueRatio: スライダーが動かされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackBlueRatio_Scroll(object sender, EventArgs e)
		{
			var ctrl = (TrackBar)sender;
			if (ctrl.Focused)
			{
				numericBlueRatio.Value = ctrl.Value;
				Updated = true;
			}
		}

		#endregion
	}
}
