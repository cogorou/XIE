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
template<class TD, class TM, class TS, class TFUNC> static inline void XIE_API fnPRV_2D_GeoTrans_pp(TxImage dst, TxImage mask, TxImage src, TFUNC element_operator)
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

		if (dst_scan.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (mask_scan.IsValid() == false)
		{
			src_scan.ForEach( 
				[dst_field,src_field,element_operator,&dst_scan](int y, int x, TS* _src)
				{
					TxPointI dp = element_operator(x, y);
					TD* _dst = &dst_scan(dp.Y, dp.X);
					_dst[dst_field] = _src[src_field];
				});
		}
		else
		{
			src_scan.ForEach(mask_scan,
				[dst_field,src_field,element_operator,&dst_scan](int y, int x, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
					{
						TxPointI dp = element_operator(x, y);
						TD* _dst = &dst_scan(dp.Y, dp.X);
						_dst[dst_field] = _src[src_field];
					}
				});
		}
	}
}

// ======================================================================
template<class TM, class TS, class TFUNC> static inline void XIE_API fnPRV_2D_GeoTrans_p_(TxImage dst, TxImage mask, TxImage src, TFUNC element_operator)
{
	fnPRV_2D_GeoTrans_pp<TS,TM,TS>	(dst, mask, src, element_operator);
}

// ======================================================================
template<class TM, class TFUNC> static inline void XIE_API fnPRV_2D_GeoTrans___(TxImage dst, TxImage mask, TxImage src, TFUNC element_operator)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_GeoTrans_p_<TM,unsigned char>	(dst, mask, src, element_operator);	break;
	case ExType::U16:	fnPRV_2D_GeoTrans_p_<TM,unsigned short>	(dst, mask, src, element_operator);	break;
	case ExType::U32:	fnPRV_2D_GeoTrans_p_<TM,unsigned int>	(dst, mask, src, element_operator);	break;
	case ExType::S8:	fnPRV_2D_GeoTrans_p_<TM,char>			(dst, mask, src, element_operator);	break;
	case ExType::S16:	fnPRV_2D_GeoTrans_p_<TM,short>			(dst, mask, src, element_operator);	break;
	case ExType::S32:	fnPRV_2D_GeoTrans_p_<TM,int>			(dst, mask, src, element_operator);	break;
	case ExType::F32:	fnPRV_2D_GeoTrans_p_<TM,float>			(dst, mask, src, element_operator);	break;
	case ExType::F64:	fnPRV_2D_GeoTrans_p_<TM,double>			(dst, mask, src, element_operator);	break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
static void XIE_API fnPRV_2D_Mirror(TxImage dst, TxImage mask, TxImage src, int mode)
{
	typedef unsigned char	TM;

	if (src.Model.Type != dst.Model.Type)
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	if (src.Width != dst.Width || src.Height != dst.Height)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (MaskValidity<TM>(mask, src) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	int w = src.Width;
	int h = src.Height;

	fnPRV_2D_GeoTrans___<TM> (dst, mask, src,
		[mode,w,h](int x, int y) -> TxPointI
		{
			int dx;
			int dy;
			switch(mode)
			{
			default:
				dx = x;
				dy = y;
				break;
			case 1:
				dx = w - x - 1;
				dy = y;
				break;
			case 2:
				dx = x;
				dy = h - y - 1;
				break;
			case 3:
				dx = w - x - 1;
				dy = h - y - 1;
				break;
			}
			return TxPointI(dx, dy);
		}
	);
}

// ======================================================================
static void XIE_API fnPRV_2D_Rotate(TxImage dst, TxImage mask, TxImage src, int mode)
{
	typedef unsigned char	TM;

	if (src.Model.Type != dst.Model.Type)
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	if (MaskValidity<TM>(mask, src) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	switch(mode)
	{
	case +1:
	case -1:
		if (dst.Width != src.Height || dst.Height != src.Width)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		break;
	default:
		if (dst.Width != src.Width || dst.Height != src.Height)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		break;
	}

	int w = src.Width;
	int h = src.Height;

	fnPRV_2D_GeoTrans___<TM> (dst, mask, src,
		[mode,w,h](int x, int y) -> TxPointI
		{
			int dx;
			int dy;
			switch(mode)
			{
			default:
				dx = x;
				dy = y;
				break;
			case +1:
				dx = h - y - 1;
				dy = x;
				break;
			case -1:
				dx = y;
				dy = w - x - 1;
				break;
			case +2:
			case -2:
				dx = w - x - 1;
				dy = h - y - 1;
				break;
			}
			return TxPointI(dx, dy);
		}
	);
}

// ======================================================================
static void XIE_API fnPRV_2D_Transpose(TxImage dst, TxImage mask, TxImage src)
{
	typedef unsigned char	TM;

	if (src.Model.Type != dst.Model.Type)
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	if (src.Width != dst.Height || src.Height != dst.Width)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (MaskValidity<TM>(mask, src) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	fnPRV_2D_GeoTrans___<TM> (dst, mask, src,
		[](int x, int y) -> TxPointI
		{
			return TxPointI(y, x);
		}
	);
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Mirror(TxImage dst, TxImage mask, TxImage src, int mode)
{
	try
	{
		if (dst.Model.Type == src.Model.Type)
		{
			fnPRV_2D_Mirror(dst, mask, src, mode);
		}
		else
		{
			TxModel dst_model(src.Model.Type, dst.Model.Pack);
			CxImage dst_tmp(dst.Width, dst.Height, dst_model, dst.Channels);
			fnPRV_2D_Mirror(dst_tmp.Tag(), mask, src, mode);

			CxImage dst_act;
			dst_act.Attach(dst);
			dst_act.Filter().Copy(dst_tmp);
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Rotate(TxImage dst, TxImage mask, TxImage src, int mode)
{
	try
	{
		if (dst.Model.Type == src.Model.Type)
		{
			fnPRV_2D_Rotate(dst, mask, src, mode);
		}
		else
		{
			TxModel dst_model(src.Model.Type, dst.Model.Pack);
			CxImage dst_tmp(dst.Width, dst.Height, dst_model, dst.Channels);
			fnPRV_2D_Rotate(dst_tmp.Tag(), mask, src, mode);

			CxImage dst_act;
			dst_act.Attach(dst);
			dst_act.Filter().Copy(dst_tmp);
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Transpose(TxImage dst, TxImage mask, TxImage src)
{
	try
	{
		if (dst.Model.Type == src.Model.Type)
		{
			fnPRV_2D_Transpose(dst, mask, src);
		}
		else
		{
			TxModel dst_model(src.Model.Type, dst.Model.Pack);
			CxImage dst_tmp(dst.Width, dst.Height, dst_model, dst.Channels);
			fnPRV_2D_Transpose(dst_tmp.Tag(), mask, src);

			CxImage dst_act;
			dst_act.Attach(dst);
			dst_act.Filter().Copy(dst_tmp);
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
