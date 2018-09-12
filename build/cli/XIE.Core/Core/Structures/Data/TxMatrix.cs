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

namespace XIE
{
	/// <summary>
	/// 行列オブジェクト構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxMatrix :
		IEquatable<TxMatrix>
	{
		#region プロパティ:

		/// <summary>
		/// 領域の先頭アドレス
		/// </summary>
		public IntPtr Address;

		/// <summary>
		/// 行数 [範囲:1~]
		/// </summary>
		public int Rows;

		/// <summary>
		/// 列数 [範囲:1~]
		/// </summary>
		public int Columns;

		/// <summary>
		/// 要素の型 [範囲:Atom6F]
		/// </summary>
		public TxModel Model;

		/// <summary>
		/// 領域の水平方向サイズ (bytes)
		/// </summary>
		public int Stride;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addr">先頭アドレス</param>
		/// <param name="rows">行数 [範囲:1~]</param>
		/// <param name="cols">列数 [範囲:1~]</param>
		/// <param name="model">要素の型 [既定値:F64]</param>
		/// <param name="stride">領域の水平方向サイズ (bytes)</param>
		public TxMatrix(IntPtr addr, int rows, int cols, TxModel model, int stride)
		{
			Address = addr;
			Rows = rows;
			Columns = cols;
			Model = model;
			Stride = stride;
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
		public bool Equals(TxMatrix other)
		{
			if (this.Address != other.Address) return false;
			if (this.Rows != other.Rows) return false;
			if (this.Columns != other.Columns) return false;
			if (this.Model != other.Model) return false;
			if (this.Stride != other.Stride) return false;
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
			if (!(obj is TxMatrix)) return false;
			return this.Equals((TxMatrix)obj);
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
		public static bool operator ==(TxMatrix left, TxMatrix right)
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
		public static bool operator !=(TxMatrix left, TxMatrix right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換:

		/// <summary>
		/// 暗黙的な型変換 (TxMatrix ← 自身)
		/// </summary>
		/// <param name="src"></param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxMatrix(TxScanner2D src)
		{
			return new TxMatrix(src.Address, src.Height, src.Width, src.Model, src.Stride);
		}

		#endregion
	}
}
