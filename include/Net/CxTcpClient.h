/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXTCPCLIENT_H_INCLUDED_
#define _CXTCPCLIENT_H_INCLUDED_

#include "xie_high.h"

#include "Core/CxModule.h"
#include "Core/CxThread.h"
#include "Core/CxThreadEvent.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxRunnable.h"
#include "Net/TxTcpClient.h"
#include "Net/TxSocketStream.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Net
{

class XIE_EXPORT_CLASS CxTcpClient : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxRunnable
{
protected:
	TxTcpClient		m_Tag;

private:
	void _Constructor();

public:
	CxTcpClient();
	CxTcpClient( const CxTcpClient& src );
	CxTcpClient(const TxIPAddress& addr, int port);
	CxTcpClient(const TxIPEndPoint& endpoint);
	virtual ~CxTcpClient();

	CxTcpClient& operator = ( const CxTcpClient& src );
	bool operator == ( const CxTcpClient& src ) const;
	bool operator != ( const CxTcpClient& src ) const;

	virtual TxTcpClient Tag() const;
	virtual void* TagPtr() const;

public:
	virtual void Setup();

	// IxDisposable
	virtual void Dispose();
	virtual bool IsValid() const;

	// IxRunnable
	virtual void Reset();
	virtual void Start();
	virtual void Stop();
	virtual bool Wait(int timeout) const;
	virtual bool IsRunning() const;

public:
	virtual HxSocket Socket() const;

	virtual TxSocketStream Stream() const;

	virtual TxIPAddress IPAddress() const;
	virtual void IPAddress(const TxIPAddress& value);

	virtual int Port() const;
	virtual void Port(int value);

	virtual TxIPEndPoint EndPoint() const;
	virtual void EndPoint(const TxIPEndPoint& value);

	virtual bool Connected() const;

protected:
	void Open();
	void Close();

	void ThreadProc(void* sender, CxThreadArgs* e);
	CxThread		m_Thread;
};

}
}

#pragma pack(pop)

#endif
