using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void CxStopwatch_03()
		{
			XIE.CxStopwatch watch = new XIE.CxStopwatch();

			// 1
			System.Threading.Thread.Sleep(100);
			watch.Stop();
			Console.WriteLine("1: Lap={0,9:0.000} msec, Elapsed={1,9:0.000} msec", watch.Lap, watch.Elapsed);

			// 2
			System.Threading.Thread.Sleep(100);
			watch.Stop();
			Console.WriteLine("2: Lap={0,9:0.000} msec, Elapsed={1,9:0.000} msec", watch.Lap, watch.Elapsed);

			watch.Reset();

			// 3
			watch.Start();
			System.Threading.Thread.Sleep(100);
			watch.Stop();
			Console.WriteLine("3: Lap={0,9:0.000} msec, Elapsed={1,9:0.000} msec", watch.Lap, watch.Elapsed);
		}
	}
}
