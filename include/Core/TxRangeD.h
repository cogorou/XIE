/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXRANGED_H_INCLUDED_
#define _TXRANGED_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxRangeD
{
	double	Lower;
	double	Upper;

#if defined(__cplusplus)
	static inline TxRangeD Default()
	{
		TxRangeD result;
		result.Lower = 0;
		result.Upper = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxRangeD();
	TxRangeD(double lower, double upper);

	bool operator == (const TxRangeD& cmp) const;
	bool operator != (const TxRangeD& cmp) const;

	operator TxRangeI() const;

	TxRangeD  operator + (const TxRangeD& value) const;
	TxRangeD  operator + (double value) const;
	TxRangeD& operator += (const TxRangeD& value);
	TxRangeD& operator += (double value);

	TxRangeD  operator - (const TxRangeD& value) const;
	TxRangeD  operator - (double value) const;
	TxRangeD& operator -= (const TxRangeD& value);
	TxRangeD& operator -= (double value);

	TxRangeD  operator * (const TxRangeD& value) const;
	TxRangeD  operator * (double value) const;
	TxRangeD& operator *= (const TxRangeD& value);
	TxRangeD& operator *= (double value);

	TxRangeD  operator / (const TxRangeD& value) const;
	TxRangeD  operator / (double value) const;
	TxRangeD& operator /= (const TxRangeD& value);
	TxRangeD& operator /= (double value);
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
template<> inline TxModel ModelOf<TxRangeD>() { return TxModel::From(ExType::F64, 2); }
}	// xie
#endif	// __cplusplus

#endif
