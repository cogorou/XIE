/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using System.Configuration;
using System.Runtime.InteropServices;

namespace XIEcapture
{
	/// <summary>
	/// エントリポイントを持つクラス
	/// </summary>
	static class Program
	{
		/// <summary>
		/// エントリポイント
		/// </summary>
		/// <param name="args">コマンドライン引数</param>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			#region アセンブリと型の解決:
			ResolveEventHandler asm_resolving = new ResolveEventHandler(AssemblyResolving);
			ResolveEventHandler type_resolving = new ResolveEventHandler(TypeResolving);

			AppDomain.CurrentDomain.AssemblyResolve += asm_resolving;
			AppDomain.CurrentDomain.TypeResolve += type_resolving;
			#endregion

			#region 引数解析:
			var commands = ParseArguments(args);
			#endregion

			#region 引数に指定された構成ファイルの読み込み:
			{
				string filename = "";
				if (commands.TryGetValue("config", out filename))
				{
					if (string.IsNullOrWhiteSpace(filename) == false &&
						System.IO.File.Exists(filename) == true)
					{
						var fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = filename };
						var userLevel = ConfigurationUserLevel.None;
						var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, userLevel);

						foreach (KeyValueConfigurationElement item in config.AppSettings.Settings)
						{
							ConfigurationManager.AppSettings[item.Key] = item.Value;
						}
					}
				}
			}
			#endregion

			#region カルチャの設定:
			if (ConfigurationManager.AppSettings["Culture"] is string)
			{
				string culture = ConfigurationManager.AppSettings["Culture"].Trim();
				if (culture.ToLower() != "(default)")
				{
					System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
				}
			}
			#endregion

			#region 起動までの一連の処理:
			try
			{
				// ------------------------------------------------------------
				// ライブラリの初期化.

				#region トレースレベル:
				if (ConfigurationManager.AppSettings["TraceLevel"] is string)
				{
					int level = int.Parse(ConfigurationManager.AppSettings["TraceLevel"]);
					XIE.Axi.TraceLevel(level);
				}
				#endregion

				XIE.Axi.Setup();

				// ------------------------------------------------------------
				// インテリセンスファイルの読み込み.

				XIE.AxiTextStorage.Load("XIE.Core.XML");
				XIE.AxiTextStorage.Load("XIEcapture.XML");

				// ------------------------------------------------------------
				// 共有データの初期化.

				Setup();

				// ------------------------------------------------------------
				// メインフォーム生成.

				System.IO.Directory.SetCurrentDirectory(XIEcapture.SharedData.ProjectDir);

				var form = new CxCaptureForm();

				#region アイコン設定.
				switch (System.Environment.OSVersion.Platform)
				{
					case PlatformID.Unix:
						break;
					default:
						{
							Assembly asm = Assembly.GetExecutingAssembly();
							using (Stream stream = asm.GetManifestResourceStream("XIEcapture.app.ico"))
							{
								form.Icon = new System.Drawing.Icon(stream);
							}
						}
						break;
				}
				#endregion

				#region タイトル表示.
				{
					var ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
					form.Text = "XIE-Capture";
					form.Text += " " + string.Format("{0}", (Environment.Is64BitProcess) ? "x64" : "x86");
					form.Text += " " + string.Format("({0})", ver.FileVersion);
					form.Text += " " + System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
				}
				#endregion

				#region 位置移動.
				{
					form.StartPosition = FormStartPosition.Manual;
					System.Drawing.Point position = Form.MousePosition;
					foreach (Screen item in Screen.AllScreens)
					{
						if (item.Bounds.Left <= position.X && position.X <= item.Bounds.Right &&
							item.Bounds.Top <= position.Y && position.Y <= item.Bounds.Bottom)
						{
							form.Visible = false;
							int diff_x = (item.Bounds.Width - form.Width) / 2;
							int diff_y = (item.Bounds.Height - form.Height) / 2;
							int x = item.Bounds.X + ((diff_x < 0) ? 0 : diff_x);
							int y = item.Bounds.Y + ((diff_y < 0) ? 0 : diff_y);
							form.Location = new System.Drawing.Point(x, y);
							break;
						}
					}
				}
				#endregion

				// メインフォーム起動.
				Application.Run(form);
			}
			catch (System.Exception ex)
			{
				#region メッセージ通知:
				{
					// 異常終了時のスタックとレース保存.
					string logfile = Path.GetFileNameWithoutExtension(Application.ExecutablePath) + "-Error.log";
					StreamWriter sw = new StreamWriter(logfile);
					sw.WriteLine(ex.ToString());

					StackTrace st = new StackTrace(true);
					for (int i = 0; i < st.FrameCount; i++)
					{
						StackFrame sf = st.GetFrame(i);
						sw.WriteLine(string.Format("High up the call stack, Method: {0}", sf.GetMethod()));
						sw.WriteLine(string.Format("High up the call stack, Line Number: {0}", sf.GetFileLineNumber()));
					}
					sw.Close();

					// メッセージ通知.
					string title = string.Format("Exception occured. See also {0}.", logfile);
					MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				#endregion
			}
			finally
			{
				// 共有データの解放.
				TearDown();
			}
			#endregion
		}

		#region AppDomain 関連: (アセンブリまたは型の解決)

		/// <summary>
		/// デシリアライズ時にアセンブリの解決が失敗したときに発生します。(seealso:AppDomain.CurrentDomain.AssemblyResolve)
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="args">アセンブリ名</param>
		/// <returns>
		///		指定されたアセンブリ名に該当するアセンブリを返します。
		/// </returns>
		static Assembly AssemblyResolving(object sender, ResolveEventArgs args)
		{
			string name = args.Name.Split(',')[0];
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (name == assembly.FullName.Split(',')[0])
					return assembly;
			}
			return null;
		}

		/// <summary>
		/// Type.GetType() で型の解決が失敗したときに発生します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		/// <returns>
		///		見つかったアセンブリを返します。
		/// </returns>
		/// <remarks>
		/// この処理は、下記で示されている方法とは異なります。
		/// <![CDATA[
		/// http://msdn.microsoft.com/ja-jp/library/system.appdomain.typeresolve(v=vs.80).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-4
		/// ]]>
		/// </remarks>
		static Assembly TypeResolving(object sender, ResolveEventArgs args)
		{
			string name = args.Name.Split(',')[0];
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (name == type.FullName)
						return assembly;
				}
			}
			return null;
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// 初期化
		/// </summary>
		static void Setup()
		{
			// アプリケーション情報.
			string appname = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);

			// 構成ファイルの読み込み.
			string project_dir = "";
			if (ConfigurationManager.AppSettings["ProjectDir"] is string)
				project_dir = (string)ConfigurationManager.AppSettings["ProjectDir"];

			// 構成ファイルから読み込んだ情報の編集.
			if (string.IsNullOrWhiteSpace(project_dir) == false)
			{
				#region 特殊フォルダ.
				System.Environment.SpecialFolder[] folders =
				{
					System.Environment.SpecialFolder.MyDocuments,
					System.Environment.SpecialFolder.MyMusic,
					System.Environment.SpecialFolder.MyPictures,
					System.Environment.SpecialFolder.MyVideos,
					System.Environment.SpecialFolder.MyComputer,
				};

				foreach (var folder in folders)
				{
					string label = string.Format("({0})", folder);
					if (string.Compare(project_dir, label, true) == 0)
					{
						string description = fvi.FileDescription.Trim();
						if (description == "")
							description = "XIE-Capture";
						string base_dir = System.Environment.GetFolderPath(folder);
						string home_dir = string.Format("{0}-{1}.{2}", description, fvi.FileMajorPart, fvi.FileMinorPart);
						project_dir = System.IO.Path.Combine(base_dir, home_dir);
						if (System.IO.Directory.Exists(project_dir) == false)
							System.IO.Directory.CreateDirectory(project_dir);
						break;
					}
				}
				#endregion
			}

			#region プロジェクトディレクトリの設定:
			if (System.IO.Directory.Exists(project_dir))
			{
				XIEcapture.SharedData.ProjectDir = System.IO.Path.GetFullPath(project_dir);
			}
			else
			{
				XIEcapture.SharedData.ProjectDir = System.IO.Directory.GetCurrentDirectory();
			}
			#endregion

			#region アプリケーション設定の初期化:
			try
			{
				XIEcapture.CxCaptureForm.AppSettings = new XIEcapture.CxAppSettings();

				XIEcapture.CxCaptureForm.AppSettingsFileName = System.IO.Path.Combine(XIEcapture.SharedData.ProjectDir, "AppSettings.xml");
				if (System.IO.File.Exists(XIEcapture.CxCaptureForm.AppSettingsFileName))
					XIEcapture.CxCaptureForm.AppSettings.Load(XIEcapture.CxCaptureForm.AppSettingsFileName);
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.StackTrace);
			}
			#endregion
		}

		/// <summary>
		/// 解放
		/// </summary>
		static void TearDown()
		{
			#region アプリケーション設定の保存:
			if (string.IsNullOrEmpty(XIEcapture.CxCaptureForm.AppSettingsFileName) == false)
			{
				var dir = System.IO.Path.GetDirectoryName(XIEcapture.CxCaptureForm.AppSettingsFileName);
				if (System.IO.Directory.Exists(dir))
				{
					XIEcapture.CxCaptureForm.AppSettings.Save(XIEcapture.CxCaptureForm.AppSettingsFileName);
				}
			}
			#endregion
		}

		#endregion

		#region 引数解析:

		/// <summary>
		/// パラメータ
		/// </summary>
		static List<string> Parameters = new List<string>();

		/// <summary>
		/// 引数解析
		/// </summary>
		/// <param name="args">コマンドライン引数</param>
		/// <returns>
		///		解析結果を返します。
		/// </returns>
		private static Dictionary<string, string> ParseArguments(string[] args)
		{
			var commands = new Dictionary<string, string>();

			foreach (string arg in args)
			{
				if (arg.StartsWith("/"))
				{
					int colon = arg.IndexOf(':');
					if (colon > 1)
					{
						string key = arg.Substring(1, colon - 1);
						string val = arg.Substring(colon + 1);
						commands[key] = val;
					}
					else
					{
						string key = arg.Substring(1);
						commands[key] = "";
					}
				}
				else
				{
					Parameters.Add(arg);
				}
			}

			return commands;
		}

		#endregion
	}

	/// <summary>
	/// 共有データ
	/// </summary>
	static class SharedData
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

		#endregion
	}
}
