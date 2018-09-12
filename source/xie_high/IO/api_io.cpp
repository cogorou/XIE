/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_io.h"

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArrayEx.h"

#include "IO/CxSerialPort.h"
#include "IO/TxSerialPort.h"

#include <math.h>

namespace xie
{
namespace IO
{

// ////////////////////////////////////////////////////////////
// EXTERN VARIABLE

static bool g_setuped = false;

// ======================================================================
void XIE_API fnPRV_IO_Setup()
{
	if (g_setuped) return;
	g_setuped = true;

	#if defined(_MSC_VER)
	#else
	#endif
}

// ======================================================================
void XIE_API fnPRV_IO_TearDown()
{
	#if defined(_MSC_VER)
	#else
	#endif
}

}	// IO
}	// xie
