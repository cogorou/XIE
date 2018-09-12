/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXHSVCONVERTER_H_INCLUDED_
#define _CXHSVCONVERTER_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Effectors
{

class XIE_EXPORT_CLASS CxHsvConverter : public CxModule
	, public IxEquatable
{
public:
	int		Depth;
	int		HueDir;
	double	SaturationFactor;
	double	ValueFactor;

public:
	CxHsvConverter();
	CxHsvConverter(int depth, int hue_dir, double saturation_factor, double value_factor);
	CxHsvConverter( const CxHsvConverter& src );
	virtual ~CxHsvConverter();

	CxHsvConverter& operator = ( const CxHsvConverter& src );
	bool operator == ( const CxHsvConverter& src ) const;
	bool operator != ( const CxHsvConverter& src ) const;

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
