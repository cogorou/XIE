/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XIE
{
	/// <summary>
	/// IPL 定数
	/// </summary>
	public static class IPLDefs
	{
		// ======================================================================

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_DEPTH_1U = 1;

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_DEPTH_8U = 8;

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_DEPTH_16U = 16;

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_DEPTH_32F = 32;

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_DEPTH_64F = 64;	// unofficial


		/// <summary>
		/// 
		/// </summary>
		public const int IPL_DEPTH_8S = unchecked((int)0x80000008);

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_DEPTH_16S = unchecked((int)0x80000010);

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_DEPTH_32S = unchecked((int)0x80000020);

		// ======================================================================

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_DATA_ORDER_PIXEL = 0;

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_DATA_ORDER_PLANE = 1;

		// ======================================================================

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_ORIGIN_TL = 0;

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_ORIGIN_BL = 1;

		// ======================================================================

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_ALIGN_DWORD = 4;

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_ALIGN_QWORD = 8;

		// ======================================================================

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_BORDER_CONSTANT = 0;

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_BORDER_REPLICATE = 1;

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_BORDER_REFLECT = 2;

		/// <summary>
		/// 
		/// </summary>
		public const int IPL_BORDER_WRAP = 3;
	}
}
