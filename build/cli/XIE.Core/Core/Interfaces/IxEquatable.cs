/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;

namespace XIE
{
	/// <summary>
	/// オブジェクトの内容を比較するインターフェース
	/// </summary>
	public interface IxEquatable
	{
		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		void CopyFrom(object src);

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		bool ContentEquals(object src);
	}

	/// <summary>
	/// オブジェクトの内容を複製するインターフェース
	/// </summary>
	public interface IxConvertible
	{
		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="dst">複製先</param>
		void CopyTo(object dst);
	}
}
