/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXDISPOSABLE_H_INCLUDED_
#define _IXDISPOSABLE_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

struct IxDisposable
{
	virtual void Dispose() = 0;
	virtual bool IsValid() const = 0;
};

}

#pragma pack(pop)

#endif
