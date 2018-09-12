/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXELLIPSEARCD_H_INCLUDED_
#define _TXELLIPSEARCD_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxEllipseArcD
{
	double	X;
	double	Y;
	double	RadiusX;
	double	RadiusY;
	double	StartAngle;
	double	SweepAngle;

#if defined(__cplusplus)
	static inline TxEllipseArcD Default()
	{
		TxEllipseArcD result;
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
	TxEllipseArcD();
	TxEllipseArcD(double x, double y, double radius_x, double radius_y, double start_angle, double sweep_angle);
	TxEllipseArcD(TxPointD center, double radius_x, double radius_y, double start_angle, double sweep_angle);

	bool operator == (const TxEllipseArcD& cmp) const;
	bool operator != (const TxEllipseArcD& cmp) const;

	operator TxEllipseArcI() const;

	TxPointD Center() const;
	void Center(const TxPointD& value);
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
	template<> inline TxModel ModelOf<TxEllipseArcD>() { return TxModel::From(ExType::F64, 6); }
}	// xie
#endif	// __cplusplus

#endif
