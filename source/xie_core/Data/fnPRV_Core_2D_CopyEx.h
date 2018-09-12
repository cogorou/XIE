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
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_CopyEx_pp(TxImage dst, TxImage mask, TxImage src, int index, int count)
{
	for(int ch=0 ; ch<dst.Channels ; ch++)
	{
		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, ch);
		TxScanner2D<TS> src_scan = ToScanner<TS>(src, (src.Channels == 1) ? 0 : ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : ch);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src_scan,
				[index,count](int y, int x, TD* _dst, TS* _src)
				{
					for(int k=0 ; k<count ; k++)
						_dst[k] = saturate_cast<TD>( XIE_ELEMENT_OPERATOR(_src[k + index]) );
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[index,count](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
					{
						for(int k=0 ; k<count ; k++)
							_dst[k] = saturate_cast<TD>( XIE_ELEMENT_OPERATOR(_src[k + index]) );
					}
				});
		}
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_CopyEx__p(TxImage dst, TxImage mask, TxImage src, int index, int count)
{
	switch(dst.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_CopyEx_pp<unsigned char,TM,TS>			(dst, mask, src, index, count); break;
	case ExType::U16:	fnPRV_2D_CopyEx_pp<unsigned short,TM,TS>		(dst, mask, src, index, count); break;
	case ExType::U32:	fnPRV_2D_CopyEx_pp<unsigned int,TM,TS>			(dst, mask, src, index, count); break;
	case ExType::U64:	fnPRV_2D_CopyEx_pp<unsigned long long,TM,TS>	(dst, mask, src, index, count); break;
	case ExType::S8:	fnPRV_2D_CopyEx_pp<char,TM,TS>					(dst, mask, src, index, count); break;
	case ExType::S16:	fnPRV_2D_CopyEx_pp<short,TM,TS>					(dst, mask, src, index, count); break;
	case ExType::S32:	fnPRV_2D_CopyEx_pp<int,TM,TS>					(dst, mask, src, index, count); break;
	case ExType::S64:	fnPRV_2D_CopyEx_pp<long long,TM,TS>				(dst, mask, src, index, count); break;
	case ExType::F32:	fnPRV_2D_CopyEx_pp<float,TM,TS>					(dst, mask, src, index, count); break;
	case ExType::F64:	fnPRV_2D_CopyEx_pp<double,TM,TS>				(dst, mask, src, index, count); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM> static inline void XIE_API fnPRV_2D_CopyEx___(TxImage dst, TxImage mask, TxImage src, int index, int count)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_CopyEx__p<TM,unsigned char>		(dst, mask, src, index, count); break;
	case ExType::U16:	fnPRV_2D_CopyEx__p<TM,unsigned short>		(dst, mask, src, index, count); break;
	case ExType::U32:	fnPRV_2D_CopyEx__p<TM,unsigned int>			(dst, mask, src, index, count); break;
	case ExType::U64:	fnPRV_2D_CopyEx__p<TM,unsigned long long>	(dst, mask, src, index, count); break;
	case ExType::S8:	fnPRV_2D_CopyEx__p<TM,char>					(dst, mask, src, index, count); break;
	case ExType::S16:	fnPRV_2D_CopyEx__p<TM,short>				(dst, mask, src, index, count); break;
	case ExType::S32:	fnPRV_2D_CopyEx__p<TM,int>					(dst, mask, src, index, count); break;
	case ExType::S64:	fnPRV_2D_CopyEx__p<TM,long long>			(dst, mask, src, index, count); break;
	case ExType::F32:	fnPRV_2D_CopyEx__p<TM,float>				(dst, mask, src, index, count); break;
	case ExType::F64:	fnPRV_2D_CopyEx__p<TM,double>				(dst, mask, src, index, count); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

}
