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
	/// レンジ構造体 (実数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.F64, Pack = 2)]
	public struct TxRangeD :
		IEquatable<TxRangeD>
	{
		#region フィールド:

		double m_Lower;
		double m_Upper;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 下限
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRangeD.Lower")]
		public double Lower
		{
			get { return m_Lower; }
			set { m_Lower = value; }
		}

		/// <summary>
		/// 上限
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRangeD.Upper")]
		public double Upper
		{
			get { return m_Upper; }
			set { m_Upper = value; }
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="lower">下限</param>
		/// <param name="upper">上限</param>
		public TxRangeD(double lower, double upper)
		{
			m_Lower = lower;
			m_Upper = upper;
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
		public bool Equals(TxRangeD other)
		{
			if (this.Lower != other.Lower) return false;
			if (this.Upper != other.Upper) return false;
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
			if (!(obj is TxRangeD)) return false;
			return this.Equals((TxRangeD)obj);
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
		public static bool operator ==(TxRangeD left, TxRangeD right)
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
		public static bool operator !=(TxRangeD left, TxRangeD right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region オペレータ.(加算)

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxRangeD operator +(TxRangeD ope1, TxRangeD ope2)
		{
			TxRangeD dst = new TxRangeD();
			dst.Lower = (double)(ope1.Lower + ope2.Lower);
			dst.Upper = (double)(ope1.Upper + ope2.Upper);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxRangeD operator +(TxRangeD ope1, double ope2)
		{
			TxRangeD dst = new TxRangeD();
			dst.Lower = (double)(ope1.Lower + ope2);
			dst.Upper = (double)(ope1.Upper + ope2);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxRangeD operator +(double ope1, TxRangeD ope2)
		{
			TxRangeD dst = new TxRangeD();
			dst.Lower = (double)(ope1 + ope2.Lower);
			dst.Upper = (double)(ope1 + ope2.Upper);
			return dst;
		}

		#endregion

		#region オペレータ.(減算)

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxRangeD operator -(TxRangeD ope1, TxRangeD ope2)
		{
			TxRangeD dst = new TxRangeD();
			dst.Lower = (double)(ope1.Lower - ope2.Lower);
			dst.Upper = (double)(ope1.Upper - ope2.Upper);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxRangeD operator -(TxRangeD ope1, double ope2)
		{
			TxRangeD dst = new TxRangeD();
			dst.Lower = (double)(ope1.Lower - ope2);
			dst.Upper = (double)(ope1.Upper - ope2);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxRangeD operator -(double ope1, TxRangeD ope2)
		{
			TxRangeD dst = new TxRangeD();
			dst.Lower = (double)(ope1 - ope2.Lower);
			dst.Upper = (double)(ope1 - ope2.Upper);
			return dst;
		}

		#endregion

		#region オペレータ.(乗算)

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxRangeD operator *(TxRangeD ope1, TxRangeD ope2)
		{
			TxRangeD dst = new TxRangeD();
			dst.Lower = (double)(ope1.Lower * ope2.Lower);
			dst.Upper = (double)(ope1.Upper * ope2.Upper);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxRangeD operator *(TxRangeD ope1, double ope2)
		{
			TxRangeD dst = new TxRangeD();
			dst.Lower = (double)(ope1.Lower * ope2);
			dst.Upper = (double)(ope1.Upper * ope2);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxRangeD operator *(double ope1, TxRangeD ope2)
		{
			TxRangeD dst = new TxRangeD();
			dst.Lower = (double)(ope1 * ope2.Lower);
			dst.Upper = (double)(ope1 * ope2.Upper);
			return dst;
		}

		#endregion

		#region オペレータ.(除算)

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxRangeD operator /(TxRangeD ope1, TxRangeD ope2)
		{
			TxRangeD dst = new TxRangeD();
			dst.Lower = (double)(ope1.Lower / ope2.Lower);
			dst.Upper = (double)(ope1.Upper / ope2.Upper);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxRangeD operator /(TxRangeD ope1, double ope2)
		{
			TxRangeD dst = new TxRangeD();
			dst.Lower = (double)(ope1.Lower / ope2);
			dst.Upper = (double)(ope1.Upper / ope2);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxRangeD operator /(double ope1, TxRangeD ope2)
		{
			TxRangeD dst = new TxRangeD();
			dst.Lower = (double)(ope1 / ope2.Lower);
			dst.Upper = (double)(ope1 / ope2.Upper);
			return dst;
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxRangeI ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxRangeI(TxRangeD src)
		{
			return new TxRangeI(
				(int)System.Math.Round(src.Lower),
				(int)System.Math.Round(src.Upper)
				);
		}

		#endregion

		#region 変換系: (文字列変換)

		/// <summary>
		/// 文字列変換
		/// </summary>
		/// <returns>
		///		現在の値を文字列に変換して返します。
		/// </returns>
		public override string ToString()
		{
			return string.Format("{0},{1}",
				this.Lower,
				this.Upper
				);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxRangeD)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxRangeD)
				{
					var _value = (TxRangeD)value;
					return string.Format("{0},{1}",
						_value.Lower,
						_value.Upper
						);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				if (sourceType == typeof(string)) return true;
				return base.CanConvertFrom(context, sourceType);
			}

			public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
			{
				if (value is string)
				{
					var values = value.ToString().Split(new char[] { ',' });
					var dst = new TxRangeD();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.Lower = Convert.ToDouble(values[i]); break;
							case 1: dst.Upper = Convert.ToDouble(values[i]); break;
						}
					}
					return dst;
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}
}
