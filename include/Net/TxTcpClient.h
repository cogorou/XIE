/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXTCPCLIENT_H_INCLUDED_
#define _TXTCPCLIENT_H_INCLUDED_

#include "xie_high.h"

#include "Net/TxIPAddress.h"
#include "Net/TxIPEndPoint.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Net
{

struct XIE_EXPORT_CLASS TxTcpClient
{
	HxSocket		Socket;
	TxIPAddress		IPAddress;
	int				Port;

#if defined(__cplusplus)
	static inline TxTcpClient Default()
	{
		TxTcpClient result;
		result.Socket		= XIE_INVALID_SOCKET;
		result.IPAddress	= TxIPAddress::Default();
		result.Port			= 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxTcpClient();
#endif
};

}
}

#pragma pack(pop)

#endif
