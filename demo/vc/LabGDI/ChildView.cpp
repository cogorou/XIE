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
	// demo: �摜�t�@�C���ǂݍ���.
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
	CPaintDC dc(this); // �`��̃f�o�C�X �R���e�L�X�g
	
	try
	{
		HDC hdc = dc.GetSafeHdc();

		CRect client;
		GetClientRect(&client);

		// ----------------------------------------------------------------------
		// (1) ������.
		this->Canvas.Setup(hdc);

		// ----------------------------------------------------------------------
		// (2) �o�b�t�@�̃T�C�Y�ύX.
		if (this->Canvas.Width()  != client.Width() ||
			this->Canvas.Height() != client.Height())
			this->Canvas.Resize(client.Width(), client.Height());

		// ----------------------------------------------------------------------
		// (3) �w�i�摜�̕`��.
		this->Canvas.DrawImage(this->Image);

		// ----------------------------------------------------------------------
		// (4) �I�[�o���C�}�`�̕`��.
		this->DrawOverlays1();
		this->DrawOverlays2();

		// ----------------------------------------------------------------------
		// (5) �f�o�C�X�R���e�L�X�g�ւ̔��f.
		this->Canvas.Flush();
	}
	catch(const xie::CxException&)
	{
	}

	// ���b�Z�[�W�̕`��̂��߂� CWnd::OnPaint() ���Ăяo���Ȃ��ł��������B
}

// =================================================================
/*!
	@brief	�I�[�o���C�`�� (1)

	�����ł� ExGdiScalingMode:TopLeft �ŕ`�悷��I�[�o���C�}�`�𐶐����܂��B
	�w�i�摜�̍�������_(0,0)�Ƃ��ĕ`�悵�܂��B
	�w�i�摜�̕\���{���ɒǏ]���܂��B

	���݂� Center �̏ꍇ�͔w�i�摜�̍���� -0.5,-0.5 �Ƃ��ĕ`�悷�郂�[�h�ł��B
	��f�̒��S�� 0,0 �Ƃ��Čv�Z����摜�����̏ꍇ�́A
	��f�̍��オ -0.5�A�E���� +0.5 �ƂȂ�̂� Center �ŕ`�悷������K���Ă��܂��B
*/
void CChildView::DrawOverlays1()
{
	// �S�̂��������̉摜.
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

		// �Ԃ������̋�`.(�X������)
		{
			xie::GDI::CxGdiRectangle figure(10, 20, 100, 50);
			figure.PenColor(xie::TxRGB8x4(255, 0, 0));			// ��.
			figure.PenStyle(xie::GDI::ExGdiPenStyle::Solid);	// ����.
			figure.PenWidth(1);
			figure.Angle(45);									// +45�x(�E��])
			figure.BkColor(xie::TxRGB8x4(255, 0, 0, 64));		// �� (������) 64=�s�����x25%
			figure.BkEnable(true);
			figures.push_back(figure);
		}

		// ���j���̋�`.
		{
			xie::GDI::CxGdiRectangle figure(320, 240, 320, 240);
			figure.PenColor(xie::TxRGB8x4(0, 0, 255));			// ��.
			figure.PenStyle(xie::GDI::ExGdiPenStyle::Dash);		// �j��.
			figure.PenWidth(1);
			figure.Angle(0.0);
			figures.push_back(figure);
		}

		this->Canvas.DrawOverlay(figures, xie::GDI::ExGdiScalingMode::TopLeft);
	}

	// �ΐF�̋�`�ƕ�����.(�X������)
	{
		xie::TxRectangleD rect(100, 240, 100, 50);
		double angle = -15;		// -15�x(����])

		// �ΐF�̎����̋�`.
		{
			xie::GDI::CxGdiRectangle figure(rect);
			figure.PenColor(xie::TxRGB8x4(0, 255, 0));		// �ΐF.
			figure.PenStyle(xie::GDI::ExGdiPenStyle::Solid);	// ����.
			figure.PenWidth(3);
			figure.Angle(angle);

			this->Canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::TopLeft);
		}

		std::vector<xie::GDI::CxGdiStringA>	figures;

		// ��`�̏��(����)�ɕ��s�ȕ�����.
		{
			xie::GDI::CxGdiStringA figure;
			figure.Text(xie::CxStringA::Format("�� %.0f ��", rect.Width));
			figure.Location(xie::TxPointD(rect.X, rect.Y-4));
			figure.Align(xie::GDI::ExGdiTextAlign::BottomLeft);
			figure.CodePage(932);				// Text �Ɏw�肵�������񂪉����Ȃ��ׂ̑΍�.
			figure.FontSize( 24 );
			figure.Angle(angle);
			figure.PenColor(xie::TxRGB8x4(0xFF, 0x00, 0x00));
			figure.PenWidth(4);
			figures.push_back(figure);
		}

		// ��`�̍���(�c��)�ɕ��s�ȕ�����.
		{
			xie::GDI::CxGdiStringA figure;
			figure.Text(xie::CxStringA::Format("�� %.0f ��", rect.Height));
			figure.Location(xie::TxPointD(rect.X-4, rect.Y));
			figure.Align(xie::GDI::ExGdiTextAlign::TopLeft);
			figure.CodePage(932);				// Text �Ɏw�肵�������񂪉����Ȃ��ׂ̑΍�.
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
	@brief	�I�[�o���C�`�� (2)

	�����ł� ExGdiScalingMode:None �ŕ`�悷��I�[�o���C�}�`�𐶐����܂��B
*/
void CChildView::DrawOverlays2()
{
	// ���F�̕�����.
	{
		xie::GDI::CxGdiStringW figure;
		figure.Text(L"�n���[���E��");
		figure.Location(xie::TxPointD(0, 0));
		figure.Align(xie::GDI::ExGdiTextAlign::TopLeft);
		figure.CodePage(0);
		figure.PenColor(xie::TxRGB8x4(0xFF, 0xFF, 0x00));

		this->Canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::None);
	}

	// �摜 (��f���̓��߂���) (�X������)
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
		figure.Angle(15);	// +15�x(�E��])

		this->Canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::None);
	}

	// �摜���͂� Pink �̓_��. (�X������)
	{
		xie::GDI::CxGdiRectangle figure(640, 480, 320, 240);
		figure.PenColor(xie::TxRGB8x4(255, 0, 255));		// Pink.
		figure.PenStyle(xie::GDI::ExGdiPenStyle::Dot);		// �_��.
		figure.PenWidth(1);
		figure.Angle(15);									// +15�x(�E��])

		this->Canvas.DrawOverlay(figure, xie::GDI::ExGdiScalingMode::None);
	}
}
