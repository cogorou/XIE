
#include "stdafx.h"
#include "afxwinappex.h"
#include "afxdialogex.h"
#include "tutorial.h"
#include "MainFrm.h"

#include "xie_core.h"			// (1)
#include "xie_high.h"			// (1)
#include "xie.h"				// (1)

CtutorialApp::CtutorialApp()
{
}

CtutorialApp theApp;

BOOL CtutorialApp::InitInstance()
{
	CWinAppEx::InitInstance();

	xie::Axi::Setup();			// (2)

	//
	// TODO: Please set your company name.
	//
	SetRegistryKey(_T("Eggs Imaging Laboratory"));

	// create and show window.
	CMainFrame* pFrame = new CMainFrame;
	if (!pFrame) return FALSE;
	m_pMainWnd = pFrame;
	pFrame->LoadFrame(IDR_MAINFRAME, WS_OVERLAPPEDWINDOW | FWS_ADDTOTITLE, NULL, NULL);
	pFrame->ShowWindow(SW_SHOW);
	pFrame->UpdateWindow();
	
	return TRUE;
}

int CtutorialApp::ExitInstance()
{
	xie::Axi::TearDown();		// (3)

	return CWinAppEx::ExitInstance();
}
