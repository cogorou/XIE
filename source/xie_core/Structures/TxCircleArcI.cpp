/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxCircleArcI.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxCircleArcI::TxCircleArcI()
{
	X = 0;
	Y = 0;
	Radius = 0;
	StartAngle = 0;
	SweepAngle = 0;
}

// ============================================================
TxCircleArcI::TxCircleArcI(int x, int y, int radius, int start_angle, int sweep_angle)
{
	X = x;
	Y = y;
	Radius = radius;
	StartAngle = start_angle;
	SweepAngle = sweep_angle;
}

// ============================================================
TxCircleArcI::TxCircleArcI(TxPointI center, int radius, int start_angle, int sweep_angle)
{
	X = center.X;
	Y = center.Y;
	Radius = radius;
	StartAngle = start_angle;
	SweepAngle = sweep_angle;
}

// ============================================================
bool TxCircleArcI::operator == (const TxCircleArcI& cmp) const
{
	const TxCircleArcI& src = *this;
	if (src.X		!= cmp.X) return false;
	if (src.Y		!= cmp.Y) return false;
	if (src.Radius	!= cmp.Radius) return false;
	if (src.StartAngle	!= cmp.StartAngle) return false;
	if (src.SweepAngle	!= cmp.SweepAngle) return false;
	return true;
}

// ============================================================
bool TxCircleArcI::operator != (const TxCircleArcI& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxCircleArcI::operator TxCircleArcD() const
{
	return TxCircleArcD(X, Y, Radius, StartAngle, SweepAngle);
}

// ============================================================
TxPointI TxCircleArcI::Center() const
{
	return TxPointI(X, Y);
}

// ============================================================
void TxCircleArcI::Center(const TxPointI& value)
{
	X = value.X;
	Y = value.Y;
}

}
