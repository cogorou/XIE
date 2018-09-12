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

namespace XIE.Log
{
	/// <summary>
	/// ログフォーム
	/// </summary>
	public partial class CxLogForm : Form
	{
		#region コンストラクタ.

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxLogForm()
		{
			InitializeComponent();
		}

		#endregion

		#region 初期化と解放.

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxLogForm_Load(object sender, EventArgs e)
		{
			#region ログビューの初期化.
			KeyValuePair<ExLogLevel, CxLogView>[] items = 
			{
				new KeyValuePair<ExLogLevel, CxLogView>(ExLogLevel.Trace, logView0),
				new KeyValuePair<ExLogLevel, CxLogView>(ExLogLevel.Error, logView1),
			};
			for (int i = 0; i < items.Length; i++)
			{
				items[i].Value.Logs = XIE.Log.Api.Logs;
				items[i].Value.Level = items[i].Key;
				items[i].Value.AutoScroll = true;
			}
			#endregion
		}

		#endregion

		#region コントロールイベント.

		/// <summary>
		/// 閉じるボタンが押下された時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// 保存ボタンが押下された時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonSave_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog dlg = new SaveFileDialog())
			{
				#region 既定のファイル名の生成.
				DateTime timestamp = DateTime.Now;
				string filename =
					string.Format(
						"Log-{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}.log",
						timestamp.Year, timestamp.Month, timestamp.Day,
						timestamp.Hour, timestamp.Minute, timestamp.Second);
				#endregion

				dlg.Filter = "Log File(*.log)|*.log|All Files(*.*)|*.*";
				dlg.FilterIndex = 0;
				dlg.FileName = filename;
				//dlg.InitialDirectory;

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					Cursor.Current = Cursors.WaitCursor;

					System.IO.StreamWriter writer = new System.IO.StreamWriter(dlg.FileName);
					XIE.Log.Api.Logs.WriteToStream(writer);
					writer.Close();
					m_LogDir = System.IO.Path.GetDirectoryName(dlg.FileName);

					Cursor.Current = Cursors.Default;
				}
			}
		}
		private string m_LogDir = "";

		#endregion
	}
}
