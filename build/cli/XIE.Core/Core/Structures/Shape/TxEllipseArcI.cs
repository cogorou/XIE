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
	/// 楕円の円弧構造体 (整数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.S32, Pack = 6)]
	public struct TxEllipseArcI :
		IEquatable<TxEllipseArcI>
	{
		#region フィールド:

		int m_X;
		int m_Y;
		int m_RadiusX;
		int m_RadiusY;
		int m_StartAngle;
		int m_SweepAngle;

		#endregion

		#region プロパティ:

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxEllipseArcI.X")]
		public int X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxEllipseArcI.Y")]
		public int Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		/// <summary>
		/// 半径(X軸)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxEllipseArcI.RadiusX")]
		public int RadiusX
		{
			get { return m_RadiusX; }
			set { m_RadiusX = value; }
		}

		/// <summary>
		/// 半径(Y軸)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxEllipseArcI.RadiusY")]
		public int RadiusY
		{
			get { return m_RadiusY; }
			set { m_RadiusY = value; }
		}

		/// <summary>
		/// 開始角 (度) [0~360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxEllipseArcI.StartAngle")]
		public int StartAngle
		{
			get { return m_StartAngle; }
			set { m_StartAngle = value; }
		}

		/// <summary>
		/// 円弧範囲 (度) [0~±360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxEllipseArcI.SweepAngle")]
		public int SweepAngle
		{
			get { return m_SweepAngle; }
			set { m_SweepAngle = value; }
		}

		#endregion

		#region プロパティ:(補助)

		/// <summary>
		/// 中心
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxEllipseArcI.Center")]
		public TxPointI Center
		{
			get { return new TxPointI(this.X, this.Y); }
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="x">X 座標</param>
		/// <param name="y">Y 座標</param>
		/// <param name="radius_x">半径(X軸)</param>
		/// <param name="radius_y">半径(Y軸)</param>
		/// <param name="start_angle">開始角 (度) [0~360]</param>
		/// <param name="sweep_angle">円弧範囲 (度) [0~±360]</param>
		public TxEllipseArcI(int x, int y, int radius_x, int radius_y, int start_angle, int sweep_angle)
		{
			m_X = x;
			m_Y = y;
			m_RadiusX = radius_x;
			m_RadiusY = radius_y;
			m_StartAngle = start_angle;
			m_SweepAngle = sweep_angle;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="center">中心</param>
		/// <param name="radius_x">半径(X軸)</param>
		/// <param name="radius_y">半径(Y軸)</param>
		/// <param name="start_angle">開始角 (度) [0~360]</param>
		/// <param name="sweep_angle">円弧範囲 (度) [0~±360]</param>
		public TxEllipseArcI(TxPointI center, int radius_x, int radius_y, int start_angle, int sweep_angle)
		{
			m_X = center.X;
			m_Y = center.Y;
			m_RadiusX = radius_x;
			m_RadiusY = radius_y;
			m_StartAngle = start_angle;
			m_SweepAngle = sweep_angle;
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
		public bool Equals(TxEllipseArcI other)
		{
			if (this.X != other.X) return false;
			if (this.Y != other.Y) return false;
			if (this.RadiusX != other.RadiusX) return false;
			if (this.RadiusY != other.RadiusY) return false;
			if (this.StartAngle != other.StartAngle) return false;
			if (this.SweepAngle != other.SweepAngle) return false;
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
			if (!(obj is TxEllipseArcI)) return false;
			return this.Equals((TxEllipseArcI)obj);
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
		public static bool operator ==(TxEllipseArcI left, TxEllipseArcI right)
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
		public static bool operator !=(TxEllipseArcI left, TxEllipseArcI right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxEllipseArcD ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxEllipseArcD(TxEllipseArcI src)
		{
			return new TxEllipseArcD(src.X, src.Y, src.RadiusX, src.RadiusY, src.StartAngle, src.SweepAngle);
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
			return string.Format("{0},{1},{2},{3},{4},{5}",
				this.X,
				this.Y,
				this.RadiusX,
				this.RadiusY,
				this.StartAngle,
				this.SweepAngle
				);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxEllipseArcI)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxEllipseArcI)
				{
					var _value = (TxEllipseArcI)value;
					return string.Format("{0},{1},{2},{3},{4},{5}",
						_value.X,
						_value.Y,
						_value.RadiusX,
						_value.RadiusY,
						_value.StartAngle,
						_value.SweepAngle
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
					var dst = new TxEllipseArcI();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.X = Convert.ToInt32(values[i]); break;
							case 1: dst.Y = Convert.ToInt32(values[i]); break;
							case 2: dst.RadiusX = Convert.ToInt32(values[i]); break;
							case 3: dst.RadiusY = Convert.ToInt32(values[i]); break;
							case 4: dst.StartAngle = Convert.ToInt32(values[i]); break;
							case 5: dst.SweepAngle = Convert.ToInt32(values[i]); break;
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
