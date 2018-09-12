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
	/// 矩形構造体 (実数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.F64, Pack = 4)]
	public struct TxRectangleD :
		IEquatable<TxRectangleD>
	{
		#region フィールド:

		double m_X;
		double m_Y;
		double m_Width;
		double m_Height;

		#endregion

		#region プロパティ:

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRectangleD.X")]
		public double X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRectangleD.Y")]
		public double Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		/// <summary>
		/// 幅
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRectangleD.Width")]
		public double Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}

		/// <summary>
		/// 高さ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRectangleD.Height")]
		public double Height
		{
			get { return m_Height; }
			set { m_Height = value; }
		}

		#endregion

		#region プロパティ:(補助)

		/// <summary>
		/// 矩形の位置 (左上)
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRectangleD.Location")]
		public TxPointD Location
		{
			get { return new TxPointD(this.X, this.Y); }
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}

		/// <summary>
		/// 矩形のサイズ
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRectangleD.Size")]
		public TxSizeD Size
		{
			get { return new TxSizeD(this.Width, this.Height); }
			set
			{
				Width = value.Width;
				Height = value.Height;
			}
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="x">X 座標</param>
		/// <param name="y">Y 座標</param>
		/// <param name="width">幅</param>
		/// <param name="height">高さ</param>
		public TxRectangleD(double x, double y, double width, double height)
		{
			m_X = x;
			m_Y = y;
			m_Width = width;
			m_Height = height;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="location">位置</param>
		/// <param name="size">サイズ</param>
		public TxRectangleD(TxPointD location, TxSizeD size)
		{
			m_X = location.X;
			m_Y = location.Y;
			m_Width = size.Width;
			m_Height = size.Height;
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
		public bool Equals(TxRectangleD other)
		{
			if (this.X != other.X) return false;
			if (this.Y != other.Y) return false;
			if (this.Width != other.Width) return false;
			if (this.Height != other.Height) return false;
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
			if (!(obj is TxRectangleD)) return false;
			return this.Equals((TxRectangleD)obj);
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
		public static bool operator ==(TxRectangleD left, TxRectangleD right)
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
		public static bool operator !=(TxRectangleD left, TxRectangleD right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxRectangleI ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxRectangleI(TxRectangleD src)
		{
			return new TxRectangleI(
				(int)System.Math.Round(src.X),
				(int)System.Math.Round(src.Y),
				(int)System.Math.Round(src.Width),
				(int)System.Math.Round(src.Height)
				);
		}

		#endregion

		#region 暗黙的な型変換: (自身 ⇔ CLI)

		/// <summary>
		/// 暗黙的な型変換 (Rectangle ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator Rectangle(TxRectangleD src)
		{
			return new Rectangle(
				(int)System.Math.Round(src.X),
				(int)System.Math.Round(src.Y),
				(int)System.Math.Round(src.Width),
				(int)System.Math.Round(src.Height)
				);
		}

		/// <summary>
		/// 暗黙的な型変換 (RectangleF ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator RectangleF(TxRectangleD src)
		{
			return new RectangleF(
				(float)src.X,
				(float)src.Y,
				(float)src.Width,
				(float)src.Height
				);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← Rectangle)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxRectangleD(Rectangle src)
		{
			return new TxRectangleD(src.X, src.Y, src.Width, src.Height);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← RectangleF)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxRectangleD(RectangleF src)
		{
			return new TxRectangleD(src.X, src.Y, src.Width, src.Height);
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 台形への変換
		/// </summary>
		/// <returns>
		///		現在の内容を台形構造体に格納して返します。
		/// </returns>
		public TxTrapezoidD ToTrapezoid()
		{
			return new TxTrapezoidD(
				X,
				Y,
				X + Width,
				Y,
				X + Width,
				Y + Height,
				X,
				Y + Height
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
			return string.Format("{0},{1},{2},{3}",
				this.X,
				this.Y,
				this.Width,
				this.Height
				);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxRectangleD)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxRectangleD)
				{
					var _value = (TxRectangleD)value;
					return string.Format("{0},{1},{2},{3}",
						_value.X,
						_value.Y,
						_value.Width,
						_value.Height
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
					var dst = new TxRectangleD();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.X = Convert.ToDouble(values[i]); break;
							case 1: dst.Y = Convert.ToDouble(values[i]); break;
							case 2: dst.Width = Convert.ToDouble(values[i]); break;
							case 3: dst.Height = Convert.ToDouble(values[i]); break;
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
