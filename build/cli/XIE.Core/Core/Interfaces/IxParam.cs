/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;

namespace XIE
{
	/// <summary>
	/// パラメータの取得と設定を持つ機能のインターフェース
	/// </summary>
	public interface IxParam
	{
		/// <summary>
		/// パラメータの取得
		/// </summary>
		/// <param name="name">パラメータ名称</param>
		/// <returns>
		///		取得した値を返します。
		///	</returns>
		object GetParam(string name);

		/// <summary>
		/// パラメータの設定
		/// </summary>
		/// <param name="name">パラメータ名称</param>
		/// <param name="value">設定値</param>
		void SetParam(string name, object value);
	}

	/// <summary>
	/// 指標付きパラメータの取得と設定を持つ機能のインターフェース
	/// </summary>
	public interface IxIndexedParam
	{
		/// <summary>
		/// パラメータの取得
		/// </summary>
		/// <param name="name">パラメータ名称</param>
		/// <param name="index">指標</param>
		/// <returns>
		///		取得した値を返します。
		///	</returns>
		object GetParam(string name, int index);

		/// <summary>
		/// パラメータの設定
		/// </summary>
		/// <param name="name">パラメータ名称</param>
		/// <param name="index">指標</param>
		/// <param name="value">設定値</param>
		void SetParam(string name, int index, object value);
	}
}
