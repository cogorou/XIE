/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace XIE.Log
{
	#region ログ出力レベル:

	/// <summary>
	/// ログ出力レベル
	/// </summary>
	public enum ExLogLevel
	{
		/// <summary>
		/// トレース出力 (処理の経過などを解析したい場合に使う)
		/// </summary>
		Trace = 0,

		/// <summary>
		/// 例外が発生したときに出力する。
		/// </summary>
		Error = 1,
	}

	#endregion
}
