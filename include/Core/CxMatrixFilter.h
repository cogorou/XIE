/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXMATRIXFILTER_H_INCLUDED_
#define _CXMATRIXFILTER_H_INCLUDED_

#include "xie_core.h"
#include "Core/CxModule.h"
#include "Core/CxArray.h"
#include "Core/TxStatistics.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxMatrixFilter
{
public:
	CxMatrixFilter();
	CxMatrixFilter(HxModule hdst);
	CxMatrixFilter(const CxMatrixFilter& src);
	virtual ~CxMatrixFilter();

	CxMatrixFilter& operator = ( const CxMatrixFilter& src );
	bool operator == ( const CxMatrixFilter& src ) const;
	bool operator != ( const CxMatrixFilter& src ) const;

	HxModule hDst;

public:
	// Basic
	virtual void Cast		(HxModule hsrc);
	virtual void Copy		(HxModule hsrc);

	// Segmentation
	virtual void Compare	(HxModule hsrc, HxModule hcmp, double error_range);

	// GeoTrans
	virtual void Mirror		(HxModule hsrc, int mode);
	virtual void Rotate		(HxModule hsrc, int mode);
	virtual void Transpose	(HxModule hsrc);

	// Linear
	virtual void Invert		(HxModule hsrc);
	virtual void Submatrix	(HxModule hsrc, int row, int col);
	virtual void Product	(HxModule hsrc, HxModule hval);

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
};

}

#pragma pack(pop)

#endif
