/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiStringA.h"
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

static const char* g_ClassName = "CxGdiStringA";

// ================================================================================
void CxGdiStringA::_Constructor()
{
	m_Tag.Param = TxGdi2dParam::Default();
	m_Tag = TxGdiStringA::Default();
}

// ================================================================================
CxGdiStringA::CxGdiStringA()
{
	_Constructor();
}

// ================================================================================
CxGdiStringA::CxGdiStringA( const CxGdiStringA& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiStringA::CxGdiStringA( const CxStringA& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiStringA::~CxGdiStringA()
{
	Dispose();
	FontName(NULL);
}

// ================================================================================
CxGdiStringA& CxGdiStringA::operator = ( const CxGdiStringA& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiStringA& CxGdiStringA::operator = ( const CxStringA& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiStringA& CxGdiStringA::operator = ( TxCharCPtrA src )
{
	this->Text( src );
	return *this;
}

// ================================================================================
bool CxGdiStringA::operator == ( const CxGdiStringA& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiStringA::operator != ( const CxGdiStringA& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxGdiStringA CxGdiStringA::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxGdiStringA::TagPtr() const
{
	return (void*)&m_Tag;
}

// ================================================================================
CxGdiStringA::operator TxCharCPtrA() const
{
	return m_Tag.Address;
}

// ================================================================================
CxGdiStringA::operator CxStringA() const
{
	CxStringA dst;
	this->CopyTo(dst);
	return dst;
}

// ================================================================================
void CxGdiStringA::CopyTo(IxModule& dst) const
{
	if (xie::Axi::ClassIs<CxStringA>(dst))
	{
		CxStringA&	_dst = static_cast<CxStringA&>(dst);

		if (this->IsValid() == false)
			_dst.Dispose();
		else
			_dst = m_Tag.Address;
		return;
	}
	if (xie::Axi::ClassIs<CxStringW>(dst))
	{
		CxStringW&	_dst = static_cast<CxStringW&>(dst);

		if (this->IsValid() == false)
			_dst.Dispose();
		else
			_dst.CopyFrom( CxStringA(m_Tag.Address) );
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
CxGdiStringA CxGdiStringA::Clone() const
{
	CxGdiStringA clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiStringA::Dispose()
{
	if (m_Tag.Address != NULL)
		xie::Axi::MemoryFree(m_Tag.Address);
	m_Tag.Address	= NULL;
	m_Tag.Length	= 0;
}

// ================================================================================
void CxGdiStringA::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiStringA>(src))
	{
		auto&	_src = static_cast<const CxGdiStringA&>(src);
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
		CxGdiStringA&		_dst = *this;
		_dst.Text(_src.Address());
		return;
	}
	if (xie::Axi::ClassIs<CxStringW>(src))
	{
		const CxStringW&	_src = static_cast<const CxStringW&>(src);
		CxStringA			_tmp;
		_tmp.CopyFrom(_src);

		CxGdiStringA&		_dst = *this;
		_dst.Text(_tmp.Address());
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
bool CxGdiStringA::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiStringA>(src))
	{
		auto&	_src = static_cast<const CxGdiStringA&>(src);
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
			if (strcmp(_dst.m_Tag.FontName, _src.m_Tag.FontName) != 0) return false;
		}

		// 文字列の比較.
		if (_dst.m_Tag.Address == NULL && _src.m_Tag.Address != NULL) return false;
		if (_dst.m_Tag.Address != NULL && _src.m_Tag.Address == NULL) return false;
		if (_dst.m_Tag.Address != NULL && _src.m_Tag.Address != NULL)
		{
			if (strcmp(_dst.m_Tag.Address, _src.m_Tag.Address) != 0) return false;
		}

		return true;
	}
	return false;
}

// ============================================================
bool CxGdiStringA::IsValid() const
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
void CxGdiStringA::Render(HxModule hcanvas, ExGdiScalingMode mode) const
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
					BOOL status = ::TextOutA( hdcDst, dst_x, dst_y, m_Tag.Address, m_Tag.Length );

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
			CxStringA	font_name = _CreateFontName();
			fnXIE_Core_TraceOut(2, "font_name=%s\n", (font_name.IsValid() ? font_name.Address() : "(NULL)"));

			XFontStruct*	font_info = ::XLoadQueryFont(xserver, font_name.Address());
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
TxPointD CxGdiStringA::Location() const
{
	return TxPointD(m_Tag.X, m_Tag.Y);
}

// ================================================================================
void CxGdiStringA::Location(TxPointD value)
{
	m_Tag.X = value.X;
	m_Tag.Y = value.Y;
}

// ================================================================================
TxRectangleD CxGdiStringA::Bounds() const
{
	TxRectangleD bounds;
	bounds.X = m_Tag.X;
	bounds.Y = m_Tag.Y;
	bounds.Width = 0;
	bounds.Height = 0;

	if (m_Tag.Address != NULL)
	{
		TxCharCPtrA	text = m_Tag.Address;
		int		length = (int)strlen(text);

		TxSizeI size(0, 0);

		#if defined(_MSC_VER)
		{
			HDC		hdc		= fnPRV_GDI_CreateDC();
			HFONT	hfont	= this->_CreateFont();
			if (hdc != NULL && hfont != NULL)
			{
				::SelectObject(hdc, hfont);

				SIZE sz = { 0, 0 };
				::GetTextExtentPoint32A( hdc, text, length, &sz );
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
					CxString	font_name = _CreateFontName();
					int			missing_count = 0;
					char**		missing_list = NULL;
					char*		def_string = NULL;
					XFontSet font_set = XCreateFontSet(xserver, font_name.Address(), &missing_list, &missing_count, &def_string);
					CxFinalizer font_finalizer([xserver,font_set,missing_list]()
						{
							if (font_set != NULL)
								XFreeFontSet(xserver,font_set);
							if (missing_list != NULL)
								XFreeStringList(missing_list);
						});

					if (font_set != NULL)
					{
						XRectangle ink;
						XRectangle log;
						XmbTextExtents(font_set, Text(), Length(), &ink, &log);

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
double CxGdiStringA::Angle() const
{
	return m_Tag.Param.Angle;
}

// ============================================================
void CxGdiStringA::Angle( double degree )
{
	m_Tag.Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiStringA::Axis() const
{
	return m_Tag.Param.Axis;
}

// ============================================================
void CxGdiStringA::Axis( TxPointD value )
{
	m_Tag.Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiStringA::HitTest( TxPointD position, double margin ) const
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
void CxGdiStringA::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiStringA>(prev_figure))
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
TxRGB8x4 CxGdiStringA::BkColor() const
{
	return m_Tag.Param.BkColor;
}

// ============================================================
void CxGdiStringA::BkColor( TxRGB8x4 value )
{
	m_Tag.Param.BkColor = value;
}

// ============================================================
bool CxGdiStringA::BkEnable() const
{
	return (m_Tag.Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiStringA::BkEnable( bool value )
{
	m_Tag.Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiStringA::PenColor() const
{
	return m_Tag.Param.PenColor;
}

// ============================================================
void CxGdiStringA::PenColor( TxRGB8x4 value )
{
	m_Tag.Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiStringA::PenStyle() const
{
	return m_Tag.Param.PenStyle;
}

// ============================================================
void CxGdiStringA::PenStyle( ExGdiPenStyle value )
{
	m_Tag.Param.PenStyle = value;
}

// ============================================================
int CxGdiStringA::PenWidth() const
{
	return m_Tag.Param.PenWidth;
}

// ============================================================
void CxGdiStringA::PenWidth( int value )
{
	m_Tag.Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiStringA::Param() const
{
	return m_Tag.Param;
}

// ============================================================
void CxGdiStringA::Param( const TxGdi2dParam& value )
{
	m_Tag.Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ================================================================================
TxCharCPtrA CxGdiStringA::Text() const
{
	return m_Tag.Address;
}

// ================================================================================
void CxGdiStringA::Text(TxCharCPtrA value)
{
	Dispose();
	if (value != NULL)
	{
		int length = (int)strlen(value);
		if (length < 0) 
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);

		char* addr = (char*)xie::Axi::MemoryAlloc((size_t)(length + 1) * sizeof(char));
		if (addr == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		if (length > 0)
			strcpy(addr, value);

		m_Tag.Address	= addr;
		m_Tag.Length	= length;
	}
}

// ================================================================================
int CxGdiStringA::Length() const
{
	if (m_Tag.Address == NULL)
		return 0;
	return (int)strlen(m_Tag.Address);
}

// ================================================================================
double CxGdiStringA::X() const
{
	return m_Tag.X;
}

// ================================================================================
void CxGdiStringA::X(double value)
{
	m_Tag.X = value;
}

// ================================================================================
double CxGdiStringA::Y() const
{
	return m_Tag.Y;
}

// ================================================================================
void CxGdiStringA::Y(double value)
{
	m_Tag.Y = value;
}

// ================================================================================
TxCharCPtrA CxGdiStringA::FontName() const
{
	return m_Tag.FontName;
}

// ================================================================================
void CxGdiStringA::FontName(TxCharCPtrA value)
{
	if (value == NULL)
	{
		if (m_Tag.FontName != NULL)
			xie::Axi::MemoryFree(m_Tag.FontName);
		m_Tag.FontName = NULL;
	}
	else
	{
		int length = (int)strlen(value);
		if (length < 0) 
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);

		m_Tag.FontName = (char*)xie::Axi::MemoryAlloc((length + 1) * sizeof(char));
		if (m_Tag.FontName == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		if (length > 0)
			strcpy(m_Tag.FontName, value);
	}
}

// ================================================================================
int CxGdiStringA::FontSize() const
{
	return m_Tag.FontSize;
}

// ================================================================================
void CxGdiStringA::FontSize(int value)
{
	m_Tag.FontSize = value;
}

// ================================================================================
ExGdiTextAlign CxGdiStringA::Align() const
{
	return m_Tag.Align;
}

// ================================================================================
void CxGdiStringA::Align(ExGdiTextAlign value)
{
	m_Tag.Align = value;
}

// ================================================================================
int CxGdiStringA::CodePage() const
{
	return m_Tag.CodePage;
}

// ================================================================================
void CxGdiStringA::CodePage(int value)
{
	m_Tag.CodePage = value;
}

// ============================================================
void CxGdiStringA::GetParam(TxCharCPtrA name, void* value, TxModel model) const
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
void CxGdiStringA::SetParam(TxCharCPtrA name, const void* value, TxModel model)
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
HFONT CxGdiStringA::_CreateFont() const
{
	LOGFONTA	font;
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
		strncpy_s( font.lfFaceName, LF_FACESIZE, m_Tag.FontName, _TRUNCATE );
	return ::CreateFontIndirectA( &font );
}
#else
// ================================================================================
CxStringA CxGdiStringA::_CreateFontName() const
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

	CxStringA fontname = FontName();
	if (fontname.IsValid())
		return fontname;
	return CxStringA("fixed");
}
#endif

}	// GDI
}	// xie
