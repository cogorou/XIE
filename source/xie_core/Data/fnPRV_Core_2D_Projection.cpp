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
#include "Core/TxScanner1D.h"

namespace xie
{

// ======================================================================
template<class TS, class TM, class TR> static inline void fnPRV_2D_Projection_aa(TxImage src, TxImage mask, int ch, ExScanDir dir, TxArray result)
{
	int src_pxc   = src.Model.Pack * src.Channels;
	if (src_pxc <= 0)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	int src_ch    = ch / src.Model.Pack;
	int src_field = ch % src.Model.Pack;

	TxScanner2D<TS> src_scan = ToScanner<TS>(src, src_ch);
	TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : src_ch);
	TxScanner1D<TR> dst_scan = ToScanner<TR>(result);

	if (src_scan.IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (dst_scan.IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	switch(dir)
	{
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	case ExScanDir::X:
		if (mask_scan.IsValid() == false)
		{
			for(int y=0 ; y<src.Height ; y++)
			{
				TxStatistics stat;
				for(int x=0 ; x<src.Width ; x++)
				{
					TS* _src = &src_scan(y, x);
					stat += (double)_src[src_field];
				}
				dst_scan[y] = stat;
			}
		}
		else
		{
			for(int y=0 ; y<src.Height ; y++)
			{
				TxStatistics stat;
				for(int x=0 ; x<src.Width ; x++)
				{
					if (mask_scan(y, x) != 0)
					{
						TS* _src = &src_scan(y, x);
						stat += (double)_src[src_field];
					}
				}
				dst_scan[y] = stat;
			}
		}
		break;

	case ExScanDir::Y:
		if (mask_scan.IsValid() == false)
		{
			for(int x=0 ; x<src.Width ; x++)
			{
				TxStatistics stat;
				for(int y=0 ; y<src.Height ; y++)
				{
					TS* _src = &src_scan(y, x);
					stat += (double)_src[src_field];
				}
				dst_scan[x] = stat;
			}
		}
		else
		{
			for(int x=0 ; x<src.Width ; x++)
			{
				TxStatistics stat;
				for(int y=0 ; y<src.Height ; y++)
				{
					if (mask_scan(y, x) != 0)
					{
						TS* _src = &src_scan(y, x);
						stat += (double)_src[src_field];
					}
				}
				dst_scan[x] = stat;
			}
		}
		break;
	}
}

// ======================================================================
template<class TM, class TR> static inline void fnPRV_2D_Projection___(TxImage src, TxImage mask, int ch, ExScanDir dir, TxArray result)
{
	switch(src.Model.Type)
	{
	case ExType::U8:		fnPRV_2D_Projection_aa<unsigned char,TM,TR>		(src, mask, ch, dir, result);	break;
	case ExType::U16:	fnPRV_2D_Projection_aa<unsigned short,TM,TR>		(src, mask, ch, dir, result);	break;
	case ExType::U32:	fnPRV_2D_Projection_aa<unsigned int,TM,TR>		(src, mask, ch, dir, result);	break;
	case ExType::U64:	fnPRV_2D_Projection_aa<unsigned long long,TM,TR>	(src, mask, ch, dir, result);	break;
	case ExType::S8:		fnPRV_2D_Projection_aa<char,TM,TR>				(src, mask, ch, dir, result);	break;
	case ExType::S16:	fnPRV_2D_Projection_aa<short,TM,TR>				(src, mask, ch, dir, result);	break;
	case ExType::S32:	fnPRV_2D_Projection_aa<int,TM,TR>					(src, mask, ch, dir, result);	break;
	case ExType::S64:	fnPRV_2D_Projection_aa<long long,TM,TR>			(src, mask, ch, dir, result);	break;
	case ExType::F32:	fnPRV_2D_Projection_aa<float,TM,TR>				(src, mask, ch, dir, result);	break;
	case ExType::F64:	fnPRV_2D_Projection_aa<double,TM,TR>				(src, mask, ch, dir, result);	break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Projection(TxImage src, TxImage mask, int ch, ExScanDir dir, TxArray result)
{
	typedef unsigned char	TM;
	typedef TxStatistics	TR;

	try
	{
		if (result.Model != ModelOf<TR>())
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (MaskValidity<TM>(mask, src) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		switch(dir)
		{
		case ExScanDir::X:
			if (result.Length != src.Height)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			break;
		case ExScanDir::Y:
			if (result.Length != src.Width)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			break;
		default:
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		}

		fnPRV_2D_Projection___<TM,TR>(src, mask, ch, dir, result);

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
