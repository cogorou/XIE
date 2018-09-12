/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_core.h"
#include "api_data.h"

#pragma warning (disable:4127)	// 条件式が定数です.
#pragma warning (disable:4189)	// ローカル変数が初期化されましたが、参照されていません.

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxImage.h"
#include "Core/TxScanner2D.h"

#include <math.h>

namespace xie
{

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_Not_pp(TxImage dst, TxImage mask, TxImage src)
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
				[dst_field,src_field](int y, int x, TD* _dst, TS* _src)
				{
					_dst[dst_field] = saturate_cast<TD>(_not(_src[src_field]));
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[dst_field,src_field](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
					{
						_dst[dst_field] = saturate_cast<TD>(_not(_src[src_field]));
					}
				});
		}
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_Not__p(TxImage dst, TxImage mask, TxImage src)
{
	fnPRV_2D_Not_pp<TS,TM,TS>		(dst, mask, src);
}

// ======================================================================
template<class TM> static inline void XIE_API fnPRV_2D_Not___(TxImage dst, TxImage mask, TxImage src)
{
	switch(src.Model.Type)
	{
	case ExType::S8:
	case ExType::U8:	fnPRV_2D_Not__p<TM,unsigned char>	(dst, mask, src); break;
	case ExType::S16:
	case ExType::U16:	fnPRV_2D_Not__p<TM,unsigned short>	(dst, mask, src); break;
	case ExType::S32:
	case ExType::U32:	fnPRV_2D_Not__p<TM,unsigned int>	(dst, mask, src); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
static void XIE_API fnPRV_2D_Not(TxImage dst, TxImage mask, TxImage src)
{
	typedef unsigned char	TM;

	if (MaskValidity<TM>(mask, dst) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (dst.Model.Type != src.Model.Type)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	fnPRV_2D_Not___<TM>(dst, mask, src);
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Not(TxImage dst, TxImage mask, TxImage src)
{
	try
	{
		if (dst.Model.Type == src.Model.Type)
		{
			fnPRV_2D_Not(dst, mask, src);
		}
		else
		{
			CxImage dst_tmp(dst.Width, dst.Height, src.Model, src.Channels);
			fnPRV_2D_Not(dst_tmp.Tag(), mask, src);

			CxImage dst_act;
			dst_act.Attach(dst);
			dst_act.Filter().Cast(dst_tmp);
		}
		
		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
