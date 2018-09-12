/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#include "xie_ds.h"
#include "baseclasses/streams.h"

namespace xie
{
namespace DS
{

class CxDSScreenCapturePin : public CSourceStream
	, public IAMStreamConfig
	, public IxDSScreenCapturePin
{
protected:
	HWND				m_hDesktop;
	HWND				m_hWnd;
	HDC					m_hDC;
	HBITMAP				m_hBitmap;
	void*				m_Address;

	RECT				m_DesktopRect;
	RECT				m_WindowRect;
	int					m_FrameRate;

	LARGE_INTEGER    Time;			// time
	LARGE_INTEGER    Freq;			// frequency

	int				m_FrameNumber;
	int				m_BitDepth;			// Screen bit depth
	int				m_iRepeatTime;		// Time in msec between frames
	int				m_FramesWritten;	// To track where we are in the file
	BOOL			m_bZeroMemory;		// Do we need to clear the buffer?
	CRefTime		m_rtSampleTime;		// The time stamp for each sample
	CMediaType		m_MediaType;
	CCritSec		m_cSharedState;		// Protects our internal state
	CImageDisplay	m_Display;			// Figures out our media type for us

public:
	CxDSScreenCapturePin(HRESULT *phr, CSource *pFilter);
	~CxDSScreenCapturePin();

	DECLARE_IUNKNOWN

	// IAMStreamConfig

	virtual HRESULT STDMETHODCALLTYPE SetFormat(AM_MEDIA_TYPE* pmt);
	virtual HRESULT STDMETHODCALLTYPE GetFormat(AM_MEDIA_TYPE** ppmt);
	virtual HRESULT STDMETHODCALLTYPE GetNumberOfCapabilities(int* piCount, int* piSize);
	virtual HRESULT STDMETHODCALLTYPE GetStreamCaps(int iIndex, AM_MEDIA_TYPE** ppmt, BYTE* pSCC);

	// Quality control
	// Not implemented because we aren't going in real time.
	// If the file-writing filter slows the graph down, we just do nothing, which means
	// wait until we're unblocked. No frames are ever dropped.
	STDMETHODIMP Notify(IBaseFilter *pSelf, Quality q)
	{
		return E_FAIL;
	}

	HRESULT STDMETHODCALLTYPE Setup(RECT rect);
	HRESULT STDMETHODCALLTYPE Dispose();

	HRESULT STDMETHODCALLTYPE GetDC(HDC* hDC);
	HRESULT STDMETHODCALLTYPE GetWindowRect(RECT* rect);
	HRESULT STDMETHODCALLTYPE GetFrameSize(int* width, int* height);
	HRESULT STDMETHODCALLTYPE SetFrameRate(int value);
	HRESULT STDMETHODCALLTYPE GetFrameRate(int* value);

	// Override the version that offers exactly one media type
	HRESULT DecideBufferSize(IMemAllocator *pAlloc, ALLOCATOR_PROPERTIES *pRequest);
	HRESULT FillBuffer(IMediaSample *pSample);
	
	// Set the agreed media type and set up the necessary parameters
	HRESULT SetMediaType(const CMediaType *pMediaType);

	// Support multiple display formats
	HRESULT CheckMediaType(const CMediaType *pMediaType);
	HRESULT GetMediaType(int iPosition, CMediaType *pmt);
};

}
}
