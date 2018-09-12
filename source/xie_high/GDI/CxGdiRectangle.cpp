/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiRectangle.h"
#include "GDI/CxGdiEllipseArc.h"
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

static const char* g_ClassName = "CxGdiRectangle";

// ================================================================================
void CxGdiRectangle::_Constructor()
{
	m_Param = TxGdi2dParam::Default();
	Shape = TxRectangleD::Default();
	m_RoundSize = TxSizeD::Default();
}

// ================================================================================
CxGdiRectangle::CxGdiRectangle()
{
	_Constructor();
}

// ================================================================================
CxGdiRectangle::CxGdiRectangle( CxGdiRectangle&& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiRectangle::CxGdiRectangle( const CxGdiRectangle& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiRectangle::CxGdiRectangle( const TxRectangleD& shape )
{
	_Constructor();
	Shape = shape;
}

// ================================================================================
CxGdiRectangle::CxGdiRectangle( const TxRectangleI& shape )
{
	_Constructor();
	Shape = shape;
}

// ============================================================
CxGdiRectangle::CxGdiRectangle(double x, double y, double width, double height)
{
	_Constructor();
	Shape.X = x;
	Shape.Y = y;
	Shape.Width = width;
	Shape.Height = height;
}

// ============================================================
CxGdiRectangle::CxGdiRectangle(TxPointD location, TxSizeD size)
{
	_Constructor();
	Shape.X = location.X;
	Shape.Y = location.Y;
	Shape.Width = size.Width;
	Shape.Height = size.Height;
}

// ================================================================================
CxGdiRectangle::~CxGdiRectangle()
{
}

// ============================================================
CxGdiRectangle& CxGdiRectangle::operator = ( CxGdiRectangle&& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiRectangle& CxGdiRectangle::operator = ( const CxGdiRectangle& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxGdiRectangle::operator == ( const CxGdiRectangle& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiRectangle::operator != ( const CxGdiRectangle& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxRectangleD CxGdiRectangle::Tag() const
{
	return Shape;
}

// ============================================================
void* CxGdiRectangle::TagPtr() const
{
	return (void*)&Shape;
}

// ============================================================
CxGdiRectangle& CxGdiRectangle::operator = ( const TxRectangleD& src )
{
	Shape = src;
	return *this;
}

// ============================================================
CxGdiRectangle& CxGdiRectangle::operator = ( const TxRectangleI& src )
{
	Shape = (TxRectangleD)src;
	return *this;
}

// ================================================================================
CxGdiRectangle::operator TxRectangleD() const
{
	return Shape;
}

// ================================================================================
CxGdiRectangle::operator TxRectangleI() const
{
	return (TxRectangleI)Shape;
}

// ================================================================================
CxGdiRectangle CxGdiRectangle::Clone() const
{
	CxGdiRectangle clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiRectangle::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiRectangle>(src))
	{
		auto&	_src = static_cast<const CxGdiRectangle&>(src);
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
bool CxGdiRectangle::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiRectangle>(src))
	{
		auto&	_src = static_cast<const CxGdiRectangle&>(src);
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
void CxGdiRectangle::Render(HxModule hcanvas, ExGdiScalingMode mode) const
{
	// ï\é¶îÕàÕÉpÉâÉÅÅ[É^éÊìæ.
	TxCanvas	canvas;
	IxParam*	dpi = xie::Axi::SafeCast<IxParam>(hcanvas);
	if (dpi == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	dpi->GetParam("Tag", &canvas, TxModel::Default());

	::glPushMatrix();

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
	if (m_RoundSize.Width == 0 || m_RoundSize.Height == 0)
	{
		::glEnable( GL_BLEND );
		::glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

		double	dx1 = Shape.X;
		double	dy1 = Shape.Y;
		double	dx2 = Shape.X + Shape.Width;
		double	dy2 = Shape.Y + Shape.Height;

		// îwåi.
		if (this->BkEnable() == true)
		{
			fnPRV_GDI_BkPrepare(&m_Param);

			::glBegin(GL_TRIANGLE_FAN);
			::glVertex2d( dx1, dy1 );
			::glVertex2d( dx2, dy1 );
			::glVertex2d( dx2, dy2 );
			::glVertex2d( dx1, dy2 );
			::glEnd();

			fnPRV_GDI_BkRestore(&m_Param);
		}

		// ògê¸.
		if (this->PenStyle() != ExGdiPenStyle::None)
		{
			fnPRV_GDI_PenPrepare(&m_Param);

			::glBegin(GL_LINE_LOOP);
			::glVertex2d( dx1, dy1 );
			::glVertex2d( dx2, dy1 );
			::glVertex2d( dx2, dy2 );
			::glVertex2d( dx1, dy2 );
			::glEnd();

			fnPRV_GDI_PenRestore(&m_Param);
		}

		::glDisable( GL_BLEND );
	}
	else
	{
		/*	
		 *   1        2
		 *	Ñ°ÑüÑ¶ÑüÑüÑüÑüÑüÑüÑ¶ÑüÑ¢
		 *	Ñ•ÑüÑ©ÑüÑüÑüÑüÑüÑüÑ©ÑüÑß
		 *	Ñ† Ñ†      Ñ† Ñ†
		 *	Ñ† Ñ†      Ñ† Ñ†
		 *	Ñ•ÑüÑ©ÑüÑüÑüÑüÑüÑüÑ©ÑüÑß
		 *	Ñ§ÑüÑ®ÑüÑüÑüÑüÑüÑüÑ®ÑüÑ£
		 *   4        3
		 */
		double rw = _abs(m_RoundSize.Width);
		double rh = _abs(m_RoundSize.Height);
		auto bounds = this->Bounds();
		auto inside = TxRectangleD(
				bounds.Location() + TxPointD(rw, rh),
				bounds.Size() - (TxSizeD(rw, rh) * 2)
			);
		auto vo = bounds.ToTrapezoid();
		auto vi = inside.ToTrapezoid();

		xie::CxArrayEx<CxGdiEllipseArc> arcs = 
		{
			CxGdiEllipseArc((vi.X1 - rw), (vi.Y1 - rh), rw * 2, rh * 2, 180, 90),
			CxGdiEllipseArc((vi.X2 - rw), (vi.Y2 - rh), rw * 2, rh * 2, 270, 90),
			CxGdiEllipseArc((vi.X3 - rw), (vi.Y3 - rh), rw * 2, rh * 2, 0, 90),
			CxGdiEllipseArc((vi.X4 - rw), (vi.Y4 - rh), rw * 2, rh * 2, 90, 90),
		};

		// îwåi.
		if (this->BkEnable() == true)
		{
			for(int i=0 ; i<arcs.Length() ; i++)
			{
				arcs[i].Closed(true);
				arcs[i].BkEnable(true);
				arcs[i].BkColor(this->BkColor());
				arcs[i].PenStyle(ExGdiPenStyle::None);
				arcs[i].Render(hcanvas, mode);
			}

			xie::CxArrayEx<CxGdiRectangle> rects = 
			{
				CxGdiRectangle(inside),
				CxGdiRectangle(vi.X1, vo.Y1, inside.Width, rh),
				CxGdiRectangle(vi.X4, vi.Y4, inside.Width, rh),
				CxGdiRectangle(vo.X1, vi.Y1, rw, inside.Height),
				CxGdiRectangle(vi.X2, vi.Y2, rw, inside.Height),
			};

			for(int i=0 ; i<arcs.Length() ; i++)
			{
				rects[i].BkEnable(true);
				rects[i].BkColor(this->BkColor());
				rects[i].PenStyle(ExGdiPenStyle::None);
				rects[i].Render(hcanvas, mode);
			}
		}

		// ògê¸.
		if (this->PenStyle() != ExGdiPenStyle::None)
		{
			for(int i=0 ; i<arcs.Length() ; i++)
			{
				arcs[i].Closed(false);
				arcs[i].BkEnable(false);
				arcs[i].PenColor(this->PenColor());
				arcs[i].PenWidth(this->PenWidth());
				arcs[i].PenStyle(this->PenStyle());
			}

			xie::CxArrayEx<CxGdiLineSegment> sides = 
			{
				CxGdiLineSegment(vi.X1, vo.Y1, vi.X2, vo.Y2),
				CxGdiLineSegment(vi.X4, vo.Y4, vi.X3, vo.Y3),
				CxGdiLineSegment(vo.X1, vi.Y1, vo.X4, vi.Y4),
				CxGdiLineSegment(vo.X2, vi.Y2, vo.X3, vi.Y3),
			};

			for(int i=0 ; i<arcs.Length() ; i++)
			{
				sides[i].BkEnable(false);
				sides[i].PenColor(this->PenColor());
				sides[i].PenWidth(this->PenWidth());
				sides[i].PenStyle(this->PenStyle());
				sides[i].Render(hcanvas, mode);
			}
		}
	}

	::glPopMatrix();
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
TxPointD CxGdiRectangle::Location() const
{
	return TxPointD(Shape.X, Shape.Y);
}

// ================================================================================
void CxGdiRectangle::Location(TxPointD value)
{
	Shape.X = value.X;
	Shape.Y = value.Y;
}

// ================================================================================
TxRectangleD CxGdiRectangle::Bounds() const
{
	auto trapezoid = Shape.ToTrapezoid();
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
double CxGdiRectangle::Angle() const
{
	return m_Param.Angle;
}

// ============================================================
void CxGdiRectangle::Angle( double degree )
{
	m_Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiRectangle::Axis() const
{
	return m_Param.Axis;
}

// ============================================================
void CxGdiRectangle::Axis( TxPointD value )
{
	m_Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiRectangle::HitTest( TxPointD position, double margin ) const
{
	auto p0 = xie::Axi::Rotate<TxPointD>(position, this->Location()+this->Axis(), -this->Angle());
	auto shape = (TxRectangleD)Shape;

	auto hit = fnPRV_GDI_HitTest_Rectangle(p0, margin, shape);
	return hit;
}

// ================================================================================
void CxGdiRectangle::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiRectangle>(prev_figure))
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
						case -4:
							this->X( figure->X() + mv.X );
							this->Width( figure->Width() - mv.X );
							break;
						case -1:
							this->Y( figure->Y() + mv.Y );
							this->Height( figure->Height() - mv.Y );
							break;
						case -2:
							this->Width( figure->Width() + mv.X );
							break;
						case -3:
							this->Height( figure->Height() + mv.Y );
							break;
						case +1:
							this->X( figure->X() + mv.X );
							this->Width( figure->Width() - mv.X );
							this->Y( figure->Y() + mv.Y );
							this->Height( figure->Height() - mv.Y );
							break;
						case +2:
							this->Width( figure->Width() + mv.X );
							this->Y( figure->Y() + mv.Y );
							this->Height( figure->Height() - mv.Y );
							break;
						case +3:
							this->Width( figure->Width() + mv.X );
							this->Height( figure->Height() + mv.Y );
							break;
						case +4:
							this->X( figure->X() + mv.X );
							this->Width( figure->Width() - mv.X );
							this->Height( figure->Height() + mv.Y );
							break;
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
TxRGB8x4 CxGdiRectangle::BkColor() const
{
	return m_Param.BkColor;
}

// ============================================================
void CxGdiRectangle::BkColor( TxRGB8x4 value )
{
	m_Param.BkColor = value;
}

// ============================================================
bool CxGdiRectangle::BkEnable() const
{
	return (m_Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiRectangle::BkEnable( bool value )
{
	m_Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiRectangle::PenColor() const
{
	return m_Param.PenColor;
}

// ============================================================
void CxGdiRectangle::PenColor( TxRGB8x4 value )
{
	m_Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiRectangle::PenStyle() const
{
	return m_Param.PenStyle;
}

// ============================================================
void CxGdiRectangle::PenStyle( ExGdiPenStyle value )
{
	m_Param.PenStyle = value;
}

// ============================================================
int CxGdiRectangle::PenWidth() const
{
	return m_Param.PenWidth;
}

// ============================================================
void CxGdiRectangle::PenWidth( int value )
{
	m_Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiRectangle::Param() const
{
	return m_Param;
}

// ============================================================
void CxGdiRectangle::Param( const TxGdi2dParam& value )
{
	m_Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
double CxGdiRectangle::X() const
{
	return Shape.X;
}

// ============================================================
void CxGdiRectangle::X(double value)
{
	Shape.X = value;
}

// ============================================================
double CxGdiRectangle::Y() const
{
	return Shape.Y;
}

// ============================================================
void CxGdiRectangle::Y(double value)
{
	Shape.Y = value;
}

// ============================================================
double CxGdiRectangle::Width() const
{
	return Shape.Width;
}

// ============================================================
void CxGdiRectangle::Width(double value)
{
	Shape.Width = value;
}

// ============================================================
double CxGdiRectangle::Height() const
{
	return Shape.Height;
}

// ============================================================
void CxGdiRectangle::Height(double value)
{
	Shape.Height = value;
}

// ============================================================
void CxGdiRectangle::Size(const TxSizeD& value)
{
	Shape.Width = value.Width;
	Shape.Height = value.Height;
}

// ============================================================
TxSizeD CxGdiRectangle::Size() const
{
	return TxSizeD(Shape.Width, Shape.Height);
}

// ============================================================
TxSizeD CxGdiRectangle::RoundSize() const
{
	return m_RoundSize;
}

// ============================================================
void CxGdiRectangle::RoundSize( TxSizeD value )
{
	m_RoundSize = value;
}

// ============================================================
void CxGdiRectangle::GetParam(TxCharCPtrA name, void* value, TxModel model) const
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
void CxGdiRectangle::SetParam(TxCharCPtrA name, const void* value, TxModel model)
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
