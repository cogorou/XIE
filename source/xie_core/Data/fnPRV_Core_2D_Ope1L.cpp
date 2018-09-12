/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_core.h"
#include "api_data.h"

#pragma warning (disable:4127)	// 条件式が定数です.
#pragma warning (disable:4189)	// ローカル変数が初期化されましたが、参照されていません.

#include <math.h>
#include <functional>

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxImage.h"
#include "Core/TxScanner2D.h"

namespace xie
{

// ======================================================================
template<class TE, class TM, class TV, class TFUNC> static inline void XIE_API fnPRV_2D_Ope1L_pp(TxImage dst, TxImage mask, TxImage src, TV value, TFUNC element_operator)
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

		TxScanner2D<TE> dst_scan = ToScanner<TE>(dst, dst_ch);
		TxScanner2D<TE> src_scan = ToScanner<TE>(src, src_ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : dst_ch);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src_scan,
				[dst_field,src_field,value,element_operator](int y, int x, TE* _dst, TE* _src)
				{
					_dst[dst_field] = (TE)element_operator(_src[src_field], value);
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[dst_field,src_field,value,element_operator](int y, int x, TE* _dst, TE* _src, TM* _mask)
				{
					if (*_mask != 0)
					{
						_dst[dst_field] = (TE)element_operator(_src[src_field], value);
					}
				});
		}
	}
}

// ======================================================================
template<class TM, class TV, class TFUNC> static inline void fnPRV_2D_Ope1L_(TxImage dst, TxImage mask, TxImage src, TV value, TFUNC element_operator)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Ope1L_pp<unsigned char,TM>		(dst, mask, src, value, element_operator); break;
	case ExType::U16:	fnPRV_2D_Ope1L_pp<unsigned short,TM>	(dst, mask, src, value, element_operator); break;
	case ExType::U32:	fnPRV_2D_Ope1L_pp<unsigned int,TM>		(dst, mask, src, value, element_operator); break;
	case ExType::S8:	fnPRV_2D_Ope1L_pp<char,TM>				(dst, mask, src, value, element_operator); break;
	case ExType::S16:	fnPRV_2D_Ope1L_pp<short,TM>				(dst, mask, src, value, element_operator); break;
	case ExType::S32:	fnPRV_2D_Ope1L_pp<int,TM>				(dst, mask, src, value, element_operator); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
static void XIE_API fnPRV_2D_Ope1L(TxImage dst, TxImage mask, TxImage src, unsigned int value, ExOpe1L mode)
{
	typedef unsigned char	TM;

	if (MaskValidity<TM>(mask, dst) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (dst.Model.Type != src.Model.Type)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	switch(mode)
	{
	case ExOpe1L::And:		fnPRV_2D_Ope1L_<TM>(dst, mask, src, value, [](unsigned int _src, unsigned int _val) { return  (_src & _val); }); break;
	case ExOpe1L::Nand:		fnPRV_2D_Ope1L_<TM>(dst, mask, src, value, [](unsigned int _src, unsigned int _val) { return ~(_src & _val); }); break;
	case ExOpe1L::Or:		fnPRV_2D_Ope1L_<TM>(dst, mask, src, value, [](unsigned int _src, unsigned int _val) { return  (_src | _val); }); break;
	case ExOpe1L::Xor:		fnPRV_2D_Ope1L_<TM>(dst, mask, src, value, [](unsigned int _src, unsigned int _val) { return  (_src ^ _val); }); break;
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Ope1L(TxImage dst, TxImage mask, TxImage src, unsigned int value, ExOpe1L mode)
{
	try
	{
		if (dst.Model.Type == src.Model.Type)
		{
			fnPRV_2D_Ope1L(dst, mask, src, value, mode);
		}
		else
		{
			CxImage dst_tmp = CxImage::FromTag(src).Clone(TxModel::U32(src.Model.Pack));
			fnPRV_2D_Ope1L(dst_tmp.Tag(), mask, dst_tmp.Tag(), value, mode);

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
