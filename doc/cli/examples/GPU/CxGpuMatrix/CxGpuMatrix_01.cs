using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void CxGpuMatrix_01()
		{
			// ----- allocate
			var src = new XIE.GPU.CxGpuMatrix(3, 3);

			Console.WriteLine("{0,-20} {1}", "Rows", src.Rows);
			Console.WriteLine("{0,-20} {1}", "Columns", src.Columns);
			Console.WriteLine("{0,-20} {1},{2}", "Model", src.Model.Type, src.Model.Pack);

			// ----- release and allocate
			src.Resize(4, 3, XIE.TxModel.F32(1));

			Console.WriteLine("{0,-20} {1}", "Rows", src.Rows);
			Console.WriteLine("{0,-20} {1}", "Columns", src.Columns);
			Console.WriteLine("{0,-20} {1},{2}", "Model", src.Model.Type, src.Model.Pack);

			// ----- release
			src.Dispose();

			Console.WriteLine("{0,-20} {1}", "Rows", src.Rows);
			Console.WriteLine("{0,-20} {1}", "Columns", src.Columns);
			Console.WriteLine("{0,-20} {1},{2}", "Model", src.Model.Type, src.Model.Pack);
		}
	}
}
