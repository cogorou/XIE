/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/DS/CxDSControlProperty.h"
#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxDSControlProperty";

// ============================================================
void CxDSControlProperty::_Constructor()
{
}

// ============================================================
CxDSControlProperty::CxDSControlProperty()
{
	_Constructor();
}

// ============================================================
CxDSControlProperty::CxDSControlProperty(HxModule controller, TxCharCPtrA name)
{
	_Constructor();
	m_Wrapper.Controller(controller);
	m_Wrapper.Name(name);
}

// ============================================================
CxDSControlProperty::CxDSControlProperty(const CxDSControlProperty& src)
{
	_Constructor();
	operator = (src);
}

// ============================================================
CxDSControlProperty::~CxDSControlProperty()
{
	Dispose();
}

// =================================================================
CxDSControlProperty& CxDSControlProperty::operator = ( const CxDSControlProperty& src )
{
	CopyFrom(src);
	return *this;
}

// =================================================================
bool CxDSControlProperty::operator == ( const CxDSControlProperty& src ) const
{
	return ContentEquals(src);
}

// =================================================================
bool CxDSControlProperty::operator != ( const CxDSControlProperty& src ) const
{
	return !(operator == (src));
}

// ============================================================
void CxDSControlProperty::Dispose()
{
	m_Wrapper.Dispose();
}

// ============================================================
bool CxDSControlProperty::IsValid() const
{
	return m_Wrapper.IsValid();
}

// ================================================================================
void CxDSControlProperty::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (xie::Axi::ClassIs<CxDSControlProperty>(src))
	{
		auto&	_src = static_cast<const CxDSControlProperty&>(src);
		auto&	_dst = *this;
		_dst.m_Wrapper	= _src.m_Wrapper;
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxDSControlProperty::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (xie::Axi::ClassIs<CxDSControlProperty>(src))
	{
		auto&	_src = static_cast<const CxDSControlProperty&>(src);
		auto&	_dst = *this;
		if (_dst.m_Wrapper	!= _src.m_Wrapper) return false;
		return true;
	}
	return false;
}

// ============================================================
HxModule CxDSControlProperty::Controller() const
{
	return m_Wrapper.Controller();
}

// ============================================================
void CxDSControlProperty::Controller(HxModule value)
{
	m_Wrapper.Controller(value);
}

// ============================================================
TxCharCPtrA CxDSControlProperty::Name() const
{
	return m_Wrapper.Name();
}

// ============================================================
void CxDSControlProperty::Name(TxCharCPtrA value)
{
	m_Wrapper.Name(value);
}

// ============================================================
bool CxDSControlProperty::IsSupported() const
{
	return m_Wrapper.IsSupported();
}

// ============================================================
TxRangeI CxDSControlProperty::GetRange() const
{
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	long minval = 0, maxval = 0, step = 0, defval = 0, flags = 0;
	HRESULT hr = m_Wrapper.GetRange(&minval, &maxval, &step, &defval, &flags);
	if (FAILED(hr))
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	return TxRangeI(minval, maxval);
}

// ============================================================
int CxDSControlProperty::GetStep() const
{
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	long minval = 0, maxval = 0, step = 0, defval = 0, flags = 0;
	HRESULT hr = m_Wrapper.GetRange(&minval, &maxval, &step, &defval, &flags);
	if (FAILED(hr))
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	return step;
}

// ============================================================
int CxDSControlProperty::GetDefault() const
{
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	long minval = 0, maxval = 0, step = 0, defval = 0, flags = 0;
	HRESULT hr = m_Wrapper.GetRange(&minval, &maxval, &step, &defval, &flags);
	if (FAILED(hr))
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	return defval;
}

// ============================================================
int CxDSControlProperty::GetFlags() const
{
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	long minval = 0, maxval = 0, step = 0, defval = 0, flags = 0;
	HRESULT hr = m_Wrapper.GetRange(&minval, &maxval, &step, &defval, &flags);
	if (FAILED(hr))
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	return flags;
}

// ============================================================
void CxDSControlProperty::SetFlags(int value)
{
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	long current = 0, flags = 0;
	HRESULT hr = m_Wrapper.Get(&current, &flags);
	if (FAILED(hr))
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	hr = m_Wrapper.Set(current, value);
	if (FAILED(hr))
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
int CxDSControlProperty::GetValue() const
{
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	long value = 0, flags = 0;
	HRESULT hr = m_Wrapper.Get(&value, &flags);
	if (FAILED(hr))
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

	return value;
}

// ============================================================
void CxDSControlProperty::SetValue(int value, bool relative)
{
	if (IsValid() == false)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);

	long flags = CameraControl_Flags_Manual;
	if (relative)
		flags |= 0x0010;	// relative;

	HRESULT hr = m_Wrapper.Set(value, flags);
	if (FAILED(hr))
		throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);
}

}
}
