/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXPOINTD_H_INCLUDED_
#define _TXPOINTD_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxPointD
{
	double	X;
	double	Y;

#if defined(__cplusplus)
	static inline TxPointD Default()
	{
		TxPointD result;
		result.X = 0;
		result.Y = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxPointD();
	TxPointD(double x, double y);

	bool operator == (const TxPointD& cmp) const;
	bool operator != (const TxPointD& cmp) const;

	operator TxPointI() const;

	TxPointD  operator + (const TxPointD& value) const;
	TxPointD  operator + (double value) const;
	TxPointD& operator += (const TxPointD& value);
	TxPointD& operator += (double value);

	TxPointD  operator - (const TxPointD& value) const;
	TxPointD  operator - (double value) const;
	TxPointD& operator -= (const TxPointD& value);
	TxPointD& operator -= (double value);

	TxPointD  operator * (const TxPointD& value) const;
	TxPointD  operator * (double value) const;
	TxPointD& operator *= (const TxPointD& value);
	TxPointD& operator *= (double value);

	TxPointD  operator / (const TxPointD& value) const;
	TxPointD  operator / (double value) const;
	TxPointD& operator /= (const TxPointD& value);
	TxPointD& operator /= (double value);
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
template<> inline TxModel ModelOf<TxPointD>() { return TxModel::From(ExType::F64, 2); }
}	// xie
#endif	// __cplusplus

#endif
