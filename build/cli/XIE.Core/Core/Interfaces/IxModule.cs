/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;

namespace XIE
{
	/// <summary>
	/// XIE モジュールインターフェース
	/// </summary>
	public interface IxModule
	{
		/// <summary>
		/// ハンドルの取得
		/// </summary>
		/// <returns>
		///		保有しているハンドルを返します。
		/// </returns>
		HxModule GetHandle();

		/// <summary>
		/// ハンドルの設定
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		void SetHandle(HxModule handle, bool disposable);

		/// <summary>
		/// モジュールの破棄
		/// </summary>
		void Destroy();
	}
}
