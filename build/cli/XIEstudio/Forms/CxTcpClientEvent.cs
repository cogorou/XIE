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
	/// TCP/IP 受信イベントハンドラのデリゲート
	/// </summary>
	/// <param name="sender">イベント送り主</param>
	/// <param name="e">イベントの内容</param>
	public delegate void CxTcpClientEventHandler(object sender, CxTcpClientEventArgs e);

	/// <summary>
	/// TCP/IP 受信イベント引数クラス
	/// </summary>
	public partial class CxTcpClientEventArgs : EventArgs
	{
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTcpClientEventArgs()
		{
		}

		/// <summary>
		/// 初期値指定コンストラクタ
		/// </summary>
		/// <param name="timestamp">タイムスタンプ</param>
		public CxTcpClientEventArgs(DateTime timestamp)
		{
			TimeStamp = timestamp;
		}

		/// <summary>
		/// 初期値指定コンストラクタ
		/// </summary>
		/// <param name="timestamp">タイムスタンプ</param>
		/// <param name="exception">エラーの内容</param>
		public CxTcpClientEventArgs(DateTime timestamp, System.Exception exception)
		{
			TimeStamp = timestamp;
			Exception = exception;
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
		/// エラーの内容
		/// </summary>
		[NonSerialized]
		public System.Exception Exception = null;

		#endregion
	}
}
