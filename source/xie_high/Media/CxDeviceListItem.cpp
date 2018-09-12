/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/CxDeviceListItem.h"

#include "api_media.h"
#include "Core/CxException.h"
#include "Core/CxString.h"

#include <utility>

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxDeviceListItem";

// ============================================================
void CxDeviceListItem::_Constructor()
{
	m_Tag = TxDeviceListItem::Default();
}

// ============================================================
CxDeviceListItem::CxDeviceListItem()
{
	_Constructor();
}

// ============================================================
CxDeviceListItem::CxDeviceListItem(ExMediaType type, ExMediaDir dir, TxCharCPtrA name, int index)
{
	_Constructor();
	MediaType(type);
	MediaDir(dir);
	Name(NULL); 
	Index(index);
}

// ============================================================
CxDeviceListItem::CxDeviceListItem(CxDeviceListItem&& src)
{
	_Constructor();
	MoveFrom(src);
}

// ============================================================
CxDeviceListItem::CxDeviceListItem(const CxDeviceListItem& src)
{
	_Constructor();
	CopyFrom(src);
}

// ============================================================
CxDeviceListItem::~CxDeviceListItem()
{
	Dispose();
}

// ============================================================
CxDeviceListItem& CxDeviceListItem::operator = ( CxDeviceListItem&& src )
{
	MoveFrom(src);
	return *this;
}

// ============================================================
CxDeviceListItem& CxDeviceListItem::operator = ( const CxDeviceListItem& src )
{
	CopyFrom(src);
	return *this;
}

// ============================================================
bool CxDeviceListItem::operator == ( const CxDeviceListItem& src ) const
{
	return ContentEquals(src);
}

// ============================================================
bool CxDeviceListItem::operator != ( const CxDeviceListItem& src ) const
{
	return !(CxDeviceListItem::operator == (src));
}

// ======================================================================
TxDeviceListItem CxDeviceListItem::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxDeviceListItem::TagPtr() const
{
	return (void*)&m_Tag;
}

//
// IxDisposable
//

// ============================================================
void CxDeviceListItem::Dispose()
{
	Name(NULL); 
	m_Tag = TxDeviceListItem::Default();
}

// ============================================================
bool CxDeviceListItem::IsValid() const
{
	return true;
}

// ================================================================================
void CxDeviceListItem::MoveFrom(CxDeviceListItem& src)
{
	if (this == &src) return;
	CxDeviceListItem&	dst = *this;
	dst.Dispose();
	dst.m_Tag.MediaType	= src.m_Tag.MediaType;
	dst.m_Tag.MediaDir	= src.m_Tag.MediaDir;
	dst.m_Tag.Name		= src.m_Tag.Name;
	dst.m_Tag.Index		= src.m_Tag.Index;
	src.m_Tag.Name = NULL;
}

// ================================================================================
void CxDeviceListItem::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (xie::Axi::ClassIs<CxDeviceListItem>(src))
	{
		auto&	_src = static_cast<const CxDeviceListItem&>(src);
		auto&	_dst = *this;
		_dst.Dispose();
		_dst.m_Tag.MediaType	= _src.m_Tag.MediaType;
		_dst.m_Tag.MediaDir		= _src.m_Tag.MediaDir;
		_dst.m_Tag.Index		= _src.m_Tag.Index;
		_dst.Name(_src.m_Tag.Name);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxDeviceListItem::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (xie::Axi::ClassIs<CxDeviceListItem>(src))
	{
		auto&	_src = static_cast<const CxDeviceListItem&>(src);
		auto&	_dst = *this;
		if (_dst.m_Tag.MediaType	!= _src.m_Tag.MediaType) return false;
		if (_dst.m_Tag.MediaDir		!= _src.m_Tag.MediaDir) return false;
		if (_dst.m_Tag.Index		!= _src.m_Tag.Index) return false;
		if (_dst.m_Tag.Name	== NULL && _src.m_Tag.Name != NULL) return false;
		if (_dst.m_Tag.Name	!= NULL && _src.m_Tag.Name == NULL) return false;
		if (_dst.m_Tag.Name	!= NULL && _src.m_Tag.Name != NULL)
		{
			if (strcmp(_dst.m_Tag.Name, _src.m_Tag.Name) != 0)
				return false;
		}
		return true;
	}
	return false;
}
// ================================================================================
ExMediaType	CxDeviceListItem::MediaType() const
{
	return m_Tag.MediaType;
}

// ================================================================================
void CxDeviceListItem::MediaType(ExMediaType value)
{
	m_Tag.MediaType = value;
}

// ================================================================================
ExMediaDir CxDeviceListItem::MediaDir() const
{
	return m_Tag.MediaDir;
}

// ================================================================================
void CxDeviceListItem::MediaDir(ExMediaDir value)
{
	m_Tag.MediaDir = value;
}

// ================================================================================
TxCharCPtrA CxDeviceListItem::Name() const
{
	return m_Tag.Name;
}

// ================================================================================
void CxDeviceListItem::Name(TxCharCPtrA value)
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
int CxDeviceListItem::Index() const
{
	return m_Tag.Index;
}

// ================================================================================
void CxDeviceListItem::Index(int value)
{
	m_Tag.Index = value;
}

// ================================================================================
CxStringA CxDeviceListItem::GetProductName() const
{
#if defined(_MSC_VER)
	return fnPRV_DS_GetProductName(m_Tag.MediaType, m_Tag.MediaDir, m_Tag.Name, m_Tag.Index);
#else
	return fnPRV_VL_GetProductName(m_Tag.MediaType, m_Tag.MediaDir, m_Tag.Name, m_Tag.Index);
#endif
}

// ================================================================================
CxArrayEx<CxStringA> CxDeviceListItem::GetPinNames() const
{
#if defined(_MSC_VER)
	return fnPRV_DS_GetPinNames(m_Tag.MediaType, m_Tag.MediaDir, m_Tag.Name, m_Tag.Index);
#else
	return fnPRV_VL_GetPinNames(m_Tag.MediaType, m_Tag.MediaDir, m_Tag.Name, m_Tag.Index);
#endif
}

// ================================================================================
CxArrayEx<TxSizeI> CxDeviceListItem::GetFrameSizes(int pin) const
{
#if defined(_MSC_VER)
	return fnPRV_DS_GetFrameSizes(m_Tag.MediaType, m_Tag.MediaDir, m_Tag.Name, m_Tag.Index, pin);
#else
	return fnPRV_VL_GetFrameSizes(m_Tag.MediaType, m_Tag.MediaDir, m_Tag.Name, m_Tag.Index, pin);
#endif
}

}
}
