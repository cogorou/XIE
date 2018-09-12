/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace StatImage
{
	partial class Program
	{
		const string AppName = "StatImage";
		static string ResultDir = "";

		/// <summary>
		/// エントリポイント
		/// </summary>
		/// <param name="args"></param>
		[STAThread]
		static void Main(string[] args)
		{
			XIE.Axi.Setup();

			try
			{
				// Results
				const string result_dir = "Results";
				System.IO.Directory.CreateDirectory(result_dir);

				// Results/AppName
				ResultDir = System.IO.Path.Combine(result_dir, AppName);
				System.IO.Directory.CreateDirectory(ResultDir);

				// mode
				int mode = 1;
				if (args.Length > 0)
					mode = Convert.ToInt32(args[0]);

				Console.WriteLine("mode={0}", mode);

				var dlg = new OpenFileDialog();
				dlg.AddExtension = true;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = true;
				dlg.Filter = "Image files |*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw;";
				dlg.Filter += "|All files (*.*)|*.*";

				if (dlg.ShowDialog(Form.ActiveForm) == DialogResult.OK)
				{
					var filenames = dlg.FileNames;

					Console.WriteLine("{0} files", filenames.Length);

					var now = DateTime.Now;
					var suffix = string.Format("{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}",
						now.Year, now.Month, now.Day,
						now.Hour, now.Minute, now.Second);

					// Make Thumbnail
					switch (mode)
					{
						default:
						case 1: MakeCompositImage1(filenames, ResultDir, suffix); break;
						case 2: MakeCompositImage2(filenames, ResultDir, suffix); break;
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}

			XIE.Axi.TearDown();
		}

		/// <summary>
		/// 合成画像の生成 (平均、最少、最大)
		/// </summary>
		/// <param name="filenames">元画像</param>
		/// <param name="result_dir">保存先のディレクトリ</param>
		/// <param name="suffix">ファイル名のサフィックス</param>
		static void MakeCompositImage1(string[] filenames, string result_dir, string suffix)
		{
			var __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			var watch = new XIE.CxStopwatch();
			var stat = new XIE.TxStatistics();

			using (var sum = new XIE.CxImage())
			using (var ave = new XIE.CxImage())
			using (var min = new XIE.CxImage())
			using (var max = new XIE.CxImage())
			{
				for (int i = 0; i < filenames.Length; i++)
				{
					using (var src = new XIE.CxImage(filenames[i], true))
					{
						watch.Start();
						if (i == 0)
						{
							sum.Resize(src.Width, src.Height, XIE.TxModel.F64(src.Model.Pack), src.Channels);
							sum.Reset();
							ave.Resize(src.Width, src.Height, src.Model, src.Channels);
							min.Resize(src.Width, src.Height, src.Model, src.Channels);
							max.Resize(src.Width, src.Height, src.Model, src.Channels);
							{
								ave.ExifCopy(src.Exif());
								min.ExifCopy(src.Exif());
								max.ExifCopy(src.Exif());
							}
							min.Filter().Copy(src);
							max.Filter().Copy(src);
						}
						else
						{
							min.Filter().Min(min, src);
							max.Filter().Max(max, src);
						}
						sum.Filter().Add(sum, src);
						watch.Stop();
						stat += watch.Lap;
						Console.WriteLine("{0,2}/{1,2}: Lap = {2:F3} msec", i + 1, filenames.Length, watch.Lap);
					}
				}

				watch.Start();
				ave.Filter().Div(sum, filenames.Length);
				watch.Stop();
				stat += watch.Lap;

				Console.WriteLine("{0,-10}: {1,9:F0}", "Count", stat.Count);
				Console.WriteLine("{0,-10}: {1,9:F3} msec", "Sum", stat.Sum1);
				Console.WriteLine("{0,-10}: {1,9:F3} msec", "Mean", stat.Mean);
				Console.WriteLine("{0,-10}: {1,9:F3} msec", "Sigma", stat.Sigma);
				Console.WriteLine("{0,-10}: {1,9:F3} msec", "Min", stat.Min);
				Console.WriteLine("{0,-10}: {1,9:F3} msec", "Max", stat.Max);

				ave.Save(string.Format("{0}/{1}_ave.jpg", result_dir, suffix));
				min.Save(string.Format("{0}/{1}_min.jpg", result_dir, suffix));
				max.Save(string.Format("{0}/{1}_max.jpg", result_dir, suffix));
			}
		}

		/// <summary>
		/// 合成画像の生成 (彩度の最大)
		/// </summary>
		/// <param name="filenames">元画像</param>
		/// <param name="result_dir">保存先のディレクトリ</param>
		/// <param name="suffix">ファイル名のサフィックス</param>
		static void MakeCompositImage2(string[] filenames, string result_dir, string suffix)
		{
			var __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			var watch = new XIE.CxStopwatch();
			var stat = new XIE.TxStatistics();
			var dst = new XIE.CxImage();

			for (int i = 0; i < filenames.Length; i++)
			{
				using (var src = new XIE.CxImage(filenames[i], true))
				using (var hsv = new XIE.CxImage())
				{
					watch.Start();
					var effector = new XIE.Effectors.CxRgbToHsv();
					effector.Execute(src, hsv);
					if (i == 0)
					{
						dst.CopyFrom(hsv);
						dst.ExifCopy(src.Exif());
					}
					else
					{
						for (int ch = 0; ch < src.Channels; ch++)
						{
							var src_scan = hsv.Scanner(ch);
							var dst_scan = dst.Scanner(ch);

							for (int y = 0; y < src_scan.Height; y++)
							{
								for (int x = 0; x < src_scan.Width; x++)
								{
									var _src = (XIE.Ptr.DoublePtr)src_scan[y, x];
									var _dst = (XIE.Ptr.DoublePtr)dst_scan[y, x];
									if (_dst[1] < _src[1])
									{
										_dst[0] = _src[0];
										_dst[1] = _src[1];
										_dst[2] = _src[2];
									}
								}
							}
						}
					}
					watch.Stop();
					stat += watch.Lap;
					Console.WriteLine("{0,2}/{1,2}: Lap = {2:F3} msec", i + 1, filenames.Length, watch.Lap);
				}
			}

			Console.WriteLine("{0,-10}: {1,9:F0}", "Count", stat.Count);
			Console.WriteLine("{0,-10}: {1,9:F3} msec", "Sum", stat.Sum1);
			Console.WriteLine("{0,-10}: {1,9:F3} msec", "Mean", stat.Mean);
			Console.WriteLine("{0,-10}: {1,9:F3} msec", "Sigma", stat.Sigma);
			Console.WriteLine("{0,-10}: {1,9:F3} msec", "Min", stat.Min);
			Console.WriteLine("{0,-10}: {1,9:F3} msec", "Max", stat.Max);

			using (var ans = new XIE.CxImage())
			{
				var effector = new XIE.Effectors.CxHsvToRgb();
				effector.Execute(dst, ans);
				ans.ExifCopy(dst.Exif());
				ans.Save(string.Format("{0}/{1}.jpg", result_dir, suffix));
			}
		}
	}
}
