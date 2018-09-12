using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void CxTcpServer_01()
		{
			XIE.Net.CxTcpServer server = new XIE.Net.CxTcpServer();
			XIE.Net.CxTcpClient client = new XIE.Net.CxTcpClient();

			try
			{
				XIE.Net.TxIPEndPoint host = new XIE.Net.TxIPEndPoint(System.Net.IPAddress.Loopback, 5000);

				// ----------------------------------------------------------------------
				// server
				{
					server.EndPoint = host;
					server.Setup();
					server.Start();
				}

				// ----------------------------------------------------------------------
				// Wait until server is started.
				for (int i = 0; i < 30; i++)
				{
					if (server.IsValid) break;
					System.Threading.Thread.Sleep(100);
				}

				// ----------------------------------------------------------------------
				// client
				{
					client.EndPoint = host;
					client.Setup();
					client.Start();
				}

				// ----------------------------------------------------------------------
				// Wait for connection.
				for (int i = 0; i < 5; i++)
				{
					System.Threading.Thread.Sleep(1000);
					int connections = server.Connections();
					if (connections >= 2) break;
				}

				// ----------------------------------------------------------------------
				// server Write.
				{
					int connections = server.Connections();
					for (int i = 0; i < connections; i++)
					{
						var stream = server.Stream(i);
						string text = string.Format("Hello {0}!", (i + 1));
						byte[] buf = Encoding.UTF8.GetBytes(text);
						int count = stream.Write(buf, buf.Length, 1000);
					}
				}

				// ----------------------------------------------------------------------
				// client Read.
				{
					byte[] buf = new byte[256];
					XIE.Net.TxSocketStream stream = client.Stream();
					int count = stream.Read(buf, buf.Length, 1000);
					string text = Encoding.UTF8.GetString(buf, 0, count);
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
