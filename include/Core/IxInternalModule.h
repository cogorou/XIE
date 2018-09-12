/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXINTERNALMODULE_H_INCLUDED_
#define _IXINTERNALMODULE_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

// ============================================================
struct IxInternalModule
{
	virtual IxModule* GetModule() const = 0;
};

}

#pragma pack(pop)

#endif
