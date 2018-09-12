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
	/// 回転フォーム
	/// </summary>
	public partial class CxRotateForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxRotateForm()
		{
			InitializeComponent();

			numericAngle.TextChanged += numericAngle_TextChanged;
			numericAxisX.TextChanged += numericAxisX_TextChanged;
			numericAxisY.TextChanged += numericAxisY_TextChanged;
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxRotateForm_Load(object sender, EventArgs e)
		{
			radioMode2.Checked = true;
			radioMode1.Checked = true;
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxRotateForm_FormClosing(object sender, FormClosingEventArgs e)
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
		/// 回転角
		/// </summary>
		public double Angle
		{
			get
			{
				return (double)numericAngle.Value;
			}
			set
			{
				numericAngle.Value = (decimal)value;
			}
		}

		/// <summary>
		/// 回転の機軸
		/// </summary>
		public XIE.TxPointD Axis
		{
			get
			{
				return new XIE.TxPointD(
						(double)numericAxisX.Value,
						(double)numericAxisY.Value
					);
			}
			set
			{
				numericAxisX.Value = (decimal)value.X;
				numericAxisY.Value = (decimal)value.Y;
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
		/// radioMode1: Center
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void radioMode1_CheckedChanged(object sender, EventArgs e)
		{
			numericAngle.Enabled = radioMode1.Checked;
			if (numericAngle.Enabled)
			{
				numericAngle.Focus();
				numericAxisX.Value = SrcSize.Width / 2;
				numericAxisY.Value = SrcSize.Height / 2;
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
				numericAxisX.Focus();
			}
		}

		#endregion

		#region numericUpDown (Angle):

		/// <summary>
		/// Angle: フォーカスが移ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericAngle_Enter(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			ctrl.Select(0, ctrl.Value.ToString().Length);
		}

		/// <summary>
		/// Angle: 値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericAngle_ValueChanged(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			if (ctrl.Focused)
			{
				if (ctrl.Minimum <= ctrl.Value && ctrl.Value <= ctrl.Maximum)
				{
				}
			}
		}

		/// <summary>
		/// Angle: 値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericAngle_TextChanged(object sender, EventArgs e)
		{
			numericAngle_ValueChanged(sender, e);
		}

		#endregion

		#region numericUpDown (Axis):

		/// <summary>
		/// Axis: AxisX にフォーカスが移ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericAxisX_Enter(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			ctrl.Select(0, ctrl.Value.ToString().Length);
		}

		/// <summary>
		/// Axis: AxisX の値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericAxisX_ValueChanged(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			if (ctrl.Focused)
			{
				if (ctrl.Minimum <= ctrl.Value && ctrl.Value <= ctrl.Maximum)
				{
				}
			}
		}

		/// <summary>
		/// Axis: AxisX の値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericAxisX_TextChanged(object sender, EventArgs e)
		{
			numericAxisX_ValueChanged(sender, e);
		}

		/// <summary>
		/// Axis: AxisY にフォーカスが移ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericAxisY_Enter(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			ctrl.Select(0, ctrl.Value.ToString().Length);
		}

		/// <summary>
		/// Axis: AxisY の値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericAxisY_ValueChanged(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			if (ctrl.Focused)
			{
				if (ctrl.Minimum <= ctrl.Value && ctrl.Value <= ctrl.Maximum)
				{
				}
			}
		}

		/// <summary>
		/// Axis: AxisY の値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericAxisY_TextChanged(object sender, EventArgs e)
		{
			numericAxisY_ValueChanged(sender, e);
		}

		#endregion
	}
}
