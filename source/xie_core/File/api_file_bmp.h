/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _API_FILE_BMP_H_INCLUDED_
#define _API_FILE_BMP_H_INCLUDED_

#include <math.h>
#include <stdio.h>

#define BM_HEADER_MARKER   ((unsigned short) ('M' << 8) | 'B')

const unsigned int	_BITMAPFILEHEADER_SIZE = 14;

namespace xie
{
namespace File
{

// ======================================================================
static inline int fnPRV_Bmp_CalcStride(int width, int bpp)
{
	return ((width * bpp + 31) >> 5) << 2;	// 4bytes packing
}

// ======================================================================
template<class TS> static inline void fnPRV_Load_Bmp_1g(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src)
{
	typedef unsigned char	TB;
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;
	unsigned int	value0 = 0x00;
	unsigned int	value1 = 0x01;

	if (palette.Length() == 2)
	{
		TxBGR8x4* pv = (TxBGR8x4*)palette.Address();

		// Measures against strange palettes that are inverted
		if ((pv[0].R != 0 || pv[0].G != 0 || pv[0].B != 0) &&
			(pv[1].R == 0 && pv[1].G == 0 && pv[1].B == 0))
		{
			value0 = 0x01;
			value1 = 0x00;
		}
	}

	if (stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			_src[x] = (((_buf[x >> 3]) & ((TB)0x1 << (7-(x%8)))) == 0) ? value0 : value1;
		}

		src_addr += stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_Load_Bmp_4g(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src)
{
	typedef unsigned char	TB;
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;

	if (stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			int tmp = (_buf[x>>1] >> ((x%2) ? 0 : 4)) & 0x0F;
			const TxBGR8x4* color = (const TxBGR8x4*)palette[tmp];
			_src[x] = color->G;
		}

		src_addr += stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_Load_Bmp_4p(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src)
{
	typedef unsigned char	TB;
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;

	if (stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			int tmp = (_buf[x>>1] >> ((x%2) ? 0 : 4)) & 0x0F;
			const TxBGR8x4* color = (const TxBGR8x4*)palette[tmp];
			_src[x].R = color->R;
			_src[x].G = color->G;
			_src[x].B = color->B;
		}

		src_addr += stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_Load_Bmp_4u(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src)
{
	typedef unsigned char	TB;
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src0_addr = (unsigned char*)src[0];
	unsigned char*	src1_addr = (unsigned char*)src[1];
	unsigned char*	src2_addr = (unsigned char*)src[2];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;

	if (stride < 0)
	{
		TxIntPtr bottom = (TxIntPtr)src.Stride() * (src.Height() - 1);
		src0_addr += bottom;
		src1_addr += bottom;
		src2_addr += bottom;
	}

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src0 = (TS*)src0_addr;
		TS*	_src1 = (TS*)src1_addr;
		TS*	_src2 = (TS*)src2_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			int tmp = (_buf[x>>1] >> ((x%2) ? 0 : 4)) & 0x0F;
			const TxBGR8x4* color = (const TxBGR8x4*)palette[tmp];
			_src0[x] = color->R;
			_src1[x] = color->G;
			_src2[x] = color->B;
		}

		src0_addr += stride;
		src1_addr += stride;
		src2_addr += stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_Load_Bmp_8g(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src)
{
	typedef unsigned char	TB;
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;

	if (stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			const TxBGR8x4* color = (const TxBGR8x4*)palette[_buf[x]];
			_src[x] = color->G;
		}

		src_addr += stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_Load_Bmp_8p(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src)
{
	typedef unsigned char	TB;
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;

	if (stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			const TxBGR8x4* color = (const TxBGR8x4*)palette[_buf[x]];
			_src[x].R = color->R;
			_src[x].G = color->G;
			_src[x].B = color->B;
		}

		src_addr += stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_Load_Bmp_8u(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src)
{
	typedef unsigned char	TB;
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src0_addr = (unsigned char*)src[0];
	unsigned char*	src1_addr = (unsigned char*)src[1];
	unsigned char*	src2_addr = (unsigned char*)src[2];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;

	if (stride < 0)
	{
		TxIntPtr bottom = (TxIntPtr)src.Stride() * (src.Height() - 1);
		src0_addr += bottom;
		src1_addr += bottom;
		src2_addr += bottom;
	}

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src0 = (TS*)src0_addr;
		TS*	_src1 = (TS*)src1_addr;
		TS*	_src2 = (TS*)src2_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			const TxBGR8x4* color = (const TxBGR8x4*)palette[_buf[x]];
			_src0[x] = color->R;
			_src1[x] = color->G;
			_src2[x] = color->B;
		}

		src0_addr += stride;
		src1_addr += stride;
		src2_addr += stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_Load_Bmp_16g(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src, float scale)
{
	typedef unsigned short	TB;
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;

	if (stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			_src[x] = (TS)(_buf[x] * scale + 0.5f);
		}

		src_addr += stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_Load_Bmp_16p(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src, const unsigned int* mask, const int* shift)
{
	typedef unsigned short	TB;
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;

	if (stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			_src[x].R = (_buf[x] && mask[0]) >> shift[0];
			_src[x].G = (_buf[x] && mask[1]) >> shift[1];
			_src[x].B = (_buf[x] && mask[2]) << shift[2];
		}

		src_addr += stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_Load_Bmp_16u(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src, const unsigned int* mask, const int* shift)
{
	typedef unsigned short	TB;
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src0_addr = (unsigned char*)src[0];
	unsigned char*	src1_addr = (unsigned char*)src[1];
	unsigned char*	src2_addr = (unsigned char*)src[2];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;

	if (stride < 0)
	{
		TxIntPtr bottom = (TxIntPtr)src.Stride() * (src.Height() - 1);
		src0_addr += bottom;
		src1_addr += bottom;
		src2_addr += bottom;
	}

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src0 = (TS*)src0_addr;
		TS*	_src1 = (TS*)src1_addr;
		TS*	_src2 = (TS*)src2_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			_src0[x] = (_buf[x] && mask[0]) >> shift[0];
			_src1[x] = (_buf[x] && mask[1]) >> shift[1];
			_src2[x] = (_buf[x] && mask[2]) << shift[2];
		}

		src0_addr += stride;
		src1_addr += stride;
		src2_addr += stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_Load_Bmp_24p(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src)
{
	typedef TxBGR8x3	TB;
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;

	if (stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			_src[x].R = _buf[x].R;
			_src[x].G = _buf[x].G;
			_src[x].B = _buf[x].B;
		}

		src_addr += stride;
	}
}

// ======================================================================
template<class TS> static inline void fnPRV_Load_Bmp_32p(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src)
{
	typedef TxBGR8x4	TB;
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src_addr = (unsigned char*)src[0];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;

	if (stride < 0)
		src_addr += (TxIntPtr)src.Stride() * (src.Height() - 1);

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src = (TS*)src_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			_src[x].R = _buf[x].R;
			_src[x].G = _buf[x].G;
			_src[x].B = _buf[x].B;
		}

		src_addr += stride;
	}
}

// ======================================================================
template<class TB, class TS> static inline void fnPRV_Load_Bmp_pu(FILE* stream, int direction, const CxArray& palette, CxArray& buffer, CxImage& src)
{
	TB*				buf_addr = (TB*)buffer[0];
	int				buf_size = buffer.Length() * buffer.Model().Size();
	unsigned char*	src0_addr = (unsigned char*)src[0];
	unsigned char*	src1_addr = (unsigned char*)src[1];
	unsigned char*	src2_addr = (unsigned char*)src[2];
	TxIntPtr		stride = direction * src.Stride();
	int				width = src.Width();
	int				height = src.Height();
	size_t			uiReadSize;

	if (stride < 0)
	{
		TxIntPtr bottom = (TxIntPtr)src.Stride() * (src.Height() - 1);
		src0_addr += bottom;
		src1_addr += bottom;
		src2_addr += bottom;
	}

	for(int y=0 ; y<height ; y++)
	{
		uiReadSize = fread(buf_addr, buf_size, 1, stream);
		if (uiReadSize != 1)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		TS*	_src0 = (TS*)src0_addr;
		TS*	_src1 = (TS*)src1_addr;
		TS*	_src2 = (TS*)src2_addr;
		TB*	_buf = (TB*)buf_addr;

		for(int x=0 ; x<width ; x++)
		{
			_src0[x] = _buf[x].R;
			_src1[x] = _buf[x].G;
			_src2[x] = _buf[x].B;
		}

		src0_addr += stride;
		src1_addr += stride;
		src2_addr += stride;
	}
}

}	// File
}	// xie

#endif
