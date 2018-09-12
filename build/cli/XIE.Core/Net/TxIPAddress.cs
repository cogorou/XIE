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

namespace XIE.Net
{
	/// <summary>
	/// IPアドレス構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxIPAddress :
		IEquatable<TxIPAddress>
	{
		#region プロパティ:

		/// <summary>
		/// セグメント1
		/// </summary>
		public byte	S1;
		/// <summary>
		/// セグメント2
		/// </summary>
		public byte S2;
		/// <summary>
		/// セグメント3
		/// </summary>
		public byte S3;
		/// <summary>
		/// セグメント4
		/// </summary>
		public byte S4;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="s1">セグメント1</param>
		/// <param name="s2">セグメント2</param>
		/// <param name="s3">セグメント3</param>
		/// <param name="s4">セグメント4</param>
		public TxIPAddress(byte s1, byte s2, byte s3, byte s4)
		{
			S1 = s1;
			S2 = s2;
			S3 = s3;
			S4 = s4;
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
		public bool Equals(TxIPAddress other)
		{
			if (this.S1 != other.S1) return false;
			if (this.S2 != other.S2) return false;
			if (this.S3 != other.S3) return false;
			if (this.S4 != other.S4) return false;
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
			if (!(obj is TxIPAddress)) return false;
			return this.Equals((TxIPAddress)obj);
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
		public static bool operator ==(TxIPAddress left, TxIPAddress right)
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
		public static bool operator !=(TxIPAddress left, TxIPAddress right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (自身 ← IPAddress)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxIPAddress(System.Net.IPAddress src)
		{
			byte[] addr = src.GetAddressBytes();
			return new TxIPAddress(addr[0], addr[1], addr[2], addr[3]);
		}

		/// <summary>
		/// 暗黙的な型変換 (IPAddress ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator System.Net.IPAddress(TxIPAddress src)
		{
			return new System.Net.IPAddress(new byte[] { src.S1, src.S2, src.S3, src.S4 });
		}

		#endregion
	}
}
