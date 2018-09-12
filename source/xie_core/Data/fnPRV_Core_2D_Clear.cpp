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

namespace xie
{

// ======================================================================
template<class TD, class TM, class TV> static inline void XIE_API fnPRV_2D_Clear_aa(TxImage dst, TxImage mask, const TV* value, TxModel model)
{
	for(int ch=0 ; ch<dst.Channels ; ch++)
	{
		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : ch);
		int pack = Axi::Min(dst.Model.Pack, model.Pack);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(
				[pack,value](int y, int x, TD* _dst)
				{
					for(int k=0 ; k<pack ; k++)
						_dst[k] = static_cast<TD>((double)value[k]);
				});
		}
		else
		{
			dst_scan.ForEach(mask_scan,
				[pack,value](int y, int x, TD* _dst, TM* _mask)
				{
					if (*_mask != 0)
					{
						for(int k=0 ; k<pack ; k++)
							_dst[k] = static_cast<TD>((double)value[k]);
					}
				});
		}
	}
}

// ======================================================================
template<class TD, class TM> static inline void XIE_API fnPRV_2D_Clear_av(TxImage dst, TxImage mask, TD value)
{
	for(int ch=0 ; ch<dst.Channels ; ch++)
	{
		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : ch);
		int pack = dst.Model.Pack;

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(
				[pack,value](int y, int x, TD* _dst)
				{
					for(int k=0 ; k<pack ; k++)
						_dst[k] = value;
				});
		}
		else
		{
			dst_scan.ForEach(mask_scan,
				[pack,value](int y, int x, TD* _dst, TM* _mask)
				{
					if (*_mask != 0)
					{
						for(int k=0 ; k<pack ; k++)
							_dst[k] = value;
					}
				});
		}
	}
}

// ======================================================================
template<class TD, class TM> static inline void XIE_API fnPRV_2D_Clear___(TxImage dst, TxImage mask, const void* value, TxModel model)
{
	if (model.Pack == 1)
	{
		switch(model.Type)
		{
		case ExType::U8:	fnPRV_2D_Clear_av<TD,TM>	( dst, mask, saturate_cast<TD>(*static_cast<const unsigned char*>(value)) );	break;
		case ExType::U16:	fnPRV_2D_Clear_av<TD,TM>	( dst, mask, saturate_cast<TD>(*static_cast<const unsigned short*>(value)) );	break;
		case ExType::U32:	fnPRV_2D_Clear_av<TD,TM>	( dst, mask, saturate_cast<TD>(*static_cast<const unsigned int*>(value)) );	break;
		case ExType::S8:	fnPRV_2D_Clear_av<TD,TM>	( dst, mask, saturate_cast<TD>(*static_cast<const char*>(value)) );	break;
		case ExType::S16:	fnPRV_2D_Clear_av<TD,TM>	( dst, mask, saturate_cast<TD>(*static_cast<const short*>(value)) );	break;
		case ExType::S32:	fnPRV_2D_Clear_av<TD,TM>	( dst, mask, saturate_cast<TD>(*static_cast<const int*>(value)) );	break;
		case ExType::F32:	fnPRV_2D_Clear_av<TD,TM>	( dst, mask, saturate_cast<TD>(*static_cast<const float*>(value)) );	break;
		case ExType::F64:	fnPRV_2D_Clear_av<TD,TM>	( dst, mask, saturate_cast<TD>(*static_cast<const double*>(value)) );	break;
		case ExType::U64:	fnPRV_2D_Clear_av<TD,TM>	( dst, mask, saturate_cast<TD>((double)*static_cast<const unsigned long long*>(value)) );	break;
		case ExType::S64:	fnPRV_2D_Clear_av<TD,TM>	( dst, mask, saturate_cast<TD>((double)*static_cast<const long long*>(value)) );	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
	}
	else
	{
		switch(model.Type)
		{
		case ExType::U8:	fnPRV_2D_Clear_aa<TD,TM>	( dst, mask, static_cast<const unsigned char*>(value), model );	break;
		case ExType::U16:	fnPRV_2D_Clear_aa<TD,TM>	( dst, mask, static_cast<const unsigned short*>(value), model );	break;
		case ExType::U32:	fnPRV_2D_Clear_aa<TD,TM>	( dst, mask, static_cast<const unsigned int*>(value), model );	break;
		case ExType::U64:	fnPRV_2D_Clear_aa<TD,TM>	( dst, mask, static_cast<const unsigned long long*>(value), model );	break;
		case ExType::S8:	fnPRV_2D_Clear_aa<TD,TM>	( dst, mask, static_cast<const char*>(value), model );	break;
		case ExType::S16:	fnPRV_2D_Clear_aa<TD,TM>	( dst, mask, static_cast<const short*>(value), model );	break;
		case ExType::S32:	fnPRV_2D_Clear_aa<TD,TM>	( dst, mask, static_cast<const int*>(value), model );	break;
		case ExType::S64:	fnPRV_2D_Clear_aa<TD,TM>	( dst, mask, static_cast<const long long*>(value), model );	break;
		case ExType::F32:	fnPRV_2D_Clear_aa<TD,TM>	( dst, mask, static_cast<const float*>(value), model );	break;
		case ExType::F64:	fnPRV_2D_Clear_aa<TD,TM>	( dst, mask, static_cast<const double*>(value), model );	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Clear(TxImage dst, TxImage mask, const void* value, TxModel model)
{
	typedef unsigned char	TM;

	try
	{
		if (value == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (MaskValidity<TM>(mask, dst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		switch(dst.Model.Type)
		{
		case ExType::U8:	fnPRV_2D_Clear___<unsigned char,TM>			(dst, mask, value, model);	break;
		case ExType::U16:	fnPRV_2D_Clear___<unsigned short,TM>		(dst, mask, value, model);	break;
		case ExType::U32:	fnPRV_2D_Clear___<unsigned int,TM>			(dst, mask, value, model);	break;
		case ExType::U64:	fnPRV_2D_Clear___<unsigned long long,TM>	(dst, mask, value, model);	break;
		case ExType::S8:	fnPRV_2D_Clear___<char,TM>					(dst, mask, value, model);	break;
		case ExType::S16:	fnPRV_2D_Clear___<short,TM>					(dst, mask, value, model);	break;
		case ExType::S32:	fnPRV_2D_Clear___<int,TM>					(dst, mask, value, model);	break;
		case ExType::S64:	fnPRV_2D_Clear___<long long,TM>				(dst, mask, value, model);	break;
		case ExType::F32:	fnPRV_2D_Clear___<float,TM>					(dst, mask, value, model);	break;
		case ExType::F64:	fnPRV_2D_Clear___<double,TM>				(dst, mask, value, model);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
