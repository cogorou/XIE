/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "GDI/TxGdi2dParam.h"

namespace xie
{
namespace GDI
{

// ================================================================================
TxGdi2dParam::TxGdi2dParam()
{
	Angle		= 0;
	Axis		= TxPointD(0, 0);
	BkColor		= TxRGB8x4(0x00, 0x00, 0x00, 0xFF);
	BkEnable	= ExBoolean::False;
	PenColor	= TxRGB8x4(0xFF, 0xFF, 0xFF, 0xFF);
	PenStyle	= ExGdiPenStyle::Solid;
	PenWidth	= 1;
}

// ================================================================================
bool TxGdi2dParam::operator == (const TxGdi2dParam& cmp) const
{
	const TxGdi2dParam&	src = *this;
	if (src.Angle		!= cmp.Angle) return false;
	if (src.Axis		!= cmp.Axis) return false;
	if (src.BkColor		!= cmp.BkColor) return false;
	if (src.BkEnable	!= cmp.BkEnable) return false;
	if (src.PenColor	!= cmp.PenColor) return false;
	if (src.PenStyle	!= cmp.PenStyle) return false;
	if (src.PenWidth	!= cmp.PenWidth) return false;
	return true; 
}

// ================================================================================
bool TxGdi2dParam::operator != (const TxGdi2dParam& cmp) const
{
	return !(operator == (cmp));
}

}	// GDI
}	// xie
