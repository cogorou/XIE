/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXCONTROLPROPERTY_H_INCLUDED_
#define _CXCONTROLPROPERTY_H_INCLUDED_

#include "xie_high.h"
#include "Core/CxModule.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/IxInternalModule.h"
#include "Core/TxRangeI.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

class XIE_EXPORT_CLASS CxControlProperty : public CxModule
	, public IxInternalModule
	, public IxDisposable
	, public IxEquatable
{
private:
	void _Constructor();
	HxModule m_Handle;

public:
	CxControlProperty();
	CxControlProperty(HxModule controller, TxCharCPtrA name);
	CxControlProperty(const CxControlProperty& src);
	virtual ~CxControlProperty();
	virtual IxModule* GetModule() const;

	CxControlProperty& operator = ( const CxControlProperty& src );
	bool operator == ( const CxControlProperty& src ) const;
	bool operator != ( const CxControlProperty& src ) const;

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
	virtual void SetValue(int value, bool relative = false);
};

}
}

#pragma pack(pop)

#endif
