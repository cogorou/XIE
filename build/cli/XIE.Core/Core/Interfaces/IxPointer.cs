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
	/// 構造体のポインタインターフェース (1次元配列用) 
	/// </summary>
	/// <typeparam name="ITEM_TYPE">配列要素の型</typeparam>
	public interface IxPointer<ITEM_TYPE>
	{
		#region アンマネージリソースの操作

		/// <summary>
		/// アンマネージリソースの確保
		/// </summary>
		/// <param name="num">要素数 [1~]</param>
		/// <returns>
		///		生成されたオブジェクトを返します。
		/// </returns>
		IntPtr Allocate(int num);

		/// <summary>
		/// 配列の先頭アドレス
		/// </summary>
		/// <returns>
		///		配列の先頭アドレスを返します。
		/// </returns>
		IntPtr Address
		{
			get;
			set;
		}

		/// <summary>
		/// 配列要素の値
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		/// <returns>
		///		指定された要素を返します。
		/// </returns>
		ITEM_TYPE this[int index]
		{
			get;
			set;
		}

		/// <summary>
		/// マネージ配列の複製
		/// </summary>
		/// <param name="src">複製元</param>
		void CopyFrom(IEnumerable<ITEM_TYPE> src);

		/// <summary>
		/// マネージ配列への変換
		/// </summary>
		/// <param name="length">要素数 [0~]</param>
		/// <returns>
		///		各要素を格納したマネージ配列を返します。
		/// </returns>
		ITEM_TYPE[] ToArray(int length);

		#endregion

		#region 型変換

		/// <summary>
		/// 型変換
		/// </summary>
		/// <returns>
		///		このオブジェクトの値を Int32 に変換して返します。
		/// </returns>
		Int32 ToInt32();

		/// <summary>
		/// 型変換
		/// </summary>
		/// <returns>
		///		このオブジェクトの値を Int64 に変換して返します。
		/// </returns>
		Int64 ToInt64();

		/// <summary>
		/// 型変換
		/// </summary>
		/// <returns>
		///		このオブジェクトの値を void* に変換して返します。
		/// </returns>
		unsafe void* ToPointer();

		/// <summary>
		/// 型変換
		/// </summary>
		/// <returns>
		///		このオブジェクトの値を文字列に変換して返します。
		/// </returns>
		string ToString();

		/// <summary>
		/// 型変換
		/// </summary>
		/// <returns>
		///		このオブジェクトの値を文字列に変換して返します。
		/// </returns>
		string ToString(string format);

		#endregion
	}
}
