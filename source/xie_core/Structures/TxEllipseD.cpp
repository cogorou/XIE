/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxEllipseD.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxEllipseD::TxEllipseD()
{
	X = 0;
	Y = 0;
	RadiusX = 0;
	RadiusY = 0;
}

// ============================================================
TxEllipseD::TxEllipseD(double x, double y, double radius_x, double radius_y)
{
	X = x;
	Y = y;
	RadiusX = radius_x;
	RadiusY = radius_y;
}

// ============================================================
TxEllipseD::TxEllipseD(TxPointD center, double radius_x, double radius_y)
{
	X = center.X;
	Y = center.Y;
	RadiusX = radius_x;
	RadiusY = radius_y;
}

// ============================================================
bool TxEllipseD::operator == (const TxEllipseD& cmp) const
{
	const TxEllipseD& src = *this;
	if (src.X		!= cmp.X) return false;
	if (src.Y		!= cmp.Y) return false;
	if (src.RadiusX	!= cmp.RadiusX) return false;
	if (src.RadiusY	!= cmp.RadiusY) return false;
	return true;
}

// ============================================================
bool TxEllipseD::operator != (const TxEllipseD& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxEllipseD::operator TxEllipseI() const
{
	return TxEllipseI(
		(int)round(X),
		(int)round(Y),
		(int)round(RadiusX),
		(int)round(RadiusY)
		);
}

// ============================================================
TxPointD TxEllipseD::Center() const
{
	return TxPointD(X, Y);
}

// ============================================================
void TxEllipseD::Center(const TxPointD& value)
{
	X = value.X;
	Y = value.Y;
}

}
