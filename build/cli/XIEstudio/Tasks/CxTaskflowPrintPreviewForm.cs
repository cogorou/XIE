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
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Threading;
using System.Linq;
using System.Diagnostics;

namespace XIEstudio
{
	/// <summary>
	/// タスクフロー印刷プレビューフォーム
	/// </summary>
	public partial class CxTaskflowPrintPreviewForm : Form
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskflowPrintPreviewForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="node">タスクフローノード</param>
		public CxTaskflowPrintPreviewForm(CxTaskNode node)
		{
			InitializeComponent();
			this.TaskNode = node;
		}

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowPrintPreviewForm_Load(object sender, EventArgs e)
		{
			this.printPreview.Document = this.TaskNode.Document;

			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowPrintPreviewForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();
		}

		/// <summary>
		/// タスクフローノード
		/// </summary>
		public CxTaskNode TaskNode
		{
			get { return m_TaskNode; }
			set { m_TaskNode = value; }
		}
		private CxTaskNode m_TaskNode = null;

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			PageNo_Update();
		}

		/// <summary>
		/// 印刷
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPrint_Click(object sender, EventArgs e)
		{
			#region 印刷:
			try
			{
				this.printDlg.Document = this.TaskNode.Document;
				if (this.printDlg.ShowDialog(this) == DialogResult.OK)
				{
					this.TaskNode.Document.Print();
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion
		}

		/// <summary>
		/// ページ設定
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPageSetup_Click(object sender, EventArgs e)
		{
			#region ページ設定:
			try
			{
				this.pageSetup.Document = this.TaskNode.Document;
				if (this.pageSetup.ShowDialog(this) == DialogResult.OK)
				{
					this.printPreview.Document = this.TaskNode.Document;
					this.printPreview.Refresh();
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion
		}

		/// <summary>
		/// ページ番号の表示更新
		/// </summary>
		private void PageNo_Update()
		{
			int page_no = this.printPreview.StartPage;
			int page_num = this.TaskNode.Document_Taskflows.Count;
			toolPageNoPrev.Enabled = (page_no > 0);
			toolPageNoNext.Enabled = (page_no < page_num - 1);
			this.toolPageNo.Text = string.Format("{0}/{1}", page_no + 1, page_num);
		}

		/// <summary>
		/// ページ番号: 前へ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPageNoPrev_Click(object sender, EventArgs e)
		{
			int page_no = this.printPreview.StartPage - 1;
			if (page_no >= 0)
			{
				this.printPreview.StartPage = page_no;
				PageNo_Update();
			}
		}

		/// <summary>
		/// ページ番号: 次へ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPageNoNext_Click(object sender, EventArgs e)
		{
			int page_no = this.printPreview.StartPage + 1;
			if (page_no < this.TaskNode.Document_Taskflows.Count)
			{
				this.printPreview.StartPage = page_no;
				PageNo_Update();
			}
		}
	}
}
