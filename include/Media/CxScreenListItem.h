/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXSCREENLISTITEM_H_INCLUDED_
#define _CXSCREENLISTITEM_H_INCLUDED_

#include "xie_high.h"
#include "Media/TxScreenListItem.h"
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

class XIE_EXPORT_CLASS CxScreenListItem : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxEquatable
{
protected:
	TxScreenListItem	m_Tag;

private:
	void _Constructor();

public:
	CxScreenListItem();
	#if defined(_MSC_VER)
	CxScreenListItem(HWND handle, TxCharCPtrA name, TxRectangleI bounds);
	#else
	CxScreenListItem(TxIntPtr handle, TxCharCPtrA name, TxRectangleI bounds);
	#endif
	CxScreenListItem(CxScreenListItem&& src);
	CxScreenListItem(const CxScreenListItem& src);
	virtual ~CxScreenListItem();

	CxScreenListItem& operator = ( CxScreenListItem&& src );
	CxScreenListItem& operator = ( const CxScreenListItem& src );
	bool operator == ( const CxScreenListItem& src ) const;
	bool operator != ( const CxScreenListItem& src ) const;

	TxScreenListItem Tag() const;
	virtual void* TagPtr() const;

protected:
	virtual void MoveFrom(CxScreenListItem& src);

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

public:
	#if defined(_MSC_VER)
	virtual HWND	Handle() const;
	virtual void	Handle(HWND value);
	#else
	virtual TxIntPtr	Handle() const;
	virtual void		Handle(TxIntPtr value);
	#endif

	virtual TxCharCPtrA	Name() const;
	virtual void	Name(TxCharCPtrA value);

	virtual TxRectangleI Bounds() const;
	virtual void Bounds(TxRectangleI value);
};

}
}

#pragma pack(pop)

#endif
