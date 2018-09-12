/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXRUNNABLE_H_INCLUDED_
#define _IXRUNNABLE_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

struct IxRunnable
{
	virtual void Reset() = 0;
	virtual void Start() = 0;
	virtual void Stop() = 0;
	virtual bool Wait(int timeout) const = 0;
	virtual bool IsRunning() const = 0;
};

}

#pragma pack(pop)

#endif
