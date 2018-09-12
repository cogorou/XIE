using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void CxArray_01()
		{
			// ----- allocate
			var src = new XIE.CxArray(320, XIE.TxModel.U8(4));

			Console.WriteLine("{0,-20} {1}", "Length", src.Length);
			Console.WriteLine("{0,-20} {1},{2}", "Model", src.Model.Type, src.Model.Pack);

			// ----- release and allocate
			src.Resize(240, XIE.TxModel.F64(3));

			Console.WriteLine("{0,-20} {1}", "Length", src.Length);
			Console.WriteLine("{0,-20} {1},{2}", "Model", src.Model.Type, src.Model.Pack);

			// ----- release
			src.Dispose();

			Console.WriteLine("{0,-20} {1}", "Length", src.Length);
			Console.WriteLine("{0,-20} {1},{2}", "Model", src.Model.Type, src.Model.Pack);
		}
	}
}
