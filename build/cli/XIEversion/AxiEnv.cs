/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace XIEversion
{
	/// <summary>
	/// 環境変数関連
	/// </summary>
	static class AxiEnv
	{
		#region コンストラクタ

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

		#region 定数

		/// <summary>
		/// 環境変数のベースキー
		/// </summary>
		/// <remarks>
		///		HKLM : "SYSTEM\CurrentControlSet\Control\Session Manager\Environment"
		///		HKCU : "Environment"
		///		seealso: OpenSubKey
		/// </remarks>
		public static string BaseKey = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";

		#endregion

		#region オブジェクト

		/// <summary>
		/// 環境変数名 ([ProductName][ProductVersion]ROOT)
		/// </summary>
		public static string ProductROOTVariable = "";

		/// <summary>
		/// 環境変数名 ([ProductName][ProductVersion]BIN)
		/// </summary>
		public static string ProductBINVariable = "";

		/// <summary>
		/// 環境変数名 ([ProductName][ProductVersion]ASM)
		/// </summary>
		public static string ProductASMVariable = "";

		#endregion

		#region プロパティ

		/// <summary>
		/// PATH 環境変数
		/// </summary>
		public static string PATH
		{
			get
			{
				Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, false);
				object value = key.GetValue("PATH", null, Microsoft.Win32.RegistryValueOptions.DoNotExpandEnvironmentNames);
				return value as string;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, true);
					if (key != null)
						key.SetValue("PATH", value, Microsoft.Win32.RegistryValueKind.ExpandString);
				}
			}
		}

		/// <summary>
		/// 環境変数の値 ([ProductName][ProductVersion]ROOT)
		/// </summary>
		public static string ProductROOT
		{
			get
			{
				Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, false);
				object value = key.GetValue(ProductROOTVariable);
				if (value is string)
					return value.ToString();
				return "";
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, true);
					if (key != null)
					{
						if (key.GetValue(ProductROOTVariable) != null)
							key.DeleteValue(ProductROOTVariable);
					}
				}
				else
				{
					Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, true);
					if (key != null)
						key.SetValue(ProductROOTVariable, value, Microsoft.Win32.RegistryValueKind.String);
				}
			}
		}

		/// <summary>
		/// 環境変数の値 ([ProductName][ProductVersion]BIN)
		/// </summary>
		public static string ProductBIN
		{
			get
			{
				Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, false);
				object value = key.GetValue(ProductBINVariable);
				if (value is string)
					return value.ToString();
				return "";
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, true);
					if (key != null)
					{
						if (key.GetValue(ProductBINVariable) != null)
							key.DeleteValue(ProductBINVariable);
					}
				}
				else
				{
					Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, true);
					if (key != null)
						key.SetValue(ProductBINVariable, value, Microsoft.Win32.RegistryValueKind.String);
				}
			}
		}

		/// <summary>
		/// 環境変数の値 ([ProductName][ProductVersion]ASM)
		/// </summary>
		public static string ProductASM
		{
			get
			{
				Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, false);
				object value = key.GetValue(ProductASMVariable);
				if (value is string)
					return value.ToString();
				return "";
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, true);
					if (key != null)
					{
						if (key.GetValue(ProductASMVariable) != null)
							key.DeleteValue(ProductASMVariable);
					}
				}
				else
				{
					Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, true);
					if (key != null)
						key.SetValue(ProductASMVariable, value, Microsoft.Win32.RegistryValueKind.String);
				}
			}
		}

		/// <summary>
		/// OMP_NUM_THREADS 環境変数
		/// </summary>
		public static int OMP_NUM_THREADS
		{
			get
			{
				Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, false);
				object value = key.GetValue("OMP_NUM_THREADS");
				if (value != null)
					return int.Parse(value.ToString());
				return 0;
			}
			set
			{
				Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("OMP_NUM_THREADS", true);
				key.SetValue("OMP_NUM_THREADS", value.ToString(), Microsoft.Win32.RegistryValueKind.String);
			}
		}

		/// <summary>
		/// KMP_DUPLICATE_LIB_OK 環境変数
		/// </summary>
		public static bool KMP_DUPLICATE_LIB_OK
		{
			get
			{
				Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, false);
				object value = key.GetValue("KMP_DUPLICATE_LIB_OK");
				if (value != null)
					return bool.Parse(value.ToString());
				return false;
			}
			set
			{
				Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("KMP_DUPLICATE_LIB_OK", true);
				key.SetValue("KMP_DUPLICATE_LIB_OK", (value ? "TRUE" : "FALSE"), Microsoft.Win32.RegistryValueKind.String);
			}
		}

		#endregion

		#region 関数.

		/// <summary>
		/// インストール
		/// </summary>
		public static void Register()
		{
			string dir = System.IO.Directory.GetCurrentDirectory();
			string dir_root = System.IO.Directory.GetParent(dir).FullName;
			string dir_bin = dir;
			string dir_asm = dir;

			// 環境変数.
			AxiEnv.ProductROOT = dir_root;
			AxiEnv.ProductBIN = dir_bin;
			AxiEnv.ProductASM = dir_asm;

			#region PATH 環境変数.
			try
			{
				string path = AxiEnv.PATH;
				string name_bin = "%" + AxiEnv.ProductBINVariable + "%";
				//string name_asm = "%" + AxiEnv.ProductASMVariable + "%";

				bool updated = false;
				if (path.Length > 0 && path.EndsWith(";") == false)
					path = path + ";";
				if (path.Contains(name_bin) == false)
				{
					path = path + name_bin + ";";
					updated = true;
				}
				//if (path.Contains(name_asm) == false)
				//{
				//    path = path + name_asm + ";";
				//    updated = true;
				//}
				if (updated)
					AxiEnv.PATH = path;
			}
			catch (System.Exception)
			{
			}
			#endregion
		}

		/// <summary>
		/// アンインストール
		/// </summary>
		/// <param name="level">0:なし、1:PATH 削除、2:すべて削除</param>
		public static void Remove(int level)
		{
			if (level >= 1)
			{
				#region PATH 環境変数.
				try
				{
					string path = AxiEnv.PATH;
					if (path != null)
					{
						string name_bin = "%" + AxiEnv.ProductBINVariable + "%";
						string name_asm = "%" + AxiEnv.ProductASMVariable + "%";
						if (path.Contains(name_bin) ||
							path.Contains(name_asm))
						{
							string[] items = path.Split(new char[] { ';' });
							StringBuilder temp = new StringBuilder();
							foreach (string item in items)
							{
								string _item = item.Trim();
								if (_item == "") continue;
								if (_item == name_bin) continue;
								if (_item == name_asm) continue;
								temp.Append(_item + ";");
							}

							if (temp.Length > 0)
							{
								AxiEnv.PATH = temp.ToString();
								temp.Length = 0;
							}
						}
					}
				}
				catch (System.Exception)
				{
				}
				#endregion
			}

			if (level >= 2)
			{
				// 環境変数.
				AxiEnv.ProductROOT = "";
				AxiEnv.ProductBIN = "";
				AxiEnv.ProductASM = "";
			}
		}

		/// <summary>
		/// 環境変数の削除
		/// </summary>
		/// <param name="value">環境変数名</param>
		public static void DeleteVariable(string value)
		{
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(BaseKey, true);
			if (key != null)
				key.DeleteValue(value);
		}

		/// <summary>
		/// レジストリへの変更を反映します
		/// </summary>
		public static void Flush()
		{
			uint result = 0;
			int ret = SendMessageTimeout((IntPtr)HWND_BROADCAST, WM_WININICHANGE, 0, "Environment", SMTO_ABORTIFHUNG, 100, ref result);
			if (0 == ret)
			{
				uint errorcode = GetLastError();
				System.Text.StringBuilder message = new StringBuilder(255);
				FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, (uint)errorcode, 0, message, message.Capacity, IntPtr.Zero);
			}
		}

		/// <summary>
		/// リダイレクションの有効/無効
		/// </summary>
		/// <param name="enable">true=有効、false=無効</param>
		public static void EnableRedirect(bool enable)
		{
			try
			{
				Wow64EnableWow64FsRedirection(enable);
			}
			catch (System.Exception)
			{
			}
		}

		/// <summary>
		/// Wow64 か否かを検証します。
		/// </summary>
		/// <returns>
		///		wow64 で動作している場合は true、それ以外は false を返します。
		/// </returns>
		public static bool IsWow64()
		{
			try
			{
				IntPtr hProcess = GetCurrentProcess();
				bool wow64 = false;
				IsWow64Process(hProcess, ref wow64);
				return wow64;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region Win32

		private const int WM_WININICHANGE = 0x001A;
		private const int HWND_BROADCAST = 0xFFFF;

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, string lParam);

		[DllImport("user32.dll")]
		private static extern int SendNotifyMessage(IntPtr hWnd, uint msg, int wParam, string lParam);

		[DllImport("user32.dll")]
		private static extern int PostMessage(IntPtr hWnd, uint msg, int wParam, string lParam);

		[DllImport("user32.dll")]
		private static extern int SendMessageTimeout(IntPtr hWnd, uint msg, int wParam, string lParam, uint fuFlags, uint uTimeout, ref uint lpdwResult);

		/// <summary>
		/// 受信側プロセスがハングアップ状態であると判断した場合、タイムアウト期間の経過を待たずに制御を返す.
		/// </summary>
		private const int SMTO_ABORTIFHUNG = 0x2;

		[DllImport("kernel32.dll")]
		private static extern uint GetLastError();

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetCurrentProcess();

		/// <summary>
		/// システムからメッセージを取得する.
		/// </summary>
		private const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

		[DllImport("kernel32.dll")]
		private static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr arguments);

		[DllImport("kernel32.dll")]
		private static extern bool Wow64EnableWow64FsRedirection(bool enable);

		[DllImport("kernel32.dll")]
		private static extern bool IsWow64Process(IntPtr hProcess, ref bool wow64);

		#endregion
	}
}
