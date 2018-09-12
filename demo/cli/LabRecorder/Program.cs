/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;

namespace demo
{
	partial class Program
	{
		/// <summary>
		/// エントリポイント
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			try
			{
				XIE.Axi.Setup();

				if (Directory.Exists(Results) == false)
					Directory.CreateDirectory(Results);

				test01();
			}
			catch (System.Exception ex)
			{
				Console.WriteLine("{0}", ex.StackTrace);
			}
			finally
			{
				XIE.Axi.TearDown();
			}
		}

		/// <summary>
		/// 処理結果ディレクトリ
		/// </summary>
		static string Results = "Results";

		#region test01: CxScreenCapture

		/// <summary>
		/// CxDeviceList
		/// </summary>
		static void test01()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			using (var list = new XIE.Media.CxScreenList())
			{
				// ウィンドウリストの取得:
				list.Setup();

				// ウィンドウの選択:
				int length = list.Length;
				for (int i = 0; i < length; i++)
				{
					var item = list[i];
					Console.WriteLine("{0,2}: {1}: {2} [{3},{4}-{5},{6}]"
						, i
						, item.Handle.ToString("X8")
						, item.Name
						, item.Bounds.X
						, item.Bounds.Y
						, item.Bounds.Width
						, item.Bounds.Height
						);
				}
				Console.WriteLine("{0,2}: exit", length);
				Console.Write("input number > ");
				var ans = Console.ReadLine();
				var index = int.Parse(ans);

				if (0 <= index && index < length)
				{
					CaptureWindow(list[index]);
				}
			}
		}

		/// <summary>
		/// ウィンドウのキャプチャ
		/// </summary>
		/// <param name="info"></param>
		static void CaptureWindow(XIE.Media.CxScreenListItem info)
		{
			var watch = new XIE.CxStopwatch();
			var stat = new XIE.TxStatistics();
			var output_file = new XIE.CxStringA(string.Format("Results/capture.avi"));

			var controller = new XIE.Media.CxScreenCapture();
			controller.Setup(info, null, output_file);

			Console.WriteLine("{0} fps", controller.FrameRate);

			// 描画処理: (※必須でない)
			var grabber = controller.CreateGrabber();
			grabber.Notify +=
				(object sender, XIE.Media.CxGrabberArgs e) =>
				{
					watch.Stop();
					stat += watch.Lap;
				};
			grabber.Reset();
			grabber.Start();

			watch.Start();
			{
				// キャプチャ開始:
				controller.Start();

				// 待機:
				grabber.Wait(10 * 1000);	// 10sec
				grabber.Stop();

				// キャプチャ停止:
				controller.Stop();
			}
			watch.Stop();

			Console.WriteLine("Count   = {0,9:F0}", stat.Count);
			Console.WriteLine("Mean    = {0,9:F3}", stat.Mean);
			Console.WriteLine("Sigma   = {0,9:F3}", stat.Sigma);
			Console.WriteLine("Min     = {0,9:F3}", stat.Min);
			Console.WriteLine("Max     = {0,9:F3}", stat.Max);
			Console.WriteLine("Elapsed = {0,9:F3}", watch.Elapsed);
		}


		#endregion
	}
}
