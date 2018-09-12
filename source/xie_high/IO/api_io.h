/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _API_IO_H_INCLUDED_
#define _API_IO_H_INCLUDED_

#include "xie_high.h"
#include "Core/xie_core_defs.h"

#if defined(_MSC_VER)
#else
	#include <termios.h>
#endif

// ////////////////////////////////////////////////////////////
// TYPEDEF

// ////////////////////////////////////////////////////////////
// PROTOTYPE

namespace xie
{
namespace IO
{

void XIE_API fnPRV_IO_Setup();
void XIE_API fnPRV_IO_TearDown();

}	// IO
}	// xie

#endif
