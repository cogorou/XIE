using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void CxTcpClient_01()
		{
			XIE.Net.CxTcpClient client = new XIE.Net.CxTcpClient();
			XIE.Net.CxTcpServer server = new XIE.Net.CxTcpServer();

			try
			{
				XIE.Net.TxIPEndPoint host = new XIE.Net.TxIPEndPoint(System.Net.IPAddress.Loopback, 5000);

				// ----------------------------------------------------------------------
				// client
				{
					client.EndPoint = host;
					client.Setup();
					client.Start();
				}

				// ----------------------------------------------------------------------
				// server
				{
					server.EndPoint = host;
					server.Setup();
					server.Start();
				}

				// ----------------------------------------------------------------------
				// Wait until client is started.
				for (int i = 0; i < 30; i++)
				{
					if (client.IsValid) break;
					System.Threading.Thread.Sleep(100);
				}

				// ----------------------------------------------------------------------
				// client Write.
				{
					string text = "This is client.";
					byte[] buf = Encoding.UTF8.GetBytes(text);
					XIE.Net.TxSocketStream stream = client.Stream();
					int count = stream.Write(buf, buf.Length, 1000);
				}

				// ----------------------------------------------------------------------
				// server Read.
				{
					int connections = server.Connections();
					for (int i = 0; i < connections; i++)
					{
						var stream = server.Stream(i);
						byte[] buf = new byte[256];
						int count = stream.Read(buf, buf.Length, 1000);
						string text = (count <= 0) ? "" : Encoding.UTF8.GetString(buf, 0, count);
					}
				}
			}
			finally
			{
				client.Dispose();
				server.Dispose();
			}
		}
	}
}
