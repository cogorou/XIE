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

namespace XIE.IO
{
	/// <summary>
	/// シリアル通信構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxSerialPort :
		IEquatable<TxSerialPort>
	{
		#region プロパティ:

		/// <summary>
		/// ボーレート
		/// </summary>
		public int BaudRate;
		/// <summary>
		/// パリティチェック
		/// </summary>
		public ExParity Parity;
		/// <summary>
		/// データビット [5~8]
		/// </summary>
		public int DataBits;
		/// <summary>
		/// ストップビット
		/// </summary>
		public ExStopBits StopBits;
		/// <summary>
		/// ハンドシェイク
		/// </summary>
		public ExHandshake Handshake;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="buadrate">ボーレート</param>
		/// <param name="parity">パリティチェック</param>
		/// <param name="databits">データビット [5~8]</param>
		/// <param name="stopbits">ストップビット</param>
		/// <param name="handshake">ハンドシェイク</param>
		public TxSerialPort(int buadrate, ExParity parity, int databits, ExStopBits stopbits, ExHandshake handshake)
		{
			BaudRate = buadrate;
			Parity = parity;
			DataBits = databits;
			StopBits = stopbits;
			Handshake = handshake;
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
		public bool Equals(TxSerialPort other)
		{
			if (this.BaudRate != other.BaudRate) return false;
			if (this.Parity != other.Parity) return false;
			if (this.DataBits != other.DataBits) return false;
			if (this.StopBits != other.StopBits) return false;
			if (this.Handshake != other.Handshake) return false;
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
			if (!(obj is TxSerialPort)) return false;
			return this.Equals((TxSerialPort)obj);
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
		public static bool operator ==(TxSerialPort left, TxSerialPort right)
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
		public static bool operator !=(TxSerialPort left, TxSerialPort right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}
