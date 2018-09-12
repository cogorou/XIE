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

namespace XIE.Tasks
{
	/// <summary>
	/// 行番号指定ツールボックス
	/// </summary>
	partial class CxScriptToolboxGoto : Form
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="editor">エディタコントロール</param>
		public CxScriptToolboxGoto(Sgry.Azuki.WinForms.AzukiControl editor)
		{
			InitializeComponent();
			this.Editor = editor;
		}

		/// <summary>
		/// エディタコントロール
		/// </summary>
		public Sgry.Azuki.WinForms.AzukiControl Editor
		{
			get { return m_Editor; }
			set { m_Editor = value; }
		}
		private Sgry.Azuki.WinForms.AzukiControl m_Editor = null;

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EditorToolboxGoto_Load(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// OK ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (this.textLineNo.Text != "")
			{
				try
				{
					int line_no = Convert.ToInt32(this.textLineNo.Text);
					if (0 < line_no && line_no <= this.Editor.LineCount)
					{
						int index = this.Editor.GetLineHeadIndex(line_no - 1);
						this.Editor.SetSelection(index, index);
						this.Editor.ScrollToCaret();
					}
				}
				catch (System.Exception)
				{
				}
			}
		}

		/// <summary>
		/// Cancel ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonCancel_Click(object sender, EventArgs e)
		{

		}
	}
}
