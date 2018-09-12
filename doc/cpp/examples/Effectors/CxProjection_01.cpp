
#include "xie_core.h"
#include "xie_high.h"
#include <stdio.h>

namespace User
{

void CxProjection_01_Dump(const xie::CxImage& src, const xie::CxArray& result, int dir, int ch);

// ============================================================
void CxProjection_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/cube_320x240.png");
	xie::CxArray result;
	xie::Effectors::CxProjection effector;

	int pxc = src.Model().Pack * src.Channels();

	for(int dir=0 ; dir<2 ; dir++)
	{
		// parameter settings
		switch(dir)
		{
		case 0: effector.ScanDir = xie::ExScanDir::X; break;
		case 1: effector.ScanDir = xie::ExScanDir::Y; break;
		}

		for(int ch=0 ; ch<pxc ; ch++)
		{
			// parameter settings
			effector.Channel = ch;

			// execution
			result.Dispose();
			effector.Execute(src, result);

			// for DEBUG
			CxProjection_01_Dump(src, result, dir, ch);
		}
	}
}

// ============================================================
void CxProjection_01_Dump(const xie::CxImage& src, const xie::CxArray& result, int dir, int ch)
{
	xie::GDI::CxCanvas canvas;

	if (dir == 0)
	{
		printf("(dir=%d,ch=%d) dst.Length = %d, src.Height = %d\n", dir, ch, result.Length(), src.Height());

		canvas.Setup(NULL);
		canvas.Resize(256, src.Height());
		canvas.Clear();
	}
	else
	{
		printf("(dir=%d,ch=%d) dst.Length = %d, src.Width = %d\n", dir, ch, result.Length(), src.Width());

		canvas.Setup(NULL);
		canvas.Resize(src.Width(), 256);
		canvas.Clear();
	}

	// graph
	{
		xie::GDI::CxGdiPolyline polyline;
		polyline.Resize(result.Length());
		polyline.PenColor(xie::TxRGB8x4(0xFF, 0x00, 0x00));

		auto dst_scan = polyline.Scanner();
		auto src_scan = result.Scanner<xie::TxStatistics>();

		dst_scan.ForEach(src_scan, [dir](int iii, xie::TxPointD* dst_addr, xie::TxStatistics* src_addr)
		{
			auto value = src_addr->Mean();

			switch(dir)
			{
			case 0: *dst_addr = xie::TxPointD(value, iii); break;
			case 1: *dst_addr = xie::TxPointD(iii, value); break; 
			}
		});

		canvas.DrawOverlay(polyline, xie::GDI::ExGdiScalingMode::None);
	}

	xie::CxImage image;
	canvas.Flush(image);
	image.Save(xie::CxStringA::Format("Results/CxProjection_01_dir%d_ch%d.png", dir, ch));
}


}
