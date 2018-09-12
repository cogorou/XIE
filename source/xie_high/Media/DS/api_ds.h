/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifdef _MSC_VER

#pragma once

#ifndef _API_DS_H_INCLUDED_
#define _API_DS_H_INCLUDED_

#include "xie_high.h"

#if defined(_MSC_VER)
#include "xie_ds.h"
#include "api_ds_com.h"
#endif

#include "Core/xie_core_defs.h"
#include "Core/xie_core_math.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxArrayEx.h"
#include "Core/CxImage.h"
#include "Core/CxArray.h"
#include "Core/CxString.h"

#include <windows.h>
#include <tchar.h>			// need include before strsafe
#include <ole2.h>
#include <dshow.h>
#include <dshowasf.h>		// require for Asf
#include <initguid.h>
#include <strsafe.h>
#include <atlbase.h>
#include <math.h>
#include <wmcodecdsp.h>		// reqiured for CLSID_CMPEG2EncoderVideoDS
#include <ks.h>				// required for DEFINE_GUIDSTRUCT
#include <math.h>

// ////////////////////////////////////////////////////////////
// PROTOTYPE

namespace xie
{
namespace Media
{

void XIE_API fnPRV_DS_Setup();
void XIE_API fnPRV_DS_TearDown();

bool				XIE_API fnPRV_DS_CompareGuid(TxCharCPtrW src1, TxCharCPtrW src2);
CxStringA			XIE_API fnPRV_DS_ToString(GUID guid);
GUID				XIE_API fnPRV_DS_GetDeviceCategory(ExMediaType type, ExMediaDir dir);

int					XIE_API fnPRV_DS_GetDeviceCount	(ExMediaType type, ExMediaDir dir);
CxArrayEx<CxStringA>	XIE_API fnPRV_DS_GetDeviceNames	(ExMediaType type, ExMediaDir dir);

int					XIE_API fnPRV_DS_GetDeviceIndex	(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index);

CxStringA			XIE_API fnPRV_DS_GetProductName	(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index);
CxArrayEx<CxStringA>	XIE_API fnPRV_DS_GetPinNames		(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index);
CxArrayEx<TxSizeI>	XIE_API fnPRV_DS_GetFrameSizes	(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index, int pin);

IBaseFilter*		XIE_API fnPRV_DS_CreateDeviceFilter(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index, CxStringA* device_name, CxStringA* product_name);
IPin*				XIE_API fnPRV_DS_FindPin			(IBaseFilter* filter, int index, PIN_DIRECTION direction);
void				XIE_API fnPRV_DS_SetFrameSize		(IBaseFilter* filter, TxSizeI frame_size);
TxSizeI				XIE_API fnPRV_DS_GetFrameSize		(IBaseFilter* filter);
CxArrayEx<TxSizeI>	XIE_API fnPRV_DS_GetFrameSizes	(IBaseFilter* filter);

void				XIE_API fnPRV_DS_SetVideoFrameSize(IBaseFilter* mux, TxSizeI frame_size);

VIDEOINFOHEADER		XIE_API fnPRV_DS_GetVideoInfo		(IBaseFilter* grabber);
bool				XIE_API fnPRV_DS_Connected		(IBaseFilter* grabber);

HRESULT XIE_API fnPRV_DS_SaveGraphFileA(IGraphBuilder *pGraph, TxCharCPtrA szPath);
HRESULT XIE_API fnPRV_DS_SaveGraphFileW(IGraphBuilder *pGraph, TxCharCPtrW szPath);

}	// Media
}	// xie

#endif

#endif	// _MCS_VER
