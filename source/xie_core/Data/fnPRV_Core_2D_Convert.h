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
template<class TD, class TM, class TS, class TV> static inline void XIE_API fnPRV_2D_Convert_pp(TxImage dst, TxImage mask, TxImage src, TV scale)
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
				[dst_field,src_field,scale](int y, int x, TD* _dst, TS* _src)
				{
					_dst[dst_field] = saturate_cast<TD>( XIE_ELEMENT_OPERATOR(_src[src_field], scale) );
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[dst_field,src_field,scale](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
					{
						_dst[dst_field] = saturate_cast<TD>( XIE_ELEMENT_OPERATOR(_src[src_field], scale) );
					}
				});
		}
	}
}

// ======================================================================
template<class TM, class TS, class TV> static inline void XIE_API fnPRV_2D_Convert__p(TxImage dst, TxImage mask, TxImage src, TV scale)
{
	switch(dst.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Convert_pp<unsigned char,TM,TS,TV>			(dst, mask, src, scale); break;
	case ExType::U16:	fnPRV_2D_Convert_pp<unsigned short,TM,TS,TV>		(dst, mask, src, scale); break;
	case ExType::U32:	fnPRV_2D_Convert_pp<unsigned int,TM,TS,TV>			(dst, mask, src, scale); break;
	case ExType::U64:	fnPRV_2D_Convert_pp<unsigned long long,TM,TS,TV>	(dst, mask, src, scale); break;
	case ExType::S8:	fnPRV_2D_Convert_pp<char,TM,TS,TV>					(dst, mask, src, scale); break;
	case ExType::S16:	fnPRV_2D_Convert_pp<short,TM,TS,TV>					(dst, mask, src, scale); break;
	case ExType::S32:	fnPRV_2D_Convert_pp<int,TM,TS,TV>					(dst, mask, src, scale); break;
	case ExType::S64:	fnPRV_2D_Convert_pp<long long,TM,TS,TV>				(dst, mask, src, scale); break;
	case ExType::F32:	fnPRV_2D_Convert_pp<float,TM,TS,TV>					(dst, mask, src, scale); break;
	case ExType::F64:	fnPRV_2D_Convert_pp<double,TM,TS,TV>				(dst, mask, src, scale); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM, class TV> static inline void XIE_API fnPRV_2D_Convert___(TxImage dst, TxImage mask, TxImage src, TV scale)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Convert__p<TM,unsigned char,TV>		(dst, mask, src, scale); break;
	case ExType::U16:	fnPRV_2D_Convert__p<TM,unsigned short,TV>		(dst, mask, src, scale); break;
	case ExType::U32:	fnPRV_2D_Convert__p<TM,unsigned int,TV>			(dst, mask, src, scale); break;
	case ExType::U64:	fnPRV_2D_Convert__p<TM,unsigned long long,TV>	(dst, mask, src, scale); break;
	case ExType::S8:	fnPRV_2D_Convert__p<TM,char,TV>					(dst, mask, src, scale); break;
	case ExType::S16:	fnPRV_2D_Convert__p<TM,short,TV>				(dst, mask, src, scale); break;
	case ExType::S32:	fnPRV_2D_Convert__p<TM,int,TV>					(dst, mask, src, scale); break;
	case ExType::S64:	fnPRV_2D_Convert__p<TM,long long,TV>			(dst, mask, src, scale); break;
	case ExType::F32:	fnPRV_2D_Convert__p<TM,float,TV>				(dst, mask, src, scale); break;
	case ExType::F64:	fnPRV_2D_Convert__p<TM,double,TV>				(dst, mask, src, scale); break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

}
