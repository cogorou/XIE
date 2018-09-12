/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "GDI/CxCanvas.h"

#include "api_gdi.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxImage.h"
#include "GDI/IxGdi2d.h"
#include "GDI/CxOverlay.h"
#include "GDI/CxTexture.h"
#include "GDI/CxBitmap.h"
#include "CxGLContext.h"

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

static const char* g_ClassName = "CxCanvas";

// ============================================================
void CxCanvas::_Constructor()
{
	m_Tag = TxCanvas::Default();

#if defined(_MSC_VER)
	m_Target	= NULL;
	m_Context	= NULL;
#else
	m_Target	= 0;
	m_Context	= NULL;
	m_WindowID	= 0;
	m_XServer	= NULL;
	m_XVisual	= NULL;
	m_FrameBufferID		= 0;
	m_RenderBufferID	= 0;
	m_TextureID			= 0;
#endif
}

// ======================================================================
CxCanvas::CxCanvas()
{
	_Constructor();
}

// ======================================================================
CxCanvas::CxCanvas(const CxCanvas& src)
{
	_Constructor();
	operator = (src);
}

// ======================================================================
CxCanvas::~CxCanvas()
{
	Dispose();
}

// ============================================================
CxCanvas& CxCanvas::operator = ( const CxCanvas& src )
{
	if (this == &src) return *this;
	CxCanvas& dst = *this;

	Dispose();

#if _MSC_VER
	if (src.m_Target != NULL)
		dst.Setup( src.m_Target );
#else
	if (src.m_Target != 0 && src.m_WindowID == 0)
		dst.Setup( src.m_Target );
#endif
	if (dst.Size() != src.Size())
		dst.Resize(src.Size());

	dst.m_Tag = src.m_Tag;

	return *this;
}

// ============================================================
bool CxCanvas::operator == ( const CxCanvas& cmp ) const
{
	if (this == &cmp) return true;
	const CxCanvas& src = *this;

#if _MSC_VER
	if (src.m_Target	!= cmp.m_Target) return false;
#else
	if (src.m_Target	!= cmp.m_Target) return false;
#endif

	if (src.m_Tag		!= cmp.m_Tag) return false;

	return true;
}

// ============================================================
bool CxCanvas::operator != ( const CxCanvas& cmp ) const
{
	return !(CxCanvas::operator == (cmp));
}

// ============================================================
TxCanvas CxCanvas::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxCanvas::TagPtr() const
{
	return (void*)&m_Tag;
}

// ============================================================
void CxCanvas::Dispose()
{
	m_Tag.Width = 0;
	m_Tag.Height = 0;

#if defined(_MSC_VER)
	if (m_Context != NULL)
		::wglDeleteContext( m_Context );
	m_Context	= NULL;
	m_Target	= NULL;
	m_Buffer.Dispose();
#else
	if (m_FrameBufferID != 0)
		::glDeleteFramebuffersEXT(1, &m_FrameBufferID);
	m_FrameBufferID = 0;

	if (m_RenderBufferID != 0)
		::glDeleteRenderbuffersEXT(1, &m_RenderBufferID);
	m_RenderBufferID = 0;

	if (m_TextureID != 0)
		::glDeleteTextures(1, &m_TextureID);
	m_TextureID = 0;

	if (m_Context != NULL)
		::glXDestroyContext(m_XServer, m_Context);
	m_Context	= NULL;
	m_Target	= 0;

	if (m_WindowID != 0)
		::XDestroyWindow( m_XServer, m_WindowID );
	m_WindowID = 0;

	if (m_XVisual != NULL)
		fnPRV_GDI_XVisual_Close(m_XVisual);
	m_XVisual = NULL;

	if (m_XServer != NULL)
		fnPRV_GDI_XServer_Close(m_XServer);
	m_XServer = NULL;
#endif
}

// ============================================================
bool CxCanvas::IsValid() const
{
	if (m_Context == NULL) return false;
	if (m_Tag.Width == 0) return false;
	if (m_Tag.Height == 0) return false;
	return true;
}

// ============================================================
#if defined(_MSC_VER)
void CxCanvas::Setup(HDC target)
{
	if (m_Buffer.IsValid() == false)
	{
		Dispose();

		// 2015.03.01(Sun): 方針変更. Windows 環境では GDI を使用する.
		m_Buffer.Resize(1, 1);
		HDC hdc = m_Buffer.GetHDC();

		static PIXELFORMATDESCRIPTOR pfd =
		{
			sizeof(PIXELFORMATDESCRIPTOR),  // size of this pfd
			1,                              // version number
			PFD_SUPPORT_OPENGL |			// support OpenGL
			PFD_TYPE_RGBA,                  // RGBA model
			24,                             // 24-bit color depth
			0, 0, 0, 0, 0, 0,               // color bits ignored
			0,                              // no alpha buffer
			0,                              // shift bit ignored
			0,                              // no accumulation buffer
			0, 0, 0, 0,                     // accum bits ignored
			32,                             // 32-bit z-buffer
			0,                              // no stencil buffer
			0,                              // no auxiliary buffer
			PFD_MAIN_PLANE,                 // main layer
			0,                              // reserved
			0, 0, 0                         // layer masks ignored
		};

		// NOTE:
		/*
			case1: [0x04] PFD_DRAW_TO_WINDOW
			case2: [0x0C] PFD_DRAW_TO_WINDOW|PFD_DRAW_TO_BITMAP
			case3: [0x08] PFD_DRAW_TO_BITMAP

			hdc が Window から取得したものであれば case1 で良いが、
			メモリデバイスコンテキストの場合は case2 でなければならない。
			但し、ピクセルサイズが 24 bpp の時は case3 でなければならない。
			現在は 32 bpp 限定の処理にしている。

			OBJ_DC の時は PFD_DOUBLEBUFFER を有効にする.
			OBJ_MEMDC の時は PFD_DOUBLEBUFFER は使えない.

			see:
				GetObjectType
				http://msdn.microsoft.com/en-us/library/windows/desktop/dd144905(v=vs.85).aspx
		*/
		int bitspixel = ::GetDeviceCaps(hdc, BITSPIXEL);
		DWORD objtype = ::GetObjectType(hdc);
		switch(objtype)
		{
		case OBJ_DC:		// Device context
			pfd.dwFlags |= PFD_DRAW_TO_WINDOW;		// 0x04: support window
			pfd.dwFlags |= PFD_DOUBLEBUFFER;
			break;
		case OBJ_MEMDC:		// Memory DC
			pfd.dwFlags |= PFD_DRAW_TO_WINDOW;		// 0x04: support window
			pfd.dwFlags |= PFD_DRAW_TO_BITMAP;		// 0x08: support bitmap
			break;
		default:
			break;
		}

		int iPixelformat = ::ChoosePixelFormat( hdc, &pfd );
		if (iPixelformat == 0)
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

		BOOL status = ::SetPixelFormat(hdc, iPixelformat, &pfd);
		if (status == FALSE)
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

		int num = ::GetPixelFormat( hdc );
		int index = ::DescribePixelFormat( hdc, num, sizeof(pfd), &pfd );
		if (index == 0)
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

		// -- Make Rendering Context
		m_Context = ::wglCreateContext( hdc );
		if (m_Context == NULL)
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
	}

	m_Target = target;
}
#else
void CxCanvas::Setup(GLXDrawable target)
{
	if (m_Target != target || target == 0)
	{
		Dispose();

		m_Target	= target;

		m_XServer = fnPRV_GDI_XServer_Open();
		if (m_XServer == NULL)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		m_XVisual = fnPRV_GDI_XVisual_Open(m_XServer);
		if (m_XVisual == NULL)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// create rendering context
		m_Context = ::glXCreateContext(m_XServer, m_XVisual, NULL, True);
		if (m_Context == NULL)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		if (m_Target == 0)
		{
			Window parent = RootWindow( m_XServer, m_XVisual->screen );
			Window window = ::XCreateSimpleWindow(m_XServer, parent, 0, 0, 1, 1, 0, 0, BlackPixel( m_XServer, m_XVisual->screen ));
			if (window == 0)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
			m_Target = window;
			m_WindowID = window;
		}

		CxGLContext<CxCanvas> gc(this, false);

		// Initialize glew
		GLenum err = ::glewInit();
		if (GLEW_OK != err)
			throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);

		// Initialize Frame Buffer
		if (GLEW_EXT_framebuffer_object)
		{
			// テクスチャ.
			::glGenTextures		(1, &m_TextureID);
			::glBindTexture		(GL_TEXTURE_2D, m_TextureID);
			::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
			::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
			::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
			::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);

			// レンダーバッファ.
			::glGenRenderbuffersEXT	(1, &m_RenderBufferID);
			::glBindRenderbufferEXT	(GL_RENDERBUFFER_EXT, m_RenderBufferID);

			// フレームバッファ.
			::glGenFramebuffersEXT			(1, &m_FrameBufferID);
			::glBindFramebufferEXT			(GL_FRAMEBUFFER_EXT, m_FrameBufferID);
			::glFramebufferTexture2DEXT		(GL_FRAMEBUFFER_EXT, GL_COLOR_ATTACHMENT0_EXT, GL_TEXTURE_2D, m_TextureID, 0);
			::glFramebufferRenderbufferEXT	(GL_FRAMEBUFFER_EXT, GL_DEPTH_ATTACHMENT_EXT, GL_RENDERBUFFER_EXT, m_RenderBufferID);
			::glBindFramebufferEXT			(GL_FRAMEBUFFER_EXT, 0);
		}
	}
}
#endif

// ======================================================================
void CxCanvas::Resize(const TxSizeI& size)
{
	Resize(size.Width, size.Height);
}

// ======================================================================
void CxCanvas::Resize(int width, int height)
{
	if (width < 0 || height < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (width != m_Tag.Width || height != m_Tag.Height)
	{
		#if defined(_MSC_VER)
		{
			m_Buffer.Resize(width, height);
		}
		#else
		{
			if (width == 0)
				width = 1;
			if (height == 0)
				height = 1;

			if (m_RenderBufferID != 0 && m_TextureID != 0)
			{
				CxGLContext<CxCanvas> gc(this, false);

				// テクスチャ.
				::glBindTexture	(GL_TEXTURE_2D, m_TextureID);
				::glTexImage2D	(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, 0);

				// レンダーバッファ.
				::glBindRenderbufferEXT		(GL_RENDERBUFFER_EXT, m_RenderBufferID);
				::glRenderbufferStorageEXT	(GL_RENDERBUFFER_EXT, GL_DEPTH_COMPONENT24, width, height);

				::glBindTexture	(GL_TEXTURE_2D, 0);
			}
		}
		#endif
	}

	m_Tag.Width		= width;
	m_Tag.Height	= height;
}

// ============================================================
void CxCanvas::Clear()
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	#if defined(_MSC_VER)
	{
		m_Buffer.Clear(m_Tag.BkColor);
	}
	#else
	{
		CxGLContext<CxCanvas> gc(this, true);

		// 背景の塗り潰し.
		float	R = m_Tag.BkColor.R / 255.0f;
		float	G = m_Tag.BkColor.G / 255.0f;
		float	B = m_Tag.BkColor.B / 255.0f;
		::glClearColor( R, G, B, 1.0f );
		::glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
		::glFlush();
	}
	#endif
}

// ============================================================
void CxCanvas::DrawImage(HxModule himage)
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	CxImage image;
	if (xie::Axi::ClassIs<CxImage>(himage))
	{
		image.Attach(*xie::Axi::SafeCast<CxImage>(himage));
	}
	else if (xie::Axi::ClassIs<IxModule>(himage))
	{
		image.CopyFrom(*xie::Axi::SafeCast<IxModule>(himage));
	}

	// 背景の塗り潰し.
	#if defined(_MSC_VER)
	m_Buffer.Clear(m_Tag.BkColor);
	#else
	CxGLContext<CxCanvas> gc(this, true);
	if (BkEnable())
	{
		float	R = m_Tag.BkColor.R / 255.0f;
		float	G = m_Tag.BkColor.G / 255.0f;
		float	B = m_Tag.BkColor.B / 255.0f;
		::glClearColor( R, G, B, 1.0f );
	}
	::glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
	#endif

	// 画像の描画.
	if (image.IsValid() == false) return;

	m_Tag.BgSize = image.Size();

	auto mag = m_Tag.Magnification;
	auto view_point = m_Tag.ViewPoint;
	auto display_rect = DisplayRect();
	auto eff = EffectiveRect();
	auto vis = VisibleRect();

	if (m_Tag.BgSize.Width <= 0 || m_Tag.BgSize.Height <= 0) return;
	if (display_rect.Width <= 0 || display_rect.Height <= 0) return;
	if (eff.Width <= 0 || eff.Height <= 0) return;
	if (vis.Width <= 0 || vis.Height <= 0) return;

	// ----------------------------------------------------------------------
	// 切り出し.
	#if defined(_MSC_VER)
	bool rgb_swap = true;	// swap=true: BGR 
	#else
	bool rgb_swap = false;	// swap=false: RGB 
	#endif

	fnPRV_GDI_Extract(m_DrawImage, image, m_Tag, rgb_swap);
	if (m_DrawImage.IsValid() == false) return;

	// ----------------------------------------------------------------------
	// 描画.
	#if defined(_MSC_VER)
	HDC hdcSrc = m_DrawImage.GetHDC();	// 描画対象画像.(上記の処理で加工されている)
	HDC hdcDst = m_Buffer.GetHDC();		// 描画先のバッファ.
	int dcid = ::SaveDC( hdcDst );
	if( dcid )
	{
		if( mag <= 1.0 )
		{
			// 
			// 倍率 ≦ 1.0 : 可視範囲を 1.0 倍でコピーします. 既に縮小処理が行われています.
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
			// 倍率 ＞ 1.0 : 可視範囲を拡大してコピーします.
			// 
			TxRectangleI clip;
			clip.X		= (int)floor(vis.X);
			clip.Y		= (int)floor(vis.Y);
			clip.Width	= (int)(ceil(vis.X + vis.Width)  - floor(vis.X));
			clip.Height	= (int)(ceil(vis.Y + vis.Height) - floor(vis.Y));

			{
				HRGN hRegion = ::CreateRectRgn( eff.X, eff.Y, eff.X+eff.Width+1, eff.Y+eff.Height+1 );

				// StretchBlt に指定する転送元の矩形との差異を考慮して伸縮する.
				double magx = clip.Width / vis.Width;
				double magy = clip.Height / vis.Height;

				// 端数分 移動する.
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
			if (Halftone())
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
			// 倍率 ≦ 1.0 : 可視範囲を 1.0 倍でコピーします. 既に縮小処理が行われています.
			// 倍率 ＞ 1.0 : 可視範囲を拡大してコピーします.
			double zoom = (mag <= 1.0) ? 1.0 : mag;

			// ----------------------------------------------------------------------
			CxTexture texture;
			texture.Setup();

			if (texture.TextureID() != 0)
			{
				unsigned int tid = texture.TextureID();

				::glEnable			(GL_TEXTURE_2D);
				::glBindTexture		(GL_TEXTURE_2D, tid);
				::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
				::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

				// テクスチャを拡大・縮小する方法.
				if (this->Halftone())
				{
					::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
					::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
				}
				else
				{
					::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
					::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
				}

				void* addr = m_DrawImage[0];
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
					::glTexCoord2d( 1.0, 0.0 ); glVertex2d( R, T );	// 2:|1　2|↓
					::glTexCoord2d( 1.0, 1.0 ); glVertex2d( R, B );	// 3:|4　3|
					::glTexCoord2d( 0.0, 1.0 ); glVertex2d( L, B );	// 4:+----+
					#else
					::glTexCoord2d( 0.0, 0.0 ); glVertex2d( L, B );	// 1:+----+
					::glTexCoord2d( 1.0, 0.0 ); glVertex2d( R, B );	// 2:|4　3|
					::glTexCoord2d( 1.0, 1.0 ); glVertex2d( R, T );	// 3:|1　2|↑
					::glTexCoord2d( 0.0, 1.0 ); glVertex2d( L, T );	// 4:+----+
					#endif
					::glEnd();
				}
				::glBindTexture	(GL_TEXTURE_2D, 0);
				::glDisable		(GL_TEXTURE_2D);
			}
			else
			{
				double L = eff.X - ((vis.X - floor(vis.X)) * zoom);
				double T = eff.Y - ((vis.Y - floor(vis.Y)) * zoom);

				void* addr = m_DrawImage[0];
				::glEnable( GL_REPLACE );
				::glRasterPos2d( L, T );
				::glPixelZoom( (float)zoom, (float)zoom );
				::glDrawPixels( m_DrawImage.Width(), m_DrawImage.Height(), format, GL_UNSIGNED_BYTE, addr );	// とても遅い.
				::glDisable( GL_REPLACE );
			}
		}

		::glFlush();
		::glPopMatrix();
	}
	#endif
}

// ============================================================
void CxCanvas::DrawOverlayCB(fnXIE_GDI_CallBack_Render render, void* param, ExGdiScalingMode mode)
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	CxGLContext<CxCanvas> gc(this, true);

	double			mag = m_Tag.Magnification;
	TxRectangleI	eff = EffectiveRect();
	TxRectangleD	vis = VisibleRect();

	// ----------------------------------------------------------------------
	// 描画.
	{
		::glPushMatrix();

		// affine
		/*
			指定された mode に従いアフィン変換行列を設定する.
			・OpenGL     … glTranslated, glScaled
			・WindowsGDI … ModifyWorldTransform (※各クラスの Render 内で行う)
		*/
		{
			double nearPlane  = -1.0;
			double farPlane   = +1.0;
			::glViewport		(0, 0, m_Tag.Width, m_Tag.Height);
			::glMatrixMode		(GL_PROJECTION);
			::glLoadIdentity	();
			#if defined(_MSC_VER)
			::glOrtho			(0, m_Tag.Width, 0, m_Tag.Height, nearPlane, farPlane);
			#else
			::glOrtho			(0, m_Tag.Width, m_Tag.Height, 0, nearPlane, farPlane);
			#endif
			::glMatrixMode		(GL_MODELVIEW);
			::glLoadIdentity	();

			switch(mode)
			{
			default:
			case ExGdiScalingMode::None:
				{
					::glTranslated	(0.0, 0.0, 0.0);
					::glScaled		(1.0, 1.0, 1.0);
				}
				break;
			case ExGdiScalingMode::TopLeft:
				{
					double tx = eff.X - (vis.X * mag);
					double ty = eff.Y - (vis.Y * mag);
					::glTranslated	(tx, ty, 0.0);
					::glScaled		(mag, mag, 1.0);
				}
				break;
			case ExGdiScalingMode::Center:
				{
					double tx = eff.X - ((vis.X - 0.5) * mag);
					double ty = eff.Y - ((vis.Y - 0.5) * mag);
					::glTranslated	(tx, ty, 0.0);
					::glScaled		(mag, mag, 1.0);
				}
				break;
			}
		}

		if (render != NULL)
		{
			render(this, param, mode);
		}

		::glFlush();
		::glPopMatrix();
	}
}

// ============================================================
static void XIE_API fnPRV_CallBack_Render(void* canvas, void* param, ExGdiScalingMode mode)
{
	auto info = (TxArray*)param;
	auto figures = (HxModule*)info->Address;
	auto count = info->Length;

	for(int i=0 ; i<count ; i++)
	{
		try
		{
			if (auto figure = xie::Axi::SafeCast<IxGdi2dRendering>(figures[i]))
			{
				figure->Render((HxModule)canvas, mode);
				continue;
			}
		}
		catch(const CxException&)
		{
		}
	}
}

// ============================================================
void CxCanvas::DrawOverlay(HxModule hfigure, ExGdiScalingMode mode)
{
	HxModule hfigures[1] = { hfigure };
	DrawOverlay(hfigures, 1, mode);
}

// ============================================================
void CxCanvas::DrawOverlay(const HxModule* hfigures, int count, ExGdiScalingMode mode)
{
	TxArray info((void*)hfigures, count, TxModel::Default());
	DrawOverlayCB(fnPRV_CallBack_Render, &info, mode);
}

// ============================================================
void CxCanvas::Flush()
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	#if defined(_MSC_VER)
	if (m_Target != NULL)
	{
		/*
			描画先: m_Target (Setup に指定された DC)
			描画元: m_Buffer (DrawImage や DrawOverlay で重畳された結果)
		*/
		::BitBlt(m_Target, 0, 0, m_Tag.Width, m_Tag.Height, m_Buffer.GetHDC(), 0, 0, SRCCOPY);
		::SwapBuffers( m_Target );
	}
	#else
	{
		CxGLContext<CxCanvas> gc(this, false);

		if (m_TextureID != 0)
		{
			::glPushMatrix();

			float nearPlane	= -1.0;
			float farPlane	= +1.0;
			::glViewport		(0, 0, m_Tag.Width, m_Tag.Height);
			::glMatrixMode		(GL_PROJECTION);
			::glLoadIdentity	();
			::glOrtho			(0, m_Tag.Width, 0, m_Tag.Height, nearPlane, farPlane);
			::glMatrixMode		(GL_MODELVIEW);
			::glLoadIdentity	();

			::glClearColor	(0.0f, 0.0f, 0.0f, 0.0f);
			::glClearDepth	(1.0);
			::glColor3f		(1.0f, 1.0f, 1.0f);

			::glDisable	(GL_BLEND);
			::glDisable	(GL_DEPTH_TEST);

			::glEnable			(GL_TEXTURE_2D);
			::glBindTexture		(GL_TEXTURE_2D, m_TextureID);
			::glTexEnvf			(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
			::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
			::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

			// テクスチャを拡大・縮小する方法.
			::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
			::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);

			{
				float L = 0.0f;
				float T = 0.0f;
				float R = (float)m_Tag.Width;
				float B = (float)m_Tag.Height;

				::glBegin( GL_QUADS );
				#if 1
				::glTexCoord2f( 0.0f, 0.0f ); glVertex2f( L, T );
				::glTexCoord2f( 1.0f, 0.0f ); glVertex2f( R, T );
				::glTexCoord2f( 1.0f, 1.0f ); glVertex2f( R, B );
				::glTexCoord2f( 0.0f, 1.0f ); glVertex2f( L, B );
				#else
				::glTexCoord2f( 0.0f, 0.0f ); glVertex2f( L, B );
				::glTexCoord2f( 1.0f, 0.0f ); glVertex2f( R, B );
				::glTexCoord2f( 1.0f, 1.0f ); glVertex2f( R, T );
				::glTexCoord2f( 0.0f, 1.0f ); glVertex2f( L, T );
				#endif
				::glEnd();
			}
			::glBindTexture	(GL_TEXTURE_2D, 0);
			::glDisable		(GL_TEXTURE_2D);

			::glPopMatrix();
		}

		if (m_XServer == NULL)
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		::glXSwapBuffers( m_XServer, m_Target );
	}
	#endif
}

// ======================================================================
void CxCanvas::Flush(HxModule hdst)
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	CxImage* _dst = xie::Axi::SafeCast<CxImage>(hdst);
	if (_dst == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

#if defined(_MSC_VER)
	{
		_dst->CopyFrom(m_Buffer);
	}
#else
	{
		CxGLContext<CxCanvas> gc(this, false);

		TxImageSize size(m_Tag.Width, m_Tag.Height, TxModel::U8(4), 1);
		CxImage buf(size);
		_dst->Resize(size);

		::glPixelStorei	(GL_PACK_ALIGNMENT, XIE_IMAGE_PACKING_SIZE);
		::glPixelStorei	(GL_PACK_ROW_LENGTH, buf.Stride()/XIE_IMAGE_PACKING_SIZE);
		::glPixelStorei	(GL_PACK_SWAP_BYTES, GL_FALSE);
		::glPixelStorei	(GL_PACK_LSB_FIRST, GL_FALSE);
		::glPixelStorei	(GL_PACK_SKIP_PIXELS, 0);
		::glPixelStorei	(GL_PACK_SKIP_ROWS, 0);
	
		if (m_FrameBufferID != 0)
		{
			void* addr = buf[0];
			::glBindFramebufferEXT	(GL_FRAMEBUFFER_EXT, m_FrameBufferID);
			::glReadPixels(0, 0, size.Width, size.Height, GL_RGBA, GL_UNSIGNED_BYTE, addr);
			::glBindFramebufferEXT	(GL_FRAMEBUFFER_EXT, 0);
			_dst->Filter().Mirror(buf, 2);
		}
		else
		{
			void* addr = buf[0];
			::glReadPixels(0, 0, size.Width, size.Height, GL_RGBA, GL_UNSIGNED_BYTE, addr);
			_dst->Filter().Mirror(buf, 2);
		}
	}
#endif
}

// ======================================================================
void CxCanvas::BeginPaint()
{
#if defined(_MSC_VER)
	if (m_Context == NULL) return;
	::wglMakeCurrent( m_Buffer.GetHDC(), m_Context );
#else
	if (m_Context == NULL) return;
	if (m_XServer == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	::glXMakeCurrent( m_XServer, m_Target, m_Context );
#endif
}

// ======================================================================
void CxCanvas::EndPaint()
{
#if defined(_MSC_VER)
	::wglMakeCurrent( NULL, NULL );
#else
	if (m_XServer == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	::glXMakeCurrent( m_XServer, 0, NULL );
#endif
}

// ============================================================
void CxCanvas::Lock()
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
#if defined(_MSC_VER)
#else
	if (m_FrameBufferID != 0)
		::glBindFramebufferEXT(GL_FRAMEBUFFER_EXT, m_FrameBufferID);
#endif
}

// ============================================================
void CxCanvas::Unlock()
{
#if defined(_MSC_VER)
#else
	if (m_FrameBufferID != 0)
		::glBindFramebufferEXT(GL_FRAMEBUFFER_EXT, 0);
#endif
}

#if defined(_MSC_VER)
// ======================================================================
HDC CxCanvas::Target() const
{
	return m_Target;
}

// ======================================================================
HGLRC CxCanvas::Context() const
{
	return m_Context;
}

// ======================================================================
const CxBitmap& CxCanvas::Buffer() const
{
	return m_Buffer;
}
#else
// ======================================================================
GLXDrawable CxCanvas::Target() const
{
	return m_Target;
}

// ======================================================================
GLXContext CxCanvas::Context() const
{
	return m_Context;
}

// ============================================================
Display* CxCanvas::XServer() const
{
	return m_XServer;
}

// ============================================================
XVisualInfo* CxCanvas::XVisual() const
{
	return m_XVisual;
}

// ======================================================================
Window CxCanvas::WindowID() const
{
	return m_WindowID;
}

// ======================================================================
unsigned int CxCanvas::FrameBufferID() const
{
	return m_FrameBufferID;
}

// ======================================================================
unsigned int CxCanvas::RenderBufferID() const
{
	return m_RenderBufferID;
}

// ======================================================================
unsigned int CxCanvas::TextureID() const
{
	return m_TextureID;
}
#endif

// ============================================================
TxSizeI CxCanvas::Size() const
{
	return TxSizeI(m_Tag.Width, m_Tag.Height);
}

// ======================================================================
int CxCanvas::Width() const
{
	return m_Tag.Width;
}

// ======================================================================
int CxCanvas::Height() const
{
	return m_Tag.Height;
}

// ============================================================
TxSizeI CxCanvas::BgSize() const
{
	return m_Tag.BgSize;
}

// ============================================================
void CxCanvas::BgSize(TxSizeI value)
{
	if (value.Width < 0) return;
	if (value.Height < 0) return;
	m_Tag.BgSize = value;
}

// ============================================================
TxRGB8x4 CxCanvas::BkColor() const
{
	return m_Tag.BkColor;
}

// ============================================================
void CxCanvas::BkColor(TxRGB8x4 value)
{
	m_Tag.BkColor = value;
}

// ============================================================
bool CxCanvas::BkEnable() const
{
	return (m_Tag.BkEnable == ExBoolean::True);
}

// ============================================================
void CxCanvas::BkEnable(bool value)
{
	m_Tag.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
double CxCanvas::Magnification() const
{
	return m_Tag.Magnification;
}

// ============================================================
void CxCanvas::Magnification(double value)
{
	if (value <= 0) return;
	m_Tag.Magnification = value;
}

// ============================================================
TxPointD CxCanvas::ViewPoint() const
{
	return m_Tag.ViewPoint;
}

// ============================================================
void CxCanvas::ViewPoint(TxPointD value)
{
	m_Tag.ViewPoint = value;
}

// ============================================================
int CxCanvas::ChannelNo() const
{
	return m_Tag.ChannelNo;
}

// ============================================================
void CxCanvas::ChannelNo(int value)
{
	if (!(0 <= value && value < XIE_IMAGE_CHANNELS_MAX)) return;
	m_Tag.ChannelNo = value;
}

// ============================================================
bool CxCanvas::Unpack() const
{
	return (m_Tag.Unpack == ExBoolean::True);
}

// ============================================================
void CxCanvas::Unpack(bool value)
{
	m_Tag.Unpack = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
bool CxCanvas::Halftone() const
{
	return (m_Tag.Halftone == ExBoolean::True);
}

// ============================================================
void CxCanvas::Halftone(bool value)
{
	m_Tag.Halftone = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxSizeI CxCanvas::DisplaySize() const
{
	return TxSizeI(m_Tag.Width, m_Tag.Height);
}

// ============================================================
TxRectangleI CxCanvas::DisplayRect() const
{
	return TxRectangleI(0, 0, m_Tag.Width, m_Tag.Height);
}

// ============================================================
TxRectangleI CxCanvas::EffectiveRect() const
{
	return m_Tag.EffectiveRect();
}

// ============================================================
TxRectangleD CxCanvas::VisibleRect() const
{
	return m_Tag.VisibleRect();
}

// ============================================================
TxRectangleI CxCanvas::VisibleRectI(bool includePartialPixel) const
{
	return m_Tag.VisibleRectI(includePartialPixel);
}

// ============================================================
TxPointD CxCanvas::DPtoIP(const TxPointD& dp, ExGdiScalingMode mode) const
{
	return m_Tag.DPtoIP(dp, mode);
}

// ============================================================
TxPointD CxCanvas::IPtoDP(const TxPointD& ip, ExGdiScalingMode mode) const
{
	return m_Tag.IPtoDP(ip, mode);
}

// ============================================================
void CxCanvas::GetParam(TxCharCPtrA name, void* value, TxModel model) const
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
	if (strcmp(name, "Target") == 0 && model == TxModel::Ptr(1))
	{
		auto dst = static_cast<HDC*>(value);
		*dst = this->m_Target;
		return;
	}
	if (strcmp(name, "Context") == 0 && model == TxModel::Ptr(1))
	{
		auto dst = static_cast<HGLRC*>(value);
		*dst = this->m_Context;
		return;
	}
	if (strcmp(name, "Buffer") == 0 && model == TxModel::Ptr(1))
	{
		auto dst = static_cast<HxModule*>(value);
		*dst = (HxModule)&this->m_Buffer;
		return;
	}
#else
	if (strcmp(name, "Target") == 0 && model == TxModel::S32(1))
	{
		auto dst = static_cast<GLXDrawable*>(value);
		*dst = this->m_Target;
		return;
	}
	if (strcmp(name, "Context") == 0 && model == TxModel::Ptr(1))
	{
		auto dst = static_cast<GLXContext*>(value);
		*dst = this->m_Context;
		return;
	}
	if (strcmp(name, "XServer") == 0 && model == TxModel::Ptr(1))
	{
		auto dst = static_cast<Display**>(value);
		*dst = this->m_XServer;
		return;
	}
	if (strcmp(name, "XVisual") == 0 && model == TxModel::Ptr(1))
	{
		auto dst = static_cast<XVisualInfo**>(value);
		*dst = this->m_XVisual;
		return;
	}
	if (strcmp(name, "WindowID") == 0 && model == ModelOf<Window>())
	{
		auto dst = static_cast<Window*>(value);
		*dst = this->m_WindowID;
		return;
	}
#endif

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxCanvas::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL ||
		value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ======================================================================
TxRectangleI CxCanvas::EffectiveRect(TxRectangleI display_rect, TxSizeI bg_size, double mag)
{
	return TxCanvas::EffectiveRect(display_rect, bg_size, mag);
}

// ======================================================================
TxRectangleD CxCanvas::VisibleRect(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point)
{
	return TxCanvas::VisibleRect(display_rect, bg_size, mag, view_point);
}

// ======================================================================
TxPointD CxCanvas::DPtoIP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD dp, xie::GDI::ExGdiScalingMode mode)
{
	return TxCanvas::DPtoIP(display_rect, bg_size, mag, view_point, dp, mode);
}

// ======================================================================
TxPointD CxCanvas::IPtoDP(TxRectangleI display_rect, TxSizeI bg_size, double mag, TxPointD view_point, TxPointD ip, xie::GDI::ExGdiScalingMode mode)
{
	return TxCanvas::IPtoDP(display_rect, bg_size, mag, view_point, ip, mode);
}

}
}
