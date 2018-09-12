/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXPOINTI_H_INCLUDED_
#define _TXPOINTI_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxPointI
{
	int	X;
	int	Y;

#if defined(__cplusplus)
	static inline TxPointI Default()
	{
		TxPointI result;
		result.X = 0;
		result.Y = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxPointI();
	TxPointI(int x, int y);

	bool operator == (const TxPointI& cmp) const;
	bool operator != (const TxPointI& cmp) const;

	operator TxPointD() const;

	TxPointI  operator + (const TxPointI& value) const;
	TxPointI  operator + (int value) const;
	TxPointD  operator + (double value) const;

	TxPointI& operator += (const TxPointI& value);
	TxPointI& operator += (int value);
	TxPointI& operator += (double value);

	TxPointI  operator - (const TxPointI& value) const;
	TxPointI  operator - (int value) const;
	TxPointD  operator - (double value) const;

	TxPointI& operator -= (const TxPointI& value);
	TxPointI& operator -= (int value);
	TxPointI& operator -= (double value);

	TxPointI  operator * (const TxPointI& value) const;
	TxPointI  operator * (int value) const;
	TxPointD  operator * (double value) const;

	TxPointI& operator *= (const TxPointI& value);
	TxPointI& operator *= (int value);
	TxPointI& operator *= (double value);

	TxPointI  operator / (const TxPointI& value) const;
	TxPointI  operator / (int value) const;
	TxPointD  operator / (double value) const;

	TxPointI& operator /= (const TxPointI& value);
	TxPointI& operator /= (int value);
	TxPointI& operator /= (double value);
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
template<> inline TxModel ModelOf<TxPointI>() { return TxModel::From(ExType::S32, 2); }
}	// xie
#endif	// __cplusplus

#endif
