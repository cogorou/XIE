/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "stdafx.h"
#include "xie_ds.h"
#include "CxDSScreenCapturePin.h"
#include "CxDSScreenCaptureFilter.h"
#include "baseclasses/streams.h"
#include "baseclasses/dshowutil.h"

#pragma comment (lib,"msimg32.lib" )
#pragma comment (lib,"strmiids.lib")
#pragma comment (lib,"winmm.lib")

using namespace xie;
using namespace xie::DS;

// =================================================================
// フィルタ情報の宣言
// refer to: https://msdn.microsoft.com/ja-jp/library/cc353819.aspx

#define g_wszCxDSScreenCaptureFilter	L"CxDSScreenCaptureFilter"

const AMOVIESETUP_MEDIATYPE sudCxDSScreenCapturePinTypes =
{
    &MEDIATYPE_Video,       // Major type
    &MEDIASUBTYPE_NULL      // Minor type
};

const AMOVIESETUP_PIN sudCxDSScreenCapturePin = 
{
    L"Output",		// Obsolete, not used.
    FALSE,			// Is this pin rendered?
    TRUE,			// Is it an output pin?
    FALSE,			// Can the filter create zero instances?
    FALSE,			// Does the filter create multiple instances?
    &CLSID_NULL,	// Obsolete.
    NULL,			// Obsolete.
    1,				// Number of media types.
    &sudCxDSScreenCapturePinTypes	// Pointer to media types.
};

const AMOVIESETUP_FILTER sudCxDSScreenCaptureFilter =
{
    &CLSID_CxDSScreenCaptureFilter,		// Filter CLSID
    g_wszCxDSScreenCaptureFilter,		// String name
    MERIT_DO_NOT_USE,					// Filter merit
    1,									// Number pins
    &sudCxDSScreenCapturePin			// Pin details
};

// =================================================================
// ファクトリテンプレートの宣言
// refer to: https://msdn.microsoft.com/ja-jp/library/cc353831.aspx

CFactoryTemplate g_Templates[] =
{
	// CxDSScreenCapture
    {
		g_wszCxDSScreenCaptureFilter,
		&CLSID_CxDSScreenCaptureFilter,
		CxDSScreenCaptureFilter::CreateInstance,
		NULL,
		&sudCxDSScreenCaptureFilter
	},
};

int g_cTemplates = sizeof(g_Templates) / sizeof(g_Templates[0]);

// =================================================================
// DllRegisterServer の実装
// refer to: https://msdn.microsoft.com/ja-jp/library/cc357210.aspx
STDAPI DllRegisterServer()
{
    return AMovieDllRegisterServer2( TRUE );
}

// =================================================================
// フィルタの登録解除
// refer to https://msdn.microsoft.com/ja-jp/library/cc371059.aspx
STDAPI DllUnregisterServer()
{
    return AMovieDllRegisterServer2( FALSE );
}

//
// DllEntryPoint
//
extern "C" BOOL WINAPI DllEntryPoint(HINSTANCE, ULONG, LPVOID);

// =================================================================
BOOL APIENTRY DllMain(HMODULE hModule, DWORD  dwReason, LPVOID lpReserved)
{
	return DllEntryPoint((HINSTANCE)(hModule), dwReason, lpReserved);
}
