/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXDEVICELISTITEM_H_INCLUDED_
#define _CXDEVICELISTITEM_H_INCLUDED_

#include "xie_high.h"
#include "Media/TxDeviceListItem.h"
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

class XIE_EXPORT_CLASS CxDeviceListItem : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxEquatable
{
protected:
	TxDeviceListItem	m_Tag;

private:
	void _Constructor();

public:
	CxDeviceListItem();
	CxDeviceListItem(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index);
	CxDeviceListItem(CxDeviceListItem&& src);
	CxDeviceListItem(const CxDeviceListItem& src);
	virtual ~CxDeviceListItem();

	CxDeviceListItem& operator = ( CxDeviceListItem&& src );
	CxDeviceListItem& operator = ( const CxDeviceListItem& src );
	bool operator == ( const CxDeviceListItem& src ) const;
	bool operator != ( const CxDeviceListItem& src ) const;

	TxDeviceListItem Tag() const;
	virtual void* TagPtr() const;

protected:
	virtual void MoveFrom(CxDeviceListItem& src);

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

public:
	virtual ExMediaType	MediaType() const;
	virtual void		MediaType(ExMediaType value);

	virtual ExMediaDir	MediaDir() const;
	virtual void		MediaDir(ExMediaDir value);

	virtual TxCharCPtrA		Name() const;
	virtual void		Name(TxCharCPtrA value);

	virtual int			Index() const;
	virtual void		Index(int value);

	virtual CxStringA GetProductName() const;
	virtual CxArrayEx<CxStringA> GetPinNames() const;
	virtual CxArrayEx<TxSizeI> GetFrameSizes(int pin = 0) const;
};

}
}

#pragma pack(pop)

#endif
