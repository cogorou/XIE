/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXFILEPLUGINTIFF_H_INCLUDED_
#define _CXFILEPLUGINTIFF_H_INCLUDED_

#include "xie_high.h"
#include "Core/xie_core_math.h"
#include "Core/IxFilePlugin.h"
#include "Core/CxModule.h"
#include "Core/CxImage.h"
#include "Core/CxString.h"

#include <stdio.h>
#include "tiffio.h"

// ======================================================================
#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace File
{

class CxFilePluginTiff : public CxModule
	, public IxFilePluginTiff
{
public:
	CxFilePluginTiff();
	virtual ~CxFilePluginTiff();

public:
	virtual void* OpenA(TxCharCPtrA filename, int mode);
	virtual void* OpenW(TxCharCPtrW filename, int mode);
	virtual void Close(void* handle);

	virtual ExStatus Check	(TxImageSize* image_size, void* handle, bool unpack);
	virtual ExStatus Load	(HxModule hdst, void* handle, bool unpack);
	virtual ExStatus Save	(HxModule hsrc, void* handle, int level);

private:
	void LoadDefault(CxImage& dst, TIFF* tif, bool unpack);
	void LoadScanline(CxImage& dst, TIFF* tif, bool unpack);
	void LoadPalette(CxImage& dst, TIFF* tif, bool unpack);
	CxStringA ToString_PHOTOMETRIC(int value);
	CxStringA ToString_SAMPLEFORMAT(int value);
	CxStringA ToString_COMPRESSION(int value);
	ExType ToExType(int bits_per_sample, int sample_format);

private:
	void Save_B(const CxImage& src, TxCharCPtrA filename, int level);
	void Save_U(const CxImage& src, TxCharCPtrA filename, int level);

private:
	// ======================================================================
	template<class TE> static inline void Buffer_CmykToRgb_pp(void* dst, void* src, int width, int dst_pack, int src_pack, double max_value)
	{
		TE* _dst = static_cast<TE*>(dst);
		TE* _src = static_cast<TE*>(src);
		for(int x=0 ; x<width ; x++)
		{
			_dst[0] = saturate_cast<TE>(max_value - (_src[0] + _src[3]));	// R = max - (C + K)
			_dst[1] = saturate_cast<TE>(max_value - (_src[1] + _src[3]));	// G = max - (M + K)
			_dst[2] = saturate_cast<TE>(max_value - (_src[2] + _src[3]));	// B = max - (Y + K)
			_dst += dst_pack;
			_src += src_pack;
		}
	}

	// ======================================================================
	template<class TE> static inline void Buffer_Copy_pp(void* dst, void* src, int width, int dst_pack, int src_pack)
	{
		TE* _dst = static_cast<TE*>(dst);
		TE* _src = static_cast<TE*>(src);
		int min_pack = Axi::Min(dst_pack, src_pack);
		for(int x=0 ; x<width ; x++)
		{
			for(int k=0 ; k<min_pack ; k++)
				_dst[k] = _src[k];
			_dst += dst_pack;
			_src += src_pack;
		}
	}

	// ======================================================================
	template<class TE> static inline void Buffer_Copy_up(void* dst, void* src, int width, int dst_ch, int src_pack)
	{
		TE* _dst = static_cast<TE*>(dst);
		TE* _src = static_cast<TE*>(src);
		for(int x=0 ; x<width ; x++)
		{
			_dst[x] = _src[x * src_pack + dst_ch];
		}
	}

	// ======================================================================
	template<class TE> static inline void Buffer_Copy_pu(void* dst, void* src, int width, int dst_ch, int dst_pack)
	{
		TE* _dst = static_cast<TE*>(dst);
		TE* _src = static_cast<TE*>(src);
		for(int x=0 ; x<width ; x++)
		{
			_dst[x * dst_pack + dst_ch] = _src[x];
		}
	}

	// ======================================================================
	template<class TE> static inline void Buffer_Copy_uu(void* dst, void* src, int width, int dst_pack, int src_pack)
	{
		TE* _dst = static_cast<TE*>(dst);
		TE* _src = static_cast<TE*>(src);
		for(int x=0 ; x<width ; x++)
		{
			_dst[0] = _src[0];
			_dst += dst_pack;
			_src += src_pack;
		}
	}

	// ======================================================================
	template<class TD, class TS> static inline void Buffer_Copy_ab(void* dst, void* src, int width, TxRangeI value)
	{
		TD* _dst = static_cast<TD*>(dst);
		TS* _src = static_cast<TS*>(src);
		int bits = sizeof(TS) * 8;
		for (int x = 0; x<width; x++)
		{
			TS mask = (0x1 << ((bits - 1) - (x % bits)));
			_dst[x] = ((_src[x / bits] & mask) == 0)
				? value.Lower
				: value.Upper;
		}
	}

	// ======================================================================
	template<class TD, class TS> static inline void Buffer_Copy_ba(void* dst, void* src, int width)
	{
		TD* _dst = static_cast<TD*>(dst);
		TS* _src = static_cast<TS*>(src);
		int bits = sizeof(TD) * 8;
		for (int x = 0; x<width; x++)
		{
			TD mask = (0x1 << ((bits - 1) - (x % bits)));
			if (_src[x] == 0)
				_dst[x / bits] &= (~mask);
			else
				_dst[x / bits] |= mask;
		}
	}

	// ======================================================================
	template<class TD, class TS, class TP> static inline void Palette_Copy_p1(void* dst, void* src, TP* colormapR, TP* colormapG, TP* colormapB, int width, int pack)
	{
		TD* _dst = static_cast<TD*>(dst);
		TS* _src = static_cast<TS*>(src);
		int bits = sizeof(TS) * 8;
		for(int x=0 ; x<width ; x++)
		{
			TS index = ((_src[x / bits] & (0x1 << ((bits - 1) - (x % bits)))) == 0) ? 0 : 1;
			_dst[x * pack + 0] = (TD)colormapR[index];
			_dst[x * pack + 1] = (TD)colormapG[index];
			_dst[x * pack + 2] = (TD)colormapB[index];
		}
	}

	// ======================================================================
	template<class TD, class TS, class TP> static inline void Palette_Copy_u1(void* dst0, void* dst1, void* dst2, void* src, TP* colormapR, TP* colormapG, TP* colormapB, int width, int pack)
	{
		TD* _dst0 = static_cast<TD*>(dst0);
		TD* _dst1 = static_cast<TD*>(dst1);
		TD* _dst2 = static_cast<TD*>(dst2);
		TS* _src = static_cast<TS*>(src);
		int bits = sizeof(TS) * 8;
		for(int x=0 ; x<width ; x++)
		{
			TS index = ((_src[x / bits] & (0x1 << ((bits - 1) - (x % bits)))) == 0) ? 0 : 1;
			_dst0[x] = (TD)colormapR[index];
			_dst1[x] = (TD)colormapG[index];
			_dst2[x] = (TD)colormapB[index];
		}
	}

	// ======================================================================
	template<class TD, class TS, class TP> static inline void Palette_Copy_p2(void* dst, void* src, TP* colormapR, TP* colormapG, TP* colormapB, int width, int pack)
	{
		TD* _dst = static_cast<TD*>(dst);
		TS* _src = static_cast<TS*>(src);
		for(int x=0 ; x<width ; x++)
		{
			TS index = 0;
			switch(x % 4)
			{
			case 0: index = (_src[x / 4] >> 0) & 0x03; break;
			case 1: index = (_src[x / 4] >> 2) & 0x03; break;
			case 2: index = (_src[x / 4] >> 4) & 0x03; break;
			case 3: index = (_src[x / 4] >> 6) & 0x03; break;
			}
			_dst[x * pack + 0] = (TD)colormapR[index];
			_dst[x * pack + 1] = (TD)colormapG[index];
			_dst[x * pack + 2] = (TD)colormapB[index];
		}
	}

	// ======================================================================
	template<class TD, class TS, class TP> static inline void Palette_Copy_u2(void* dst0, void* dst1, void* dst2, void* src, TP* colormapR, TP* colormapG, TP* colormapB, int width, int pack)
	{
		TD* _dst0 = static_cast<TD*>(dst0);
		TD* _dst1 = static_cast<TD*>(dst1);
		TD* _dst2 = static_cast<TD*>(dst2);
		TS* _src = static_cast<TS*>(src);
		for(int x=0 ; x<width ; x++)
		{
			TS index = 0;
			switch(x % 4)
			{
			case 0: index = (_src[x / 4] >> 0) & 0x03; break;
			case 1: index = (_src[x / 4] >> 2) & 0x03; break;
			case 2: index = (_src[x / 4] >> 4) & 0x03; break;
			case 3: index = (_src[x / 4] >> 6) & 0x03; break;
			}
			_dst0[x] = (TD)colormapR[index];
			_dst1[x] = (TD)colormapG[index];
			_dst2[x] = (TD)colormapB[index];
		}
	}

	// ======================================================================
	template<class TD, class TS, class TP> static inline void Palette_Copy_p4(void* dst, void* src, TP* colormapR, TP* colormapG, TP* colormapB, int width, int pack)
	{
		TD* _dst = static_cast<TD*>(dst);
		TS* _src = static_cast<TS*>(src);
		for(int x=0 ; x<width ; x++)
		{
			TS index = ((x % 2) == 1)
				? (_src[x / 2] >> 0) & 0x0F
				: (_src[x / 2] >> 4) & 0x0F;

			_dst[x * pack + 0] = (TD)colormapR[index];
			_dst[x * pack + 1] = (TD)colormapG[index];
			_dst[x * pack + 2] = (TD)colormapB[index];
		}
	}

	// ======================================================================
	template<class TD, class TS, class TP> static inline void Palette_Copy_u4(void* dst0, void* dst1, void* dst2, void* src, TP* colormapR, TP* colormapG, TP* colormapB, int width, int pack)
	{
		TD* _dst0 = static_cast<TD*>(dst0);
		TD* _dst1 = static_cast<TD*>(dst1);
		TD* _dst2 = static_cast<TD*>(dst2);
		TS* _src = static_cast<TS*>(src);
		for(int x=0 ; x<width ; x++)
		{
			TS index = ((x % 2) == 1)
				? (_src[x / 2] >> 0) & 0x0F
				: (_src[x / 2] >> 4) & 0x0F;

			_dst0[x] = (TD)colormapR[index];
			_dst1[x] = (TD)colormapG[index];
			_dst2[x] = (TD)colormapB[index];
		}
	}

	// ======================================================================
	template<class TD, class TS, class TP> static inline void Palette_Copy_p8(void* dst, void* src, TP* colormapR, TP* colormapG, TP* colormapB, int width, int pack)
	{
		TD* _dst = static_cast<TD*>(dst);
		TS* _src = static_cast<TS*>(src);
		for(int x=0 ; x<width ; x++)
		{
			_dst[x * pack + 0] = (TD)colormapR[_src[x]];
			_dst[x * pack + 1] = (TD)colormapG[_src[x]];
			_dst[x * pack + 2] = (TD)colormapB[_src[x]];
		}
	}

	// ======================================================================
	template<class TD, class TS, class TP> static inline void Palette_Copy_u8(void* dst0, void* dst1, void* dst2, void* src, TP* colormapR, TP* colormapG, TP* colormapB, int width, int pack)
	{
		TD* _dst0 = static_cast<TD*>(dst0);
		TD* _dst1 = static_cast<TD*>(dst1);
		TD* _dst2 = static_cast<TD*>(dst2);
		TS* _src = static_cast<TS*>(src);
		for(int x=0 ; x<width ; x++)
		{
			_dst0[x] = (TD)colormapR[_src[x]];
			_dst1[x] = (TD)colormapG[_src[x]];
			_dst2[x] = (TD)colormapB[_src[x]];
		}
	}
};

}
}

#pragma pack(pop)

#endif
