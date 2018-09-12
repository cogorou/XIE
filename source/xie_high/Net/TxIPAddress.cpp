/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "api_net.h"
#include "Net/TxIPAddress.h"

#if defined(_MSC_VER)
#include "Ws2tcpip.h"	// require for InetPton
#endif

namespace xie
{
namespace Net
{

// =================================================================
TxIPAddress::TxIPAddress()
{
	S1 = 0;
	S2 = 0;
	S3 = 0;
	S4 = 0;
}

// =================================================================
TxIPAddress::TxIPAddress(unsigned char s1, unsigned char s2, unsigned char s3, unsigned char s4)
{
	S1 = s1;
	S2 = s2;
	S3 = s3;
	S4 = s4;
}

// =================================================================
TxIPAddress::TxIPAddress(unsigned int addr)
{
	TxIPAddress* paddr = reinterpret_cast<TxIPAddress*>(&addr);
	S1 = paddr->S1;
	S2 = paddr->S2;
	S3 = paddr->S3;
	S4 = paddr->S4;
}

// =================================================================
TxIPAddress TxIPAddress::From(TxCharCPtrA value)
{
#if defined(_MSC_VER)
	in_addr addr;
	if (0 == InetPton(AF_INET, value, &addr))
		return TxIPAddress();
	return TxIPAddress(addr.S_un.S_addr);
#else
	in_addr addr;
	if (0 == inet_pton(AF_INET, value, &addr))
		return TxIPAddress();
	return TxIPAddress(addr.s_addr);
#endif
}

// =================================================================
bool TxIPAddress::operator == (const TxIPAddress& cmp) const
{
	if (S1	!= cmp.S1) return false;
	if (S2	!= cmp.S2) return false;
	if (S3	!= cmp.S3) return false;
	if (S4	!= cmp.S4) return false;
	return true;
}

// =================================================================
bool TxIPAddress::operator != (const TxIPAddress& cmp) const
{
	return !(operator == (cmp));
}

// =================================================================
unsigned int TxIPAddress::ToUInt32() const
{
	return *reinterpret_cast<const unsigned int*>(this);
}

}
}
