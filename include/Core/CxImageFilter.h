/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXIMAGEFILTER_H_INCLUDED_
#define _CXIMAGEFILTER_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/CxArray.h"
#include "Core/TxStatistics.h"
#include "Core/TxSizeI.h"
#include "Core/TxRangeD.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxImageFilter
{
public:
	CxImageFilter();
	CxImageFilter(HxModule hdst, HxModule hmask);
	CxImageFilter(const CxImageFilter& src);
	virtual ~CxImageFilter();

	CxImageFilter& operator = ( const CxImageFilter& src );
	bool operator == ( const CxImageFilter& src ) const;
	bool operator != ( const CxImageFilter& src ) const;

	HxModule hDst;
	HxModule hMask;

public:
	// Basic
	virtual void Cast		(HxModule hsrc);
	virtual void Copy		(HxModule hsrc, double scale = 0);
	virtual void CopyEx		(HxModule hsrc, int index, int count);
	virtual void RgbToBgr	(HxModule hsrc, double scale = 0);
	virtual void Compare	(HxModule hsrc, HxModule hcmp, double error_range);

	// Filter
	virtual void ColorMatrix	(HxModule hsrc, HxModule hmatrix);

	// GeoTrans
	virtual void Affine		(HxModule hsrc, HxModule hmatrix, int interpolation);
	virtual void Mirror		(HxModule hsrc, int mode);
	virtual void Rotate		(HxModule hsrc, int mode);
	virtual void Transpose	(HxModule hsrc);
	virtual void Scale		(HxModule hsrc, double sx, double sy, int interpolation);

	// Math
	virtual void Math		(HxModule hsrc, ExMath type);

	// Operation (Not)
	virtual void Not		(HxModule hsrc);

	// Operation (Arithmetic)
	virtual void Add		(HxModule hsrc, HxModule hval);
	virtual void Add		(HxModule hsrc, double value);
	virtual void Mul		(HxModule hsrc, HxModule hval);
	virtual void Mul		(HxModule hsrc, double value);
	virtual void Sub		(HxModule hsrc, HxModule hval);
	virtual void Sub		(HxModule hsrc, double value);
	virtual void Sub		(double value, HxModule hsrc);
	virtual void Div		(HxModule hsrc, HxModule hval);
	virtual void Div		(HxModule hsrc, double value);
	virtual void Div		(double value, HxModule hsrc);
	virtual void Mod		(HxModule hsrc, HxModule hval);
	virtual void Mod		(HxModule hsrc, double value);
	virtual void Mod		(double value, HxModule hsrc);
	virtual void Pow		(HxModule hsrc, HxModule hval);
	virtual void Pow		(HxModule hsrc, double value);
	virtual void Pow		(double value, HxModule hsrc);
	virtual void Atan2		(HxModule hsrc, HxModule hval);
	virtual void Atan2		(HxModule hsrc, double value);
	virtual void Atan2		(double value, HxModule hsrc);
	virtual void Diff		(HxModule hsrc, HxModule hval);
	virtual void Diff		(HxModule hsrc, double value);
	virtual void Max		(HxModule hsrc, HxModule hval);
	virtual void Max		(HxModule hsrc, double value);
	virtual void Min		(HxModule hsrc, HxModule hval);
	virtual void Min		(HxModule hsrc, double value);

	// Operation (Logic)
	virtual void And		(HxModule hsrc, HxModule hval);
	virtual void And		(HxModule hsrc, unsigned int value);
	virtual void Nand		(HxModule hsrc, HxModule hval);
	virtual void Nand		(HxModule hsrc, unsigned int value);
	virtual void Or			(HxModule hsrc, HxModule hval);
	virtual void Or			(HxModule hsrc, unsigned int value);
	virtual void Xor		(HxModule hsrc, HxModule hval);
	virtual void Xor		(HxModule hsrc, unsigned int value);
};

}

#pragma pack(pop)

#endif
