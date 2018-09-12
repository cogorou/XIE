/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _API_NET_H_INCLUDED_
#define _API_NET_H_INCLUDED_

#include "xie_high.h"
#include "Core/xie_core_defs.h"
#include "Net/TxIPEndPoint.h"

#if defined(_MSC_VER)
	#include <winsock2.h>
#else
	#include <sys/socket.h>
	#include <netinet/in.h>
	#include <arpa/inet.h>
#endif

// ////////////////////////////////////////////////////////////
// TYPEDEF

#if defined(_MSC_VER)
typedef int	socklen_t;
#endif

#if defined(_MSC_VER)
#define CLOSE_SOCKET(sock)	closesocket(sock);
#else
#define CLOSE_SOCKET(sock)	close(sock);
#endif

// ////////////////////////////////////////////////////////////
// PROTOTYPE

namespace xie
{
namespace Net
{

void XIE_API fnPRV_Net_Setup();
void XIE_API fnPRV_Net_TearDown();

TxIPEndPoint XIE_API fnPRV_IPEndPoint_FromSockAddr(const sockaddr_in& src);
sockaddr_in XIE_API fnPRV_IPEndPoint_ToSockAddr(const TxIPEndPoint& src);

}	// IO
}	// xie

#endif
