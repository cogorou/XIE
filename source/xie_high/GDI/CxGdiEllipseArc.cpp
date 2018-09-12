/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiEllipseArc.h"
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

static const char* g_ClassName = "CxGdiEllipseArc";

// ================================================================================
void CxGdiEllipseArc::_Constructor()
{
	m_Param = TxGdi2dParam::Default();
	Shape = TxEllipseArcD::Default();
}

// ================================================================================
CxGdiEllipseArc::CxGdiEllipseArc()
{
	_Constructor();
}

// ================================================================================
CxGdiEllipseArc::CxGdiEllipseArc( CxGdiEllipseArc&& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiEllipseArc::CxGdiEllipseArc( const CxGdiEllipseArc& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiEllipseArc::CxGdiEllipseArc( const TxEllipseArcD& shape, bool closed )
{
	_Constructor();
	Shape = shape;
	m_Closed = closed;
}

// ================================================================================
CxGdiEllipseArc::CxGdiEllipseArc( const TxEllipseArcI& shape, bool closed )
{
	_Constructor();
	Shape = shape;
	m_Closed = closed;
}

// ============================================================
CxGdiEllipseArc::CxGdiEllipseArc(double x, double y, double radius_x, double radius_y, double start_angle, double sweep_angle, bool closed)
{
	_Constructor();
	Shape.X = x;
	Shape.Y = y;
	Shape.RadiusX = radius_x;
	Shape.RadiusY = radius_y;
	Shape.StartAngle = start_angle;
	Shape.SweepAngle = sweep_angle;
	m_Closed = closed;
}

// ============================================================
CxGdiEllipseArc::CxGdiEllipseArc(TxPointD center, double radius_x, double radius_y, double start_angle, double sweep_angle, bool closed)
{
	_Constructor();
	Shape.X = center.X;
	Shape.Y = center.Y;
	Shape.RadiusX = radius_x;
	Shape.RadiusY = radius_y;
	Shape.StartAngle = start_angle;
	Shape.SweepAngle = sweep_angle;
	m_Closed = closed;
}

// ================================================================================
CxGdiEllipseArc::~CxGdiEllipseArc()
{
}

// ============================================================
CxGdiEllipseArc& CxGdiEllipseArc::operator = ( CxGdiEllipseArc&& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiEllipseArc& CxGdiEllipseArc::operator = ( const CxGdiEllipseArc& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxGdiEllipseArc::operator == ( const CxGdiEllipseArc& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiEllipseArc::operator != ( const CxGdiEllipseArc& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxEllipseArcD CxGdiEllipseArc::Tag() const
{
	return Shape;
}

// ============================================================
void* CxGdiEllipseArc::TagPtr() const
{
	return (void*)&Shape;
}

// ============================================================
CxGdiEllipseArc& CxGdiEllipseArc::operator = ( const TxEllipseArcD& src )
{
	Shape = src;
	return *this;
}
// ============================================================
CxGdiEllipseArc& CxGdiEllipseArc::operator = ( const TxEllipseArcI& src )
{
	Shape = (TxEllipseArcD)src;
	return *this;
}

// ================================================================================
CxGdiEllipseArc::operator TxEllipseArcD() const
{
	return Shape;
}

// ================================================================================
CxGdiEllipseArc::operator TxEllipseArcI() const
{
	return (TxEllipseArcI)Shape;
}

// ================================================================================
CxGdiEllipseArc CxGdiEllipseArc::Clone() const
{
	CxGdiEllipseArc clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiEllipseArc::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiEllipseArc>(src))
	{
		auto&	_src = static_cast<const CxGdiEllipseArc&>(src);
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
bool CxGdiEllipseArc::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiEllipseArc>(src))
	{
		auto&	_src = static_cast<const CxGdiEllipseArc&>(src);
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
void CxGdiEllipseArc::Render(HxModule hcanvas, ExGdiScalingMode mode) const
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
	auto radius_x = Shape.RadiusX;
	auto radius_y = Shape.RadiusY;
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
TxPointD CxGdiEllipseArc::Location() const
{
	return TxPointD(Shape.X, Shape.Y);
}

// ================================================================================
void CxGdiEllipseArc::Location(TxPointD value)
{
	Shape.X = value.X;
	Shape.Y = value.Y;
}

// ================================================================================
TxRectangleD CxGdiEllipseArc::Bounds() const
{
	TxRectangleD rect(
		Shape.X - Shape.RadiusX,
		Shape.Y - Shape.RadiusY,
		Shape.RadiusX * 2,
		Shape.RadiusY * 2
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
double CxGdiEllipseArc::Angle() const
{
	return m_Param.Angle;
}

// ============================================================
void CxGdiEllipseArc::Angle( double degree )
{
	m_Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiEllipseArc::Axis() const
{
	return m_Param.Axis;
}

// ============================================================
void CxGdiEllipseArc::Axis( TxPointD value )
{
	m_Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiEllipseArc::HitTest( TxPointD position, double margin ) const
{
	auto p0 = xie::Axi::Rotate<TxPointD>(position, this->Location()+this->Axis(), -this->Angle());

	auto dx = p0.X - Shape.X;
	auto dy = p0.Y - Shape.Y;
	if (Shape.RadiusX != 0 && Shape.RadiusY != 0)
	{
		auto dx1 = (dx - margin) * (dx - margin);
		auto dy1 = (dy - margin) * (dy - margin);
		auto dx2 = (dx + margin) * (dx + margin);
		auto dy2 = (dy + margin) * (dy + margin);
		auto ax = (Shape.RadiusX * Shape.RadiusX);
		auto ay = (Shape.RadiusY * Shape.RadiusY);
		TxStatistics stat;
		stat += ((dx1 / ax) + (dy1 / ay));
		stat += ((dx2 / ax) + (dy1 / ay));
		stat += ((dx2 / ax) + (dy2 / ay));
		stat += ((dx1 / ax) + (dy2 / ay));
		if (stat.Min <= 1.0 && 1.0 <= stat.Max)
			return TxHitPosition(2, 0, +1);
		if (stat.Min <= 1.0)
			return TxHitPosition(1, 0, 0);
	}
	else if (
		Shape.RadiusX-margin <= dx && dx <= Shape.RadiusX+margin &&
		Shape.RadiusY-margin <= dy && dy <= Shape.RadiusY+margin
		)
	{
		return TxHitPosition(2, 0, +1);
	}

	return TxHitPosition::Default();
}

// ================================================================================
void CxGdiEllipseArc::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiEllipseArc>(prev_figure))
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
					prev_diff.X = abs(prev_diff.X);
					prev_diff.Y = abs(prev_diff.Y);
					move_diff.X = abs(move_diff.X);
					move_diff.Y = abs(move_diff.Y);
					auto mv = move_diff - prev_diff;
					this->RadiusX( figure->RadiusX() + mv.X );
					this->RadiusY( figure->RadiusY() + mv.Y );
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
TxRGB8x4 CxGdiEllipseArc::BkColor() const
{
	return m_Param.BkColor;
}

// ============================================================
void CxGdiEllipseArc::BkColor( TxRGB8x4 value )
{
	m_Param.BkColor = value;
}

// ============================================================
bool CxGdiEllipseArc::BkEnable() const
{
	return (m_Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiEllipseArc::BkEnable( bool value )
{
	m_Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiEllipseArc::PenColor() const
{
	return m_Param.PenColor;
}

// ============================================================
void CxGdiEllipseArc::PenColor( TxRGB8x4 value )
{
	m_Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiEllipseArc::PenStyle() const
{
	return m_Param.PenStyle;
}

// ============================================================
void CxGdiEllipseArc::PenStyle( ExGdiPenStyle value )
{
	m_Param.PenStyle = value;
}

// ============================================================
int CxGdiEllipseArc::PenWidth() const
{
	return m_Param.PenWidth;
}

// ============================================================
void CxGdiEllipseArc::PenWidth( int value )
{
	m_Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiEllipseArc::Param() const
{
	return m_Param;
}

// ============================================================
void CxGdiEllipseArc::Param( const TxGdi2dParam& value )
{
	m_Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
double CxGdiEllipseArc::X() const
{
	return Shape.X;
}

// ============================================================
void CxGdiEllipseArc::X(double value)
{
	Shape.X = value;
}

// ============================================================
double CxGdiEllipseArc::Y() const
{
	return Shape.Y;
}

// ============================================================
void CxGdiEllipseArc::Y(double value)
{
	Shape.Y = value;
}

// ============================================================
double CxGdiEllipseArc::RadiusX() const
{
	return Shape.RadiusX;
}

// ============================================================
void CxGdiEllipseArc::RadiusX(double value)
{
	Shape.RadiusX = value;
}

// ============================================================
double CxGdiEllipseArc::RadiusY() const
{
	return Shape.RadiusY;
}

// ============================================================
void CxGdiEllipseArc::RadiusY(double value)
{
	Shape.RadiusY = value;
}

// ============================================================
double CxGdiEllipseArc::StartAngle() const
{
	return Shape.StartAngle;
}

// ============================================================
void CxGdiEllipseArc::StartAngle(double value)
{
	Shape.StartAngle = value;
}

// ============================================================
double CxGdiEllipseArc::SweepAngle() const
{
	return Shape.SweepAngle;
}

// ============================================================
void CxGdiEllipseArc::SweepAngle(double value)
{
	Shape.SweepAngle = value;
}

// ============================================================
bool CxGdiEllipseArc::Closed() const
{
	return m_Closed;
}

// ============================================================
void CxGdiEllipseArc::Closed(bool value)
{
	m_Closed = value;
}

// ============================================================
void CxGdiEllipseArc::GetParam(TxCharCPtrA name, void* value, TxModel model) const
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
void CxGdiEllipseArc::SetParam(TxCharCPtrA name, const void* value, TxModel model)
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
