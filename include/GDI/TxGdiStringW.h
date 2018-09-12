/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXGDISTRINGW_H_INCLUDED_
#define _TXGDISTRINGW_H_INCLUDED_

#include "xie_high.h"
#include "Core/TxModel.h"
#include "GDI/TxGdi2dParam.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
namespace GDI
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxGdiStringW
{
	wchar_t*		Address;
	int				Length;
	double			X;
	double			Y;
	wchar_t*		FontName;
	int				FontSize;
	ExGdiTextAlign	Align;
	int				CodePage;
	TxGdi2dParam	Param;

#if defined(__cplusplus)
	static inline TxGdiStringW Default()
	{
		TxGdiStringW result;
		result.Address = NULL;
		result.Length = 0;
		result.X = 0;
		result.Y = 0;
		result.FontName = NULL;
		result.FontSize = 12;
		result.Align = ExGdiTextAlign::TopLeft;
		result.CodePage = 0;
		result.Param = TxGdi2dParam::Default();
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxGdiStringW();
#endif	// __cplusplus
};

#if defined(__cplusplus)
}	// GDI
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
