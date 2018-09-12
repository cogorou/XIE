/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_core.h"
#include "api_data.h"

#pragma warning (disable:4127)	// 条件式が定数です.
#pragma warning (disable:4189)	// ローカル変数が初期化されましたが、参照されていません.

#include <math.h>

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/TxScanner2D.h"

namespace xie
{

// ======================================================================
template<class TD, class TS> static inline TxRGBx4<TS> fnPRV_2D_ColorMatrix_Job(TS R, TS G, TS B, TS A, const TxScanner2D<double>& matrix)
{
	double dR = R * matrix(0,0) + G * matrix(1,0) + B * matrix(2,0);
	double dG = R * matrix(0,1) + G * matrix(1,1) + B * matrix(2,1);
	double dB = R * matrix(0,2) + G * matrix(1,2) + B * matrix(2,2);
	double dA = A;

	TxRGBx4<TD> result;
	result.R = xie::saturate_cast<TD>(dR);
	result.G = xie::saturate_cast<TD>(dG);
	result.B = xie::saturate_cast<TD>(dB);
	result.A = xie::saturate_cast<TD>(dA);
	return result;
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_ColorMatrix_uu(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix)
{
	TxScanner2D<TS> src0_scan = ToScanner<TS>(src, 0);
	TxScanner2D<TS> src1_scan = ToScanner<TS>(src, 1);
	TxScanner2D<TS> src2_scan = ToScanner<TS>(src, 2);
	TxScanner2D<TD> dst0_scan = ToScanner<TD>(dst, 0);
	TxScanner2D<TD> dst1_scan = ToScanner<TD>(dst, 1);
	TxScanner2D<TD> dst2_scan = ToScanner<TD>(dst, 2);
	TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, 0);
	TxScanner2D<double> matrix_scan = ToScanner<double>(matrix);

	for(int y=0 ; y<dst0_scan.Height ; y++)
	{
		TS* _src0 = src0_scan[y];
		TS* _src1 = src1_scan[y];
		TS* _src2 = src2_scan[y];
		TD* _dst0 = dst0_scan[y];
		TD* _dst1 = dst1_scan[y];
		TD* _dst2 = dst2_scan[y];
		TM* _mask = (mask_scan.IsValid()) ? mask_scan[y] : NULL;

		for(int x=0 ; x<dst0_scan.Width ; x++)
		{
			if (_mask == NULL || _mask[x] != 0)
			{
				auto result = fnPRV_2D_ColorMatrix_Job<TD, TS>(_src0[x], _src1[x], _src2[x], 0, matrix_scan);
				_dst0[x] = result.R;
				_dst1[x] = result.G;
				_dst2[x] = result.B;
			}
		}
	}
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_ColorMatrix_pp(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix)
{
	TxScanner2D<TS> src_scan = ToScanner<TS>(src, 0);
	TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, 0);
	TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, 0);
	TxScanner2D<double> matrix_scan = ToScanner<double>(matrix);

	if (mask_scan.IsValid() == false)
	{
		dst_scan.ForEach(src_scan,
			[matrix_scan](int y, int x, TD* _dst, TS* _src)
			{
				auto result = fnPRV_2D_ColorMatrix_Job<TD, TS>(_src[0], _src[1], _src[2], 0, matrix_scan);
				_dst[0] = result.R;
				_dst[1] = result.G;
				_dst[2] = result.B;
			});
	}
	else
	{
		dst_scan.ForEach(src_scan, mask_scan,
			[matrix_scan](int y, int x, TD* _dst, TS* _src, TM* _mask)
			{
				if (*_mask != 0)
				{
					auto result = fnPRV_2D_ColorMatrix_Job<TD, TS>(_src[0], _src[1], _src[2], 0, matrix_scan);
					_dst[0] = result.R;
					_dst[1] = result.G;
					_dst[2] = result.B;
				}
			});
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_ColorMatrix__u(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix)
{
	typedef TS	TD; 

	fnPRV_2D_ColorMatrix_uu<TD,TM,TS>	(dst, mask, src, matrix);
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_ColorMatrix__p(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix)
{
	typedef TS	TD; 

	fnPRV_2D_ColorMatrix_pp<TD,TM,TS>	(dst, mask, src, matrix);
}

// ======================================================================
template<class TM> static inline void XIE_API fnPRV_2D_ColorMatrix___(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix)
{
	switch(src.Model.Pack)
	{
	case 1:
		if (src.Channels < 3)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		switch(src.Model.Type)
		{
		case ExType::U8:	fnPRV_2D_ColorMatrix__u<TM,unsigned char>	(dst, mask, src, matrix);	break;
		case ExType::U16:	fnPRV_2D_ColorMatrix__u<TM,unsigned short>	(dst, mask, src, matrix);	break;
		case ExType::U32:	fnPRV_2D_ColorMatrix__u<TM,unsigned int>	(dst, mask, src, matrix);	break;
		case ExType::S8:	fnPRV_2D_ColorMatrix__u<TM,char>			(dst, mask, src, matrix);	break;
		case ExType::S16:	fnPRV_2D_ColorMatrix__u<TM,short>			(dst, mask, src, matrix);	break;
		case ExType::S32:	fnPRV_2D_ColorMatrix__u<TM,int>				(dst, mask, src, matrix);	break;
		case ExType::F32:	fnPRV_2D_ColorMatrix__u<TM,float>			(dst, mask, src, matrix);	break;
		case ExType::F64:	fnPRV_2D_ColorMatrix__u<TM,double>			(dst, mask, src, matrix);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 3:
	case 4:
		if (src.Channels != 1)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		switch(src.Model.Type)
		{
		case ExType::U8:	fnPRV_2D_ColorMatrix__p<TM,unsigned char>	(dst, mask, src, matrix);	break;
		case ExType::U16:	fnPRV_2D_ColorMatrix__p<TM,unsigned short>	(dst, mask, src, matrix);	break;
		case ExType::U32:	fnPRV_2D_ColorMatrix__p<TM,unsigned int>	(dst, mask, src, matrix);	break;
		case ExType::S8:	fnPRV_2D_ColorMatrix__p<TM,char>			(dst, mask, src, matrix);	break;
		case ExType::S16:	fnPRV_2D_ColorMatrix__p<TM,short>			(dst, mask, src, matrix);	break;
		case ExType::S32:	fnPRV_2D_ColorMatrix__p<TM,int>				(dst, mask, src, matrix);	break;
		case ExType::F32:	fnPRV_2D_ColorMatrix__p<TM,float>			(dst, mask, src, matrix);	break;
		case ExType::F64:	fnPRV_2D_ColorMatrix__p<TM,double>			(dst, mask, src, matrix);	break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_ColorMatrix(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix)
{
	typedef unsigned char	TM;

	try
	{
		if (MaskValidity<TM>(mask, dst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (matrix.Address == NULL)
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (matrix.Model != TxModel::F64(1))
			throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
		if (!(matrix.Rows == 3 && matrix.Columns == 3))
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		fnPRV_2D_ColorMatrix___<TM>(dst, mask, src, matrix);

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
