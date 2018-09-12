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
template<class TE, class TM, class TFUNC> static inline void XIE_API fnPRV_2D_Ope2A_pp(TxImage dst, TxImage mask, TxImage src, TxImage val, TFUNC element_operator)
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
		int dst_ch = ch / dst.Model.Pack;
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
					_dst[dst_field] = saturate_cast<TE>( element_operator(_src[src_field], _val[val_field]) );
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, val_scan, mask_scan,
				[dst_field,src_field,val_field,element_operator](int y, int x, TE* _dst, TE* _src, TE* _val, TM* _mask)
				{
					if (*_mask != 0)
					{
						_dst[dst_field] = saturate_cast<TE>( element_operator(_src[src_field], _val[val_field]) );
					}
				});
		}
	}
}

// ======================================================================
template<class TM, class TFUNC> static inline void fnPRV_2D_Ope2A_(TxImage dst, TxImage mask, TxImage src, TxImage val, TFUNC element_operator)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Ope2A_pp<unsigned char,TM>		(dst, mask, src, val, element_operator); break;
	case ExType::U16:	fnPRV_2D_Ope2A_pp<unsigned short,TM>	(dst, mask, src, val, element_operator); break;
	case ExType::U32:	fnPRV_2D_Ope2A_pp<unsigned int,TM>		(dst, mask, src, val, element_operator); break;
	case ExType::S8:	fnPRV_2D_Ope2A_pp<char,TM>				(dst, mask, src, val, element_operator); break;
	case ExType::S16:	fnPRV_2D_Ope2A_pp<short,TM>				(dst, mask, src, val, element_operator); break;
	case ExType::S32:	fnPRV_2D_Ope2A_pp<int,TM>				(dst, mask, src, val, element_operator); break;
	case ExType::F32:	fnPRV_2D_Ope2A_pp<float,TM>				(dst, mask, src, val, element_operator); break;
	case ExType::F64:	fnPRV_2D_Ope2A_pp<double,TM>			(dst, mask, src, val, element_operator); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
static void XIE_API fnPRV_2D_Ope2A(TxImage dst, TxImage mask, TxImage src, TxImage val, ExOpe2A mode)
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
	case ExOpe2A::Add:		fnPRV_2D_Ope2A_<TM>(dst, mask, src, val, [](double _src, double _val) { return (_src + _val); }); break;
	case ExOpe2A::Mul:		fnPRV_2D_Ope2A_<TM>(dst, mask, src, val, [](double _src, double _val) { return (_src * _val); }); break;
	case ExOpe2A::Sub:		fnPRV_2D_Ope2A_<TM>(dst, mask, src, val, [](double _src, double _val) { return (_src - _val); }); break;
	case ExOpe2A::Div:		fnPRV_2D_Ope2A_<TM>(dst, mask, src, val, [](double _src, double _val) { return (_val == 0) ? 0 : (_src / _val); }); break;
	case ExOpe2A::Mod:		fnPRV_2D_Ope2A_<TM>(dst, mask, src, val, [](double _src, double _val) { return (_val == 0) ? _src : _mod(_src, _val); }); break;
	case ExOpe2A::Pow:		fnPRV_2D_Ope2A_<TM>(dst, mask, src, val, [](double _src, double _val) { return (pow((double)_src, (double)_val)); }); break;
	case ExOpe2A::Atan2:	fnPRV_2D_Ope2A_<TM>(dst, mask, src, val, [](double _src, double _val) { return (atan2((double)_src, (double)_val)); }); break;
	case ExOpe2A::Diff:		fnPRV_2D_Ope2A_<TM>(dst, mask, src, val, [](double _src, double _val) { return (_abs(_src - _val)); }); break;
	case ExOpe2A::Min:		fnPRV_2D_Ope2A_<TM>(dst, mask, src, val, [](double _src, double _val) { return (XIE_MIN(_src, _val)); }); break;
	case ExOpe2A::Max:		fnPRV_2D_Ope2A_<TM>(dst, mask, src, val, [](double _src, double _val) { return (XIE_MAX(_src, _val)); }); break;
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Ope2A(TxImage dst, TxImage mask, TxImage src, TxImage val, ExOpe2A mode)
{
	try
	{
		if (dst.Model.Type == src.Model.Type &&
			src.Model.Type == val.Model.Type)
		{
			fnPRV_2D_Ope2A(dst, mask, src, val, mode);
		}
		else
		{
			int dst_pxc = Axi::Max(src.Model.Pack * src.Channels, val.Model.Pack * val.Channels);

			CxImage tmp0(dst.Width, dst.Height, TxModel::F64(dst_pxc), 1);
			CxImage tmp1 = CxImage::FromTag(src).Clone(TxModel::F64(src.Model.Pack), src.Channels);
			CxImage tmp2 = CxImage::FromTag(val).Clone(TxModel::F64(val.Model.Pack), val.Channels);
			fnPRV_2D_Ope2A(tmp0.Tag(), mask, tmp1.Tag(), tmp2.Tag(), mode);

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
