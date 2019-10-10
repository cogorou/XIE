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
	/// グラバー引数構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxGrabberArgs :
		IEquatable<TxGrabberArgs>
	{
		#region プロパティ:

		/// <summary>
		/// タイムスタンプ
		/// </summary>
		public ulong TimeStamp;

		/// <summary>
		/// フレームサイズ
		/// </summary>
		public TxImageSize FrameSize;

		/// <summary>
		/// 処理経過
		/// </summary>
		public double Progress;

		/// <summary>
		/// データ領域の先頭アドレス
		/// </summary>
		public IntPtr Address;

		/// <summary>
		/// データ長 (bytes)
		/// </summary>
		public int Length;

		/// <summary>
		/// フレーム指標 [-1,0~]
		/// </summary>
		public int Index;

		/// <summary>
		/// 処理中断の応答
		/// </summary>
		public ExBoolean Cancellation;

		#endregion

		#region 比較:

		/// <summary>
		/// IEquatable の実装: 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="other">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public bool Equals(TxGrabberArgs other)
		{
			if (this.TimeStamp != other.TimeStamp) return false;
			if (this.FrameSize != other.FrameSize) return false;
			if (this.Progress != other.Progress) return false;
			if (this.Address != other.Address) return false;
			if (this.Length != other.Length) return false;
			if (this.Index != other.Index) return false;
			if (this.Cancellation != other.Cancellation) return false;
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
			if (!(obj is TxGrabberArgs)) return false;
			return this.Equals((TxGrabberArgs)obj);
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
		public static bool operator ==(TxGrabberArgs left, TxGrabberArgs right)
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
		public static bool operator !=(TxGrabberArgs left, TxGrabberArgs right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}
