/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxRangeD.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxRangeD::TxRangeD()
{
	Lower = 0;
	Upper = 0;
}

// ============================================================
TxRangeD::TxRangeD(double lower, double upper)
{
	Lower = lower;
	Upper = upper;
}

// ============================================================
bool TxRangeD::operator == (const TxRangeD& cmp) const
{
	const TxRangeD& src = *this;
	if (src.Lower	!= cmp.Lower) return false;
	if (src.Upper	!= cmp.Upper) return false;
	return true;
}

// ============================================================
bool TxRangeD::operator != (const TxRangeD& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxRangeD::operator TxRangeI() const
{
	return TxRangeI(
		(int)round(Lower),
		(int)round(Upper)
		);
}

// ============================================================
TxRangeD TxRangeD::operator + (const TxRangeD& value) const
{
	const TxRangeD&	src = *this;
	TxRangeD			ans;
	ans.Lower	= src.Lower + value.Lower;
	ans.Upper	= src.Upper + value.Upper;
	return ans;
}
TxRangeD TxRangeD::operator + (double value) const
{
	const TxRangeD&	src = *this;
	TxRangeD			ans;
	ans.Lower	= src.Lower + value;
	ans.Upper	= src.Upper + value;
	return ans;
}
TxRangeD& TxRangeD::operator += (const TxRangeD& value)
{
	TxRangeD&	src = *this;
	src.Lower	+= value.Lower;
	src.Upper	+= value.Upper;
	return src;
}
TxRangeD& TxRangeD::operator += (double value)
{
	TxRangeD&	src = *this;
	src.Lower	+= value;
	src.Upper	+= value;
	return src;
}

// ============================================================
TxRangeD TxRangeD::operator - (const TxRangeD& value) const
{
	const TxRangeD&	src = *this;
	TxRangeD			ans;
	ans.Lower	= src.Lower - value.Lower;
	ans.Upper	= src.Upper - value.Upper;
	return ans;
}
TxRangeD TxRangeD::operator - (double value) const
{
	const TxRangeD&	src = *this;
	TxRangeD			ans;
	ans.Lower	= src.Lower - value;
	ans.Upper	= src.Upper - value;
	return ans;
}
TxRangeD& TxRangeD::operator -= (const TxRangeD& value)
{
	TxRangeD&	src = *this;
	src.Lower	-= value.Lower;
	src.Upper	-= value.Upper;
	return src;
}
TxRangeD& TxRangeD::operator -= (double value)
{
	TxRangeD&	src = *this;
	src.Lower	-= value;
	src.Upper	-= value;
	return src;
}

// ============================================================
TxRangeD TxRangeD::operator * (const TxRangeD& value) const
{
	const TxRangeD&	src = *this;
	TxRangeD			ans;
	ans.Lower	= src.Lower * value.Lower;
	ans.Upper	= src.Upper * value.Upper;
	return ans;
}
TxRangeD TxRangeD::operator * (double value) const
{
	const TxRangeD&	src = *this;
	TxRangeD			ans;
	ans.Lower	= src.Lower * value;
	ans.Upper	= src.Upper * value;
	return ans;
}
TxRangeD& TxRangeD::operator *= (const TxRangeD& value)
{
	TxRangeD&	src = *this;
	src.Lower	*= value.Lower;
	src.Upper	*= value.Upper;
	return src;
}
TxRangeD& TxRangeD::operator *= (double value)
{
	TxRangeD&	src = *this;
	src.Lower	*= value;
	src.Upper	*= value;
	return src;
}

// ============================================================
TxRangeD TxRangeD::operator / (const TxRangeD& value) const
{
	const TxRangeD&	src = *this;
	TxRangeD			ans;
	ans.Lower	= src.Lower / value.Lower;
	ans.Upper	= src.Upper / value.Upper;
	return ans;
}
TxRangeD TxRangeD::operator / (double value) const
{
	const TxRangeD&	src = *this;
	TxRangeD			ans;
	ans.Lower	= src.Lower / value;
	ans.Upper	= src.Upper / value;
	return src;
}
TxRangeD& TxRangeD::operator /= (const TxRangeD& value)
{
	TxRangeD&	src = *this;
	src.Lower	/= value.Lower;
	src.Upper	/= value.Upper;
	return src;
}
TxRangeD& TxRangeD::operator /= (double value)
{
	TxRangeD&	src = *this;
	src.Lower	/= value;
	src.Upper	/= value;
	return src;
}

}
