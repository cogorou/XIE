
#include "stdafx.h"
#include "tutorial.h"
#include "ChildView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// =================================================================
CChildView::CChildView()
{
	this->Image.Load("TestFiles/cube.png");	// (1) for demo.
}

CChildView::~CChildView()
{
}

BEGIN_MESSAGE_MAP(CChildView, CWnd)
	ON_WM_ERASEBKGND()							// (2) flicker prevention.
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
	(2) flicker prevention
*/
BOOL CChildView::OnEraseBkgnd( CDC* pDC )
{
    return TRUE;
}

// =================================================================
/*
	(3) rendering
*/
void CChildView::OnPaint() 
{
	CPaintDC dc(this);
	
	// 
	// TODO: implementation of rendering.
	// 
}
