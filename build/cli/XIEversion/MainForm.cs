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
using System.Reflection;
using System.Diagnostics;
using System.Management;

namespace XIEversion
{
	/// <summary>
	/// メインフォーム
	/// </summary>
	public partial class MainForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MainForm()
		{
			InitializeComponent();
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_Load(object sender, EventArgs e)
		{
			try
			{
				// 環境変数名.
				labelProductROOT.Text = AxiEnv.ProductROOTVariable;
				labelProductBIN.Text = AxiEnv.ProductBINVariable;
				labelProductASM.Text = AxiEnv.ProductASMVariable;

				// 環境変数の値.
				textProductROOT.Text = AxiEnv.ProductROOT;
				textProductBIN.Text = AxiEnv.ProductBIN;
				textProductASM.Text = AxiEnv.ProductASM;
				textPATH.Text = AxiEnv.PATH;

				// AssemblyFoldersEx
				textAssemblyFoldersEx.Text = AxiGAC.AssemblyFoldersEx;

				// Framework Version
				textCLR20.Text = AxiGAC.GetFrameworkVersion(2, 0);
				textCLR30.Text = AxiGAC.GetFrameworkVersion(3, 0);
				textCLR35.Text = AxiGAC.GetFrameworkVersion(3, 5);
				textCLR40.Text = AxiGAC.GetFrameworkVersion(4, 0);

				string dir = System.IO.Directory.GetCurrentDirectory();

				#region ファイルバージョン (.NET)
				{
					var files = new List<string>();
					files.AddRange(System.IO.Directory.GetFiles(dir, "XIE.*.dll"));
					//files.AddRange(System.IO.Directory.GetFiles(dir, "XIE*.exe"));
					foreach (string file in files)
					{
						FileVersionInfo info = FileVersionInfo.GetVersionInfo(file);
						ListViewItem lvitem = new ListViewItem();
						lvitem.ImageKey = "Ask.Cancel";
						lvitem.Text = System.IO.Path.GetFileName(file);
						lvitem.SubItems.Add(info.FileVersion);
						lvitem.SubItems.Add(info.ProductVersion);
						lvitem.SubItems.Add(info.ProductName);
						lvitem.SubItems.Add(info.FileDescription);
						lvitem.SubItems.Add(info.Comments);
						lvitem.SubItems.Add(info.CompanyName);
						lvitem.SubItems.Add(info.LegalCopyright);
						listVersionNet.Items.Add(lvitem);

						// グローバルアセンブリキャッシュ.
						try
						{
							AssemblyName asmname = AssemblyName.GetAssemblyName(file);
							lvitem.SubItems.Add(asmname.FullName);

							Assembly asm = Assembly.ReflectionOnlyLoad(asmname.FullName);
							if (asm != null && asm.GlobalAssemblyCache)
							{
								lvitem.ImageKey = "Ask.OK";
							}
						}
						catch (System.Exception)
						{
						}
					}
				}
				#endregion

				#region ファイルバージョン (Win32 (x86))
				{
					string[] files = System.IO.Directory.GetFiles(dir, "xie_*_x86_*.dll");
					foreach (string file in files)
					{
						FileVersionInfo info = FileVersionInfo.GetVersionInfo(file);
						ListViewItem lvitem = new ListViewItem();
						lvitem.Text = System.IO.Path.GetFileName(file);
						lvitem.SubItems.Add(info.FileVersion);
						lvitem.SubItems.Add(info.ProductVersion);
						lvitem.SubItems.Add(info.ProductName);
						lvitem.SubItems.Add(info.FileDescription);
						lvitem.SubItems.Add(info.Comments);
						lvitem.SubItems.Add(info.CompanyName);
						lvitem.SubItems.Add(info.LegalCopyright);
						listVersionX86.Items.Add(lvitem);
					}
				}
				#endregion

				#region ファイルバージョン (Win32 (x64))
				{
					string[] files = System.IO.Directory.GetFiles(dir, "xie_*_x64_*.dll");
					foreach (string file in files)
					{
						FileVersionInfo info = FileVersionInfo.GetVersionInfo(file);
						ListViewItem lvitem = new ListViewItem();
						lvitem.Text = System.IO.Path.GetFileName(file);
						lvitem.SubItems.Add(info.FileVersion);
						lvitem.SubItems.Add(info.ProductVersion);
						lvitem.SubItems.Add(info.ProductName);
						lvitem.SubItems.Add(info.FileDescription);
						lvitem.SubItems.Add(info.Comments);
						lvitem.SubItems.Add(info.CompanyName);
						lvitem.SubItems.Add(info.LegalCopyright);
						listVersionX64.Items.Add(lvitem);
					}
				}
				#endregion

				#region ファイルバージョン (MSVCR)
				if (System.IO.Directory.Exists(System.Environment.SystemDirectory))
				{
					string sysdir = System.Environment.SystemDirectory;
					string[] files = 
					{
						"msvcr100.dll",
						"msvcp100.dll",
						"mfc100.dll",
						"mfc100u.dll",
						"msvcr110.dll",
						"msvcp110.dll",
						"mfc110.dll",
						"mfc110u.dll",
						"msvcr120.dll",
						"msvcp120.dll",
						"mfc120.dll",
						"mfc120u.dll",
						"vcruntime140.dll",
						"msvcp140.dll",
						"mfc140.dll",
						"mfc140u.dll",
					};
					foreach (string file in files)
					{
						string filename = System.IO.Path.Combine(sysdir, file);
						if (System.IO.File.Exists(filename))
						{
							FileVersionInfo info = FileVersionInfo.GetVersionInfo(filename);
							ListViewItem lvitem = new ListViewItem();
							lvitem.Text = System.IO.Path.GetFileName(filename);
							lvitem.SubItems.Add(info.FileVersion);
							lvitem.SubItems.Add(info.ProductVersion);
							lvitem.SubItems.Add(info.ProductName);
							lvitem.SubItems.Add(info.FileDescription);
							lvitem.SubItems.Add(info.Comments);
							lvitem.SubItems.Add(info.CompanyName);
							lvitem.SubItems.Add(info.LegalCopyright);
							listVersionMSVCR.Items.Add(lvitem);
						}
					}
				}
				#endregion

				#region O/S、CPU、バージョン情報.
				{
					textEnvironment.AppendText(string.Format("{0}: {1} ({2})", ".NET Framework", Environment.Version, (Environment.Is64BitOperatingSystem ? "x64" : "x86")) + Environment.NewLine);
					textEnvironment.AppendText(string.Format("{0}: {1}", "OS", Environment.OSVersion) + Environment.NewLine);
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
								textEnvironment.AppendText(string.Format("{0}: {1}", key, obj[key]) + Environment.NewLine);
							}
						}
					}
					catch (System.Exception ex)
					{
						textEnvironment.AppendText(ex.StackTrace + Environment.NewLine);
					}
				}
				#endregion
			}
			catch (System.Exception)
			{
			}
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (IsUpdated)
				AxiEnv.Flush();
		}
		private bool IsUpdated = false;

		#endregion

		#region コントロールイベント:

		/// <summary>
		/// PATH 環境変数の編集ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonPATH_Click(object sender, EventArgs e)
		{
			PathForm dlg = new PathForm();
			dlg.StartPosition = FormStartPosition.CenterParent;
			if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				textPATH.Text = AxiEnv.PATH;
			}
		}

		/// <summary>
		/// All Register ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonAllRegister_Click(object sender, EventArgs e)
		{
			Cursor cursor = this.Cursor;

			try
			{
				this.Cursor = Cursors.WaitCursor;
				this.IsUpdated = true;

				string dir = System.IO.Directory.GetCurrentDirectory();

				// AssemblyFoldersEx
				AxiGAC.AssemblyFoldersEx = dir;

				// RegSvr 登録.
				AxiRegSvr.Register();

				// Win32 登録.
				AxiEnv.Register();

				// 表示更新.
				textAssemblyFoldersEx.Text = AxiGAC.AssemblyFoldersEx;
				textProductROOT.Text = AxiEnv.ProductROOT;
				textProductBIN.Text = AxiEnv.ProductBIN;
				textProductASM.Text = AxiEnv.ProductASM;

				// GAC 登録.
				var pub = new System.EnterpriseServices.Internal.Publish();
				foreach (ListViewItem item in listVersionNet.Items)
				{
					string assemblyFile = System.IO.Path.Combine(dir, item.Text);
					if (assemblyFile.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
					{
						AssemblyName asmname = AssemblyName.GetAssemblyName(assemblyFile);
						byte[] token = asmname.GetPublicKeyToken();
						if (token.Length > 0)
						{
							pub.GacInstall(assemblyFile);
							item.ImageKey = "Ask.OK";
						}
					}
				}
			}
			finally
			{
				this.Cursor = cursor;
			}
		}

		/// <summary>
		/// All Release ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonAllRelease_Click(object sender, EventArgs e)
		{
			Cursor cursor = this.Cursor;

			try
			{
				this.Cursor = Cursors.WaitCursor;
				this.IsUpdated = true;

				string dir = System.IO.Directory.GetCurrentDirectory();

				// AssemblyFoldersEx
				AxiGAC.AssemblyFoldersEx = "";

				// RegSvr 登録解除.
				AxiRegSvr.Remove();

				// Win32 登録解除.
				AxiEnv.Remove(0);

				// 表示更新.
				textAssemblyFoldersEx.Text = AxiGAC.AssemblyFoldersEx;
				textProductROOT.Text = AxiEnv.ProductROOT;
				textProductBIN.Text = AxiEnv.ProductBIN;
				textProductASM.Text = AxiEnv.ProductASM;

				// GAC 登録解除.
				var pub = new System.EnterpriseServices.Internal.Publish();
				foreach (ListViewItem item in listVersionNet.Items)
				{
					string assemblyFile = item.Text;
					pub.GacRemove(assemblyFile);
					item.ImageKey = "Ask.Cancel";
				}
			}
			finally
			{
				this.Cursor = cursor;
			}
		}


		/// <summary>
		/// GAC Register ボタンが押下されたとき (GAC 登録)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolGACRegister_Click(object sender, EventArgs e)
		{
			Cursor cursor = this.Cursor;

			try
			{
				this.Cursor = Cursors.WaitCursor;

				string dir = System.IO.Directory.GetCurrentDirectory();

				// GAC 登録.
				var pub = new System.EnterpriseServices.Internal.Publish();
				foreach (ListViewItem item in listVersionNet.Items)
				{
					string assemblyFile = System.IO.Path.Combine(dir, item.Text);
					if (assemblyFile.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
					{
						AssemblyName asmname = AssemblyName.GetAssemblyName(assemblyFile);
						byte[] token = asmname.GetPublicKeyToken();
						if (token.Length > 0)
						{
							pub.GacInstall(assemblyFile);
							item.ImageKey = "Ask.OK";
						}
					}
				}
			}
			finally
			{
				this.Cursor = cursor;
			}
		}

		/// <summary>
		/// GAC Release ボタンが押下されたとき (GAC 登録解除)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolGACRelease_Click(object sender, EventArgs e)
		{
			Cursor cursor = this.Cursor;

			try
			{
				this.Cursor = Cursors.WaitCursor;

				string dir = System.IO.Directory.GetCurrentDirectory();

				// GAC 登録解除.
				var pub = new System.EnterpriseServices.Internal.Publish();
				foreach (ListViewItem item in listVersionNet.Items)
				{
					string assemblyFile = item.Text;
					pub.GacRemove(assemblyFile);
					item.ImageKey = "Ask.Cancel";
				}
			}
			finally
			{
				this.Cursor = cursor;
			}
		}

		/// <summary>
		/// タブコントロールにフォーカスが当たった時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabVersion_Enter(object sender, EventArgs e)
		{
			if (tabVersion.SelectedTab.Controls.Count > 0)
				tabVersion.SelectedTab.Controls[0].Select();
		}

		/// <summary>
		/// タブページが切り替わった時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabVersion_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabVersion.SelectedTab.Controls.Count > 0)
				tabVersion.SelectedTab.Controls[0].Select();
		}

		/// <summary>
		/// リストビューのキーイベント監視
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ListView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.A && e.Control && !e.Alt && !e.Shift)
			{
				ListView view = (ListView)sender;
				foreach (ListViewItem item in view.Items)
					item.Selected = true;
			}
			if (e.KeyCode == Keys.D && e.Control && !e.Alt && !e.Shift)
			{
				ListView view = (ListView)sender;
				foreach (ListViewItem item in view.Items)
					item.Selected = false;
			}
			else if (e.KeyCode == Keys.C && e.Control && !e.Alt && !e.Shift)
			{
				string text = "";
				ListView view = (ListView)sender;
				foreach (ListViewItem item in view.Items)
				{
					if (item.Selected)
					{
						if (0 < text.Length)
							text += "\n";

						text += item.Text;
						foreach (ListViewItem.ListViewSubItem subitem in item.SubItems)
							text = text + "\t" + subitem.Text;
					}
				}
				if (0 < text.Length)
					System.Windows.Forms.Clipboard.SetText(text);
			}
		}

		#endregion
	}
}
