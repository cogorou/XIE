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
	/// TCP/IP サーバーフォーム
	/// </summary>
	partial class CxTcpServerPropertyForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTcpServerPropertyForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ (指定されたオブジェクトをプロパティに設定して構築します。)
		/// </summary>
		/// <param name="node">TCP/IP サーバーノード</param>
		public CxTcpServerPropertyForm(CxTcpServerNode node)
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
		protected virtual void TcpServerPropertyForm_Load(object sender, EventArgs e)
		{
			toolEncoding.SelectedIndex = 0;

			// プロパティグリッド.
			propertyParam.SelectedObject = Node.Controller;

			timerUpdateUI.Enabled = true;
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void TcpServerPropertyForm_FormClosing(object sender, FormClosingEventArgs e)
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
			int connections = (controller == null) ? 0 : controller.Connections();

			#region 表示更新.(Prev/Next)
			if (connections == 0)
			{
				ClientIndex = 0;
				toolClientPrev.Enabled = false;
				toolClientNext.Enabled = false;
				toolClientIndex.Text = "";
				statusClientCount.Text = "Non client";
			}
			else if (connections == 1)
			{
				ClientIndex = 0;
				toolClientPrev.Enabled = false;
				toolClientNext.Enabled = false;
				toolClientIndex.Text = "0";
				statusClientCount.Text = "1 client";
			}
			else
			{
				if (ClientIndex > (connections - 1))
					ClientIndex = (connections - 1);
				toolClientPrev.Enabled = true;
				toolClientNext.Enabled = true;
				toolClientIndex.Text = ClientIndex.ToString();
				statusClientCount.Text = connections + " clients";
			}
			#endregion

			#region 表示更新.
			propertyParam.Enabled = (controller != null);
			buttonSend.Enabled = (controller != null);
			textSend.Enabled = (controller != null);
			textRecv.Enabled = (controller != null);
			#endregion

			#region データ受信.
			if (controller != null && controller.IsValid)
			{
				for (int i = 0; i < controller.Connections(); i++)
				{
					XIE.Net.TxSocketStream stream = controller.Stream(i);
					while (stream.Readable(0))
					{
						byte[] buffer = new byte[256];
						int count = stream.Read(buffer, buffer.Length, 0);
						if (count <= 0) break;
						statusInfo.Text = string.Format("RECV: {0} bytes", count);

						var encoding = GetEncoding();
						string text = encoding.GetString(buffer, 0, count);
						text = System.Text.RegularExpressions.Regex.Replace(text, @"[\r\n]+$", "");

						textRecv.SelectionColor = Color.Red;
						textRecv.AppendText(text + System.Environment.NewLine);
						textRecv.SelectionStart = textRecv.Text.Length;
						textRecv.ScrollToCaret();
					}
				}
			}
			#endregion
		}

		#region プロパティ:

		/// <summary>
		/// TCP/IP サーバーノード
		/// </summary>
		public virtual CxTcpServerNode Node
		{
			get { return m_Node; }
			set { m_Node = value; }
		}
		private CxTcpServerNode m_Node = null;

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
			#region 送信.
			try
			{
				if (0 <= ClientIndex && ClientIndex < Node.Controller.Connections())
				{
					XIE.Net.TxSocketStream stream = Node.Controller.Stream(ClientIndex);

					// 送信.
					var encoding = GetEncoding();
					byte[] buffer = encoding.GetBytes(textSend.Text);
					int count = stream.Write(buffer, buffer.Length, 0);
					statusInfo.Text = string.Format("SEND: {0} bytes", count);

					// 表示.
					textRecv.SelectionColor = Color.Blue;
					textRecv.AppendText(textSend.Text + System.Environment.NewLine);
					textRecv.SelectionStart = textRecv.Text.Length;
					textRecv.ScrollToCaret();

					textSend.Text = "";
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			#endregion
		}

		/// <summary>
		/// 現在のエンコーディングを取得します。
		/// </summary>
		/// <returns>
		///		エンコーディングを返します。
		/// </returns>
		private Encoding GetEncoding()
		{
			// エンコーディング.
			var encoding = Encoding.Default;
			switch (toolEncoding.Text)
			{
				case "Default":
					encoding = Encoding.Default;
					break;
				case "ASCII":
					encoding = Encoding.ASCII;
					break;
				case "UTF7":
					encoding = Encoding.UTF7;
					break;
				case "UTF8":
					encoding = Encoding.UTF8;
					break;
				case "UTF32":
					encoding = Encoding.UTF32;
					break;
				case "Unicode":
					encoding = Encoding.Unicode;
					break;
				case "BigEndianUnicode":
					encoding = Encoding.BigEndianUnicode;
					break;
				default:
					try
					{
						var codepage = Convert.ToInt32(toolEncoding.Text);
						encoding = Encoding.GetEncoding(codepage);
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					break;
			}
			return encoding;
		}

		#endregion

		#region クライアント指標.

		/// <summary>
		/// クライアント指標 [範囲:0~]
		/// </summary>
		private int ClientIndex = 0;

		/// <summary>
		/// クライアント指標選択 (前へ)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolClientPrev_Click(object sender, EventArgs e)
		{
			var controller = Node.Controller;
			if (controller == null) return;

			int connections = (controller == null) ? 0 : controller.Connections();
			if (connections <= 1) return;

			ClientIndex--;
			if (ClientIndex < 0)
				ClientIndex = connections - 1;
		}

		/// <summary>
		/// クライアント指標選択 (次へ)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolClientNext_Click(object sender, EventArgs e)
		{
			var controller = Node.Controller;
			if (controller == null) return;

			int connections = (controller == null) ? 0 : controller.Connections();
			if (connections <= 1) return;

			ClientIndex++;
			if (ClientIndex > (connections - 1))
				ClientIndex = 0;
		}

		#endregion
	}
}