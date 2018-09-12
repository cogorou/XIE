/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "GDI/CxBitmap.h"

#include "api_gdi.h"
#include "Core/CxImage.h"
#include "Core/CxException.h"
#include "Core/Axi.h"
#include "Effectors/CxRgbToGray.h"

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

static const char* g_ClassName = "CxBitmap";

// ======================================================================
void CxBitmap::_Constructor()
{
	m_Tag = TxBitmap::Default();

#if defined(_MSC_VER)
	m_hDC		= NULL;
	m_hBitmap	= NULL;
#else
	m_XImage	= NULL;
	m_XServer	= NULL;
	m_XVisual	= NULL;
#endif
}

// ======================================================================
CxBitmap::CxBitmap()
{
	_Constructor();
}

// ======================================================================
CxBitmap::CxBitmap(const CxBitmap& src)
{
	_Constructor();
	operator = (src);
}

// ======================================================================
CxBitmap::CxBitmap(const TxSizeI& size)
{
	_Constructor();
	Resize(size);
}

// ======================================================================
CxBitmap::CxBitmap(int width, int height)
{
	_Constructor();
	Resize(width, height);
}

// ======================================================================
CxBitmap::~CxBitmap()
{
	Dispose();
}

// ============================================================
CxBitmap& CxBitmap::operator = ( const CxBitmap& src )
{
	if (this == &src) return *this;

	CxBitmap& dst = *this;
	if (src.IsValid() == false)
		dst.Dispose();
	else
	{
		if (dst.Size() != src.Size())
			dst.Resize(src.Size());

		CxImage _dst; _dst.Attach(dst.Tag());
		CxImage _src; _src.Attach(src.Tag());
		_dst.Filter().Copy(_src);
	}
	return *this;
}

// ============================================================
bool CxBitmap::operator == ( const CxBitmap& cmp ) const
{
	if (this == &cmp) return true;

	const CxBitmap& src = *this;
	if (src.IsValid() != cmp.IsValid()) return false;
	if (src.IsValid() && cmp.IsValid())
	{
		CxImage _src; _src.Attach(src.Tag());
		CxImage _cmp; _cmp.Attach(cmp.Tag());
		if (_src != _cmp) return false;
	}
	return true;
}

// ============================================================
bool CxBitmap::operator != ( const CxBitmap& cmp ) const
{
	return !(operator == (cmp));
}

// ======================================================================
TxBitmap CxBitmap::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxBitmap::TagPtr() const
{
	return (void*)&m_Tag;
}

// ================================================================================
CxBitmap::operator CxImage() const
{
	CxImage dst;
	this->CopyTo(dst);
	return dst;
}

// ============================================================
void CxBitmap::CopyTo(IxModule& dst) const
{
	if (this->IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	if (xie::Axi::ClassIs<CxImage>(dst))
	{
		CxImage		_src;	_src.Attach( this->Tag() );
		CxImage&	_dst = static_cast<CxImage&>(dst);
		if (_dst.IsValid() == false)
			_dst.Resize(_src.ImageSize());

#if defined(_MSC_VER)
		switch(_dst.Model().Pack)
		{
		case 1:
			switch(_dst.Channels())
			{
			case 1:
				xie::Effectors::CxRgbToGray(0, 0.114, 0.587, 0.299).Execute(_src, _dst);	// ŒW”‚ð BGR ‚Ì‡‚ÉŽw’è‚µ‚Ä‚¢‚é.
				break;
			case 3:
			case 4:
				_dst.Filter().RgbToBgr(_src);	// dst=RGB © src=BGR (Method –¼‚Æ‚ÍˆÓ–¡‚ª”½“]‚µ‚Ä‚¢‚é.)
				break;
			default:
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		case 3:
		case 4:
			_dst.Filter().RgbToBgr(_src);		// dst=RGB © src=BGR (Method –¼‚Æ‚ÍˆÓ–¡‚ª”½“]‚µ‚Ä‚¢‚é.)
			break;
		default:
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		}
#else
		switch(_dst.Model().Pack)
		{
		case 1:
			switch(_dst.Channels())
			{
			case 1:
				xie::Effectors::CxRgbToGray(0, 0.299, 0.587, 0.114).Execute(_src, _dst);	// ŒW”‚ð RGB ‚Ì‡‚ÉŽw’è‚µ‚Ä‚¢‚é.
				break;
			case 3:
			case 4:
				_dst.Filter().Copy(_src);
				break;
			default:
				throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
			}
			break;
		case 3:
		case 4:
			_dst.Filter().Copy(_src);
			break;
		default:
			throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
		}
#endif
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ======================================================================
void CxBitmap::Dispose()
{
	Free();

#if defined(_MSC_VER)
	if (m_hDC != NULL)
		::DeleteDC( m_hDC );
	m_hDC = NULL;
#else
	if (m_XVisual != NULL)
		fnPRV_GDI_XVisual_Close(m_XVisual);
	m_XVisual = NULL;

	if (m_XServer != NULL)
		fnPRV_GDI_XServer_Close(m_XServer);
	m_XServer = NULL;
#endif
}

// ============================================================
void CxBitmap::Free()
{
#if defined(_MSC_VER)
	if (m_hDC != NULL)
		::SelectObject( m_hDC, NULL );

	if (m_hBitmap != NULL)
		::DeleteObject( m_hBitmap );
	m_hBitmap = NULL;
#else
	if (m_XImage != NULL)
	{
		if (m_XImage->data != NULL)
			xie::Axi::MemoryFree(m_XImage->data);
		m_XImage->data = NULL;
		XDestroyImage( m_XImage );
	}
	m_XImage = NULL;
#endif

	m_Tag = TxBitmap::Default();
}

// ============================================================
bool CxBitmap::IsValid() const
{
#if defined(_MSC_VER)
	if (m_hDC == NULL) return false;
	if (m_hBitmap == NULL) return false;
#else
	if (m_XImage == NULL) return false;
#endif
	if (m_Tag.Width <= 0) return false;
	if (m_Tag.Height <= 0) return false;
	return true;
}

// ======================================================================
void CxBitmap::Resize(const TxSizeI& size)
{
	Resize(size.Width, size.Height);
}

// ======================================================================
void CxBitmap::Resize(int width, int height)
{
	Free();

	if (width < 0 || height < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (width == 0)
		width = 1;
	if (height == 0)
		height = 1;

#if defined(_MSC_VER)
	try
	{
		TxModel model	= TxModel::U8(4);					// ModelOf<TxRGB8x4>()
		int pixelsize	= model.Size();						// bytes
		int bpp			= pixelsize * 8;					// bits
		int wbytes		= ((width * bpp + 31) >> 5) << 2;	// 4bytes packing

		if (m_hDC == NULL)
		{
			m_hDC = fnPRV_GDI_CreateDC();
			if (m_hDC == NULL)
				throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		}

		BITMAPINFO bmi;
		{
			DWORD	dwByteSize = wbytes * height;

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

		void* addr = NULL;

		m_hBitmap = ::CreateDIBSection(m_hDC, &bmi, DIB_RGB_COLORS, &addr, NULL, 0);
		if (m_hBitmap == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);

		::SelectObject( m_hDC, m_hBitmap );

		m_Tag.Address	= addr;
		m_Tag.Width		= width;
		m_Tag.Height	= height;
		m_Tag.Stride	= wbytes;
		m_Tag.Model		= model;
	}
	catch(const CxException& ex)
	{
		Dispose();
		throw ex;
	}
#else
	try
	{
		if (m_XServer == NULL)
		{
			m_XServer = fnPRV_GDI_XServer_Open();
			if (m_XServer == NULL)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}

		if (m_XVisual == NULL)
		{
			m_XVisual = fnPRV_GDI_XVisual_Open(m_XServer);
			if (m_XVisual == NULL)
				throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
		}

		TxModel model	= TxModel::U8(4);					// ModelOf<TxRGB8x4>()
		int pixelsize	= model.Size();						// bytes
		int bpp			= pixelsize * 8;					// bits
		int wbytes		= ((width * bpp + 31) >> 5) << 2;	// 4bytes packing

		m_XImage = ::XCreateImage(
					m_XServer,
					m_XVisual->visual,
					bpp,
					ZPixmap,
					0,
					0,
					width,
					height,
					32,
					0
				);
		if (m_XImage == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);

		m_XImage->data = (char*)xie::Axi::MemoryAlloc((TxIntPtr)width * (TxIntPtr)height * pixelsize);
		if (m_XImage->data == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
			
		m_Tag.Address	= m_XImage->data;
		m_Tag.Width		= width;
		m_Tag.Height	= height;
		m_Tag.Stride	= wbytes;
		m_Tag.Model	= model;
	}
	catch(const CxException& ex)
	{
		Dispose();
		throw ex;
	}
#endif
}

// ======================================================================
void CxBitmap::Clear(TxRGB8x4 value)
{
	if (m_Tag.Address != NULL)
	{
		CxImage		src;
		src.Attach( m_Tag );
		src.Clear(value);
	}
}

#if defined(_MSC_VER)
// ======================================================================
HDC CxBitmap::GetHDC() const
{
	return m_hDC;
}

// ======================================================================
HBITMAP CxBitmap::GetHBITMAP() const
{
	return m_hBitmap;
}
#else
// ======================================================================
XImage* CxBitmap::GetXImage() const
{
	return m_XImage;
}

// ============================================================
Display* CxBitmap::XServer() const
{
	return m_XServer;
}

// ============================================================
XVisualInfo* CxBitmap::XVisual() const
{
	return m_XVisual;
}
#endif

// ======================================================================
void* CxBitmap::Address() const
{
	return m_Tag.Address;
}

// ======================================================================
int CxBitmap::Width() const
{
	return m_Tag.Width;
}

// ======================================================================
int CxBitmap::Height() const
{
	return m_Tag.Height;
}

// ============================================================
TxModel CxBitmap::Model() const
{
	return m_Tag.Model;
}

// ======================================================================
int CxBitmap::Stride() const
{
	return m_Tag.Stride;
}

// ============================================================
TxSizeI CxBitmap::Size() const
{
	return TxSizeI(m_Tag.Width, m_Tag.Height);
}

// ============================================================
TxImageSize CxBitmap::ImageSize() const
{
	return TxImageSize(m_Tag.Width, m_Tag.Height, m_Tag.Model, 1, 0);
}

// ============================================================
void* CxBitmap::operator [] (int y)
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= y && y < m_Tag.Height))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return static_cast<unsigned char*>(m_Tag.Address) + ((TxIntPtr)m_Tag.Stride * y);
}

// ============================================================
const void* CxBitmap::operator [] (int y) const
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= y && y < m_Tag.Height))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return static_cast<unsigned char*>(m_Tag.Address) + ((TxIntPtr)m_Tag.Stride * y);
}

// ============================================================
void* CxBitmap::operator () (int y, int x)
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
const void* CxBitmap::operator () (int y, int x) const
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

}
}
