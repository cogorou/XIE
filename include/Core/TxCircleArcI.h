/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXCIRCLEARCI_H_INCLUDED_
#define _TXCIRCLEARCI_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxCircleArcI
{
	int	X;
	int	Y;
	int	Radius;
	int	StartAngle;
	int	SweepAngle;

#if defined(__cplusplus)
	static inline TxCircleArcI Default()
	{
		TxCircleArcI result;
		result.X = 0;
		result.Y = 0;
		result.Radius = 0;
		result.StartAngle = 0;
		result.SweepAngle = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxCircleArcI();
	TxCircleArcI(int x, int y, int radius, int start_angle, int sweep_angle);
	TxCircleArcI(TxPointI center, int radius, int start_angle, int sweep_angle);

	bool operator == (const TxCircleArcI& cmp) const;
	bool operator != (const TxCircleArcI& cmp) const;

	operator TxCircleArcD() const;

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
template<> inline TxModel ModelOf<TxCircleArcI>() { return TxModel::From(ExType::S32, 6); }
}	// xie
#endif	// __cplusplus

#endif
