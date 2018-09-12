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

namespace XIE.GDI
{
	/// <summary>
	/// ペン情報構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public struct TxGdiPen :
		IEquatable<TxGdiPen>
	{
		#region フィールド:

		private TxRGB8x4 m_Color;

		private float m_Width;

		private ExGdiPenStyle m_Style;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 色
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdiPen.Color")]
		public TxRGB8x4 Color
		{
			get { return m_Color; }
			set { m_Color = value; }
		}

		/// <summary>
		/// ペン幅 [0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdiPen.Width")]
		public float Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}

		/// <summary>
		/// スタイル
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdiPen.Style")]
		public ExGdiPenStyle Style
		{
			get { return m_Style; }
			set { m_Style = value; }
		}

		#endregion

		#region 既定値:

		/// <summary>
		/// 既定値
		/// </summary>
		public static TxGdiPen Default
		{
			get
			{
				var result = new TxGdiPen();
				result.Color = new TxRGB8x4(0xFF, 0xFF, 0xFF, 0xFF);
				result.Width = 0;
				result.Style = ExGdiPenStyle.Solid;
				return result;
			}
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="color">色</param>
		public TxGdiPen(TxRGB8x4 color)
		{
			m_Color = color;
			m_Width = 0;
			m_Style = ExGdiPenStyle.Solid;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="color">色</param>
		/// <param name="width">ペン幅 [0~]</param>
		/// <param name="style">スタイル</param>
		public TxGdiPen(TxRGB8x4 color, float width, ExGdiPenStyle style)
		{
			m_Color = color;
			m_Width = width;
			m_Style = style;
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
		public bool Equals(TxGdiPen other)
		{
			if (this.Color != other.Color) return false;
			if (this.Width != other.Width) return false;
			if (this.Style != other.Style) return false;
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
			if (!(obj is TxGdiPen)) return false;
			return this.Equals((TxGdiPen)obj);
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
		public static bool operator ==(TxGdiPen left, TxGdiPen right)
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
		public static bool operator !=(TxGdiPen left, TxGdiPen right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 変換系:

		/// <summary>
		/// ペンの生成
		/// </summary>
		/// <param name="mag">表示倍率</param>
		/// <returns>
		///		ペンオブジェクトを新しく生成して返します。
		/// </returns>
		public Pen ToPen(double mag)
		{
			var pen = new Pen(this.Color);
			pen.Width = (float)(this.Width * mag);
			switch (this.Style)
			{
				default:
				case ExGdiPenStyle.None:
				case ExGdiPenStyle.Solid:
					break;
				case ExGdiPenStyle.Dot:
					pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
					break;
				case ExGdiPenStyle.Dash:
					pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
					break;
				case ExGdiPenStyle.DashDot:
					pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
					break;
				case ExGdiPenStyle.DashDotDot:
					pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
					break;
			}
			return pen;
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxGdiPen)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxGdiPen)
				{
					var _value = (TxGdiPen)value;
					return string.Format("{0},{1},{2}",
						_value.Color,
						_value.Width,
						_value.Style
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
					var dst = new TxGdiPen();
					var color = new TxRGB8x4();
					var width = 0f;
					var style = ExGdiPenStyle.None;
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: color.R = Convert.ToByte(values[i]); break;
							case 1: color.G = Convert.ToByte(values[i]); break;
							case 2: color.B = Convert.ToByte(values[i]); break;
							case 3: color.A = Convert.ToByte(values[i]); break;
							case 4: width = Convert.ToSingle(values[i]); break;
							case 5: Enum.TryParse<ExGdiPenStyle>(values[i], out style); break;
						}
					}
					dst.Color = color;
					dst.Width = width;
					dst.Style = style;
					return dst;
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}
}
