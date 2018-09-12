/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _TXIPADDRESS_H_INCLUDED_
#define _TXIPADDRESS_H_INCLUDED_

#include "xie_high.h"

#pragma pack(push,XIE_PACKING_SIZE)

namespace xie
{
namespace Net
{

struct XIE_EXPORT_CLASS TxIPAddress
{
	unsigned char	S1;
	unsigned char	S2;
	unsigned char	S3;
	unsigned char	S4;

#if defined(__cplusplus)
	static inline TxIPAddress Default()
	{
		TxIPAddress result = {0, 0, 0, 0};
		return result;
	}
	static inline TxIPAddress Any()
	{
		TxIPAddress result = {0, 0, 0, 0};
		return result;
	}
	static inline TxIPAddress Broadcast()
	{
		TxIPAddress result = {255, 255, 255, 255};
		return result;
	}
	static inline TxIPAddress Loopback()
	{
		TxIPAddress result = {127, 0, 0, 1};
		return result;
	}
#endif

#if defined(__cplusplus) && !defined(XIE_EXPORTS_DISABLED)
	TxIPAddress();
	TxIPAddress(unsigned char s1, unsigned char s2, unsigned char s3, unsigned char s4);
	TxIPAddress(unsigned int addr);
	static TxIPAddress From(TxCharCPtrA value);

	bool operator == (const TxIPAddress& cmp) const;
	bool operator != (const TxIPAddress& cmp) const;

	unsigned int ToUInt32() const;
#endif
};

}
}

#pragma pack(pop)

#endif
