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
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToBgr_pp(TxImage dst, TxImage mask, TxImage src, double scale)
{
	int channels = Axi::Min(dst.Channels, src.Channels);
	int pack = Axi::Min(dst.Model.Pack, src.Model.Pack);

	for(int ch=0 ; ch<channels ; ch++)
	{
		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, ch);
		TxScanner2D<TS> src_scan = ToScanner<TS>(src, ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : ch);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src_scan,
				[scale,pack](int y, int x, TD* _dst, TS* _src)
				{
					for(int k=0 ; k<pack ; k++)
					{
						switch(k)
						{
						case 0: _dst[2] = saturate_cast<TD>(_src[0] * scale); break;
						case 1: _dst[1] = saturate_cast<TD>(_src[1] * scale); break;
						case 2: _dst[0] = saturate_cast<TD>(_src[2] * scale); break;
						case 3: _dst[3] = saturate_cast<TD>(_src[3] * scale); break;
						}
					}
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[scale,pack](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
					{
						for(int k=0 ; k<pack ; k++)
						{
							switch(k)
							{
							case 0: _dst[2] = saturate_cast<TD>(_src[0] * scale); break;
							case 1: _dst[1] = saturate_cast<TD>(_src[1] * scale); break;
							case 2: _dst[0] = saturate_cast<TD>(_src[2] * scale); break;
							case 3: _dst[3] = saturate_cast<TD>(_src[3] * scale); break;
							}
						}
					}
				});
		}
	}
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToBgr_up(TxImage dst, TxImage mask, TxImage src, double scale)
{
	int channels = Axi::Min(dst.Channels, src.Model.Pack);

	for(int ch=0 ; ch<channels ; ch++)
	{
		int swap = ch;
		switch(ch)
		{
		case 0: swap = 2; break;
		case 1: swap = 1; break;
		case 2: swap = 0; break;
		}

		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, swap);
		TxScanner2D<TS> src_scan = ToScanner<TS>(src, 0);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : ch);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src_scan,
				[scale,ch](int y, int x, TD* _dst, TS* _src)
				{
					_dst[0] = saturate_cast<TD>(_src[ch] * scale);
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[scale,ch](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
						_dst[0] = saturate_cast<TD>(_src[ch] * scale);
				});
		}
	}
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToBgr_pu(TxImage dst, TxImage mask, TxImage src, double scale)
{
	int channels = Axi::Min(dst.Model.Pack, src.Channels);

	for(int ch=0 ; ch<channels ; ch++)
	{
		int swap = ch;
		switch(ch)
		{
		case 0: swap = 2; break;
		case 1: swap = 1; break;
		case 2: swap = 0; break;
		}

		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, 0);
		TxScanner2D<TS> src_scan = ToScanner<TS>(src, swap);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, 0);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src_scan,
				[scale,ch](int y, int x, TD* _dst, TS* _src)
				{
					_dst[ch] = saturate_cast<TD>(_src[0] * scale);
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[scale,ch](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
						_dst[ch] = saturate_cast<TD>(_src[0] * scale);
				});
		}
	}
}

// ======================================================================
template<class TD, class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToBgr_uu(TxImage dst, TxImage mask, TxImage src, double scale)
{
	int channels = Axi::Min(dst.Channels, src.Channels);

	for(int ch=0 ; ch<channels ; ch++)
	{
		int swap = ch;
		switch(ch)
		{
		case 0: swap = 2; break;
		case 1: swap = 1; break;
		case 2: swap = 0; break;
		}

		TxScanner2D<TD> dst_scan = ToScanner<TD>(dst, swap);
		TxScanner2D<TS> src_scan = ToScanner<TS>(src, ch);
		TxScanner2D<TM> mask_scan = ToScanner<TM>(mask, (mask.Channels == 1) ? 0 : ch);

		if (mask_scan.IsValid() == false)
		{
			dst_scan.ForEach(src_scan,
				[scale](int y, int x, TD* _dst, TS* _src)
				{
					_dst[0] = saturate_cast<TD>(_src[0] * scale);
				});
		}
		else
		{
			dst_scan.ForEach(src_scan, mask_scan,
				[scale](int y, int x, TD* _dst, TS* _src, TM* _mask)
				{
					if (*_mask != 0)
						_dst[0] = saturate_cast<TD>(_src[0] * scale);
				});
		}
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToBgr__p(TxImage dst, TxImage mask, TxImage src, double scale)
{
	switch(dst.Channels)
	{
	case 1:
		switch(dst.Model.Pack)
		{
		case 3:
		case 4:
			switch(dst.Model.Type)
			{
			case ExType::U8:	fnPRV_2D_RgbToBgr_pp<unsigned char,TM,TS>	(dst, mask, src, scale);	break;
			case ExType::U16:	fnPRV_2D_RgbToBgr_pp<unsigned short,TM,TS>	(dst, mask, src, scale);	break;
			case ExType::U32:	fnPRV_2D_RgbToBgr_pp<unsigned int,TM,TS>	(dst, mask, src, scale);	break;
			case ExType::S8:	fnPRV_2D_RgbToBgr_pp<char,TM,TS>			(dst, mask, src, scale);	break;
			case ExType::S16:	fnPRV_2D_RgbToBgr_pp<short,TM,TS>			(dst, mask, src, scale);	break;
			case ExType::S32:	fnPRV_2D_RgbToBgr_pp<int,TM,TS>				(dst, mask, src, scale);	break;
			case ExType::F32:	fnPRV_2D_RgbToBgr_pp<float,TM,TS>			(dst, mask, src, scale);	break;
			case ExType::F64:	fnPRV_2D_RgbToBgr_pp<double,TM,TS>			(dst, mask, src, scale);	break;
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
		break;
	case 3:
	case 4:
		switch(dst.Model.Pack)
		{
		case 1:
			switch(dst.Model.Type)
			{
			case ExType::U8:	fnPRV_2D_RgbToBgr_up<unsigned char,TM,TS>	(dst, mask, src, scale);	break;
			case ExType::U16:	fnPRV_2D_RgbToBgr_up<unsigned short,TM,TS>	(dst, mask, src, scale);	break;
			case ExType::U32:	fnPRV_2D_RgbToBgr_up<unsigned int,TM,TS>	(dst, mask, src, scale);	break;
			case ExType::S8:	fnPRV_2D_RgbToBgr_up<char,TM,TS>			(dst, mask, src, scale);	break;
			case ExType::S16:	fnPRV_2D_RgbToBgr_up<short,TM,TS>			(dst, mask, src, scale);	break;
			case ExType::S32:	fnPRV_2D_RgbToBgr_up<int,TM,TS>				(dst, mask, src, scale);	break;
			case ExType::F32:	fnPRV_2D_RgbToBgr_up<float,TM,TS>			(dst, mask, src, scale);	break;
			case ExType::F64:	fnPRV_2D_RgbToBgr_up<double,TM,TS>			(dst, mask, src, scale);	break;
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM, class TS> static inline void XIE_API fnPRV_2D_RgbToBgr__u(TxImage dst, TxImage mask, TxImage src, double scale)
{
	switch(dst.Channels)
	{
	case 1:
		switch(dst.Model.Pack)
		{
		case 3:
		case 4:
			switch(dst.Model.Type)
			{
			case ExType::U8:	fnPRV_2D_RgbToBgr_pu<unsigned char,TM,TS>	(dst, mask, src, scale);	break;
			case ExType::U16:	fnPRV_2D_RgbToBgr_pu<unsigned short,TM,TS>	(dst, mask, src, scale);	break;
			case ExType::U32:	fnPRV_2D_RgbToBgr_pu<unsigned int,TM,TS>	(dst, mask, src, scale);	break;
			case ExType::S8:	fnPRV_2D_RgbToBgr_pu<char,TM,TS>			(dst, mask, src, scale);	break;
			case ExType::S16:	fnPRV_2D_RgbToBgr_pu<short,TM,TS>			(dst, mask, src, scale);	break;
			case ExType::S32:	fnPRV_2D_RgbToBgr_pu<int,TM,TS>				(dst, mask, src, scale);	break;
			case ExType::F32:	fnPRV_2D_RgbToBgr_pu<float,TM,TS>			(dst, mask, src, scale);	break;
			case ExType::F64:	fnPRV_2D_RgbToBgr_pu<double,TM,TS>			(dst, mask, src, scale);	break;
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 3:
	case 4:
		switch(dst.Model.Pack)
		{
		case 1:
			switch(dst.Model.Type)
			{
			case ExType::U8:	fnPRV_2D_RgbToBgr_uu<unsigned char,TM,TS>	(dst, mask, src, scale);	break;
			case ExType::U16:	fnPRV_2D_RgbToBgr_uu<unsigned short,TM,TS>	(dst, mask, src, scale);	break;
			case ExType::U32:	fnPRV_2D_RgbToBgr_uu<unsigned int,TM,TS>	(dst, mask, src, scale);	break;
			case ExType::S8:	fnPRV_2D_RgbToBgr_uu<char,TM,TS>			(dst, mask, src, scale);	break;
			case ExType::S16:	fnPRV_2D_RgbToBgr_uu<short,TM,TS>			(dst, mask, src, scale);	break;
			case ExType::S32:	fnPRV_2D_RgbToBgr_uu<int,TM,TS>				(dst, mask, src, scale);	break;
			case ExType::F32:	fnPRV_2D_RgbToBgr_uu<float,TM,TS>			(dst, mask, src, scale);	break;
			case ExType::F64:	fnPRV_2D_RgbToBgr_uu<double,TM,TS>			(dst, mask, src, scale);	break;
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
template<class TM> static inline void XIE_API fnPRV_2D_RgbToBgr___(TxImage dst, TxImage mask, TxImage src, double scale)
{
	switch(src.Channels)
	{
	case 1:
		switch(src.Model.Pack)
		{
		case 3:
		case 4:
			switch(src.Model.Type)
			{
			case ExType::U8:	fnPRV_2D_RgbToBgr__p<TM,unsigned char>	(dst, mask, src, scale);	break;
			case ExType::U16:	fnPRV_2D_RgbToBgr__p<TM,unsigned short>	(dst, mask, src, scale);	break;
			case ExType::U32:	fnPRV_2D_RgbToBgr__p<TM,unsigned int>	(dst, mask, src, scale);	break;
			case ExType::S8:	fnPRV_2D_RgbToBgr__p<TM,char>			(dst, mask, src, scale);	break;
			case ExType::S16:	fnPRV_2D_RgbToBgr__p<TM,short>			(dst, mask, src, scale);	break;
			case ExType::S32:	fnPRV_2D_RgbToBgr__p<TM,int>			(dst, mask, src, scale);	break;
			case ExType::F32:	fnPRV_2D_RgbToBgr__p<TM,float>			(dst, mask, src, scale);	break;
			case ExType::F64:	fnPRV_2D_RgbToBgr__p<TM,double>			(dst, mask, src, scale);	break;
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	case 3:
	case 4:
		switch(src.Model.Pack)
		{
		case 1:
			switch(src.Model.Type)
			{
			case ExType::U8:	fnPRV_2D_RgbToBgr__u<TM,unsigned char>	(dst, mask, src, scale);	break;
			case ExType::U16:	fnPRV_2D_RgbToBgr__u<TM,unsigned short>	(dst, mask, src, scale);	break;
			case ExType::U32:	fnPRV_2D_RgbToBgr__u<TM,unsigned int>	(dst, mask, src, scale);	break;
			case ExType::S8:	fnPRV_2D_RgbToBgr__u<TM,char>			(dst, mask, src, scale);	break;
			case ExType::S16:	fnPRV_2D_RgbToBgr__u<TM,short>			(dst, mask, src, scale);	break;
			case ExType::S32:	fnPRV_2D_RgbToBgr__u<TM,int>			(dst, mask, src, scale);	break;
			case ExType::F32:	fnPRV_2D_RgbToBgr__u<TM,float>			(dst, mask, src, scale);	break;
			case ExType::F64:	fnPRV_2D_RgbToBgr__u<TM,double>			(dst, mask, src, scale);	break;
			default:
				throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		default:
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
		}
		break;
	default:
		throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
ExStatus XIE_API fnPRV_Core_2D_RgbToBgr(TxImage dst, TxImage mask, TxImage src, double scale)
{
	typedef unsigned char	TM;

	try
	{
		if (MaskValidity<TM>(mask, dst) == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		fnPRV_2D_RgbToBgr___<TM>(dst, mask, src, scale);

		return ExStatus::Success;
	}
	catch(const CxException& ex)
	{
		return ex.Code();
	}
}

}
