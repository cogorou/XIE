/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXFILEPLUGINBMP_H_INCLUDED_
#define _CXFILEPLUGINBMP_H_INCLUDED_

#include "api_file.h"
#include "Core/CxModule.h"
#include "Core/TxImageSize.h"

// ======================================================================
#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace File
{

class CxFilePluginBmp : public CxModule
{
public:
	CxFilePluginBmp();
	virtual ~CxFilePluginBmp();

public:
	virtual void* OpenA(TxCharCPtrA filename, int mode);
	virtual void* OpenW(TxCharCPtrW filename, int mode);
	virtual void Close(void* handle);

	virtual ExStatus Check	(TxImageSize* image_size, void* handle, bool unpack);
	virtual ExStatus Load	(HxModule hdst, void* handle, bool unpack);
	virtual ExStatus Save	(HxModule hsrc, void* handle);

public:
	static bool CreateBitmapInfo( int width, int height, int bpp, BITMAPINFO** ppBmpInfo, unsigned int* puiBmpInfoSize );
	static void FreeBitmapInfo( BITMAPINFO* bmpInfo );
};

}
}

#pragma pack(pop)

#endif
