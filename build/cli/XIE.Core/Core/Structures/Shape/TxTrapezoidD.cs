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
	/// 台形構造体 (実数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.F64, Pack = 8)]
	public struct TxTrapezoidD :
		IEquatable<TxTrapezoidD>
	{
		#region フィールド:

		double m_X1;
		double m_Y1;
		double m_X2;
		double m_Y2;
		double m_X3;
		double m_Y3;
		double m_X4;
		double m_Y4;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 左上 (X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidD.X1")]
		public double X1
		{
			get { return m_X1; }
			set { m_X1 = value; }
		}

		/// <summary>
		/// 左上 (Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidD.Y1")]
		public double Y1
		{
			get { return m_Y1; }
			set { m_Y1 = value; }
		}

		/// <summary>
		/// 右上(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidD.X2")]
		public double X2
		{
			get { return m_X2; }
			set { m_X2 = value; }
		}

		/// <summary>
		/// 右上(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidD.Y2")]
		public double Y2
		{
			get { return m_Y2; }
			set { m_Y2 = value; }
		}

		/// <summary>
		/// 右下(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidD.X3")]
		public double X3
		{
			get { return m_X3; }
			set { m_X3 = value; }
		}

		/// <summary>
		/// 右下(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidD.Y3")]
		public double Y3
		{
			get { return m_Y3; }
			set { m_Y3 = value; }
		}

		/// <summary>
		/// 左下(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidD.X4")]
		public double X4
		{
			get { return m_X4; }
			set { m_X4 = value; }
		}

		/// <summary>
		/// 左下(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidD.Y4")]
		public double Y4
		{
			get { return m_Y4; }
			set { m_Y4 = value; }
		}

		#endregion

		#region プロパティ:(補助)

		/// <summary>
		/// 頂点1
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidD.Vertex1")]
		public TxPointD Vertex1
		{
			get { return new TxPointD(this.X1, this.Y1); }
			set
			{
				X1 = value.X;
				Y1 = value.Y;
			}
		}

		/// <summary>
		/// 頂点2
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidD.Vertex2")]
		public TxPointD Vertex2
		{
			get { return new TxPointD(this.X2, this.Y2); }
			set
			{
				X2 = value.X;
				Y2 = value.Y;
			}
		}

		/// <summary>
		/// 頂点3
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidD.Vertex3")]
		public TxPointD Vertex3
		{
			get { return new TxPointD(this.X3, this.Y3); }
			set
			{
				X3 = value.X;
				Y3 = value.Y;
			}
		}

		/// <summary>
		/// 頂点4
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidD.Vertex4")]
		public TxPointD Vertex4
		{
			get { return new TxPointD(this.X4, this.Y4); }
			set
			{
				X4 = value.X;
				Y4 = value.Y;
			}
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="x1">左上(X 座標)</param>
		/// <param name="y1">左上(Y 座標)</param>
		/// <param name="x2">右上(X 座標)</param>
		/// <param name="y2">右上(Y 座標)</param>
		/// <param name="x3">右下(X 座標)</param>
		/// <param name="y3">右下(Y 座標)</param>
		/// <param name="x4">左下(X 座標)</param>
		/// <param name="y4">左下(Y 座標)</param>
		public TxTrapezoidD(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
		{
			m_X1 = x1;
			m_Y1 = y1;
			m_X2 = x2;
			m_Y2 = y2;
			m_X3 = x3;
			m_Y3 = y3;
			m_X4 = x4;
			m_Y4 = y4;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="p1">左上</param>
		/// <param name="p2">右上</param>
		/// <param name="p3">右下</param>
		/// <param name="p4">左下</param>
		public TxTrapezoidD(TxPointD p1, TxPointD p2, TxPointD p3, TxPointD p4)
		{
			m_X1 = p1.X;
			m_Y1 = p1.Y;
			m_X2 = p2.X;
			m_Y2 = p2.Y;
			m_X3 = p3.X;
			m_Y3 = p3.Y;
			m_X4 = p4.X;
			m_Y4 = p4.Y;
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
		public bool Equals(TxTrapezoidD other)
		{
			if (this.X1 != other.X1) return false;
			if (this.Y1 != other.Y1) return false;
			if (this.X2 != other.X2) return false;
			if (this.Y2 != other.Y2) return false;
			if (this.X3 != other.X3) return false;
			if (this.Y3 != other.Y3) return false;
			if (this.X4 != other.X4) return false;
			if (this.Y4 != other.Y4) return false;
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
			if (!(obj is TxTrapezoidD)) return false;
			return this.Equals((TxTrapezoidD)obj);
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
		public static bool operator ==(TxTrapezoidD left, TxTrapezoidD right)
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
		public static bool operator !=(TxTrapezoidD left, TxTrapezoidD right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxTrapezoidI ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxTrapezoidI(TxTrapezoidD src)
		{
			return new TxTrapezoidI(
				(int)System.Math.Round(src.X1),
				(int)System.Math.Round(src.Y1),
				(int)System.Math.Round(src.X2),
				(int)System.Math.Round(src.Y2),
				(int)System.Math.Round(src.X3),
				(int)System.Math.Round(src.Y3),
				(int)System.Math.Round(src.X4),
				(int)System.Math.Round(src.Y4)
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
			return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
				this.X1, this.Y1,
				this.X2, this.Y2,
				this.X3, this.Y3,
				this.X4, this.Y4
				);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxTrapezoidD)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxTrapezoidD)
				{
					var _value = (TxTrapezoidD)value;
					return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
						_value.X1, _value.Y1,
						_value.X2, _value.Y2,
						_value.X3, _value.Y3,
						_value.X4, _value.Y4
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
					var dst = new TxTrapezoidD();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.X1 = Convert.ToDouble(values[i]); break;
							case 1: dst.Y1 = Convert.ToDouble(values[i]); break;
							case 2: dst.X2 = Convert.ToDouble(values[i]); break;
							case 3: dst.Y2 = Convert.ToDouble(values[i]); break;
							case 4: dst.X3 = Convert.ToDouble(values[i]); break;
							case 5: dst.Y3 = Convert.ToDouble(values[i]); break;
							case 6: dst.X4 = Convert.ToDouble(values[i]); break;
							case 7: dst.Y4 = Convert.ToDouble(values[i]); break;
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
