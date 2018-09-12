/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXCIRCLEI_H_INCLUDED_
#define _TXCIRCLEI_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxCircleI
{
	int	X;
	int	Y;
	int	Radius;

#if defined(__cplusplus)
	static inline TxCircleI Default()
	{
		TxCircleI result;
		result.X = 0;
		result.Y = 0;
		result.Radius = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxCircleI();
	TxCircleI(int x, int y, int radius);
	TxCircleI(TxPointI center, int radius);

	bool operator == (const TxCircleI& cmp) const;
	bool operator != (const TxCircleI& cmp) const;

	operator TxCircleD() const;

	TxPointI Center() const;
	void Center(const TxPointI& value);

	TxEllipseI ToEllipse() const;
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
template<> inline TxModel ModelOf<TxCircleI>() { return TxModel::From(ExType::S32, 3); }
}	// xie
#endif	// __cplusplus

#endif
