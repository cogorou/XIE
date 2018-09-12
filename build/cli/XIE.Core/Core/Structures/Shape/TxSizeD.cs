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
	/// サイズ構造体 (実数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.F64, Pack = 2)]
	public struct TxSizeD :
		IEquatable<TxSizeD>
	{
		#region フィールド:

		double m_Width;
		double m_Height;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 幅
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxSizeD.Width")]
		public double Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}

		/// <summary>
		/// 高さ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxSizeD.Height")]
		public double Height
		{
			get { return m_Height; }
			set { m_Height = value; }
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="width">幅</param>
		/// <param name="height">高さ</param>
		public TxSizeD(double width, double height)
		{
			m_Width = width;
			m_Height = height;
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
		public bool Equals(TxSizeD other)
		{
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
			if (!(obj is TxSizeD)) return false;
			return this.Equals((TxSizeD)obj);
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
		public static bool operator ==(TxSizeD left, TxSizeD right)
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
		public static bool operator !=(TxSizeD left, TxSizeD right)
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
		public static TxSizeD operator +(TxSizeD ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width + ope2.Width);
			dst.Height = (double)(ope1.Height + ope2.Height);
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
		public static TxSizeD operator +(TxSizeD ope1, TxPointD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width + ope2.X);
			dst.Height = (double)(ope1.Height + ope2.Y);
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
		public static TxSizeD operator +(TxSizeD ope1, double ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width + ope2);
			dst.Height = (double)(ope1.Height + ope2);
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
		public static TxSizeD operator +(double ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1 + ope2.Width);
			dst.Height = (double)(ope1 + ope2.Height);
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
		public static TxSizeD operator +(TxSizeD ope1, Size ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width + ope2.Width);
			dst.Height = (double)(ope1.Height + ope2.Height);
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
		public static TxSizeD operator +(Size ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width + ope2.Width);
			dst.Height = (double)(ope1.Height + ope2.Height);
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
		public static TxSizeD operator +(TxSizeD ope1, SizeF ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width + ope2.Width);
			dst.Height = (double)(ope1.Height + ope2.Height);
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
		public static TxSizeD operator +(SizeF ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width + ope2.Width);
			dst.Height = (double)(ope1.Height + ope2.Height);
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
		public static TxSizeD operator +(TxSizeD ope1, Point ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width + ope2.X);
			dst.Height = (double)(ope1.Height + ope2.Y);
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
		public static TxSizeD operator +(Point ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.X + ope2.Width);
			dst.Height = (double)(ope1.Y + ope2.Height);
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
		public static TxSizeD operator +(TxSizeD ope1, PointF ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width + ope2.X);
			dst.Height = (double)(ope1.Height + ope2.Y);
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
		public static TxSizeD operator +(PointF ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.X + ope2.Width);
			dst.Height = (double)(ope1.Y + ope2.Height);
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
		public static TxSizeD operator -(TxSizeD ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width - ope2.Width);
			dst.Height = (double)(ope1.Height - ope2.Height);
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
		public static TxSizeD operator -(TxSizeD ope1, TxPointD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width - ope2.X);
			dst.Height = (double)(ope1.Height - ope2.Y);
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
		public static TxSizeD operator -(TxSizeD ope1, double ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width - ope2);
			dst.Height = (double)(ope1.Height - ope2);
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
		public static TxSizeD operator -(double ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1 - ope2.Width);
			dst.Height = (double)(ope1 - ope2.Height);
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
		public static TxSizeD operator -(TxSizeD ope1, Size ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width - ope2.Width);
			dst.Height = (double)(ope1.Height - ope2.Height);
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
		public static TxSizeD operator -(Size ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width - ope2.Width);
			dst.Height = (double)(ope1.Height - ope2.Height);
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
		public static TxSizeD operator -(TxSizeD ope1, SizeF ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width - ope2.Width);
			dst.Height = (double)(ope1.Height - ope2.Height);
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
		public static TxSizeD operator -(SizeF ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width - ope2.Width);
			dst.Height = (double)(ope1.Height - ope2.Height);
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
		public static TxSizeD operator -(TxSizeD ope1, Point ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width - ope2.X);
			dst.Height = (double)(ope1.Height - ope2.Y);
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
		public static TxSizeD operator -(Point ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.X - ope2.Width);
			dst.Height = (double)(ope1.Y - ope2.Height);
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
		public static TxSizeD operator -(TxSizeD ope1, PointF ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width - ope2.X);
			dst.Height = (double)(ope1.Height - ope2.Y);
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
		public static TxSizeD operator -(PointF ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.X - ope2.Width);
			dst.Height = (double)(ope1.Y - ope2.Height);
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
		public static TxSizeD operator *(TxSizeD ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width * ope2.Width);
			dst.Height = (double)(ope1.Height * ope2.Height);
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
		public static TxSizeD operator *(TxSizeD ope1, TxPointD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width * ope2.X);
			dst.Height = (double)(ope1.Height * ope2.Y);
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
		public static TxSizeD operator *(TxSizeD ope1, double ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width * ope2);
			dst.Height = (double)(ope1.Height * ope2);
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
		public static TxSizeD operator *(double ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1 * ope2.Width);
			dst.Height = (double)(ope1 * ope2.Height);
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
		public static TxSizeD operator *(TxSizeD ope1, Size ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width * ope2.Width);
			dst.Height = (double)(ope1.Height * ope2.Height);
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
		public static TxSizeD operator *(Size ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width * ope2.Width);
			dst.Height = (double)(ope1.Height * ope2.Height);
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
		public static TxSizeD operator *(TxSizeD ope1, SizeF ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width * ope2.Width);
			dst.Height = (double)(ope1.Height * ope2.Height);
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
		public static TxSizeD operator *(SizeF ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width * ope2.Width);
			dst.Height = (double)(ope1.Height * ope2.Height);
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
		public static TxSizeD operator *(TxSizeD ope1, Point ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width * ope2.X);
			dst.Height = (double)(ope1.Height * ope2.Y);
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
		public static TxSizeD operator *(Point ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.X * ope2.Width);
			dst.Height = (double)(ope1.Y * ope2.Height);
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
		public static TxSizeD operator *(TxSizeD ope1, PointF ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width * ope2.X);
			dst.Height = (double)(ope1.Height * ope2.Y);
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
		public static TxSizeD operator *(PointF ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.X * ope2.Width);
			dst.Height = (double)(ope1.Y * ope2.Height);
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
		public static TxSizeD operator /(TxSizeD ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width / ope2.Width);
			dst.Height = (double)(ope1.Height / ope2.Height);
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
		public static TxSizeD operator /(TxSizeD ope1, TxPointD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width / ope2.X);
			dst.Height = (double)(ope1.Height / ope2.Y);
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
		public static TxSizeD operator /(TxSizeD ope1, double ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width / ope2);
			dst.Height = (double)(ope1.Height / ope2);
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
		public static TxSizeD operator /(double ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1 / ope2.Width);
			dst.Height = (double)(ope1 / ope2.Height);
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
		public static TxSizeD operator /(TxSizeD ope1, Size ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width / ope2.Width);
			dst.Height = (double)(ope1.Height / ope2.Height);
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
		public static TxSizeD operator /(Size ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width / ope2.Width);
			dst.Height = (double)(ope1.Height / ope2.Height);
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
		public static TxSizeD operator /(TxSizeD ope1, SizeF ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width / ope2.Width);
			dst.Height = (double)(ope1.Height / ope2.Height);
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
		public static TxSizeD operator /(SizeF ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width / ope2.Width);
			dst.Height = (double)(ope1.Height / ope2.Height);
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
		public static TxSizeD operator /(TxSizeD ope1, Point ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width / ope2.X);
			dst.Height = (double)(ope1.Height / ope2.Y);
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
		public static TxSizeD operator /(Point ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.X / ope2.Width);
			dst.Height = (double)(ope1.Y / ope2.Height);
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
		public static TxSizeD operator /(TxSizeD ope1, PointF ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.Width / ope2.X);
			dst.Height = (double)(ope1.Height / ope2.Y);
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
		public static TxSizeD operator /(PointF ope1, TxSizeD ope2)
		{
			TxSizeD dst = new TxSizeD();
			dst.Width = (double)(ope1.X / ope2.Width);
			dst.Height = (double)(ope1.Y / ope2.Height);
			return dst;
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxSizeI ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxSizeI(TxSizeD src)
		{
			return new TxSizeI(
				(int)System.Math.Round(src.Width),
				(int)System.Math.Round(src.Height)
				);
		}

		#endregion

		#region 暗黙的な型変換: (自身 ⇔ CLI)

		/// <summary>
		/// 暗黙的な型変換 (Size ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator Size(TxSizeD src)
		{
			return new Size(
				(int)System.Math.Round(src.Width),
				(int)System.Math.Round(src.Height)
				);
		}

		/// <summary>
		/// 暗黙的な型変換 (SizeF ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator SizeF(TxSizeD src)
		{
			return new SizeF(
				(float)src.Width,
				(float)src.Height
				);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← Size)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxSizeD(Size src)
		{
			return new TxSizeD(src.Width, src.Height);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← SizeF)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxSizeD(SizeF src)
		{
			return new TxSizeD(src.Width, src.Height);
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
				if (destinationType == typeof(TxSizeD)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxSizeD)
				{
					var _value = (TxSizeD)value;
					return string.Format("{0},{1}",
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
					var dst = new TxSizeD();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.Width = Convert.ToDouble(values[i]); break;
							case 1: dst.Height = Convert.ToDouble(values[i]); break;
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
