/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiLineSegment.h"
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

static const char* g_ClassName = "CxGdiLineSegment";

// ================================================================================
void CxGdiLineSegment::_Constructor()
{
	m_Param = TxGdi2dParam::Default();
	Shape = TxLineSegmentD::Default();
}

// ================================================================================
CxGdiLineSegment::CxGdiLineSegment()
{
	_Constructor();
}

// ================================================================================
CxGdiLineSegment::CxGdiLineSegment( CxGdiLineSegment&& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiLineSegment::CxGdiLineSegment( const CxGdiLineSegment& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiLineSegment::CxGdiLineSegment( const TxLineSegmentD& shape )
{
	_Constructor();
	Shape = shape;
}

// ================================================================================
CxGdiLineSegment::CxGdiLineSegment( const TxLineSegmentI& shape )
{
	_Constructor();
	Shape = shape;
}

// ============================================================
CxGdiLineSegment::CxGdiLineSegment(double x1, double y1, double x2, double y2)
{
	_Constructor();
	Shape.X1 = x1;
	Shape.Y1 = y1;
	Shape.X2 = x2;
	Shape.Y2 = y2;
}

// ============================================================
CxGdiLineSegment::CxGdiLineSegment(TxPointD st, TxPointD ed)
{
	_Constructor();
	Shape.X1 = st.X;
	Shape.Y1 = st.Y;
	Shape.X2 = ed.X;
	Shape.Y2 = ed.Y;
}

// ================================================================================
CxGdiLineSegment::~CxGdiLineSegment()
{
}

// ============================================================
CxGdiLineSegment& CxGdiLineSegment::operator = ( CxGdiLineSegment&& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiLineSegment& CxGdiLineSegment::operator = ( const CxGdiLineSegment& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxGdiLineSegment::operator == ( const CxGdiLineSegment& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiLineSegment::operator != ( const CxGdiLineSegment& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxLineSegmentD CxGdiLineSegment::Tag() const
{
	return Shape;
}

// ============================================================
void* CxGdiLineSegment::TagPtr() const
{
	return (void*)&Shape;
}

// ============================================================
CxGdiLineSegment& CxGdiLineSegment::operator = ( const TxLineSegmentD& src )
{
	Shape = src;
	return *this;
}

// ============================================================
CxGdiLineSegment& CxGdiLineSegment::operator = ( const TxLineSegmentI& src )
{
	Shape = (TxLineSegmentD)src;
	return *this;
}

// ================================================================================
CxGdiLineSegment::operator TxLineSegmentD() const
{
	return Shape;
}

// ================================================================================
CxGdiLineSegment::operator TxLineSegmentI() const
{
	return (TxLineSegmentI)Shape;
}

// ================================================================================
CxGdiLineSegment CxGdiLineSegment::Clone() const
{
	CxGdiLineSegment clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiLineSegment::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiLineSegment>(src))
	{
		auto&	_src = static_cast<const CxGdiLineSegment&>(src);
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
bool CxGdiLineSegment::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiLineSegment>(src))
	{
		auto&	_src = static_cast<const CxGdiLineSegment&>(src);
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
void CxGdiLineSegment::Render(HxModule hcanvas, ExGdiScalingMode mode) const
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
		double		angle = this->Angle();	// degree
		TxPointD	axis = this->Axis() + this->Location();
		::glTranslated	(+axis.X, +axis.Y, 0.0);
		::glRotated		(angle, 0, 0, 1);
		::glTranslated	(-axis.X, -axis.Y, 0.0);
	}

	// 枠線.
	if (this->PenStyle() != ExGdiPenStyle::None)
	{
		fnPRV_GDI_PenPrepare(&m_Param);

		::glBegin(GL_LINES);
		::glVertex2d( Shape.X1, Shape.Y1 );
		::glVertex2d( Shape.X2, Shape.Y2 );
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
TxPointD CxGdiLineSegment::Location() const
{
	return this->Point1();
}

// ================================================================================
void CxGdiLineSegment::Location(TxPointD value)
{
	TxPointD	diff = value - this->Point1();
	Shape.X1 += diff.X;
	Shape.X2 += diff.X;

	Shape.Y1 += diff.Y;
	Shape.Y2 += diff.Y;
}

// ================================================================================
TxRectangleD CxGdiLineSegment::Bounds() const
{
	TxStatistics stat_x;
	TxStatistics stat_y;

	stat_x += Shape.X1;
	stat_x += Shape.X2;

	stat_y += Shape.Y1;
	stat_y += Shape.Y2;

	auto sx = stat_x.Min;
	auto sy = stat_y.Min;
	auto ex = stat_x.Max;
	auto ey = stat_y.Max;

	return TxRectangleD(sx, sy, (ex - sx), (ey - sy));
}

// ============================================================
double CxGdiLineSegment::Angle() const
{
	return m_Param.Angle;
}

// ============================================================
void CxGdiLineSegment::Angle( double degree )
{
	m_Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiLineSegment::Axis() const
{
	return m_Param.Axis;
}

// ============================================================
void CxGdiLineSegment::Axis( TxPointD value )
{
	m_Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiLineSegment::HitTest( TxPointD position, double margin ) const
{
	auto p0 = xie::Axi::Rotate<TxPointD>(position, this->Location()+this->Axis(), -this->Angle());
	auto shape = (TxLineSegmentD)*this;

	auto hit = fnPRV_GDI_HitTest_LineSegment(p0, margin, shape);
	return hit;
}

// ================================================================================
void CxGdiLineSegment::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiLineSegment>(prev_figure))
	{
		auto hit = figure->HitTest(prev_position, margin);

		switch (hit.Mode)
		{
			case 1:
				{
					auto mv = move_position - prev_position;
					this->Location( figure->Location() + mv );
				}
				break;
			case 2:
				{
					auto _prev_position = xie::Axi::Rotate<TxPointD>(prev_position, figure->Location() + figure->Axis(), -figure->Angle());
					auto _move_position = xie::Axi::Rotate<TxPointD>(move_position, figure->Location() + figure->Axis(), -figure->Angle());
					auto mv = _move_position - _prev_position;

					auto _this_location = this->Location();
					switch (hit.Site)
					{
						case +1: this->Point1( figure->Point1() + mv ); break;
						case +2: this->Point2( figure->Point2() + mv ); break;
					}
					if (figure->Angle() != 0)
					{
						if (_this_location.X != this->Location().X)
							this->m_Param.Axis.X = figure->m_Param.Axis.X - mv.X;
						if (_this_location.Y != this->Location().Y)
							this->m_Param.Axis.Y = figure->m_Param.Axis.Y - mv.Y;
					}
				}
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
TxRGB8x4 CxGdiLineSegment::BkColor() const
{
	return m_Param.BkColor;
}

// ============================================================
void CxGdiLineSegment::BkColor( TxRGB8x4 value )
{
	m_Param.BkColor = value;
}

// ============================================================
bool CxGdiLineSegment::BkEnable() const
{
	return (m_Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiLineSegment::BkEnable( bool value )
{
	m_Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiLineSegment::PenColor() const
{
	return m_Param.PenColor;
}

// ============================================================
void CxGdiLineSegment::PenColor( TxRGB8x4 value )
{
	m_Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiLineSegment::PenStyle() const
{
	return m_Param.PenStyle;
}

// ============================================================
void CxGdiLineSegment::PenStyle( ExGdiPenStyle value )
{
	m_Param.PenStyle = value;
}

// ============================================================
int CxGdiLineSegment::PenWidth() const
{
	return m_Param.PenWidth;
}

// ============================================================
void CxGdiLineSegment::PenWidth( int value )
{
	m_Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiLineSegment::Param() const
{
	return m_Param;
}

// ============================================================
void CxGdiLineSegment::Param( const TxGdi2dParam& value )
{
	m_Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
double CxGdiLineSegment::X1() const
{
	return Shape.X1;
}

// ============================================================
void CxGdiLineSegment::X1(double value)
{
	Shape.X1 = value;
}

// ============================================================
double CxGdiLineSegment::Y1() const
{
	return Shape.Y1;
}

// ============================================================
void CxGdiLineSegment::Y1(double value)
{
	Shape.Y1 = value;
}

// ============================================================
double CxGdiLineSegment::X2() const
{
	return Shape.X2;
}

// ============================================================
void CxGdiLineSegment::X2(double value)
{
	Shape.X2 = value;
}

// ============================================================
double CxGdiLineSegment::Y2() const
{
	return Shape.Y2;
}

// ============================================================
void CxGdiLineSegment::Y2(double value)
{
	Shape.Y2 = value;
}

// ============================================================
TxPointD CxGdiLineSegment::Point1() const
{
	return TxPointD(Shape.X1, Shape.Y1);
}

// ============================================================
void CxGdiLineSegment::Point1(const TxPointD& value)
{
	Shape.X1 = value.X;
	Shape.Y1 = value.Y;
}

// ============================================================
TxPointD CxGdiLineSegment::Point2() const
{
	return TxPointD(Shape.X2, Shape.Y2);
}

// ============================================================
void CxGdiLineSegment::Point2(const TxPointD& value)
{
	Shape.X2 = value.X;
	Shape.Y2 = value.Y;
}

// ============================================================
void CxGdiLineSegment::GetParam(TxCharCPtrA name, void* value, TxModel model) const
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
void CxGdiLineSegment::SetParam(TxCharCPtrA name, const void* value, TxModel model)
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
