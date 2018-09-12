/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxStatistics.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxStatistics::TxStatistics()
{
	Count = 0;
	Max = -1.7976931348623158e+308;
	Min = +1.7976931348623158e+308;
	Sum1 = 0;
	Sum2 = 0;
}

// ============================================================
TxStatistics::TxStatistics(double count, double maxval, double minval, double sum1, double sum2)
{
	Count = count;
	Max = maxval;
	Min = minval;
	Sum1 = sum1;
	Sum2 = sum2;
}

// ===================================================================
void TxStatistics::Reset()
{
	Count = 0;
	Max = -1.7976931348623158e+308;
	Min = +1.7976931348623158e+308;
	Sum1 = 0;
	Sum2 = 0;
}

// ============================================================
bool TxStatistics::operator == (const TxStatistics& cmp) const
{
	const TxStatistics& src = *this;
	if (src.Count	!= cmp.Count) return false;
	if (src.Max		!= cmp.Max) return false;
	if (src.Min		!= cmp.Min) return false;
	if (src.Sum1	!= cmp.Sum1) return false;
	if (src.Sum2	!= cmp.Sum2) return false;
	return true;
}

// ============================================================
bool TxStatistics::operator != (const TxStatistics& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxStatistics TxStatistics::operator + (const TxStatistics& value) const
{
	const TxStatistics& src = *this;
	TxStatistics		ans;
	ans.Count	= src.Count + value.Count;
	ans.Sum1	= src.Sum1 + value.Sum1;
	ans.Sum2	= src.Sum2 + value.Sum2;
	ans.Min		= xie::Axi::Min(src.Min, value.Min);
	ans.Max		= xie::Axi::Max(src.Max, value.Max);
	return ans;
}
TxStatistics TxStatistics::operator + (double value) const
{
	const TxStatistics&	src = *this;
	TxStatistics		ans;
	ans.Count	= src.Count + 1;
	ans.Sum1	= src.Sum1 + value;
	ans.Sum2	= src.Sum2 + value * value;
	ans.Min		= xie::Axi::Min(src.Min, value);
	ans.Max		= xie::Axi::Max(src.Max, value);
	return ans;
}
TxStatistics TxStatistics::operator + (int value) const
{
	const TxStatistics&	src = *this;
	TxStatistics		ans;
	ans.Count	= src.Count + 1;
	ans.Sum1	= src.Sum1 + value;
	ans.Sum2	= src.Sum2 + value * value;
	ans.Min		= xie::Axi::Min(src.Min, (double)value);
	ans.Max		= xie::Axi::Max(src.Max, (double)value);
	return ans;
}
TxStatistics& TxStatistics::operator += (const TxStatistics& value)
{
	TxStatistics&	src = *this;
	src.Count += value.Count;
	src.Sum1 += value.Sum1;
	src.Sum2 += value.Sum2;
	src.Min = xie::Axi::Min(src.Min, value.Min);
	src.Max = xie::Axi::Max(src.Max, value.Max);
	return src;
}
TxStatistics& TxStatistics::operator += (double value)
{
	TxStatistics&	src = *this;
	src.Count ++;
	src.Sum1 += value;
	src.Sum2 += value * value;
	src.Min = xie::Axi::Min(src.Min, value);
	src.Max = xie::Axi::Max(src.Max, value);
	return src;
}
TxStatistics& TxStatistics::operator += (int value)
{
	TxStatistics&	src = *this;
	src.Count ++;
	src.Sum1 += value;
	src.Sum2 += value * value;
	src.Min = xie::Axi::Min(src.Min, (double)value);
	src.Max = xie::Axi::Max(src.Max, (double)value);
	return src;
}

// ===================================================================
double TxStatistics::Mean()
{
	return (Count <= XIE_EPSd) ? 0 : Sum1 / Count;
}

// ===================================================================
double TxStatistics::Sigma()
{
	return (Count <= XIE_EPSd) ? 0 : sqrt(Variance());
}

// ===================================================================
double TxStatistics::Variance()
{
	if (Count <= XIE_EPSd)
		return 0;
	else
	{
		double mean = Mean();
		return (Sum2 / Count) - (mean * mean);
	}
}

}
