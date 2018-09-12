/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGRABBER_H_INCLUDED_
#define _CXGRABBER_H_INCLUDED_

#include "xie_high.h"
#include "Media/CxGrabberArgs.h"
#include "Media/CxGrabberEvent.h"
#include "Core/CxModule.h"
#include "Core/CxArrayEx.h"
#include "Core/CxImage.h"
#include "Core/CxString.h"
#include "Core/TxSizeI.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/IxRunnable.h"
#include "Core/IxEventReceiver.h"
#include "Core/IxInternalModule.h"
#include <memory>

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

class XIE_EXPORT_CLASS CxGrabber : public CxModule
	, public IxInternalModule
	, public IxDisposable
	, public IxEquatable
	, public IxRunnable
	, public IxEventReceiver
{
private:
	void _Constructor();
	HxModule m_Handle;

public:
	CxGrabber();
	CxGrabber(const CxGrabber& src);
	virtual ~CxGrabber();
	virtual IxModule* GetModule() const;

	CxGrabber& operator = ( const CxGrabber& src );
	bool operator == ( const CxGrabber& src ) const;
	bool operator != ( const CxGrabber& src ) const;

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

	// IxRunnable
	virtual void Reset();
	virtual void Start();
	virtual void Stop();
	virtual bool Wait(int timeout) const;
	virtual bool IsRunning() const;

	virtual int Index() const;
	virtual void Index(int value);

	std::shared_ptr<CxGrabberEvent> Notify;

protected:
	// IxEventReceiver
	virtual void Receive(void* sender, IxModule* e);

	bool	m_Enabled;
	int		m_Index;
};

}
}

#pragma pack(pop)

#endif
