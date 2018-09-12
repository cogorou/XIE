/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXFILEACCESS_H_INCLUDED_
#define _IXFILEACCESS_H_INCLUDED_

#include "xie_core.h"
#include "Core/TxModel.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

struct IxFileAccess
{
	virtual void LoadA(TxCharCPtrA filename, const void* option, TxModel model) = 0;
	virtual void LoadW(TxCharCPtrW filename, const void* option, TxModel model) = 0;
	virtual void SaveA(TxCharCPtrA filename, const void* option, TxModel model) const = 0;
	virtual void SaveW(TxCharCPtrW filename, const void* option, TxModel model) const = 0;
};

}

#pragma pack(pop)

#endif
