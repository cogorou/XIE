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
	/// TCP/IP クライアントフォーム
	/// </summary>
	partial class CxTcpClientPropertyForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTcpClientPropertyForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ (指定されたオブジェクトをプロパティに設定して構築します。)
		/// </summary>
		/// <param name="node">TCP/IP クライアントノード</param>
		public CxTcpClientPropertyForm(CxTcpClientNode node)
		{
			InitializeComponent();
			Node = node;
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void TcpClientPropertyForm_Load(object sender, EventArgs e)
		{
			// プロパティグリッド.
			propertyParam.SelectedObject = Node.Controller;
			timerUpdateUI.Enabled = true;
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void TcpClientPropertyForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			timerUpdateUI.Enabled = false;
		}

		#endregion

		/// <summary>
		/// 定期的な表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void timerUpdateUI_Tick(object sender, EventArgs e)
		{
			var controller = Node.Controller;

			#region 表示更新.
			propertyParam.Enabled = (controller != null);
			buttonSend.Enabled = (controller != null);
			textSend.Enabled = (controller != null);
			textRecv.Enabled = (controller != null);
			#endregion

			#region 表示更新: (サーバー接続状態)
			if (controller != null &&
				controller.Connected())
			{
				statusServerStatus.Text = "[ Connected ]";
			}
			else
			{
				statusServerStatus.Text = "[ Disconnected ]";
			}
			#endregion

			#region データ受信.
			if (controller != null && controller.IsValid)
			{
				XIE.Net.TxSocketStream stream = controller.Stream();
				while (stream.Readable(0))
				{
					byte[] buffer = new byte[256];
					int count = stream.Read(buffer, buffer.Length, 0);
					if (count <= 0) break;
					string text = Encoding.UTF8.GetString(buffer, 0, count);
					text = System.Text.RegularExpressions.Regex.Replace(text, @"[\r\n]+$", "");

					textRecv.SelectionColor = Color.Red;
					textRecv.AppendText(text + System.Environment.NewLine);
					textRecv.SelectionStart = textRecv.Text.Length;
					textRecv.ScrollToCaret();
				}
			}
			#endregion
		}

		#region プロパティ:

		/// <summary>
		/// TCP/IP クライアントノード
		/// </summary>
		public virtual CxTcpClientNode Node
		{
			get { return m_Node; }
			set { m_Node = value; }
		}
		private CxTcpClientNode m_Node = null;

		#endregion

		#region コントロールイベント:

		/// <summary>
		/// クリア
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolClear_Click(object sender, EventArgs e)
		{
			textSend.Text = "";
			textRecv.Text = "";
		}

		/// <summary>
		/// 送信テキストボックス
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textSend_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
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
			if (Node == null) return;
			if (Node.Controller == null) return;

			#region 送信.
			try
			{
				XIE.Net.TxSocketStream stream = Node.Controller.Stream();

				// 送信.
				byte[] buffer = Encoding.UTF8.GetBytes(textSend.Text);
				int count = stream.Write(buffer, buffer.Length, 0);

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