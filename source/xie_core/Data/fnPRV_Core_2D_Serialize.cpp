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
#include "Core/TxScanner1D.h"
#include "Core/TxScanner2D.h"

#include <math.h>

namespace xie
{

// ======================================================================
template<class TS> static inline void XIE_API fnPRV_2D_Serialize_pp(TxArray dst, TxImage src)
{
	typedef TS	TD;
	TxScanner1D<TD> dst_scan = ToScanner<TD>(dst);
	TxScanner2D<TS> src_scan = ToScanner<TS>(src, 0);
	int pack = src.Model.Pack;
	int width = src.Width;

	src_scan.ForEach(
		[&dst_scan,pack,width](int y, int x, TS* _src)
		{
			int index = y * width + x;
			TD* _dst = &dst_scan[index];
			for(int k=0 ; k<pack ; k++)
				_dst[k] = _src[k];
		});
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_Serialize(TxArray dst, TxImage src)
{
	try
	{
		if (src.Model != dst.Model)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		switch(src.Model.Type)
		{
		case ExType::S8:		
		case ExType::U8:	fnPRV_2D_Serialize_pp<unsigned char>	(dst, src);	break;
		case ExType::S16:	
		case ExType::U16:	fnPRV_2D_Serialize_pp<unsigned short>	(dst, src);	break;
		case ExType::S32:	
		case ExType::F32:	
		case ExType::U32:	fnPRV_2D_Serialize_pp<unsigned int>		(dst, src);	break;
		case ExType::U64:	
		case ExType::F64:	
		case ExType::S64:	fnPRV_2D_Serialize_pp<long long>		(dst, src);	break;
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
