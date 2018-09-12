/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "api_net.h"
#include "Net/TxIPEndPoint.h"

namespace xie
{
namespace Net
{

// =================================================================
TxIPEndPoint::TxIPEndPoint()
{
	IPAddress	= TxIPAddress::Default();
	Port		= 0;
	Family		= AF_INET;
}

// =================================================================
TxIPEndPoint::TxIPEndPoint(TxIPAddress addr, int port)
{
	IPAddress	= addr;
	Port		= port;
	Family		= AF_INET;
}

// =================================================================
TxIPEndPoint::TxIPEndPoint(TxIPAddress addr, int port, int family)
{
	IPAddress	= addr;
	Port		= port;
	Family		= family;
}

// =================================================================
bool TxIPEndPoint::operator == (const TxIPEndPoint& cmp) const
{
	if (IPAddress	!= cmp.IPAddress) return false;
	if (Port		!= cmp.Port) return false;
	if (Family		!= cmp.Family) return false;
	return true;
}

// =================================================================
bool TxIPEndPoint::operator != (const TxIPEndPoint& cmp) const
{
	return !(operator == (cmp));
}

}
}
