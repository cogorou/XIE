/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"

#include "api_gdi.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxImage.h"
#include "Core/CxArrayEx.h"
#include "GDI/CxBitmap.h"

namespace xie
{
namespace GDI
{

// ======================================================================
template <class TD, class TS> static CxImage fnPRV_GDI_Extract_FieldU(const CxImage& src, int index)
{
	CxImage dst(src.Width(), src.Height(), ModelOf<TD>(), 1);
	double scale = xie::Axi::CalcScale(src.Model().Type, src.Depth(), dst.Model().Type, dst.Depth());
	int height = src.Height();
	int width  = src.Width();
	int pack   = src.Model().Pack;
	for(int y=0 ; y<height ; y++)
	{
		TD* _dst = (TD*)dst(0, y, 0);
		TS* _src = (TS*)src(0, y, 0);
		for(int x=0 ; x<width ; x++)
		{
			_dst[x] = saturate_cast<TD>(_src[x * pack + index] * scale);
		}
	}
	return dst;
}

// ======================================================================
template <class TDF, class TS> static CxImage fnPRV_GDI_Extract_FieldS(const CxImage& src, int index)
{
	typedef TxRGBx3<TDF> TD;
	CxImage dst(src.Width(), src.Height(), TD::Model(), 1);
	double scale = xie::Axi::CalcScale(src.Model().Type, src.Depth(), dst.Model().Type, dst.Depth());
	int height = src.Height();
	int width  = src.Width();
	int pack   = src.Model().Pack;
	for(int y=0 ; y<height ; y++)
	{
		TD* _dst = (TD*)dst(0, y, 0);
		TS* _src = (TS*)src(0, y, 0);
		for(int x=0 ; x<width ; x++)
		{
			double src_value = _src[x * pack + index] * scale;
			if (src_value >= 0)
			{
				TDF value = saturate_cast<TDF>(src_value);
				_dst[x].R = value;
				_dst[x].G = value;
				_dst[x].B = value;
			}
			else
			{
				TDF value = saturate_cast<TDF>(src_value * -1);
				_dst[x].R = value;
				_dst[x].G = 0;
				_dst[x].B = value;
			}
		}
	}
	return dst;
}

// ======================================================================
template <class TD, class TS> static CxImage fnPRV_GDI_Abs_uu(const CxImage& src)
{
	CxImage dst(src.Width(), src.Height(), ModelOf<TD>(), (src.Channels() == 1) ? 1 : 3);
	double scale = xie::Axi::CalcScale(src.Model().Type, src.Depth(), dst.Model().Type, dst.Depth());
	int height = src.Height();
	int width  = src.Width();
	int dst_channels = dst.Channels();
	int src_channels = src.Channels();
	int channels   = xie::Axi::Min(dst_channels, src_channels);
	for(int ch=0 ; ch<channels ; ch++)
	{
		for(int y=0 ; y<height ; y++)
		{
			TD*	_dst = (TD*)dst(ch, y, 0);
			TS*	_src = (TS*)src(ch, y, 0);
			for(int x=0 ; x<width ; x++)
			{
				_dst[x] = saturate_cast<TD>(_abs(_src[x]) * scale);
			}
		}
	}
	return dst;
}

// ======================================================================
template <class TDF, class TS> static CxImage fnPRV_GDI_Abs_pp(const CxImage& src)
{
	typedef TxRGBx3<TDF> TD;
	CxImage dst(src.Width(), src.Height(), TD::Model(), 1);
	double scale = xie::Axi::CalcScale(src.Model().Type, src.Depth(), dst.Model().Type, dst.Depth());
	int height = src.Height();
	int width  = src.Width();
	int dst_pack = dst.Model().Pack;
	int src_pack = src.Model().Pack;
	int pack   = xie::Axi::Min(dst_pack, src_pack);
	for(int y=0 ; y<height ; y++)
	{
		TDF*	_dst = (TDF*)dst(0, y, 0);
		TS*		_src = (TS*)src(0, y, 0);
		for(int x=0 ; x<width ; x++)
		{
			for(int k=0 ; k<pack ; k++)
				_dst[k] = saturate_cast<TDF>(_abs(_src[k]) * scale);
			_dst += dst_pack;
			_src += src_pack;
		}
	}
	return dst;
}

// ======================================================================
void XIE_API fnPRV_GDI_Extract(HxModule hdst, HxModule hsrc, const TxCanvas& canvas, bool swap)
{
	if (xie::Axi::ClassIs<CxImage>(hsrc) == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	CxImage&	src = *reinterpret_cast<CxImage*>(hsrc);
	CxImage		act;
	if (src.IsValid() == false)
	{
		if (auto dst = xie::Axi::SafeCast<IxDisposable>(hdst))
		{
			dst->Dispose();
		}
		return;
	}

	double			mag = canvas.Magnification;
	TxRectangleI	eff = canvas.EffectiveRect();
	TxRectangleI	vis = canvas.VisibleRectI(true);

	// ----------------------------------------------------------------------
	// ì¸óÕâÊëú.
	if (canvas.Unpack == ExBoolean::True)
	{
		//
		// unpacking (specify channel)
		//

		typedef unsigned char	TD;

		switch( src.Model().Pack )
		{
		case 0: break;
		case 1:
			{
				int ch = canvas.ChannelNo;
				if (ch < 0)
					ch = 0;
				if (ch > src.Channels() - 1)
					ch = src.Channels() - 1;

				switch( src.Model().Type )
				{
				default: break;
				case ExType::U8:	act.Attach(src, ch, vis); break;
				case ExType::U16:	act.Attach(src, ch, vis); break;
				case ExType::U32:	act.Attach(src, ch, vis); break;
				case ExType::S8:	act = fnPRV_GDI_Extract_FieldS<TD, char>	(src.Child(ch, vis), 0); break;
				case ExType::S16:	act = fnPRV_GDI_Extract_FieldS<TD, short>	(src.Child(ch, vis), 0); break;
				case ExType::S32:	act = fnPRV_GDI_Extract_FieldS<TD, int>		(src.Child(ch, vis), 0); break;
				case ExType::F32:	act = fnPRV_GDI_Extract_FieldS<TD, float>	(src.Child(ch, vis), 0); break;
				case ExType::F64:	act = fnPRV_GDI_Extract_FieldS<TD, double>	(src.Child(ch, vis), 0); break;
				}
			}
			break;
		default:
			{
				int ch = canvas.ChannelNo / src.Model().Pack;
				if (ch < 0)
					ch = 0;
				if (ch > src.Channels() - 1)
					ch = src.Channels() - 1;

				int pack = canvas.ChannelNo % src.Model().Pack;
				if (pack < 0)
					pack = 0;
				if (pack > src.Model().Pack - 1)
					pack = src.Model().Pack - 1;

				switch( src.Model().Type )
				{
				default: break;
				case ExType::U8:	act = fnPRV_GDI_Extract_FieldU<TD, unsigned char>	(src.Child(ch, vis), pack); break;
				case ExType::U16:	act = fnPRV_GDI_Extract_FieldU<TD, unsigned short>	(src.Child(ch, vis), pack); break;
				case ExType::U32:	act = fnPRV_GDI_Extract_FieldU<TD, unsigned int>	(src.Child(ch, vis), pack); break;
				case ExType::S8:	act = fnPRV_GDI_Extract_FieldS<TD, char>			(src.Child(ch, vis), pack); break;
				case ExType::S16:	act = fnPRV_GDI_Extract_FieldS<TD, short>			(src.Child(ch, vis), pack); break;
				case ExType::S32:	act = fnPRV_GDI_Extract_FieldS<TD, int>				(src.Child(ch, vis), pack); break;
				case ExType::F32:	act = fnPRV_GDI_Extract_FieldS<TD, float>			(src.Child(ch, vis), pack); break;
				case ExType::F64:	act = fnPRV_GDI_Extract_FieldS<TD, double>			(src.Child(ch, vis), pack); break;
				}
			}
			break;
		}
	}
	else
	{
		//
		// packing
		//

		int ch = canvas.ChannelNo; 
		if (ch < 0)
			ch = 0;
		if (ch > src.Channels() - 1)
			ch = src.Channels() - 1;

		int		channels = src.Channels();
		int		pack = src.Model().Pack;
		ExType	type = src.Model().Type;

		switch( pack )
		{
		case 0: break;
		case 1:
			switch(channels)
			{
			case 0: break;
			case 1:
			case 3:
			case 4:
				switch(type)
				{
				default: break;
				case ExType::U8:	act.Attach(src, -1, vis); break;
				case ExType::U16:	act.Attach(src, -1, vis); break;
				case ExType::U32:	act.Attach(src, -1, vis); break;
				case ExType::S8:	act = fnPRV_GDI_Abs_uu<unsigned char, char>		(src.Child(-1, vis)); break;
				case ExType::S16:	act = fnPRV_GDI_Abs_uu<unsigned char, short>	(src.Child(-1, vis)); break;
				case ExType::S32:	act = fnPRV_GDI_Abs_uu<unsigned char, int>		(src.Child(-1, vis)); break;
				case ExType::F32:	act = fnPRV_GDI_Abs_uu<unsigned char, float>	(src.Child(-1, vis)); break;
				case ExType::F64:	act = fnPRV_GDI_Abs_uu<unsigned char, double>	(src.Child(-1, vis)); break;
				}
				break;
			default:
				switch(type)
				{
				default: break;
				case ExType::U8:	act = fnPRV_GDI_Abs_uu<unsigned char, unsigned char>	(src.Child(-1, vis)); break;
				case ExType::U16:	act = fnPRV_GDI_Abs_uu<unsigned char, unsigned short>	(src.Child(-1, vis)); break;
				case ExType::U32:	act = fnPRV_GDI_Abs_uu<unsigned char, unsigned int>		(src.Child(-1, vis)); break;
				case ExType::S8:	act = fnPRV_GDI_Abs_uu<unsigned char, char>		(src.Child(-1, vis)); break;
				case ExType::S16:	act = fnPRV_GDI_Abs_uu<unsigned char, short>	(src.Child(-1, vis)); break;
				case ExType::S32:	act = fnPRV_GDI_Abs_uu<unsigned char, int>		(src.Child(-1, vis)); break;
				case ExType::F32:	act = fnPRV_GDI_Abs_uu<unsigned char, float>	(src.Child(-1, vis)); break;
				case ExType::F64:	act = fnPRV_GDI_Abs_uu<unsigned char, double>	(src.Child(-1, vis)); break;
				}
				break;
			}
			break;
		case 3:
		case 4:
			{
				switch(type)
				{
				default: break;
				case ExType::U8:	act.Attach(src, ch, vis); break;
				case ExType::U16:	act.Attach(src, ch, vis); break;
				case ExType::U32:	act.Attach(src, ch, vis); break;
				case ExType::S8:	act = fnPRV_GDI_Abs_pp<unsigned char, char>		(src.Child(ch, vis)); break;
				case ExType::S16:	act = fnPRV_GDI_Abs_pp<unsigned char, short>	(src.Child(ch, vis)); break;
				case ExType::S32:	act = fnPRV_GDI_Abs_pp<unsigned char, int>		(src.Child(ch, vis)); break;
				case ExType::F32:	act = fnPRV_GDI_Abs_pp<unsigned char, float>	(src.Child(ch, vis)); break;
				case ExType::F64:	act = fnPRV_GDI_Abs_pp<unsigned char, double>	(src.Child(ch, vis)); break;
				}
			}
			break;
		default:
			{
				switch(type)
				{
				default: break;
				case ExType::U8:	act = fnPRV_GDI_Abs_pp<unsigned char, unsigned char>	(src.Child(ch, vis)); break;
				case ExType::U16:	act = fnPRV_GDI_Abs_pp<unsigned char, unsigned short>	(src.Child(ch, vis)); break;
				case ExType::U32:	act = fnPRV_GDI_Abs_pp<unsigned char, unsigned int>		(src.Child(ch, vis)); break;
				case ExType::S8:	act = fnPRV_GDI_Abs_pp<unsigned char, char>		(src.Child(ch, vis)); break;
				case ExType::S16:	act = fnPRV_GDI_Abs_pp<unsigned char, short>	(src.Child(ch, vis)); break;
				case ExType::S32:	act = fnPRV_GDI_Abs_pp<unsigned char, int>		(src.Child(ch, vis)); break;
				case ExType::F32:	act = fnPRV_GDI_Abs_pp<unsigned char, float>	(src.Child(ch, vis)); break;
				case ExType::F64:	act = fnPRV_GDI_Abs_pp<unsigned char, double>	(src.Child(ch, vis)); break;
				}
			}
			break;
		}
	}

	// ----------------------------------------------------------------------
	// î{ó¶ ÅÉ 1.0 : â¬éãîÕàÕÇèkè¨ÇµÇƒÉRÉsÅ[ÇµÇ‹Ç∑.
	// î{ó¶ ÅÜ 1.0 : â¬éãîÕàÕÇ 1.0 î{Ç≈ÉRÉsÅ[ÇµÇ‹Ç∑.
	if (mag < 1.0)
	{
		CxImage buf;
		int		dst_width	= (int)(act.Width() * mag);
		int		dst_height	= (int)(act.Height() * mag);
		if (auto dst = xie::Axi::SafeCast<CxBitmap>(hdst))
		{
			dst->Resize(dst_width, dst_height);
			buf.Attach(dst->Tag());
		}
		else if (auto dst = xie::Axi::SafeCast<CxImage>(hdst))
		{
			dst->Resize(dst_width, dst_height, TxModel::U8(4), 1);
			buf.Attach(dst->Tag());
		}

		if (buf.IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		if (1.0 <= mag || canvas.Halftone == ExBoolean::False)
			fnPRV_GDI_Shrink0(buf, act, mag, swap);
		else if (0.5 < mag && mag < 1.0)
			fnPRV_GDI_Shrink1(buf, act, mag, swap);
		else
			fnPRV_GDI_Shrink2(buf, act, mag, swap);
	}
	else
	{
		if (act.IsValid() == false)
		{
			if (auto dst = xie::Axi::SafeCast<IxDisposable>(hdst))
			{
				dst->Dispose();
			}
		}
		else
		{
			CxImage buf;
			if (auto dst = xie::Axi::SafeCast<CxBitmap>(hdst))
			{
				if (dst->Size() != act.Size())
					dst->Resize(act.Size());
				buf.Attach(dst->Tag());
			}
			else if (auto dst = xie::Axi::SafeCast<CxImage>(hdst))
			{
				if (dst->Size() != act.Size() ||
					dst->Model() != TxModel::U8(4) ||
					dst->Channels() != 1)
				{
					dst->Resize(act.Width(), act.Height(), TxModel::U8(4), 1);
				}
				buf.Attach(dst->Tag());
			}

			if (buf.IsValid() == false)
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

			double scale = Axi::CalcScale(act.Model().Type, act.Depth(), buf.Model().Type, buf.Depth());

			if (act.Channels() == 1 && act.Model().Pack == 1)
			{
				buf.Filter().Copy(act, scale);				// RGB Å© Gray
			}
			else
			{
				if (swap == false)
					buf.Filter().Copy(act, scale);			// RGB Å© RGB
				else
					buf.Filter().RgbToBgr(act, scale);		// BGR Å© RGB
			}
		}
	}
}

}
}
