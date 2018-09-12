/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "GDI/CxTexture.h"

#include "api_gdi.h"
#include "Core/CxImage.h"
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

static const char* g_ClassName = "CxTexture";

// ======================================================================
void CxTexture::_Constructor()
{
	m_TextureID = 0;
}

// ======================================================================
CxTexture::CxTexture()
{
	_Constructor();
}

// ======================================================================
CxTexture::CxTexture(const CxTexture& src)
{
	_Constructor();
	operator = (src);
}

// ======================================================================
CxTexture::~CxTexture()
{
	Dispose();
}

// ============================================================
CxTexture& CxTexture::operator = ( const CxTexture& src )
{
	if (this == &src) return *this;
	return *this;
}

// ============================================================
bool CxTexture::operator == ( const CxTexture& cmp ) const
{
	if (this == &cmp) return true;
	return false;
}

// ============================================================
bool CxTexture::operator != ( const CxTexture& cmp ) const
{
	return !(operator == (cmp));
}

// ============================================================
void CxTexture::Dispose()
{
	if (m_TextureID != 0)
		::glDeleteTextures(1, &m_TextureID);
	m_TextureID = 0;
}

// ============================================================
bool CxTexture::IsValid() const
{
	if (m_TextureID == 0) return false;
	return true;
}

// ======================================================================
void CxTexture::Setup()
{
	Dispose();

	// テクスチャ.
	::glGenTextures(1, &m_TextureID);
}

// ======================================================================
unsigned int CxTexture::TextureID() const
{
	return m_TextureID;
}

}
}
