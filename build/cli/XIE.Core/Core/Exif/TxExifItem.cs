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
	/// Exif 項目構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxExifItem :
		IEquatable<TxExifItem>
	{
		#region フィールド:

		int m_Offset;
		ExEndianType m_EndianType;
		ushort m_ID;
		short m_Type;
		int m_Count;
		int m_ValueOrIndex;

		#endregion

		#region プロパティ:

		/// <summary>
		/// オフセット (この項目が格納されていた位置の先頭を示します。この位置から ID,Type,Count,ValueOrIndex が Exif に書き込まれていることを示します。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxExifItem.Offset")]
		public int Offset
		{
			get { return m_Offset; }
			set { m_Offset = value; }
		}

		/// <summary>
		/// エンディアンタイプ (この項目が格納されていた Exif のエンディアンタイプを示します。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxExifItem.EndianType")]
		public ExEndianType EndianType
		{
			get { return m_EndianType; }
			set { m_EndianType = value; }
		}

		/// <summary>
		/// 識別子 (Exif 仕様では Tag ID と呼称されています。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxExifItem.ID")]
		public ushort ID
		{
			get { return m_ID; }
			set { m_ID = value; }
		}

		/// <summary>
		/// 型 [範囲: 1=BYTE,2=ASCII,3=SHORT,4=LONG,5=RATIONAL,7=UNDEFINED,9=SLONG,10=SRATIONAL]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxExifItem.Type")]
		public short Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}

		/// <summary>
		/// 個数
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxExifItem.Count")]
		public int Count
		{
			get { return m_Count; }
			set { m_Count = value; }
		}

		/// <summary>
		/// 値または指標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxExifItem.ValueOrIndex")]
		public int ValueOrIndex
		{
			get { return m_ValueOrIndex; }
			set { m_ValueOrIndex = value; }
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="offset">オフセット</param>
		/// <param name="endian_type">エンディアンタイプ</param>
		/// <param name="id">識別子</param>
		/// <param name="type">型</param>
		/// <param name="count">個数</param>
		/// <param name="value">値または指標</param>
		public TxExifItem(int offset, ExEndianType endian_type, ushort id, short type, int count, int value)
		{
			m_Offset = offset;
			m_EndianType = endian_type;
			m_ID = id;
			m_Type = type;
			m_Count = count;
			m_ValueOrIndex = value;
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
		public bool Equals(TxExifItem other)
		{
			if (this.m_Offset != other.m_Offset) return false;
			if (this.m_EndianType != other.m_EndianType) return false;
			if (this.m_ID != other.m_ID) return false;
			if (this.m_Type != other.m_Type) return false;
			if (this.m_Count != other.m_Count) return false;
			if (this.m_ValueOrIndex != other.m_ValueOrIndex) return false;
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
			if (!(obj is TxExifItem)) return false;
			return this.Equals((TxExifItem)obj);
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
		public static bool operator ==(TxExifItem left, TxExifItem right)
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
		public static bool operator !=(TxExifItem left, TxExifItem right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}
