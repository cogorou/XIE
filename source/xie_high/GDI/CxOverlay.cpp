/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_gdi.h"
#include "GDI/CxOverlay.h"

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

// ////////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLE

// ////////////////////////////////////////////////////////////////////////////////
// FUNCTION

// ======================================================================
void CxOverlay::_Constructor()
{
	m_Visible = true;
}

// ================================================================================
CxOverlay::CxOverlay()
{
	_Constructor();
}

// ================================================================================
CxOverlay::CxOverlay(const CxOverlay& src)
{
	_Constructor();
	CopyFrom(src);
}

// ================================================================================
CxOverlay::~CxOverlay()
{
}

// ================================================================================
CxOverlay& CxOverlay::operator = ( const CxOverlay& src )
{
	CopyFrom(src);
	return *this;
}

// ================================================================================
bool CxOverlay::operator == ( const CxOverlay& src ) const
{
	return ContentEquals(src);
}

// ================================================================================
bool CxOverlay::operator != ( const CxOverlay& src ) const
{
	return !ContentEquals(src);
}

//
// IxEquatable
//

// ================================================================================
void CxOverlay::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (auto psrc = xie::Axi::SafeCast<CxOverlay>(&src)) 
	{
		this->m_Visible	= psrc->m_Visible;
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxOverlay::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (auto psrc = xie::Axi::SafeCast<CxOverlay>(&src)) 
	{
		if (this->m_Visible	!= psrc->m_Visible) return false;
		return true;
	}
	return false;
}

//
// IxParam
//

// ============================================================
void CxOverlay::GetParam(TxCharCPtrA name, void* value, TxModel model) const
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Visible") == 0)
	{
		if (fnPRV_GDI_GetParam(value, model, m_Visible)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
void CxOverlay::SetParam(TxCharCPtrA name, const void* value, TxModel model)
{
	if (name == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	if (value == NULL)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	if (strcmp(name, "Visible") == 0)
	{
		if (fnPRV_GDI_SetParam(value, model, m_Visible)) return;
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	}

	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
bool CxOverlay::Visible() const
{
	return m_Visible;
}

// ============================================================
void CxOverlay::Visible( bool value )
{
	m_Visible = value;
}

}	// GDI
}	// xie
