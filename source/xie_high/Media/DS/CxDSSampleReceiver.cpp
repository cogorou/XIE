/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#include "Media/DS/CxDSSampleReceiver.h"
#include "Media/DS/CxDSGrabberArgs.h"
#include "Media/api_media.h"
#include "Core/CxFinalizer.h"

#include <algorithm>
#include <vector>

#if defined(_MSC_VER)

namespace xie
{
namespace Media
{

// =================================================================
CxDSSampleReceiver::CxDSSampleReceiver()
{
	FrameSize = TxImageSize::Default();
}

// =================================================================
CxDSSampleReceiver::~CxDSSampleReceiver()
{
}

// =================================================================
ULONG STDMETHODCALLTYPE CxDSSampleReceiver::AddRef()
{
	return 1;
}

// =================================================================
ULONG STDMETHODCALLTYPE CxDSSampleReceiver::Release()
{
	return 2;
}

// =================================================================
HRESULT STDMETHODCALLTYPE CxDSSampleReceiver::QueryInterface(REFIID, void **ppvObject)
{
	return S_OK;
}

// =================================================================
HRESULT STDMETHODCALLTYPE CxDSSampleReceiver::Receive(IMediaSample *sample)
{
	Mutex.Lock();
	CxFinalizer mutex_finalizer([this]()
	{
		Mutex.Unlock();
	});

	HRESULT hr;

	const LONGLONG UNITS = (NANOSECONDS / 100);      // 10 ^ 7
	REFERENCE_TIME nextSt = 0;
	REFERENCE_TIME nextEd = 0;

	double progress = 0;
	hr = sample->GetTime(&nextSt, &nextEd);
	if (SUCCEEDED(hr))
		progress = ((double)nextSt)/UNITS;

	BYTE *addr = NULL;
	sample->GetPointer(&addr);

	long length = sample->GetSize();

	for(int i=0 ; i<(int)Notify.size() ; i++)
	{
		if (Notify[i] != NULL)
		{
			CxDSGrabberArgs e(FrameSize, progress, addr, length, sample);
			Notify[i]->Receive(this, &e);
		}
	}
	return S_OK;
}

// =================================================================
void CxDSSampleReceiver::Add(IxEventReceiver* src)
{
	Mutex.Lock();
	CxFinalizer mutex_finalizer([this]()
	{
		Mutex.Unlock();
	});

	if (src != NULL)
		Notify.push_back(src);
}

// =================================================================
void CxDSSampleReceiver::Remove(IxEventReceiver* src)
{
	Mutex.Lock();
	CxFinalizer mutex_finalizer([this]()
	{
		Mutex.Unlock();
	});

	if (src != NULL)
		Notify.erase(std::remove(Notify.begin(), Notify.end(), src), Notify.end());
}

// =================================================================
HRESULT CxDSSampleReceiver::SampleCB(double SampleTime, IMediaSample *pSample)
{
	Mutex.Lock();
	CxFinalizer mutex_finalizer([this]()
	{
		Mutex.Unlock();
	});

	HRESULT hr;

	const LONGLONG UNITS = (NANOSECONDS / 100);      // 10 ^ 7
	REFERENCE_TIME nextSt = 0;
	REFERENCE_TIME nextEd = 0;

	double progress = 0;
	hr = pSample->GetTime(&nextSt, &nextEd);
	if (SUCCEEDED(hr))
		progress = ((double)nextSt)/UNITS;

	BYTE *addr = NULL;
	pSample->GetPointer(&addr);

	long length = pSample->GetSize();

	for(int i=0 ; i<(int)Notify.size() ; i++)
	{
		if (Notify[i] != NULL)
		{
			CxDSGrabberArgs e(FrameSize, progress, addr, length, pSample);
			Notify[i]->Receive(this, &e);
		}
	}
	return S_OK;
}

// =================================================================
HRESULT CxDSSampleReceiver::BufferCB(double SampleTime, BYTE *pBuffer, LONG BufferLen)
{
	return S_OK;
}

}
}

#endif

#endif	// _MCS_VER
