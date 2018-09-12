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
	/// スクリーンリスト項目構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxScreenListItem :
		IEquatable<TxScreenListItem>
	{
		#region プロパティ:

		/// <summary>
		/// ウィンドウハンドル
		/// </summary>
		public IntPtr Handle;

		/// <summary>
		/// ウィンドウ名称
		/// </summary>
		public IntPtr Name;

		/// <summary>
		/// ウィンドウの X 座標
		/// </summary>
		public int X;

		/// <summary>
		/// ウィンドウの Y 座標
		/// </summary>
		public int Y;

		/// <summary>
		/// ウィンドウの幅
		/// </summary>
		public int Width;

		/// <summary>
		/// ウィンドウの高さ
		/// </summary>
		public int Height;

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="handle">ウィンドウハンドル</param>
		/// <param name="name">ウィンドウ名称</param>
		/// <param name="x">ウィンドウの位置 (X 座標)</param>
		/// <param name="y">ウィンドウの位置 (Y 座標)</param>
		/// <param name="width">ウィンドウの幅</param>
		/// <param name="height">ウィンドウの高さ</param>
		public TxScreenListItem(IntPtr handle, IntPtr name, int x, int y, int width, int height)
		{
			Handle = handle;
			Name = name;
			X = x;
			Y = y;
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
		public bool Equals(TxScreenListItem other)
		{
			if (this.Handle != other.Handle) return false;
			if (this.Name != other.Name) return false;
			if (this.X != other.X) return false;
			if (this.Y != other.Y) return false;
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
			if (!(obj is TxScreenListItem)) return false;
			return this.Equals((TxScreenListItem)obj);
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
		public static bool operator ==(TxScreenListItem left, TxScreenListItem right)
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
		public static bool operator !=(TxScreenListItem left, TxScreenListItem right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}
