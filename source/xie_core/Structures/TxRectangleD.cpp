/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxRectangleD.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxRectangleD::TxRectangleD()
{
	X = 0;
	Y = 0;
	Width = 0;
	Height = 0;
}

// ============================================================
TxRectangleD::TxRectangleD(double x, double y, double width, double height)
{
	X = x;
	Y = y;
	Width = width;
	Height = height;
}

// ============================================================
TxRectangleD::TxRectangleD(TxPointD location, TxSizeD size)
{
	X = location.X;
	Y = location.Y;
	Width = size.Width;
	Height = size.Height;
}

// ============================================================
bool TxRectangleD::operator == (const TxRectangleD& cmp) const
{
	const TxRectangleD& src = *this;
	if (src.X		!= cmp.X) return false;
	if (src.Y		!= cmp.Y) return false;
	if (src.Width	!= cmp.Width) return false;
	if (src.Height	!= cmp.Height) return false;
	return true;
}

// ============================================================
bool TxRectangleD::operator != (const TxRectangleD& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxRectangleD::operator TxRectangleI() const
{
	return TxRectangleI(
		(int)round(X),
		(int)round(Y),
		(int)round(Width),
		(int)round(Height)
		);
}

// ============================================================
TxPointD TxRectangleD::Location() const
{
	return TxPointD(X, Y);
}

// ============================================================
void TxRectangleD::Location(const TxPointD& value)
{
	X = value.X;
	Y = value.Y;
}

// ============================================================
TxSizeD TxRectangleD::Size() const
{
	return TxSizeD(Width, Height);
}

// ============================================================
void TxRectangleD::Size(const TxSizeD& value)
{
	Width = value.Width;
	Height = value.Height;
}

// ============================================================
TxTrapezoidD TxRectangleD::ToTrapezoid() const
{
	return TxTrapezoidD(
		X,
		Y,
		X + Width,
		Y,
		X + Width,
		Y + Height,
		X,
		Y + Height
		);
}

}
