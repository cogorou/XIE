/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#pragma once

#ifndef _CXVLCONTROLPROPERTY_H_INCLUDED_
#define _CXVLCONTROLPROPERTY_H_INCLUDED_

#include "xie_high.h"
#include "Core/CxModule.h"
#include "Core/CxString.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"
#include "Core/TxRangeI.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Media
{

class XIE_EXPORT_CLASS CxVLControlProperty : public CxModule
	, public IxDisposable
	, public IxEquatable
{
protected:
	HxModule m_Controller;
	CxString m_Name;

private:
	void _Constructor();

public:
	CxVLControlProperty();
	CxVLControlProperty(HxModule controller, TxCharCPtrA name);
	CxVLControlProperty(const CxVLControlProperty& src);
	virtual ~CxVLControlProperty();

	CxVLControlProperty& operator = ( const CxVLControlProperty& src );
	bool operator == ( const CxVLControlProperty& src ) const;
	bool operator != ( const CxVLControlProperty& src ) const;

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
