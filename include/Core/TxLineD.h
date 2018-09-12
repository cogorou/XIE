/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXLINED_H_INCLUDED_
#define _TXLINED_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxLineD
{
	double	A;
	double	B;
	double	C;

#if defined(__cplusplus)
	static inline TxLineD Default()
	{
		TxLineD result;
		result.A = 0;
		result.B = 0;
		result.C = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxLineD();
	TxLineD(double a, double b, double c);

	bool operator == (const TxLineD& cmp) const;
	bool operator != (const TxLineD& cmp) const;

	operator TxLineI() const;

	TxLineSegmentD ToLineSegment(const TxRectangleD& region) const;
	TxLineSegmentD ToLineSegment(const TxPointD& st, const TxPointD& ed) const;
	TxLineSegmentD ToLineSegment(double x1, double y1, double x2, double y2) const;
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
template<> inline TxModel ModelOf<TxLineD>() { return TxModel::From(ExType::F64, 3); }
}	// xie
#endif	// __cplusplus

#endif
