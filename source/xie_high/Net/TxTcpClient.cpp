/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "api_net.h"
#include "Net/TxTcpClient.h"

namespace xie
{
namespace Net
{

// =================================================================
TxTcpClient::TxTcpClient()
{
	Socket		= XIE_INVALID_SOCKET;
	IPAddress	= TxIPAddress::Default();
	Port		= 0;
}

}
}
