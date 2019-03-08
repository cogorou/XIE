/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Management;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Sgry.Azuki;
using Sgry.Azuki.WinForms;
using Sgry.Azuki.Highlighter;
using XIE;

namespace XIEprompt
{
	/// <summary>
	/// プロンプトフォーム
	/// </summary>
	public partial class CxPromptForm : Form
	{
		#region 共有データ:

		/// <summary>
		/// アプリケーション設定
		/// </summary>
		public static CxAppSettings AppSettings = null;

		/// <summary>
		/// アプリケーション設定ファイルパス
		/// </summary>
		public static string AppSettingsFileName = "";

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxPromptForm()
		{
			InitializeComponent();
		}

		#endregion

		#region オブジェクト:

		/// <summary>
		/// ターゲットファイル
		/// </summary>
		public List<string> TargetFiles
		{
			get { return m_TargetFiles; }
			set { m_TargetFiles = value; }
		}
		private List<string> m_TargetFiles = new List<string>();

		/// <summary>
		/// パラメータ
		/// </summary>
		public List<string> Parameters
		{
			get { return m_Parameters; }
			set { m_Parameters = value; }
		}
		private List<string> m_Parameters = new List<string>();

		/// <summary>
		/// エントリポイント
		/// </summary>
		public string EntryPoint
		{
			get { return m_EntryPoint; }
			set { m_EntryPoint = value; }
		}
		private string m_EntryPoint = "";

		/// <summary>
		/// ログファイル
		/// </summary>
		public string LogFile
		{
			get { return m_LogFile; }
			set { m_LogFile = value; }
		}
		private string m_LogFile = "";

		/// <summary>
		/// ログファイル出力用
		/// </summary>
		private StreamWriter LogFileWriter = null;

		/// <summary>
		/// エディタコントロールのコレクション
		/// </summary>
		private List<Control> Editors = new List<Control>();

		/// <summary>
		/// 言語種別
		/// </summary>
		private XIE.Tasks.ExLanguageType LanguageType = XIE.Tasks.ExLanguageType.None;

		/// <summary>
		/// コンパイル結果
		/// </summary>
		private CompilerResults Results = null;

		/// <summary>
		/// ビルドの状態 [0:正常、1:警告あり、2:エラーあり]
		/// </summary>
		private int BuildStatus = 0;

		/// <summary>
		/// ビルド後に編集されたか否か
		/// </summary>
		private bool ModifiedAfterBuild = false;

		/// <summary>
		/// 保存後に編集されたか否か
		/// </summary>
		private bool ModifiedAfterSave = false;

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxPromptForm_Load(object sender, EventArgs e)
		{
			Prompt_Setup();

			splitView.Panel2Collapsed = true;
			timerUpdateUI.Start();

			if (!string.IsNullOrWhiteSpace(this.LogFile))
				this.LogFileWriter = new StreamWriter(this.LogFile);

			Editor_TearDown();
			Editor_Setup();

			toolBuild_Click(sender, e);
			toolTaskStart_Click(sender, e);
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxPromptForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.TaskThread != null && this.TaskThread.IsAlive)
			{
				this.TaskThread.Abort();
				e.Cancel = true;
				return;
			}

			if (this.TaskThread != null)
				this.TaskThread.Join();
			this.TaskThread = null;

			timerUpdateUI.Stop();
			Editor_TearDown();
			Prompt_TearDown();

			if (this.LogFileWriter != null)
				this.LogFileWriter.Close();
			this.LogFileWriter = null;
		}

		#endregion

		#region Prompt: (初期化と解放)

		private System.IO.TextWriter StandardOutput = null;
		private System.IO.StringWriter SpecialOutputStream = null;
		private StringBuilder SpecialOutputString = null;

		//private System.IO.TextReader StandardInput = null;
		//private System.IO.StreamReader SpecialInputStream = null;
		//private System.IO.MemoryStream SpecialInputString = null;

		/// <summary>
		/// Prompt: 初期化
		/// </summary>
		private void Prompt_Setup()
		{
			#region コンソール出力設定:
			if (SpecialOutputString == null &&
				SpecialOutputStream == null)
			{
				StandardOutput = System.Console.Out;
				SpecialOutputString = new StringBuilder();
				SpecialOutputStream = new System.IO.StringWriter(SpecialOutputString);
				System.Console.SetOut(SpecialOutputStream);
			}
			#endregion

			#region コンソール入力設定:
			//if (SpecialInputString == null &&
			//	SpecialInputStream == null)
			//{
			//	StandardInput = System.Console.In;
			//	SpecialInputString = new System.IO.MemoryStream();
			//	SpecialInputStream = new System.IO.StreamReader(SpecialInputString);
			//	System.Console.SetIn(SpecialInputStream);
			//	SpecialInputString.Seek(0, SeekOrigin.Begin);
			//}
			#endregion
		}

		/// <summary>
		/// Prompt: 解放
		/// </summary>
		private void Prompt_TearDown()
		{
			#region コンソール出力設定: (復元)
			{
				System.Console.SetOut(StandardOutput);
				if (SpecialOutputStream != null)
					SpecialOutputStream.Dispose();
				SpecialOutputStream = null;
				if (SpecialOutputString != null)
					SpecialOutputString.Clear();
				SpecialOutputString = null;
			}
			#endregion

			#region コンソール入力設定: (復元)
			//{
			//	System.Console.SetIn(StandardInput);
			//	if (SpecialInputStream != null)
			//		SpecialInputStream.Dispose();
			//	SpecialInputStream = null;
			//	if (SpecialInputString != null)
			//		SpecialInputString.Dispose();
			//	SpecialInputString = null;
			//}
			#endregion
		}

		#endregion

		#region Editor: (初期化と解放)

		/// <summary>
		/// Editor: 初期化
		/// </summary>
		private void Editor_Setup()
		{
			this.LanguageType = XIE.Tasks.ExLanguageType.None;

			foreach (var target in this.TargetFiles)
			{
				var filename = System.IO.Path.GetFileName(target);
				var page = new TabPage(string.Format("{0}", filename));

				switch (System.IO.Path.GetExtension(target))
				{
					default:
					case ".cs":
						this.LanguageType = XIE.Tasks.ExLanguageType.CSharp;
						break;
					case ".vb":
						this.LanguageType = XIE.Tasks.ExLanguageType.VisualBasic;
						break;
				}

#if LINUX
				var editor = new RichTextBox();
#else
				var editor = new AzukiControl();
#endif
				using (StreamReader reader = new StreamReader(target))
				{
					string statement = reader.ReadToEnd();
					SetupEditor(editor, statement);
				}

#if LINUX
#else
				editor.ClearHistory();
#endif
				editor.TextChanged += Editor_TextChanged;

				this.Editors.Add(editor);
				page.Controls.Add(editor);
				tabSource.TabPages.Add(page);
			}
		}

		/// <summary>
		/// Editor: 解放
		/// </summary>
		private void Editor_TearDown()
		{
			this.Results = null;
			richOutput.Clear();
			tabSource.TabPages.Clear();

			foreach (var editor in this.Editors)
				editor.Dispose();
			this.Editors.Clear();
		}

		/// <summary>
		/// Editor: テキストが変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Editor_TextChanged(object sender, EventArgs e)
		{
			this.ModifiedAfterBuild = true;
			this.ModifiedAfterSave = true;
		}

		/// <summary>
		/// ソースコードエディタの初期化
		/// </summary>
		/// <param name="editor">初期化対象のコントロール</param>
		/// <param name="text">表示するテキスト</param>
		private void SetupEditor(Control editor, string text)
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
					default:
					case XIE.Tasks.ExLanguageType.CSharp:
						azuki.Highlighter = Highlighters.CSharp;
						azuki.AutoIndentHook = Sgry.Azuki.FileType.CSharpFileType.AutoIndentHook;
						break;
					case XIE.Tasks.ExLanguageType.VisualBasic:
						azuki.Highlighter = Highlighters.VisualBasic;
						azuki.AutoIndentHook = Sgry.Azuki.FileType.VbFileType.AutoIndentHook;
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
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			bool is_empty = (this.TargetFiles.Count == 0);
			bool is_valid = (this.Results != null && this.BuildStatus <= 1);
			bool is_busy = (this.TaskThread != null && this.TaskThread.IsAlive);
			int editor_index = tabSource.SelectedIndex;

			#region ステータスバー:
			if (is_valid)
				statusMessage.Text = is_busy ? "Busy ..." : "Ready";
			#endregion

			#region ツールバー:
			toolFileSave.Enabled = ModifiedAfterSave;
			toolBuild.Enabled = !is_empty;
			toolTaskStart.Enabled = is_valid && !is_busy;
			toolTaskStop.Enabled = is_valid && is_busy;
			#endregion

			#region ツールバー: (エディタ)
			if (0 <= editor_index && editor_index < this.Editors.Count)
			{
				if (this.Editors[editor_index] is AzukiControl)
				{
					var editor = (AzukiControl)this.Editors[editor_index];
					toolEditCut.Enabled = editor.CanCut;
					toolEditCopy.Enabled = editor.CanCopy;
					toolEditPaste.Enabled = editor.CanPaste;
					toolEditDel.Enabled = editor.CanCut;
					toolEditUndo.Enabled = editor.CanUndo;
					toolEditRedo.Enabled = editor.CanRedo;
					toolFind.Enabled = true;
					toolGoto.Enabled = true;
				}
				else if (this.Editors[editor_index] is RichTextBox)
				{
					var editor = (RichTextBox)this.Editors[editor_index];
					toolEditCut.Enabled = true;
					toolEditCopy.Enabled = true;
					toolEditPaste.Enabled = System.Windows.Forms.Clipboard.ContainsText();
					toolEditDel.Enabled = false;
					toolEditUndo.Enabled = editor.CanUndo;
					toolEditRedo.Enabled = editor.CanRedo;
					toolFind.Enabled = true;
					toolGoto.Enabled = true;
				}
			}
			else
			{
				toolEditCut.Enabled = false;
				toolEditCopy.Enabled = false;
				toolEditPaste.Enabled = false;
				toolEditDel.Enabled = false;
				toolEditUndo.Enabled = false;
				toolEditRedo.Enabled = false;
				toolFind.Enabled = false;
				toolGoto.Enabled = false;
			}
			#endregion

			#region コンソール出力:
			if (SpecialOutputString != null)
			{
				lock (SpecialOutputString)
				{
					try
					{
						this.textPrompt_IgnoreChanged = true;

						string text = SpecialOutputString.ToString();
						if (string.IsNullOrEmpty(text) == false)
						{
							SpecialOutputString.Clear();

							long sum_length = textPrompt.TextLength;
							sum_length += text.Length;
							if (sum_length >= (long)textPrompt.MaxLength)
								textPrompt.Clear();

							// 追加して最後の行までスクロールする.
							textPrompt.AppendText(text);
							textPrompt.SelectionStart = textPrompt.Text.Length;
							textPrompt.Focus();
							textPrompt.ScrollToCaret();

							if (this.LogFileWriter != null)
							{
								this.LogFileWriter.Write(text);
							}
						}
					}
					catch (System.Exception ex)
					{
						XIE.Log.Api.Error(ex.Message);
					}
					finally
					{
						this.textPrompt_IgnoreChanged = false;
					}
				}
			}
			#endregion
		}

		#region コントロールイベント: (ドラッグ＆ドロップ)

		/// <summary>
		/// ドラッグされた項目が Form 内に入ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxPromptForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
				return;
			}
		}

		/// <summary>
		/// ドラッグされた項目が Form 上にドロップされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxPromptForm_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				#region 読み込み処理:
				try
				{
					var drops = (string[])e.Data.GetData(DataFormats.FileDrop, false);

					Editor_TearDown();

					this.TargetFiles.Clear();
					foreach (var filename in drops)
					{
						if (filename.EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase) ||
							filename.EndsWith(".vb", StringComparison.InvariantCultureIgnoreCase))
						{
							this.TargetFiles.Add(filename);
						}
					}

					Editor_Setup();
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				#endregion
			}
		}

		#endregion

		#region コントロールイベント: (キーバインド)

		/// <summary>
		/// キーバインド
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxPromptForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Alt && !e.Shift && e.Control && e.KeyCode == Keys.S)
			{
				if (toolFileSave.Enabled)
					toolFileSave_Click(sender, e);
				return;
			}
			if (!e.Alt && !e.Shift && !e.Control && e.KeyCode == Keys.F7)
			{
				if (toolBuild.Enabled)
					toolBuild_Click(sender, e);
				return;
			}
			if (!e.Alt && !e.Shift && !e.Control && e.KeyCode == Keys.F5)
			{
				if (toolTaskStart.Enabled)
					toolTaskStart_Click(sender, e);
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

		#region コントロールイベント: (ログ)

		/// <summary>
		/// ログ表示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolLogForm_Click(object sender, EventArgs e)
		{
			if (this.LogForm == null)
			{
				this.LogForm = new XIE.Log.CxLogForm();
				this.LogForm.Text = "Log";
				this.LogForm.Owner = this;
				this.LogForm.StartPosition = FormStartPosition.Manual;
				this.LogForm.Location = ApiHelper.GetNeighborPosition(this.LogForm.Size);
				this.LogForm.FormClosed += delegate(object _sender, FormClosedEventArgs _e)
				{
					this.LogForm = null;
				};
				this.LogForm.Show();
			}
			else
			{
				this.LogForm.Visible = !this.LogForm.Visible;
			}
		}

		/// <summary>
		/// ログフォーム
		/// </summary>
		private XIE.Log.CxLogForm LogForm = null;

		#endregion

		#region コントロールイベント: (ファイル)

		/// <summary>
		/// ファイル読み込み
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFileOpen_Click(object sender, EventArgs e)
		{
			#region OpenFileDialog
			var dlg = new OpenFileDialog();
			{
				dlg.AddExtension = true;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = true;
				dlg.Filter = "Supported files |*.cs;*.vb;";
				dlg.Filter += "|C# files |*.cs;";
				dlg.Filter += "|VisualBasic files |*.vb;";
				dlg.Filter += "|All files |*.*";

				if (XIE.Tasks.SharedData.ProjectDir != "")
					dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);

				if (string.IsNullOrWhiteSpace(CxPromptForm.AppSettings.ScriptFileDirectory) == false)
					dlg.InitialDirectory = CxPromptForm.AppSettings.ScriptFileDirectory;
			}
			#endregion

			#region 読み込み処理:
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				try
				{
					CxPromptForm.AppSettings.ScriptFileDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);

					Editor_TearDown();

					this.TargetFiles.Clear();
					foreach (var filename in dlg.FileNames)
					{
						this.TargetFiles.Add(filename);
					}

					Editor_Setup();
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			#endregion
		}

		/// <summary>
		/// ファイル保存
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFileSave_Click(object sender, EventArgs e)
		{
			if (this.TargetFiles.Count == 0)
			{
				#region SaveFileDialog
				var dlg = new SaveFileDialog();
				{
					string suffix = ApiHelper.MakeFileNameSuffix(DateTime.Now, true);

					string filename;
					switch (this.LanguageType)
					{
						default:
						case XIE.Tasks.ExLanguageType.CSharp:
							{
								filename = string.Format("{0}.cs", suffix);
								dlg.Filter = "CSharp files |*.cs";
							}
							break;
						case XIE.Tasks.ExLanguageType.VisualBasic:
							{
								filename = string.Format("{0}.vb", suffix);
								dlg.Filter = "VisualBasic files |*.vb";
							}
							break;
					}

					dlg.OverwritePrompt = true;
					dlg.AddExtension = true;
					dlg.FileName = filename;

					if (string.IsNullOrWhiteSpace(XIE.Tasks.SharedData.ProjectDir) == false)
						dlg.CustomPlaces.Add(XIE.Tasks.SharedData.ProjectDir);

					if (string.IsNullOrWhiteSpace(CxPromptForm.AppSettings.ScriptFileDirectory) == false)
						dlg.InitialDirectory = CxPromptForm.AppSettings.ScriptFileDirectory;
				}
				#endregion

				#region ファイル名追加:
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					CxPromptForm.AppSettings.ScriptFileDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);

					foreach (var filename in dlg.FileNames)
					{
						this.TargetFiles.Add(filename);
					}
				}
				#endregion
			}

			#region 保存処理:
			try
			{
				for (int i = 0; i < this.TargetFiles.Count; i++)
				{
					string text = "";
					var editor = this.Editors[i];
					if (editor is AzukiControl)
					{
						text = ((AzukiControl)editor).Text;
					}
					else if (editor is RichTextBox)
					{
						text = ((RichTextBox)editor).Text;
					}

					string filename = this.TargetFiles[i];
					using (StreamWriter writer = new StreamWriter(System.IO.Path.GetFullPath(filename)))
					{
						writer.Write(text);
					}
				}

				ModifiedAfterSave = false;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion
		}

		#endregion

		#region コントロールイベント: (タブ)

		/// <summary>
		/// エディタのタブが切り替わったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabSource_SelectedIndexChanged(object sender, EventArgs e)
		{
			var index = tabSource.SelectedIndex;
			if (0 <= index && index < this.Editors.Count)
				this.Editors[index].Focus();
		}

		#endregion

		#region コントロールイベント: (エディタ)

		/// <summary>
		/// 名前空間設定ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolImports_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// 編集: 切り取り
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditCut_Click(object sender, EventArgs e)
		{
			int index = tabSource.SelectedIndex;
			if (!(0 <= index && index < this.Editors.Count)) return;
			if (this.Editors[index] is AzukiControl)
			{
				var editor = (AzukiControl)this.Editors[index];
				editor.Cut();
			}
			else if (this.Editors[index] is RichTextBox)
			{
				var editor = (RichTextBox)this.Editors[index];
				editor.Cut();
			}
		}

		/// <summary>
		/// 編集: コピー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditCopy_Click(object sender, EventArgs e)
		{
			int index = tabSource.SelectedIndex;
			if (!(0 <= index && index < this.Editors.Count)) return;
			if (this.Editors[index] is AzukiControl)
			{
				var editor = (AzukiControl)this.Editors[index];
				editor.Copy();
			}
			else if (this.Editors[index] is RichTextBox)
			{
				var editor = (RichTextBox)this.Editors[index];
				editor.Copy();
			}
		}

		/// <summary>
		/// 編集: 貼り付け
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditPaste_Click(object sender, EventArgs e)
		{
			int index = tabSource.SelectedIndex;
			if (!(0 <= index && index < this.Editors.Count)) return;
			if (this.Editors[index] is AzukiControl)
			{
				var editor = (AzukiControl)this.Editors[index];
				editor.Paste();
			}
			else if (this.Editors[index] is RichTextBox)
			{
				var editor = (RichTextBox)this.Editors[index];
				editor.Paste();
			}
		}

		/// <summary>
		/// 編集: 削除
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditDel_Click(object sender, EventArgs e)
		{
			int index = tabSource.SelectedIndex;
			if (!(0 <= index && index < this.Editors.Count)) return;
			if (this.Editors[index] is AzukiControl)
			{
				var editor = (AzukiControl)this.Editors[index];
				editor.Delete();
			}
			else if (this.Editors[index] is RichTextBox)
			{
				var editor = (RichTextBox)this.Editors[index];
				// unsupported
			}
		}

		/// <summary>
		/// 編集: 元に戻す
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditUndo_Click(object sender, EventArgs e)
		{
			int index = tabSource.SelectedIndex;
			if (!(0 <= index && index < this.Editors.Count)) return;
			if (this.Editors[index] is AzukiControl)
			{
				var editor = (AzukiControl)this.Editors[index];
				editor.Undo();
			}
			else if (this.Editors[index] is RichTextBox)
			{
				var editor = (RichTextBox)this.Editors[index];
				editor.Undo();
			}
		}

		/// <summary>
		/// 編集: やり直し
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolEditRedo_Click(object sender, EventArgs e)
		{
			int index = tabSource.SelectedIndex;
			if (!(0 <= index && index < this.Editors.Count)) return;
			if (this.Editors[index] is AzukiControl)
			{
				var editor = (AzukiControl)this.Editors[index];
				editor.Redo();
			}
			else if (this.Editors[index] is RichTextBox)
			{
				var editor = (RichTextBox)this.Editors[index];
				editor.Redo();
			}
		}

		/// <summary>
		/// 検索: Find ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolFind_Click(object sender, EventArgs e)
		{
#if LINUX
#else
			if (this.ToolboxFind == null)
			{
				int index = tabSource.SelectedIndex;
				if (!(0 <= index && index < this.Editors.Count)) return;
				if (this.Editors[index] is AzukiControl)
				{
					var editor = (AzukiControl)this.Editors[index];

					int st = 0;
					int ed = 0;
					editor.Document.GetSelection(out st, out ed);
					string text = editor.Document.GetTextInRange(st, ed);
					if (text.Length > 0 && text.IndexOf('\n') < 0)
						Find_Keyword = text;
					else if (text.IndexOf('\n') >= 0)
						Find_Keyword = "";

					var form = new CxPromptToolboxFind(editor, Find_Keyword, Find_MatchCase);
					this.ToolboxFind = form;

					form.Owner = this;
					form.StartPosition = FormStartPosition.Manual;
					form.Location = this.splitView.Panel1.PointToScreen(new Point(editor.Location.X + 8, editor.Location.Y + 8));
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
			}
			else
			{
				this.ToolboxFind.Close();
			}
#endif

		}
		private CxPromptToolboxFind ToolboxFind = null;
		private static string Find_Keyword = "";
		private static bool Find_MatchCase = true;

		/// <summary>
		/// 検索: Goto ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolGoto_Click(object sender, EventArgs e)
		{
#if LINUX
#else
			int index = tabSource.SelectedIndex;
			if (!(0 <= index && index < this.Editors.Count)) return;
			if (this.Editors[index] is AzukiControl)
			{
				var editor = (AzukiControl)this.Editors[index];
				var form = new CxPromptToolboxGoto(editor);
				form.StartPosition = FormStartPosition.CenterParent;
				form.ShowDialog(this);
			}
#endif
		}

		#endregion

		#region コントロールイベント: (ビルド)

		/// <summary>
		/// ビルドボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBuild_Click(object sender, EventArgs e)
		{
			if (this.TargetFiles.Count == 0) return;

			statusMessage.Text = "";
			richOutput.Clear();
			this.Results = null;

			try
			{
				CodeDomProvider provider = null;
				var compilerOptions = "/t:library /optimize";
				var providerOptions = new Dictionary<string, string>();
				var statements = new List<string>();
				var refasm = this.GetReferencedAssemblies(XIE.Tasks.SharedData.References.Values);

				#region ProviderOptions:
				try
				{
					// (!) var を使用したいだけなので .NET 4.0 以降では指定不要です.
					if (Environment.Version.Major == 2)
					{
						Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5", false);
						object value = regkey.GetValue("Install");
						if (value is int && ((int)value) == 1)
						{
							providerOptions["CompilerVersion"] = "v3.5";
						}
					}
				}
				catch (System.Exception)
				{
				}
				#endregion

				#region ProviderSettings:
				switch (this.LanguageType)
				{
					case XIE.Tasks.ExLanguageType.CSharp:
						provider = new Microsoft.CSharp.CSharpCodeProvider(providerOptions);
						compilerOptions += " /unsafe+";
						break;
					case XIE.Tasks.ExLanguageType.VisualBasic:
						provider = new Microsoft.VisualBasic.VBCodeProvider(providerOptions);
						break;
					default:
						throw new CxException(ExStatus.Unsupported, "The language is not supported.");
				}
				#endregion

				#region CompilerParameters:
				CompilerParameters param = new CompilerParameters();
				param.GenerateExecutable = false;
				param.GenerateInMemory = true;
				param.WarningLevel = 4;
				param.TreatWarningsAsErrors = false;
				param.IncludeDebugInformation = false;
				param.CompilerOptions = compilerOptions;
				param.ReferencedAssemblies.AddRange(refasm);
				#endregion

				#region Statements:
				foreach (var item in this.Editors)
				{
					if (item is AzukiControl)
					{
						var editor = (AzukiControl)item;
						statements.Add(editor.Text);
					}
					else if (item is RichTextBox)
					{
						var editor = (RichTextBox)item;
						statements.Add(editor.Text);
					}
				}
				if (statements.Count == 0)
				{
					Console.WriteLine("Statements is empty.");
					return;
				}
				#endregion

				// Compile
				this.Results = provider.CompileAssemblyFromSource(param, statements.ToArray());
				this.ModifiedAfterBuild = false;

				if (this.Results.Errors.HasErrors)
				{
					splitView.Panel2Collapsed = false;	// 開ける.
					this.BuildStatus = 2;
					statusMessage.Text = "Failed";
				}
				else if (this.Results.Errors.HasWarnings)
				{
					splitView.Panel2Collapsed = false;	// 開ける.
					this.BuildStatus = 1;
				}
				else
				{
					splitView.Panel2Collapsed = true;	// 閉じる.
					this.BuildStatus = 0;
				}

				#region コンパイルエラー表示.
				if (this.Results.Errors.Count > 0)
				{
					// コンパイル結果.
					foreach (CompilerError item in this.Results.Errors)
					{
						// xxxxx.##.xxx
						var file_no = 0;
						var fileparts = item.FileName.Split('.');
						if (fileparts.Length == 3)
							file_no = Convert.ToInt32(fileparts[1]);
						int line_no = item.Line;
						string msg = string.Format(
							"{0}:({1},{2}): {3} {4} {5}\n",
							file_no,
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
			catch (System.Exception ex)
			{
				// 開ける.
				splitView.Panel2Collapsed = false;

				#region 例外メッセージ:
				// 例外メッセージ.
				statusMessage.Text = "Exception";
				richOutput.AppendText(ex.Message);
				richOutput.SelectionStart = richOutput.Text.Length;
				richOutput.Focus();
				richOutput.ScrollToCaret();
				#endregion
			}
			finally
			{
			}
		}

		/// <summary>
		/// 参照設定を取得します。
		/// </summary>
		/// <param name="references">参照設定</param>
		/// <returns>
		///		参照するアセンブリ名の一覧を返します。
		/// </returns>
		private string[] GetReferencedAssemblies(IEnumerable<XIE.Tasks.CxReferencedAssembly> references)
		{
			var result = new List<string>();

			foreach (var item in references)
			{
				Assembly asm = null;

				#region 厳密名があれば優先的に使用する.
				if (string.IsNullOrWhiteSpace(item.FullName) == false)
				{
					try
					{
						asm = Assembly.Load(item.FullName);

						// フルパスで参照する形式.
						result.Add(asm.Location);
					}
					catch (System.Exception)
					{
						// 一時的に GAC 登録解除している可能性を考慮する.
					}
				}
				#endregion

				#region ファイル名でロードを試みる.
				if (asm == null &&
					string.IsNullOrWhiteSpace(item.HintPath) == false &&
					System.IO.File.Exists(item.HintPath))
				{
					try
					{
						asm = Assembly.LoadFrom(item.HintPath);

						// ファイル名で参照する形式.
						result.Add(item.HintPath);
					}
					catch (System.Exception ex)
					{
						XIE.Log.Api.Error(ex.StackTrace);
					}
				}
				#endregion
			}

			return result.ToArray();
		}

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

					// 行の先頭が fff:(lll,ccc): に一致するか否か.
					var regex = new System.Text.RegularExpressions.Regex(@"^[0-9]+\:\([0-9]+,[0-9]+\)\:");
					if (regex.IsMatch(text))
					{
						int file_no = 0;
						int line_no = 0;
						int char_no = 0;

						#region 抽出:
						{
							int cron = text.IndexOf(":");
							file_no = Convert.ToInt32(text.Substring(0, cron));

							int st = text.IndexOf("(");
							int ed = text.IndexOf(")");
							string[] parts = text.Substring(st + 1, (ed - st - 1)).Split(new char[] { ',' });

							line_no = int.Parse(parts[0]);
							char_no = int.Parse(parts[1]);
						}
						#endregion

						#region 移動:
#if LINUX
#else
						if (0 <= file_no && file_no < tabSource.TabCount)
						{
							tabSource.SelectedIndex = file_no;
							var editor = (AzukiControl)this.Editors[file_no];
							if (0 < line_no && line_no <= editor.LineCount)
							{
								int line_head = editor.GetLineHeadIndex(line_no - 1);
								editor.SetSelection(line_head, line_head);
								editor.View.SetDesiredColumn();
								editor.Focus();
								editor.ScrollToCaret();
							}
						}
#endif
						#endregion
					}
				}
			}
			catch (System.Exception)
			{
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

		#region コントロールイベント: (開始/停止)

		/// <summary>
		/// 開始
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskStart_Click(object sender, EventArgs e)
		{
			if (this.Results == null) return;
			if (this.BuildStatus > 1) return;
			if (this.TaskThread != null && this.TaskThread.IsAlive) return;

			if (this.TaskThread != null)
				this.TaskThread.Join();
			this.TaskThread = null;

			if (this.ModifiedAfterBuild)
			{
				toolBuild_Click(sender, e);
				if (this.Results == null) return;
				if (this.BuildStatus > 1) return;
			}

			// 引数.
			this.Arguments.Clear();
			// 空白やTABで区切って分割する処理。
			// 但し、二十引用符で括られた部分は区切らない。
			// ▽ 下記(csv の処理)を参考にした。
			// https://www.ipentec.com/document/csharp-read-csv-file-by-regex
			{
				var args = System.Text.RegularExpressions.Regex.Split(textArgs.Text, "\\s(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
				foreach (var arg in args)
				{
					if (string.IsNullOrEmpty(arg) == false)
					{
						var match = System.Text.RegularExpressions.Regex.Match(arg, "(?<=\").*(?=\")");
						if (match.Success)
							this.Arguments.Add(match.Value);
						else
							this.Arguments.Add(arg);
					}
				}
			}
			this.Arguments.AddRange(this.Parameters);

			// 実行.
			this.TaskThread = new System.Threading.Thread(Execute);
			this.TaskThread.Start();
		}

		/// <summary>
		/// 停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolTaskStop_Click(object sender, EventArgs e)
		{
			if (this.TaskThread != null && this.TaskThread.IsAlive)
				this.TaskThread.Abort();
		}

		#endregion

		#region 実行:

		/// <summary>
		/// 実行スレッド
		/// </summary>
		private System.Threading.Thread TaskThread = null;

		/// <summary>
		/// 引数
		/// </summary>
		private List<string> Arguments = new List<string>();

		/// <summary>
		/// 実行
		/// </summary>
		private void Execute(object args)
		{
			string prev_dir = System.IO.Directory.GetCurrentDirectory();

			try
			{
				if (this.Results.Errors.HasErrors)
				{
					#region コンパイルエラー表示.
					foreach (CompilerError item in this.Results.Errors)
					{
						string filename = this.TargetFiles[0];

						try
						{
							string[] parts = item.FileName.Split('.');
							int index = 0;
							if (parts.Length >= 3)
								index = Convert.ToInt32(parts[1]);
							if (index < this.TargetFiles.Count)
								filename = this.TargetFiles[index];
						}
						catch (System.Exception)
						{
						}

						string msg = string.Format(
							"{0}({1},{2}): {3} {4} {5}\n",
							filename,
							item.Line,
							item.Column,
							(item.IsWarning) ? "Warning" : "Error",
							item.ErrorNumber,
							item.ErrorText
							);
						Console.WriteLine(msg);
					}
					#endregion
				}
				else
				{
					#region EntryPoint
					Type entry_type = null;
					MethodInfo entry_method = null;
					Assembly asm = this.Results.CompiledAssembly;
					Type[] types = asm.GetTypes();
					foreach (Type type in types)
					{
						if (entry_method == null)
						{
							BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

							if (string.IsNullOrEmpty(this.EntryPoint))
							{
								if (type.Name == "Program")
								{
									MethodInfo[] methods = type.GetMethods(flags);
									foreach (MethodInfo method in methods)
									{
										if (method.Name == "Main")
										{
											entry_type = type;
											entry_method = method;
											break;
										}
									}
								}
							}
							else
							{
								if (this.EntryPoint.StartsWith(type.FullName))
								{
									MethodInfo[] methods = type.GetMethods(flags);
									foreach (MethodInfo method in methods)
									{
										if (this.EntryPoint == (type.FullName + "." + method.Name))
										{
											entry_type = type;
											entry_method = method;
											break;
										}
									}
								}
							}
						}
					}
					#endregion

					#region Execute.
					if (entry_method == null)
					{
						Console.WriteLine("Entry point not found.");
					}
					else
					{
						#region ディレクトリ移動:
						if (TargetFiles.Count > 0)
						{
							var filename = TargetFiles[0];
							if (string.IsNullOrWhiteSpace(filename) == false &&
								System.IO.File.Exists(filename))
							{
								var dir = System.IO.Path.GetDirectoryName(filename);
								System.IO.Directory.SetCurrentDirectory(dir);
							}
						}
						#endregion

						Binder binder = Type.DefaultBinder;
						BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
						if (entry_method.GetParameters().Length == 0)
							entry_type.InvokeMember(entry_method.Name, flags, binder, null, new object[] { });
						else
							entry_type.InvokeMember(entry_method.Name, flags, binder, null, new object[] { this.Arguments.ToArray() });
					}
					#endregion
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
				Console.WriteLine("Task was canceled.");
			}
			catch (System.Exception ex)
			{
				#region 例外メッセージ:
				if (ex.InnerException != null)
				{
					Console.WriteLine(ex.InnerException.Message);
					Console.WriteLine(ex.InnerException.StackTrace);
				}
				else
				{
					Console.WriteLine(ex.Message);
					Console.WriteLine(ex.StackTrace);
				}
				#endregion
			}
			finally
			{
				System.IO.Directory.SetCurrentDirectory(prev_dir);
			}
		}

		#endregion

		#region コントロールイベント: (プロンプト)

		/// <summary>
		/// テキストが変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textPrompt_TextChanged(object sender, EventArgs e)
		{
			if (this.textPrompt_IgnoreChanged == false)
			{
				//if (SpecialInputString != null)
				//{
				//	lock (SpecialInputString)
				//	{
				//		var text = textPrompt.Text;
				//		if (string.IsNullOrEmpty(text) == false)
				//		{
				//			var data = System.Text.Encoding.UTF8.GetBytes(text);
				//			SpecialInputString.Write(data, 0, data.Length);
				//		}
				//		SpecialInputString.Seek(0, SeekOrigin.Begin);
				//	}
				//}
			}
		}
		private bool textPrompt_IgnoreChanged = false;

		/// <summary>
		/// プロンプトのクリア
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPromptClear_Click(object sender, EventArgs e)
		{
			try
			{
				this.textPrompt_IgnoreChanged = true;

				textPrompt.Clear();
			}
			finally
			{
				this.textPrompt_IgnoreChanged = false;
			}
		}

		/// <summary>
		/// バージョン表示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolVersion_Click(object sender, EventArgs e)
		{
			try
			{
				#region アプリケーションのバージョン.
				{
					Console.WriteLine("[Application]");
					Assembly asm = Assembly.LoadFrom(Application.ExecutablePath);
					//Assembly asm = Assembly.GetExecutingAssembly();
					FileVersionInfo info = FileVersionInfo.GetVersionInfo(asm.Location);
					Console.WriteLine("{0,-20}: {1}", "FullName", asm.FullName);
					Console.WriteLine("{0,-20}: {1}", "FileName", System.IO.Path.GetFileName(info.FileName));
					Console.WriteLine("{0,-20}: {1}", "FileDescription", info.FileDescription);
					Console.WriteLine("{0,-20}: {1}", "Comments", info.Comments);
					Console.WriteLine("{0,-20}: {1}", "FileVersion", info.FileVersion);
					Console.WriteLine("{0,-20}: {1}", "ProductVersion", info.ProductVersion);
					Console.WriteLine("{0,-20}: {1}", "ProductName", info.ProductName);
					Console.WriteLine("{0,-20}: {1}", "CompanyName", info.CompanyName);
					Console.WriteLine("{0,-20}: {1}", "LegalCopyright", info.LegalCopyright);
					Console.WriteLine("{0,-20}: {1}", "LegalTrademarks", info.LegalTrademarks);
					Console.WriteLine("{0,-20}: {1}", "Language", info.Language);
					//Console.WriteLine("{0,-20}: {1}", "InternalName", info.InternalName);
					//Console.WriteLine("{0,-20}: {1}", "OriginalFilename", info.OriginalFilename);
					//Console.WriteLine("{0,-20}: {1}", "IsDebug", info.IsDebug);
					//Console.WriteLine("{0,-20}: {1}", "IsPatched", info.IsPatched);
					//Console.WriteLine("{0,-20}: {1}", "IsPreRelease", info.IsPreRelease);
					//Console.WriteLine("{0,-20}: {1}", "IsPrivateBuild", info.IsPrivateBuild);
					//Console.WriteLine("{0,-20}: {1}", "IsSpecialBuild", info.IsSpecialBuild);
					//Console.WriteLine("{0,-20}: {1}", "PrivateBuild", info.PrivateBuild);
					//Console.WriteLine("{0,-20}: {1}", "SpecialBuild", info.SpecialBuild);
					//Console.WriteLine("{0,-20}: {1}", "FileBuildPart", info.FileBuildPart);
					//Console.WriteLine("{0,-20}: {1}", "FileMajorPart", info.FileMajorPart);
					//Console.WriteLine("{0,-20}: {1}", "FileMinorPart", info.FileMinorPart);
					//Console.WriteLine("{0,-20}: {1}", "FilePrivatePart", info.FilePrivatePart);
					//Console.WriteLine("{0,-20}: {1}", "ProductBuildPart", info.ProductBuildPart);
					//Console.WriteLine("{0,-20}: {1}", "ProductMajorPart", info.ProductMajorPart);
					//Console.WriteLine("{0,-20}: {1}", "ProductMinorPart", info.ProductMinorPart);
					//Console.WriteLine("{0,-20}: {1}", "ProductPrivatePart", info.ProductPrivatePart);
				}
				#endregion

				#region O/S、CPU、バージョン情報.
				{
					Console.WriteLine("");
					Console.WriteLine("[Environment]");
					Console.WriteLine("{0,-20}: {1} ({2})", ".NET Framework", Environment.Version, (Environment.Is64BitOperatingSystem ? "x64" : "x86"));
					Console.WriteLine("{0,-20}: {1}", "OS", Environment.OSVersion);
					try
					{
						string[] keys = 
						{
							"Name",
							"Description",
							"CurrentClockSpeed",
							"MaxClockSpeed",
							"NumberOfCores",
							"NumberOfLogicalProcessors",
						};
						ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
						ManagementObjectCollection moc = searcher.Get();
						foreach (ManagementObject obj in moc)
						{
							foreach (string key in keys)
							{
								Console.WriteLine("{0,-20}: {1}", key, obj[key]);
							}
						}
					}
					catch (System.Exception)
					{
					}
				}
				#endregion
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		#endregion

	}
}
