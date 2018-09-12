/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxLineSegmentD.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/TxLineD.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxLineSegmentD::TxLineSegmentD()
{
	X1 = 0;
	Y1 = 0;
	X2 = 0;
	Y2 = 0;
}

// ============================================================
TxLineSegmentD::TxLineSegmentD(double x1, double y1, double x2, double y2)
{
	X1 = x1;
	Y1 = y1;
	X2 = x2;
	Y2 = y2;
}

// ============================================================
TxLineSegmentD::TxLineSegmentD(TxPointD st, TxPointD ed)
{
	X1 = st.X;
	Y1 = st.Y;
	X2 = ed.X;
	Y2 = ed.Y;
}

// ============================================================
bool TxLineSegmentD::operator == (const TxLineSegmentD& cmp) const
{
	const TxLineSegmentD& src = *this;
	if (src.X1	!= cmp.X1) return false;
	if (src.Y1	!= cmp.Y1) return false;
	if (src.X2	!= cmp.X2) return false;
	if (src.Y2	!= cmp.Y2) return false;
	return true;
}

// ============================================================
bool TxLineSegmentD::operator != (const TxLineSegmentD& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxLineSegmentD::operator TxLineSegmentI() const
{
	return TxLineSegmentI(
		(int)round(X1),
		(int)round(Y1),
		(int)round(X2),
		(int)round(Y2)
		);
}

// ============================================================
TxPointD TxLineSegmentD::Point1() const
{
	return TxPointD(X1, Y1);
}

// ============================================================
void TxLineSegmentD::Point1(const TxPointD& value)
{
	X1 = value.X;
	Y1 = value.Y;
}

// ============================================================
TxPointD TxLineSegmentD::Point2() const
{
	return TxPointD(X2, Y2);
}

// ============================================================
void TxLineSegmentD::Point2(const TxPointD& value)
{
	X2 = value.X;
	Y2 = value.Y;
}

// ============================================================
TxLineD TxLineSegmentD::ToLine() const
{
	double da = Y1 - Y2;
	double db = X2 - X1;
	double dc = X1 * Y2 - X2 * Y1;
	
	if ((da == 0) && (db == 0))
		throw CxException(ExStatus::Impossible, __FUNCTION__, __FILE__, __LINE__);

	double denom = (fabs(da) > fabs(db)) ? da : db;
	
	TxLineD ans;
	ans.A = da / denom;
	ans.B = db / denom;
	ans.C = dc / denom;
	return	ans;
}

}
