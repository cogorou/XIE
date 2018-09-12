
#pragma once

#include "xie_high.h"						// (1)

class CChildView : public CWnd
{
public:
	CChildView();
	virtual ~CChildView();

public:
	xie::GDI::CxCanvas		Canvas;			// (2)
	xie::CxImage			Image;			// (3)

protected:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);

protected:
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);	// (4) flicker prevention.
	afx_msg void OnPaint();
	DECLARE_MESSAGE_MAP()
};

