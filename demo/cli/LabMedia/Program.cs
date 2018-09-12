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
				test11();
				test12();
				test13();
				test21();
				test22();
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
		/// テストファイルディレクトリ
		/// </summary>
		static string TestFiles = "TestFiles";

		/// <summary>
		/// 処理結果ディレクトリ
		/// </summary>
		static string Results = "Results";

		#region test01: CxDeviceList

		/// <summary>
		/// CxDeviceList
		/// </summary>
		static void test01()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			// --------------------------------------------------
			// A) ビデオ入力デバイスの列挙.
			try
			{
				Console.WriteLine("----------------------------------------");
				var devices = new XIE.Media.CxDeviceList();
				Console.WriteLine("{0}: Video Input Device", __FUNCTION__);
				devices.Setup(XIE.Media.ExMediaType.Video, XIE.Media.ExMediaDir.Input);
				int length = devices.Length;
				Console.WriteLine("{0,-20} : {1}", "Length", length);
				for(int i=0 ; i<length ; i++)
				{
					var item = devices[i];
					Console.WriteLine("|- {0}", i);
					Console.WriteLine("|  |- {0,-20} : \"{1}\" ({2})", "Name(Index)", item.Name, item.Index);
					Console.WriteLine("|  |- {0,-20} : \"{1}\" ", "ProductName", item.GetProductName());

					var pins = item.GetPinNames();
					Console.WriteLine("|  |- {0,-20} : %d", "PinNames", pins.Length);
					for (int s = 0; s < pins.Length; s++)
					{
						Console.WriteLine("|  |  |- {0,2}: \"{1}\"", s, pins[s]);
					}

					var sizes = item.GetFrameSizes();
					Console.WriteLine("|  |- {0,-20} : %d", "FrameSizes", sizes.Length);
					for(int s=0 ; s<sizes.Length ; s++)
					{
						Console.WriteLine("|  |  |- {0,2}: {1,4} x {2,4}", s, sizes[s].Width, sizes[s].Height);
					}
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}

			// --------------------------------------------------
			// B) オーディオ入力デバイスの列挙.
			try
			{
				Console.WriteLine("----------------------------------------");
				var devices = new XIE.Media.CxDeviceList();
				Console.WriteLine("{0}: Audio Input Device", __FUNCTION__);
				devices.Setup(XIE.Media.ExMediaType.Audio, XIE.Media.ExMediaDir.Input);
				int length = devices.Length;
				Console.WriteLine("{0,-20} : {1}", "Length", length);
				for(int i=0 ; i<length ; i++)
				{
					var item = devices[i];
					Console.WriteLine("|- {0}", i);
					Console.WriteLine("|  |- {0,-20} : \"{1}\" ({2})", "Name(Index)", item.Name, item.Index);
					Console.WriteLine("|  |- {0,-20} : \"{1}\" ", "ProductName", item.GetProductName());
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}

			// --------------------------------------------------
			// C) オーディオ出力デバイスの列挙.
			try
			{
				Console.WriteLine("----------------------------------------");
				var devices = new XIE.Media.CxDeviceList();
				Console.WriteLine("{0}: Audio Output Device", __FUNCTION__);
				devices.Setup(XIE.Media.ExMediaType.Audio, XIE.Media.ExMediaDir.Output);
				int length = devices.Length;
				Console.WriteLine("{0,-20} : {1}", "Length", length);
				for(int i=0 ; i<length ; i++)
				{
					var item = devices[i];
					Console.WriteLine("|- {0}", i);
					Console.WriteLine("|  |- {0,-20} : \"{1}\" ({2})", "Name(Index)", item.Name, item.Index);
					Console.WriteLine("|  |- {0,-20} : \"{1}\" ", "ProductName", item.GetProductName());
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
		}

		#endregion

		#region test11: CxCamera

		/// <summary>
		/// CxCamera
		/// </summary>
		static void test11()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			// --------------------------------------------------
			try
			{
				var watch = new XIE.CxStopwatch();

				// 1) 初期化.
				var videoParam = new XIE.Media.CxDeviceParam(null, 0, 0, new XIE.TxSizeI(640, 480));
				var controller = new XIE.Media.CxCamera();
				controller.Setup(videoParam, null, null);

				// 2) 露光開始.
				controller.Start();

				// 3) 取り込み用バッファ.
				var images = new List<XIE.CxImage>();
				var args = new List<XIE.Media.CxGrabberArgs>();

				// 4) 周期処理.
				watch.Start();
				{
					var grabber = controller.CreateGrabber();
					grabber.Notify += delegate(object sender, XIE.Media.CxGrabberArgs e)
						{
							var image = (XIE.CxImage)e;	// similar to CopyTo(image)
							args.Add(e);
							images.Add(image);
						};
					grabber.Reset();
					grabber.Start();
					grabber.Wait(3000);
					grabber.Stop();
				}
				watch.Stop();
				Console.WriteLine("{0}: Elapsed={1:F3} msec", __FUNCTION__, watch.Elapsed);

				// 5) 露光停止.
				controller.Stop();
				watch.Stop();
				Console.WriteLine("{0}: Stopped={1:F3} msec", __FUNCTION__, watch.Elapsed);

				// E) 確認用.
				{
					Console.WriteLine("{0,-20}:[{1}]", "ProductName", (string)controller.GetParam("ProductName"));

					var images_dir = Results + string.Format("/{0}", __FUNCTION__);
					if (Directory.Exists(images_dir) == false)
						Directory.CreateDirectory(images_dir);

					for(int i=0 ; i<images.Count ; i++)
					{
						var ts = XIE.TxDateTime.FromBinary(args[i].TimeStamp, true);
						//Console.WriteLine("{0:00}:{1:00}:{2:00}.{3:000}: index={4}",
						//	ts.Hour, ts.Minute, ts.Second, ts.Millisecond,
						//	args[i].Index);
						images[i].Save(images_dir + string.Format("/image{0}.png", i));
					}
				}
				watch.Stop();
				Console.WriteLine("{0}: Completed={1:F3} msec", __FUNCTION__, watch.Elapsed);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
		}

		#endregion

		#region test12: CxCamera

		/// <summary>
		/// CxCamera
		/// </summary>
		static void test12()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			string[] output_files = 
			{
				string.Format("{0}.avi", __FUNCTION__),
				string.Format("{0}.asf", __FUNCTION__),
				string.Format("{0}.wmv", __FUNCTION__),
			};

			for(int ddd=0 ; ddd<output_files.Length ; ddd++)
			{
				Console.WriteLine("{0}: {1}={2}", __FUNCTION__, ddd, output_files[ddd]);

				// --------------------------------------------------
				try
				{
					var watch = new XIE.CxStopwatch();

					// 1) 初期化.
					var videoParam = new XIE.Media.CxDeviceParam(null, 0, 0, new XIE.TxSizeI(640, 480));
					var audioParam = new XIE.Media.CxDeviceParam(null, 0);
					var output_file = new XIE.CxStringA(Results + "/" + output_files[ddd]);

					var controller = new XIE.Media.CxCamera();
					controller.Setup(videoParam, audioParam, output_file);

					// 2) 露光開始.
					controller.Start();

					// 3) 取り込み用バッファ.
					var args = new List<XIE.Media.CxGrabberArgs>();

					// 4) 周期処理.
					watch.Start();
					{
						var grabber = controller.CreateGrabber();
						grabber.Notify +=
							delegate(object sender, XIE.Media.CxGrabberArgs e)
							{
								var image = (XIE.CxImage)e;	// similar to CopyTo(image)
								args.Add(e);
							};
						grabber.Reset();
						grabber.Start();
						grabber.Wait(3000);
						grabber.Stop();
					}
					watch.Stop();
					Console.WriteLine("{0}: Elapsed={1:F3} msec", __FUNCTION__, watch.Elapsed);

					// 5) 露光停止.
					controller.Stop();
					watch.Stop();
					Console.WriteLine("{0}: Stopped={1:F3} msec", __FUNCTION__, watch.Elapsed);

					// E) 確認用.
					{
						Console.WriteLine("{0,-20}:[{1}]", "ProductName", (string)controller.GetParam("ProductName"));

						for(int i=0 ; i<(int)args.Count ; i++)
						{
							var ts = XIE.TxDateTime.FromBinary(args[i].TimeStamp, true);
							Console.WriteLine("{0:00}:{1:00}:{2:00}.{3:000}: index={4}",
								ts.Hour, ts.Minute, ts.Second, ts.Millisecond,
								args[i].Index);
						}
					}
					watch.Stop();
					Console.WriteLine("{0}: Completed={1:F3} msec", __FUNCTION__, watch.Elapsed);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.StackTrace);
				}
			}
		}

		#endregion

		#region test13: CxCamera

		/// <summary>
		/// CxCamera + CxControlProperty
		/// </summary>
		static void test13()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			// --------------------------------------------------
			try
			{
				var watch = new XIE.CxStopwatch();

				// 1) 初期化.
				var videoParam = new XIE.Media.CxDeviceParam(null, 0, 0, new XIE.TxSizeI(640, 480));
				var controller = new XIE.Media.CxCamera();
				controller.Setup(videoParam, null, null);

				// 2) 制御プロパティ.
				{
					Console.WriteLine("");
					Console.WriteLine("{0}", (string)controller.GetParam("ProductName"));

					string[] names = 
					{
						// 2-1) カメラ制御.
						"Pan", "Tilt", "Roll", "Zoom", "Exposure", "Iris", "Focus", 

						// 2-2) 映像品質.
						"Brightness", "Contrast", "Gain", "Hue", "Saturation",
						"Sharpness", "Gamma", "ColorEnable", "WhiteBalance", "BacklightCompensation", 
					};

					Console.WriteLine("");
					Console.WriteLine("--- {0,-22}: {1,5}, {2,5}, {3,6}, {4,4}, {5,5} ~ {6,5}",
						"Name", "Value", "Def", "Flag", "Step", "Lower", "Upper");

					for (int i = 0; i < names.Length; i++)
					{
						var cp = controller.ControlProperty(names[i]);
						if (cp.IsSupported())
						{
							var range = cp.GetRange();
							Console.WriteLine("[O] {0,-22}: {1,5}, {2,5}, 0x{3:X4}, {4,4}, {5,5} ~ {6,5}",
								names[i],
								cp.GetValue(),
								cp.GetDefault(),
								cp.GetFlags(),
								cp.GetStep(),
								range.Lower,
								range.Upper
								);
						}
						else
						{
							Console.WriteLine("[-] {0,-22}:", names[i]);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
			}
		}

		#endregion

		#region test21: CxMediaPlayer

		/// <summary>
		/// CxMediaPlayer
		/// </summary>
		static void test21()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			string[] source_files = 
			{
				string.Format("stopwatch_320x240.wmv"),
			};

			for(int sss=0 ; sss<source_files.Length ; sss++)
			{
				Console.WriteLine("{0}: {1}={2}", __FUNCTION__, sss, source_files[sss]);

				// --------------------------------------------------
				try
				{
					var watch = new XIE.CxStopwatch();

					// 1) 初期化.
					var controller = new XIE.Media.CxMediaPlayer();
					var source_file = new XIE.CxStringA(TestFiles + "/" + source_files[sss]);

					// -) 音声をスピーカーに出力する場合は第３引数を指定してください.
					bool use_speaker = false;
					if (use_speaker)
						controller.Setup(source_file, null, null);
					else
					{
						var outputParam = new XIE.Media.CxDeviceParam(null, 0);
						controller.Setup(source_file, null, outputParam);
					}

					// DEBUG: 保存したファイルを graphedt で確認できます.
					if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
					{
						var grf = string.Format("{0}-{1}.GRF", __FUNCTION__, sss);
						controller.SetParam("SaveGraphFile", grf);
					}

					// 2) 取り込み用バッファ.
					var images = new List<XIE.CxImage>();
					var args = new List<XIE.Media.CxGrabberArgs>();

					// 3) 周期処理.
					watch.Start();
					{
						var grabber = controller.CreateGrabber();
						grabber.Notify +=
							delegate(object sender, XIE.Media.CxGrabberArgs e)
							{
								var image = (XIE.CxImage)e;	// similar to CopyTo(image)
								args.Add(e);
								images.Add(image);
							};
						grabber.Reset();
						grabber.Start();

						controller.Start();					// 再生開始.
						controller.WaitForCompletion(5000);	// 待機.
						controller.Stop();					// 再生停止.
					}
					watch.Stop();
					Console.WriteLine("{0}: Elapsed={1} msec", __FUNCTION__, watch.Elapsed);

					// E) 確認用.
					{
						var images_dir = Results + string.Format("/{0}-{1}", __FUNCTION__, sss);
						if (Directory.Exists(images_dir) == false)
							Directory.CreateDirectory(images_dir);

						for(int i=0 ; i<(int)images.Count ; i++)
						{
							var ts = XIE.TxDateTime.FromBinary(args[i].TimeStamp, true);
							Console.WriteLine("{0:00}:{1:00}:{2:00}.{3:000}: index={4}",
								ts.Hour, ts.Minute, ts.Second, ts.Millisecond,
								args[i].Index);
							images[i].Save(images_dir + string.Format("/image{0}.png", i));
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.StackTrace);
				}
			}
		}

		#endregion

		#region test22: CxMediaPlayer

		/// <summary>
		/// CxMediaPlayer
		/// </summary>
		static void test22()
		{
			string __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			string[] source_files = 
			{
				string.Format("stopwatch_320x240.wmv"),
			};

			string[] output_files = 
			{
				string.Format("{0}.avi", __FUNCTION__),
				string.Format("{0}.asf", __FUNCTION__),
				string.Format("{0}.wmv", __FUNCTION__),
			};

			for(int sss=0 ; sss<source_files.Length ; sss++)
			{
				Console.WriteLine("{0}: {1}={2}", __FUNCTION__, sss, source_files[sss]);

				for(int ddd=0 ; ddd<output_files.Length ; ddd++)
				{
					Console.WriteLine("{0}: {1}={2}", __FUNCTION__, ddd, output_files[ddd]);

					// --------------------------------------------------
					try
					{
						var watch = new XIE.CxStopwatch();

						// 1) 初期化.
						var controller = new XIE.Media.CxMediaPlayer();
						var source_file = new XIE.CxStringA(TestFiles + "/" + source_files[sss]);
						var output_file = new XIE.CxStringA(Results + "/" + output_files[ddd]);
						controller.Setup(source_file, null, output_file);
		
						// DEBUG: 保存したファイルを graphedt で確認できます.
						if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
						{
							var grf = string.Format("{0}-{1}-{2}.GRF", __FUNCTION__, sss, ddd);
							controller.SetParam("SaveGraphFile", grf);
						}

						// 2) 周期処理.
						watch.Start();
						{
							controller.Start();					// 再生開始.
							controller.WaitForCompletion(5000);	// 待機.
							controller.Stop();					// 再生停止.
						}
						watch.Stop();
						Console.WriteLine("{0}: Elapsed={1} msec", __FUNCTION__, watch.Elapsed);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.StackTrace);
					}
				}
			}
		}

		#endregion
	}
}
