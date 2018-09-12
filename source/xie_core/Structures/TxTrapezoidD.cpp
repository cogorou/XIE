/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxTrapezoidD.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxTrapezoidD::TxTrapezoidD()
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
TxTrapezoidD::TxTrapezoidD(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
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
TxTrapezoidD::TxTrapezoidD(TxPointD p1, TxPointD p2, TxPointD p3, TxPointD p4)
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
bool TxTrapezoidD::operator == (const TxTrapezoidD& cmp) const
{
	const TxTrapezoidD& src = *this;
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
bool TxTrapezoidD::operator != (const TxTrapezoidD& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxPointD TxTrapezoidD::Vertex1() const
{
	return TxPointD(X1, Y1);
}

// ============================================================
void TxTrapezoidD::Vertex1(const TxPointD& value)
{
	X1 = value.X;
	Y1 = value.Y;
}

// ============================================================
TxPointD TxTrapezoidD::Vertex2() const
{
	return TxPointD(X2, Y2);
}

// ============================================================
void TxTrapezoidD::Vertex2(const TxPointD& value)
{
	X2 = value.X;
	Y2 = value.Y;
}

// ============================================================
TxPointD TxTrapezoidD::Vertex3() const
{
	return TxPointD(X3, Y3);
}

// ============================================================
void TxTrapezoidD::Vertex3(const TxPointD& value)
{
	X3 = value.X;
	Y3 = value.Y;
}

// ============================================================
TxPointD TxTrapezoidD::Vertex4() const
{
	return TxPointD(X4, Y4);
}

// ============================================================
void TxTrapezoidD::Vertex4(const TxPointD& value)
{
	X4 = value.X;
	Y4 = value.Y;
}

// ============================================================
TxTrapezoidD::operator TxTrapezoidI() const
{
	return TxTrapezoidI(
		(int)round(X1),
		(int)round(Y1),
		(int)round(X2),
		(int)round(Y2),
		(int)round(X3),
		(int)round(Y3),
		(int)round(X4),
		(int)round(Y4)
		);
}

}
