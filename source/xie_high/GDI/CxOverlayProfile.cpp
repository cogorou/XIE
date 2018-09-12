/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxOverlayProfile.h"
#include "Core/CxException.h"
#include "Core/Axi.h"
#include "GDI/CxCanvas.h"
#include "GDI/CxGdiLineSegment.h"
#include "GDI/CxGdiPolyline.h"
#include "GDI/CxGdiRectangle.h"
#include "GDI/CxGdiStringA.h"

#if defined(_MSC_VER)
#include <GL/gl.h>
#else
#include <GL/glew.h>
#include <GL/gl.h>
#endif

namespace xie
{
namespace GDI
{

static const char* g_ClassName = "CxOverlayProfile";

// ================================================================================
void CxOverlayProfile::_Constructor()
{
}

// ================================================================================
CxOverlayProfile::CxOverlayProfile() : CxOverlay()
{
	_Constructor();
}

// ================================================================================
CxOverlayProfile::CxOverlayProfile( const CxOverlayProfile& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxOverlayProfile::~CxOverlayProfile()
{
}

// ================================================================================
CxOverlayProfile& CxOverlayProfile::operator = ( const CxOverlayProfile& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxOverlayProfile::operator == ( const CxOverlayProfile& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxOverlayProfile::operator != ( const CxOverlayProfile& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
CxOverlayProfile CxOverlayProfile::Clone() const
{
	CxOverlayProfile clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

//
// IxEquatable
//

// ================================================================================
void CxOverlayProfile::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxOverlayProfile>(src))
	{
		auto&	_src = static_cast<const CxOverlayProfile&>(src);
		auto&	_dst = *this;
		CxOverlay::CopyFrom(src);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxOverlayProfile::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxOverlayProfile>(src))
	{
		auto&	_src = static_cast<const CxOverlayProfile&>(src);
		auto&	_dst = *this;
		return CxOverlay::ContentEquals(src);
	}
	return false;
}

//
// IxParam
//

// ============================================================
void CxOverlayProfile::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "CursorPosition") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, m_CursorPosition)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	CxOverlay::GetParam(name, value, model);
}

// ============================================================
void CxOverlayProfile::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Image") == 0)
	{
		if (auto image = xie::Axi::SafeCast<CxImage>((HxModule)value))
		{
			m_Image.Attach(*image);
			return;
		}
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "CursorPosition") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, m_CursorPosition)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	CxOverlay::SetParam(name, value, model);
}

// ////////////////////////////////////////////////////////////
// 
// Overlay Inheritance
// 

// ================================================================================
void CxOverlayProfile::Render(HxModule hcanvas, ExGdiScalingMode mode) const
{
	if (m_Visible == false) return;

	if (auto canvas = xie::Axi::SafeCast<CxCanvas>(hcanvas))
	{
		if (m_Image.IsValid())
		{
			auto mag = canvas->Magnification();
			auto dsp = canvas->DisplayRect();
			auto vis = canvas->VisibleRect();
			auto eff = canvas->EffectiveRect();

			auto size = m_Image.ImageSize();
			auto elements = size.Model.Pack * size.Channels;

			auto channelNo = canvas->ChannelNo();
			if (channelNo < 0)
				channelNo = 0;
			if (channelNo > elements - 1)
				channelNo = elements - 1;

			auto unpack = canvas->Unpack();
			if (unpack)
				elements = 1;

			auto pos_x = (int)round(m_CursorPosition.X);
			auto pos_y = (int)round(m_CursorPosition.Y);
			if (pos_x < 0)
				pos_x = 0;
			if (pos_y < 0)
				pos_y = 0;
			if (pos_x > size.Width - 1)
				pos_x = size.Width - 1;
			if (pos_y > size.Height - 1)
				pos_y = size.Height - 1;

			// ----------------------------------------------------------------------
			// Visualization
			/*
				RulerX         Cursor[0]
				 L M H           X
				 | | |           |
				-----------------+-------- Y: Cursor[1]
				 | | |           |
				 | | |           |
				-+-+-+-----------|-------- H: RulerY
				-+-+-+-----------|-------- M:
				-+-+-+-----------|-------- L:
			*/
			CxArrayEx<TxRGB8x3> colors = 
			{
				TxRGB8x3(0xFF, 0x00, 0x00),	// red
				TxRGB8x3(0x00, 0xFF, 0x00),	// green
				TxRGB8x3(0x00, 0x00, 0xFF),	// blue
				TxRGB8x3(0xFF, 0x00, 0xFF),	// magenta
				TxRGB8x3(0xFF, 0xFF, 0x00),	// yellow
				TxRGB8x3(0x00, 0xFF, 0xFF),	// cyan
			};

			auto origin = TxPointD(eff.X, eff.Y + eff.Height);

			auto rulerL = TxPointD(origin.X +  25.0, origin.Y -  25.0);
			auto rulerM = TxPointD(origin.X +  75.0, origin.Y -  75.0);
			auto rulerH = TxPointD(origin.X + 125.0, origin.Y - 125.0);

			auto w_enough = (rulerH.X < (eff.X + eff.Width - 25.0));
			auto h_enough = (rulerH.Y > (eff.Y + 25.0));

			auto range = xie::Axi::CalcRange(size.Model.Type, size.Depth);
			auto range_diff = range.Upper - range.Lower;

			// RulerX
			if (w_enough)
			{
				CxArrayEx<CxGdiLineSegment>	figures = 
				{
					CxGdiLineSegment(rulerL.X, eff.Y, rulerL.X, eff.Y + eff.Height),
					CxGdiLineSegment(rulerM.X, eff.Y, rulerM.X, eff.Y + eff.Height),
					CxGdiLineSegment(rulerH.X, eff.Y, rulerH.X, eff.Y + eff.Height),
				};
				auto color = TxRGB8x3(0x00, 0xFF, 0xFF);	// cyan

				for(int i=0 ; i<figures.Length() ; i++)
				{
					figures[i].PenColor(color);
					figures[i].PenStyle(ExGdiPenStyle::Dot);
				}

				for(int i=0 ; i<figures.Length() ; i++)
					canvas->DrawOverlay(figures[i], ExGdiScalingMode::None);
			}

			// RulerY
			if (h_enough)
			{
				CxArrayEx<CxGdiLineSegment>	figures
				{
					CxGdiLineSegment(eff.X, rulerL.Y, eff.X + eff.Width, rulerL.Y),
					CxGdiLineSegment(eff.X, rulerM.Y, eff.X + eff.Width, rulerM.Y),
					CxGdiLineSegment(eff.X, rulerH.Y, eff.X + eff.Width, rulerH.Y),
				};
				auto color = TxRGB8x3(0x00, 0xFF, 0xFF);	// cyan

				for(int i=0 ; i<figures.Length() ; i++)
				{
					figures[i].PenColor(color);
					figures[i].PenStyle(ExGdiPenStyle::Dot);
				}

				for(int i=0 ; i<figures.Length() ; i++)
					canvas->DrawOverlay(figures[i], ExGdiScalingMode::None);
			}

			// ProfileX
			if (w_enough && range_diff > 0)
			{
				CxArrayEx<CxGdiPolyline>	figures(elements);
				CxArray profile(size.Height, ModelOf<double>());
				auto st = canvas->DPtoIP(rulerM, ExGdiScalingMode::TopLeft);
				auto ed = canvas->DPtoIP(rulerH, ExGdiScalingMode::TopLeft);

				for(int ch=0 ; ch<size.Channels ; ch++)
				{
					for(int k=0 ; k<size.Model.Pack ; k++)
					{
						auto index = ch * size.Model.Pack + k;
						if (unpack)
						{
							if (index != channelNo) continue;
							index = 0;
						}

						GetProfileX(profile, m_Image, ch, k, pos_x);

						CxGdiPolyline& figure = figures[index];
						auto color = (unpack)
							? colors[channelNo % colors.Length()]
							: colors[index % colors.Length()];
						figure.PenColor(color);
						figure.Resize(profile.Length());
						auto dst_scan = figure.Scanner();
						auto src_scan = profile.Scanner<double>();
						for(int s=0 ; s<dst_scan.Length ; s++)
						{
							auto dens = src_scan[s] / range_diff * (ed.X - st.X);
							dst_scan[s] = TxPointD(st.X + dens, s);
						}
					}
				}

				for(int i=0 ; i<elements ; i++)
					canvas->DrawOverlay(figures[i], ExGdiScalingMode::TopLeft);
			}

			// ProfileY
			if (h_enough && range_diff > 0)
			{
				CxArrayEx<CxGdiPolyline>	figures(elements);
				CxArray profile(size.Width, ModelOf<double>());
				auto st = canvas->DPtoIP(rulerM, ExGdiScalingMode::TopLeft);
				auto ed = canvas->DPtoIP(rulerH, ExGdiScalingMode::TopLeft);

				for(int ch=0 ; ch<size.Channels ; ch++)
				{
					for(int k=0 ; k<size.Model.Pack ; k++)
					{
						auto index = ch * size.Model.Pack + k;
						if (unpack)
						{
							if (index != channelNo) continue;
							index = 0;
						}

						GetProfileY(profile, m_Image, ch, k, pos_y);

						CxGdiPolyline& figure = figures[index];
						auto color = (unpack)
							? colors[channelNo % colors.Length()]
							: colors[index % colors.Length()];
						figure.PenColor(color);
						figure.Resize(profile.Length());
						auto dst_scan = figure.Scanner();
						auto src_scan = profile.Scanner<double>();
						for(int s=0 ; s<dst_scan.Length ; s++)
						{
							auto dens = src_scan[s] / range_diff * (ed.X - st.X);
							dst_scan[s] = TxPointD(s, st.Y - dens);
						}
					}
				}

				for(int i=0 ; i<elements ; i++)
					canvas->DrawOverlay(figures[i], ExGdiScalingMode::TopLeft);
			}

			// Cursor
			{
				auto dp = canvas->IPtoDP(m_CursorPosition, ExGdiScalingMode::Center);
				CxArrayEx<CxGdiLineSegment>	figures = 
				{
					CxGdiLineSegment(dp.X, eff.Y, dp.X, eff.Y + eff.Height),
					CxGdiLineSegment(eff.X, dp.Y, eff.X + eff.Width, dp.Y),
				};
				auto color = TxRGB8x3(0xFF, 0xFF, 0x00);	// yellow

				for(int i=0 ; i<figures.Length() ; i++)
				{
					figures[i].PenColor(color);
				}

				for(int i=0 ; i<figures.Length() ; i++)
					canvas->DrawOverlay(figures[i], ExGdiScalingMode::None);
			}

			// Text
			if (elements > 0)
			{
				CxArrayEx<double>		values(elements);
				CxArrayEx<CxGdiStringA>	figures(elements + 1);
				auto color = TxRGB8x3(0x00, 0xFF, 0x00);	// green
				double max_w = 0;
				double sum_h = 0;

				// values on cursor position
				for(int ch=0 ; ch<size.Channels ; ch++)
				{
					for(int k=0 ; k<size.Model.Pack ; k++)
					{
						auto index = ch * size.Model.Pack + k;
						if (unpack)
						{
							if (index != channelNo) continue;
							index = 0;
						}

						values[index] = GetValue(m_Image, ch, k, pos_y, pos_x);
					}
				}

				// coordinate and the value of each element
				for(int i=0 ; i<figures.Length() ; i++)
				{
					#if defined(_MSC_VER)
					TxCharCPtrA		font_name = "Consolas";
					const int	font_size = 14;
					#else
					TxCharCPtrA		font_name = NULL;
					const int	font_size = 12;
					#endif

					figures[i].FontName(font_name);
					figures[i].FontSize(font_size);

					if (i == 0)
					{
						figures[i] = CxString::Format(" X,Y = %.2f, %.2f ", m_CursorPosition.X, m_CursorPosition.Y);
					}
					else
					{
						auto index = i - 1;
						auto value = values[index];

						switch(size.Model.Type)
						{
						default:
							break;
						case ExType::U8:
						case ExType::U16:
						case ExType::U32:
						case ExType::U64:
						case ExType::S8:
						case ExType::S16:
						case ExType::S32:
						case ExType::S64:
							figures[i] = CxString::Format(" [%d] %.0f ", index, value);
							break;
						case ExType::F32:
						case ExType::F64:
							figures[i] = CxString::Format(" [%d] %.6f ", index, value);
							break;
						}
					}

					auto bounds = figures[i].Bounds();
					if (max_w < bounds.Width)
						max_w = bounds.Width;
					sum_h += bounds.Height;
				}

				auto dp = canvas->IPtoDP(m_CursorPosition, ExGdiScalingMode::Center);
				auto ave_h = sum_h / figures.Length();
				auto background = TxRectangleD(dp + 16, TxSizeD(max_w, sum_h + ave_h * 2));

				if (background.X + background.Width > dsp.X + dsp.Width)
					background.X = dp.X - 16 - background.Width;

				if (background.Y + background.Height > dsp.Y + dsp.Height)
					background.Y = dp.Y - 16 - background.Height;

				// text position
				for(int i=0 ; i<figures.Length() ; i++)
				{
					figures[i].PenColor(color);
					figures[i].Align(ExGdiTextAlign::TopLeft);
					figures[i].Location(TxPointD(background.X, background.Y + ave_h * (i+1)));
				}

				// draw background
				{
					CxGdiRectangle figure;
					auto color = TxRGB8x4(32, 32, 32, 192);	// black (alpha=75%)
					figure = background;
					figure.PenColor(color);
					figure.BkColor(color);
					figure.BkEnable(true);

					canvas->DrawOverlay(figure, ExGdiScalingMode::None);
				}

				// draw texts
				for(int i=0 ; i<figures.Length() ; i++)
					canvas->DrawOverlay(figures[i], ExGdiScalingMode::None);
			}
		}
		return;
	}
}

// ================================================================================
template<class TD, class TS> void fnPRV_GDI_GetProfileX(CxArray& dst, const CxImage& src, int ch, int k, int x)
{
	auto dst_scan = dst.Scanner<TD>();
	auto src_scan = src.Scanner<TS>(ch);
	for(int i=0 ; i<src_scan.Height ; i++)
	{
		TD& _dst = (TD&)dst_scan[i];
		TS* _src = (TS*)&src_scan(i, x);
		_dst = (TD)_src[k];
	}
}

// ================================================================================
void CxOverlayProfile::GetProfileX(CxArray& dst, const CxImage& src, int ch, int k, int x)
{
	if (dst.IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (src.IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	switch(src.Model().Type)
	{
	default:
		break;
	case ExType::U8:	fnPRV_GDI_GetProfileX<double, unsigned char>		(dst, src, ch, k, x);	break;
	case ExType::U16:	fnPRV_GDI_GetProfileX<double, unsigned short>		(dst, src, ch, k, x);	break;
	case ExType::U32:	fnPRV_GDI_GetProfileX<double, unsigned int>			(dst, src, ch, k, x);	break;
	case ExType::U64:	fnPRV_GDI_GetProfileX<double, unsigned long long>	(dst, src, ch, k, x);	break;
	case ExType::S8:	fnPRV_GDI_GetProfileX<double, char>					(dst, src, ch, k, x);	break;
	case ExType::S16:	fnPRV_GDI_GetProfileX<double, short>				(dst, src, ch, k, x);	break;
	case ExType::S32:	fnPRV_GDI_GetProfileX<double, int>					(dst, src, ch, k, x);	break;
	case ExType::S64:	fnPRV_GDI_GetProfileX<double, long long>			(dst, src, ch, k, x);	break;
	case ExType::F32:	fnPRV_GDI_GetProfileX<double, float>				(dst, src, ch, k, x);	break;
	case ExType::F64:	fnPRV_GDI_GetProfileX<double, double>				(dst, src, ch, k, x);	break;
	}
}

// ================================================================================
template<class TD, class TS> void fnPRV_GDI_GetProfileY(CxArray& dst, const CxImage& src, int ch, int k, int y)
{
	auto dst_scan = dst.Scanner<TD>();
	auto src_scan = src.Scanner<TS>(ch);
	for(int i=0 ; i<src_scan.Width ; i++)
	{
		TD& _dst = (TD&)dst_scan[i];
		TS* _src = (TS*)&src_scan(y, i);
		_dst = (TD)_src[k];
	}
}

// ================================================================================
void CxOverlayProfile::GetProfileY(CxArray& dst, const CxImage& src, int ch, int k, int y)
{
	if (dst.IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (src.IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	switch(src.Model().Type)
	{
	default:
		break;
	case ExType::U8:	fnPRV_GDI_GetProfileY<double, unsigned char>		(dst, src, ch, k, y);	break;
	case ExType::U16:	fnPRV_GDI_GetProfileY<double, unsigned short>		(dst, src, ch, k, y);	break;
	case ExType::U32:	fnPRV_GDI_GetProfileY<double, unsigned int>			(dst, src, ch, k, y);	break;
	case ExType::U64:	fnPRV_GDI_GetProfileY<double, unsigned long long>	(dst, src, ch, k, y);	break;
	case ExType::S8:	fnPRV_GDI_GetProfileY<double, char>					(dst, src, ch, k, y);	break;
	case ExType::S16:	fnPRV_GDI_GetProfileY<double, short>				(dst, src, ch, k, y);	break;
	case ExType::S32:	fnPRV_GDI_GetProfileY<double, int>					(dst, src, ch, k, y);	break;
	case ExType::S64:	fnPRV_GDI_GetProfileY<double, long long>			(dst, src, ch, k, y);	break;
	case ExType::F32:	fnPRV_GDI_GetProfileY<double, float>				(dst, src, ch, k, y);	break;
	case ExType::F64:	fnPRV_GDI_GetProfileY<double, double>				(dst, src, ch, k, y);	break;
	}
}

// ================================================================================
template<class TD, class TS> TD fnPRV_GDI_GetValue(const CxImage& src, int ch, int k, int y, int x)
{
	auto src_scan = src.Scanner<TS>(ch);
	TS* _src = (TS*)&src_scan(y, x);
	return (TD)_src[k];
}

// ================================================================================
double CxOverlayProfile::GetValue(const CxImage& src, int ch, int k, int y, int x)
{
	if (src.IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	double dst = 0;
	switch(src.Model().Type)
	{
	default:
		break;
	case ExType::U8:	dst = fnPRV_GDI_GetValue<double, unsigned char>			(src, ch, k, y, x);	break;
	case ExType::U16:	dst = fnPRV_GDI_GetValue<double, unsigned short>		(src, ch, k, y, x);	break;
	case ExType::U32:	dst = fnPRV_GDI_GetValue<double, unsigned int>			(src, ch, k, y, x);	break;
	case ExType::U64:	dst = fnPRV_GDI_GetValue<double, unsigned long long>	(src, ch, k, y, x);	break;
	case ExType::S8:	dst = fnPRV_GDI_GetValue<double, char>					(src, ch, k, y, x);	break;
	case ExType::S16:	dst = fnPRV_GDI_GetValue<double, short>					(src, ch, k, y, x);	break;
	case ExType::S32:	dst = fnPRV_GDI_GetValue<double, int>					(src, ch, k, y, x);	break;
	case ExType::S64:	dst = fnPRV_GDI_GetValue<double, long long>				(src, ch, k, y, x);	break;
	case ExType::F32:	dst = fnPRV_GDI_GetValue<double, float>					(src, ch, k, y, x);	break;
	case ExType::F64:	dst = fnPRV_GDI_GetValue<double, double>				(src, ch, k, y, x);	break;
	}
	return dst;
}

}	// GDI
}	// xie
