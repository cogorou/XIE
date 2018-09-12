/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXRANGEI_H_INCLUDED_
#define _TXRANGEI_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxRangeI
{
	int	Lower;
	int	Upper;

#if defined(__cplusplus)
	static inline TxRangeI Default()
	{
		TxRangeI result;
		result.Lower = 0;
		result.Upper = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxRangeI();
	TxRangeI(int lower, int upper);

	bool operator == (const TxRangeI& cmp) const;
	bool operator != (const TxRangeI& cmp) const;

	operator TxRangeD() const;

	TxRangeI  operator + (const TxRangeI& value) const;
	TxRangeI  operator + (int value) const;
	TxRangeD  operator + (double value) const;

	TxRangeI& operator += (const TxRangeI& value);
	TxRangeI& operator += (int value);
	TxRangeI& operator += (double value);

	TxRangeI  operator - (const TxRangeI& value) const;
	TxRangeI  operator - (int value) const;
	TxRangeD  operator - (double value) const;

	TxRangeI& operator -= (const TxRangeI& value);
	TxRangeI& operator -= (int value);
	TxRangeI& operator -= (double value);

	TxRangeI  operator * (const TxRangeI& value) const;
	TxRangeI  operator * (int value) const;
	TxRangeD  operator * (double value) const;

	TxRangeI& operator *= (const TxRangeI& value);
	TxRangeI& operator *= (int value);
	TxRangeI& operator *= (double value);

	TxRangeI  operator / (const TxRangeI& value) const;
	TxRangeI  operator / (int value) const;
	TxRangeD  operator / (double value) const;

	TxRangeI& operator /= (const TxRangeI& value);
	TxRangeI& operator /= (int value);
	TxRangeI& operator /= (double value);
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
template<> inline TxModel ModelOf<TxRangeI>() { return TxModel::From(ExType::S32, 2); }
}	// xie
#endif	// __cplusplus

#endif
