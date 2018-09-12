/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;

namespace XIE
{
	/// <summary>
	/// 自身が保有するオブジェクトを有効化するインターフェース
	/// </summary>
	public interface IxValidatable
	{
		/// <summary>
		/// 有効化
		/// </summary>
		void Validate();
	}

	/// <summary>
	/// 指定されたオブジェクトを有効化するインターフェース
	/// </summary>
	public interface IxValidator
	{
		/// <summary>
		/// 有効化
		/// </summary>
		/// <param name="target">処理対象</param>
		void Validate(object target);
	}
}
