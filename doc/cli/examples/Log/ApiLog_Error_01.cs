using System;
using System.Collections.Generic;
using System.Text;

namespace User
{
	partial class Examples
	{
		[XIE.CxPluginExecute]
		public void Api_Error_01()
		{
			try
			{
				throw new XIE.CxException(XIE.ExStatus.InvalidParam);
			}
			catch (XIE.CxException ex)
			{
				XIE.Log.Api.Error("error occured. core={0}", ex.Code);
			}
			catch (Exception ex)
			{
				XIE.Log.Api.Error(ex.StackTrace);
			}
		}
	}
}
