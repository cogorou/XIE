/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _AXIDIAGNOSTICS_H_INCLUDED_
#define _AXIDIAGNOSTICS_H_INCLUDED_

#include "xie_core.h"
#include "Core/TxModel.h"
#include "Core/CxString.h"

namespace xie
{

XIE_EXPORT_CLASS CxStringA ToString(ExStatus value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(ExType value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(TxModel value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(ExBoolean value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(ExScanDir value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(ExEndianType value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(ExMath value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(ExOpe1A value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(ExOpe1L value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(ExOpe2A value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(ExOpe2L value, bool is_fullname = false);

}	// xie

#endif
