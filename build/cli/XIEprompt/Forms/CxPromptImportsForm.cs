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

namespace XIEprompt
{
	/// <summary>
	/// 名前空間設定フォーム
	/// </summary>
	public partial class CxPromptImportsForm : Form
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxPromptImportsForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="imports">名前空間の初期値</param>
		public CxPromptImportsForm(IEnumerable<string> imports)
		{
			InitializeComponent();
			this.Imports.AddRange(imports);
		}

		/// <summary>
		/// 名前空間
		/// </summary>
		public virtual List<string> Imports
		{
			get { return m_Imports; }
			set { m_Imports = value; }
		}
		private List<string> m_Imports = new List<string>();

		/// <summary>
		/// フォームがロードされたときの初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxPromptImportsForm_Load(object sender, EventArgs e)
		{
			textImports.Lines = this.Imports.ToArray();
		}

		/// <summary>
		/// OK ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolOK_Click(object sender, EventArgs e)
		{
			this.Imports.Clear();
			this.Imports.AddRange(textImports.Lines);
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Cancel ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}
	}
}
