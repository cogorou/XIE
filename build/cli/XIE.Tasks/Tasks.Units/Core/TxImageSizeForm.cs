/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XIE.Tasks
{
	/// <summary>
	/// 画像サイズ情報入力フォーム
	/// </summary>
	partial class TxImageSizeForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public TxImageSizeForm()
		{
			InitializeComponent();
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 画像サイズ
		/// </summary>
		public TxImageSize ImageSize
		{
			get { return m_ImageSize; }
			set { m_ImageSize = value; }
		}
		private TxImageSize m_ImageSize = new TxImageSize();

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TxImageSizeForm_Load(object sender, EventArgs e)
		{
			numericWidth.Minimum = 0;
			numericWidth.Maximum = int.MaxValue;
			numericWidth.Value = ImageSize.Width;
			numericHeight.Minimum = 0;
			numericHeight.Maximum = int.MaxValue;
			numericHeight.Value = ImageSize.Height;
			numericPack.Minimum = 0;
			numericPack.Maximum = XIE.Defs.XIE_IMAGE_CHANNELS_MAX;
			numericPack.Value = ImageSize.Model.Pack;
			numericChannels.Minimum = 0;
			numericChannels.Maximum = XIE.Defs.XIE_IMAGE_CHANNELS_MAX;
			numericChannels.Value = ImageSize.Channels;
			numericDepth.Minimum = 0;
			numericDepth.Maximum = 64;
			numericDepth.Value = ImageSize.Depth;

			foreach(var type in Types)
				comboType.Items.Add(type.ToString());
			comboType.SelectedIndex = Array.FindIndex(Types, (item) => { return item == ImageSize.Model.Type; });
		}

		/// <summary>
		/// 要素モデルの型
		/// </summary>
		private ExType[] Types = 
		{
			XIE.ExType.None,
			XIE.ExType.U8,
			XIE.ExType.U16,
			XIE.ExType.U32,
			XIE.ExType.U64,
			XIE.ExType.S8,
			XIE.ExType.S16,
			XIE.ExType.S32,
			XIE.ExType.S64,
			XIE.ExType.F32,
			XIE.ExType.F64,
			XIE.ExType.Ptr,
		};

		#endregion

		#region OK/Cancel:

		/// <summary>
		/// OK ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Cancel ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion

		#region コントロールイベント:

		/// <summary>
		/// numericUpDown にフォーカスを得たとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericUpDown_Enter(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			ctrl.Select(0, ctrl.Value.ToString().Length);
		}

		/// <summary>
		/// numericUpDown の値が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericUpDown_ValueChanged(object sender, EventArgs e)
		{
			var ctrl = (NumericUpDown)sender;
			if (ReferenceEquals(sender, numericWidth))
			{
				m_ImageSize.Width = (int)ctrl.Value;
				return;
			}
			if (ReferenceEquals(sender, numericHeight))
			{
				m_ImageSize.Height = (int)ctrl.Value;
				return;
			}
			if (ReferenceEquals(sender, numericPack))
			{
				var model = m_ImageSize.Model;
				model.Pack = (int)ctrl.Value;
				m_ImageSize.Model = model;
				return;
			}
			if (ReferenceEquals(sender, numericChannels))
			{
				m_ImageSize.Channels = (int)ctrl.Value;
				return;
			}
			if (ReferenceEquals(sender, numericDepth))
			{
				m_ImageSize.Depth = (int)ctrl.Value;
				return;
			}
		}

		/// <summary>
		/// 要素モデルの型が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboType_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = comboType.SelectedIndex;
			if (0 <= index && index < Types.Length)
			{
				var model = m_ImageSize.Model;
				model.Type = Types[index];
				m_ImageSize.Model = model;
				return;
			}
		}

		#endregion
	}
}
