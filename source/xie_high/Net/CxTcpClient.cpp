/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "api_net.h"
#include "Net/CxTcpClient.h"
#include "Net/TxSocketStream.h"
#include "Core/Axi.h"
#include "Core/CxStopwatch.h"
#include "Core/CxException.h"

namespace xie
{
namespace Net
{

static const char* g_ClassName = "CxTcpClient";

// =================================================================
void CxTcpClient::_Constructor()
{
	m_Tag = TxTcpClient::Default();
	m_Thread.Notify	= CxThreadEvent(std::bind(&CxTcpClient::ThreadProc, this, std::placeholders::_1, std::placeholders::_2));
	m_Thread.Param	= this;
	m_Thread.Delay(1000);
}

// =================================================================
CxTcpClient::CxTcpClient()
{
	_Constructor();
}

// =================================================================
CxTcpClient::CxTcpClient( const CxTcpClient& src )
{
	_Constructor();
	operator = (src);
}

// =================================================================
CxTcpClient::CxTcpClient(const TxIPAddress& addr, int port)
{
	_Constructor();
	m_Tag.IPAddress	= addr;
	m_Tag.Port		= port;
}

// =================================================================
CxTcpClient::CxTcpClient(const TxIPEndPoint& endpoint)
{
	_Constructor();
	m_Tag.IPAddress	= endpoint.IPAddress;
	m_Tag.Port		= endpoint.Port;
}

// =================================================================
CxTcpClient::~CxTcpClient()
{
	Dispose();
}

// ============================================================
CxTcpClient& CxTcpClient::operator = ( const CxTcpClient& src )
{
	m_Tag.IPAddress	= src.m_Tag.IPAddress;
	m_Tag.Port		= src.m_Tag.Port;
	return *this;
}

// ============================================================
bool CxTcpClient::operator == ( const CxTcpClient& src ) const
{
	if (m_Tag.IPAddress	!= src.m_Tag.IPAddress) return false;
	if (m_Tag.Port		!= src.m_Tag.Port) return false;
	return true;
}

// ============================================================
bool CxTcpClient::operator != ( const CxTcpClient& src ) const
{
	return !(CxTcpClient::operator == (src));
}

// =================================================================
TxTcpClient CxTcpClient::Tag() const
{
	return m_Tag;
}

// =================================================================
void* CxTcpClient::TagPtr() const
{
	return (void*)&m_Tag;
}

// =================================================================
void CxTcpClient::Setup()
{
	Dispose();
	m_Thread.Setup();
}

// =================================================================
void CxTcpClient::Dispose()
{
	this->Stop();
	m_Thread.Dispose();
}

// =================================================================
bool CxTcpClient::IsValid() const
{
	if (m_Tag.Socket == XIE_INVALID_SOCKET) return false;
	if (m_Thread.IsValid() == false) return false;
	return true;
}

// =================================================================
void CxTcpClient::Reset()
{
	m_Thread.Reset();
}

// =================================================================
void CxTcpClient::Start()
{
	m_Thread.Start();
}

// =================================================================
void CxTcpClient::Stop()
{
	m_Thread.Stop();
	this->Close();
}

// ============================================================
bool CxTcpClient::Wait(int timeout) const
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
bool CxTcpClient::IsRunning() const
{
	return m_Thread.IsRunning();
}

// =================================================================
HxSocket CxTcpClient::Socket() const
{
	return m_Tag.Socket;
}

// =================================================================
TxSocketStream CxTcpClient::Stream() const
{
	return TxSocketStream(this->Socket(), TxIPEndPoint::Default(), this->EndPoint());
}

// =================================================================
TxIPAddress CxTcpClient::IPAddress() const
{
	return m_Tag.IPAddress;
}

// =================================================================
void CxTcpClient::IPAddress(const TxIPAddress& value)
{
	m_Tag.IPAddress = value;
}

// =================================================================
int CxTcpClient::Port() const
{
	return m_Tag.Port;
}

// =================================================================
void CxTcpClient::Port(int value)
{
	m_Tag.Port = value;
}

// =================================================================
TxIPEndPoint CxTcpClient::EndPoint() const
{
	return TxIPEndPoint(
		m_Tag.IPAddress,
		m_Tag.Port,
		AF_INET
		);
}

// =================================================================
void CxTcpClient::EndPoint(const TxIPEndPoint& value)
{
	m_Tag.IPAddress	= value.IPAddress;
	m_Tag.Port		= value.Port;
}

// =================================================================
bool CxTcpClient::Connected() const
{
	HxSocket sock = this->Socket();
	if (sock != XIE_INVALID_SOCKET)
	{
		char	buf[1];
		int		buflen = 1;

		int nfds = (int)(sock+1);

		fd_set fdsR; FD_ZERO(&fdsR); FD_SET(sock, &fdsR);

		struct timeval	tv;
		tv.tv_sec  = 0 / 1000;
		tv.tv_usec = 0 % 1000 * 1000;

		bool abnormal = (
			::select(nfds, &fdsR, NULL, NULL, &tv) > 0 &&
			::recv(sock, buf, buflen, MSG_PEEK) <= 0
			);
		
		return !abnormal;
	}
	return false;
}

// =================================================================
void CxTcpClient::Open()
{
	HxSocket sock = XIE_INVALID_SOCKET;

	int status = 0;

	// create socket
	sock = ::socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (sock != XIE_INVALID_SOCKET)
	{
		// connect
		sockaddr_in service = fnPRV_IPEndPoint_ToSockAddr(this->EndPoint());
		status = ::connect(sock, (sockaddr*)&service, sizeof(service));
		if (status == 0)
			this->m_Tag.Socket = sock;
		else
			::CLOSE_SOCKET(sock);
	}
}

// =================================================================
void CxTcpClient::Close()
{
	if (m_Tag.Socket != XIE_INVALID_SOCKET)
		::CLOSE_SOCKET(m_Tag.Socket);
	m_Tag.Socket = XIE_INVALID_SOCKET;
}

// =================================================================
void CxTcpClient::ThreadProc(void* sender, CxThreadArgs* e)
{
	try
	{
		if (this->Socket() == XIE_INVALID_SOCKET)
		{
			this->Open();
		}
		else if (this->Connected() == false)
		{
			this->Close();
		}
	}
	catch(const CxException&)
	{
		e->Cancellation = true;
	}
}

}
}
