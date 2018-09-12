/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"

#pragma warning (disable:4127)	// 条件式が定数です.
#pragma warning (disable:4189)	// ローカル変数が初期化されましたが、参照されていません.

#include "api_gdi.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxImage.h"
#include "Core/CxArrayEx.h"

namespace xie
{
namespace GDI
{

const double FILTER_MODE_MAG = 0.5;

// ================================================================================
template<class TD, class TS> static inline void XIE_API fnPRV_GDI_Shrink2_pg(HxModule hdst, HxModule hsrc, double mag)
{
	CxImage* src = reinterpret_cast<CxImage*>(hsrc);
	CxImage* dst = reinterpret_cast<CxImage*>(hdst);
	
	// size
	if (src->Channels()	!= dst->Channels())
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	float scale = (float)Axi::CalcScale(src->Model().Type, src->Depth(), dst->Model().Type, dst->Depth());
	float fetch_mag = (float)(1 / mag);
	
	{
		int src_height = src->Height();
		int src_width  = src->Width();
		int dst_height = (int)(src_height * mag);
		int dst_width  = (int)(src_width  * mag);
		if (0 < dst_height && dst_height <= dst->Height() &&
			0 < dst_width && dst_width <= dst->Width())
		{
			// 出力側から見た入力側の指標.
			CxArrayEx<int> index_y(dst_height + 1);	index_y.Clear(0);
			CxArrayEx<int> index_x(dst_width + 1);	index_x.Clear(0);
			for(int i=0 ; i<index_y.Length() ; i++)
				index_y[i] = (int)(i / mag);
			for(int i=0 ; i<index_x.Length() ; i++)
				index_x[i] = (int)(i / mag);
			if (index_y[index_y.Length() -1] > src_height)
				index_y[index_y.Length() -1] = src_height;
			if (index_x[index_x.Length() -1] > src_width)
				index_x[index_x.Length() -1] = src_width;

			int inc = (int)round(FILTER_MODE_MAG * fetch_mag);
			for(int y=0 ; y<dst_height ; y++)
			{
				// 積分画像.
				int size_y = 0;
				CxArrayEx<int> sumy(src_width); sumy.Clear(0);
				for(int yyy=index_y[y] ; yyy<index_y[y+1] ; yyy++)
				{
					const TS* src_addr = (const TS*)(*src)(0, yyy, 0);
					for(int xxx=0 ; xxx<src_width ; xxx++)
						sumy[xxx] += (int)(src_addr[xxx] * scale + 0.5f);
					size_y++;
				}
				if (size_y == 0) continue;

				// 出力画像.
				TD* dst_addr = (TD*)(*dst)(0, y, 0);
				for(int x=0 ; x<dst_width ; x++)
				{
					int size_x = 0;
					int	sumx = 0;
					for(int i=index_x[x] ; i<index_x[x+1] ; i++)
					{
						sumx += sumy[i];
						size_x++;
					}
					if (size_x == 0) continue;
					int G = (int)(sumx / size_x / size_y);
					dst_addr[x].R = ((G < 0) ? 0 : (G > 255) ? 255 : (unsigned char)(G));
					dst_addr[x].G = ((G < 0) ? 0 : (G > 255) ? 255 : (unsigned char)(G));
					dst_addr[x].B = ((G < 0) ? 0 : (G > 255) ? 255 : (unsigned char)(G));
				}
			}
		}
	}
}

// ================================================================================
template<class TD, class TS> static inline void XIE_API fnPRV_GDI_Shrink2_pp(HxModule hdst, HxModule hsrc, double mag)
{
	CxImage* src = reinterpret_cast<CxImage*>(hsrc);
	CxImage* dst = reinterpret_cast<CxImage*>(hdst);
	
	// size
	if (src->Channels()	!= dst->Channels())
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	float scale = (float)Axi::CalcScale(src->Model().Type, src->Depth(), dst->Model().Type, dst->Depth());
	float fetch_mag = (float)(1 / mag);

	{
		int src_height = src->Height();
		int src_width  = src->Width();
		int dst_height = (int)(src_height * mag);
		int dst_width  = (int)(src_width  * mag);
		if (0 < dst_height && dst_height <= dst->Height() &&
			0 < dst_width && dst_width <= dst->Width())
		{
			// 出力側から見た入力側の指標.
			CxArrayEx<int> index_y(dst_height + 1);	index_y.Clear(0);
			CxArrayEx<int> index_x(dst_width + 1);	index_x.Clear(0);
			for(int i=0 ; i<index_y.Length() ; i++)
				index_y[i] = (int)(i / mag);
			for(int i=0 ; i<index_x.Length() ; i++)
				index_x[i] = (int)(i / mag);
			if (index_y[index_y.Length() -1] > src_height)
				index_y[index_y.Length() -1] = src_height;
			if (index_x[index_x.Length() -1] > src_width)
				index_x[index_x.Length() -1] = src_width;

			int inc = (int)round(FILTER_MODE_MAG * fetch_mag);
			for(int y=0 ; y<dst_height ; y++)
			{
				// 積分画像.
				int size_y = 0;
				CxArrayEx<int> sumy0(src_width);	sumy0.Clear(0);
				CxArrayEx<int> sumy1(src_width);	sumy1.Clear(0);
				CxArrayEx<int> sumy2(src_width);	sumy2.Clear(0);
				for(int yyy=index_y[y] ; yyy<index_y[y+1] ; yyy++)
				{
					const TS* src_addr = (const TS*)(*src)(0, yyy, 0);
					for(int xxx=0 ; xxx<src_width ; xxx++)
					{
						sumy0[xxx] += (int)(src_addr[xxx].R * scale + 0.5f);
						sumy1[xxx] += (int)(src_addr[xxx].G * scale + 0.5f);
						sumy2[xxx] += (int)(src_addr[xxx].B * scale + 0.5f);
					}
					size_y++;
				}
				if (size_y == 0) continue;

				// 出力画像.
				TD* dst_addr = (TD*)(*dst)(0, y, 0);
				for(int x=0 ; x<dst_width ; x++)
				{
					int size_x = 0;
					int	sumx0 = 0;
					int	sumx1 = 0;
					int	sumx2 = 0;
					for(int i=index_x[x] ; i<index_x[x+1] ; i++)
					{
						sumx0 += sumy0[i];
						sumx1 += sumy1[i];
						sumx2 += sumy2[i];
						size_x++;
					}
					if (size_x == 0) continue;
					int R = (int)(sumx0 / size_x / size_y);
					int G = (int)(sumx1 / size_x / size_y);
					int B = (int)(sumx2 / size_x / size_y);
					dst_addr[x].R = ((R < 0) ? 0 : (R > 255) ? 255 : (unsigned char)(R));
					dst_addr[x].G = ((G < 0) ? 0 : (G > 255) ? 255 : (unsigned char)(G));
					dst_addr[x].B = ((B < 0) ? 0 : (B > 255) ? 255 : (unsigned char)(B));
				}
			}
		}
	}
}

// ================================================================================
template<class TD, class TS> static inline void XIE_API fnPRV_GDI_Shrink2_pu(HxModule hdst, HxModule hsrc, double mag)
{
	CxImage* src = reinterpret_cast<CxImage*>(hsrc);
	CxImage* dst = reinterpret_cast<CxImage*>(hdst);
	
	// size
	if (src->Channels() < 3)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	float scale = (float)Axi::CalcScale(src->Model().Type, src->Depth(), dst->Model().Type, dst->Depth());
	float fetch_mag = (float)(1 / mag);
	
	{
		int src_height = src->Height();
		int src_width  = src->Width();
		int dst_height = (int)(src_height * mag);
		int dst_width  = (int)(src_width  * mag);
		if (0 < dst_height && dst_height <= dst->Height() &&
			0 < dst_width && dst_width <= dst->Width())
		{
			// 出力側から見た入力側の指標.
			CxArrayEx<int> index_y(dst_height + 1);	index_y.Clear(0);
			CxArrayEx<int> index_x(dst_width + 1);	index_x.Clear(0);
			for(int i=0 ; i<index_y.Length() ; i++)
				index_y[i] = (int)(i / mag);
			for(int i=0 ; i<index_x.Length() ; i++)
				index_x[i] = (int)(i / mag);
			if (index_y[index_y.Length() -1] > src_height)
				index_y[index_y.Length() -1] = src_height;
			if (index_x[index_x.Length() -1] > src_width)
				index_x[index_x.Length() -1] = src_width;

			int inc = (int)round(FILTER_MODE_MAG * fetch_mag);
			for(int y=0 ; y<dst_height ; y++)
			{
				// 積分画像.
				int size_y = 0;
				CxArrayEx<int> sumy0(src_width);	sumy0.Clear(0);
				CxArrayEx<int> sumy1(src_width);	sumy1.Clear(0);
				CxArrayEx<int> sumy2(src_width);	sumy2.Clear(0);
				for(int yyy=index_y[y] ; yyy<index_y[y+1] ; yyy++)
				{
					const TS* src_addr0 = (const TS*)(*src)(0, yyy, 0);
					const TS* src_addr1 = (const TS*)(*src)(1, yyy, 0);
					const TS* src_addr2 = (const TS*)(*src)(2, yyy, 0);
					for(int xxx=0 ; xxx<src_width ; xxx++)
					{
						sumy0[xxx] += (int)(src_addr0[xxx] * scale + 0.5f);
						sumy1[xxx] += (int)(src_addr1[xxx] * scale + 0.5f);
						sumy2[xxx] += (int)(src_addr2[xxx] * scale + 0.5f);
					}
					size_y++;
				}
				if (size_y == 0) continue;

				// 出力画像.
				TD* dst_addr = (TD*)(*dst)(0, y, 0);
				for(int x=0 ; x<dst_width ; x++)
				{
					int size_x = 0;
					int	sumx0 = 0;
					int	sumx1 = 0;
					int	sumx2 = 0;
					for(int i=index_x[x] ; i<index_x[x+1] ; i++)
					{
						sumx0 += sumy0[i];
						sumx1 += sumy1[i];
						sumx2 += sumy2[i];
						size_x++;
					}
					if (size_x == 0) continue;
					int R = (int)(sumx0 / size_x / size_y);
					int G = (int)(sumx1 / size_x / size_y);
					int B = (int)(sumx2 / size_x / size_y);
					dst_addr[x].R = ((R < 0) ? 0 : (R > 255) ? 255 : (unsigned char)(R));
					dst_addr[x].G = ((G < 0) ? 0 : (G > 255) ? 255 : (unsigned char)(G));
					dst_addr[x].B = ((B < 0) ? 0 : (B > 255) ? 255 : (unsigned char)(B));
				}
			}
		}
	}
}

// ================================================================================
template<class TD, class TS> static inline void XIE_API fnPRV_GDI_Shrink2_uu(HxModule hdst, HxModule hsrc, double mag)
{
	CxImage* src = reinterpret_cast<CxImage*>(hsrc);
	CxImage* dst = reinterpret_cast<CxImage*>(hdst);
	
	// size
	if (src->Channels()	!= dst->Channels())
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	float scale = (float)Axi::CalcScale(src->Model().Type, src->Depth(), dst->Model().Type, dst->Depth());
	float fetch_mag = (float)(1 / mag);
	
	{
		int src_height = src->Height();
		int src_width  = src->Width();
		int dst_height = (int)(src_height * mag);
		int dst_width  = (int)(src_width  * mag);
		if (0 < dst_height && dst_height <= dst->Height() &&
			0 < dst_width && dst_width <= dst->Width())
		{
			// 出力側から見た入力側の指標.
			CxArrayEx<int> index_y(dst_height + 1);	index_y.Clear(0);
			CxArrayEx<int> index_x(dst_width + 1);	index_x.Clear(0);
			for(int i=0 ; i<index_y.Length() ; i++)
				index_y[i] = (int)(i / mag);
			for(int i=0 ; i<index_x.Length() ; i++)
				index_x[i] = (int)(i / mag);
			if (index_y[index_y.Length() -1] > src_height)
				index_y[index_y.Length() -1] = src_height;
			if (index_x[index_x.Length() -1] > src_width)
				index_x[index_x.Length() -1] = src_width;

			int inc = (int)round(FILTER_MODE_MAG * fetch_mag);
			for(int y=0 ; y<dst_height ; y++)
			{
				// 積分画像.
				int size_y = 0;
				CxArrayEx<int> sumy(src_width); sumy.Clear(0);
				for(int yyy=index_y[y] ; yyy<index_y[y+1] ; yyy++)
				{
					const TS* src_addr = (const TS*)(*src)(0, yyy, 0);
					for(int xxx=0 ; xxx<src_width ; xxx++)
						sumy[xxx] += (int)(src_addr[xxx] * scale + 0.5f);
					size_y++;
				}
				if (size_y == 0) continue;

				// 出力画像.
				TD* dst_addr = (TD*)(*dst)(0, y, 0);
				for(int x=0 ; x<dst_width ; x++)
				{
					int size_x = 0;
					int	sumx = 0;
					for(int i=index_x[x] ; i<index_x[x+1] ; i++)
					{
						sumx += sumy[i];
						size_x++;
					}
					if (size_x == 0) continue;
					int G = (int)(sumx / size_x / size_y);
					dst_addr[x] = ((G < 0) ? 0 : (G > 255) ? 255 : (unsigned char)(G));
				}
			}
		}
	}
}

// ================================================================================
template<class TD> static inline void XIE_API fnPRV_GDI_Shrink2__p(HxModule hdst, HxModule hsrc, double mag)
{
	CxImage* src = reinterpret_cast<CxImage*>(hsrc);

	switch (src->Model().Pack)
	{
	case 1:
		if (src->Channels() == 1)
		{
			switch(src->Model().Type)
			{
			case ExType::U8:	fnPRV_GDI_Shrink2_pg<TD, unsigned char>		( hdst, hsrc, mag );	break;
			case ExType::U16:	fnPRV_GDI_Shrink2_pg<TD, unsigned short>	( hdst, hsrc, mag );	break;
			case ExType::U32:	fnPRV_GDI_Shrink2_pg<TD, unsigned int>		( hdst, hsrc, mag );	break;
			case ExType::S8:	fnPRV_GDI_Shrink2_pg<TD, char>				( hdst, hsrc, mag );	break;
			case ExType::S16:	fnPRV_GDI_Shrink2_pg<TD, short>				( hdst, hsrc, mag );	break;
			case ExType::S32:	fnPRV_GDI_Shrink2_pg<TD, int>				( hdst, hsrc, mag );	break;
			case ExType::F32:	fnPRV_GDI_Shrink2_pg<TD, float>				( hdst, hsrc, mag );	break;
			case ExType::F64:	fnPRV_GDI_Shrink2_pg<TD, double>			( hdst, hsrc, mag );	break;
			default:
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}
		}
		else
		{
			switch(src->Model().Type)
			{
			case ExType::U8:	fnPRV_GDI_Shrink2_pu<TD, unsigned char>		( hdst, hsrc, mag );	break;
			case ExType::U16:	fnPRV_GDI_Shrink2_pu<TD, unsigned short>	( hdst, hsrc, mag );	break;
			case ExType::U32:	fnPRV_GDI_Shrink2_pu<TD, unsigned int>		( hdst, hsrc, mag );	break;
			case ExType::S8:	fnPRV_GDI_Shrink2_pu<TD, char>				( hdst, hsrc, mag );	break;
			case ExType::S16:	fnPRV_GDI_Shrink2_pu<TD, short>				( hdst, hsrc, mag );	break;
			case ExType::S32:	fnPRV_GDI_Shrink2_pu<TD, int>				( hdst, hsrc, mag );	break;
			case ExType::F32:	fnPRV_GDI_Shrink2_pu<TD, float>				( hdst, hsrc, mag );	break;
			case ExType::F64:	fnPRV_GDI_Shrink2_pu<TD, double>			( hdst, hsrc, mag );	break;
			default:
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}
		}
		break;
	case 3:
		{
			switch(src->Model().Type)
			{
			case ExType::U8:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx3<unsigned char>>	( hdst, hsrc, mag );	break;
			case ExType::U16:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx3<unsigned short>>	( hdst, hsrc, mag );	break;
			case ExType::U32:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx3<unsigned int>>		( hdst, hsrc, mag );	break;
			case ExType::S8:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx3<char>>				( hdst, hsrc, mag );	break;
			case ExType::S16:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx3<short>>			( hdst, hsrc, mag );	break;
			case ExType::S32:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx3<int>>				( hdst, hsrc, mag );	break;
			case ExType::F32:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx3<float>>			( hdst, hsrc, mag );	break;
			case ExType::F64:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx3<double>>			( hdst, hsrc, mag );	break;
			default:
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}
		}
		break;
	case 4:
		{
			switch(src->Model().Type)
			{
			case ExType::U8:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx4<unsigned char>>	( hdst, hsrc, mag );	break;
			case ExType::U16:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx4<unsigned short>>	( hdst, hsrc, mag );	break;
			case ExType::U32:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx4<unsigned int>>		( hdst, hsrc, mag );	break;
			case ExType::S8:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx4<char>>				( hdst, hsrc, mag );	break;
			case ExType::S16:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx4<short>>			( hdst, hsrc, mag );	break;
			case ExType::S32:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx4<int>>				( hdst, hsrc, mag );	break;
			case ExType::F32:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx4<float>>			( hdst, hsrc, mag );	break;
			case ExType::F64:	fnPRV_GDI_Shrink2_pp<TD, TxRGBx4<double>>			( hdst, hsrc, mag );	break;
			default:
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}
		}
		break;
	default:
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ================================================================================
template<class TD> static inline void XIE_API fnPRV_GDI_Shrink2__u(HxModule hdst, HxModule hsrc, double mag)
{
	CxImage* src = reinterpret_cast<CxImage*>(hsrc);

	// 入力画像の種別による分岐.
	switch(src->Model().Type)
	{
	case ExType::U8:	fnPRV_GDI_Shrink2_uu<TD, unsigned char>		( hdst, hsrc, mag );	break;
	case ExType::U16:	fnPRV_GDI_Shrink2_uu<TD, unsigned short>	( hdst, hsrc, mag );	break;
	case ExType::U32:	fnPRV_GDI_Shrink2_uu<TD, unsigned int>		( hdst, hsrc, mag );	break;
	case ExType::S16:	fnPRV_GDI_Shrink2_uu<TD, short>				( hdst, hsrc, mag );	break;
	case ExType::S8:	fnPRV_GDI_Shrink2_uu<TD, char>				( hdst, hsrc, mag );	break;
	case ExType::S32:	fnPRV_GDI_Shrink2_uu<TD, int>				( hdst, hsrc, mag );	break;
	case ExType::F32:	fnPRV_GDI_Shrink2_uu<TD, float>				( hdst, hsrc, mag );	break;
	case ExType::F64:	fnPRV_GDI_Shrink2_uu<TD, double>			( hdst, hsrc, mag );	break;
	default:
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	}
}

// ======================================================================
void XIE_API fnPRV_GDI_Shrink2(HxModule hdst, HxModule hsrc, double mag, bool swap)
{
	if (!(0.0 < mag && mag <= FILTER_MODE_MAG))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	CxImage* src = xie::Axi::SafeCast<CxImage>(hsrc);
	CxImage* dst = xie::Axi::SafeCast<CxImage>(hdst);
	if (src == NULL || dst == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (src->IsValid() == false ||
		dst->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	// 出力画像の種別による分岐.
	switch(dst->Model().Type)
	{
	default:
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	case ExType::U8:
		switch(dst->Model().Pack)
		{
		default:
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		case 1: fnPRV_GDI_Shrink2__u<unsigned char>	( hdst, hsrc, mag );	break;
		case 3:
			if (swap)
				fnPRV_GDI_Shrink2__p<TxBGR8x3>		( hdst, hsrc, mag );
			else
				fnPRV_GDI_Shrink2__p<TxRGB8x3>		( hdst, hsrc, mag );
			break;
		case 4:
			if (swap)
				fnPRV_GDI_Shrink2__p<TxBGR8x4>		( hdst, hsrc, mag );
			else
				fnPRV_GDI_Shrink2__p<TxRGB8x4>		( hdst, hsrc, mag );
			break;
		}
		break;
	}
}

}
}
