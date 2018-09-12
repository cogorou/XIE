/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _XIE_H_INCLUDED_
#define _XIE_H_INCLUDED_

namespace xie
{
namespace Axi
{

// ======================================================================
static inline void Setup()
{
#if defined(XIE_CORE_PREFIX)
	xie_core_setup();
#endif
#if defined(XIE_HIGH_PREFIX)
	xie_high_setup();
#endif
}

// ======================================================================
static inline void TearDown()
{
#if defined(XIE_HIGH_PREFIX)
	xie_high_teardown();
#endif
#if defined(XIE_CORE_PREFIX)
	xie_core_teardown();
#endif
}

}
}

#endif
