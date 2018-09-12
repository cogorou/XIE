/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace XIE.IO
{
	#region パリティチェック:

	/// <summary>
	/// パリティチェック
	/// </summary>
	public enum ExParity
	{
		/// <summary>
		/// なし
		/// </summary>
		None = 0,
		/// <summary>
		/// 奇数
		/// </summary>
		Odd = 1,
		/// <summary>
		/// 偶数
		/// </summary>
		Even = 2,
		/// <summary>
		/// 常時1
		/// </summary>
		Mark = 3,
		/// <summary>
		/// 常時0
		/// </summary>
		Space = 4,
	}

	#endregion

	#region ストップビット:

	/// <summary>
	/// ストップビット
	/// </summary>
	public enum ExStopBits
	{
		/// <summary>
		/// 1ストップビット
		/// </summary>
		One = 0,
		/// <summary>
		/// 1.5ストップビット
		/// </summary>
		One5 = 1,
		/// <summary>
		/// 2ストップビット
		/// </summary>
		Two = 2,
	}

	#endregion

	#region ハンドシェイク:

	/// <summary>
	/// ハンドシェイク
	/// </summary>
	public enum ExHandshake
	{
		/// <summary>
		/// なし
		/// </summary>
		None = 0,
		/// <summary>
		/// ソフトウェア制御 (Xon/Xoff)
		/// </summary>
		XonXoff = 1,
		/// <summary>
		/// ハードウェア制御 (RTS/CTS)
		/// </summary>
		RtsCts = 2,
		/// <summary>
		/// ハードウェア制御 (DSR/DTR)
		/// </summary>
		DsrDtr = 3,
	}

	#endregion

	#region 改行コード:

	/// <summary>
	/// 改行コード
	/// </summary>
	public enum ExNewLine
	{
		/// <summary>
		/// なし
		/// </summary>
		None = 0,
		/// <summary>
		/// ラインフィード
		/// </summary>
		LF = 1,
		/// <summary>
		/// キャリッジリターン
		/// </summary>
		CR = 2,
		/// <summary>
		/// キャリッジリターン＋ラインフィード
		/// </summary>
		CRLF = 3,
	}

	#endregion
}
