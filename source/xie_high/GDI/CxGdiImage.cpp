/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxGdiImage.h"
#include "GDI/CxCanvas.h"
#include "Core/CxException.h"
#include "Core/Axi.h"
#include "Core/IxParam.h"
#include "GDI/CxTexture.h"
#include "GDI/CxGdiRectangle.h"

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

static const char* g_ClassName = "CxGdiImage";

// ================================================================================
void CxGdiImage::_Constructor()
{
	m_Tag.Param = TxGdi2dParam::Default();
	m_Tag = TxGdiImage::Default();
	m_IsAttached = false;
#if defined(_MSC_VER)
	m_hBitmap	= NULL;
#endif
}

// ================================================================================
CxGdiImage::CxGdiImage()
{
	_Constructor();
}

// ================================================================================
CxGdiImage::CxGdiImage( CxGdiImage&& src )
{
	_Constructor();
	MoveFrom(src);
}

// ================================================================================
CxGdiImage::CxGdiImage( const CxGdiImage& src )
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxGdiImage::CxGdiImage( const CxImage& src )
{
	_Constructor();
	operator = (src);
}

// ================================================================================
CxGdiImage::CxGdiImage( int width, int height )
{
	_Constructor();
	Resize(width, height);
}

// ================================================================================
CxGdiImage::~CxGdiImage()
{
	Dispose();
}

// ============================================================
CxGdiImage& CxGdiImage::operator = ( CxGdiImage&& src )
{
	MoveFrom(src);
	return *this;
}

// ================================================================================
CxGdiImage& CxGdiImage::operator = ( const CxGdiImage& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
CxGdiImage& CxGdiImage::operator = ( const CxImage& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxGdiImage::operator == ( const CxGdiImage& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxGdiImage::operator != ( const CxGdiImage& src ) const
{
	return !ContentEquals(src);
}

// ================================================================================
TxGdiImage CxGdiImage::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxGdiImage::TagPtr() const
{
	return (void*)&m_Tag;
}

// ================================================================================
CxGdiImage::operator CxImage() const
{
	CxImage dst;
	this->CopyTo(dst);
	return dst;
}

// ================================================================================
void CxGdiImage::CopyTo(IxModule& dst) const
{
	if (xie::Axi::ClassIs<CxImage>(dst))
	{
		CxImage		_src; _src.Attach(this->Tag());
		CxImage&	_dst = static_cast<CxImage&>(dst);

		if (_src.IsValid() == false)
			_dst.Dispose();
		else
			_dst.CopyFrom(_src);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
CxGdiImage CxGdiImage::Clone() const
{
	CxGdiImage clone;
	clone.CopyFrom(*this);
	return clone;
}

// ////////////////////////////////////////////////////////////
// 
// INHERIT MEMBERS
// 

// ================================================================================
void CxGdiImage::Dispose()
{
	if (IsAttached() == false)
	{
		#if defined(_MSC_VER)
		if (m_hBitmap != NULL)
			::DeleteObject( m_hBitmap );
		m_hBitmap = NULL;
		#else
		if (m_Tag.Address != NULL)
			xie::Axi::MemoryFree(m_Tag.Address);
		#endif
	}
	m_Tag.Address = NULL;
	m_Tag.Width = 0;
	m_Tag.Height = 0;
	m_Tag.Model = TxModel::Default();
	m_Tag.Stride = 0;
	m_IsAttached = false;
}

// ================================================================================
void CxGdiImage::MoveFrom(CxGdiImage& src)
{
	if (this == &src) return;

	CxGdiImage& dst = *this;
	dst.Dispose();
	#if defined(_MSC_VER)
	dst.m_hBitmap		= dst.m_hBitmap;
	#endif
	dst.m_Tag.Param		= src.m_Tag.Param;
	dst.m_Tag			= src.m_Tag;
	dst.m_IsAttached	= src.m_IsAttached;
	src.m_IsAttached	= true;
}

// ================================================================================
void CxGdiImage::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxGdiImage>(src))
	{
		auto&	_src = static_cast<const CxGdiImage&>(src);
		auto&	_dst = *this;

		_dst.m_Tag.Param = _src.m_Tag.Param;

		// 画像データの複製.
	//	dst.m_Tag.Address	= _src.m_Tag.Address;
	//	dst.m_Tag.Model		= _src.m_Tag.Model;
	//	dst.m_Tag.Width		= _src.m_Tag.Width;
	//	dst.m_Tag.Height	= _src.m_Tag.Height;
	//	dst.m_Tag.Stride	= _src.m_Tag.Stride;

		Resize(_src.m_Tag.Width, _src.m_Tag.Height);
		if (_dst.m_Tag.Address != NULL)
		{
			char*		dst_addr = (char*)_dst.m_Tag.Address;
			const char*	src_addr = (const char*)_src.m_Tag.Address;
			size_t size = _dst.m_Tag.Model.Size();
			size_t length = (size_t)_dst.m_Tag.Width;
			size_t stride = size * length;
			for(int y=0 ; y<_dst.m_Tag.Height ; y++)
			{
				memcpy(dst_addr, src_addr, stride);
				dst_addr += _dst.m_Tag.Stride;
				src_addr += _src.m_Tag.Stride;
			}
		}

		_dst.m_Tag.X			= _src.m_Tag.X;
		_dst.m_Tag.Y			= _src.m_Tag.Y;
		_dst.m_Tag.MagX			= _src.m_Tag.MagX;
		_dst.m_Tag.MagY			= _src.m_Tag.MagY;
		_dst.m_Tag.Alpha		= _src.m_Tag.Alpha;
		_dst.m_Tag.AlphaFormat	= _src.m_Tag.AlphaFormat;
		return;
	}
	if (xie::Axi::ClassIs<CxImage>(src))
	{
		const CxImage&	_src = static_cast<const CxImage&>(src);
		CxImage			_dst;
		this->Resize(_src.Size());
		_dst.Attach(this->Tag());
		#if defined(_MSC_VER)
		{
			if (_src.Channels() == 1 && _src.Model().Pack == 1)
				_dst.Filter().Copy(_src);		// BGR ← Gray
			else
				_dst.Filter().RgbToBgr(_src);	// BGR ← RGB
		}
		#else
		{
			_dst.Filter().Copy(_src);			// RGB ← RGB
		}
		#endif
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
bool CxGdiImage::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;

	if (xie::Axi::ClassIs<CxGdiImage>(src))
	{
		auto&	_src = static_cast<const CxGdiImage&>(src);
		auto&	_dst = *this;

		if (_dst.m_Tag.Param != _src.m_Tag.Param) return false;

		if (_dst.m_Tag.Model		!= _src.m_Tag.Model) return false;
		if (_dst.m_Tag.Width		!= _src.m_Tag.Width) return false;
		if (_dst.m_Tag.Height		!= _src.m_Tag.Height) return false;
		if (_dst.m_Tag.Stride		!= _src.m_Tag.Stride) return false;
		if (_dst.m_Tag.X			!= _src.m_Tag.X) return false;
		if (_dst.m_Tag.Y			!= _src.m_Tag.Y) return false;
		if (_dst.m_Tag.MagX			!= _src.m_Tag.MagX) return false;
		if (_dst.m_Tag.MagY			!= _src.m_Tag.MagY) return false;
		if (_dst.m_Tag.Alpha		!= _src.m_Tag.Alpha) return false;
		if (_dst.m_Tag.AlphaFormat	!= _src.m_Tag.AlphaFormat) return false;

		// 画像データの比較.
		if (_dst.m_Tag.Address == NULL && _src.m_Tag.Address != NULL) return false;
		if (_dst.m_Tag.Address != NULL && _src.m_Tag.Address == NULL) return false;
		if (_dst.m_Tag.Address != NULL && _src.m_Tag.Address != NULL)
		{
			const char* dst_addr = (const char*)_dst.m_Tag.Address;
			const char* src_addr = (const char*)_src.m_Tag.Address;
			size_t size = _dst.m_Tag.Model.Size();
			size_t length = (size_t)_dst.m_Tag.Width;
			size_t stride = size * length;
			for(int y=0 ; y<_dst.m_Tag.Height ; y++)
			{
				if (memcmp(dst_addr, src_addr, stride) != 0) return false;
				dst_addr += _dst.m_Tag.Stride;
				src_addr += _src.m_Tag.Stride;
			}
		}

		return true;
	}
	return false;
}

// ============================================================
bool CxGdiImage::IsValid() const
{
	if (m_Tag.Address == NULL) return false;
	if (m_Tag.Width <= 0) return false;
	if (m_Tag.Height <= 0) return false;
	if (m_Tag.Model != TxModel::U8(4)) return false;
	int stride = xie::Axi::CalcStride(m_Tag.Model, m_Tag.Width, XIE_IMAGE_PACKING_SIZE);
	if (m_Tag.Stride != stride) return false;
	return true;
}

// ============================================================
bool CxGdiImage::IsAttached() const
{
	return m_IsAttached;
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
void CxGdiImage::Render(HxModule hcanvas, ExGdiScalingMode mode) const
{
	if (this->IsValid() == false) return;

	// 表示範囲パラメータ取得.
	TxCanvas	canvas;
	IxParam*	dpi = xie::Axi::SafeCast<IxParam>(hcanvas);
	if (dpi == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	dpi->GetParam("Tag", &canvas, TxModel::Default());

	#if defined(_MSC_VER)
	{
		HxModule	hBuffer = NULL;
		dpi->GetParam("Buffer", &hBuffer, TxModel::Ptr(1));
		CxBitmap*	buffer = xie::Axi::SafeCast<CxBitmap>(hBuffer);
		if (buffer != NULL && buffer->IsValid())
		{
			// note:
			/*
				デバイスコンテキストは描画の度に生成/破棄を行うこと.
				各インスタンスが保有するのは得策でない.
				GDIリソースが枯渇すると O/S 全体が動作不能になる.
				~~~~~~~~~~~ 上限は約10,000
			*/
			HDC hdcDst		= buffer->GetHDC();
			HDC hdcScreen	= ::GetDC(NULL);
			HDC hdcSrc		= ::CreateCompatibleDC(hdcScreen);
			::ReleaseDC(NULL, hdcScreen);

			if (hdcSrc != NULL)
			{
				::SelectObject( hdcSrc, m_hBitmap );

				int dcid = ::SaveDC( hdcDst );
				if( dcid )
				{
					// ----------------------------------------------------------------------
					// affine
					TxPointD	pos = this->Location();
					{
						double			mag = canvas.Magnification;
						TxRectangleI	display_rect(0, 0, canvas.Width, canvas.Height);
						TxPointD		vp = canvas.ViewPoint;
						TxRectangleI	eff = CxCanvas::EffectiveRect(display_rect, canvas.BgSize, mag);
						TxRectangleD	vis = CxCanvas::VisibleRect(display_rect, canvas.BgSize, mag, vp);

						switch(mode)
						{
						default:
						case ExGdiScalingMode::None:
							mag = 1.0;
							break;
						case ExGdiScalingMode::TopLeft:
							pos = (this->Location() * mag) + (eff.Location() - (vis.Location() * mag));
							break;
						case ExGdiScalingMode::Center:
							pos = (this->Location() * mag) + (eff.Location() - ((vis.Location() - 0.5) * mag));
							break;
						}

						fnPRV_GDI_WorldTransformReset(hdcDst);

						if (this->Angle() != 0.0)
						{
							TxPointD axis = pos + this->Axis();
							fnPRV_GDI_WorldTransformRotate(hdcDst, axis.X, axis.Y, this->Angle());
						}

						if (mode != ExGdiScalingMode::None)
							fnPRV_GDI_WorldTransformScale(hdcDst, pos.X, pos.Y, mag * m_Tag.MagX, mag * m_Tag.MagY);
					}

					// StretchBlt Mode.
					if (canvas.Halftone == ExBoolean::True)
						::SetStretchBltMode( hdcDst, HALFTONE );
					else
						::SetStretchBltMode( hdcDst, COLORONCOLOR );

					::SetBrushOrgEx( hdcDst, 0, 0, NULL );

					// coordinate
					int dst_x = (int)round(pos.X);
					int dst_y = (int)round(pos.Y);
					int	dst_w = m_Tag.Width;
					int	dst_h = m_Tag.Height;
					int src_x = 0;
					int src_y = 0;
					int src_w = m_Tag.Width;
					int src_h = m_Tag.Height;

					// alpha blend function
					BLENDFUNCTION	blendfunction;
					if (m_Tag.AlphaFormat == ExBoolean::True)
					{
						blendfunction.BlendOp				= AC_SRC_OVER;
						blendfunction.BlendFlags			= 0;
						blendfunction.SourceConstantAlpha	= 255;			// 全体の不透明度: 「画素毎の透過」と同時に使用できない.
						blendfunction.AlphaFormat			= AC_SRC_ALPHA;	// 画素毎の透過  : 1=有効.(各画素の A フィールドを不透明度とする)
					}
					else
					{
						blendfunction.BlendOp				= AC_SRC_OVER;
						blendfunction.BlendFlags			= 0;
						blendfunction.SourceConstantAlpha	= saturate_cast<BYTE>(255 * m_Tag.Alpha);	// 全体の不透明度: 0~255 (0~100%)
						blendfunction.AlphaFormat			= 0;										// 画素毎の透過  : 0=無効.
					}

					// draw
					BOOL status = ::AlphaBlend(
						hdcDst, dst_x, dst_y, dst_w, dst_h,
						hdcSrc, src_x, src_y, src_w, src_h,
						blendfunction
						);

					::RestoreDC( hdcDst, dcid );
				}

				if (hdcSrc != NULL)
				{
					::SelectObject( hdcSrc, NULL );
					::DeleteDC( hdcSrc );
					hdcSrc = NULL;
				}
			}
		}
	}
	#else
	{
		::glPushMatrix();

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

		CxTexture texture;
		texture.Setup();
		if (texture.TextureID() != 0)
		{
			unsigned int tid = texture.TextureID();

			::glEnable		(GL_BLEND);
			::glDisable		(GL_DEPTH_TEST);

			::glPixelStorei	(GL_PACK_ALIGNMENT, XIE_IMAGE_PACKING_SIZE);
			::glPixelStorei	(GL_PACK_ROW_LENGTH, this->Stride()/XIE_IMAGE_PACKING_SIZE);
			::glPixelStorei	(GL_PACK_SWAP_BYTES, GL_FALSE);
			::glPixelStorei	(GL_PACK_LSB_FIRST, GL_FALSE);
			::glPixelStorei	(GL_PACK_SKIP_PIXELS, 0);
			::glPixelStorei	(GL_PACK_SKIP_ROWS, 0);

			::glEnable			(GL_TEXTURE_2D);
			::glBindTexture		(GL_TEXTURE_2D, tid);
			::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
			::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

			// テクスチャを拡大・縮小する方法.
			if (canvas.Halftone == ExBoolean::True)
			{
				::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
				::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
			}
			else
			{
				::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
				::glTexParameteri	(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
			}

			// 画素毎の透過.
			int format = (m_Tag.AlphaFormat == ExBoolean::True) ? GL_RGBA : GL_RGB;

			::glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
			::glTexImage2D(GL_TEXTURE_2D, 0, format, m_Tag.Width, m_Tag.Height, 0, GL_RGBA, GL_UNSIGNED_BYTE, m_Tag.Address);
			{
				// 全体の不透明度.
				::glColor4f( 1.0f, 1.0f, 1.0f, (float)m_Tag.Alpha );

				//double magx = (m_Tag.MagX <= 0) ? 1.0 : m_Tag.MagX;
				//double magy = (m_Tag.MagY <= 0) ? 1.0 : m_Tag.MagY;
				double magx = m_Tag.MagX;
				double magy = m_Tag.MagY;
				double L = m_Tag.X;
				double T = m_Tag.Y;
				double R = L + (m_Tag.Width * magx);
				double B = T + (m_Tag.Height * magy);

				::glBegin( GL_QUADS );
				#if 1
				::glTexCoord2d( 0.0, 0.0 ); glVertex2d( L, T );	// 1:+----+
				::glTexCoord2d( 1.0, 0.0 ); glVertex2d( R, T );	// 2:|1　2|
				::glTexCoord2d( 1.0, 1.0 ); glVertex2d( R, B );	// 3:|4　3|
				::glTexCoord2d( 0.0, 1.0 ); glVertex2d( L, B );	// 4:+----+
				#else
				::glTexCoord2d( 0.0, 0.0 ); glVertex2d( L, B );	// 1:+----+
				::glTexCoord2d( 1.0, 0.0 ); glVertex2d( R, B );	// 2:|4　3|
				::glTexCoord2d( 1.0, 1.0 ); glVertex2d( R, T );	// 3:|1　2|
				::glTexCoord2d( 0.0, 1.0 ); glVertex2d( L, T );	// 4:+----+
				#endif
				::glEnd();
			}
			::glBindTexture	(GL_TEXTURE_2D, 0);
			::glDisable		(GL_TEXTURE_2D);
			::glDisable		(GL_BLEND);
		}
		else
		{
			double magx = m_Tag.MagX;
			double magy = m_Tag.MagY;
			double L = m_Tag.X;
			double T = m_Tag.Y;

			::glEnable(GL_REPLACE);
			::glRasterPos2d( L, T );
			::glPixelZoom( (float)magx, (float)magy );
			::glDrawPixels( m_Tag.Width, m_Tag.Height, GL_RGBA, GL_UNSIGNED_BYTE, m_Tag.Address );	// とても遅い.
			::glDisable(GL_REPLACE);
			::glDisable(GL_BLEND);
		}

		::glPopMatrix();
	}
	#endif
}

// ////////////////////////////////////////////////////////////
// 
// Gdi Inheritance
// 

// ================================================================================
TxPointD CxGdiImage::Location() const
{
	return TxPointD(m_Tag.X, m_Tag.Y);
}

// ================================================================================
void CxGdiImage::Location(TxPointD value)
{
	m_Tag.X = value.X;
	m_Tag.Y = value.Y;
}

// ================================================================================
TxRectangleD CxGdiImage::Bounds() const
{
	auto rect = TxRectangleD(this->Location(), this->VisualSize());
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
double CxGdiImage::Angle() const
{
	return m_Tag.Param.Angle;
}

// ============================================================
void CxGdiImage::Angle( double degree )
{
	m_Tag.Param.Angle = degree;
}

// ============================================================
TxPointD CxGdiImage::Axis() const
{
	return m_Tag.Param.Axis;
}

// ============================================================
void CxGdiImage::Axis( TxPointD value )
{
	m_Tag.Param.Axis = value;
}

// ================================================================================
TxHitPosition CxGdiImage::HitTest( TxPointD position, double margin ) const
{
	if (this->IsValid() == false)
		return TxHitPosition::Default();

	auto rect = TxRectangleD(this->Location(), this->VisualSize());
	auto p0 = xie::Axi::Rotate<TxPointD>(position, this->Location()+this->Axis(), -this->Angle());

	auto hit = fnPRV_GDI_HitTest_Rectangle(p0, margin, rect);
	return hit;
}

// ================================================================================
void CxGdiImage::Modify(HxModule prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
{
	if (auto figure = xie::Axi::SafeCast<CxGdiImage>(prev_figure))
	{
		if (this->IsValid() && figure->IsValid())
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
					auto _prev_position = xie::Axi::Rotate<TxPointD>(prev_position, figure->Location() + figure->Axis(), -figure->Angle());
					auto _move_position = xie::Axi::Rotate<TxPointD>(move_position, figure->Location() + figure->Axis(), -figure->Angle());
					auto mv = _move_position - _prev_position;

					auto bounds = TxRectangleD(figure->Location(), figure->VisualSize());
					auto dummy1 = xie::GDI::CxGdiRectangle(bounds); dummy1.Param( figure->Param() );
					auto dummy2 = dummy1;
					dummy2.Modify(dummy1, prev_position, move_position, margin);
					this->MagX( dummy2.Width() / figure->Width() );
					this->MagY( dummy2.Height() / figure->Height() );

					auto _this_location = this->Location();
					switch (hit.Site)
					{
						case -4:
							this->X( figure->X() + mv.X );
							break;
						case -1:
							this->Y( figure->Y() + mv.Y );
							break;
						case +1:
							this->X( figure->X() + mv.X );
							this->Y( figure->Y() + mv.Y );
							break;
						case +2:
							this->Y( figure->Y() + mv.Y );
							break;
						case +4:
							this->X( figure->X() + mv.X );
							break;
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
TxRGB8x4 CxGdiImage::BkColor() const
{
	return m_Tag.Param.BkColor;
}

// ============================================================
void CxGdiImage::BkColor( TxRGB8x4 value )
{
	m_Tag.Param.BkColor = value;
}

// ============================================================
bool CxGdiImage::BkEnable() const
{
	return (m_Tag.Param.BkEnable == ExBoolean::True);
}

// ============================================================
void CxGdiImage::BkEnable( bool value )
{
	m_Tag.Param.BkEnable = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
TxRGB8x4 CxGdiImage::PenColor() const
{
	return m_Tag.Param.PenColor;
}

// ============================================================
void CxGdiImage::PenColor( TxRGB8x4 value )
{
	m_Tag.Param.PenColor = value;
}

// ============================================================
ExGdiPenStyle CxGdiImage::PenStyle() const
{
	return m_Tag.Param.PenStyle;
}

// ============================================================
void CxGdiImage::PenStyle( ExGdiPenStyle value )
{
	m_Tag.Param.PenStyle = value;
}

// ============================================================
int CxGdiImage::PenWidth() const
{
	return m_Tag.Param.PenWidth;
}

// ============================================================
void CxGdiImage::PenWidth( int value )
{
	m_Tag.Param.PenWidth = value;
}

// ////////////////////////////////////////////////////////////
// 
// Properties
// 

// ============================================================
TxGdi2dParam CxGdiImage::Param() const
{
	return m_Tag.Param;
}

// ============================================================
void CxGdiImage::Param( const TxGdi2dParam& value )
{
	m_Tag.Param = value;
}

// ////////////////////////////////////////////////////////////
// 
// METHOD
// 

// ================================================================================
void CxGdiImage::Resize(const TxSizeI& size)
{
	Resize(size.Width, size.Height);
}

// ================================================================================
void CxGdiImage::Resize(int width, int height)
{
	Dispose();

	if (width == 0 || height == 0) return;

	if (width < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (height < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	try
	{
		TxModel	model	= TxModel::U8(4);
		int		bpp		= model.Size() * 8;					// bits
		int		stride	= xie::Axi::CalcStride(model, width, XIE_IMAGE_PACKING_SIZE);

		#if defined(_MSC_VER)
		void* addr = NULL;
		{
			BITMAPINFO bmi;
			{
				DWORD	dwByteSize = stride * height;

				int		iClrUsed = 0;
				int		iPalletSize = sizeof(RGBQUAD) * iClrUsed;
				UINT	uiBitmapInfoSize = sizeof(BITMAPINFOHEADER) + iPalletSize;

				::ZeroMemory( &bmi, uiBitmapInfoSize );

				bmi.bmiHeader.biSize			= sizeof(BITMAPINFOHEADER);
				bmi.bmiHeader.biWidth			= width;
				bmi.bmiHeader.biHeight			= -height;	// Top-down DIB 
			//	bmi.bmiHeader.biHeight			=  height;	// Bottom-up DIB 
				bmi.bmiHeader.biPlanes			= 1;
				bmi.bmiHeader.biBitCount		= bpp;
				bmi.bmiHeader.biCompression		= BI_RGB;		// An uncompressed format.
				bmi.bmiHeader.biSizeImage		= dwByteSize;
				bmi.bmiHeader.biXPelsPerMeter	= 0;
				bmi.bmiHeader.biYPelsPerMeter	= 0;
				bmi.bmiHeader.biClrUsed			= iClrUsed;
				bmi.bmiHeader.biClrImportant	= 0;
			}

			HDC hScreenDC = ::GetDC(NULL);
			m_hBitmap = ::CreateDIBSection(hScreenDC, &bmi, DIB_RGB_COLORS, &addr, NULL, 0);
			::ReleaseDC(NULL, hScreenDC);
			if (m_hBitmap == NULL)
				throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		}
		#else
		void* addr = xie::Axi::MemoryAlloc( (TxIntPtr)height * stride );
		if (addr == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		#endif

		m_Tag.Address	= addr;
		m_Tag.Width		= width;
		m_Tag.Height	= height;
		m_Tag.Model		= model;
		m_Tag.Stride	= stride;
	}
	catch(const CxException& ex)
	{
		this->Dispose();
		throw ex;
	}
}

// ================================================================================
void CxGdiImage::Reset()
{
	if (IsValid() == false) return;
	for(int y=0 ; y<m_Tag.Height ; y++)
	{
		#if defined(_MSC_VER)
		TxBGR8x4* _src = static_cast<TxBGR8x4*>((*this)[y]);
		#else
		TxRGB8x4* _src = static_cast<TxRGB8x4*>((*this)[y]);
		#endif
		for(int x=0 ; x<m_Tag.Width ; x++)
		{
			_src->R = 0x00;
			_src->G = 0x00;
			_src->B = 0x00;
			_src->A = 0xFF;
		}
	}
}

// ================================================================================
int CxGdiImage::Width() const
{
	return m_Tag.Width;
}

// ================================================================================
int CxGdiImage::Height() const
{
	return m_Tag.Height;
}

// ============================================================
TxModel CxGdiImage::Model() const
{
	return m_Tag.Model;
}

// ============================================================
int CxGdiImage::Stride() const
{
	return m_Tag.Stride;
}

// ============================================================
TxSizeI CxGdiImage::Size() const
{
	return TxSizeI(m_Tag.Width, m_Tag.Height);
}

// ============================================================
TxImageSize CxGdiImage::ImageSize() const
{
	return TxImageSize(m_Tag.Width, m_Tag.Height, m_Tag.Model, 1, 0);
}

// ============================================================
void* CxGdiImage::Address()
{
	return m_Tag.Address;
}

// ============================================================
const void* CxGdiImage::Address() const
{
	return m_Tag.Address;
}

// ============================================================
void* CxGdiImage::operator [] (int y)
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= y && y < m_Tag.Height))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return static_cast<unsigned char*>(m_Tag.Address) + ((TxIntPtr)m_Tag.Stride * y);
}

// ============================================================
const void* CxGdiImage::operator [] (int y) const
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= y && y < m_Tag.Height))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return static_cast<unsigned char*>(m_Tag.Address) + ((TxIntPtr)m_Tag.Stride * y);
}

// ============================================================
void* CxGdiImage::operator () (int y, int x)
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= y && y < m_Tag.Height))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= x && x < m_Tag.Width))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	TxIntPtr size = m_Tag.Model.Size();
	return static_cast<unsigned char*>(m_Tag.Address) + ((TxIntPtr)m_Tag.Stride * y) + (size * x);
}

// ============================================================
const void* CxGdiImage::operator () (int y, int x) const
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= y && y < m_Tag.Height))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= x && x < m_Tag.Width))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	TxIntPtr size = m_Tag.Model.Size();
	return static_cast<unsigned char*>(m_Tag.Address) + ((TxIntPtr)m_Tag.Stride * y) + (size * x);
}

// ================================================================================
double CxGdiImage::X() const
{
	return m_Tag.X;
}

// ================================================================================
void CxGdiImage::X(double value)
{
	m_Tag.X = value;
}

// ================================================================================
double CxGdiImage::Y() const
{
	return m_Tag.Y;
}

// ================================================================================
void CxGdiImage::Y(double value)
{
	m_Tag.Y = value;
}

// ================================================================================
TxSizeD CxGdiImage::VisualSize() const
{
	return TxSizeD(
		m_Tag.Width  * m_Tag.MagX,
		m_Tag.Height * m_Tag.MagY
		);
}

// ================================================================================
double CxGdiImage::MagX() const
{
	return m_Tag.MagX;
}

// ================================================================================
void CxGdiImage::MagX(double value)
{
	m_Tag.MagX = value;
}

// ================================================================================
double CxGdiImage::MagY() const
{
	return m_Tag.MagY;
}

// ================================================================================
void CxGdiImage::MagY(double value)
{
	m_Tag.MagY = value;
}

// ================================================================================
double CxGdiImage::Alpha() const
{
	return m_Tag.Alpha;
}

// ================================================================================
void CxGdiImage::Alpha(double value)
{
	m_Tag.Alpha = value;
}

// ================================================================================
bool CxGdiImage::AlphaFormat() const
{
	return (m_Tag.AlphaFormat == ExBoolean::True);
}

// ================================================================================
void CxGdiImage::AlphaFormat(bool value)
{
	m_Tag.AlphaFormat = (value) ? ExBoolean::True : ExBoolean::False;
}

// ============================================================
void CxGdiImage::GetParam(TxCharCPtrA name, void* value, TxModel model) const
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
void CxGdiImage::SetParam(TxCharCPtrA name, const void* value, TxModel model)
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
