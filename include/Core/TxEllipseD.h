/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXELLIPSED_H_INCLUDED_
#define _TXELLIPSED_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxEllipseD
{
	double	X;
	double	Y;
	double	RadiusX;
	double	RadiusY;

#if defined(__cplusplus)
	static inline TxEllipseD Default()
	{
		TxEllipseD result;
		result.X = 0;
		result.Y = 0;
		result.RadiusX = 0;
		result.RadiusY = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxEllipseD();
	TxEllipseD(double x, double y, double radius_x, double radius_y);
	TxEllipseD(TxPointD center, double radius_x, double radius_y);

	bool operator == (const TxEllipseD& cmp) const;
	bool operator != (const TxEllipseD& cmp) const;

	operator TxEllipseI() const;

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
	template<> inline TxModel ModelOf<TxEllipseD>() { return TxModel::From(ExType::F64, 4); }
}	// xie
#endif	// __cplusplus

#endif
