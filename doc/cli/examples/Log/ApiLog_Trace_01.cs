using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void Api_Trace_01()
		{
			var watch = new XIE.CxStopwatch();

			XIE.Log.Api.Trace("Filter Copy.");

			XIE.ExType[] types =
			{
				XIE.ExType.S32,
				XIE.ExType.F32,
				XIE.ExType.F64
			};

			foreach (var type in types)
			{
				var src = new XIE.CxImage(32, 24, XIE.TxModel.U8(3), 1);
				var dst = new XIE.CxImage(32, 24, new XIE.TxModel(type, 1), 3);
				dst.Filter().Copy(src);
				watch.Stop();

				XIE.Log.Api.Trace("{0,10}: {0:F3} msec", type, watch.Elapsed);
			}
		}
	}
}
