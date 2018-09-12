/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXSTRING_H_INCLUDED_
#define _CXSTRING_H_INCLUDED_

#include "xie_core.h"

#include "Core/CxStringA.h"
#include "Core/CxStringW.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

#if defined(UNICODE)
typedef CxStringW	CxString;
#else
typedef CxStringA	CxString;
#endif

}

#pragma pack(pop)

#endif
