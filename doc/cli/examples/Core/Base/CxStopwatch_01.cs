using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void CxStopwatch_01()
		{
			XIE.CxStopwatch watch = new XIE.CxStopwatch();
			watch.Start();
			System.Threading.Thread.Sleep(100);
			watch.Stop();
			Console.WriteLine("Lap={0,9:0.000} msec, Elapsed={0,9:0.000} msec", watch.Lap, watch.Elapsed);
		}
	}
}
