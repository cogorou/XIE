/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"

#pragma warning (disable:4127)	// 条件式が定数です.
#pragma warning (disable:4189)	// ローカル変数が初期化されましたが、参照されていません.

#include "api_gdi.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxImage.h"
#include "Core/CxArrayEx.h"

namespace xie
{
namespace GDI
{

// ================================================================================
template<class TD, class TS> static inline void XIE_API fnPRV_GDI_Shrink0_pg(HxModule hdst, HxModule hsrc, double mag)
{
	CxImage* src = reinterpret_cast<CxImage*>(hsrc);
	CxImage* dst = reinterpret_cast<CxImage*>(hdst);
	
	// size
	if (src->Channels()	!= dst->Channels())
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	float scale = (float)Axi::CalcScale(src->Model().Type, src->Depth(), dst->Model().Type, dst->Depth());
	float fetch_mag = (float)(1 / mag);
	
	{
		int		width		= Axi::Min(dst->Width(), (int)(src->Width() * mag));
		int		height		= Axi::Min(dst->Height(), (int)(src->Height() * mag));
		int		channels	= dst->Channels();

		TxIntPtr	src_step	= src->Stride();
		TxIntPtr	dst_step	= dst->Stride();

		for(int ch=0 ; ch<channels ; ch++)
		{
			unsigned char*	src_addr	= static_cast<unsigned char*>((*src)[ch]);
			unsigned char*	dst_addr	= static_cast<unsigned char*>((*dst)[ch]);

			for(int y=0 ; y<height ; y++)
			{
				int yyy = (int)(y * fetch_mag);
				TS*	_src = (TS*)(src_addr + (src_step * yyy));
				TD*	_dst = (TD*)(dst_addr + (dst_step * y));
				
				for(int x=0 ; x<width ; x++)
				{
					int xxx = (int)(x * fetch_mag);
					float G = (float)(_src[xxx] * scale);
					_dst[x].R = (G < 0) ? 0 : (G > 255) ? 255 : (unsigned char)(G + 0.5f);
					_dst[x].G = (G < 0) ? 0 : (G > 255) ? 255 : (unsigned char)(G + 0.5f);
					_dst[x].B = (G < 0) ? 0 : (G > 255) ? 255 : (unsigned char)(G + 0.5f);
				}
			}
		}
	}
}

// ================================================================================
template<class TD, class TS> static inline void XIE_API fnPRV_GDI_Shrink0_pp(HxModule hdst, HxModule hsrc, double mag)
{
	CxImage* src = reinterpret_cast<CxImage*>(hsrc);
	CxImage* dst = reinterpret_cast<CxImage*>(hdst);
	
	// size
	if (src->Channels()	!= dst->Channels())
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	float scale = (float)Axi::CalcScale(src->Model().Type, src->Depth(), dst->Model().Type, dst->Depth());
	float fetch_mag = (float)(1 / mag);

	{
		int		width		= Axi::Min(dst->Width(), (int)(src->Width() * mag));
		int		height		= Axi::Min(dst->Height(), (int)(src->Height() * mag));
		int		channels	= dst->Channels();

		TxIntPtr	src_step	= src->Stride();
		TxIntPtr	dst_step	= dst->Stride();

		for(int ch=0 ; ch<channels ; ch++)
		{
			unsigned char*	src_addr	= static_cast<unsigned char*>((*src)[ch]);
			unsigned char*	dst_addr	= static_cast<unsigned char*>((*dst)[ch]);

			for(int y=0 ; y<height ; y++)
			{
				int yyy = (int)(y * fetch_mag);
				TS*	_src = (TS*)(src_addr + (src_step * yyy));
				TD*	_dst = (TD*)(dst_addr + (dst_step * y));
				
				for(int x=0 ; x<width ; x++)
				{
					int xxx = (int)(x * fetch_mag);
					float R = (float)(_src[xxx].R * scale);
					float G = (float)(_src[xxx].G * scale);
					float B = (float)(_src[xxx].B * scale);
					_dst[x].R = (R < 0) ? 0 : (R > 255) ? 255 : (unsigned char)(R + 0.5f);
					_dst[x].G = (G < 0) ? 0 : (G > 255) ? 255 : (unsigned char)(G + 0.5f);
					_dst[x].B = (B < 0) ? 0 : (B > 255) ? 255 : (unsigned char)(B + 0.5f);
				}
			}
		}
	}
}

// ================================================================================
template<class TD, class TS> static inline void XIE_API fnPRV_GDI_Shrink0_pu(HxModule hdst, HxModule hsrc, double mag)
{
	CxImage* src = reinterpret_cast<CxImage*>(hsrc);
	CxImage* dst = reinterpret_cast<CxImage*>(hdst);
	
	// size
	if (src->Channels() < 3)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	float scale = (float)Axi::CalcScale(src->Model().Type, src->Depth(), dst->Model().Type, dst->Depth());
	float fetch_mag = (float)(1 / mag);
	
	{
		int		width		= Axi::Min(dst->Width(), (int)(src->Width() * mag));
		int		height		= Axi::Min(dst->Height(), (int)(src->Height() * mag));

		TxIntPtr	src_step	= src->Stride();
		TxIntPtr	dst_step	= dst->Stride();

		{
			unsigned char*	src_addr0	= static_cast<unsigned char*>((*src)[0]);
			unsigned char*	src_addr1	= static_cast<unsigned char*>((*src)[1]);
			unsigned char*	src_addr2	= static_cast<unsigned char*>((*src)[2]);
			unsigned char*	dst_addr	= static_cast<unsigned char*>((*dst)[0]);

			for(int y=0 ; y<height ; y++)
			{
				int yyy = (int)(y * fetch_mag);
				TS*	_src0 = (TS*)(src_addr0 + (src_step * yyy));
				TS*	_src1 = (TS*)(src_addr1 + (src_step * yyy));
				TS*	_src2 = (TS*)(src_addr2 + (src_step * yyy));
				TD*	_dst = (TD*)(dst_addr + (dst_step * y));
				
				for(int x=0 ; x<width ; x++)
				{
					int xxx = (int)(x * fetch_mag);
					float R = (float)(_src0[xxx] * scale);
					float G = (float)(_src1[xxx] * scale);
					float B = (float)(_src2[xxx] * scale);
					_dst[x].R = (R < 0) ? 0 : (R > 255) ? 255 : (unsigned char)(R + 0.5f);
					_dst[x].G = (G < 0) ? 0 : (G > 255) ? 255 : (unsigned char)(G + 0.5f);
					_dst[x].B = (B < 0) ? 0 : (B > 255) ? 255 : (unsigned char)(B + 0.5f);
				}
			}
		}
	}
}

// ================================================================================
template<class TD, class TS> static inline void XIE_API fnPRV_GDI_Shrink0_uu(HxModule hdst, HxModule hsrc, double mag)
{
	CxImage* src = reinterpret_cast<CxImage*>(hsrc);
	CxImage* dst = reinterpret_cast<CxImage*>(hdst);
	
	// size
	if (src->Channels()	!= dst->Channels())
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	float scale = (float)Axi::CalcScale(src->Model().Type, src->Depth(), dst->Model().Type, dst->Depth());
	float fetch_mag = (float)(1 / mag);
	
	{
		int		width		= Axi::Min(dst->Width(), (int)(src->Width() * mag));
		int		height		= Axi::Min(dst->Height(), (int)(src->Height() * mag));
		int		channels	= dst->Channels();

		TxIntPtr	src_step	= src->Stride();
		TxIntPtr	dst_step	= dst->Stride();

		for(int ch=0 ; ch<channels ; ch++)
		{
			unsigned char*	src_addr	= static_cast<unsigned char*>((*src)[ch]);
			unsigned char*	dst_addr	= static_cast<unsigned char*>((*dst)[ch]);

			for(int y=0 ; y<height ; y++)
			{
				int yyy = (int)(y * fetch_mag);
				TS*	_src = (TS*)(src_addr + (src_step * yyy));
				TD*	_dst = (TD*)(dst_addr + (dst_step * y));
				
				for(int x=0 ; x<width ; x++)
				{
					int xxx = (int)(x * fetch_mag);
					float G = (float)(_src[xxx] * scale);
					_dst[x] = (G < 0) ? 0 : (G > 255) ? 255 : (unsigned char)(G + 0.5f);
				}
			}
		}
	}
}

// ================================================================================
template<class TD> static inline void XIE_API fnPRV_GDI_Shrink0__p(HxModule hdst, HxModule hsrc, double mag)
{
	CxImage* src = reinterpret_cast<CxImage*>(hsrc);

	// 入力画像の種別による分岐.
	switch(src->Model().Pack)
	{
	default:
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	case 1:
		if (src->Channels() == 1)
		{
			switch(src->Model().Type)
			{
			default:
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			case ExType::U8:	fnPRV_GDI_Shrink0_pg<TD, unsigned char>		( hdst, hsrc, mag );	break;
			case ExType::U16:	fnPRV_GDI_Shrink0_pg<TD, unsigned short>	( hdst, hsrc, mag );	break;
			case ExType::U32:	fnPRV_GDI_Shrink0_pg<TD, unsigned int>		( hdst, hsrc, mag );	break;
			case ExType::S8:	fnPRV_GDI_Shrink0_pg<TD, char>				( hdst, hsrc, mag );	break;
			case ExType::S16:	fnPRV_GDI_Shrink0_pg<TD, short>				( hdst, hsrc, mag );	break;
			case ExType::S32:	fnPRV_GDI_Shrink0_pg<TD, int>				( hdst, hsrc, mag );	break;
			case ExType::F32:	fnPRV_GDI_Shrink0_pg<TD, float>				( hdst, hsrc, mag );	break;
			case ExType::F64:	fnPRV_GDI_Shrink0_pg<TD, double>			( hdst, hsrc, mag );	break;
			}
		}
		else
		{
			switch(src->Model().Type)
			{
			default:
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			case ExType::U8:	fnPRV_GDI_Shrink0_pu<TD, unsigned char>		( hdst, hsrc, mag );	break;
			case ExType::U16:	fnPRV_GDI_Shrink0_pu<TD, unsigned short>	( hdst, hsrc, mag );	break;
			case ExType::U32:	fnPRV_GDI_Shrink0_pu<TD, unsigned int>		( hdst, hsrc, mag );	break;
			case ExType::S8:	fnPRV_GDI_Shrink0_pu<TD, char>				( hdst, hsrc, mag );	break;
			case ExType::S16:	fnPRV_GDI_Shrink0_pu<TD, short>				( hdst, hsrc, mag );	break;
			case ExType::S32:	fnPRV_GDI_Shrink0_pu<TD, int>				( hdst, hsrc, mag );	break;
			case ExType::F32:	fnPRV_GDI_Shrink0_pu<TD, float>				( hdst, hsrc, mag );	break;
			case ExType::F64:	fnPRV_GDI_Shrink0_pu<TD, double>			( hdst, hsrc, mag );	break;
			}
		}
		break;
	case 3:
		{
			switch(src->Model().Type)
			{
			default:
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			case ExType::U8:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx3<unsigned char>>	( hdst, hsrc, mag );	break;
			case ExType::U16:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx3<unsigned short>>	( hdst, hsrc, mag );	break;
			case ExType::U32:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx3<unsigned int>>		( hdst, hsrc, mag );	break;
			case ExType::S8:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx3<char>>				( hdst, hsrc, mag );	break;
			case ExType::S16:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx3<short>>			( hdst, hsrc, mag );	break;
			case ExType::S32:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx3<int>>				( hdst, hsrc, mag );	break;
			case ExType::F32:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx3<float>>			( hdst, hsrc, mag );	break;
			case ExType::F64:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx3<double>>			( hdst, hsrc, mag );	break;
			}
		}
		break;
	case 4:
		{
			switch(src->Model().Type)
			{
			default:
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			case ExType::U8:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx4<unsigned char>>	( hdst, hsrc, mag );	break;
			case ExType::U16:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx4<unsigned short>>	( hdst, hsrc, mag );	break;
			case ExType::U32:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx4<unsigned int>>		( hdst, hsrc, mag );	break;
			case ExType::S8:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx4<char>>				( hdst, hsrc, mag );	break;
			case ExType::S16:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx4<short>>			( hdst, hsrc, mag );	break;
			case ExType::S32:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx4<int>>				( hdst, hsrc, mag );	break;
			case ExType::F32:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx4<float>>			( hdst, hsrc, mag );	break;
			case ExType::F64:	fnPRV_GDI_Shrink0_pp<TD, TxRGBx4<double>>			( hdst, hsrc, mag );	break;
			}
		}
		break;
	}
}

// ================================================================================
template<class TD> static inline void XIE_API fnPRV_GDI_Shrink0__u(HxModule hdst, HxModule hsrc, double mag)
{
	CxImage* src = reinterpret_cast<CxImage*>(hsrc);

	// 入力画像の種別による分岐.
	switch(src->Model().Pack)
	{
	default:
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	case 1:
		switch(src->Model().Type)
		{
		default:
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		case ExType::U8:	fnPRV_GDI_Shrink0_uu<TD, unsigned char>		( hdst, hsrc, mag );	break;
		case ExType::U16:	fnPRV_GDI_Shrink0_uu<TD, unsigned short>	( hdst, hsrc, mag );	break;
		case ExType::U32:	fnPRV_GDI_Shrink0_uu<TD, unsigned int>		( hdst, hsrc, mag );	break;
		case ExType::S8:	fnPRV_GDI_Shrink0_uu<TD, char>				( hdst, hsrc, mag );	break;
		case ExType::S16:	fnPRV_GDI_Shrink0_uu<TD, short>				( hdst, hsrc, mag );	break;
		case ExType::S32:	fnPRV_GDI_Shrink0_uu<TD, int>				( hdst, hsrc, mag );	break;
		case ExType::F32:	fnPRV_GDI_Shrink0_uu<TD, float>				( hdst, hsrc, mag );	break;
		case ExType::F64:	fnPRV_GDI_Shrink0_uu<TD, double>			( hdst, hsrc, mag );	break;
		}
		break;
	}
}

// ======================================================================
void XIE_API fnPRV_GDI_Shrink0(HxModule hdst, HxModule hsrc, double mag, bool swap)
{
	if (xie::Axi::ClassIs<CxImage>(hdst) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (xie::Axi::ClassIs<CxImage>(hsrc) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	if (!(0.0 < mag && mag < 1.0))
	{
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	CxImage* src = reinterpret_cast<CxImage*>(hsrc);
	CxImage* dst = reinterpret_cast<CxImage*>(hdst);

	if (src->IsValid() == false ||
		dst->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	// 出力画像の種別による分岐.
	switch(dst->Model().Type)
	{
	default:
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	case ExType::U8:
		switch(dst->Model().Pack)
		{
		default:
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		case 1: fnPRV_GDI_Shrink0__u<unsigned char>	( hdst, hsrc, mag );	break;
		case 3:
			if (swap)
				fnPRV_GDI_Shrink0__p<TxBGR8x3>		( hdst, hsrc, mag );
			else
				fnPRV_GDI_Shrink0__p<TxRGB8x3>		( hdst, hsrc, mag );
			break;
		case 4:
			if (swap)
				fnPRV_GDI_Shrink0__p<TxBGR8x4>		( hdst, hsrc, mag );
			else
				fnPRV_GDI_Shrink0__p<TxRGB8x4>		( hdst, hsrc, mag );
			break;
		}
		break;
	}
}

}
}
