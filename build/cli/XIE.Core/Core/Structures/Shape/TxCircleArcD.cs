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
	/// 真円の円弧構造体 (実数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.F64, Pack = 6)]
	public struct TxCircleArcD :
		IEquatable<TxCircleArcD>
	{
		#region フィールド:

		double m_X;
		double m_Y;
		double m_Radius;
		double m_StartAngle;
		double m_SweepAngle;

		#endregion

		#region プロパティ:

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxCircleArcD.X")]
		public double X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxCircleArcD.Y")]
		public double Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		/// <summary>
		/// 半径
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxCircleArcD.Radius")]
		public double Radius
		{
			get { return m_Radius; }
			set { m_Radius = value; }
		}

		/// <summary>
		/// 開始角 (度) [0~360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxCircleArcD.StartAngle")]
		public double StartAngle
		{
			get { return m_StartAngle; }
			set { m_StartAngle = value; }
		}

		/// <summary>
		/// 円弧範囲 (度) [0~±360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxCircleArcD.SweepAngle")]
		public double SweepAngle
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
		[CxDescription("P:XIE.TxCircleArcD.Center")]
		public TxPointD Center
		{
			get { return new TxPointD(this.X, this.Y); }
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
		/// <param name="radius">半径</param>
		/// <param name="start_angle">開始角 (度) [0~360]</param>
		/// <param name="sweep_angle">円弧範囲 (度) [0~±360]</param>
		public TxCircleArcD(double x, double y, double radius, double start_angle, double sweep_angle)
		{
			m_X = x;
			m_Y = y;
			m_Radius = radius;
			m_StartAngle = start_angle;
			m_SweepAngle = sweep_angle;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="center">中心</param>
		/// <param name="radius">半径</param>
		/// <param name="start_angle">開始角 (度) [0~360]</param>
		/// <param name="sweep_angle">円弧範囲 (度) [0~±360]</param>
		public TxCircleArcD(TxPointD center, double radius, double start_angle, double sweep_angle)
		{
			m_X = center.X;
			m_Y = center.Y;
			m_Radius = radius;
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
		public bool Equals(TxCircleArcD other)
		{
			if (this.X != other.X) return false;
			if (this.Y != other.Y) return false;
			if (this.Radius != other.Radius) return false;
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
			if (!(obj is TxCircleArcD)) return false;
			return this.Equals((TxCircleArcD)obj);
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
		public static bool operator ==(TxCircleArcD left, TxCircleArcD right)
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
		public static bool operator !=(TxCircleArcD left, TxCircleArcD right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxCircleArcI ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxCircleArcI(TxCircleArcD src)
		{
			return new TxCircleArcI(
				(int)System.Math.Round(src.X),
				(int)System.Math.Round(src.Y),
				(int)System.Math.Round(src.Radius),
				(int)System.Math.Round(src.StartAngle),
				(int)System.Math.Round(src.SweepAngle)
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
			return string.Format("{0},{1},{2},{3},{4}",
				this.X,
				this.Y,
				this.Radius,
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
				if (destinationType == typeof(TxCircleArcD)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxCircleArcD)
				{
					var _value = (TxCircleArcD)value;
					return string.Format("{0},{1},{2},{3},{4}",
						_value.X,
						_value.Y,
						_value.Radius,
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
					var dst = new TxCircleArcD();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.X = Convert.ToDouble(values[i]); break;
							case 1: dst.Y = Convert.ToDouble(values[i]); break;
							case 2: dst.Radius = Convert.ToDouble(values[i]); break;
							case 3: dst.StartAngle = Convert.ToDouble(values[i]); break;
							case 4: dst.SweepAngle = Convert.ToDouble(values[i]); break;
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
