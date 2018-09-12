/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiPoint.h"
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

static const char* g_ClassName = "CxGdiPoint";

// ================================================================================
void CxGdiPoint::_Constructor()
{
	m_Param = TxGdi2dParam::Default();
	Shape = TxPointD::Default();
	m_AnchorSize = TxSizeD(0.5, 0.5);
	m_AnchorStyle = ExGdiAnchorStyle::Cross;
}

// ================================================================================
CxGdiPoint::CxGdiPoint()
{
	_Constructor();
}

// ================================================================================
CxGdiPoint::CxGdiPoint( CxGdiPoint&& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiPoint::CxGdiPoint( const CxGdiPoint& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiPoint::CxGdiPoint( const TxPointD& shape )
{
	_Constructor();
	Shape = shape;
}

// ================================================================================
CxGdiPoint::CxGdiPoint( const TxPointI& shape )
{
	_Constructor();
	Shape = shape;
}

// ============================================================
CxGdiPoint::CxGdiPoint(double x, double y)
{
	_Constructor();
	Shape.X = x;
	Shape.Y = y;
}

// ================================================================================
CxGdiPoint::~CxGdiPoint()
{
}

// ============================================================
CxGdiPoint& CxGdiPoint::operator = ( CxGdiPoint&& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiPoint& CxGdiPoint::operator = ( const CxGdiPoint& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxGdiPoint::operator == ( const CxGdiPoint& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiPoint::operator != ( const CxGdiPoint& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxPointD CxGdiPoint::Tag() const
{
	return Shape;
}

// ============================================================
void* CxGdiPoint::TagPtr() const
{
	return (void*)&Shape;
}

// ================================================================================
CxGdiPoint& CxGdiPoint::operator = ( const TxPointD& src )
{
	Shape = src;
	return *this;
}

// ================================================================================
CxGdiPoint& CxGdiPoint::operator = ( const TxPointI& src )
{
	Shape = (TxPointD)src;
	return *this;
}

// ================================================================================
CxGdiPoint::operator TxPointD() const
{
	return Shape;
}

// ================================================================================
CxGdiPoint::operator TxPointI() const
{
	return (TxPointI)Shape;
}

// ================================================================================
CxGdiPoint CxGdiPoint::Clone() const
{
	CxGdiPoint clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiPoint::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiPoint>(src))
	{
		auto&	_src = static_cast<const CxGdiPoint&>(src);
		auto&	_dst = *this;

		_dst.Shape = _src.Shape;
		_dst.m_Param = _src.m_Param;
		_dst.m_AnchorStyle = _src.m_AnchorStyle;
		_dst.m_AnchorSize = _src.m_AnchorSize;

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
bool CxGdiPoint::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiPoint>(src))
	{
		auto&	_src = static_cast<const CxGdiPoint&>(src);
		auto&	_dst = *this;

		if (_dst.Shape	!= _src.Shape) return false;
		if (_dst.m_Param != _src.m_Param) return false;
		if (_dst.m_AnchorStyle != _src.m_AnchorStyle) return false;
		if (_dst.m_AnchorSize != _src.m_AnchorSize) return false;

		return true;
	}
	return false;
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
void CxGdiPoint::Render(HxModule hcanvas, ExGdiScalingMode mode) const
{
	// ï\é¶îÕàÕÉpÉâÉÅÅ[É^éÊìæ.
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

	switch(this->AnchorStyle())
	{
	default:
	case ExGdiAnchorStyle::None:
		// ògê¸.
		if (this->PenStyle() != ExGdiPenStyle::None)
		{
			double w = 0.5;
			double h = 0.5;
			auto p0 = TxPointD(Shape.X - w, Shape.Y - h);
			auto p1 = TxPointD(Shape.X + w, Shape.Y + h);

			fnPRV_GDI_PenPrepare(&m_Param);
			{
				::glBegin(GL_LINES);
				::glVertex2d( p0.X, p0.Y );
				::glVertex2d( p1.X, p1.Y );
				::glEnd();
			}
			fnPRV_GDI_PenRestore(&m_Param);
		}
		break;
	case ExGdiAnchorStyle::Cross:
		{
			auto w = this->AnchorSize().Width;
			auto h = this->AnchorSize().Height;
			auto p0 = TxPointD(Shape.X - 0, Shape.Y - h);
			auto p1 = TxPointD(Shape.X + 0, Shape.Y + h);
			auto p2 = TxPointD(Shape.X - w, Shape.Y - 0);
			auto p3 = TxPointD(Shape.X + w, Shape.Y + 0);

			// ògê¸.
			if (this->PenStyle() != ExGdiPenStyle::None)
			{
				fnPRV_GDI_PenPrepare(&m_Param);

				::glBegin(GL_LINES);
				::glVertex2d( p0.X, p0.Y );
				::glVertex2d( p1.X, p1.Y );
				::glEnd();

				::glBegin(GL_LINES);
				::glVertex2d( p2.X, p3.Y );
				::glVertex2d( p3.X, p3.Y );
				::glEnd();

				fnPRV_GDI_PenRestore(&m_Param);
			}
		}
		break;
	case ExGdiAnchorStyle::Diamond:
		{
			auto w = this->AnchorSize().Width;
			auto h = this->AnchorSize().Height;
			auto p0 = TxPointD(Shape.X - 0, Shape.Y - h);
			auto p1 = TxPointD(Shape.X + w, Shape.Y + 0);
			auto p2 = TxPointD(Shape.X + 0, Shape.Y + h);
			auto p3 = TxPointD(Shape.X - w, Shape.Y - 0);

			// îwåi.
			if (this->BkEnable() == true)
			{
				fnPRV_GDI_BkPrepare(&m_Param);

				::glBegin(GL_TRIANGLE_FAN);
				::glVertex2d( p0.X, p0.Y );
				::glVertex2d( p1.X, p1.Y );
				::glVertex2d( p2.X, p2.Y );
				::glVertex2d( p3.X, p3.Y );
				::glEnd();

				fnPRV_GDI_BkRestore(&m_Param);
			}

			// ògê¸.
			if (this->PenStyle() != ExGdiPenStyle::None)
			{
				fnPRV_GDI_PenPrepare(&m_Param);

				::glBegin(GL_LINE_LOOP);
				::glVertex2d( p0.X, p0.Y );
				::glVertex2d( p1.X, p1.Y );
				::glVertex2d( p2.X, p2.Y );
				::glVertex2d( p3.X, p3.Y );
				::glEnd();

				fnPRV_GDI_PenRestore(&m_Param);
			}
		}
		break;
	case ExGdiAnchorStyle::Arrow:
		// ògê¸.
		if (this->PenStyle() != ExGdiPenStyle::None)
		{
			auto w = this->AnchorSize().Width;
			auto h = this->AnchorSize().Height;
			auto p0 = TxPointD(Shape.X - w, Shape.Y - h);
			auto p1 = TxPointD(Shape.X - 0, Shape.Y - 0);
			auto p2 = TxPointD(Shape.X - w, Shape.Y + h);

			fnPRV_GDI_PenPrepare(&m_Param);
			{
				::glBegin(GL_LINE_STRIP);
				::glVertex2d( p0.X, p0.Y );
				::glVertex2d( p1.X, p1.Y );
				::glVertex2d( p2.X, p2.Y );
				::glEnd();
			}
			fnPRV_GDI_PenRestore(&m_Param);
		}
		break;
	case ExGdiAnchorStyle::Triangle:
		{
			auto w = this->AnchorSize().Width;
			auto h = this->AnchorSize().Height;
			auto p0 = TxPointD(Shape.X - w, Shape.Y - h);
			auto p1 = TxPointD(Shape.X - 0, Shape.Y - 0);
			auto p2 = TxPointD(Shape.X - w, Shape.Y + h);

			// îwåi.
			if (this->BkEnable() == true)
			{
				fnPRV_GDI_BkPrepare(&m_Param);

				::glBegin(GL_TRIANGLE_FAN);
				::glVertex2d( p0.X, p0.Y );
				::glVertex2d( p1.X, p1.Y );
				::glVertex2d( p2.X, p2.Y );
				::glEnd();

				fnPRV_GDI_BkRestore(&m_Param);
			}

			// ògê¸.
			if (this->PenStyle() != ExGdiPenStyle::None)
			{
				fnPRV_GDI_PenPrepare(&m_Param);

				::glBegin(GL_LINE_LOOP);
				::glVertex2d( p0.X, p0.Y );
				::glVertex2d( p1.X, p1.Y );
				::glVertex2d( p2.X, p2.Y );
				::glEnd();

				fnPRV_GDI_PenRestore(&m_Param);
			}
		}
		break;
	case ExGdiAnchorStyle::Diagcross:
		// ògê¸.
		if (this->PenStyle() != ExGdiPenStyle::None)
		{
			auto w = this->AnchorSize().Width;
			auto h = this->AnchorSize().Height;
			auto p0 = TxPointD(Shape.X - w, Shape.Y - h);
			auto p1 = TxPointD(Shape.X + w, Shape.Y + h);
			auto p2 = TxPointD(Shape.X + w, Shape.Y - h);
			auto p3 = TxPointD(Shape.X - w, Shape.Y + h);

			fnPRV_GDI_PenPrepare(&m_Param);
			{
				::glBegin(GL_LINE_STRIP);
				::glVertex2d( p0.X, p0.Y );
				::glVertex2d( p1.X, p1.Y );
				::glEnd();

				::glBegin(GL_LINE_STRIP);
				::glVertex2d( p2.X, p2.Y );
				::glVertex2d( p3.X, p3.Y );
				::glEnd();
			}
			fnPRV_GDI_PenRestore(&m_Param);
		}
		break;
	case ExGdiAnchorStyle::Rectangle:
		{
			auto w = this->AnchorSize().Width;
			auto h = this->AnchorSize().Height;
			auto p0 = TxPointD(Shape.X - w, Shape.Y - h);
			auto p1 = TxPointD(Shape.X + w, Shape.Y - h);
			auto p2 = TxPointD(Shape.X + w, Shape.Y + h);
			auto p3 = TxPointD(Shape.X - w, Shape.Y + h);

			// îwåi.
			if (this->BkEnable() == true)
			{
				fnPRV_GDI_BkPrepare(&m_Param);

				::glBegin(GL_TRIANGLE_FAN);
				::glVertex2d( p0.X, p0.Y );
				::glVertex2d( p1.X, p1.Y );
				::glVertex2d( p2.X, p2.Y );
				::glVertex2d( p3.X, p3.Y );
				::glEnd();

				fnPRV_GDI_BkRestore(&m_Param);
			}

			// ògê¸.
			if (this->PenStyle() != ExGdiPenStyle::None)
			{
				fnPRV_GDI_PenPrepare(&m_Param);

				::glBegin(GL_LINE_LOOP);
				::glVertex2d( p0.X, p0.Y );
				::glVertex2d( p1.X, p1.Y );
				::glVertex2d( p2.X, p2.Y );
				::glVertex2d( p3.X, p3.Y );
				::glEnd();

				fnPRV_GDI_PenRestore(&m_Param);
			}
		}
		break;
	}

	::glDisable( GL_BLEND );
	::glPopMatrix();
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
TxPointD CxGdiPoint::Location() const
{
	return TxPointD(Shape.X, Shape.Y);
}

// ================================================================================
void CxGdiPoint::Location(TxPointD value)
{
	Shape.X = value.X;
	Shape.Y = value.Y;
}

// ================================================================================
TxRectangleD CxGdiPoint::Bounds() const
{
	double	sx = Shape.X - this->AnchorSize().Width;
	double	sy = Shape.Y - this->AnchorSize().Height;
	double	ex = Shape.X + this->AnchorSize().Width;
	double	ey = Shape.Y + this->AnchorSize().Height;

	TxRectangleD rect(sx, sy, (ex - sx), (ey - sy));

	auto trapezoid = rect.ToTrapezoid();
	TxStatistics stat_x;
	TxStatistics stat_y;

	stat_x += trapezoid.X1;
	stat_x += trapezoid.X2;
	stat_x += trapezoid.X3;
	stat_x += trapezoid.X4;

	stat_y += trapezoid.Y1;
	stat_y += trapezoid.Y2;
	stat_y += trapezoid.Y3;
	stat_y += trapezoid.Y4;

	sx = stat_x.Min;
	sy = stat_y.Min;
	ex = stat_x.Max;
	ey = stat_y.Max;

	return TxRectangleD(sx, sy, (ex - sx), (ey - sy));
}

// ============================================================
double CxGdiPoint::Angle() const
{
	return m_Param.Angle;
}

// ============================================================
void CxGdiPoint::Angle( double degree )
{
	m_Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiPoint::Axis() const
{
	return m_Param.Axis;
}

// ============================================================
void CxGdiPoint::Axis( TxPointD value )
{
	m_Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiPoint::HitTest( TxPointD position, double margin ) const
{
	auto p0 = xie::Axi::Rotate<TxPointD>(position, this->Location()+this->Axis(), -this->Angle());
	auto shape = this->Bounds();

	auto hit = fnPRV_GDI_HitTest_Rectangle(p0, margin, shape);
	if (hit.Mode != 0)
		return TxHitPosition(1, 0, 0);

	return TxHitPosition::Default();
}

// ================================================================================
void CxGdiPoint::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiPoint>(prev_figure))
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
TxRGB8x4 CxGdiPoint::BkColor() const
{
	return m_Param.BkColor;
}

// ============================================================
void CxGdiPoint::BkColor( TxRGB8x4 value )
{
	m_Param.BkColor = value;
}

// ============================================================
bool CxGdiPoint::BkEnable() const
{
	return (m_Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiPoint::BkEnable( bool value )
{
	m_Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiPoint::PenColor() const
{
	return m_Param.PenColor;
}

// ============================================================
void CxGdiPoint::PenColor( TxRGB8x4 value )
{
	m_Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiPoint::PenStyle() const
{
	return m_Param.PenStyle;
}

// ============================================================
void CxGdiPoint::PenStyle( ExGdiPenStyle value )
{
	m_Param.PenStyle = value;
}

// ============================================================
int CxGdiPoint::PenWidth() const
{
	return m_Param.PenWidth;
}

// ============================================================
void CxGdiPoint::PenWidth( int value )
{
	m_Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiPoint::Param() const
{
	return m_Param;
}

// ============================================================
void CxGdiPoint::Param( const TxGdi2dParam& value )
{
	m_Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
double CxGdiPoint::X() const
{
	return Shape.X;
}

// ============================================================
void CxGdiPoint::X(double value)
{
	Shape.X = value;
}

// ============================================================
double CxGdiPoint::Y() const
{
	return Shape.Y;
}

// ============================================================
void CxGdiPoint::Y(double value)
{
	Shape.Y = value;
}

// ============================================================
ExGdiAnchorStyle CxGdiPoint::AnchorStyle() const
{
	return m_AnchorStyle;
}

// ============================================================
void CxGdiPoint::AnchorStyle( ExGdiAnchorStyle value )
{
	m_AnchorStyle = value;
}

// ============================================================
TxSizeD CxGdiPoint::AnchorSize() const
{
	return m_AnchorSize;
}

// ============================================================
void CxGdiPoint::AnchorSize( TxSizeD value )
{
	m_AnchorSize = value;
}

// ============================================================
void CxGdiPoint::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL || value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "AnchorStyle") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, m_AnchorStyle)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "AnchorSize") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, m_AnchorSize)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "m_Param") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, m_Param)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	if (fnPRV_Gdi2d_GetParam(name, value, model, m_Param)) return;

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxGdiPoint::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL || value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "AnchorStyle") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, m_AnchorStyle)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (strcmp(name, "AnchorSize") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, m_AnchorSize)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
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
