/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#if defined(_MSC_VER)
	#include "stdafx.h"
#else
	#include <stdio.h>
	#include <stdlib.h>
	#include <stddef.h>
#endif

#include "xie_core.h"
#include "xie_high.h"
#include "xie.h"

void test01();
void test02();
void test03();

// ==================================================
/*!
	@brief	EntryPoint
*/
int main(int argc, const char* argv[])
{
	xie::Axi::Setup();

	try
	{
		test01();	// SerialPort
		test02();	// UDP
		test03();	// TCP/IP
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	xie::Axi::TearDown();

	return 0;
}

// ==================================================
/*!
	@brief	SerialPort
*/
void test01()
{
	printf("%s\n", __FUNCTION__);

	xie::IO::CxSerialPort port1;
	xie::IO::CxSerialPort port2;
	xie::CxStopwatch watch;

	try
	{
		int seqno = 0;

		// PortName
		xie::CxStringA port1_name = "";
		xie::CxStringA port2_name = "";
#if defined(_MSC_VER)
		port1_name = "COM1";
		port2_name = "COM11";
#else
		port1_name = "/dev/ttyS1";
		port2_name = "/dev/ttyS2";
#endif

		// Param
		xie::IO::TxSerialPort param(115200, xie::IO::ExParity::None, 8, xie::IO::ExStopBits::One, xie::IO::ExHandshake::None);

		port1.PortName(port1_name);
		port2.PortName(port2_name);
		port1.Param(param);
		port2.Param(param);
		port1.Setup();
		port2.Setup();

		// ----------------------------------------------------------------------
		printf("[%d] port1.Write()\n", ++seqno);
		{
			const char*	buf = "hello";
			const int	buflen = strlen(buf);

			watch.Start();
			int count = port1.Write(buf, buflen, 1000);
			watch.Stop();
			printf("[%d] port1.Write returned %d. [%s], %9.3f msec\n", seqno, count, buf, watch.Lap);
		}

		// ----------------------------------------------------------------------
		printf("[%d] port2.Read()\n", ++seqno);
		{
			const int	buflen = 256;
			char		buf[buflen] = {0};

			watch.Start();
			int count = port2.Read(buf, buflen, 1000);
			watch.Stop();
			printf("[%d] port2.Read returned %d. [%s], %9.3f msec\n", seqno, count, buf, watch.Lap);
		}
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	port1.Dispose();
	port2.Dispose();
}

// ==================================================
/*!
	@brief	UDP
*/
void test02()
{
	printf("%s\n", __FUNCTION__);

	xie::Net::CxUdpClient client1;
	xie::Net::CxUdpClient client2;

	try
	{
		const xie::Net::TxIPEndPoint	ep1(xie::Net::TxIPAddress::Loopback(), 5001);
		const xie::Net::TxIPEndPoint	ep2(xie::Net::TxIPAddress::Loopback(), 5002);
		int seqno = 0;

		client1.EndPoint(ep1);
		client2.EndPoint(ep2);

		client1.Setup();
		client2.Setup();

		// ----------------------------------------------------------------------
		printf("[%d] client1.Write()\n", ++seqno);
		{
			const char*	buf = "Hola, mundo!";
			int			buflen = strlen(buf);

			xie::Net::TxIPEndPoint	remoteEP = ep2;

			int count = client1.Write(buf, buflen, 1000, remoteEP);
			printf("[%d] client1.Write returned %d. [%s]\n", seqno, count, buf);
		}

		// ----------------------------------------------------------------------
		printf("[%d] client2.Read()\n", ++seqno);
		if (client2.Readable(1000))
		{
			const int	buflen = 256;
			char		buf[buflen] = {0};

			xie::Net::TxIPEndPoint	remoteEP(xie::Net::TxIPAddress::Any(), 0);

			int count = client2.Read(buf, buflen, 1000, remoteEP);
			printf("[%d] client2.Read returned %d. [%s] (Port=%d)\n", seqno, count, buf, remoteEP.Port);
		}
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	client1.Dispose();
	client2.Dispose();
}

// ==================================================
/*!
	@brief	TCP/IP
*/
void test03()
{
	printf("%s\n", __FUNCTION__);

	xie::Net::CxTcpServer Server;
	xie::Net::CxTcpClient Client1;
	xie::Net::CxTcpClient Client2;

	try
	{
		xie::Net::TxIPEndPoint	host(xie::Net::TxIPAddress::Loopback(), 5000);
		int seqno = 0;
		
		// ----------------------------------------------------------------------
		// Server
		printf("[%d] Server\n", ++seqno);
		{
			Server.EndPoint(host);
			Server.Setup();
			Server.Start();
		}

		// ----------------------------------------------------------------------
		// Wait until Server is started.
		printf("[%d] Server.IsValid\n", ++seqno);
		for(int i=0 ; i<30 ; i++)
		{
			if (Server.IsValid()) break;
			xie::Axi::Sleep(100);
		}

		// ----------------------------------------------------------------------
		// Client1
		printf("[%d] Client1\n", ++seqno);
		{
			Client1.EndPoint(host);
			Client1.Setup();
			Client1.Start();
		}

		// ----------------------------------------------------------------------
		// Client2
		printf("[%d] Client2\n", ++seqno);
		{
			Client2.EndPoint(host);
			Client2.Setup();
			Client2.Start();
		}

		// ----------------------------------------------------------------------
		// Wait for connection.
		++seqno;
		for(int i=0 ; i<5 ; i++)
		{
			xie::Axi::Sleep(1000);
			int connections = Server.Connections();
			printf("[%d] Server.Connections() = %d\n", seqno, connections);
			if (connections >= 2) break;
		}

		// ----------------------------------------------------------------------
		// Server Write.
		printf("[%d] Server.Write()\n", ++seqno);
		{
			for (int i=0 ; i<Server.Connections() ; i++)
			{
				xie::Net::TxSocketStream stream = Server.Stream(i);
				xie::CxStringA buf = xie::CxStringA::Format("Hello %d!", (i+1));
				int count = stream.Write(buf.Address(), buf.Length(), 1000);
				printf("[%d] stream.Write returned %d. [%s]\n", seqno, count, buf.Address());
			}
		}

		// ----------------------------------------------------------------------
		// Client1 Read.
		printf("[%d] Client1.Read()\n", ++seqno);
		{
			const int	buflen = 256;
			char		buf[buflen] = {0};

			xie::Net::TxSocketStream stream = Client1.Stream();
			int count = stream.Read(buf, buflen, 1000);
			printf("[%d] stream.Read returned %d. [%s]\n", seqno, count, buf);
		}

		// ----------------------------------------------------------------------
		// Client2 Read.
		printf("[%d] Client2.Read()\n", ++seqno);
		{
			const int	buflen = 256;
			char		buf[buflen] = {0};

			xie::Net::TxSocketStream stream = Client2.Stream();
			int count = stream.Read(buf, buflen, 1000);
			printf("[%d] stream.Read returned %d. [%s]\n", seqno, count, buf);
		}

		// ----------------------------------------------------------------------
		// Client1 Write.
		printf("[%d] Client1.Write()\n", ++seqno);
		{
			char	buf[] = "This is Client1.";
			int		buflen = strlen(buf);

			xie::Net::TxSocketStream stream = Client1.Stream();
			int count = stream.Write(buf, buflen, 1000);
			printf("[%d] stream.Write returned %d. [%s]\n", seqno, count, buf);
		}

		// ----------------------------------------------------------------------
		// Client2 Write.
		printf("[%d] Client2.Write()\n", ++seqno);
		{
			char	buf[] = "This is Client2.";
			int		buflen = strlen(buf);

			xie::Net::TxSocketStream stream = Client2.Stream();
			int count = stream.Write(buf, buflen, 1000);
			printf("[%d] stream.Write returned %d. [%s]\n", seqno, count, buf);
		}

		// ----------------------------------------------------------------------
		// Server Read.
		printf("[%d] Server.Read()\n", ++seqno);
		{
			for (int i=0 ; i<Server.Connections() ; i++)
			{
				xie::Net::TxSocketStream stream = Server.Stream(i);
				const int	buflen = 256;
				char		buf[buflen] = {0};
				int count = stream.Read(buf, buflen, 1000);
				printf("[%d] stream.Read returned %d. [%s]\n", seqno, count, buf);
			}
		}
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	Client1.Dispose();
	Client2.Dispose();
	Server.Dispose();
}
