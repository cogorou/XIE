/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXRGBTOGRAY_H_INCLUDED_
#define _CXRGBTOGRAY_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Effectors
{

class XIE_EXPORT_CLASS CxRgbToGray : public CxModule
	, public IxEquatable
{
public:
	double		Scale;
	double		RedRatio;
	double		GreenRatio;
	double		BlueRatio;

private:
	void _Constructor();

public:
	CxRgbToGray();
	CxRgbToGray(double scale, double red_ratio = 0.299, double green_ratio = 0.587, double blue_ratio = 0.114);
	CxRgbToGray( const CxRgbToGray& src );
	virtual ~CxRgbToGray();

	CxRgbToGray& operator = ( const CxRgbToGray& src );
	bool operator == ( const CxRgbToGray& src ) const;
	bool operator != ( const CxRgbToGray& src ) const;

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
