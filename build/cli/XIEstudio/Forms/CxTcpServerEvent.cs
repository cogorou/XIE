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
using System.Net;
using System.Net.Sockets;

namespace XIEstudio
{
	/// <summary>
	/// TCP/IP サーバーイベントハンドラのデリゲート
	/// </summary>
	/// <param name="sender">イベント送り主</param>
	/// <param name="e">イベントの内容</param>
	public delegate void CxTcpServerEventHandler(object sender, CxTcpServerEventArgs e);

	/// <summary>
	/// TCP/IP サーバーイベント引数クラス
	/// </summary>
	public partial class CxTcpServerEventArgs : EventArgs
	{
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTcpServerEventArgs()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="timestamp">タイムスタンプ</param>
		/// <param name="client">クライアント</param>
		public CxTcpServerEventArgs(DateTime timestamp, Socket client)
		{
			TimeStamp = timestamp;
			Client = client;
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// タイムスタンプ (LCT)
		/// </summary>
		public virtual System.DateTime TimeStamp
		{
			get { return m_TimeStamp; }
			set { m_TimeStamp = value; }
		}
		private System.DateTime m_TimeStamp = System.DateTime.Now;

		/// <summary>
		/// クライアント
		/// </summary>
		public virtual Socket Client
		{
			get { return m_Client; }
			set { m_Client = value; }
		}
		private Socket m_Client = null;

		#endregion
	}
}
