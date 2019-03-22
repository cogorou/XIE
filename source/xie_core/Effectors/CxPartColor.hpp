/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXPARTCOLOR_HPP_INCLUDED_
#define _CXPARTCOLOR_HPP_INCLUDED_

#include "xie_core.h"
#include "Core/xie_core_math.h"

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
template<class TD> static inline TD fnPRV_2D_PartColor_Saturate(double value, TxRangeD range)
{
	if (value < range.Lower) return (TD)range.Lower;
	if (value > range.Upper) return (TD)range.Upper;
	return (TD)value;
}

// ======================================================================
template<class TD, class TS> static inline TxRGBx3<TD> fnPRV_2D_PartColor_Job(TS R, TS G, TS B, TxRangeD range, int hue_dir, int hue_range, double red_ratio, double green_ratio, double blue_ratio)
{
	// レンジ変換:
	double dR = (double)R / range.Upper;
	double dG = (double)G / range.Upper;
	double dB = (double)B / range.Upper;

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

	// 色相(Hue)の確認:
	{
		auto hue_L = hue_dir - hue_range;
		auto hue_H = hue_dir + hue_range;
		auto hue_V = dH;
		if (hue_L < 0)
		{
			hue_V -= hue_L;
			hue_H -= hue_L;
			hue_L -= hue_L;
			if (hue_V >= 360)
				hue_V = _mod(hue_V, 360);
		}
		if (0.0 < dS && hue_L <= hue_V && hue_V <= hue_H)
		{
			// HSV から RGB への変換:
			dR = dV;
			dG = dV;
			dB = dV;

			{
				int iH = (int)(dH / 60);

				double dF = (dH / 60) - iH;
				double d0 = dV * (1 - dS);
				double d1 = dV * (1 - dS * dF);
				double d2 = dV * (1 - dS * (1 - dF));

				switch (iH)
				{
					case 0: dR = dV; dG = d2; dB = d0; break;	// V,2,0
					case 1: dR = d1; dG = dV; dB = d0; break;	// 1,V,0
					case 2: dR = d0; dG = dV; dB = d2; break;	// 0,V,2
					case 3: dR = d0; dG = d1; dB = dV; break;	// 0,1,V
					case 4: dR = d2; dG = d0; dB = dV; break;	// 2,0,V
					case 5: dR = dV; dG = d0; dB = d1; break;	// V,0,1
				}
			}

			dR = dR * range.Upper;
			dG = dG * range.Upper;
			dB = dB * range.Upper;
		}
		else
		{
			// RGB から Monochrome への変換:
			auto dV = R * red_ratio + G * green_ratio + B * blue_ratio;
			dR = dV;
			dG = dV;
			dB = dV;
		}
	}

	// 結果:
	TxRGBx3<TD> result;
	result.R = fnPRV_2D_PartColor_Saturate<TD>(dR, range);
	result.G = fnPRV_2D_PartColor_Saturate<TD>(dG, range);
	result.B = fnPRV_2D_PartColor_Saturate<TD>(dB, range);
	return result;
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_PartColor_uu(TxImage dst, TxImage mask, TxImage src, int depth, int hue_dir, int hue_range, double red_ratio, double green_ratio, double blue_ratio)
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
				auto result = fnPRV_2D_PartColor_Job<TD, TS>(_src0[x], _src1[x], _src2[x], range, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);
				_dst0[x] = result.R;
				_dst1[x] = result.G;
				_dst2[x] = result.B;
			}
		}
	}
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_PartColor_pp(TxImage dst, TxImage mask, TxImage src, int depth, int hue_dir, int hue_range, double red_ratio, double green_ratio, double blue_ratio)
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
			[range, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio](int y, int x, TD* _dst, TS* _src)
			{
				auto result = fnPRV_2D_PartColor_Job<TD, TS>(_src[0], _src[1], _src[2], range, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);
				_dst[0] = result.R;
				_dst[1] = result.G;
				_dst[2] = result.B;
			});
	}
	else
	{
		dst_scan.ForEach(src_scan, mask_scan,
			[range, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio](int y, int x, TD* _dst, TS* _src, TM* _mask)
			{
				if (*_mask != 0)
				{
					auto result = fnPRV_2D_PartColor_Job<TD, TS>(_src[0], _src[1], _src[2], range, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);
					_dst[0] = result.R;
					_dst[1] = result.G;
					_dst[2] = result.B;
				}
			});
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_PartColor__u(TxImage dst, TxImage mask, TxImage src, int depth, int hue_dir, int hue_range, double red_ratio, double green_ratio, double blue_ratio)
{
	typedef TS	TD; 

	fnPRV_2D_PartColor_uu<TD,TM,TS>	(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_PartColor__p(TxImage dst, TxImage mask, TxImage src, int depth, int hue_dir, int hue_range, double red_ratio, double green_ratio, double blue_ratio)
{
	typedef TS	TD; 

	fnPRV_2D_PartColor_pp<TD,TM,TS>	(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);
}

// ======================================================================
template<class TM> static inline void XIE_API fnPRV_2D_PartColor___(TxImage dst, TxImage mask, TxImage src, int depth, int hue_dir, int hue_range, double red_ratio, double green_ratio, double blue_ratio)
{
	// hue_dir [0~359]
	hue_dir = hue_dir % 360;
	if (hue_dir < 0)
		hue_dir += 360;

	// hue_range [0~180]
	hue_range = hue_range % 360;
	if (hue_range > 180)
	{
		hue_range -= 360;
		hue_range = _abs(hue_range);
	}

	switch(src.Model.Pack)
	{
	case 1:
		if (src.Channels < 3)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		switch(src.Model.Type)
		{
		case ExType::U8:	fnPRV_2D_PartColor__u<TM,unsigned char>		(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::U16:	fnPRV_2D_PartColor__u<TM,unsigned short>	(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::U32:	fnPRV_2D_PartColor__u<TM,unsigned int>		(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::S8:	fnPRV_2D_PartColor__u<TM,char>				(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::S16:	fnPRV_2D_PartColor__u<TM,short>				(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::S32:	fnPRV_2D_PartColor__u<TM,int>				(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::F32:	fnPRV_2D_PartColor__u<TM,float>				(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::F64:	fnPRV_2D_PartColor__u<TM,double>			(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
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
		case ExType::U8:	fnPRV_2D_PartColor__p<TM,unsigned char>		(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::U16:	fnPRV_2D_PartColor__p<TM,unsigned short>	(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::U32:	fnPRV_2D_PartColor__p<TM,unsigned int>		(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::S8:	fnPRV_2D_PartColor__p<TM,char>				(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::S16:	fnPRV_2D_PartColor__p<TM,short>				(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::S32:	fnPRV_2D_PartColor__p<TM,int>				(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::F32:	fnPRV_2D_PartColor__p<TM,float>				(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
		case ExType::F64:	fnPRV_2D_PartColor__p<TM,double>			(dst, mask, src, depth, hue_dir, hue_range, red_ratio, green_ratio, blue_ratio);	break;
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
