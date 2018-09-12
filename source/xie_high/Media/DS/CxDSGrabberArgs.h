/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#pragma once

#ifndef _CXDSGRABBERARGS_H_INCLUDED_
#define _CXDSGRABBERARGS_H_INCLUDED_

#include "api_ds.h"
#include "Media/CxGrabberArgs.h"

#include "Core/CxModule.h"
#include "Core/IxEquatable.h"
#include "Core/IxParam.h"
#include "Core/TxImageSize.h"

namespace xie
{
namespace Media
{

// /////////////////////////////////////////////////////////////////
class XIE_EXPORT_CLASS CxDSGrabberArgs : public CxGrabberArgs
{
private:
	void _Constructor();

public:
	CxDSGrabberArgs();
	CxDSGrabberArgs(TxImageSize frame_size, double progress, void* addr, int length, IMediaSample *sample);
	CxDSGrabberArgs(CxDSGrabberArgs&& src);
	CxDSGrabberArgs(const CxDSGrabberArgs& src);
	virtual ~CxDSGrabberArgs();

	CxDSGrabberArgs& operator = ( CxDSGrabberArgs&& src );
	CxDSGrabberArgs& operator = ( const CxDSGrabberArgs& src );
	bool operator == ( const CxDSGrabberArgs& src ) const;
	bool operator != ( const CxDSGrabberArgs& src ) const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

	// IxConvertible
	virtual void CopyTo(IxModule& dst) const;

	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);

	// set by SampleGrabberCB
	IMediaSample*	Sample;		// SampleCB
};

}
}

#endif

#endif	// _MCS_VER
