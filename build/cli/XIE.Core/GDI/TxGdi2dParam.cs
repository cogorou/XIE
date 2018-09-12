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
	/// オーバレイ図形パラメータ構造体 (２次元)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxGdi2dParam :
		IEquatable<TxGdi2dParam>
	{
		#region フィールド:

		private double m_Angle;
		private TxPointD m_Axis;
		private TxRGB8x4 m_BkColor;
		private ExBoolean m_BkEnable;
		private TxRGB8x4 m_PenColor;
		private ExGdiPenStyle m_PenStyle;
		private int m_PenWidth;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 回転角 (degree)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdi2dParam.Angle")]
		public double Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}

		/// <summary>
		/// 回転の機軸 (Location からの相対値)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdi2dParam.Axis")]
		public TxPointD Axis
		{
			get { return m_Axis; }
			set { m_Axis = value; }
		}

		/// <summary>
		/// 背景色
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdi2dParam.BkColor")]
		public TxRGB8x4 BkColor
		{
			get { return m_BkColor; }
			set { m_BkColor = value; }
		}

		/// <summary>
		/// 背景の有効属性
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdi2dParam.BkEnable")]
		public ExBoolean BkEnable
		{
			get { return m_BkEnable; }
			set { m_BkEnable = value; }
		}

		/// <summary>
		/// ペン色
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdi2dParam.PenColor")]
		public TxRGB8x4 PenColor
		{
			get { return m_PenColor; }
			set { m_PenColor = value; }
		}

		/// <summary>
		/// ペンスタイル
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdi2dParam.PenStyle")]
		public ExGdiPenStyle PenStyle
		{
			get { return m_PenStyle; }
			set { m_PenStyle = value; }
		}

		/// <summary>
		/// ペン幅 [0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdi2dParam.PenWidth")]
		public int PenWidth
		{
			get { return m_PenWidth; }
			set { m_PenWidth = value; }
		}

		#endregion

		#region 既定値:

		/// <summary>
		/// 既定値
		/// </summary>
		public static TxGdi2dParam Default
		{
			get
			{
				var result = new TxGdi2dParam();
				result.Angle = 0;
				result.Axis = new TxPointD(0, 0);
				result.BkColor = new TxRGB8x4(0xFF, 0xFF, 0xFF, 0xFF);
				result.BkEnable = ExBoolean.False;
				result.PenColor = new TxRGB8x4(0xFF, 0xFF, 0xFF, 0xFF);
				result.PenStyle = ExGdiPenStyle.Solid;
				result.PenWidth = 1;
				return result;
			}
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="angle">回転角 (degree)</param>
		/// <param name="axis">回転の機軸 (Location からの相対値)</param>
		/// <param name="bkColor">背景色</param>
		/// <param name="bkEnable">背景の有効属性</param>
		/// <param name="penColor">ペン色</param>
		/// <param name="penStyle">ペンスタイル</param>
		/// <param name="penWidth">ペン幅 [0~]</param>
		public TxGdi2dParam(double angle, TxPointD axis, TxRGB8x4 bkColor, ExBoolean bkEnable, TxRGB8x4 penColor, ExGdiPenStyle penStyle, int penWidth)
		{
			m_Angle = angle;
			m_Axis = axis;
			m_BkColor = bkColor;
			m_BkEnable = bkEnable;
			m_PenColor = penColor;
			m_PenStyle = penStyle;
			m_PenWidth = penWidth;
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
		public bool Equals(TxGdi2dParam other)
		{
			if (this.Angle != other.Angle) return false;
			if (this.Axis != other.Axis) return false;
			if (this.BkColor != other.BkColor) return false;
			if (this.BkEnable != other.BkEnable) return false;
			if (this.PenColor != other.PenColor) return false;
			if (this.PenStyle != other.PenStyle) return false;
			if (this.PenWidth != other.PenWidth) return false;
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
			if (!(obj is TxGdi2dParam)) return false;
			return this.Equals((TxGdi2dParam)obj);
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
		public static bool operator ==(TxGdi2dParam left, TxGdi2dParam right)
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
		public static bool operator !=(TxGdi2dParam left, TxGdi2dParam right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxGdi2dParam)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxGdi2dParam)
				{
					var _value = (TxGdi2dParam)value;
					return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}",
						_value.Axis.X,
						_value.Axis.Y,
						_value.Angle,
						_value.BkColor.R,
						_value.BkColor.G,
						_value.BkColor.B,
						_value.BkColor.A,
						_value.BkEnable,
						_value.PenColor.R,
						_value.PenColor.G,
						_value.PenColor.B,
						_value.PenColor.A,
						_value.PenStyle,
						_value.PenWidth
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
					var dst = new TxGdi2dParam();
					var axis = new TxPointD();
					var angle = 0.0;
					var bkcolor = new TxRGB8x4();
					var bkenable = ExBoolean.False;
					var pencolor = new TxRGB8x4();
					var penstyle = ExGdiPenStyle.Solid;
					var penwidth = 1;
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: axis.X = Convert.ToDouble(values[i]); break;
							case 1: axis.Y = Convert.ToDouble(values[i]); break;
							case 2: angle = Convert.ToDouble(values[i]); break;
							case 3: bkcolor.R = Convert.ToByte(values[i]); break;
							case 4: bkcolor.G = Convert.ToByte(values[i]); break;
							case 5: bkcolor.B = Convert.ToByte(values[i]); break;
							case 6: bkcolor.A = Convert.ToByte(values[i]); break;
							case 7: Enum.TryParse<ExBoolean>(values[i], out bkenable); break;
							case 8: pencolor.R = Convert.ToByte(values[i]); break;
							case 9: pencolor.G = Convert.ToByte(values[i]); break;
							case 10: pencolor.B = Convert.ToByte(values[i]); break;
							case 11: pencolor.A = Convert.ToByte(values[i]); break;
							case 12: Enum.TryParse<ExGdiPenStyle>(values[i], out penstyle); break;
							case 13: penwidth = Convert.ToInt32(values[i]); break;
						}
					}
					dst.Axis = axis;
					dst.Angle = angle;
					dst.BkColor = bkcolor;
					dst.BkEnable = bkenable;
					dst.PenColor = pencolor;
					dst.PenStyle = penstyle;
					dst.PenWidth = penwidth;
					return dst;
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}
}
