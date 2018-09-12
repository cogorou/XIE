/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_core.h"
#include "api_core.h"

#include "Core/CxException.h"
#include "Core/Axi.h"

#pragma warning (disable:4100)	// à¯êîÇÕä÷êîÇÃñ{ëÃïîÇ≈ 1 ìxÇ‡éQè∆Ç≥ÇÍÇ‹ÇπÇÒ.

namespace xie
{

// ===================================================================
CxException::CxException( ExStatus code, TxCharCPtrA function, TxCharCPtrA file, int line )
{
	m_Code		= code;
	m_Line		= line;
	m_File		= NULL;
	m_Function	= NULL;
	m_File		= _copy( file );
	m_Function	= _copy( function );

	fnXIE_Core_TraceOut(1, "%s(%d): Code=%d, Function=%s\n", m_File, m_Line, m_Code, m_Function);
}

// ===================================================================
CxException::CxException( const CxException& src )
{
	m_File		= NULL;
	m_Function	= NULL;
	operator = (src);
}

// ===================================================================
CxException::~CxException()
{
	if( m_File != NULL )
		delete [] m_File;
	if( m_Function != NULL )
		delete [] m_Function;
}

// ===================================================================
CxException& CxException::operator = ( const CxException& src )
{
	if (this == &src) return *this;

	m_Code	= src.m_Code;
	m_Line	= src.m_Line;

	if( m_File != NULL )
		delete [] m_File;
	if( m_Function != NULL )
		delete [] m_Function;

	m_File		= NULL;
	m_Function	= NULL;

	m_File		= _copy( src.m_File );
	m_Function	= _copy( src.m_Function );

	return *this;
}

// ===================================================================
bool CxException::operator == ( const CxException& src ) const
{
	if (m_Code	!= src.m_Code) return false;
	if (m_Line	!= src.m_Line) return false;
	if (!_compare( m_File, src.m_File )) return false;
	if (!_compare( m_Function, src.m_Function )) return false;
	return true;
}

// ===================================================================
bool CxException::operator != ( const CxException& src ) const
{
	return !(CxException::operator == (src));
}

// ===================================================================
ExStatus CxException::Code() const
{
	return m_Code;
}

// ===================================================================
void CxException::Code(ExStatus value)
{
	m_Code = value;
}

// ===================================================================
TxCharCPtrA CxException::File() const
{
	return m_File;
}

// ===================================================================
void CxException::File( TxCharCPtrA value )
{
	if (m_File != NULL)
		delete [] m_File;
	m_File = NULL;
	m_File = _copy( value );
}

// ===================================================================
TxCharCPtrA CxException::Function() const
{
	return m_Function;
}

// ===================================================================
void CxException::Function( TxCharCPtrA value )
{
	if (m_Function != NULL)
		delete [] m_Function;
	m_Function = NULL;
	m_Function = _copy( value );
}

// ===================================================================
int CxException::Line() const
{
	return m_Line;
}

// ===================================================================
void CxException::Line(int value)
{
	m_Line = value;
}

// ===================================================================
TxCharPtrA CxException::_copy( TxCharCPtrA src )
{
	if (src == NULL)
	{
		char* result = new char[1];
		result[0] = '\0';
		return result;
	}

	size_t	length = strlen(src);
	char* result = new char[length+1];
	strncpy( result, src, length+1 );
	return result;
}

// ===================================================================
bool CxException::_compare( TxCharCPtrA ope1, TxCharCPtrA ope2 )
{
	if( ope1 == NULL && ope2 == NULL ) return true;
	if( ope1 == NULL && ope2 != NULL ) return false;
	if( ope1 != NULL && ope2 == NULL ) return false;
	if( 0 == strcmp( ope1, ope2 ) ) return true;
	return false;
}

}	// xie
