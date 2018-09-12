/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXVALIDATABLE_H_INCLUDED_
#define _IXVALIDATABLE_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

// ============================================================
struct IxValidatable
{
	virtual void Validate() = 0;
};

// ============================================================
struct IxValidator
{
	virtual void Validate(IxModule& src) const = 0;
};

}

#pragma pack(pop)

#endif
