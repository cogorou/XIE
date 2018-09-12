/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace XIE
{
	/// <summary>
	/// SIZE_T 構造体
	/// </summary>
	[TypeConverter(typeof(SelfConverter))]
	public struct SIZE_T :
		IEquatable<SIZE_T>
	{
		/// <summary>
		/// 値
		/// </summary>
		public System.IntPtr Value;

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="value">値</param>
		public SIZE_T(IntPtr value)
		{
			Value = value;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="value">値 (32bit 整数(符号付き))</param>
		public SIZE_T(Int32 value)
		{
			Value = new IntPtr(value);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="value">値 (32bit 整数)</param>
		public SIZE_T(UInt32 value)
		{
			Value = new IntPtr(value);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="value">値 (64bit 整数(符号付き))</param>
		public SIZE_T(Int64 value)
		{
			Value = new IntPtr(value);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="value">値 (64bit 整数)</param>
		public SIZE_T(UInt64 value)
		{
			Value = new IntPtr((Int64)value);
		}

		#endregion

		#region 比較.

		/// <summary>
		/// IEquatable の実装: 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="other">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public bool Equals(SIZE_T other)
		{
			if (this.Value != other.Value) return false;
			return true;
		}

		/// <summary>
		/// 比較メソッド (等価)
		/// </summary>
		/// <param name="obj">比較対象</param>
		/// <returns>
		///		等価の場合 true、不等価の場合 false を返します。
		///	</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			if (!(obj is SIZE_T)) return false;
			return this.Equals((SIZE_T)obj);
		}

		/// <summary>
		/// ハッシュコードの取得
		/// </summary>
		/// <returns>
		///		常に 0 を返します。
		///	</returns>
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
		public static bool operator ==(SIZE_T left, SIZE_T right)
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
		public static bool operator !=(SIZE_T left, SIZE_T right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 型変換メソッド:

		/// <summary>
		/// 型変換メソッド (Int32←SIZE_T)
		/// </summary>
		/// <returns>
		///		Int32 に変換して返します。
		///	</returns>
		public Int32 ToInt32()
		{
			return Value.ToInt32();
		}

		/// <summary>
		/// 型変換メソッド (UInt32←SIZE_T)
		/// </summary>
		/// <returns>
		///		UInt32 に変換して返します。
		///	</returns>
		public UInt32 ToUInt32()
		{
			return (UInt32)Value.ToInt32();
		}

		/// <summary>
		/// 型変換メソッド (Int64←SIZE_T)
		/// </summary>
		/// <returns>
		///		Int64 に変換して返します。
		///	</returns>
		public Int64 ToInt64()
		{
			return Value.ToInt64();
		}

		/// <summary>
		/// 型変換メソッド (UInt64←SIZE_T)
		/// </summary>
		/// <returns>
		///		UInt64 に変換して返します。
		///	</returns>
		public UInt64 ToUInt64()
		{
			return (UInt64)Value.ToInt64();
		}

		#endregion

		#region 型変換演算子: (Any←SIZE_T)

		/// <summary>
		/// 型変換演算子 (IntPtr←SIZE_T) (暗黙的)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		IntPtr に変換して返します。
		///	</returns>
		public static implicit operator IntPtr(SIZE_T src)
		{
			return src.Value;
		}

		/// <summary>
		/// 型変換演算子 (Int32←SIZE_T) (暗黙的)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		Int32 に変換して返します。
		///	</returns>
		public static implicit operator Int32(SIZE_T src)
		{
			return src.Value.ToInt32();
		}

		/// <summary>
		/// 型変換演算子 (UInt32←SIZE_T) (暗黙的)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		UInt32 に変換して返します。
		///	</returns>
		public static implicit operator UInt32(SIZE_T src)
		{
			return (UInt32)src.Value.ToInt32();
		}

		/// <summary>
		/// 型変換演算子 (Int64←SIZE_T) (暗黙的)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		Int64 に変換して返します。
		///	</returns>
		public static implicit operator Int64(SIZE_T src)
		{
			return src.Value.ToInt64();
		}

		/// <summary>
		/// 型変換演算子 (UInt64←SIZE_T) (暗黙的)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		UInt64 に変換して返します。
		///	</returns>
		public static implicit operator UInt64(SIZE_T src)
		{
			return (UInt64)src.Value.ToInt64();
		}

		#endregion

		#region 型変換演算子: (SIZE_T←Any)

		/// <summary>
		/// 型変換演算子 (SIZE_T←IntPtr) (暗黙的)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		SIZE_T 構造体に代入して返します。
		///	</returns>
		public static implicit operator SIZE_T(IntPtr src)
		{
			return new SIZE_T(src);
		}

		/// <summary>
		/// 型変換演算子 (SIZE_T←Int32) (暗黙的)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		SIZE_T 構造体に代入して返します。
		///	</returns>
		public static implicit operator SIZE_T(Int32 src)
		{
			return new SIZE_T(src);
		}

		/// <summary>
		/// 型変換演算子 (SIZE_T←UInt32) (暗黙的)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		SIZE_T 構造体に代入して返します。
		///	</returns>
		public static implicit operator SIZE_T(UInt32 src)
		{
			return new SIZE_T(src);
		}

		/// <summary>
		/// 型変換演算子 (SIZE_T←Int64) (暗黙的)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		SIZE_T 構造体に代入して返します。
		///	</returns>
		public static implicit operator SIZE_T(Int64 src)
		{
			return new SIZE_T(src);
		}

		/// <summary>
		/// 型変換演算子 (SIZE_T←UInt64) (暗黙的)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		SIZE_T 構造体に代入して返します。
		///	</returns>
		public static implicit operator SIZE_T(UInt64 src)
		{
			return new SIZE_T(src);
		}

		#endregion

		#region SelfConverter

		/// <summary>
		/// 型変換クラス
		/// </summary>
		internal class SelfConverter : CxSortingConverter
		{
			/// <summary>
			/// コンバータがオブジェクトを指定した型に変換できるか否かを示します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="destinationType"></param>
			/// <returns>
			///		変換可能な場合は true を返します。
			/// </returns>
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(SIZE_T))
					return true;
				return base.CanConvertTo(context, destinationType);
			}

			/// <summary>
			/// 指定されたオブジェクトを指定した型に変換します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture"></param>
			/// <param name="value"></param>
			/// <param name="destinationType"></param>
			/// <returns>
			///		オブジェクトの内容を文字列に変換して返します。
			/// </returns>
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is SIZE_T)
				{
					return string.Format("{0}", ((SIZE_T)value).ToInt64());
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			/// <summary>
			/// コンバータが指定した型のオブジェクトから自身の型に変換できるか否かを示します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="sourceType"></param>
			/// <returns>
			///		変換可能な場合は true を返します。
			/// </returns>
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				if (sourceType == typeof(string))
					return true;
				return base.CanConvertFrom(context, sourceType);
			}

			/// <summary>
			/// 指定された型のオブジェクトから自身の型への変換
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture"></param>
			/// <param name="value"></param>
			/// <returns>
			///		変換後のオブジェクトを返します。
			/// </returns>
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value is string)
				{
					return new SIZE_T(long.Parse((string)value));
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}
}
