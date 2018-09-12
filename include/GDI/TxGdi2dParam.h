/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXGDI2DPARAM_H_INCLUDED_
#define _TXGDI2DPARAM_H_INCLUDED_

#include "xie_high.h"
#include "Core/TxPointD.h"
#include "Core/TxSizeD.h"
#include "Core/TxRGB8x4.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
namespace GDI
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxGdi2dParam
{
	double				Angle;
	TxPointD			Axis;
	TxRGB8x4			BkColor;
	ExBoolean			BkEnable;
	TxRGB8x4			PenColor;
	ExGdiPenStyle		PenStyle;
	int					PenWidth;

#if defined(__cplusplus)
	static inline TxGdi2dParam Default()
	{
		TxGdi2dParam result;
		result.Angle		= 0;
		result.Axis			= TxPointD(0, 0);
		result.BkColor		= TxRGB8x4(0x00, 0x00, 0x00, 0xFF);
		result.BkEnable		= ExBoolean::False;
		result.PenColor		= TxRGB8x4(0xFF, 0xFF, 0xFF, 0xFF);
		result.PenStyle		= ExGdiPenStyle::Solid;
		result.PenWidth		= 1;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxGdi2dParam();

	bool operator == (const TxGdi2dParam& cmp) const;
	bool operator != (const TxGdi2dParam& cmp) const;
#endif	// __cplusplus
};

#if defined(__cplusplus)
}	// GDI
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

// ============================================================
#if defined(__cplusplus) && !defined(XIE_TEMPLATE_SPECIALIZE_DISABLED)
namespace xie
{
	template<> inline TxModel ModelOf<xie::GDI::TxGdi2dParam>()
	{
		TxModel model(ExType::U8, 0);
		model += ModelOf<double>().Size();
		model += ModelOf<TxPointD>().Size();
		model += ModelOf<TxRGB8x4>().Size();
		model += ModelOf<ExBoolean>().Size();
		model += ModelOf<TxRGB8x4>().Size();
		model += ModelOf<xie::GDI::ExGdiPenStyle>().Size();
		model += ModelOf<int>().Size();
		return model;
	}
}	// xie
#endif	// __cplusplus

#endif
