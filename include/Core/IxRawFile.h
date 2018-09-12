/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXRAWFILE_H_INCLUDED_
#define _IXRAWFILE_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

struct IxRawFile
{
	virtual void* OpenRawA(TxCharCPtrA filename, int mode) = 0;
	virtual void* OpenRawW(TxCharCPtrW filename, int mode) = 0;
	virtual void CloseRaw(void* handle) = 0;
	virtual void LoadRaw(void* handle) = 0;
	virtual void SaveRaw(void* handle) const = 0;
};

}

#pragma pack(pop)

#endif
