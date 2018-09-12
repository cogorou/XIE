
#include "xie_core.h"
#include "xie_high.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxUdpClient_01()
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

}
