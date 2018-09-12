/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace XIE.Media
{
	/// <summary>
	/// メディア種別
	/// </summary>
	public enum ExMediaType
	{
		None = 0,
		Audio = 1,
		Video = 2,
	}

	/// <summary>
	/// メディア方向
	/// </summary>
	public enum ExMediaDir
	{
		None = 0,
		Input = 1,
		Output = 2,
	}

}
