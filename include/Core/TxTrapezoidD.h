/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXTRAPEZOIDD_H_INCLUDED_
#define _TXTRAPEZOIDD_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxTrapezoidD
{
	double	X1;
	double	Y1;
	double	X2;
	double	Y2;
	double	X3;
	double	Y3;
	double	X4;
	double	Y4;

#if defined(__cplusplus)
	static inline TxTrapezoidD Default()
	{
		TxTrapezoidD result;
		result.X1 = 0;
		result.Y1 = 0;
		result.X2 = 0;
		result.Y2 = 0;
		result.X3 = 0;
		result.Y3 = 0;
		result.X4 = 0;
		result.Y4 = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxTrapezoidD();
	TxTrapezoidD(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4);
	TxTrapezoidD(TxPointD p1, TxPointD p2, TxPointD p3, TxPointD p4);

	bool operator == (const TxTrapezoidD& cmp) const;
	bool operator != (const TxTrapezoidD& cmp) const;

	TxPointD Vertex1() const;
	void Vertex1(const TxPointD& value);

	TxPointD Vertex2() const;
	void Vertex2(const TxPointD& value);

	TxPointD Vertex3() const;
	void Vertex3(const TxPointD& value);

	TxPointD Vertex4() const;
	void Vertex4(const TxPointD& value);

	operator TxTrapezoidI() const;
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

// ============================================================
#if defined(__cplusplus) && !defined(XIE_TEMPLATE_SPECIALIZE_DISABLED)
namespace xie
{
template<> inline TxModel ModelOf<TxTrapezoidD>() { return TxModel::From(ExType::F64, 8); }
}	// xie
#endif	// __cplusplus

#endif
