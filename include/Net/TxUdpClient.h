/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXUDPCLIENT_H_INCLUDED_
#define _TXUDPCLIENT_H_INCLUDED_

#include "xie_high.h"

#include "Net/TxIPAddress.h"
#include "Net/TxIPEndPoint.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Net
{

struct XIE_EXPORT_CLASS TxUdpClient
{
	HxSocket		Socket;
	TxIPAddress		IPAddress;
	int				Port;

#if defined(__cplusplus)
	static inline TxUdpClient Default()
	{
		TxUdpClient result;
		result.Socket		= XIE_INVALID_SOCKET;
		result.IPAddress	= TxIPAddress::Default();
		result.Port			= 0;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxUdpClient();
#endif
};

}
}

#pragma pack(pop)

#endif
