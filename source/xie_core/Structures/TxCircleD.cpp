/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxCircleD.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxCircleD::TxCircleD()
{
	X = 0;
	Y = 0;
	Radius = 0;
}

// ============================================================
TxCircleD::TxCircleD(double x, double y, double radius)
{
	X = x;
	Y = y;
	Radius = radius;
}

// ============================================================
TxCircleD::TxCircleD(TxPointD center, double radius)
{
	X = center.X;
	Y = center.Y;
	Radius = radius;
}

// ============================================================
bool TxCircleD::operator == (const TxCircleD& cmp) const
{
	const TxCircleD& src = *this;
	if (src.X		!= cmp.X) return false;
	if (src.Y		!= cmp.Y) return false;
	if (src.Radius	!= cmp.Radius) return false;
	return true;
}

// ============================================================
bool TxCircleD::operator != (const TxCircleD& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxCircleD::operator TxCircleI() const
{
	return TxCircleI(
		(int)round(X),
		(int)round(Y),
		(int)round(Radius)
		);
}

// ============================================================
TxPointD TxCircleD::Center() const
{
	return TxPointD(X, Y);
}

// ============================================================
void TxCircleD::Center(const TxPointD& value)
{
	X = value.X;
	Y = value.Y;
}

// ============================================================
TxEllipseD TxCircleD::ToEllipse() const
{
	return TxEllipseD(X, Y, Radius, Radius);
}

}
