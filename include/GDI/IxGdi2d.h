/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXGDI2D_H_INCLUDED_
#define _IXGDI2D_H_INCLUDED_

#include "xie_high.h"

#include "Core/CxModule.h"
#include "Core/IxEquatable.h"
#include "GDI/TxCanvas.h"
#include "GDI/TxHitPosition.h"
#include "Core/TxRectangleD.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

struct IxGdi2dRendering
{
	virtual void Render(HxModule hcanvas, ExGdiScalingMode mode) const = 0;
};

struct IxGdi2dHandling
{
	virtual TxPointD Location() const = 0;
	virtual void Location(TxPointD value) = 0;
	virtual TxRectangleD Bounds() const = 0;

	virtual double Angle() const = 0;
	virtual void Angle( double degree ) = 0;

	virtual TxPointD Axis() const = 0;
	virtual void Axis( TxPointD value ) = 0;

	virtual TxHitPosition HitTest( TxPointD position, double margin ) const = 0;
	virtual void Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin) = 0;
};

struct IxGdi2dVisualizing
{
	virtual TxRGB8x4 BkColor() const = 0;
	virtual void BkColor( TxRGB8x4 value ) = 0;

	virtual bool BkEnable() const = 0;
	virtual void BkEnable( bool value ) = 0;

	virtual TxRGB8x4 PenColor() const = 0;
	virtual void PenColor( TxRGB8x4 value ) = 0;

	virtual ExGdiPenStyle PenStyle() const = 0;
	virtual void PenStyle( ExGdiPenStyle value ) = 0;

	virtual int PenWidth() const = 0;
	virtual void PenWidth( int value ) = 0;
};

struct IxGdi2d
	: public IxGdi2dRendering
	, public IxGdi2dHandling
	, public IxGdi2dVisualizing
{
};

}	// GDI
}	// xie

#pragma pack(pop)

#endif	// _CXGDI2D_H_INCLUDED_
