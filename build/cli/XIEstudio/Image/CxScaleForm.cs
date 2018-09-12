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
	/// スケールフォーム
	/// </summary>
	public partial class CxScaleForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxScaleForm()
		{
			InitializeComponent();

			numericPercentage.TextChanged += numericPercentage_TextChanged;
			numericWidth.TextChanged += numericWidth_TextChanged;
			numericHeight.TextChanged += numericHeight_TextChanged;
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxScaleForm_Load(object sender, EventArgs e)
		{
			radioMode2.Checked = true;
			radioMode1.Checked = true;

			numericWidth.Value = SrcSize.Width;
			numericHeight.Value = SrcSize.Height;
			checkKeepRatio.Checked = true;
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxScaleForm_FormClosing(object sender, FormClosingEventArgs e)
		{

		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 入力サイズ
		/// </summary>
		public Size SrcSize
		{
			get { return m_SrcSize; }
			set { m_SrcSize = value; }
		}
		private Size m_SrcSize = new Size(1, 1);

		/// <summary>
		/// 出力サイズ
		/// </summary>
		public Size DstSize
		{
			get
			{
				return new Size(
					(int)System.Math.Round(numericWidth.Value),
					(int)System.Math.Round(numericHeight.Value)
					);
			}
		}

		/// <summary>
		/// 濃度補間の有無
		/// </summary>
		public bool Interpolation
		{
			get { return checkInterpolation.Checked; }
			set { checkInterpolation.Checked = value; }
		}

		#endregion

		#region radioMode:

		/// <summary>
		/// radioMode1: Percentage
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void radioMode1_CheckedChanged(object sender, EventArgs e)
		{
			numericPercentage.Enabled = radioMode1.Checked;
			if (numericPercentage.Enabled)
			{
				numericPercentage.Focus();
			}
		}

		/// <summary>
		/// radioMode2: Absolute
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void radioMode2_CheckedChanged(object sender, EventArgs e)
		{
			groupSize.Enabled = radioMode2.Checked;
			if (groupSize.Enabled)
			{
				numericWidth.Focus();
			}
		}

		#endregion

		#region numericUpDown:

		/// <summary>
		/// Percentage: フォーカスが移ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericPercentage_Enter(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			ctrl.Select(0, ctrl.Value.ToString().Length);
		}

		/// <summary>
		/// Percentage: 値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericPercentage_ValueChanged(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			if (ctrl.Focused)
			{
				if (ctrl.Minimum <= ctrl.Value && ctrl.Value <= ctrl.Maximum)
				{
					var mag = (double)ctrl.Value / 100;
					var width = (int)(SrcSize.Width * mag);
					var height = (int)(SrcSize.Height * mag);

					if (numericWidth.Minimum <= width && width <= numericWidth.Maximum)
						numericWidth.Value = width;
					if (numericHeight.Minimum <= height && height <= numericHeight.Maximum)
						numericHeight.Value = height;
				}
			}
		}

		/// <summary>
		/// Percentage: 値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericPercentage_TextChanged(object sender, EventArgs e)
		{
			numericPercentage_ValueChanged(sender, e);
		}

		/// <summary>
		/// Absolute: Width にフォーカスが移ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericWidth_Enter(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			ctrl.Select(0, ctrl.Value.ToString().Length);
		}

		/// <summary>
		/// Absolute: Width の値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericWidth_ValueChanged(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			if (ctrl.Focused)
			{
				if (ctrl.Minimum <= ctrl.Value && ctrl.Value <= ctrl.Maximum)
				{
					if (checkKeepRatio.Checked)
					{
						var height = ctrl.Value * SrcSize.Height / SrcSize.Width;
						if (numericHeight.Minimum <= height && height <= numericHeight.Maximum)
							numericHeight.Value = height;
					}
				}
			}
		}

		/// <summary>
		/// Absolute: Width の値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericWidth_TextChanged(object sender, EventArgs e)
		{
			numericWidth_ValueChanged(sender, e);
		}

		/// <summary>
		/// Absolute: Height にフォーカスが移ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericHeight_Enter(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			ctrl.Select(0, ctrl.Value.ToString().Length);
		}

		/// <summary>
		/// Absolute: Height の値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericHeight_ValueChanged(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			if (ctrl.Focused)
			{
				if (ctrl.Minimum <= ctrl.Value && ctrl.Value <= ctrl.Maximum)
				{
					if (checkKeepRatio.Checked)
					{
						var width = ctrl.Value * SrcSize.Width / SrcSize.Height;
						if (numericWidth.Minimum <= width && width <= numericWidth.Maximum)
							numericWidth.Value = width;
					}
				}
			}
		}

		/// <summary>
		/// Absolute: Height の値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericHeight_TextChanged(object sender, EventArgs e)
		{
			numericHeight_ValueChanged(sender, e);
		}

		#endregion
	}
}
