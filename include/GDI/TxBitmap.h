/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXBITMAP_H_INCLUDED_
#define _TXBITMAP_H_INCLUDED_

#include "xie_high.h"
#include "Core/TxImage.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
namespace GDI
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxBitmap
{
	void*			Address;
	int				Width;
	int				Height;
	TxModel			Model;
	int				Stride;

#if defined(__cplusplus)
	static inline TxBitmap Default()
	{
		TxBitmap result;
		result.Address = NULL;
		result.Width = 0;
		result.Height = 0;
		result.Model = TxModel::Default();
		result.Stride = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxBitmap();
	TxBitmap(void* addr, int width, int height, TxModel model, int stride);
	operator TxImage() const;
#endif	// __cplusplus
};

#if defined(__cplusplus)
}	// GDI
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
