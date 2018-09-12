/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

namespace xie
{

// ======================================================================
#ifndef XIE_ELEMENT_OPERATOR
#error You have to define the operator.
#endif

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_Copy_pp(TxImage dst, TxImage mask, TxImage src)
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

		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, dst_ch);
		TxScanner2D<TS> src_scan = ToScanner<TS>(src, src_ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : dst_ch);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src_scan,
				[dst_field,src_field](int y, int x, TD* _dst, TS* _src)
				{
					_dst[dst_field] = saturate_cast<TD>( XIE_ELEMENT_OPERATOR(_src[src_field]) );
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[dst_field,src_field](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
					{
						_dst[dst_field] = saturate_cast<TD>( XIE_ELEMENT_OPERATOR(_src[src_field]) );
					}
				});
		}
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_Copy__p(TxImage dst, TxImage mask, TxImage src)
{
	switch(dst.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Copy_pp<unsigned char,TM,TS>		(dst, mask, src); break;
	case ExType::U16:	fnPRV_2D_Copy_pp<unsigned short,TM,TS>		(dst, mask, src); break;
	case ExType::U32:	fnPRV_2D_Copy_pp<unsigned int,TM,TS>		(dst, mask, src); break;
	case ExType::U64:	fnPRV_2D_Copy_pp<unsigned long long,TM,TS>	(dst, mask, src); break;
	case ExType::S8:	fnPRV_2D_Copy_pp<char,TM,TS>				(dst, mask, src); break;
	case ExType::S16:	fnPRV_2D_Copy_pp<short,TM,TS>				(dst, mask, src); break;
	case ExType::S32:	fnPRV_2D_Copy_pp<int,TM,TS>					(dst, mask, src); break;
	case ExType::S64:	fnPRV_2D_Copy_pp<long long,TM,TS>			(dst, mask, src); break;
	case ExType::F32:	fnPRV_2D_Copy_pp<float,TM,TS>				(dst, mask, src); break;
	case ExType::F64:	fnPRV_2D_Copy_pp<double,TM,TS>				(dst, mask, src); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM> static inline void XIE_API fnPRV_2D_Copy___(TxImage dst, TxImage mask, TxImage src)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Copy__p<TM,unsigned char>		(dst, mask, src); break;
	case ExType::U16:	fnPRV_2D_Copy__p<TM,unsigned short>		(dst, mask, src); break;
	case ExType::U32:	fnPRV_2D_Copy__p<TM,unsigned int>		(dst, mask, src); break;
	case ExType::U64:	fnPRV_2D_Copy__p<TM,unsigned long long>	(dst, mask, src); break;
	case ExType::S8:	fnPRV_2D_Copy__p<TM,char>				(dst, mask, src); break;
	case ExType::S16:	fnPRV_2D_Copy__p<TM,short>				(dst, mask, src); break;
	case ExType::S32:	fnPRV_2D_Copy__p<TM,int>				(dst, mask, src); break;
	case ExType::S64:	fnPRV_2D_Copy__p<TM,long long>			(dst, mask, src); break;
	case ExType::F32:	fnPRV_2D_Copy__p<TM,float>				(dst, mask, src); break;
	case ExType::F64:	fnPRV_2D_Copy__p<TM,double>				(dst, mask, src); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

}
