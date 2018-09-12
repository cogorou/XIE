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
template<class TS, class TM> static inline void fnPRV_2D_Statistics_a(TxImage src, TxImage mask, int ch, int fieldno, TxStatistics* result)
{
	TxScanner2D<TS> src_scan = ToScanner<TS>(src, ch);
	TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : ch);
	TxStatistics stat = TxStatistics::Default();

	if (mask_scan.IsValid() == false)
	{
		src_scan.ForEach(
			[ch,fieldno,&stat](int y, int x, TS* _src)
			{
				stat += (double)(_src[fieldno]);
			});
	}
	else
	{
		src_scan.ForEach(mask_scan,
			[ch,fieldno,&stat](int y, int x, TS* _src,TM* _mask)
			{
				if (*_mask != 0)
					stat += (double)(_src[fieldno]);
			});
	}

	(*result) = stat;
}
// ======================================================================
template<class TM> static inline void fnPRV_2D_Statistics__(TxImage src, TxImage mask, int ch, int fieldno, TxStatistics* result)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Statistics_a<unsigned char,TM>		(src, mask, ch, fieldno, result);	break;
	case ExType::U16:	fnPRV_2D_Statistics_a<unsigned short,TM>	(src, mask, ch, fieldno, result);	break;
	case ExType::U32:	fnPRV_2D_Statistics_a<unsigned int,TM>		(src, mask, ch, fieldno, result);	break;
	case ExType::S8:	fnPRV_2D_Statistics_a<char,TM>				(src, mask, ch, fieldno, result);	break;
	case ExType::S16:	fnPRV_2D_Statistics_a<short,TM>				(src, mask, ch, fieldno, result);	break;
	case ExType::S32:	fnPRV_2D_Statistics_a<int,TM>				(src, mask, ch, fieldno, result);	break;
	case ExType::F32:	fnPRV_2D_Statistics_a<float,TM>				(src, mask, ch, fieldno, result);	break;
	case ExType::F64:	fnPRV_2D_Statistics_a<double,TM>			(src, mask, ch, fieldno, result);	break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Statistics(TxImage src, TxImage mask, int ch, TxStatistics* result)
{
	typedef unsigned char	TM;

	try
	{
		if (result == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (MaskValidity<TM>(mask, src) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		int chno = ch / src.Model.Pack;
		int fieldno = ch % src.Model.Pack;

		fnPRV_2D_Statistics__<TM>(src, mask, chno, fieldno, result);

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
