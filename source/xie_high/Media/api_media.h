/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _API_MEDIA_H_INCLUDED_
#define _API_MEDIA_H_INCLUDED_

#include "xie_high.h"
#include "Core/xie_core_defs.h"
#include "Core/xie_core_math.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArrayEx.h"
#include "Core/CxImage.h"
#include "Core/CxArray.h"
#include "Core/CxString.h"

#if defined(_MSC_VER)
#include "DS/api_ds.h"
#else
#include "V4L2/api_v4l2.h"
#endif

// ////////////////////////////////////////////////////////////
// PROTOTYPE

namespace xie
{
namespace Media
{

void XIE_API fnPRV_Media_Setup();
void XIE_API fnPRV_Media_TearDown();

// ============================================================
template<class TCLASS, class TS> static inline bool fnPRV_Media_GetParam(void* dst, TxModel model, const TCLASS* owner, TS (TCLASS::*src)(void) const)
{
	if (model == ModelOf<TS>())
	{
		*static_cast<TS*>(dst) = (owner->*src)();
		return true;
	}
	return false;
}

// ============================================================
template<class TCLASS, class TD> static inline bool fnPRV_Media_SetParam(const void* src, TxModel model, TCLASS* owner, void (TCLASS::*dst)(TD value))
{
	if (model == ModelOf<TD>())
	{
		(owner->*dst)(*static_cast<const TD*>(src));
		return true;
	}
	return false;
}

}	// Media
}	// xie

#endif
