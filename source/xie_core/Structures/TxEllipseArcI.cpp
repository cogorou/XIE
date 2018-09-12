/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxEllipseArcI.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxEllipseArcI::TxEllipseArcI()
{
	X = 0;
	Y = 0;
	RadiusX = 0;
	RadiusY = 0;
	StartAngle = 0;
	SweepAngle = 0;
}

// ============================================================
TxEllipseArcI::TxEllipseArcI(int x, int y, int radius_x, int radius_y, int start_angle, int sweep_angle)
{
	X = x;
	Y = y;
	RadiusX = radius_x;
	RadiusY = radius_y;
	StartAngle = start_angle;
	SweepAngle = sweep_angle;
}

// ============================================================
TxEllipseArcI::TxEllipseArcI(TxPointI center, int radius_x, int radius_y, int start_angle, int sweep_angle)
{
	X = center.X;
	Y = center.Y;
	RadiusX = radius_x;
	RadiusY = radius_y;
	StartAngle = start_angle;
	SweepAngle = sweep_angle;
}

// ============================================================
bool TxEllipseArcI::operator == (const TxEllipseArcI& cmp) const
{
	const TxEllipseArcI& src = *this;
	if (src.X		!= cmp.X) return false;
	if (src.Y		!= cmp.Y) return false;
	if (src.RadiusX	!= cmp.RadiusX) return false;
	if (src.RadiusY	!= cmp.RadiusY) return false;
	if (src.StartAngle	!= cmp.StartAngle) return false;
	if (src.SweepAngle	!= cmp.SweepAngle) return false;
	return true;
}

// ============================================================
bool TxEllipseArcI::operator != (const TxEllipseArcI& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxEllipseArcI::operator TxEllipseArcD() const
{
	return TxEllipseArcD(X, Y, RadiusX, RadiusY, StartAngle, SweepAngle);
}

// ============================================================
TxPointI TxEllipseArcI::Center() const
{
	return TxPointI(X, Y);
}

// ============================================================
void TxEllipseArcI::Center(const TxPointI& value)
{
	X = value.X;
	Y = value.Y;
}

}
