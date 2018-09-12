/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiStringW.h"
#include "GDI/CxCanvas.h"
#include "GDI/CxBitmap.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"
#include "Core/CxString.h"
#include "Core/IxParam.h"
#include "Core/Axi.h"

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

static const char* g_ClassName = "CxGdiStringW";

// ================================================================================
void CxGdiStringW::_Constructor()
{
	m_Tag.Param = TxGdi2dParam::Default();
	m_Tag = TxGdiStringW::Default();
}

// ================================================================================
CxGdiStringW::CxGdiStringW()
{
	_Constructor();
}

// ================================================================================
CxGdiStringW::CxGdiStringW( const CxGdiStringW& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiStringW::CxGdiStringW( const CxStringW& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiStringW::~CxGdiStringW()
{
	Dispose();
	FontName(NULL);
}

// ================================================================================
CxGdiStringW& CxGdiStringW::operator = ( const CxGdiStringW& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiStringW& CxGdiStringW::operator = ( const CxStringW& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiStringW& CxGdiStringW::operator = ( TxCharCPtrW src )
{
	this->Text( src );
	return *this;
}

// ================================================================================
bool CxGdiStringW::operator == ( const CxGdiStringW& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiStringW::operator != ( const CxGdiStringW& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxGdiStringW CxGdiStringW::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxGdiStringW::TagPtr() const
{
	return (void*)&m_Tag;
}

// ================================================================================
CxGdiStringW::operator TxCharCPtrW() const
{
	return m_Tag.Address;
}

// ================================================================================
CxGdiStringW::operator CxStringW() const
{
	CxStringW dst;
	this->CopyTo(dst);
	return dst;
}

// ================================================================================
void CxGdiStringW::CopyTo(IxModule& dst) const
{
	if (xie::Axi::ClassIs<CxStringA>(dst))
	{
		CxStringA&	_dst = static_cast<CxStringA&>(dst);

		if (this->IsValid() == false)
			_dst.Dispose();
		else
			_dst.CopyFrom( CxStringW(m_Tag.Address) );
		return;
	}
	if (xie::Axi::ClassIs<CxStringW>(dst))
	{
		CxStringW&	_dst = static_cast<CxStringW&>(dst);

		if (this->IsValid() == false)
			_dst.Dispose();
		else
			_dst = m_Tag.Address;
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
CxGdiStringW CxGdiStringW::Clone() const
{
	CxGdiStringW clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiStringW::Dispose()
{
	if (m_Tag.Address != NULL)
		xie::Axi::MemoryFree(m_Tag.Address);
	m_Tag.Address	= NULL;
	m_Tag.Length	= 0;
}

// ================================================================================
void CxGdiStringW::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiStringW>(src))
	{
		auto&	_src = static_cast<const CxGdiStringW&>(src);
		auto&	_dst = *this;

		_dst.m_Tag.Param = _src.m_Tag.Param;

		// 文字列は Text で複製されます.
		_dst.Text(_src.Text());
	//	_dst.m_Tag.Address		= _src.m_Tag.Address;
	//	_dst.m_Tag.Length	= _src.m_Tag.Length;
	//	_dst.m_Tag.Model	= _src.m_Tag.Model;

		_dst.m_Tag.X		= _src.m_Tag.X;
		_dst.m_Tag.Y		= _src.m_Tag.Y;

		// フォント名は FontName で複製されます.
		_dst.FontName(_src.FontName());
		_dst.m_Tag.FontSize	= _src.m_Tag.FontSize;
		_dst.m_Tag.Align	= _src.m_Tag.Align;

		return;
	}
	if (xie::Axi::ClassIs<CxStringA>(src))
	{
		const CxStringA&	_src = static_cast<const CxStringA&>(src);
		CxStringW			_tmp;
		_tmp.CopyFrom(_src);

		CxGdiStringW&		_dst = *this;
		_dst.Text(_tmp.Address());
		return;
	}
	if (xie::Axi::ClassIs<CxStringW>(src))
	{
		const CxStringW&	_src = static_cast<const CxStringW&>(src);
		CxGdiStringW&		_dst = *this;
		_dst.Text(_src.Address());
		return;
	}
	if (auto _src = xie::Axi::SafeCast<IxConvertible>(&src))
	{
		_src->CopyTo(*this);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxGdiStringW::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiStringW>(src))
	{
		auto&	_src = static_cast<const CxGdiStringW&>(src);
		auto&	_dst = *this;

		if (_dst.m_Tag.Param != _src.m_Tag.Param) return false;

		if (_dst.m_Tag.Length	!= _src.m_Tag.Length) return false;
		if (_dst.m_Tag.X		!= _src.m_Tag.X) return false;
		if (_dst.m_Tag.Y		!= _src.m_Tag.Y) return false;

		if (_dst.m_Tag.Align	!= _src.m_Tag.Align) return false;
		if (_dst.m_Tag.FontSize	!= _src.m_Tag.FontSize) return false;

		// フォント名の比較.
		if (_dst.m_Tag.FontName == NULL && _src.m_Tag.FontName != NULL) return false;
		if (_dst.m_Tag.FontName != NULL && _src.m_Tag.FontName == NULL) return false;
		if (_dst.m_Tag.FontName != NULL && _src.m_Tag.FontName != NULL)
		{
			if (wcscmp(_dst.m_Tag.FontName, _src.m_Tag.FontName) != 0) return false;
		}

		// 文字列の比較.
		if (_dst.m_Tag.Address == NULL && _src.m_Tag.Address != NULL) return false;
		if (_dst.m_Tag.Address != NULL && _src.m_Tag.Address == NULL) return false;
		if (_dst.m_Tag.Address != NULL && _src.m_Tag.Address != NULL)
		{
			if (wcscmp(_dst.m_Tag.Address, _src.m_Tag.Address) != 0) return false;
		}

		return true;
	}
	return false;
}

// ============================================================
bool CxGdiStringW::IsValid() const
{
	if (m_Tag.Address == NULL) return false;
	if (m_Tag.Length <= 0) return false;
	return true;
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
void CxGdiStringW::Render(HxModule hcanvas, ExGdiScalingMode mode) const
{
	if (this->IsValid() == false) return;

	#if defined(_MSC_VER)
	{
		IxParam*	dpi = xie::Axi::SafeCast<IxParam>(hcanvas);
		if (dpi == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		// 表示範囲パラメータ取得.
		TxCanvas	canvas;
		dpi->GetParam("Tag", &canvas, TxModel::Default());

		HxModule	hBuffer = NULL;
		dpi->GetParam("Buffer", &hBuffer, TxModel::Ptr(1));
		CxBitmap*	buffer = xie::Axi::SafeCast<CxBitmap>(hBuffer);
		if (buffer != NULL)
		{
			double			mag = canvas.Magnification;
			TxRectangleI	display_rect(0, 0, canvas.Width, canvas.Height);
			TxPointD		vp = canvas.ViewPoint;
			TxRectangleI	eff = CxCanvas::EffectiveRect(display_rect, canvas.BgSize, mag);
			TxRectangleD	vis = CxCanvas::VisibleRect(display_rect, canvas.BgSize, mag, vp);

			int			pen_width = this->PenWidth();
			TxRGB8x4	pen_color = this->PenColor();
			int			bk_mode  = this->BkEnable() ? OPAQUE : TRANSPARENT;
			TxRGB8x4	bk_color = this->BkColor();
			COLORREF	back_color = RGB(bk_color.R, bk_color.G, bk_color.B);
			COLORREF	text_color = RGB(pen_color.R, pen_color.G, pen_color.B);
			int			text_align = (int)m_Tag.Align;

			// Create FONT
			HFONT hfont	= this->_CreateFont();

			// Rendering
			{
				HDC hdcDst = buffer->GetHDC();
				int dcid = ::SaveDC( hdcDst );
				if( dcid )
				{
					HGDIOBJ hPrevFont = NULL;
					if (hfont != NULL)
						hPrevFont = ::SelectObject(hdcDst, hfont);

					// background mode / color
					::SetBkMode( hdcDst, bk_mode );
					::SetBkColor( hdcDst, back_color );

					// text color / align
					::SetTextColor( hdcDst, text_color );
					::SetTextAlign( hdcDst, text_align );
					
					// ----------------------------------------------------------------------
					// affine
					TxPointD	pos = this->Location();
					{
						double			mag = canvas.Magnification;
						TxRectangleI	display_rect(0, 0, canvas.Width, canvas.Height);
						TxPointD		vp = canvas.ViewPoint;
						TxRectangleI	eff = CxCanvas::EffectiveRect(display_rect, canvas.BgSize, mag);
						TxRectangleD	vis = CxCanvas::VisibleRect(display_rect, canvas.BgSize, mag, vp);

						switch(mode)
						{
						default:
						case ExGdiScalingMode::None:
							mag = 1.0;
							break;
						case ExGdiScalingMode::TopLeft:
							pos = (this->Location() * mag) + (eff.Location() - (vis.Location() * mag));
							break;
						case ExGdiScalingMode::Center:
							pos = (this->Location() * mag) + (eff.Location() - ((vis.Location() - 0.5) * mag));
							break;
						}

						fnPRV_GDI_WorldTransformReset(hdcDst);

						if (this->Angle() != 0.0)
						{
							TxPointD axis = pos + this->Axis();
							fnPRV_GDI_WorldTransformRotate(hdcDst, axis.X, axis.Y, this->Angle());
						}

						if (mode != ExGdiScalingMode::None)
							fnPRV_GDI_WorldTransformScale(hdcDst, pos.X, pos.Y, mag, mag);
					}

					// coordinate
					int dst_x = (int)round(pos.X);
					int dst_y = (int)round(pos.Y);

					// draw
					BOOL status = ::TextOutW( hdcDst, dst_x, dst_y, m_Tag.Address, m_Tag.Length );

					// restore
					if (hPrevFont != NULL)
						::SelectObject(hdcDst, hPrevFont);

					::RestoreDC( hdcDst, dcid );
				}
			}

			// Release
			::DeleteObject(hfont);
		}
	}
	#else
	{
		CxCanvas*	canvas = xie::Axi::SafeCast<CxCanvas>(hcanvas);
		if (canvas == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		if (canvas->IsValid() == false)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

		::glPushMatrix();
		::glEnable( GL_BLEND );
		::glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

		// ----------------------------------------------------------------------
		// affine (Rotate)
		if (this->Angle() != 0.0)
		{
			double		angle = this->Angle();	// degree
			TxPointD	axis = this->Axis() + this->Location();
			::glTranslated	(+axis.X, +axis.Y, 0.0);
			::glRotated		(angle, 0, 0, 1);
			::glTranslated	(-axis.X, -axis.Y, 0.0);
		}

		{
			auto xserver = canvas->XServer();

			// Font
			CxStringW	font_name_w = _CreateFontName();
			CxStringA	font_name_a;
			font_name_a.CopyFrom(font_name_w);
			fnXIE_Core_TraceOut(2, "font_name=%s\n", (font_name_a.IsValid() ? font_name_a.Address() : "(NULL)"));

			XFontStruct*	font_info = ::XLoadQueryFont(xserver, font_name_a.Address());
			fnXIE_Core_TraceOut(2, "XLoadQueryFont return %p.\n", font_info);

			if (font_info != NULL)
			{
				/*
					https://www.opengl.org/wiki/Programming_OpenGL_in_Linux:_Programming_Animations_with_GLX_and_Xlib

					https://www.opengl.org/sdk/docs/man2/xhtml/glGenLists.xml
					https://www.opengl.org/sdk/docs/man2/xhtml/glDeleteLists.xml
					https://www.opengl.org/sdk/docs/man2/xhtml/glXUseXFont.xml
				*/
				auto st = font_info->min_char_or_byte2;
				auto ed = font_info->max_char_or_byte2;
				auto range = ed+1;
				auto lists = ::glGenLists(range);

				CxFinalizer lists_finalizer([lists,range]()
					{
						if (lists != 0)
							::glDeleteLists(lists, range);
					});

				if (lists != 0)
				{
					::glXUseXFont(font_info->fid, st, ed-st+1, lists+st);
				}

				if (lists != 0)
				{
					fnPRV_GDI_PenPrepare(&m_Tag.Param);

					TxRectangleD rect = this->Bounds();
					::glRasterPos2i(rect.X, rect.Y + rect.Height);
					::glListBase(lists);
					::glCallLists(this->Length(), GL_UNSIGNED_BYTE, this->Text());

					fnPRV_GDI_PenRestore(&m_Tag.Param);
				}
			}
		}

		::glDisable( GL_BLEND );
		::glPopMatrix();
	}
	#endif
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
TxPointD CxGdiStringW::Location() const
{
	return TxPointD(m_Tag.X, m_Tag.Y);
}

// ================================================================================
void CxGdiStringW::Location(TxPointD value)
{
	m_Tag.X = value.X;
	m_Tag.Y = value.Y;
}

// ================================================================================
TxRectangleD CxGdiStringW::Bounds() const
{
	TxRectangleD bounds;
	bounds.X = m_Tag.X;
	bounds.Y = m_Tag.Y;
	bounds.Width = 0;
	bounds.Height = 0;

	if (m_Tag.Address != NULL)
	{
		TxCharCPtrW	text = m_Tag.Address;
		int		length = (int)wcslen(text);

		TxSizeI size(0, 0);

		#if defined(_MSC_VER)
		{
			HDC		hdc		= fnPRV_GDI_CreateDC();
			HFONT	hfont	= this->_CreateFont();
			if (hdc != NULL && hfont != NULL)
			{
				::SelectObject(hdc, hfont);

				SIZE sz = { 0, 0 };
				::GetTextExtentPoint32W( hdc, text, length, &sz );
				size.Width  = sz.cx;
				size.Height = sz.cy;

				::SelectObject(hdc, NULL);
			}
			::DeleteObject(hfont);
			::DeleteDC(hdc);
		}
		#else
		{
			size.Height = m_Tag.FontSize;
			size.Width  = m_Tag.FontSize * length * 60 / 100;

			#if 1
			if (size.Height > 0 && size.Width > 0)
			{
				Display* xserver = fnPRV_GDI_XServer_Open();
				CxFinalizer xserver_finalizer([xserver]()
					{
						if (xserver != NULL)
							fnPRV_GDI_XServer_Close(xserver);
					});
				if (xserver != NULL)
				{
					CxStringW	font_name_w = _CreateFontName();
					CxStringA	font_name_a;
					font_name_a.CopyFrom(font_name_w);

					int			missing_count = 0;
					char**		missing_list = NULL;
					char*		def_string = NULL;
					XFontSet font_set = XCreateFontSet(xserver, font_name_a.Address(), &missing_list, &missing_count, &def_string);
					CxFinalizer font_finalizer([xserver,font_set,missing_list]()
						{
							if (font_set != NULL)
								XFreeFontSet(xserver,font_set);
							if (missing_list != NULL)
								XFreeStringList(missing_list);
						});

					if (font_set != NULL)
					{
						CxStringW	text_w = Text();
						CxStringA	text_a;
						text_a.CopyFrom(text_w);
						int length_a = text_a.Length();

						XRectangle ink;
						XRectangle log;
						XmbTextExtents(font_set, text_a.Address(), length_a, &ink, &log);

						size.Height = log.height;
						size.Width = log.width;
					}
				}
			}
			#endif
		}
		#endif

		switch(m_Tag.Align)
		{
		default:
			break;
		case ExGdiTextAlign::TopLeft:
			bounds.X		= m_Tag.X;
			bounds.Y		= m_Tag.Y;
			bounds.Width	= size.Width;
			bounds.Height	= size.Height;
			break;
		case ExGdiTextAlign::TopRight:
			bounds.X		= m_Tag.X - size.Width;
			bounds.Y		= m_Tag.Y;
			bounds.Width	= size.Width;
			bounds.Height	= size.Height;
			break;
		case ExGdiTextAlign::TopCenter:
			bounds.X		= m_Tag.X - size.Width / 2;
			bounds.Y		= m_Tag.Y;
			bounds.Width	= size.Width;
			bounds.Height	= size.Height;
			break;
		case ExGdiTextAlign::BottomLeft:
			bounds.X		= m_Tag.X;
			bounds.Y		= m_Tag.Y - size.Height;
			bounds.Width	= size.Width;
			bounds.Height	= size.Height;
			break;
		case ExGdiTextAlign::BottomRight:
			bounds.X		= m_Tag.X - size.Width;
			bounds.Y		= m_Tag.Y - size.Height;
			bounds.Width	= size.Width;
			bounds.Height	= size.Height;
			break;
		case ExGdiTextAlign::BottomCenter:
			bounds.X		= m_Tag.X - size.Width / 2;
			bounds.Y		= m_Tag.Y - size.Height;
			bounds.Width	= size.Width;
			bounds.Height	= size.Height;
			break;
		case ExGdiTextAlign::BaselineLeft:
			bounds.X		= m_Tag.X;
			bounds.Y		= m_Tag.Y - size.Height;
			bounds.Width	= size.Width;
			bounds.Height	= size.Height;
			break;
		case ExGdiTextAlign::BaselineRight:
			bounds.X		= m_Tag.X - size.Width;
			bounds.Y		= m_Tag.Y - size.Height;
			bounds.Width	= size.Width;
			bounds.Height	= size.Height;
			break;
		case ExGdiTextAlign::BaselineCenter:
			bounds.X		= m_Tag.X - size.Width / 2;
			bounds.Y		= m_Tag.Y - size.Height;
			bounds.Width	= size.Width;
			bounds.Height	= size.Height;
			break;
		}
	}

	return bounds;
}

// ============================================================
double CxGdiStringW::Angle() const
{
	return m_Tag.Param.Angle;
}

// ============================================================
void CxGdiStringW::Angle( double degree )
{
	m_Tag.Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiStringW::Axis() const
{
	return m_Tag.Param.Axis;
}

// ============================================================
void CxGdiStringW::Axis( TxPointD value )
{
	m_Tag.Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiStringW::HitTest( TxPointD position, double margin ) const
{
	if (this->IsValid() == false)
		return TxHitPosition::Default();

	TxRectangleD	rect = this->Bounds();
	TxPointD		p0 = xie::Axi::Rotate<TxPointD>(position, this->Location()+this->Axis(), -this->Angle());

	auto hit = fnPRV_GDI_HitTest_Rectangle(p0, margin, rect);
	if (hit.Mode != 0)
		return TxHitPosition(1, 0, 0);
	return TxHitPosition::Default();
}

// ================================================================================
void CxGdiStringW::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiStringW>(prev_figure))
	{
		auto hit = figure->HitTest(prev_position, margin);

		switch(hit.Mode)
		{
			case 1:
				{
					auto mv = move_position - prev_position;
					this->Location( figure->Location() + mv );
				}
				break;
			case 2:
				break;
		}

		return;
	}
	throw xie::CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ============================================================
TxRGB8x4 CxGdiStringW::BkColor() const
{
	return m_Tag.Param.BkColor;
}

// ============================================================
void CxGdiStringW::BkColor( TxRGB8x4 value )
{
	m_Tag.Param.BkColor = value;
}

// ============================================================
bool CxGdiStringW::BkEnable() const
{
	return (m_Tag.Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiStringW::BkEnable( bool value )
{
	m_Tag.Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiStringW::PenColor() const
{
	return m_Tag.Param.PenColor;
}

// ============================================================
void CxGdiStringW::PenColor( TxRGB8x4 value )
{
	m_Tag.Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiStringW::PenStyle() const
{
	return m_Tag.Param.PenStyle;
}

// ============================================================
void CxGdiStringW::PenStyle( ExGdiPenStyle value )
{
	m_Tag.Param.PenStyle = value;
}

// ============================================================
int CxGdiStringW::PenWidth() const
{
	return m_Tag.Param.PenWidth;
}

// ============================================================
void CxGdiStringW::PenWidth( int value )
{
	m_Tag.Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiStringW::Param() const
{
	return m_Tag.Param;
}

// ============================================================
void CxGdiStringW::Param( const TxGdi2dParam& value )
{
	m_Tag.Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ================================================================================
TxCharCPtrW CxGdiStringW::Text() const
{
	return m_Tag.Address;
}

// ================================================================================
void CxGdiStringW::Text(TxCharCPtrW value)
{
	Dispose();
	if (value != NULL)
	{
		int length = (int)wcslen(value);
		if (length < 0) 
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);

		wchar_t* addr = (wchar_t*)xie::Axi::MemoryAlloc((size_t)(length + 1) * sizeof(wchar_t));
		if (addr == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		if (length > 0)
			wcscpy(addr, value);

		m_Tag.Address	= addr;
		m_Tag.Length	= length;
	}
}

// ================================================================================
int CxGdiStringW::Length() const
{
	if (m_Tag.Address == NULL)
		return 0;
	return (int)wcslen(m_Tag.Address);
}

// ================================================================================
double CxGdiStringW::X() const
{
	return m_Tag.X;
}

// ================================================================================
void CxGdiStringW::X(double value)
{
	m_Tag.X = value;
}

// ================================================================================
double CxGdiStringW::Y() const
{
	return m_Tag.Y;
}

// ================================================================================
void CxGdiStringW::Y(double value)
{
	m_Tag.Y = value;
}

// ================================================================================
TxCharCPtrW CxGdiStringW::FontName() const
{
	return m_Tag.FontName;
}

// ================================================================================
void CxGdiStringW::FontName(TxCharCPtrW value)
{
	if (value == NULL)
	{
		if (m_Tag.FontName != NULL)
			xie::Axi::MemoryFree(m_Tag.FontName);
		m_Tag.FontName = NULL;
	}
	else
	{
		int length = (int)wcslen(value);
		if (length < 0) 
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);

		m_Tag.FontName = (wchar_t*)xie::Axi::MemoryAlloc((length + 1) * sizeof(wchar_t));
		if (m_Tag.FontName == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		if (length > 0)
			wcscpy(m_Tag.FontName, value);
	}
}

// ================================================================================
int CxGdiStringW::FontSize() const
{
	return m_Tag.FontSize;
}

// ================================================================================
void CxGdiStringW::FontSize(int value)
{
	m_Tag.FontSize = value;
}

// ================================================================================
ExGdiTextAlign CxGdiStringW::Align() const
{
	return m_Tag.Align;
}

// ================================================================================
void CxGdiStringW::Align(ExGdiTextAlign value)
{
	m_Tag.Align = value;
}

// ================================================================================
int CxGdiStringW::CodePage() const
{
	return m_Tag.CodePage;
}

// ================================================================================
void CxGdiStringW::CodePage(int value)
{
	m_Tag.CodePage = value;
}

// ============================================================
void CxGdiStringW::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL || value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "m_Tag.Param") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, m_Tag.Param)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (fnPRV_Gdi2d_GetParam(name, value, model, m_Tag.Param)) return;

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxGdiStringW::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL || value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "m_Tag.Param") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, m_Tag.Param)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (fnPRV_Gdi2d_SetParam(name, value, model, m_Tag.Param)) return;

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

#if defined(_MSC_VER)
// ================================================================================
HFONT CxGdiStringW::_CreateFont() const
{
	LOGFONTW	font;
	font.lfHeight			= m_Tag.FontSize;
	font.lfWidth			= 0;
	font.lfEscapement		= 0;
	font.lfOrientation		= 0;
	font.lfWeight			= this->PenWidth() * 100;
	font.lfItalic			= FALSE; 
	font.lfUnderline		= FALSE;
	font.lfStrikeOut		= FALSE;
	font.lfCharSet			= fnPRV_GDI_CodePageToCharset(m_Tag.CodePage);
	font.lfOutPrecision		= OUT_DEFAULT_PRECIS;
	font.lfClipPrecision	= CLIP_DEFAULT_PRECIS;
	font.lfQuality			= DEFAULT_QUALITY; 
	font.lfPitchAndFamily	= DEFAULT_PITCH;
	memset( font.lfFaceName, 0, sizeof(font.lfFaceName) );
	if (m_Tag.FontName != NULL)
		wcsncpy_s( font.lfFaceName, LF_FACESIZE, m_Tag.FontName, _TRUNCATE );
	return ::CreateFontIndirectW( &font );
}
#else
// ================================================================================
CxStringW CxGdiStringW::_CreateFontName() const
{
	// Font
	//    -Adobe-Courier-Bold-R-Normal--25-180-75-75-M-150-ISO859-1
	//  ~~ ~~~~~ ~~~~~~~ ~~~~ ~ ~~~~~~  ~~ ~~~ ~~ ~~ ~ ~~~ ~~~~~~ ~
	//  01 02    03      04   05 06  07 08 09  10 11 12 13 14     15
	// 
	// 01: FontNameRegistry	: 
	// 02: Foundry			: 
	// 03: Family			: 
	// 04: WeightName		: thin,extralight,light,book,medium,demibold,bold,heavy,black
	// 05: Slant			: R (Regular), I (Italic)
	// 06: SetwidthName		: Narrow, Normal, Wide
	// 07: AddStyleName		: 
	// 08: PixelSize		: font size (pixels)
	// 09: PointSize		: font size (points)
	// 10: ResolutionX		: X resolution (dpi)
	// 11: ResolutionY		: Y resolution (dpi)
	// 12: Spacing			: M (monospace), P (proportional), C (cell)
	// 13: AverageWidth		: Avarage width (points)
	// 14: CharSetRegistry	: 
	// 15: CharSetEncoding	: 
	// 

	CxStringW fontname = FontName();
	if (fontname.IsValid())
		return fontname;
	return CxStringW(L"fixed");
}
#endif

}	// GDI
}	// xie
