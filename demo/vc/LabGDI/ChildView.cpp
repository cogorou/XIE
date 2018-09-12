/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "stdafx.h"
#include "demo.h"
#include "ChildView.h"
#include <vector>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CChildView

// =================================================================
CChildView::CChildView()
{
	// demo: 画像ファイル読み込み.
	this->Image.Load("TestFiles/cube.png", false);
	this->OverlayImage.Load("TestFiles/bird.png");

	this->Canvas.Magnification( 1.0 );
}

// =================================================================
CChildView::~CChildView()
{
}


BEGIN_MESSAGE_MAP(CChildView, CWnd)
	ON_WM_ERASEBKGND()
	ON_WM_PAINT()
END_MESSAGE_MAP()

// =================================================================
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
BOOL CChildView::OnEraseBkgnd( CDC* pDC )
{
    return TRUE;
}

// =================================================================
void CChildView::OnPaint() 
{
	CPaintDC dc(this); // 描画のデバイス コンテキスト
	
	try
	{
		HDC hdc = dc.GetSafeHdc();

		CRect client;
		GetClientRect(&client);

		// ----------------------------------------------------------------------
		// (1) 初期化.
		this->Canvas.Setup(hdc);

		// ----------------------------------------------------------------------
		// (2) バッファのサイズ変更.
		if (this->Canvas.Width()  != client.Width() ||
			this->Canvas.Height() != client.Height())
			this->Canvas.Resize(client.Width(), client.Height());

		// ----------------------------------------------------------------------
		// (3) 背景画像の描画.
		this->Canvas.DrawImage(this->Image);

		// ----------------------------------------------------------------------
		// (4) オーバレイ図形の描画.
		this->DrawOverlays1();
		this->DrawOverlays2();

		// ----------------------------------------------------------------------
		// (5) デバイスコンテキストへの反映.
		this->Canvas.Flush();
	}
	catch(const xie::CxException&)
	{
	}

	// メッセージの描画のために CWnd::OnPaint() を呼び出さないでください。
}

// =================================================================
/*!
	@brief	オーバレイ描画 (1)

	ここでは ExGdiScalingMode:TopLeft で描画するオーバレイ図形を生成します。
	背景画像の左上を原点(0,0)として描画します。
	背景画像の表示倍率に追従します。

	因みに Center の場合は背景画像の左上を -0.5,-0.5 として描画するモードです。
	画素の中心を 0,0 として計算する画像処理の場合は、
	画素の左上が -0.5、右下が +0.5 となるので Center で描画する方が適しています。
*/
void CChildView::DrawOverlays1()
{
	// 全体が半透明の画像.
	{
		xie::GDI::CxGdiImage figure(this->OverlayImage);
		figure.Location(xie::TxPointD(320, 240));
		figure.Alpha(0.5);
		figure.AlphaFormat(false);

		this->Canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::TopLeft);
	}

	// 
	{
		std::vector<xie::GDI::CxGdiRectangle>	figures;

		// 赤い実線の矩形.(傾きあり)
		{
			xie::GDI::CxGdiRectangle figure(10, 20, 100, 50);
			figure.PenColor(xie::TxRGB8x4(255, 0, 0));			// 赤.
			figure.PenStyle(xie::GDI::ExGdiPenStyle::Solid);	// 実線.
			figure.PenWidth(1);
			figure.Angle(45);									// +45度(右回転)
			figure.BkColor(xie::TxRGB8x4(255, 0, 0, 64));		// 赤 (半透明) 64=不透明度25%
			figure.BkEnable(true);
			figures.push_back(figure);
		}

		// 青い破線の矩形.
		{
			xie::GDI::CxGdiRectangle figure(320, 240, 320, 240);
			figure.PenColor(xie::TxRGB8x4(0, 0, 255));			// 青.
			figure.PenStyle(xie::GDI::ExGdiPenStyle::Dash);		// 破線.
			figure.PenWidth(1);
			figure.Angle(0.0);
			figures.push_back(figure);
		}

		this->Canvas.DrawOverlay(figures, xie::GDI::ExGdiScalingMode::TopLeft);
	}

	// 緑色の矩形と文字列.(傾きあり)
	{
		xie::TxRectangleD rect(100, 240, 100, 50);
		double angle = -15;		// -15度(左回転)

		// 緑色の実線の矩形.
		{
			xie::GDI::CxGdiRectangle figure(rect);
			figure.PenColor(xie::TxRGB8x4(0, 255, 0));		// 緑色.
			figure.PenStyle(xie::GDI::ExGdiPenStyle::Solid);	// 実線.
			figure.PenWidth(3);
			figure.Angle(angle);

			this->Canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::TopLeft);
		}

		std::vector<xie::GDI::CxGdiStringA>	figures;

		// 矩形の上辺(横線)に平行な文字列.
		{
			xie::GDI::CxGdiStringA figure;
			figure.Text(xie::CxStringA::Format("← %.0f →", rect.Width));
			figure.Location(xie::TxPointD(rect.X, rect.Y-4));
			figure.Align(xie::GDI::ExGdiTextAlign::BottomLeft);
			figure.CodePage(932);				// Text に指定した文字列が化けない為の対策.
			figure.FontSize( 24 );
			figure.Angle(angle);
			figure.PenColor(xie::TxRGB8x4(0xFF, 0x00, 0x00));
			figure.PenWidth(4);
			figures.push_back(figure);
		}

		// 矩形の左辺(縦線)に平行な文字列.
		{
			xie::GDI::CxGdiStringA figure;
			figure.Text(xie::CxStringA::Format("← %.0f →", rect.Height));
			figure.Location(xie::TxPointD(rect.X-4, rect.Y));
			figure.Align(xie::GDI::ExGdiTextAlign::TopLeft);
			figure.CodePage(932);				// Text に指定した文字列が化けない為の対策.
			figure.FontSize( 14 );
			figure.Angle(90.0 + angle);
			figure.PenColor(xie::TxRGB8x4(0xFF, 0x00, 0x00));
			figure.PenWidth(4);
			figures.push_back(figure);
		}

		this->Canvas.DrawOverlay(figures, xie::GDI::ExGdiScalingMode::TopLeft);
	}
}

// =================================================================
/*!
	@brief	オーバレイ描画 (2)

	ここでは ExGdiScalingMode:None で描画するオーバレイ図形を生成します。
*/
void CChildView::DrawOverlays2()
{
	// 黄色の文字列.
	{
		xie::GDI::CxGdiStringW figure;
		figure.Text(L"ハロー世界♪");
		figure.Location(xie::TxPointD(0, 0));
		figure.Align(xie::GDI::ExGdiTextAlign::TopLeft);
		figure.CodePage(0);
		figure.PenColor(xie::TxRGB8x4(0xFF, 0xFF, 0x00));

		this->Canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::None);
	}

	// 画像 (画素毎の透過あり) (傾きあり)
	{
		xie::GDI::CxGdiImage figure(this->OverlayImage);
		figure.Location(xie::TxPointD(640, 480));
		figure.Scanner<xie::TxBGR8x4>().ForEach(
				[](int y, int x, xie::TxBGR8x4* _dst)
				{
					_dst->A = ((_dst->R * 0.299 + _dst->G * 0.587 * _dst->B * 0.114) < 32) ? 0x00 : 0xFF;
				}
			);
		figure.Alpha(1.0);
		figure.AlphaFormat(true);
		figure.Angle(15);	// +15度(右回転)

		this->Canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::None);
	}

	// 画像を囲む Pink の点線. (傾きあり)
	{
		xie::GDI::CxGdiRectangle figure(640, 480, 320, 240);
		figure.PenColor(xie::TxRGB8x4(255, 0, 255));		// Pink.
		figure.PenStyle(xie::GDI::ExGdiPenStyle::Dot);		// 点線.
		figure.PenWidth(1);
		figure.Angle(15);									// +15度(右回転)

		this->Canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::None);
	}
}
