/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxPointD.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxPointD::TxPointD()
{
	X = 0;
	Y = 0;
}

// ============================================================
TxPointD::TxPointD(double x, double y)
{
	X = x;
	Y = y;
}

// ============================================================
bool TxPointD::operator == (const TxPointD& cmp) const
{
	const TxPointD& src = *this;
	if (src.X	!= cmp.X) return false;
	if (src.Y	!= cmp.Y) return false;
	return true;
}

// ============================================================
bool TxPointD::operator != (const TxPointD& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxPointD::operator TxPointI() const
{
	return TxPointI(
		(int)round(X),
		(int)round(Y)
		);
}

// ============================================================
TxPointD TxPointD::operator + (const TxPointD& value) const
{
	const TxPointD&	src = *this;
	TxPointD		ans;
	ans.X	= src.X + value.X;
	ans.Y	= src.Y + value.Y;
	return ans;
}
TxPointD TxPointD::operator + (double value) const
{
	const TxPointD&	src = *this;
	TxPointD		ans;
	ans.X	= src.X + value;
	ans.Y	= src.Y + value;
	return ans;
}
TxPointD& TxPointD::operator += (const TxPointD& value)
{
	TxPointD&	src = *this;
	src.X	+= value.X;
	src.Y	+= value.Y;
	return src;
}
TxPointD& TxPointD::operator += (double value)
{
	TxPointD&	src = *this;
	src.X	+= value;
	src.Y	+= value;
	return src;
}

// ============================================================
TxPointD TxPointD::operator - (const TxPointD& value) const
{
	const TxPointD&	src = *this;
	TxPointD		ans;
	ans.X	= src.X - value.X;
	ans.Y	= src.Y - value.Y;
	return ans;
}
TxPointD TxPointD::operator - (double value) const
{
	const TxPointD&	src = *this;
	TxPointD		ans;
	ans.X	= src.X - value;
	ans.Y	= src.Y - value;
	return ans;
}
TxPointD& TxPointD::operator -= (const TxPointD& value)
{
	TxPointD&	src = *this;
	src.X	-= value.X;
	src.Y	-= value.Y;
	return src;
}
TxPointD& TxPointD::operator -= (double value)
{
	TxPointD&	src = *this;
	src.X	-= value;
	src.Y	-= value;
	return src;
}

// ============================================================
TxPointD TxPointD::operator * (const TxPointD& value) const
{
	const TxPointD&	src = *this;
	TxPointD		ans;
	ans.X	= src.X * value.X;
	ans.Y	= src.Y * value.Y;
	return ans;
}
TxPointD TxPointD::operator * (double value) const
{
	const TxPointD&	src = *this;
	TxPointD		ans;
	ans.X	= src.X * value;
	ans.Y	= src.Y * value;
	return ans;
}
TxPointD& TxPointD::operator *= (const TxPointD& value)
{
	TxPointD&	src = *this;
	src.X	*= value.X;
	src.Y	*= value.Y;
	return src;
}
TxPointD& TxPointD::operator *= (double value)
{
	TxPointD&	src = *this;
	src.X	*= value;
	src.Y	*= value;
	return src;
}

// ============================================================
TxPointD TxPointD::operator / (const TxPointD& value) const
{
	const TxPointD&	src = *this;
	TxPointD		ans;
	ans.X	= src.X / value.X;
	ans.Y	= src.Y / value.Y;
	return ans;
}
TxPointD TxPointD::operator / (double value) const
{
	const TxPointD&	src = *this;
	TxPointD		ans;
	ans.X	= src.X / value;
	ans.Y	= src.Y / value;
	return ans;
}
TxPointD& TxPointD::operator /= (const TxPointD& value)
{
	TxPointD&	src = *this;
	src.X	/= value.X;
	src.Y	/= value.Y;
	return src;
}
TxPointD& TxPointD::operator /= (double value)
{
	TxPointD&	src = *this;
	src.X	/= value;
	src.Y	/= value;
	return src;
}

}
