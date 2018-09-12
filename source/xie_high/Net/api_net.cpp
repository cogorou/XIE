/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "api_net.h"

#include "Core/Axi.h"
#include "Core/CxException.h"
#include "Core/CxArrayEx.h"

#include "Net/CxTcpServer.h"
#include "Net/CxTcpClient.h"
#include "Net/CxUdpClient.h"
#include "Net/TxIPAddress.h"
#include "Net/TxIPEndPoint.h"
#include "Net/TxSocketStream.h"
#include "Net/TxTcpServer.h"
#include "Net/TxTcpClient.h"
#include "Net/TxUdpClient.h"

#include <math.h>

namespace xie
{
namespace Net
{

// ////////////////////////////////////////////////////////////
// EXTERN VARIABLE

static bool g_setuped = false;

// ======================================================================
void XIE_API fnPRV_Net_Setup()
{
	if (g_setuped) return;
	g_setuped = true;

	#if defined(_MSC_VER)
	WORD wVersionRequested = MAKEWORD(2, 2);
	WSADATA wsaData;
	int status = ::WSAStartup(wVersionRequested, &wsaData);
	#else
	#endif
}

// ======================================================================
void XIE_API fnPRV_Net_TearDown()
{
	#if defined(_MSC_VER)
	int status = ::WSACleanup();
	#else
	#endif
}

// ======================================================================
TxIPEndPoint XIE_API fnPRV_IPEndPoint_FromSockAddr(const sockaddr_in& src)
{
	TxIPEndPoint dst;
	dst.IPAddress	= TxIPAddress(src.sin_addr.s_addr);
	dst.Port		= src.sin_port;
	dst.Family		= src.sin_family;
	return dst;
}

// ======================================================================
sockaddr_in XIE_API fnPRV_IPEndPoint_ToSockAddr(const TxIPEndPoint& src)
{
	sockaddr_in dst;
	dst.sin_addr.s_addr	= src.IPAddress.ToUInt32();
	dst.sin_port		= src.Port;
	dst.sin_family		= src.Family;
	return dst;
}

}	// IO
}	// xie
