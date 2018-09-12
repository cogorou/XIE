/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _AXIGDIDIAGNOSTICS_H_INCLUDED_
#define _AXIGDIDIAGNOSTICS_H_INCLUDED_

#include "xie_high.h"
#include "Core/CxString.h"

namespace xie
{

XIE_EXPORT_CLASS CxStringA ToString(xie::GDI::ExGdiScalingMode value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(xie::GDI::ExGdiAnchorStyle value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(xie::GDI::ExGdiBrushStyle value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(xie::GDI::ExGdiPenStyle value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(xie::GDI::ExGdiTextAlign value, bool is_fullname = false);

}	// xie

#endif
