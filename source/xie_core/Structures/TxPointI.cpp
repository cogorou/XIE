/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxPointI.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxPointI::TxPointI()
{
	X = 0;
	Y = 0;
}

// ============================================================
TxPointI::TxPointI(int x, int y)
{
	X = x;
	Y = y;
}

// ============================================================
bool TxPointI::operator == (const TxPointI& cmp) const
{
	const TxPointI& src = *this;
	if (src.X	!= cmp.X) return false;
	if (src.Y	!= cmp.Y) return false;
	return true;
}

// ============================================================
bool TxPointI::operator != (const TxPointI& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxPointI::operator TxPointD() const
{
	return TxPointD(X, Y);
}

// ============================================================
TxPointI TxPointI::operator + (const TxPointI& value) const
{
	const TxPointI&	src = *this;
	TxPointI		ans;
	ans.X	= src.X + value.X;
	ans.Y	= src.Y + value.Y;
	return ans;
}
TxPointI TxPointI::operator + (int value) const
{
	const TxPointI&	src = *this;
	TxPointI		ans;
	ans.X	= src.X + value;
	ans.Y	= src.Y + value;
	return ans;
}
TxPointD TxPointI::operator + (double value) const
{
	const TxPointI&	src = *this;
	TxPointD		ans;
	ans.X	= src.X + value;
	ans.Y	= src.Y + value;
	return ans;
}

// ============================================================
TxPointI& TxPointI::operator += (const TxPointI& value)
{
	TxPointI&	ans = *this;
	ans.X	= ans.X + value.X;
	ans.Y	= ans.Y + value.Y;
	return ans;
}
TxPointI& TxPointI::operator += (int value)
{
	TxPointI&	ans = *this;
	ans.X	= ans.X + value;
	ans.Y	= ans.Y + value;
	return ans;
}
TxPointI& TxPointI::operator += (double value)
{
	TxPointI&	ans = *this;
	ans.X	= (int)round(ans.X + value);
	ans.Y	= (int)round(ans.Y + value);
	return ans;
}

// ============================================================
TxPointI TxPointI::operator - (const TxPointI& value) const
{
	const TxPointI&	src = *this;
	TxPointI		ans;
	ans.X	= src.X - value.X;
	ans.Y	= src.Y - value.Y;
	return ans;
}
TxPointI TxPointI::operator - (int value) const
{
	const TxPointI&	src = *this;
	TxPointI		ans;
	ans.X	= src.X - value;
	ans.Y	= src.Y - value;
	return ans;
}
TxPointD TxPointI::operator - (double value) const
{
	const TxPointI&	src = *this;
	TxPointD		ans;
	ans.X	= src.X - value;
	ans.Y	= src.Y - value;
	return ans;
}

// ============================================================
TxPointI& TxPointI::operator -= (const TxPointI& value)
{
	TxPointI&	ans = *this;
	ans.X	= ans.X - value.X;
	ans.Y	= ans.Y - value.Y;
	return ans;
}
TxPointI& TxPointI::operator -= (int value)
{
	TxPointI&	ans = *this;
	ans.X	= ans.X - value;
	ans.Y	= ans.Y - value;
	return ans;
}
TxPointI& TxPointI::operator -= (double value)
{
	TxPointI&	ans = *this;
	ans.X	= (int)round(ans.X - value);
	ans.Y	= (int)round(ans.Y - value);
	return ans;
}

// ============================================================
TxPointI TxPointI::operator * (const TxPointI& value) const
{
	const TxPointI&	src = *this;
	TxPointI		ans;
	ans.X	= src.X * value.X;
	ans.Y	= src.Y * value.Y;
	return ans;
}
TxPointI TxPointI::operator * (int value) const
{
	const TxPointI&	src = *this;
	TxPointI		ans;
	ans.X	= src.X * value;
	ans.Y	= src.Y * value;
	return ans;
}
TxPointD TxPointI::operator * (double value) const
{
	const TxPointI&	src = *this;
	TxPointD		ans;
	ans.X	= src.X * value;
	ans.Y	= src.Y * value;
	return ans;
}

// ============================================================
TxPointI& TxPointI::operator *= (const TxPointI& value)
{
	TxPointI&	ans = *this;
	ans.X	= ans.X * value.X;
	ans.Y	= ans.Y * value.Y;
	return ans;
}
TxPointI& TxPointI::operator *= (int value)
{
	TxPointI&	ans = *this;
	ans.X	= ans.X * value;
	ans.Y	= ans.Y * value;
	return ans;
}
TxPointI& TxPointI::operator *= (double value)
{
	TxPointI&	ans = *this;
	ans.X	= (int)round(ans.X * value);
	ans.Y	= (int)round(ans.Y * value);
	return ans;
}

// ============================================================
TxPointI TxPointI::operator / (const TxPointI& value) const
{
	const TxPointI&	src = *this;
	TxPointI		ans;
	ans.X	= src.X / value.X;
	ans.Y	= src.Y / value.Y;
	return ans;
}
TxPointI TxPointI::operator / (int value) const
{
	const TxPointI&	src = *this;
	TxPointI		ans;
	ans.X	= src.X / value;
	ans.Y	= src.Y / value;
	return ans;
}
TxPointD TxPointI::operator / (double value) const
{
	const TxPointI&	src = *this;
	TxPointD		ans;
	ans.X	= src.X * value;
	ans.Y	= src.Y * value;
	return ans;
}

// ============================================================
TxPointI& TxPointI::operator /= (const TxPointI& value)
{
	TxPointI&	ans = *this;
	ans.X	= ans.X / value.X;
	ans.Y	= ans.Y / value.Y;
	return ans;
}
TxPointI& TxPointI::operator /= (int value)
{
	TxPointI&	ans = *this;
	ans.X	= ans.X / value;
	ans.Y	= ans.Y / value;
	return ans;
}
TxPointI& TxPointI::operator /= (double value)
{
	TxPointI&	ans = *this;
	ans.X	= (int)round(ans.X / value);
	ans.Y	= (int)round(ans.Y / value);
	return ans;
}

}
