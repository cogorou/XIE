/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _AXIMEDIADIAGNOSTICS_H_INCLUDED_
#define _AXIMEDIADIAGNOSTICS_H_INCLUDED_

#include "xie_high.h"
#include "Core/CxString.h"

namespace xie
{

XIE_EXPORT_CLASS CxStringA ToString(xie::Media::ExMediaType value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(xie::Media::ExMediaDir value, bool is_fullname = false);

}	// xie

#endif
