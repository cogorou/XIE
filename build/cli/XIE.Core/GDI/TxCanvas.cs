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

namespace XIE.GDI
{
	/// <summary>
	/// キャンバス構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxCanvas :
		IEquatable<TxCanvas>
	{
		#region プロパティ:

		/// <summary>
		/// 幅 [範囲:1~]
		/// </summary>
		public int Width;

		/// <summary>
		/// 高さ [範囲:1~]
		/// </summary>
		public int Height;

		/// <summary>
		/// 背景画像サイズ (pixels)
		/// </summary>
		public TxSizeI BgSize;

		/// <summary>
		/// 背景色
		/// </summary>
		public TxRGB8x4 BkColor;

		/// <summary>
		/// 背景の有効属性
		/// </summary>
		public ExBoolean BkEnable;

		/// <summary>
		/// 表示倍率 [範囲:0.0 より大きい値]
		/// </summary>
		public double Magnification;

		/// <summary>
		/// 視点 (pixels)
		/// </summary>
		public TxPointD ViewPoint;

		/// <summary>
		/// 表示対象チャネル指標 [範囲:0~3]
		/// </summary>
		public int ChannelNo;

		/// <summary>
		/// アンパック表示の指示
		/// </summary>
		public ExBoolean Unpack;

		/// <summary>
		/// ハーフトーン
		/// </summary>
		public ExBoolean Halftone;

		#endregion

		#region 比較:

		/// <summary>
		/// IEquatable の実装: 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="other">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public bool Equals(TxCanvas other)
		{
			if (this.Width != other.Width) return false;
			if (this.Height != other.Height) return false;
			if (this.BgSize != other.BgSize) return false;
			if (this.BkColor != other.BkColor) return false;
			if (this.BkEnable != other.BkEnable) return false;
			if (this.Magnification != other.Magnification) return false;
			if (this.ViewPoint != other.ViewPoint) return false;
			if (this.ChannelNo != other.ChannelNo) return false;
			if (this.Unpack != other.Unpack) return false;
			if (this.Halftone != other.Halftone) return false;
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
			if (!(obj is TxCanvas)) return false;
			return this.Equals((TxCanvas)obj);
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
		public static bool operator ==(TxCanvas left, TxCanvas right)
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
		public static bool operator !=(TxCanvas left, TxCanvas right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}
