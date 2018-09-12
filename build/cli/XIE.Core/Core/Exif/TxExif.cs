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
	/// Exif オブジェクト構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxExif :
		IEquatable<TxExif>
	{
		#region プロパティ:

		/// <summary>
		/// 領域の先頭アドレス
		/// </summary>
		public IntPtr Address;

		/// <summary>
		/// 要素数
		/// </summary>
		public int Length;

		/// <summary>
		/// 要素の型
		/// </summary>
		public TxModel Model;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addr">配列の先頭アドレス</param>
		/// <param name="length">要素数 [範囲:0~]</param>
		/// <param name="model">要素モデル</param>
		public TxExif(IntPtr addr, int length, TxModel model)
		{
			Address = addr;
			Length = length;
			Model = model;
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
		public bool Equals(TxExif other)
		{
			if (this.Address != other.Address) return false;
			if (this.Length != other.Length) return false;
			if (this.Model != other.Model) return false;
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
			if (!(obj is TxExif)) return false;
			return this.Equals((TxExif)obj);
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
		public static bool operator ==(TxExif left, TxExif right)
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
		public static bool operator !=(TxExif left, TxExif right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 明示的な型変換:

		/// <summary>
		/// 明示的な型変換 (TxExif ← 自身)
		/// </summary>
		/// <param name="src"></param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxExif(TxScanner1D src)
		{
			return new TxExif(src.Address, src.Length, src.Model);
		}

		#endregion

		#region 有効性検査:

		/// <summary>
		/// 有効性検査
		/// </summary>
		/// <returns>
		///		現在保有する領域が有効な場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		[XmlIgnore]
		[ReadOnly(true)]
		public bool IsValid
		{
			get
			{
				if (CheckValidity(Address, Length) == false) return false;
				if (Model != TxModel.U8(1)) return false;
				return true;
			}
		}

		/// <summary>
		/// 有効性検査
		/// </summary>
		/// <param name="addr">領域の先頭アドレス</param>
		/// <param name="length">領域の長さ (bytes)</param>
		/// <returns>
		///		指定された領域が有効な場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public static bool CheckValidity(IntPtr addr, int length)
		{
			if (addr == IntPtr.Zero) return false;
			if (length < 16) return false;
			var buf = (XIE.Ptr.BytePtr)addr;

			#region 0~5: 'Exif\0\0'
			if (buf[0] != 0x45 ||
				buf[1] != 0x78 ||
				buf[2] != 0x69 ||
				buf[3] != 0x66 ||
				buf[4] != 0x00 ||
				buf[5] != 0x00)
			{
				return false;
			}
			#endregion

			return true;
		}

		#endregion
	}
}
