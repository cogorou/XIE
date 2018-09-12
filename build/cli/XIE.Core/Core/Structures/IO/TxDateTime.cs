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
	/// 日時構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.S32, Pack = 7)]
	public struct TxDateTime :
		IEquatable<TxDateTime>
	{
		#region フィールド:

		int m_Year;
		int m_Month;
		int m_Day;
		int m_Hour;
		int m_Minute;
		int m_Second;
		int m_Millisecond;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 年 [0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxDateTime.Year")]
		public int Year
		{
			get { return m_Year; }
			set { m_Year = value; }
		}

		/// <summary>
		/// 月 [1~12]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxDateTime.Month")]
		public int Month
		{
			get { return m_Month; }
			set { m_Month = value; }
		}

		/// <summary>
		/// 日 [1~31]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxDateTime.Day")]
		public int Day
		{
			get { return m_Day; }
			set { m_Day = value; }
		}

		/// <summary>
		/// 時 [0~23]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxDateTime.Hour")]
		public int Hour
		{
			get { return m_Hour; }
			set { m_Hour = value; }
		}

		/// <summary>
		/// 分 [0~59]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxDateTime.Minute")]
		public int Minute
		{
			get { return m_Minute; }
			set { m_Minute = value; }
		}

		/// <summary>
		/// 秒 [0~59, 60]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxDateTime.Second")]
		public int Second
		{
			get { return m_Second; }
			set { m_Second = value; }
		}

		/// <summary>
		/// ミリ秒 [0~999]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxDateTime.Millisecond")]
		public int Millisecond
		{
			get { return m_Millisecond; }
			set { m_Millisecond = value; }
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="year">年 [0~]</param>
		/// <param name="month">月 [1~12]</param>
		/// <param name="day">日 [1~31]</param>
		/// <param name="hour">時 [0~23]</param>
		/// <param name="minute">分 [0~59]</param>
		/// <param name="second">秒 [0~59, 60]</param>
		/// <param name="millisecond">ミリ秒 [0~999]</param>
		public TxDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
		{
			m_Year = year;
			m_Month = month;
			m_Day = day;
			m_Hour = hour;
			m_Minute = minute;
			m_Second = second;
			m_Millisecond = millisecond;
		}

		#endregion

		#region スタティックメソッド:

		/// <summary>
		/// 現在時刻
		/// </summary>
		/// <param name="ltc">true=ローカル時刻、false=協定世界時</param>
		/// <returns>
		///		現在時刻で初期化したオブジェクトを返します。
		/// </returns>
		public static TxDateTime Now(bool ltc)
		{
			TxDateTime result = new TxDateTime();
			ExStatus status = xie_core.fnXIE_Core_DateTime_Now((ltc ? ExBoolean.True : ExBoolean.False), out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		/// <summary>
		/// バイナリ日時から日時構造体への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <param name="ltc">変換後のタイムゾーン [true=ローカル時刻、false=協定世界時]</param>
		/// <returns>
		///		日時構造体に変換して返します。
		/// </returns>
		public static TxDateTime FromBinary(ulong src, bool ltc)
		{
			TxDateTime result = new TxDateTime();
			ExStatus status = xie_core.fnXIE_Core_DateTime_FromBinary(src, (ltc ? ExBoolean.True : ExBoolean.False), out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		#endregion

		#region 変換系:

		/// <summary>
		/// 日時構造体からバイナリ日時への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <param name="ltc">変換元のタイムゾーン [true=ローカル時刻、false=協定世界時]</param>
		/// <returns>
		///		バイナリ日時に変換して返します。
		/// </returns>
		public ulong ToBinary(TxDateTime src, bool ltc)
		{
			ulong result = 0;
			ExStatus status = xie_core.fnXIE_Core_DateTime_ToBinary(src, (ltc ? ExBoolean.True : ExBoolean.False), out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
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
		public bool Equals(TxDateTime other)
		{
			if (this.Year != other.Year) return false;
			if (this.Month != other.Month) return false;
			if (this.Day != other.Day) return false;
			if (this.Hour != other.Hour) return false;
			if (this.Minute != other.Minute) return false;
			if (this.Second != other.Second) return false;
			if (this.Millisecond != other.Millisecond) return false;
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
			if (!(obj is TxDateTime)) return false;
			return this.Equals((TxDateTime)obj);
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
		public static bool operator ==(TxDateTime left, TxDateTime right)
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
		public static bool operator !=(TxDateTime left, TxDateTime right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (自身 ← DateTime)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxDateTime(DateTime src)
		{
			return new TxDateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second, src.Millisecond);
		}

		/// <summary>
		/// 暗黙的な型変換 (DateTime ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator DateTime(TxDateTime src)
		{
			return new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second, src.Millisecond);
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
			return string.Format("{0},{1},{2},{3},{4},{5},{6}",
				this.Year,
				this.Month,
				this.Day,
				this.Hour,
				this.Minute,
				this.Second,
				this.Millisecond
				);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxDateTime)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxDateTime)
				{
					var _value = (TxDateTime)value;
					return string.Format("{0},{1},{2},{3},{4},{5},{6}",
						_value.Year,
						_value.Month,
						_value.Day,
						_value.Hour,
						_value.Minute,
						_value.Second,
						_value.Millisecond
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
					var dst = new TxDateTime();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.Year = Convert.ToInt32(values[i]); break;
							case 1: dst.Month = Convert.ToInt32(values[i]); break;
							case 2: dst.Day = Convert.ToInt32(values[i]); break;
							case 3: dst.Hour = Convert.ToInt32(values[i]); break;
							case 4: dst.Minute = Convert.ToInt32(values[i]); break;
							case 5: dst.Second = Convert.ToInt32(values[i]); break;
							case 6: dst.Millisecond = Convert.ToInt32(values[i]); break;
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
