/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXIMAGE_H_INCLUDED_
#define _TXIMAGE_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxLayer.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxImage
{
	TxLayer		Layer;
	int			Width;
	int			Height;
	TxModel		Model;
	int			Channels;
	int			Stride;
	int			Depth;

#if defined(__cplusplus)
	static inline TxImage Default()
	{
		TxImage result;
		result.Layer	= TxLayer::Default();
		result.Width	= 0;
		result.Height	= 0;
		result.Model	= TxModel::Default();
		result.Channels	= 0;
		result.Stride	= 0;
		result.Depth	= 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxImage();
	TxImage(void* addr, int width, int height, TxModel model, int stride, int depth);
	TxImage(TxLayer layer, int width, int height, TxModel model, int channels, int stride, int depth);

	bool operator == (const TxImage& cmp) const;
	bool operator != (const TxImage& cmp) const;
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
