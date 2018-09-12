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
	/// Loading Progress Form
	/// </summary>
	public partial class LoadingProgressForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public LoadingProgressForm()
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
		private void LoadingProgressForm_Load(object sender, EventArgs e)
		{
			UpdateUI();
			this.TimerStart();
		}

		#endregion

		#region メソッド: (時間計測)

		/// <summary>
		/// 処理時間の計測開始
		/// </summary>
		public void TimerStart()
		{
			this.Timer.Start();
		}

		/// <summary>
		/// 処理時間の計測停止
		/// </summary>
		public void TimerStop()
		{
			this.ProcessTime += this.Timer.Lap;
		}

		/// <summary>
		/// 処理時間計測用
		/// </summary>
		public XIE.CxStopwatch Timer
		{
			get { return m_Timer; }
			set { m_Timer = value; }
		}
		private XIE.CxStopwatch m_Timer = new XIE.CxStopwatch();

		#endregion

		#region メソッド: (更新)

		/// <summary>
		/// 表示更新
		/// </summary>
		/// <param name="description">説明文</param>
		public void UpdateUI(string description)
		{
			this.Description = description;
			this.ProcessNum += 1;
			this.UpdateUI();
		}

		/// <summary>
		/// 表示更新
		/// </summary>
		public void UpdateUI()
		{
			int percentage = 0;
			if (ProcessMax > 0)
				percentage = ProcessNum * 100 / ProcessMax;
			this.labelPercentage.Text = percentage.ToString() + "%";
			this.Refresh();
		}

		/// <summary>
		/// 説明
		/// </summary>
		public string Description
		{
			get { return this.labelDescription.Text; }
			set { this.labelDescription.Text = value; }
		}

		/// <summary>
		/// 現在の処理数 [0~]
		/// </summary>
		public int ProcessNum
		{
			get { return m_ProcessNum; }
			set { m_ProcessNum = value; }
		}
		private int m_ProcessNum = 0;

		/// <summary>
		/// 目標の処理数 [1~]
		/// </summary>
		public int ProcessMax
		{
			get { return m_ProcessMax; }
			set { m_ProcessMax = value; }
		}
		private int m_ProcessMax = 1;

		/// <summary>
		/// 処理時間合計 (msec)
		/// </summary>
		public double ProcessTime
		{
			get { return m_ProcessTime; }
			set { m_ProcessTime = value; }
		}
		private double m_ProcessTime = 0;

		#endregion
	}
}
