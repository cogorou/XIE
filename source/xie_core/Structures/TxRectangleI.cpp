/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/TxRectangleI.h"
#include "Core/Axi.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxRectangleI::TxRectangleI()
{
	X = 0;
	Y = 0;
	Width = 0;
	Height = 0;
}

// ============================================================
TxRectangleI::TxRectangleI(int x, int y, int width, int height)
{
	X = x;
	Y = y;
	Width = width;
	Height = height;
}

// ============================================================
TxRectangleI::TxRectangleI(TxPointI location, TxSizeI size)
{
	X = location.X;
	Y = location.Y;
	Width = size.Width;
	Height = size.Height;
}

// ============================================================
bool TxRectangleI::operator == (const TxRectangleI& cmp) const
{
	const TxRectangleI& src = *this;
	if (src.X		!= cmp.X) return false;
	if (src.Y		!= cmp.Y) return false;
	if (src.Width	!= cmp.Width) return false;
	if (src.Height	!= cmp.Height) return false;
	return true;
}

// ============================================================
bool TxRectangleI::operator != (const TxRectangleI& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxRectangleI::operator TxRectangleD() const
{
	return TxRectangleD(X, Y, Width, Height);
}

// ============================================================
TxPointI TxRectangleI::Location() const
{
	return TxPointI(X, Y);
}

// ============================================================
void TxRectangleI::Location(const TxPointI& value)
{
	X = value.X;
	Y = value.Y;
}

// ============================================================
TxSizeI TxRectangleI::Size() const
{
	return TxSizeI(Width, Height);
}

// ============================================================
void TxRectangleI::Size(const TxSizeI& value)
{
	Width = value.Width;
	Height = value.Height;
}

// ============================================================
TxTrapezoidI TxRectangleI::ToTrapezoid() const
{
	return TxTrapezoidI(
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
