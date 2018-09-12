using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void CxException_01()
		{
			try
			{
				throw new XIE.CxException(XIE.ExStatus.InvalidParam);
			}
			catch (XIE.CxException ex)
			{
				Console.WriteLine("Code={0}", ex.Code);
				Console.WriteLine("{0}", ex.StackTrace);
			}
		}
	}
}
