/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_media.h"

namespace xie
{
namespace Media
{

// ////////////////////////////////////////////////////////////
// EXTERN VARIABLE

// ======================================================================
void XIE_API fnPRV_Media_Setup()
{
#if defined(_MSC_VER)
	fnPRV_DS_Setup();
#else
	fnPRV_VL_Setup();
#endif
}

// ======================================================================
void XIE_API fnPRV_Media_TearDown()
{
#if defined(_MSC_VER)
	fnPRV_DS_TearDown();
#else
	fnPRV_VL_TearDown();
#endif
}

}	// Media
}	// xie
