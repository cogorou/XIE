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
	/// 点構造体 (実数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.F64, Pack = 2)]
	public struct TxPointD :
		IEquatable<TxPointD>
	{
		#region フィールド:

		double m_X;
		double m_Y;

		#endregion

		#region プロパティ:

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxPointD.X")]
		public double X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxPointD.Y")]
		public double Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="x">X 座標</param>
		/// <param name="y">Y 座標</param>
		public TxPointD(double x, double y)
		{
			m_X = x;
			m_Y = y;
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
		public bool Equals(TxPointD other)
		{
			if (this.X != other.X) return false;
			if (this.Y != other.Y) return false;
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
			if (!(obj is TxPointD)) return false;
			return this.Equals((TxPointD)obj);
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
		public static bool operator ==(TxPointD left, TxPointD right)
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
		public static bool operator !=(TxPointD left, TxPointD right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region オペレータ.(加算)

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(TxPointD ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X + ope2.X);
			dst.Y = (double)(ope1.Y + ope2.Y);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(TxPointD ope1, TxSizeD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X + ope2.Width);
			dst.Y = (double)(ope1.Y + ope2.Height);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(TxPointD ope1, TxSizeI ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X + ope2.Width);
			dst.Y = (double)(ope1.Y + ope2.Height);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(TxPointD ope1, double ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X + ope2);
			dst.Y = (double)(ope1.Y + ope2);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(double ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1 + ope2.X);
			dst.Y = (double)(ope1 + ope2.Y);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(TxPointD ope1, Point ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X + ope2.X);
			dst.Y = (double)(ope1.Y + ope2.Y);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(Point ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X + ope2.X);
			dst.Y = (double)(ope1.Y + ope2.Y);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(TxPointD ope1, PointF ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X + ope2.X);
			dst.Y = (double)(ope1.Y + ope2.Y);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(PointF ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X + ope2.X);
			dst.Y = (double)(ope1.Y + ope2.Y);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(TxPointD ope1, Size ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X + ope2.Width);
			dst.Y = (double)(ope1.Y + ope2.Height);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(Size ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.Width + ope2.X);
			dst.Y = (double)(ope1.Height + ope2.Y);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(TxPointD ope1, SizeF ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X + ope2.Width);
			dst.Y = (double)(ope1.Y + ope2.Height);
			return dst;
		}

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxPointD operator +(SizeF ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.Width + ope2.X);
			dst.Y = (double)(ope1.Height + ope2.Y);
			return dst;
		}

		#endregion

		#region オペレータ.(減算)

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(TxPointD ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X - ope2.X);
			dst.Y = (double)(ope1.Y - ope2.Y);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(TxPointD ope1, TxSizeD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X - ope2.Width);
			dst.Y = (double)(ope1.Y - ope2.Height);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(TxPointD ope1, TxSizeI ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X - ope2.Width);
			dst.Y = (double)(ope1.Y - ope2.Height);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(TxPointD ope1, double ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X - ope2);
			dst.Y = (double)(ope1.Y - ope2);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(double ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1 - ope2.X);
			dst.Y = (double)(ope1 - ope2.Y);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(TxPointD ope1, Point ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X - ope2.X);
			dst.Y = (double)(ope1.Y - ope2.Y);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(Point ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X - ope2.X);
			dst.Y = (double)(ope1.Y - ope2.Y);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(TxPointD ope1, PointF ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X - ope2.X);
			dst.Y = (double)(ope1.Y - ope2.Y);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(PointF ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X - ope2.X);
			dst.Y = (double)(ope1.Y - ope2.Y);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(TxPointD ope1, Size ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X - ope2.Width);
			dst.Y = (double)(ope1.Y - ope2.Height);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(Size ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.Width - ope2.X);
			dst.Y = (double)(ope1.Height - ope2.Y);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(TxPointD ope1, SizeF ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X - ope2.Width);
			dst.Y = (double)(ope1.Y - ope2.Height);
			return dst;
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxPointD operator -(SizeF ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.Width - ope2.X);
			dst.Y = (double)(ope1.Height - ope2.Y);
			return dst;
		}

		#endregion

		#region オペレータ.(乗算)

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(TxPointD ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X * ope2.X);
			dst.Y = (double)(ope1.Y * ope2.Y);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(TxPointD ope1, TxSizeD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X * ope2.Width);
			dst.Y = (double)(ope1.Y * ope2.Height);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(TxPointD ope1, TxSizeI ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X * ope2.Width);
			dst.Y = (double)(ope1.Y * ope2.Height);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(TxPointD ope1, double ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X * ope2);
			dst.Y = (double)(ope1.Y * ope2);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(double ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1 * ope2.X);
			dst.Y = (double)(ope1 * ope2.Y);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(TxPointD ope1, Point ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X * ope2.X);
			dst.Y = (double)(ope1.Y * ope2.Y);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(Point ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X * ope2.X);
			dst.Y = (double)(ope1.Y * ope2.Y);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(TxPointD ope1, PointF ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X * ope2.X);
			dst.Y = (double)(ope1.Y * ope2.Y);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(PointF ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X * ope2.X);
			dst.Y = (double)(ope1.Y * ope2.Y);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(TxPointD ope1, Size ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X * ope2.Width);
			dst.Y = (double)(ope1.Y * ope2.Height);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(Size ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.Width * ope2.X);
			dst.Y = (double)(ope1.Height * ope2.Y);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(TxPointD ope1, SizeF ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X * ope2.Width);
			dst.Y = (double)(ope1.Y * ope2.Height);
			return dst;
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxPointD operator *(SizeF ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.Width * ope2.X);
			dst.Y = (double)(ope1.Height * ope2.Y);
			return dst;
		}

		#endregion

		#region オペレータ.(除算)

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(TxPointD ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X / ope2.X);
			dst.Y = (double)(ope1.Y / ope2.Y);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(TxPointD ope1, TxSizeD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X / ope2.Width);
			dst.Y = (double)(ope1.Y / ope2.Height);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(TxPointD ope1, TxSizeI ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X / ope2.Width);
			dst.Y = (double)(ope1.Y / ope2.Height);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(TxPointD ope1, double ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X / ope2);
			dst.Y = (double)(ope1.Y / ope2);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(double ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1 / ope2.X);
			dst.Y = (double)(ope1 / ope2.Y);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(TxPointD ope1, Point ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X / ope2.X);
			dst.Y = (double)(ope1.Y / ope2.Y);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(Point ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X / ope2.X);
			dst.Y = (double)(ope1.Y / ope2.Y);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(TxPointD ope1, PointF ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X / ope2.X);
			dst.Y = (double)(ope1.Y / ope2.Y);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(PointF ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X / ope2.X);
			dst.Y = (double)(ope1.Y / ope2.Y);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(TxPointD ope1, Size ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X / ope2.Width);
			dst.Y = (double)(ope1.Y / ope2.Height);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(Size ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.Width / ope2.X);
			dst.Y = (double)(ope1.Height / ope2.Y);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(TxPointD ope1, SizeF ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.X / ope2.Width);
			dst.Y = (double)(ope1.Y / ope2.Height);
			return dst;
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxPointD operator /(SizeF ope1, TxPointD ope2)
		{
			TxPointD dst = new TxPointD();
			dst.X = (double)(ope1.Width / ope2.X);
			dst.Y = (double)(ope1.Height / ope2.Y);
			return dst;
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxPointI ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxPointI(TxPointD src)
		{
			return new TxPointI(
				(int)System.Math.Round(src.X),
				(int)System.Math.Round(src.Y)
				);
		}

		#endregion

		#region 暗黙的な型変換: (自身 ⇔ CLI)

		/// <summary>
		/// 暗黙的な型変換 (Point ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator Point(TxPointD src)
		{
			return new Point(
				(int)System.Math.Round(src.X),
				(int)System.Math.Round(src.Y)
				);
		}

		/// <summary>
		/// 暗黙的な型変換 (PointF ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator PointF(TxPointD src)
		{
			return new PointF(
				(float)src.X,
				(float)src.Y
				);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← Point)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxPointD(Point src)
		{
			return new TxPointD(src.X, src.Y);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← PointF)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxPointD(PointF src)
		{
			return new TxPointD(src.X, src.Y);
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
			return string.Format("{0},{1}",
				this.X,
				this.Y
				);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxPointD)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxPointD)
				{
					var _value = (TxPointD)value;
					return string.Format("{0},{1}",
						_value.X,
						_value.Y
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
					var dst = new TxPointD();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.X = Convert.ToDouble(values[i]); break;
							case 1: dst.Y = Convert.ToDouble(values[i]); break;
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
