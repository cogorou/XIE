/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _XIE_DS_H_INCLUDED_
#define _XIE_DS_H_INCLUDED_

// ============================================================
// Header

// ============================================================
#if defined(_MSC_VER)

#include <windows.h>
#include <tchar.h>
#include <dshow.h>
#include <initguid.h>

namespace xie
{
namespace DS
{

// ============================================================
// GUID

// {10CB259A-AC45-496C-9EA5-DA2B8BDB6E22}
DEFINE_GUID(IID_IxDSScreenCapturePin, 
0x10CB259A, 0xAC45, 0x496C, 0x9E, 0xA5, 0xDA, 0x2B, 0x8B, 0xDB, 0x6E, 0x22);

// {74EB5751-A444-4798-AC1A-05175E2CCE8F}
DEFINE_GUID(CLSID_CxDSScreenCaptureFilter, 
0x74EB5751, 0xA444, 0x4798, 0xAC, 0x1A, 0x5, 0x17, 0x5E, 0x2C, 0xCE, 0x8F);

// ============================================================
// Interface

// =================================================================
#if defined(__cplusplus) && !defined(CINTERFACE)
	MIDL_INTERFACE("10CB259A-AC45-496C-9EA5-DA2B8BDB6E22")
	IxDSScreenCapturePin : public IUnknown
	{
		virtual HRESULT STDMETHODCALLTYPE Setup(_In_ RECT rect) = 0;
		virtual HRESULT STDMETHODCALLTYPE GetDC(_Out_ HDC* hDC) = 0;
		virtual HRESULT STDMETHODCALLTYPE GetWindowRect(RECT* rect) = 0;
		virtual HRESULT STDMETHODCALLTYPE GetFrameSize(_Out_ int* width, _Out_ int* height) = 0;
		virtual HRESULT STDMETHODCALLTYPE SetFrameRate(_In_ int value) = 0;
		virtual HRESULT STDMETHODCALLTYPE GetFrameRate(_Out_ int* value) = 0;
	};
#endif

}	// DS
}	// xie

#endif	// _MSC_VER

#endif
