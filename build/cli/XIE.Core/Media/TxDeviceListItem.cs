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
	/// デバイスリスト項目構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxDeviceListItem :
		IEquatable<TxDeviceListItem>
	{
		#region プロパティ:

		/// <summary>
		/// メディア種別
		/// </summary>
		public ExMediaType MediaType;

		/// <summary>
		/// メディア方向
		/// </summary>
		public ExMediaDir MediaDir;

		/// <summary>
		/// デバイス名称
		/// </summary>
		public IntPtr Name;

		/// <summary>
		/// デバイス指標
		/// </summary>
		public int Index;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">メディア種別</param>
		/// <param name="dir">メディア方向</param>
		/// <param name="name">デバイス名称</param>
		/// <param name="index">デバイス指標</param>
		public TxDeviceListItem(ExMediaType type, ExMediaDir dir, IntPtr name, int index)
		{
			MediaType = type;
			MediaDir = dir;
			Name = name;
			Index = index;
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
		public bool Equals(TxDeviceListItem other)
		{
			if (this.MediaType != other.MediaType) return false;
			if (this.MediaDir != other.MediaDir) return false;
			if (this.Name != other.Name) return false;
			if (this.Index != other.Index) return false;
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
			if (!(obj is TxDeviceListItem)) return false;
			return this.Equals((TxDeviceListItem)obj);
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
		public static bool operator ==(TxDeviceListItem left, TxDeviceListItem right)
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
		public static bool operator !=(TxDeviceListItem left, TxDeviceListItem right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}
