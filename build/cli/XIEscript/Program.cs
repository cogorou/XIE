/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Configuration;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Windows.Forms;

namespace XIEscript
{
	/// <summary>
	/// エントリポイントを持つクラス
	/// </summary>
	class Program
	{
		/// <summary>
		/// エントリポイント
		/// </summary>
		/// <param name="args"></param>
		[STAThread]
		static void Main(string[] args)
		{
			var entry_point = "";					// entry point name
			var targets = new List<string>();		// target files
			var references = new List<string>();	// reference assembly
			var arguments = new List<string>();		// arguments
			var commands = new Dictionary<string, string>();

			#region アセンブリと型の解決.
			AppDomain.CurrentDomain.AssemblyResolve += Program.AssemblyResolving;
			AppDomain.CurrentDomain.TypeResolve += Program.TypeResolving;
			#endregion

			#region 引数解析:
			foreach (string arg in args)
			{
				if (arg.StartsWith("/"))
				{
					int colon = arg.IndexOf(':');
					if (colon > 1)
					{
						string key = arg.Substring(1, colon - 1);
						string val = arg.Substring(colon + 1);
						switch (key.ToLower())
						{
							case "e":
								entry_point = val;
								break;
							case "t":
								targets.AddRange(val.Split(','));
								break;
							case "r":
								references.AddRange(val.Split(','));
								break;
							default:
								commands[key] = val;
								break;
						}
					}
					else
					{
						string key = arg.Substring(1);
						commands[key] = "";
					}
				}
				else
				{
					arguments.Add(arg);
				}
			}
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

			#region 引数チェック:
			{
				string key = null;
				if (commands.TryGetValue("gen", out key) && commands["gen"] != "")
				{
					Generate(commands["gen"]);
					return;
				}
				if (commands.TryGetValue("dep", out key) && commands["dep"] != "")
				{
					ParseDependency(commands["dep"]);
					return;
				}
				if (commands.TryGetValue("v", out key))
				{
					Version();
					return;
				}
				if (targets.Count == 0 ||
					commands.TryGetValue("?", out key) ||
					commands.TryGetValue("h", out key) ||
					commands.TryGetValue("help", out key))
				{
					Usage();
					return;
				}
			}
			#endregion

			#region コンパイル＆実行.
			try
			{
				CodeDomProvider provider = null;
				var referencedAssemblies = new List<string>();
				var compilerOptions = "/t:library /optimize";
				var providerOptions = new Dictionary<string, string>();
				var statements = new List<string>();

				#region Statements:
				foreach (string file in targets)
				{
					using (StreamReader reader = new StreamReader(file))
					{
						string statement = reader.ReadToEnd();
						statements.Add(statement);
					}
				}
				if (statements.Count == 0)
				{
					Console.WriteLine("Statements is empty.");
					return;
				}
				#endregion

				#region ReferencedAssemblies:
				{
					var asm = Assembly.GetExecutingAssembly();

					#region System
					{
						string[] assemblies =
						{
							"System.dll",
							"System.Data.dll",
							"System.Design.dll",
							"System.Drawing.dll",
							"System.Management.dll",
							"System.Windows.Forms.dll",
							"System.Xml.dll",
							"System.Linq.dll",
#if LINUX
#else
							"System.configuration.dll",
							"System.Deployment.dll",
#endif
						};
						references.AddRange(assemblies);
					}
					#endregion

					#region XIE
					try
					{
						string bindir = System.IO.Path.GetDirectoryName(asm.Location);
						if (bindir != "" && System.IO.Directory.Exists(bindir))
						{
							// dll: XIE.*.dll
							{
								string[] filenames = System.IO.Directory.GetFiles(bindir, "XIE.*.dll");
								foreach (string filename in filenames)
								{
									Assembly asm1 = Assembly.ReflectionOnlyLoadFrom(filename);
									Assembly asm2 = Assembly.Load(asm1.FullName);
									references.Add(asm2.Location);
								}
							}
						}
					}
					catch (System.Exception)
					{
					}
					#endregion

					#region additional
					referencedAssemblies.AddRange(references);
					#endregion
				}
				#endregion

				#region ProviderOptions:
				try
				{
					// (!) var を使用したいだけなので .NET 4.0 以降では指定不要です.
					if (Environment.Version.Major == 2)
					{
						Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5", false);
						object value = regkey.GetValue("Install");
						if (value is int && ((int)value) == 1)
						{
							providerOptions["CompilerVersion"] = "v3.5";
						}
					}
				}
				catch (System.Exception)
				{
				}
				#endregion

				#region ProviderSettings:
				switch (System.IO.Path.GetExtension(targets[0]))
				{
					default:
					case ".cs":
						provider = new Microsoft.CSharp.CSharpCodeProvider(providerOptions);
						compilerOptions += " /unsafe+";
						break;
					case ".vb":
						provider = new Microsoft.VisualBasic.VBCodeProvider(providerOptions);
						break;
				}
				#endregion

				#region CompilerPArameters:
				CompilerParameters param = new CompilerParameters();
				param.GenerateExecutable = false;
				param.GenerateInMemory = true;
				param.WarningLevel = 4;
				param.TreatWarningsAsErrors = false;
				param.IncludeDebugInformation = false;
				param.CompilerOptions = compilerOptions;
				param.ReferencedAssemblies.AddRange(referencedAssemblies.ToArray());
				#endregion

				// Compile
				CompilerResults results = provider.CompileAssemblyFromSource(param, statements.ToArray());

				if (results.Errors.Count == 0)
				{
					#region EntryPoint
					Type entry_type = null;
					MethodInfo entry_method = null;
					Assembly asm = results.CompiledAssembly;
					Type[] types = asm.GetTypes();
					foreach (Type type in types)
					{
						if (entry_method == null)
						{
							BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

							if (string.IsNullOrEmpty(entry_point))
							{
								if (type.Name == "Program")
								{
									MethodInfo[] methods = type.GetMethods(flags);
									foreach (MethodInfo method in methods)
									{
										if (method.Name == "Main")
										{
											entry_type = type;
											entry_method = method;
											break;
										}
									}
								}
							}
							else
							{
								if (entry_point.StartsWith(type.FullName))
								{
									MethodInfo[] methods = type.GetMethods(flags);
									foreach (MethodInfo method in methods)
									{
										if (entry_point == (type.FullName + "." + method.Name))
										{
											entry_type = type;
											entry_method = method;
											break;
										}
									}
								}
							}
						}
					}
					#endregion

					#region Execute.
					if (entry_method == null)
					{
						Console.WriteLine("Entry point not found.");
					}
					else
					{
						Binder binder = Type.DefaultBinder;
						if (entry_method.GetParameters().Length == 0)
							entry_type.InvokeMember(entry_method.Name, BindingFlags.InvokeMethod, binder, null, new object[] { });
						else
							entry_type.InvokeMember(entry_method.Name, BindingFlags.InvokeMethod, binder, null, new object[] { arguments.ToArray() });
					}
					#endregion
				}
				else
				{
					#region コンパイルエラー表示.
					foreach (CompilerError item in results.Errors)
					{
						string filename = targets[0];

						try
						{
							string[] parts = item.FileName.Split('.');
							int index = 0;
							if (parts.Length >= 3)
								index = Convert.ToInt32(parts[1]);
							if (index < targets.Count)
								filename = targets[index];
						}
						catch (System.Exception)
						{
						}

						string msg = string.Format(
							"{0}({1},{2}): {3} {4} {5}\n",
							filename,
							item.Line,
							item.Column,
							(item.IsWarning) ? "Warning" : "Error",
							item.ErrorNumber,
							item.ErrorText
							);
						Console.WriteLine(msg);
					}
					#endregion
				}
			}
			catch (System.Exception ex)
			{
				if (ex.InnerException != null)
				{
					Console.WriteLine(ex.InnerException.Message);
					Console.WriteLine(ex.InnerException.StackTrace);
				}
				else
				{
					Console.WriteLine(ex.Message);
					Console.WriteLine(ex.StackTrace);
				}
			}
			finally
			{
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
		public static Assembly AssemblyResolving(object sender, ResolveEventArgs args)
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
		public static Assembly TypeResolving(object sender, ResolveEventArgs args)
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

		#region 関数:

		/// <summary>
		/// Program.Main の生成
		/// </summary>
		/// <param name="filename">ファイル名称</param>
		static void Generate(string filename)
		{
			try
			{
				string resname = "";
				switch (System.IO.Path.GetExtension(filename))
				{
					default:
					case ".cs": resname = "XIEscript.Program_cs.txt"; break;
					case ".vb": resname = "XIEscript.Program_vb.txt"; break;
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
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// 依存関係の探索
		/// </summary>
		/// <param name="filename">ファイル名称</param>
		static void ParseDependency(string filename)
		{
			try
			{
				var asmnames = new List<AssemblyName>();

				// 引数に指定されたアセンブリをロードして参照しているアセンブリを解析します。
				string fullpath = System.IO.Path.GetFullPath(filename);
				Assembly asm = Assembly.LoadFile(fullpath);
				Console.WriteLine("{0}", asm.FullName);
				ParseDependency(fullpath, asm, asmnames, 0);
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// 依存関係の探索
		/// </summary>
		/// <param name="filename">探索元のファイル (フルパス)</param>
		/// <param name="asm">対象のアセンブリ</param>
		/// <param name="asmnames">参照アセンブリを格納する配列</param>
		/// <param name="level">階層 [0~]</param>
		static void ParseDependency(string filename, Assembly asm, List<AssemblyName> asmnames, int level)
		{
			foreach (AssemblyName item in asm.GetReferencedAssemblies())
			{
				if (0 > asmnames.FindIndex(delegate(AssemblyName element) { return (item.Name == element.Name); }))
				{
					asmnames.Add(item);
					Assembly dep = null;
					try
					{
						dep = Assembly.Load(item);
					}
					catch (System.Exception)
					{
						var dir = System.IO.Path.GetDirectoryName(filename);
						var filepath = System.IO.Path.Combine(dir, item.Name + ".dll");
						dep = Assembly.LoadFile(filepath);
					}
					for (int i = 0; i < level; i++)
						Console.Write("|");
					Console.WriteLine("+ {0}", dep.FullName);
					ParseDependency(filename, dep, asmnames, level + 1);
				}
			}
		}

		/// <summary>
		/// バージョン表示
		/// </summary>
		public static void Version()
		{
			try
			{
				#region アプリケーションのバージョン.
				{
					Console.WriteLine("[Application]");
					Assembly asm = Assembly.GetExecutingAssembly();
					FileVersionInfo info = FileVersionInfo.GetVersionInfo(asm.Location);
					Console.WriteLine("{0,-20}: {1}", "FullName", asm.FullName);
					Console.WriteLine("{0,-20}: {1}", "FileName", System.IO.Path.GetFileName(info.FileName));
					Console.WriteLine("{0,-20}: {1}", "FileDescription", info.FileDescription);
					Console.WriteLine("{0,-20}: {1}", "Comments", info.Comments);
					Console.WriteLine("{0,-20}: {1}", "FileVersion", info.FileVersion);
					Console.WriteLine("{0,-20}: {1}", "ProductVersion", info.ProductVersion);
					Console.WriteLine("{0,-20}: {1}", "ProductName", info.ProductName);
					Console.WriteLine("{0,-20}: {1}", "CompanyName", info.CompanyName);
					Console.WriteLine("{0,-20}: {1}", "LegalCopyright", info.LegalCopyright);
					Console.WriteLine("{0,-20}: {1}", "LegalTrademarks", info.LegalTrademarks);
					Console.WriteLine("{0,-20}: {1}", "Language", info.Language);
					//Console.WriteLine("{0,-20}: {1}", "InternalName", info.InternalName);
					//Console.WriteLine("{0,-20}: {1}", "OriginalFilename", info.OriginalFilename);
					//Console.WriteLine("{0,-20}: {1}", "IsDebug", info.IsDebug);
					//Console.WriteLine("{0,-20}: {1}", "IsPatched", info.IsPatched);
					//Console.WriteLine("{0,-20}: {1}", "IsPreRelease", info.IsPreRelease);
					//Console.WriteLine("{0,-20}: {1}", "IsPrivateBuild", info.IsPrivateBuild);
					//Console.WriteLine("{0,-20}: {1}", "IsSpecialBuild", info.IsSpecialBuild);
					//Console.WriteLine("{0,-20}: {1}", "PrivateBuild", info.PrivateBuild);
					//Console.WriteLine("{0,-20}: {1}", "SpecialBuild", info.SpecialBuild);
					//Console.WriteLine("{0,-20}: {1}", "FileBuildPart", info.FileBuildPart);
					//Console.WriteLine("{0,-20}: {1}", "FileMajorPart", info.FileMajorPart);
					//Console.WriteLine("{0,-20}: {1}", "FileMinorPart", info.FileMinorPart);
					//Console.WriteLine("{0,-20}: {1}", "FilePrivatePart", info.FilePrivatePart);
					//Console.WriteLine("{0,-20}: {1}", "ProductBuildPart", info.ProductBuildPart);
					//Console.WriteLine("{0,-20}: {1}", "ProductMajorPart", info.ProductMajorPart);
					//Console.WriteLine("{0,-20}: {1}", "ProductMinorPart", info.ProductMinorPart);
					//Console.WriteLine("{0,-20}: {1}", "ProductPrivatePart", info.ProductPrivatePart);
				}
				#endregion

				#region O/S、CPU、バージョン情報.
				{
					Console.WriteLine("");
					Console.WriteLine("[Environment]");
					Console.WriteLine("{0,-20}: {1} ({2})", ".NET Framework", Environment.Version, (Environment.Is64BitOperatingSystem ? "x64" : "x86"));
					Console.WriteLine("{0,-20}: {1}", "OS", Environment.OSVersion);
					try
					{
						string[] keys = 
							{
								"Name",
								"Description",
								"CurrentClockSpeed",
								"MaxClockSpeed",
								"NumberOfCores",
								"NumberOfLogicalProcessors",
							};
						ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
						ManagementObjectCollection moc = searcher.Get();
						foreach (ManagementObject obj in moc)
						{
							foreach (string key in keys)
							{
								Console.WriteLine("{0,-20}: {1}", key, obj[key]);
							}
						}
					}
					catch (System.Exception)
					{
					}
				}
				#endregion
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// 使用方法の表示
		/// </summary>
		public static void Usage()
		{
			string[] lines = 
			{
				"",
				"Usage:",
				"   XIEscript.exe [command]",
				"   XIEscript.exe [switch] [arguments]",
				"",
				"Command)",
				"   /? ... show help.",
				"   /v ... show version.",
				"   /gen:(file) ... generate a Program.Main.",
				"   /dep:(file) ... parse dependency",
				"",
				"Switch)",
				"   /t:(files) ... specify target files. [necessary]",
				"   /e:(method) ... specify an entry point method name.",
				"   /r:(assemblies) ... specify reference assembly.",
				"   /config:(file) ... specify other app.config",
				"",
				"Language)",
				"   Visual C# ... The file extension must be .cs",
				"   Visual Basic ... The file extension must be .vb",
				"",
				"1) Generate",
				"   a) XIEscript.exe /gen:Program.cs",
				"",
				"      Program.Main is generated.",
				"",
				"2) Execute",
				"   a) XIEscript.exe /t:Program.cs",
				"   b) XIEscript.exe /t:Program.cs /e:User.Hello.Start",
				"",
				"      The script is executed.",
				"      a : When has a Program.Main .",
				"      b : When does not have a Program.Main .",
				"",
				"3) Options",
				"   a) XIEscript.exe /t:Program.cs arg1 arg2 arg3",
				"   b) XIEscript.exe /t:Program.cs,Class1.cs,Class2.cs",
				"   c) XIEscript.exe /t:Program.cs /r:User1.dll,User2.dll",
				"",
				"      a : If you want to specify arguments.",
				"      b : If you want to specify more than one file.",
				"      c : If you want to specify reference assemblies.",
				"",
			};
			foreach (string line in lines)
			{
				Console.WriteLine(line);
			}
		}

		#endregion
	}
}
