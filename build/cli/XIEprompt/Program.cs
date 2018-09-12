/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using System.Configuration;
using System.Runtime.InteropServices;

namespace XIEprompt
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		/// <param name="args">コマンドライン引数</param>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			#region アセンブリと型の解決.
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
				XIE.AxiTextStorage.Load("XIE.Tasks.XML");
				XIE.AxiTextStorage.Load("XIEprompt.XML");

				// ------------------------------------------------------------
				// 共有データの初期化.

				Setup();

				// ------------------------------------------------------------
				// プラグインのロード.

				{
					var plugins1 = LoadPlugins(Application.StartupPath);
					var plugins2 = LoadPlugins(System.IO.Directory.GetCurrentDirectory());
				}

				// ------------------------------------------------------------
				// メインフォーム生成.

				System.IO.Directory.SetCurrentDirectory(XIE.Tasks.SharedData.ProjectDir);

				var form = new CxPromptForm();

				#region アイコン設定.
				// 現象)
				//   デザイナでアイコンを指定した場合、Linux Mono 環境では下記の例外が発生する.
				//   System.ArgumentException: A null reference or invalid value was found [GDI+ status: InvalidParameter]
				// 原因)
				//   CxAuxInfoForm 初期化処理の途中(下記)でリソースの取得に失敗している模様。
				//   this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
				// 対策)
				//   Linux Mono 環境ではフォームのアイコンを設定しない。
				//   実行時に判定する為、Application のリソースに埋め込まれたアイコンを利用する。
				//                                     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
				//   ※注1) app.ico のビルドアクションを "埋め込まれたリソース" にしなければ GetManifestResourceStream で例外が発生する。
				//   ※注2) GetManifestResourceStream に渡す引数には "既定の名前空間" と "アイコンファイル名" を指定する。
				// 
				switch (System.Environment.OSVersion.Platform)
				{
					case PlatformID.Unix:
						// 
						// 上記の理由によりアイコンの設定は行わない.
						// 
						break;
					default:
						{
							Assembly asm = Assembly.GetExecutingAssembly();
							using (Stream stream = asm.GetManifestResourceStream("XIEprompt.app.ico"))
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
					form.Text = "XIE-Prompt";
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

				#region 引数に指定された値の反映:
				foreach (var item in commands)
				{
					switch (item.Key.ToLower())
					{
						case "e":
							form.EntryPoint = item.Value;
							break;
						case "t":
							form.TargetFiles.AddRange(item.Value.Split(','));
							break;
						case "log":
							form.LogFile = item.Value;
							break;
					}
				}
				form.Parameters = Parameters;
				#endregion

				// メインフォーム起動.
				Application.Run(form);
			}
			catch (System.Exception ex)
			{
				#region メッセージ通知.
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

		#region AppDomain 関連. (アセンブリまたは型の解決)

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
		/// ディレクトリ名のプレフィックスに使用します。
		/// </summary>
		const string AppDesc = "XIE-Prompt";

		/// <summary>
		/// ファイル名のプレフィックスに使用します。
		/// </summary>
		const string AppName = "XIEprompt";

		/// <summary>
		/// 初期化
		/// </summary>
		static void Setup()
		{
			// アプリケーション情報.
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
						string base_dir = System.Environment.GetFolderPath(folder);
						string home_dir = string.Format("{0}-{1}.{2}", AppDesc, fvi.FileMajorPart, fvi.FileMinorPart);
						project_dir = System.IO.Path.Combine(base_dir, home_dir);
						if (System.IO.Directory.Exists(project_dir) == false)
							System.IO.Directory.CreateDirectory(project_dir);
						break;
					}
				}
				#endregion
			}

			#region プロジェクトディレクトリの設定:
			if (string.IsNullOrWhiteSpace(project_dir) == true ||
				System.IO.Directory.Exists(project_dir) == false)
			{
				XIE.Tasks.SharedData.ProjectDir = System.IO.Directory.GetCurrentDirectory();
			}
			else
			{
				XIE.Tasks.SharedData.ProjectDir = System.IO.Path.GetFullPath(project_dir);
			}
			#endregion

			#region アプリケーション設定の初期化:
			try
			{
				CxPromptForm.AppSettings = new CxAppSettings();

				CxPromptForm.AppSettingsFileName = System.IO.Path.Combine(XIE.Tasks.SharedData.ProjectDir, "AppSettings.xml");
				if (System.IO.File.Exists(CxPromptForm.AppSettingsFileName))
					CxPromptForm.AppSettings.Load(CxPromptForm.AppSettingsFileName);

				CxPromptForm.AppSettings.Setup();
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.StackTrace);
			}
			#endregion

			#region 参照設定の初期化:
			try
			{
				foreach (var item in CxPromptForm.AppSettings.References)
				{
					XIE.Tasks.SharedData.References[item.FullName + item.HintPath] = item;
				}
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.StackTrace);
			}
			#endregion

			RemoveTempFiles();
		}

		/// <summary>
		/// 解放
		/// </summary>
		static void TearDown()
		{
			#region アプリケーション設定の保存:
			if (string.IsNullOrEmpty(CxPromptForm.AppSettingsFileName) == false)
			{
				var dir = System.IO.Path.GetDirectoryName(CxPromptForm.AppSettingsFileName);
				if (System.IO.Directory.Exists(dir))
				{
					CxPromptForm.AppSettings.Save(CxPromptForm.AppSettingsFileName);
				}
			}
			#endregion

			RemoveTempFiles();
		}

		/// <summary>
		/// 一時ファイルの削除
		/// </summary>
		private static void RemoveTempFiles()
		{
			#region 一時ファイルの削除:
			try
			{
				string tempDir = XIE.Tasks.SharedData.TempDir;
				if (System.IO.Directory.Exists(tempDir))
				{
					string[] filenames = System.IO.Directory.GetFiles(tempDir);
					foreach (var filename in filenames)
					{
						try
						{
							System.IO.File.Delete(filename);
						}
						catch (System.Exception)
						{
						}
					}
				}
			}
			catch (System.Exception)
			{
			}
			#endregion
		}

		/// <summary>
		/// プラグインのロード
		/// </summary>
		/// <param name="dir">検索するディレクトリ</param>
		/// <returns>
		///		ロードしたプラグインのコレクションを返します。
		/// </returns>
		private static List<Assembly> LoadPlugins(string dir)
		{
			var plugins = new List<Assembly>();
			var files = System.IO.Directory.GetFiles(dir, "*.plugin");

			#region プラグインの初期化:
			foreach (var file in files)
			{
				try
				{
					var info = new XIE.Tasks.CxReferencedAssembly();
					info.Load(file);

					Assembly asm = null;

					// 厳密名があれば優先的に使用する.
					if (string.IsNullOrWhiteSpace(info.FullName) == false)
					{
						try
						{
							asm = Assembly.Load(info.FullName);
						}
						catch (System.Exception)
						{
							// 一時的に GAC 登録解除している可能性を考慮する.
						}
					}

					// ファイル名でロードを試みる.
					if (asm == null)
					{
						var assemblyFile = System.IO.Path.GetFileNameWithoutExtension(file);
						var assemblyPath = "";
						if (string.IsNullOrWhiteSpace(info.HintPath) == false)
							assemblyPath = System.IO.Path.GetFullPath(info.HintPath);
						else
							assemblyPath = System.IO.Path.Combine(dir, assemblyFile);

						if (System.IO.File.Exists(assemblyPath))
						{
							asm = Assembly.LoadFrom(assemblyPath);
						}
					}

					// 何れか一方で正常にロードされていればコレクションに追加する.
					if (asm != null)
					{
						plugins.Add(asm);

						XIE.Tasks.SharedData.References[info.FullName + info.HintPath] = info;
					}
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message);
					XIE.Log.Api.Error("Assembly loading failed. file is {0}", file);
				}
			}
			#endregion

			return plugins;
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

		/// <summary>
		/// 使用方法の表示
		/// </summary>
		private static void Usage()
		{
			string[] lines = 
			{
				"",
				"Usage:",
				"   XIEprompt.exe [command]",
				"   XIEprompt.exe [switch] [arguments]",
				"",
				"Command)",
				"   /? ... show help.",
				"   /v ... show version.",
				"   /gen:(file) ... generate a Program.Main.",
				"   /dep:(file) ... parse dependency",
				"   /plugin:(assemblyFilename) ... create a plugin file",
				"",
				"Switch)",
				"   /t:(files) ... specify target files. [necessary]",
				"   /e:(method) ... specify an entry point method name.",
				"   /log:(file) ... output to specify a file.",
				"",
				"Language)",
				"   Visual C# ... The file extension must be .cs",
				"   Visual Basic ... The file extension must be .vb",
				"",
				"1) Generate",
				"   a) XIEprompt.exe /gen:Program.cs",
				"",
				"      Program.Main is generated.",
				"",
				"2) Execute",
				"   a) XIEprompt.exe /t:Program.cs",
				"   b) XIEprompt.exe /t:Program.cs /e:User.Hello.Start",
				"",
				"      The script is executed.",
				"      a : When has a Program.Main .",
				"      b : When does not have a Program.Main .",
				"",
				"3) Options",
				"   a) XIEprompt.exe /t:Program.cs arg1 arg2 arg3",
				"   b) XIEprompt.exe /t:Program.cs,Class1.cs,Class2.cs",
				"",
				"      a : If you want to specify arguments.",
				"      b : If you want to specify more than one file.",
				"",
			};

			foreach (string line in lines)
			{
				Console.WriteLine(line);
			}
		}

		/// <summary>
		/// Program.Main の生成
		/// </summary>
		/// <param name="filename">ファイル名称</param>
		private static void Generate(string filename)
		{
			try
			{
				string resname = "";
				switch (System.IO.Path.GetExtension(filename))
				{
					default:
					case ".cs": resname = "XIEprompt.Program_cs.txt"; break;
					case ".vb": resname = "XIEprompt.Program_vb.txt"; break;
				}

				Assembly asm = Assembly.GetExecutingAssembly();
				using (Stream stream = asm.GetManifestResourceStream(resname))
				{
					using (StreamReader reader = new StreamReader(stream))
					using (StreamWriter writer = new StreamWriter(System.IO.Path.GetFullPath(filename)))
					{
						writer.Write(reader.ReadToEnd());
					}
				}

				Console.WriteLine("{0} was generated.", filename);
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// プラグインファイルの生成
		/// </summary>
		/// <param name="filename">アセンブリファイル名</param>
		private static void CreatePluginFile(string filename)
		{
			try
			{
				var info = new XIE.Tasks.CxReferencedAssembly();
				if (System.IO.File.Exists(filename))
				{
					try
					{
						var asm = Assembly.LoadFrom(filename);
						info.FullName = asm.FullName;
					}
					catch (System.Exception)
					{
					}

					info.HintPath = System.IO.Path.GetFullPath(filename);
				}

				var plugin_file = string.Format("{0}.plugin", filename);
				info.Save(plugin_file);

				Console.WriteLine("{0} was created.", plugin_file);
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		#endregion
	}

	/// <summary>
	/// 補助関数
	/// </summary>
	static class ApiHelper
	{
		#region マウス付近の座標.

		/// <summary>
		/// ウィンドウがスクリーンに収まるように考慮したマウス付近の座標を計算します。
		/// </summary>
		/// <param name="size">ウィンドウのサイズ</param>
		/// <param name="mode">モード [0:TopLeft, 1:Center]</param>
		/// <returns>
		///		計算した結果を返します。
		/// </returns>
		public static System.Drawing.Point GetNeighborPosition(System.Drawing.Size size, int mode = 0)
		{
			return GetNeighborPosition(size.Width, size.Height, mode);
		}

		/// <summary>
		/// ウィンドウがスクリーンに収まるように考慮したマウス付近の座標を計算します。
		/// </summary>
		/// <param name="width">ウィンドウ幅</param>
		/// <param name="height">ウィンドウ高さ</param>
		/// <param name="mode">モード [0:TopLeft, 1:Center]</param>
		/// <returns>
		///		計算した結果を返します。
		/// </returns>
		public static System.Drawing.Point GetNeighborPosition(int width, int height, int mode = 0)
		{
			var pos = Control.MousePosition;
			var screen = Screen.FromPoint(Control.MousePosition).Bounds;
			switch (mode)
			{
				default:
				case 0:
					break;
				case 1:
					pos.X -= (width / 2);
					pos.Y -= (height / 2);
					break;
			}
			if (pos.X > (screen.Right - 72) - width)
				pos.X = (screen.Right - 72) - width;
			if (pos.Y > (screen.Bottom - 72) - height)
				pos.Y = (screen.Bottom - 72) - height;
			if (pos.X < screen.Left)
				pos.X = screen.Left;
			if (pos.Y < screen.Top)
				pos.Y = screen.Top;
			return pos;
		}

		#endregion

		#region ファイル名関連:

		/// <summary>
		/// クラス名のサフィックス(年月日_時分秒)の作成
		/// </summary>
		/// <param name="timestamp">ファイル名のサフィックスにするタイムスタンプ</param>
		/// <param name="use_msec">ミリ秒も付加するか否か</param>
		/// <returns>
		///		生成したサフィックスを返します。
		/// </returns>
		public static string MakeClassNameSuffix(DateTime timestamp, bool use_msec)
		{
			string suffix = string.Format("{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}",
				timestamp.Year, timestamp.Month, timestamp.Day,
				timestamp.Hour, timestamp.Minute, timestamp.Second);
			if (use_msec)
				suffix += "_" + timestamp.Millisecond.ToString("000");
			return suffix;
		}

		/// <summary>
		/// ファイル名のサフィックス(年月日_時分秒)の作成
		/// </summary>
		/// <param name="timestamp">ファイル名のサフィックスにするタイムスタンプ</param>
		/// <param name="use_msec">ミリ秒も付加するか否か</param>
		/// <returns>
		///		生成したサフィックスを返します。
		/// </returns>
		public static string MakeFileNameSuffix(DateTime timestamp, bool use_msec)
		{
			string suffix = string.Format("{0:0000}{1:00}{2:00}-{3:00}{4:00}{5:00}",
				timestamp.Year, timestamp.Month, timestamp.Day,
				timestamp.Hour, timestamp.Minute, timestamp.Second);
			if (use_msec)
				suffix += "-" + timestamp.Millisecond.ToString("000");
			return suffix;
		}

		/// <summary>
		/// 指定されたファイル名に無効な文字があれば指定文字に置き換えます。
		/// </summary>
		/// <param name="filename">検査対象のファイル名</param>
		/// <param name="substitute">代用する文字</param>
		/// <returns>
		///		検査後のファイル名を返します。
		/// </returns>
		public static string MakeValidFileName(string filename, string substitute)
		{
			char[] invalids = System.IO.Path.GetInvalidFileNameChars();
			foreach (char item in invalids)
			{
				string invalid = string.Format("{0}", item);
				if (filename.Contains(invalid))
					filename = filename.Replace(invalid, substitute);
			}
			return filename;
		}

		#endregion

		#region コンパイラ関連:

		/// <summary>
		/// 参照設定の生成
		/// </summary>
		/// <returns>
		///		参照設定を初期化して返します。
		/// </returns>
		public static XIE.Tasks.CxReferencedAssembly[] CreateReferences()
		{
			var result = new List<XIE.Tasks.CxReferencedAssembly>();
			var assemblies = new List<Assembly>();
			assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());
			assemblies.Sort((item1, item2) => { return item1.FullName.CompareTo(item2.FullName); });
			foreach (var asm in assemblies)
			{
				var name = asm.FullName.Split(',')[0];
				switch (name)
				{
					case "System":
					case "System.Core":
					case "System.Drawing":
					case "System.Windows.Forms":
					case "System.Xml":
					case "System.Xml.Linq":
					case "XIE.Core":
					case "XIE.Tasks":
						result.Add(new XIE.Tasks.CxReferencedAssembly(asm.FullName, ""));
						break;
				}
			}
			return result.ToArray();
		}

		/// <summary>
		/// 名前空間設定の生成
		/// </summary>
		/// <returns>
		///		名前空間設定を初期化して返します。
		/// </returns>
		public static string[] CreateImports()
		{
			return new string[]
			{
				"System",
				"System.Collections.Generic",
				"System.ComponentModel",
				"System.Drawing",
				"System.Text",
				"System.Windows.Forms",
				"System.Reflection",
				"System.IO",
				"System.Xml",
				"System.Threading",
				"System.Diagnostics",
			};
		}

		#endregion
	}
}
