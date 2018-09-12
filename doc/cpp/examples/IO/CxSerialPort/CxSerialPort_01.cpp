
#include "xie_core.h"
#include "xie_high.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxSerialPort_01()
{
	printf("%s\n", __FUNCTION__);

	xie::IO::CxSerialPort port1;
	xie::IO::CxSerialPort port2;
	xie::CxStopwatch watch;

	try
	{
		int seqno = 0;

		// PortName
		xie::CxString port1_name = "";
		xie::CxString port2_name = "";
#if defined(_MSC_VER)
		port1_name = "COM1";
		port2_name = "COM11";
#else
		port1_name = "/dev/ttyS1";
		port2_name = "/dev/ttyS2";
#endif

		// Param
		xie::IO::TxSerialPort param(
			115200,
			xie::IO::ExParity::None,
			8,
			xie::IO::ExStopBits::One,
			xie::IO::ExHandshake::None
			);

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
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	port1.Dispose();
	port2.Dispose();
}

}
