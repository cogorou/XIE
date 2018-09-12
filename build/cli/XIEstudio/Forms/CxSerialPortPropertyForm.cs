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
using System.IO.Ports;

namespace XIEstudio
{
	/// <summary>
	/// シリアル通信ポートプロパティフォーム
	/// </summary>
	partial class CxSerialPortPropertyForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxSerialPortPropertyForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ (指定されたオブジェクトをプロパティに設定して構築します。)
		/// </summary>
		/// <param name="src">シリアル通信ポートオブジェクト</param>
		public CxSerialPortPropertyForm(XIE.IO.CxSerialPort src)
		{
			InitializeComponent();
			SerialPort = src;
			SerialPortInfo.CopyFrom(src);
			CloseWhenClosing = false;
		}

		/// <summary>
		/// コンストラクタ (指定された情報に従ってシリアル通信ポートオブジェクトを生成して構築します。)
		/// </summary>
		/// <param name="info">シリアル通信ポート情報</param>
		public CxSerialPortPropertyForm(XIE.Tasks.CxSerialPortInfo info)
		{
			InitializeComponent();
			SerialPortInfo = info;
			if (SerialPortInfo != null)
				SerialPortInfo.CopyTo(SerialPort);
			CloseWhenClosing = true;
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void SerialPortSelectForm_Load(object sender, EventArgs e)
		{
			// プロパティグリッド.
			propertyParam.SelectedObject = SerialPortInfo;

			timerUpdateUI.Enabled = true;
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void SerialPortSelectForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Enabled = false;

			#region ポートクローズ.
			try
			{
				if (CloseWhenClosing)
					this.SerialPort.Dispose();
			}
			catch (System.Exception)
			{
			}
			#endregion
		}

		#endregion

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			#region 表示更新.
			toolConnect.Enabled = (SerialPort != null);
			toolConnect.Checked = (SerialPort != null && SerialPort.IsValid);

			propertyParam.Enabled = (SerialPort != null && !SerialPort.IsValid);

			buttonSend.Enabled = (SerialPort != null && SerialPort.IsValid);
			textSend.Enabled = (SerialPort != null && SerialPort.IsValid);
			textRecv.Enabled = (SerialPort != null && SerialPort.IsValid);
			#endregion

			#region OK/Cancel の可視属性.
			buttonOK.Visible = EnableResultButton;
			buttonCancel.Visible = EnableResultButton;
			#endregion

			#region データ受信.
			while (SerialPort != null && SerialPort.IsValid)
			{
				byte[] buffer = new byte[256];
				int count = SerialPort.Read(buffer, buffer.Length, 0);
				if (count <= 0) break;
				string text = Encoding.UTF8.GetString(buffer, 0, count);
				text = System.Text.RegularExpressions.Regex.Replace(text, @"[\r\n]+$", "\r\n");

				textRecv.SelectionColor = Color.Red;
				textRecv.AppendText(text);
				if (text.IndexOf("\r\n") >= 0)
				{
					textRecv.SelectionStart = textRecv.Text.Length;
					textRecv.ScrollToCaret();
				}
			}
			#endregion
		}

		#region プロパティ:

		/// <summary>
		/// シリアル通信ポートオブジェクト
		/// </summary>
		public virtual XIE.IO.CxSerialPort SerialPort
		{
			get { return m_SerialPort; }
			set { m_SerialPort = value; }
		}
		private XIE.IO.CxSerialPort m_SerialPort = new XIE.IO.CxSerialPort();

		/// <summary>
		/// シリアル通信ポート情報
		/// </summary>
		public virtual XIE.Tasks.CxSerialPortInfo SerialPortInfo
		{
			get { return m_SerialPortInfo; }
			set { m_SerialPortInfo = value; }
		}
		private XIE.Tasks.CxSerialPortInfo m_SerialPortInfo = new XIE.Tasks.CxSerialPortInfo();

		/// <summary>
		/// OK/Cancel ボタンの可視属性 [既定値:true]
		/// </summary>
		public virtual bool EnableResultButton
		{
			get { return m_EnableResultButton; }
			set { m_EnableResultButton = value; }
		}
		private bool m_EnableResultButton = true;

		/// <summary>
		/// フォームが閉じられるときにポートをクローズするか否か
		/// </summary>
		public virtual bool CloseWhenClosing
		{
			get { return m_CloseWhenClosing; }
			set { m_CloseWhenClosing = value; }
		}
		private bool m_CloseWhenClosing = false;

		#endregion

		#region コントロールイベント: (OK/Cancel)

		/// <summary>
		/// OK ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void buttonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Cancel ボタンが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void buttonCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		#endregion

		#region コントロールイベント:

		/// <summary>
		/// 接続／切断
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void toolConnect_Click(object sender, EventArgs e)
		{
			if (SerialPort == null) return;
			if (SerialPortInfo == null) return;

			#region 接続／切断.
			try
			{
				if (SerialPort.IsValid)
					SerialPort.Dispose();
				else
				{
					SerialPortInfo.CopyTo(SerialPort);
					SerialPort.Setup();
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion
		}

		/// <summary>
		/// クリア
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolClear_Click(object sender, EventArgs e)
		{
			textSend.Text = "";
			textRecv.Text = "";

			if (SerialPort != null && SerialPort.IsValid)
			{
			}
		}

		/// <summary>
		/// 送信テキストボックス
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textSend_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (SerialPort == null) return;
			if (SerialPort.IsValid == false) return;

			if (e.KeyCode == Keys.Enter)
				buttonSend_Click(sender, e);
		}

		/// <summary>
		/// 送信
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void buttonSend_Click(object sender, EventArgs e)
		{
			if (SerialPort == null) return;
			if (SerialPort.IsValid == false) return;

			#region 送信.
			try
			{
				// 改行コード.
				string newline = "";
				switch (SerialPortInfo.NewLine)
				{
					case XIE.IO.ExNewLine.LF: newline = "\n"; break;
					case XIE.IO.ExNewLine.CR: newline = "\r"; break;
					case XIE.IO.ExNewLine.CRLF: newline = "\r\n"; break;
				}

				// 送信.
				byte[] buffer = Encoding.UTF8.GetBytes(textSend.Text + newline);
				int count = SerialPort.Write(buffer, buffer.Length, 0);

				// 表示.
				textRecv.SelectionColor = Color.Blue;
				textRecv.AppendText(textSend.Text + System.Environment.NewLine);
				textRecv.SelectionStart = textRecv.Text.Length;
				textRecv.ScrollToCaret();

				textSend.Text = "";
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion
		}

		#endregion
	}
}