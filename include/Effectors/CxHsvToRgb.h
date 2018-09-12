/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXHSVTORGB_H_INCLUDED_
#define _CXHSVTORGB_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Effectors
{

class XIE_EXPORT_CLASS CxHsvToRgb : public CxModule
	, public IxEquatable
{
public:
	int		Depth;

public:
	CxHsvToRgb();
	CxHsvToRgb(int depth);
	CxHsvToRgb( const CxHsvToRgb& src );
	virtual ~CxHsvToRgb();

	CxHsvToRgb& operator = ( const CxHsvToRgb& src );
	bool operator == ( const CxHsvToRgb& src ) const;
	bool operator != ( const CxHsvToRgb& src ) const;

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
