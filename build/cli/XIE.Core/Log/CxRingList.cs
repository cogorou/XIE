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
	/// リングリスト
	/// </summary>
	/// <typeparam name="T"></typeparam>
	partial class CxRingList<T> : System.Collections.Generic.IEnumerable<T>
	{
		static T[] Empty = new T[0];

		/// <summary>
		/// 
		/// </summary>
		public CxRingList()
		{
			Items = Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="size"></param>
		public CxRingList(int size)
		{
			if (size < 0)
				throw new ArgumentOutOfRangeException();

			Items = new T[size + 1];
		}

		/// <summary>
		/// 
		/// </summary>
		int ItemSize
		{
			get
			{
				return Items.Length - 1;
			}
		}

		/// <summary>
		/// 許容量
		/// </summary>
		public int Capacity
		{
			get
			{
				return Items.Length;
			}
			set
			{
				if (value == ItemSize) return;
				if (value < m_Count)
					throw new ArgumentOutOfRangeException();

				if (value > 0)
				{
					Items = new T[value + 1];
				}
				else
				{
					Items = Empty;
				}
			}
		}

		/// <summary>
		/// 要素数
		/// </summary>
		public int Count
		{
			get
			{
				return m_Count;
			}
		}
		int m_Count = 0;

		/// <summary>
		/// 要素
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[int index]
		{
			get
			{
				if (index >= m_Count)
					throw new ArgumentOutOfRangeException();

				return Items[(First + index) % Items.Length];
			}
			set
			{
				if (index >= m_Count)
					throw new ArgumentOutOfRangeException();

				Items[(First + index) % Items.Length] = value;
			}
		}
		T[] Items;
		int First;
		int Last;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item)
		{
			Items[Last] = item;
			Last = (Last + 1) % Items.Length;

			if (First == Last)
			{
				First = (First + 1) % Items.Length;
			}
			else
			{
				m_Count++;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Remove()
		{
			if (First == Last)
				throw new InvalidOperationException();

			Items[First] = default(T);
			First = (First + 1) % Items.Length;

			m_Count--;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			if (m_Count > 0)
			{
				Array.Clear(Items, 0, Items.Length);
				m_Count = 0;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}

		#region Enumerator
		[Serializable()]
		public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
		{
			CxRingList<T> m_Items;
			int m_Index;
			T m_Current;

			internal Enumerator(CxRingList<T> list)
			{
				m_Items = list;
				m_Index = 0;
				m_Current = default(T);
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				CxRingList<T> localList = m_Items;

				if ((uint)m_Index < (uint)localList.Count)
				{
					m_Current = localList.Items[m_Index];
					m_Index++;

					return true;
				}

				return MoveNextRare();
			}

			bool MoveNextRare()
			{
				m_Index = m_Items.Count + 1;
				m_Current = default(T);

				return false;
			}

			public T Current
			{
				get { return m_Current; }
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					if (m_Index == 0 || m_Index == m_Items.Count + 1)
						throw new System.InvalidOperationException();

					return m_Current;
				}
			}

			void System.Collections.IEnumerator.Reset()
			{
				m_Index = 0;
				m_Current = default(T);
			}
		}
		#endregion
	}
}
