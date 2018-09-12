/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXRGBCONVERTER_H_INCLUDED_
#define _CXRGBCONVERTER_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/CxMatrix.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Effectors
{

class XIE_EXPORT_CLASS CxRgbConverter : public CxModule
	, public IxEquatable
{
public:
	double		RedRatio;
	double		GreenRatio;
	double		BlueRatio;

protected:
	CxMatrix	Matrix;
	void SetupMatrix() const;

private:
	void _Constructor();

public:
	CxRgbConverter();
	CxRgbConverter(double red_ratio, double green_ratio, double blue_ratio);
	CxRgbConverter( const CxRgbConverter& src );
	virtual ~CxRgbConverter();

	CxRgbConverter& operator = ( const CxRgbConverter& src );
	bool operator == ( const CxRgbConverter& src ) const;
	bool operator != ( const CxRgbConverter& src ) const;

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
