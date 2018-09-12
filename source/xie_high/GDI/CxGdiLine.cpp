/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiLine.h"
#include "GDI/CxCanvas.h"
#include "Core/CxException.h"
#include "Core/Axi.h"
#include "Core/CxArrayEx.h"
#include "Core/IxParam.h"

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

static const char* g_ClassName = "CxGdiLine";

// ================================================================================
void CxGdiLine::_Constructor()
{
	m_Param = TxGdi2dParam::Default();
	Shape = TxLineD::Default();
}

// ================================================================================
CxGdiLine::CxGdiLine()
{
	_Constructor();
}

// ================================================================================
CxGdiLine::CxGdiLine( CxGdiLine&& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiLine::CxGdiLine( const CxGdiLine& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiLine::CxGdiLine( const TxLineD& shape )
{
	_Constructor();
	Shape = shape;
}

// ================================================================================
CxGdiLine::CxGdiLine( const TxLineI& shape )
{
	_Constructor();
	Shape = shape;
}

// ============================================================
CxGdiLine::CxGdiLine(double a, double b, double c)
{
	_Constructor();
	Shape.A = a;
	Shape.B = b;
	Shape.C = c;
}

// ================================================================================
CxGdiLine::~CxGdiLine()
{
}

// ============================================================
CxGdiLine& CxGdiLine::operator = ( CxGdiLine&& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiLine& CxGdiLine::operator = ( const CxGdiLine& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxGdiLine::operator == ( const CxGdiLine& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiLine::operator != ( const CxGdiLine& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxLineD CxGdiLine::Tag() const
{
	return Shape;
}

// ============================================================
void* CxGdiLine::TagPtr() const
{
	return (void*)&Shape;
}

// ============================================================
CxGdiLine& CxGdiLine::operator = ( const TxLineD& src )
{
	Shape = src;
	return *this;
}

// ============================================================
CxGdiLine& CxGdiLine::operator = ( const TxLineI& src )
{
	Shape = (TxLineD)src;
	return *this;
}

// ================================================================================
CxGdiLine::operator TxLineD() const
{
	return Shape;
}

// ================================================================================
CxGdiLine::operator TxLineI() const
{
	return (TxLineI)Shape;
}

// ================================================================================
CxGdiLine CxGdiLine::Clone() const
{
	CxGdiLine clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiLine::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiLine>(src))
	{
		auto&	_src = static_cast<const CxGdiLine&>(src);
		auto&	_dst = *this;

		_dst.Shape = _src.Shape;
		_dst.m_Param = _src.m_Param;

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
bool CxGdiLine::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiLine>(src))
	{
		auto&	_src = static_cast<const CxGdiLine&>(src);
		auto&	_dst = *this;

		if (_dst.Shape	!= _src.Shape) return false;
		if (_dst.m_Param != _src.m_Param) return false;

		return true;
	}
	return false;
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
void CxGdiLine::Render(HxModule hcanvas, ExGdiScalingMode mode) const
{
	// 表示範囲パラメータ取得.
	TxCanvas	canvas;
	IxParam*	dpi = xie::Axi::SafeCast<IxParam>(hcanvas);
	if (dpi == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	dpi->GetParam("Tag", &canvas, TxModel::Default());

	::glPushMatrix();
	::glEnable( GL_BLEND );
	::glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	// ----------------------------------------------------------------------
	// affine (Rotate)
	if (this->Angle() != 0.0)
	{
		auto angle = this->Angle();	// degree
		auto axis = this->Axis() + this->Location();
		::glTranslated	(+axis.X, +axis.Y, 0.0);
		::glRotated		(angle, 0, 0, 1);
		::glTranslated	(-axis.X, -axis.Y, 0.0);
	}

	auto shape = (TxLineD)*this;

	TxPointD st(0, 0);
	TxPointD ed(canvas.Width, canvas.Height);
	if (mode != ExGdiScalingMode::None)
	{
		double	mag = canvas.Magnification;
		TxRectangleI display_rect(0, 0, canvas.Width, canvas.Height);
		st = CxCanvas::DPtoIP(display_rect, canvas.BgSize, mag, canvas.ViewPoint, st, mode);
		ed = CxCanvas::DPtoIP(display_rect, canvas.BgSize, mag, canvas.ViewPoint, ed, mode);
	}

	// 枠線.
	if (this->PenStyle() != ExGdiPenStyle::None)
	{
		fnPRV_GDI_PenPrepare(&m_Param);

		auto ls = shape.ToLineSegment(st.X, st.Y, ed.X, ed.Y);

		::glBegin(GL_LINES);
		::glVertex2d( ls.X1, ls.Y1 );
		::glVertex2d( ls.X2, ls.Y2 );
		::glEnd();

		fnPRV_GDI_PenRestore(&m_Param);
	}

	::glDisable( GL_BLEND );
	::glPopMatrix();
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
TxPointD CxGdiLine::Location() const
{
	return TxPointD(0, 0);
}

// ================================================================================
void CxGdiLine::Location(TxPointD value)
{
	return;
}

// ================================================================================
TxRectangleD CxGdiLine::Bounds() const
{
	return TxRectangleD(0, 0, 0, 0);
}

// ============================================================
double CxGdiLine::Angle() const
{
	return m_Param.Angle;
}

// ============================================================
void CxGdiLine::Angle( double degree )
{
	m_Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiLine::Axis() const
{
	return m_Param.Axis;
}

// ============================================================
void CxGdiLine::Axis( TxPointD value )
{
	m_Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiLine::HitTest( TxPointD position, double margin ) const
{
	auto p0 = xie::Axi::Rotate<TxPointD>(position, this->Location()+this->Axis(), -this->Angle());
	auto shape = (TxLineD)*this;

	if (fnPRV_GDI_HitTest_Line(p0, margin, shape) != 0)
		return TxHitPosition(1, 0, 0);

	return TxHitPosition::Default();
}

// ================================================================================
void CxGdiLine::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiLine>(prev_figure))
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
TxRGB8x4 CxGdiLine::BkColor() const
{
	return m_Param.BkColor;
}

// ============================================================
void CxGdiLine::BkColor( TxRGB8x4 value )
{
	m_Param.BkColor = value;
}

// ============================================================
bool CxGdiLine::BkEnable() const
{
	return (m_Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiLine::BkEnable( bool value )
{
	m_Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiLine::PenColor() const
{
	return m_Param.PenColor;
}

// ============================================================
void CxGdiLine::PenColor( TxRGB8x4 value )
{
	m_Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiLine::PenStyle() const
{
	return m_Param.PenStyle;
}

// ============================================================
void CxGdiLine::PenStyle( ExGdiPenStyle value )
{
	m_Param.PenStyle = value;
}

// ============================================================
int CxGdiLine::PenWidth() const
{
	return m_Param.PenWidth;
}

// ============================================================
void CxGdiLine::PenWidth( int value )
{
	m_Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiLine::Param() const
{
	return m_Param;
}

// ============================================================
void CxGdiLine::Param( const TxGdi2dParam& value )
{
	m_Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// METHOD
// 

// ============================================================
double CxGdiLine::A() const
{
	return Shape.A;
}

// ============================================================
void CxGdiLine::A(double value)
{
	Shape.A = value;
}

// ============================================================
double CxGdiLine::B() const
{
	return Shape.B;
}

// ============================================================
void CxGdiLine::B(double value)
{
	Shape.B = value;
}

// ============================================================
double CxGdiLine::C() const
{
	return Shape.C;
}

// ============================================================
void CxGdiLine::C(double value)
{
	Shape.C = value;
}

// ============================================================
void CxGdiLine::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL || value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "m_Param") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, m_Param)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (fnPRV_Gdi2d_GetParam(name, value, model, m_Param)) return;

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxGdiLine::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL || value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "m_Param") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, m_Param)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (fnPRV_Gdi2d_SetParam(name, value, model, m_Param)) return;

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

}	// GDI
}	// xie
