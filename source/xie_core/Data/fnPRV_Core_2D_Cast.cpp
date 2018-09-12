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
#include "Core/TxScanner2D.h"

#include <math.h>

namespace xie
{

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_Cast_aa(TxImage dst, TxImage mask, TxImage src)
{
	int min_channels = Axi::Min(dst.Channels, src.Channels);
	for(int ch=0 ; ch<min_channels ; ch++)
	{
		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, ch);
		TxScanner2D<TS> src_scan = ToScanner<TS>(src, ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : ch);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src_scan,
				[](int y, int x, TD* _dst, TS* _src)
				{
					*_dst = (TD)*_src;
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
						*_dst = (TD)*_src;
				});
		}
	}
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_Cast_pp(TxImage dst, TxImage mask, TxImage src)
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
					_dst[dst_field] = (TD)_src[src_field];
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[dst_field,src_field](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
					{
						_dst[dst_field] = (TD)_src[src_field];
					}
				});
		}
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_Cast__p(TxImage dst, TxImage mask, TxImage src)
{
	if (src.Model.Pack == 1 && dst.Model.Pack == 1 && src.Channels == dst.Channels)
	{
		switch(dst.Model.Type)
		{
		case ExType::S8:		
		case ExType::U8:	fnPRV_2D_Cast_aa<unsigned char,TM,TS>		(dst, mask, src);	break;
		case ExType::S16:	
		case ExType::U16:	fnPRV_2D_Cast_aa<unsigned short,TM,TS>		(dst, mask, src);	break;
		case ExType::S32:	
		case ExType::F32:	
		case ExType::U32:	fnPRV_2D_Cast_aa<unsigned int,TM,TS>		(dst, mask, src);	break;
		case ExType::F64:	
		case ExType::S64:	
		case ExType::U64:	fnPRV_2D_Cast_aa<unsigned long long,TM,TS>	(dst, mask, src);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
	}
	else
	{
		switch(dst.Model.Type)
		{
		case ExType::S8:		
		case ExType::U8:	fnPRV_2D_Cast_pp<unsigned char,TM,TS>		(dst, mask, src);	break;
		case ExType::S16:	
		case ExType::U16:	fnPRV_2D_Cast_pp<unsigned short,TM,TS>		(dst, mask, src);	break;
		case ExType::S32:	
		case ExType::F32:	
		case ExType::U32:	fnPRV_2D_Cast_pp<unsigned int,TM,TS>		(dst, mask, src);	break;
		case ExType::F64:	
		case ExType::S64:	
		case ExType::U64:	fnPRV_2D_Cast_pp<unsigned long long,TM,TS>	(dst, mask, src);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
	}
}

// ======================================================================
template<class TM> static inline void XIE_API fnPRV_2D_Cast___(TxImage dst, TxImage mask, TxImage src)
{
	switch(src.Model.Type)
	{
	case ExType::S8:		
	case ExType::U8:	fnPRV_2D_Cast__p<TM,unsigned char>		(dst, mask, src);	break;
	case ExType::S16:	
	case ExType::U16:	fnPRV_2D_Cast__p<TM,unsigned short>		(dst, mask, src);	break;
	case ExType::S32:	
	case ExType::F32:	
	case ExType::U32:	fnPRV_2D_Cast__p<TM,unsigned int>		(dst, mask, src);	break;
	case ExType::U64:	
	case ExType::F64:	
	case ExType::S64:	fnPRV_2D_Cast__p<TM,long long>			(dst, mask, src);	break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Cast(TxImage dst, TxImage mask, TxImage src)
{
	typedef unsigned char	TM;

	try
	{
		if (MaskValidity<TM>(mask, dst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		fnPRV_2D_Cast___<TM>(dst, mask, src);

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
