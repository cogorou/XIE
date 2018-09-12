/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiEllipse.h"
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

static const char* g_ClassName = "CxGdiEllipse";

// ================================================================================
void CxGdiEllipse::_Constructor()
{
	m_Param = TxGdi2dParam::Default();
	Shape = TxEllipseD::Default();
}

// ================================================================================
CxGdiEllipse::CxGdiEllipse()
{
	_Constructor();
}

// ================================================================================
CxGdiEllipse::CxGdiEllipse( CxGdiEllipse&& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiEllipse::CxGdiEllipse( const CxGdiEllipse& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiEllipse::CxGdiEllipse( const TxEllipseD& shape )
{
	_Constructor();
	Shape = shape;
}

// ================================================================================
CxGdiEllipse::CxGdiEllipse( const TxEllipseI& shape )
{
	_Constructor();
	Shape = shape;
}

// ============================================================
CxGdiEllipse::CxGdiEllipse(double x, double y, double radius_x, double radius_y)
{
	_Constructor();
	Shape.X = x;
	Shape.Y = y;
	Shape.RadiusX = radius_x;
	Shape.RadiusY = radius_y;
}

// ============================================================
CxGdiEllipse::CxGdiEllipse(TxPointD center, double radius_x, double radius_y)
{
	_Constructor();
	Shape.X = center.X;
	Shape.Y = center.Y;
	Shape.RadiusX = radius_x;
	Shape.RadiusY = radius_y;
}

// ================================================================================
CxGdiEllipse::~CxGdiEllipse()
{
}

// ============================================================
CxGdiEllipse& CxGdiEllipse::operator = ( CxGdiEllipse&& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiEllipse& CxGdiEllipse::operator = ( const CxGdiEllipse& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxGdiEllipse::operator == ( const CxGdiEllipse& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiEllipse::operator != ( const CxGdiEllipse& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxEllipseD CxGdiEllipse::Tag() const
{
	return Shape;
}

// ============================================================
void* CxGdiEllipse::TagPtr() const
{
	return (void*)&Shape;
}

// ============================================================
CxGdiEllipse& CxGdiEllipse::operator = ( const TxEllipseD& src )
{
	Shape = src;
	return *this;
}

// ============================================================
CxGdiEllipse& CxGdiEllipse::operator = ( const TxEllipseI& src )
{
	Shape = (TxEllipseD)src;
	return *this;
}

// ================================================================================
CxGdiEllipse::operator TxEllipseD() const
{
	return Shape;
}

// ================================================================================
CxGdiEllipse::operator TxEllipseI() const
{
	return (TxEllipseI)Shape;
}

// ================================================================================
CxGdiEllipse CxGdiEllipse::Clone() const
{
	CxGdiEllipse clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiEllipse::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiEllipse>(src))
	{
		auto&	_src = static_cast<const CxGdiEllipse&>(src);
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
bool CxGdiEllipse::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiEllipse>(src))
	{
		auto&	_src = static_cast<const CxGdiEllipse&>(src);
		auto&	_dst = *this;

		if (_dst.Shape != _src.Shape) return false;
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
void CxGdiEllipse::Render(HxModule hcanvas, ExGdiScalingMode mode) const
{
	//CxStopwatch watch;

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

	// ----------------------------------------------------------------------
	auto closed = true;
	auto mag = canvas.Magnification;
	auto radius_x = Shape.RadiusX;
	auto radius_y = Shape.RadiusY;
	auto arc_angle = xie::Axi::DegToRad(0);
	auto arc_range = xie::Axi::DegToRad(360);
	auto center = TxPointD(Shape.X, Shape.Y);
	auto perimeter = (int)(mag * ((radius_x + radius_y) * 0.5) * _abs(arc_range));

	// 枠線 及び 背景の輪郭 の前計算.
	CxArrayEx<TxPointD> vertex(perimeter + 1);
	for(int pp=0 ; pp<perimeter ; pp++)
	{
		vertex[pp].X = center.X + radius_x * cos(arc_angle + arc_range * pp / perimeter);
		vertex[pp].Y = center.Y + radius_y * sin(arc_angle + arc_range * pp / perimeter);
	}
	{
		vertex[perimeter].X = center.X + radius_x * cos(0);
		vertex[perimeter].Y = center.Y + radius_y * sin(0);
	}

	// 背景.
	// 方法) 中心と円周上の各点を結ぶ三角形で塗り潰す.
	/*
		円周上の点の個数が 500 を超えた辺りから設定した描画色に白色が混ざる現象が見られた。
		何かのバグか環境依存かも知れない。
		対症療法となるが glBegin/glEnd の間に描画する点数の上限を設ける事とした。
	*/
	if (this->BkEnable() == true)
	{
		fnPRV_GDI_BkPrepare(&m_Param);

		::glBegin(GL_TRIANGLE_FAN);
		::glVertex2d(center.X, center.Y);
		{
			const int limit = 36;
			const int overlap = 1;	// must be 1.
			for(int pp=0 ; pp<vertex.Length() ; pp++)
			{
				// 描画する点数の上限のチェック.
				if (pp != 0 && pp % limit == 0)
				{
					::glEnd();
					::glBegin(GL_TRIANGLE_FAN);
					::glVertex2d(center.X, center.Y);

					// オーバーラップ.
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
		::glEnd();

		fnPRV_GDI_BkRestore(&m_Param);
	}

	// 枠線.
	if (this->PenStyle() != ExGdiPenStyle::None)
	{
		fnPRV_GDI_PenPrepare(&m_Param);
		::glBegin(GL_LINE_LOOP);
		for(int pp=0 ; pp<vertex.Length() ; pp++)
		{
			::glVertex2d(vertex[pp].X, vertex[pp].Y);
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
TxPointD CxGdiEllipse::Location() const
{
	return TxPointD(Shape.X, Shape.Y);
}

// ================================================================================
void CxGdiEllipse::Location(TxPointD value)
{
	Shape.X = value.X;
	Shape.Y = value.Y;
}

// ================================================================================
TxRectangleD CxGdiEllipse::Bounds() const
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
double CxGdiEllipse::Angle() const
{
	return m_Param.Angle;
}

// ============================================================
void CxGdiEllipse::Angle( double degree )
{
	m_Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiEllipse::Axis() const
{
	return m_Param.Axis;
}

// ============================================================
void CxGdiEllipse::Axis( TxPointD value )
{
	m_Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiEllipse::HitTest( TxPointD position, double margin ) const
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
void CxGdiEllipse::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiEllipse>(prev_figure))
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
TxRGB8x4 CxGdiEllipse::BkColor() const
{
	return m_Param.BkColor;
}

// ============================================================
void CxGdiEllipse::BkColor( TxRGB8x4 value )
{
	m_Param.BkColor = value;
}

// ============================================================
bool CxGdiEllipse::BkEnable() const
{
	return (m_Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiEllipse::BkEnable( bool value )
{
	m_Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiEllipse::PenColor() const
{
	return m_Param.PenColor;
}

// ============================================================
void CxGdiEllipse::PenColor( TxRGB8x4 value )
{
	m_Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiEllipse::PenStyle() const
{
	return m_Param.PenStyle;
}

// ============================================================
void CxGdiEllipse::PenStyle( ExGdiPenStyle value )
{
	m_Param.PenStyle = value;
}

// ============================================================
int CxGdiEllipse::PenWidth() const
{
	return m_Param.PenWidth;
}

// ============================================================
void CxGdiEllipse::PenWidth( int value )
{
	m_Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiEllipse::Param() const
{
	return m_Param;
}

// ============================================================
void CxGdiEllipse::Param( const TxGdi2dParam& value )
{
	m_Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
double CxGdiEllipse::X() const
{
	return Shape.X;
}

// ============================================================
void CxGdiEllipse::X(double value)
{
	Shape.X = value;
}

// ============================================================
double CxGdiEllipse::Y() const
{
	return Shape.Y;
}

// ============================================================
void CxGdiEllipse::Y(double value)
{
	Shape.Y = value;
}

// ============================================================
double CxGdiEllipse::RadiusX() const
{
	return Shape.RadiusX;
}

// ============================================================
void CxGdiEllipse::RadiusX(double value)
{
	Shape.RadiusX = value;
}

// ============================================================
double CxGdiEllipse::RadiusY() const
{
	return Shape.RadiusY;
}

// ============================================================
void CxGdiEllipse::RadiusY(double value)
{
	Shape.RadiusY = value;
}

// ============================================================
void CxGdiEllipse::GetParam(TxCharCPtrA name, void* value, TxModel model) const
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
void CxGdiEllipse::SetParam(TxCharCPtrA name, const void* value, TxModel model)
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
