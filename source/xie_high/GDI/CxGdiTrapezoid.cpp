/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiTrapezoid.h"
#include "Core/CxException.h"
#include "Core/Axi.h"
#include "Core/CxArrayEx.h"
#include "Core/IxParam.h"
#include "Core/TxStatistics.h"

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

static const char* g_ClassName = "CxGdiTrapezoid";

// ================================================================================
void CxGdiTrapezoid::_Constructor()
{
	m_Param = TxGdi2dParam::Default();
	Shape = TxTrapezoidD::Default();
}

// ================================================================================
CxGdiTrapezoid::CxGdiTrapezoid()
{
	_Constructor();
}

// ================================================================================
CxGdiTrapezoid::CxGdiTrapezoid( CxGdiTrapezoid&& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiTrapezoid::CxGdiTrapezoid( const CxGdiTrapezoid& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiTrapezoid::CxGdiTrapezoid( const TxTrapezoidD& shape )
{
	_Constructor();
	Shape = shape;
}

// ================================================================================
CxGdiTrapezoid::CxGdiTrapezoid( const TxTrapezoidI& shape )
{
	_Constructor();
	Shape = shape;
}

// ============================================================
CxGdiTrapezoid::CxGdiTrapezoid(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
{
	_Constructor();
	Shape.X1 = x1;
	Shape.Y1 = y1;
	Shape.X2 = x2;
	Shape.Y2 = y2;
	Shape.X3 = x3;
	Shape.Y3 = y3;
	Shape.X4 = x4;
	Shape.Y4 = y4;
}

// ============================================================
CxGdiTrapezoid::CxGdiTrapezoid(TxPointD p1, TxPointD p2, TxPointD p3, TxPointD p4)
{
	_Constructor();
	Shape.X1 = p1.X;
	Shape.Y1 = p1.Y;
	Shape.X2 = p2.X;
	Shape.Y2 = p2.Y;
	Shape.X3 = p3.X;
	Shape.Y3 = p3.Y;
	Shape.X4 = p4.X;
	Shape.Y4 = p4.Y;
}

// ================================================================================
CxGdiTrapezoid::~CxGdiTrapezoid()
{
}

// ============================================================
CxGdiTrapezoid& CxGdiTrapezoid::operator = ( CxGdiTrapezoid&& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiTrapezoid& CxGdiTrapezoid::operator = ( const CxGdiTrapezoid& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxGdiTrapezoid::operator == ( const CxGdiTrapezoid& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiTrapezoid::operator != ( const CxGdiTrapezoid& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxTrapezoidD CxGdiTrapezoid::Tag() const
{
	return Shape;
}

// ============================================================
void* CxGdiTrapezoid::TagPtr() const
{
	return (void*)&Shape;
}

// ============================================================
CxGdiTrapezoid& CxGdiTrapezoid::operator = (const TxTrapezoidD& src)
{
	Shape = src;
	return *this;
}

// ============================================================
CxGdiTrapezoid& CxGdiTrapezoid::operator = (const TxTrapezoidI& src)
{
	Shape = (TxTrapezoidD)src;
	return *this;
}

// ================================================================================
CxGdiTrapezoid::operator TxTrapezoidD() const
{
	return Shape;
}

// ================================================================================
CxGdiTrapezoid::operator TxTrapezoidI() const
{
	return (TxTrapezoidI)Shape;
}

// ================================================================================
CxGdiTrapezoid CxGdiTrapezoid::Clone() const
{
	CxGdiTrapezoid clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiTrapezoid::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiTrapezoid>(src))
	{
		auto&	_src = static_cast<const CxGdiTrapezoid&>(src);
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
bool CxGdiTrapezoid::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiTrapezoid>(src))
	{
		auto&	_src = static_cast<const CxGdiTrapezoid&>(src);
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
void CxGdiTrapezoid::Render(HxModule hcanvas, ExGdiScalingMode mode) const
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

	// 背景.
	if (this->BkEnable() == true)
	{
		fnPRV_GDI_BkPrepare(&m_Param);

		::glBegin(GL_TRIANGLE_FAN);
		::glVertex2d( Shape.X1, Shape.Y1 );
		::glVertex2d( Shape.X2, Shape.Y2 );
		::glVertex2d( Shape.X3, Shape.Y3 );
		::glVertex2d( Shape.X4, Shape.Y4 );
		::glEnd();

		fnPRV_GDI_BkRestore(&m_Param);
	}

	// 枠線.
	if (this->PenStyle() != ExGdiPenStyle::None)
	{
		fnPRV_GDI_PenPrepare(&m_Param);

		::glBegin(GL_LINE_LOOP);
		::glVertex2d( Shape.X1, Shape.Y1 );
		::glVertex2d( Shape.X2, Shape.Y2 );
		::glVertex2d( Shape.X3, Shape.Y3 );
		::glVertex2d( Shape.X4, Shape.Y4 );
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
TxPointD CxGdiTrapezoid::Location() const
{
	return this->Vertex1();
}

// ================================================================================
void CxGdiTrapezoid::Location(TxPointD value)
{
	auto diff = value - this->Vertex1();

	Shape.X1 += diff.X;
	Shape.X2 += diff.X;
	Shape.X3 += diff.X;
	Shape.X4 += diff.X;

	Shape.Y1 += diff.Y;
	Shape.Y2 += diff.Y;
	Shape.Y3 += diff.Y;
	Shape.Y4 += diff.Y;
}

// ================================================================================
TxRectangleD CxGdiTrapezoid::Bounds() const
{
	TxStatistics stat_x;
	TxStatistics stat_y;

	stat_x += Shape.X1;
	stat_x += Shape.X2;
	stat_x += Shape.X3;
	stat_x += Shape.X4;

	stat_y += Shape.Y1;
	stat_y += Shape.Y2;
	stat_y += Shape.Y3;
	stat_y += Shape.Y4;

	auto sx = stat_x.Min;
	auto sy = stat_y.Min;
	auto ex = stat_x.Max;
	auto ey = stat_y.Max;

	return TxRectangleD(sx, sy, (ex - sx), (ey - sy));
}

// ============================================================
double CxGdiTrapezoid::Angle() const
{
	return m_Param.Angle;
}

// ============================================================
void CxGdiTrapezoid::Angle( double degree )
{
	m_Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiTrapezoid::Axis() const
{
	return m_Param.Axis;
}

// ============================================================
void CxGdiTrapezoid::Axis( TxPointD value )
{
	m_Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiTrapezoid::HitTest( TxPointD position, double margin ) const
{
	auto p0 = xie::Axi::Rotate<TxPointD>(position, Axis(), -Angle());

	TxPointD vertex[] = 
	{
		this->Vertex1(),
		this->Vertex2(),
		this->Vertex3(),
		this->Vertex4(),
		this->Vertex1(),
	};

	// 頂点と辺の判定:
	{
		for(int j=0 ; j<4 ; j++)
		{
			TxLineSegmentD ls(
					vertex[j+0].X,
					vertex[j+0].Y,
					vertex[j+1].X,
					vertex[j+1].Y
					);
			auto hit = fnPRV_GDI_HitTest_LineSegment(p0, margin, ls);
			switch(hit.Mode)
			{
			case 2:		// 端点.
				if (hit.Site == 1)	// 始点.
					return TxHitPosition(2, 0, +(j+1));
				break;
			case 1:		// 辺上.
				return TxHitPosition(2, 0, -(j+1));
			}
		}
	}

	// 内外判定:
	{
		auto bounds = this->Bounds();
		auto hit = fnPRV_GDI_HitTest_Rectangle(p0, margin, bounds);
		if (hit.Mode != 0)
			return TxHitPosition(1, 0, 0);
	}

	return TxHitPosition::Default();
}

// ================================================================================
void CxGdiTrapezoid::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiTrapezoid>(prev_figure))
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
							this->X1( figure->X1() + mv.X );
							this->X4( figure->X4() + mv.X );
							break;
						case -1:
							this->Y1( figure->Y1() + mv.Y );
							this->Y2( figure->Y2() + mv.Y );
							break;
						case -2:
							this->X2( figure->X2() + mv.X );
							this->X3( figure->X3() + mv.X );
							break;
						case -3:
							this->Y3( figure->Y3() + mv.Y );
							this->Y4( figure->Y4() + mv.Y );
							break;
						case +1:
							this->X1( figure->X1() + mv.X );
							this->Y1( figure->Y1() + mv.Y );
							break;
						case +2:
							this->X2( figure->X2() + mv.X );
							this->Y2( figure->Y2() + mv.Y );
							break;
						case +3:
							this->X3( figure->X3() + mv.X );
							this->Y3( figure->Y3() + mv.Y );
							break;
						case +4:
							this->X4( figure->X4() + mv.X );
							this->Y4( figure->Y4() + mv.Y );
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
TxRGB8x4 CxGdiTrapezoid::BkColor() const
{
	return m_Param.BkColor;
}

// ============================================================
void CxGdiTrapezoid::BkColor( TxRGB8x4 value )
{
	m_Param.BkColor = value;
}

// ============================================================
bool CxGdiTrapezoid::BkEnable() const
{
	return (m_Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiTrapezoid::BkEnable( bool value )
{
	m_Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiTrapezoid::PenColor() const
{
	return m_Param.PenColor;
}

// ============================================================
void CxGdiTrapezoid::PenColor( TxRGB8x4 value )
{
	m_Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiTrapezoid::PenStyle() const
{
	return m_Param.PenStyle;
}

// ============================================================
void CxGdiTrapezoid::PenStyle( ExGdiPenStyle value )
{
	m_Param.PenStyle = value;
}

// ============================================================
int CxGdiTrapezoid::PenWidth() const
{
	return m_Param.PenWidth;
}

// ============================================================
void CxGdiTrapezoid::PenWidth( int value )
{
	m_Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiTrapezoid::Param() const
{
	return m_Param;
}

// ============================================================
void CxGdiTrapezoid::Param( const TxGdi2dParam& value )
{
	m_Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// METHOD
// 

// ============================================================
double CxGdiTrapezoid::X1() const
{
	return Shape.X1;
}

// ============================================================
void CxGdiTrapezoid::X1(double value)
{
	Shape.X1 = value;
}

// ============================================================
double CxGdiTrapezoid::Y1() const
{
	return Shape.Y1;
}

// ============================================================
void CxGdiTrapezoid::Y1(double value)
{
	Shape.Y1 = value;
}

// ============================================================
double CxGdiTrapezoid::X2() const
{
	return Shape.X2;
}

// ============================================================
void CxGdiTrapezoid::X2(double value)
{
	Shape.X2 = value;
}

// ============================================================
double CxGdiTrapezoid::Y2() const
{
	return Shape.Y2;
}

// ============================================================
void CxGdiTrapezoid::Y2(double value)
{
	Shape.Y2 = value;
}

// ============================================================
double CxGdiTrapezoid::X3() const
{
	return Shape.X3;
}

// ============================================================
void CxGdiTrapezoid::X3(double value)
{
	Shape.X3 = value;
}

// ============================================================
double CxGdiTrapezoid::Y3() const
{
	return Shape.Y3;
}

// ============================================================
void CxGdiTrapezoid::Y3(double value)
{
	Shape.Y3 = value;
}

// ============================================================
double CxGdiTrapezoid::X4() const
{
	return Shape.X4;
}

// ============================================================
void CxGdiTrapezoid::X4(double value)
{
	Shape.X4 = value;
}

// ============================================================
double CxGdiTrapezoid::Y4() const
{
	return Shape.Y4;
}

// ============================================================
void CxGdiTrapezoid::Y4(double value)
{
	Shape.Y4 = value;
}

// ============================================================
TxPointD CxGdiTrapezoid::Vertex1() const
{
	return TxPointD(Shape.X1, Shape.Y1);
}

// ============================================================
void CxGdiTrapezoid::Vertex1(const TxPointD& value)
{
	Shape.X1 = value.X;
	Shape.Y1 = value.Y;
}

// ============================================================
TxPointD CxGdiTrapezoid::Vertex2() const
{
	return TxPointD(Shape.X2, Shape.Y2);
}

// ============================================================
void CxGdiTrapezoid::Vertex2(const TxPointD& value)
{
	Shape.X2 = value.X;
	Shape.Y2 = value.Y;
}

// ============================================================
TxPointD CxGdiTrapezoid::Vertex3() const
{
	return TxPointD(Shape.X3, Shape.Y3);
}

// ============================================================
void CxGdiTrapezoid::Vertex3(const TxPointD& value)
{
	Shape.X3 = value.X;
	Shape.Y3 = value.Y;
}

// ============================================================
TxPointD CxGdiTrapezoid::Vertex4() const
{
	return TxPointD(Shape.X4, Shape.Y4);
}

// ============================================================
void CxGdiTrapezoid::Vertex4(const TxPointD& value)
{
	Shape.X4 = value.X;
	Shape.Y4 = value.Y;
}

// ============================================================
void CxGdiTrapezoid::GetParam(TxCharCPtrA name, void* value, TxModel model) const
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
void CxGdiTrapezoid::SetParam(TxCharCPtrA name, const void* value, TxModel model)
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
