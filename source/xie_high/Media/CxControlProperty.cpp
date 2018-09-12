/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/CxControlProperty.h"
#include "Core/Axi.h"
#include "Core/CxException.h"

#if defined(_MSC_VER)
#include "Media/DS/CxDSControlProperty.h"
typedef xie::Media::CxDSControlProperty	TBODY;	//!< The type only used in this file.
#else
#include "Media/V4L2/CxVLControlProperty.h"
typedef xie::Media::CxVLControlProperty	TBODY;	//!< The type only used in this file.
#endif

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxControlProperty";

// ============================================================
void CxControlProperty::_Constructor()
{
	m_Handle = (HxModule)new TBODY();
}

// ============================================================
CxControlProperty::CxControlProperty()
{
	_Constructor();
}

// ============================================================
CxControlProperty::CxControlProperty(HxModule controller, TxCharCPtrA name)
{
	_Constructor();
	Controller(controller);
	Name(name);
}

// ============================================================
CxControlProperty::CxControlProperty(const CxControlProperty& src)
{
	_Constructor();
	operator = (src);
}

// ============================================================
CxControlProperty::~CxControlProperty()
{
	delete reinterpret_cast<TBODY*>(m_Handle);
}

// ============================================================
IxModule* CxControlProperty::GetModule() const
{
	return reinterpret_cast<IxModule*>(m_Handle);
}

// =================================================================
CxControlProperty& CxControlProperty::operator = ( const CxControlProperty& src )
{
	CopyFrom(src);
	return *this;
}

// =================================================================
bool CxControlProperty::operator == ( const CxControlProperty& src ) const
{
	return ContentEquals(src);
}

// =================================================================
bool CxControlProperty::operator != ( const CxControlProperty& src ) const
{
	return !(operator == (src));
}

// ============================================================
void CxControlProperty::Dispose()
{
	reinterpret_cast<TBODY*>(m_Handle)->Dispose();
}

// ============================================================
bool CxControlProperty::IsValid() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->IsValid();
}

// ================================================================================
void CxControlProperty::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (xie::Axi::ClassIs<CxControlProperty>(src))
	{
		auto&	_src = static_cast<const CxControlProperty&>(src);
		auto&	_dst = *this;
		_dst.CopyFrom(_src);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxControlProperty::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (xie::Axi::ClassIs<CxControlProperty>(src))
	{
		auto&	_src = static_cast<const CxControlProperty&>(src);
		auto&	_dst = *this;
		return _dst.ContentEquals(_src);
	}
	return false;
}

// ============================================================
HxModule CxControlProperty::Controller() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->Controller();
}

// ============================================================
void CxControlProperty::Controller(HxModule value)
{
	reinterpret_cast<TBODY*>(m_Handle)->Controller(value);
}

// ============================================================
TxCharCPtrA CxControlProperty::Name() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->Name();
}

// ============================================================
void CxControlProperty::Name(TxCharCPtrA value)
{
	reinterpret_cast<TBODY*>(m_Handle)->Name(value);
}

// ============================================================
bool CxControlProperty::IsSupported() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->IsSupported();
}

// ============================================================
TxRangeI CxControlProperty::GetRange() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->GetRange();
}

// ============================================================
int CxControlProperty::GetStep() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->GetStep();
}

// ============================================================
int CxControlProperty::GetDefault() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->GetDefault();
}

// ============================================================
int CxControlProperty::GetFlags() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->GetFlags();
}

// ============================================================
void CxControlProperty::SetFlags(int value)
{
	reinterpret_cast<TBODY*>(m_Handle)->SetFlags(value);
}

// ============================================================
int CxControlProperty::GetValue() const
{
	return reinterpret_cast<TBODY*>(m_Handle)->GetValue();
}

// ============================================================
void CxControlProperty::SetValue(int value, bool relative)
{
	reinterpret_cast<TBODY*>(m_Handle)->SetValue(value, relative);
}

}
}
