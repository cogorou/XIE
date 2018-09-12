/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HKEY = System.UIntPtr;

namespace XIEversion
{
	/// <summary>
	/// RegSvr 関連
	/// </summary>
	static class AxiRegSvr
	{
		#region コンストラクタ:

		/// <summary>
		/// スタティックコンストラクタ
		/// </summary>
		static AxiRegSvr()
		{
			Files = new List<string>();

			if (System.IO.File.Exists("xie_ds_x86_100.dll"))
				Files.Add("xie_ds_x86_100.dll");

			if (Environment.Is64BitOperatingSystem)
			{
				if (System.IO.File.Exists("xie_ds_x64_100.dll"))
					Files.Add("xie_ds_x64_100.dll");
			}
		}

		#endregion

		#region 専用関数:

		/// <summary>
		/// 対象のファイル
		/// </summary>
		public static List<string> Files;

		/// <summary>
		/// レジストリ登録
		/// </summary>
		public static void Register()
		{
			Register(Files);
		}

		/// <summary>
		/// レジストリ登録解除
		/// </summary>
		public static void Remove()
		{
			Remove(Files);
		}

		#endregion

		#region 関数: RegSvr

		/// <summary>
		/// レジストリ登録
		/// </summary>
		/// <param name="files">登録対象のファイル</param>
		public static void Register(IEnumerable<string> files)
		{
			foreach (string file in files)
			{
				System.Diagnostics.Process proc = new System.Diagnostics.Process();
				proc.StartInfo.FileName = "regsvr32";
				proc.StartInfo.Arguments = string.Format("/s {0}", file);
				proc.StartInfo.CreateNoWindow = true;
				proc.Start();
				proc.WaitForExit();
			}
		}

		/// <summary>
		/// レジストリ登録解除
		/// </summary>
		/// <param name="files">解除対象のファイル</param>
		public static void Remove(IEnumerable<string> files)
		{
			foreach (string file in files)
			{
				System.Diagnostics.Process proc = new System.Diagnostics.Process();
				proc.StartInfo.FileName = "regsvr32";
				proc.StartInfo.Arguments = string.Format("/s /u {0}", file);
				proc.StartInfo.CreateNoWindow = true;
				proc.Start();
				proc.WaitForExit();
			}
		}

		#endregion
	}
}
