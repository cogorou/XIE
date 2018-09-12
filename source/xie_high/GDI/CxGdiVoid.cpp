/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiVoid.h"
#include "Core/CxException.h"
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

static const char* g_ClassName = "CxGdiVoid";

// ================================================================================
void CxGdiVoid::_Constructor()
{
	m_Param = TxGdi2dParam::Default();
}

// ================================================================================
CxGdiVoid::CxGdiVoid()
{
	_Constructor();
}

// ================================================================================
CxGdiVoid::CxGdiVoid( const CxGdiVoid& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiVoid::~CxGdiVoid()
{
}

// ================================================================================
CxGdiVoid& CxGdiVoid::operator = ( const CxGdiVoid& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxGdiVoid::operator == ( const CxGdiVoid& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiVoid::operator != ( const CxGdiVoid& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
CxGdiVoid CxGdiVoid::Clone() const
{
	CxGdiVoid clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiVoid::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiVoid>(src))
	{
		auto&	_src = static_cast<const CxGdiVoid&>(src);
		auto&	_dst = *this;
		_dst.m_Param = _src.m_Param;
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxGdiVoid::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiVoid>(src))
	{
		auto&	_src = static_cast<const CxGdiVoid&>(src);
		auto&	_dst = *this;
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
void CxGdiVoid::Render(HxModule hcanvas, ExGdiScalingMode mode) const
{
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
TxPointD CxGdiVoid::Location() const
{
	return TxPointD();
}

// ================================================================================
void CxGdiVoid::Location(TxPointD value)
{
}

// ================================================================================
TxRectangleD CxGdiVoid::Bounds() const
{
	return TxRectangleD();
}

// ============================================================
double CxGdiVoid::Angle() const
{
	return m_Param.Angle;
}

// ============================================================
void CxGdiVoid::Angle( double degree )
{
	m_Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiVoid::Axis() const
{
	return m_Param.Axis;
}

// ============================================================
void CxGdiVoid::Axis( TxPointD value )
{
	m_Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiVoid::HitTest( TxPointD position, double margin ) const
{
	return TxHitPosition::Default();
}

// ================================================================================
void CxGdiVoid::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiVoid>(prev_figure))
	{
	}
	throw xie::CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ============================================================
TxRGB8x4 CxGdiVoid::BkColor() const
{
	return m_Param.BkColor;
}

// ============================================================
void CxGdiVoid::BkColor( TxRGB8x4 value )
{
	m_Param.BkColor = value;
}

// ============================================================
bool CxGdiVoid::BkEnable() const
{
	return (m_Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiVoid::BkEnable( bool value )
{
	m_Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiVoid::PenColor() const
{
	return m_Param.PenColor;
}

// ============================================================
void CxGdiVoid::PenColor( TxRGB8x4 value )
{
	m_Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiVoid::PenStyle() const
{
	return m_Param.PenStyle;
}

// ============================================================
void CxGdiVoid::PenStyle( ExGdiPenStyle value )
{
	m_Param.PenStyle = value;
}

// ============================================================
int CxGdiVoid::PenWidth() const
{
	return m_Param.PenWidth;
}

// ============================================================
void CxGdiVoid::PenWidth( int value )
{
	m_Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiVoid::Param() const
{
	return m_Param;
}

// ============================================================
void CxGdiVoid::Param( const TxGdi2dParam& value )
{
	m_Param = value;
}

// ============================================================
void CxGdiVoid::GetParam(TxCharCPtrA name, void* value, TxModel model) const
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
void CxGdiVoid::SetParam(TxCharCPtrA name, const void* value, TxModel model)
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
