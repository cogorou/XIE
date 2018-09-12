using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void CxImage_01()
		{
			// ----- allocate
			var src = new XIE.CxImage(320, 240, XIE.TxModel.U8(4), 1);

			Console.WriteLine("{0,-20} {1}", "Width", src.Width);
			Console.WriteLine("{0,-20} {1}", "Height", src.Height);
			Console.WriteLine("{0,-20} {1},{2}", "Model", src.Model.Type, src.Model.Pack);
			Console.WriteLine("{0,-20} {1}", "Channels", src.Channels);

			// ----- release and allocate
			src.Resize(320, 240, XIE.TxModel.F64(1), 3);

			Console.WriteLine("{0,-20} {1}", "Width", src.Width);
			Console.WriteLine("{0,-20} {1}", "Height", src.Height);
			Console.WriteLine("{0,-20} {1},{2}", "Model", src.Model.Type, src.Model.Pack);
			Console.WriteLine("{0,-20} {1}", "Channels", src.Channels);

			// ----- release
			src.Dispose();

			Console.WriteLine("{0,-20} {1}", "Width", src.Width);
			Console.WriteLine("{0,-20} {1}", "Height", src.Height);
			Console.WriteLine("{0,-20} {1},{2}", "Model", src.Model.Type, src.Model.Pack);
			Console.WriteLine("{0,-20} {1}", "Channels", src.Channels);
		}
	}
}
