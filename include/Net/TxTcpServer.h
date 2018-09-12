/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXTCPSERVER_H_INCLUDED_
#define _TXTCPSERVER_H_INCLUDED_

#include "xie_high.h"

#include "Net/TxIPAddress.h"
#include "Net/TxSocketStream.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Net
{

struct XIE_EXPORT_CLASS TxTcpServer
{
	HxSocket			Socket;
	TxSocketStream*		Clients;
	int					Connections;
	TxIPAddress			IPAddress;
	int					Port;
	int					Backlog;

#if defined(__cplusplus)
	static inline TxTcpServer Default()
	{
		TxTcpServer result;
		result.Socket		= XIE_INVALID_SOCKET;
		result.Clients		= NULL;
		result.Connections	= 0;
		result.IPAddress	= TxIPAddress::Default();
		result.Port			= 0;
		result.Backlog		= 5;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxTcpServer();
#endif
};

}
}

#pragma pack(pop)

#endif
