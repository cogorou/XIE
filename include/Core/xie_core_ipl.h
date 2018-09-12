/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _XIE_CORE_IPL_H_INCLUDED_
#define _XIE_CORE_IPL_H_INCLUDED_

#include "xie_core.h"

// ======================================================================

#if defined(__cplusplus)
namespace xie
{
namespace IPLDefs
{
#endif	// __cplusplus

// ////////////////////////////////////////////////////////////
// ENUM

// ======================================================================
enum IPL_DEPTH
{
	IPL_DEPTH_1U    =  1,
	IPL_DEPTH_8U    =  8,
	IPL_DEPTH_16U   = 16,
	IPL_DEPTH_32F   = 32,
	IPL_DEPTH_64F   = 64,	// unofficial

	IPL_DEPTH_8S  = (0x80000000| 8),
	IPL_DEPTH_16S = (0x80000000|16),
	IPL_DEPTH_32S = (0x80000000|32),
};

// ======================================================================
enum IPL_DATA_ORDER
{
	IPL_DATA_ORDER_PIXEL  = 0,
	IPL_DATA_ORDER_PLANE  = 1,
};

// ======================================================================
enum IPL_ORIGIN
{
	IPL_ORIGIN_TL = 0,
	IPL_ORIGIN_BL = 1,
};

// ======================================================================
enum IPL_ALIGN
{
	IPL_ALIGN_DWORD   = 4,
	IPL_ALIGN_QWORD   = 8,
};

// ======================================================================
enum IPL_BORDER
{
	IPL_BORDER_CONSTANT   = 0,
	IPL_BORDER_REPLICATE  = 1,
	IPL_BORDER_REFLECT    = 2,
	IPL_BORDER_WRAP       = 3,
};

#if defined(__cplusplus)
}
}
#endif	// __cplusplus

#endif
