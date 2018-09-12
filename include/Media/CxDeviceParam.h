/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXDEVICEPARAM_H_INCLUDED_
#define _CXDEVICEPARAM_H_INCLUDED_

#include "xie_high.h"
#include "Media/TxDeviceParam.h"
#include "Core/CxModule.h"
#include "Core/CxArrayEx.h"
#include "Core/CxImage.h"
#include "Core/CxString.h"
#include "Core/TxSizeI.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

class XIE_EXPORT_CLASS CxDeviceParam : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxEquatable
{
protected:
	TxDeviceParam	m_Tag;

private:
	void _Constructor();

public:
	CxDeviceParam();
	CxDeviceParam(TxCharCPtrA name, int index);
	CxDeviceParam(TxCharCPtrA name, int index, int pin, TxSizeI size);
	CxDeviceParam(CxDeviceParam&& src);
	CxDeviceParam(const CxDeviceParam& src);
	virtual ~CxDeviceParam();

	CxDeviceParam& operator = ( CxDeviceParam&& src );
	CxDeviceParam& operator = ( const CxDeviceParam& src );
	bool operator == ( const CxDeviceParam& src ) const;
	bool operator != ( const CxDeviceParam& src ) const;

	TxDeviceParam Tag() const;
	virtual void* TagPtr() const;

protected:
	virtual void MoveFrom(CxDeviceParam& src);

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

public:
	virtual TxCharCPtrA		Name() const;
	virtual void		Name(TxCharCPtrA value);

	virtual int			Index() const;
	virtual void		Index(int value);

	virtual int			Pin() const;
	virtual void		Pin(int value);

	virtual TxSizeI		Size() const;
	virtual void		Size(TxSizeI value);
};

}
}

#pragma pack(pop)

#endif
