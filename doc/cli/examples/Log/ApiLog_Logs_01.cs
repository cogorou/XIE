using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void Api_Logs_01()
		{
			try
			{
				var src = new XIE.CxImage(32, 24, XIE.TxModel.U8(3), 1);
				var dst = new XIE.CxImage(32, 24, XIE.TxModel.F64(1), 3);

				var watch = new XIE.CxStopwatch();
				dst.Filter().Copy(src);
				watch.Stop();

				// Trace
				XIE.Log.Api.Logs.Add(
					XIE.Log.ExLogLevel.Trace,
					"Filter Copy. {0:F3} msec",
					watch.Elapsed
				);
			}
			catch (XIE.CxException ex)
			{
				// Error
				XIE.Log.Api.Logs.Add(
					XIE.Log.ExLogLevel.Error,
					"Error occured. core={0}",
					ex.Code
				);
			}
			catch (Exception ex)
			{
				// Error
				XIE.Log.Api.Logs.Add(
					XIE.Log.ExLogLevel.Error,
					ex.StackTrace
					);
			}
		}
	}
}
