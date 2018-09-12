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
	/// ログコレクション
	/// </summary>
	public class CxLogCollection
	{
		#region コンストラクタ.

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="capacity">許容量</param>
		public CxLogCollection(int capacity)
		{
			lock (Items)
			{
				Capacity = capacity;
			}
		}

		#endregion

		#region プロパティ:

		private CxRingList<CxLogElement> Items = new CxRingList<CxLogElement>();
		private Dictionary<ExLogLevel, CxRingList<CxLogElement>> Groups = new Dictionary<ExLogLevel, CxRingList<CxLogElement>>();

		/// <summary>
		/// 許容量
		/// </summary>
		public int Capacity
		{
			get
			{
				return Items.Capacity;
			}
			set
			{
				if (value <= 0)
					throw new System.ArgumentOutOfRangeException();

				lock (Items)
				{
					Items.Capacity = value;
					foreach (KeyValuePair<ExLogLevel, CxRingList<CxLogElement>> log in Groups)
					{
						log.Value.Capacity = value;
					}
				}
			}
		}

		/// <summary>
		/// シーケンス番号
		/// </summary>
		public UInt32 SequenceNo
		{
			get { return m_SequenceNo; }
		}
		private UInt32 m_SequenceNo = 0;

		/// <summary>
		/// ログ数 (全要素)
		/// </summary>
		public int Count
		{
			get { return Items.Count; }
		}

		/// <summary>
		/// ログ数 (種別毎)
		/// </summary>
		/// <param name="level">ログ出力レベル</param>
		public int CountInGroup(ExLogLevel level)
		{
			CxRingList<CxLogElement> log;
			if (Groups.TryGetValue(level, out log) == false)
				return 0;
			return log.Count;
		}

		#endregion

		#region メソッド: (追加)

		/// <summary>
		/// 追加
		/// </summary>
		/// <param name="level">ログ出力レベル</param>
		/// <param name="format"></param>
		/// <param name="arg0"></param>
		/// <example>
		/// <code lang="C#" source="examples/Log/ApiLog_Logs_01.cs"/>
		/// </example>
		public void Add(ExLogLevel level, string format, object arg0)
		{
			Add(level, string.Format(format, arg0));
		}
		
		/// <summary>
		/// 追加
		/// </summary>
		/// <param name="level">ログ出力レベル</param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		/// <example>
		/// <code lang="C#" source="examples/Log/ApiLog_Logs_01.cs"/>
		/// </example>
		public void Add(ExLogLevel level, string format, params object[] args)
		{
			Add(level, string.Format(format, args));
		}

		/// <summary>
		/// 追加
		/// </summary>
		/// <param name="level">ログ出力レベル</param>
		/// <param name="message"></param>
		/// <example>
		/// <code lang="C#" source="examples/Log/ApiLog_Logs_01.cs"/>
		/// </example>
		public void Add(ExLogLevel level, string message)
		{
			lock (Items)
			{
				CxLogElement log = new CxLogElement(m_SequenceNo, level, message);
				m_SequenceNo++;

				if (Items.Count == Items.Capacity)
				{
					try
					{
						Groups[level].Remove();
					}
					catch (System.Collections.Generic.KeyNotFoundException)
					{
					}
					catch (System.InvalidOperationException)
					{
					}
				}

				Items.Add(log);

				CxRingList<CxLogElement> list;
				if (Groups.TryGetValue(level, out list))
				{
					list.Add(log);
				}
				else
				{
					list = new CxRingList<CxLogElement>(Items.Capacity);
					list.Add(log);
					Groups.Add(level, list);
				}
			}
		}

		#endregion

		#region メソッド: (フェッチ)

		/// <summary>
		/// フェッチ
		/// </summary>
		/// <param name="index"></param>
		/// <returns>
		/// </returns>
		public CxLogElement Fetch(int index)
		{
			lock (Items)
			{
				CxRingList<CxLogElement> list = Items;
				if (index < 0 || index > list.Count)
				{
					throw new System.ArgumentOutOfRangeException();
				}
				return list[index];
			}
		}

		/// <summary>
		/// フェッチ
		/// </summary>
		/// <param name="level">ログ出力レベル</param>
		/// <param name="index"></param>
		/// <returns>
		/// </returns>
		public CxLogElement Fetch(ExLogLevel level, int index)
		{
			lock (Items)
			{
				CxRingList<CxLogElement> list = Groups[level];
				if (index < 0 || index > list.Count)
				{
					throw new System.ArgumentOutOfRangeException();
				}
				return list[index];
			}
		}

		#endregion

		#region メソッド: (保存)

		/// <summary>
		/// 全てのログをストリームに書き込みます
		/// </summary>
		/// <param name="stream"></param>
		/// <example>
		/// <code lang="C#" source="examples/Log/ApiLog_Logs_02.cs"/>
		/// </example>
		public void WriteToStream(System.IO.StreamWriter stream)
		{
			lock (Items)
			{
				foreach (CxLogElement log in Items)
				{
					stream.WriteLine("{0}\t{1}\t{2}", log.Number.ToString(), log.TimeStamp.ToString("yyyy/MM/dd HH:mm:ss.fff"), log.Message);
				}
			}
		}

		/// <summary>
		/// 指定されたグループのログをストリームに書き込みます
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="level">ログ出力レベル</param>
		/// <example>
		/// <code lang="C#" source="examples/Log/ApiLog_Logs_02.cs"/>
		/// </example>
		public void WriteToStream(System.IO.StreamWriter stream, ExLogLevel level)
		{
			CxRingList<CxLogElement> list = Groups[level];
			lock (list)
			{
				foreach (CxLogElement log in list)
				{
					stream.WriteLine("{0}\t{1}\t{2}", log.Number.ToString(), log.TimeStamp.ToString("yyyy/MM/dd HH:mm:ss.fff"), log.Message);
				}
			}
		}

		#endregion
	}
}
