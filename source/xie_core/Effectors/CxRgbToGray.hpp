/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXRGBTOGRAY_HPP_INCLUDED_
#define _CXRGBTOGRAY_HPP_INCLUDED_

#include "xie_core.h"

#pragma warning (disable:4127)	// 条件式が定数です.
#pragma warning (disable:4189)	// ローカル変数が初期化されましたが、参照されていません.

#include <math.h>

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/TxScanner2D.h"

namespace xie
{
namespace Effectors
{

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToGray_gu(TxImage dst, TxImage mask, TxImage src, double scale, double coefR, double coefG, double coefB)
{
	if (src.Channels < 3 || dst.Channels != 1)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	// Scale
	if (scale == 0)
		scale = 1;

	for(int ch=0 ; ch<src.Channels ; ch++)
	{
		TxScanner2D<TS> src0_scan = ToScanner<TS>(src, 0);
		TxScanner2D<TS> src1_scan = ToScanner<TS>(src, 1);
		TxScanner2D<TS> src2_scan = ToScanner<TS>(src, 2);
		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, 0);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, 0);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src0_scan, src1_scan, src2_scan,
				[scale,coefR,coefG,coefB](int y, int x, TD* _dst, TS* _src0, TS* _src1, TS* _src2)
				{
					*_dst = saturate_cast<TD>(((*_src0 * coefR) + (*_src1 * coefG) + (*_src2 * coefB)) * scale);
				});
		}
		else
		{
			dst_scan.ForEach(src0_scan, src1_scan, src2_scan, mask_scan,
				[scale,coefR,coefG,coefB](int y, int x, TD* _dst, TS* _src0, TS* _src1, TS* _src2, TM* _mask)
				{
					if (*_mask != 0)
						*_dst = saturate_cast<TD>(((*_src0 * coefR) + (*_src1 * coefG) + (*_src2 * coefB)) * scale);
				});
		}
	}
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToGray_gp(TxImage dst, TxImage mask, TxImage src, double scale, double coefR, double coefG, double coefB)
{
	if (src.Channels != dst.Channels)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	// Scale
	if (scale == 0)
		scale = 1;

	for(int ch=0 ; ch<src.Channels ; ch++)
	{
		TxScanner2D<TS> src_scan = ToScanner<TS>(src, ch);
		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : ch);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src_scan,
				[scale,coefR,coefG,coefB](int y, int x, TD* _dst, TS* _src)
				{
					*_dst = saturate_cast<TD>(((_src[0] * coefR) + (_src[1] * coefG) + (_src[2] * coefB)) * scale);
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[scale,coefR,coefG,coefB](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
						*_dst = saturate_cast<TD>(((_src[0] * coefR) + (_src[1] * coefG) + (_src[2] * coefB)) * scale);
				});
		}
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToGray__u(TxImage dst, TxImage mask, TxImage src, double scale, double coefR, double coefG, double coefB)
{
	switch(dst.Model.Type)
	{
	case ExType::U8:		fnPRV_2D_RgbToGray_gu<unsigned char,TM,TS>	(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::U16:	fnPRV_2D_RgbToGray_gu<unsigned short,TM,TS>	(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::U32:	fnPRV_2D_RgbToGray_gu<unsigned int,TM,TS>		(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::S8:		fnPRV_2D_RgbToGray_gu<char,TM,TS>				(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::S16:	fnPRV_2D_RgbToGray_gu<short,TM,TS>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::S32:	fnPRV_2D_RgbToGray_gu<int,TM,TS>				(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::F32:	fnPRV_2D_RgbToGray_gu<float,TM,TS>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::F64:	fnPRV_2D_RgbToGray_gu<double,TM,TS>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToGray__p(TxImage dst, TxImage mask, TxImage src, double scale, double coefR, double coefG, double coefB)
{
	switch(dst.Model.Type)
	{
	case ExType::U8:		fnPRV_2D_RgbToGray_gp<unsigned char,TM,TS>	(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::U16:	fnPRV_2D_RgbToGray_gp<unsigned short,TM,TS>	(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::U32:	fnPRV_2D_RgbToGray_gp<unsigned int,TM,TS>		(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::S8:		fnPRV_2D_RgbToGray_gp<char,TM,TS>				(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::S16:	fnPRV_2D_RgbToGray_gp<short,TM,TS>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::S32:	fnPRV_2D_RgbToGray_gp<int,TM,TS>				(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::F32:	fnPRV_2D_RgbToGray_gp<float,TM,TS>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
	case ExType::F64:	fnPRV_2D_RgbToGray_gp<double,TM,TS>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM> static inline void XIE_API fnPRV_2D_RgbToGray___(TxImage dst, TxImage mask, TxImage src, double scale, double coefR, double coefG, double coefB)
{
	switch(src.Model.Pack)
	{
	case 1:
		switch(src.Model.Type)
		{
		case ExType::U8:		fnPRV_2D_RgbToGray__u<TM,unsigned char>	(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::U16:	fnPRV_2D_RgbToGray__u<TM,unsigned short>	(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::U32:	fnPRV_2D_RgbToGray__u<TM,unsigned int>	(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::S8:		fnPRV_2D_RgbToGray__u<TM,char>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::S16:	fnPRV_2D_RgbToGray__u<TM,short>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::S32:	fnPRV_2D_RgbToGray__u<TM,int>				(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::F32:	fnPRV_2D_RgbToGray__u<TM,float>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::F64:	fnPRV_2D_RgbToGray__u<TM,double>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 3:
	case 4:
		switch(src.Model.Type)
		{
		case ExType::U8:		fnPRV_2D_RgbToGray__p<TM,unsigned char>	(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::U16:	fnPRV_2D_RgbToGray__p<TM,unsigned short>	(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::U32:	fnPRV_2D_RgbToGray__p<TM,unsigned int>	(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::S8:		fnPRV_2D_RgbToGray__p<TM,char>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::S16:	fnPRV_2D_RgbToGray__p<TM,short>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::S32:	fnPRV_2D_RgbToGray__p<TM,int>				(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::F32:	fnPRV_2D_RgbToGray__p<TM,float>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
		case ExType::F64:	fnPRV_2D_RgbToGray__p<TM,double>			(dst, mask, src, scale, coefR, coefG, coefB);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

}	// Effectors
}	// xie

#endif
