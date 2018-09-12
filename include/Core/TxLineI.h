/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXLINEI_H_INCLUDED_
#define _TXLINEI_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxLineI
{
	int	A;
	int	B;
	int	C;

#if defined(__cplusplus)
	static inline TxLineI Default()
	{
		TxLineI result;
		result.A = 0;
		result.B = 0;
		result.C = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxLineI();
	TxLineI(int a, int b, int c);

	bool operator == (const TxLineI& cmp) const;
	bool operator != (const TxLineI& cmp) const;

	operator TxLineD() const;

	TxLineSegmentI ToLineSegment(const TxRectangleI& region) const;
	TxLineSegmentI ToLineSegment(const TxPointI& st, const TxPointI& ed) const;
	TxLineSegmentI ToLineSegment(int x1, int y1, int x2, int y2) const;
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
template<> inline TxModel ModelOf<TxLineI>() { return TxModel::From(ExType::S32, 3); }
}	// xie
#endif	// __cplusplus

#endif
