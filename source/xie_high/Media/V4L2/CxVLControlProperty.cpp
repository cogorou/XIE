/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#include "Media/V4L2/CxVLControlProperty.h"
#include "Core/Axi.h"
#include "Core/CxException.h"

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxVLControlProperty";

// ============================================================
void CxVLControlProperty::_Constructor()
{
	m_Controller = NULL;
}

// ============================================================
CxVLControlProperty::CxVLControlProperty()
{
	_Constructor();
}

// ============================================================
CxVLControlProperty::CxVLControlProperty(HxModule controller, TxCharCPtrA name)
{
	_Constructor();
	m_Controller	= controller;
	m_Name			= name;
}

// ============================================================
CxVLControlProperty::CxVLControlProperty(const CxVLControlProperty& src)
{
	_Constructor();
	operator = (src);
}

// ============================================================
CxVLControlProperty::~CxVLControlProperty()
{
	Dispose();
}

// =================================================================
CxVLControlProperty& CxVLControlProperty::operator = ( const CxVLControlProperty& src )
{
	CopyFrom(src);
	return *this;
}

// =================================================================
bool CxVLControlProperty::operator == ( const CxVLControlProperty& src ) const
{
	return ContentEquals(src);
}

// =================================================================
bool CxVLControlProperty::operator != ( const CxVLControlProperty& src ) const
{
	return !(operator == (src));
}

// ============================================================
void CxVLControlProperty::Dispose()
{
	m_Controller = NULL;
	m_Name = NULL;
}

// ============================================================
bool CxVLControlProperty::IsValid() const
{
	if (m_Name.IsValid() == false) return false;
	if (xie::Axi::ClassIs<CxModule>(m_Controller) == false) return false;
	return true;
}

// ================================================================================
void CxVLControlProperty::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (xie::Axi::ClassIs<CxVLControlProperty>(src))
	{
		auto&	_src = static_cast<const CxVLControlProperty&>(src);
		auto&	_dst = *this;
		_dst.m_Controller	= _src.m_Controller;
		_dst.m_Name			= _src.m_Name;
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxVLControlProperty::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (xie::Axi::ClassIs<CxVLControlProperty>(src))
	{
		auto&	_src = static_cast<const CxVLControlProperty&>(src);
		auto&	_dst = *this;
		if (_dst.m_Controller	!= _src.m_Controller) return false;
		if (_dst.m_Name			!= _src.m_Name) return false;
		return true;
	}
	return false;
}

// ============================================================
HxModule CxVLControlProperty::Controller() const
{
	return m_Controller;
}

// ============================================================
void CxVLControlProperty::Controller(HxModule value)
{
	m_Controller = value;
}

// ============================================================
TxCharCPtrA CxVLControlProperty::Name() const
{
	return m_Name;
}

// ============================================================
void CxVLControlProperty::Name(TxCharCPtrA value)
{
	m_Name = value;
}

// ============================================================
bool CxVLControlProperty::IsSupported() const
{
	return false;
}

// ============================================================
TxRangeI CxVLControlProperty::GetRange() const
{
	return TxRangeI();
}

// ============================================================
int CxVLControlProperty::GetStep() const
{
	return 0;
}

// ============================================================
int CxVLControlProperty::GetDefault() const
{
	return 0;
}

// ============================================================
int CxVLControlProperty::GetFlags() const
{
	return 0;
}

// ============================================================
void CxVLControlProperty::SetFlags(int value)
{
}

// ============================================================
int CxVLControlProperty::GetValue() const
{
	return 0;
}

// ============================================================
void CxVLControlProperty::SetValue(int value, bool relative)
{
}

}
}

#endif	// _MCS_VER
