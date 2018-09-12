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

namespace XIEversion
{
	/// <summary>
	/// PATH 環境変数編集フォーム
	/// </summary>
	public partial class PathForm : Form
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public PathForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// フォームがロードされたときの初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PathForm_Load(object sender, EventArgs e)
		{
			string path = AxiEnv.PATH;
			if (path != null)
			{
				List<string> lines = new List<string>();
				string[] items = path.Split(';');
				foreach (string item in items)
				{
					string temp = item.Trim();
					if (string.IsNullOrEmpty(temp)) continue;
					lines.Add(temp);
				}
				textPath.Lines = lines.ToArray();
			}
		}

		/// <summary>
		/// OK ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolOK_Click(object sender, EventArgs e)
		{
			StringBuilder path = new StringBuilder();

			try
			{
				string sysdir = Environment.GetFolderPath(Environment.SpecialFolder.System);
				string windir = System.IO.Directory.GetParent(sysdir).FullName;
				int sysdirs = 0;
				int windirs = 0;
				foreach (string line in textPath.Lines)
				{
					if (line.ToLower() == sysdir.ToLower()) sysdirs++;
					if (line.ToLower() == windir.ToLower()) windirs++;
					path.Append(line.Trim() + ";");
				}

				if (sysdirs == 0 || windirs == 0)
				{
					MessageBox.Show(this, "System directory does not contain.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

#if true
				AxiEnv.PATH = path.ToString();
#else
				// DEBUG
				MessageBox.Show(this, path.ToString(), "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif

				this.DialogResult = System.Windows.Forms.DialogResult.OK;
				this.Close();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				path.Length = 0;
			}
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

		/// <summary>
		/// テキストボックスの内容が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textPath_TextChanged(object sender, EventArgs e)
		{
			int count = 0;
			foreach (string line in textPath.Lines)
			{
				count += line.Trim().Length;
			}
			statusbarMessage.Text = count.ToString() + " characters";
		}
	}
}
