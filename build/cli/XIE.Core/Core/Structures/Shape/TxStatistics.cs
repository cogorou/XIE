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
	/// 統計データ構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.F64, Pack = 5)]
	public struct TxStatistics :
		IEquatable<TxStatistics>
	{
		#region フィールド:

		double m_Count;
		double m_Max;
		double m_Min;
		double m_Sum1;
		double m_Sum2;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 要素数
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxStatistics.Count")]
		public double Count
		{
			get { return m_Count; }
			set { m_Count = value; }
		}

		/// <summary>
		/// 最大
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxStatistics.Max")]
		public double Max
		{
			get { return m_Max; }
			set { m_Max = value; }
		}

		/// <summary>
		/// 最小
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxStatistics.Min")]
		public double Min
		{
			get { return m_Min; }
			set { m_Min = value; }
		}

		/// <summary>
		/// 総和
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxStatistics.Sum1")]
		public double Sum1
		{
			get { return m_Sum1; }
			set { m_Sum1 = value; }
		}

		/// <summary>
		/// ２乗の総和
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxStatistics.Sum2")]
		public double Sum2
		{
			get { return m_Sum2; }
			set { m_Sum2 = value; }
		}

		#endregion

		#region プロパティ: (拡張)

		/// <summary>
		/// 平均
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Extension")]
		[CxDescription("P:XIE.TxStatistics.Mean")]
		public double Mean
		{
			get
			{
				if (Count == 0)
					return 0;
				return Sum1 / Count;
			}
		}

		/// <summary>
		/// 標準偏差
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Extension")]
		[CxDescription("P:XIE.TxStatistics.Sigma")]
		public double Sigma
		{
			get
			{
				if (Count == 0)
					return 0;
				return System.Math.Sqrt(Variance);
			}
		}

		/// <summary>
		/// 分散
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Extension")]
		[CxDescription("P:XIE.TxStatistics.Variance")]
		public double Variance
		{
			get
			{
				if (Count == 0)
					return 0;
				return (Sum2 / Count) - (Mean * Mean);
			}
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="count">個数</param>
		/// <param name="maxval">最大</param>
		/// <param name="minval">最小</param>
		/// <param name="sum1">総和</param>
		/// <param name="sum2">２乗の総和</param>
		public TxStatistics(double count, double maxval, double minval, double sum1, double sum2)
		{
			m_Count = count;
			m_Max = maxval;
			m_Min = minval;
			m_Sum1 = sum1;
			m_Sum2 = sum2;
		}

		#endregion

		#region スタティックメソッド:

		/// <summary>
		/// 統計データの初期値
		/// </summary>
		/// <returns>
		///		初期値にリセットした統計データを返します。
		/// </returns>
		public static TxStatistics Default()
		{
			var result = new TxStatistics();
			result.m_Count = 0;
			result.m_Sum1 = 0;
			result.m_Sum2 = 0;
			result.m_Min = +double.MaxValue;
			result.m_Max = -double.MaxValue;
			return result;
		}

		/// <summary>
		/// 配列要素の統計
		/// </summary>
		/// <param name="src">配列</param>
		/// <returns>
		///		計算した統計データを返します。
		/// </returns>
		public static TxStatistics From(System.Collections.IEnumerable src)
		{
			var stat = TxStatistics.Default();

			#region 最大、最小、総和、２乗の総和を求める.
			if (src is IEnumerable<double>)
			{
				foreach (double data in src)
					stat += data;
			}
			else if (src is IEnumerable<float>)
			{
				foreach (float data in src)
					stat += data;
			}
			else if (src is IEnumerable<uint>)
			{
				foreach (uint data in src)
					stat += data;
			}
			else if (src is IEnumerable<int>)
			{
				foreach (int data in src)
					stat += data;
			}
			else if (src is IEnumerable<ushort>)
			{
				foreach (ushort data in src)
					stat += data;
			}
			else if (src is IEnumerable<short>)
			{
				foreach (short data in src)
					stat += data;
			}
			else if (src is IEnumerable<byte>)
			{
				foreach (byte data in src)
					stat += data;
			}
			else if (src is IEnumerable<char>)
			{
				foreach (char data in src)
					stat += data;
			}
			#endregion

			return stat;
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 統計データのリセット
		/// </summary>
		public void Reset()
		{
			m_Count = 0;
			m_Sum1 = 0;
			m_Sum2 = 0;
			m_Min = +double.MaxValue;
			m_Max = -double.MaxValue;
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
		public bool Equals(TxStatistics other)
		{
			if (this.Count != other.Count) return false;
			if (this.Max != other.Max) return false;
			if (this.Min != other.Min) return false;
			if (this.Sum1 != other.Sum1) return false;
			if (this.Sum2 != other.Sum2) return false;
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
			if (!(obj is TxStatistics)) return false;
			return this.Equals((TxStatistics)obj);
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
		public static bool operator ==(TxStatistics left, TxStatistics right)
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
		public static bool operator !=(TxStatistics left, TxStatistics right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 加算オペレータ:

		/// <summary>
		/// 加算オペレータ
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算後の統計データを返します。
		/// </returns>
		public static TxStatistics operator +(TxStatistics ope1, TxStatistics ope2)
		{
			var ans = new TxStatistics();
			ans.m_Count = ope1.Count + ope2.Count;
			ans.m_Max = System.Math.Max(ope1.Max, ope2.Max);
			ans.m_Min = System.Math.Min(ope1.Min, ope2.Min);
			ans.m_Sum1 = ope1.Sum1 + ope2.Sum1;
			ans.m_Sum2 = ope1.Sum2 + ope2.Sum2;
			return ans;
		}
		/// <summary>
		/// 加算オペレータ
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算後の統計データを返します。
		/// </returns>
		public static TxStatistics operator +(TxStatistics ope1, double ope2)
		{
			var ans = new TxStatistics();
			ans.m_Count = ope1.Count + 1;
			if (ope1.Count == 0)
			{
				ope1.Max = -double.MaxValue;
				ope1.Min = +double.MaxValue;
			}
			ans.m_Max = System.Math.Max(ope1.Max, ope2);
			ans.m_Min = System.Math.Min(ope1.Min, ope2);
			ans.m_Sum1 = ope1.Sum1 + ope2;
			ans.m_Sum2 = ope1.Sum2 + ope2 * ope2;
			return ans;
		}
		/// <summary>
		/// 加算オペレータ
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算後の統計データを返します。
		/// </returns>
		public static TxStatistics operator +(TxStatistics ope1, int ope2)
		{
			var ans = new TxStatistics();
			ans.m_Count = ope1.Count + 1;
			if (ope1.Count == 0)
			{
				ope1.Max = -double.MaxValue;
				ope1.Min = +double.MaxValue;
			}
			ans.m_Max = System.Math.Max(ope1.Max, ope2);
			ans.m_Min = System.Math.Min(ope1.Min, ope2);
			ans.m_Sum1 = ope1.Sum1 + ope2;
			ans.m_Sum2 = ope1.Sum2 + ope2 * ope2;
			return ans;
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
			return string.Format("{0},{1},{2},{3},{4}",
				this.Count,
				this.Max,
				this.Min,
				this.Sum1,
				this.Sum2
				);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxStatistics)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxStatistics)
				{
					var _value = (TxStatistics)value;
					return string.Format("{0},{1},{2},{3},{4}",
							_value.Count,
							_value.Max,
							_value.Min,
							_value.Sum1,
							_value.Sum2
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
					var dst = new TxStatistics();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.Count = Convert.ToDouble(values[i]); break;
							case 1: dst.Max = Convert.ToDouble(values[i]); break;
							case 2: dst.Min = Convert.ToDouble(values[i]); break;
							case 3: dst.Sum1 = Convert.ToDouble(values[i]); break;
							case 4: dst.Sum2 = Convert.ToDouble(values[i]); break;
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
