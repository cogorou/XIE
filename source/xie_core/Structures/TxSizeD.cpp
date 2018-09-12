/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxSizeD.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxSizeD::TxSizeD()
{
	Width = 0;
	Height = 0;
}

// ============================================================
TxSizeD::TxSizeD(double width, double height)
{
	Width = width;
	Height = height;
}

// ============================================================
bool TxSizeD::operator == (const TxSizeD& cmp) const
{
	const TxSizeD& src = *this;
	if (src.Width	!= cmp.Width) return false;
	if (src.Height	!= cmp.Height) return false;
	return true;
}

// ============================================================
bool TxSizeD::operator != (const TxSizeD& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxSizeD::operator TxSizeI() const
{
	return TxSizeI(
		(int)round(Width),
		(int)round(Height)
		);
}

// ============================================================
TxSizeD TxSizeD::operator + (const TxSizeD& value) const
{
	const TxSizeD&	src = *this;
	TxSizeD			ans;
	ans.Width	= src.Width + value.Width;
	ans.Height	= src.Height + value.Height;
	return ans;
}
TxSizeD TxSizeD::operator + (double value) const
{
	const TxSizeD&	src = *this;
	TxSizeD			ans;
	ans.Width	= src.Width + value;
	ans.Height	= src.Height + value;
	return ans;
}
TxSizeD& TxSizeD::operator += (const TxSizeD& value)
{
	TxSizeD&	src = *this;
	src.Width	+= value.Width;
	src.Height	+= value.Height;
	return src;
}
TxSizeD& TxSizeD::operator += (double value)
{
	TxSizeD&	src = *this;
	src.Width	+= value;
	src.Height	+= value;
	return src;
}

// ============================================================
TxSizeD TxSizeD::operator - (const TxSizeD& value) const
{
	const TxSizeD&	src = *this;
	TxSizeD			ans;
	ans.Width	= src.Width - value.Width;
	ans.Height	= src.Height - value.Height;
	return ans;
}
TxSizeD TxSizeD::operator - (double value) const
{
	const TxSizeD&	src = *this;
	TxSizeD			ans;
	ans.Width	= src.Width - value;
	ans.Height	= src.Height - value;
	return ans;
}
TxSizeD& TxSizeD::operator -= (const TxSizeD& value)
{
	TxSizeD&	src = *this;
	src.Width	-= value.Width;
	src.Height	-= value.Height;
	return src;
}
TxSizeD& TxSizeD::operator -= (double value)
{
	TxSizeD&	src = *this;
	src.Width	-= value;
	src.Height	-= value;
	return src;
}

// ============================================================
TxSizeD TxSizeD::operator * (const TxSizeD& value) const
{
	const TxSizeD&	src = *this;
	TxSizeD			ans;
	ans.Width	= src.Width * value.Width;
	ans.Height	= src.Height * value.Height;
	return ans;
}
TxSizeD TxSizeD::operator * (double value) const
{
	const TxSizeD&	src = *this;
	TxSizeD			ans;
	ans.Width	= src.Width * value;
	ans.Height	= src.Height * value;
	return ans;
}
TxSizeD& TxSizeD::operator *= (const TxSizeD& value)
{
	TxSizeD&	src = *this;
	src.Width	*= value.Width;
	src.Height	*= value.Height;
	return src;
}
TxSizeD& TxSizeD::operator *= (double value)
{
	TxSizeD&	src = *this;
	src.Width	*= value;
	src.Height	*= value;
	return src;
}

// ============================================================
TxSizeD TxSizeD::operator / (const TxSizeD& value) const
{
	const TxSizeD&	src = *this;
	TxSizeD			ans;
	ans.Width	= src.Width / value.Width;
	ans.Height	= src.Height / value.Height;
	return ans;
}
TxSizeD TxSizeD::operator / (double value) const
{
	const TxSizeD&	src = *this;
	TxSizeD			ans;
	ans.Width	= src.Width / value;
	ans.Height	= src.Height / value;
	return src;
}
TxSizeD& TxSizeD::operator /= (const TxSizeD& value)
{
	TxSizeD&	src = *this;
	src.Width	/= value.Width;
	src.Height	/= value.Height;
	return src;
}
TxSizeD& TxSizeD::operator /= (double value)
{
	TxSizeD&	src = *this;
	src.Width	/= value;
	src.Height	/= value;
	return src;
}

}
