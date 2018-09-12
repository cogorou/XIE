/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#pragma once

#ifndef _CXVLGRABBERARGS_H_INCLUDED_
#define _CXVLGRABBERARGS_H_INCLUDED_

#include "xie_high.h"
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
class XIE_EXPORT_CLASS CxVLGrabberArgs : public CxGrabberArgs
{
private:
	void _Constructor();

public:
	CxVLGrabberArgs();
	CxVLGrabberArgs(TxImageSize frame_size, double progress, void* addr, int length);
	CxVLGrabberArgs(CxVLGrabberArgs&& src);
	CxVLGrabberArgs(const CxVLGrabberArgs& src);
	virtual ~CxVLGrabberArgs();

	CxVLGrabberArgs& operator = ( CxVLGrabberArgs&& src );
	CxVLGrabberArgs& operator = ( const CxVLGrabberArgs& src );
	bool operator == ( const CxVLGrabberArgs& src ) const;
	bool operator != ( const CxVLGrabberArgs& src ) const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

	// IxConvertible
	virtual void CopyTo(IxModule& dst) const;

	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);
};

}
}

#endif

#endif	// _MCS_VER
