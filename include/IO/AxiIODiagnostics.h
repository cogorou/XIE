/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _AXIIODIAGNOSTICS_H_INCLUDED_
#define _AXIIODIAGNOSTICS_H_INCLUDED_

#include "xie_high.h"
#include "Core/CxString.h"

namespace xie
{

XIE_EXPORT_CLASS CxStringA ToString(xie::IO::ExParity value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(xie::IO::ExStopBits value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(xie::IO::ExHandshake value, bool is_fullname = false);
XIE_EXPORT_CLASS CxStringA ToString(xie::IO::ExNewLine value, bool is_fullname = false);

}	// xie

#endif
