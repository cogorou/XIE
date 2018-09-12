/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGAMMACONVERTER_HPP_INCLUDED_
#define _CXGAMMACONVERTER_HPP_INCLUDED_

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
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_GammaConverter_pp(TxImage dst, TxImage mask, TxImage src, int depth, double gamma)
{
	int dst_pxc = dst.Model.Pack * dst.Channels;
	int src_pxc = src.Model.Pack * src.Channels;

	int min_pxc = (src_pxc == 1) ? dst_pxc : Axi::Min(dst_pxc, src_pxc);

	const TxRangeD range = xie::Axi::CalcRange(src.Model.Type, depth);
	const double gamma_correction = (gamma == 0) ? 1 : 1 / gamma;

	for(int ch=0 ; ch<min_pxc ; ch++)
	{
		int dst_ch    = ch / dst.Model.Pack;
		int dst_field = ch % dst.Model.Pack;

		int src_ch    = (src_pxc == 1) ? 0 : ch / src.Model.Pack;
		int src_field = (src_pxc == 1) ? 0 : ch % src.Model.Pack;

		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, dst_ch);
		TxScanner2D<TS> src_scan = ToScanner<TS>(src, src_ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : dst_ch);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src_scan,
				[dst_field,src_field,range,gamma_correction](int y, int x, TD* _dst, TS* _src)
				{
					_dst[dst_field] = saturate_cast<TD>( pow(_src[src_field] / range.Upper, gamma_correction) * range.Upper );
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[dst_field,src_field,range,gamma_correction](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
					{
						_dst[dst_field] = saturate_cast<TD>( pow(_src[src_field] / range.Upper, gamma_correction) * range.Upper );
					}
				});
		}
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_GammaConverter__p(TxImage dst, TxImage mask, TxImage src, int depth, double gamma)
{
	typedef TS	TD; 

	fnPRV_2D_GammaConverter_pp<TD,TM,TS>	(dst, mask, src, depth, gamma);
}

// ======================================================================
template<class TM> static inline void XIE_API fnPRV_2D_GammaConverter___(TxImage dst, TxImage mask, TxImage src, int depth, double gamma)
{
	switch(src.Model.Type)
	{
	case ExType::U8:		fnPRV_2D_GammaConverter__p<TM,unsigned char>	(dst, mask, src, depth, gamma);	break;
	case ExType::U16:	fnPRV_2D_GammaConverter__p<TM,unsigned short>	(dst, mask, src, depth, gamma);	break;
	case ExType::U32:	fnPRV_2D_GammaConverter__p<TM,unsigned int>		(dst, mask, src, depth, gamma);	break;
	case ExType::S8:		fnPRV_2D_GammaConverter__p<TM,char>				(dst, mask, src, depth, gamma);	break;
	case ExType::S16:	fnPRV_2D_GammaConverter__p<TM,short>			(dst, mask, src, depth, gamma);	break;
	case ExType::S32:	fnPRV_2D_GammaConverter__p<TM,int>				(dst, mask, src, depth, gamma);	break;
	case ExType::F32:	fnPRV_2D_GammaConverter__p<TM,float>			(dst, mask, src, depth, gamma);	break;
	case ExType::F64:	fnPRV_2D_GammaConverter__p<TM,double>			(dst, mask, src, depth, gamma);	break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

}	// Effectors
}	// xie

#endif
