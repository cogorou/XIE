/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace demo
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				XIE.Axi.Setup();

				string result_dir = System.IO.Path.GetFullPath("Results");
				if (!System.IO.Directory.Exists(result_dir))
					System.IO.Directory.CreateDirectory(result_dir);

				test01();
				test02();
				test11();
				test12();
				test13();
				test14();
				test15();
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
		/// How to use Effect (RgbToGray)
		/// </summary>
		static void test01()
		{
			var watch = new XIE.CxStopwatch();
			var src = new XIE.CxImage("TestFiles/cube.png", true);	// unpacking (multi channel)
			var dst = new XIE.CxImage();
			watch.Start();
			new XIE.Effectors.CxRgbToGray().Execute(src, dst);
			watch.Stop();
			Console.WriteLine("{0,-20}: {1,9:F3} msec", MethodBase.GetCurrentMethod().Name, watch.Lap);

			dst.Save(string.Format("Results/{0}.png", MethodBase.GetCurrentMethod().Name));
		}

		/// <summary>
		/// How to use Filter (Affine)
		/// </summary>
		static void test02()
		{
			var watch = new XIE.CxStopwatch();
			var src = new XIE.CxImage("TestFiles/cube.png", true);	// unpacking (multi channel)
			var dst = new XIE.CxImage();
			var matrix =
				XIE.CxMatrix.PresetScale(0.5, 0.5) *
				XIE.CxMatrix.PresetRotate(+45.0, src.Width * 0.5, src.Height * 0.5);
			watch.Start();
			dst.Filter().Affine(src, matrix, 0);
			watch.Stop();
			Console.WriteLine("{0,-20}: {1,9:F3} msec", MethodBase.GetCurrentMethod().Name, watch.Lap);

			dst.Save(string.Format("Results/{0}.png", MethodBase.GetCurrentMethod().Name));
		}

		/// <summary>
		/// How to use Scanner (RgbToHsv)
		/// </summary>
		static void test11()
		{
			var watch = new XIE.CxStopwatch();
			var src = new XIE.CxImage("TestFiles/cube.png", false);	// packing (single channel)
			var dst = new XIE.CxImage(src.ImageSize);
			var hsv = new XIE.CxImage(src.Width, src.Height, XIE.TxModel.F64(3), 1);

			for (int i = 0; i < 4; i++)
			{
				watch.Reset();

				int hue_dir = i * 60;	// 0=0,1=60,2=120,3=180

				#region HSV ← RGB
				watch.Start();
				unsafe
				{
					var src_scan = src.Scanner(0);
					var hsv_scan = hsv.Scanner(0);

					hsv_scan.ForEach(src_scan,
						delegate(int y, int x, IntPtr hsv_addr, IntPtr src_addr)
						{
							var _src = (byte*)src_addr.ToPointer();
							var _hsv = (double*)hsv_addr.ToPointer();

							#region 変換処理:
							double dR = (double)_src[0] / 255;
							double dG = (double)_src[1] / 255;
							double dB = (double)_src[2] / 255;

							double max = System.Math.Max(dR, System.Math.Max(dG, dB));
							double min = System.Math.Min(dR, System.Math.Min(dG, dB));
							double delta = (max - min);

							double dH = 0;
							double dS = 0;
							double dV = max;

							//dS = delta;			// 円錐モデル.
							if (max != 0)
								dS = delta / max;	// 円柱モデル.

							if (delta == 0)		dH = 0;
							else if (max == dR)	dH = ((dG - dB) / delta + 0) * 60;
							else if (max == dG)	dH = ((dB - dR) / delta + 2) * 60;
							else if (max == dB)	dH = ((dR - dG) / delta + 4) * 60;

							// 色相(Hue)の変換.
							if (delta != 0)
								dH += hue_dir;

							dH = dH % 360;
							if (dH < 0)
								dH += 360;

							_hsv[0] = dH;	// 0~359
							_hsv[1] = dS;	// 0.0~1.0
							_hsv[2] = dV;	// 0.0~1.0
							#endregion
						}
					);
				}
				watch.Stop();
				#endregion

				#region RGB ← HSV
				watch.Start();
				unsafe
				{
					var hsv_scan = hsv.Scanner(0);
					var dst_scan = dst.Scanner(0);

					dst_scan.ForEach(hsv_scan,
						delegate(int y, int x, IntPtr dst_addr, IntPtr hsv_addr)
						{
							var _hsv = (double*)hsv_addr.ToPointer();
							var _rgb = (byte*)dst_addr.ToPointer();

							#region 変換処理:
							double dS = _hsv[1];
							double dV = _hsv[2];

							double dR = dV;
							double dG = dV;
							double dB = dV;

							if (0 != dS)
							{
								int iH = (int)(_hsv[0] / 60);

								double dF = (_hsv[0] / 60) - iH;
								double d0 = dV * (1 - dS);
								double d1 = dV * (1 - dS * dF);
								double d2 = dV * (1 - dS * (1 - dF));

								switch (iH)
								{
									case 0: dR = dV; dG = d2; dB = d0; break;	// V,2,0
									case 1: dR = d1; dG = dV; dB = d0; break;	// 1,V,0
									case 2: dR = d0; dG = dV; dB = d2; break;	// 0,V,2
									case 3: dR = d0; dG = d1; dB = dV; break;	// 0,1,V
									case 4: dR = d2; dG = d0; dB = dV; break;	// 2,0,V
									case 5: dR = dV; dG = d0; dB = d1; break;	// V,0,1
								}
							}

							_rgb[0] = (byte)((dR < 0) ? 0 : (dR > 1) ? 255 : dR * 255);
							_rgb[1] = (byte)((dG < 0) ? 0 : (dG > 1) ? 255 : dG * 255);
							_rgb[2] = (byte)((dB < 0) ? 0 : (dB > 1) ? 255 : dB * 255);
							#endregion
						}
					);
				}
				watch.Stop();
				#endregion

				Console.WriteLine("{0,-20}: {1,9:F3} msec :  ({2:000}) RGB +- HSV +- RGB", MethodBase.GetCurrentMethod().Name, watch.Elapsed, i);

				dst.Save(string.Format("Results/{0}-{1:00}-hue+{2:000}.png", MethodBase.GetCurrentMethod().Name, i, hue_dir));
			}
		}

		/// <summary>
		/// How to use CxImage.FromTag
		/// </summary>
		static void test12()
		{
			#region Single Channel
			{
				ushort[] addr =
				{
					0xFF, 0x00, 0xFF, 0x7F,	// 0
					0x00, 0xFF, 0xFF, 0x7F,	// 1
					0xFF, 0xFF, 0x00, 0x7F,	// 2
				};
				var arr = XIE.CxArray.From(addr);

				int width = 4;
				int height = 3;
				var model = XIE.TxModel.U16(1);
				int stride = width * model.Size;
				int depth = 8;

				var tag = new XIE.TxImage(arr.Address(), width, height, model, stride, depth);
				var src = XIE.CxImage.FromTag(tag);

				src.Save(string.Format("Results/{0}-1.png", MethodBase.GetCurrentMethod().Name));
			}
			#endregion

			#region Single Channel (Packing)
			{
				XIE.TxRGB8x3[] addr =
				{
					// 0
					new XIE.TxRGB8x3(0xFF, 0x00, 0x00),
					new XIE.TxRGB8x3(0x00, 0x00, 0x00),
					new XIE.TxRGB8x3(0xFF, 0xFF, 0xFF),
					new XIE.TxRGB8x3(0x7F, 0x7F, 0x7F),
					// 1
					new XIE.TxRGB8x3(0x00, 0x00, 0x00),
					new XIE.TxRGB8x3(0x00, 0xFF, 0x00),
					new XIE.TxRGB8x3(0xFF, 0xFF, 0xFF),
					new XIE.TxRGB8x3(0x7F, 0x7F, 0x7F),
					// 2
					new XIE.TxRGB8x3(0xFF, 0x00, 0x00),
					new XIE.TxRGB8x3(0x00, 0x00, 0x00),
					new XIE.TxRGB8x3(0x00, 0x00, 0xFF),
					new XIE.TxRGB8x3(0x7F, 0x7F, 0x7F),
				};
				var arr = XIE.CxArray.From(addr);

				int width = 4;
				int height = 3;
				var model = XIE.TxModel.U8(3);
				int stride = width * model.Size;
				int depth = 8;

				var tag = new XIE.TxImage(arr.Address(), width, height, model, stride, depth);
				var src = XIE.CxImage.FromTag(tag);

				src.Save(string.Format("Results/{0}-2.png", MethodBase.GetCurrentMethod().Name));
			}
			#endregion

			#region Multi Channel
			{
				ushort[] addr0 =
				{
					0xFF, 0x00, 0xFF, 0x7F,	// 0
					0x00, 0x00, 0xFF, 0x7F,	// 1
					0xFF, 0xFF, 0x00, 0x7F,	// 2
				};

				ushort[] addr1 =
				{
					0x00, 0x00, 0xFF, 0x7F,	// 0
					0x00, 0xFF, 0xFF, 0x7F,	// 1
					0xFF, 0xFF, 0x00, 0x7F,	// 2
				};

				ushort[] addr2 =
				{
					0x00, 0x00, 0xFF, 0x7F,	// 0
					0x00, 0x00, 0xFF, 0x7F,	// 1
					0xFF, 0xFF, 0xFF, 0x7F,	// 2
				};

				var arr0 = XIE.CxArray.From(addr0);
				var arr1 = XIE.CxArray.From(addr1);
				var arr2 = XIE.CxArray.From(addr2);
				var layer = new XIE.TxLayer(
					new IntPtr[]
					{
						arr0.Address(),
						arr1.Address(),
						arr2.Address()
					});

				int width = 4;
				int height = 3;
				var model = XIE.TxModel.U16(1);
				int channels = 3;
				int stride = width * model.Size;
				int depth = 8;

				var tag = new XIE.TxImage(layer, width, height, model, channels, stride, depth);
				var src = XIE.CxImage.FromTag(tag);

				src.Save(string.Format("Results/{0}-3.png", MethodBase.GetCurrentMethod().Name));
			}
			#endregion
		}

		/// <summary>
		/// How to use DataObject Initializer
		/// </summary>
		static void test13()
		{
			Console.WriteLine(MethodBase.GetCurrentMethod().Name);

			#region Array
			unsafe
			{
				var src = XIE.CxArray.From(new int[]
					{
						10, 11, 12, 13,
						20, 21, 22, 23,
						30, 31, 32, 33,
					});
				Console.WriteLine("{0,-10} len={1} model={2}",
					"array",
					src.Length,
					string.Format("{0}({1})", src.Model.Type, src.Model.Pack)
					);
				Console.WriteLine("Data=");
				src.Scanner().ForEach(delegate(int i, IntPtr src_addr)
				{
					var _src = (int*)src_addr.ToPointer();
					Console.Write("{0} ", *_src);
				});
				Console.WriteLine("");
			}
			#endregion

			#region Image
			unsafe
			{
				var src = XIE.CxImage.From(4, 3, new float[]
					{
						10.0f, 20.0f, 30.0f, 40.0f,
						10.1f, 20.1f, 30.1f, 40.1f,
						10.2f, 20.2f, 30.2f, 40.2f,
					});
				Console.Write("{0,-10} w,h={1},{2} model={3}",
					"image",
					src.Width,
					src.Height,
					string.Format("{0}({1})", src.Model.Type, src.Model.Pack)
					);
				Console.Write("Data=");
				src.Scanner(0).ForEach(delegate(int y, int x, IntPtr src_addr)
				{
					if (x == 0)
						Console.WriteLine("");
					var _src = (float*)src_addr.ToPointer();
					Console.Write("{0,4:F1} ", *_src);
				});
				Console.WriteLine("");
			}
			#endregion

			#region Matrix
			unsafe
			{
				var src = XIE.CxMatrix.From(3, 4, new double[]
					{
						10.0, 20.0, 30.0, 40.0,
						10.1, 20.1, 30.1, 40.1,
						10.2, 20.2, 30.2, 40.2,
					});
				Console.WriteLine("{0,-10} r,c={1},{2} model={3}",
					"matrix",
					src.Rows,
					src.Columns,
					string.Format("{0}({1})", src.Model.Type, src.Model.Pack)
					);
				Console.Write("Data=");
				src.Scanner().ForEach(delegate(int row, int col, IntPtr src_addr)
				{
					if (col == 0)
						Console.WriteLine("");
					var _src = (double*)src_addr.ToPointer();
					Console.Write("{0,4:F1} ", *_src);
				});
				Console.WriteLine("");
			}
			#endregion
		}

		/// <summary>
		/// How to use CxArrayEx
		/// </summary>
		static void test14()
		{
			// XIE.Core have not CxArrayEx
		}

		/// <summary>
		/// How to use TxScanner2D
		/// </summary>
		static void test15()
		{
			int width = 4;
			int height = 3;
			var model = XIE.ModelOf.From<byte>();
			int channels = 1;

			var src = new XIE.CxImage(width, height, model, channels);

			#region copy
			{
				src.Scanner(0).Copy(new byte[]
					{
						0xFF, 0x00, 0xFF, 0x00,
						0x00, 0xFF, 0xFF, 0x00,
						0xFF, 0xFF, 0x00, 0xFF,
					});

				src.Save(string.Format("Results/{0}-1.png", MethodBase.GetCurrentMethod().Name));
			}
			#endregion

			#region copy region
			{
				/*
					   |  0 | [1]|  2 |  3 |
					[0]|0xFF|0x00|0xFF|0x00|
					 1 |0x00|0xFF|0xFF|0x00|
					 2 |0xFF|0xFF|0x00|0xFF|
							  ↑
							|0x7F|+
							|0x7F|3rows
							|0x7F|+
							+1col+
				*/
				src.Scanner(0, new XIE.TxRectangleI(1, 0, 1, 3)).Copy(new byte[]
					{
						0x7F,
						0x7F,
						0x7F,
					});

				src.Save(string.Format("Results/{0}-2.png", MethodBase.GetCurrentMethod().Name));
			}
			#endregion

			#region foreach
			unsafe
			{
				var dst = new XIE.CxImage(width, height, XIE.ModelOf.From<ushort>(), channels);

				var dst_scan = dst.Scanner(0);
				var src_scan = src.Scanner(0);
				dst_scan.ForEach(
					src_scan,
					delegate(int y, int x, IntPtr dst_addr, IntPtr src_addr)
					{
						var _dst = (ushort*)dst_addr.ToPointer();
						var _src = (byte*)src_addr.ToPointer();
						*_dst = (ushort)(*_src);
					});

				dst.Depth = 8;
				dst.Save(string.Format("Results/{0}-3.png", MethodBase.GetCurrentMethod().Name));
			}
			#endregion
		}
	}
}
