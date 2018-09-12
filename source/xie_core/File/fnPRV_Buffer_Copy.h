/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _FNPRV_BUFFER_COPY_H_INCLUDED_
#define _FNPRV_BUFFER_COPY_H_INCLUDED_

#include "xie_core.h"
#include "Core/Axi.h"

namespace xie
{
namespace File
{

// ======================================================================
template<class TD, class TS> static inline void fnPRV_Buffer_Copy_uu(TD* dst, const TS* src, int length)
{
	for(int i=0 ; i<length ; i++)
	{
		*dst = (TD)(*src);
	}
}

// ======================================================================
template<class TD, class TS> static inline void fnPRV_Buffer_Copy_pu(TD* dst, const TS* src0, const TS* src1, const TS* src2, int length)
{
	for(int i=0 ; i<length ; i++)
	{
		dst[i].R = src0[i];
		dst[i].G = src1[i];
		dst[i].B = src2[i];
	}
}

// ======================================================================
template<class TD, class TS> static inline void fnPRV_Buffer_Copy_qu(TD* dst, const TS* src0, const TS* src1, const TS* src2, const TS* src3, int length)
{
	for(int i=0 ; i<length ; i++)
	{
		dst[i].R = src0[i];
		dst[i].G = src1[i];
		dst[i].B = src2[i];
		dst[i].A = src3[i];
	}
}

// ======================================================================
template<class TD, class TS> static inline void fnPRV_Buffer_Copy_pp(TD* dst, const TS* src, int length)
{
	for(int i=0 ; i<length ; i++)
	{
		dst[i].R = src[i].R;
		dst[i].G = src[i].G;
		dst[i].B = src[i].B;
	}
}

// ======================================================================
template<class TD, class TS> static inline void fnPRV_Buffer_Copy_qq(TD* dst, const TS* src, int length)
{
	for(int i=0 ; i<length ; i++)
	{
		dst[i].R = src[i].R;
		dst[i].G = src[i].G;
		dst[i].B = src[i].B;
		dst[i].A = src[i].A;
	}
}

// ======================================================================
template<class TD, class TS> static inline void fnPRV_Buffer_Copy_up(TD* dst0, TD* dst1, TD* dst2, const TS* src, int length)
{
	for(int i=0 ; i<length ; i++)
	{
		dst0[i] = (TD)src[i].R;
		dst1[i] = (TD)src[i].G;
		dst2[i] = (TD)src[i].B;
	}
}

// ======================================================================
template<class TD, class TS> static inline void fnPRV_Buffer_Copy_u1(TD* dst, const TS* src, int length, TD lower, TD upper)
{
	for(int i=0 ; i<length ; i++)
	{
		dst[i] = (((src[i >> 3]) & ((TS)0x1 << (7-(i%8)))) == 0) ? lower : upper;
	}
}

// ======================================================================
template<class TD, class TS> static inline void fnPRV_Buffer_Copy_u4(TD* dst, const TS* src, int length, TD lower, TD upper)
{
	for(int i=0 ; i<length ; i++)
	{
		dst[i] = (((src[i >> 5]) & ((TS)0x1 << (31-(i%32)))) == 0) ? lower : upper;
	}
}

// ======================================================================
template<class TD, class TS> static inline void fnPRV_Buffer_Copy_1u(TD* dst, const TS* src, int length, TS threshold)
{
	for(int i=0 ; i<length ; i++)
	{
		if (src[i] < threshold)
			dst[i >> 3] &= ~((TS)0x1 << (7-(i%8)));
		else
			dst[i >> 3] |=  ((TS)0x1 << (7-(i%8)));
	}
}

// ======================================================================
template<class TD, class TS> static inline void fnPRV_Buffer_Copy_4u(TD* dst, const TS* src, int length, TS threshold)
{
	for(int i=0 ; i<length ; i++)
	{
		if (src[i] < threshold)
			dst[i >> 5] &= ~((TS)0x1 << (31-(i%32)));
		else
			dst[i >> 5] |=  ((TS)0x1 << (31-(i%32)));
	}
}

}	// File
}	// xie

#endif
