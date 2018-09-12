
#pragma once

#ifndef __AFXWIN_H__
	#error "Please include the 'stdafx.h' before you include this file for PCH."
#endif

#include "resource.h"

class CtutorialApp : public CWinAppEx
{
public:
	CtutorialApp();

public:
	virtual BOOL InitInstance();
	virtual int ExitInstance();

public:
	BOOL  m_bHiColorIcons;

	virtual void PreLoadState();
	virtual void LoadCustomState();
	virtual void SaveCustomState();

	afx_msg void OnAppAbout();
	DECLARE_MESSAGE_MAP()
};

extern CtutorialApp theApp;
