
#include "xie_core.h"
#include "xie_high.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxTcpClient_01()
{
	printf("%s\n", __FUNCTION__);

	xie::Net::CxTcpServer server;
	xie::Net::CxTcpClient client;

	try
	{
		xie::Net::TxIPEndPoint	host(xie::Net::TxIPAddress::Loopback(), 5000);
		int seqno = 0;

		// ----------------------------------------------------------------------
		// client
		printf("[%d] client\n", ++seqno);
		{
			client.EndPoint(host);
			client.Setup();
			client.Start();
		}
		
		// ----------------------------------------------------------------------
		// server
		printf("[%d] server\n", ++seqno);
		{
			server.EndPoint(host);
			server.Setup();
			server.Start();
		}

		// ----------------------------------------------------------------------
		// Wait until client is valid.
		printf("[%d] client.IsValid\n", ++seqno);
		for(int i=0 ; i<30 ; i++)
		{
			if (client.IsValid())
			{
				printf("[%d] client.IsValid (i=%d)\n", seqno, i);
				break;
			}
			xie::Axi::Sleep(100);
		}

		// ----------------------------------------------------------------------
		// client Write.
		printf("[%d] client.Write()\n", ++seqno);
		{
			char	buf[] = "This is client.";
			int		buflen = strlen(buf);

			xie::Net::TxSocketStream stream = client.Stream();
			int count = stream.Write(buf, buflen, 1000);
			printf("[%d] stream.Write returned %d. [%s]\n", seqno, count, buf);
		}

		// ----------------------------------------------------------------------
		// server Read/Write.
		printf("[%d] server.Read()\n", ++seqno);
		{
			for (int i=0 ; i<server.Connections() ; i++)
			{
				xie::Net::TxSocketStream stream = server.Stream(i);

				// Read
				const int	buflen = 256;
				char		buf[buflen] = {0};
				int count = stream.Read(buf, buflen, 1000);
				printf("[%d] stream.Read returned %d. [%s]\n", seqno, count, buf);

				// Write
				xie::CxStringA ans = xie::CxStringA::Format("Hello %d!", (i+1));
				count = stream.Write(ans.Address(), ans.Length(), 1000);
				printf("[%d] stream.Write returned %d. [%s]\n", seqno, count, ans.Address());
			}
		}

		// ----------------------------------------------------------------------
		// client Read.
		printf("[%d] client.Read()\n", ++seqno);
		{
			const int	buflen = 256;
			char		buf[buflen] = {0};

			xie::Net::TxSocketStream stream = client.Stream();
			int count = stream.Read(buf, buflen, 1000);
			printf("[%d] stream.Read returned %d. [%s]\n", seqno, count, buf);
		}
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	client.Dispose();
	server.Dispose();
}

}
