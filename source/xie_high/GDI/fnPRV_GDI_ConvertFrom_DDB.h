/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _FNfnPRV_GDI__CONVERTFROM_DDB_H_INCLUDED_
#define _FNfnPRV_GDI__CONVERTFROM_DDB_H_INCLUDED_

#include "xie_high.h"
#include "api_gdi.h"
#include "Core/xie_core_defs.h"
#include "Core/xie_core_math.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/TxImage.h"
#include "Core/TxScanner2D.h"
#include "Core/TxRGB8x3.h"
#include "Core/TxRGB8x4.h"
#include "Core/TxBGR8x3.h"
#include "Core/TxBGR8x4.h"

namespace xie
{
namespace GDI
{

// ======================================================================
template<class TD, class TS> static inline void XIE_API fnPRV_GDI_ConvertFrom_DDB_gp(TxImage dst, TxImage src)
{
	TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, 0);
	TxScanner2D<TS> src_scan = ToScanner<TS>(src, 0);

	for(int y=0 ; y<src_scan.Height ; y++)
	{
		TD* _dst = dst_scan[y];							// TopDown
		TS* _src = src_scan[src_scan.Height - y - 1];	// BottomUp

		for(int x=0 ; x<src_scan.Width ; x++)
		{
			_dst[x] = saturate_cast<TD>(xie::Axi::RgbToGray(_src[x].R, _src[x].G, _src[x].B));
		}
	}
}

// ======================================================================
template<class TD, class TS> static inline void XIE_API fnPRV_GDI_ConvertFrom_DDB_pp(TxImage dst, TxImage src)
{
	TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, 0);
	TxScanner2D<TS> src_scan = ToScanner<TS>(src, 0);

	for(int y=0 ; y<src_scan.Height ; y++)
	{
		TD* _dst = dst_scan[y];							// TopDown
		TS* _src = src_scan[src_scan.Height - y - 1];	// BottomUp

		for(int x=0 ; x<src_scan.Width ; x++)
		{
			_dst[x].R = _src[x].R;	// R
			_dst[x].G = _src[x].G;	// G
			_dst[x].B = _src[x].B;	// B
		}
	}
}

// ======================================================================
template<class TD, class TS> static inline void XIE_API fnPRV_GDI_ConvertFrom_DDB_up(TxImage dst, TxImage src)
{
	TxScanner2D<TD> dst_scan0 = ToScanner<TD>(dst, 0);
	TxScanner2D<TD> dst_scan1 = ToScanner<TD>(dst, 1);
	TxScanner2D<TD> dst_scan2 = ToScanner<TD>(dst, 2);
	TxScanner2D<TS> src_scan = ToScanner<TS>(src, 0);

	for(int y=0 ; y<src_scan.Height ; y++)
	{
		TD* _dst0 = dst_scan0[y];						// TopDown
		TD* _dst1 = dst_scan1[y];						// TopDown
		TD* _dst2 = dst_scan2[y];						// TopDown
		TS* _src = src_scan[src_scan.Height - y - 1];	// BottomUp

		for(int x=0 ; x<src_scan.Width ; x++)
		{
			_dst0[x] = _src[x].R;	// R
			_dst1[x] = _src[x].G;	// G
			_dst2[x] = _src[x].B;	// B
		}
	}
}

// ======================================================================
template<class TS> static inline void XIE_API fnPRV_GDI_ConvertFrom_DDB__p(TxImage dst, TxImage src)
{
	switch(dst.Channels)
	{
	case 1:
		switch(dst.Model.Type)
		{
		case ExType::U8:
			switch(dst.Model.Pack)
			{
			case 1:	fnPRV_GDI_ConvertFrom_DDB_gp<unsigned char,TS>	(dst, src);	break;
			case 3:	fnPRV_GDI_ConvertFrom_DDB_pp<TxRGB8x3,TS>			(dst, src);	break;
			case 4:	fnPRV_GDI_ConvertFrom_DDB_pp<TxRGB8x4,TS>			(dst, src);	break;
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 3:
	case 4:
		switch(dst.Model.Type)
		{
		case ExType::U8:
			switch(dst.Model.Pack)
			{
			case 1:	fnPRV_GDI_ConvertFrom_DDB_up<unsigned char,TS>	(dst, src);	break;
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
static inline void XIE_API fnPRV_GDI_ConvertFrom_DDB___(TxImage dst, TxImage src)
{
	switch(src.Channels)
	{
	case 1:
		switch(src.Model.Type)
		{
		case ExType::U8:
			switch(src.Model.Pack)
			{
			case 3:	fnPRV_GDI_ConvertFrom_DDB__p<TxBGR8x3>	(dst, src);	break;
			case 4:	fnPRV_GDI_ConvertFrom_DDB__p<TxBGR8x4>	(dst, src);	break;
				break;
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

}
}

#endif
