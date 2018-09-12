/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXDEVICELISTITEM_H_INCLUDED_
#define _TXDEVICELISTITEM_H_INCLUDED_

#include "xie_high.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
namespace Media
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxDeviceListItem
{
	ExMediaType	MediaType;
	ExMediaDir	MediaDir;
	char*		Name;
	int			Index;

#if defined(__cplusplus)
	static inline TxDeviceListItem Default()
	{
		TxDeviceListItem result;
		result.MediaType	= ExMediaType::None;
		result.MediaDir		= ExMediaDir::None;
		result.Name		= NULL;
		result.Index	= 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxDeviceListItem();
	TxDeviceListItem(ExMediaType type, ExMediaDir dir, char* name, int index);
#endif
};

#if defined(__cplusplus)
}	// Media
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
