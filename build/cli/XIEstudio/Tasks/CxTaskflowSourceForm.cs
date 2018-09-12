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

namespace XIEstudio
{
	/// <summary>
	/// タスクフローソースコード表示フォーム
	/// </summary>
	public partial class CxTaskflowSourceForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTaskflowSourceForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="task_node">タスクノード</param>
		/// <param name="language_type">言語種別</param>
		public CxTaskflowSourceForm(CxTaskNode task_node, XIE.Tasks.ExLanguageType language_type)
		{
			InitializeComponent();
			this.TaskNode = task_node;
			this.LanguageType = language_type;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// タスクノード
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxTaskflowSourceForm.TaskNode")]
		public virtual CxTaskNode TaskNode
		{
			get { return m_TaskNode; }
			set { m_TaskNode = value; }
		}
		private CxTaskNode m_TaskNode = new CxTaskNode();

		/// <summary>
		/// 言語種別
		/// </summary>
		[XIE.CxCategory("Parameters")]
		[XIE.CxDescription("P:XIEstudio.CxTaskflowSourceForm.LanguageType")]
		public virtual XIE.Tasks.ExLanguageType LanguageType
		{
			get { return m_LanguageType; }
			set { m_LanguageType = value; }
		}
		private XIE.Tasks.ExLanguageType m_LanguageType = XIE.Tasks.ExLanguageType.CSharp;

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowSourceForm_Load(object sender, EventArgs e)
		{
			#region フォーム初期化.
			switch (this.LanguageType)
			{
				case XIE.Tasks.ExLanguageType.CSharp:
					{
						this.Text = this.TaskNode.Name + " (C#)";
						this.Icon = Icon.FromHandle(((Bitmap)this.imageList16.Images["CSharp"]).GetHicon());
					}
					break;
				case XIE.Tasks.ExLanguageType.VisualBasic:
					{
						this.Text = this.TaskNode.Name + " (Visual Basic)";
						this.Icon = Icon.FromHandle(((Bitmap)this.imageList16.Images["VisualBasic"]).GetHicon());
					}
					break;
			}
			#endregion

			#region コントロール初期化.
#if LINUX
			this.Editor = new RichTextBox();
			SetupSourceEditor(this.Editor, this.Statement);
#else
			this.Editor = new AzukiControl();
#endif

			splitView.Panel1.Controls.Add(this.Editor);	// エディタ.
			#endregion

			#region ビルド.
			if (this.Build())
			{
				splitView.Panel2Collapsed = true;			// コンパイル結果表示用.
			}
			else
			{
				splitView.Panel2Collapsed = false;			// コンパイル結果表示用.
			}
			SetupSourceEditor(this.Editor, this.Statement);
			#endregion

			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxTaskflowSourceForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();

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

				switch (this.LanguageType)
				{
					case XIE.Tasks.ExLanguageType.CSharp:
						{
							azuki.Highlighter = Highlighters.CSharp;
							azuki.AutoIndentHook = Sgry.Azuki.FileType.CSharpFileType.AutoIndentHook;
						}
						break;
					case XIE.Tasks.ExLanguageType.VisualBasic:
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
		}

		#region コントロールイベント: (キーバインド)

		/// <summary>
		/// キーバインド
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EditorForm_KeyDown(object sender, KeyEventArgs e)
		{
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
				if (this.CxTaskflowSourceToolboxFind != null)
				{
					this.CxTaskflowSourceToolboxFind.FindNext();
					return;
				}
			}
			if (!e.Alt && e.Shift && !e.Control && e.KeyCode == Keys.F3)
			{
				if (this.CxTaskflowSourceToolboxFind != null)
				{
					this.CxTaskflowSourceToolboxFind.FindPrev();
					return;
				}
			}
			if (!e.Alt && !e.Shift && !e.Control && e.KeyCode == Keys.Escape)
			{
				if (this.CxTaskflowSourceToolboxFind != null)
				{
					this.CxTaskflowSourceToolboxFind.Close();
					return;
				}
			}
		}

		#endregion

		#region コントロールイベント: (ファイル)

		/// <summary>
		/// 保存ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFileSave_Click(object sender, EventArgs e)
		{
			#region SaveFileDialog
			var dlg = new SaveFileDialog();
			{
				var node = this.TaskNode;

				string name;
				string dir = null;
				if (string.IsNullOrEmpty(node.Info.FileName))
				{
					name = ApiHelper.MakeValidFileName(node.Name, "_");
				}
				else
				{
					name = System.IO.Path.GetFileNameWithoutExtension(node.Info.FileName);
					dir = System.IO.Path.GetDirectoryName(node.Info.FileName);
				}

				string filename;
				switch (this.LanguageType)
				{
					default:
					case XIE.Tasks.ExLanguageType.CSharp:
						{
							filename = string.Format("{0}.cs", name);
							dlg.Filter = "CSharp files |*.cs";
						}
						break;
					case XIE.Tasks.ExLanguageType.VisualBasic:
						{
							filename = string.Format("{0}.vb", name);
							dlg.Filter = "VisualBasic files |*.vb";
						}
						break;
				}

				dlg.OverwritePrompt = true;
				dlg.AddExtension = true;
				dlg.FileName = filename;
				if (dir != null)
					dlg.InitialDirectory = dir;
				if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
					dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);
			}
			#endregion

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				try
				{
					using (StreamWriter writer = new StreamWriter(dlg.FileName))
					{
						writer.Write(this.Editor.Text);
						writer.Close();
					}
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
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
			if (this.CxTaskflowSourceToolboxFind == null)
			{
				int st = 0;
				int ed = 0;
				this.Editor.Document.GetSelection(out st, out ed);
				string text = this.Editor.Document.GetTextInRange(st, ed);
				if (text.Length > 0 && text.IndexOf('\n') < 0)
					Find_Keyword = text;
				else if (text.IndexOf('\n') >= 0)
					Find_Keyword = "";

				CxTaskflowSourceToolboxFind form = new CxTaskflowSourceToolboxFind(this.Editor, Find_Keyword, Find_MatchCase);
				this.CxTaskflowSourceToolboxFind = form;

				form.Owner = this;
				form.StartPosition = FormStartPosition.Manual;
				form.Location = this.splitView.Panel1.PointToScreen(new Point(this.Editor.Location.X + 8, this.Editor.Location.Y + 8));
				form.FormClosed += new FormClosedEventHandler(
						delegate(object _sender, FormClosedEventArgs _e)
						{
							Find_Keyword = form.Keyword;
							Find_MatchCase = form.MatchCase;
							this.CxTaskflowSourceToolboxFind = null;
						}
					);
				form.Show();
			}
			else
			{
				this.CxTaskflowSourceToolboxFind.Close();
			}
#endif
		}
		private CxTaskflowSourceToolboxFind CxTaskflowSourceToolboxFind = null;
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
			CxTaskflowSourceToolboxGoto form = new CxTaskflowSourceToolboxGoto(this.Editor);
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

		// 
		// コンパイラ 関連
		// 

		#region ビルド:

		/// <summary>
		/// 構文
		/// </summary>
		private string Statement = "";

		/// <summary>
		/// ビルド
		/// </summary>
		/// <returns>
		///		コンパイルが正常に完了した場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool Build()
		{
			statusMessage.Text = "";
			richOutput.Clear();
			Statement = "";

			try
			{
				#region ソースコードへの変換:
				var cc_unit = this.TaskNode.GenerateCode(this.LanguageType, CxAuxInfoForm.AppSettings.GenerateEntryPoint);
				var provider = this.TaskNode.GetProvider(this.LanguageType);
				var line_offset = 0;

				using (var stream = new MemoryStream())
				using (var writer = new StreamWriter(stream, Encoding.UTF8))
				{
					#region コード生成:
					{
						// コード生成オプション: (共通)
						var cg_opt = new CodeGeneratorOptions();
						cg_opt.ElseOnClosing = true;
						cg_opt.BlankLinesBetweenMembers = true;
						cg_opt.BracingStyle = "C";
						cg_opt.IndentString = "	";	// (!) これは Tab です.

						provider.GenerateCodeFromCompileUnit(cc_unit, writer, cg_opt);
					}
					#endregion

					writer.Flush();

					stream.Seek(0, SeekOrigin.Begin);
					this.Statement = Encoding.UTF8.GetString(stream.ToArray());

					#region 上部のコメントの除去:
#if false
					{
						var statements = Statement.Split('\n');
						var sb = new StringBuilder();
						sb.Append("" + "\n");
						bool ns_found = false;
						foreach (var item in statements)
						{
							if (!ns_found)
							{
								if (item.StartsWith("namespace", StringComparison.CurrentCultureIgnoreCase) == false)
								{
									continue;
								}
								ns_found = true;
							}
							sb.Append(item + "\n");
						}
						Statement = sb.ToString();
						sb.Length = 0;	// Release
					}
#endif
					#endregion
				}
				#endregion

				/*
				 * C# の場合はユニット(cc_unit)を直接コンパイルするとエラー行番号が一致しない問題がある。
				 * 原因は上記の "ソースコードへの変換" で BracingStyle="C" としている為。
				 * BracingStyle="C" の場合はブレイス "{" を改行することを意味する。
				 * ユニットをコンパイルした場合は BracingStyle="Block" 相当になっており、
				 * ブレイスは改行されない為、行数が一致しない。
				 */
				CompilerResults results;
				switch (this.LanguageType)
				{
					default:
					case XIE.Tasks.ExLanguageType.CSharp:
						results = this.TaskNode.Compile(this.LanguageType, this.Statement);
						break;
					case XIE.Tasks.ExLanguageType.VisualBasic:
						results = this.TaskNode.Compile(this.LanguageType, cc_unit);
						break;
				}
				if (results == null)
				{
					// 閉じる.
					splitView.Panel2Collapsed = true;
					statusMessage.Text = "Unknown";
					return false;
				}
				else if (results.Errors.Count > 0)
				{
					// 開ける.
					splitView.Panel2Collapsed = false;
					statusMessage.Text = "Failed";

					foreach (CompilerError item in results.Errors)
					{
						int line_no = item.Line + line_offset;
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
					return false;
				}
				else
				{
					// 閉じる.
					splitView.Panel2Collapsed = true;
					statusMessage.Text = "Success";

					Assembly asm = results.CompiledAssembly;
					Type[] types = asm.GetTypes();
					foreach (Type type in types)
					{
						if (typeof(XIE.Tasks.CxTaskUnit).IsAssignableFrom(type))
						{
							var task = asm.CreateInstance(type.FullName) as XIE.Tasks.CxTaskUnit;
							break;
						}
					}

					return true;
				}
			}
			catch (System.Exception ex)
			{
				#region 例外メッセージ.

				// 開ける.
				splitView.Panel2Collapsed = false;

				// 例外メッセージ.
				statusMessage.Text = "Exception";
				richOutput.AppendText(ex.Message);
				richOutput.SelectionStart = richOutput.Text.Length;
				richOutput.Focus();
				richOutput.ScrollToCaret();
				#endregion

				return false;
			}
		}

		#endregion
	}
}