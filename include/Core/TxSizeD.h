/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXSIZED_H_INCLUDED_
#define _TXSIZED_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxSizeD
{
	double	Width;
	double	Height;

#if defined(__cplusplus)
	static inline TxSizeD Default()
	{
		TxSizeD result;
		result.Width = 0;
		result.Height = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxSizeD();
	TxSizeD(double width, double height);

	bool operator == (const TxSizeD& cmp) const;
	bool operator != (const TxSizeD& cmp) const;

	operator TxSizeI() const;

	TxSizeD  operator + (const TxSizeD& value) const;
	TxSizeD  operator + (double value) const;
	TxSizeD& operator += (const TxSizeD& value);
	TxSizeD& operator += (double value);

	TxSizeD  operator - (const TxSizeD& value) const;
	TxSizeD  operator - (double value) const;
	TxSizeD& operator -= (const TxSizeD& value);
	TxSizeD& operator -= (double value);

	TxSizeD  operator * (const TxSizeD& value) const;
	TxSizeD  operator * (double value) const;
	TxSizeD& operator *= (const TxSizeD& value);
	TxSizeD& operator *= (double value);

	TxSizeD  operator / (const TxSizeD& value) const;
	TxSizeD  operator / (double value) const;
	TxSizeD& operator /= (const TxSizeD& value);
	TxSizeD& operator /= (double value);
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
template<> inline TxModel ModelOf<TxSizeD>() { return TxModel::From(ExType::F64, 2); }
}	// xie
#endif	// __cplusplus

#endif
