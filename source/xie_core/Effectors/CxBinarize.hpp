/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXBINARIZE_HPP_INCLUDED_
#define _CXBINARIZE_HPP_INCLUDED_

#include "xie_core.h"

#pragma warning (disable:4127)	// 条件式が定数です.
#pragma warning (disable:4189)	// ローカル変数が初期化されましたが、参照されていません.

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxImage.h"
#include "Core/TxScanner2D.h"

#include <math.h>

namespace xie
{
namespace Effectors
{

// ======================================================================
template<class TD, class TM, class TS, class TFUNC> static inline void XIE_API fnPRV_2D_Binarize_pp(TxImage dst, TxImage mask, TxImage src, TFUNC element_operator)
{
	int dst_pxc = dst.Model.Pack * dst.Channels;
	int src_pxc = src.Model.Pack * src.Channels;

	int min_pxc = (src_pxc == 1) ? dst_pxc : Axi::Min(dst_pxc, src_pxc);

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
				[dst_field,src_field,element_operator](int y, int x, TD* _dst, TS* _src)
				{
					_dst[dst_field] = (TD)element_operator(_src[src_field]);
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[dst_field,src_field,element_operator](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
					{
						_dst[dst_field] = (TD)element_operator(_src[src_field]);
					}
				});
		}
	}
}

// ======================================================================
template<class TD, class TM, class TFUNC> static inline void XIE_API fnPRV_2D_Binarize_p_(TxImage dst, TxImage mask, TxImage src, TFUNC element_operator)
{
	switch(src.Model.Type)
	{
	case ExType::U8:		fnPRV_2D_Binarize_pp<TD,TM,unsigned char>		(dst, mask, src, element_operator); break;
	case ExType::U16:	fnPRV_2D_Binarize_pp<TD,TM,unsigned short>	(dst, mask, src, element_operator); break;
	case ExType::U32:	fnPRV_2D_Binarize_pp<TD,TM,unsigned int>		(dst, mask, src, element_operator); break;
	case ExType::S8:		fnPRV_2D_Binarize_pp<TD,TM,char>				(dst, mask, src, element_operator); break;
	case ExType::S16:	fnPRV_2D_Binarize_pp<TD,TM,short>				(dst, mask, src, element_operator); break;
	case ExType::S32:	fnPRV_2D_Binarize_pp<TD,TM,int>				(dst, mask, src, element_operator); break;
	case ExType::F32:	fnPRV_2D_Binarize_pp<TD,TM,float>				(dst, mask, src, element_operator); break;
	case ExType::F64:	fnPRV_2D_Binarize_pp<TD,TM,double>			(dst, mask, src, element_operator); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM, class TFUNC> static inline void XIE_API fnPRV_2D_Binarize___(TxImage dst, TxImage mask, TxImage src, TFUNC element_operator)
{
	switch(dst.Model.Type)
	{
	case ExType::U8:		fnPRV_2D_Binarize_p_<unsigned char,TM>		(dst, mask, src, element_operator); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

}	// Effectors
}	// xie

#endif
