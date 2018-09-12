/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXEVENTRECEIVER_H_INCLUDED_
#define _IXEVENTRECEIVER_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

// ============================================================
struct IxEventReceiver
{
	virtual void Receive(void* sender, IxModule* e) = 0;
};

}

#pragma pack(pop)

#endif
