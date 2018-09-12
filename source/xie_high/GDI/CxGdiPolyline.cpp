/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiPolyline.h"
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

static const char* g_ClassName = "CxGdiPolyline";

// ================================================================================
void CxGdiPolyline::_Constructor()
{
	m_Tag.Param = TxGdi2dParam::Default();
	m_Tag = TxGdiPolyline::Default();
	m_IsAttached = false;
}

// ================================================================================
CxGdiPolyline::CxGdiPolyline()
{
	_Constructor();
}

// ================================================================================
CxGdiPolyline::CxGdiPolyline( CxGdiPolyline&& src )
{
	_Constructor();
	MoveFrom(src);
}

// ================================================================================
CxGdiPolyline::CxGdiPolyline( const CxGdiPolyline& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiPolyline::CxGdiPolyline(int length)
{
	_Constructor();
	Resize(length);
}

// ================================================================================
CxGdiPolyline::CxGdiPolyline(const CxArray& src)
{
	_Constructor();
	operator = (src);
}

// ================================================================================
CxGdiPolyline::~CxGdiPolyline()
{
	Dispose();
}

// ============================================================
CxGdiPolyline& CxGdiPolyline::operator = (const CxArray& src)
{
	CopyFrom(src);
	return *this;
}

// ============================================================
CxGdiPolyline& CxGdiPolyline::operator = (const xie::CxArrayEx<TE>& src)
{
	Resize(src.Length());
	Scanner().Copy(src);
	return *this;
}

// ============================================================
CxGdiPolyline& CxGdiPolyline::operator = ( CxGdiPolyline&& src )
{
	MoveFrom(src);
	return *this;
}

// ================================================================================
CxGdiPolyline& CxGdiPolyline::operator = ( const CxGdiPolyline& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxGdiPolyline::operator == ( const CxGdiPolyline& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiPolyline::operator != ( const CxGdiPolyline& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxGdiPolyline CxGdiPolyline::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxGdiPolyline::TagPtr() const
{
	return (void*)&m_Tag;
}

// ================================================================================
CxGdiPolyline::operator CxArray() const
{
	CxArray dst;
	this->CopyTo(dst);
	return dst;
}

// ================================================================================
void CxGdiPolyline::CopyTo(IxModule& dst) const
{
	if (xie::Axi::ClassIs<CxGdiPolyline>(dst))
	{
		CxArray		_src; _src.Attach(this->Tag());
		CxArray&	_dst = static_cast<CxArray&>(dst);

		if (_src.IsValid() == false)
			_dst.Dispose();
		else
			_dst.CopyFrom(_src);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
CxGdiPolyline CxGdiPolyline::Clone() const
{
	CxGdiPolyline clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiPolyline::Dispose()
{
	if (IsAttached() == false)
	{
		if (m_Tag.Address != NULL)
			xie::Axi::MemoryFree(m_Tag.Address);
	}
	m_Tag.Address = NULL;
	m_Tag.Length = 0;
	m_Tag.Model = TxModel::Default();
	m_IsAttached = false;
}

// ================================================================================
void CxGdiPolyline::MoveFrom(CxGdiPolyline& src)
{
	if (this == &src) return;

	CxGdiPolyline& dst = *this;
	dst.Dispose();
	dst.m_Tag.Param		= src.m_Tag.Param;
	dst.m_Tag			= src.m_Tag;
	dst.m_IsAttached	= src.m_IsAttached;
	src.m_IsAttached	= true;
}

// ================================================================================
void CxGdiPolyline::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiPolyline>(src))
	{
		auto&	_src = static_cast<const CxGdiPolyline&>(src);
		auto&	_dst = *this;

		_dst.m_Tag.Param = _src.m_Tag.Param;

		// 配列要素の複製.
	//	_dst.m_Tag.Address	= _src.m_Tag.Address;
	//	_dst.m_Tag.Length	= _src.m_Tag.Length;
	//	_dst.m_Tag.Model	= _src.m_Tag.Model;

		Resize(_src.m_Tag.Length);
		if (_dst.m_Tag.Address != NULL)
		{
			size_t	size = _dst.m_Tag.Model.Size();
			size_t	length = (size_t)_dst.m_Tag.Length;
			size_t	stride = size * length;
			memcpy(_dst.m_Tag.Address, _src.m_Tag.Address, stride);
		}

		_dst.m_Tag.Closed = _src.m_Tag.Closed;
		return;
	}
	if (xie::Axi::ClassIs<CxArray>(src))
	{
		const CxArray&	_src = static_cast<const CxArray&>(src);
		CxGdiPolyline&	_dst = *this;

		_dst.Resize(_src.Length());
		if (_dst.IsValid())
		{
			CxArray _act;
			_act.Attach(_dst.Tag());
			_act.Filter().Copy(_src);
		}
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
bool CxGdiPolyline::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiPolyline>(src))
	{
		auto&	_src = static_cast<const CxGdiPolyline&>(src);
		auto&	_dst = *this;

		if (_dst.m_Tag.Param != _src.m_Tag.Param) return false;

		if (_dst.m_Tag.Length	!= _src.m_Tag.Length) return false;
		if (_dst.m_Tag.Model	!= _src.m_Tag.Model) return false;

		// 配列要素の比較.
		if (_dst.m_Tag.Address == NULL && _src.m_Tag.Address != NULL) return false;
		if (_dst.m_Tag.Address != NULL && _src.m_Tag.Address == NULL) return false;
		if (_dst.m_Tag.Address != NULL && _src.m_Tag.Address != NULL)
		{
			size_t	size = _dst.m_Tag.Model.Size();
			size_t	length = (size_t)_dst.m_Tag.Length;
			size_t	stride = size * length;
			if (memcmp(_dst.m_Tag.Address, _src.m_Tag.Address, stride) != 0) return false;
		}

		return true;
	}
	return false;
}

// ============================================================
bool CxGdiPolyline::IsValid() const
{
	if (m_Tag.Address == NULL) return false;
	if (m_Tag.Length <= 0) return false;
	if (m_Tag.Model != ModelOf<TE>()) return false;
	return true;
}

// ============================================================
bool CxGdiPolyline::IsAttached() const
{
	return m_IsAttached;
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
void CxGdiPolyline::Render(HxModule hcanvas, ExGdiScalingMode mode) const
{
	if (this->IsValid() == false) return;

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

	bool	closed = this->Closed();
	int		length = this->Length();
	const TxPointD*	addr = this->Address();

	// 背景.
	if (this->BkEnable() == true && closed)
	{
		fnPRV_GDI_BkPrepare(&m_Tag.Param);

		::glBegin(GL_POLYGON);
		for(int i=0 ; i<length ; i++)
		{
			::glVertex2d(addr[i].X, addr[i].Y);
		}
		::glEnd();

		fnPRV_GDI_BkRestore(&m_Tag.Param);
	}

	// 枠線.
	if (this->PenStyle() != ExGdiPenStyle::None)
	{
		fnPRV_GDI_PenPrepare(&m_Tag.Param);

		::glBegin(closed ? GL_LINE_LOOP : GL_LINE_STRIP);
		for(int i=0 ; i<length ; i++)
		{
			::glVertex2d(addr[i].X, addr[i].Y);
		}
		::glEnd();

		fnPRV_GDI_PenRestore(&m_Tag.Param);
	}

	::glDisable( GL_BLEND );
	::glPopMatrix();
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
TxPointD CxGdiPolyline::Location() const
{
	if (this->IsValid() == false)
		return TxPointD(0, 0);

	return static_cast<TE*>(m_Tag.Address)[0];
}

// ================================================================================
void CxGdiPolyline::Location(TxPointD value)
{
	if (this->IsValid() == false)
		return;

	int			length = this->Length();
	TE*			addr = this->Address();
	TxPointD	diff = value - addr[0];
	for(int i=0 ; i<length ; i++)
		addr[i] += diff;
}

// ================================================================================
TxRectangleD CxGdiPolyline::Bounds() const
{
	if (this->IsValid() == false)
		return TxRectangleD(0, 0, 0, 0);

	int			length = this->Length();
	const TE*	addr = this->Address();

	TxStatistics stat_x;
	TxStatistics stat_y;

	for(int i=0 ; i<length ; i++)
	{
		stat_x += addr[i].X;
		stat_y += addr[i].Y;
	}

	auto sx = stat_x.Min;
	auto sy = stat_y.Min;
	auto ex = stat_x.Max;
	auto ey = stat_y.Max;

	return TxRectangleD(sx, sy, (ex - sx), (ey - sy));
}

// ============================================================
double CxGdiPolyline::Angle() const
{
	return m_Tag.Param.Angle;
}

// ============================================================
void CxGdiPolyline::Angle( double degree )
{
	m_Tag.Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiPolyline::Axis() const
{
	return m_Tag.Param.Axis;
}

// ============================================================
void CxGdiPolyline::Axis( TxPointD value )
{
	m_Tag.Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiPolyline::HitTest( TxPointD position, double margin ) const
{
	if (this->IsValid() == false)
		return TxHitPosition::Default();

	TxPointD	p0 = xie::Axi::Rotate<TxPointD>(position, this->Location()+this->Axis(), -this->Angle());

	int			length = this->Length();
	const TE*	addr = this->Address();

	// 各頂点.
	for(int i=0 ; i<length ; i++)
	{
		if (fnPRV_GDI_HitTest_Point(p0, margin, addr[i]) != 0)
			return TxHitPosition(2, i,  +1);
	}

	// 各辺(頂点間).
	for(int i=0 ; i<length-1 ; i++)
	{
		TxLineSegmentD ls(addr[i], addr[i+1]);
		auto hit = fnPRV_GDI_HitTest_LineSegment(p0, margin, ls);
		if (hit.Mode == 1)
			return TxHitPosition(2, i, -1);
	}

	// 終点と始点の間.
	if (this->Closed() && length >= 3)
	{
		TxLineSegmentD ls(addr[length-1], addr[0]);
		auto hit1 = fnPRV_GDI_HitTest_LineSegment(p0, margin, ls);
		if (hit1.Mode == 1)
			return TxHitPosition(2, length-1, -1);

		TxRectangleD bounds = this->Bounds();
		auto hit2 = fnPRV_GDI_HitTest_Rectangle(p0, margin, bounds);
		if (hit2.Mode != 0)
			return TxHitPosition(1, 0, 0);
	}

	return TxHitPosition::Default();
}

// ================================================================================
void CxGdiPolyline::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiPolyline>(prev_figure))
	{
		if (this->IsValid() && figure->IsValid())
		{
			auto hit = figure->HitTest(prev_position, margin);
			auto mv = move_position - prev_position;

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
						auto _prev_position = xie::Axi::Rotate<TxPointD>(prev_position, figure->Location() + figure->Axis(), -figure->Angle());
						auto _move_position = xie::Axi::Rotate<TxPointD>(move_position, figure->Location() + figure->Axis(), -figure->Angle());
						auto mv = _move_position - _prev_position;

						auto _this_location = this->Location();
						auto min_length = XIE_MIN(this->Length(), figure->Length());
						if (0 <= hit.Index && hit.Index < min_length)
						{
							// 頂点.
							if (hit.Site > 0)
							{
								(*this)[hit.Index] = (*figure)[hit.Index] + mv;
							}
							// 辺.
							else if (hit.Site < 0)
							{
								int index0 = hit.Index;
								int index1 = (hit.Index == (min_length - 1)) ? 0 : hit.Index + 1;
								(*this)[index0] = (*figure)[index0] + mv;
								(*this)[index1] = (*figure)[index1] + mv;
							}
						}
						if (figure->Angle() != 0)
						{
							if (_this_location.X != this->Location().X)
								this->m_Tag.Param.Axis.X = figure->m_Tag.Param.Axis.X - mv.X;
							if (_this_location.Y != this->Location().Y)
								this->m_Tag.Param.Axis.Y = figure->m_Tag.Param.Axis.Y - mv.Y;
						}
					}
					break;
			}
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
TxRGB8x4 CxGdiPolyline::BkColor() const
{
	return m_Tag.Param.BkColor;
}

// ============================================================
void CxGdiPolyline::BkColor( TxRGB8x4 value )
{
	m_Tag.Param.BkColor = value;
}

// ============================================================
bool CxGdiPolyline::BkEnable() const
{
	return (m_Tag.Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiPolyline::BkEnable( bool value )
{
	m_Tag.Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiPolyline::PenColor() const
{
	return m_Tag.Param.PenColor;
}

// ============================================================
void CxGdiPolyline::PenColor( TxRGB8x4 value )
{
	m_Tag.Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiPolyline::PenStyle() const
{
	return m_Tag.Param.PenStyle;
}

// ============================================================
void CxGdiPolyline::PenStyle( ExGdiPenStyle value )
{
	m_Tag.Param.PenStyle = value;
}

// ============================================================
int CxGdiPolyline::PenWidth() const
{
	return m_Tag.Param.PenWidth;
}

// ============================================================
void CxGdiPolyline::PenWidth( int value )
{
	m_Tag.Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiPolyline::Param() const
{
	return m_Tag.Param;
}

// ============================================================
void CxGdiPolyline::Param( const TxGdi2dParam& value )
{
	m_Tag.Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ================================================================================
void CxGdiPolyline::Resize(int length)
{
	this->Dispose();

	if (length == 0) return;

	if (length < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	TxModel model = ModelOf<TE>();
	int size = model.Size();
	if (size <= 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	void* addr = xie::Axi::MemoryAlloc( (TxIntPtr)length * size );
	if (addr == NULL)
		throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);

	m_Tag.Address	= addr;
	m_Tag.Length	= length;
	m_Tag.Model		= model;
}

// ============================================================
void CxGdiPolyline::Reset()
{
	if (IsValid() == false) return;
	size_t size = (size_t)m_Tag.Model.Size() * (size_t)m_Tag.Length;
	if (size != 0)
		memset(m_Tag.Address, 0, size);
}

// ============================================================
int CxGdiPolyline::Length() const
{
	return m_Tag.Length;
}

// ============================================================
TxModel CxGdiPolyline::Model() const
{
	return m_Tag.Model;
}

// ============================================================
CxGdiPolyline::TE* CxGdiPolyline::Address()
{
	return static_cast<TE*>(m_Tag.Address);
}

// ============================================================
const CxGdiPolyline::TE* CxGdiPolyline::Address() const
{
	return static_cast<TE*>(m_Tag.Address);
}

// ============================================================
CxGdiPolyline::TE& CxGdiPolyline::operator [] (int index)
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= index && index < m_Tag.Length))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return static_cast<TE*>(m_Tag.Address)[index];
}

// ============================================================
const CxGdiPolyline::TE& CxGdiPolyline::operator [] (int index) const
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= index && index < m_Tag.Length))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return static_cast<TE*>(m_Tag.Address)[index];
}

// ============================================================
bool CxGdiPolyline::Closed() const
{
	return (m_Tag.Closed == ExBoolean::True);
}

// ============================================================
void CxGdiPolyline::Closed(bool value)
{
	m_Tag.Closed = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
void CxGdiPolyline::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL || value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "m_Tag.Param") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, m_Tag.Param)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (fnPRV_Gdi2d_GetParam(name, value, model, m_Tag.Param)) return;

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxGdiPolyline::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL || value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "m_Tag.Param") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, m_Tag.Param)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}
	if (fnPRV_Gdi2d_SetParam(name, value, model, m_Tag.Param)) return;

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

}	// GDI
}	// xie
