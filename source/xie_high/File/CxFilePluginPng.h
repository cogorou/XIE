/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXFILEPLUGINPNG_H_INCLUDED_
#define _CXFILEPLUGINPNG_H_INCLUDED_

#include "xie_high.h"
#include "Core/IxFilePlugin.h"
#include "Core/CxModule.h"

#include <stdio.h>
#include <setjmp.h>

#define PNG_SKIP_SETJMP_CHECK
extern "C" {
#include "zlib.h"
#include "png.h"
#if PNG_LIBPNG_VER_MAJOR >= 1
	#if PNG_LIBPNG_VER_MINOR >= 6
	#include "pngstruct.h"
	#include "pnginfo.h"
	#endif
#endif
}

#ifndef png_jmpbuf
#  define png_jmpbuf(png_ptr) ((png_ptr)->png_jmpbuf)
#endif

// ======================================================================
#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace File
{

class CxFilePluginPng : public CxModule
	, public IxFilePluginPng
{
public:
	CxFilePluginPng();
	virtual ~CxFilePluginPng();

public:
	virtual void* OpenA(TxCharCPtrA filename, int mode);
	virtual void* OpenW(TxCharCPtrW filename, int mode);
	virtual void Close(void* handle);

	virtual ExStatus Check	(TxImageSize* image_size, void* handle, bool unpack);
	virtual ExStatus Load	(HxModule hdst, void* handle, bool unpack);
	virtual ExStatus Save	(HxModule hsrc, void* handle, int level);

private:
	// ======================================================================
	template<class TD, class TS> static inline void Buffer_Copy_pp(TD* dst, const TS* src, int length)
	{
		for(int i=0 ; i<length ; i++)
		{
			dst[i].R = src[i].R;
			dst[i].G = src[i].G;
			dst[i].B = src[i].B;
		}
	}

	// ======================================================================
	template<class TD, class TS> static inline void Buffer_Copy_up(TD* dst0, TD* dst1, TD* dst2, const TS* src, int length)
	{
		for(int i=0 ; i<length ; i++)
		{
			dst0[i] = (TD)src[i].R;
			dst1[i] = (TD)src[i].G;
			dst2[i] = (TD)src[i].B;
		}
	}

	// ======================================================================
	template<class TD, class TS> static inline void Buffer_Copy_pu(TD* dst, const TS* src0, const TS* src1, const TS* src2, int length)
	{
		for(int i=0 ; i<length ; i++)
		{
			dst[i].R = src0[i];
			dst[i].G = src1[i];
			dst[i].B = src2[i];
		}
	}

	// ======================================================================
	template<class TD, class TS> static inline void Buffer_Copy_qu(TD* dst, const TS* src0, const TS* src1, const TS* src2, const TS* src3, int length)
	{
		for(int i=0 ; i<length ; i++)
		{
			dst[i].R = src0[i];
			dst[i].G = src1[i];
			dst[i].B = src2[i];
			dst[i].A = src3[i];
		}
	}
};

}
}

#pragma pack(pop)

#endif
