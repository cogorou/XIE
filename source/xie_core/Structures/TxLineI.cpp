/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxLineI.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxLineI::TxLineI()
{
	A = 0;
	B = 0;
	C = 0;
}

// ============================================================
TxLineI::TxLineI(int a, int b, int c)
{
	A = a;
	B = b;
	C = c;
}

// ============================================================
bool TxLineI::operator == (const TxLineI& cmp) const
{
	const TxLineI& src = *this;
	if (src.A	!= cmp.A) return false;
	if (src.B	!= cmp.B) return false;
	if (src.C	!= cmp.C) return false;
	return true;
}

// ============================================================
bool TxLineI::operator != (const TxLineI& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxLineI::operator TxLineD() const
{
	return TxLineD(A, B, C);
}

// ============================================================
TxLineSegmentI TxLineI::ToLineSegment(const TxRectangleI& region) const
{
	return ToLineSegment(region.X, region.Y, region.X+region.Width, region.Y+region.Height);
}

// ============================================================
TxLineSegmentI TxLineI::ToLineSegment(const TxPointI& st, const TxPointI& ed) const
{
	return ToLineSegment(st.X, st.Y, ed.X, ed.Y);
}

// ============================================================
TxLineSegmentI TxLineI::ToLineSegment(int x1, int y1, int x2, int y2) const
{
	TxLineSegmentI ans;
	const TxLineI& src = *this;
	if (src.A == 0 && src.B == 0)
		throw CxException(ExStatus::Impossible, __FUNCTION__, __FILE__, __LINE__);

	if (x1 > x2)
	{
		int tmp = x1;
		x1 = x2;
		x2 = tmp;
	}
	if (y1 > y2)
	{
		int tmp = y1;
		y1 = y2;
		y2 = tmp;
	}

	TxLineI sideL = TxLineSegmentI(x1,y1,x1,y2).ToLine();
	TxLineI sideT = TxLineSegmentI(x1,y1,x2,y1).ToLine();
	TxLineI sideR = TxLineSegmentI(x2,y1,x2,y2).ToLine();
	TxLineI sideB = TxLineSegmentI(x1,y2,x2,y2).ToLine();

	int  denomL = src.A * sideL.B - src.B * sideL.A;
	int  denomT = src.A * sideT.B - src.B * sideT.A;
	int  denomR = src.A * sideR.B - src.B * sideR.A;
	int  denomB = src.A * sideB.B - src.B * sideB.A;

	TxPointI crossL(x1, y1);
	TxPointI crossT(x1, y1);
	TxPointI crossR(x2, y2);
	TxPointI crossB(x2, y2);

	// Left
	if (abs(denomL) > XIE_EPSd)
	{
		crossL.X = (int)round( (double)(src.B * sideL.C - src.C * sideL.B) / denomL );
		crossL.Y = (int)round( (double)(src.C * sideL.A - src.A * sideL.C) / denomL );
	}

	// Top
	if (abs(denomT) > XIE_EPSd)
	{
		crossT.X = (int)round( (double)(src.B * sideT.C - src.C * sideT.B) / denomT );
		crossT.Y = (int)round( (double)(src.C * sideT.A - src.A * sideT.C) / denomT );
	}

	// Right
	if (abs(denomR) > XIE_EPSd)
	{
		crossR.X = (int)round( (double)(src.B * sideR.C - src.C * sideR.B) / denomR );
		crossR.Y = (int)round( (double)(src.C * sideR.A - src.A * sideR.C) / denomR );
	}

	// Bottom
	if (abs(denomB) > XIE_EPSd)
	{
		crossB.X = (int)round( (double)(src.B * sideB.C - src.C * sideB.B) / denomB );
		crossB.Y = (int)round( (double)(src.C * sideB.A - src.A * sideB.C) / denomB );
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

	TxPointI cp[] = {crossL, crossT, crossR, crossB};
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
