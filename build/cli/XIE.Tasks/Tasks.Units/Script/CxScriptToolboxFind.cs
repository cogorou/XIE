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
	/// キーワード検索ツールボックス
	/// </summary>
	partial class CxScriptToolboxFind : Form
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="editor">エディタコントロール</param>
		/// <param name="keyword">キーワード</param>
		/// <param name="match_case">大小区別</param>
		public CxScriptToolboxFind(Sgry.Azuki.WinForms.AzukiControl editor, string keyword, bool match_case)
		{
			InitializeComponent();
			this.Editor = editor;
			this.Keyword = keyword;
			this.MatchCase = match_case;
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
		/// キーワード
		/// </summary>
		public string Keyword
		{
			get { return this.textKeyword.Text; }
			set { this.textKeyword.Text = value; }
		}

		/// <summary>
		/// 大小区別
		/// </summary>
		public bool MatchCase
		{
			get { return this.checkCase.Checked; }
			set { this.checkCase.Checked = value; }
		}

		/// <summary>
		/// 検索 (次)
		/// </summary>
		public void FindNext()
		{
			buttonNext_Click(this, EventArgs.Empty);
		}

		/// <summary>
		/// 検索 (前)
		/// </summary>
		public void FindPrev()
		{
			buttonPrev_Click(this, EventArgs.Empty);
		}

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EditorToolboxFind_Load(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// キーバインド
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EditorToolboxFind_KeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Alt && !e.Shift && !e.Control && e.KeyCode == Keys.Escape)
			{
				this.Close();
				return;
			}
			if (!e.Alt && !e.Shift && !e.Control && e.KeyCode == Keys.F3)
			{
				buttonNext_Click(sender, e);
				return;
			}
			if (!e.Alt && e.Shift && !e.Control && e.KeyCode == Keys.F3)
			{
				buttonPrev_Click(sender, e);
				return;
			}
		}

		/// <summary>
		/// テキストボックスのキー入力監視
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textKeyword_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (!e.Alt && !e.Shift && !e.Control && e.KeyCode == Keys.Enter)
			{
				buttonNext_Click(sender, e);
				return;
			}
		}

		/// <summary>
		/// フォーカスを受けたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textKeyword_Enter(object sender, EventArgs e)
		{
			this.textKeyword.SelectAll();
		}

		/// <summary>
		/// 検索 (次)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonNext_Click(object sender, EventArgs e)
		{
			Sgry.Azuki.Document doc = this.Editor.Document;
			int startIndex = System.Math.Max(doc.CaretIndex, doc.AnchorIndex);

			Sgry.Azuki.SearchResult result = doc.FindNext(this.textKeyword.Text, startIndex, doc.Length, this.checkCase.Checked);
			if (result != null)
			{
				this.Editor.Document.SetSelection(result.Begin, result.End);
				this.Editor.View.SetDesiredColumn();
				this.Editor.ScrollToCaret();
			}
		}

		/// <summary>
		/// 検索 (前)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonPrev_Click(object sender, EventArgs e)
		{
			Sgry.Azuki.Document doc = this.Editor.Document;
			int startIndex = System.Math.Min(doc.CaretIndex, doc.AnchorIndex);

			Sgry.Azuki.SearchResult result = doc.FindPrev(this.textKeyword.Text, 0, startIndex, this.checkCase.Checked);
			if (result != null)
			{
				this.Editor.Document.SetSelection(result.End, result.Begin);
				this.Editor.View.SetDesiredColumn();
				this.Editor.ScrollToCaret();
			}
		}
	}
}
