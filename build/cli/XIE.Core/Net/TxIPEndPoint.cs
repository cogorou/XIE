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
	/// IPエンドポイント構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxIPEndPoint :
		IEquatable<TxIPEndPoint>
	{
		#region プロパティ:

		/// <summary>
		/// IPアドレス [既定値:0,0,0,0]
		/// </summary>
		public TxIPAddress IPAddress;
		/// <summary>
		/// ポート番号 [既定値:0] [範囲:0~65535]
		/// </summary>
		public int Port;
		/// <summary>
		/// プロトコルファミリ [既定値:0] [範囲:2=System.Net.Sockets.ProtocolFamily]
		/// </summary>
		public int Family;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addr">IPアドレス [既定値:0,0,0,0]</param>
		/// <param name="port">ポート番号 [既定値:0] [範囲:0~65535]</param>
		public TxIPEndPoint(TxIPAddress addr, int port)
		{
			IPAddress = addr;
			Port = port;
			Family = (int)System.Net.Sockets.ProtocolFamily.InterNetwork;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addr">IPアドレス [既定値:0,0,0,0]</param>
		/// <param name="port">ポート番号 [既定値:0] [範囲:0~65535]</param>
		/// <param name="family">プロトコルファミリ [既定値:0] [範囲:2=System.Net.Sockets.ProtocolFamily]</param>
		public TxIPEndPoint(TxIPAddress addr, int port, int family)
		{
			IPAddress = addr;
			Port = port;
			Family = family;
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
		public bool Equals(TxIPEndPoint other)
		{
			if (this.IPAddress != other.IPAddress) return false;
			if (this.Port != other.Port) return false;
			if (this.Family != other.Family) return false;
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
			if (!(obj is TxIPEndPoint)) return false;
			return this.Equals((TxIPEndPoint)obj);
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
		public static bool operator ==(TxIPEndPoint left, TxIPEndPoint right)
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
		public static bool operator !=(TxIPEndPoint left, TxIPEndPoint right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (自身 ← IPEndPoint)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxIPEndPoint(System.Net.IPEndPoint src)
		{
			return new TxIPEndPoint(src.Address, src.Port);
		}

		/// <summary>
		/// 暗黙的な型変換 (IPEndPoint ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator System.Net.IPEndPoint(TxIPEndPoint src)
		{
			return new System.Net.IPEndPoint(src.IPAddress, src.Port);
		}

		#endregion
	}
}
