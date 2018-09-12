/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;

namespace XIE.Media
{
	/// <summary>
	/// メディアの非同期処理を操作するインターフェース
	/// </summary>
	public interface IxMediaControl : IxRunnable
	{
		/// <summary>
		/// 中断
		/// </summary>
		void Abort();

		/// <summary>
		/// 一時停止
		/// </summary>
		void Pause();

		/// <summary>
		/// 一時停止状態
		/// </summary>
		/// <returns>
		///		非同期処理が一時停止中の場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		bool IsPaused
		{
			get;
		}
	}
}
