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

namespace Concatenate
{
	class Program
	{
		const string AppName = "Concatenate";

		const string DstExtension = "jpg";

		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				XIE.Axi.Setup();

				// mode
				int mode = 1;
				if (args.Length > 0)
					mode = Convert.ToInt32(args[0]);

				Console.WriteLine("mode={0}", mode);

				// Concatenate Images
				ConcatenateImages(mode);
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
		/// 画像の連結 (水平方向)
		/// </summary>
		/// <param name="mode">連結方向 [1:水平, 2:垂直]</param>
		static void ConcatenateImages(int mode)
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

				XIE.CxImage dst = null;

				try
				{
					switch (mode)
					{
						default:
						case 1: dst = ConcatenateImagesH(filenames); break;
						case 2: dst = ConcatenateImagesV(filenames); break;
					}

					#region ファイル保存:
					var now = DateTime.Now;
					var suffix = string.Format("{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}",
						now.Year, now.Month, now.Day,
						now.Hour, now.Minute, now.Second);

					var sfd = new SaveFileDialog();
					sfd.AddExtension = true;
					sfd.DefaultExt = DstExtension;
					sfd.Filter = "Image files |*.bmp;*.dib;*.png;*.jpg;*.jpeg;*.tif;*.tiff;*.raw;";
					sfd.Filter += "|Bmp files |*.bmp;*.dib";
					sfd.Filter += "|Png files |*.png";
					sfd.Filter += "|Jpeg files |*.jpg;*.jpeg;";
					sfd.Filter += "|Tiff files |*.tif;*.tiff";
					sfd.Filter += "|Raw files |*.raw";
					sfd.Filter += "|All files |*.*";
					sfd.FileName = string.Format("{0}_{1}.{2}", AppName, suffix, DstExtension);

					if (sfd.ShowDialog(Form.ActiveForm) == DialogResult.OK)
					{
						Console.WriteLine("destination file:");
						Console.WriteLine("{0}", sfd.FileName);
						dst.Save(sfd.FileName);
					}
					#endregion
				}
				finally
				{
					if (dst != null)
						dst.Dispose();
				}
			}
		}

		/// <summary>
		/// 画像の連結 (水平方向)
		/// </summary>
		/// <param name="filenames">元画像</param>
		/// <returns>
		///		連結した結果画像を返します。
		/// </returns>
		static XIE.CxImage ConcatenateImagesH(string[] filenames)
		{
			Console.WriteLine("Concatenate Images (Horizontal Direction)");

			#region 出力画像の確保:
			var dst_width = 0;
			var dst_height = 0;
			var is_color = false;

			foreach (var filename in filenames)
			{
				using (var tmp = new XIE.CxImage(filename))
				{
					dst_width += tmp.Width;
					dst_height = System.Math.Max(dst_height, tmp.Height);
					if (tmp.Model.Pack > 1 || tmp.Channels > 1)
						is_color = true;
				}
			}

			var dst = (is_color)
				? new XIE.CxImage(dst_width, dst_height, XIE.TxModel.U8(3), 1)
				: new XIE.CxImage(dst_width, dst_height, XIE.TxModel.U8(1), 1);

			dst.Reset();
			#endregion

			#region 連結処理:
			int x_pos = 0;
			foreach (var filename in filenames)
			{
				var watch = new XIE.CxStopwatch();
				watch.Start();

				using (var src = new XIE.CxImage(filename))
				using (var act = dst.Child(-1, new XIE.TxRectangleI(x_pos, 0, src.Width, src.Height)))
				{
					var scale = XIE.Axi.CalcScale(src.Model.Type, src.Depth, dst.Model.Type, dst.Depth);
					act.Filter().Copy(src, scale);

					x_pos += src.Width;
				}

				watch.Stop();
				Console.WriteLine("{0:F3} msec : {1}", watch.Lap, System.IO.Path.GetFileName(filename));
			}
			#endregion

			return dst;
		}

		/// <summary>
		/// 画像の連結 (垂直方向)
		/// </summary>
		/// <param name="filenames">元画像</param>
		/// <returns>
		///		連結した結果画像を返します。
		/// </returns>
		static XIE.CxImage ConcatenateImagesV(string[] filenames)
		{
			Console.WriteLine("Concatenate Images (Vertical Direction)");

			#region 出力画像の確保:
			var dst_width = 0;
			var dst_height = 0;
			var is_color = false;

			foreach (var filename in filenames)
			{
				using (var tmp = new XIE.CxImage(filename))
				{
					dst_width = System.Math.Max(dst_width, tmp.Width);
					dst_height += tmp.Height;
					if (tmp.Model.Pack > 1 || tmp.Channels > 1)
						is_color = true;
				}
			}

			var dst = (is_color)
				? new XIE.CxImage(dst_width, dst_height, XIE.TxModel.U8(3), 1)
				: new XIE.CxImage(dst_width, dst_height, XIE.TxModel.U8(1), 1);

			dst.Reset();
			#endregion

			#region 連結処理:
			int y_pos = 0;
			foreach (var filename in filenames)
			{
				var watch = new XIE.CxStopwatch();
				watch.Start();

				using (var src = new XIE.CxImage(filename))
				using (var act = dst.Child(-1, new XIE.TxRectangleI(0, y_pos, src.Width, src.Height)))
				{
					var scale = XIE.Axi.CalcScale(src.Model.Type, src.Depth, dst.Model.Type, dst.Depth);
					act.Filter().Copy(src, scale);

					y_pos += src.Height;
				}

				watch.Stop();
				Console.WriteLine("{0:F3} msec : {1}", watch.Lap, System.IO.Path.GetFileName(filename));
			}
			#endregion

			return dst;
		}
	}
}
