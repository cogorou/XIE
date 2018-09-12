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
	/// 線分構造体 (整数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.S32, Pack = 4)]
	public struct TxLineSegmentI :
		IEquatable<TxLineSegmentI>
	{
		#region フィールド:

		int m_X1;
		int m_Y1;
		int m_X2;
		int m_Y2;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 始点(X)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineSegmentI.X1")]
		public int X1
		{
			get { return m_X1; }
			set { m_X1 = value; }
		}

		/// <summary>
		/// 始点(Y)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineSegmentI.Y1")]
		public int Y1
		{
			get { return m_Y1; }
			set { m_Y1 = value; }
		}

		/// <summary>
		/// 終点(X)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineSegmentI.X2")]
		public int X2
		{
			get { return m_X2; }
			set { m_X2 = value; }
		}

		/// <summary>
		/// 終点(Y)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineSegmentI.Y2")]
		public int Y2
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
		[CxDescription("P:XIE.TxLineSegmentI.Point1")]
		public TxPointI Point1
		{
			get { return new TxPointI(this.X1, this.Y1); }
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
		[CxDescription("P:XIE.TxLineSegmentI.Point2")]
		public TxPointI Point2
		{
			get { return new TxPointI(this.X2, this.Y2); }
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
		public TxLineSegmentI(int x1, int y1, int x2, int y2)
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
		public TxLineSegmentI(TxPointI point1, TxPointI point2)
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
		public bool Equals(TxLineSegmentI other)
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
			if (!(obj is TxLineSegmentI)) return false;
			return this.Equals((TxLineSegmentI)obj);
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
		public static bool operator ==(TxLineSegmentI left, TxLineSegmentI right)
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
		public static bool operator !=(TxLineSegmentI left, TxLineSegmentI right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxLineSegmentD ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxLineSegmentD(TxLineSegmentI src)
		{
			return new TxLineSegmentD(src.X1, src.Y1, src.X2, src.Y2);
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 直線への変換
		/// </summary>
		/// <returns>
		///		この線分上を平行に通る直線を返します。
		/// </returns>
		public TxLineI ToLine()
		{
			int da = Y1 - Y2;
			int db = X2 - X1;
			int dc = X1 * Y2 - X2 * Y1;

			if ((da == 0) && (db == 0))
				throw new CxException(ExStatus.Impossible);

			int denom = (System.Math.Abs(da) > System.Math.Abs(db)) ? da : db;

			var ans = new TxLineI();
			ans.A = (int)System.Math.Round((double)da / denom);
			ans.B = (int)System.Math.Round((double)db / denom);
			ans.C = (int)System.Math.Round((double)dc / denom);
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
				if (destinationType == typeof(TxLineSegmentI)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxLineSegmentI)
				{
					var _value = (TxLineSegmentI)value;
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
					var dst = new TxLineSegmentI();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.X1 = Convert.ToInt32(values[i]); break;
							case 1: dst.Y1 = Convert.ToInt32(values[i]); break;
							case 2: dst.X2 = Convert.ToInt32(values[i]); break;
							case 3: dst.Y2 = Convert.ToInt32(values[i]); break;
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
