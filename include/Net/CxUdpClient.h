/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXUDPCLIENT_H_INCLUDED_
#define _CXUDPCLIENT_H_INCLUDED_

#include "xie_high.h"

#include "Core/CxModule.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Net/TxUdpClient.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Net
{

class XIE_EXPORT_CLASS CxUdpClient : public CxModule
	, public IxTagPtr
	, public IxDisposable
{
protected:
	TxUdpClient		m_Tag;

private:
	void _Constructor();

public:
	CxUdpClient();
	CxUdpClient( const CxUdpClient& src );
	CxUdpClient(const TxIPAddress& addr, int port);
	CxUdpClient(const TxIPEndPoint& endpoint);
	virtual ~CxUdpClient();

	CxUdpClient& operator = ( const CxUdpClient& src );
	bool operator == ( const CxUdpClient& src ) const;
	bool operator != ( const CxUdpClient& src ) const;

	virtual TxUdpClient Tag() const;
	virtual void* TagPtr() const;

public:
	virtual void Setup();

	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	bool Readable(int timeout) const;
	int Read(char* buffer, int length, int timeout, TxIPEndPoint& remoteEP) const;

	bool Writeable(int timeout) const;
	int Write(const char* buffer, int length, int timeout, const TxIPEndPoint& remoteEP) const;

public:
	virtual HxSocket Socket() const;

	virtual TxIPAddress IPAddress() const;
	virtual void IPAddress(const TxIPAddress& value);

	virtual int Port() const;
	virtual void Port(int value);

	virtual TxIPEndPoint EndPoint() const;
	virtual void EndPoint(const TxIPEndPoint& value);
};

}
}

#pragma pack(pop)

#endif
