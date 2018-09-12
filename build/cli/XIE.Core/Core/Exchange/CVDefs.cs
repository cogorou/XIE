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
	/// OpenCV 定数
	/// </summary>
	public static class CVDefs
	{
		/// <summary>
		/// 
		/// </summary>
		public const int CV_CN_MAX		= 512;

		/// <summary>
		/// 
		/// </summary>
		public const int CV_CN_SHIFT = 3;

		/// <summary>
		/// 
		/// </summary>
		public const int CV_DEPTH_MAX = (1 << CV_CN_SHIFT);

		/// <summary>
		/// 
		/// </summary>
		public const int CV_8U	= 0;

		/// <summary>
		/// 
		/// </summary>
		public const int CV_8S = 1;

		/// <summary>
		/// 
		/// </summary>
		public const int CV_16U = 2;

		/// <summary>
		/// 
		/// </summary>
		public const int CV_16S = 3;

		/// <summary>
		/// 
		/// </summary>
		public const int CV_32S = 4;

		/// <summary>
		/// 
		/// </summary>
		public const int CV_32F = 5;

		/// <summary>
		/// 
		/// </summary>
		public const int CV_64F = 6;

		/// <summary>
		/// 
		/// </summary>
		public const int CV_USRTYPE1 = 7;

		/// <summary>
		/// 
		/// </summary>
		public const int CV_MAT_DEPTH_MASK	= (CV_DEPTH_MAX - 1);

		/// <summary>
		/// 
		/// </summary>
		public const int CV_MAT_CN_MASK	= ((CV_CN_MAX - 1) << CV_CN_SHIFT);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static int CV_MAT_DEPTH(int flag)
		{
			return ((flag) & CV_MAT_DEPTH_MASK);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="flags"></param>
		/// <returns></returns>
		public static int CV_MAT_CN(int flags)
		{
			return ((((flags) & CV_MAT_CN_MASK) >> CV_CN_SHIFT) + 1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="depth"></param>
		/// <param name="cn"></param>
		/// <returns></returns>
		public static int CV_MAKETYPE(int depth, int cn)
		{
			return (CV_MAT_DEPTH(depth) + (((cn)-1) << CV_CN_SHIFT));
		}
	}
}
