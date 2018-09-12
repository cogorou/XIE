/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Media/CxScreenList.h"

#if defined(_MSC_VER)
#include "Media/DS/api_ds.h"
#else
#include "Media/V4L2/api_v4l2.h"
#endif

#include <string>
#include <vector>

namespace xie
{
namespace Media
{

static const char* g_ClassName = "CxScreenList";

// ============================================================
void CxScreenList::_Constructor()
{
}

// ============================================================
CxScreenList::CxScreenList()
{
	_Constructor();
}

// ============================================================
CxScreenList::CxScreenList(const CxScreenList& src)
{
	_Constructor();
	operator = (src);
}

// ============================================================
CxScreenList::~CxScreenList()
{
	Dispose();
}

// ============================================================
CxScreenList& CxScreenList::operator = ( const CxScreenList& src )
{
	CopyFrom(src);
	return *this;
}

// ============================================================
bool CxScreenList::operator == ( const CxScreenList& src ) const
{
	return ContentEquals(src);
}

// ============================================================
bool CxScreenList::operator != ( const CxScreenList& src ) const
{
	return !(operator == (src));
}

//
// IxDisposable
//

// ============================================================
void CxScreenList::Dispose()
{
	m_Items.Dispose();
}

// ============================================================
bool CxScreenList::IsValid() const
{
	return m_Items.IsValid();
}

// ============================================================
void CxScreenList::CopyFrom(const IxModule& src)
{
	if (this == &src) return;
	if (Axi::ClassIs<CxScreenList>(src))
	{
		auto& _src = static_cast<const CxScreenList&>(src);
		auto& _dst = *this;
		_dst.m_Items = _src.m_Items;
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ============================================================
bool CxScreenList::ContentEquals(const IxModule& src) const
{
	if (this == &src) return true;
	if (Axi::ClassIs<CxScreenList>(src))
	{
		auto& _src = static_cast<const CxScreenList&>(src);
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


#if defined(_MSC_VER)
// ============================================================
static BOOL CALLBACK EnumWindowsProc(HWND hwnd, LPARAM lParam)
{
	auto infos = (std::vector<CxScreenListItem>*)lParam;

	RECT rect = {0};
	BOOL status = GetWindowRect(hwnd, &rect);
	if (status)
	{
		TxRectangleI bounds(rect.left, rect.top, (rect.right-rect.left), (rect.bottom-rect.top));

		auto is_iconic = IsIconic(hwnd);
		auto is_visible = IsWindowVisible(hwnd);
		auto is_valid = (bounds.Width >= 320 && bounds.Height >= 240);

		if (!is_iconic && is_visible && is_valid)
		{
			const int maxlen = 40;
			char text[maxlen + 1] = "";
			int retlen = ::GetWindowTextA(hwnd, text, maxlen);
			if (retlen > 0)
			{
				infos->push_back(CxScreenListItem(hwnd, text, bounds));
			}
		}
	}
	return TRUE;
}
#else
#endif

// ============================================================
void CxScreenList::Setup()
{
	Dispose();

#if defined(_MSC_VER)
	std::vector<CxScreenListItem> infos;
	// Desktop Window
	{
		RECT rect = {0};
		HWND hWnd = ::GetDesktopWindow();
		::GetWindowRect(hWnd, &rect);
		TxRectangleI bounds(rect.left, rect.top, (rect.right-rect.left), (rect.bottom-rect.top));
		infos.push_back(CxScreenListItem(NULL, "Desktop", bounds));
	}
	// Top-Level Windows
	{
		BOOL status = EnumWindows((WNDENUMPROC)EnumWindowsProc, (LPARAM)&infos);
	}
	// copy
	m_Items.Resize((int)infos.size());
	for(int i=0 ; i<m_Items.Length() ; i++)
	{
		m_Items[i] = infos[i];
	}
#else
#endif
}

// ============================================================
int CxScreenList::Length() const
{
	return m_Items.Length();
}

// ============================================================
CxScreenListItem& CxScreenList::operator [] (int index)
{
	return m_Items[index];
}

// ============================================================
const CxScreenListItem& CxScreenList::operator [] (int index) const
{
	return m_Items[index];
}

}
}
