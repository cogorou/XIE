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
#include "xie.h"	// Setup/TearDown

#include <vector>
#include <list>

xie::CxStringA TestFiles;
xie::CxStringA Results;

void test01();
void test11();
void test12();
void test13();
void test21();
void test22();

// ==================================================
void MakeDir(LPCSTR dir)
{
	#if defined(_MSC_VER)
	::CreateDirectoryA( dir, NULL );
	#else
	mkdir(dir, 0775);
	#endif
}

// ==================================================
/*!
	@brief	EntryPoint
*/
int main(int argc, const char* argv[])
{
	xie::Axi::Setup();

	TestFiles = "TestFiles";
	Results = "Results";
	MakeDir(Results);

	try
	{
		test01();
		test11();
		test12();
		test13();
		test21();
		test22();
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	xie::Axi::TearDown();

	return 0;
}

// ==================================================
xie::CxStringA ToString(xie::TxDateTime dt)
{
	return xie::CxStringA::Format(
			"%04d%02d%02d-%02d%02d%02d",
			dt.Year, dt.Month, dt.Day,
			dt.Hour, dt.Minute, dt.Second
		);
}

// ==================================================
/*!
	@brief	CxDeviceList
*/
void test01()
{
	printf("%s\n", __FUNCTION__);

	// --------------------------------------------------
	// A) ビデオ入力デバイスの列挙.
	try
	{
		printf("----------------------------------------\n");
		xie::Media::CxDeviceList devices;
		printf("%s: Video Input Device\n", __FUNCTION__);
		devices.Setup(xie::Media::ExMediaType::Video, xie::Media::ExMediaDir::Input);
		int length = devices.Length();
		printf("%-20s : %d\n", "Length", length);
		for(int i=0 ; i<length ; i++)
		{
			auto item = devices[i];
			printf("|- %d\n", i);
			printf("|  |- %-20s : \"%s\" (%d)\n", "Name(Index)", item.Name(), item.Index());
			printf("|  |- %-20s : \"%s\" \n", "ProductName", item.GetProductName().Address());

			auto pins = item.GetPinNames();
			printf("|  |- %-20s : %d\n", "PinNames", pins.Length());
			for(int s=0 ; s<pins.Length() ; s++)
			{
				printf("|  |  |- %2d: \"%s\"\n", s, pins[s].Address());
			}

			auto sizes = item.GetFrameSizes();
			printf("|  |- %-20s : %d\n", "FrameSizes", sizes.Length());
			for(int s=0 ; s<sizes.Length() ; s++)
			{
				printf("|  |  |- %2d: %4d x %4d\n", s, sizes[s].Width, sizes[s].Height);
			}
		}
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	// --------------------------------------------------
	// B) オーディオ入力デバイスの列挙.
	try
	{
		printf("----------------------------------------\n");
		xie::Media::CxDeviceList devices;
		printf("%s: Audio Input Device\n", __FUNCTION__);
		devices.Setup(xie::Media::ExMediaType::Audio, xie::Media::ExMediaDir::Input);
		int length = devices.Length();
		printf("%-20s : %d\n", "Length", length);
		for(int i=0 ; i<length ; i++)
		{
			auto item = devices[i];
			printf("|- %d\n", i);
			printf("|  |- %-20s : \"%s\" (%d)\n", "Name(Index)", item.Name(), item.Index());
			printf("|  |- %-20s : \"%s\" \n", "ProductName", item.GetProductName().Address());

			auto pins = item.GetPinNames();
			printf("|  |- %-20s : %d\n", "PinNames", pins.Length());
			for(int s=0 ; s<pins.Length() ; s++)
			{
				printf("|  |  |- %2d: \"%s\"\n", s, pins[s].Address());
			}
		}
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	// --------------------------------------------------
	// C) オーディオ出力デバイスの列挙.
	try
	{
		printf("----------------------------------------\n");
		xie::Media::CxDeviceList devices;
		printf("%s: Audio Output Device\n", __FUNCTION__);
		devices.Setup(xie::Media::ExMediaType::Audio, xie::Media::ExMediaDir::Output);
		int length = devices.Length();
		printf("%-20s : %d\n", "Length", length);
		for(int i=0 ; i<length ; i++)
		{
			auto item = devices[i];
			printf("|- %d\n", i);
			printf("|  |- %-20s : \"%s\" (%d)\n", "Name(Index)", item.Name(), item.Index());
			printf("|  |- %-20s : \"%s\" \n", "ProductName", item.GetProductName().Address());
		}
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
}

// ==================================================
/*!
	@brief	CxCamera
*/
void test11()
{
	printf("%s\n", __FUNCTION__);

	// --------------------------------------------------
	try
	{
		xie::CxStopwatch watch;

		// 1) 初期化.
		xie::Media::CxDeviceParam videoParam(NULL, 0, 0, {640,480});
		xie::Media::CxCamera controller;
		controller.Setup(videoParam, NULL, NULL);

		// 2) 露光開始.
		controller.Start();

		// 3) 取り込み用バッファ.
		std::vector<xie::CxImage> images;
		std::vector<xie::Media::CxGrabberArgs> args;

		// 4) 周期処理.
		watch.Start();
		{
			auto grabber = controller.CreateGrabber();
			grabber.Notify = xie::Media::CxGrabberEvent(
				[&images,&args](void* sender, xie::Media::CxGrabberArgs* e)
				{
					xie::CxImage image = *e;	// similar to CopyTo(image)
					args.push_back(*e);
					images.push_back(image);
				});
			grabber.Reset();
			grabber.Start();
			grabber.Wait(3000);
			grabber.Stop();
		}
		watch.Stop();
		printf("%s: Elapsed=%f msec\n", __FUNCTION__, watch.Elapsed);

		// 5) 露光停止.
		controller.Stop();
		watch.Stop();
		printf("%s: Stopped=%f msec\n", __FUNCTION__, watch.Elapsed);

		// E) 確認用.
		{
			printf("%-20s:[%s]\n", "ProductName", controller.GetParam<xie::CxStringA>("ProductName").Address());

			auto images_dir = Results + xie::CxStringA::Format("/%s", __FUNCTION__);
			MakeDir(images_dir);

			for(int i=0 ; i<(int)images.size() ; i++)
			{
				auto ts = xie::TxDateTime::FromBinary(args[i].TimeStamp(), true);
				printf("%02d:%02d:%02d.%03d: index=%d\n",
					ts.Hour, ts.Minute, ts.Second, ts.Millisecond,
					args[i].Index());
				images[i].Save(images_dir + xie::CxStringA::Format("/image%d.png", i));
			}
		}

		watch.Stop();
		printf("%s: Completed=%f msec\n", __FUNCTION__, watch.Elapsed);
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
}

// ==================================================
/*!
	@brief	CxCamera
*/
void test12()
{
	printf("%s\n", __FUNCTION__);

	xie::CxArrayEx<xie::CxStringA> output_files = 
	{
		xie::CxStringA::Format("%s.avi", __FUNCTION__),
		xie::CxStringA::Format("%s.asf", __FUNCTION__),
		xie::CxStringA::Format("%s.wmv", __FUNCTION__),
	};

	for(int ddd=0 ; ddd<output_files.Length() ; ddd++)
	{
		printf("%s: %d=%s\n", __FUNCTION__, ddd, output_files[ddd].Address());

		// --------------------------------------------------
		try
		{
			xie::CxStopwatch watch;

			// 1) 初期化.
			xie::Media::CxDeviceParam videoParam(NULL, 0, 0, {640,480});
			xie::Media::CxDeviceParam audioParam(NULL, 0);
			xie::CxStringA output_file = Results + "/" + output_files[ddd];

			xie::Media::CxCamera controller;
			controller.Setup(videoParam, audioParam, output_file);

			// 2) 露光開始.
			controller.Start();

			// 3) 取り込み用バッファ.
			std::vector<xie::Media::CxGrabberArgs> args;

			// 4) 周期処理.
			watch.Start();
			{
				auto grabber = controller.CreateGrabber();
				grabber.Notify = xie::Media::CxGrabberEvent(
					[&args](void* sender, xie::Media::CxGrabberArgs* e)
					{
						xie::CxImage image = *e;	// similar to CopyTo(image)
						args.push_back(*e);
					});
				grabber.Reset();
				grabber.Start();
				grabber.Wait(3000);
				grabber.Stop();
			}
			watch.Stop();
			printf("%s: Elapsed=%f msec\n", __FUNCTION__, watch.Elapsed);

			// 5) 露光停止.
			controller.Stop();
			watch.Stop();
			printf("%s: Stopped=%f msec\n", __FUNCTION__, watch.Elapsed);

			// E) 確認用.
			{
				printf("%-20s:[%s]\n", "ProductName", controller.GetParam<xie::CxStringA>("ProductName").Address());

				for(int i=0 ; i<(int)args.size() ; i++)
				{
					auto ts = xie::TxDateTime::FromBinary(args[i].TimeStamp(), true);
					printf("%02d:%02d:%02d.%03d: index=%d\n",
						ts.Hour, ts.Minute, ts.Second, ts.Millisecond,
						args[i].Index());
				}
			}

			watch.Stop();
			printf("%s: Completed=%f msec\n", __FUNCTION__, watch.Elapsed);
		}
		catch(const xie::CxException& ex)
		{
			printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
		}
	}
}

// ==================================================
/*!
	@brief	CxCamera + CxControlProperty
*/
void test13()
{
	printf("%s\n", __FUNCTION__);

	// --------------------------------------------------
	try
	{
		xie::CxStopwatch watch;

		// 1) 初期化.
		xie::Media::CxDeviceParam videoParam(NULL, 0, 0, {640,480});
		xie::Media::CxCamera controller;
		controller.Setup(videoParam, NULL, NULL);

		// 2) 制御プロパティ.
		{
			printf("\n");
			printf("%s\n", controller.GetParam<xie::CxStringA>("ProductName").Address());

			xie::CxArrayEx<xie::CxStringA> names = 
			{
				// 2-1) カメラ制御.
				"Pan", "Tilt", "Roll", "Zoom", "Exposure", "Iris", "Focus", 

				// 2-2) 映像品質.
				"Brightness", "Contrast", "Gain", "Hue", "Saturation",
				"Sharpness", "Gamma", "ColorEnable", "WhiteBalance", "BacklightCompensation", 
			};

			printf("\n");
			printf("--- %-22s: %5s, %5s, %4s, %4s, %5s ~ %5s\n",
				"Name", "Value", "Def", "Flag", "Step", "Lower", "Upper");

			for(int i=0 ; i<names.Length() ; i++)
			{
				auto cp = controller.ControlProperty(names[i]);
				if (cp.IsSupported())
				{
					auto range = cp.GetRange();
					printf("[O] %-22s: %5d, %5d, 0x%02X, %4d, %5d ~ %5d\n",
						names[i].Address(),
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
					printf("[-] %-22s:\n", names[i].Address());
				}
			}
		}
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
}

// ==================================================
/*!
	@brief	CxMediaPlayer
*/
void test21()
{
	printf("%s\n", __FUNCTION__);

	xie::CxArrayEx<xie::CxStringA> source_files = 
	{
		xie::CxStringA::Format("stopwatch_320x240.wmv"),
	};

	for(int sss=0 ; sss<source_files.Length() ; sss++)
	{
		printf("%s: %d=%s\n", __FUNCTION__, sss, source_files[sss].Address());

		try
		{
			xie::CxStopwatch watch;

			// 1) 初期化.
			xie::Media::CxMediaPlayer controller;
			xie::CxStringA source_file = TestFiles + "/" + source_files[sss];

			// -) 音声をスピーカーに出力する場合は第３引数を指定してください.
			bool use_speaker = false;
			if (use_speaker)
				controller.Setup(source_file, NULL, NULL);
			else
			{
				xie::Media::CxDeviceParam outputParam(NULL, 0);
				controller.Setup(source_file, NULL, outputParam);
			}
		
			#if defined(_MSC_VER)
			// DEBUG: 保存したファイルを graphedt で確認できます.
			{
				auto grf = xie::CxStringA::Format("%s-%d.GRF", __FUNCTION__, sss);
				controller.SetParam("SaveGraphFile", grf);
			}
			#endif

			// 2) 取り込み用バッファ.
			std::vector<xie::CxImage> images;
			std::vector<xie::Media::CxGrabberArgs> args;

			// 3) 周期処理.
			watch.Start();
			{
				auto grabber = controller.CreateGrabber();
				grabber.Notify = xie::Media::CxGrabberEvent(
					[&images,&args](void* sender, xie::Media::CxGrabberArgs* e)
					{
						xie::CxImage image = *e;	// similar to CopyTo(image)
						args.push_back(*e);
						images.push_back(image);
					});
				grabber.Reset();
				grabber.Start();

				controller.Start();					// 再生開始.
				controller.WaitForCompletion(5000);	// 待機.
				controller.Stop();					// 再生停止.
			}
			watch.Stop();
			printf("%s: Elapsed=%f msec\n", __FUNCTION__, watch.Elapsed);

			// E) 確認用.
			{
				auto images_dir = Results + xie::CxStringA::Format("/%s-%d", __FUNCTION__, sss);
				MakeDir(images_dir);

				for(int i=0 ; i<(int)images.size() ; i++)
				{
					auto ts = xie::TxDateTime::FromBinary(args[i].TimeStamp(), true);
					printf("%02d:%02d:%02d.%03d: index=%d\n",
						ts.Hour, ts.Minute, ts.Second, ts.Millisecond,
						args[i].Index());
					images[i].Save(images_dir + xie::CxStringA::Format("/image%d.png", i));
				}
			}
		}
		catch(const xie::CxException& ex)
		{
			printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
		}
	}
}

// ==================================================
/*!
	@brief	CxMediaPlayer
*/
void test22()
{
	printf("%s\n", __FUNCTION__);

	xie::CxArrayEx<xie::CxStringA> source_files = 
	{
		xie::CxStringA::Format("stopwatch_320x240.wmv"),
	};

	xie::CxArrayEx<xie::CxStringA> output_files = 
	{
		xie::CxStringA::Format("%s.avi", __FUNCTION__),
		xie::CxStringA::Format("%s.asf", __FUNCTION__),
		xie::CxStringA::Format("%s.wmv", __FUNCTION__),
	};

	for(int sss=0 ; sss<source_files.Length() ; sss++)
	{
		printf("%s: %d=%s\n", __FUNCTION__, sss, source_files[sss].Address());

		for(int ddd=0 ; ddd<output_files.Length() ; ddd++)
		{
			printf("%s: %d=%s\n", __FUNCTION__, ddd, output_files[ddd].Address());

			try
			{
				xie::CxStopwatch watch;

				// 1) 初期化.
				xie::Media::CxMediaPlayer controller;
				xie::CxStringA source_file = TestFiles + "/" + source_files[sss];
				xie::CxStringA output_file = Results + "/" + output_files[ddd];
				controller.Setup(source_file, NULL, output_file);
		
				#if defined(_MSC_VER)
				// DEBUG: 保存したファイルを graphedt で確認できます.
				{
					auto grf = xie::CxStringA::Format("%s-%d-%d.GRF", __FUNCTION__, sss, ddd);
					controller.SetParam("SaveGraphFile", grf);
				}
				#endif

				// 2) 周期処理.
				watch.Start();
				{
					controller.Start();					// 再生開始.
					controller.WaitForCompletion(5000);	// 待機.
					controller.Stop();					// 再生停止.
				}
				watch.Stop();
				printf("%s: Elapsed=%f msec\n", __FUNCTION__, watch.Elapsed);
			}
			catch(const xie::CxException& ex)
			{
				printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
			}
		}
	}
}
