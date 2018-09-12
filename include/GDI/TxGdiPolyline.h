/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXGDIPOLYLINE_H_INCLUDED_
#define _TXGDIPOLYLINE_H_INCLUDED_

#include "xie_high.h"
#include "Core/TxArray.h"
#include "Core/TxModel.h"
#include "GDI/TxGdi2dParam.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
namespace GDI
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxGdiPolyline
{
	void*			Address;
	int				Length;
	TxModel			Model;
	ExBoolean		Closed;
	TxGdi2dParam	Param;

#if defined(__cplusplus)
	static inline TxGdiPolyline Default()
	{
		TxGdiPolyline result;
		result.Address = NULL;
		result.Length = 0;
		result.Model = TxModel::Default();
		result.Closed = ExBoolean::False;
		result.Param = TxGdi2dParam::Default();
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxGdiPolyline();
	TxGdiPolyline(void* addr, int length, TxModel model, ExBoolean closed);
	operator TxArray() const;
#endif	// __cplusplus
};

#if defined(__cplusplus)
}	// GDI
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
