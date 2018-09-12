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
	/// 矩形構造体 (整数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.S32, Pack = 4)]
	public struct TxRectangleI :
		IEquatable<TxRectangleI>
	{
		#region フィールド:

		int m_X;
		int m_Y;
		int m_Width;
		int m_Height;

		#endregion

		#region プロパティ:

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRectangleI.X")]
		public int X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRectangleI.Y")]
		public int Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		/// <summary>
		/// 幅
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRectangleI.Width")]
		public int Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}

		/// <summary>
		/// 高さ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRectangleI.Height")]
		public int Height
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
		[CxDescription("P:XIE.TxRectangleI.Location")]
		public TxPointI Location
		{
			get { return new TxPointI(this.X, this.Y); }
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
		[CxDescription("P:XIE.TxRectangleI.Size")]
		public TxSizeI Size
		{
			get { return new TxSizeI(this.Width, this.Height); }
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
		public TxRectangleI(int x, int y, int width, int height)
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
		public TxRectangleI(TxPointI location, TxSizeI size)
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
		public bool Equals(TxRectangleI other)
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
			if (!(obj is TxRectangleI)) return false;
			return this.Equals((TxRectangleI)obj);
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
		public static bool operator ==(TxRectangleI left, TxRectangleI right)
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
		public static bool operator !=(TxRectangleI left, TxRectangleI right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxRectangleD ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxRectangleD(TxRectangleI src)
		{
			return new TxRectangleD(src.X, src.Y, src.Width, src.Height);
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
		public static implicit operator Rectangle(TxRectangleI src)
		{
			return new Rectangle(src.X, src.Y, src.Width, src.Height);
		}

		/// <summary>
		/// 暗黙的な型変換 (RectangleF ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator RectangleF(TxRectangleI src)
		{
			return new RectangleF(src.X, src.Y, src.Width, src.Height);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← Rectangle)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxRectangleI(Rectangle src)
		{
			return new TxRectangleI(src.X, src.Y, src.Width, src.Height);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← RectangleF)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxRectangleI(RectangleF src)
		{
			return new TxRectangleI(
				(int)System.Math.Round(src.X),
				(int)System.Math.Round(src.Y),
				(int)System.Math.Round(src.Width),
				(int)System.Math.Round(src.Height)
				);
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 台形への変換
		/// </summary>
		/// <returns>
		///		現在の内容を台形構造体に格納して返します。
		/// </returns>
		public TxTrapezoidI ToTrapezoid()
		{
			return new TxTrapezoidI(
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
				if (destinationType == typeof(TxRectangleI)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxRectangleI)
				{
					var _value = (TxRectangleI)value;
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
					var dst = new TxRectangleI();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.X = Convert.ToInt32(values[i]); break;
							case 1: dst.Y = Convert.ToInt32(values[i]); break;
							case 2: dst.Width = Convert.ToInt32(values[i]); break;
							case 3: dst.Height = Convert.ToInt32(values[i]); break;
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
