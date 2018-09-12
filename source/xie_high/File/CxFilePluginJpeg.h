/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXFILEPLUGINJPEG_H_INCLUDED_
#define _CXFILEPLUGINJPEG_H_INCLUDED_

#include "xie_high.h"
#include "Core/IxFilePlugin.h"
#include "Core/CxModule.h"
#include "Core/CxArray.h"
#include "Core/CxImage.h"

#include <stdio.h>
#include <setjmp.h>

#undef FALSE
#undef TRUE

extern "C" {

#define XMD_H
#include "jpeglib.h"

}

// ======================================================================
struct TxJpegErrorManager
{
	struct jpeg_error_mgr	pub;
	jmp_buf					setjmp_buffer;
};

// ======================================================================
void fnPRV_Jpeg_ErrorHandler (j_common_ptr cinfo);

// ======================================================================
void fnPRV_Jpeg_OutputMessage( j_common_ptr cinfo );

// ======================================================================
#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace File
{

class CxFilePluginJpeg : public CxModule
	, public IxFilePluginJpeg
{
public:
	CxFilePluginJpeg();
	virtual ~CxFilePluginJpeg();

public:
	virtual void* OpenA(TxCharCPtrA filename, int mode);
	virtual void* OpenW(TxCharCPtrW filename, int mode);
	virtual void Close(void* handle);

	virtual ExStatus Check	(TxImageSize* image_size, void* handle, bool unpack);
	virtual ExStatus Load	(HxModule hdst, void* handle, bool unpack);
	virtual ExStatus Save	(HxModule hsrc, void* handle, int quality);

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
};

}
}

#pragma pack(pop)

#endif
