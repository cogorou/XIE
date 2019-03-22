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

		const string DstExtension = "jpg";

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
					if (filenames.Length == 0) return;

					Console.WriteLine("{0} files", filenames.Length);

					// Make Thumbnail
					switch (mode)
					{
						default:
						case 1: MakeCompositImage1(filenames); break;
						case 2: MakeCompositImage2(filenames); break;
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
		static void MakeCompositImage1(string[] filenames)
		{
			var __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			var watch = new XIE.CxStopwatch();
			var stat = new XIE.TxStatistics();
			var defaultExt = DstExtension;
			var initialDir = "";

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

				#region ファイル保存:
				var now = DateTime.Now;
				var suffix = string.Format("{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}",
					now.Year, now.Month, now.Day,
					now.Hour, now.Minute, now.Second);

				var sfd = new SaveFileDialog();
				sfd.AddExtension = true;
				sfd.DefaultExt = defaultExt;
				sfd.Filter = "Image files |*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw;";
				sfd.Filter += "|Bmp files |*.bmp;*.dib";
				sfd.Filter += "|Png files |*.png";
				sfd.Filter += "|Jpeg files |*.jpg;*.jpeg;";
				sfd.Filter += "|Tiff files |*.tif;*.tiff";
				sfd.Filter += "|Raw files |*.raw";
				sfd.Filter += "|All files |*.*";
				sfd.FileName = string.Format("{0}.{1}", System.IO.Path.GetFileNameWithoutExtension(filenames[0]), defaultExt);
				if (string.IsNullOrEmpty(initialDir) == false)
					sfd.InitialDirectory = initialDir;

				if (sfd.ShowDialog(Form.ActiveForm) == DialogResult.OK)
				{
					var dst_dir = System.IO.Path.GetDirectoryName(sfd.FileName);
					var prefix = System.IO.Path.GetFileNameWithoutExtension(sfd.FileName);
					var ext = System.IO.Path.GetExtension(sfd.FileName);

					var ave_filename = string.Format("{0}-{1}{2}", prefix, "ave", ext);
					Console.WriteLine("dst: {0}", ave_filename);
					ave.Save(System.IO.Path.Combine(dst_dir, ave_filename));

					var min_filename = string.Format("{0}-{1}{2}", prefix, "min", ext);
					Console.WriteLine("dst: {0}", min_filename);
					min.Save(System.IO.Path.Combine(dst_dir, min_filename));

					var max_filename = string.Format("{0}-{1}{2}", prefix, "max", ext);
					Console.WriteLine("dst: {0}", max_filename);
					max.Save(System.IO.Path.Combine(dst_dir, max_filename));

					defaultExt = System.IO.Path.GetExtension(sfd.FileName).Substring(1);
					initialDir = System.IO.Path.GetDirectoryName(sfd.FileName);
				}
				else
				{
					Console.WriteLine("dst: canceled.");
				}
				#endregion
			}
		}

		/// <summary>
		/// 合成画像の生成 (彩度の最大)
		/// </summary>
		/// <param name="filenames">元画像</param>
		static void MakeCompositImage2(string[] filenames)
		{
			var __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			var watch = new XIE.CxStopwatch();
			var stat = new XIE.TxStatistics();
			var defaultExt = DstExtension;
			var initialDir = "";

			using (var dst = new XIE.CxImage())
			{
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
										// 0:Hue, 1:Saturation, 2:Value
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

					#region ファイル保存:
					var now = DateTime.Now;
					var suffix = string.Format("{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}",
						now.Year, now.Month, now.Day,
						now.Hour, now.Minute, now.Second);

					var sfd = new SaveFileDialog();
					sfd.AddExtension = true;
					sfd.DefaultExt = defaultExt;
					sfd.Filter = "Image files |*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw;";
					sfd.Filter += "|Bmp files |*.bmp;*.dib";
					sfd.Filter += "|Png files |*.png";
					sfd.Filter += "|Jpeg files |*.jpg;*.jpeg;";
					sfd.Filter += "|Tiff files |*.tif;*.tiff";
					sfd.Filter += "|Raw files |*.raw";
					sfd.Filter += "|All files |*.*";
					sfd.FileName = string.Format("{0}.{1}", System.IO.Path.GetFileNameWithoutExtension(filenames[0]), defaultExt);
					if (string.IsNullOrEmpty(initialDir) == false)
						sfd.InitialDirectory = initialDir;

					if (sfd.ShowDialog(Form.ActiveForm) == DialogResult.OK)
					{
						var dst_dir = System.IO.Path.GetDirectoryName(sfd.FileName);
						var prefix = System.IO.Path.GetFileNameWithoutExtension(sfd.FileName);
						var ext = System.IO.Path.GetExtension(sfd.FileName);

						var dst_filename = string.Format("{0}-{1}{2}", prefix, "satmax", ext);
						Console.WriteLine("dst: {0}", dst_filename);
						ans.Save(System.IO.Path.Combine(dst_dir, dst_filename));

						defaultExt = System.IO.Path.GetExtension(sfd.FileName).Substring(1);
						initialDir = System.IO.Path.GetDirectoryName(sfd.FileName);
					}
					else
					{
						Console.WriteLine("dst: canceled.");
					}
					#endregion
				}
			}
		}
	}
}
