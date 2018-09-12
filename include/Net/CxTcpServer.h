/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXTCPSERVER_H_INCLUDED_
#define _CXTCPSERVER_H_INCLUDED_

#include "xie_high.h"

#include "Core/CxModule.h"
#include "Core/CxMutex.h"
#include "Core/CxThread.h"
#include "Core/CxThreadEvent.h"
#include "Core/IxTagPtr.h"
#include "Core/IxDisposable.h"
#include "Core/IxRunnable.h"
#include "Net/TxTcpServer.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Net
{

class XIE_EXPORT_CLASS CxTcpServer : public CxModule
	, public IxTagPtr
	, public IxDisposable
	, public IxRunnable
{
protected:
	TxTcpServer		m_Tag;

private:
	void _Constructor();

public:
	CxTcpServer();
	CxTcpServer( const CxTcpServer& src );
	CxTcpServer(const TxIPAddress& addr, int port);
	CxTcpServer(const TxIPEndPoint& endpoint);
	virtual ~CxTcpServer();

	CxTcpServer& operator = ( const CxTcpServer& src );
	bool operator == ( const CxTcpServer& src ) const;
	bool operator != ( const CxTcpServer& src ) const;

	virtual TxTcpServer Tag() const;
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

	virtual TxSocketStream Stream(int index) const;
	virtual int Connections() const;

	virtual TxIPAddress IPAddress() const;
	virtual void IPAddress(const TxIPAddress& value);

	virtual int Port() const;
	virtual void Port(int value);

	virtual int Backlog() const;
	virtual void Backlog(int value);

	virtual TxIPEndPoint EndPoint() const;
	virtual void EndPoint(const TxIPEndPoint& value);

protected:
	void Open();
	void Close();

	void ThreadProc(void* sender, CxThreadArgs* e);
	mutable CxMutex	m_Mutex;
	CxThread		m_Thread;
};

}
}

#pragma pack(pop)

#endif
