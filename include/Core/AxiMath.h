/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _AXIMATH_H_INCLUDED_
#define _AXIMATH_H_INCLUDED_

#include "xie_core.h"
#include <math.h>

namespace xie
{
namespace Axi
{

// ======================================================================
static inline double DegToRad(double degree)
{
	return degree * XIE_PI / 180;
}

// ======================================================================
static inline double RadToDeg(double radian)
{
	return radian * 180 / XIE_PI;
}

// ======================================================================
template<class T> static inline T Min(T left, T right)
{
	return (left < right) ? left : right;
}

// ======================================================================
template<class T> static inline T Max(T left, T right)
{
	return (left > right) ? left : right;
}

// ======================================================================
template<class TD, class TS, class TA, class TF> static inline TD Rotate( const TS& src, const TA& axis, TF degree )
{
	TD	dst;
	TF	R =  (TF)(degree * XIE_PI / 180);
	TF	dx = src.X - axis.X;
	TF	dy = src.Y - axis.Y;
	dst.X = axis.X + (dx * cos(R)) - (dy * sin(R));
	dst.Y = axis.Y + (dx * sin(R)) + (dy * cos(R));
	return dst;
}

// ======================================================================
template<class TD, class TS, class TF> static inline TD Saturate(TS value, TF lower, TF upper)
{
	if (value < lower) return (TD)lower;
	if (value > upper) return (TD)upper;
	return (TD)value;
}

// ======================================================================
template<class TS> static inline double RgbToGray(TS red, TS green, TS blue)
{
	return (red * 0.299 + green * 0.587 + blue * 0.114);
}

// ======================================================================
template<class TS> static inline double RgbToGray(TS value)
{
	return (value.R * 0.299 + value.G * 0.587 + value.B * 0.114);
}

// ============================================================
/*
	http://ja.wikipedia.org/wiki/YUV

	YUV (PAL, SECAM)
		R = Y + 1.13983 Å~ V
		G = Y - 0.39465 Å~ U - 0.58060 Å~ V
		B = Y + 2.03211 Å~ U

	ITU-R BT.601 / ITU-R BT.709 (1250/50/2:1)
		R = Y + 1.402 Å~ Cr
		G = Y - 0.344136 Å~ Cb - 0.714136 Å~ Cr
		B = Y + 1.772 Å~ Cb
		
	ITU-R BT.709 (1125/60/2:1)
		R = Y + 1.5748 Å~ Cr
		G = Y - 0.187324 Å~ Cb - 0.468124 Å~ Cr
		B = Y + 1.8556 Å~ Cb
*/
template<class TD, class TS> static inline TD YuvToRgb(TS y, TS u, TS v)
{
	double R = y + (1.402000 * (v - 128));
	double G = y - (0.344136 * (u - 128)) - (0.714136 * (v - 128));
	double B = y + (1.772000 * (u - 128));

	R = (R < 0) ? R - 0.5 : R + 0.5;
	G = (G < 0) ? G - 0.5 : G + 0.5;
	B = (B < 0) ? B - 0.5 : B + 0.5;
	
	if (R < 0)
		R = 0;
	if (R > 255)
		R = 255;
	
	if (G < 0)
		G = 0;
	if (G > 255)
		G = 255;
	
	if (B < 0)
		B = 0;
	if (B > 255)
		B = 255;

	return TD(
			(unsigned char)(R),
			(unsigned char)(G),
			(unsigned char)(B)
		);
}

}
}	// xie

#endif
