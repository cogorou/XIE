/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#pragma once

#ifndef _CXMMAP_H_INCLUDED_
#define _CXMMAP_H_INCLUDED_

#include "xie_high.h"
#include "Core/Axi.h"
#include "Core/CxModule.h"
#include "Core/CxException.h"
#include "Core/TxArray.h"
#include "Core/IxDisposable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxMMap : public CxModule
	, public IxDisposable
{
protected:
	void*		m_Address;
	int			m_Length;
	int			m_Offset;
	int			m_FD;
	bool		m_IsAttached;

private:
	void _Constructor();

public:
	CxMMap();
	CxMMap(CxMMap&& src);
	CxMMap(const CxMMap& src);
	CxMMap(int length, int offset, int fd);
	virtual ~CxMMap();

	CxMMap& operator = ( CxMMap&& src );
	CxMMap& operator = ( const CxMMap& src );
	bool operator == ( const CxMMap& src ) const;
	bool operator != ( const CxMMap& src ) const;

	virtual TxArray Tag() const;

public:
	virtual void Dispose();
	virtual bool IsValid() const;
	virtual bool IsAttached() const;

public:
	virtual void Resize(int length, int offset, int fd);
	virtual void Reset();

public:
	virtual       void* Address();
	virtual const void* Address() const;

	virtual int Length() const;
	virtual int Offset() const;
	virtual int FD() const;
};

}

#pragma pack(pop)

#endif

#endif	// _MCS_VER
