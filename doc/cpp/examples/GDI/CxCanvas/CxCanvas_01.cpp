
#include "xie_core.h"
#include "xie_high.h"

namespace User
{

// ============================================================
void CxCanvas_01()
{
	xie::CxImage src("images/src/cube_320x240.png", true);
	xie::CxImage dst;

	xie::GDI::CxCanvas canvas;
	canvas.Setup(NULL);	// NULL = windowless
	canvas.Resize(src.Size());
	//canvas.Magnification( 0.5 );
	//canvas.BkEnable( true );
	//canvas.BkColor(xie::TxRGB8x4(0x5F, 0x7F, 0x9F));
	canvas.DrawImage(src);

	// ----------------------------------------------------------------------
	{
		// ellipse
		xie::GDI::CxGdiEllipse figure1(75, 50, 50, 25);
		figure1.PenColor(xie::TxRGB8x4(255, 0, 0));		// FF,00,00 (red)
		figure1.PenStyle(xie::GDI::ExGdiPenStyle::Solid);
		figure1.PenWidth(1);

		// linesegment
		xie::GDI::CxGdiLineSegment figure2(100, 100, 200, 200);
		figure2.PenColor(xie::TxRGB8x4(0, 255, 0));		// 00,FF,00 (green)
		figure2.PenStyle(xie::GDI::ExGdiPenStyle::Solid);
		figure2.PenWidth(3);

		// rectangle
		xie::GDI::CxGdiRectangle figure3(160, 120, 100, 90);
		figure3.PenColor(xie::TxRGB8x4(0, 0, 255));		// 00,00,FF (blue)
		figure3.PenStyle(xie::GDI::ExGdiPenStyle::Dash);
		figure3.PenWidth(5);
		figure3.BkColor(xie::TxRGB8x4(0, 0, 255, 64));	// 00,00,FF (blue) opacity 25% (64/255)
		figure3.BkEnable(true);

		xie::CxArrayEx<xie::HxModule> figures(3);
		figures[0] = figure1;
		figures[1] = figure2;
		figures[2] = figure3;

		canvas.DrawOverlay(figures, xie::GDI::ExGdiScalingMode::TopLeft);
	}

	/*
		Flush(HxModule) draws to the image object.
		If canvas has the window, you can use Flush().
	*/

	canvas.Flush(dst);

	dst.Save("Results/CxCanvas_01.png");
}

}
