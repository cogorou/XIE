using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void CxSerialPort_01()
		{
			var port1 = new XIE.IO.CxSerialPort();
			var port2 = new XIE.IO.CxSerialPort();
			var param = new XIE.IO.TxSerialPort(115200, XIE.IO.ExParity.None, 8, XIE.IO.ExStopBits.One, XIE.IO.ExHandshake.None);

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

			try
			{
				// Parameter
				port1.PortName = port1_name;
				port1.Param = param;

				port2.PortName = port2_name;
				port2.Param = param;

				// Setup (Open Port)
				port1.Setup();
				port2.Setup();

				// Write
				{
					string text = "Hello, World!!";
					byte[] data = Encoding.ASCII.GetBytes(text);
					int length = port1.Write(data, data.Length, 1000);
					Console.WriteLine("port1.Write = {0} bytes", length);
				}

				// Read
				{
					byte[] data = new byte[256];
					int length = port2.Read(data, data.Length, 0);
					string text = Encoding.ASCII.GetString(data, 0, length);
					Console.WriteLine("port2.Read = {0} bytes, data=({1})", length, text);
				}
			}
			finally
			{
				// Dispose (Close Port)
				port1.Dispose();
				port2.Dispose();
			}
		}
	}
}
