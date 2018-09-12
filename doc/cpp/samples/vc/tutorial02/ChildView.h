
#pragma once

//
// TODO: Please include headers.
//
#include "xie_high.h"

class CChildView : public CWnd
{
public:
	CChildView();
	virtual ~CChildView();

public:
	//
	// TODO: Declarations.
	//
	xie::GDI::CxCanvas		Canvas;
	xie::CxImage			Image;

protected:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);

protected:
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);	// TODO: flicker prevention.
	afx_msg void OnPaint();
	DECLARE_MESSAGE_MAP()
};

