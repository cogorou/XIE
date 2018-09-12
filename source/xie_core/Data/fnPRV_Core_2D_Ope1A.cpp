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
template<class TE, class TM, class TV, class TFUNC> static inline void XIE_API fnPRV_2D_Ope1A_pp(TxImage dst, TxImage mask, TxImage src, TV value, TFUNC element_operator)
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
					_dst[dst_field] = saturate_cast<TE>( element_operator(_src[src_field], value) );
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[dst_field,src_field,value,element_operator](int y, int x, TE* _dst, TE* _src, TM* _mask)
				{
					if (*_mask != 0)
					{
						_dst[dst_field] = saturate_cast<TE>( element_operator(_src[src_field], value) );
					}
				});
		}
	}
}

// ======================================================================
template<class TM, class TV, class TFUNC> static inline void fnPRV_2D_Ope1A_(TxImage dst, TxImage mask, TxImage src, TV value, TFUNC element_operator)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Ope1A_pp<unsigned char,TM>		(dst, mask, src, value, element_operator); break;
	case ExType::U16:	fnPRV_2D_Ope1A_pp<unsigned short,TM>	(dst, mask, src, value, element_operator); break;
	case ExType::U32:	fnPRV_2D_Ope1A_pp<unsigned int,TM>		(dst, mask, src, value, element_operator); break;
	case ExType::S8:	fnPRV_2D_Ope1A_pp<char,TM>				(dst, mask, src, value, element_operator); break;
	case ExType::S16:	fnPRV_2D_Ope1A_pp<short,TM>				(dst, mask, src, value, element_operator); break;
	case ExType::S32:	fnPRV_2D_Ope1A_pp<int,TM>				(dst, mask, src, value, element_operator); break;
	case ExType::F32:	fnPRV_2D_Ope1A_pp<float,TM>				(dst, mask, src, value, element_operator); break;
	case ExType::F64:	fnPRV_2D_Ope1A_pp<double,TM>			(dst, mask, src, value, element_operator); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
static void XIE_API fnPRV_2D_Ope1A(TxImage dst, TxImage mask, TxImage src, double value, ExOpe1A mode)
{
	typedef unsigned char	TM;

	if (MaskValidity<TM>(mask, dst) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (dst.Model.Type != src.Model.Type)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	switch(mode)
	{
	case ExOpe1A::Add:		fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (_src + _val); }); break;
	case ExOpe1A::Mul:		fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (_src * _val); }); break;
	case ExOpe1A::Sub:		fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (_src - _val); }); break;
	case ExOpe1A::SubInv:	fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (_val - _src); }); break;
	case ExOpe1A::Div:		fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (_val == 0) ? 0 : (_src / _val); }); break;
	case ExOpe1A::DivInv:	fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (_src == 0) ? 0 : (_val / _src); }); break;
	case ExOpe1A::Mod:		fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (_val == 0) ? _src : _mod(_src, _val); }); break;
	case ExOpe1A::ModInv:	fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (_src == 0) ? _val : _mod(_val, _src); }); break;
	case ExOpe1A::Pow:		fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (pow((double)_src, (double)_val)); }); break;
	case ExOpe1A::PowInv:	fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (pow((double)_val, (double)_src)); }); break;
	case ExOpe1A::Atan2:	fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (atan2((double)_src, (double)_val)); }); break;
	case ExOpe1A::Atan2Inv:	fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (atan2((double)_val, (double)_src)); }); break;
	case ExOpe1A::Diff:		fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (_abs(_src - _val)); }); break;
	case ExOpe1A::Min:		fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (XIE_MIN(_src, _val)); }); break;
	case ExOpe1A::Max:		fnPRV_2D_Ope1A_<TM>(dst, mask, src, value, [](double _src, double _val) { return (XIE_MAX(_src, _val)); }); break;
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Ope1A(TxImage dst, TxImage mask, TxImage src, double value, ExOpe1A mode)
{
	try
	{
		if (dst.Model.Type == src.Model.Type)
		{
			fnPRV_2D_Ope1A(dst, mask, src, value, mode);
		}
		else
		{
			CxImage dst_tmp = CxImage::FromTag(src).Clone(TxModel::F64(src.Model.Pack));
			fnPRV_2D_Ope1A(dst_tmp.Tag(), mask, dst_tmp.Tag(), value, mode);

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
