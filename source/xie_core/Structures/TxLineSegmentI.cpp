/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxLineSegmentI.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/TxLineI.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxLineSegmentI::TxLineSegmentI()
{
	X1 = 0;
	Y1 = 0;
	X2 = 0;
	Y2 = 0;
}

// ============================================================
TxLineSegmentI::TxLineSegmentI(int x1, int y1, int x2, int y2)
{
	X1 = x1;
	Y1 = y1;
	X2 = x2;
	Y2 = y2;
}

// ============================================================
TxLineSegmentI::TxLineSegmentI(TxPointI st, TxPointI ed)
{
	X1 = st.X;
	Y1 = st.Y;
	X2 = ed.X;
	Y2 = ed.Y;
}

// ============================================================
bool TxLineSegmentI::operator == (const TxLineSegmentI& cmp) const
{
	const TxLineSegmentI& src = *this;
	if (src.X1	!= cmp.X1) return false;
	if (src.Y1	!= cmp.Y1) return false;
	if (src.X2	!= cmp.X2) return false;
	if (src.Y2	!= cmp.Y2) return false;
	return true;
}

// ============================================================
bool TxLineSegmentI::operator != (const TxLineSegmentI& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxLineSegmentI::operator TxLineSegmentD() const
{
	return TxLineSegmentD(X1, Y1, X2, Y2);
}

// ============================================================
TxPointI TxLineSegmentI::Point1() const
{
	return TxPointI(X1, Y1);
}

// ============================================================
void TxLineSegmentI::Point1(const TxPointI& value)
{
	X1 = value.X;
	Y1 = value.Y;
}

// ============================================================
TxPointI TxLineSegmentI::Point2() const
{
	return TxPointI(X2, Y2);
}

// ============================================================
void TxLineSegmentI::Point2(const TxPointI& value)
{
	X2 = value.X;
	Y2 = value.Y;
}

// ============================================================
TxLineI TxLineSegmentI::ToLine() const
{
	int da = Y1 - Y2;
	int db = X2 - X1;
	int dc = X1 * Y2 - X2 * Y1;
	
	if ((da == 0) && (db == 0))
		throw CxException(ExStatus::Impossible, __FUNCTION__, __FILE__, __LINE__);

	int denom = (abs(da) > abs(db)) ? da : db;
	
	TxLineI ans;
	ans.A = (int)round((double)da / denom);
	ans.B = (int)round((double)db / denom);
	ans.C = (int)round((double)dc / denom);
	return	ans;
}

}
