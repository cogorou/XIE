/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXSTRINGW_H_INCLUDED_
#define _CXSTRINGW_H_INCLUDED_

#include "xie_core.h"

#include "Core/TxStringW.h"
#include "Core/CxModule.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxStringW : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxEquatable
{
protected:
	TxStringW	m_Tag;

private:
	void _Constructor();

public:
	static CxStringW Format(TxCharCPtrW format, ...);
	static CxStringW From(TxCharCPtrA src, unsigned int codepage = 0);

public:
	CxStringW();
	CxStringW(CxStringW&& src);
	CxStringW(const CxStringW& src);
	CxStringW(TxCharCPtrW text);
	virtual ~CxStringW();

	CxStringW& operator = ( CxStringW&& src );
	CxStringW& operator = ( const CxStringW& src );
	CxStringW& operator = ( TxCharCPtrW src );

	bool operator == ( const CxStringW& src ) const;
	bool operator != ( const CxStringW& src ) const;

	bool operator == ( TxCharCPtrW src ) const;
	bool operator != ( TxCharCPtrW src ) const;

	CxStringW operator + (const CxStringW& src) const;
	CxStringW& operator += (const CxStringW& src);

	CxStringW operator + (TxCharCPtrW src) const;
	CxStringW& operator += (TxCharCPtrW src);

	virtual TxStringW Tag() const;
	virtual void* TagPtr() const;

	virtual operator TxCharCPtrW() const;
	virtual operator TxCharPtrW();

public:
	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

protected:
	virtual void MoveFrom(CxStringW& src);

public:
	virtual void Resize(int length);
	virtual void Reset();

	virtual int Length() const;

	virtual       wchar_t* Address();
	virtual const wchar_t* Address() const;

	virtual       wchar_t& operator [] (int index);
	virtual const wchar_t& operator [] (int index) const;

	virtual bool StartsWith(TxCharCPtrW value, bool ignore_case) const;
	virtual bool EndsWith(TxCharCPtrW value, bool ignore_case) const;

	static bool StartsWith(TxCharCPtrW src, TxCharCPtrW value, bool ignore_case);
	static bool EndsWith(TxCharCPtrW src, TxCharCPtrW value, bool ignore_case);
};

}

#pragma pack(pop)

#endif
