/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXSCREENLISTITEM_H_INCLUDED_
#define _TXSCREENLISTITEM_H_INCLUDED_

#include "xie_high.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
namespace Media
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxScreenListItem
{
	#if defined(_MSC_VER)
	HWND		Handle;
	#else
	TxIntPtr	Handle;
	#endif
	char*		Name;
	int			X;
	int			Y;
	int			Width;
	int			Height;

#if defined(__cplusplus)
	static inline TxScreenListItem Default()
	{
		TxScreenListItem result;
		#if defined(_MSC_VER)
		result.Handle	= NULL;
		#else
		result.Handle	= 0;
		#endif
		result.Name		= NULL;
		result.X		= 0;
		result.Y		= 0;
		result.Width	= 0;
		result.Height	= 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxScreenListItem();
	#if defined(_MSC_VER)
	TxScreenListItem(HWND handle, char* name, int x, int y, int width, int height);
	#else
	TxScreenListItem(TxIntPtr handle, char* name, int x, int y, int width, int height);
	#endif
#endif
};

#if defined(__cplusplus)
}	// Media
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
