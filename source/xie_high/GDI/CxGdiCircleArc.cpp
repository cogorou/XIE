/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiCircleArc.h"
#include "Core/CxException.h"
#include "Core/Axi.h"
#include "Core/CxArrayEx.h"
#include "Core/IxParam.h"
#include "Core/CxStopwatch.h"

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

static const char* g_ClassName = "CxGdiCircleArc";

// ================================================================================
void CxGdiCircleArc::_Constructor()
{
	m_Param = TxGdi2dParam::Default();
	Shape = TxCircleArcD::Default();
}

// ================================================================================
CxGdiCircleArc::CxGdiCircleArc()
{
	_Constructor();
}

// ================================================================================
CxGdiCircleArc::CxGdiCircleArc( CxGdiCircleArc&& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiCircleArc::CxGdiCircleArc( const CxGdiCircleArc& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiCircleArc::CxGdiCircleArc( const TxCircleArcD& shape, bool closed )
{
	_Constructor();
	Shape = shape;
	m_Closed = closed;
}

// ================================================================================
CxGdiCircleArc::CxGdiCircleArc( const TxCircleArcI& shape, bool closed )
{
	_Constructor();
	Shape = shape;
	m_Closed = closed;
}

// ============================================================
CxGdiCircleArc::CxGdiCircleArc(double x, double y, double radius, double start_angle, double sweep_angle, bool closed)
{
	_Constructor();
	Shape.X = x;
	Shape.Y = y;
	Shape.Radius = radius;
	Shape.StartAngle = start_angle;
	Shape.SweepAngle = sweep_angle;
	m_Closed = closed;
}

// ============================================================
CxGdiCircleArc::CxGdiCircleArc(TxPointD center, double radius, double start_angle, double sweep_angle, bool closed)
{
	_Constructor();
	Shape.X = center.X;
	Shape.Y = center.Y;
	Shape.Radius = radius;
	Shape.StartAngle = start_angle;
	Shape.SweepAngle = sweep_angle;
	m_Closed = closed;
}

// ================================================================================
CxGdiCircleArc::~CxGdiCircleArc()
{
}

// ============================================================
CxGdiCircleArc& CxGdiCircleArc::operator = ( CxGdiCircleArc&& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiCircleArc& CxGdiCircleArc::operator = ( const CxGdiCircleArc& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxGdiCircleArc::operator == ( const CxGdiCircleArc& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiCircleArc::operator != ( const CxGdiCircleArc& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxCircleArcD CxGdiCircleArc::Tag() const
{
	return Shape;
}

// ============================================================
void* CxGdiCircleArc::TagPtr() const
{
	return (void*)&Shape;
}

// ============================================================
CxGdiCircleArc& CxGdiCircleArc::operator = ( const TxCircleArcD& src )
{
	Shape = src;
	return *this;
}
// ============================================================
CxGdiCircleArc& CxGdiCircleArc::operator = ( const TxCircleArcI& src )
{
	Shape = (TxCircleArcD)src;
	return *this;
}

// ================================================================================
CxGdiCircleArc::operator TxCircleArcD() const
{
	return Shape;
}

// ================================================================================
CxGdiCircleArc::operator TxCircleArcI() const
{
	return (TxCircleArcI)Shape;
}

// ================================================================================
CxGdiCircleArc CxGdiCircleArc::Clone() const
{
	CxGdiCircleArc clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiCircleArc::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiCircleArc>(src))
	{
		auto&	_src = static_cast<const CxGdiCircleArc&>(src);
		auto&	_dst = *this;

		_dst.Shape = _src.Shape;
		_dst.m_Param = _src.m_Param;
		_dst.m_Closed = _src.m_Closed;
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
bool CxGdiCircleArc::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiCircleArc>(src))
	{
		auto&	_src = static_cast<const CxGdiCircleArc&>(src);
		auto&	_dst = *this;

		if (_dst.Shape != _src.Shape) return false;
		if (_dst.m_Param != _src.m_Param) return false;
		if (_dst.m_Closed != _src.m_Closed) return false;

		return true;
	}
	return false;
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
void CxGdiCircleArc::Render(HxModule hcanvas, ExGdiScalingMode mode) const
{
	//CxStopwatch watch;

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

	// ----------------------------------------------------------------------
	auto closed = this->Closed();
	auto mag = canvas.Magnification;
	auto radius_x = Shape.Radius;
	auto radius_y = Shape.Radius;
	auto arc_angle = xie::Axi::DegToRad(Shape.StartAngle);
	auto arc_range = xie::Axi::DegToRad(Shape.SweepAngle);
	auto is_arc = (XIE_EPSd < _abs(2 * XIE_PI - _abs(arc_range)));
	auto center = TxPointD(Shape.X, Shape.Y);
	auto st = TxPointD(
				center.X + radius_x * cos(arc_angle),
				center.Y + radius_y * sin(arc_angle)
				);
	auto ed = TxPointD(
				center.X + radius_x * cos(arc_angle + arc_range),
				center.Y + radius_y * sin(arc_angle + arc_range)
				);
	auto perimeter = (int)(mag * ((radius_x + radius_y) * 0.5) * _abs(arc_range));

	// ògê¸ ãyÇ— îwåiÇÃó÷äs ÇÃëOåvéZ.
	CxArrayEx<TxPointD> vertex(perimeter);
	for(int pp=0 ; pp<perimeter ; pp++)
	{
		vertex[pp].X = center.X + radius_x * cos(arc_angle + arc_range * pp / perimeter);
		vertex[pp].Y = center.Y + radius_y * sin(arc_angle + arc_range * pp / perimeter);
	}

	// îwåi.
	// ï˚ñ@) íÜêSÇ∆â~é¸è„ÇÃäeì_ÇåãÇ‘éOäpå`Ç≈ìhÇËí◊Ç∑.
	/*
		â~é¸è„ÇÃì_ÇÃå¬êîÇ™ 500 Çí¥Ç¶ÇΩï”ÇËÇ©ÇÁê›íËÇµÇΩï`âÊêFÇ…îíêFÇ™ç¨Ç¥ÇÈåªè€Ç™å©ÇÁÇÍÇΩÅB
		âΩÇ©ÇÃÉoÉOÇ©ä¬ã´àÀë∂Ç©Ç‡ímÇÍÇ»Ç¢ÅB
		ëŒè«ó√ñ@Ç∆Ç»ÇÈÇ™ glBegin/glEnd ÇÃä‘Ç…ï`âÊÇ∑ÇÈì_êîÇÃè„å¿Çê›ÇØÇÈéñÇ∆ÇµÇΩÅB
	*/
	if (this->BkEnable() == true && closed)
	{
		fnPRV_GDI_BkPrepare(&m_Param);

		::glBegin(GL_TRIANGLE_FAN);
		::glVertex2d( center.X, center.Y );
		::glVertex2d( st.X, st.Y );
		{
			const int limit = 36;
			const int overlap = 1;	// must be 1.
			for(int pp=0 ; pp<vertex.Length() ; pp++)
			{
				// ï`âÊÇ∑ÇÈì_êîÇÃè„å¿ÇÃÉ`ÉFÉbÉN.
				if (pp != 0 && pp % limit == 0)
				{
					::glEnd();
					::glBegin(GL_TRIANGLE_FAN);
					::glVertex2d(center.X, center.Y);

					// ÉIÅ[ÉoÅ[ÉâÉbÉv.
					for(int i=0 ; i<overlap ; i++)
					{
						::glVertex2d(
							vertex[pp-(overlap-i)].X,
							vertex[pp-(overlap-i)].Y
						);
					}
				}
				::glVertex2d(vertex[pp].X, vertex[pp].Y);
			}
		}
		::glVertex2d( ed.X, ed.Y );
		if (!is_arc)
			::glVertex2d( st.X, st.Y );
		::glEnd();

		fnPRV_GDI_BkRestore(&m_Param);
	}

	// ògê¸.
	if (this->PenStyle() != ExGdiPenStyle::None)
	{
		fnPRV_GDI_PenPrepare(&m_Param);

		::glBegin(closed ? GL_LINE_LOOP : GL_LINE_STRIP);
		::glVertex2d( st.X, st.Y );
		for(int pp=0 ; pp<vertex.Length() ; pp++)
		{
			::glVertex2d(vertex[pp].X, vertex[pp].Y);
		}
		::glVertex2d( ed.X, ed.Y );
		if (closed && is_arc)
		{
			::glVertex2d( center.X, center.Y );
			::glVertex2d( st.X, st.Y );
		}
		::glEnd();

		fnPRV_GDI_PenRestore(&m_Param);
	}

	::glDisable( GL_BLEND );
	::glPopMatrix();

	//watch.Stop();
	//fnXIE_Core_TraceOut(0, "%s: %f msec\n", __FUNCTION__, watch.Elapsed);
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
TxPointD CxGdiCircleArc::Location() const
{
	return TxPointD(Shape.X, Shape.Y);
}

// ================================================================================
void CxGdiCircleArc::Location(TxPointD value)
{
	Shape.X = value.X;
	Shape.Y = value.Y;
}

// ================================================================================
TxRectangleD CxGdiCircleArc::Bounds() const
{
	TxRectangleD rect(
		Shape.X - Shape.Radius,
		Shape.Y - Shape.Radius,
		Shape.Radius * 2,
		Shape.Radius * 2
		);

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

	auto sx = stat_x.Min;
	auto sy = stat_y.Min;
	auto ex = stat_x.Max;
	auto ey = stat_y.Max;

	return TxRectangleD(sx, sy, (ex - sx), (ey - sy));
}

// ============================================================
double CxGdiCircleArc::Angle() const
{
	return m_Param.Angle;
}

// ============================================================
void CxGdiCircleArc::Angle( double degree )
{
	m_Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiCircleArc::Axis() const
{
	return m_Param.Axis;
}

// ============================================================
void CxGdiCircleArc::Axis( TxPointD value )
{
	m_Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiCircleArc::HitTest( TxPointD position, double margin ) const
{
	auto p0 = xie::Axi::Rotate<TxPointD>(position, this->Location()+this->Axis(), -this->Angle());

	auto dx = p0.X - Shape.X;
	auto dy = p0.Y - Shape.Y;
	auto distance = sqrt(dx * dx + dy * dy);

	if (Shape.Radius-margin <= distance && distance <= Shape.Radius+margin)
		return TxHitPosition(2, 0, +1);

	if (0 <= distance && distance <= Shape.Radius-margin)
		return TxHitPosition(1, 0, 0);

	return TxHitPosition::Default();
}

// ================================================================================
void CxGdiCircleArc::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiCircleArc>(prev_figure))
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
					auto _base_position = xie::Axi::Rotate<TxPointD>(figure->Location(), figure->Location() + figure->Axis(), -figure->Angle());
					auto _prev_position = xie::Axi::Rotate<TxPointD>(prev_position, figure->Location() + figure->Axis(), -figure->Angle());
					auto _move_position = xie::Axi::Rotate<TxPointD>(move_position, figure->Location() + figure->Axis(), -figure->Angle());
					auto prev_diff = _prev_position - _base_position;
					auto move_diff = _move_position - _base_position;
					auto prev_radius = sqrt(prev_diff.X * prev_diff.X + prev_diff.Y * prev_diff.Y);
					auto move_radius = sqrt(move_diff.X * move_diff.X + move_diff.Y * move_diff.Y);
					auto mv = move_radius - prev_radius;
					this->Radius( figure->Radius() + mv );
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
TxRGB8x4 CxGdiCircleArc::BkColor() const
{
	return m_Param.BkColor;
}

// ============================================================
void CxGdiCircleArc::BkColor( TxRGB8x4 value )
{
	m_Param.BkColor = value;
}

// ============================================================
bool CxGdiCircleArc::BkEnable() const
{
	return (m_Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiCircleArc::BkEnable( bool value )
{
	m_Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiCircleArc::PenColor() const
{
	return m_Param.PenColor;
}

// ============================================================
void CxGdiCircleArc::PenColor( TxRGB8x4 value )
{
	m_Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiCircleArc::PenStyle() const
{
	return m_Param.PenStyle;
}

// ============================================================
void CxGdiCircleArc::PenStyle( ExGdiPenStyle value )
{
	m_Param.PenStyle = value;
}

// ============================================================
int CxGdiCircleArc::PenWidth() const
{
	return m_Param.PenWidth;
}

// ============================================================
void CxGdiCircleArc::PenWidth( int value )
{
	m_Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiCircleArc::Param() const
{
	return m_Param;
}

// ============================================================
void CxGdiCircleArc::Param( const TxGdi2dParam& value )
{
	m_Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
double CxGdiCircleArc::X() const
{
	return Shape.X;
}

// ============================================================
void CxGdiCircleArc::X(double value)
{
	Shape.X = value;
}

// ============================================================
double CxGdiCircleArc::Y() const
{
	return Shape.Y;
}

// ============================================================
void CxGdiCircleArc::Y(double value)
{
	Shape.Y = value;
}

// ============================================================
double CxGdiCircleArc::Radius() const
{
	return Shape.Radius;
}

// ============================================================
void CxGdiCircleArc::Radius(double value)
{
	Shape.Radius = value;
}

// ============================================================
double CxGdiCircleArc::StartAngle() const
{
	return Shape.StartAngle;
}

// ============================================================
void CxGdiCircleArc::StartAngle(double value)
{
	Shape.StartAngle = value;
}

// ============================================================
double CxGdiCircleArc::SweepAngle() const
{
	return Shape.SweepAngle;
}

// ============================================================
void CxGdiCircleArc::SweepAngle(double value)
{
	Shape.SweepAngle = value;
}

// ============================================================
bool CxGdiCircleArc::Closed() const
{
	return m_Closed;
}

// ============================================================
void CxGdiCircleArc::Closed(bool value)
{
	m_Closed = value;
}

// ============================================================
void CxGdiCircleArc::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL || value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Closed") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, m_Closed)) return;
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
void CxGdiCircleArc::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL || value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Closed") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, m_Closed)) return;
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
