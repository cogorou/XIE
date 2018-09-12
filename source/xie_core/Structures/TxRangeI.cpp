/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxRangeI.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxRangeI::TxRangeI()
{
	Lower = 0;
	Upper = 0;
}

// ============================================================
TxRangeI::TxRangeI(int lower, int upper)
{
	Lower = lower;
	Upper = upper;
}

// ============================================================
bool TxRangeI::operator == (const TxRangeI& cmp) const
{
	const TxRangeI& src = *this;
	if (src.Lower	!= cmp.Lower) return false;
	if (src.Upper	!= cmp.Upper) return false;
	return true;
}

// ============================================================
bool TxRangeI::operator != (const TxRangeI& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxRangeI::operator TxRangeD() const
{
	return TxRangeD(Lower, Upper);
}

// ============================================================
TxRangeI TxRangeI::operator + (const TxRangeI& value) const
{
	const TxRangeI&	src = *this;
	TxRangeI		ans;
	ans.Lower	= src.Lower + value.Lower;
	ans.Upper	= src.Upper + value.Upper;
	return ans;
}
TxRangeI TxRangeI::operator + (int value) const
{
	const TxRangeI&	src = *this;
	TxRangeI		ans;
	ans.Lower	= src.Lower + value;
	ans.Upper	= src.Upper + value;
	return ans;
}
TxRangeD TxRangeI::operator + (double value) const
{
	const TxRangeI&	src = *this;
	TxRangeD		ans;
	ans.Lower	= src.Lower + value;
	ans.Upper	= src.Upper + value;
	return ans;
}

// ============================================================
TxRangeI& TxRangeI::operator += (const TxRangeI& value)
{
	TxRangeI&	ans = *this;
	ans.Lower	= ans.Lower + value.Lower;
	ans.Upper	= ans.Upper + value.Upper;
	return ans;
}
TxRangeI& TxRangeI::operator += (int value)
{
	TxRangeI&	ans = *this;
	ans.Lower	= ans.Lower + value;
	ans.Upper	= ans.Upper + value;
	return ans;
}
TxRangeI& TxRangeI::operator += (double value)
{
	TxRangeI&	ans = *this;
	ans.Lower	= (int)round(ans.Lower + value);
	ans.Upper	= (int)round(ans.Upper + value);
	return ans;
}

// ============================================================
TxRangeI TxRangeI::operator - (const TxRangeI& value) const
{
	const TxRangeI&	src = *this;
	TxRangeI		ans;
	ans.Lower	= src.Lower - value.Lower;
	ans.Upper	= src.Upper - value.Upper;
	return ans;
}
TxRangeI TxRangeI::operator - (int value) const
{
	const TxRangeI&	src = *this;
	TxRangeI		ans;
	ans.Lower	= src.Lower - value;
	ans.Upper	= src.Upper - value;
	return ans;
}
TxRangeD TxRangeI::operator - (double value) const
{
	const TxRangeI&	src = *this;
	TxRangeD		ans;
	ans.Lower	= src.Lower - value;
	ans.Upper	= src.Upper - value;
	return ans;
}

// ============================================================
TxRangeI& TxRangeI::operator -= (const TxRangeI& value)
{
	TxRangeI&	ans = *this;
	ans.Lower	= ans.Lower - value.Lower;
	ans.Upper	= ans.Upper - value.Upper;
	return ans;
}
TxRangeI& TxRangeI::operator -= (int value)
{
	TxRangeI&	ans = *this;
	ans.Lower	= ans.Lower - value;
	ans.Upper	= ans.Upper - value;
	return ans;
}
TxRangeI& TxRangeI::operator -= (double value)
{
	TxRangeI&	ans = *this;
	ans.Lower	= (int)round(ans.Lower - value);
	ans.Upper	= (int)round(ans.Upper - value);
	return ans;
}

// ============================================================
TxRangeI TxRangeI::operator * (const TxRangeI& value) const
{
	const TxRangeI&	src = *this;
	TxRangeI		ans;
	ans.Lower	= src.Lower * value.Lower;
	ans.Upper	= src.Upper * value.Upper;
	return ans;
}
TxRangeI TxRangeI::operator * (int value) const
{
	const TxRangeI&	src = *this;
	TxRangeI		ans;
	ans.Lower	= src.Lower * value;
	ans.Upper	= src.Upper * value;
	return ans;
}
TxRangeD TxRangeI::operator * (double value) const
{
	const TxRangeI&	src = *this;
	TxRangeD		ans;
	ans.Lower	= src.Lower * value;
	ans.Upper	= src.Upper * value;
	return ans;
}

// ============================================================
TxRangeI& TxRangeI::operator *= (const TxRangeI& value)
{
	TxRangeI&	ans = *this;
	ans.Lower	= ans.Lower * value.Lower;
	ans.Upper	= ans.Upper * value.Upper;
	return ans;
}
TxRangeI& TxRangeI::operator *= (int value)
{
	TxRangeI&	ans = *this;
	ans.Lower	= ans.Lower * value;
	ans.Upper	= ans.Upper * value;
	return ans;
}
TxRangeI& TxRangeI::operator *= (double value)
{
	TxRangeI&	ans = *this;
	ans.Lower	= (int)round(ans.Lower * value);
	ans.Upper	= (int)round(ans.Upper * value);
	return ans;
}

// ============================================================
TxRangeI TxRangeI::operator / (const TxRangeI& value) const
{
	const TxRangeI&	src = *this;
	TxRangeI		ans;
	ans.Lower	= src.Lower / value.Lower;
	ans.Upper	= src.Upper / value.Upper;
	return ans;
}
TxRangeI TxRangeI::operator / (int value) const
{
	const TxRangeI&	src = *this;
	TxRangeI		ans;
	ans.Lower	= src.Lower / value;
	ans.Upper	= src.Upper / value;
	return ans;
}
TxRangeD TxRangeI::operator / (double value) const
{
	const TxRangeI&	src = *this;
	TxRangeD		ans;
	ans.Lower	= src.Lower * value;
	ans.Upper	= src.Upper * value;
	return ans;
}

// ============================================================
TxRangeI& TxRangeI::operator /= (const TxRangeI& value)
{
	TxRangeI&	ans = *this;
	ans.Lower	= ans.Lower / value.Lower;
	ans.Upper	= ans.Upper / value.Upper;
	return ans;
}
TxRangeI& TxRangeI::operator /= (int value)
{
	TxRangeI&	ans = *this;
	ans.Lower	= ans.Lower / value;
	ans.Upper	= ans.Upper / value;
	return ans;
}
TxRangeI& TxRangeI::operator /= (double value)
{
	TxRangeI&	ans = *this;
	ans.Lower	= (int)round(ans.Lower / value);
	ans.Upper	= (int)round(ans.Upper / value);
	return ans;
}

}
