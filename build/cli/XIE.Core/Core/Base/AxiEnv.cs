/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Diagnostics;
using XIE.Ptr;

namespace XIE
{
	/// <summary>
	/// 補助関数群 (環境変数関連)
	/// </summary>
	public static partial class AxiEnv : System.Object
	{
		#region コンストラクタ: 

		/// <summary>
		/// スタティックコンストラクタ
		/// </summary>
		static AxiEnv()
		{
			System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
			System.Diagnostics.FileVersionInfo verinfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(asm.Location);
			string product = verinfo.ProductName;
			string version = string.Format("{0}{1:00}", verinfo.ProductMajorPart, verinfo.ProductMinorPart);
			ProductROOTVariable = string.Format("{0}{1}ROOT", product, version);
			ProductBINVariable = string.Format("{0}{1}BIN", product, version);
			ProductASMVariable = string.Format("{0}{1}ASM", product, version);
		}

		#endregion

		#region 定数:

		/// <summary>
		/// 環境変数のベースキー
		/// </summary>
		/// <remarks>
		///		HKLM : "SYSTEM\CurrentControlSet\Control\Session Manager\Environment"
		///		HKCU : "Environment"
		///		seealso: OpenSubKey
		/// </remarks>
		public const string BaseKey = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";

		#endregion

		#region オブジェクト:

		/// <summary>
		/// 環境変数名 ([ProductName][ProductVersion]ROOT)
		/// </summary>
		public static string ProductROOTVariable
		{
			get { return m_ProductROOTVariable; }
			private set { m_ProductROOTVariable = value; }
		}
		private static string m_ProductROOTVariable = "";

		/// <summary>
		/// 環境変数名 ([ProductName][ProductVersion]BIN)
		/// </summary>
		public static string ProductBINVariable
		{
			get { return m_ProductBINVariable; }
			private set { m_ProductBINVariable = value; }
		}
		private static string m_ProductBINVariable = "";

		/// <summary>
		/// 環境変数名 ([ProductName][ProductVersion]ASM)
		/// </summary>
		public static string ProductASMVariable
		{
			get { return m_ProductASMVariable; }
			private set { m_ProductASMVariable = value; }
		}
		private static string m_ProductASMVariable = "";

		#endregion

		#region プロパティ

		/// <summary>
		/// 環境変数の値 ([ProductName][ProductVersion]ROOT)
		/// </summary>
		public static string ProductROOT
		{
			get
			{
#if LINUX
#else
				//Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, false);
				//object value = key.GetValue(ProductROOTVariable);
				//if (value is string)
				//	return value.ToString();
#endif
				//return "";
				return System.Environment.GetEnvironmentVariable(ProductROOTVariable);
			}
		}

		/// <summary>
		/// 環境変数の値 ([ProductName][ProductVersion]BIN)
		/// </summary>
		public static string ProductBIN
		{
			get
			{
#if LINUX
#else
				//Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, false);
				//object value = key.GetValue(ProductBINVariable);
				//if (value is string)
				//	return value.ToString();
#endif
				//return "";
				return System.Environment.GetEnvironmentVariable(ProductBINVariable);
			}
		}

		/// <summary>
		/// 環境変数の値 ([ProductName][ProductVersion]ASM)
		/// </summary>
		public static string ProductASM
		{
			get
			{
#if LINUX
#else
				//Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, false);
				//object value = key.GetValue(ProductASMVariable);
				//if (value is string)
				//	return value.ToString();
#endif
				//return "";
				return System.Environment.GetEnvironmentVariable(ProductASMVariable);
			}
		}

		#endregion
	}
}
