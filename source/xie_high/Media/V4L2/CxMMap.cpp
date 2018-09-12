/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#ifndef _MSC_VER

#include "CxMMap.h"

#include "Core/Axi.h"
#include "Core/CxException.h"
#include <sys/mman.h>
#include <utility>

namespace xie
{

static const char* g_ClassName = "CxMMap";

// ============================================================
void CxMMap::_Constructor()
{
	m_Address	= NULL;
	m_Length	= 0;
	m_Offset	= 0;
	m_FD		= -1;
	m_IsAttached = false;
}

// ============================================================
CxMMap::CxMMap()
{
	_Constructor();
}

// ============================================================
CxMMap::CxMMap(CxMMap&& src)
{
	_Constructor();
	operator = (std::move(src));
}

// ============================================================
CxMMap::CxMMap(const CxMMap& src)
{
	_Constructor();
	operator = (src);
}

// ============================================================
CxMMap::CxMMap(int length, int offset, int fd)
{
	_Constructor();
	Resize(length, offset, fd);
}

// ============================================================
CxMMap::~CxMMap()
{
	Dispose();
}

// ============================================================
CxMMap& CxMMap::operator = ( CxMMap&& src )
{
	if (this == &src) return *this;

	CxMMap& dst = *this;
	dst.Dispose();
	dst.m_Address	= src.m_Address;
	dst.m_Length	= src.m_Length;
	dst.m_Offset	= src.m_Offset;
	dst.m_FD		= src.m_FD;
	dst.m_IsAttached	= src.m_IsAttached;
	src.m_IsAttached	= true;

	return *this;
}

// ============================================================
CxMMap& CxMMap::operator = ( const CxMMap& src )
{
	if (this == &src) return *this;

	CxMMap& dst = *this;
	dst.Dispose();
	if (src.IsValid())
	{
		dst.Resize(src.m_Length, src.m_Offset, src.m_FD);
		memcpy(dst.m_Address, src.m_Address, src.m_Length);
	}

	return *this;
}

// ============================================================
bool CxMMap::operator == ( const CxMMap& src ) const
{
	const CxMMap& dst = *this;

	if (dst.m_Length	!= src.m_Length) return false;
	if (dst.m_Offset	!= src.m_Offset) return false;
	if (dst.m_FD		!= src.m_FD) return false;

	if (dst.m_Address == NULL)
		return (src.m_Address == NULL);
	else if (src.m_Address == NULL)
		return false;

	return (memcmp(dst.m_Address, src.m_Address, src.m_Length) == 0);
}

// ============================================================
bool CxMMap::operator != ( const CxMMap& src ) const
{
	return !(operator == (src));
}

// ============================================================
TxArray CxMMap::Tag() const
{
	return TxArray(m_Address, m_Length, xie::TxModel::U8(1));
}

// ============================================================
void CxMMap::Dispose()
{
	if (IsAttached() == false)
	{
		if (m_Address != NULL)
			munmap( m_Address, m_Length );
	}

	m_Address	= NULL;
	m_Length	= 0;
	m_Offset	= 0;
	m_FD		= -1;
	m_IsAttached = false;
}

// ============================================================
bool CxMMap::IsValid() const
{
	if (m_Address == NULL) return false;
	if (m_Length <= 0) return false;
	return true;
}

// ============================================================
bool CxMMap::IsAttached() const
{
	return m_IsAttached;
}

// ============================================================
void CxMMap::Resize(int length, int offset, int fd)
{
	this->Dispose();

	if (length == 0) return;
	if (length < 0)
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);

	try
	{
		m_Address = mmap(
					NULL,					// start anywhere
					length,
					PROT_READ | PROT_WRITE,	// required by V4L2
					MAP_SHARED,				// recommended by V4L2
					fd,
					offset
				);
		if (m_Address == NULL)
			throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
		if (m_Address == MAP_FAILED)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		m_Length	= length;
		m_Offset	= offset;
		m_FD		= fd;
	}
	catch(const CxException& ex)
	{
		this->Dispose();
		throw ex;
	}
}

// ============================================================
void CxMMap::Reset()
{
	if (IsValid() == false) return;

	memset(m_Address, 0, m_Length);
}

// ============================================================
void* CxMMap::Address()
{
	return m_Address;
}

// ============================================================
const void* CxMMap::Address() const
{
	return m_Address;
}

// ============================================================
int CxMMap::Length() const
{
	return m_Length;
}

// ============================================================
int CxMMap::Offset() const
{
	return m_Offset;
}

// ============================================================
int CxMMap::FD() const
{
	return m_FD;
}

}

#endif	// _MCS_VER
