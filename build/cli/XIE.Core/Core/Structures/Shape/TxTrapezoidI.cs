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
	/// 台形構造体 (整数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.S32, Pack = 8)]
	public struct TxTrapezoidI :
		IEquatable<TxTrapezoidI>
	{
		#region フィールド:

		int m_X1;
		int m_Y1;
		int m_X2;
		int m_Y2;
		int m_X3;
		int m_Y3;
		int m_X4;
		int m_Y4;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 左上 (X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidI.X1")]
		public int X1
		{
			get { return m_X1; }
			set { m_X1 = value; }
		}

		/// <summary>
		/// 左上 (Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidI.Y1")]
		public int Y1
		{
			get { return m_Y1; }
			set { m_Y1 = value; }
		}

		/// <summary>
		/// 右上(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidI.X2")]
		public int X2
		{
			get { return m_X2; }
			set { m_X2 = value; }
		}

		/// <summary>
		/// 右上(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidI.Y2")]
		public int Y2
		{
			get { return m_Y2; }
			set { m_Y2 = value; }
		}

		/// <summary>
		/// 右下(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidI.X3")]
		public int X3
		{
			get { return m_X3; }
			set { m_X3 = value; }
		}

		/// <summary>
		/// 右下(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidI.Y3")]
		public int Y3
		{
			get { return m_Y3; }
			set { m_Y3 = value; }
		}

		/// <summary>
		/// 左下(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidI.X4")]
		public int X4
		{
			get { return m_X4; }
			set { m_X4 = value; }
		}

		/// <summary>
		/// 左下(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxTrapezoidI.Y4")]
		public int Y4
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
		[CxDescription("P:XIE.TxTrapezoidI.Vertex1")]
		public TxPointI Vertex1
		{
			get { return new TxPointI(this.X1, this.Y1); }
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
		[CxDescription("P:XIE.TxTrapezoidI.Vertex2")]
		public TxPointI Vertex2
		{
			get { return new TxPointI(this.X2, this.Y2); }
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
		[CxDescription("P:XIE.TxTrapezoidI.Vertex3")]
		public TxPointI Vertex3
		{
			get { return new TxPointI(this.X3, this.Y3); }
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
		[CxDescription("P:XIE.TxTrapezoidI.Vertex4")]
		public TxPointI Vertex4
		{
			get { return new TxPointI(this.X4, this.Y4); }
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
		public TxTrapezoidI(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
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
		public TxTrapezoidI(TxPointI p1, TxPointI p2, TxPointI p3, TxPointI p4)
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
		public bool Equals(TxTrapezoidI other)
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
			if (!(obj is TxTrapezoidI)) return false;
			return this.Equals((TxTrapezoidI)obj);
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
		public static bool operator ==(TxTrapezoidI left, TxTrapezoidI right)
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
		public static bool operator !=(TxTrapezoidI left, TxTrapezoidI right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxTrapezoidD ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxTrapezoidD(TxTrapezoidI src)
		{
			return new TxTrapezoidD(
				src.X1, src.Y1,
				src.X2, src.Y2,
				src.X3, src.Y3,
				src.X4, src.Y4
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
				if (destinationType == typeof(TxTrapezoidI)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxTrapezoidI)
				{
					var _value = (TxTrapezoidI)value;
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
					var dst = new TxTrapezoidI();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.X1 = Convert.ToInt32(values[i]); break;
							case 1: dst.Y1 = Convert.ToInt32(values[i]); break;
							case 2: dst.X2 = Convert.ToInt32(values[i]); break;
							case 3: dst.Y2 = Convert.ToInt32(values[i]); break;
							case 4: dst.X3 = Convert.ToInt32(values[i]); break;
							case 5: dst.Y3 = Convert.ToInt32(values[i]); break;
							case 6: dst.X4 = Convert.ToInt32(values[i]); break;
							case 7: dst.Y4 = Convert.ToInt32(values[i]); break;
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
