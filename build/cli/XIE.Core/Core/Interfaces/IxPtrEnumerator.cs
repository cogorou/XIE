/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace XIE
{
	/// <summary>
	/// コレクションを反復処理する列挙子 (1次元配列用)
	/// </summary>
	/// <typeparam name="ITEM_TYPE">配列のインデクサが返す型</typeparam>
	public class IxPtrEnumerator<ITEM_TYPE> : System.Object
		, IDisposable
		, IEnumerator
		, IEnumerator<ITEM_TYPE>
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="body">配列本体</param>
		/// <param name="length">要素数</param>
		public IxPtrEnumerator(IxPointer<ITEM_TYPE> body, int length)
		{
			Body = body;
			Length = length;
		}

		/// <summary>
		/// リソースの解放
		/// </summary>
		void IDisposable.Dispose()
		{
		}

		/// <summary>
		/// 配列本体
		/// </summary>
		IxPointer<ITEM_TYPE> Body;
		
		/// <summary>
		/// 要素数 [範囲:0~、初期値:0]
		/// </summary>
		int Length = 0;

		/// <summary>
		/// 指標 [範囲:0~、初期値:-1]
		/// </summary>
		int Index = -1;

		/// <summary>
		/// 指標を１つ進めます。
		/// </summary>
		/// <returns>
		///		指標が要素数に達したら false を返します。
		///		それ以外の場合は true を返します。
		/// </returns>
		bool IEnumerator.MoveNext()
		{
			Index++;
			return (0 <= Index && Index < Length);
		}

		/// <summary>
		/// 指標を初期値に戻します。
		/// </summary>
		void IEnumerator.Reset()
		{
			Index = -1;
		}

		/// <summary>
		/// 現在の要素を返します。
		/// </summary>
		object IEnumerator.Current
		{
			get { return Body[Index]; }
		}

		/// <summary>
		/// 現在の要素を返します。
		/// </summary>
		ITEM_TYPE IEnumerator<ITEM_TYPE>.Current
		{
			get { return Body[Index]; }
		}
	}
}
