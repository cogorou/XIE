/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxStringA.h"
#include "Core/CxString.h"
#include <typeinfo>
#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdarg.h>
#include <string.h>

#if !defined(_MSC_VER)
#define _stricmp strcasecmp
#define _strnicmp strncasecmp
#endif

namespace xie
{

static const char* g_ClassName = "CxStringA";

// ============================================================
void CxStringA::_Constructor()
{
	memset( &m_Tag, 0, sizeof(TxStringA) );
}

// ============================================================
CxStringA CxStringA::Format(TxCharCPtrA format, ...)
{
#if defined(_MSC_VER)
	{
		CxStringA result;
		va_list argList;
		va_start(argList, format);
		int length = _vscprintf( format, argList );
		if (length < 0)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		result.m_Tag.Length = length;
		result.m_Tag.Address = (char*)Axi::MemoryAlloc((TxIntPtr)(length + 1) * sizeof(char));
		vsprintf( result.m_Tag.Address, format, argList );
		return result;
	}
#else
	{
		CxStringA result;
		va_list argList;
		va_start(argList, format);
		char* buffer = NULL;
		int length = vasprintf( &buffer, format, argList );
		if (length < 0)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		result = buffer;
		free( buffer );
		return result;
	}
#endif
}

// ============================================================
CxStringA CxStringA::From(TxCharCPtrW src, unsigned int codepage)
{
	CxStringA result;

#if defined(_MSC_VER)
	if (codepage == 0)
	{
		result.CopyFrom(CxStringW(src));
	}
	else
	{
		int required_length = WideCharToMultiByte(CP_UTF8, 0, src, -1, NULL, 0, NULL, NULL);
		if (required_length > 0)
		{
			result.Resize(required_length);
			WideCharToMultiByte(CP_UTF8, 0, src, -1, (TxCharPtrA)result, required_length, NULL, NULL);
		}
	}
#else
	{
		result.CopyFrom(CxStringW(src));
	}
#endif

	return result;
}

// ============================================================
CxStringA::CxStringA()
{
	_Constructor();
}

// ============================================================
CxStringA::CxStringA(CxStringA&& src)
{
	_Constructor();
	MoveFrom(src);
}

// ============================================================
CxStringA::CxStringA(const CxStringA& src)
{
	_Constructor();
	CopyFrom(src);
}

// ============================================================
CxStringA::CxStringA(TxCharCPtrA text)
{
	_Constructor();
	operator = (text);
}

// ============================================================
CxStringA::~CxStringA()
{
	Dispose();
}

// ============================================================
CxStringA& CxStringA::operator = ( CxStringA&& src )
{
	MoveFrom(src);
	return *this;
}

// ============================================================
CxStringA& CxStringA::operator = ( const CxStringA& src )
{
	CopyFrom(src);
	return *this;
}

// ============================================================
CxStringA& CxStringA::operator = ( TxCharCPtrA src )
{
	this->Dispose();
	if (src != NULL)
	{
		size_t	length = strlen(src);
		if (length > 0)
		{
			m_Tag.Address = (char*)Axi::MemoryAlloc((TxIntPtr)(length + 1) * sizeof(char));
			if (m_Tag.Address == NULL)
				throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
			m_Tag.Length = (int)length;
			strcpy(m_Tag.Address, src);
		}
	}
	return *this;
}

// ============================================================
bool CxStringA::operator == ( const CxStringA& src ) const
{
	const CxStringA& dst = *this;

	int src_length = 0;
	if (src.m_Tag.Address != NULL)
		src_length = src.Length();
	int dst_length = 0;
	if (dst.m_Tag.Address != NULL)
		dst_length = dst.Length();

	if (dst_length != src_length) return false;
	if (dst_length == 0) return true;
	if (strcmp(dst.m_Tag.Address, src.m_Tag.Address) != 0) return false;

	return true;
}

// ============================================================
bool CxStringA::operator != ( const CxStringA& src ) const
{
	return !(CxStringA::operator == (src));
}

// ============================================================
bool CxStringA::operator == ( TxCharCPtrA src ) const
{
	const CxStringA& dst = *this;

	int src_length = 0;
	if (src != NULL)
		src_length = (int)strlen(src);
	int dst_length = 0;
	if (dst.m_Tag.Address != NULL)
		dst_length = dst.Length();

	if (dst_length != src_length) return false;
	if (dst_length == 0) return true;
	if (strcmp(dst.m_Tag.Address, src) != 0) return false;

	return true;
}

// ============================================================
bool CxStringA::operator != ( TxCharCPtrA src ) const
{
	return !(CxStringA::operator == (src));
}

// ============================================================
CxStringA CxStringA::operator + (const CxStringA& src) const
{
	CxStringA dst;
	if (this->IsValid() && src.IsValid())
	{
		dst.Resize(this->Length() + src.Length());
		char* dst0_addr = dst.Address();
		char* dst1_addr = dst.Address() + this->Length();
		strcpy(dst0_addr, this->Address());
		strcpy(dst1_addr, src.Address());
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
CxStringA& CxStringA::operator += (const CxStringA& src)
{
	CxStringA tmp = *this + src;
	this->MoveFrom(tmp);
	return *this;
}

// ============================================================
CxStringA CxStringA::operator + (TxCharCPtrA src) const
{
	CxStringA dst;
	if (this->IsValid() && src != NULL)
	{
		dst.Resize(this->Length() + (int)strlen(src));
		char* dst0_addr = dst.Address();
		char* dst1_addr = dst.Address() + this->Length();
		strcpy(dst0_addr, this->Address());
		strcpy(dst1_addr, src);
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
CxStringA& CxStringA::operator += (TxCharCPtrA src)
{
	CxStringA tmp = *this + src;
	this->MoveFrom(tmp);
	return *this;
}

// ============================================================
TxStringA CxStringA::Tag() const
{
	return m_Tag;
}

// ============================================================
void* CxStringA::TagPtr() const
{
	return (void*)&m_Tag;
}

// ============================================================
CxStringA::operator TxCharCPtrA() const
{
	return m_Tag.Address;
}

// ============================================================
CxStringA::operator TxCharPtrA()
{
	return m_Tag.Address;
}

// ============================================================
void CxStringA::Dispose()
{
	if (m_Tag.Address != NULL)
		Axi::MemoryFree( m_Tag.Address );
	memset( &m_Tag, 0, sizeof(TxStringA) );
}

// ================================================================================
void CxStringA::MoveFrom(CxStringA& src)
{
	if (this == &src) return;

	CxStringA& dst = *this;
	dst.Dispose();
	dst.m_Tag = src.m_Tag;
	src.m_Tag = TxStringA::Default();
}

// ============================================================
void CxStringA::CopyFrom(const IxModule& src)
{
	if (this == &src) return;

	if (xie::Axi::ClassIs<CxStringA>(src))
	{
		auto&	_src = static_cast<const CxStringA&>(src);
		auto&	_dst = *this;

		this->Dispose();
		if (_src.m_Tag.Address != NULL)
		{
			size_t	length = (size_t)_src.m_Tag.Length;
			if (length > 0)
			{
				_dst.m_Tag.Address = (char*)Axi::MemoryAlloc((TxIntPtr)(length + 1) * sizeof(char));
				if (_dst.m_Tag.Address == NULL)
					throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
				_dst.m_Tag.Length = (int)length;
				strcpy(_dst.m_Tag.Address, _src.m_Tag.Address);
			}
		}
		return;
	}
	if (xie::Axi::ClassIs<CxStringW>(src))
	{
		auto& _src = static_cast<const CxStringW&>(src);
		this->Dispose();
		if (_src.Length() > 0)
		{
			size_t length = wcstombs(NULL, _src.Address(), _src.Length());
			if (length == (size_t)-1)
				throw CxException(ExStatus::Impossible, __FUNCTION__, __FILE__, __LINE__);
			if (length > 0)
			{
				m_Tag.Address = (char*)Axi::MemoryAlloc((TxIntPtr)(length + 1) * sizeof(char));
				if (m_Tag.Address == NULL)
					throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
				wcstombs(m_Tag.Address, _src.Address(), (length + 1));
				m_Tag.Length = (int)strlen(m_Tag.Address);
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
bool CxStringA::ContentEquals(const IxModule& src) const
{
	if (xie::Axi::ClassIs<CxStringA>(src) == false) return false;
	if (*this != static_cast<const CxStringA&>(src)) return false;
	return true;
}

// ============================================================
bool CxStringA::IsValid() const
{
	if (m_Tag.Address == NULL) return false;
	if (m_Tag.Length <= 0) return false;
	return true;
}

// ============================================================
void CxStringA::Resize(int length)
{
	this->Dispose();
	if (length > 0)
	{
		m_Tag.Address = (char*)Axi::MemoryAlloc((TxIntPtr)(length + 1) * sizeof(char));
		if (m_Tag.Address == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		m_Tag.Length = length;
	}
}

// ============================================================
void CxStringA::Reset()
{
	if (IsValid() == false) return;
	memset(m_Tag.Address, 0, (TxIntPtr)(m_Tag.Length + 1) * sizeof(char));
}

// ============================================================
int CxStringA::Length() const
{
	return m_Tag.Length;
}

// ============================================================
char* CxStringA::Address()
{
	return m_Tag.Address;
}

// ============================================================
const char* CxStringA::Address() const
{
	return m_Tag.Address;
}

// ============================================================
char& CxStringA::operator [] (int index)
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= index && index < m_Tag.Length))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return m_Tag.Address[index];
}

// ============================================================
const char& CxStringA::operator [] (int index) const
{
	if (m_Tag.Address == NULL)
		throw CxException(ExStatus::InvalidObject, __FUNCTION__, __FILE__, __LINE__);
	if (!(0 <= index && index < m_Tag.Length))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return m_Tag.Address[index];
}

// ============================================================
bool CxStringA::StartsWith(TxCharCPtrA value, bool ignore_case) const
{
	return StartsWith(m_Tag.Address, value, ignore_case);
}

// ============================================================
bool CxStringA::EndsWith(TxCharCPtrA value, bool ignore_case) const
{
	return EndsWith(m_Tag.Address, value, ignore_case);
}

// ============================================================
bool CxStringA::StartsWith(TxCharCPtrA src, TxCharCPtrA value, bool ignore_case)
{
	if (src == NULL) return false;
	if (value == NULL) return false;
	int src_length = (int)strlen(src);
	int val_length = (int)strlen(value);
	int pos = src_length - val_length;
	if (pos < 0 || val_length == 0) return false;
	bool ans = false;
	if (ignore_case)
		ans = (_strnicmp(&src[0], value, val_length) == 0);
	else
		ans = (strncmp(&src[0], value, val_length) == 0);
	return ans;
}

// ============================================================
bool CxStringA::EndsWith(TxCharCPtrA src, TxCharCPtrA value, bool ignore_case)
{
	if (src == NULL) return false;
	if (value == NULL) return false;
	int src_length = (int)strlen(src);
	int val_length = (int)strlen(value);
	int pos = src_length - val_length;
	if (pos < 0 || val_length == 0) return false;
	bool ans = false;
	if (ignore_case)
		ans = (_stricmp(&src[pos], value) == 0);
	else
		ans = (strcmp(&src[pos], value) == 0);
	return ans;
}

}
