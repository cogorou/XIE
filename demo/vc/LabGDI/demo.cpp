/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "stdafx.h"
#include "afxwinappex.h"
#include "afxdialogex.h"
#include "demo.h"
#include "MainFrm.h"

#include "xie_core.h"
#include "xie_high.h"
#include "xie.h"		// Setup/TearDown

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

BEGIN_MESSAGE_MAP(CLabCPP2App, CWinApp)
	ON_COMMAND(ID_APP_ABOUT, &CLabCPP2App::OnAppAbout)
END_MESSAGE_MAP()


CLabCPP2App::CLabCPP2App()
{
	SetAppID(_T("demo.AppID.NoVersion"));
}

CLabCPP2App theApp;

BOOL CLabCPP2App::InitInstance()
{
	INITCOMMONCONTROLSEX InitCtrls;
	InitCtrls.dwSize = sizeof(InitCtrls);
	InitCtrls.dwICC = ICC_WIN95_CLASSES;
	InitCommonControlsEx(&InitCtrls);

	CWinApp::InitInstance();

	// xie ライブラリを初期化します。
	xie::Axi::Setup();

	EnableTaskbarInteraction(FALSE);

	// AfxInitRichEdit2();

	SetRegistryKey(_T("Eggs Imaging Laboratory"));


	CMainFrame* pFrame = new CMainFrame;
	if (!pFrame)
		return FALSE;
	m_pMainWnd = pFrame;
	pFrame->LoadFrame(IDR_MAINFRAME,
		WS_OVERLAPPEDWINDOW | FWS_ADDTOTITLE, NULL,
		NULL);
	pFrame->ShowWindow(SW_SHOW);
	pFrame->UpdateWindow();
	return TRUE;
}

int CLabCPP2App::ExitInstance()
{
	// xie ライブラリを解放します。
	xie::Axi::TearDown();

	return CWinApp::ExitInstance();
}

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

	enum { IDD = IDD_ABOUTBOX };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);

protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()

void CLabCPP2App::OnAppAbout()
{
	CAboutDlg aboutDlg;
	aboutDlg.DoModal();
}
