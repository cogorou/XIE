/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#include "xie_high.h"
#include "api_net.h"
#include "Net/TxTcpServer.h"

namespace xie
{
namespace Net
{

// =================================================================
TxTcpServer::TxTcpServer()
{
	Socket		= XIE_INVALID_SOCKET;
	Clients		= NULL;
	Connections	= 0;
	IPAddress	= TxIPAddress::Default();
	Port		= 0;
	Backlog		= 5;
}

}
}
