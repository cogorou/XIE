/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _API_DS_COM_H_INCLUDED_
#define _API_DS_COM_H_INCLUDED_

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

// {6B652FFF-11FE-4fce-92AD-0266B5D7C78F}
DEFINE_GUID(IID_SampleGrabber,
0x6B652FFF, 0x11FE, 0x4FCE, 0x92, 0xAD, 0x02, 0x66, 0xB5, 0xD7, 0xC7, 0x8F);

// {C1F400A0-3F08-11D3-9F0B-006008039E37}
DEFINE_GUID(CLSID_SampleGrabber,
0xC1F400A0, 0x3F08, 0x11D3, 0x9F, 0x0B, 0x00, 0x60, 0x08, 0x03, 0x9E, 0x37);

// {C1F400A4-3F08-11D3-9F0B-006008039E37}
DEFINE_GUID(CLSID_NullRenderer,
0xC1F400A4, 0x3F08, 0x11D3, 0x9F, 0x0B, 0x00, 0x60, 0x08, 0x03, 0x9E, 0x37);

// for MP4

// 61F47056-E400-43D3-AF1E-AB7DFFD4C4AD		MP4 Splitter
DEFINE_GUID(CLSID_MPEG4Splitter, 0x61F47056, 0xE400, 0x43D3, 0xAF, 0x1E, 0xAB, 0x7D, 0xFF,0xD4, 0xC4, 0xAD);

// 212690FB-83E5-4526-8FD7-74478B7939CD		Microsoft DTV-DVD Video Decoder
DEFINE_GUID(CLSID_CMPEG2VidDecoderDS, 0x212690FB, 0x83E5, 0x4526, 0x8F, 0xD7, 0x74, 0x47, 0x8B, 0x79, 0x39, 0xCD);

// E1F1A0B8-BEEE-490D-BA7C-066C40B5E2B9		Microsoft DTV-DVD Audio Decoder
DEFINE_GUID(CLSID_CMPEG2AudDecoderDS, 0xE1F1A0B8, 0xBEEE, 0x490D, 0xBA, 0x7C, 0x06, 0x6C, 0x40, 0xB5, 0xE2, 0xB9);

// ============================================================
// Interface

// =================================================================
#if defined(__cplusplus) && !defined(CINTERFACE)
	struct IxDSGraphBuilderProvider
	{
		virtual IGraphBuilder* GraphBuilder() const = 0;
	};
#endif

// =================================================================
#if defined(__cplusplus) && !defined(CINTERFACE)
	MIDL_INTERFACE("0579154A-2B53-4994-B0D0-E773148EFF85")	// /*interface*/
	ISampleGrabberCB : public IUnknown
	{
		virtual HRESULT STDMETHODCALLTYPE SampleCB(double SampleTime, IMediaSample *pSample) = 0;
		virtual HRESULT STDMETHODCALLTYPE BufferCB(double SampleTime, BYTE *pBuffer, LONG BufferLen) = 0;
		virtual ~ISampleGrabberCB() {}
	};
#endif

// =================================================================
#if defined(__cplusplus) && !defined(CINTERFACE)
	MIDL_INTERFACE("6B652FFF-11FE-4fce-92AD-0266B5D7C78F")	// /*interface*/
	ISampleGrabber : public IUnknown
	{
		virtual HRESULT STDMETHODCALLTYPE SetOneShot(BOOL OneShot) = 0;
		virtual HRESULT STDMETHODCALLTYPE SetMediaType(const AM_MEDIA_TYPE *pType) = 0;
		virtual HRESULT STDMETHODCALLTYPE GetConnectedMediaType(AM_MEDIA_TYPE *pType) = 0;
		virtual HRESULT STDMETHODCALLTYPE SetBufferSamples(BOOL BufferThem) = 0;
		virtual HRESULT STDMETHODCALLTYPE GetCurrentBuffer(LONG *pBufferSize, LONG *pBuffer) = 0;
		virtual HRESULT STDMETHODCALLTYPE GetCurrentSample(IMediaSample **ppSample) = 0;
		virtual HRESULT STDMETHODCALLTYPE SetCallback(ISampleGrabberCB *pCallback, LONG WhichMethodToCallback) = 0;
		virtual ~ISampleGrabber() {}
	};
#endif

}	// DS
}	// xie

#endif	// _MSC_VER

#endif
