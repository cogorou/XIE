/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXLINESEGMENTD_H_INCLUDED_
#define _TXLINESEGMENTD_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxLineSegmentD
{
	double	X1;
	double	Y1;
	double	X2;
	double	Y2;

#if defined(__cplusplus)
	static inline TxLineSegmentD Default()
	{
		TxLineSegmentD result;
		result.X1 = 0;
		result.Y1 = 0;
		result.X2 = 0;
		result.Y2 = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxLineSegmentD();
	TxLineSegmentD(double x1, double y1, double x2, double y2);
	TxLineSegmentD(TxPointD st, TxPointD ed);

	bool operator == (const TxLineSegmentD& cmp) const;
	bool operator != (const TxLineSegmentD& cmp) const;

	operator TxLineSegmentI() const;

	TxPointD Point1() const;
	void Point1(const TxPointD& value);

	TxPointD Point2() const;
	void Point2(const TxPointD& value);

	TxLineD ToLine() const;
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
template<> inline TxModel ModelOf<TxLineSegmentD>() { return TxModel::From(ExType::F64, 4); }
}	// xie
#endif	// __cplusplus

#endif
