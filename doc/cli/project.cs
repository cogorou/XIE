/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#region アセンブリ情報:
// アセンブリに関する一般情報は以下の属性セットをとおして制御されます。
// アセンブリに関連付けられている情報を変更するには、
// これらの属性値を変更してください。
[assembly: AssemblyTitle("sample")]
[assembly: AssemblyDescription("sample")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Eggs Imaging Laboratory")]
[assembly: AssemblyProduct("XIE")]
[assembly: AssemblyCopyright("Copyright (C) 2013 Eggs Imaging Laboratory")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// ComVisible を false に設定すると、その型はこのアセンブリ内で COM コンポーネントから 
// 参照不可能になります。COM からこのアセンブリ内の型にアクセスする場合は、
// その型の ComVisible 属性を true に設定してください。
[assembly: ComVisible(false)]

// 次の GUID は、このプロジェクトが COM に公開される場合の、typelib の ID です
[assembly: Guid("197c87c8-3dc0-4d99-bb28-4c0881a290d1")]

// アセンブリのバージョン情報は、以下の 4 つの値で構成されています:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// すべての値を指定するか、下のように '*' を使ってビルドおよびリビジョン番号を 
// 既定値にすることができます:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
#endregion

namespace User
{
	class Program
	{
		/// <summary>
		/// エントリポイント
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			XIE.Axi.Setup();

			string result_dir = System.IO.Path.GetFullPath("Results");
			if (!System.IO.Directory.Exists(result_dir))
				System.IO.Directory.CreateDirectory(result_dir);

			Execute(args);

			XIE.Axi.TearDown();
		}

		#region Execute:

		/// <summary>
		/// 実行
		/// </summary>
		/// <param name="args"></param>
		static void Execute(string[] args)
		{
			int success = 0;
			int failure = 0;
			double total_time = 0;
			List<TestResult> results = new List<TestResult>();

			Assembly asm = Assembly.GetExecutingAssembly();

			#region クラス情報の抽出.
			SortedList<string, Type> types = new SortedList<string, Type>();
			{
				Type[] types1 = asm.GetTypes();
				foreach (Type type in types1)
					types[type.FullName] = type;
			}
			#endregion

			foreach (KeyValuePair<string, Type> types_item in types)
			{
				Type type = types_item.Value;

				// [CxPluginClass] 属性が付加されたクラスか否かの検査.
				object[] attrs = type.GetCustomAttributes(typeof(XIE.CxPluginClassAttribute), true);
				if (attrs.Length == 0) continue;

				SortedList<string, MethodInfo> prepares = new SortedList<string, MethodInfo>();
				SortedList<string, MethodInfo> executes = new SortedList<string, MethodInfo>();
				SortedList<string, MethodInfo> restores = new SortedList<string, MethodInfo>();

				#region メソッド情報の抽出.
				MethodInfo[] methods = type.GetMethods();
				foreach (MethodInfo method in methods)
				{
					// ターゲット指定.
					if (args.Length > 0 && Contains(type.FullName + "." + method.Name, args) == false) continue;

					// [CxPluginSetup] 属性が付加された関数.
					attrs = method.GetCustomAttributes(typeof(XIE.CxPluginSetupAttribute), true);
					if (attrs.Length > 0)
						prepares.Add(type.FullName + "." + method.Name, method);

					// [CxPluginExecute] 属性が付加された関数.
					attrs = method.GetCustomAttributes(typeof(XIE.CxPluginExecuteAttribute), true);
					if (attrs.Length > 0)
						executes.Add(type.FullName + "." + method.Name, method);

					// [CxPluginTearDown] 属性が付加された関数.
					attrs = method.GetCustomAttributes(typeof(XIE.CxPluginTearDownAttribute), true);
					if (attrs.Length > 0)
						restores.Add(type.FullName + "." + method.Name, method);
				}
				#endregion

				object instance = Activator.CreateInstance(type);

				#region 処理前の準備.
				foreach (KeyValuePair<string, MethodInfo> item in prepares)
				{
					MethodInfo method = item.Value;
					try
					{
						method.Invoke(instance, null);
					}
					catch (System.Exception ex)
					{
						if (ex.InnerException != null)
							Console.WriteLine("[ERR] {0}: {1}", item.Key, ex.InnerException.Message);
						else
							Console.WriteLine("[ERR] {0}: {1}", item.Key, ex.Message);
					}
				}
				#endregion

				#region 処理.
				foreach (KeyValuePair<string, MethodInfo> item in executes)
				{
					MethodInfo method = item.Value;
					try
					{
						Console.WriteLine(item.Key);
						Stopwatch watch = new Stopwatch();
						watch.Start();
						method.Invoke(instance, null);
						watch.Stop();
						Console.WriteLine("{0}: {1:0.000} msec", item.Key, watch.ElapsedMilliseconds);
						success++;
						total_time += watch.ElapsedMilliseconds;
						results.Add(new TestResult(item.Key, watch.ElapsedMilliseconds, ""));
					}
					catch (System.Exception ex)
					{
						if (ex.InnerException != null)
						{
							Console.WriteLine("[ERR] {0}: {1}", item.Key, ex.InnerException.Message);
							results.Add(new TestResult(item.Key, 0.0, ex.InnerException.Message));
						}
						else
						{
							Console.WriteLine("[ERR] {0}: {1}", item.Key, ex.Message);
							results.Add(new TestResult(item.Key, 0.0, ex.Message));
						}
						failure++;
					}
				}
				#endregion

				#region 処理後の復元.
				foreach (KeyValuePair<string, MethodInfo> item in restores)
				{
					MethodInfo method = item.Value;
					try
					{
						method.Invoke(instance, null);
					}
					catch (System.Exception ex)
					{
						if (ex.InnerException != null)
							Console.WriteLine("[ERR] {0}: {1}", item.Key, ex.InnerException.Message);
						else
							Console.WriteLine("[ERR] {0}: {1}", item.Key, ex.Message);
					}
				}
				#endregion
			}

			Console.WriteLine("==================================================");
			for (int i = 0; i < results.Count; i++)
			{
				Console.WriteLine("{0,-70}: {1,9:0.000} msec {2}", results[i].Name, results[i].Time, results[i].Message);
			}
			{
				Console.WriteLine("{0,-70}: {1,9:0.000} msec", "Total time", total_time);
				Console.WriteLine("success: {0}", success);
				Console.WriteLine("failure: {0}", failure);
			}
		}

		/// <summary>
		/// 検索
		/// </summary>
		/// <param name="target"></param>
		/// <param name="values"></param>
		static bool Contains(string target, string[] values)
		{
			string key = target.ToLower();
			foreach (string item in values)
			{
				if (key.Contains(item.ToLower()))
					return true;
			}
			return false;
		}

		#endregion
	}

	#region TestResult 構造体:

	/// <summary>
	/// テスト結果
	/// </summary>
	struct TestResult
	{
		/// <summary>
		/// 名称
		/// </summary>
		public string Name;
		/// <summary>
		/// 処理時間(msec)
		/// </summary>
		public double Time;
		/// <summary>
		/// エラーメッセージ
		/// </summary>
		public string Message;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="time"></param>
		/// <param name="message"></param>
		public TestResult(string name, double time, string message)
		{
			Name = name;
			Time = time;
			Message = message;
		}
	}

	#endregion

	#region Examples クラス:

	[XIE.CxPluginClass]
	partial class Examples
	{
	}

	#endregion
}
