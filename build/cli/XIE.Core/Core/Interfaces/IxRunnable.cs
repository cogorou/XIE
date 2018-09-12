/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;

namespace XIE
{
	/// <summary>
	/// 非同期処理が可能な機能のインターフェース
	/// </summary>
	public interface IxRunnable
	{
		/// <summary>
		/// リセット
		/// </summary>
		void Reset();

		/// <summary>
		/// 開始
		/// </summary>
		void Start();

		/// <summary>
		/// 停止
		/// </summary>
		void Stop();

		/// <summary>
		/// 待機
		/// </summary>
		bool Wait(int timeout);

		/// <summary>
		/// 動作状態
		/// </summary>
		/// <returns>
		///		非同期処理が動作中の場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		bool IsRunning
		{
			get;
		}
	}
}
