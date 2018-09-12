using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void Api_Logs_02()
		{
			// all
			using (var writer = new StreamWriter("Axi_Logs.txt", false, Encoding.UTF8))
			{
				XIE.Log.Api.Logs.WriteToStream(writer);
			}

			// each level
			XIE.Log.ExLogLevel[] levels =
			{
				XIE.Log.ExLogLevel.Trace,
				XIE.Log.ExLogLevel.Error
			};

			foreach (var level in levels)
			{
				string filename = string.Format("Axi_Logs_{0}.txt", level);
				using (var writer = new StreamWriter(filename, false, Encoding.UTF8))
				{
					XIE.Log.Api.Logs.WriteToStream(writer, level);
				}
			}
		}
	}
}
