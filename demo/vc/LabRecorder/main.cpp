/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#define _CRT_SECURE_NO_WARNINGS

#include "stdafx.h"

#include "xie_core.h"
#include "xie_high.h"
#include "xie.h"

#include "Core/xie_core_math.h"

void test01();
void DrawWindows(const xie::Media::CxScreenList& infos);
void CaptureWindow(const xie::Media::CxScreenListItem& info);

// ============================================================
/*!
	@brief	EntryPoint
*/
int main(int argc, const char* argv[])
{
	xie::Axi::Setup();

	::CreateDirectoryA( "Results", NULL );

	try
	{
		test01();
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	xie::Axi::TearDown();

	return 0;
}

// ============================================================
/*!
	@brief	ウィンドウの列挙
*/
void test01()
{
	printf("%s\n", __FUNCTION__);

	// ウィンドウリストの取得:
	xie::Media::CxScreenList list;
	list.Setup();

	// ウィンドウの描画:
	DrawWindows(list);

	// ウィンドウの選択:
	for(int i=0 ; i<list.Length() ; i++)
	{
		auto hwnd = list[i].Handle();
		auto name = list[i].Name();
		auto bounds = list[i].Bounds();
		printf("%2d: %p: %-40s [%d,%d-%d,%d]\n"
			, i
			, hwnd
			, name
			, bounds.X
			, bounds.Y
			, bounds.Width
			, bounds.Height
			);
	}
	printf("%2d: exit\n", list.Length());
	printf("input number > ");

	char ans[32] = "";
	scanf("%[0123456789]", ans);
	int index = atoi(ans);

	if (0 <= index && index < list.Length())
	{
		xie::Media::CxScreenListItem item = list[index];
		CaptureWindow(item);
	}
}

// ============================================================
/*!
	@breif	ウィンドウの描画
*/
void DrawWindows(const xie::Media::CxScreenList& infos)
{
	xie::GDI::CxCanvas canvas;

	xie::TxStatistics stat_x;
	xie::TxStatistics stat_y;
	for(int i=0 ; i<infos.Length() ; i++)
	{
		auto vertex = infos[i].Bounds().ToTrapezoid();
		stat_x += vertex.X1;
		stat_y += vertex.Y1;
		stat_x += vertex.X3;
		stat_y += vertex.Y3;
	}

	RECT desktop_rect =
	{
		(int)stat_x.Min,
		(int)stat_y.Min,
		(int)stat_x.Max,
		(int)stat_y.Max,
	};

	xie::TxSizeI size(
		desktop_rect.right-desktop_rect.left,
		desktop_rect.bottom-desktop_rect.top
		);
	canvas.Setup(NULL);
	canvas.Resize(size);

	// 背景画像の描画:
	{
		xie::GDI::CxBitmap buf(size);
		HWND hDesktop = ::GetDesktopWindow();
		HDC hdc = ::GetDC(hDesktop);
		::BitBlt(buf.GetHDC(), 0, 0, buf.Width(), buf.Height(), hdc, desktop_rect.left, desktop_rect.top, SRCCOPY);
		::ReleaseDC(hDesktop, hdc);
		canvas.DrawImage(buf);
	}

	// ウインドウ位置の描画:
	{
		const int color_num = 6;
		xie::TxRGB8x4 colors[] =
		{
			xie::TxRGB8x4(0xFF, 0x00, 0x00),
			xie::TxRGB8x4(0x00, 0xFF, 0x00),
			xie::TxRGB8x4(0x00, 0x00, 0xFF),
			xie::TxRGB8x4(0xFF, 0xFF, 0x00),
			xie::TxRGB8x4(0xFF, 0x00, 0xFF),
			xie::TxRGB8x4(0x00, 0xFF, 0xFF),
		};

		std::vector<xie::GDI::CxGdiRectangle> figures;
		std::vector<xie::GDI::CxGdiString> texts;
		for(int i=0 ; i<infos.Length() ; i++)
		{
			auto color = colors[i % color_num];

			auto bounds = infos[i].Bounds();
			auto location = xie::TxPointI(
					bounds.X - desktop_rect.left,
					bounds.Y - desktop_rect.top
				);

			xie::GDI::CxGdiRectangle figure(
				location.X,
				location.Y,
				bounds.Width,
				bounds.Height
				);
			figure.PenColor(color);
			figures.push_back(figure);

			xie::GDI::CxGdiString text;
			text.Location(location + 8);
			text.Align(xie::GDI::ExGdiTextAlign::TopLeft);
			text.Text(xie::CxString::Format(" %d ", i));
			text.FontName("Consolas");
			text.FontSize(14);
			text.PenColor(color);
			text.BkColor(xie::TxRGB8x4(0x00, 0x00, 0x00));
			text.BkEnable(true);
			texts.push_back(text);
		}
		canvas.DrawOverlay(figures, xie::GDI::ExGdiScalingMode::None);
		canvas.DrawOverlay(texts, xie::GDI::ExGdiScalingMode::None);
	}

	xie::CxImage dst;
	canvas.Flush(dst);
	dst.Save("Results/dst.png");
}

// ============================================================
/*!
	@brief	ウィンドウのキャプチャ
*/
void CaptureWindow(const xie::Media::CxScreenListItem& info)
{
	auto hwnd = info.Handle();
	auto name = info.Name();
	auto bounds = info.Bounds();
	printf("%p: %-40s [%d,%d-%d,%d]\n"
		, hwnd
		, name
		, bounds.X
		, bounds.Y
		, bounds.Width
		, bounds.Height
		);

	xie::CxStopwatch watch;
	xie::TxStatistics stat;
	auto output_file = xie::CxString::Format("Results/capture.avi");

	// 初期化.
	xie::Media::CxScreenCapture controller;
	controller.Setup(info, NULL, output_file);
	controller.FrameRate(10);

	// fps
	printf("%d fps\n", controller.FrameRate());

	// 描画処理: (※必須でない)
	auto grabber = controller.CreateGrabber();
	grabber.Notify = xie::Media::CxGrabberEvent(
		[&watch, &stat](void* sender, xie::Media::CxGrabberArgs* e)
		{
			watch.Stop();
			stat += watch.Lap;
		});
	grabber.Reset();
	grabber.Start();

	watch.Start();
	{
		// キャプチャ開始.
		controller.Start();

		// 待機.
		grabber.Wait(10 * 1000);	// 10sec
		grabber.Stop();

		// キャプチャ停止.
		controller.Stop();
	}
	watch.Stop();

	printf("Count   = %9.0f\n", stat.Count);
	printf("Mean    = %9.3f\n", stat.Mean());
	printf("Sigma   = %9.3f\n", stat.Sigma());
	printf("Min     = %9.3f\n", stat.Min);
	printf("Max     = %9.3f\n", stat.Max);
	printf("Elapsed = %9.3f\n", watch.Elapsed);
}
