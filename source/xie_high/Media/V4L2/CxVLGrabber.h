/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#pragma once

#ifndef _CXVLGRABBER_H_INCLUDED_
#define _CXVLGRABBER_H_INCLUDED_

#include "xie_high.h"
#include "Media/CxGrabber.h"
#include "Media/V4L2/CxVLGrabberArgs.h"
#include "Media/V4L2/CxVLSampleReceiver.h"
#include "Core/CxModule.h"
#include "Core/CxArrayEx.h"
#include "Core/CxImage.h"
#include "Core/CxString.h"
#include "Core/TxSizeI.h"
#include "Core/IxDisposable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

class XIE_EXPORT_CLASS CxVLGrabber : public CxModule
	, public IxDisposable
	, public IxEquatable
{
public:
	static CxGrabber From(const CxVLSampleReceiver* receiver)
	{
		CxGrabber result;
		CxVLGrabber* body = (CxVLGrabber*)result.GetModule();
		body->Receiver(receiver);
		return result;
	}

	CxVLGrabber(CxGrabber* parent);
	virtual ~CxVLGrabber();

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

	virtual CxGrabber* Parent() const;

	virtual CxVLSampleReceiver* Receiver() const;
	virtual void Receiver(const CxVLSampleReceiver* receiver);

protected:
	CxGrabber*		m_Parent;
	CxVLSampleReceiver*	m_Receiver;
};

}
}

#pragma pack(pop)

#endif

#endif	// _MCS_VER
