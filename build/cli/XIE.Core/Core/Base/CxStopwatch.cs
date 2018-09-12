/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace XIE
{
	/// <summary>
	/// 時間計測クラス
	/// </summary>
	/// <example>
	/// <b>基本的な使用方法:</b><br/>
	/// <code lang="C#" source="examples/Core/Base/CxStopwatch_01.cs"/>
	/// <br/>
	/// <b>Lap と Elapsed の違い:</b><br/>
	/// <code lang="C#" source="examples/Core/Base/CxStopwatch_02.cs"/>
	/// <br/>
	/// <b>Reset メソッドによる Elapsed のリセット:</b><br/>
	/// <code lang="C#" source="examples/Core/Base/CxStopwatch_03.cs"/>
	/// </example>
	public class CxStopwatch : System.Object
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxStopwatch()
		{
			this.Watch = new System.Diagnostics.Stopwatch();
			this.Reset();
			this.Start();
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// ストップウォッチ
		/// </summary>
		private System.Diagnostics.Stopwatch Watch;

		/// <summary>
		/// ラップタイム (msec)
		/// </summary>
		public double Lap;

		/// <summary>
		/// 経過時間 (msec)
		/// </summary>
		public double Elapsed;

		#endregion

		#region メソッド:

		/// <summary>
		/// リセット
		/// </summary>
		public void Reset()
		{
			this.Lap = 0;
			this.Elapsed = 0;
		}

		/// <summary>
		/// 計測開始
		/// </summary>
		public void Start()
		{
			this.Watch.Reset();
			this.Watch.Start();
		}

		/// <summary>
		/// 計測停止
		/// </summary>
		public void Stop()
		{
			this.Watch.Stop();
			this.Lap = this.Watch.Elapsed.TotalMilliseconds;
			this.Elapsed += this.Lap;
			this.Watch.Reset();
			this.Watch.Start();
		}

		#endregion
	}
}
