/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#include "xie_ds.h"
#include "CxDSScreenCapturePin.h"
#include "baseclasses/streams.h"

namespace xie
{
namespace DS
{

class CxDSScreenCaptureFilter : public CSource
	, public IAMFilterMiscFlags
{
private:
	CxDSScreenCaptureFilter(IUnknown *pUnk, HRESULT *phr);
	~CxDSScreenCaptureFilter();

	CxDSScreenCapturePin *m_pPin;

public:
	static CUnknown * WINAPI CreateInstance(IUnknown *pUnk, HRESULT *phr);	

	DECLARE_IUNKNOWN

	// IAMFilterMiscFlags

	virtual ULONG STDMETHODCALLTYPE GetMiscFlags();
};

}
}
