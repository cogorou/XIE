/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXCIRCLEARCD_H_INCLUDED_
#define _TXCIRCLEARCD_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxCircleArcD
{
	double	X;
	double	Y;
	double	Radius;
	double	StartAngle;
	double	SweepAngle;

#if defined(__cplusplus)
	static inline TxCircleArcD Default()
	{
		TxCircleArcD result;
		result.X = 0;
		result.Y = 0;
		result.Radius = 0;
		result.StartAngle = 0;
		result.SweepAngle = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxCircleArcD();
	TxCircleArcD(double x, double y, double radius, double start_angle, double sweep_angle);
	TxCircleArcD(TxPointD center, double radius, double start_angle, double sweep_angle);

	bool operator == (const TxCircleArcD& cmp) const;
	bool operator != (const TxCircleArcD& cmp) const;

	operator TxCircleArcI() const;

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
	template<> inline TxModel ModelOf<TxCircleArcD>() { return TxModel::From(ExType::F64, 6); }
}	// xie
#endif	// __cplusplus

#endif
