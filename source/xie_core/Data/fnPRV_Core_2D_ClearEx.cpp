/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_core.h"
#include "api_data.h"

#pragma warning (disable:4127)	// 条件式が定数です.
#pragma warning (disable:4189)	// ローカル変数が初期化されましたが、参照されていません.
#pragma warning (disable:4244)	// '引数' : '__int64' から 'double' への変換です。データが失われる可能性があります。

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/TxScanner2D.h"

namespace xie
{

// ======================================================================
template<class TD, class TM> static inline void XIE_API fnPRV_2D_ClearEx_p(TxImage dst, TxImage mask, TD value, int index, int count)
{
	for(int ch=0 ; ch<dst.Channels ; ch++)
	{
		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : ch);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(
				[value, index, count](int y, int x, TD* _dst)
				{
					for(int k=0 ; k<count ; k++)
						_dst[index + k] = value;
				});
		}
		else
		{
			dst_scan.ForEach(
				mask_scan,
				[value, index, count](int y, int x, TD* _dst, TM* _mask)
				{
					if (*_mask != 0)
					{
						for(int k=0 ; k<count ; k++)
							_dst[index + k] = value;
					}
				});
		}
	}
}

// ======================================================================
template<class TD, class TM, class TV> static inline void XIE_API fnPRV_2D_ClearEx_s(TxImage dst, TxImage mask, TV value, int index, int count)
{
	fnPRV_2D_ClearEx_p<TD, TM>(dst, mask, saturate_cast<TD>(value), index, count);
}

// ======================================================================
template<class TM, class TV> static inline void XIE_API fnPRV_2D_ClearEx__(TxImage dst, TxImage mask, TV value, int index, int count)
{
	switch(dst.Model.Type)
	{
	default: break;
	case ExType::U8		: fnPRV_2D_ClearEx_s<unsigned char,TM,TV>		(dst, mask, value, index, count); break;
	case ExType::U16	: fnPRV_2D_ClearEx_s<unsigned short,TM,TV>		(dst, mask, value, index, count); break;
	case ExType::U32	: fnPRV_2D_ClearEx_s<unsigned int,TM,TV>		(dst, mask, value, index, count); break;
	case ExType::U64	: fnPRV_2D_ClearEx_s<unsigned long long,TM,TV>	(dst, mask, value, index, count); break;
	case ExType::S8		: fnPRV_2D_ClearEx_s<char,TM,TV>				(dst, mask, value, index, count); break;
	case ExType::S16	: fnPRV_2D_ClearEx_s<short,TM,TV>				(dst, mask, value, index, count); break;
	case ExType::S32	: fnPRV_2D_ClearEx_s<int,TM,TV>					(dst, mask, value, index, count); break;
	case ExType::S64	: fnPRV_2D_ClearEx_s<long long,TM,TV>			(dst, mask, value, index, count); break;
	case ExType::F32	: fnPRV_2D_ClearEx_s<float,TM,TV>				(dst, mask, value, index, count); break;
	case ExType::F64	: fnPRV_2D_ClearEx_s<double,TM,TV>				(dst, mask, value, index, count); break;
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_ClearEx(TxImage dst, TxImage mask, const void* value, TxModel model, int index, int count)
{
	typedef unsigned char	TM;

	try
	{
		if (model.Pack != 1)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (index < 0)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (count < 1)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (dst.Model.Pack < (index + count))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (MaskValidity<TM>(mask, dst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		switch(model.Type)
		{
		default: break;
		case ExType::U8		: fnPRV_2D_ClearEx__<TM>	(dst, mask, *static_cast<const unsigned char*>(value), index, count); break;
		case ExType::U16	: fnPRV_2D_ClearEx__<TM>	(dst, mask, *static_cast<const unsigned short*>(value), index, count); break;
		case ExType::U32	: fnPRV_2D_ClearEx__<TM>	(dst, mask, *static_cast<const unsigned int*>(value), index, count); break;
		case ExType::U64	: fnPRV_2D_ClearEx__<TM>	(dst, mask, *static_cast<const unsigned long long*>(value), index, count); break;
		case ExType::S8		: fnPRV_2D_ClearEx__<TM>	(dst, mask, *static_cast<const char*>(value), index, count); break;
		case ExType::S16	: fnPRV_2D_ClearEx__<TM>	(dst, mask, *static_cast<const short*>(value), index, count); break;
		case ExType::S32	: fnPRV_2D_ClearEx__<TM>	(dst, mask, *static_cast<const int*>(value), index, count); break;
		case ExType::S64	: fnPRV_2D_ClearEx__<TM>	(dst, mask, *static_cast<const long long*>(value), index, count); break;
		case ExType::F32	: fnPRV_2D_ClearEx__<TM>	(dst, mask, *static_cast<const float*>(value), index, count); break;
		case ExType::F64	: fnPRV_2D_ClearEx__<TM>	(dst, mask, *static_cast<const double*>(value), index, count); break;
		}

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
