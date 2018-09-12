/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGRABBERARGS_H_INCLUDED_
#define _CXGRABBERARGS_H_INCLUDED_

#include "xie_high.h"
#include "Media/TxGrabberArgs.h"
#include "Core/CxModule.h"
#include "Core/CxImage.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/IxParam.h"
#include "Core/TxImageSize.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

class XIE_EXPORT_CLASS CxGrabberArgs : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxConvertible
	, public IxEquatable
	, public IxParam
{
protected:
	TxGrabberArgs	m_Tag;

private:
	void _Constructor();

public:
	CxGrabberArgs();
	CxGrabberArgs(TxImageSize frame_size, double progress, void* addr, int length);
	CxGrabberArgs(CxGrabberArgs&& src);
	CxGrabberArgs(const CxGrabberArgs& src);
	virtual ~CxGrabberArgs();

	CxGrabberArgs& operator = ( CxGrabberArgs&& src );
	CxGrabberArgs& operator = ( const CxGrabberArgs& src );
	bool operator == ( const CxGrabberArgs& src ) const;
	bool operator != ( const CxGrabberArgs& src ) const;

	TxGrabberArgs Tag() const;
	virtual void* TagPtr() const;

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

	// IxConvertible
	virtual void CopyTo(IxModule& dst) const;
	virtual operator CxImage() const;

	// IxParam
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const;
	template<class TV> TV GetParam(TxCharCPtrA name) const
	{
		TV value;
		GetParam(name, &value, xie::ModelOf(value));
		return value;
	}
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model);
	template<class TV> void SetParam(TxCharCPtrA name, TV value)
	{
		SetParam(name, &value, xie::ModelOf(value));
	}

public:
	virtual unsigned long long TimeStamp() const;
	virtual void TimeStamp(unsigned long long value);

	virtual TxImageSize FrameSize() const;
	virtual void FrameSize(TxImageSize value);

	virtual double Progress() const;
	virtual void Progress(double value);

	virtual void* Address() const;
	virtual void Address(void* value);

	virtual int Length() const;
	virtual void Length(int value);

	virtual int Index() const;
	virtual void Index(int value);

	virtual bool Cancellation() const;
	virtual void Cancellation(bool value);
};

}
}

#pragma pack(pop)

#endif
