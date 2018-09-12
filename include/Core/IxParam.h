/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXPARAM_H_INCLUDED_
#define _IXPARAM_H_INCLUDED_

#include "xie_core.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

// ============================================================
struct IxParam
{
	virtual void GetParam(TxCharCPtrA name, void* value, TxModel model) const = 0;
	virtual void SetParam(TxCharCPtrA name, const void* value, TxModel model) = 0;
};

// ============================================================
struct IxIndexedParam
{
	virtual void GetParam(TxCharCPtrA name, int index, void* value, TxModel model) const = 0;
	virtual void SetParam(TxCharCPtrA name, int index, const void* value, TxModel model) = 0;
};

}

#pragma pack(pop)

#endif
