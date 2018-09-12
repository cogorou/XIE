/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXCANVAS_H_INCLUDED_
#define _TXCANVAS_H_INCLUDED_

#include "xie_high.h"
#include "Core/TxRectangleD.h"
#include "Core/TxRectangleI.h"
#include "Core/TxSizeI.h"
#include "Core/TxPointD.h"
#include "Core/TxRGB8x4.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
namespace GDI
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxCanvas
{
	int				Width;
	int				Height;
	TxSizeI			BgSize;
	TxRGB8x4		BkColor;
	ExBoolean		BkEnable;
	double			Magnification;
	TxPointD		ViewPoint;
	int				ChannelNo;
	ExBoolean		Unpack;
	ExBoolean		Halftone;

#if defined(__cplusplus)
	static inline TxCanvas Default()
	{
		TxCanvas result;
		result.Width			= 0;
		result.Height			= 0;
		result.BgSize			= TxSizeI::Default();
		result.BkColor			= TxRGB8x4::Default();
		result.BkEnable			= ExBoolean::False;
		result.Magnification	= 1.0;
		result.ViewPoint		= TxPointD::Default();
		result.ChannelNo		= 0;
		result.Unpack			= ExBoolean::False;
		result.Halftone			= ExBoolean::False;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxCanvas();
	TxCanvas(int width, int height, const TxSizeI& bg_size, double mag, const TxPointD& view_point);
	TxCanvas(int width, int height, const TxSizeI& bg_size, double mag, const TxPointD& view_point, int ch, ExBoolean unpack, ExBoolean halftone);

	bool operator == (const TxCanvas& cmp) const;
	bool operator != (const TxCanvas& cmp) const;

	TxSizeI DisplaySize() const;
	TxRectangleI DisplayRect() const;
	TxRectangleI EffectiveRect() const;
	TxRectangleD VisibleRect() const;
	TxRectangleI VisibleRectI(bool includePartialPixel) const;
	TxPointD DPtoIP(const TxPointD& dp, ExGdiScalingMode mode) const;
	TxPointD IPtoDP(const TxPointD& ip, ExGdiScalingMode mode) const;

	static TxRectangleI	EffectiveRect(TxRectangleI display_rect, TxSizeI bg_size, double mag);
	static TxRectangleD	VisibleRect(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point);
	static TxRectangleI	VisibleRectI(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, bool includePartialPixel);
	static TxPointD		DPtoIP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD dp, ExGdiScalingMode mode);
	static TxPointD		IPtoDP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD ip, ExGdiScalingMode mode);
#endif	// __cplusplus
};

#if defined(__cplusplus)
}	// GDI
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
