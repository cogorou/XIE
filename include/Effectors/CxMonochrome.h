/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXMONOCHROME_H_INCLUDED_
#define _CXMONOCHROME_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/CxMatrix.h"
#include "Core/IxEquatable.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Effectors
{

class XIE_EXPORT_CLASS CxMonochrome : public CxModule
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
	CxMonochrome();
	CxMonochrome(double red_ratio, double green_ratio, double blue_ratio);
	CxMonochrome( const CxMonochrome& src );
	virtual ~CxMonochrome();

	CxMonochrome& operator = ( const CxMonochrome& src );
	bool operator == ( const CxMonochrome& src ) const;
	bool operator != ( const CxMonochrome& src ) const;

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
