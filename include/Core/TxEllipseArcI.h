/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXELLIPSEARCI_H_INCLUDED_
#define _TXELLIPSEARCI_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxEllipseArcI
{
	int	X;
	int	Y;
	int	RadiusX;
	int	RadiusY;
	int	StartAngle;
	int	SweepAngle;

#if defined(__cplusplus)
	static inline TxEllipseArcI Default()
	{
		TxEllipseArcI result;
		result.X = 0;
		result.Y = 0;
		result.RadiusX = 0;
		result.RadiusY = 0;
		result.StartAngle = 0;
		result.SweepAngle = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxEllipseArcI();
	TxEllipseArcI(int x, int y, int radius_x, int radius_y, int start_angle, int sweep_angle);
	TxEllipseArcI(TxPointI center, int radius_x, int radius_y, int start_angle, int sweep_angle);

	bool operator == (const TxEllipseArcI& cmp) const;
	bool operator != (const TxEllipseArcI& cmp) const;

	operator TxEllipseArcD() const;

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
template<> inline TxModel ModelOf<TxEllipseArcI>() { return TxModel::From(ExType::S32, 6); }
}	// xie
#endif	// __cplusplus

#endif
