/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXCIRCLED_H_INCLUDED_
#define _TXCIRCLED_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxCircleD
{
	double	X;
	double	Y;
	double	Radius;

#if defined(__cplusplus)
	static inline TxCircleD Default()
	{
		TxCircleD result;
		result.X = 0;
		result.Y = 0;
		result.Radius = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxCircleD();
	TxCircleD(double x, double y, double radius);
	TxCircleD(TxPointD center, double radius);

	bool operator == (const TxCircleD& cmp) const;
	bool operator != (const TxCircleD& cmp) const;

	operator TxCircleI() const;

	TxPointD Center() const;
	void Center(const TxPointD& value);

	TxEllipseD ToEllipse() const;
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
template<> inline TxModel ModelOf<TxCircleD>() { return TxModel::From(ExType::F64, 3); }
}	// xie
#endif	// __cplusplus

#endif
