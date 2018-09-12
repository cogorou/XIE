/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXATTACHABLE_H_INCLUDED_
#define _IXATTACHABLE_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

struct IxAttachable
{
	virtual void Attach(const IxModule& src) = 0;
	virtual bool IsAttached() const = 0;
};

}

#pragma pack(pop)

#endif
