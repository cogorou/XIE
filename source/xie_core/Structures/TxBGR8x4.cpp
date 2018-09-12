/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxBGR8x4.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxBGR8x4::TxBGR8x4()
{
	R = 0;
	G = 0;
	B = 0;
	A = 0xFF;
}

// ============================================================
TxBGR8x4::TxBGR8x4(unsigned char red, unsigned char green, unsigned char blue)
{
	R = red;
	G = green;
	B = blue;
	A = 0xFF;
}

// ============================================================
TxBGR8x4::TxBGR8x4(unsigned char red, unsigned char green, unsigned char blue, unsigned char alpha)
{
	R = red;
	G = green;
	B = blue;
	A = alpha;
}

// ============================================================
bool TxBGR8x4::operator == (const TxBGR8x4& cmp) const
{
	const TxBGR8x4& src = *this;
	if (src.R	!= cmp.R) return false;
	if (src.G	!= cmp.G) return false;
	if (src.B	!= cmp.B) return false;
//	if (src.A	!= cmp.A) return false;
	return true;
}

// ============================================================
bool TxBGR8x4::operator != (const TxBGR8x4& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxBGR8x4::operator TxBGR8x3() const
{
	return TxBGR8x3(R, G, B);
}
// ============================================================
TxBGR8x4::operator TxRGB8x3() const
{
	return TxRGB8x3(R, G, B);
}
// ============================================================
TxBGR8x4::operator TxRGB8x4() const
{
	return TxRGB8x4(R, G, B, A);
}
// ============================================================
TxBGR8x4 TxBGR8x4::operator + (const TxBGR8x4& value) const
{
	const TxBGR8x4&	src = *this;
	TxBGR8x4		ans;
	ans.R	= src.R + value.R;
	ans.G	= src.G + value.G;
	ans.B	= src.B + value.B;
	ans.A	= src.A;
	return ans;
}
TxBGR8x4 TxBGR8x4::operator + (int value) const
{
	const TxBGR8x4&	src = *this;
	TxBGR8x4		ans;
	ans.R	= (unsigned char)(src.R + value);
	ans.G	= (unsigned char)(src.G + value);
	ans.B	= (unsigned char)(src.B + value);
	ans.A	= src.A;
	return ans;
}
TxBGR8x4 TxBGR8x4::operator + (double value) const
{
	const TxBGR8x4&	src = *this;
	TxBGR8x4		ans;
	ans.R	= (unsigned char)round(src.R + value);
	ans.G	= (unsigned char)round(src.G + value);
	ans.B	= (unsigned char)round(src.B + value);
	ans.A	= src.A;
	return ans;
}

// ============================================================
TxBGR8x4& TxBGR8x4::operator += (const TxBGR8x4& value)
{
	TxBGR8x4&	ans = *this;
	ans.R	= ans.R + value.R;
	ans.G	= ans.G + value.G;
	ans.B	= ans.B + value.B;
	return ans;
}
TxBGR8x4& TxBGR8x4::operator += (int value)
{
	TxBGR8x4&	ans = *this;
	ans.R	= (unsigned char)(ans.R + value);
	ans.G	= (unsigned char)(ans.G + value);
	ans.B	= (unsigned char)(ans.B + value);
	return ans;
}
TxBGR8x4& TxBGR8x4::operator += (double value)
{
	TxBGR8x4&	ans = *this;
	ans.R	= (unsigned char)round(ans.R + value);
	ans.G	= (unsigned char)round(ans.G + value);
	ans.B	= (unsigned char)round(ans.B + value);
	return ans;
}

// ============================================================
TxBGR8x4 TxBGR8x4::operator - (const TxBGR8x4& value) const
{
	const TxBGR8x4&	src = *this;
	TxBGR8x4		ans;
	ans.R	= src.R - value.R;
	ans.G	= src.G - value.G;
	ans.B	= src.B - value.B;
	ans.A	= src.A;
	return ans;
}
TxBGR8x4 TxBGR8x4::operator - (int value) const
{
	const TxBGR8x4&	src = *this;
	TxBGR8x4		ans;
	ans.R	= (unsigned char)(src.R - value);
	ans.G	= (unsigned char)(src.G - value);
	ans.B	= (unsigned char)(src.B - value);
	ans.A	= src.A;
	return ans;
}
TxBGR8x4 TxBGR8x4::operator - (double value) const
{
	const TxBGR8x4&	src = *this;
	TxBGR8x4		ans;
	ans.R	= (unsigned char)round(src.R - value);
	ans.G	= (unsigned char)round(src.G - value);
	ans.B	= (unsigned char)round(src.B - value);
	ans.A	= src.A;
	return ans;
}

// ============================================================
TxBGR8x4& TxBGR8x4::operator -= (const TxBGR8x4& value)
{
	TxBGR8x4&	ans = *this;
	ans.R	= ans.R - value.R;
	ans.G	= ans.G - value.G;
	ans.B	= ans.B - value.B;
	return ans;
}
TxBGR8x4& TxBGR8x4::operator -= (int value)
{
	TxBGR8x4&	ans = *this;
	ans.R	= (unsigned char)(ans.R - value);
	ans.G	= (unsigned char)(ans.G - value);
	ans.B	= (unsigned char)(ans.B - value);
	return ans;
}
TxBGR8x4& TxBGR8x4::operator -= (double value)
{
	TxBGR8x4&	ans = *this;
	ans.R	= (unsigned char)round(ans.R - value);
	ans.G	= (unsigned char)round(ans.G - value);
	ans.B	= (unsigned char)round(ans.B - value);
	return ans;
}

// ============================================================
TxBGR8x4 TxBGR8x4::operator * (const TxBGR8x4& value) const
{
	const TxBGR8x4&	src = *this;
	TxBGR8x4		ans;
	ans.R	= src.R * value.R;
	ans.G	= src.G * value.G;
	ans.B	= src.B * value.B;
	ans.A	= src.A;
	return ans;
}
TxBGR8x4 TxBGR8x4::operator * (int value) const
{
	const TxBGR8x4&	src = *this;
	TxBGR8x4		ans;
	ans.R	= (unsigned char)(src.R * value);
	ans.G	= (unsigned char)(src.G * value);
	ans.B	= (unsigned char)(src.B * value);
	ans.A	= src.A;
	return ans;
}
TxBGR8x4 TxBGR8x4::operator * (double value) const
{
	const TxBGR8x4&	src = *this;
	TxBGR8x4		ans;
	ans.R	= (unsigned char)round(src.R * value);
	ans.G	= (unsigned char)round(src.G * value);
	ans.B	= (unsigned char)round(src.B * value);
	ans.A	= src.A;
	return ans;
}

// ============================================================
TxBGR8x4& TxBGR8x4::operator *= (const TxBGR8x4& value)
{
	TxBGR8x4&	ans = *this;
	ans.R	= ans.R * value.R;
	ans.G	= ans.G * value.G;
	ans.B	= ans.B * value.B;
	return ans;
}
TxBGR8x4& TxBGR8x4::operator *= (int value)
{
	TxBGR8x4&	ans = *this;
	ans.R	= (unsigned char)(ans.R * value);
	ans.G	= (unsigned char)(ans.G * value);
	ans.B	= (unsigned char)(ans.B * value);
	return ans;
}
TxBGR8x4& TxBGR8x4::operator *= (double value)
{
	TxBGR8x4&	ans = *this;
	ans.R	= (unsigned char)round(ans.R * value);
	ans.G	= (unsigned char)round(ans.G * value);
	ans.B	= (unsigned char)round(ans.B * value);
	return ans;
}

// ============================================================
TxBGR8x4 TxBGR8x4::operator / (const TxBGR8x4& value) const
{
	const TxBGR8x4&	src = *this;
	TxBGR8x4		ans;
	ans.R	= src.R / value.R;
	ans.G	= src.G / value.G;
	ans.B	= src.B / value.B;
	ans.A	= src.A;
	return ans;
}
TxBGR8x4 TxBGR8x4::operator / (int value) const
{
	const TxBGR8x4&	src = *this;
	TxBGR8x4		ans;
	ans.R	= (unsigned char)(src.R / value);
	ans.G	= (unsigned char)(src.G / value);
	ans.B	= (unsigned char)(src.B / value);
	ans.A	= src.A;
	return ans;
}
TxBGR8x4 TxBGR8x4::operator / (double value) const
{
	const TxBGR8x4&	src = *this;
	TxBGR8x4		ans;
	ans.R	= (unsigned char)round(src.R / value);
	ans.G	= (unsigned char)round(src.G / value);
	ans.B	= (unsigned char)round(src.B / value);
	ans.A	= src.A;
	return ans;
}

// ============================================================
TxBGR8x4& TxBGR8x4::operator /= (const TxBGR8x4& value)
{
	TxBGR8x4&	ans = *this;
	ans.R	= ans.R / value.R;
	ans.G	= ans.G / value.G;
	ans.B	= ans.B / value.B;
	return ans;
}
TxBGR8x4& TxBGR8x4::operator /= (int value)
{
	TxBGR8x4&	ans = *this;
	ans.R	= (unsigned char)(ans.R / value);
	ans.G	= (unsigned char)(ans.G / value);
	ans.B	= (unsigned char)(ans.B / value);
	return ans;
}
TxBGR8x4& TxBGR8x4::operator /= (double value)
{
	TxBGR8x4&	ans = *this;
	ans.R	= (unsigned char)round(ans.R / value);
	ans.G	= (unsigned char)round(ans.G / value);
	ans.B	= (unsigned char)round(ans.B / value);
	return ans;
}

}
