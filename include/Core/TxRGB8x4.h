/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXRGB8X4_H_INCLUDED_
#define _TXRGB8X4_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

// ===========================================================================
#if defined(__cplusplus)
template<class TE> struct TxRGBx4
{
	TE	R;
	TE	G;
	TE	B;
	TE	A;

	static inline TxRGBx4 Default()
	{
		TxRGBx4 result;
		return result;
	}
	static inline TxModel Model()
	{
		TxModel result;
		result.Type = TypeOf<TE>();
		result.Pack = 4;
		return result;
	}
	TxRGBx4()
	{
		R = 0;
		G = 0;
		B = 0;
		A = 0;
	}
	TxRGBx4(TE red, TE green, TE blue, TE alpha = 0)
	{
		R = red;
		G = green;
		B = blue;
		A = alpha;
	}
	bool operator == (const TxRGBx4& cmp) const
	{
		if (this->R != cmp.R) return false;
		if (this->B != cmp.G) return false;
		if (this->G != cmp.B) return false;
		if (this->A != cmp.A) return false;
		return true;
	}
	bool operator != (const TxRGBx4& cmp) const
	{
		return !(operator == (cmp));
	}
};
#endif

// ===========================================================================
struct XIE_EXPORT_CLASS TxRGB8x4
{
	unsigned char R;
	unsigned char G;
	unsigned char B;
	unsigned char A;

#if defined(__cplusplus)
	static inline TxRGB8x4 Default()
	{
		TxRGB8x4 result;
		result.R = 0;
		result.G = 0;
		result.B = 0;
		result.A = 0xFF;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxRGB8x4();
	TxRGB8x4(unsigned char red, unsigned char green, unsigned char blue);
	TxRGB8x4(unsigned char red, unsigned char green, unsigned char blue, unsigned char alpha);

	bool operator == (const TxRGB8x4& cmp) const;
	bool operator != (const TxRGB8x4& cmp) const;

	operator TxBGR8x3() const;
	operator TxBGR8x4() const;
	operator TxRGB8x3() const;

	TxRGB8x4  operator + (const TxRGB8x4& value) const;
	TxRGB8x4  operator + (int value) const;
	TxRGB8x4  operator + (double value) const;

	TxRGB8x4& operator += (const TxRGB8x4& value);
	TxRGB8x4& operator += (int value);
	TxRGB8x4& operator += (double value);

	TxRGB8x4  operator - (const TxRGB8x4& value) const;
	TxRGB8x4  operator - (int value) const;
	TxRGB8x4  operator - (double value) const;

	TxRGB8x4& operator -= (const TxRGB8x4& value);
	TxRGB8x4& operator -= (int value);
	TxRGB8x4& operator -= (double value);

	TxRGB8x4  operator * (const TxRGB8x4& value) const;
	TxRGB8x4  operator * (int value) const;
	TxRGB8x4  operator * (double value) const;

	TxRGB8x4& operator *= (const TxRGB8x4& value);
	TxRGB8x4& operator *= (int value);
	TxRGB8x4& operator *= (double value);

	TxRGB8x4  operator / (const TxRGB8x4& value) const;
	TxRGB8x4  operator / (int value) const;
	TxRGB8x4  operator / (double value) const;

	TxRGB8x4& operator /= (const TxRGB8x4& value);
	TxRGB8x4& operator /= (int value);
	TxRGB8x4& operator /= (double value);
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
template<> inline TxModel ModelOf<TxRGB8x4>() { return TxModel::From(ExType::U8, 4); }
}	// xie
#endif	// __cplusplus

#endif
