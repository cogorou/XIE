/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxCircleI.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxCircleI::TxCircleI()
{
	X = 0;
	Y = 0;
	Radius = 0;
}

// ============================================================
TxCircleI::TxCircleI(int x, int y, int radius)
{
	X = x;
	Y = y;
	Radius = radius;
}

// ============================================================
TxCircleI::TxCircleI(TxPointI center, int radius)
{
	X = center.X;
	Y = center.Y;
	Radius = radius;
}

// ============================================================
bool TxCircleI::operator == (const TxCircleI& cmp) const
{
	const TxCircleI& src = *this;
	if (src.X		!= cmp.X) return false;
	if (src.Y		!= cmp.Y) return false;
	if (src.Radius	!= cmp.Radius) return false;
	return true;
}

// ============================================================
bool TxCircleI::operator != (const TxCircleI& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxCircleI::operator TxCircleD() const
{
	const TxCircleI&	src = *this;
	TxCircleD			dst;
	dst.X		= src.X;
	dst.Y		= src.Y;
	dst.Radius	= src.Radius;
	return dst;
}

// ============================================================
TxPointI TxCircleI::Center() const
{
	return TxPointI(X, Y);
}

// ============================================================
void TxCircleI::Center(const TxPointI& value)
{
	X = value.X;
	Y = value.Y;
}

// ============================================================
TxEllipseI TxCircleI::ToEllipse() const
{
	return TxEllipseI(X, Y, Radius, Radius);
}

}
