/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXINTEGRAL_HPP_INCLUDED_
#define _CXINTEGRAL_HPP_INCLUDED_

#include "xie_core.h"

#pragma warning (disable:4127)	// 条件式が定数です.
#pragma warning (disable:4189)	// ローカル変数が初期化されましたが、参照されていません.

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxImage.h"
#include "Core/TxScanner1D.h"
#include "Core/TxScanner2D.h"
#include "Core/CxArrayEx.h"

#include <math.h>

namespace xie
{
namespace Effectors
{

// ======================================================================
template<class TD, class TM, class TS, class TB, class TFUNC> static inline void XIE_API fnPRV_2D_Integral_aa(TxImage dst, TxImage mask, TxImage src, TxArray buf, TFUNC element_operator)
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
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : src_ch);
		TxScanner1D<TB> buf_scan = ToScanner<TB>(buf);

		if (dst_scan.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src_scan.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (buf_scan.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		buf_scan.ForEach([](int x, TB* _buf)
			{
				*_buf = 0;
			});

		for(int y=0 ; y<src_scan.Height ; y++)
		{
			TD*	_dst = dst_scan[y];
			TS*	_src = src_scan[y];
			TB* _buf = buf_scan.Address;
			TM*	_mask = mask_scan.IsValid() ? mask_scan[y] : NULL;
			double sum = 0;
			for(int x=0 ; x<src_scan.Width ; x++)
			{
				if (_mask == NULL || _mask[x] != 0)
					_buf[x * buf_scan.Model.Pack] += element_operator(_src[x * src_scan.Model.Pack + src_field]);
				sum += _buf[x * buf_scan.Model.Pack];
				_dst[x * dst_scan.Model.Pack + dst_field] = (TD)sum;
			}
		}
	}
}

// ======================================================================
template<class TD, class TM, class TB, class TFUNC> static inline void XIE_API fnPRV_2D_Integral_a_(TxImage dst, TxImage mask, TxImage src, TxArray buf, TFUNC element_operator)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Integral_aa<TD,TM,unsigned char,TB>	(dst, mask, src, buf, element_operator);	break;
	case ExType::U16:	fnPRV_2D_Integral_aa<TD,TM,unsigned short,TB>	(dst, mask, src, buf, element_operator);	break;
	case ExType::U32:	fnPRV_2D_Integral_aa<TD,TM,unsigned int,TB>	(dst, mask, src, buf, element_operator);	break;
	case ExType::S8:	fnPRV_2D_Integral_aa<TD,TM,char,TB>			(dst, mask, src, buf, element_operator);	break;
	case ExType::S16:	fnPRV_2D_Integral_aa<TD,TM,short,TB>			(dst, mask, src, buf, element_operator);	break;
	case ExType::S32:	fnPRV_2D_Integral_aa<TD,TM,int,TB>			(dst, mask, src, buf, element_operator);	break;
	case ExType::F32:	fnPRV_2D_Integral_aa<TD,TM,float,TB>			(dst, mask, src, buf, element_operator);	break;
	case ExType::F64:	fnPRV_2D_Integral_aa<TD,TM,double,TB>			(dst, mask, src, buf, element_operator);	break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM, class TB, class TFUNC> static inline void XIE_API fnPRV_2D_Integral___(TxImage dst, TxImage mask, TxImage src, TxArray buf, TFUNC element_operator)
{
	switch(dst.Model.Type)
	{
	case ExType::F64:	fnPRV_2D_Integral_a_<double,TM,TB>	(dst, mask, src, buf, element_operator);	break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
static void XIE_API fnPRV_2D_Integral1(TxImage dst, TxImage mask, TxImage src, TxArray buf)
{
	typedef unsigned char	TM;
	typedef double			TB;

	if (buf.Model.Type != TypeOf<TB>())
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (buf.Model.Pack != 1)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (buf.Length != src.Width)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (src.Width != dst.Width || src.Height != dst.Height)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (MaskValidity<TM>(mask, src) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	fnPRV_2D_Integral___<TM,TB> (dst, mask, src, buf, [](double _src) { return _src; });
}

// ======================================================================
static void XIE_API fnPRV_2D_Integral2(TxImage dst, TxImage mask, TxImage src, TxArray buf)
{
	typedef unsigned char	TM;
	typedef double			TB;

	if (buf.Model.Type != TypeOf<TB>())
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (buf.Model.Pack != 1)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (buf.Length != src.Width)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (src.Width != dst.Width || src.Height != dst.Height)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (MaskValidity<TM>(mask, src) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	fnPRV_2D_Integral___<TM,TB> (dst, mask, src, buf, [](double _src) { return _src * _src; });
}

}	// Effectors
}	// xie

#endif
