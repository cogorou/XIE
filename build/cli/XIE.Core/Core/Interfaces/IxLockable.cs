/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;

namespace XIE
{
	/// <summary>
	/// メモリロックが可能な機能のインターフェース
	/// </summary>
	public interface IxLockable
	{
		/// <summary>
		/// ロック
		/// </summary>
		void Lock();

		/// <summary>
		/// ロック解除
		/// </summary>
		void Unlock();

		/// <summary>
		/// ロック状態
		/// </summary>
		bool IsLocked
		{
			get;
		}
	}
}
