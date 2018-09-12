/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration;

namespace XIEversion
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		/// <param name="args">コマンド ライン引数</param>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm());
			}
			else
			{
				AttachConsole(uint.MaxValue);
				CommandRegister(args);
				FreeConsole();
				AxiEnv.Flush();
			}
		}

		#region Win32

		/// <summary>
		/// コンソールの生成
		/// </summary>
		/// <returns>
		/// </returns>
		[DllImport("kernel32")]
		static extern bool AllocConsole();

		/// <summary>
		/// コンソールへのアタッチ
		/// </summary>
		/// <returns>
		/// </returns>
		[DllImport("kernel32.dll")]
		public static extern bool AttachConsole(uint dwProcessId);

		/// <summary>
		/// コンソールの開放
		/// </summary>
		/// <returns>
		/// </returns>
		[DllImport("kernel32.dll")]
		public static extern bool FreeConsole();

		/// <summary>
		/// システムディレクトリの取得
		/// </summary>
		/// <param name="sysDirBuffer"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern int GetSystemDirectory(StringBuilder sysDirBuffer, int size);

		#endregion

		#region コマンドライン実行

		/// <summary>
		/// コマンド ライン引数指定による GAC 登録
		/// </summary>
		/// <param name="args">コマンド ライン引数</param>
		static void CommandRegister(string[] args)
		{
			bool install_mode = true;
			List<string> filenames = new List<string>();
			foreach (string item in args)
			{
				if (item == "/i") install_mode = true;
				else if (item == "/u") install_mode = false;
				else
					filenames.Add(item);
			}

			string dir = System.IO.Directory.GetCurrentDirectory();

			if (install_mode)
			{
				Console.WriteLine("Installing ...");
				
				if (filenames.Count <= 0)
				{
					filenames.AddRange(System.IO.Directory.GetFiles(dir, "XIE.*.dll"));

					// AssemblyFoldersEx
					AxiGAC.AssemblyFoldersEx = dir;

					// GAC 登録.
					AxiGAC.Register(filenames);

					// RegSvr 登録.
					AxiRegSvr.Register();

					// 環境変数.
					AxiEnv.Register();
				}
				else
				{
					// GAC 登録.
					AxiGAC.Register(filenames);
				}

				Console.WriteLine("completed.");
			}
			else
			{
				Console.WriteLine("Uninstalling ...");
				
				if (filenames.Count <= 0)
				{
					filenames.AddRange(System.IO.Directory.GetFiles(dir, "XIE.*.dll"));

					// AssemblyFoldersEx
					AxiGAC.AssemblyFoldersEx = "";

					// GAC 登録解除.
					AxiGAC.Remove(filenames);

					// RegSvr 登録解除.
					AxiRegSvr.Remove();

					// 環境変数.
					AxiEnv.Remove(2);
				}
				else
				{
					// GAC 登録解除.
					AxiGAC.Remove(filenames);
				}

				Console.WriteLine("completed.");
			}
		}

		#endregion
	}
}
