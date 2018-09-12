/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;

namespace XIE
{
	/// <summary>
	/// 外部リソースへのアタッチが可能な機能のインターフェース
	/// </summary>
	public interface IxAttachable
	{
		/// <summary>
		/// アタッチ
		/// </summary>
		/// <param name="src">アタッチ先</param>
		void Attach(object src);

		/// <summary>
		/// アタッチ状態
		/// </summary>
		bool IsAttached
		{
			get;
		}
	}
}
