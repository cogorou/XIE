/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXHSVTORGB_HPP_INCLUDED_
#define _CXHSVTORGB_HPP_INCLUDED_

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
template<class TD> static inline TD fnPRV_2D_HsvToRgb_Saturate(double value, TxRangeD range)
{
	if (value < range.Lower) return (TD)range.Lower;
	if (value > range.Upper) return (TD)range.Upper;
	return (TD)value;
}

// ======================================================================
template<class TD, class TS> static inline TxRGBx3<TD> fnPRV_2D_HsvToRgb_Job(TS dH, TS dS, TS dV, TxRangeD range)
{
	// HSV から RGB への変換:
	double dR = dV;
	double dG = dV;
	double dB = dV;

	// 色相(Hue)のレンジ変換:
	{
		dH = (int)(dH * 360) % 360;
		if (dH < 0)
			dH += 360;
	}

	if (0 != dS)
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

	// 結果:
	TxRGBx3<TD> result;
	result.R = fnPRV_2D_HsvToRgb_Saturate<TD>(dR * range.Upper, range);
	result.G = fnPRV_2D_HsvToRgb_Saturate<TD>(dG * range.Upper, range);
	result.B = fnPRV_2D_HsvToRgb_Saturate<TD>(dB * range.Upper, range);
	return result;
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_HsvToRgb_uu(TxImage dst, TxImage mask, TxImage src, int depth)
{
	TxScanner2D<TS> src0_scan = ToScanner<TS>(src, 0);
	TxScanner2D<TS> src1_scan = ToScanner<TS>(src, 1);
	TxScanner2D<TS> src2_scan = ToScanner<TS>(src, 2);
	TxScanner2D<TD> dst0_scan = ToScanner<TD>(dst, 0);
	TxScanner2D<TD> dst1_scan = ToScanner<TD>(dst, 1);
	TxScanner2D<TD> dst2_scan = ToScanner<TD>(dst, 2);
	TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, 0);

	auto range = xie::Axi::CalcRange(dst.Model.Type, depth);
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
				auto result = fnPRV_2D_HsvToRgb_Job<TD, TS>(_src0[x], _src1[x], _src2[x], range);
				_dst0[x] = result.R;
				_dst1[x] = result.G;
				_dst2[x] = result.B;
			}
		}
	}
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_HsvToRgb_pp(TxImage dst, TxImage mask, TxImage src, int depth)
{
	TxScanner2D<TS> src_scan = ToScanner<TS>(src, 0);
	TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, 0);
	TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, 0);

	auto range = xie::Axi::CalcRange(dst.Model.Type, depth);
	if (range.Upper == 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (mask_scan.IsValid() == false)
	{
		dst_scan.ForEach(src_scan,
			[range](int y, int x, TD* _dst, TS* _src)
			{
				auto result = fnPRV_2D_HsvToRgb_Job<TD, TS>(_src[0], _src[1], _src[2], range);
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
					auto result = fnPRV_2D_HsvToRgb_Job<TD, TS>(_src[0], _src[1], _src[2], range);
					_dst[0] = result.R;
					_dst[1] = result.G;
					_dst[2] = result.B;
				}
			});
	}
}

// ======================================================================
template<class TD, class TM> static inline void XIE_API fnPRV_2D_HsvToRgb__u(TxImage dst, TxImage mask, TxImage src, int depth)
{
	switch(src.Model.Type)
	{
		case ExType::F64:	fnPRV_2D_HsvToRgb_uu<TD,TM,double>	(dst, mask, src, depth);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TD, class TM> static inline void XIE_API fnPRV_2D_HsvToRgb__p(TxImage dst, TxImage mask, TxImage src, int depth)
{
	switch(src.Model.Type)
	{
		case ExType::F64:	fnPRV_2D_HsvToRgb_pp<TD,TM,double>	(dst, mask, src, depth);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM> static inline void XIE_API fnPRV_2D_HsvToRgb___(TxImage dst, TxImage mask, TxImage src, int depth)
{
	switch(dst.Model.Pack)
	{
	case 1:
		if (dst.Channels < 3)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		switch(dst.Model.Type)
		{
		case ExType::U8:	fnPRV_2D_HsvToRgb__u<unsigned char,TM>	(dst, mask, src, depth);	break;
		case ExType::U16:	fnPRV_2D_HsvToRgb__u<unsigned short,TM>	(dst, mask, src, depth);	break;
		case ExType::U32:	fnPRV_2D_HsvToRgb__u<unsigned int,TM>		(dst, mask, src, depth);	break;
		case ExType::S8:	fnPRV_2D_HsvToRgb__u<char,TM>				(dst, mask, src, depth);	break;
		case ExType::S16:	fnPRV_2D_HsvToRgb__u<short,TM>			(dst, mask, src, depth);	break;
		case ExType::S32:	fnPRV_2D_HsvToRgb__u<int,TM>				(dst, mask, src, depth);	break;
		case ExType::F32:	fnPRV_2D_HsvToRgb__u<float,TM>			(dst, mask, src, depth);	break;
		case ExType::F64:	fnPRV_2D_HsvToRgb__u<double,TM>			(dst, mask, src, depth);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 3:
	case 4:
		if (dst.Channels != 1)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		switch(dst.Model.Type)
		{
		case ExType::U8:	fnPRV_2D_HsvToRgb__p<unsigned char,TM>	(dst, mask, src, depth);	break;
		case ExType::U16:	fnPRV_2D_HsvToRgb__p<unsigned short,TM>	(dst, mask, src, depth);	break;
		case ExType::U32:	fnPRV_2D_HsvToRgb__p<unsigned int,TM>		(dst, mask, src, depth);	break;
		case ExType::S8:	fnPRV_2D_HsvToRgb__p<char,TM>				(dst, mask, src, depth);	break;
		case ExType::S16:	fnPRV_2D_HsvToRgb__p<short,TM>			(dst, mask, src, depth);	break;
		case ExType::S32:	fnPRV_2D_HsvToRgb__p<int,TM>				(dst, mask, src, depth);	break;
		case ExType::F32:	fnPRV_2D_HsvToRgb__p<float,TM>			(dst, mask, src, depth);	break;
		case ExType::F64:	fnPRV_2D_HsvToRgb__p<double,TM>			(dst, mask, src, depth);	break;
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
