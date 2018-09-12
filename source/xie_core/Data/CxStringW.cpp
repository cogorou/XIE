/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxStringW.h"
#include "Core/CxString.h"
#include <typeinfo>
#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdarg.h>
#include <string.h>

#if !defined(_MSC_VER)
#define _wcsicmp wcscasecmp
#define _wcsnicmp wcsncasecmp
#endif

namespace xie
{

static const char* g_ClassName = "CxStringW";

// ============================================================
void CxStringW::_Constructor()
{
	memset( &m_Tag, 0, sizeof(TxStringW) );
}

// ============================================================
CxStringW CxStringW::Format(TxCharCPtrW format, ...)
{
#if defined(_MSC_VER)
	{
		CxStringW result;
		va_list argList;
		va_start(argList, format);
		int length = -1;
		{
			int tmp_size = _vscwprintf( format, argList );
			if (tmp_size < 0)
				throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
			for(; tmp_size < 1024*10 ; tmp_size++)
			{
				result.Resize(tmp_size);
				result.Reset();
				int ans_length = _vsnwprintf_s( result.Address(), (size_t)tmp_size, _TRUNCATE, format, argList );
				if (ans_length >= 0)
				{
					length = ans_length;
					break;
				}
			}
		}
		if (length < 0)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		if (length == 0)
			result.Dispose();
		else
		{
			auto tag = (TxStringW*)result.TagPtr();
			tag->Length = length;
		}
		return result;
	}
#else
	{
		CxStringW result;
		va_list argList;
		va_start(argList, format);
		int length = -1;
		// vscwprintf( &buffer, format, argList );
		for(int tmp_size = 1024 ; tmp_size < 1024*10 ; tmp_size += 1024)
		{
			result.Resize(tmp_size);
			result.Reset();

			va_list args;
			va_copy(args, argList);
			int ans_length = vswprintf(result.Address(), tmp_size, format, args);
			if (ans_length >= 0)
			{
				length = ans_length;
				break;
			}
		}
		if (length < 0)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		if (length == 0)
			result.Dispose();
		else
		{
			auto tag = (TxStringW*)result.TagPtr();
			tag->Length = length;
		}
		return result;
	}
#endif
}

// ============================================================
CxStringW CxStringW::From(TxCharCPtrA src, unsigned int codepage)
{
	CxStringW result;

#if defined(_MSC_VER)
	if (codepage == 0)
	{
		result.CopyFrom(CxStringA(src));
	}
	else
	{
		int required_length = MultiByteToWideChar(CP_UTF8, MB_ERR_INVALID_CHARS, src, -1, NULL, 0);
		if (required_length > 0)
		{
			result.Resize(required_length);
			MultiByteToWideChar(CP_UTF8, MB_ERR_INVALID_CHARS, src, -1, (TxCharPtrW)result, required_length);
		}
	}
#else
	{
		result.CopyFrom(CxStringA(src));
	}
#endif

	return result;
}

// ============================================================
CxStringW::CxStringW()
{
	_Constructor();
}

// ============================================================
CxStringW::CxStringW(CxStringW&& src)
{
	_Constructor();
	MoveFrom(src);
}

// ============================================================
CxStringW::CxStringW(const CxStringW& src)
{
	_Constructor();
	CopyFrom(src);
}

// ============================================================
CxStringW::CxStringW(TxCharCPtrW text)
{
	_Constructor();
	operator = (text);
}

// ============================================================
CxStringW::~CxStringW()
{
	Dispose();
}

// ============================================================
CxStringW& CxStringW::operator = ( CxStringW&& src )
{
	MoveFrom(src);
	return *this;
}

// ============================================================
CxStringW& CxStringW::operator = ( const CxStringW& src )
{
	CopyFrom(src);
	return *this;
}

// ============================================================
CxStringW& CxStringW::operator = ( TxCharCPtrW src )
{
	this->Dispose();
	if (src != NULL)
	{
		size_t	length = wcslen(src);
		if (length > 0)
		{
			m_Tag.Address = (wchar_t*)Axi::MemoryAlloc((TxIntPtr)(length + 1) * sizeof(wchar_t));
			if (m_Tag.Address == NULL)
				throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
			m_Tag.Length = (int)length;
			wcscpy(m_Tag.Address, src);
		}
	}
	return *this;
}

// ============================================================
bool CxStringW::operator == ( const CxStringW& src ) const
{
	const CxStringW& dst = *this;

	int src_length = 0;
	if (src.m_Tag.Address != NULL)
		src_length = src.Length();
	int dst_length = 0;
	if (dst.m_Tag.Address != NULL)
		dst_length = dst.Length();

	if (dst_length != src_length) return false;
	if (dst_length == 0) return true;
	if (wcscmp(dst.m_Tag.Address, src.m_Tag.Address) != 0) return false;

	return true;
}

// ============================================================
bool CxStringW::operator != ( const CxStringW& src ) const
{
	return !(CxStringW::operator == (src));
}

// ============================================================
bool CxStringW::operator == ( TxCharCPtrW src ) const
{
	const CxStringW& dst = *this;

	int src_length = 0;
	if (src != NULL)
		src_length = (int)wcslen(src);
	int dst_length = 0;
	if (dst.m_Tag.Address != NULL)
		dst_length = dst.Length();

	if (dst_length != src_length) return false;
	if (dst_length == 0) return true;
	if (wcscmp(dst.m_Tag.Address, src) != 0) return false;

	return true;
}

// ============================================================
bool CxStringW::operator != ( TxCharCPtrW src ) const
{
	return !(CxStringW::operator == (src));
}

// ============================================================
CxStringW CxStringW::operator + (const CxStringW& src) const
{
	CxStringW dst;
	if (this->IsValid() && src.IsValid())
	{
		dst.Resize(this->Length() + src.Length());
		wchar_t* dst0_addr = dst.Address();
		wchar_t* dst1_addr = dst.Address() + this->Length();
		wcscpy(dst0_addr, this->Address());
		wcscpy(dst1_addr, src.Address());
	}
	else if (this->IsValid())
	{
		dst = *this;
	}
	else if (src.IsValid())
	{
		dst = src;
	}
	return dst;
}

// ============================================================
CxStringW& CxStringW::operator += (const CxStringW& src)
{
	CxStringW tmp = *this + src;
	this->MoveFrom(tmp);
	return *this;
}

// ============================================================
CxStringW CxStringW::operator + (TxCharCPtrW src) const
{
	CxStringW dst;
	if (this->IsValid() && src != NULL)
	{
		dst.Resize(this->Length() + (int)wcslen(src));
		wchar_t* dst0_addr = dst.Address();
		wchar_t* dst1_addr = dst.Address() + this->Length();
		wcscpy(dst0_addr, this->Address());
		wcscpy(dst1_addr, src);
	}
	else if (this->IsValid())
	{
		dst = *this;
	}
	else if (src != NULL)
	{
		dst = src;
	}
	return dst;
}

// ============================================================
CxStringW& CxStringW::operator += (TxCharCPtrW src)
{
	CxStringW tmp = *this + src;
	this->MoveFrom(tmp);
	return *this;
}

// ============================================================
TxStringW CxStringW::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxStringW::TagPtr() const
{
	return (void*)&m_Tag;
}

// ============================================================
CxStringW::operator TxCharCPtrW() const
{
	return m_Tag.Address;
}

// ============================================================
CxStringW::operator TxCharPtrW()
{
	return m_Tag.Address;
}

// ============================================================
void CxStringW::Dispose()
{
	if (m_Tag.Address != NULL)
		Axi::MemoryFree( m_Tag.Address );
	memset( &m_Tag, 0, sizeof(TxStringW) );
}

// ================================================================================
void CxStringW::MoveFrom(CxStringW& src)
{
	if (this == &src) return;

	CxStringW& dst = *this;
	dst.Dispose();
	dst.m_Tag = src.m_Tag;
	src.m_Tag = TxStringW::Default();
}

// ============================================================
void CxStringW::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxStringW>(src))
	{
		auto&	_src = static_cast<const CxStringW&>(src);
		auto&	_dst = *this;
		this->Dispose();
		if (_src.m_Tag.Address != NULL)
		{
			size_t	length = (size_t)_src.m_Tag.Length;
			if (length > 0)
			{
				_dst.m_Tag.Address = (wchar_t*)Axi::MemoryAlloc((TxIntPtr)(length + 1) * sizeof(wchar_t));
				if (_dst.m_Tag.Address == NULL)
					throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
				_dst.m_Tag.Length = (int)length;
				wcscpy(_dst.m_Tag.Address, _src.m_Tag.Address);
			}
		}
		return;
	}
	if (xie::Axi::ClassIs<CxStringA>(src))
	{
		auto& _src = static_cast<const CxStringA&>(src);
		this->Dispose();
		if (_src.Length() > 0)
		{
			size_t length = mbstowcs(NULL, _src.Address(), _src.Length());
			if (length == (size_t)-1)
				throw CxException(ExStatus::Impossible, __FUNCTION__, __FILE__, __LINE__);
			if (length > 0)
			{
				m_Tag.Address = (wchar_t*)Axi::MemoryAlloc((TxIntPtr)(length + 1) * sizeof(wchar_t));
				if (m_Tag.Address == NULL)
					throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
				mbstowcs(m_Tag.Address, _src.Address(), (length + 1));
				m_Tag.Length = (int)wcslen(m_Tag.Address);
			}
		}
		return;
	}
	if (auto _src = xie::Axi::SafeCast<IxConvertible>(&src))
	{
		_src->CopyTo(*this);
		return;
	}
	throw CxException(ExStatus::Unsupported, __FUNCTION__, __FILE__, __LINE__);
}

// ================================================================================
bool CxStringW::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxStringW>(src) == false) return false;
	if (*this != static_cast<const CxStringW&>(src)) return false;
	return true;
}

// ============================================================
bool CxStringW::IsValid() const
{
	if (m_Tag.Address == NULL) return false;
	if (m_Tag.Length <= 0) return false;
	return true;
}

// ============================================================
void CxStringW::Resize(int length)
{
	this->Dispose();
	if (length > 0)
	{
		m_Tag.Address = (wchar_t*)Axi::MemoryAlloc((TxIntPtr)(length + 1) * sizeof(wchar_t));
		if (m_Tag.Address == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		m_Tag.Length = length;
	}
}

// ============================================================
void CxStringW::Reset()
{
	if (IsValid() == false) return;
	memset(m_Tag.Address, 0, (TxIntPtr)(m_Tag.Length + 1) * sizeof(wchar_t));
}

// ============================================================
int CxStringW::Length() const
{
	return m_Tag.Length;
}

// ============================================================
wchar_t* CxStringW::Address()
{
	return m_Tag.Address;
}

// ============================================================
const wchar_t* CxStringW::Address() const
{
	return m_Tag.Address;
}

// ============================================================
wchar_t& CxStringW::operator [] (int index)
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= index && index < m_Tag.Length))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return m_Tag.Address[index];
}

// ============================================================
const wchar_t& CxStringW::operator [] (int index) const
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= index && index < m_Tag.Length))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return m_Tag.Address[index];
}

// ============================================================
bool CxStringW::StartsWith(TxCharCPtrW value, bool ignore_case) const
{
	return StartsWith(m_Tag.Address, value, ignore_case);
}

// ============================================================
bool CxStringW::EndsWith(TxCharCPtrW value, bool ignore_case) const
{
	return EndsWith(m_Tag.Address, value, ignore_case);
}

// ============================================================
bool CxStringW::StartsWith(TxCharCPtrW src, TxCharCPtrW value, bool ignore_case)
{
	if (src == NULL) return false;
	if (value == NULL) return false;
	int src_length = (int)wcslen(src);
	int val_length = (int)wcslen(value);
	int pos = src_length - val_length;
	if (pos < 0 || val_length == 0) return false;
	bool ans = false;
	if (ignore_case)
		ans = (_wcsnicmp(&src[0], value, val_length) == 0);
	else
		ans = (wcsncmp(&src[0], value, val_length) == 0);
	return ans;
}

// ============================================================
bool CxStringW::EndsWith(TxCharCPtrW src, TxCharCPtrW value, bool ignore_case)
{
	if (src == NULL) return false;
	if (value == NULL) return false;
	int src_length = (int)wcslen(src);
	int val_length = (int)wcslen(value);
	int pos = src_length - val_length;
	if (pos < 0 || val_length == 0) return false;
	bool ans = false;
	if (ignore_case)
		ans = (_wcsicmp(&src[pos], value) == 0);
	else
		ans = (wcscmp(&src[pos], value) == 0);
	return ans;
}

}
