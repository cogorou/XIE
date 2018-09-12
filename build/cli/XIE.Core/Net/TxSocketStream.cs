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
	/// ソケットストリーム構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxSocketStream :
		IEquatable<TxSocketStream>
	{
		#region プロパティ:

		/// <summary>
		/// ソケットハンドル
		/// </summary>
		public SOCKET Socket;

		/// <summary>
		/// ローカルエンドポイント
		/// </summary>
		public TxIPEndPoint LocalEndPoint;

		/// <summary>
		/// リモートエンドポイント
		/// </summary>
		public TxIPEndPoint RemoteEndPoint;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="sock">ソケットハンドル</param>
		public TxSocketStream(SOCKET sock)
		{
			Socket = sock;
			LocalEndPoint = new TxIPEndPoint();
			RemoteEndPoint = new TxIPEndPoint();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="sock">ソケットハンドル</param>
		/// <param name="localeEP">ローカルエンドポイント</param>
		/// <param name="remoteEP">リモートエンドポイント</param>
		public TxSocketStream(SOCKET sock, TxIPEndPoint localeEP, TxIPEndPoint remoteEP)
		{
			Socket = sock;
			LocalEndPoint = localeEP;
			RemoteEndPoint = remoteEP;
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
		public bool Equals(TxSocketStream other)
		{
			if (this.Socket != other.Socket) return false;
			if (this.LocalEndPoint != other.LocalEndPoint) return false;
			if (this.RemoteEndPoint != other.RemoteEndPoint) return false;
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
			if (!(obj is TxSocketStream)) return false;
			return this.Equals((TxSocketStream)obj);
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
		public static bool operator ==(TxSocketStream left, TxSocketStream right)
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
		public static bool operator !=(TxSocketStream left, TxSocketStream right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 読み込み可能か否かを検査します。
		/// </summary>
		/// <param name="timeout">タイムアウト (msec) [-1,0,1~]</param>
		/// <returns>
		///		読み込み可能なデータが到達している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public bool Readable(int timeout)
		{
			ExBoolean result = ExBoolean.False;
			ExStatus status = xie_high.fnXIE_Net_SocketStream_Readable(this, timeout, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return (result == ExBoolean.True);
		}

		/// <summary>
		/// データを読み込みます。
		/// </summary>
		/// <param name="buffer">読み込むデータを格納するバッファ</param>
		/// <param name="length">読み込むバイト数の上限値</param>
		/// <param name="timeout">タイムアウト (msec) [-1,0,1~]</param>
		/// <returns>
		///		実際に読み込まれたバイト数を返します。<br/>
		///		データが無ければ 0 を返します。<br/>
		///		異常があれば例外を発行します。
		/// </returns>
		public int Read(byte[] buffer, int length, int timeout)
		{
			int result = 0;
			ExStatus status = xie_high.fnXIE_Net_SocketStream_Read(this, buffer, length, timeout, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		/// <summary>
		/// 書き込み可能か否かを検査します。
		/// </summary>
		/// <param name="timeout">タイムアウト (msec) [-1,0,1~]</param>
		/// <returns>
		/// </returns>
		public bool Writeable(int timeout)
		{
			ExBoolean result = ExBoolean.False;
			ExStatus status = xie_high.fnXIE_Net_SocketStream_Writeable(this, timeout, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return (result == ExBoolean.True);
		}

		/// <summary>
		/// データを書き込みます。
		/// </summary>
		/// <param name="buffer">書き込むデータが格納されたバッファ</param>
		/// <param name="length">書き込むバイト数</param>
		/// <param name="timeout">タイムアウト (msec) [-1,0,1~]</param>
		/// <returns>
		///		実際に書き込まれたバイト数を返します。<br/>
		///		書き込めなければ 0 を返します。<br/>
		///		異常があれば例外を発行します。
		/// </returns>
		public int Write(byte[] buffer, int length, int timeout)
		{
			int result = 0;
			ExStatus status = xie_high.fnXIE_Net_SocketStream_Write(this, buffer, length, timeout, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		#endregion
	}
}
