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
template<class TD, class TM, class TS, class TFUNC> static inline void XIE_API fnPRV_2D_Compare_pp(TxImage dst, TxImage mask, TxImage src, TxImage cmp, TFUNC element_operator)
{
	int dst_pxc = dst.Model.Pack * dst.Channels;
	int src_pxc = src.Model.Pack * src.Channels;
	int cmp_pxc = cmp.Model.Pack * cmp.Channels;

	int min_pxc;

	if (src_pxc == 1 && cmp_pxc == 1)
		min_pxc = dst_pxc;
	else if (src_pxc != 1 && cmp_pxc == 1)
		min_pxc = Axi::Min(dst_pxc, src_pxc);
	else if (src_pxc == 1 && cmp_pxc != 1)
		min_pxc = Axi::Min(dst_pxc, cmp_pxc);
	else
		min_pxc = Axi::Min(dst_pxc, Axi::Min(src_pxc, cmp_pxc));

	for(int ch=0 ; ch<min_pxc ; ch++)
	{
		int dst_ch    = ch / dst.Model.Pack;
		int dst_field = ch % dst.Model.Pack;

		int src_ch    = (src_pxc == 1) ? 0 : ch / src.Model.Pack;
		int src_field = (src_pxc == 1) ? 0 : ch % src.Model.Pack;

		int cmp_ch    = (cmp_pxc == 1) ? 0 : ch / cmp.Model.Pack;
		int cmp_field = (cmp_pxc == 1) ? 0 : ch % cmp.Model.Pack;

		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, dst_ch);
		TxScanner2D<TS> src_scan = ToScanner<TS>(src, src_ch);
		TxScanner2D<TS> cmp_scan = ToScanner<TS>(cmp, cmp_ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : dst_ch);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src_scan, cmp_scan,
				[dst_field,src_field,cmp_field,element_operator](int y, int x, TD* _dst, TS* _src, TS* _cmp)
				{
					_dst[dst_field] = element_operator(_src[src_field], _cmp[cmp_field]);
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, cmp_scan, mask_scan,
				[dst_field,src_field,cmp_field,element_operator](int y, int x, TD* _dst, TS* _src, TS* _cmp, TM* _mask)
				{
					if (*_mask != 0)
					{
						_dst[dst_field] = element_operator(_src[src_field], _cmp[cmp_field]);
					}
				});
		}
	}
}

// ======================================================================
template<class TD, class TM, class TFUNC> static inline void XIE_API fnPRV_2D_Compare_p_(TxImage dst, TxImage mask, TxImage src, TxImage cmp, TFUNC element_operator)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Compare_pp<TD,TM,unsigned char>	(dst, mask, src, cmp, element_operator); break;
	case ExType::U16:	fnPRV_2D_Compare_pp<TD,TM,unsigned short>	(dst, mask, src, cmp, element_operator); break;
	case ExType::U32:	fnPRV_2D_Compare_pp<TD,TM,unsigned int>		(dst, mask, src, cmp, element_operator); break;
	case ExType::S8:	fnPRV_2D_Compare_pp<TD,TM,char>				(dst, mask, src, cmp, element_operator); break;
	case ExType::S16:	fnPRV_2D_Compare_pp<TD,TM,short>			(dst, mask, src, cmp, element_operator); break;
	case ExType::S32:	fnPRV_2D_Compare_pp<TD,TM,int>				(dst, mask, src, cmp, element_operator); break;
	case ExType::F32:	fnPRV_2D_Compare_pp<TD,TM,float>			(dst, mask, src, cmp, element_operator); break;
	case ExType::F64:	fnPRV_2D_Compare_pp<TD,TM,double>			(dst, mask, src, cmp, element_operator); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM, class TFUNC> static inline void XIE_API fnPRV_2D_Compare___(TxImage dst, TxImage mask, TxImage src, TxImage cmp, TFUNC element_operator)
{
	switch(dst.Model.Type)
	{
	case ExType::U8:		fnPRV_2D_Compare_p_<unsigned char,TM>	(dst, mask, src, cmp, element_operator); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
static void XIE_API fnPRV_2D_Compare(TxImage dst, TxImage mask, TxImage src, TxImage cmp, double error_range)
{
	typedef unsigned char	TM;

	if (MaskValidity<TM>(mask, dst) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (src.Model.Type != cmp.Model.Type)
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

	fnPRV_2D_Compare___<TM>(dst, mask, src, cmp, [error_range](double _src, double _cmp) { return (_abs(_src - _cmp) > error_range) ? 1 : 0; }); 
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Compare(TxImage dst, TxImage mask, TxImage src, TxImage cmp, double error_range)
{
	try
	{
		if (dst.Model.Type == ExType::U8 &&
			src.Model.Type == cmp.Model.Type)
		{
			fnPRV_2D_Compare(dst, mask, src, cmp, error_range);
		}
		else
		{
			int dst_pxc = Axi::Max(src.Model.Pack * src.Channels, cmp.Model.Pack * cmp.Channels);

			CxImage tmp0(dst.Width, dst.Height, TxModel::U8(dst_pxc), 1);
			CxImage tmp1 = CxImage::FromTag(src).Clone(TxModel::F64(src.Model.Pack), src.Channels);
			CxImage tmp2 = CxImage::FromTag(cmp).Clone(TxModel::F64(cmp.Model.Pack), cmp.Channels);
			fnPRV_2D_Compare(tmp0.Tag(), mask, tmp1.Tag(), tmp2.Tag(), error_range);

			CxImage dst_act;
			dst_act.Attach(dst);
			dst_act.Filter().Copy(tmp0);
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
