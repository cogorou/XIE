/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxEllipseI.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxEllipseI::TxEllipseI()
{
	X = 0;
	Y = 0;
	RadiusX = 0;
	RadiusY = 0;
}

// ============================================================
TxEllipseI::TxEllipseI(int x, int y, int radius_x, int radius_y)
{
	X = x;
	Y = y;
	RadiusX = radius_x;
	RadiusY = radius_y;
}

// ============================================================
TxEllipseI::TxEllipseI(TxPointI center, int radius_x, int radius_y)
{
	X = center.X;
	Y = center.Y;
	RadiusX = radius_x;
	RadiusY = radius_y;
}

// ============================================================
bool TxEllipseI::operator == (const TxEllipseI& cmp) const
{
	const TxEllipseI& src = *this;
	if (src.X		!= cmp.X) return false;
	if (src.Y		!= cmp.Y) return false;
	if (src.RadiusX	!= cmp.RadiusX) return false;
	if (src.RadiusY	!= cmp.RadiusY) return false;
	return true;
}

// ============================================================
bool TxEllipseI::operator != (const TxEllipseI& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxEllipseI::operator TxEllipseD() const
{
	return TxEllipseD(X, Y, RadiusX, RadiusY);
}

// ============================================================
TxPointI TxEllipseI::Center() const
{
	return TxPointI(X, Y);
}

// ============================================================
void TxEllipseI::Center(const TxPointI& value)
{
	X = value.X;
	Y = value.Y;
}

}
