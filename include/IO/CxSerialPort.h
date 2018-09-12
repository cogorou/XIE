/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXSERIAPORT_H_INCLUDED_
#define _CXSERIAPORT_H_INCLUDED_

#include "xie_high.h"

#include "Core/CxModule.h"
#include "Core/CxString.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "IO/TxSerialPort.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace IO
{

class XIE_EXPORT_CLASS CxSerialPort : public CxModule
	, public IxTagPtr
	, public IxDisposable
{
protected:
	void*				m_Controller;
	CxStringA			m_PortName;
	TxSerialPort		m_Param;

private:
	void _Constructor();

public:
	CxSerialPort();
	CxSerialPort( const CxSerialPort& src );
	CxSerialPort(TxCharCPtrA portname, const TxSerialPort& param);
	virtual ~CxSerialPort();

	CxSerialPort& operator = ( const CxSerialPort& src );
	bool operator == ( const CxSerialPort& src ) const;
	bool operator != ( const CxSerialPort& src ) const;

	virtual TxSerialPort Tag() const;
	virtual void* TagPtr() const;

public:
	virtual void Setup();

	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	bool Readable(int timeout) const;
	int Read(char* buffer, int length, int timeout) const;

	bool Writeable(int timeout) const;
	int Write(const char* buffer, int length, int timeout) const;

public:
	virtual TxCharCPtrA PortName() const;
	virtual void PortName(TxCharCPtrA value);

	virtual TxSerialPort& Param();
	virtual const TxSerialPort& Param() const;
	virtual void Param(const TxSerialPort& value);
};

}
}

#pragma pack(pop)

#endif
