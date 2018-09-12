/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXIPENDPOINT_H_INCLUDED_
#define _TXIPENDPOINT_H_INCLUDED_

#include "xie_high.h"

#include "Net/TxIPAddress.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Net
{

struct XIE_EXPORT_CLASS TxIPEndPoint
{
	TxIPAddress		IPAddress;
	int				Port;
	int				Family;

#if defined(__cplusplus)
	static inline TxIPEndPoint Default()
	{
		TxIPEndPoint result;
		result.IPAddress	= TxIPAddress::Any();
		result.Port			= 0;
		result.Family		= XIE_AF_INET;
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxIPEndPoint();
	TxIPEndPoint(TxIPAddress addr, int port);
	TxIPEndPoint(TxIPAddress addr, int port, int family);

	bool operator == (const TxIPEndPoint& cmp) const;
	bool operator != (const TxIPEndPoint& cmp) const;
#endif
};

}
}

#pragma pack(pop)

#endif
