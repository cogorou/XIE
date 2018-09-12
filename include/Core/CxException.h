/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXEXCEPTION_H_INCLUDED_
#define _CXEXCEPTION_H_INCLUDED_

#include "xie_core.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{

class XIE_EXPORT_CLASS CxException
{
public:
	CxException( ExStatus code, TxCharCPtrA function, TxCharCPtrA file, int line );
	CxException( const CxException& src );
	virtual ~CxException();

	CxException& operator = ( const CxException& src );
	bool operator == ( const CxException& src ) const;
	bool operator != ( const CxException& src ) const;

public:
	virtual ExStatus Code() const;
	virtual void Code(ExStatus value);
	
	virtual TxCharCPtrA Function() const;
	virtual void Function(TxCharCPtrA value);
	
	virtual TxCharCPtrA File() const;
	virtual void File(TxCharCPtrA value);
	
	virtual int Line() const;
	virtual void Line(int value);

protected:
	ExStatus	m_Code;
	TxCharPtrA		m_Function;
	TxCharPtrA		m_File;
	int			m_Line;
	
private:
	static TxCharPtrA _copy( TxCharCPtrA src );
	static bool _compare( TxCharCPtrA ope1, TxCharCPtrA ope2 );
};

}

#pragma pack(pop)

#endif
