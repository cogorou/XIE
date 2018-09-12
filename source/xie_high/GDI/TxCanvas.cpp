/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "GDI/TxCanvas.h"
#include "Core/Axi.h"
#include "Core/AxiMath.h"

namespace xie
{
namespace GDI
{

// ================================================================================
TxCanvas::TxCanvas()
{
	Width			= 0;
	Height			= 0;
	BgSize			= TxSizeI::Default();
	BkColor			= TxRGB8x4::Default();
	BkEnable		= ExBoolean::False;
	Magnification	= 1.0;
	ViewPoint		= TxPointD::Default();
	ChannelNo		= 0;
	Unpack			= ExBoolean::False;
	Halftone		= ExBoolean::False;
}

// ================================================================================
TxCanvas::TxCanvas(int width, int height, const TxSizeI& bg_size, double mag, const TxPointD& view_point)
{
	Width			= width;
	Height			= height;
	BgSize			= bg_size;
	Magnification	= mag;
	ViewPoint		= view_point;
	ChannelNo		= 0;
	Unpack			= ExBoolean::False;
	Halftone		= ExBoolean::False;
}

// ================================================================================
TxCanvas::TxCanvas(int width, int height, const TxSizeI& bg_size, double mag, const TxPointD& view_point, int ch, ExBoolean unpack, ExBoolean halftone)
{
	Width			= width;
	Height			= height;
	BgSize			= bg_size;
	Magnification	= mag;
	ViewPoint		= view_point;
	ChannelNo		= ch;
	Unpack			= unpack;
	Halftone		= halftone;
}

// ================================================================================
bool TxCanvas::operator == (const TxCanvas& cmp) const
{
	const TxCanvas&	src = *this;
	if (src.Width			!= cmp.Width) return false;
	if (src.Height			!= cmp.Height) return false;
	if (src.BgSize			!= cmp.BgSize) return false;
	if (src.Magnification	!= cmp.Magnification) return false;
	if (src.ViewPoint		!= cmp.ViewPoint) return false;
	if (src.ChannelNo		!= cmp.ChannelNo) return false;
	if (src.Unpack			!= cmp.Unpack) return false;
	if (src.Halftone		!= cmp.Halftone) return false;
	return true; 
}

// ================================================================================
bool TxCanvas::operator != (const TxCanvas& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxSizeI TxCanvas::DisplaySize() const
{
	return TxSizeI(Width, Height);
}

// ============================================================
TxRectangleI TxCanvas::DisplayRect() const
{
	return TxRectangleI(0, 0, Width, Height);
}

// ============================================================
TxRectangleI TxCanvas::EffectiveRect() const
{
	return EffectiveRect(DisplayRect(), BgSize, Magnification);
}

// ============================================================
TxRectangleD TxCanvas::VisibleRect() const
{
	return VisibleRect(DisplayRect(), BgSize, Magnification, ViewPoint);
}

// ============================================================
TxRectangleI TxCanvas::VisibleRectI(bool includePartialPixel) const
{
	return VisibleRectI(DisplayRect(), BgSize, Magnification, ViewPoint, includePartialPixel);
}

// ============================================================
TxPointD TxCanvas::DPtoIP(const TxPointD& dp, ExGdiScalingMode mode) const
{
	return DPtoIP(DisplayRect(), BgSize, Magnification, ViewPoint, dp, mode);
}

// ============================================================
TxPointD TxCanvas::IPtoDP(const TxPointD& ip, ExGdiScalingMode mode) const
{
	return IPtoDP(DisplayRect(), BgSize, Magnification, ViewPoint, ip, mode);
}

// ======================================================================
TxRectangleI TxCanvas::EffectiveRect(TxRectangleI display_rect, TxSizeI bg_size, double mag)
{
	if (mag <= 0)
		return TxRectangleI();

	auto	result = TxRectangleI(display_rect.X, display_rect.Y, display_rect.Width, display_rect.Height);

	int	xoi = (int)(bg_size.Width * mag);		// X on image
	int	yoi = (int)(bg_size.Height * mag);		// Y on image

	int	xod = (int)(display_rect.Width);		// X on display
	int	yod = (int)(display_rect.Height);		// Y on display

	// HORZ
	if (xoi < xod)
	{
		result.X = (int)(display_rect.X + (int)round((xod-xoi)/2.0));
		result.Width = (int)xoi;
	}
	// VERT
	if (yoi < yod)
	{
		result.Y = (int)(display_rect.Y + (int)round((yod-yoi)/2.0));
		result.Height = (int)yoi;
	}

	return result;
}

// ======================================================================
TxRectangleD TxCanvas::VisibleRect(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point)
{
	if (mag <= 0)
		return TxRectangleD();

	auto	result = TxRectangleD( 0, 0, bg_size.Width, bg_size.Height );

	double	xod = display_rect.Width  / mag;	// X on display
	double	yod = display_rect.Height / mag;	// Y on display

	// HORZ
	if (xod < bg_size.Width)
	{
		double sx = (view_point.X - xod / 2.0);
		if (sx < 0)
			sx = 0;

		double ex = (sx + xod);
		if (ex > bg_size.Width)
		{
			ex = bg_size.Width;
			sx = bg_size.Width - xod;
		}

		result.X = sx;
		result.Width = (ex - sx);
	}
	// VERT
	if (yod < bg_size.Height)
	{
		double sy = (view_point.Y - yod / 2.0);
		if (sy < 0)
			sy = 0;

		double ey = (sy + yod);
		if (ey > bg_size.Height)
		{
			ey = bg_size.Height;
			sy = bg_size.Height - yod;
		}

		result.Y = sy;
		result.Height = (ey - sy);
	}

	return result;
}

// ======================================================================
TxRectangleI TxCanvas::VisibleRectI(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, bool includePartialPixel)
{
	TxRectangleD src = VisibleRect(display_rect, bg_size, mag, view_point);

	int sx, sy, ex, ey;
	if (includePartialPixel)
	{
		sx = (int)floor(src.X);
		sy = (int)floor(src.Y);
		ex = (int)ceil(src.X + src.Width);
		ey = (int)ceil(src.Y + src.Height);
	}
	else
	{
		sx = (int)ceil(src.X);
		sy = (int)ceil(src.Y);
		ex = (int)floor(src.X + src.Width);
		ey = (int)floor(src.Y + src.Height);
	}

	if (ex > bg_size.Width)
		ex = bg_size.Width;
	if (ey > bg_size.Height)
		ey = bg_size.Height;

	TxRectangleI dst(sx, sy, (ex - sx), (ey - sy));
	return dst;
}

// ======================================================================
TxPointD TxCanvas::DPtoIP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD dp, ExGdiScalingMode mode)
{
	if (mag <= 0)
		return TxPointD();

	TxRectangleD eff = EffectiveRect( display_rect, bg_size, mag );
	TxRectangleD vis = VisibleRect( display_rect, bg_size, mag, view_point );

	TxPointD ip;
	switch( mode )
	{
	default:
	case ExGdiScalingMode::TopLeft:
		ip.X = (dp.X - eff.X) / mag + vis.X;
		ip.Y = (dp.Y - eff.Y) / mag + vis.Y;
		break;
	case ExGdiScalingMode::Center:
		ip.X = (dp.X - eff.X) / mag + vis.X - 0.5;
		ip.Y = (dp.Y - eff.Y) / mag + vis.Y - 0.5;
		break;
	}
	return ip;
}

// ======================================================================
TxPointD TxCanvas::IPtoDP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD ip, ExGdiScalingMode mode)
{
	if (mag <= 0)
		return TxPointD();

	TxRectangleD eff = EffectiveRect( display_rect, bg_size, mag );
	TxRectangleD vis = VisibleRect( display_rect, bg_size, mag, view_point );

	TxPointD dp;
	switch( mode )
	{
	default:
	case ExGdiScalingMode::TopLeft:
		dp.X = (ip.X - vis.X) * mag + eff.X;
		dp.Y = (ip.Y - vis.Y) * mag + eff.Y;
		break;
	case ExGdiScalingMode::Center:
		dp.X = (ip.X - vis.X + 0.5) * mag + eff.X;
		dp.Y = (ip.Y - vis.Y + 0.5) * mag + eff.Y;
		break;
	}

	return dp;
}

}	// GDI
}	// xie
