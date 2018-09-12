/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

namespace xie
{

// ======================================================================
template<class TD, class TM, class TS, class TK, class TR> static inline void XIE_API fnPRV_2D_Affine2_aa(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix, TR region)
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
			[dst_field,src_field,&mat,region,src_scan,mask_scan,mask_is_valid](int y, int x, TD* _dst)
			{
				int sx = (int)(x * mat[0] + y * mat[1] + mat[2]);
				int sy = (int)(x * mat[3] + y * mat[4] + mat[5]);
				if (0 <= sx && sx < src_scan.Width &&
					0 <= sy && sy < src_scan.Height)
				{
					if (mask_is_valid == false || mask_scan(sy, sx) != 0)
					{
						int sx0 = sx - region.X;
						int sy0 = sy - region.Y;
						int sx1 = sx + region.X;
						int sy1 = sy + region.Y;

						if (sx0 < 0) sx0 = 0;
						if (sy0 < 0) sy0 = 0;
						if (sx1 > src_scan.Width  - 1) sx1 = src_scan.Width  - 1;
						if (sy1 > src_scan.Height - 1) sy1 = src_scan.Height - 1;

						if (sx0 != sx1 && sy0 != sy1)
						{
							const TS* src00 = &src_scan(sy0, sx0);
							const TS* src01 = &src_scan(sy0, sx1);
							const TS* src10 = &src_scan(sy1, sx0);
							const TS* src11 = &src_scan(sy1, sx1);
							_dst[dst_field] = saturate_cast<TD>((src11[src_field] - src01[src_field] - src10[src_field] + src00[src_field]) / ((sy1 - sy0) * (sx1 - sx0)));
						}
						else if (sx0 != sx1)
						{
							const TS* src00 = &src_scan(sy0, sx0);
							const TS* src01 = &src_scan(sy0, sx1);
							_dst[dst_field] = saturate_cast<TD>((src01[src_field] - src00[src_field]) / (sx1 - sx0));
						}
						else if (sy0 != sy1)
						{
							const TS* src00 = &src_scan(sy0, sx0);
							const TS* src10 = &src_scan(sy1, sx0);
							_dst[dst_field] = saturate_cast<TD>((src10[src_field] - src00[src_field]) / (sy1 - sy0));
						}
						else
						{
							const TS* src00 = &src_scan(sy0, sx0);
							_dst[dst_field] = saturate_cast<TD>(src00[src_field]);
						}
					}
				}
			});
	}
}

// ======================================================================
template<class TM, class TS, class TK, class TR> static inline void XIE_API fnPRV_2D_Affine2_a_(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix, TR region)
{
	switch(dst.Model.Type)
	{
	case ExType::U8:	fnPRV_2D_Affine2_aa<unsigned char,TM,TS,TK,TR>	(dst, mask, src, matrix, region);	break;
	case ExType::U16:	fnPRV_2D_Affine2_aa<unsigned short,TM,TS,TK,TR>	(dst, mask, src, matrix, region);	break;
	case ExType::U32:	fnPRV_2D_Affine2_aa<unsigned int,TM,TS,TK,TR>	(dst, mask, src, matrix, region);	break;
	case ExType::S8:	fnPRV_2D_Affine2_aa<char,TM,TS,TK,TR>			(dst, mask, src, matrix, region);	break;
	case ExType::S16:	fnPRV_2D_Affine2_aa<short,TM,TS,TK,TR>			(dst, mask, src, matrix, region);	break;
	case ExType::S32:	fnPRV_2D_Affine2_aa<int,TM,TS,TK,TR>			(dst, mask, src, matrix, region);	break;
	case ExType::F32:	fnPRV_2D_Affine2_aa<float,TM,TS,TK,TR>			(dst, mask, src, matrix, region);	break;
	case ExType::F64:	fnPRV_2D_Affine2_aa<double,TM,TS,TK,TR>			(dst, mask, src, matrix, region);	break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM, class TS, class TK, class TR> static inline void XIE_API fnPRV_2D_Affine2___(TxImage dst, TxImage mask, TxImage src, TxMatrix matrix, TR region)
{
	fnPRV_2D_Affine2_a_<TM,TS,TK,TR>(dst, mask, src, matrix, region);
}

}
