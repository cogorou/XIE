/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXRGB8X3_H_INCLUDED_
#define _TXRGB8X3_H_INCLUDED_

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
template<class TE> struct TxRGBx3
{
	TE	R;
	TE	G;
	TE	B;

	static inline TxRGBx3 Default()
	{
		TxRGBx3 result;
		return result;
	}
	static inline TxModel Model()
	{
		TxModel result;
		result.Type = TypeOf<TE>();
		result.Pack = 3;
		return result;
	}
	TxRGBx3()
	{
		R = 0;
		G = 0;
		B = 0;
	}
	TxRGBx3(TE red, TE green, TE blue)
	{
		R = red;
		G = green;
		B = blue;
	}
	bool operator == (const TxRGBx3& cmp) const
	{
		if (this->R != cmp.R) return false;
		if (this->B != cmp.G) return false;
		if (this->G != cmp.B) return false;
		return true;
	}
	bool operator != (const TxRGBx3& cmp) const
	{
		return !(operator == (cmp));
	}
};
#endif

// ===========================================================================
struct XIE_EXPORT_CLASS TxRGB8x3
{
	unsigned char R;
	unsigned char G;
	unsigned char B;

#if defined(__cplusplus)
	static inline TxRGB8x3 Default()
	{
		TxRGB8x3 result;
		result.R = 0;
		result.G = 0;
		result.B = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxRGB8x3();
	TxRGB8x3(unsigned char red, unsigned char green, unsigned char blue);

	bool operator == (const TxRGB8x3& cmp) const;
	bool operator != (const TxRGB8x3& cmp) const;

	operator TxBGR8x3() const;
	operator TxBGR8x4() const;
	operator TxRGB8x4() const;

	TxRGB8x3  operator + (const TxRGB8x3& value) const;
	TxRGB8x3  operator + (int value) const;
	TxRGB8x3  operator + (double value) const;

	TxRGB8x3& operator += (const TxRGB8x3& value);
	TxRGB8x3& operator += (int value);
	TxRGB8x3& operator += (double value);

	TxRGB8x3  operator - (const TxRGB8x3& value) const;
	TxRGB8x3  operator - (int value) const;
	TxRGB8x3  operator - (double value) const;

	TxRGB8x3& operator -= (const TxRGB8x3& value);
	TxRGB8x3& operator -= (int value);
	TxRGB8x3& operator -= (double value);

	TxRGB8x3  operator * (const TxRGB8x3& value) const;
	TxRGB8x3  operator * (int value) const;
	TxRGB8x3  operator * (double value) const;

	TxRGB8x3& operator *= (const TxRGB8x3& value);
	TxRGB8x3& operator *= (int value);
	TxRGB8x3& operator *= (double value);

	TxRGB8x3  operator / (const TxRGB8x3& value) const;
	TxRGB8x3  operator / (int value) const;
	TxRGB8x3  operator / (double value) const;

	TxRGB8x3& operator /= (const TxRGB8x3& value);
	TxRGB8x3& operator /= (int value);
	TxRGB8x3& operator /= (double value);
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
template<> inline TxModel ModelOf<TxRGB8x3>() { return TxModel::From(ExType::U8, 3); }
}	// xie
#endif	// __cplusplus

#endif
