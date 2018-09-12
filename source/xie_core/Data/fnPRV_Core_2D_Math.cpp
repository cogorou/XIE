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
template<class TE, class TM, class TFUNC> static inline void XIE_API fnPRV_2D_Math_pp(TxImage dst, TxImage mask, TxImage src, TFUNC element_operator)
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
				[dst_field,src_field,element_operator](int y, int x, TE* _dst, TE* _src)
				{
					_dst[dst_field] = saturate_cast<TE>( element_operator(_src[src_field]) );
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[dst_field,src_field,element_operator](int y, int x, TE* _dst, TE* _src, TM* _mask)
				{
					if (*_mask != 0)
					{
						_dst[dst_field] = saturate_cast<TE>( element_operator(_src[src_field]) );
					}
				});
		}
	}
}

// ======================================================================
template<class TM, class TFUNC> static inline void fnPRV_2D_Math_(TxImage dst, TxImage mask, TxImage src, TFUNC element_operator)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Math_pp<unsigned char,TM>	(dst, mask, src, element_operator); break;
	case ExType::U16:	fnPRV_2D_Math_pp<unsigned short,TM>	(dst, mask, src, element_operator); break;
	case ExType::U32:	fnPRV_2D_Math_pp<unsigned int,TM>	(dst, mask, src, element_operator); break;
	case ExType::S8:	fnPRV_2D_Math_pp<char,TM>			(dst, mask, src, element_operator); break;
	case ExType::S16:	fnPRV_2D_Math_pp<short,TM>			(dst, mask, src, element_operator); break;
	case ExType::S32:	fnPRV_2D_Math_pp<int,TM>			(dst, mask, src, element_operator); break;
	case ExType::F32:	fnPRV_2D_Math_pp<float,TM>			(dst, mask, src, element_operator); break;
	case ExType::F64:	fnPRV_2D_Math_pp<double,TM>			(dst, mask, src, element_operator); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
static void XIE_API fnPRV_2D_Math(TxImage dst, TxImage mask, TxImage src, ExMath mode)
{
	typedef unsigned char	TM;

	if (MaskValidity<TM>(mask, dst) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (dst.Model.Type != src.Model.Type)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	switch(mode)
	{
	case ExMath::Abs:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return abs((double)_src); }); break;
	case ExMath::Sign:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return _sign((double)_src); }); break;
	case ExMath::Sqrt:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return sqrt((double)_src); }); break;
	case ExMath::Exp:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return exp((double)_src); }); break;
	case ExMath::Log:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return log((double)_src); }); break;
	case ExMath::Log10:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return log10((double)_src); }); break;
	case ExMath::Sin:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return sin((double)_src); }); break;
	case ExMath::Cos:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return cos((double)_src); }); break;
	case ExMath::Tan:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return tan((double)_src); }); break;
	case ExMath::Sinh:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return sinh((double)_src); }); break;
	case ExMath::Cosh:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return cosh((double)_src); }); break;
	case ExMath::Tanh:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return tanh((double)_src); }); break;
	case ExMath::Asin:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return asin((double)_src); }); break;
	case ExMath::Acos:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return acos((double)_src); }); break;
	case ExMath::Atan:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return atan((double)_src); }); break;
	case ExMath::Ceiling:	fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return ceil((double)_src); }); break;
	case ExMath::Floor:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return floor((double)_src); }); break;
	case ExMath::Round:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return round((double)_src); }); break;
	case ExMath::Truncate:	fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return trunc((double)_src); }); break;
	case ExMath::Modf:		fnPRV_2D_Math_<TM>(dst, mask, src, [](double _src) { return _modf((double)_src); }); break;
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Math(TxImage dst, TxImage mask, TxImage src, ExMath mode)
{
	try
	{
		if (dst.Model.Type == src.Model.Type)
		{
			fnPRV_2D_Math(dst, mask, src, mode);
		}
		else
		{
			CxImage dst_tmp = CxImage::FromTag(src).Clone(TxModel::F64(src.Model.Pack));
			fnPRV_2D_Math(dst_tmp.Tag(), mask, dst_tmp.Tag(), mode);

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
