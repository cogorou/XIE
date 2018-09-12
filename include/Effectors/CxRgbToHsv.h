/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXRGBTOHSV_H_INCLUDED_
#define _CXRGBTOHSV_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Effectors
{

class XIE_EXPORT_CLASS CxRgbToHsv : public CxModule
	, public IxEquatable
{
public:
	int		Depth;

public:
	CxRgbToHsv();
	CxRgbToHsv(int depth);
	CxRgbToHsv( const CxRgbToHsv& src );
	virtual ~CxRgbToHsv();

	CxRgbToHsv& operator = ( const CxRgbToHsv& src );
	bool operator == ( const CxRgbToHsv& src ) const;
	bool operator != ( const CxRgbToHsv& src ) const;

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
