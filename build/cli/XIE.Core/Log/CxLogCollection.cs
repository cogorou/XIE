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
	/// ���O�R���N�V����
	/// </summary>
	public class CxLogCollection
	{
		#region �R���X�g���N�^.

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="capacity">���e��</param>
		public CxLogCollection(int capacity)
		{
			lock (Items)
			{
				Capacity = capacity;
			}
		}

		#endregion

		#region �v���p�e�B:

		private CxRingList<CxLogElement> Items = new CxRingList<CxLogElement>();
		private Dictionary<ExLogLevel, CxRingList<CxLogElement>> Groups = new Dictionary<ExLogLevel, CxRingList<CxLogElement>>();

		/// <summary>
		/// ���e��
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
		/// �V�[�P���X�ԍ�
		/// </summary>
		public UInt32 SequenceNo
		{
			get { return m_SequenceNo; }
		}
		private UInt32 m_SequenceNo = 0;

		/// <summary>
		/// ���O�� (�S�v�f)
		/// </summary>
		public int Count
		{
			get { return Items.Count; }
		}

		/// <summary>
		/// ���O�� (��ʖ�)
		/// </summary>
		/// <param name="level">���O�o�̓��x��</param>
		public int CountInGroup(ExLogLevel level)
		{
			CxRingList<CxLogElement> log;
			if (Groups.TryGetValue(level, out log) == false)
				return 0;
			return log.Count;
		}

		#endregion

		#region ���\�b�h: (�ǉ�)

		/// <summary>
		/// �ǉ�
		/// </summary>
		/// <param name="level">���O�o�̓��x��</param>
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
		/// �ǉ�
		/// </summary>
		/// <param name="level">���O�o�̓��x��</param>
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
		/// �ǉ�
		/// </summary>
		/// <param name="level">���O�o�̓��x��</param>
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

		#region ���\�b�h: (�t�F�b�`)

		/// <summary>
		/// �t�F�b�`
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
		/// �t�F�b�`
		/// </summary>
		/// <param name="level">���O�o�̓��x��</param>
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

		#region ���\�b�h: (�ۑ�)

		/// <summary>
		/// �S�Ẵ��O���X�g���[���ɏ������݂܂�
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
		/// �w�肳�ꂽ�O���[�v�̃��O���X�g���[���ɏ������݂܂�
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="level">���O�o�̓��x��</param>
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
