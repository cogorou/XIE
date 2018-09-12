/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXRECTANGLEI_H_INCLUDED_
#define _TXRECTANGLEI_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxRectangleI
{
	int	X;
	int	Y;
	int	Width;
	int	Height;

#if defined(__cplusplus)
	static inline TxRectangleI Default()
	{
		TxRectangleI result;
		result.X = 0;
		result.Y = 0;
		result.Width = 0;
		result.Height = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxRectangleI();
	TxRectangleI(int x, int y, int width, int height);
	TxRectangleI(TxPointI location, TxSizeI size);

	bool operator == (const TxRectangleI& cmp) const;
	bool operator != (const TxRectangleI& cmp) const;

	operator TxRectangleD() const;

	TxPointI Location() const;
	void Location(const TxPointI& value);

	TxSizeI Size() const;
	void Size(const TxSizeI& value);

	TxTrapezoidI ToTrapezoid() const;
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
template<> inline TxModel ModelOf<TxRectangleI>() { return TxModel::From(ExType::S32, 4); }
}	// xie
#endif	// __cplusplus

#endif
