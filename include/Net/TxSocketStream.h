/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXNETWORKSTREAM_H_INCLUDED_
#define _TXNETWORKSTREAM_H_INCLUDED_

#include "xie_high.h"

#include "Net/TxIPEndPoint.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Net
{

struct XIE_EXPORT_CLASS TxSocketStream
{
	HxSocket		Socket;
	TxIPEndPoint	LocalEndPoint;
	TxIPEndPoint	RemoteEndPoint;

#if defined(__cplusplus)
	static inline TxSocketStream Default()
	{
		TxSocketStream result;
		result.Socket			= XIE_INVALID_SOCKET;
		result.LocalEndPoint	= TxIPEndPoint::Default();
		result.RemoteEndPoint	= TxIPEndPoint::Default();
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxSocketStream();
	TxSocketStream(HxSocket sock);
	TxSocketStream(HxSocket sock, TxIPEndPoint localeEP, TxIPEndPoint remoteEP);

	bool operator == (const TxSocketStream& cmp) const;
	bool operator != (const TxSocketStream& cmp) const;

	bool Connected() const;

	bool Readable(int timeout) const;
	int Read(char* buffer, int length, int timeout) const;

	bool Writeable(int timeout) const;
	int Write(const char* buffer, int length, int timeout) const;
#endif
};

}
}

#pragma pack(pop)

#endif
