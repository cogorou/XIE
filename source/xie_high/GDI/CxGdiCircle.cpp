/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiCircle.h"
#include "Core/CxException.h"
#include "Core/Axi.h"
#include "Core/CxArrayEx.h"
#include "Core/IxParam.h"
#include "Core/CxStopwatch.h"
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

static const char* g_ClassName = "CxGdiCircle";

// ================================================================================
void CxGdiCircle::_Constructor()
{
	m_Param = TxGdi2dParam::Default();
	Shape = TxCircleD::Default();
}

// ================================================================================
CxGdiCircle::CxGdiCircle()
{
	_Constructor();
}

// ================================================================================
CxGdiCircle::CxGdiCircle( CxGdiCircle&& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiCircle::CxGdiCircle( const CxGdiCircle& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiCircle::CxGdiCircle( const TxCircleD& shape )
{
	_Constructor();
	Shape = shape;
}

// ================================================================================
CxGdiCircle::CxGdiCircle( const TxCircleI& shape )
{
	_Constructor();
	Shape = shape;
}

// ============================================================
CxGdiCircle::CxGdiCircle(double x, double y, double radius)
{
	_Constructor();
	Shape.X = x;
	Shape.Y = y;
	Shape.Radius = radius;
}

// ============================================================
CxGdiCircle::CxGdiCircle(TxPointD center, double radius)
{
	_Constructor();
	Shape.X = center.X;
	Shape.Y = center.Y;
	Shape.Radius = radius;
}

// ================================================================================
CxGdiCircle::~CxGdiCircle()
{
}

// ============================================================
CxGdiCircle& CxGdiCircle::operator = ( CxGdiCircle&& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiCircle& CxGdiCircle::operator = ( const CxGdiCircle& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxGdiCircle::operator == ( const CxGdiCircle& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiCircle::operator != ( const CxGdiCircle& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxCircleD CxGdiCircle::Tag() const
{
	return Shape;
}

// ============================================================
void* CxGdiCircle::TagPtr() const
{
	return (void*)&Shape;
}

// ============================================================
CxGdiCircle& CxGdiCircle::operator = ( const TxCircleD& src )
{
	Shape = src;
	return *this;
}

// ============================================================
CxGdiCircle& CxGdiCircle::operator = ( const TxCircleI& src )
{
	Shape = (TxCircleD)src;
	return *this;
}

// ================================================================================
CxGdiCircle::operator TxCircleD() const
{
	return Shape;
}

// ================================================================================
CxGdiCircle::operator TxCircleI() const
{
	return (TxCircleI)Shape;
}

// ================================================================================
CxGdiCircle CxGdiCircle::Clone() const
{
	CxGdiCircle clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiCircle::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiCircle>(src))
	{
		auto&	_src = static_cast<const CxGdiCircle&>(src);
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
bool CxGdiCircle::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiCircle>(src))
	{
		auto&	_src = static_cast<const CxGdiCircle&>(src);
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
void CxGdiCircle::Render(HxModule hcanvas, ExGdiScalingMode mode) const
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
	auto radius_x = Shape.Radius;
	auto radius_y = Shape.Radius;
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
TxPointD CxGdiCircle::Location() const
{
	return TxPointD(Shape.X, Shape.Y);
}

// ================================================================================
void CxGdiCircle::Location(TxPointD value)
{
	Shape.X = value.X;
	Shape.Y = value.Y;
}

// ================================================================================
TxRectangleD CxGdiCircle::Bounds() const
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
double CxGdiCircle::Angle() const
{
	return m_Param.Angle;
}

// ============================================================
void CxGdiCircle::Angle( double degree )
{
	m_Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiCircle::Axis() const
{
	return m_Param.Axis;
}

// ============================================================
void CxGdiCircle::Axis( TxPointD value )
{
	m_Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiCircle::HitTest( TxPointD position, double margin ) const
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
void CxGdiCircle::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiCircle>(prev_figure))
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
TxRGB8x4 CxGdiCircle::BkColor() const
{
	return m_Param.BkColor;
}

// ============================================================
void CxGdiCircle::BkColor( TxRGB8x4 value )
{
	m_Param.BkColor = value;
}

// ============================================================
bool CxGdiCircle::BkEnable() const
{
	return (m_Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiCircle::BkEnable( bool value )
{
	m_Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiCircle::PenColor() const
{
	return m_Param.PenColor;
}

// ============================================================
void CxGdiCircle::PenColor( TxRGB8x4 value )
{
	m_Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiCircle::PenStyle() const
{
	return m_Param.PenStyle;
}

// ============================================================
void CxGdiCircle::PenStyle( ExGdiPenStyle value )
{
	m_Param.PenStyle = value;
}

// ============================================================
int CxGdiCircle::PenWidth() const
{
	return m_Param.PenWidth;
}

// ============================================================
void CxGdiCircle::PenWidth( int value )
{
	m_Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiCircle::Param() const
{
	return m_Param;
}

// ============================================================
void CxGdiCircle::Param( const TxGdi2dParam& value )
{
	m_Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
double CxGdiCircle::X() const
{
	return Shape.X;
}

// ============================================================
void CxGdiCircle::X(double value)
{
	Shape.X = value;
}

// ============================================================
double CxGdiCircle::Y() const
{
	return Shape.Y;
}

// ============================================================
void CxGdiCircle::Y(double value)
{
	Shape.Y = value;
}

// ============================================================
double CxGdiCircle::Radius() const
{
	return Shape.Radius;
}

// ============================================================
void CxGdiCircle::Radius(double value)
{
	Shape.Radius = value;
}

// ============================================================
void CxGdiCircle::GetParam(TxCharCPtrA name, void* value, TxModel model) const
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
void CxGdiCircle::SetParam(TxCharCPtrA name, const void* value, TxModel model)
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
