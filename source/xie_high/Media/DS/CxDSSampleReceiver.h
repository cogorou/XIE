/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#pragma once

#ifndef _CXDSSAMPLERECEIVER_H_INCLUDED_
#define _CXDSSAMPLERECEIVER_H_INCLUDED_

#include "api_ds.h"
#include "Core/CxMutex.h"
#include "Core/TxImageSize.h"
#include "Core/IxEventReceiver.h"

#include <vector>

namespace xie
{
namespace Media
{

// /////////////////////////////////////////////////////////////////
class CxDSSampleReceiver : public xie::DS::ISampleGrabberCB
{
public:
	CxDSSampleReceiver();
	virtual ~CxDSSampleReceiver();

	// IUnknown
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID, void **ppvObject);

	// IxDSSampleReceiver
    virtual HRESULT STDMETHODCALLTYPE Receive(IMediaSample *sample);

	virtual void Add(IxEventReceiver* src);
	virtual void Remove(IxEventReceiver* src);

	CxMutex Mutex;
	TxImageSize FrameSize;
	std::vector<IxEventReceiver*> Notify;

protected:
	virtual HRESULT STDMETHODCALLTYPE SampleCB(double SampleTime, IMediaSample *pSample);
	virtual HRESULT STDMETHODCALLTYPE BufferCB(double SampleTime, BYTE *pBuffer, LONG BufferLen);
};

}
}

#endif

#endif	// _MCS_VER
