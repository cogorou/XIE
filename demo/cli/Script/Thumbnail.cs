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

namespace Thumbnail
{
	class Program
	{
		const string AppName = "Thumbnail";

		const string DstExtension = "jpg";

		const int margin_x = 16;		// フレーム間のマージン.(横方向)
		const int margin_y = 16;		// フレーム間のマージン.(縦方向)

		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				XIE.Axi.Setup();

				// mode
				int mode = 3;
				if (args.Length > 0)
					mode = Convert.ToInt32(args[0]);

				Console.WriteLine("mode={0}", mode);

				// Make Thumbnail
				switch (mode)
				{
					case 1: MakeThumbnail(1000, 2); break;
					case 2: MakeThumbnailFixedSize(1000, 750, 2); break;
					default:
					case 3: MakeThumbnailFlexibleSize(2048); break;
				}
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
		/// サムネイルの生成
		/// </summary>
		/// <param name="max_length">辺の長さの最大.(pixels)</param>
		/// <param name="max_cols">列数</param>
		static void MakeThumbnail(int max_length, int max_cols)
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

				using (var dst = new XIE.CxImage())
				{
					#region サムネイル生成:
					using (var tmp = new XIE.CxImage())
					{
						var cols = (filenames.Length < max_cols) ? filenames.Length : max_cols;
						var rows = (filenames.Length + (cols - 1)) / cols;
						var dst_width = margin_x + (margin_x + max_length) * cols;
						var dst_height = margin_y + (margin_y + max_length) * rows;

						tmp.Resize(dst_width, dst_height, XIE.TxModel.U8(1), 3);

						int roi_w_max = 0;
						int roi_y = margin_y;

						#region 一時バッファへの書き込み:
						var watch = new XIE.CxStopwatch();
						tmp.Reset();
						for (int row = 0; row < rows; row++)
						{
							int roi_x = margin_x;
							int roi_h_max = 0;
							for (int col = 0; col < cols; col++)
							{
								int index = row * cols + col;
								if (index >= filenames.Length) break;

								watch.Start();
								using (var src = new XIE.CxImage(filenames[index], true))
								using (var roi = new XIE.CxImage())
								{
									if (src.Model.Type != XIE.ExType.U8)
									{
										var scale_value = XIE.Axi.CalcScale(src.Model.Type, src.Depth, XIE.ExType.U8, 8);
										src.Filter().Mul(src, scale_value);
										src.Depth = 8;
									}
									else if (src.Depth != 0 && src.Depth < 8)
									{
										var scale_value = XIE.Axi.CalcScale(src.Model.Type, src.Depth, XIE.ExType.U8, 8);
										src.Filter().Mul(src, scale_value);
										src.Depth = 8;
									}

									double mag_x = (double)max_length / src.Width;
									double mag_y = (double)max_length / src.Height;
									double mag = System.Math.Min(mag_x, mag_y);
									if (mag > 1.0)
										mag = 1.0;
									int interpolation = (mag == 1.0) ? 0 : ((mag <= 0.5) ? 2 : 1);

									int roi_w = (int)System.Math.Round(src.Width * mag);
									int roi_h = (int)System.Math.Round(src.Height * mag);

									// scale
									roi.Attach(tmp, new XIE.TxRectangleI(roi_x, roi_y, roi_w, roi_h));
									roi.Filter().Scale(src, mag, mag, interpolation);

									// exif
									if (tmp.Exif().IsValid == false && src.Exif().IsValid == true)
									{
										tmp.ExifCopy(src.Exif());
									}

									// next
									roi_x += (roi_w + margin_x);
									roi_h_max = System.Math.Max(roi_h_max, roi_h);
								}
								watch.Stop();
								Console.WriteLine("{0},{1}: {2:F3} msec", row, col, watch.Lap);
							}

							// next
							roi_y += (roi_h_max + margin_y);
							roi_w_max = System.Math.Max(roi_w_max, roi_x);
						}
						#endregion

						// 出力画像への書き込み:
						using (var tmp_child = tmp.Child(new XIE.TxRectangleI(0, 0, roi_w_max, roi_y)))
						{
							if (tmp_child.IsValid)
							{
								dst.Resize(roi_w_max, roi_y, XIE.TxModel.U8(1), 3);
								dst.Filter().Copy(tmp_child);
							}
						}
					}
					#endregion

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
			}
		}

		/// <summary>
		/// サムネイルの生成 (固定リージョンサイズ)
		/// </summary>
		/// <param name="roi_width">リージョンの幅 (pixels)</param>
		/// <param name="roi_height">リージョンの高さ (pixels)</param>
		static void MakeThumbnailFixedSize(int roi_width, int roi_height, int max_cols)
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

				using (var dst = new XIE.CxImage())
				{
					#region サムネイル生成:
					{
						var cols = (filenames.Length < max_cols) ? filenames.Length : max_cols;
						var rows = (filenames.Length + (cols - 1)) / cols;
						var dst_width = margin_x + (margin_x + roi_width) * cols;
						var dst_height = margin_y + (margin_y + roi_height) * rows;

						dst.Resize(dst_width, dst_height, XIE.TxModel.U8(1), 3);

						#region 出力画像の生成:
						var watch = new XIE.CxStopwatch();
						dst.Reset();
						for (int row = 0; row < rows; row++)
						{
							for (int col = 0; col < cols; col++)
							{
								int index = row * cols + col;
								if (index >= filenames.Length) break;

								watch.Start();
								using (var src = new XIE.CxImage(filenames[index], true))
								using (var roi = new XIE.CxImage())
								{
									if (src.Model.Type != XIE.ExType.U8)
									{
										var scale_value = XIE.Axi.CalcScale(src.Model.Type, src.Depth, XIE.ExType.U8, 8);
										src.Filter().Mul(src, scale_value);
										src.Depth = 8;
									}
									else if (src.Depth != 0 && src.Depth < 8)
									{
										var scale_value = XIE.Axi.CalcScale(src.Model.Type, src.Depth, XIE.ExType.U8, 8);
										src.Filter().Mul(src, scale_value);
										src.Depth = 8;
									}

									double mag_x = (double)roi_width / src.Width;
									double mag_y = (double)roi_height / src.Height;
									double mag = System.Math.Min(mag_x, mag_y);
									int interpolation = (mag <= 0.5) ? 2 : 1;

									int roi_x = margin_x + (margin_x + roi_width) * col;
									int roi_y = margin_y + (margin_y + roi_height) * row;
									int roi_w = (int)System.Math.Round(src.Width * mag);
									int roi_h = (int)System.Math.Round(src.Height * mag);
									roi_x += ((roi_width - roi_w) / 2);
									roi_y += ((roi_height - roi_h) / 2);

									// scale
									roi.Attach(dst, new XIE.TxRectangleI(roi_x, roi_y, roi_w, roi_h));
									roi.Filter().Scale(src, mag, mag, interpolation);

									// exif
									if (dst.Exif().IsValid == false && src.Exif().IsValid == true)
									{
										dst.ExifCopy(src.Exif());
									}
								}
								watch.Stop();
								Console.WriteLine("{0},{1}: {2:F3} msec", row, col, watch.Lap);
							}
						}
						#endregion
					}
					#endregion

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
			}
		}

		/// <summary>
		/// サムネイルの生成 (可変リージョンサイズ)
		/// </summary>
		/// <param name="image_size_max">画像の辺の長さの最大幅 (pixels)</param>
		static void MakeThumbnailFlexibleSize(int image_size_max)
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

				var images = new List<XIE.CxImage>();

				#region 全てのファイルを読み込みます.
				foreach (var filename in filenames)
				{
					try
					{
						var src = new XIE.CxImage(filename);
						images.Add(src);

						if (src.Model.Type != XIE.ExType.U8)
						{
							var scale_value = XIE.Axi.CalcScale(src.Model.Type, src.Depth, XIE.ExType.U8, 8);
							src.Filter().Mul(src, scale_value);
							src.Depth = 8;
						}
						else if (src.Depth != 0 && src.Depth < 8)
						{
							var scale_value = XIE.Axi.CalcScale(src.Model.Type, src.Depth, XIE.ExType.U8, 8);
							src.Filter().Mul(src, scale_value);
							src.Depth = 8;
						}
					}
					catch (System.Exception ex)
					{
						Console.WriteLine("Failed to load {0}", filename);
						Console.WriteLine("	{0}", ex.Message);
					}
				}
				#endregion

				int rows = 1;
				int cols = 1;

				#region ファイル数に応じて行列数を決定します.
				{
					if (images.Count <= 1)
					{
						cols = 1;
						rows = 1;
					}
					else if (images.Count <= 4)
					{
						cols = 2;
						rows = (int)System.Math.Ceiling((double)images.Count / cols);
					}
					else
					{
						cols = 3;
						rows = (int)System.Math.Ceiling((double)images.Count / cols);
					}

					Console.WriteLine("rows={0}", rows);
					Console.WriteLine("cols={0}", cols);
				}
				#endregion

				int sum_width = 0;
				int sum_height = 0;

				#region 行の合計幅の最大値、各行の最大の高さの合計値を計算します.
				{
					int index = 0;
					for (int row = 0; row < rows; row++)
					{
						int sum = 0;
						int max_height = 0;
						if (!(index < images.Count)) break;
						for (int col = 0; col < cols; col++)
						{
							if (!(index < images.Count)) break;
							var src = images[index];
							sum += src.Width;
							if (max_height < src.Height)
								max_height = src.Height;
							index++;
						}
						if (sum_width < sum)
							sum_width = sum;
						sum_height += max_height;
					}

					Console.WriteLine("sum_width ={0}", sum_width);
					Console.WriteLine("sum_height={0}", sum_height);
				}
				#endregion

				double mag = 1.0;

				#region 縮小率を計算します.
				{
					double mag_x = (double)(image_size_max - (margin_x * cols + margin_x)) / sum_width;
					double mag_y = (double)(image_size_max - (margin_y * rows + margin_y)) / sum_height;

					if (mag_x < mag_y && mag_x < 1.0)
						mag = mag_x;
					else if (mag_y < mag_x && mag_y < 1.0)
						mag = mag_y;

					Console.WriteLine("mag_x={0}", mag_x);
					Console.WriteLine("mag_y={0}", mag_y);
					Console.WriteLine("mag  ={0}", mag);
				}
				#endregion

				#region サムネイルを生成します.
				using (var dst = new XIE.CxImage())
				using (var roi = new XIE.CxImage())
				{
					var watch = new XIE.CxStopwatch();
					watch.Start();

					int interpolation = 2;
					if (mag >= 1.0)
						interpolation = 0;
					else if (mag > 0.5)
						interpolation = 1;

					int dst_width = (int)System.Math.Round(sum_width * mag + (margin_x * cols + margin_x));
					int dst_height = (int)System.Math.Round(sum_height * mag + (margin_y * rows + margin_y));

					Console.WriteLine("dst_width ={0}", dst_width);
					Console.WriteLine("dst_height={0}", dst_height);

					dst.Resize(dst_width, dst_height, XIE.TxModel.U8(4), 1);
					dst.Reset();

					watch.Stop();
					Console.WriteLine("Lap: {0:F3} msec", watch.Lap);

					#region サムネイル生成:
					int roi_y = margin_y;
					int index = 0;
					for (int row = 0; row < rows; row++)
					{
						if (!(index < images.Count)) break;
						int roi_x = margin_x;
						int max_height = 0;
						for (int col = 0; col < cols; col++)
						{
							if (!(index < images.Count)) break;
							var src = images[index];

							watch.Start();

							// ROI を設定して縮小します.
							int roi_width = (int)System.Math.Round(src.Width * mag);
							int roi_height = (int)System.Math.Round(src.Height * mag);
							var region = new XIE.TxRectangleI(roi_x, roi_y, roi_width, roi_height);
							roi.Attach(dst, region);
							roi.Filter().Scale(src, mag, mag, interpolation);

							// ROI の開始位置(水平方向)を移動します.
							roi_x += (roi_width + margin_x);
							if (max_height < roi_height)
								max_height = roi_height;
							index++;

							watch.Stop();
							Console.WriteLine("{0},{1}: {2:F3} msec", row, col, watch.Lap);
						}
						// ROI の開始位置(垂直方向)を移動します.
						roi_y += (max_height + margin_y);
					}
					#endregion

					watch.Stop();
					Console.WriteLine("Elapsed: {0:N3} msec", watch.Elapsed);

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
				#endregion
			}
		}
	}
}
