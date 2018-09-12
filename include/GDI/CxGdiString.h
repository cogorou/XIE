/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGDISTRING_H_INCLUDED_
#define _CXGDISTRING_H_INCLUDED_

#include "xie_high.h"

#include "GDI/CxGdiStringA.h"
#include "GDI/CxGdiStringW.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace GDI
{

#if defined(UNICODE)
typedef CxGdiStringW	CxGdiString;
#else
typedef CxGdiStringA	CxGdiString;
#endif

}
}

#pragma pack(pop)

#endif
