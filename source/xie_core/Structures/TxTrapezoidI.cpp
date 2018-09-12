/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxTrapezoidI.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxTrapezoidI::TxTrapezoidI()
{
	X1 = 0;
	Y1 = 0;
	X2 = 0;
	Y2 = 0;
	X3 = 0;
	Y3 = 0;
	X4 = 0;
	Y4 = 0;
}

// ============================================================
TxTrapezoidI::TxTrapezoidI(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
{
	X1 = x1;
	Y1 = y1;
	X2 = x2;
	Y2 = y2;
	X3 = x3;
	Y3 = y3;
	X4 = x4;
	Y4 = y4;
}

// ============================================================
TxTrapezoidI::TxTrapezoidI(TxPointI p1, TxPointI p2, TxPointI p3, TxPointI p4)
{
	X1 = p1.X;
	Y1 = p1.Y;
	X2 = p2.X;
	Y2 = p2.Y;
	X3 = p3.X;
	Y3 = p3.Y;
	X4 = p4.X;
	Y4 = p4.Y;
}

// ============================================================
bool TxTrapezoidI::operator == (const TxTrapezoidI& cmp) const
{
	const TxTrapezoidI& src = *this;
	if (src.X1	!= cmp.X1) return false;
	if (src.Y1	!= cmp.Y1) return false;
	if (src.X2	!= cmp.X2) return false;
	if (src.Y2	!= cmp.Y2) return false;
	if (src.X3	!= cmp.X3) return false;
	if (src.Y3	!= cmp.Y3) return false;
	if (src.X4	!= cmp.X4) return false;
	if (src.Y4	!= cmp.Y4) return false;
	return true;
}

// ============================================================
bool TxTrapezoidI::operator != (const TxTrapezoidI& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxPointI TxTrapezoidI::Vertex1() const
{
	return TxPointI(X1, Y1);
}

// ============================================================
void TxTrapezoidI::Vertex1(const TxPointI& value)
{
	X1 = value.X;
	Y1 = value.Y;
}

// ============================================================
TxPointI TxTrapezoidI::Vertex2() const
{
	return TxPointI(X2, Y2);
}

// ============================================================
void TxTrapezoidI::Vertex2(const TxPointI& value)
{
	X2 = value.X;
	Y2 = value.Y;
}

// ============================================================
TxPointI TxTrapezoidI::Vertex3() const
{
	return TxPointI(X3, Y3);
}

// ============================================================
void TxTrapezoidI::Vertex3(const TxPointI& value)
{
	X3 = value.X;
	Y3 = value.Y;
}

// ============================================================
TxPointI TxTrapezoidI::Vertex4() const
{
	return TxPointI(X4, Y4);
}

// ============================================================
void TxTrapezoidI::Vertex4(const TxPointI& value)
{
	X4 = value.X;
	Y4 = value.Y;
}

// ============================================================
TxTrapezoidI::operator TxTrapezoidD() const
{
	return TxTrapezoidD(X1, Y1, X2, Y2, X3, Y3, X4, Y4);
}

}
