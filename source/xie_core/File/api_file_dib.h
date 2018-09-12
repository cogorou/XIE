/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _API_FILE_DIB_H_INCLUDED_
#define _API_FILE_DIB_H_INCLUDED_

#include <math.h>
#include <stdio.h>

namespace xie
{
namespace File
{

// ======================================================================
static inline int fnPRV_DIB_CalcStride(int width, int bpp)
{
	return ((width * bpp + 31) >> 5) << 2;	// 4bytes packing
}

// ======================================================================
static inline RGBQUAD fnPRV_DIB_FromRGBA(unsigned char R, unsigned char G, unsigned char B, unsigned char A)
{
	RGBQUAD result;
	result.rgbRed	= R;
	result.rgbGreen	= G;
	result.rgbBlue	= B;
	result.rgbReserved = A;
	return result;
}

// ======================================================================
template<class TS> static inline void fnPRV_DIB_Load_1g(CxImage& src, const void* pvimg, const CxArray& palette, int direction)
{
	typedef unsigned char	TB;
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 1);

	if (src_stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			_src[x] = (((_buf[x >> 3]) & ((TB)0x1 << (7-(x%8)))) == 0) ? 0x00 : 0x01;
		}

		src_addr += src_stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_DIB_Load_4g(CxImage& src, const void* pvimg, const CxArray& palette, int direction)
{
	typedef unsigned char	TB;
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 4);

	if (src_stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			int tmp = (_buf[x>>1] >> ((x%2) ? 0 : 4)) & 0x0F;
			const TxBGR8x4* color = (const TxBGR8x4*)palette[tmp];
			_src[x] = color->G;
		}

		src_addr += src_stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_DIB_Load_4p(CxImage& src, const void* pvimg, const CxArray& palette, int direction)
{
	typedef unsigned char	TB;
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 4);

	if (src_stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			int tmp = (_buf[x>>1] >> ((x%2) ? 0 : 4)) & 0x0F;
			const TxBGR8x4* color = (const TxBGR8x4*)palette[tmp];
			_src[x].R = color->R;
			_src[x].G = color->G;
			_src[x].B = color->B;
		}

		src_addr += src_stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_DIB_Load_4u(CxImage& src, const void* pvimg, const CxArray& palette, int direction)
{
	typedef unsigned char	TB;
	unsigned char*	src0_addr = (unsigned char*)src[0];
	unsigned char*	src1_addr = (unsigned char*)src[1];
	unsigned char*	src2_addr = (unsigned char*)src[2];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 4);

	if (src_stride < 0)
	{
		TxIntPtr bottom = (TxIntPtr)src.Stride() * (src.Height() - 1);
		src0_addr += bottom;
		src1_addr += bottom;
		src2_addr += bottom;
	}

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src0 = (TS*)src0_addr;
		TS*	_src1 = (TS*)src1_addr;
		TS*	_src2 = (TS*)src2_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			int tmp = (_buf[x>>1] >> ((x%2) ? 0 : 4)) & 0x0F;
			const TxBGR8x4* color = (const TxBGR8x4*)palette[tmp];
			_src0[x] = color->R;
			_src1[x] = color->G;
			_src2[x] = color->B;
		}

		src0_addr += src_stride;
		src1_addr += src_stride;
		src2_addr += src_stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_DIB_Load_8g(CxImage& src, const void* pvimg, const CxArray& palette, int direction)
{
	typedef unsigned char	TB;
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 8);

	if (src_stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			const TxBGR8x4* color = (const TxBGR8x4*)palette[_buf[x]];
			_src[x] = color->G;
		}

		src_addr += src_stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_DIB_Load_8p(CxImage& src, const void* pvimg, const CxArray& palette, int direction)
{
	typedef unsigned char	TB;
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 8);

	if (src_stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			const TxBGR8x4* color = (const TxBGR8x4*)palette[_buf[x]];
			_src[x].R = color->R;
			_src[x].G = color->G;
			_src[x].B = color->B;
		}

		src_addr += src_stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_DIB_Load_8u(CxImage& src, const void* pvimg, const CxArray& palette, int direction)
{
	typedef unsigned char	TB;
	unsigned char*	src0_addr = (unsigned char*)src[0];
	unsigned char*	src1_addr = (unsigned char*)src[1];
	unsigned char*	src2_addr = (unsigned char*)src[2];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 8);

	if (src_stride < 0)
	{
		TxIntPtr bottom = (TxIntPtr)src.Stride() * (src.Height() - 1);
		src0_addr += bottom;
		src1_addr += bottom;
		src2_addr += bottom;
	}

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src0 = (TS*)src0_addr;
		TS*	_src1 = (TS*)src1_addr;
		TS*	_src2 = (TS*)src2_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			const TxBGR8x4* color = (const TxBGR8x4*)palette[_buf[x]];
			_src0[x] = color->R;
			_src1[x] = color->G;
			_src2[x] = color->B;
		}

		src0_addr += src_stride;
		src1_addr += src_stride;
		src2_addr += src_stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_DIB_Load_16g(CxImage& src, const void* pvimg, const CxArray& palette, int direction, float scale)
{
	typedef unsigned short	TB;
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 16);

	if (src_stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			_src[x] = (TS)(_buf[x] * scale + 0.5f);
		}

		src_addr += src_stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_DIB_Load_16p(CxImage& src, const void* pvimg, const CxArray& palette, int direction, const unsigned int* mask, const int* shift)
{
	typedef unsigned short	TB;
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 16);

	if (src_stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			_src[x].R = (_buf[x] && mask[0]) >> shift[0];
			_src[x].G = (_buf[x] && mask[1]) >> shift[1];
			_src[x].B = (_buf[x] && mask[2]) << shift[2];
		}

		src_addr += src_stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_DIB_Load_16u(CxImage& src, const void* pvimg, const CxArray& palette, int direction, const unsigned int* mask, const int* shift)
{
	typedef unsigned short	TB;
	unsigned char*	src0_addr = (unsigned char*)src[0];
	unsigned char*	src1_addr = (unsigned char*)src[1];
	unsigned char*	src2_addr = (unsigned char*)src[2];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 16);

	if (src_stride < 0)
	{
		TxIntPtr bottom = (TxIntPtr)src.Stride() * (src.Height() - 1);
		src0_addr += bottom;
		src1_addr += bottom;
		src2_addr += bottom;
	}

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src0 = (TS*)src0_addr;
		TS*	_src1 = (TS*)src1_addr;
		TS*	_src2 = (TS*)src2_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			_src0[x] = (_buf[x] && mask[0]) >> shift[0];
			_src1[x] = (_buf[x] && mask[1]) >> shift[1];
			_src2[x] = (_buf[x] && mask[2]) << shift[2];
		}

		src0_addr += src_stride;
		src1_addr += src_stride;
		src2_addr += src_stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_DIB_Load_24p(CxImage& src, const void* pvimg, const CxArray& palette, int direction)
{
	typedef TxBGR8x3	TB;
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 24);

	if (src_stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			_src[x].R = _buf[x].R;
			_src[x].G = _buf[x].G;
			_src[x].B = _buf[x].B;
		}

		src_addr += src_stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_DIB_Load_32p(CxImage& src, const void* pvimg, const CxArray& palette, int direction)
{
	typedef TxBGR8x4	TB;
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 32);

	if (src_stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			_src[x].R = _buf[x].R;
			_src[x].G = _buf[x].G;
			_src[x].B = _buf[x].B;
			_src[x].A = _buf[x].A;
		}

		src_addr += src_stride;
	}
}

// ======================================================================
template<class TB, class TS> static inline void fnPRV_DIB_Load_pu(CxImage& src, const void* pvimg, const CxArray& palette, int direction)
{
	unsigned char*	src0_addr = (unsigned char*)src[0];
	unsigned char*	src1_addr = (unsigned char*)src[1];
	unsigned char*	src2_addr = (unsigned char*)src[2];
	TxIntPtr		src_stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	TxIntPtr		img_stride = fnPRV_DIB_CalcStride(width, 32);

	if (src_stride < 0)
	{
		TxIntPtr bottom = (TxIntPtr)src.Stride() * (src.Height() - 1);
		src0_addr += bottom;
		src1_addr += bottom;
		src2_addr += bottom;
	}

	for(int y=0 ; y<height ; y++)
	{
		TS*	_src0 = (TS*)src0_addr;
		TS*	_src1 = (TS*)src1_addr;
		TS*	_src2 = (TS*)src2_addr;
		TB*	_buf = (TB*)(static_cast<const char*>(pvimg) + img_stride * y);

		for(int x=0 ; x<width ; x++)
		{
			_src0[x] = _buf[x].R;
			_src1[x] = _buf[x].G;
			_src2[x] = _buf[x].B;
		}

		src0_addr += src_stride;
		src1_addr += src_stride;
		src2_addr += src_stride;
	}
}

}	// File
}	// xie

#endif
