/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "Core/Axi.h"
#include "Core/CxModule.h"
#include "Core/CxException.h"
#include <typeinfo>

#pragma warning (disable:4100)	// à¯êîÇÕä÷êîÇÃñ{ëÃïîÇ≈ 1 ìxÇ‡éQè∆Ç≥ÇÍÇ‹ÇπÇÒ.

namespace xie
{

// ============================================================
CxModule::CxModule()
{
}

// ============================================================
CxModule::~CxModule()
{
}

// ============================================================
CxModule& CxModule::operator = ( const CxModule& src )
{
	return *this;
}

// ============================================================
bool CxModule::operator == ( const CxModule& src ) const
{
	return true;
}

// ============================================================
bool CxModule::operator != ( const CxModule& src ) const
{
	return !(CxModule::operator == (src));
}

// ============================================================
void* CxModule::operator new ( size_t size )
{
	return Axi::MemoryAlloc( size );
}

// ============================================================
void* CxModule::operator new [] ( size_t size )
{
	return Axi::MemoryAlloc( size );
}

// ============================================================
void CxModule::operator delete ( void* ptr )
{
	Axi::MemoryFree( ptr );
}

// ============================================================
void CxModule::operator delete [] ( void* ptr )
{
	Axi::MemoryFree( ptr );
}

// ============================================================
void* CxModule::operator new ( size_t size, TxCharCPtrA filename, int line )
{
	return Axi::MemoryAlloc( size );
}

// ============================================================
void* CxModule::operator new [] ( size_t size, TxCharCPtrA filename, int line )
{
	return Axi::MemoryAlloc( size );
}

// ============================================================
void  CxModule::operator delete ( void* ptr, TxCharCPtrA filename, int line )
{
	Axi::MemoryFree( ptr );
}

// ============================================================
void  CxModule::operator delete [] ( void* ptr, TxCharCPtrA filename, int line )
{
	Axi::MemoryFree( ptr );
}

// ============================================================
CxModule::operator HxModule() const
{
	return (HxModule)this;
}

// ============================================================
int CxModule::ModuleID() const
{
	return XIE_MODULE_ID;
}

}
