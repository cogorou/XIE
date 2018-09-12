/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXEQUATABLE_H_INCLUDED_
#define _IXEQUATABLE_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

// ============================================================
struct IxEquatable
{
	virtual void CopyFrom(const IxModule& src) = 0;
	virtual bool ContentEquals(const IxModule& src) const = 0;
};

// ============================================================
struct IxConvertible
{
	virtual void CopyTo(IxModule& dst) const = 0;
};

}

#pragma pack(pop)

#endif
