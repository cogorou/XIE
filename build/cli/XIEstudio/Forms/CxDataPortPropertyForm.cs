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

namespace XIEstudio
{
	/// <summary>
	/// データ入出力ポートプロパティフォーム
	/// </summary>
	partial class CxDataPortPropertyForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDataPortPropertyForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="thread">スレッド</param>
		public CxDataPortPropertyForm(XIE.Tasks.CxDataPortThread thread)
		{
			InitializeComponent();
			Thread = thread;
			propertyParam.SelectedObject = thread;
		}

		#endregion
		
		#region プロパティ:

		/// <summary>
		/// スレッド
		/// </summary>
		public XIE.Tasks.CxDataPortThread Thread
		{
			get { return m_Thread;  }
			set { m_Thread = value;  }
		}
		private XIE.Tasks.CxDataPortThread m_Thread = null;

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxDataPortPropertyForm_Load(object sender, EventArgs e)
		{
			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxDataPortPropertyForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Stop();
		}

		#endregion

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			bool enable = (Thread != null);
			bool running = (Thread != null && Thread.IsRunning);

			toolRun.Enabled = enable && !running;
			toolStop.Enabled = running;
		}

		// //////////////////////////////////////////////////
		// 全般
		// //////////////////////////////////////////////////

		#region コントロールイベント:

		/// <summary>
		/// 開始
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolRun_Click(object sender, EventArgs e)
		{
			if (Thread == null) return;

			try
			{
				if (Thread.IsRunning)
					Thread.Stop();
				else
					Thread.Start();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// 停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStop_Click(object sender, EventArgs e)
		{
			if (Thread == null) return;

			try
			{
				Thread.Stop();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion
	}
}