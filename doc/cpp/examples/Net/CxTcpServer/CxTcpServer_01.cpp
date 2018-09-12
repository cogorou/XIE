
#include "xie_core.h"
#include "xie_high.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxTcpServer_01()
{
	printf("%s\n", __FUNCTION__);

	xie::Net::CxTcpServer server;
	xie::Net::CxTcpClient client1;
	xie::Net::CxTcpClient client2;

	try
	{
		xie::Net::TxIPEndPoint	host(xie::Net::TxIPAddress::Loopback(), 5000);
		int seqno = 0;
		
		// ----------------------------------------------------------------------
		// server
		printf("[%d] server\n", ++seqno);
		{
			server.EndPoint(host);
			server.Setup();
			server.Start();
		}

		// ----------------------------------------------------------------------
		// Wait until server is started.
		printf("[%d] server.IsValid\n", ++seqno);
		for(int i=0 ; i<30 ; i++)
		{
			if (server.IsValid()) break;
			xie::Axi::Sleep(100);
		}

		// ----------------------------------------------------------------------
		// client1
		printf("[%d] client1\n", ++seqno);
		{
			client1.EndPoint(host);
			client1.Setup();
			client1.Start();
		}

		// ----------------------------------------------------------------------
		// client2
		printf("[%d] client2\n", ++seqno);
		{
			client2.EndPoint(host);
			client2.Setup();
			client2.Start();
		}

		// ----------------------------------------------------------------------
		// Wait for connection.
		++seqno;
		for(int i=0 ; i<5 ; i++)
		{
			xie::Axi::Sleep(1000);
			int connections = server.Connections();
			printf("[%d] server.Connections() = %d\n", seqno, connections);
			if (connections >= 2) break;
		}

		// ----------------------------------------------------------------------
		// server Write.
		printf("[%d] server.Write()\n", ++seqno);
		{
			for (int i=0 ; i<server.Connections() ; i++)
			{
				xie::Net::TxSocketStream stream = server.Stream(i);
				xie::CxStringA buf = xie::CxStringA::Format("Hello %d!", (i+1));
				int count = stream.Write(buf.Address(), buf.Length(), 1000);
				printf("[%d] stream.Write returned %d. [%s]\n", seqno, count, buf.Address());
			}
		}

		// ----------------------------------------------------------------------
		// client1 Read.
		printf("[%d] client1.Read()\n", ++seqno);
		{
			const int	buflen = 256;
			char		buf[buflen] = {0};

			xie::Net::TxSocketStream stream = client1.Stream();
			int count = stream.Read(buf, buflen, 1000);
			printf("[%d] stream.Read returned %d. [%s]\n", seqno, count, buf);
		}

		// ----------------------------------------------------------------------
		// client2 Read.
		printf("[%d] client2.Read()\n", ++seqno);
		{
			const int	buflen = 256;
			char		buf[buflen] = {0};

			xie::Net::TxSocketStream stream = client2.Stream();
			int count = stream.Read(buf, buflen, 1000);
			printf("[%d] stream.Read returned %d. [%s]\n", seqno, count, buf);
		}

		// ----------------------------------------------------------------------
		// client1 Write.
		printf("[%d] client1.Write()\n", ++seqno);
		{
			char	buf[] = "This is client1.";
			int		buflen = strlen(buf);

			xie::Net::TxSocketStream stream = client1.Stream();
			int count = stream.Write(buf, buflen, 1000);
			printf("[%d] stream.Write returned %d. [%s]\n", seqno, count, buf);
		}

		// ----------------------------------------------------------------------
		// client2 Write.
		printf("[%d] client2.Write()\n", ++seqno);
		{
			char	buf[] = "This is client2.";
			int		buflen = strlen(buf);

			xie::Net::TxSocketStream stream = client2.Stream();
			int count = stream.Write(buf, buflen, 1000);
			printf("[%d] stream.Write returned %d. [%s]\n", seqno, count, buf);
		}

		// ----------------------------------------------------------------------
		// server Read.
		printf("[%d] server.Read()\n", ++seqno);
		{
			for (int i=0 ; i<server.Connections() ; i++)
			{
				xie::Net::TxSocketStream stream = server.Stream(i);
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

	client1.Dispose();
	client2.Dispose();
	server.Dispose();
}

}
