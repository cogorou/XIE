/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxEllipseArcD.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxEllipseArcD::TxEllipseArcD()
{
	X = 0;
	Y = 0;
	RadiusX = 0;
	RadiusY = 0;
	StartAngle = 0;
	SweepAngle = 0;
}

// ============================================================
TxEllipseArcD::TxEllipseArcD(double x, double y, double radius_x, double radius_y, double start_angle, double sweep_angle)
{
	X = x;
	Y = y;
	RadiusX = radius_x;
	RadiusY = radius_y;
	StartAngle = start_angle;
	SweepAngle = sweep_angle;
}

// ============================================================
TxEllipseArcD::TxEllipseArcD(TxPointD center, double radius_x, double radius_y, double start_angle, double sweep_angle)
{
	X = center.X;
	Y = center.Y;
	RadiusX = radius_x;
	RadiusY = radius_y;
	StartAngle = start_angle;
	SweepAngle = sweep_angle;
}

// ============================================================
bool TxEllipseArcD::operator == (const TxEllipseArcD& cmp) const
{
	const TxEllipseArcD& src = *this;
	if (src.X		!= cmp.X) return false;
	if (src.Y		!= cmp.Y) return false;
	if (src.RadiusX	!= cmp.RadiusX) return false;
	if (src.RadiusY	!= cmp.RadiusY) return false;
	if (src.StartAngle	!= cmp.StartAngle) return false;
	if (src.SweepAngle	!= cmp.SweepAngle) return false;
	return true;
}

// ============================================================
bool TxEllipseArcD::operator != (const TxEllipseArcD& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxEllipseArcD::operator TxEllipseArcI() const
{
	return TxEllipseArcI(
		(int)round(X),
		(int)round(Y),
		(int)round(RadiusX),
		(int)round(RadiusY),
		(int)round(StartAngle),
		(int)round(SweepAngle)
		);
}

// ============================================================
TxPointD TxEllipseArcD::Center() const
{
	return TxPointD(X, Y);
}

// ============================================================
void TxEllipseArcD::Center(const TxPointD& value)
{
	X = value.X;
	Y = value.Y;
}

}
