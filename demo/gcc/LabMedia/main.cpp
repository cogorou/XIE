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

xie::CxStringA		Results;

void test01();
void test11();

// ==================================================
void MakeDir(const char* dir)
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

	Results = "Results";
	MakeDir(Results);

	try
	{
		test01();
		test11();
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	xie::Axi::TearDown();

	return 0;
}

// ==================================================
xie::CxString ToString(xie::TxDateTime dt)
{
	return xie::CxString::Format(
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

			auto images_dir = Results + xie::CxString::Format("/%s", __FUNCTION__);
			MakeDir(images_dir);

			for(int i=0 ; i<(int)images.size() ; i++)
			{
				auto ts = xie::TxDateTime::FromBinary(args[i].TimeStamp(), true);
				printf("%02d:%02d:%02d.%03d: index=%d\n",
					ts.Hour, ts.Minute, ts.Second, ts.Millisecond,
					args[i].Index());
				images[i].Save(images_dir + xie::CxString::Format("/image%d.png", i));
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
