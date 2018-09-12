/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Diagnostics;
using XIE.Ptr;

namespace XIE
{
	/// <summary>
	/// 定数
	/// </summary>
	/// <remarks>
	///		ネイティブ C++ ライブラリでマクロ定義(#define)されているものは、このクラスに集約されています。
	/// </remarks>
	public static partial class Defs : System.Object
	{
		#region 全般:

		/// <summary>
		/// XIE バージョン (major * 100 + minor)
		/// </summary>
		public const int XIE_VER = 100;

		/// <summary>
		/// XIE バージョン文字列 (major * 100 + minor)
		/// </summary>
		public const string XIE_VER_STR = "100";

		/// <summary>
		/// XIE モジュール識別子
		/// </summary>
		public const int XIE_MODULE_ID = 0x00454958;

		/// <summary>
		/// 構造体のパッキングサイズ (bytes)
		/// </summary>
		public const int XIE_PACKING_SIZE = 8;

		/// <summary>
		/// 画像のパッキングサイズ (bytes)
		/// </summary>
		public const int XIE_IMAGE_PACKING_SIZE = 4;

		/// <summary>
		/// 画像チャネル数最大
		/// </summary>
		public const int XIE_IMAGE_CHANNELS_MAX = 16;

		#endregion

		#region 型のレンジ:

		/// <summary>
		/// 8bit 符号なし整数 (最小値)
		/// </summary>
		public const byte XIE_U08Min = 0;

		/// <summary>
		/// 8bit 符号なし整数 (最大値)
		/// </summary>
		public const byte XIE_U08Max = 255;

		/// <summary>
		/// 16bit 符号なし整数 (最小値)
		/// </summary>
		public const ushort XIE_U16Min = 0;

		/// <summary>
		/// 16bit 符号なし整数 (最大値)
		/// </summary>
		public const ushort XIE_U16Max = 65535;

		/// <summary>
		/// 32bit 符号なし整数 (最小値)
		/// </summary>
		public const uint XIE_U32Min = 0;

		/// <summary>
		/// 32bit 符号なし整数 (最大値)
		/// </summary>
		public const uint XIE_U32Max = 4294967295;

		/// <summary>
		/// 64bit 符号なし整数 (最小値)
		/// </summary>
		public const ulong XIE_U64Min = 0UL;

		/// <summary>
		/// 64bit 符号なし整数 (最大値)
		/// </summary>
		public const ulong XIE_U64Max = 18446744073709551615UL;

		/// <summary>
		/// 8bit 符号つき整数 (最小値)
		/// </summary>
		public const sbyte XIE_S08Min = -128;

		/// <summary>
		/// 8bit 符号つき整数 (最大値)
		/// </summary>
		public const sbyte XIE_S08Max = +127;

		/// <summary>
		/// 16bit 符号つき整数 (最小値)
		/// </summary>
		public const short XIE_S16Min = -32768;

		/// <summary>
		/// 16bit 符号つき整数 (最大値)
		/// </summary>
		public const short XIE_S16Max = +32767;

		/// <summary>
		/// 32bit 符号つき整数 (最小値)
		/// </summary>
		public const int XIE_S32Min = -2147483648;

		/// <summary>
		/// 32bit 符号つき整数 (最大値)
		/// </summary>
		public const int XIE_S32Max = +2147483647;

		/// <summary>
		/// 64bit 符号つき整数 (最小値)
		/// </summary>
		public const long XIE_S64Min = -9223372036854775808L;

		/// <summary>
		/// 64bit 符号つき整数 (最大値)
		/// </summary>
		public const long XIE_S64Max = +9223372036854775807L;

		/// <summary>
		/// 32bit 単精度浮動小数点 (最小値)
		/// </summary>
		public const float XIE_F32Min = -3.402823466e+38F;

		/// <summary>
		/// 32bit 単精度浮動小数点 (最大値)
		/// </summary>
		public const float XIE_F32Max = +3.402823466e+38F;

		/// <summary>
		/// 64bit 倍精度浮動小数点 (最小値)
		/// </summary>
		public const double XIE_F64Min = -1.7976931348623158e+308;

		/// <summary>
		/// 64bit 倍精度浮動小数点 (最大値) 
		/// </summary>
		public const double XIE_F64Max = +1.7976931348623158e+308;

		#endregion

		#region 数理計算:

		/// <summary>
		/// 円周率
		/// </summary>
		public const double XIE_PI = 3.14159265358979323846;

		/// <summary>
		/// イプシロン (倍精度浮動小数点)
		/// </summary>
		public const double XIE_EPSd = 1.0e-9;

		/// <summary>
		/// イプシロン (単精度浮動小数点)
		/// </summary>
		public const float XIE_EPSf = 1.0e-5f;

		#endregion
	}
}
