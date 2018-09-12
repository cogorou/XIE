/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#pragma once

#ifndef _CXDSGRABBER_H_INCLUDED_
#define _CXDSGRABBER_H_INCLUDED_

#include "api_ds.h"
#include "Media/CxGrabber.h"
#include "Media/DS/CxDSGrabberArgs.h"
#include "Media/DS/CxDSSampleReceiver.h"
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

class XIE_EXPORT_CLASS CxDSGrabber : public CxModule
	, public IxDisposable
	, public IxEquatable
{
public:
	static CxGrabber From(const CxDSSampleReceiver* receiver)
	{
		CxGrabber result;
		CxDSGrabber* body = (CxDSGrabber*)result.GetModule();
		body->Receiver(receiver);
		return result;
	}

	CxDSGrabber(CxGrabber* parent);
	virtual ~CxDSGrabber();

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

	virtual CxGrabber* Parent() const;

	virtual CxDSSampleReceiver* Receiver() const;
	virtual void Receiver(const CxDSSampleReceiver* receiver);

protected:
	CxGrabber*	m_Parent;
	CxDSSampleReceiver*	m_Receiver;
};

}
}

#pragma pack(pop)

#endif

#endif	// _MCS_VER
