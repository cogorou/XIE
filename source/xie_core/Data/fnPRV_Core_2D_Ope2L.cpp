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
template<class TE, class TM, class TFUNC> static inline void XIE_API fnPRV_2D_Ope2L_pp(TxImage dst, TxImage mask, TxImage src, TxImage val, TFUNC element_operator)
{
	int dst_pxc = dst.Model.Pack * dst.Channels;
	int src_pxc = src.Model.Pack * src.Channels;
	int val_pxc = val.Model.Pack * val.Channels;

	int min_pxc;

	if (src_pxc == 1 && val_pxc == 1)
		min_pxc = dst_pxc;
	else if (src_pxc != 1 && val_pxc == 1)
		min_pxc = Axi::Min(dst_pxc, src_pxc);
	else if (src_pxc == 1 && val_pxc != 1)
		min_pxc = Axi::Min(dst_pxc, val_pxc);
	else
		min_pxc = Axi::Min(dst_pxc, Axi::Min(src_pxc, val_pxc));

	for(int ch=0 ; ch<min_pxc ; ch++)
	{
		int dst_ch    = ch / dst.Model.Pack;
		int dst_field = ch % dst.Model.Pack;

		int src_ch    = (src_pxc == 1) ? 0 : ch / src.Model.Pack;
		int src_field = (src_pxc == 1) ? 0 : ch % src.Model.Pack;

		int val_ch    = (val_pxc == 1) ? 0 : ch / val.Model.Pack;
		int val_field = (val_pxc == 1) ? 0 : ch % val.Model.Pack;

		TxScanner2D<TE> dst_scan = ToScanner<TE>(dst, dst_ch);
		TxScanner2D<TE> src_scan = ToScanner<TE>(src, src_ch);
		TxScanner2D<TE> val_scan = ToScanner<TE>(val, val_ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : dst_ch);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src_scan, val_scan,
				[dst_field,src_field,val_field,element_operator](int y, int x, TE* _dst, TE* _src, TE* _val)
				{
					_dst[dst_field] = (TE)element_operator(_src[src_field], _val[val_field]);
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, val_scan, mask_scan,
				[dst_field,src_field,val_field,element_operator](int y, int x, TE* _dst, TE* _src, TE* _val, TM* _mask)
				{
					if (*_mask != 0)
					{
						_dst[dst_field] = (TE)element_operator(_src[src_field], _val[val_field]);
					}
				});
		}
	}
}

// ======================================================================
template<class TM, class TFUNC> static inline void fnPRV_2D_Ope2L_(TxImage dst, TxImage mask, TxImage src, TxImage val, TFUNC element_operator)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Ope2L_pp<unsigned char,TM>		(dst, mask, src, val, element_operator); break;
	case ExType::U16:	fnPRV_2D_Ope2L_pp<unsigned short,TM>	(dst, mask, src, val, element_operator); break;
	case ExType::U32:	fnPRV_2D_Ope2L_pp<unsigned int,TM>		(dst, mask, src, val, element_operator); break;
	case ExType::S8:	fnPRV_2D_Ope2L_pp<char,TM>				(dst, mask, src, val, element_operator); break;
	case ExType::S16:	fnPRV_2D_Ope2L_pp<short,TM>				(dst, mask, src, val, element_operator); break;
	case ExType::S32:	fnPRV_2D_Ope2L_pp<int,TM>				(dst, mask, src, val, element_operator); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
static void XIE_API fnPRV_2D_Ope2L(TxImage dst, TxImage mask, TxImage src, TxImage val, ExOpe2L mode)
{
	typedef unsigned char	TM;

	if (MaskValidity<TM>(mask, dst) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (dst.Model.Type != src.Model.Type)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (dst.Model.Type != val.Model.Type)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	switch(mode)
	{
	case ExOpe2L::And:		fnPRV_2D_Ope2L_<TM>(dst, mask, src, val, [](unsigned int _src, unsigned int _val) { return  (_src & _val); }); break;
	case ExOpe2L::Nand:		fnPRV_2D_Ope2L_<TM>(dst, mask, src, val, [](unsigned int _src, unsigned int _val) { return ~(_src & _val); }); break;
	case ExOpe2L::Or:		fnPRV_2D_Ope2L_<TM>(dst, mask, src, val, [](unsigned int _src, unsigned int _val) { return  (_src | _val); }); break;
	case ExOpe2L::Xor:		fnPRV_2D_Ope2L_<TM>(dst, mask, src, val, [](unsigned int _src, unsigned int _val) { return  (_src ^ _val); }); break;
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Ope2L(TxImage dst, TxImage mask, TxImage src, TxImage val, ExOpe2L mode)
{
	try
	{
		if (dst.Model.Type == src.Model.Type &&
			src.Model.Type == val.Model.Type)
		{
			fnPRV_2D_Ope2L(dst, mask, src, val, mode);
		}
		else
		{
			int dst_pxc = Axi::Max(src.Model.Pack * src.Channels, val.Model.Pack * val.Channels);

			CxImage tmp0(dst.Width, dst.Height, TxModel::U32(dst_pxc), 1);
			CxImage tmp1 = CxImage::FromTag(src).Clone(TxModel::U32(src.Model.Pack), src.Channels);
			CxImage tmp2 = CxImage::FromTag(val).Clone(TxModel::U32(val.Model.Pack), val.Channels);
			fnPRV_2D_Ope2L(tmp0.Tag(), mask, tmp1.Tag(), tmp2.Tag(), mode);

			CxImage dst_act;
			dst_act.Attach(dst);
			dst_act.Filter().Cast(tmp0);
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
