/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXINTEGRAL_H_INCLUDED_
#define _CXINTEGRAL_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Effectors
{

class XIE_EXPORT_CLASS CxIntegral : public CxModule
	, public IxEquatable
{
public:
	int		Mode;

public:
	CxIntegral();
	CxIntegral(int mode);
	CxIntegral( const CxIntegral& src );
	virtual ~CxIntegral();

	CxIntegral& operator = ( const CxIntegral& src );
	bool operator == ( const CxIntegral& src ) const;
	bool operator != ( const CxIntegral& src ) const;

	// IxEquatable
	virtual void CopyFrom(const IxModule& src);
	virtual bool ContentEquals(const IxModule& src) const;

public:
	virtual void Execute(HxModule hsrc, HxModule hdst, HxModule hmask = NULL) const;
};

}
}

#pragma pack(pop)

#endif
