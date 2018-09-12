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

namespace XIE.Media
{
	/// <summary>
	/// デバイスパラメータ構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxDeviceParam :
		IEquatable<TxDeviceParam>
	{
		#region プロパティ:

		/// <summary>
		/// デバイス名称
		/// </summary>
		public IntPtr Name;

		/// <summary>
		/// デバイス指標
		/// </summary>
		public int Index;

		/// <summary>
		/// ピン指標
		/// </summary>
		public int Pin;

		/// <summary>
		/// フレームサイズ(幅)
		/// </summary>
		public int Width;

		/// <summary>
		/// フレームサイズ(高さ)
		/// </summary>
		public int Height;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">デバイス名称</param>
		/// <param name="index">デバイス指標</param>
		/// <param name="pin">ピン指標</param>
		/// <param name="width">フレームサイズ(幅)</param>
		/// <param name="height">フレームサイズ(高さ)</param>
		public TxDeviceParam(IntPtr name, int index, int pin, int width, int height)
		{
			Name = name;
			Index = index;
			Pin = pin;
			Width = width;
			Height = height;
		}

		#endregion

		#region 比較:

		/// <summary>
		/// IEquatable の実装: 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="other">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public bool Equals(TxDeviceParam other)
		{
			if (this.Name != other.Name) return false;
			if (this.Index != other.Index) return false;
			if (this.Pin != other.Pin) return false;
			if (this.Width != other.Width) return false;
			if (this.Height != other.Height) return false;
			return true;
		}

		/// <summary>
		/// 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="obj">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			if (!(obj is TxDeviceParam)) return false;
			return this.Equals((TxDeviceParam)obj);
		}

		/// <summary>
		/// ハッシュ値の取得
		/// </summary>
		/// <returns>
		///		常に 0 を返します。
		/// </returns>
		public override int GetHashCode()
		{
			return 0;
		}

		#endregion

		#region 比較オペレータ:

		/// <summary>
		/// ２つのオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public static bool operator ==(TxDeviceParam left, TxDeviceParam right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// ２つのオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		内容が等しい場合は false、それ以外は true を返します。
		/// </returns>
		public static bool operator !=(TxDeviceParam left, TxDeviceParam right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}
