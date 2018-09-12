using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void CxUdpClient_01()
		{
			XIE.Net.CxUdpClient client1 = new XIE.Net.CxUdpClient();
			XIE.Net.CxUdpClient client2 = new XIE.Net.CxUdpClient();

			try
			{
				XIE.Net.TxIPEndPoint ep1 = new XIE.Net.TxIPEndPoint(System.Net.IPAddress.Loopback, 5001);
				XIE.Net.TxIPEndPoint ep2 = new XIE.Net.TxIPEndPoint(System.Net.IPAddress.Loopback, 5002);
				int seqno = 0;

				client1.EndPoint = ep1;
				client2.EndPoint = ep2;

				client1.Setup();
				client2.Setup();

				// ----------------------------------------------------------------------
				Console.WriteLine("[{0}] client1.Write()", ++seqno);
				{
					string text = "Hola, mundo!";
					byte[] buf = Encoding.UTF8.GetBytes(text);

					XIE.Net.TxIPEndPoint remoteEP = ep2;

					int count = client1.Write(buf, buf.Length, 1000, remoteEP);
					Console.WriteLine("[{0}] client1.Write returned {1}. [{2}]", seqno, count, text);
				}

				// ----------------------------------------------------------------------
				Console.WriteLine("[{0}] client2.Read()", ++seqno);
				if (client2.Readable(1000))
				{
					byte[] buf = new byte[256];

					XIE.Net.TxIPEndPoint remoteEP = new XIE.Net.TxIPEndPoint(System.Net.IPAddress.Any, 0);

					int count = client2.Read(buf, buf.Length, 1000, ref remoteEP);

					string text = Encoding.UTF8.GetString(buf, 0, count);
					Console.WriteLine("[{0}] client2.Read returned {1}. [{2}] (Port={3})", seqno, count, text, remoteEP.Port);
				}
			}
			finally
			{
				client1.Dispose();
				client2.Dispose();
			}
		}
	}
}
