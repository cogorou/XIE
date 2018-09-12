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
using XIE;

namespace XIEstudio
{
	/// <summary>
	/// ランレングス構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.S32, Pack = 3)]
	public struct TxRunLength :
		IEquatable<TxRunLength>
		, IComparable<TxRunLength>
	{
		#region フィールド:

		int m_X1;
		int m_X2;
		int m_Y;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 始点(X)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRunLength.X1")]
		public int X1
		{
			get { return m_X1; }
			set { m_X1 = value; }
		}

		/// <summary>
		/// 終点(X)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRunLength.X2")]
		public int X2
		{
			get { return m_X2; }
			set { m_X2 = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRunLength.Y")]
		public int Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="x1">始点(X)</param>
		/// <param name="x2">終点(X)</param>
		/// <param name="y">Y 座標</param>
		public TxRunLength(int x1, int x2, int y)
		{
			m_X1 = x1;
			m_X2 = x2;
			m_Y = y;
		}

		#endregion

		#region IEquatable の実装:

		/// <summary>
		/// IEquatable の実装: 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="other">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public bool Equals(TxRunLength other)
		{
			if (this.X1 != other.X1) return false;
			if (this.X2 != other.X2) return false;
			if (this.Y != other.Y) return false;
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
			if (!(obj is TxRunLength)) return false;
			return this.Equals((TxRunLength)obj);
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

		#region IComparable の実装:

		/// <summary>
		/// IComparable の実装: 自身の内容と指定されたオブジェクトの内容を比較し大小関係を返します。
		/// </summary>
		/// <param name="other">比較対象</param>
		/// <returns>
		///		このインスタンスが other と同一の場合は 0 、
		///		other より小さい場合は 0 未満、
		///		other より大きい場合は 0 より大きい値を返します。
		/// </returns>
		public int CompareTo(TxRunLength other)
		{
			var ans = Y.CompareTo(other.Y);
			if (ans == 0)
			{
				ans = X1.CompareTo(other.X1);
				if (ans == 0)
					ans = X2.CompareTo(other.X2);
			}
			return ans;
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
		public static bool operator ==(TxRunLength left, TxRunLength right)
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
		public static bool operator !=(TxRunLength left, TxRunLength right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 比較オペレータ: (不等号)

		/// <summary>
		/// ２つのオブジェクトの大小関係を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		左辺値 が 右辺値 より大きいの場合は true、それ以外は false を返します。
		/// </returns>
		public static bool operator >(TxRunLength left, TxRunLength right)
		{
			return left.CompareTo(right) > 0;
		}

		/// <summary>
		/// ２つのオブジェクトの大小関係を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		左辺値 が 右辺値 未満の場合は true、それ以外は false を返します。
		/// </returns>
		public static bool operator <(TxRunLength left, TxRunLength right)
		{
			return left.CompareTo(right) < 0;
		}

		/// <summary>
		/// ２つのオブジェクトの大小関係を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		左辺値 が 右辺値 以上の場合は true、それ以外は false を返します。
		/// </returns>
		public static bool operator >=(TxRunLength left, TxRunLength right)
		{
			return left.CompareTo(right) >= 0;
		}

		/// <summary>
		/// ２つのオブジェクトの大小関係を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		左辺値 が 右辺値 以下の場合は true、それ以外は false を返します。
		/// </returns>
		public static bool operator <=(TxRunLength left, TxRunLength right)
		{
			return left.CompareTo(right) <= 0;
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
			return string.Format("{0},{1},{2}",
				this.X1,
				this.X2,
				this.Y
				);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxRunLength)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxRunLength)
				{
					var _value = (TxRunLength)value;
					return string.Format("{0},{1},{2}",
						_value.X1,
						_value.X2,
						_value.Y
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
					var dst = new TxRunLength();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.X1 = Convert.ToInt32(values[i]); break;
							case 1: dst.X2 = Convert.ToInt32(values[i]); break;
							case 2: dst.Y = Convert.ToInt32(values[i]); break;
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
