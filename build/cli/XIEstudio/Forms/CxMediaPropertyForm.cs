/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XIEstudio
{
	/// <summary>
	/// メディアプロパティフォーム
	/// </summary>
	public partial class CxMediaPropertyForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxMediaPropertyForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="player">プレイヤー</param>
		public CxMediaPropertyForm(XIE.Media.CxMediaPlayer player)
		{
			InitializeComponent();
			Player = player;
			propertyParam.SelectedObject = player;
		}

		#endregion
		
		#region プロパティ:

		/// <summary>
		/// プレイヤー
		/// </summary>
		public XIE.Media.CxMediaPlayer Player
		{
			get { return m_Player;  }
			set { m_Player = value;  }
		}
		private XIE.Media.CxMediaPlayer m_Player = null;

		/// <summary>
		/// シークの最小単位 (x 100 nsec)
		/// </summary>
		const int UNIT = 1000 * 10;

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxMediaPropertyForm_Load(object sender, EventArgs e)
		{
			#region Seeking の初期化:
			{
				var duration = Player.GetDuration();
				var st = Player.GetStartPosition();
				var ed = Player.GetStopPosition();

				// Current Position
				{
					var track = trackCurrentPosition;
					// 
					track.Minimum = 0;
					track.Maximum = (int)(duration / UNIT);
					track.Value = (int)(st / UNIT);
				}
				// Start Position
				{
					var numeric = numericStartPosition;
					var track = trackStartPosition;
					// 
					track.Minimum = 0;
					track.Maximum = (int)(duration / UNIT);
					track.Value = (int)(st / UNIT);
					// 
					numeric.Minimum = 0;
					numeric.Maximum = (int)(duration / UNIT);
					numeric.Value = (int)(st / UNIT);
				}
				// Stop Position
				{
					var numeric = numericStopPosition;
					var track = trackStopPosition;
					// 
					track.Minimum = 0;
					track.Maximum = (int)(duration / UNIT);
					track.Value = (int)(ed / UNIT);
					// 
					numeric.Minimum = 0;
					numeric.Maximum = (int)(duration / UNIT);
					numeric.Value = (int)(ed / UNIT);
				}
			}
			#endregion

			timerUpdateUI.Start();
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CxMediaPropertyForm_FormClosing(object sender, FormClosingEventArgs e)
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
			bool enable = (Player != null);
			bool running = (Player != null && Player.IsRunning);
			bool paused = (Player != null && Player.IsPaused);

			toolRun.Enabled = enable && !running;
			toolPause.Enabled = running;
			toolPause.Checked = paused;
			toolStop.Enabled = running;

			trackCurrentPosition.Value = (int)(Player.GetStartPosition() / UNIT);
			textCurrentPosition.Text = trackCurrentPosition.Value.ToString();
		}

		// //////////////////////////////////////////////////
		// 全般
		// //////////////////////////////////////////////////

		#region コントロールイベント: (ToolBar)

		/// <summary>
		/// 開始
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolRun_Click(object sender, EventArgs e)
		{
			if (Player == null) return;

			try
			{
				if (Player.IsRunning)
					Player.Stop();
				else
				{
					Player.SetStartPosition((long)numericStartPosition.Value * UNIT);
					Player.SetStopPosition((long)numericStopPosition.Value * UNIT);
					Player.Start();
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// 一時停止
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolPause_Click(object sender, EventArgs e)
		{
			if (Player == null) return;

			try
			{
				if (Player.IsPaused)
					Player.Start();
				else
					Player.Pause();
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
			if (Player == null) return;

			try
			{
				Player.Stop();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// リセット
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolReset_Click(object sender, EventArgs e)
		{
			if (Player == null) return;

			try
			{
				Player.Reset();
				numericStartPosition.Value = (int)(Player.GetStartPosition() / UNIT);
				numericStopPosition.Value = (int)(Player.GetStopPosition() / UNIT);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region コントロールイベント: (Seeking)

		/// <summary>
		/// 開始位置の numericUpDown の値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericStartPosition_ValueChanged(object sender, EventArgs e)
		{
			int value = (int)numericStartPosition.Value;
			Player.SetStartPosition((long)value * UNIT);
			if (trackStartPosition.Value != value)
				trackStartPosition.Value = value;
		}

		/// <summary>
		/// 終了位置の numericUpDown の値が変更されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void numericStopPosition_ValueChanged(object sender, EventArgs e)
		{
			int value = (int)numericStopPosition.Value;
			Player.SetStopPosition((long)value * UNIT);
			if (trackStopPosition.Value != value)
				trackStopPosition.Value = value;
		}

		/// <summary>
		/// 開始位置の trackBar がスクロールされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackStartPosition_Scroll(object sender, EventArgs e)
		{
			numericStartPosition.Value = trackStartPosition.Value;
		}

		/// <summary>
		/// 終了位置の trackBar がスクロールされたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackStopPosition_Scroll(object sender, EventArgs e)
		{
			numericStopPosition.Value = trackStopPosition.Value;
		}

		#endregion
	}
}
