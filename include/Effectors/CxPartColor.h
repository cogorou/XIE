/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXPARTCOLOR_H_INCLUDED_
#define _CXPARTCOLOR_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Effectors
{

class XIE_EXPORT_CLASS CxPartColor : public CxModule
	, public IxEquatable
{
public:
	int			Depth;
	int			HueDir;
	int			HueRange;
	double		RedRatio;
	double		GreenRatio;
	double		BlueRatio;

public:
	CxPartColor();
	CxPartColor(int depth, int hue_dir, int hue_range, double red_ratio = 0.299, double green_ratio = 0.587, double blue_ratio = 0.114);
	CxPartColor( const CxPartColor& src );
	virtual ~CxPartColor();

	CxPartColor& operator = ( const CxPartColor& src );
	bool operator == ( const CxPartColor& src ) const;
	bool operator != ( const CxPartColor& src ) const;

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
