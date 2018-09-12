/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXLOCKABLE_H_INCLUDED_
#define _IXLOCKABLE_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

struct IxLockable
{
	virtual void Lock() = 0;
	virtual void Unlock() = 0;
	virtual bool IsLocked() const = 0;
};

}

#pragma pack(pop)

#endif
