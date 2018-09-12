
#include "stdafx.h"
#include "tutorial.h"
#include "ChildView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// =================================================================
CChildView::CChildView()
{
	//
	// DEMO: Loading an image used in OnPaint.
	//
	this->Image.Load("TestFiles/cube.png");
}

CChildView::~CChildView()
{
}

BEGIN_MESSAGE_MAP(CChildView, CWnd)
	ON_WM_ERASEBKGND()	// TODO: flicker prevention.
	ON_WM_PAINT()
END_MESSAGE_MAP()

BOOL CChildView::PreCreateWindow(CREATESTRUCT& cs) 
{
	if (!CWnd::PreCreateWindow(cs))
		return FALSE;

	cs.dwExStyle |= WS_EX_CLIENTEDGE;
	cs.style &= ~WS_BORDER;
	cs.lpszClass = AfxRegisterWndClass(CS_HREDRAW|CS_VREDRAW|CS_DBLCLKS, 
		::LoadCursor(NULL, IDC_ARROW), reinterpret_cast<HBRUSH>(COLOR_WINDOW+1), NULL);

	return TRUE;
}

// =================================================================
/*
	TODO: flicker prevention
*/
BOOL CChildView::OnEraseBkgnd( CDC* pDC )
{
    return TRUE;
}

// =================================================================
/*
	TODO: rendering
*/
void CChildView::OnPaint() 
{
	CPaintDC dc(this);
	
	try
	{
		HDC hdc = dc.GetSafeHdc();

		CRect client;
		GetClientRect(&client);

		// ----------------------------------------------------------------------
		// (a) initializing.
		this->Canvas.Setup(hdc);

		// ----------------------------------------------------------------------
		// (b) changing of rendering buffer size.
		this->Canvas.Resize(client.Width(), client.Height());

		// ----------------------------------------------------------------------
		// (c) drawing of the image.
		this->Canvas.DrawImage(this->Image);

		// ----------------------------------------------------------------------
		// (d) drawing of the overlay.
		{
			// ellipse
			xie::GDI::CxGdiEllipse figure1(10, 20, 100, 50);
			figure1.PenColor(xie::TxRGB8x4(255, 0, 0));		// FF,00,00 (red)
			figure1.PenStyle(xie::GDI::ExGdiPenStyle::Solid);
			figure1.PenWidth(1);

			// linesegment
			xie::GDI::CxGdiLineSegment figure2(100, 60, 300, 200);
			figure2.PenColor(xie::TxRGB8x4(0, 255, 0));		// 00,FF,00 (green)
			figure2.PenStyle(xie::GDI::ExGdiPenStyle::Solid);
			figure2.PenWidth(3);

			// rectangle
			xie::GDI::CxGdiRectangle figure3(350, 250, 200, 150);
			figure3.PenColor(xie::TxRGB8x4(0, 0, 255));		// 00,00,FF (blue)
			figure3.PenStyle(xie::GDI::ExGdiPenStyle::Dash);
			figure3.PenWidth(5);
			figure3.BkColor(xie::TxRGB8x4(0, 0, 255, 64));	// 00,00,FF (blue) opacity 25% (64/255)
			figure3.BkEnable(true);

			xie::CxArrayEx<xie::HxModule> figures(3);
			figures[0] = figure1;
			figures[1] = figure2;
			figures[2] = figure3;

			this->Canvas.DrawOverlay(figures, xie::GDI::ExGdiScalingMode::TopLeft);
		}

		// ----------------------------------------------------------------------
		// (e) reflection to a device context.
		this->Canvas.Flush();
	}
	catch(const xie::CxException&)
	{
	}
}
