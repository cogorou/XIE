/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXTAGPTR_H_INCLUDED_
#define _IXTAGPTR_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

struct IxTagPtr
{
	virtual void* TagPtr() const = 0;
};

}

#pragma pack(pop)

#endif
