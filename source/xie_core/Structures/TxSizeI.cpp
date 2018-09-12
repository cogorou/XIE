/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxSizeI.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxSizeI::TxSizeI()
{
	Width = 0;
	Height = 0;
}

// ============================================================
TxSizeI::TxSizeI(int width, int height)
{
	Width = width;
	Height = height;
}

// ============================================================
bool TxSizeI::operator == (const TxSizeI& cmp) const
{
	const TxSizeI& src = *this;
	if (src.Width	!= cmp.Width) return false;
	if (src.Height	!= cmp.Height) return false;
	return true;
}

// ============================================================
bool TxSizeI::operator != (const TxSizeI& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxSizeI::operator TxSizeD() const
{
	return TxSizeD(Width, Height);
}

// ============================================================
TxSizeI TxSizeI::operator + (const TxSizeI& value) const
{
	const TxSizeI&	src = *this;
	TxSizeI			ans;
	ans.Width	= src.Width + value.Width;
	ans.Height	= src.Height + value.Height;
	return ans;
}
TxSizeI TxSizeI::operator + (int value) const
{
	const TxSizeI&	src = *this;
	TxSizeI			ans;
	ans.Width	= src.Width + value;
	ans.Height	= src.Height + value;
	return ans;
}
TxSizeD TxSizeI::operator + (double value) const
{
	const TxSizeI&	src = *this;
	TxSizeD			ans;
	ans.Width	= src.Width + value;
	ans.Height	= src.Height + value;
	return ans;
}

// ============================================================
TxSizeI& TxSizeI::operator += (const TxSizeI& value)
{
	TxSizeI&	ans = *this;
	ans.Width	= ans.Width + value.Width;
	ans.Height	= ans.Height + value.Height;
	return ans;
}
TxSizeI& TxSizeI::operator += (int value)
{
	TxSizeI&	ans = *this;
	ans.Width	= ans.Width + value;
	ans.Height	= ans.Height + value;
	return ans;
}
TxSizeI& TxSizeI::operator += (double value)
{
	TxSizeI&	ans = *this;
	ans.Width	= (int)round(ans.Width + value);
	ans.Height	= (int)round(ans.Height + value);
	return ans;
}

// ============================================================
TxSizeI TxSizeI::operator - (const TxSizeI& value) const
{
	const TxSizeI&	src = *this;
	TxSizeI			ans;
	ans.Width	= src.Width - value.Width;
	ans.Height	= src.Height - value.Height;
	return ans;
}
TxSizeI TxSizeI::operator - (int value) const
{
	const TxSizeI&	src = *this;
	TxSizeI			ans;
	ans.Width	= src.Width - value;
	ans.Height	= src.Height - value;
	return ans;
}
TxSizeD TxSizeI::operator - (double value) const
{
	const TxSizeI&	src = *this;
	TxSizeD			ans;
	ans.Width	= src.Width - value;
	ans.Height	= src.Height - value;
	return ans;
}

// ============================================================
TxSizeI& TxSizeI::operator -= (const TxSizeI& value)
{
	TxSizeI&	ans = *this;
	ans.Width	= ans.Width - value.Width;
	ans.Height	= ans.Height - value.Height;
	return ans;
}
TxSizeI& TxSizeI::operator -= (int value)
{
	TxSizeI&	ans = *this;
	ans.Width	= ans.Width - value;
	ans.Height	= ans.Height - value;
	return ans;
}
TxSizeI& TxSizeI::operator -= (double value)
{
	TxSizeI&	ans = *this;
	ans.Width	= (int)round(ans.Width - value);
	ans.Height	= (int)round(ans.Height - value);
	return ans;
}

// ============================================================
TxSizeI TxSizeI::operator * (const TxSizeI& value) const
{
	const TxSizeI&	src = *this;
	TxSizeI			ans;
	ans.Width	= src.Width * value.Width;
	ans.Height	= src.Height * value.Height;
	return ans;
}
TxSizeI TxSizeI::operator * (int value) const
{
	const TxSizeI&	src = *this;
	TxSizeI			ans;
	ans.Width	= src.Width * value;
	ans.Height	= src.Height * value;
	return ans;
}
TxSizeD TxSizeI::operator * (double value) const
{
	const TxSizeI&	src = *this;
	TxSizeD			ans;
	ans.Width	= src.Width * value;
	ans.Height	= src.Height * value;
	return ans;
}

// ============================================================
TxSizeI& TxSizeI::operator *= (const TxSizeI& value)
{
	TxSizeI&	ans = *this;
	ans.Width	= ans.Width * value.Width;
	ans.Height	= ans.Height * value.Height;
	return ans;
}
TxSizeI& TxSizeI::operator *= (int value)
{
	TxSizeI&	ans = *this;
	ans.Width	= ans.Width * value;
	ans.Height	= ans.Height * value;
	return ans;
}
TxSizeI& TxSizeI::operator *= (double value)
{
	TxSizeI&	ans = *this;
	ans.Width	= (int)round(ans.Width * value);
	ans.Height	= (int)round(ans.Height * value);
	return ans;
}

// ============================================================
TxSizeI TxSizeI::operator / (const TxSizeI& value) const
{
	const TxSizeI&	src = *this;
	TxSizeI			ans;
	ans.Width	= src.Width / value.Width;
	ans.Height	= src.Height / value.Height;
	return ans;
}
TxSizeI TxSizeI::operator / (int value) const
{
	const TxSizeI&	src = *this;
	TxSizeI			ans;
	ans.Width	= src.Width / value;
	ans.Height	= src.Height / value;
	return ans;
}
TxSizeD TxSizeI::operator / (double value) const
{
	const TxSizeI&	src = *this;
	TxSizeD			ans;
	ans.Width	= src.Width / value;
	ans.Height	= src.Height / value;
	return ans;
}

// ============================================================
TxSizeI& TxSizeI::operator /= (const TxSizeI& value)
{
	TxSizeI&	ans = *this;
	ans.Width	= ans.Width / value.Width;
	ans.Height	= ans.Height / value.Height;
	return ans;
}
TxSizeI& TxSizeI::operator /= (int value)
{
	TxSizeI&	ans = *this;
	ans.Width	= ans.Width / value;
	ans.Height	= ans.Height / value;
	return ans;
}
TxSizeI& TxSizeI::operator /= (double value)
{
	TxSizeI&	ans = *this;
	ans.Width	= (int)round(ans.Width / value);
	ans.Height	= (int)round(ans.Height / value);
	return ans;
}

}
