/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;

namespace XIE
{
	/// <summary>
	/// ファイルへの保存または読み込みを行うインターフェース
	/// </summary>
	public interface IxFileAccess
	{
		/// <summary>
		/// ファイルの読み込み
		/// </summary>
		/// <param name="filename">ファイル名称</param>
		/// <param name="options">オプション。</param>
		void Load(string filename, params object[] options);

		/// <summary>
		/// ファイルへの保存
		/// </summary>
		/// <param name="filename">ファイル名称</param>
		/// <param name="options">オプション。</param>
		void Save(string filename, params object[] options);
	}
}
