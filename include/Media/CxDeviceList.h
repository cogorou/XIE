/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXDEVICELIST_H_INCLUDED_
#define _CXDEVICELIST_H_INCLUDED_

#include "xie_high.h"
#include "Media/CxDeviceListItem.h"
#include "Core/CxModule.h"
#include "Core/CxArrayEx.h"
#include "Core/CxString.h"
#include "Core/TxSizeI.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

class XIE_EXPORT_CLASS CxDeviceList : public CxModule
	, public IxDisposable
	, public IxEquatable
{
private:
	void _Constructor();

public:
	CxDeviceList();
	CxDeviceList(const CxDeviceList& src);
	virtual ~CxDeviceList();

	CxDeviceList& operator = ( const CxDeviceList& src );
	bool operator == ( const CxDeviceList& src ) const;
	bool operator != ( const CxDeviceList& src ) const;

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

public:
	virtual void Setup(ExMediaType type, ExMediaDir dir);

public:
	virtual int Length() const;

	virtual       CxDeviceListItem& operator [] (int index);
	virtual const CxDeviceListItem& operator [] (int index) const;

protected:
	CxArrayEx<CxDeviceListItem>	m_Items;
};

}
}

#pragma pack(pop)

#endif
