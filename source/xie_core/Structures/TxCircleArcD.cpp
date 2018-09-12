/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxCircleArcD.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxCircleArcD::TxCircleArcD()
{
	X = 0;
	Y = 0;
	Radius = 0;
	StartAngle = 0;
	SweepAngle = 0;
}

// ============================================================
TxCircleArcD::TxCircleArcD(double x, double y, double radius, double start_angle, double sweep_angle)
{
	X = x;
	Y = y;
	Radius = radius;
	StartAngle = start_angle;
	SweepAngle = sweep_angle;
}

// ============================================================
TxCircleArcD::TxCircleArcD(TxPointD center, double radius, double start_angle, double sweep_angle)
{
	X = center.X;
	Y = center.Y;
	Radius = radius;
	StartAngle = start_angle;
	SweepAngle = sweep_angle;
}

// ============================================================
bool TxCircleArcD::operator == (const TxCircleArcD& cmp) const
{
	const TxCircleArcD& src = *this;
	if (src.X		!= cmp.X) return false;
	if (src.Y		!= cmp.Y) return false;
	if (src.Radius	!= cmp.Radius) return false;
	if (src.StartAngle	!= cmp.StartAngle) return false;
	if (src.SweepAngle	!= cmp.SweepAngle) return false;
	return true;
}

// ============================================================
bool TxCircleArcD::operator != (const TxCircleArcD& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxCircleArcD::operator TxCircleArcI() const
{
	return TxCircleArcI(
		(int)round(X),
		(int)round(Y),
		(int)round(Radius),
		(int)round(StartAngle),
		(int)round(SweepAngle)
		);
}

// ============================================================
TxPointD TxCircleArcD::Center() const
{
	return TxPointD(X, Y);
}

// ============================================================
void TxCircleArcD::Center(const TxPointD& value)
{
	X = value.X;
	Y = value.Y;
}

}
