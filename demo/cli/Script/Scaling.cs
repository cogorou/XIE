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
using System.Windows.Forms;

namespace Scaling
{
	class Program
	{
		const string AppName = "Scaling";

		const string DstExtension = "jpg";

		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				XIE.Axi.Setup();

				int max_length = 2048;
				if (args.Length > 0)
					max_length = Convert.ToInt32(args[0]);

				Console.WriteLine("max_length={0}", max_length);

				Scale(max_length);
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
		/// 画像の縮小
		/// </summary>
		/// <param name="max_length">辺の長さの最大.(pixels)</param>
		static void Scale(int max_length)
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

				Console.WriteLine(dir);
				Console.WriteLine("{0} files", filenames.Length);

				var watch = new XIE.CxStopwatch();
				var defaultExt = DstExtension;
				var initialDir = "";

				foreach (var filename in filenames)
				{
					using (var src = new XIE.CxImage(filename, true))
					using (var dst = new XIE.CxImage())
					{
						watch.Start();

						double mag_x = (double)max_length / src.Width;
						double mag_y = (double)max_length / src.Height;
						double mag = System.Math.Min(mag_x, mag_y);
						if (mag > 1.0)
							mag = 1.0;
						int interpolation = (mag == 1.0) ? 0 : ((mag <= 0.5) ? 2 : 1);

						int dst_width = (int)System.Math.Round(src.Width * mag);
						int dst_height = (int)System.Math.Round(src.Height * mag);

						// scale
						dst.Resize(dst_width, dst_height, XIE.TxModel.U8(1), 3);
						dst.Filter().Scale(src, mag, mag, interpolation);

						// exif
						if (src.Exif().IsValid)
							dst.ExifCopy(src.Exif());

						watch.Stop();
						Console.WriteLine("{0:F3} msec : {1}x{2} to {3}x{4}", watch.Lap, src.Width, src.Height, dst_width, dst_height);
						Console.WriteLine("src: {0}", filename);

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
						sfd.FileName = string.Format("{0}.{1}", System.IO.Path.GetFileNameWithoutExtension(filename), defaultExt);
						//sfd.FileName = string.Format("{0}-{1}.{2}", System.IO.Path.GetFileNameWithoutExtension(filename), suffix, defaultExt);
						if (string.IsNullOrEmpty(initialDir) == false)
							sfd.InitialDirectory = initialDir;

						if (sfd.ShowDialog(Form.ActiveForm) == DialogResult.OK)
						{
							Console.WriteLine("dst: {0}", sfd.FileName);
							dst.Save(sfd.FileName);
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
}
