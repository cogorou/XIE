/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/CxDeviceParam.h"

#include "api_media.h"
#include "Core/CxException.h"
#include "Core/CxString.h"

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxDeviceParam";

// ============================================================
void CxDeviceParam::_Constructor()
{
	m_Tag = TxDeviceParam::Default();
}

// ============================================================
CxDeviceParam::CxDeviceParam()
{
	_Constructor();
}

// ============================================================
CxDeviceParam::CxDeviceParam(TxCharCPtrA name, int index)
{
	_Constructor();
	Name(name); 
	Index(index);
}

// ============================================================
CxDeviceParam::CxDeviceParam(TxCharCPtrA name, int index, int pin, TxSizeI size)
{
	_Constructor();
	Name(name); 
	Index(index);
	Pin(pin);
	Size(size);
}

// ============================================================
CxDeviceParam::CxDeviceParam(CxDeviceParam&& src)
{
	_Constructor();
	MoveFrom(src);
}

// ============================================================
CxDeviceParam::CxDeviceParam(const CxDeviceParam& src)
{
	_Constructor();
	CopyFrom(src);
}

// ============================================================
CxDeviceParam::~CxDeviceParam()
{
	Dispose();
}

// ============================================================
CxDeviceParam& CxDeviceParam::operator = ( CxDeviceParam&& src )
{
	MoveFrom(src);
	return *this;
}

// ============================================================
CxDeviceParam& CxDeviceParam::operator = ( const CxDeviceParam& src )
{
	CopyFrom(src);
	return *this;
}

// ============================================================
bool CxDeviceParam::operator == ( const CxDeviceParam& src ) const
{
	return ContentEquals(src);
}

// ============================================================
bool CxDeviceParam::operator != ( const CxDeviceParam& src ) const
{
	return !(CxDeviceParam::operator == (src));
}

// ======================================================================
TxDeviceParam CxDeviceParam::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxDeviceParam::TagPtr() const
{
	return (void*)&m_Tag;
}

//
// IxDisposable
//

// ============================================================
void CxDeviceParam::Dispose()
{
	Name(NULL); 
	m_Tag = TxDeviceParam::Default();
}

// ============================================================
bool CxDeviceParam::IsValid() const
{
	if (m_Tag.Name == NULL) return false;
	if (strlen(m_Tag.Name) == 0) return false;
	if (m_Tag.Index < 0) return false;
	if (m_Tag.Pin < 0) return false;
	return true;
}

// ================================================================================
void CxDeviceParam::MoveFrom(CxDeviceParam& src)
{
	if (this == &src) return;
	CxDeviceParam&	dst = *this;
	dst.m_Tag.Name		= src.m_Tag.Name;
	dst.m_Tag.Index		= src.m_Tag.Index;
	dst.m_Tag.Pin		= src.m_Tag.Pin;
	dst.m_Tag.Width		= src.m_Tag.Width;
	dst.m_Tag.Height	= src.m_Tag.Height;
	src.m_Tag.Name = NULL;
}

// ================================================================================
void CxDeviceParam::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (xie::Axi::ClassIs<CxDeviceParam>(src))
	{
		auto&	_src = static_cast<const CxDeviceParam&>(src);
		auto&	_dst = *this;
		_dst.Name(_src.m_Tag.Name);
		_dst.m_Tag.Index	= _src.m_Tag.Index;
		_dst.m_Tag.Pin		= _src.m_Tag.Pin;
		_dst.m_Tag.Width	= _src.m_Tag.Width;
		_dst.m_Tag.Height	= _src.m_Tag.Height;
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxDeviceParam::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (xie::Axi::ClassIs<CxDeviceParam>(src))
	{
		auto&	_src = static_cast<const CxDeviceParam&>(src);
		auto&	_dst = *this;
		if (_dst.m_Tag.Name	== NULL && _src.m_Tag.Name != NULL) return false;
		if (_dst.m_Tag.Name	!= NULL && _src.m_Tag.Name == NULL) return false;
		if (_dst.m_Tag.Name	!= NULL && _src.m_Tag.Name != NULL)
		{
			if (strcmp(_dst.m_Tag.Name, _src.m_Tag.Name) != 0)
				return false;
		}
		if (_dst.m_Tag.Index	!= _src.m_Tag.Index) return false;
		if (_dst.m_Tag.Pin		!= _src.m_Tag.Pin) return false;
		if (_dst.m_Tag.Width	!= _src.m_Tag.Width) return false;
		if (_dst.m_Tag.Height	!= _src.m_Tag.Height) return false;
		return true;
	}
	return false;
}

// ================================================================================
TxCharCPtrA CxDeviceParam::Name() const
{
	return m_Tag.Name;
}

// ================================================================================
void CxDeviceParam::Name(TxCharCPtrA value)
{
	if (m_Tag.Name == value) return;
	if (m_Tag.Name != NULL)
		Axi::MemoryFree(m_Tag.Name);
	m_Tag.Name = NULL;
	if (value != NULL)
	{
		m_Tag.Name = (char*)Axi::MemoryAlloc((strlen(value) + 1) * sizeof(char));
		strcpy(m_Tag.Name, value);
	}
}

// ================================================================================
int CxDeviceParam::Index() const
{
	return m_Tag.Index;
}

// ================================================================================
void CxDeviceParam::Index(int value)
{
	m_Tag.Index = value;
}

// ================================================================================
int CxDeviceParam::Pin() const
{
	return m_Tag.Pin;
}

// ================================================================================
void CxDeviceParam::Pin(int value)
{
	m_Tag.Pin = value;
}

// ================================================================================
TxSizeI CxDeviceParam::Size() const
{
	return TxSizeI(m_Tag.Width, m_Tag.Height);
}

// ================================================================================
void CxDeviceParam::Size(TxSizeI value)
{
	m_Tag.Width = value.Width;
	m_Tag.Height = value.Height;
}

}
}
