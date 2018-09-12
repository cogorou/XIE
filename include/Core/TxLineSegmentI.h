/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXLINESEGMENTI_H_INCLUDED_
#define _TXLINESEGMENTI_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxLineSegmentI
{
	int	X1;
	int	Y1;
	int	X2;
	int	Y2;

#if defined(__cplusplus)
	static inline TxLineSegmentI Default()
	{
		TxLineSegmentI result;
		result.X1 = 0;
		result.Y1 = 0;
		result.X2 = 0;
		result.Y2 = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxLineSegmentI();
	TxLineSegmentI(int x1, int y1, int x2, int y2);
	TxLineSegmentI(TxPointI st, TxPointI ed);

	bool operator == (const TxLineSegmentI& cmp) const;
	bool operator != (const TxLineSegmentI& cmp) const;

	operator TxLineSegmentD() const;

	TxPointI Point1() const;
	void Point1(const TxPointI& value);

	TxPointI Point2() const;
	void Point2(const TxPointI& value);

	TxLineI ToLine() const;
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
template<> inline TxModel ModelOf<TxLineSegmentI>() { return TxModel::From(ExType::S32, 4); }
}	// xie
#endif	// __cplusplus

#endif
