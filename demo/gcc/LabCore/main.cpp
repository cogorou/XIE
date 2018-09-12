/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#if defined(_MSC_VER)
	#include "stdafx.h"
#else
	#include <stdio.h>
	#include <stdlib.h>
	#include <stddef.h>
#endif

#include "xie_core.h"
#include "xie_high.h"
#include "xie.h"

#include "Core/xie_core_math.h"

void test01();
void test02();
void test03();
void test04();
void test11();
void test12();
void test13();
void test14();
void test15();

// ============================================================
/*!
	@brief	EntryPoint
*/
int main(int argc, const char* argv[])
{
	xie::Axi::Setup();

	#if defined(_MSC_VER)
	::CreateDirectoryA( "Results", NULL );
	#else
	mkdir("Results", 0775);
	#endif

	try
	{
		test01();
		test02();
		test03();
		test04();
		test11();
		test12();
		test13();
		test14();
		test15();
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): %s(%d) %s\n", ex.File(), ex.Line(), xie::ToString(ex.Code()).Address(), ex.Code(), ex.Function());
	}

	xie::Axi::TearDown();

	return 0;
}

// ============================================================
/*!
	@brief	How to use Effect (RgbToGray)
*/
void test01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxStopwatch watch;
	xie::CxImage src("TestFiles/cube.png", true);	// unpacking (multi channel)
	xie::CxImage dst;
	watch.Start();
	xie::Effectors::CxRgbToGray().Execute(src, dst);
	watch.Stop();
	printf("%-20s: %9.3f msec\n", __FUNCTION__, watch.Lap);

	dst.Save(xie::CxString::Format("Results/%s.png", __FUNCTION__));
}

// ============================================================
/*!
	@brief	How to use Filter (Affine)
*/
void test02()
{
	printf("%s\n", __FUNCTION__);

	xie::CxStopwatch watch;
	xie::CxImage src("TestFiles/cube.png", true);	// unpacking (multi channel)
	xie::CxImage dst;
	xie::CxMatrix matrix = 
		xie::CxMatrix::PresetScale(0.5, 0.5) *
		xie::CxMatrix::PresetRotate(+45.0, src.Width()*0.5, src.Height()*0.5);
	watch.Start();
	dst.Filter().Affine(src, matrix, 0);
	watch.Stop();
	printf("%-20s: %9.3f msec\n", __FUNCTION__, watch.Lap);

	dst.Save(xie::CxString::Format("Results/%s.png", __FUNCTION__));
}

// ============================================================
/*!
	@brief	How to use Filter (Scale)
*/
void test03()
{
	printf("%s\n", __FUNCTION__);

	xie::CxStopwatch watch;
	xie::CxImage src("TestFiles/cube.png", false);
	xie::CxImage dst;
	watch.Start();
	dst.Filter().Scale(src, 0.5, 0.5, 2);
	watch.Stop();
	printf("%-20s: %9.3f msec\n", __FUNCTION__, watch.Lap);

	dst.Save(xie::CxString::Format("Results/%s.png", __FUNCTION__));
}

// ============================================================
/*!
	@brief	How to use Effect (HsvConverter)
*/
void test04()
{
	printf("%s\n", __FUNCTION__);

	xie::CxStopwatch watch;
	xie::CxImage src("TestFiles/cube.png", false);
	xie::CxImage dst;
	xie::Effectors::CxHsvConverter effector(0, 45, 0.5, 0.75);
	watch.Start();
	effector.Execute(src, dst);
	watch.Stop();
	printf("%-20s: %9.3f msec\n", __FUNCTION__, watch.Lap);

	dst.Save(xie::CxString::Format("Results/%s.png", __FUNCTION__));
}

// ============================================================
/*!
	@brief	How to use Scanner (RgbToHsv)
*/
void test11()
{
	printf("%s\n", __FUNCTION__);

	xie::CxStopwatch watch;
	xie::CxImage src("TestFiles/cube.png", false);	// packing (single channel)
	xie::CxImage dst(src.ImageSize());
	xie::CxImage hsv(src.Width(), src.Height(), xie::TxModel::F64(3), 1);

	for(int i=0 ; i<4 ; i++)
	{
		watch.Reset();

		int hue_dir = i * 60;	// 0=0,1=60,2=120,3=180
		int parallel_num = 0;	// OpenMP

		// HSV Å© RGB
		watch.Start();
		{
			typedef unsigned char	TS;
			typedef double			TD;
			xie::TxScanner2D<TS> src_scan = src.Scanner<TS>(0);
			xie::TxScanner2D<TD> hsv_scan = hsv.Scanner<TD>(0);
			hsv_scan.Parallel = parallel_num;
			hsv_scan.ForEach(src_scan,
				[hue_dir](int y, int x, TD* _hsv, TS* _src)
				{
					double dR = (double)_src[0] / 255;
					double dG = (double)_src[1] / 255;
					double dB = (double)_src[2] / 255;

					double max_v = XIE_MAX(dR, XIE_MAX(dG, dB));
					double min_v = XIE_MIN(dR, XIE_MIN(dG, dB));
					double delta = (max_v - min_v);

					double dH = 0;
					double dS = 0;
					double dV = max_v;

					//dS = delta;			// â~êçÉÇÉfÉã.
					if (max_v != 0)
						dS = delta / max_v;	// â~íåÉÇÉfÉã.

					if (delta == 0)			dH = 0;
					else if (max_v == dR)	dH = ((dG - dB) / delta + 0) * 60;
					else if (max_v == dG)	dH = ((dB - dR) / delta + 2) * 60;
					else if (max_v == dB)	dH = ((dR - dG) / delta + 4) * 60;

					// êFëä(Hue)ÇÃïœä∑.
					if (delta != 0)
						dH += hue_dir;

					// êFëä(Hue)ÇÃê≥ãKâª.
					dH = xie::_mod(dH, 360);
					if (dH < 0)
						dH += 360;

					_hsv[0] = (TD)dH;	// 0~359
					_hsv[1] = (TD)dS;	// 0.0~1.0
					_hsv[2] = (TD)dV;	// 0.0~1.0
				});
		}
		watch.Stop();

		// RGB Å© HSV
		watch.Start();
		{
			typedef double			TS;
			typedef unsigned char	TD;
			xie::TxScanner2D<TS> hsv_scan = hsv.Scanner<TS>(0);
			xie::TxScanner2D<TD> rgb_scan = dst.Scanner<TD>(0);
			rgb_scan.Parallel = parallel_num;
			rgb_scan.ForEach(hsv_scan,
				[](int y, int x, TD* _rgb, TS* _hsv)
				{
					double dH = _hsv[0];
					double dS = _hsv[1];
					double dV = _hsv[2];

					double dR = dV;
					double dG = dV;
					double dB = dV;

					if (0 != dS)
					{
						int iH = (int)(dH / 60);

						double dF = (dH / 60) - iH;
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

					_rgb[0] = xie::saturate_cast<TD>(dR * 255);
					_rgb[1] = xie::saturate_cast<TD>(dG * 255);
					_rgb[2] = xie::saturate_cast<TD>(dB * 255);
				});
		}
		watch.Stop();
		printf("%-20s: %9.3f msec : (%02d) RGB +- HSV +- RGB\n", __FUNCTION__, watch.Elapsed, i);

		dst.Save(xie::CxString::Format("Results/%s-%02d-hue+%03d.png", __FUNCTION__, i, hue_dir));
	}
}

// ============================================================
/*!
	@brief	How to use CxImage::FromTag
*/
void test12()
{
	printf("%s\n", __FUNCTION__);

	// Single Channel
	{
		unsigned short addr[] =
		{
			0xFF, 0x00, 0xFF, 0x7F,	// 0
			0x00, 0xFF, 0xFF, 0x7F,	// 1
			0xFF, 0xFF, 0x00, 0x7F,	// 2
		};

		int width = 4;
		int height = 3;
		auto model = xie::TxModel::U16(1);
		int stride = width * model.Size();
		int depth = 8;

		xie::TxImage tag(addr, width, height, model, stride, depth);
		xie::CxImage src = xie::CxImage::FromTag(tag);

		src.Save(xie::CxString::Format("Results/%s-1.png", __FUNCTION__));
	}

	// Single Channel (Packing)
	{
		xie::TxRGB8x3 addr[] =
		{
			// 0
			xie::TxRGB8x3(0xFF, 0x00, 0x00),
			xie::TxRGB8x3(0x00, 0x00, 0x00),
			xie::TxRGB8x3(0xFF, 0xFF, 0xFF),
			xie::TxRGB8x3(0x7F, 0x7F, 0x7F),
			// 1
			xie::TxRGB8x3(0x00, 0x00, 0x00),
			xie::TxRGB8x3(0x00, 0xFF, 0x00),
			xie::TxRGB8x3(0xFF, 0xFF, 0xFF),
			xie::TxRGB8x3(0x7F, 0x7F, 0x7F),
			// 2
			xie::TxRGB8x3(0xFF, 0x00, 0x00),
			xie::TxRGB8x3(0x00, 0x00, 0x00),
			xie::TxRGB8x3(0x00, 0x00, 0xFF),
			xie::TxRGB8x3(0x7F, 0x7F, 0x7F),
		};

		int width = 4;
		int height = 3;
		auto model = xie::TxModel::U8(3);
		int stride = width * model.Size();
		int depth = 8;

		xie::TxImage tag(addr, width, height, model, stride, depth);
		xie::CxImage src = xie::CxImage::FromTag(tag);

		src.Save(xie::CxString::Format("Results/%s-2.png", __FUNCTION__));
	}

	// Multi Channel
	{
		unsigned short addr0[] =
		{
			0xFF, 0x00, 0xFF, 0x7F,	// 0
			0x00, 0x00, 0xFF, 0x7F,	// 1
			0xFF, 0xFF, 0x00, 0x7F,	// 2
		};

		unsigned short addr1[] =
		{
			0x00, 0x00, 0xFF, 0x7F,	// 0
			0x00, 0xFF, 0xFF, 0x7F,	// 1
			0xFF, 0xFF, 0x00, 0x7F,	// 2
		};

		unsigned short addr2[] =
		{
			0x00, 0x00, 0xFF, 0x7F,	// 0
			0x00, 0x00, 0xFF, 0x7F,	// 1
			0xFF, 0xFF, 0xFF, 0x7F,	// 2
		};

		int width = 4;
		int height = 3;
		auto model = xie::TxModel::U16(1);
		int channels = 3;
		int stride = width * model.Size();
		int depth = 8;

		unsigned short* addrs[] = {addr0, addr1, addr2};
		xie::TxImage tag(xie::TxLayer((void**)addrs, 3), width, height, model, channels, stride, depth);
		xie::CxImage src = xie::CxImage::FromTag(tag);

		src.Save(xie::CxString::Format("Results/%s-3.png", __FUNCTION__));
	}
}

// ============================================================
/*!
	@brief	How to use DataObject Initializer
*/
void test13()
{
	printf("%s\n", __FUNCTION__);

	// -----
	{
		auto src = xie::CxArray::From(
			{
				10, 20, 30, 40,
				10, 20, 30, 40,
				10, 20, 30, 40,
			});
		printf("%-10s: len=%d model=%s\n",
			"array",
			src.Length(),
			xie::ToString(src.Model()).Address()
			);
		printf("Data=\n");
		src.Scanner<int>().ForEach([](int i, int* _src)
		{
			printf("%d ", *_src);
		});
		printf("\n");
	}

	// -----
	{
		auto src = xie::CxImage::From<float>(4, 3,
			{
				10.0f, 20.0f, 30.0f, 40.0f,
				10.1f, 20.1f, 30.1f, 40.1f,
				10.2f, 20.2f, 30.2f, 40.2f,
			});
		printf("%-10s: w,h=%d,%d model=%s\n",
			"image",
			src.Width(),
			src.Height(),
			xie::ToString(src.Model()).Address()
			);
		printf("Data=");
		src.Scanner<float>(0).ForEach([](int y, int x, float* _src)
		{
			if (x == 0)
				printf("\n");
			printf("%4.1f ", *_src);
		});
		printf("\n");
	}

	// -----
	{
		auto src = xie::CxMatrix::From(3, 4,
			{
				10.0, 20.0, 30.0, 40.0,
				10.1, 20.1, 30.1, 40.1,
				10.2, 20.2, 30.2, 40.2,
			});
		printf("%-10s: r,c=%d,%d model=%s\n",
			"matrix",
			src.Rows(),
			src.Columns(),
			xie::ToString(src.Model()).Address()
			);
		printf("Data=");
		src.Scanner<double>().ForEach([](int row, int col, double* _src)
		{
			if (col == 0)
				printf("\n");
			printf("%4.1f ", *_src);
		});
		printf("\n");
	}
}

// ============================================================
/*!
	@brief	How to use CxArrayEx
*/
void test14()
{
	typedef int		TE;

	std::vector<TE>		src = { 10, 20, 30 };
	xie::CxArrayEx<TE>	src1 = src;
	xie::CxArrayEx<TE>	src2 = { 40, 50, 60, 70 };

	// concatenate
	{
		xie::CxArrayEx<TE>	dst = src1 + src2;
		printf("dst=");
		for(int i=0 ; i<dst.Length() ; i++)
			printf("%d,", dst[i]);
		printf("\n");
	}

	// cast operator
	{
		std::vector<TE>		dst1 = src2;
		printf("dst1=");
		for(int i=0 ; i<(int)dst1.size() ; i++)
			printf("%d,", dst1[i]);
		printf("\n");
	}

	// extract
	{
		xie::CxArrayEx<TE>	part = src2.Extract(1, 2);
		printf("part=");
		for(int i=0 ; i<part.Length() ; i++)
			printf("%d,", part[i]);
		printf("\n");
	}
}

// ============================================================
/*!
	@brief	How to use TxScanner2D
*/
void test15()
{
	typedef unsigned char	TS;
	int width = 4;
	int height = 3;
	auto model = xie::ModelOf<TS>();
	int channels = 1;

	xie::CxImage src(width, height, model, channels);

	// copy
	{
		src.Scanner<TS>(0).Copy(
			{
				0xFF, 0x00, 0xFF, 0x00,
				0x00, 0xFF, 0xFF, 0x00,
				0xFF, 0xFF, 0x00, 0xFF,
			});

		src.Save(xie::CxString::Format("Results/%s-1.png", __FUNCTION__));
	}

	// copy region
	{
		/*
			   |  0 | [1]|  2 |  3 |
			[0]|0xFF|0x00|0xFF|0x00|
			 1 |0x00|0xFF|0xFF|0x00|
			 2 |0xFF|0xFF|0x00|0xFF|
			          Å™
			        |0x7F|+
			        |0x7F|3rows
			        |0x7F|+
				    +1col+
		*/
		src.Scanner<TS>(0, {1, 0, 1, 3}).Copy(
			{
				0x7F,
				0x7F,
				0x7F,
			});

		src.Save(xie::CxString::Format("Results/%s-2.png", __FUNCTION__));
	}

	// foreach
	{
		typedef unsigned short	TD;

		xie::CxImage dst(width, height, xie::ModelOf<TD>(), channels);

		auto dst_scan = dst.Scanner<TD>(0);
		auto src_scan = src.Scanner<TS>(0);
		dst_scan.ForEach(
			src_scan,
			[](int y, int x, TD* _dst, TS* _src)
			{
				*_dst = static_cast<TD>(*_src);
			});

		dst.Depth(8);
		dst.Save(xie::CxString::Format("Results/%s-3.png", __FUNCTION__));
	}

	// cast operator
	{
		xie::CxArrayEx<TS> data1 = src.Scanner<TS>(0);
		printf("data1=");
		for(int i=0 ; i<data1.Length() ; i++)
			printf("%d,", data1[i]);
		printf("\n");
	}

	// cast operator
	{
		std::vector<TS> data2 = src.Scanner<TS>(0);
		printf("data2=");
		for(int i=0 ; i<(int)data2.size() ; i++)
			printf("%d,", data2[i]);
		printf("\n");
	}
}
