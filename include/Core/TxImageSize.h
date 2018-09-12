/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXIMAGESIZE_H_INCLUDED_
#define _TXIMAGESIZE_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxImageSize
{
	int				Width;
	int				Height;
	TxModel			Model;
	int				Channels;
	int				Depth;

#if defined(__cplusplus)
	static inline TxImageSize Default()
	{
		TxImageSize result;
		result.Width = 0;
		result.Height = 0;
		result.Model = TxModel::Default();
		result.Channels = 0;
		result.Depth = 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxImageSize();
	TxImageSize(int width, int height, TxModel model, int channels);
	TxImageSize(int width, int height, TxModel model, int channels, int depth);
	TxImageSize(TxSizeI size, TxModel model, int channels);
	TxImageSize(TxSizeI size, TxModel model, int channels, int depth);

	bool operator == (const TxImageSize& cmp) const;
	bool operator != (const TxImageSize& cmp) const;

	TxImageSize operator + (const TxSizeI& value) const;
	TxImageSize operator + (int value) const;
	TxImageSize operator + (double value) const;
	TxImageSize operator - (const TxSizeI& value) const;
	TxImageSize operator - (int value) const;
	TxImageSize operator - (double value) const;
	TxImageSize operator * (const TxSizeI& value) const;
	TxImageSize operator * (int value) const;
	TxImageSize operator * (double value) const;
	TxImageSize operator / (const TxSizeI& value) const;
	TxImageSize operator / (int value) const;
	TxImageSize operator / (double value) const;

	TxSizeI Size() const;
	void Size(const TxSizeI& value);
#endif
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
