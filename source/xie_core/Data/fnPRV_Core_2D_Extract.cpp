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
template<class TS, class TM> static inline void fnPRV_2D_Extract_aa(TxImage src, TxImage mask, int ch, int sy, int sx, int length, ExScanDir dir, TxArray result)
{
	typedef TS TD;
	TxScanner2D<TS> src_scan = ToScanner<TS>(src, ch);
	TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : ch);
	TxScanner1D<TD> dst_scan = ToScanner<TD>(result);
	int pack = src.Model.Pack;

	if (src_scan.IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (dst_scan.IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	switch(dir)
	{
	default:
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	case ExScanDir::X:
		{
			if (!(0 <= sy && sy <= src.Height))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			if (!(0 <= sx && (sx+length) <= src.Width))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

			if (mask_scan.IsValid() == false)
			{
				for(int i=0 ; i<length ; i++)
				{
					TS* _src = &src_scan(sy, sx+i);
					TD* _dst = &dst_scan[i];
					for(int k=0 ; k<pack ; k++)
						_dst[k] = _src[k];
				}
			}
			else
			{
				for(int i=0 ; i<length ; i++)
				{
					if (mask_scan(sy, sx+i) != 0)
					{
						TS* _src = &src_scan(sy, sx+i);
						TD* _dst = &dst_scan[i];
						for(int k=0 ; k<pack ; k++)
							_dst[k] = _src[k];
					}
				}
			}
		}
		break;

	case ExScanDir::Y:
		{
			if (!(0 <= sy && (sy+length) <= src.Height))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
			if (!(0 <= sx && sx <= src.Width))
				throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

			if (mask_scan.IsValid() == false)
			{
				for(int i=0 ; i<length ; i++)
				{
					TS* _src = &src_scan(sy+i, sx);
					TD* _dst = &dst_scan[i];
					for(int k=0 ; k<pack ; k++)
						_dst[k] = _src[k];
				}
			}
			else
			{
				for(int i=0 ; i<length ; i++)
				{
					if (mask_scan(sy+i, sx) != 0)
					{
						TS* _src = &src_scan(sy+i, sx);
						TD* _dst = &dst_scan[i];
						for(int k=0 ; k<pack ; k++)
							_dst[k] = _src[k];
					}
				}
			}
		}
		break;
	}
}

// ======================================================================
template<class TM> static inline void fnPRV_2D_Extract___(TxImage src, TxImage mask, int ch, int sy, int sx, int length, ExScanDir dir, TxArray result)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Extract_aa<unsigned char,TM>		(src, mask, ch, sy, sx, length, dir, result);	break;
	case ExType::U16:	fnPRV_2D_Extract_aa<unsigned short,TM>		(src, mask, ch, sy, sx, length, dir, result);	break;
	case ExType::U32:	fnPRV_2D_Extract_aa<unsigned int,TM>		(src, mask, ch, sy, sx, length, dir, result);	break;
	case ExType::U64:	fnPRV_2D_Extract_aa<unsigned long long,TM>	(src, mask, ch, sy, sx, length, dir, result);	break;
	case ExType::S8:	fnPRV_2D_Extract_aa<char,TM>				(src, mask, ch, sy, sx, length, dir, result);	break;
	case ExType::S16:	fnPRV_2D_Extract_aa<short,TM>				(src, mask, ch, sy, sx, length, dir, result);	break;
	case ExType::S32:	fnPRV_2D_Extract_aa<int,TM>					(src, mask, ch, sy, sx, length, dir, result);	break;
	case ExType::S64:	fnPRV_2D_Extract_aa<long long,TM>			(src, mask, ch, sy, sx, length, dir, result);	break;
	case ExType::F32:	fnPRV_2D_Extract_aa<float,TM>				(src, mask, ch, sy, sx, length, dir, result);	break;
	case ExType::F64:	fnPRV_2D_Extract_aa<double,TM>				(src, mask, ch, sy, sx, length, dir, result);	break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Extract(TxImage src, TxImage mask, int ch, int sy, int sx, int length, ExScanDir dir, TxArray result)
{
	typedef unsigned char	TM;

	try
	{
		if (result.Model != src.Model)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (result.Length != length)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (MaskValidity<TM>(mask, src) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		fnPRV_2D_Extract___<TM>(src, mask, ch, sy, sx, length, dir, result);

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
