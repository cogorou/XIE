/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/CxScreenListItem.h"

#include "api_media.h"
#include "Core/CxException.h"
#include "Core/CxString.h"

#include <utility>

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxScreenListItem";

// ============================================================
void CxScreenListItem::_Constructor()
{
	m_Tag = TxScreenListItem::Default();
}

// ============================================================
CxScreenListItem::CxScreenListItem()
{
	_Constructor();
}

#if defined(_MSC_VER)
// ============================================================
CxScreenListItem::CxScreenListItem(HWND handle, TxCharCPtrA name, TxRectangleI bounds)
{
	_Constructor();
	Handle(handle);
	Name(name);
	Bounds(bounds);
}
#else
// ============================================================
CxScreenListItem::CxScreenListItem(TxIntPtr handle, TxCharCPtrA name, TxRectangleI bounds)
{
	_Constructor();
	Handle(handle);
	Name(name);
	Bounds(bounds);
}
#endif

// ============================================================
CxScreenListItem::CxScreenListItem(CxScreenListItem&& src)
{
	_Constructor();
	MoveFrom(src);
}

// ============================================================
CxScreenListItem::CxScreenListItem(const CxScreenListItem& src)
{
	_Constructor();
	CopyFrom(src);
}

// ============================================================
CxScreenListItem::~CxScreenListItem()
{
	Dispose();
}

// ============================================================
CxScreenListItem& CxScreenListItem::operator = ( CxScreenListItem&& src )
{
	MoveFrom(src);
	return *this;
}

// ============================================================
CxScreenListItem& CxScreenListItem::operator = ( const CxScreenListItem& src )
{
	CopyFrom(src);
	return *this;
}

// ============================================================
bool CxScreenListItem::operator == ( const CxScreenListItem& src ) const
{
	return ContentEquals(src);
}

// ============================================================
bool CxScreenListItem::operator != ( const CxScreenListItem& src ) const
{
	return !(CxScreenListItem::operator == (src));
}

// ======================================================================
TxScreenListItem CxScreenListItem::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxScreenListItem::TagPtr() const
{
	return (void*)&m_Tag;
}

//
// IxDisposable
//

// ============================================================
void CxScreenListItem::Dispose()
{
	Name(NULL); 
	m_Tag = TxScreenListItem::Default();
}

// ============================================================
bool CxScreenListItem::IsValid() const
{
	return true;
}

// ================================================================================
void CxScreenListItem::MoveFrom(CxScreenListItem& src)
{
	if (this == &src) return;
	CxScreenListItem&	dst = *this;
	dst.Dispose();
	dst.m_Tag.Handle	= src.m_Tag.Handle;
	dst.m_Tag.Name		= src.m_Tag.Name;
	dst.m_Tag.X			= src.m_Tag.X;
	dst.m_Tag.Y			= src.m_Tag.Y;
	dst.m_Tag.Width		= src.m_Tag.Width;
	dst.m_Tag.Height	= src.m_Tag.Height;
	src.m_Tag.Name = NULL;
}

// ================================================================================
void CxScreenListItem::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (xie::Axi::ClassIs<CxScreenListItem>(src))
	{
		auto&	_src = static_cast<const CxScreenListItem&>(src);
		auto&	_dst = *this;
		_dst.Dispose();
		_dst.m_Tag.Handle	= _src.m_Tag.Handle;
		_dst.m_Tag.X		= _src.m_Tag.X;
		_dst.m_Tag.Y		= _src.m_Tag.Y;
		_dst.m_Tag.Width	= _src.m_Tag.Width;
		_dst.m_Tag.Height	= _src.m_Tag.Height;
		_dst.Name(_src.m_Tag.Name);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxScreenListItem::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (xie::Axi::ClassIs<CxScreenListItem>(src))
	{
		auto&	_src = static_cast<const CxScreenListItem&>(src);
		auto&	_dst = *this;
		if (_dst.m_Tag.Handle	!= _src.m_Tag.Handle) return false;
		if (_dst.m_Tag.X		!= _src.m_Tag.X) return false;
		if (_dst.m_Tag.Y		!= _src.m_Tag.Y) return false;
		if (_dst.m_Tag.Width	!= _src.m_Tag.Width) return false;
		if (_dst.m_Tag.Height	!= _src.m_Tag.Height) return false;
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

#if defined(_MSC_VER)
// ================================================================================
HWND CxScreenListItem::Handle() const
{
	return m_Tag.Handle;
}

// ================================================================================
void CxScreenListItem::Handle(HWND value)
{
	m_Tag.Handle = value;
}
#else
// ================================================================================
TxIntPtr CxScreenListItem::Handle() const
{
	return m_Tag.Handle;
}

// ================================================================================
void CxScreenListItem::Handle(TxIntPtr value)
{
	m_Tag.Handle = value;
}
#endif

// ================================================================================
TxCharCPtrA CxScreenListItem::Name() const
{
	return m_Tag.Name;
}

// ================================================================================
void CxScreenListItem::Name(TxCharCPtrA value)
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
TxRectangleI CxScreenListItem::Bounds() const
{
	return TxRectangleI(m_Tag.X, m_Tag.Y, m_Tag.Width, m_Tag.Height);
}

// ================================================================================
void CxScreenListItem::Bounds(TxRectangleI value)
{
	m_Tag.X			= value.X;
	m_Tag.Y			= value.Y;
	m_Tag.Width		= value.Width;
	m_Tag.Height	= value.Height;
}

}
}
