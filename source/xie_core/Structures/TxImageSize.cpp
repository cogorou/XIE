/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_core.h"
#include "Core/xie_core_ipl.h"
#include "Core/TxImageSize.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include <typeinfo>
#include <math.h>

namespace xie
{

// ============================================================
TxImageSize::TxImageSize()
{
	Width = 0;
	Height = 0;
	Model = TxModel::Default();
	Channels = 0;
	Depth = 0;
}

// ============================================================
TxImageSize::TxImageSize(int width, int height, TxModel model, int channels)
{
	Width = width;
	Height = height;
	Model = model;
	Channels = channels;
	Depth = 0;
}

// ============================================================
TxImageSize::TxImageSize(int width, int height, TxModel model, int channels, int depth)
{
	Width = width;
	Height = height;
	Model = model;
	Channels = channels;
	Depth = depth;
}

// ============================================================
TxImageSize::TxImageSize(TxSizeI size, TxModel model, int channels)
{
	Width = size.Width;
	Height = size.Height;
	Model = model;
	Channels = channels;
	Depth = 0;
}

// ============================================================
TxImageSize::TxImageSize(TxSizeI size, TxModel model, int channels, int depth)
{
	Width = size.Width;
	Height = size.Height;
	Model = model;
	Channels = channels;
	Depth = depth;
}

// ============================================================
bool TxImageSize::operator == (const TxImageSize& cmp) const
{
	const TxImageSize& src = *this;
	if (src.Width		!= cmp.Width) return false;
	if (src.Height		!= cmp.Height) return false;
	if (src.Model		!= cmp.Model) return false;
	if (src.Channels	!= cmp.Channels) return false;
//	if (src.Depth		!= cmp.Depth) return false;	// ignore
	return true;
}

// ============================================================
bool TxImageSize::operator != (const TxImageSize& cmp) const
{
	return !(operator == (cmp));
}

// ============================================================
TxImageSize TxImageSize::operator + (const TxSizeI& value) const
{
	const TxImageSize&	src = *this;
	TxImageSize			ans = src;
	ans.Width	= src.Width + value.Width;
	ans.Height	= src.Height + value.Height;
	return ans;
}
TxImageSize TxImageSize::operator + (int value) const
{
	const TxImageSize&	src = *this;
	TxImageSize			ans = src;
	ans.Width	= src.Width + value;
	ans.Height	= src.Height + value;
	return ans;
}
TxImageSize TxImageSize::operator + (double value) const
{
	const TxImageSize&	src = *this;
	TxImageSize			ans = src;
	ans.Width	= (int)round(src.Width + value);
	ans.Height	= (int)round(src.Height + value);
	return ans;
}

// ============================================================
TxImageSize TxImageSize::operator - (const TxSizeI& value) const
{
	const TxImageSize&	src = *this;
	TxImageSize			ans = src;
	ans.Width	= src.Width - value.Width;
	ans.Height	= src.Height - value.Height;
	return ans;
}
TxImageSize TxImageSize::operator - (int value) const
{
	const TxImageSize&	src = *this;
	TxImageSize			ans = src;
	ans.Width	= src.Width - value;
	ans.Height	= src.Height - value;
	return ans;
}
TxImageSize TxImageSize::operator - (double value) const
{
	const TxImageSize&	src = *this;
	TxImageSize			ans = src;
	ans.Width	= (int)round(src.Width - value);
	ans.Height	= (int)round(src.Height - value);
	return ans;
}

// ============================================================
TxImageSize TxImageSize::operator * (const TxSizeI& value) const
{
	const TxImageSize&	src = *this;
	TxImageSize			ans = src;
	ans.Width	= src.Width * value.Width;
	ans.Height	= src.Height * value.Height;
	return ans;
}
TxImageSize TxImageSize::operator * (int value) const
{
	const TxImageSize&	src = *this;
	TxImageSize			ans = src;
	ans.Width	= src.Width * value;
	ans.Height	= src.Height * value;
	return ans;
}
TxImageSize TxImageSize::operator * (double value) const
{
	const TxImageSize&	src = *this;
	TxImageSize			ans = src;
	ans.Width	= (int)round(src.Width * value);
	ans.Height	= (int)round(src.Height * value);
	return ans;
}

// ============================================================
TxImageSize TxImageSize::operator / (const TxSizeI& value) const
{
	const TxImageSize&	src = *this;
	TxImageSize			ans = src;
	ans.Width	= src.Width / value.Width;
	ans.Height	= src.Height / value.Height;
	return ans;
}
TxImageSize TxImageSize::operator / (int value) const
{
	const TxImageSize&	src = *this;
	TxImageSize			ans = src;
	ans.Width	= src.Width / value;
	ans.Height	= src.Height / value;
	return ans;
}
TxImageSize TxImageSize::operator / (double value) const
{
	const TxImageSize&	src = *this;
	TxImageSize			ans = src;
	ans.Width	= (int)round(src.Width / value);
	ans.Height	= (int)round(src.Height / value);
	return ans;
}

// ============================================================
TxSizeI TxImageSize::Size() const
{
	return TxSizeI(Width, Height);
}

// ============================================================
void TxImageSize::Size(const TxSizeI& value)
{
	Width = value.Width;
	Height = value.Height;
}

}
