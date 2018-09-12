/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#pragma once

#ifndef _API_V4L2_H_INCLUDED_
#define _API_V4L2_H_INCLUDED_

#include "xie_high.h"
#include "Core/xie_core_defs.h"
#include "Core/xie_core_math.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArrayEx.h"
#include "Core/CxImage.h"
#include "Core/CxArray.h"
#include "Core/CxString.h"

#include <math.h>

namespace xie
{
namespace Media
{

// ////////////////////////////////////////////////////////////
// PROTOTYPE

void XIE_API fnPRV_VL_Setup();
void XIE_API fnPRV_VL_TearDown();

int					XIE_API fnPRV_VL_GetDeviceCount	(ExMediaType type, ExMediaDir dir);
CxArrayEx<CxStringA>	XIE_API fnPRV_VL_GetDeviceNames	(ExMediaType type, ExMediaDir dir);
CxStringA			XIE_API fnPRV_VL_GetProductName	(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index);
CxArrayEx<CxStringA>	XIE_API fnPRV_VL_GetPinNames		(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index);
CxArrayEx<TxSizeI>	XIE_API fnPRV_VL_GetFrameSizes	(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index, int pin);

int xioctl(int fd, int request, void* arg);

}	// Media
}	// xie

#endif

#endif	// _MCS_VER
