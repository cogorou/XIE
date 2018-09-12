/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXSIZEI_H_INCLUDED_
#define _TXSIZEI_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxSizeI
{
	int	Width;
	int	Height;

#if defined(__cplusplus)
	static inline TxSizeI Default()
	{
		TxSizeI result;
		result.Width = 0;
		result.Height = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxSizeI();
	TxSizeI(int width, int height);

	bool operator == (const TxSizeI& cmp) const;
	bool operator != (const TxSizeI& cmp) const;

	operator TxSizeD() const;

	TxSizeI  operator + (const TxSizeI& value) const;
	TxSizeI  operator + (int value) const;
	TxSizeD  operator + (double value) const;

	TxSizeI& operator += (const TxSizeI& value);
	TxSizeI& operator += (int value);
	TxSizeI& operator += (double value);

	TxSizeI  operator - (const TxSizeI& value) const;
	TxSizeI  operator - (int value) const;
	TxSizeD  operator - (double value) const;

	TxSizeI& operator -= (const TxSizeI& value);
	TxSizeI& operator -= (int value);
	TxSizeI& operator -= (double value);

	TxSizeI  operator * (const TxSizeI& value) const;
	TxSizeI  operator * (int value) const;
	TxSizeD  operator * (double value) const;

	TxSizeI& operator *= (const TxSizeI& value);
	TxSizeI& operator *= (int value);
	TxSizeI& operator *= (double value);

	TxSizeI  operator / (const TxSizeI& value) const;
	TxSizeI  operator / (int value) const;
	TxSizeD  operator / (double value) const;

	TxSizeI& operator /= (const TxSizeI& value);
	TxSizeI& operator /= (int value);
	TxSizeI& operator /= (double value);
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
template<> inline TxModel ModelOf<TxSizeI>() { return TxModel::From(ExType::S32, 2); }
}	// xie
#endif	// __cplusplus

#endif
