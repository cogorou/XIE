/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "api_net.h"
#include "Net/CxUdpClient.h"
#include "Net/TxSocketStream.h"
#include "Core/Axi.h"
#include "Core/CxStopwatch.h"
#include "Core/CxException.h"

namespace xie
{
namespace Net
{

static const char* g_ClassName = "CxUdpClient";

// =================================================================
void CxUdpClient::_Constructor()
{
	m_Tag = TxUdpClient::Default();
}

// =================================================================
CxUdpClient::CxUdpClient()
{
	_Constructor();
}

// =================================================================
CxUdpClient::CxUdpClient( const CxUdpClient& src )
{
	_Constructor();
	operator = (src);
}

// =================================================================
CxUdpClient::CxUdpClient(const TxIPAddress& addr, int port)
{
	_Constructor();
	m_Tag.IPAddress	= addr;
	m_Tag.Port		= port;
}

// =================================================================
CxUdpClient::CxUdpClient(const TxIPEndPoint& endpoint)
{
	_Constructor();
	m_Tag.IPAddress	= endpoint.IPAddress;
	m_Tag.Port		= endpoint.Port;
}

// =================================================================
CxUdpClient::~CxUdpClient()
{
	Dispose();
}

// ============================================================
CxUdpClient& CxUdpClient::operator = ( const CxUdpClient& src )
{
	m_Tag.IPAddress	= src.m_Tag.IPAddress;
	m_Tag.Port		= src.m_Tag.Port;
	return *this;
}

// ============================================================
bool CxUdpClient::operator == ( const CxUdpClient& src ) const
{
	if (m_Tag.IPAddress	!= src.m_Tag.IPAddress) return false;
	if (m_Tag.Port		!= src.m_Tag.Port) return false;
	return true;
}

// ============================================================
bool CxUdpClient::operator != ( const CxUdpClient& src ) const
{
	return !(CxUdpClient::operator == (src));
}

// =================================================================
TxUdpClient CxUdpClient::Tag() const
{
	return m_Tag;
}

// =================================================================
void* CxUdpClient::TagPtr() const
{
	return (void*)&m_Tag;
}

// =================================================================
void CxUdpClient::Setup()
{
	Dispose();

	HxSocket sock = XIE_INVALID_SOCKET;

	try
	{
		int status = 0;

		// create socket
		sock = ::socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
		if (sock == XIE_INVALID_SOCKET)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		// reuse address
		int resuse = 1;
		::setsockopt(sock, SOL_SOCKET, SO_REUSEADDR, (const char*)&resuse, sizeof(resuse));

		// bind
		sockaddr_in service = fnPRV_IPEndPoint_ToSockAddr(this->EndPoint());
		status = ::bind(sock, (sockaddr*)&service, sizeof(service));
		if (status != 0)
			throw CxException(ExStatus::IOError, __FUNCTION__, __FILE__, __LINE__);

		m_Tag.Socket = sock;
	}
	catch(const CxException& ex)
	{
		::CLOSE_SOCKET(sock);
		throw CxException(ex);
	}
}

// =================================================================
void CxUdpClient::Dispose()
{
	if (m_Tag.Socket != XIE_INVALID_SOCKET)
		::CLOSE_SOCKET(m_Tag.Socket);
	m_Tag.Socket = XIE_INVALID_SOCKET;
}

// =================================================================
bool CxUdpClient::IsValid() const
{
	if (m_Tag.Socket == XIE_INVALID_SOCKET) return false;
	return true;
}

// =================================================================
bool CxUdpClient::Readable(int timeout) const
{
	HxSocket sock = this->Socket();

	if (sock != XIE_INVALID_SOCKET && sock < 0x7FFFFFFF)
	{
		int		nfds = (int)(sock + 1);

		fd_set	fds; FD_ZERO(&fds); FD_SET(sock, &fds);

		struct timeval	tv;
		tv.tv_sec  = timeout / 1000;
		tv.tv_usec = timeout % 1000 * 1000;

		return (0 < ::select(nfds, &fds, NULL, NULL, &tv));
	}
	return false;
}

// =================================================================
int CxUdpClient::Read(char* buffer, int length, int timeout, TxIPEndPoint& remoteEP) const
{
	if (Readable(timeout) == false) return 0;

	HxSocket sock = this->Socket();

	sockaddr_in dst_addr = fnPRV_IPEndPoint_ToSockAddr(remoteEP);
	socklen_t	dst_len = sizeof(dst_addr);
	int			flags = 0;

	#if defined(_MSC_VER)
	#else
	flags = MSG_DONTWAIT;
	#endif

	int status = ::recvfrom(sock, buffer, length, flags, (sockaddr*)&dst_addr, &dst_len);

	if (status > 0)
	{
		remoteEP = fnPRV_IPEndPoint_FromSockAddr(dst_addr);
	}

	return status;
}

// =================================================================
bool CxUdpClient::Writeable(int timeout) const
{
	HxSocket sock = this->Socket();

	if (sock != XIE_INVALID_SOCKET && sock < 0x7FFFFFFF)
	{
		int		nfds = (int)(sock + 1);

		fd_set	fds; FD_ZERO(&fds); FD_SET(sock, &fds);

		struct timeval	tv;
		tv.tv_sec  = timeout / 1000;
		tv.tv_usec = timeout % 1000 * 1000;

		return (0 < ::select(nfds, NULL, &fds, NULL, &tv));
	}
	return false;
}

// =================================================================
int CxUdpClient::Write(const char* buffer, int length, int timeout, const TxIPEndPoint& remoteEP) const
{
	if (Writeable(timeout) == false) return 0;

	HxSocket sock = this->Socket();

	sockaddr_in dst_addr = fnPRV_IPEndPoint_ToSockAddr(remoteEP);
	socklen_t	dst_len = sizeof(dst_addr);
	int			flags = 0;

	#if defined(_MSC_VER)
	#else
	flags = MSG_DONTWAIT;
	#endif

	int status = ::sendto(sock, buffer, length, flags, (sockaddr*)&dst_addr, dst_len);

	return status;
}

// =================================================================
HxSocket CxUdpClient::Socket() const
{
	return m_Tag.Socket;
}

// =================================================================
TxIPAddress CxUdpClient::IPAddress() const
{
	return m_Tag.IPAddress;
}

// =================================================================
void CxUdpClient::IPAddress(const TxIPAddress& value)
{
	m_Tag.IPAddress = value;
}

// =================================================================
int CxUdpClient::Port() const
{
	return m_Tag.Port;
}

// =================================================================
void CxUdpClient::Port(int value)
{
	m_Tag.Port = value;
}

// =================================================================
TxIPEndPoint CxUdpClient::EndPoint() const
{
	return TxIPEndPoint(
		m_Tag.IPAddress,
		m_Tag.Port,
		AF_INET
		);
}

// =================================================================
void CxUdpClient::EndPoint(const TxIPEndPoint& value)
{
	m_Tag.IPAddress	= value.IPAddress;
	m_Tag.Port		= value.Port;
}

}
}
