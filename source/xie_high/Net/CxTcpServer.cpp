/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "api_net.h"
#include "Net/CxTcpServer.h"
#include "Net/TxSocketStream.h"
#include "Core/Axi.h"
#include "Core/CxStopwatch.h"
#include "Core/CxException.h"
#include "Core/CxFinalizer.h"

namespace xie
{
namespace Net
{

static const char* g_ClassName = "CxTcpServer";

// =================================================================
void CxTcpServer::_Constructor()
{
	m_Tag = TxTcpServer::Default();
	m_Thread.Notify	= CxThreadEvent(std::bind(&CxTcpServer::ThreadProc, this, std::placeholders::_1, std::placeholders::_2));
	m_Thread.Param	= this;
	m_Thread.Delay(1000);
}

// =================================================================
CxTcpServer::CxTcpServer()
{
	_Constructor();
}

// =================================================================
CxTcpServer::CxTcpServer( const CxTcpServer& src )
{
	_Constructor();
	operator = (src);
}

// =================================================================
CxTcpServer::CxTcpServer(const TxIPAddress& addr, int port)
{
	_Constructor();
	m_Tag.IPAddress	= addr;
	m_Tag.Port		= port;
}

// =================================================================
CxTcpServer::CxTcpServer(const TxIPEndPoint& endpoint)
{
	_Constructor();
	m_Tag.IPAddress	= endpoint.IPAddress;
	m_Tag.Port		= endpoint.Port;
}

// =================================================================
CxTcpServer::~CxTcpServer()
{
	Dispose();
}

// ============================================================
CxTcpServer& CxTcpServer::operator = ( const CxTcpServer& src )
{
	m_Tag.IPAddress	= src.m_Tag.IPAddress;
	m_Tag.Port		= src.m_Tag.Port;
	m_Tag.Backlog	= src.m_Tag.Backlog;
	return *this;
}

// ============================================================
bool CxTcpServer::operator == ( const CxTcpServer& src ) const
{
	if (m_Tag.IPAddress	!= src.m_Tag.IPAddress) return false;
	if (m_Tag.Port		!= src.m_Tag.Port) return false;
	if (m_Tag.Backlog	!= src.m_Tag.Backlog) return false;
	return true;
}

// ============================================================
bool CxTcpServer::operator != ( const CxTcpServer& src ) const
{
	return !(CxTcpServer::operator == (src));
}

// =================================================================
TxTcpServer CxTcpServer::Tag() const
{
	return m_Tag;
}

// =================================================================
void* CxTcpServer::TagPtr() const
{
	return (void*)&m_Tag;
}

// =================================================================
void CxTcpServer::Setup()
{
	Dispose();
	m_Thread.Setup();
}

// =================================================================
void CxTcpServer::Dispose()
{
	this->Stop();
	m_Thread.Dispose();
}

// =================================================================
bool CxTcpServer::IsValid() const
{
	if (m_Tag.Socket == XIE_INVALID_SOCKET) return false;
	if (m_Thread.IsValid() == false) return false;
	return true;
}

// =================================================================
void CxTcpServer::Reset()
{
	m_Thread.Reset();
}

// =================================================================
void CxTcpServer::Start()
{
	if (this->Socket() == XIE_INVALID_SOCKET)
		this->Open();
	m_Thread.Start();
}

// =================================================================
void CxTcpServer::Stop()
{
	m_Thread.Stop();
	Close();
}

// ============================================================
bool CxTcpServer::Wait(int timeout) const
{
	xie::CxStopwatch watch;
	watch.Start();
	while(IsRunning())
	{
		watch.Stop();
		if (0 <= timeout && timeout <= watch.Elapsed)
			return false;
		xie::Axi::Sleep(1);
	}
	return true;
}

// =================================================================
bool CxTcpServer::IsRunning() const
{
	return m_Thread.IsRunning();
}

// =================================================================
HxSocket CxTcpServer::Socket() const
{
	return m_Tag.Socket;
}

// =================================================================
TxSocketStream CxTcpServer::Stream(int index) const
{
	this->m_Mutex.Lock();
	CxFinalizer finalizer([this]()
	{
		this->m_Mutex.Unlock();
	});
	if (!(0 <= index && index < m_Tag.Connections))
		throw CxException(ExStatus::InvalidParam, __FUNCTION__, __FILE__, __LINE__);
	return m_Tag.Clients[index];
}

// =================================================================
int CxTcpServer::Connections() const
{
	return m_Tag.Connections;
}

// =================================================================
TxIPAddress CxTcpServer::IPAddress() const
{
	return m_Tag.IPAddress;
}

// =================================================================
void CxTcpServer::IPAddress(const TxIPAddress& value)
{
	m_Tag.IPAddress = value;
}

// =================================================================
int CxTcpServer::Port() const
{
	return m_Tag.Port;
}

// =================================================================
void CxTcpServer::Port(int value)
{
	m_Tag.Port = value;
}

// =================================================================
int CxTcpServer::Backlog() const
{
	return m_Tag.Backlog;
}

// =================================================================
void CxTcpServer::Backlog(int value)
{
	m_Tag.Backlog = value;
}

// =================================================================
TxIPEndPoint CxTcpServer::EndPoint() const
{
	return TxIPEndPoint(
		m_Tag.IPAddress,
		m_Tag.Port,
		AF_INET
		);
}

// =================================================================
void CxTcpServer::EndPoint(const TxIPEndPoint& value)
{
	m_Tag.IPAddress	= value.IPAddress;
	m_Tag.Port		= value.Port;
}

// =================================================================
void CxTcpServer::Open()
{
	HxSocket sock = XIE_INVALID_SOCKET;

	try
	{
		int status = 0;

		// create socket
		sock = ::socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
		if (sock == XIE_INVALID_SOCKET)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// reuse
		int resuse = 1;
		::setsockopt(sock, SOL_SOCKET, SO_REUSEADDR, (const char*)&resuse, sizeof(resuse));

		// bind
		sockaddr_in service = fnPRV_IPEndPoint_ToSockAddr(this->EndPoint());
		status = ::bind(sock, (sockaddr*)&service, sizeof(service));
		if (status != 0)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// listen
		status = ::listen(sock, m_Tag.Backlog);
		if (status != 0)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// set
		m_Tag.Socket = sock;
	}
	catch(const CxException& ex)
	{
		::CLOSE_SOCKET(sock);
		throw CxException(ex);
	}
}

// =================================================================
void CxTcpServer::Close()
{
	if (m_Tag.Clients != NULL)
	{
		for(int i=0 ; i<m_Tag.Connections ; i++)
			::CLOSE_SOCKET(m_Tag.Clients[i].Socket);
		Axi::MemoryFree( m_Tag.Clients );
	}
	m_Tag.Clients		= NULL;
	m_Tag.Connections	= 0;

	if (m_Tag.Socket != XIE_INVALID_SOCKET)
		::CLOSE_SOCKET(m_Tag.Socket);
	m_Tag.Socket = XIE_INVALID_SOCKET;
}

// =================================================================
void CxTcpServer::ThreadProc(void* sender, CxThreadArgs* e)
{
	// accept
	try
	{
		HxSocket listener = this->Socket();

		if (TxSocketStream(listener).Readable(0))
		{
			sockaddr_in		addr;
			socklen_t		addrlen = sizeof(addr);
			HxSocket		sock = ::accept(listener, (sockaddr*)&addr, &addrlen);

			// add a new client to list.
			if (sock != XIE_INVALID_SOCKET)
			{
				int connections = this->m_Tag.Connections;
				TxSocketStream* old_list = this->m_Tag.Clients;
				TxSocketStream* new_list = (TxSocketStream*)Axi::MemoryAlloc((TxIntPtr)(connections+1) * sizeof(TxSocketStream));
				if (new_list == NULL)
				{
					::CLOSE_SOCKET(sock);
					this->Close();
					throw CxException(ExStatus::MemoryError, __FUNCTION__, __FILE__, __LINE__);
				}
				else
				{
					for(int i=0 ; i<connections ; i++)
						new_list[i] = old_list[i];

					new_list[connections].Socket			= sock;
					new_list[connections].LocalEndPoint		= this->EndPoint();
					new_list[connections].RemoteEndPoint	= fnPRV_IPEndPoint_FromSockAddr(addr);

					{
						this->m_Mutex.Lock();
						CxFinalizer finalizer([this]()
						{
							this->m_Mutex.Unlock();
						});
						Axi::MemoryFree(old_list);
						this->m_Tag.Clients		= new_list;
						this->m_Tag.Connections	= connections + 1;
					}
				}
			}
		}
		else
		{
			this->m_Mutex.Lock();
			CxFinalizer finalizer([this]()
			{
				this->m_Mutex.Unlock();
			});

			int connections = this->m_Tag.Connections;
			TxSocketStream* clients = this->m_Tag.Clients;

			int count = 0;
			for(int i=0 ; i<connections ; i++)
			{
				TxSocketStream client = clients[i];

				if (TxSocketStream(client).Connected())
				{
					clients[count++] = client;
				}
				else
				{
					clients[i] = TxSocketStream::Default();
					::CLOSE_SOCKET(client.Socket);
				}
			}

			this->m_Tag.Connections = count;
		}
	}
	catch(const CxException&)
	{
		e->Cancellation = true;
	}
}

}
}
