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
	/// 配列走査構造体
	/// </summary>
	unsafe struct TxScanner
	{
		#region プロパティ:

		/// <summary>
		/// 領域の先頭アドレス
		/// </summary>
		public byte* Address;

		/// <summary>
		/// 要素のサイズ (bytes)
		/// </summary>
		public int ModelSize;

		/// <summary>
		/// 水平方向サイズ (bytes)
		/// </summary>
		public int Stride;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addr">配列の先頭アドレス</param>
		/// <param name="model_size">要素のサイズ (bytes)</param>
		/// <param name="stride">水平方向サイズ (bytes)</param>
		public TxScanner(IntPtr addr, int model_size, int stride)
		{
			Address = (byte*)addr.ToPointer();
			ModelSize = model_size;
			Stride = stride;
		}

		#endregion
	}
}
