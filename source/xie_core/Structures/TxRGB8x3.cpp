/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxRGB8x3.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxRGB8x3::TxRGB8x3()
{
	R = 0;
	G = 0;
	B = 0;
}

// ============================================================
TxRGB8x3::TxRGB8x3(unsigned char red, unsigned char green, unsigned char blue)
{
	R = red;
	G = green;
	B = blue;
}

// ============================================================
bool TxRGB8x3::operator == (const TxRGB8x3& cmp) const
{
	const TxRGB8x3& src = *this;
	if (src.R	!= cmp.R) return false;
	if (src.G	!= cmp.G) return false;
	if (src.B	!= cmp.B) return false;
	return true;
}

// ============================================================
bool TxRGB8x3::operator != (const TxRGB8x3& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxRGB8x3::operator TxBGR8x3() const
{
	return TxBGR8x3(R, G, B);
}
// ============================================================
TxRGB8x3::operator TxBGR8x4() const
{
	return TxBGR8x4(R, G, B);
}
// ============================================================
TxRGB8x3::operator TxRGB8x4() const
{
	return TxRGB8x4(R, G, B);
}
// ============================================================
TxRGB8x3 TxRGB8x3::operator + (const TxRGB8x3& value) const
{
	const TxRGB8x3&	src = *this;
	TxRGB8x3		ans;
	ans.R	= src.R + value.R;
	ans.G	= src.G + value.G;
	ans.B	= src.B + value.B;
	return ans;
}
TxRGB8x3 TxRGB8x3::operator + (int value) const
{
	const TxRGB8x3&	src = *this;
	TxRGB8x3		ans;
	ans.R	= (unsigned char)(src.R + value);
	ans.G	= (unsigned char)(src.G + value);
	ans.B	= (unsigned char)(src.B + value);
	return ans;
}
TxRGB8x3 TxRGB8x3::operator + (double value) const
{
	const TxRGB8x3&	src = *this;
	TxRGB8x3		ans;
	ans.R	= (unsigned char)round(src.R + value);
	ans.G	= (unsigned char)round(src.G + value);
	ans.B	= (unsigned char)round(src.B + value);
	return ans;
}

// ============================================================
TxRGB8x3& TxRGB8x3::operator += (const TxRGB8x3& value)
{
	TxRGB8x3&	ans = *this;
	ans.R	= ans.R + value.R;
	ans.G	= ans.G + value.G;
	ans.B	= ans.B + value.B;
	return ans;
}
TxRGB8x3& TxRGB8x3::operator += (int value)
{
	TxRGB8x3&	ans = *this;
	ans.R	= (unsigned char)(ans.R + value);
	ans.G	= (unsigned char)(ans.G + value);
	ans.B	= (unsigned char)(ans.B + value);
	return ans;
}
TxRGB8x3& TxRGB8x3::operator += (double value)
{
	TxRGB8x3&	ans = *this;
	ans.R	= (unsigned char)round(ans.R + value);
	ans.G	= (unsigned char)round(ans.G + value);
	ans.B	= (unsigned char)round(ans.B + value);
	return ans;
}

// ============================================================
TxRGB8x3 TxRGB8x3::operator - (const TxRGB8x3& value) const
{
	const TxRGB8x3&	src = *this;
	TxRGB8x3		ans;
	ans.R	= src.R - value.R;
	ans.G	= src.G - value.G;
	ans.B	= src.B - value.B;
	return ans;
}
TxRGB8x3 TxRGB8x3::operator - (int value) const
{
	const TxRGB8x3&	src = *this;
	TxRGB8x3		ans;
	ans.R	= (unsigned char)(src.R - value);
	ans.G	= (unsigned char)(src.G - value);
	ans.B	= (unsigned char)(src.B - value);
	return ans;
}
TxRGB8x3 TxRGB8x3::operator - (double value) const
{
	const TxRGB8x3&	src = *this;
	TxRGB8x3		ans;
	ans.R	= (unsigned char)round(src.R - value);
	ans.G	= (unsigned char)round(src.G - value);
	ans.B	= (unsigned char)round(src.B - value);
	return ans;
}

// ============================================================
TxRGB8x3& TxRGB8x3::operator -= (const TxRGB8x3& value)
{
	TxRGB8x3&	ans = *this;
	ans.R	= ans.R - value.R;
	ans.G	= ans.G - value.G;
	ans.B	= ans.B - value.B;
	return ans;
}
TxRGB8x3& TxRGB8x3::operator -= (int value)
{
	TxRGB8x3&	ans = *this;
	ans.R	= (unsigned char)(ans.R - value);
	ans.G	= (unsigned char)(ans.G - value);
	ans.B	= (unsigned char)(ans.B - value);
	return ans;
}
TxRGB8x3& TxRGB8x3::operator -= (double value)
{
	TxRGB8x3&	ans = *this;
	ans.R	= (unsigned char)round(ans.R - value);
	ans.G	= (unsigned char)round(ans.G - value);
	ans.B	= (unsigned char)round(ans.B - value);
	return ans;
}

// ============================================================
TxRGB8x3 TxRGB8x3::operator * (const TxRGB8x3& value) const
{
	const TxRGB8x3&	src = *this;
	TxRGB8x3		ans;
	ans.R	= src.R * value.R;
	ans.G	= src.G * value.G;
	ans.B	= src.B * value.B;
	return ans;
}
TxRGB8x3 TxRGB8x3::operator * (int value) const
{
	const TxRGB8x3&	src = *this;
	TxRGB8x3		ans;
	ans.R	= (unsigned char)(src.R * value);
	ans.G	= (unsigned char)(src.G * value);
	ans.B	= (unsigned char)(src.B * value);
	return ans;
}
TxRGB8x3 TxRGB8x3::operator * (double value) const
{
	const TxRGB8x3&	src = *this;
	TxRGB8x3		ans;
	ans.R	= (unsigned char)round(src.R * value);
	ans.G	= (unsigned char)round(src.G * value);
	ans.B	= (unsigned char)round(src.B * value);
	return ans;
}

// ============================================================
TxRGB8x3& TxRGB8x3::operator *= (const TxRGB8x3& value)
{
	TxRGB8x3&	ans = *this;
	ans.R	= ans.R * value.R;
	ans.G	= ans.G * value.G;
	ans.B	= ans.B * value.B;
	return ans;
}
TxRGB8x3& TxRGB8x3::operator *= (int value)
{
	TxRGB8x3&	ans = *this;
	ans.R	= (unsigned char)(ans.R * value);
	ans.G	= (unsigned char)(ans.G * value);
	ans.B	= (unsigned char)(ans.B * value);
	return ans;
}
TxRGB8x3& TxRGB8x3::operator *= (double value)
{
	TxRGB8x3&	ans = *this;
	ans.R	= (unsigned char)round(ans.R * value);
	ans.G	= (unsigned char)round(ans.G * value);
	ans.B	= (unsigned char)round(ans.B * value);
	return ans;
}

// ============================================================
TxRGB8x3 TxRGB8x3::operator / (const TxRGB8x3& value) const
{
	const TxRGB8x3&	src = *this;
	TxRGB8x3		ans;
	ans.R	= src.R / value.R;
	ans.G	= src.G / value.G;
	ans.B	= src.B / value.B;
	return ans;
}
TxRGB8x3 TxRGB8x3::operator / (int value) const
{
	const TxRGB8x3&	src = *this;
	TxRGB8x3		ans;
	ans.R	= (unsigned char)(src.R / value);
	ans.G	= (unsigned char)(src.G / value);
	ans.B	= (unsigned char)(src.B / value);
	return ans;
}
TxRGB8x3 TxRGB8x3::operator / (double value) const
{
	const TxRGB8x3&	src = *this;
	TxRGB8x3		ans;
	ans.R	= (unsigned char)round(src.R / value);
	ans.G	= (unsigned char)round(src.G / value);
	ans.B	= (unsigned char)round(src.B / value);
	return ans;
}

// ============================================================
TxRGB8x3& TxRGB8x3::operator /= (const TxRGB8x3& value)
{
	TxRGB8x3&	ans = *this;
	ans.R	= ans.R / value.R;
	ans.G	= ans.G / value.G;
	ans.B	= ans.B / value.B;
	return ans;
}
TxRGB8x3& TxRGB8x3::operator /= (int value)
{
	TxRGB8x3&	ans = *this;
	ans.R	= (unsigned char)(ans.R / value);
	ans.G	= (unsigned char)(ans.G / value);
	ans.B	= (unsigned char)(ans.B / value);
	return ans;
}
TxRGB8x3& TxRGB8x3::operator /= (double value)
{
	TxRGB8x3&	ans = *this;
	ans.R	= (unsigned char)round(ans.R / value);
	ans.G	= (unsigned char)round(ans.G / value);
	ans.B	= (unsigned char)round(ans.B / value);
	return ans;
}

}
