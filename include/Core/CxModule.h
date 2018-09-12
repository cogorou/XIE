/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXMODULE_H_INCLUDED_
#define _CXMODULE_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxModule : public IxModule
{
public:
	CxModule();
	virtual ~CxModule();

	CxModule& operator = ( const CxModule& src );
	bool operator == ( const CxModule& src ) const;
	bool operator != ( const CxModule& src ) const;

	// operator new/delete
	void* operator new       ( size_t size );
	void* operator new    [] ( size_t size );
	void  operator delete    ( void* ptr );
	void  operator delete [] ( void* ptr );

	// operator new/delete (DEBUG_NEW)
	void* operator new       ( size_t size, TxCharCPtrA filename, int line );
	void* operator new    [] ( size_t size, TxCharCPtrA filename, int line );
	void  operator delete    ( void* ptr, TxCharCPtrA filename, int line );
	void  operator delete [] ( void* ptr, TxCharCPtrA filename, int line );

	virtual operator HxModule() const;
	virtual int ModuleID() const;
};

}

#pragma pack(pop)

#endif
