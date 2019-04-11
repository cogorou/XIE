/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace XIE.Tasks
{
	/// <summary>
	/// 共有データ
	/// </summary>
	public static class SharedData
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		static SharedData()
		{
		}

		#endregion

		#region オブジェクト:

		/// <summary>
		/// プロジェクトディレクトリ
		/// </summary>
		public static string ProjectDir = "";

		/// <summary>
		/// 一時ファイルの格納場所
		/// </summary>
		public static string TempDir
		{
			get
			{
				if (string.IsNullOrEmpty(ProjectDir))
					return System.IO.Path.GetFullPath("TempFiles");
				else
					return System.IO.Path.Combine(ProjectDir, "TempFiles");
			}
		}

		/// <summary>
		/// 参照設定
		/// </summary>
		public static Dictionary<string, CxReferencedAssembly> References = new Dictionary<string, CxReferencedAssembly>();

		/// <summary>
		/// 16x16 アイコンのコレクション
		/// </summary>
		public static IxAuxImageList16 Icons16 = null;

		/// <summary>
		/// コード生成レベル [0:最小限、1:詳細]
		/// </summary>
		public static int CodeGenerationLevel = 0;

		#endregion
	}
}
