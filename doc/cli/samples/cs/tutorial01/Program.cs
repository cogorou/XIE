using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tutorial
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				XIE.Axi.Setup();

				string result_dir = System.IO.Path.GetFullPath("Results");
				if (!System.IO.Directory.Exists(result_dir))
					System.IO.Directory.CreateDirectory(result_dir);

				XIE.CxImage src = new XIE.CxImage("TestFiles/color_grad.bmp");
				XIE.CxImage dst = new XIE.CxImage();
				var efector = new XIE.Effectors.CxRgbToGray(0.299, 0.587, 0.114);
				efector.Execute(src, dst);
				dst.Save("Results/result.png");
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
	}
}
