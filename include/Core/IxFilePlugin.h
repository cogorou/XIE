/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _IXFILEPLUGIN_H_INCLUDED_
#define _IXFILEPLUGIN_H_INCLUDED_

#include "xie_core.h"
#include "Core/TxImageSize.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

struct IxFilePluginJpeg
{
	virtual void* OpenA(TxCharCPtrA filename, int mode) = 0;
	virtual void* OpenW(TxCharCPtrW filename, int mode) = 0;
	virtual void Close(void* handle) = 0;
	virtual ExStatus Check	(TxImageSize* image_size, void* handle, bool unpack) = 0;
	virtual ExStatus Load	(HxModule hdst, void* handle, bool unpack) = 0;
	virtual ExStatus Save	(HxModule hsrc, void* handle, int quality) = 0;
};

struct IxFilePluginPng
{
	virtual void* OpenA(TxCharCPtrA filename, int mode) = 0;
	virtual void* OpenW(TxCharCPtrW filename, int mode) = 0;
	virtual void Close(void* handle) = 0;
	virtual ExStatus Check	(TxImageSize* image_size, void* handle, bool unpack) = 0;
	virtual ExStatus Load	(HxModule hdst, void* handle, bool unpack) = 0;
	virtual ExStatus Save	(HxModule hsrc, void* handle, int level) = 0;
};

struct IxFilePluginTiff
{
	virtual void* OpenA(TxCharCPtrA filename, int mode) = 0;
	virtual void* OpenW(TxCharCPtrW filename, int mode) = 0;
	virtual void Close(void* handle) = 0;
	virtual ExStatus Check	(TxImageSize* image_size, void* handle, bool unpack) = 0;
	virtual ExStatus Load	(HxModule hdst, void* handle, bool unpack) = 0;
	virtual ExStatus Save	(HxModule hsrc, void* handle, int level) = 0;
};
}

#pragma pack(pop)

#endif
