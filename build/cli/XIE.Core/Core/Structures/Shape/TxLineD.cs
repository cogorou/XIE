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
	/// 直線構造体 (実数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.F64, Pack = 3)]
	public struct TxLineD :
		IEquatable<TxLineD>
	{
		#region プロパティ:

		double m_A;
		double m_B;
		double m_C;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 係数 A
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineD.A")]
		public double A
		{
			get { return m_A; }
			set { m_A = value; }
		}

		/// <summary>
		/// 係数 B
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineD.B")]
		public double B
		{
			get { return m_B; }
			set { m_B = value; }
		}

		/// <summary>
		/// 係数 C
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxLineD.C")]
		public double C
		{
			get { return m_C; }
			set { m_C = value; }
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="a">係数 A</param>
		/// <param name="b">係数 B</param>
		/// <param name="c">係数 C</param>
		public TxLineD(double a, double b, double c)
		{
			m_A = a;
			m_B = b;
			m_C = c;
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
		public bool Equals(TxLineD other)
		{
			if (this.A != other.A) return false;
			if (this.B != other.B) return false;
			if (this.C != other.C) return false;
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
			if (!(obj is TxLineD)) return false;
			return this.Equals((TxLineD)obj);
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
		public static bool operator ==(TxLineD left, TxLineD right)
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
		public static bool operator !=(TxLineD left, TxLineD right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxLineI ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxLineI(TxLineD src)
		{
			return new TxLineI(
				(int)System.Math.Round(src.A),
				(int)System.Math.Round(src.B),
				(int)System.Math.Round(src.C)
				);
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 線分への変換
		/// </summary>
		/// <param name="region">範囲</param>
		/// <returns>
		///		この直線が指定された範囲と交差する２点を端点とする線分を返します。
		/// </returns>
		public TxLineSegmentD ToLineSegment(TxRectangleD region)
		{
			return ToLineSegment(
				region.X, 
				region.Y, 
				region.X + region.Width, 
				region.Y + region.Height
				);
		}

		/// <summary>
		/// 線分への変換
		/// </summary>
		/// <param name="st">範囲の始点</param>
		/// <param name="ed">範囲の終点</param>
		/// <returns>
		///		この直線が指定された範囲と交差する２点を端点とする線分を返します。
		/// </returns>
		public TxLineSegmentD ToLineSegment(TxPointD st, TxPointD ed)
		{
			return ToLineSegment(st.X, st.Y, ed.X, ed.Y);
		}

		/// <summary>
		/// 線分への変換
		/// </summary>
		/// <param name="x1">範囲の始点(X)</param>
		/// <param name="y1">範囲の始点(Y)</param>
		/// <param name="x2">範囲の終点(X)</param>
		/// <param name="y2">範囲の終点(Y)</param>
		/// <returns>
		///		この直線が指定された範囲と交差する２点を端点とする線分を返します。
		/// </returns>
		public TxLineSegmentD ToLineSegment(double x1, double y1, double x2, double y2)
		{
			TxLineSegmentD ans = new TxLineSegmentD();
			TxLineD src = this;
			if (src.A == 0 && src.B == 0)
				throw new CxException(ExStatus.Impossible);

			if (x1 > x2)
			{
				double tmp = x1;
				x1 = x2;
				x2 = tmp;
			}
			if (y1 > y2)
			{
				double tmp = y1;
				y1 = y2;
				y2 = tmp;
			}

			TxLineD sideL = new TxLineSegmentD(x1, y1, x1, y2).ToLine();
			TxLineD sideT = new TxLineSegmentD(x1, y1, x2, y1).ToLine();
			TxLineD sideR = new TxLineSegmentD(x2, y1, x2, y2).ToLine();
			TxLineD sideB = new TxLineSegmentD(x1, y2, x2, y2).ToLine();

			double denomL = src.A * sideL.B - src.B * sideL.A;
			double denomT = src.A * sideT.B - src.B * sideT.A;
			double denomR = src.A * sideR.B - src.B * sideR.A;
			double denomB = src.A * sideB.B - src.B * sideB.A;

			var crossL = new TxPointD(x1, y1);
			var crossT = new TxPointD(x1, y1);
			var crossR = new TxPointD(x2, y2);
			var crossB = new TxPointD(x2, y2);

			// Left
			if (System.Math.Abs(denomL) > XIE.Defs.XIE_EPSd)
			{
				crossL.X = (src.B * sideL.C - src.C * sideL.B) / denomL;
				crossL.Y = (src.C * sideL.A - src.A * sideL.C) / denomL;
			}

			// Top
			if (System.Math.Abs(denomT) > XIE.Defs.XIE_EPSd)
			{
				crossT.X = (src.B * sideT.C - src.C * sideT.B) / denomT;
				crossT.Y = (src.C * sideT.A - src.A * sideT.C) / denomT;
			}

			// Right
			if (System.Math.Abs(denomR) > XIE.Defs.XIE_EPSd)
			{
				crossR.X = (src.B * sideR.C - src.C * sideR.B) / denomR;
				crossR.Y = (src.C * sideR.A - src.A * sideR.C) / denomR;
			}

			// Bottom
			if (System.Math.Abs(denomB) > XIE.Defs.XIE_EPSd)
			{
				crossB.X = (src.B * sideB.C - src.C * sideB.B) / denomB;
				crossB.Y = (src.C * sideB.A - src.A * sideB.C) / denomB;
			}

			// ------------------------------

			// when horizontal line
			if (src.A == 0 && src.B != 0)
			{
				ans.X1 = crossL.X;
				ans.Y1 = crossL.Y;
				ans.X2 = crossR.X;
				ans.Y2 = crossR.Y;
				return ans;
			}

			// when vertical line
			if (src.A != 0 && src.B == 0)
			{
				ans.X1 = crossT.X;
				ans.Y1 = crossT.Y;
				ans.X2 = crossB.X;
				ans.Y2 = crossB.Y;
				return ans;
			}

			// ------------------------------

			var cp = new TxPointD[] { crossL, crossT, crossR, crossB };
			var rank = new int[] { 0, 0, 0, 0 };

			for (int i = 0; i < 3; i++)
			{
				for (int j = i + 1; j < 4; j++)
				{
					if (cp[i].X <= cp[j].X)
						rank[j]++;
					else
						rank[i]++;
				}
			}

			for (int i = 0; i < 4; i++)
			{
				if (rank[i] == 1)
				{
					ans.X1 = cp[i].X;
					ans.Y1 = cp[i].Y;
				}
				if (rank[i] == 2)
				{
					ans.X2 = cp[i].X;
					ans.Y2 = cp[i].Y;
				}
			}
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
			return string.Format("{0},{1},{2}",
				this.A,
				this.B,
				this.C
				);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxLineD)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxLineD)
				{
					var _value = (TxLineD)value;
					return string.Format("{0},{1},{2}",
						_value.A,
						_value.B,
						_value.C
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
					var dst = new TxLineD();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.A = Convert.ToDouble(values[i]); break;
							case 1: dst.B = Convert.ToDouble(values[i]); break;
							case 2: dst.C = Convert.ToDouble(values[i]); break;
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
