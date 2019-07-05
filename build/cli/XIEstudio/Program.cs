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

namespace XIEstudio
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
				var progressForm = new XIEstudio.LoadingProgressForm();
				progressForm.ProcessMax = 6;
				progressForm.Show();

				// ------------------------------------------------------------
				// ライブラリの初期化.
				progressForm.UpdateUI(string.Format("Initialization ({0}/{1}) ...", progressForm.ProcessNum + 1, progressForm.ProcessMax));

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
				progressForm.UpdateUI(string.Format("Initialization ({0}/{1}) ...", progressForm.ProcessNum + 1, progressForm.ProcessMax));

				XIE.AxiTextStorage.Load("XIE.Core.XML");
				XIE.AxiTextStorage.Load("XIE.Tasks.XML");
				XIE.AxiTextStorage.Load("XIEstudio.XML");

				// ------------------------------------------------------------
				// 共有データの初期化.
				progressForm.UpdateUI(string.Format("Initialization ({0}/{1}) ...", progressForm.ProcessNum + 1, progressForm.ProcessMax));

				Setup();

				// ------------------------------------------------------------
				// タスクユニットの初期化.
				progressForm.UpdateUI(string.Format("Initialization ({0}/{1}) ...", progressForm.ProcessNum + 1, progressForm.ProcessMax));

				// タスクユニットの初期化: (ビルトイン)
				XIE.Tasks.ApiPlugin.Setup(CxTaskflowForm.TaskUnits);

				// タスクユニットの初期化: (プラグイン)
				{
					var plugins1 = LoadPlugins(Application.StartupPath);
					var plugins2 = LoadPlugins(System.IO.Directory.GetCurrentDirectory());
					SetupTaskUnits(plugins1);
					SetupTaskUnits(plugins2);
				}

				// ------------------------------------------------------------
				// 外部機器情報の初期化.
				progressForm.UpdateUI(string.Format("Initialization ({0}/{1}) ...", progressForm.ProcessNum + 1, progressForm.ProcessMax));

				SetupAuxInfo();

				// ------------------------------------------------------------
				// メインフォーム生成.
				progressForm.UpdateUI(string.Format("Initialization ({0}/{1}) ...", progressForm.ProcessNum + 1, progressForm.ProcessMax));

				System.IO.Directory.SetCurrentDirectory(XIE.Tasks.SharedData.ProjectDir);

				var form = new CxAuxInfoForm();
				form.Load +=
					delegate(object _sender, EventArgs _e)
					{
						progressForm.Close();
					};

				{
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
								using (Stream stream = asm.GetManifestResourceStream("XIEstudio.app.ico"))
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
						form.Text = "XIE-Studio";
						form.Text += " " + string.Format("{0}", (Environment.Is64BitProcess) ? "x64" : "x86");
						form.Text += " " + string.Format("({0})", ver.FileVersion);
						form.Text += " " + System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
					}
					#endregion

					#region 位置移動.
					if (progressForm != null)
					{
						form.StartPosition = FormStartPosition.Manual;
						System.Drawing.Point position = progressForm.Location;
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

					#region ウィンドウ状態の設定. (!) 位置移動の後に行うこと.
					form.WindowState = CxAuxInfoForm.AppSettings.WindowState;
					#endregion
				}

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
				#region 外部機器情報の解放.
				if (CxAuxInfoForm.AuxInfo != null)
				{
					CxAuxInfoForm.AuxInfo.Dispose();
				}
				#endregion

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
		const string AppDesc = "XIE-Studio";

		/// <summary>
		/// ファイル名のプレフィックスに使用します。
		/// </summary>
		const string AppName = "XIEstudio";

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
				CxAuxInfoForm.AppSettings = new XIEstudio.CxAppSettings();

				CxAuxInfoForm.AppSettingsFileName = System.IO.Path.Combine(XIE.Tasks.SharedData.ProjectDir, "AppSettings.xml");
				if (System.IO.File.Exists(CxAuxInfoForm.AppSettingsFileName))
					CxAuxInfoForm.AppSettings.Load(CxAuxInfoForm.AppSettingsFileName);

				CxAuxInfoForm.AppSettings.Setup();
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.StackTrace);
			}
			#endregion

			#region 画像編集フォーム設定の初期化:
			try
			{
				CxImageEditorForm.ImageEditorSettings = new XIEstudio.CxImageEditorSettings();

				CxImageEditorForm.ImageEditorSettingsFileName = System.IO.Path.Combine(XIE.Tasks.SharedData.ProjectDir, "ImageEditorSettings.xml");
				if (System.IO.File.Exists(CxImageEditorForm.ImageEditorSettingsFileName))
					CxImageEditorForm.ImageEditorSettings.Load(CxImageEditorForm.ImageEditorSettingsFileName);

				CxImageEditorForm.ImageEditorSettings.Setup();
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.StackTrace);
			}
			#endregion

			#region 参照設定の初期化:
			try
			{
				foreach (var item in CxAuxInfoForm.AppSettings.References)
				{
					XIE.Tasks.SharedData.References[item.FullName + item.HintPath] = item;
				}
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.StackTrace);
			}
			#endregion

			#region コード生成レベルの設定:
			{
				XIE.Tasks.SharedData.CodeGenerationLevel = CxAuxInfoForm.AppSettings.CodeGenerationLevel;
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
			if (string.IsNullOrEmpty(CxAuxInfoForm.AppSettingsFileName) == false)
			{
				var dir = System.IO.Path.GetDirectoryName(CxAuxInfoForm.AppSettingsFileName);
				if (System.IO.Directory.Exists(dir))
				{
					CxAuxInfoForm.AppSettings.Save(CxAuxInfoForm.AppSettingsFileName);
				}
			}
			#endregion

			#region 画像編集フォーム設定の保存:
			if (string.IsNullOrEmpty(CxImageEditorForm.ImageEditorSettingsFileName) == false)
			{
				var dir = System.IO.Path.GetDirectoryName(CxImageEditorForm.ImageEditorSettingsFileName);
				if (System.IO.Directory.Exists(dir))
				{
					CxImageEditorForm.ImageEditorSettings.Save(CxImageEditorForm.ImageEditorSettingsFileName);
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

		/// <summary>
		/// タスクユニットの初期化
		/// </summary>
		/// <param name="plugins">ロードしたプラグインのコレクション</param>
		private static void SetupTaskUnits(List<Assembly> plugins)
		{
			#region プラグインのタスクユニットの初期化:
			foreach (var plugin in plugins)
			{
				var types = plugin.GetTypes();
				foreach (var type in types)
				{
					var type_attrs = type.GetCustomAttributes(true);
					int pca = Array.FindIndex<object>(
						type_attrs,
						item => (
							(item is XIE.CxPluginClassAttribute) &&
							((XIE.CxPluginClassAttribute)item).ID == "Tasks"
						));
					if (pca >= 0)
					{
						// 2015.12.15: avast detect false plugin as a virus.
						// avast が virius として誤検出する問題が発生した.
						// ここで static に限定しなければ、method.Invoke(null, ...) が危険であると判断される為だろうか?
						//
						//var methods = type.GetMethods();	// This may be not good.
						var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
						foreach (var method in methods)
						{
							var method_attrs = method.GetCustomAttributes(true);
							int psa = Array.FindIndex<object>(
								method_attrs,
								item => (
									(item is XIE.CxPluginSetupAttribute) &&
									((XIE.CxPluginSetupAttribute)item).ID == "Tasks"
								));
							if (psa >= 0)
							{
								method.Invoke(null, new object[] { CxTaskflowForm.TaskUnits });
							}
						}
					}
				}
			}
			#endregion
		}

		/// <summary>
		/// 外部機器情報の初期化
		/// </summary>
		private static void SetupAuxInfo()
		{
			#region 外部機器情報の初期化:
			try
			{
				CxAuxInfoForm.AuxInfo = new XIE.Tasks.CxAuxInfo();
				CxAuxInfoForm.AuxInfoFileName = "AuxInfo.xml";

				string filename = System.IO.Path.Combine(XIE.Tasks.SharedData.ProjectDir, CxAuxInfoForm.AuxInfoFileName);
				if (System.IO.File.Exists(filename))
					CxAuxInfoForm.AuxInfo.Load(filename);

				CxAuxInfoForm.AuxInfo.ProjectDir = XIE.Tasks.SharedData.ProjectDir;
				CxAuxInfoForm.AuxInfo.DebugMode = CxAuxInfoForm.AppSettings.DebugMode;
				CxAuxInfoForm.AuxInfo.Setup();

				XIE.Tasks.SharedData.Icons16 = (XIE.Tasks.IxAuxImageList16)CxAuxInfoForm.AuxInfo;
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.StackTrace);
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
}
