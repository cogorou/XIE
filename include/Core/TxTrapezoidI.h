/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXTRAPEZOIDI_H_INCLUDED_
#define _TXTRAPEZOIDI_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxTrapezoidI
{
	int	X1;
	int	Y1;
	int	X2;
	int	Y2;
	int	X3;
	int	Y3;
	int	X4;
	int	Y4;

#if defined(__cplusplus)
	static inline TxTrapezoidI Default()
	{
		TxTrapezoidI result;
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
	TxTrapezoidI();
	TxTrapezoidI(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4);
	TxTrapezoidI(TxPointI p1, TxPointI p2, TxPointI p3, TxPointI p4);

	bool operator == (const TxTrapezoidI& cmp) const;
	bool operator != (const TxTrapezoidI& cmp) const;

	TxPointI Vertex1() const;
	void Vertex1(const TxPointI& value);

	TxPointI Vertex2() const;
	void Vertex2(const TxPointI& value);

	TxPointI Vertex3() const;
	void Vertex3(const TxPointI& value);

	TxPointI Vertex4() const;
	void Vertex4(const TxPointI& value);

	operator TxTrapezoidD() const;
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
template<> inline TxModel ModelOf<TxTrapezoidI>() { return TxModel::From(ExType::S32, 8); }
}	// xie
#endif	// __cplusplus

#endif
