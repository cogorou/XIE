/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "GDI/CxGraphics.h"

#include "api_gdi.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxStopwatch.h"
#include "Core/CxImage.h"
#include "Core/CxFinalizer.h"
#include "GDI/IxGdi2d.h"
#include "GDI/CxOverlay.h"
#include "GDI/CxTexture.h"
#include "GDI/CxBitmap.h"
#include "CxGLContext.h"
#include "Core/CxString.h"

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

static const char* g_ClassName = "CxGraphics";

// ============================================================
void CxGraphics::_Constructor()
{
	m_Tag = TxCanvas::Default();

#if defined(_MSC_VER)
	m_DC = NULL;
#else
	m_DC = NULL;
	m_XServer	= NULL;
	m_Context	= NULL;
	m_DrawableID	= 0;
#endif
}

// ======================================================================
CxGraphics::CxGraphics()
{
	_Constructor();
}

// ======================================================================
CxGraphics::CxGraphics(const CxGraphics& src)
{
	_Constructor();
	operator = (src);
}

// ======================================================================
CxGraphics::~CxGraphics()
{
	Dispose();
}

// ============================================================
CxGraphics& CxGraphics::operator = ( const CxGraphics& src )
{
	if (this == &src) return *this;
	CxGraphics& dst = *this;

	Dispose();

#if defined(_MSC_VER)
	if (src.IsValid())
	{
		dst.Setup(src.DC());
	}

	dst.m_Tag = src.m_Tag;
#else
	if (src.IsValid())
	{
		dst.Setup(src.DC());
	}

	dst.m_Tag = src.m_Tag;
	
#endif

	return *this;
}

// ============================================================
bool CxGraphics::operator == ( const CxGraphics& cmp ) const
{
	if (this == &cmp) return true;
	const CxGraphics& src = *this;

	if (src.m_Tag != cmp.m_Tag) return false;

	return true;
}

// ============================================================
bool CxGraphics::operator != ( const CxGraphics& cmp ) const
{
	return !(CxGraphics::operator == (cmp));
}

// ============================================================
TxCanvas CxGraphics::Tag() const
{
	return m_Tag;
}

// ============================================================
void CxGraphics::Tag(TxCanvas value)
{
	m_Tag = value;
}

// ============================================================
void* CxGraphics::TagPtr() const
{
	return (void*)&m_Tag;
}

// ============================================================
void CxGraphics::Dispose()
{
	m_Tag.Width = 0;
	m_Tag.Height = 0;

#if defined(_MSC_VER)
	m_DC = NULL;
#else
	if (m_Context != NULL)
		::glXDestroyContext(m_XServer, m_Context);
	m_Context = NULL;

	m_DrawableID = 0;
	m_XServer = NULL;
	m_DC = NULL;
#endif
}

// ============================================================
bool CxGraphics::IsValid() const
{
	if (m_Tag.Width == 0) return false;
	if (m_Tag.Height == 0) return false;

	return true;
}

#if defined(_MSC_VER)
// ============================================================
bool CxGraphics::CheckValidity(HDC hdc)
{
	if (hdc == NULL) return false;
	return true;
}
#else
// ============================================================
bool CxGraphics::CheckValidity(void* hdc)
{
	if (hdc == NULL) return false;
	auto graphics = (GDIPlus_Graphics*)hdc;
	if (graphics->display == NULL)	return false;
	if (graphics->drawable == 0)	return false;
	return true;
}
#endif

#if defined(_MSC_VER)
// ============================================================
void CxGraphics::Setup(HDC hdc)
{
	Dispose();
	m_DC = hdc;
}
#else
// ============================================================
void CxGraphics::Setup(void* hdc)
{
	Dispose();

	auto graphics = (GDIPlus_Graphics*)hdc;

	if (graphics->display == NULL)
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
	if (graphics->drawable == 0)
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	m_DC			= hdc;
	m_XServer		= graphics->display;
	m_DrawableID	= graphics->drawable;

	XVisualInfo* xvisual = fnPRV_GDI_XVisual_Open(m_XServer);
	if (xvisual == NULL)
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
	CxFinalizer xvisual_finalizer([&xvisual]
	{
		if (xvisual != NULL)
			fnPRV_GDI_XVisual_Close(xvisual);
		xvisual = NULL;
	});

	// create rendering context
	m_Context = ::glXCreateContext(m_XServer, xvisual, NULL, True);
	if (m_Context == NULL)
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	// Initialize glew
	{
		CxGLContext<CxGraphics> gc(this, false);

		GLenum err = ::glewInit();
		if (GLEW_OK != err)
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}
}
#endif

// ======================================================================
void CxGraphics::Resize(const TxSizeI& size)
{
	Resize(size.Width, size.Height);
}

// ======================================================================
void CxGraphics::Resize(int width, int height)
{
	if (width < 0 || height < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	m_Tag.Width		= width;
	m_Tag.Height	= height;
}

// ============================================================
void CxGraphics::DrawImage(HxModule himage)
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	const CxImage* image = xie::Axi::SafeCast<CxImage>(himage);
	if (image == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (image->IsValid() == false) return;

	m_Tag.BgSize = image->Size();

	auto mag = m_Tag.Magnification;
	auto view_point = m_Tag.ViewPoint;
	auto display_rect = m_Tag.DisplayRect();
	auto eff = m_Tag.EffectiveRect();
	auto vis = m_Tag.VisibleRect();

	if (m_Tag.BgSize.Width <= 0 || m_Tag.BgSize.Height <= 0) return;
	if (display_rect.Width <= 0 || display_rect.Height <= 0) return;
	if (eff.Width <= 0 || eff.Height <= 0) return;
	if (vis.Width <= 0 || vis.Height <= 0) return;

	// ----------------------------------------------------------------------
	// êÿÇËèoÇµ.
	#if defined(_MSC_VER)
	bool rgb_swap = true;	// swap=true: BGR 
	#else
	bool rgb_swap = false;	// swap=false: RGB 
	#endif

	fnPRV_GDI_Extract(m_DrawImage, *image, m_Tag, rgb_swap);
	if (m_DrawImage.IsValid() == false) return;

	// ----------------------------------------------------------------------
	// ï`âÊ.
	#if defined(_MSC_VER)
	HDC hdcSrc = m_DrawImage.GetHDC();	// ï`âÊëŒè€âÊëú.(è„ãLÇÃèàóùÇ≈â¡çHÇ≥ÇÍÇƒÇ¢ÇÈ)
	HDC hdcDst = m_DC;					// ï`âÊêÊÇÃÉoÉbÉtÉ@.
	int dcid = ::SaveDC( hdcDst );
	if (dcid)
	{
		if( mag <= 1.0 )
		{
			// 
			// î{ó¶ ÅÖ 1.0 : â¬éãîÕàÕÇ 1.0 î{Ç≈ÉRÉsÅ[ÇµÇ‹Ç∑. ä˘Ç…èkè¨èàóùÇ™çsÇÌÇÍÇƒÇ¢Ç‹Ç∑.
			// 
			::SetStretchBltMode( hdcDst, COLORONCOLOR );
			::SetBrushOrgEx( hdcDst, 0, 0, NULL );

			// StretchBlt.
			int status = ::StretchBlt(
					hdcDst, eff.X, eff.Y, eff.Width, eff.Height,
					hdcSrc, 0, 0, m_DrawImage.Width(), m_DrawImage.Height(),
					SRCCOPY );
		}
		else
		{
			// 
			// î{ó¶ ÅÑ 1.0 : â¬éãîÕàÕÇägëÂÇµÇƒÉRÉsÅ[ÇµÇ‹Ç∑.
			// 
			TxRectangleI clip;
			clip.X		= (int)floor(vis.X);
			clip.Y		= (int)floor(vis.Y);
			clip.Width	= (int)(ceil(vis.X + vis.Width)  - floor(vis.X));
			clip.Height	= (int)(ceil(vis.Y + vis.Height) - floor(vis.Y));

			{
				HRGN hRegion = ::CreateRectRgn( eff.X, eff.Y, eff.X+eff.Width+1, eff.Y+eff.Height+1 );

				// StretchBlt Ç…éwíËÇ∑ÇÈì]ëóå≥ÇÃãÈå`Ç∆ÇÃç∑àŸÇçló∂ÇµÇƒêLèkÇ∑ÇÈ.
				double magx = clip.Width / vis.Width;
				double magy = clip.Height / vis.Height;

				// í[êîï™ à⁄ìÆÇ∑ÇÈ.
				double	eDx = (clip.X - vis.X) * mag;
				double	eDy = (clip.Y - vis.Y) * mag;

				// ===> magnification setting
				// x' = (x * eM11) + (y * eM21) + eDx
				// y' = (x * eM12) + (y * eM22) + eDy
				XFORM	xform =
				{
					(float)(magx),	// eM11: Horizontal scaling component
					(float)(0.0),	// eM12: not use
					(float)(0.0),	// eM21: not use
					(float)(magy),	// eM22: Vertical scaling component
					(float)(eDx),	// eDx : x' - (x * eM11) - (y * eM21)
					(float)(eDy)	// eDy : y' - (x * eM12) - (y * eM22)
				};
				::SetGraphicsMode( hdcDst, GM_ADVANCED );
				::SetWorldTransform( hdcDst, &xform );
				// ===> magnification setting

				::SelectClipRgn( hdcDst, hRegion );
				::DeleteObject( hRegion );
			}

			// StretchBlt Mode.
			if (m_Tag.Halftone == ExBoolean::True)
				::SetStretchBltMode( hdcDst, HALFTONE );
			else
				::SetStretchBltMode( hdcDst, COLORONCOLOR );

			::SetBrushOrgEx( hdcDst, 0, 0, NULL );

			// StretchBlt.
			int status = ::StretchBlt(
					hdcDst, eff.X, eff.Y, eff.Width, eff.Height,
					hdcSrc, 0, 0, clip.Width, clip.Height,
					SRCCOPY );
		}

		::RestoreDC( hdcDst, dcid );
	}
	#else
	{
		CxGLContext<CxGraphics> gc(this, false);

		auto status = glCheckFramebufferStatus(GL_FRAMEBUFFER);
		if (status != GL_FRAMEBUFFER_COMPLETE)
		{
			fnXIE_Core_TraceOut(1, "%s(%d): glCheckFramebufferStatus == %d\n", __FUNCTION__, __LINE__, status);
		}

		::glPushMatrix();

		double nearPlane  = -1.0;
		double farPlane   = +1.0;
		::glViewport		(0, 0, m_Tag.Width, m_Tag.Height);
		::glMatrixMode		(GL_PROJECTION);
		::glLoadIdentity	();
		::glOrtho			(0, m_Tag.Width, m_Tag.Height, 0, nearPlane, farPlane);
		::glMatrixMode		(GL_MODELVIEW);
		::glLoadIdentity	();

		unsigned int format = 0;
		switch(m_DrawImage.Model().Type)
		{
		default:
			break;
		case ExType::U8:
			switch(m_DrawImage.Model().Pack)
			{
			default: break;
			case 1: format = GL_LUMINANCE;	break;
			case 3: format = GL_RGB;		break;
			case 4: format = GL_RGBA;		break;
			}
			break;
		}

		if (format != 0)
		{
			::glDisable( GL_BLEND );
			::glDisable( GL_DEPTH_TEST );

			::glPixelStorei	(GL_PACK_ALIGNMENT, XIE_IMAGE_PACKING_SIZE);
			::glPixelStorei	(GL_PACK_ROW_LENGTH, m_DrawImage.Stride()/XIE_IMAGE_PACKING_SIZE);
			::glPixelStorei	(GL_PACK_SWAP_BYTES, GL_FALSE);
			::glPixelStorei	(GL_PACK_LSB_FIRST, GL_FALSE);
			::glPixelStorei	(GL_PACK_SKIP_PIXELS, 0);
			::glPixelStorei	(GL_PACK_SKIP_ROWS, 0);

			// ----------------------------------------------------------------------
			// î{ó¶ ÅÖ 1.0 : â¬éãîÕàÕÇ 1.0 î{Ç≈ÉRÉsÅ[ÇµÇ‹Ç∑. ä˘Ç…èkè¨èàóùÇ™çsÇÌÇÍÇƒÇ¢Ç‹Ç∑.
			// î{ó¶ ÅÑ 1.0 : â¬éãîÕàÕÇägëÂÇµÇƒÉRÉsÅ[ÇµÇ‹Ç∑.
			double zoom = (mag <= 1.0) ? 1.0 : mag;

			// ----------------------------------------------------------------------
			CxTexture texture;
			texture.Setup();

			if (texture.TextureID() != 0)
			{
				fnXIE_Core_TraceOut(1, "%s(%d): glTexImage2D\n", __FUNCTION__, __LINE__);

				unsigned int tid = texture.TextureID();

				::glEnable			(GL_TEXTURE_2D);
				::glBindTexture		(GL_TEXTURE_2D, tid);
				::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
				::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

				// ÉeÉNÉXÉ`ÉÉÇägëÂÅEèkè¨Ç∑ÇÈï˚ñ@.
				if (m_Tag.Halftone == ExBoolean::True)
				{
					::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
					::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
				}
				else
				{
					::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
					::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
				}

				void* addr = m_DrawImage.Address();
				::glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, m_DrawImage.Width(), m_DrawImage.Height(), 0, format, GL_UNSIGNED_BYTE, addr);
				{
					::glColor4f( 1.0f, 1.0f, 1.0f, 1.0f );

					double L = eff.X - ((vis.X - floor(vis.X)) * zoom);
					double T = eff.Y - ((vis.Y - floor(vis.Y)) * zoom);
					double R = L + (m_DrawImage.Width() * zoom);
					double B = T + (m_DrawImage.Height() * zoom);

					::glBegin( GL_QUADS );
					#if 1
					::glTexCoord2d( 0.0, 0.0 ); glVertex2d( L, T );	// 1:+----+
					::glTexCoord2d( 1.0, 0.0 ); glVertex2d( R, T );	// 2:|1Å@2|Å´
					::glTexCoord2d( 1.0, 1.0 ); glVertex2d( R, B );	// 3:|4Å@3|
					::glTexCoord2d( 0.0, 1.0 ); glVertex2d( L, B );	// 4:+----+
					#else
					::glTexCoord2d( 0.0, 0.0 ); glVertex2d( L, B );	// 1:+----+
					::glTexCoord2d( 1.0, 0.0 ); glVertex2d( R, B );	// 2:|4Å@3|
					::glTexCoord2d( 1.0, 1.0 ); glVertex2d( R, T );	// 3:|1Å@2|Å™
					::glTexCoord2d( 0.0, 1.0 ); glVertex2d( L, T );	// 4:+----+
					#endif
					::glEnd();
				}
				::glBindTexture	(GL_TEXTURE_2D, 0);
				::glDisable		(GL_TEXTURE_2D);
			}
			else
			{
				fnXIE_Core_TraceOut(1, "%s(%d): glDrawPixels\n", __FUNCTION__, __LINE__);

				double L = eff.X - ((vis.X - floor(vis.X)) * zoom);
				double T = eff.Y - ((vis.Y - floor(vis.Y)) * zoom);

				void* addr = m_DrawImage.Address();
				::glEnable( GL_REPLACE );
				::glRasterPos2d( L, T );
				::glPixelZoom( (float)zoom, (float)zoom );
				::glDrawPixels( m_DrawImage.Width(), m_DrawImage.Height(), format, GL_UNSIGNED_BYTE, addr );	// Ç∆ÇƒÇ‡íxÇ¢.
				::glDisable( GL_REPLACE );
			}
		}

		::glFlush();
		::glPopMatrix();
	}
	#endif
}

// ======================================================================
void CxGraphics::BeginPaint()
{
#if defined(_MSC_VER)
#else
	if (m_Context == NULL) return;
	if (m_XServer == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	::glXMakeCurrent( m_XServer, m_DrawableID, m_Context );
#endif
}

// ======================================================================
void CxGraphics::EndPaint()
{
#if defined(_MSC_VER)
#else
	if (m_XServer == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	::glXMakeCurrent( m_XServer, 0, NULL );
#endif
}

// ============================================================
void CxGraphics::Lock()
{
}

// ============================================================
void CxGraphics::Unlock()
{
}

#if defined(_MSC_VER)
// ======================================================================
HDC CxGraphics::DC() const
{
	return m_DC;
}
#else
// ======================================================================
void* CxGraphics::DC() const
{
	return m_DC;
}

// ============================================================
Display* CxGraphics::XServer() const
{
	return m_XServer;
}

// ======================================================================
GLXContext CxGraphics::Context() const
{
	return m_Context;
}

// ======================================================================
Window CxGraphics::DrawableID() const
{
	return m_DrawableID;
}
#endif

// ============================================================
void CxGraphics::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL ||
		value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Tag") == 0)
	{
		auto dst = static_cast<TxCanvas*>(value);
		*dst = this->m_Tag;
		return;
	}
#if defined(_MSC_VER)
	if (strcmp(name, "DC") == 0 && model == TxModel::Ptr(1))
	{
		auto dst = static_cast<HDC*>(value);
		*dst = this->m_DC;
		return;
	}
#else
	if (strcmp(name, "DC") == 0 && model == TxModel::Ptr(1))
	{
		auto dst = static_cast<void**>(value);
		*dst = this->m_DC;
		return;
	}
	if (strcmp(name, "XServer") == 0 && model == TxModel::Ptr(1))
	{
		auto dst = static_cast<Display**>(value);
		*dst = this->m_XServer;
		return;
	}
	if (strcmp(name, "Context") == 0 && model == TxModel::Ptr(1))
	{
		auto dst = static_cast<GLXContext*>(value);
		*dst = this->m_Context;
		return;
	}
	if (strcmp(name, "DrawableID") == 0 && model == ModelOf<Drawable>())
	{
		auto dst = static_cast<Drawable*>(value);
		*dst = this->m_DrawableID;
		return;
	}
#endif

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxGraphics::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL ||
		value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Tag") == 0)
	{
		auto src = static_cast<const TxCanvas*>(value);
		this->m_Tag = *src;
		return;
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

}
}
