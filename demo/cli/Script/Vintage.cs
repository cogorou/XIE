/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Vintage
{
	class Program
	{
		const string AppName = "Vintage";
		static string ResultDir = "Results";

		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				XIE.Axi.Setup();

				// Results
				const string result_dir = "Results";
				System.IO.Directory.CreateDirectory(result_dir);

				// Results/AppName
				ResultDir = System.IO.Path.Combine(result_dir, AppName);
				System.IO.Directory.CreateDirectory(ResultDir);

				Execute();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				XIE.Axi.TearDown();
			}
		}

		/// <summary>
		/// Execute
		/// </summary>
		static void Execute()
		{
			var __FUNCTION__ = MethodBase.GetCurrentMethod().Name;
			Console.WriteLine(__FUNCTION__);

			var ofd = new OpenFileDialog();
			ofd.AddExtension = true;
			ofd.CheckFileExists = true;
			ofd.CheckPathExists = true;
			ofd.Multiselect = true;
			ofd.Filter = "Image files |*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw;";
			ofd.Filter += "|All files (*.*)|*.*";

			if (ofd.ShowDialog(Form.ActiveForm) == DialogResult.OK)
			{
				var dir = System.IO.Path.GetDirectoryName(ofd.FileName);
				var filenames = ofd.FileNames;
				var now = DateTime.Now;
				var suffix = string.Format("{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}",
					now.Year, now.Month, now.Day,
					now.Hour, now.Minute, now.Second);

				Console.WriteLine(dir);
				Console.WriteLine("{0} files", filenames.Length);

				var watch = new XIE.CxStopwatch();
				foreach (var filename in filenames)
				{
					// Execute
					watch.Start();
					using (var src = new XIE.CxImage(filename, true))
					using (var dst = VintageFilter(src, true))
					{
						// exif
						using (var src_exif = XIE.CxExif.FromTag(src.Exif()))
						using (var dst_exif = src_exif.GetPurgedExif())
						{
							dst.Exif(dst_exif.Tag());
						}

						var prefix = System.IO.Path.GetFileNameWithoutExtension(filename);
						dst.Save(string.Format("{0}/{1}-{2}-{3}.jpg", ResultDir, prefix, AppName, suffix));
					}
					watch.Stop();
					Console.WriteLine("{0:F3} msec: {1}", watch.Lap, System.IO.Path.GetFileName(filename));
				}
				Console.WriteLine("Elapsed: {0:N3} msec", watch.Elapsed);
			}
		}

		/// <summary>
		/// Vintage Filter
		/// </summary>
		/// <param name="src">元画像</param>
		/// <param name="vignetting">周辺減光フィルタの施行</param>
		/// <returns>
		///		処理結果を返します。
		/// </returns>
		static XIE.CxImage VintageFilter(XIE.CxImage src, bool vignetting)
		{
			using (var dst_rgb = new XIE.CxImage())
			using (var dst_hsv = new XIE.CxImage())
			using (var dst_gamma = new XIE.CxImage())
			{
				// RGB Converter:
				{
					var effector = new XIE.Effectors.CxRgbConverter(1.0, 0.9, 0.7);
					effector.Execute(src, dst_rgb);
				}

				// HSV Converter:
				{
					var effector = new XIE.Effectors.CxHsvConverter(0, 0, 0.2, 1.5);
					effector.Execute(dst_rgb, dst_hsv);
				}

				// Gamma Converter:
				{
					var effector = new XIE.Effectors.CxGammaConverter(0, 0.9);
					effector.Execute(dst_hsv, dst_gamma);
				}

				// Result:
				if (vignetting)
					return Vignette(dst_gamma);
				else
					return (XIE.CxImage)dst_gamma.Clone();
			}
		}

		/// <summary>
		/// 周辺減光
		/// </summary>
		/// <param name="src">元画像</param>
		/// <returns>
		///		処理結果を返します。
		/// </returns>
		static XIE.CxImage Vignette(XIE.CxImage src)
		{
			using (var canvas = new XIE.GDI.CxCanvas())
			{
				canvas.Resize(src.Size);
				canvas.DrawImage(src);

				// Paint Vignette
				{
					var graphics = canvas.Graphics;
					var bounds1 = new Rectangle(0, 0, src.Width, src.Height);

					var bounds2 = new Rectangle(
							-src.Width / 2,
							-src.Height / 2,
							bounds1.Width * 2,
							bounds1.Height * 2
						);

					using (var path = new GraphicsPath())
					{
						path.AddEllipse(bounds2);
						using (var brush = new PathGradientBrush(path))
						{
							brush.WrapMode = WrapMode.Tile;
							brush.CenterColor = Color.FromArgb(0, 0, 0, 0);
							brush.SurroundColors = new Color[]
							{
								Color.FromArgb(255, 32, 29, 22)
							};
							brush.Blend = new Blend()
							{
								Positions = new float[]
								{
									0.0f,
									0.2f,
									0.4f,
									0.6f,
									0.8f,
									1.0f,
								},
								Factors = new float[]
								{
									0.0f,
									0.6f,
									0.8f,
									1.0f,
									1.0f,
									1.0f,
								},
							};
							graphics.Clip = new Region(bounds1);
							graphics.FillRectangle(brush, bounds2);
						}
					}
				}

				var dst = canvas.Snapshot();
				return dst;
			}
		}
	}
}
