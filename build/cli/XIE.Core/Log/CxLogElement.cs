/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace XIE.Log
{
	/// <summary>
	/// ログ要素
	/// </summary>
	public class CxLogElement
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="number">シーケンス番号 [範囲:0~]</param>
		/// <param name="level">ログ出力レベル [既定値:Trace]</param>
		/// <param name="message">メッセージ</param>
		public CxLogElement(uint number, ExLogLevel level, string message)
		{
			this.m_Number = number;
			this.m_Level = level;
			this.m_Message = message;
			this.m_TimeStamp = DateTime.Now;
		}

		#region プロパティ:

		/// <summary>
		/// シーケンス番号 [範囲:0~]
		/// </summary>
		public uint Number
		{
			get { return m_Number; }
		}
		private UInt32 m_Number = 0;

		/// <summary>
		/// ログ出力レベル [既定値:Trace]
		/// </summary>
		public ExLogLevel Level
		{
			get { return m_Level; }
		}
		private ExLogLevel m_Level = ExLogLevel.Trace;

		/// <summary>
		/// メッセージ
		/// </summary>
		public string Message
		{
			get { return m_Message; }
		}
		string m_Message;

		/// <summary>
		/// タイムスタンプ
		/// </summary>
		public DateTime TimeStamp
		{
			get { return m_TimeStamp; }
		}
		DateTime m_TimeStamp;

		#endregion
	}
}
