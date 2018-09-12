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
#if LINUX
	using SOCKET = System.Int32;
#else
	using SOCKET = System.IntPtr;
#endif

	/// <summary>
	/// TCP/IP通信サーバー構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxTcpServer :
		IEquatable<TxTcpServer>
	{
		#region プロパティ:

		/// <summary>
		/// ソケットハンドル
		/// </summary>
		public SOCKET Socket;

		/// <summary>
		/// 接続しているクライアントのリスト
		/// </summary>
		public IntPtr Clients;

		/// <summary>
		/// 接続しているクライアントの数
		/// </summary>
		public int Connections;

		/// <summary>
		/// IPアドレス
		/// </summary>
		public TxIPAddress IPAddress;

		/// <summary>
		/// ポート番号
		/// </summary>
		public int Port;

		/// <summary>
		/// 最大接続数
		/// </summary>
		public int Backlog;

		#endregion

		#region 比較:

		/// <summary>
		/// IEquatable の実装: 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="other">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public bool Equals(TxTcpServer other)
		{
			if (this.Socket != other.Socket) return false;
			if (this.Clients != other.Clients) return false;
			if (this.Connections != other.Connections) return false;
			if (this.IPAddress != other.IPAddress) return false;
			if (this.Port != other.Port) return false;
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
			if (!(obj is TxTcpServer)) return false;
			return this.Equals((TxTcpServer)obj);
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
		public static bool operator ==(TxTcpServer left, TxTcpServer right)
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
		public static bool operator !=(TxTcpServer left, TxTcpServer right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}
