/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXSCREENLIST_H_INCLUDED_
#define _CXSCREENLIST_H_INCLUDED_

#include "xie_high.h"
#include "Media/CxScreenListItem.h"
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

class XIE_EXPORT_CLASS CxScreenList : public CxModule
	, public IxDisposable
	, public IxEquatable
{
private:
	void _Constructor();

public:
	CxScreenList();
	CxScreenList(const CxScreenList& src);
	virtual ~CxScreenList();

	CxScreenList& operator = ( const CxScreenList& src );
	bool operator == ( const CxScreenList& src ) const;
	bool operator != ( const CxScreenList& src ) const;

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

public:
	virtual void Setup();

public:
	virtual int Length() const;

	virtual       CxScreenListItem& operator [] (int index);
	virtual const CxScreenListItem& operator [] (int index) const;

protected:
	CxArrayEx<CxScreenListItem>	m_Items;
};

}
}

#pragma pack(pop)

#endif
