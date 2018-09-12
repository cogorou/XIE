/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#include "xie_high.h"
#include <vector>

class CChildView : public CWnd
{
public:
	CChildView();
	virtual ~CChildView();

public:
	xie::GDI::CxCanvas		Canvas;
	xie::CxImage			Image;

private:
	xie::CxImage			OverlayImage;
	void DrawOverlays1();
	void DrawOverlays2();

protected:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);

protected:
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnPaint();
	DECLARE_MESSAGE_MAP()
};

