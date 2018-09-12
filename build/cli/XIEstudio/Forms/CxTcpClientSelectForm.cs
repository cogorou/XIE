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

namespace XIEstudio
{
	/// <summary>
	/// TCP/IP クライアント選択フォーム
	/// </summary>
	partial class CxTcpClientSelectForm : Form
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTcpClientSelectForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="info">TCP/IP クライアント情報</param>
		public CxTcpClientSelectForm(XIE.Tasks.CxTcpClientInfo info)
		{
			InitializeComponent();
			this.Info = info;
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// フォームロード時の初期化処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void IPSelectForm_Load(object sender, EventArgs e)
		{
			comboHostname.Text = Info.Hostname;

			#region 自動取得.
			try
			{
				string hostname = System.Net.Dns.GetHostName();
				System.Net.IPAddress[] ip_addrs = System.Net.Dns.GetHostAddresses(hostname);
				foreach (System.Net.IPAddress item in ip_addrs)
					comboHostname.Items.Add(string.Format("{0}", item));
			}
			catch (System.Exception)
			{
			}
			#endregion

			if (numericPort.Minimum <= Info.Port && Info.Port <= numericPort.Maximum)
				numericPort.Value = Info.Port;

			checkResolve.Checked = Info.IPResolve;

			if (numericIndex.Minimum <= Info.IPIndex && Info.IPIndex <= numericIndex.Maximum)
				numericIndex.Value = Info.IPIndex;
		}

		/// <summary>
		/// フォームが閉じられるときの解放処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void IPSelectForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Info.Hostname = comboHostname.Text;
			Info.Port = (int)numericPort.Value;
			Info.IPResolve = checkResolve.Checked;
			Info.IPIndex = (int)numericIndex.Value;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// TCP/IP クライアント情報
		/// </summary>
		public virtual XIE.Tasks.CxTcpClientInfo Info
		{
			get { return m_Info; }
			set { m_Info = value; }
		}
		private XIE.Tasks.CxTcpClientInfo m_Info = new XIE.Tasks.CxTcpClientInfo();

		#endregion

		#region コントロールイベント.

		/// <summary>
		/// ホスト名リストの項目が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboHostname_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (numericIndex.Minimum <= comboHostname.SelectedIndex &&
				numericIndex.Maximum >= comboHostname.SelectedIndex)
			{
				numericIndex.Value = comboHostname.SelectedIndex;
			}
		}

		/// <summary>
		/// IP Auto Resolve チェックボックスの ON/OFF が変化したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkResolve_CheckedChanged(object sender, EventArgs e)
		{
			if (checkResolve.Checked)
				checkResolve.BackColor = Color.Pink;
			else
				checkResolve.BackColor = this.BackColor;
		}

		#endregion
	}
}