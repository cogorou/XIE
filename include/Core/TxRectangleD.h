/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXRECTANGLED_H_INCLUDED_
#define _TXRECTANGLED_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxRectangleD
{
	double	X;
	double	Y;
	double	Width;
	double	Height;

#if defined(__cplusplus)
	static inline TxRectangleD Default()
	{
		TxRectangleD result;
		result.X = 0;
		result.Y = 0;
		result.Width = 0;
		result.Height = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxRectangleD();
	TxRectangleD(double x, double y, double width, double height);
	TxRectangleD(TxPointD location, TxSizeD size);

	bool operator == (const TxRectangleD& cmp) const;
	bool operator != (const TxRectangleD& cmp) const;

	operator TxRectangleI() const;

	TxPointD Location() const;
	void Location(const TxPointD& value);

	TxSizeD Size() const;
	void Size(const TxSizeD& value);

	TxTrapezoidD ToTrapezoid() const;
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
template<> inline TxModel ModelOf<TxRectangleD>() { return TxModel::From(ExType::F64, 4); }
}	// xie
#endif	// __cplusplus

#endif
