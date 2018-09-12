/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXELLIPSEI_H_INCLUDED_
#define _TXELLIPSEI_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxEllipseI
{
	int	X;
	int	Y;
	int	RadiusX;
	int	RadiusY;

#if defined(__cplusplus)
	static inline TxEllipseI Default()
	{
		TxEllipseI result;
		result.X = 0;
		result.Y = 0;
		result.RadiusX = 0;
		result.RadiusY = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxEllipseI();
	TxEllipseI(int x, int y, int radius_x, int radius_y);
	TxEllipseI(TxPointI center, int radius_x, int radius_y);

	bool operator == (const TxEllipseI& cmp) const;
	bool operator != (const TxEllipseI& cmp) const;

	operator TxEllipseD() const;

	TxPointI Center() const;
	void Center(const TxPointI& value);
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
template<> inline TxModel ModelOf<TxEllipseI>() { return TxModel::From(ExType::S32, 4); }
}	// xie
#endif	// __cplusplus

#endif
