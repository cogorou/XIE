/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_core.h"
#include "Core/TxImage.h"
#include "Core/Axi.h"
#include "Core/CxException.h"

namespace xie
{

// ============================================================
TxImage::TxImage()
{
	Layer		= TxLayer::Default();
	Width		= 0;
	Height		= 0;
	Model		= TxModel::Default();
	Channels	= 0;
	Stride		= 0;
	Depth		= 0;
}

// ============================================================
TxImage::TxImage(void* addr, int width, int height, TxModel model, int stride, int depth)
{
	Layer		= TxLayer(addr);
	Width		= width;
	Height		= height;
	Model		= model;
	Channels	= 1;
	Stride		= stride;
	Depth		= depth;
}

// ============================================================
TxImage::TxImage(TxLayer layer, int width, int height, TxModel model, int channels, int stride, int depth)
{
	Layer		= layer;
	Width		= width;
	Height		= height;
	Model		= model;
	Channels	= channels;
	Stride		= stride;
	Depth		= depth;
}

// ============================================================
bool TxImage::operator == (const TxImage& cmp) const
{
	const TxImage& src = *this;
	if (src.Layer		!= cmp.Layer) return false;
	if (src.Width		!= cmp.Width) return false;
	if (src.Height		!= cmp.Height) return false;
	if (src.Model		!= cmp.Model) return false;
	if (src.Channels	!= cmp.Channels) return false;
	if (src.Stride		!= cmp.Stride) return false;
	if (src.Depth		!= cmp.Depth) return false;
	return true;
}

// ============================================================
bool TxImage::operator != (const TxImage& cmp) const
{
	return !(operator == (cmp));
}

}
