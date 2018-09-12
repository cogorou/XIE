/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXDEVICEPARAM_H_INCLUDED_
#define _TXDEVICEPARAM_H_INCLUDED_

#include "xie_high.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
namespace Media
{
#endif	// __cplusplus

struct XIE_EXPORT_CLASS TxDeviceParam
{
	char*		Name;
	int			Index;
	int			Pin;
	int			Width;
	int			Height;

#if defined(__cplusplus)
	static inline TxDeviceParam Default()
	{
		TxDeviceParam result;
		result.Name		= NULL;
		result.Index	= 0;
		result.Pin		= 0;
		result.Width	= 0;
		result.Height	= 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxDeviceParam();
	TxDeviceParam(char* name, int index, int pin, int width, int height);
#endif
};

#if defined(__cplusplus)
}	// Media
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
