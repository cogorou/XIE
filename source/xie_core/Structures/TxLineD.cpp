/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxLineD.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxLineD::TxLineD()
{
	A = 0;
	B = 0;
	C = 0;
}

// ============================================================
TxLineD::TxLineD(double a, double b, double c)
{
	A = a;
	B = b;
	C = c;
}

// ============================================================
bool TxLineD::operator == (const TxLineD& cmp) const
{
	const TxLineD& src = *this;
	if (src.A	!= cmp.A) return false;
	if (src.B	!= cmp.B) return false;
	if (src.C	!= cmp.C) return false;
	return true;
}

// ============================================================
bool TxLineD::operator != (const TxLineD& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxLineD::operator TxLineI() const
{
	return TxLineI(
		(int)round(A),
		(int)round(B),
		(int)round(C)
		);
}

// ============================================================
TxLineSegmentD TxLineD::ToLineSegment(const TxRectangleD& region) const
{
	return ToLineSegment(region.X, region.Y, region.X+region.Width, region.Y+region.Height);
}

// ============================================================
TxLineSegmentD TxLineD::ToLineSegment(const TxPointD& st, const TxPointD& ed) const
{
	return ToLineSegment(st.X, st.Y, ed.X, ed.Y);
}

// ============================================================
TxLineSegmentD TxLineD::ToLineSegment(double x1, double y1, double x2, double y2) const
{
	TxLineSegmentD ans;
	const TxLineD& src = *this;
	if (src.A == 0 && src.B == 0)
		throw CxException(ExStatus::Impossible, __FUNCTION__, __FILE__, __LINE__);

	if (x1 > x2)
	{
		double tmp = x1;
		x1 = x2;
		x2 = tmp;
	}
	if (y1 > y2)
	{
		double tmp = y1;
		y1 = y2;
		y2 = tmp;
	}

	TxLineD sideL = TxLineSegmentD(x1,y1,x1,y2).ToLine();
	TxLineD sideT = TxLineSegmentD(x1,y1,x2,y1).ToLine();
	TxLineD sideR = TxLineSegmentD(x2,y1,x2,y2).ToLine();
	TxLineD sideB = TxLineSegmentD(x1,y2,x2,y2).ToLine();

	double  denomL = src.A * sideL.B - src.B * sideL.A;
	double  denomT = src.A * sideT.B - src.B * sideT.A;
	double  denomR = src.A * sideR.B - src.B * sideR.A;
	double  denomB = src.A * sideB.B - src.B * sideB.A;

	TxPointD crossL(x1, y1);
	TxPointD crossT(x1, y1);
	TxPointD crossR(x2, y2);
	TxPointD crossB(x2, y2);

	// Left
	if (fabs(denomL) > XIE_EPSd)
	{
		crossL.X = (src.B * sideL.C - src.C * sideL.B) / denomL;
		crossL.Y = (src.C * sideL.A - src.A * sideL.C) / denomL;
	}

	// Top
	if (fabs(denomT) > XIE_EPSd)
	{
		crossT.X = (src.B * sideT.C - src.C * sideT.B) / denomT;
		crossT.Y = (src.C * sideT.A - src.A * sideT.C) / denomT;
	}

	// Right
	if (fabs(denomR) > XIE_EPSd)
	{
		crossR.X = (src.B * sideR.C - src.C * sideR.B) / denomR;
		crossR.Y = (src.C * sideR.A - src.A * sideR.C) / denomR;
	}

	// Bottom
	if (fabs(denomB) > XIE_EPSd)
	{
		crossB.X = (src.B * sideB.C - src.C * sideB.B) / denomB;
		crossB.Y = (src.C * sideB.A - src.A * sideB.C) / denomB;
	}

	// ------------------------------

	// when horizontal line
	if (src.A == 0 && src.B != 0)
	{
		ans.X1 = crossL.X;
		ans.Y1 = crossL.Y;
		ans.X2 = crossR.X;
		ans.Y2 = crossR.Y;
		return ans;
	}

	// when vertical line
	if (src.A != 0 && src.B == 0)
	{
		ans.X1 = crossT.X;
		ans.Y1 = crossT.Y;
		ans.X2 = crossB.X;
		ans.Y2 = crossB.Y;
		return ans;
	}

	// ------------------------------

	TxPointD cp[] = {crossL, crossT, crossR, crossB};
	int rank[] = {0, 0, 0, 0};

	for(int i=0 ; i<3 ; i++)
	{
		for(int j=i+1 ; j<4 ; j++)
		{
			if (cp[i].X <= cp[j].X)
				rank[j]++;
			else
				rank[i]++;
		}
	}

	for(int i=0 ; i<4 ; i++)
	{
		if (rank[i] == 1)
		{
			ans.X1 = cp[i].X;
			ans.Y1 = cp[i].Y;
		}
		if (rank[i] == 2)
		{
			ans.X2 = cp[i].X;
			ans.Y2 = cp[i].Y;
		}
	}

	return ans;
}

}
