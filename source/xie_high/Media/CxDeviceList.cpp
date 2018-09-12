/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/CxDeviceList.h"

#if defined(_MSC_VER)
#include "Media/DS/api_ds.h"
#else
#include "Media/V4L2/api_v4l2.h"
#endif

#include <string>
#include <map>

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxDeviceList";

// ============================================================
void CxDeviceList::_Constructor()
{
}

// ============================================================
CxDeviceList::CxDeviceList()
{
	_Constructor();
}

// ============================================================
CxDeviceList::CxDeviceList(const CxDeviceList& src)
{
	_Constructor();
	operator = (src);
}

// ============================================================
CxDeviceList::~CxDeviceList()
{
	Dispose();
}

// ============================================================
CxDeviceList& CxDeviceList::operator = ( const CxDeviceList& src )
{
	CopyFrom(src);
	return *this;
}

// ============================================================
bool CxDeviceList::operator == ( const CxDeviceList& src ) const
{
	return ContentEquals(src);
}

// ============================================================
bool CxDeviceList::operator != ( const CxDeviceList& src ) const
{
	return !(operator == (src));
}

//
// IxDisposable
//

// ============================================================
void CxDeviceList::Dispose()
{
	m_Items.Dispose();
}

// ============================================================
bool CxDeviceList::IsValid() const
{
	return m_Items.IsValid();
}

// ============================================================
void CxDeviceList::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (Axi::ClassIs<CxDeviceList>(src))
	{
		auto& _src = static_cast<const CxDeviceList&>(src);
		auto& _dst = *this;
		_dst.m_Items = _src.m_Items;
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
bool CxDeviceList::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (Axi::ClassIs<CxDeviceList>(src))
	{
		auto& _src = static_cast<const CxDeviceList&>(src);
		auto&	_dst = *this;
		if (_dst.m_Items.Length() != _src.m_Items.Length()) return false;
		for(int i=0 ; i<_dst.m_Items.Length() ; i++)
		{
			if (_dst.m_Items[i] != _src.m_Items[i]) return false;
		}
		return true;
	}
	return false;
}

//
// Setup
//

// ============================================================
void CxDeviceList::Setup(ExMediaType type, ExMediaDir dir)
{
	Dispose();

#if defined(_MSC_VER)
	CxArrayEx<CxStringA> names = fnPRV_DS_GetDeviceNames(type, dir);
#else
	CxArrayEx<CxStringA> names = fnPRV_VL_GetDeviceNames(type, dir);
#endif

	int length = names.Length();
	m_Items.Resize(length);
	for(int i=0 ; i<length ; i++)
	{
		m_Items[i].MediaType(type);
		m_Items[i].MediaDir(dir);
		m_Items[i].Name(names[i].Address());

#if defined(_MSC_VER)
		int count = 0;
		for(int j=0 ; j<i ; j++)
		{
			if (names[i] == names[j])
				count++;
		}
		m_Items[i].Index(count);
#else
		m_Items[i].Index(0);
#endif
	}
}

// ============================================================
int CxDeviceList::Length() const
{
	return m_Items.Length();
}

// ============================================================
CxDeviceListItem& CxDeviceList::operator [] (int index)
{
	return m_Items[index];
}

// ============================================================
const CxDeviceListItem& CxDeviceList::operator [] (int index) const
{
	return m_Items[index];
}

}
}
