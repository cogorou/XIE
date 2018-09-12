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
	/// ガンマ変換フォーム
	/// </summary>
	public partial class CxGammaConverterForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGammaConverterForm()
		{
			InitializeComponent();

			numericGamma.TextChanged += numericGamma_TextChanged;
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxGammaConverterForm_Load(object sender, EventArgs e)
		{
			if (SrcImage == null)
				throw new CxException(ExStatus.InvalidObject);
			if (DstImage == null)
				DstImage = new CxImage();

			numericGamma.Value = trackGamma.Value = (int)(Param.Gamma * 100);

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
		private void CxGammaConverterForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (workerExecute.IsBusy)
			{
				e.Cancel = true;
				return;
			}

			timerUpdateUI.Stop();

			if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
			{
				Param.Gamma = ((double)numericGamma.Value) / 100;
			}
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 入力画像
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxGammaConverterForm.SrcImage")]
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
		[XIE.CxDescription("P:XIEstudio.CxGammaConverterForm.DstImage")]
		public XIE.CxImage DstImage
		{
			get { return m_DstImage; }
			set { m_DstImage = value; }
		}
		private XIE.CxImage m_DstImage = null;

		/// <summary>
		/// パラメータ
		/// </summary>
		public static XIE.Effectors.CxGammaConverter Param = new XIE.Effectors.CxGammaConverter();

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
					Effector.Gamma = ((double)numericGamma.Value) / 100;
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
		private XIE.Effectors.CxGammaConverter Effector = new XIE.Effectors.CxGammaConverter();

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

		#region Gamma:

		/// <summary>
		/// Gamma: フォーカスが移ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericGamma_Enter(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			ctrl.Select(0, ctrl.Value.ToString().Length);
		}

		/// <summary>
		/// Gamma: テキストが変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericGamma_TextChanged(object sender, EventArgs e)
		{
			numericGamma_ValueChanged(sender, e);
		}

		/// <summary>
		/// Gamma: 値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericGamma_ValueChanged(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			if (ctrl.Focused)
			{
				if (ctrl.Minimum <= ctrl.Value && ctrl.Value <= ctrl.Maximum)
				{
					trackGamma.Value = (int)ctrl.Value;
					Updated = true;
				}
			}
		}

		/// <summary>
		/// Gamma: スライダーが動かされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackGamma_Scroll(object sender, EventArgs e)
		{
			var ctrl = (TrackBar)sender;
			if (ctrl.Focused)
			{
				numericGamma.Value = ctrl.Value;
				Updated = true;
			}
		}

		#endregion
	}
}
