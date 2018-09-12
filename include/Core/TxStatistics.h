/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXSTATISTICS_H_INCLUDED_
#define _TXSTATISTICS_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxStatistics
{
	double	Count;
	double	Max;
	double	Min;
	double	Sum1;
	double	Sum2;

#if defined(__cplusplus)
	static inline TxStatistics Default()
	{
		TxStatistics result;
		result.Count = 0;
		result.Max = -1.7976931348623158e+308;
		result.Min = +1.7976931348623158e+308;
		result.Sum1 = 0;
		result.Sum2 = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxStatistics();
	TxStatistics(double count, double maxval, double minval, double sum1, double sum2);

	void Reset();

	bool operator == (const TxStatistics& cmp) const;
	bool operator != (const TxStatistics& cmp) const;

	TxStatistics operator + (const TxStatistics& value) const;
	TxStatistics operator + (double value) const;
	TxStatistics operator + (int value) const;

	TxStatistics& operator += (const TxStatistics& value);
	TxStatistics& operator += (double value);
	TxStatistics& operator += (int value);

	double Mean();
	double Sigma();
	double Variance();
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
template<> inline TxModel ModelOf<TxStatistics>() { return TxModel::From(ExType::F64, 5); }
}	// xie
#endif	// __cplusplus

#endif
