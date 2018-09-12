/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace demo
{
	partial class Program
	{
		static void Main(string[] args)
		{
			try
			{
				XIE.Axi.Setup();

				test01();	// SerialPort
				test02();	// UDP
				test03();	// TCP/IP
			}
			catch (System.Exception ex)
			{
				Console.WriteLine("{0}", ex.StackTrace);
			}
			finally
			{
				XIE.Axi.TearDown();
			}
		}

		/// <summary>
		/// SerialPort
		/// </summary>
		static void test01()
		{
			Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

			XIE.IO.CxSerialPort port1 = new XIE.IO.CxSerialPort();
			XIE.IO.CxSerialPort port2 = new XIE.IO.CxSerialPort();
			XIE.CxStopwatch watch = new XIE.CxStopwatch();

			try
			{
				int seqno = 0;

				// PortName
				string port1_name = "";
				string port2_name = "";
				switch (System.Environment.OSVersion.Platform)
				{
					case PlatformID.Unix:
						port1_name = "/dev/ttyS1";
						port2_name = "/dev/ttyS2";
						break;
					default:
						port1_name = "COM1";
						port2_name = "COM11";
						break;
				}

				// Param
				var param = new XIE.IO.TxSerialPort(115200, XIE.IO.ExParity.None, 8, XIE.IO.ExStopBits.One, XIE.IO.ExHandshake.None);

				port1.PortName = port1_name;
				port2.PortName = port2_name;
				port1.Param = param;
				port2.Param = param;
				port1.Setup();
				port2.Setup();

				// ----------------------------------------------------------------------
				Console.WriteLine("[{0}] port1.Write()", ++seqno);
				{
					string text = "hello";
					byte[] buf = Encoding.UTF8.GetBytes(text);

					watch.Start();
					int count = port1.Write(buf, buf.Length, 1000);
					watch.Stop();
					Console.WriteLine("[{0}] port1.Write returned {1}. [{2}], {3,9:F3} msec", seqno, count, text, watch.Lap);
				}

				// ----------------------------------------------------------------------
				Console.WriteLine("[{0}] port2.Read()", ++seqno);
				{
					byte[] buf = new byte[256];

					watch.Start();
					int count = port2.Read(buf, buf.Length, 1000);
					watch.Stop();

					string text = Encoding.UTF8.GetString(buf, 0, count);
					Console.WriteLine("[{0}] port2.Read returned {1}. [{2}], {3,9:F3} msec", seqno, count, text, watch.Lap);
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				port1.Dispose();
				port2.Dispose();
			}
		}

		/// <summary>
		/// UDP
		/// </summary>
		static void test02()
		{
			Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

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
			catch(System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				client1.Dispose();
				client2.Dispose();
			}
		}

		/// <summary>
		/// TCP/IP
		/// </summary>
		static void test03()
		{
			Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

			XIE.Net.CxTcpServer server = new XIE.Net.CxTcpServer();
			XIE.Net.CxTcpClient client1 = new XIE.Net.CxTcpClient();
			XIE.Net.CxTcpClient client2 = new XIE.Net.CxTcpClient();

			try
			{
				XIE.Net.TxIPEndPoint host = new XIE.Net.TxIPEndPoint(System.Net.IPAddress.Loopback, 5000);
				int seqno = 0;
		
				// ----------------------------------------------------------------------
				// server
				Console.WriteLine("[{0}] server", ++seqno);
				{
					server.EndPoint = host;
					server.Setup();
					server.Start();
				}

				// ----------------------------------------------------------------------
				// Wait until server is started.
				Console.WriteLine("[{0}] server.IsValid", ++seqno);
				for (int i = 0; i < 30; i++)
				{
					if (server.IsValid) break;
					System.Threading.Thread.Sleep(100);
				}

				// ----------------------------------------------------------------------
				// client1
				Console.WriteLine("[{0}] client1", ++seqno);
				{
					client1.EndPoint = host;
					client1.Setup();
					client1.Start();
				}

				// ----------------------------------------------------------------------
				// client2
				Console.WriteLine("[{0}] client2", ++seqno);
				{
					client2.EndPoint = host;
					client2.Setup();
					client2.Start();
				}

				// ----------------------------------------------------------------------
				// Wait for connection.
				++seqno;
				for(int i=0 ; i<5 ; i++)
				{
					System.Threading.Thread.Sleep(1000);
					int connections = server.Connections();
					Console.WriteLine("[{0}] server.Connections() = {1}", seqno, connections);
					if (connections >= 2) break;
				}

				// ----------------------------------------------------------------------
				// server Write.
				Console.WriteLine("[{0}] server.Write()", ++seqno);
				{
					for (int i = 0; i < server.Connections(); i++)
					{
						XIE.Net.TxSocketStream stream = server.Stream(i);

						string text = string.Format("Hello {0}!", (i + 1));
						byte[] buf = Encoding.UTF8.GetBytes(text);

						int count = stream.Write(buf, buf.Length, 1000);
						Console.WriteLine("[{0}] stream.Write returned {1}. [{2}]", seqno, count, text);
					}
				}

				// ----------------------------------------------------------------------
				// client1 Read.
				Console.WriteLine("[{0}] client1.Read()", ++seqno);
				{
					byte[] buf = new byte[256];

					XIE.Net.TxSocketStream stream = client1.Stream();
					int count = stream.Read(buf, buf.Length, 1000);
					string text = Encoding.UTF8.GetString(buf, 0, count);
					Console.WriteLine("[{0}] stream.Read returned {1}. [{2}]", seqno, count, text);
				}

				// ----------------------------------------------------------------------
				// client2 Read.
				Console.WriteLine("[{0}] client2.Read()", ++seqno);
				{
					byte[] buf = new byte[256];

					XIE.Net.TxSocketStream stream = client2.Stream();
					int count = stream.Read(buf, buf.Length, 1000);
					string text = (count <= 0) ? "" : Encoding.UTF8.GetString(buf, 0, count);
					Console.WriteLine("[{0}] stream.Read returned {1}. [{2}]", seqno, count, text);
				}

				// ----------------------------------------------------------------------
				// client1 Write.
				{
					Console.WriteLine("[{0}] client1.Write()", ++seqno);

					string text = "This is client1.";
					byte[] buf = Encoding.UTF8.GetBytes(text);

					XIE.Net.TxSocketStream stream = client1.Stream();
					int count = stream.Write(buf, buf.Length, 1000);
					Console.WriteLine("[{0}] stream.Write returned {1}. [{2}]", seqno, count, text);
				}

				// ----------------------------------------------------------------------
				// client2 Write.
				{
					Console.WriteLine("[{0}] client2.Write()", ++seqno);

					string text = "This is client2.";
					byte[] buf = Encoding.UTF8.GetBytes(text);

					XIE.Net.TxSocketStream stream = client2.Stream();
					int count = stream.Write(buf, buf.Length, 1000);
					Console.WriteLine("[{0}] stream.Write returned {1}. [{2}]", seqno, count, text);
				}

				// ----------------------------------------------------------------------
				// server Read.
				{
					Console.WriteLine("[{0}] server.Read()", ++seqno);

					for (int i = 0; i < server.Connections(); i++)
					{
						XIE.Net.TxSocketStream stream = server.Stream(i);
						byte[] buf = new byte[256];
						int count = stream.Read(buf, buf.Length, 1000);
						string text = (count <= 0) ? "" : Encoding.UTF8.GetString(buf, 0, count);
						Console.WriteLine("[{0}] stream.Read returned {1}. [{2}]", seqno, count, text);
					}
				}
			}
			catch(System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				client1.Dispose();
				client2.Dispose();
				server.Dispose();
			}
		}
	}
}
