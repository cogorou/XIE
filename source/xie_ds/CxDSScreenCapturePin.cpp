/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxDSScreenCapturePin.h"
#include "baseclasses/dshowutil.h"

namespace xie
{
namespace DS
{

// UNITS = 10 ^ 7  
// UNITS / 30 = 30 fps;
// UNITS / 20 = 20 fps, etc

// =================================================================
CxDSScreenCapturePin::CxDSScreenCapturePin(HRESULT *phr, CSource *pFilter)
		: CSourceStream(NAME("CxDSScreenCapturePin"), phr, pFilter, L"Out")
{
	// The main point of this sample is to demonstrate how to take a DIB
	// in host memory and insert it into a video stream. 

	// To keep this sample as simple as possible, we just read the desktop image
	// from a file and copy it into every frame that we send downstream.
	//
	// In the filter graph, we connect this filter to the AVI Mux, which creates 
	// the AVI file with the video frames we pass to it. In this case, 
	// the end result is a screen capture video (GDI images only, with no
	// support for overlay surfaces).

	m_hDesktop		= NULL;
	m_hWnd			= NULL;
	m_hDC			= NULL;
	m_hBitmap		= NULL;
	m_Address		= NULL;

	memset(&m_DesktopRect, 0, sizeof(m_DesktopRect));
	memset(&m_WindowRect, 0, sizeof(m_WindowRect));

	m_FrameRate		= 10;	// Capture and display desktop 10 times per second

	Time.QuadPart = 0;
	Freq.QuadPart = 0;

	m_BitDepth		= 32;
	m_FrameNumber	= 0;
	m_FramesWritten	= 0;
	m_bZeroMemory	= 0;
}

// =================================================================
CxDSScreenCapturePin::~CxDSScreenCapturePin()
{	
	DbgLog((LOG_TRACE, 3, TEXT("Frames written %d"), m_FrameNumber));
	Dispose();
}

// IAMStreamConfig

// =================================================================
// https://msdn.microsoft.com/ja-jp/library/Cc370544.aspx
HRESULT CxDSScreenCapturePin::SetFormat(AM_MEDIA_TYPE* pmt)
{
	CheckPointer(pmt, E_POINTER);
	HRESULT hr;

	// ストリーミングが開始または停止の最中でないように、フィルタ状態ロックを保持する。
	CAutoLock cAutoLock(m_pFilter->pStateLock());

	// フィルタが停止しない限り、フォーマットは設定できない。
	FILTER_STATE state = State_Stopped;
	hr = m_pFilter->GetState(0, &state);
	if (hr < S_OK || state != State_Stopped)
		return VFW_E_NOT_STOPPED;

	// 可能な出力フォーマットの集合は入力フォーマットにより異なる。
	// したがって、入力ピンが接続されてない場合、失敗コードを返す。
	//if (!m_pFilter->m_pInput->IsConnected())
	//{
	//	return VFW_E_NOT_CONNECTED;
	//}

	// ピンが既にこのフォーマットを使っている場合、処理は必要ない。
	//if (IsConnected() && CurrentMediaType() == *pmt)
	//{
	//	return S_OK;
	//}

	// このメディア タイプが受け入れ可能かどうかを確認する。
	if ((hr = CheckMediaType((CMediaType *)pmt)) != S_OK) 
	{
		return hr;
	}

	// ダウンストリーム フィルタに接続している場合、ダウンストリーム フィルタが
	// このメディア タイプを受け入れるかどうか確認する必要がある。
	if (IsConnected()) 
	{
		hr = GetConnected()->QueryAccept(pmt);
		if (hr != S_OK)
		{
			return VFW_E_INVALIDMEDIATYPE;
		}
	}

	// 以降はこれが唯一使用できるフォーマットであり、上の CheckMediaType 
	// コードではこのフォーマット以外を拒否する点に注意すること。

	// フォーマットの変更は、必要に応じて再接続することを意味する。
	if (IsConnected())
	{
		m_pFilter->ReconnectPin(this, pmt);
	}

	return NOERROR;
}

// =================================================================
HRESULT CxDSScreenCapturePin::GetFormat(AM_MEDIA_TYPE** ppmt)
{
    if (!IsConnected())
        return VFW_E_NOT_CONNECTED;
	if (ppmt != NULL)
		*ppmt = new AM_MEDIA_TYPE(m_MediaType);
	return NOERROR;
}

// =================================================================
HRESULT CxDSScreenCapturePin::GetNumberOfCapabilities(int* piCount, int* piSize)
{
	if (piCount != NULL)
		*piCount = 5;
	if (piSize != NULL)
		*piSize = sizeof(VIDEO_STREAM_CONFIG_CAPS);
	return NOERROR;
}

// =================================================================
HRESULT CxDSScreenCapturePin::GetStreamCaps(int iIndex, AM_MEDIA_TYPE** ppmt, BYTE* pSCC)
{
	CMediaType mt;
	HRESULT hr = GetMediaType(iIndex, &mt);
	if (hr < S_OK)
		return hr;
	if (ppmt != NULL)
		*ppmt = new AM_MEDIA_TYPE(mt);
	if (pSCC != NULL)
	{
		auto scc = reinterpret_cast<VIDEO_STREAM_CONFIG_CAPS*>(pSCC);
		memset(scc, 0, sizeof(VIDEO_STREAM_CONFIG_CAPS));
		scc->guid = MEDIATYPE_Video;
		//ULONG VideoStandard;
		//SIZE InputSize;
		//SIZE MinCroppingSize;
		//SIZE MaxCroppingSize;
		//int CropGranularityX;
		//int CropGranularityY;
		//int CropAlignX;
		//int CropAlignY;
		//SIZE MinOutputSize;
		//SIZE MaxOutputSize;
		//int OutputGranularityX;
		//int OutputGranularityY;
		//int StretchTapsX;
		//int StretchTapsY;
		//int ShrinkTapsX;
		//int ShrinkTapsY;
		//LONGLONG MinFrameInterval;
		//LONGLONG MaxFrameInterval;
		//LONG MinBitsPerSecond;
		//LONG MaxBitsPerSecond;
	}
	return NOERROR;
}

// =================================================================
HRESULT CxDSScreenCapturePin::Setup(RECT rect)
{
	Dispose();

	::QueryPerformanceFrequency( &Freq );
	::QueryPerformanceCounter( &Time );

	m_hDesktop = ::GetDesktopWindow();
	::GetWindowRect(m_hDesktop, &m_DesktopRect);

	// Save dimensions for later use in FillBuffer()
	m_WindowRect = rect;
	int width = rect.right - rect.left;
	int height = rect.bottom - rect.top;
	if (width < 1 || height < 1)
		return E_INVALIDARG;

	// Create the device context of the main display
	{
		HDC hScreenDC = ::GetDC(NULL);
		m_hDC = ::CreateCompatibleDC(hScreenDC);
		::ReleaseDC(NULL, hScreenDC);
	}

	// Create bitmap
	{
		int width	= (m_WindowRect.right - m_WindowRect.left);
		int height	= (m_WindowRect.bottom - m_WindowRect.top);
		int bpp		= 32;	// bits
		int wbytes	= ((width * bpp + 31) >> 5) << 2;	// 4bytes packing

		BITMAPINFO bmi;
		{
			DWORD	dwByteSize = wbytes * height;

			int		iClrUsed = 0;
			int		iPalletSize = sizeof(RGBQUAD) * iClrUsed;
			UINT	uiBitmapInfoSize = sizeof(BITMAPINFOHEADER) + iPalletSize;

			::ZeroMemory( &bmi, uiBitmapInfoSize );

			bmi.bmiHeader.biSize			= sizeof(BITMAPINFOHEADER);
			bmi.bmiHeader.biWidth			= width;
			bmi.bmiHeader.biHeight			= -height;	// Top-down DIB 
		//	bmi.bmiHeader.biHeight			=  height;	// Bottom-up DIB 
			bmi.bmiHeader.biPlanes			= 1;
			bmi.bmiHeader.biBitCount		= bpp;
			bmi.bmiHeader.biCompression		= BI_RGB;		// An uncompressed format.
			bmi.bmiHeader.biSizeImage		= dwByteSize;
			bmi.bmiHeader.biXPelsPerMeter	= 0;
			bmi.bmiHeader.biYPelsPerMeter	= 0;
			bmi.bmiHeader.biClrUsed			= iClrUsed;
			bmi.bmiHeader.biClrImportant	= 0;
		}

		m_hBitmap = ::CreateDIBSection(m_hDC, &bmi, DIB_RGB_COLORS, &m_Address, NULL, 0);
		::SelectObject( m_hDC, m_hBitmap );
	}

	return S_OK;
}

// =================================================================
HRESULT CxDSScreenCapturePin::Dispose()
{
	m_hDesktop = NULL;
	m_hWnd = NULL;

	if (m_hDC != NULL)
		::DeleteDC(m_hDC);
	m_hDC = NULL;

	if (m_hBitmap != NULL)
		::DeleteObject( m_hBitmap );
	m_hBitmap = NULL;
	m_Address = NULL;

	return S_OK;
}

// =================================================================
HRESULT CxDSScreenCapturePin::GetDC(HDC* hDC)
{
	if (hDC == NULL)
		return E_INVALIDARG;
	*hDC = m_hDC;
	return S_OK;
}

// =================================================================
HRESULT CxDSScreenCapturePin::GetWindowRect(RECT* rect)
{
	if (rect != NULL)
		*rect = m_WindowRect;
	return S_OK;
}

// =================================================================
HRESULT CxDSScreenCapturePin::GetFrameSize(int* width, int* height)
{
	if (width != NULL)
		*width = (m_WindowRect.right - m_WindowRect.left);
	if (height != NULL)
		*height = (m_WindowRect.bottom - m_WindowRect.top);
	return S_OK;
}

// =================================================================
HRESULT CxDSScreenCapturePin::SetFrameRate(int value)
{
	if (!(0 < value))
		return E_INVALIDARG;
	m_FrameRate = value;
	return S_OK;
}

// =================================================================
HRESULT CxDSScreenCapturePin::GetFrameRate(int* value)
{
	if (value == NULL)
		return E_INVALIDARG;
	*value = m_FrameRate;
	return S_OK;
}

// =================================================================
/*
	GetMediaType

	Prefer 5 formats - 8, 16 (*2), 24 or 32 bits per pixel

	Prefered types should be ordered by quality, with zero as highest quality.
	Therefore, iPosition =
		0	 Return a 32bit mediatype
		1	 Return a 24bit mediatype
		2	 Return 16bit RGB565
		3	 Return a 16bit mediatype (rgb555)
		4	 Return 8 bit palettised format
		>4	 Invalid
*/
HRESULT CxDSScreenCapturePin::GetMediaType(int iPosition, CMediaType *pmt)
{
	CheckPointer(pmt,E_POINTER);
	CAutoLock cAutoLock(m_pFilter->pStateLock());

	if (iPosition < 0)
		return E_INVALIDARG;

	// Have we run off the end of types?
	if (iPosition > 4)
		return VFW_S_NO_MORE_ITEMS;

	VIDEOINFO *pvi = (VIDEOINFO *) pmt->AllocFormatBuffer(sizeof(VIDEOINFO));
	if (NULL == pvi)
		return(E_OUTOFMEMORY);

	// Initialize the VideoInfo structure before configuring its members
	ZeroMemory(pvi, sizeof(VIDEOINFO));

	switch(iPosition)
	{
		case 0:
		{	 
			// Return our highest quality 32bit format

			// Since we use RGB888 (the default for 32 bit), there is
			// no reason to use BI_BITFIELDS to specify the RGB
			// masks. Also, not everything supports BI_BITFIELDS
			pvi->bmiHeader.biCompression = BI_RGB;
			pvi->bmiHeader.biBitCount	 = 32;
			break;
		}

		case 1:
		{	// Return our 24bit format
			pvi->bmiHeader.biCompression = BI_RGB;
			pvi->bmiHeader.biBitCount	 = 24;
			break;
		}

		case 2:
		{		
			// 16 bit per pixel RGB565

			// Place the RGB masks as the first 3 doublewords in the palette area
			for(int i = 0; i < 3; i++)
				pvi->TrueColorInfo.dwBitMasks[i] = bits565[i];

			pvi->bmiHeader.biCompression = BI_BITFIELDS;
			pvi->bmiHeader.biBitCount	 = 16;
			break;
		}

		case 3:
		{	// 16 bits per pixel RGB555

			// Place the RGB masks as the first 3 doublewords in the palette area
			for(int i = 0; i < 3; i++)
				pvi->TrueColorInfo.dwBitMasks[i] = bits555[i];

			pvi->bmiHeader.biCompression = BI_BITFIELDS;
			pvi->bmiHeader.biBitCount	 = 16;
			break;
		}

		case 4:
		{	// 8 bit palettised

			pvi->bmiHeader.biCompression = BI_RGB;
			pvi->bmiHeader.biBitCount	 = 8;
			pvi->bmiHeader.biClrUsed	 = iPALETTE_COLORS;
			break;
		}
	}

	// Adjust the parameters common to all formats
	pvi->bmiHeader.biSize		= sizeof(BITMAPINFOHEADER);
	pvi->bmiHeader.biWidth		= (m_WindowRect.right - m_WindowRect.left);
	pvi->bmiHeader.biHeight 	= (m_WindowRect.bottom - m_WindowRect.top);
	pvi->bmiHeader.biPlanes 	= 1;
	pvi->bmiHeader.biSizeImage	= GetBitmapSize(&pvi->bmiHeader);
	pvi->bmiHeader.biClrImportant = 0;

	SetRectEmpty(&(pvi->rcSource)); // we want the whole image area rendered.
	SetRectEmpty(&(pvi->rcTarget)); // no particular destination rectangle

	pmt->SetType(&MEDIATYPE_Video);
	pmt->SetFormatType(&FORMAT_VideoInfo);
	pmt->SetTemporalCompression(FALSE);

	// Work out the GUID for the subtype from the header info.
	const GUID SubTypeGUID = GetBitmapSubtype(&pvi->bmiHeader);
	pmt->SetSubtype(&SubTypeGUID);
	pmt->SetSampleSize(pvi->bmiHeader.biSizeImage);

	return NOERROR;
}

// =================================================================
/*
	CheckMediaType

	We will accept 8, 16, 24 or 32 bit video formats, in any
	image size that gives room to bounce.
	Returns E_INVALIDARG if the mediatype is not acceptable
*/
HRESULT CxDSScreenCapturePin::CheckMediaType(const CMediaType *pMediaType)
{
	CheckPointer(pMediaType,E_POINTER);

	if ((*(pMediaType->Type()) != MEDIATYPE_Video) ||   // we only output video
		!(pMediaType->IsFixedSize()))				   // in fixed size samples
	{												   
		return E_INVALIDARG;
	}

	// Check for the subtypes we support
	const GUID *SubType = pMediaType->Subtype();
	if (SubType == NULL)
		return E_INVALIDARG;

	if (   (*SubType != MEDIASUBTYPE_RGB8)
		&& (*SubType != MEDIASUBTYPE_RGB565)
		&& (*SubType != MEDIASUBTYPE_RGB555)
		&& (*SubType != MEDIASUBTYPE_RGB24)
		&& (*SubType != MEDIASUBTYPE_RGB32))
	{
		return E_INVALIDARG;
	}

	// Get the format area of the media type
	VIDEOINFO *pvi = (VIDEOINFO *) pMediaType->Format();

	if (pvi == NULL)
		return E_INVALIDARG;

	// Check if the image width & height have changed
	if (pvi->bmiHeader.biWidth != (m_WindowRect.right - m_WindowRect.left) || 
	   abs(pvi->bmiHeader.biHeight) != (m_WindowRect.bottom - m_WindowRect.top))
	{
		// If the image width/height is changed, fail CheckMediaType() to force
		// the renderer to resize the image.
		return E_INVALIDARG;
	}

	// Don't accept formats with negative height, which would cause the desktop
	// image to be displayed upside down.
	if (pvi->bmiHeader.biHeight < 0)
		return E_INVALIDARG;

	return S_OK;  // This format is acceptable.
}

// =================================================================
/*
	DecideBufferSize

	This will always be called after the format has been sucessfully
	negotiated. So we have a look at m_mt to see what size image we agreed.
	Then we can ask for buffers of the correct size to contain them.
*/
HRESULT CxDSScreenCapturePin::DecideBufferSize(IMemAllocator *pAlloc, ALLOCATOR_PROPERTIES *pProperties)
{
	CheckPointer(pAlloc,E_POINTER);
	CheckPointer(pProperties,E_POINTER);

	CAutoLock cAutoLock(m_pFilter->pStateLock());
	HRESULT hr = NOERROR;

	VIDEOINFO *pvi = (VIDEOINFO *) m_mt.Format();
	pProperties->cBuffers = 1;
	pProperties->cbBuffer = pvi->bmiHeader.biSizeImage;

	ASSERT(pProperties->cbBuffer);

	// Ask the allocator to reserve us some sample memory. NOTE: the function
	// can succeed (return NOERROR) but still not have allocated the
	// memory that we requested, so we must check we got whatever we wanted.
	ALLOCATOR_PROPERTIES Actual;
	hr = pAlloc->SetProperties(pProperties,&Actual);
	if (FAILED(hr))
	{
		return hr;
	}

	// Is this allocator unsuitable?
	if (Actual.cbBuffer < pProperties->cbBuffer)
	{
		return E_FAIL;
	}

	// Make sure that we have only 1 buffer (we erase the ball in the
	// old buffer to save having to zero a 200k+ buffer every time
	// we draw a frame)
	ASSERT(Actual.cBuffers == 1);
	return NOERROR;
}

// =================================================================
/*
	SetMediaType

	Called when a media type is agreed between filters
*/
HRESULT CxDSScreenCapturePin::SetMediaType(const CMediaType *pMediaType)
{
	CAutoLock cAutoLock(m_pFilter->pStateLock());

	// Pass the call up to my base class
	HRESULT hr = CSourceStream::SetMediaType(pMediaType);

	if (SUCCEEDED(hr))
	{
		VIDEOINFO * pvi = (VIDEOINFO *) m_mt.Format();
		if (pvi == NULL)
			return E_UNEXPECTED;

		switch(pvi->bmiHeader.biBitCount)
		{
			case 8: 	// 8-bit palettized
			case 16:	// RGB565, RGB555
			case 24:	// RGB24
			case 32:	// RGB32
				// Save the current media type and bit depth
				m_MediaType = *pMediaType;
				m_BitDepth = pvi->bmiHeader.biBitCount;
				hr = S_OK;
				break;

			default:
				// We should never agree any other media types
				ASSERT(FALSE);
				hr = E_INVALIDARG;
				break;
		}
	} 

	return hr;
}

// =================================================================
/*
	This is where we insert the DIB bits into the video stream.
	FillBuffer is called once for every sample in the stream.
*/
HRESULT CxDSScreenCapturePin::FillBuffer(IMediaSample *pSample)
{
	HRESULT hResult = S_OK;

	//printf("%s: m_FrameNumber=%d\n", __FUNCTION__, m_FrameNumber);

	CheckPointer(pSample, E_POINTER);

	CAutoLock cAutoLockShared(&m_cSharedState);

	// Check that we're still using video
	ASSERT(m_mt.formattype == FORMAT_VideoInfo);

	// Set the timestamps that will govern playback frame rate.
	// If this file is getting written out as an AVI,
	// then you'll also need to configure the AVI Mux filter to 
	// set the Average Time Per Frame for the AVI Header.
	// The current time is the sample's start.

	REFERENCE_TIME frame_length = UNITS / m_FrameRate;
	REFERENCE_TIME nextSt = frame_length * m_FrameNumber;
	REFERENCE_TIME nextEd = frame_length + nextSt;
	double sampleTime = ((double)nextSt)/UNITS;

	HRESULT hr = pSample->SetTime(&nextSt, &nextEd);
	//printf("SetTime=%d\n", hr);

	// Wait
	if( Freq.QuadPart != 0 )
	{
		LARGE_INTEGER	now = { 0 };
		if (::QueryPerformanceCounter( &now ))
		{
			double diff;
			if( now.QuadPart > Time.QuadPart )
				diff = (double)(now.QuadPart - Time.QuadPart);
			else
			{
				__int64 temp = Time.QuadPart - now.QuadPart;
				diff = (double)((~temp) + 1);
			}
			double lap = ((diff / Freq.QuadPart) * 1000);
			double span = 1000 / m_FrameRate;
			int remain = (int)(span - lap);
			if (remain > 0)
			{
				//printf("%d: Sleep(%d)\n", m_FrameNumber, remain);
				::Sleep(remain);
			}
		}
		if (::QueryPerformanceCounter( &now ))
			Time = now;
	}

	// Rendering
	if (hResult == S_OK)
	{
		int sx = m_WindowRect.left - m_DesktopRect.left;
		int sy = m_WindowRect.top - m_DesktopRect.top;
		int width	= (m_WindowRect.right - m_WindowRect.left);
		int height	= (m_WindowRect.bottom - m_WindowRect.top);

		// Draw:
		{
			// select new bitmap into memory DC
			HBITMAP hOldBitmap = (HBITMAP)::SelectObject(m_hDC, m_hBitmap);

			// capture
			HDC hdc = ::GetDC(m_hDesktop);
			::BitBlt(m_hDC, 0, 0, width, height, hdc, sx, sy, SRCCOPY);
			::ReleaseDC(m_hDesktop, hdc);

			// mouse cursor
			CURSORINFO cursor;
			memset(&cursor, 0, sizeof(cursor));
			cursor.cbSize = sizeof(CURSORINFO);
			if (::GetCursorInfo(&cursor))
			{
				POINT mp = cursor.ptScreenPos;
				int mx = mp.x - sx;
				int my = mp.y - sy;
				::DrawIconEx(m_hDC, mx, my, cursor.hCursor, 0, 0, 0, NULL, DI_NORMAL);
				//printf("GetCursorInfo: %d,%d - %d,%d (%u) %p\n", mx, my, mp.x, mp.y, cursor.flags, cursor.hCursor);
			}

			// select old bitmap back into memory DC and get handle to
			// bitmap of the screen   
			::SelectObject(m_hDC, hOldBitmap);
		}

		// Access the sample's data buffer
		BYTE* addr = NULL;
		pSample->GetPointer(&addr);
		long length = pSample->GetSize();
		VIDEOINFOHEADER *pVih = (VIDEOINFOHEADER*)m_mt.pbFormat;

		// Copy the bitmap data into the provided BYTE buffer
		::GetDIBits(m_hDC, m_hBitmap, 0, height, addr, (BITMAPINFO*)&pVih->bmiHeader, DIB_RGB_COLORS);
	}

	m_FrameNumber++;

	// Set TRUE on every sample for uncompressed frames
	pSample->SetSyncPoint(TRUE);

	return hResult;
}

}
}
