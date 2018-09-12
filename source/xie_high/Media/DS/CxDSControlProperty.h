/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#pragma once

#ifndef _CXDSCONTROLPROPERTY_H_INCLUDED_
#define _CXDSCONTROLPROPERTY_H_INCLUDED_

#include "api_ds.h"
#include "Core/CxModule.h"
#include "Core/CxString.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/TxRangeI.h"
#include "Media/DS/CxDSControlPropertyWrapper.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

class XIE_EXPORT_CLASS CxDSControlProperty : public CxModule
	, public IxDisposable
	, public IxEquatable
{
protected:
	CxDSControlPropertyWrapper	m_Wrapper;

private:
	void _Constructor();

public:
	CxDSControlProperty();
	CxDSControlProperty(HxModule controller, TxCharCPtrA name);
	CxDSControlProperty(const CxDSControlProperty& src);
	virtual ~CxDSControlProperty();

	CxDSControlProperty& operator = ( const CxDSControlProperty& src );
	bool operator == ( const CxDSControlProperty& src ) const;
	bool operator != ( const CxDSControlProperty& src ) const;

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

public:
	virtual HxModule Controller() const;
	virtual void Controller(HxModule value);

	virtual TxCharCPtrA Name() const;
	virtual void Name(TxCharCPtrA value);

public:
	virtual bool IsSupported() const;
	virtual TxRangeI GetRange() const;
	virtual int GetStep() const;
	virtual int GetDefault() const;
	virtual int GetFlags() const;
	virtual void SetFlags(int value);
	virtual int GetValue() const;
	virtual void SetValue(int value, bool relative);
};

}
}

#pragma pack(pop)

#endif

#endif	// _MCS_VER
