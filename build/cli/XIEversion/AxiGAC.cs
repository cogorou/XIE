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
	/// GAC 関連
	/// </summary>
	/// <remarks>
	/// <b>参考:</b><br/>
	/// http://dobon.net/vb/dotnet/deployment/installgac.html <br/>
	/// http://support.microsoft.com/kb/315682/ja <br/>
	/// http://dobon.net/vb/dotnet/system/registrykey.html <br/>
	/// </remarks>
	static class AxiGAC
	{
		#region コンストラクタ:

		/// <summary>
		/// スタティックコンストラクタ
		/// </summary>
		static AxiGAC()
		{
			if (Environment.Is64BitProcess)
			{
				AssemblyFoldersExKey = string.Format(
					@"SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v{0}.{1}.{2}\AssemblyFoldersEx",
					System.Environment.Version.Major,
					System.Environment.Version.Minor,
					System.Environment.Version.Build
				);
			}
			else
			{
				AssemblyFoldersExKey = string.Format(
					@"SOFTWARE\Microsoft\.NETFramework\v{0}.{1}.{2}\AssemblyFoldersEx",
					System.Environment.Version.Major,
					System.Environment.Version.Minor,
					System.Environment.Version.Build
				);
			}

			Assembly asm = Assembly.GetExecutingAssembly();
			FileVersionInfo info = FileVersionInfo.GetVersionInfo(asm.Location);
			AssemblyFoldersExPrivateKey = string.Format(
				"{0} {1}.{2}",
				info.ProductName,
				info.ProductMajorPart,
				info.ProductMinorPart
			);
		}

		#endregion

		#region 関数: AssemblyFoldersEx

		/// <summary>
		/// アセンブリが格納されているディレクトリパスを示すレジストリ
		/// </summary>
		private static string AssemblyFoldersExKey = "";

		/// <summary>
		/// アセンブリが格納されているディレクトリパスを示すレジストリ配下のプライベートキー
		/// </summary>
		private static string AssemblyFoldersExPrivateKey = "";

		/// <summary>
		/// アセンブリが格納されているディレクトリパス
		/// </summary>
		/// <exception cref="T:System.Exception">
		///		失敗した場合は、Microsoft.Win32.RegistryKey クラスの CreateSubKey 関数が例外を発行します。
		///	</exception>
		/// <remarks>
		///		指定されたディレクトリをレジストリに登録します。<br/>
		///		アセンブリを GAC に登録しただけでは、[参照の追加]ダイアログには表示されないので、
		///		アセンブリが格納されたディレクトリをレジストリに登録する必要があります。<br/>
		/// </remarks>
		/// <example>
		///	<b>登録要領:</b><br/>
		///	下記のキーに独自のキーを作成して、既定の文字列データに
		/// 自社製品のアセンブリが登録されたディレクトリを記述します。
		///	<pre><tt>
		///	BASE   : HKEY_LOCAL_MACHINE/SOFTWARE/Microsoft/.NETFramework/v[Major].[Minor].[Build]/AssemblyFoldersEx
		///	PRIVATE: └ (Vendor) (Product) [Major].[Minor].[Build]
		///	　         │名前    │種類  │データ                                  │
		///	　         ├────┼───┼────────────────────┤
		///	　         │(既定)  │REG_SZ│C:\Program Files\(Vendor)\(Product)\bin │
		///	</tt></pre>
		/// </example>
		public static string AssemblyFoldersEx
		{
			get
			{
				string subkey = System.IO.Path.Combine(AssemblyFoldersExKey, AssemblyFoldersExPrivateKey);
				using (Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(subkey))
				{
					if (regkey == null)
						return "";
					object name = regkey.GetValue("");
					if (name is string)
						return (string)name;
					return "";
				}
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					string subkey = System.IO.Path.Combine(AssemblyFoldersExKey, AssemblyFoldersExPrivateKey);
					try
					{
						Microsoft.Win32.Registry.LocalMachine.DeleteSubKey(subkey);
					}
					catch (System.Exception)
					{
						// キーが存在しない場合の例外は無視する.
					}
				}
				else
				{
					string subkey = System.IO.Path.Combine(AssemblyFoldersExKey, AssemblyFoldersExPrivateKey);
					Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(subkey);
					if (regkey != null)
						regkey.SetValue("", value);
				}
			}
		}

		#endregion

		#region 関数: GAC

		/// <summary>
		/// GAC 登録
		/// </summary>
		/// <param name="files">登録対象のファイル</param>
		/// <exception cref="T:System.Exception">
		///		失敗した場合は、System.EnterpriseServices.Internal.Publish クラスの GacInstall 関数が例外を発行します。
		///	</exception>
		/// <remarks>
		///		指定されたファイルを GAC に登録します。
		/// </remarks>
		public static void Register(IEnumerable<string> files)
		{
			System.EnterpriseServices.Internal.Publish pub = new System.EnterpriseServices.Internal.Publish();
			foreach (string file in files)
				pub.GacInstall(file);
		}

		/// <summary>
		/// GAC 登録解除
		/// </summary>
		/// <param name="files">解除対象のファイル</param>
		/// <exception cref="T:System.Exception">
		///		失敗した場合は、System.EnterpriseServices.Internal.Publish クラスの GacRemove 関数が例外を発行します。
		///	</exception>
		/// <remarks>
		///		指定されたファイルが GAC に登録されてあれば登録を解除します。
		/// </remarks>
		public static void Remove(IEnumerable<string> files)
		{
			System.EnterpriseServices.Internal.Publish pub = new System.EnterpriseServices.Internal.Publish();
			foreach (string file in files)
				pub.GacRemove(file);
		}

		#endregion

		#region 関数: Framework Version

		/// <summary>
		/// .NET Framework のバージョンの取得 (文字列)
		/// </summary>
		/// <param name="major">メジャーバージョン</param>
		/// <param name="minor">マイナーバージョン</param>
		/// <returns>
		///		バージョン文字列 (major.minor (build)) を返します。
		/// </returns>
		/// <remarks>
		///		下記レジストリの Version キーと Release キーを参照します。<br/>
		///		HKLM\SOFTWARE\\Microsoft\\NET Framework Setup\\NDP <br/>
		///		<br/>
		///		.NET Framework 4.5 以降の環境では Release キーの値から判定して
		///		形成したバージョン文字列 (major.minor (biuld) ) を返します。
		///		それ以外の環境ではバージョン文字列 (Version キーの値) をそのまま返します。
		///		<br/>
		///		Release キーの値の判定は、下記サイトに記載された最小 Release 番号との比較で行います。<br/>
		///		<br/>
		///		How to: Determine Which .NET Framework Versions Are Installed <br/>
		///		https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed <br/>
		///		<br/>
		///		※注) この関数は v4.0 Client Profile は考慮していません。
		/// </remarks>
		public static string GetFrameworkVersion(int major, int minor)
		{
			string base_key = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP";
			if (major == 2) base_key += "\\v2.0.50727";
			else if (major == 3 && minor == 0) base_key += "\\v3.0";
			else if (major == 3 && minor == 5) base_key += "\\v3.5";
			else if (major == 4) base_key += "\\v4\\Full";
			else return "";

			using (Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(base_key))
			{
				if (regkey != null)
				{
					// Install
					{
						object install = regkey.GetValue("Install");
						if (install is int)
						{
							if (((int)install) == 0) return "";
						}
					}
					// Version
					if (major < 4)
					{
						object version = regkey.GetValue("Version");
						if (version is string)
						{
							return (string)version;
						}
					}
					else
					{
						var version_items = new List<string>();
						object version = regkey.GetValue("Version");
						if (version is string)
							version_items.AddRange(((string)version).Split('.'));

						string version_str = "";
						if (version_items.Count < 3)
							version_str = (version is string) ? (string)version : "unkown";
						else
						{
							#region Release キーの値の判定:
							object ans = regkey.GetValue("Release");
							if (ans != null)
							{
								int version_minor = Convert.ToInt32(version_items[1]);

								do
								{
									var release = Convert.ToUInt32(ans);
									if (release >= NET4_Release.v472)
									{
										if (version_minor != 7) break;
										version_str = string.Format("4.7.2 ({0})", release);
										break;
									}
									if (release >= NET4_Release.v471)
									{
										if (version_minor != 7) break;
										version_str = string.Format("4.7.1 ({0})", release);
										break;
									}
									if (release >= NET4_Release.v470)
									{
										if (version_minor != 7) break;
										version_str = string.Format("4.7 ({0})", release);
										break;
									}
									if (release >= NET4_Release.v462)
									{
										if (version_minor != 6) break;
										version_str = string.Format("4.6.2 ({0})", release);
										break;
									}
									if (release >= NET4_Release.v461)
									{
										if (version_minor != 6) break;
										version_str = string.Format("4.6.1 ({0})", release);
										break;
									}
									if (release >= NET4_Release.v460)
									{
										if (version_minor != 6) break;
										version_str = string.Format("4.6 ({0})", release);
										break;
									}
									if (release >= NET4_Release.v452)
									{
										if (version_minor != 5) break;
										version_str = string.Format("4.5.2 ({0})", release);
										break;
									}
									if (release >= NET4_Release.v451)
									{
										if (version_minor != 5) break;
										version_str = string.Format("4.5.1 ({0})", release);
										break;
									}
									if (release >= NET4_Release.v450)
									{
										if (version_minor != 5) break;
										version_str = string.Format("4.5 ({0})", release);
										break;
									}
								}
								while (false);
							}
							#endregion

							// Version をそのまま使用する.
							if (string.IsNullOrEmpty(version_str))
								version_str = (string)version;
						}
						return version_str;
					}
				}
				return "";
			}
		}

		#endregion
	}

	/// <summary>
	/// .NET Framework 4.5 以降の Release 番号
	/// </summary>
	/// <remarks>
	///		seealso: <br/>
	///		How to: Determine Which .NET Framework Versions Are Installed <br/>
	///		https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed <br/>
	/// </remarks>
	struct NET4_Release
	{
		/// <summary>
		/// .NET 4.5   の 最小の Release 番号
		/// </summary>
		public const uint v450 = 378389;

		/// <summary>
		/// .NET 4.5.1 の 最小の Release 番号
		/// </summary>
		public const uint v451 = 378575;

		/// <summary>
		/// .NET 4.5.2 の 最小の Release 番号
		/// </summary>
		public const uint v452 = 379893;

		/// <summary>
		/// .NET 4.6   の 最小の Release 番号
		/// </summary>
		public const uint v460 = 393295;

		/// <summary>
		/// .NET 4.6.1 の 最小の Release 番号
		/// </summary>
		public const uint v461 = 394254;

		/// <summary>
		/// .NET 4.6.2 の 最小の Release 番号
		/// </summary>
		public const uint v462 = 394802;

		/// <summary>
		/// .NET 4.7 の 最小の Release 番号
		/// </summary>
		public const uint v470 = 460798;

		/// <summary>
		/// .NET 4.7.1 の 最小の Release 番号
		/// </summary>
		public const uint v471 = 461308;

		/// <summary>
		/// .NET 4.7.2 の 最小の Release 番号
		/// </summary>
		public const uint v472 = 461808;
	}
}
