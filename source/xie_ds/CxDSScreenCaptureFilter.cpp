/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "CxDSScreenCaptureFilter.h"
#include "baseclasses/dshowutil.h"

namespace xie
{
namespace DS
{

// =================================================================
CxDSScreenCaptureFilter::CxDSScreenCaptureFilter(IUnknown *pUnk, HRESULT *phr)
		   : CSource(NAME("CxDSScreenCaptureFilter"), pUnk, CLSID_CxDSScreenCaptureFilter)
{
	// The pin magically adds itself to our pin array.
	m_pPin = new CxDSScreenCapturePin(phr, this);

	if (phr)
	{
		if (m_pPin == NULL)
			*phr = E_OUTOFMEMORY;
		else
			*phr = S_OK;
	}
	else
	{
		*phr = E_OUTOFMEMORY;
	}
}

// =================================================================
CxDSScreenCaptureFilter::~CxDSScreenCaptureFilter()
{
	delete m_pPin;
}

// =================================================================
CUnknown * WINAPI CxDSScreenCaptureFilter::CreateInstance(IUnknown *pUnk, HRESULT *phr)
{
	CxDSScreenCaptureFilter *pNewFilter = new CxDSScreenCaptureFilter(pUnk, phr );

	if (phr)
	{
		if (pNewFilter == NULL) 
			*phr = E_OUTOFMEMORY;
		else
			*phr = S_OK;
	}
	return pNewFilter;

}

// IAMFilterMiscFlags

// =================================================================
ULONG CxDSScreenCaptureFilter::GetMiscFlags()
{
	return AM_FILTER_MISC_FLAGS_IS_SOURCE;
}

}
}
