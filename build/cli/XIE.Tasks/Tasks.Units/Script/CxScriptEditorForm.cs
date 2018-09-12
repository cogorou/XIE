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
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Sgry.Azuki;
using Sgry.Azuki.WinForms;
using Sgry.Azuki.Highlighter;

namespace XIE.Tasks
{
	/// <summary>
	/// スクリプト編集フォーム
	/// </summary>
	public partial class CxScriptEditorForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxScriptEditorForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="owner">オーナーユニット</param>
		public CxScriptEditorForm(CxScript owner)
		{
			InitializeComponent();
			this.OwnerUnit = owner;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 呼び出し元のタスクユニット
		/// </summary>
		private CxScript OwnerUnit = null;

		/// <summary>
		/// 呼び出し元のタスクユニットのバックアップ
		/// </summary>
		private CxScript BackupUnit = new CxScript();

		/// <summary>
		/// コンパイル結果
		/// </summary>
		private CompilerResults Results = null;

		/// <summary>
		/// コンパイル結果から生成したタスク
		/// </summary>
		private CxTaskUnit Task = null;

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxScriptEditorForm_Load(object sender, EventArgs e)
		{
			if (this.OwnerUnit != null)
				this.BackupUnit = (CxScript)this.OwnerUnit.Clone();

			#region アイコン設定:
			switch (this.BackupUnit.LanguageType)
			{
				case ExLanguageType.CSharp:
					this.Icon = Icon.FromHandle(((Bitmap)this.imageList16.Images["CSharp"]).GetHicon());
					break;
				case ExLanguageType.VisualBasic:
					this.Icon = Icon.FromHandle(((Bitmap)this.imageList16.Images["VisualBasic"]).GetHicon());
					break;
			}
			#endregion

			// バックアップ.
			this.Text = this.BackupUnit.Name;

			// コントロール初期化.
#if LINUX
			this.Editor = new RichTextBox();
			SetupSourceEditor(this.Editor, this.BackupUnit.Statement);
#else
			this.Editor = new AzukiControl();
			SetupSourceEditor(this.Editor, this.BackupUnit.Statement);
#endif

			splitView.Panel1.Controls.Add(this.Editor);	// エディタ.
			splitView.Panel2Collapsed = true;			// コンパイル結果表示用.
			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxScriptEditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();
			if (this.DialogResult == DialogResult.OK)
			{
				this.BackupUnit.Statement = this.Editor.Text;

				if (this.OwnerUnit != null)
					this.OwnerUnit.CopyFrom(this.BackupUnit);
			}

			splitView.Panel1.Controls.Remove(this.Editor);
			if (this.Editor != null)
				this.Editor.Dispose();
			this.Editor = null;
		}

		#endregion

		#region ソースコード: (内部関数)

#if LINUX
		/// <summary>
		/// ソースコードエディタコントロール
		/// </summary>
		private RichTextBox Editor = null;
#else
		/// <summary>
		/// ソースコードエディタコントロール
		/// </summary>
		private AzukiControl Editor = null;
#endif

		/// <summary>
		/// ソースコードエディタの初期化
		/// </summary>
		/// <param name="editor">初期化対象のコントロール</param>
		/// <param name="text">表示するテキスト</param>
		private void SetupSourceEditor(Control editor, string text)
		{
			#region Azuki 初期化.
			if (editor is AzukiControl)
			{
				AzukiControl azuki = (AzukiControl)editor;

				azuki.Dock = DockStyle.Fill;

				azuki.FontInfo.Name = "Courier New";
				azuki.FontInfo.Size = 9;
				azuki.FontInfo.Style = FontStyle.Regular;

				azuki.DrawsEolCode = false;
				azuki.DrawsFullWidthSpace = true;
				azuki.DrawsSpace = false;
				azuki.DrawsTab = true;
				azuki.DrawsEofMark = false;

				azuki.ShowsLineNumber = true;
				azuki.ShowsHRuler = true;
				azuki.ShowsHScrollBar = true;
				azuki.ShowsDirtBar = true;

				azuki.TabWidth = 4;
				azuki.LinePadding = 1;
				azuki.LeftMargin = 1;
				azuki.TopMargin = 1;
				azuki.ViewType = ViewType.Proportional;
				azuki.UsesTabForIndent = true;
				azuki.SelectionMode = TextDataType.Words;
				
				azuki.IsLineSelectMode = true;
				azuki.HighlightsCurrentLine = true;
				azuki.Document.IsReadOnly = false;

				azuki.Text = text;

				switch (this.BackupUnit.LanguageType)
				{
					case ExLanguageType.CSharp:
						{
							azuki.Highlighter = Highlighters.CSharp;
							azuki.AutoIndentHook = Sgry.Azuki.FileType.CSharpFileType.AutoIndentHook;
						}
						break;
					case ExLanguageType.VisualBasic:
						{
							azuki.Highlighter = Highlighters.VisualBasic;
							azuki.AutoIndentHook = Sgry.Azuki.FileType.VbFileType.AutoIndentHook;
						}
						break;
				}

				return;
			}
			#endregion

			#region RichTextBox 初期化.
			if (editor is RichTextBox)
			{
				RichTextBox textbox = (RichTextBox)editor;

				textbox.Dock = DockStyle.Fill;
				textbox.Font = new Font("Courier New", 9, FontStyle.Regular);
				textbox.Text = text;

				return;
			}
			#endregion
		}

		#endregion

		/// <summary>
		/// 定期的な表示更新処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			#region タイトルバー:
			this.Text = this.BackupUnit.Name;
			if (this.Results != null &&
				this.Results.TempFiles != null)
			{
				string dir = System.IO.Path.GetDirectoryName(this.Results.TempFiles.BasePath);
				string name = System.IO.Path.GetFileName(this.Results.TempFiles.BasePath);
				this.Text += string.Format(" ({0} [{1}])", name, dir);
			}
			#endregion

			#region ツールバー: (編集)
#if LINUX
			toolEditCut.Enabled = true;
			toolEditCopy.Enabled = true;
			toolEditPaste.Enabled = System.Windows.Forms.Clipboard.ContainsText();
			toolEditUndo.Enabled = this.Editor.CanUndo;
			toolEditRedo.Enabled = this.Editor.CanRedo;
#else
			toolEditCut.Enabled = this.Editor.CanCut;
			toolEditCopy.Enabled = this.Editor.CanCopy;
			toolEditPaste.Enabled = this.Editor.CanPaste;
			toolEditUndo.Enabled = this.Editor.CanUndo;
			toolEditRedo.Enabled = this.Editor.CanRedo;
#endif
			#endregion
		}

		#region コントロールイベント: (終了)

		/// <summary>
		/// OK ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Cancel ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		#endregion

		#region コントロールイベント: (キーバインド)

		/// <summary>
		/// キーバインド
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxScriptEditorForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Alt && !e.Shift && !e.Control && e.KeyCode == Keys.F7)
			{
				toolBuild_Click(sender, e);
				return;
			}
			if (!e.Alt && !e.Shift && e.Control && e.KeyCode == Keys.F)
			{
				toolFind_Click(sender, e);
				return;
			}
			if (!e.Alt && !e.Shift && e.Control && e.KeyCode == Keys.G)
			{
				toolGoto_Click(sender, e);
				return;
			}
			if (!e.Alt && !e.Shift && !e.Control && e.KeyCode == Keys.F3)
			{
				if (this.ToolboxFind != null)
				{
					this.ToolboxFind.FindNext();
					return;
				}
			}
			if (!e.Alt && e.Shift && !e.Control && e.KeyCode == Keys.F3)
			{
				if (this.ToolboxFind != null)
				{
					this.ToolboxFind.FindPrev();
					return;
				}
			}
			if (!e.Alt && !e.Shift && !e.Control && e.KeyCode == Keys.Escape)
			{
				if (this.ToolboxFind != null)
				{
					this.ToolboxFind.Close();
					return;
				}
			}
		}

		#endregion

		#region コントロールイベント: (ビルド)

		/// <summary>
		/// プロパティボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolProperty_Click(object sender, EventArgs e)
		{
			var dlg = new CxTaskPortEditForm(this.BackupUnit);
			dlg.StartPosition = FormStartPosition.Manual;
			dlg.Location = XIE.Tasks.ApiHelper.GetNeighborPosition(dlg.Size, 1);
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
			}
		}

		/// <summary>
		/// 名前空間設定ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolImports_Click(object sender, EventArgs e)
		{
			var dlg = new CxScriptImportsForm(this.BackupUnit.Imports);
			dlg.StartPosition = FormStartPosition.CenterParent;
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				this.BackupUnit.Imports = dlg.Imports.ToArray();
			}
		}

		/// <summary>
		/// ビルドボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBuild_Click(object sender, EventArgs e)
		{
			statusMessage.Text = "";
			richOutput.Clear();

			try
			{
				var references = ApiHelper.CreateReferences();
				this.Results = this.BackupUnit.Compile(this.BackupUnit.LanguageType, references, this.BackupUnit.Imports, this.Editor.Text);
				if (this.Results == null)
				{
					statusMessage.Text = "Unknown";
				}
				else if (this.Results.Errors.Count == 0)
				{
					splitView.Panel2Collapsed = true;
					statusMessage.Text = "Success";

					#region Task 更新:
					{
						Assembly asm = this.Results.CompiledAssembly;
						Type[] types = asm.GetTypes();
						foreach (Type type in types)
						{
							if (typeof(CxTaskUnit).IsAssignableFrom(type))
							{
								this.Task = asm.CreateInstance(type.FullName) as CxTaskUnit;
								break;
							}
						}
					}
					#endregion
				}
				else
				{
					#region コンパイルエラー表示.
					{
						// 開ける.
						if (splitView.Panel2Collapsed)
							splitView.Panel2Collapsed = false;

						// コンパイル結果.
						statusMessage.Text = "Failed";
						foreach (CompilerError item in this.Results.Errors)
						{
							int line_no = item.Line - this.BackupUnit.LineOffset;
							string msg = string.Format(
								"({0},{1}): {2} {3} {4}\n",
								line_no,
								item.Column,
								(item.IsWarning) ? "Warning" : "Error",
								item.ErrorNumber,
								item.ErrorText
								);
							richOutput.AppendText(msg);
						}
						richOutput.SelectionStart = richOutput.Text.Length;
						richOutput.Focus();
						richOutput.ScrollToCaret();
					}
					#endregion
				}
			}
			catch (System.Exception ex)
			{
				#region 例外メッセージ.
				// 開ける.
				if (splitView.Panel2Collapsed)
					splitView.Panel2Collapsed = false;

				// 例外メッセージ.
				statusMessage.Text = "Exception";
				richOutput.AppendText(ex.Message);
				richOutput.SelectionStart = richOutput.Text.Length;
				richOutput.Focus();
				richOutput.ScrollToCaret();
				#endregion
			}
		}

		/// <summary>
		/// Output ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolOutput_Click(object sender, EventArgs e)
		{
			splitView.Panel2Collapsed = !splitView.Panel2Collapsed;
		}

		#endregion

		#region コントロールイベント: (編集)

		/// <summary>
		/// 編集: 切り取り
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditCut_Click(object sender, EventArgs e)
		{
			this.Editor.Cut();
		}

		/// <summary>
		/// 編集: コピー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditCopy_Click(object sender, EventArgs e)
		{
			this.Editor.Copy();
		}

		/// <summary>
		/// 編集: 貼り付け
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditPaste_Click(object sender, EventArgs e)
		{
			this.Editor.Paste();
		}

		/// <summary>
		/// 編集: 削除
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditDel_Click(object sender, EventArgs e)
		{
#if LINUX
#else
			this.Editor.Delete();
#endif
		}

		/// <summary>
		/// 編集: 元に戻す
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditUndo_Click(object sender, EventArgs e)
		{
			this.Editor.Undo();
		}

		/// <summary>
		/// 編集: やり直し
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditRedo_Click(object sender, EventArgs e)
		{
			this.Editor.Redo();
		}

		#endregion

		#region コントロールイベント: (検索)

		/// <summary>
		/// Find ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFind_Click(object sender, EventArgs e)
		{
#if LINUX
#else
			if (this.ToolboxFind == null)
			{
				int st = 0;
				int ed = 0;
				this.Editor.Document.GetSelection(out st, out ed);
				string text = this.Editor.Document.GetTextInRange(st, ed);
				if (text.Length > 0 && text.IndexOf('\n') < 0)
					Find_Keyword = text;
				else if (text.IndexOf('\n') >= 0)
					Find_Keyword = "";

				var form = new CxScriptToolboxFind(this.Editor, Find_Keyword, Find_MatchCase);
				this.ToolboxFind = form;

				form.Owner = this;
				form.StartPosition = FormStartPosition.Manual;
				form.Location = this.splitView.Panel1.PointToScreen(new Point(this.Editor.Location.X + 8, this.Editor.Location.Y + 8));
				form.FormClosed += new FormClosedEventHandler(
						delegate(object _sender, FormClosedEventArgs _e)
						{
							Find_Keyword = form.Keyword;
							Find_MatchCase = form.MatchCase;
							this.ToolboxFind = null;
						}
					);
				form.Show();
			}
			else
			{
				this.ToolboxFind.Close();
			}
#endif
		}
		private CxScriptToolboxFind ToolboxFind = null;
		private static string Find_Keyword = "";
		private static bool Find_MatchCase = true;

		/// <summary>
		/// Goto ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolGoto_Click(object sender, EventArgs e)
		{
#if LINUX
#else
			var form = new CxScriptToolboxGoto(this.Editor);
			form.StartPosition = FormStartPosition.CenterParent;
			form.ShowDialog(this);
#endif
		}

		#endregion

		#region コントロールイベント: (テキストボックス)

		/// <summary>
		/// テキストボックスがダブルクリックされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void richOutput_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				int char_index = richOutput.GetCharIndexFromPosition(e.Location);
				int line_index = richOutput.GetLineFromCharIndex(char_index);

				if (line_index < richOutput.Lines.Length)
				{
					string text = richOutput.Lines[line_index];

					// 行の先頭が (xxx,ccc): に一致するか否か.
					System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^\([0-9]+,[0-9]+\)\:");
					if (regex.IsMatch(text))
					{
						int st = text.IndexOf("(");
						int ed = text.IndexOf(")");
						string[] parts = text.Substring(st + 1, (ed - st - 1)).Split(new char[] {','});

						int line_no = int.Parse(parts[0]);
						int char_no = int.Parse(parts[1]);

#if LINUX
#else
						if (0 < line_no && line_no <= this.Editor.LineCount)
						{
							int line_head = this.Editor.GetLineHeadIndex(line_no - 1);
							this.Editor.SetSelection(line_head, line_head);
							this.Editor.View.SetDesiredColumn();
							this.Editor.Focus();
							this.Editor.ScrollToCaret();
						}
#endif

						//System.Diagnostics.Debug.WriteLine(string.Format("line={0},char={1},parts={2},{3}", line_index, char_index, parts[0], parts[1]));
					}
				}
			}
			catch (System.Exception)
			{
			}
		}

		#endregion
	}
}