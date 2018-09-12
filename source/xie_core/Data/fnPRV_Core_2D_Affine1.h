/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

namespace xie
{

// ======================================================================
template<class TD, class TM, class TS, class TK> static inline void XIE_API fnPRV_2D_Affine1_aa(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix)
{
	TxScanner2D<TK> mat_scan = ToScanner<TK>(matrix);
	TK mat[6];
	mat[0] = mat_scan(0, 0);
	mat[1] = mat_scan(0, 1);
	mat[2] = mat_scan(0, 2);
	mat[3] = mat_scan(1, 0);
	mat[4] = mat_scan(1, 1);
	mat[5] = mat_scan(1, 2);

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
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : src_ch);

		if (dst_scan.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (src_scan.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		bool mask_is_valid = mask_scan.IsValid();

		dst_scan.ForEach(
			[dst_field,src_field,&mat,src_scan,mask_scan,mask_is_valid](int y, int x, TD* _dst)
			{
				TK dx = (x * mat[0] + y * mat[1] + mat[2]);
				TK dy = (x * mat[3] + y * mat[4] + mat[5]);
				int sx = (int)dx;
				int sy = (int)dy;
				if (0 <= sx && sx < src_scan.Width &&
					0 <= sy && sy < src_scan.Height)
				{
					if (mask_is_valid == false || mask_scan(sy, sx) != 0)
					{
						int sx0 = sx;
						int sy0 = sy;
						int sx1 = (sx < src_scan.Width  - 1) ? sx + 1 : sx;
						int sy1 = (sy < src_scan.Height - 1) ? sy + 1 : sy;

						TK rx0 = 1.0 - (dx - sx);
						TK ry0 = 1.0 - (dy - sy);
						TK rx1 = (dx - sx);
						TK ry1 = (dy - sy);

						const TS* src00 = &src_scan(sy0, sx0);
						const TS* src01 = &src_scan(sy0, sx1);
						const TS* src10 = &src_scan(sy1, sx0);
						const TS* src11 = &src_scan(sy1, sx1);

						_dst[dst_field] = saturate_cast<TD>((
							(src00[src_field] * rx0) + (src01[src_field] * rx1) +
							(src10[src_field] * rx0) + (src11[src_field] * rx1) +
							(src00[src_field] * ry0) + (src01[src_field] * ry0) +
							(src10[src_field] * ry1) + (src11[src_field] * ry1)
							) / 4);
					}
				}
			});
	}
}

// ======================================================================
template<class TM, class TS, class TK> static inline void XIE_API fnPRV_2D_Affine1_a_(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix)
{
	typedef TS TD;

	fnPRV_2D_Affine1_aa<TD,TM,TS,TK>(dst, mask, src, matrix);
}

// ======================================================================
template<class TM, class TK> static inline void XIE_API fnPRV_2D_Affine1___(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix)
{
	switch(src.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Affine1_a_<TM,unsigned char,TK>	(dst, mask, src, matrix);	break;
	case ExType::U16:	fnPRV_2D_Affine1_a_<TM,unsigned short,TK>	(dst, mask, src, matrix);	break;
	case ExType::U32:	fnPRV_2D_Affine1_a_<TM,unsigned int,TK>		(dst, mask, src, matrix);	break;
	case ExType::S8:	fnPRV_2D_Affine1_a_<TM,char,TK>				(dst, mask, src, matrix);	break;
	case ExType::S16:	fnPRV_2D_Affine1_a_<TM,short,TK>			(dst, mask, src, matrix);	break;
	case ExType::S32:	fnPRV_2D_Affine1_a_<TM,int,TK>				(dst, mask, src, matrix);	break;
	case ExType::F32:	fnPRV_2D_Affine1_a_<TM,float,TK>			(dst, mask, src, matrix);	break;
	case ExType::F64:	fnPRV_2D_Affine1_a_<TM,double,TK>			(dst, mask, src, matrix);	break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

}
