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
	/// 線分構造体 (実数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.F64, Pack = 4)]
	public struct TxLineSegmentD :
		IEquatable<TxLineSegmentD>
	{
		#region フィールド:

		double m_X1;
		double m_Y1;
		double m_X2;
		double m_Y2;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 始点(X)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineSegmentD.X1")]
		public double X1
		{
			get { return m_X1; }
			set { m_X1 = value; }
		}

		/// <summary>
		/// 始点(Y)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineSegmentD.Y1")]
		public double Y1
		{
			get { return m_Y1; }
			set { m_Y1 = value; }
		}

		/// <summary>
		/// 終点(X)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineSegmentD.X2")]
		public double X2
		{
			get { return m_X2; }
			set { m_X2 = value; }
		}

		/// <summary>
		/// 終点(Y)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineSegmentD.Y2")]
		public double Y2
		{
			get { return m_Y2; }
			set { m_Y2 = value; }
		}

		#endregion

		#region プロパティ:(補助)

		/// <summary>
		/// 端点1
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineSegmentD.Point1")]
		public TxPointD Point1
		{
			get { return new TxPointD(this.X1, this.Y1); }
			set
			{
				X1 = value.X;
				Y1 = value.Y;
			}
		}

		/// <summary>
		/// 端点2
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineSegmentD.Point2")]
		public TxPointD Point2
		{
			get { return new TxPointD(this.X2, this.Y2); }
			set
			{
				X2 = value.X;
				Y2 = value.Y;
			}
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="x1">始点(X)</param>
		/// <param name="y1">始点(Y)</param>
		/// <param name="x2">終点(X)</param>
		/// <param name="y2">終点(Y)</param>
		public TxLineSegmentD(double x1, double y1, double x2, double y2)
		{
			m_X1 = x1;
			m_Y1 = y1;
			m_X2 = x2;
			m_Y2 = y2;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="point1">始点</param>
		/// <param name="point2">終点</param>
		public TxLineSegmentD(TxPointD point1, TxPointD point2)
		{
			m_X1 = point1.X;
			m_Y1 = point1.Y;
			m_X2 = point2.X;
			m_Y2 = point2.Y;
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
		public bool Equals(TxLineSegmentD other)
		{
			if (this.X1 != other.X1) return false;
			if (this.Y1 != other.Y1) return false;
			if (this.X2 != other.X2) return false;
			if (this.Y2 != other.Y2) return false;
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
			if (!(obj is TxLineSegmentD)) return false;
			return this.Equals((TxLineSegmentD)obj);
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
		public static bool operator ==(TxLineSegmentD left, TxLineSegmentD right)
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
		public static bool operator !=(TxLineSegmentD left, TxLineSegmentD right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxLineSegmentI ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxLineSegmentI(TxLineSegmentD src)
		{
			return new TxLineSegmentI(
				(int)System.Math.Round(src.X1),
				(int)System.Math.Round(src.Y1),
				(int)System.Math.Round(src.X2),
				(int)System.Math.Round(src.Y2)
				);
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 直線への変換
		/// </summary>
		/// <returns>
		///		この線分上を平行に通る直線を返します。
		/// </returns>
		public TxLineD ToLine()
		{
			double da = Y1 - Y2;
			double db = X2 - X1;
			double dc = X1 * Y2 - X2 * Y1;

			if ((da == 0) && (db == 0))
				throw new CxException(ExStatus.Impossible);

			double denom = (System.Math.Abs(da) > System.Math.Abs(db)) ? da : db;

			var ans = new TxLineD();
			ans.A = da / denom;
			ans.B = db / denom;
			ans.C = dc / denom;
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
			return string.Format("{0},{1},{2},{3}",
				this.X1,
				this.Y1,
				this.X2,
				this.Y2
				);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxLineSegmentD)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxLineSegmentD)
				{
					var _value = (TxLineSegmentD)value;
					return string.Format("{0},{1},{2},{3}",
						_value.X1,
						_value.Y1,
						_value.X2,
						_value.Y2
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
					var dst = new TxLineSegmentD();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.X1 = Convert.ToDouble(values[i]); break;
							case 1: dst.Y1 = Convert.ToDouble(values[i]); break;
							case 2: dst.X2 = Convert.ToDouble(values[i]); break;
							case 3: dst.Y2 = Convert.ToDouble(values[i]); break;
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
