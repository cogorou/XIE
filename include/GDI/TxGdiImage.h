/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXGDIIMAGE_H_INCLUDED_
#define _TXGDIIMAGE_H_INCLUDED_

#include "xie_high.h"
#include "Core/TxImage.h"
#include "GDI/TxGdi2dParam.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
namespace GDI
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxGdiImage
{
	void*			Address;
	int				Width;
	int				Height;
	TxModel			Model;
	int				Stride;
	double			X;
	double			Y;
	double			MagX;
	double			MagY;
	double			Alpha;
	ExBoolean		AlphaFormat;
	TxGdi2dParam	Param;

#if defined(__cplusplus)
	static inline TxGdiImage Default()
	{
		TxGdiImage result;
		result.Address = NULL;
		result.Width = 0;
		result.Height = 0;
		result.Model = TxModel::Default();
		result.Stride = 0;
		result.X = 0;
		result.Y = 0;
		result.MagX = 1.0;
		result.MagY = 1.0;
		result.Alpha = 1.0;
		result.AlphaFormat = ExBoolean::False;
		result.Param = TxGdi2dParam::Default();
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxGdiImage();
	operator TxImage() const;
#endif	// __cplusplus
};

#if defined(__cplusplus)
}	// GDI
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
