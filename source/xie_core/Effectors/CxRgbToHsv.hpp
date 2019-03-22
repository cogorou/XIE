/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXRGBTOHSV_HPP_INCLUDED_
#define _CXRGBTOHSV_HPP_INCLUDED_

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
template<class TD, class TS> static inline TxRGBx3<TD> fnPRV_2D_RgbToHsv_Job(TS R, TS G, TS B, TxRangeD range)
{
	// レンジ変換:
	double dR = (double)R / range.Upper;
	double dG = (double)G / range.Upper;
	double dB = (double)B / range.Upper;

	// 彩度(Saturation)の変換:
	{
		double gray = 0.299 * dR + 0.587 * dG + 0.114 * dB;
		dR = (dR - gray) + gray;
		dG = (dG - gray) + gray;
		dB = (dB - gray) + gray;
		if (dR < 0) dR = 0; else if (dR > 1) dR = 1;
		if (dG < 0) dG = 0; else if (dG > 1) dG = 1;
		if (dB < 0) dB = 0; else if (dB > 1) dB = 1;
	}

	double max_val = xie::Axi::Max(dR, xie::Axi::Max(dG, dB));
	double min_val = xie::Axi::Min(dR, xie::Axi::Min(dG, dB));
	double delta = (max_val - min_val);

	double dH = 0;
	double dS = (max_val == 0) ? 0 : delta / max_val;
	double dV = max_val;

	// 色相(Hue):
	if (delta == 0) dH = 0;
	else if (max_val == dR) dH = ((dG - dB) / delta + 0) * 60;
	else if (max_val == dG) dH = ((dB - dR) / delta + 2) * 60;
	else if (max_val == dB) dH = ((dR - dG) / delta + 4) * 60;

	// 色相(Hue)の正規化:
	{
		dH = _mod(dH, 360);
		if (dH < 0)
			dH += 360;
	}

	// 色相(Hue)のレンジ変換:
	dH = dH / 360;

	// 結果:
	TxRGBx3<TD> result;
	result.R = xie::saturate_cast<TD>(dH);
	result.G = xie::saturate_cast<TD>(dS);
	result.B = xie::saturate_cast<TD>(dV);
	return result;
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToHsv_uu(TxImage dst, TxImage mask, TxImage src, int depth)
{
	TxScanner2D<TS> src0_scan = ToScanner<TS>(src, 0);
	TxScanner2D<TS> src1_scan = ToScanner<TS>(src, 1);
	TxScanner2D<TS> src2_scan = ToScanner<TS>(src, 2);
	TxScanner2D<TD> dst0_scan = ToScanner<TD>(dst, 0);
	TxScanner2D<TD> dst1_scan = ToScanner<TD>(dst, 1);
	TxScanner2D<TD> dst2_scan = ToScanner<TD>(dst, 2);
	TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, 0);

	auto range = xie::Axi::CalcRange(src.Model.Type, depth);
	if (range.Upper == 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	for(int y=0 ; y<dst0_scan.Height ; y++)
	{
		TS* _src0 = src0_scan[y];
		TS* _src1 = src1_scan[y];
		TS* _src2 = src2_scan[y];
		TD* _dst0 = dst0_scan[y];
		TD* _dst1 = dst1_scan[y];
		TD* _dst2 = dst2_scan[y];
		TM* _mask = (mask_scan.IsValid()) ? mask_scan[y] : NULL;

		for(int x=0 ; x<dst0_scan.Width ; x++)
		{
			if (_mask == NULL || _mask[x] != 0)
			{
				auto result = fnPRV_2D_RgbToHsv_Job<TD, TS>(_src0[x], _src1[x], _src2[x], range);
				_dst0[x] = result.R;
				_dst1[x] = result.G;
				_dst2[x] = result.B;
			}
		}
	}
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToHsv_pp(TxImage dst, TxImage mask, TxImage src, int depth)
{
	TxScanner2D<TS> src_scan = ToScanner<TS>(src, 0);
	TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, 0);
	TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, 0);

	auto range = xie::Axi::CalcRange(src.Model.Type, depth);
	if (range.Upper == 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (mask_scan.IsValid() == false)
	{
		dst_scan.ForEach(src_scan,
			[range](int y, int x, TD* _dst, TS* _src)
			{
				auto result = fnPRV_2D_RgbToHsv_Job<TD, TS>(_src[0], _src[1], _src[2], range);
				_dst[0] = result.R;
				_dst[1] = result.G;
				_dst[2] = result.B;
			});
	}
	else
	{
		dst_scan.ForEach(src_scan, mask_scan,
			[range](int y, int x, TD* _dst, TS* _src, TM* _mask)
			{
				if (*_mask != 0)
				{
					auto result = fnPRV_2D_RgbToHsv_Job<TD, TS>(_src[0], _src[1], _src[2], range);
					_dst[0] = result.R;
					_dst[1] = result.G;
					_dst[2] = result.B;
				}
			});
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToHsv__u(TxImage dst, TxImage mask, TxImage src, int depth)
{
	switch(dst.Model.Type)
	{
		case ExType::F64:	fnPRV_2D_RgbToHsv_uu<double,TM,TS>	(dst, mask, src, depth);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToHsv__p(TxImage dst, TxImage mask, TxImage src, int depth)
{
	switch(dst.Model.Type)
	{
		case ExType::F64:	fnPRV_2D_RgbToHsv_pp<double,TM,TS>	(dst, mask, src, depth);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM> static inline void XIE_API fnPRV_2D_RgbToHsv___(TxImage dst, TxImage mask, TxImage src, int depth)
{
	switch(src.Model.Pack)
	{
	case 1:
		if (src.Channels < 3)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		switch(src.Model.Type)
		{
		case ExType::U8:	fnPRV_2D_RgbToHsv__u<TM,unsigned char>	(dst, mask, src, depth);	break;
		case ExType::U16:	fnPRV_2D_RgbToHsv__u<TM,unsigned short>	(dst, mask, src, depth);	break;
		case ExType::U32:	fnPRV_2D_RgbToHsv__u<TM,unsigned int>		(dst, mask, src, depth);	break;
		case ExType::S8:	fnPRV_2D_RgbToHsv__u<TM,char>				(dst, mask, src, depth);	break;
		case ExType::S16:	fnPRV_2D_RgbToHsv__u<TM,short>			(dst, mask, src, depth);	break;
		case ExType::S32:	fnPRV_2D_RgbToHsv__u<TM,int>				(dst, mask, src, depth);	break;
		case ExType::F32:	fnPRV_2D_RgbToHsv__u<TM,float>			(dst, mask, src, depth);	break;
		case ExType::F64:	fnPRV_2D_RgbToHsv__u<TM,double>			(dst, mask, src, depth);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 3:
	case 4:
		if (src.Channels != 1)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		switch(src.Model.Type)
		{
		case ExType::U8:	fnPRV_2D_RgbToHsv__p<TM,unsigned char>	(dst, mask, src, depth);	break;
		case ExType::U16:	fnPRV_2D_RgbToHsv__p<TM,unsigned short>	(dst, mask, src, depth);	break;
		case ExType::U32:	fnPRV_2D_RgbToHsv__p<TM,unsigned int>		(dst, mask, src, depth);	break;
		case ExType::S8:	fnPRV_2D_RgbToHsv__p<TM,char>				(dst, mask, src, depth);	break;
		case ExType::S16:	fnPRV_2D_RgbToHsv__p<TM,short>			(dst, mask, src, depth);	break;
		case ExType::S32:	fnPRV_2D_RgbToHsv__p<TM,int>				(dst, mask, src, depth);	break;
		case ExType::F32:	fnPRV_2D_RgbToHsv__p<TM,float>			(dst, mask, src, depth);	break;
		case ExType::F64:	fnPRV_2D_RgbToHsv__p<TM,double>			(dst, mask, src, depth);	break;
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
