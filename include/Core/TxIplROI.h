/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXIPLROI_H_INCLUDED_
#define _TXIPLROI_H_INCLUDED_

#include "xie_core.h"
#include "xie_core_types.h"

#pragma pack(push,XIE_PACKING_SIZE)

#if defined(__cplusplus)
namespace xie
{
#endif	// __cplusplus

struct TxIplROI
{
	int				coi;			// Channel of Interest [0:all / 1:?]
	int				xOffset;
	int				yOffset;
	int				width;
	int				height;
};

#if defined(__cplusplus)
}	// xie
#endif	// __cplusplus

#pragma pack(pop)

#endif
