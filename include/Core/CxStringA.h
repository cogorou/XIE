/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXSTRINGA_H_INCLUDED_
#define _CXSTRINGA_H_INCLUDED_

#include "xie_core.h"

#include "Core/TxStringA.h"
#include "Core/CxModule.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxStringA : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxEquatable
{
protected:
	TxStringA	m_Tag;

private:
	void _Constructor();

public:
	static CxStringA Format(TxCharCPtrA format, ...);
	static CxStringA From(TxCharCPtrW src, unsigned int codepage = 0);

public:
	CxStringA();
	CxStringA(CxStringA&& src);
	CxStringA(const CxStringA& src);
	CxStringA(TxCharCPtrA text);
	virtual ~CxStringA();

	CxStringA& operator = ( CxStringA&& src );
	CxStringA& operator = ( const CxStringA& src );
	CxStringA& operator = ( TxCharCPtrA src );

	bool operator == ( const CxStringA& src ) const;
	bool operator != ( const CxStringA& src ) const;

	bool operator == ( TxCharCPtrA src ) const;
	bool operator != ( TxCharCPtrA src ) const;

	CxStringA operator + (const CxStringA& src) const;
	CxStringA& operator += (const CxStringA& src);

	CxStringA operator + (TxCharCPtrA src) const;
	CxStringA& operator += (TxCharCPtrA src);

	virtual TxStringA Tag() const;
	virtual void* TagPtr() const;

	virtual operator TxCharCPtrA() const;
	virtual operator TxCharPtrA();

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

protected:
	virtual void MoveFrom(CxStringA& src);

public:
	virtual void Resize(int length);
	virtual void Reset();

	virtual int Length() const;

	virtual       char* Address();
	virtual const char* Address() const;

	virtual       char& operator [] (int index);
	virtual const char& operator [] (int index) const;

	virtual bool StartsWith(TxCharCPtrA value, bool ignore_case) const;
	virtual bool EndsWith(TxCharCPtrA value, bool ignore_case) const;

	static bool StartsWith(TxCharCPtrA src, TxCharCPtrA value, bool ignore_case);
	static bool EndsWith(TxCharCPtrA src, TxCharCPtrA value, bool ignore_case);
};

}

#pragma pack(pop)

#endif
