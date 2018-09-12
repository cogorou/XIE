/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxOverlayGrid.h"
#include "Core/CxException.h"
#include "Core/Axi.h"
#include "GDI/CxCanvas.h"
#include "GDI/CxGdiLineSegment.h"

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

static const char* g_ClassName = "CxOverlayGrid";

// ================================================================================
void CxOverlayGrid::_Constructor()
{
}

// ================================================================================
CxOverlayGrid::CxOverlayGrid() : CxOverlay()
{
	_Constructor();
}

// ================================================================================
CxOverlayGrid::CxOverlayGrid( const CxOverlayGrid& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxOverlayGrid::~CxOverlayGrid()
{
}

// ================================================================================
CxOverlayGrid& CxOverlayGrid::operator = ( const CxOverlayGrid& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxOverlayGrid::operator == ( const CxOverlayGrid& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxOverlayGrid::operator != ( const CxOverlayGrid& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
CxOverlayGrid CxOverlayGrid::Clone() const
{
	CxOverlayGrid clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

//
// IxEquatable
//

// ================================================================================
void CxOverlayGrid::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxOverlayGrid>(src))
	{
		auto&	_src = static_cast<const CxOverlayGrid&>(src);
		auto&	_dst = *this;
		CxOverlay::CopyFrom(src);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxOverlayGrid::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxOverlayGrid>(src))
	{
		auto&	_src = static_cast<const CxOverlayGrid&>(src);
		auto&	_dst = *this;
		return CxOverlay::ContentEquals(src);
	}
	return false;
}

//
// IxParam
//

// ============================================================
void CxOverlayGrid::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	CxOverlay::GetParam(name, value, model);
}

// ============================================================
void CxOverlayGrid::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	CxOverlay::SetParam(name, value, model);
}

// ////////////////////////////////////////////////////////////
// 
// Overlay Inheritance
// 

// ================================================================================
void CxOverlayGrid::Render(HxModule hcanvas, ExGdiScalingMode mode) const
{
	if (m_Visible == false) return;

	if (auto canvas = xie::Axi::SafeCast<CxCanvas>(hcanvas))
	{
		auto mag = canvas->Magnification();
		if (mag >= 4.0)
		{
			auto size = canvas->BgSize();
			auto black = TxRGB8x3(0x00, 0x00, 0x00);
			auto gray = TxRGB8x3(0x3F, 0x3F, 0x3F);

			// Vertical Line (Black)
			{
				CxArrayEx<CxGdiLineSegment> figures(size.Width);
				for(int x=0 ; x<figures.Length() ; x++)
				{
					auto& figure = figures[x];
					figure = TxLineSegmentD(x, 0, x, size.Height);
					figure.PenColor(black);
				}
				canvas->DrawOverlay(figures, mode);
			}
			// Vertical Line (Gray)
			{
				CxArrayEx<CxGdiLineSegment> figures(size.Width);
				for(int x=0 ; x<figures.Length() ; x++)
				{
					auto& figure = figures[x];
					figure = TxLineSegmentD(x, 0, x, size.Height);
					figure.PenColor(gray);
					figure.PenStyle(ExGdiPenStyle::Dot);
				}
				canvas->DrawOverlay(figures, mode);
			}
			// Horizontal Line (Black)
			{
				CxArrayEx<CxGdiLineSegment> figures(size.Height);
				for(int y=0 ; y<figures.Length() ; y++)
				{
					auto& figure = figures[y];
					figure = TxLineSegmentD(0, y, size.Width, y);
					figure.PenColor(black);
				}
				canvas->DrawOverlay(figures, mode);
			}
			// Horizontal Line (Gray)
			{
				CxArrayEx<CxGdiLineSegment> figures(size.Height);
				for(int y=0 ; y<figures.Length() ; y++)
				{
					auto& figure = figures[y];
					figure = TxLineSegmentD(0, y, size.Width, y);
					figure.PenColor(gray);
					figure.PenStyle(ExGdiPenStyle::Dot);
				}
				canvas->DrawOverlay(figures, mode);
			}
		}
		return;
	}
}

}	// GDI
}	// xie
