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
	/// ブラシ情報構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public struct TxGdiBrush :
		IEquatable<TxGdiBrush>

	{
		#region フィールド:

		private TxRGB8x4 m_Color;

		private TxRGB8x4 m_Shadow;

		private ExGdiBrushStyle m_Style;

		private System.Drawing.Drawing2D.HatchStyle m_HatchStyle;

		private System.Drawing.Drawing2D.LinearGradientMode m_LinearGradientMode;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 前景色
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdiBrush.Color")]
		public TxRGB8x4 Color
		{
			get { return m_Color; }
			set { m_Color = value; }
		}

		/// <summary>
		/// 背景色
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdiBrush.Shadow")]
		public TxRGB8x4 Shadow
		{
			get { return m_Shadow; }
			set { m_Shadow = value; }
		}

		/// <summary>
		/// スタイル
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdiBrush.Style")]
		public ExGdiBrushStyle Style
		{
			get { return m_Style; }
			set { m_Style = value; }
		}

		/// <summary>
		/// ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdiBrush.HatchStyle")]
		public System.Drawing.Drawing2D.HatchStyle HatchStyle
		{
			get { return m_HatchStyle; }
			set { m_HatchStyle = value; }
		}

		/// <summary>
		/// 線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdiBrush.LinearGradientMode")]
		public System.Drawing.Drawing2D.LinearGradientMode LinearGradientMode
		{
			get { return m_LinearGradientMode; }
			set { m_LinearGradientMode = value; }
		}

		#endregion

		#region 既定値:

		/// <summary>
		/// 既定値
		/// </summary>
		public static TxGdiBrush Default
		{
			get
			{
				var result = new TxGdiBrush();
				result.Color = new TxRGB8x4(0xFF, 0xFF, 0xFF, 0xFF);
				result.Shadow = new TxRGB8x4(0x00, 0x00, 0x00, 0xFF);
				result.Style = ExGdiBrushStyle.None;
				result.HatchStyle = System.Drawing.Drawing2D.HatchStyle.Cross;
				result.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
				return result;
			}
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ (Solid ブラシ)
		/// </summary>
		/// <param name="color">前景色</param>
		public TxGdiBrush(TxRGB8x4 color)
		{
			m_Color = color;
			m_Shadow = new TxRGB8x4(0, 0, 0, 0);
			m_Style = ExGdiBrushStyle.Solid;
			m_HatchStyle = System.Drawing.Drawing2D.HatchStyle.Cross;
			m_LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
		}

		/// <summary>
		/// コンストラクタ (ハッチブラシ)
		/// </summary>
		/// <param name="color">前景色</param>
		/// <param name="shadow">背景色</param>
		/// <param name="hatch_style">ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)</param>
		public TxGdiBrush(TxRGB8x4 color, TxRGB8x4 shadow, System.Drawing.Drawing2D.HatchStyle hatch_style)
		{
			m_Color = color;
			m_Shadow = shadow;
			m_Style = ExGdiBrushStyle.Hatch;
			m_HatchStyle = hatch_style;
			m_LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
		}

		/// <summary>
		/// コンストラクタ (線形グラデーションブラシ)
		/// </summary>
		/// <param name="color">前景色</param>
		/// <param name="shadow">背景色</param>
		/// <param name="linear_gradient_mode">線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)</param>
		public TxGdiBrush(TxRGB8x4 color, TxRGB8x4 shadow, System.Drawing.Drawing2D.LinearGradientMode linear_gradient_mode)
		{
			m_Color = color;
			m_Shadow = shadow;
			m_Style = ExGdiBrushStyle.LinearGradient;
			m_HatchStyle = System.Drawing.Drawing2D.HatchStyle.Cross;
			m_LinearGradientMode = linear_gradient_mode;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="color">前景色</param>
		/// <param name="shadow">背景色</param>
		/// <param name="style">スタイル</param>
		/// <param name="hatch_style">ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)</param>
		/// <param name="linear_gradient_mode">線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)</param>
		public TxGdiBrush(TxRGB8x4 color, TxRGB8x4 shadow, ExGdiBrushStyle style, System.Drawing.Drawing2D.HatchStyle hatch_style, System.Drawing.Drawing2D.LinearGradientMode linear_gradient_mode)
		{
			m_Color = color;
			m_Shadow = shadow;
			m_Style = style;
			m_HatchStyle = hatch_style;
			m_LinearGradientMode = linear_gradient_mode;
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
		public bool Equals(TxGdiBrush other)
		{
			if (this.Color != other.Color) return false;
			if (this.Shadow != other.Shadow) return false;
			if (this.Style != other.Style) return false;
			if (this.HatchStyle != other.HatchStyle) return false;
			if (this.LinearGradientMode != other.LinearGradientMode) return false;
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
			if (!(obj is TxGdiBrush)) return false;
			return this.Equals((TxGdiBrush)obj);
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
		public static bool operator ==(TxGdiBrush left, TxGdiBrush right)
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
		public static bool operator !=(TxGdiBrush left, TxGdiBrush right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 変換系:

		/// <summary>
		/// ブラシの生成
		/// </summary>
		/// <param name="bounds">対象物の外接矩形 (LinearGradient の時のみ使用します。)</param>
		/// <returns>
		///		ブラシオブジェクトを新しく生成して返します。
		/// </returns>
		public Brush ToBrush(TxRectangleD bounds)
		{
			var rect = new Rectangle(
					(int)System.Math.Floor(bounds.X),
					(int)System.Math.Floor(bounds.Y),
					(int)System.Math.Ceiling(bounds.Width),
					(int)System.Math.Ceiling(bounds.Height)
				);

			switch (this.Style)
			{
				default:
				case ExGdiBrushStyle.None:
				case ExGdiBrushStyle.Solid:
					{
						var brush = new SolidBrush(this.Color);
						return brush;
					}
				case ExGdiBrushStyle.Hatch:
					{
						var brush = new System.Drawing.Drawing2D.HatchBrush(this.HatchStyle, this.Color, this.Shadow);
						return brush;
					}
				case ExGdiBrushStyle.LinearGradient:
					if (rect.Width > 0 && rect.Height > 0)
					{
						var brush = new System.Drawing.Drawing2D.LinearGradientBrush(rect, this.Color, this.Shadow, this.LinearGradientMode);
						return brush;
					}
					else
					{
						var brush = new SolidBrush(this.Color);
						return brush;
					}
			}
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxGdiBrush)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxGdiBrush)
				{
					var _value = (TxGdiBrush)value;
					return string.Format("{0},{1},{2},{3},{4}",
						_value.Color,
						_value.Shadow,
						_value.Style,
						_value.HatchStyle,
						_value.LinearGradientMode
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
					var dst = new TxGdiBrush();
					var color = new TxRGB8x4();
					var shadow = new TxRGB8x4();
					var style = ExGdiBrushStyle.None;
					var hatch = System.Drawing.Drawing2D.HatchStyle.Cross;
					var gradient = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: color.R = Convert.ToByte(values[i]); break;
							case 1: color.G = Convert.ToByte(values[i]); break;
							case 2: color.B = Convert.ToByte(values[i]); break;
							case 3: color.A = Convert.ToByte(values[i]); break;
							case 4: shadow.R = Convert.ToByte(values[i]); break;
							case 5: shadow.G = Convert.ToByte(values[i]); break;
							case 6: shadow.B = Convert.ToByte(values[i]); break;
							case 7: shadow.A = Convert.ToByte(values[i]); break;
							case 8: Enum.TryParse<ExGdiBrushStyle>(values[i], out style); break;
							case 9: Enum.TryParse<System.Drawing.Drawing2D.HatchStyle>(values[i], out hatch); break;
							case 10: Enum.TryParse<System.Drawing.Drawing2D.LinearGradientMode>(values[i], out gradient); break;
						}
					}
					dst.Color = color;
					dst.Shadow = shadow;
					dst.Style = style;
					dst.HatchStyle = hatch;
					dst.LinearGradientMode = gradient;
					return dst;
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}
}
