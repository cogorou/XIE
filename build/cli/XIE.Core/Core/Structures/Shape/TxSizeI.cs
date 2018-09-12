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
	/// サイズ構造体 (整数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.S32, Pack = 2)]
	public struct TxSizeI :
		IEquatable<TxSizeI>
	{
		#region フィールド:

		int m_Width;
		int m_Height;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 幅
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxSizeI.Width")]
		public int Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}

		/// <summary>
		/// 高さ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxSizeI.Height")]
		public int Height
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
		public TxSizeI(int width, int height)
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
		public bool Equals(TxSizeI other)
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
			if (!(obj is TxSizeI)) return false;
			return this.Equals((TxSizeI)obj);
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
		public static bool operator ==(TxSizeI left, TxSizeI right)
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
		public static bool operator !=(TxSizeI left, TxSizeI right)
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
		public static TxSizeI operator +(TxSizeI ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width + ope2.Width);
			dst.Height = (int)(ope1.Height + ope2.Height);
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
		public static TxSizeI operator +(TxSizeI ope1, TxPointD ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width + ope2.X);
			dst.Height = (int)(ope1.Height + ope2.Y);
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
		public static TxSizeI operator +(TxSizeI ope1, int ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width + ope2);
			dst.Height = (int)(ope1.Height + ope2);
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
		public static TxSizeI operator +(int ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1 + ope2.Width);
			dst.Height = (int)(ope1 + ope2.Height);
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
		public static TxSizeI operator +(TxSizeI ope1, Size ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width + ope2.Width);
			dst.Height = (int)(ope1.Height + ope2.Height);
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
		public static TxSizeI operator +(Size ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width + ope2.Width);
			dst.Height = (int)(ope1.Height + ope2.Height);
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
		public static TxSizeI operator +(TxSizeI ope1, SizeF ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width + ope2.Width);
			dst.Height = (int)(ope1.Height + ope2.Height);
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
		public static TxSizeI operator +(SizeF ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width + ope2.Width);
			dst.Height = (int)(ope1.Height + ope2.Height);
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
		public static TxSizeI operator +(TxSizeI ope1, Point ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width + ope2.X);
			dst.Height = (int)(ope1.Height + ope2.Y);
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
		public static TxSizeI operator +(Point ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.X + ope2.Width);
			dst.Height = (int)(ope1.Y + ope2.Height);
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
		public static TxSizeI operator +(TxSizeI ope1, PointF ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width + ope2.X);
			dst.Height = (int)(ope1.Height + ope2.Y);
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
		public static TxSizeI operator +(PointF ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.X + ope2.Width);
			dst.Height = (int)(ope1.Y + ope2.Height);
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
		public static TxSizeI operator -(TxSizeI ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width - ope2.Width);
			dst.Height = (int)(ope1.Height - ope2.Height);
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
		public static TxSizeI operator -(TxSizeI ope1, TxPointD ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width - ope2.X);
			dst.Height = (int)(ope1.Height - ope2.Y);
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
		public static TxSizeI operator -(TxSizeI ope1, int ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width - ope2);
			dst.Height = (int)(ope1.Height - ope2);
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
		public static TxSizeI operator -(int ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1 - ope2.Width);
			dst.Height = (int)(ope1 - ope2.Height);
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
		public static TxSizeI operator -(TxSizeI ope1, Size ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width - ope2.Width);
			dst.Height = (int)(ope1.Height - ope2.Height);
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
		public static TxSizeI operator -(Size ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width - ope2.Width);
			dst.Height = (int)(ope1.Height - ope2.Height);
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
		public static TxSizeI operator -(TxSizeI ope1, SizeF ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width - ope2.Width);
			dst.Height = (int)(ope1.Height - ope2.Height);
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
		public static TxSizeI operator -(SizeF ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width - ope2.Width);
			dst.Height = (int)(ope1.Height - ope2.Height);
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
		public static TxSizeI operator -(TxSizeI ope1, Point ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width - ope2.X);
			dst.Height = (int)(ope1.Height - ope2.Y);
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
		public static TxSizeI operator -(Point ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.X - ope2.Width);
			dst.Height = (int)(ope1.Y - ope2.Height);
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
		public static TxSizeI operator -(TxSizeI ope1, PointF ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width - ope2.X);
			dst.Height = (int)(ope1.Height - ope2.Y);
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
		public static TxSizeI operator -(PointF ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.X - ope2.Width);
			dst.Height = (int)(ope1.Y - ope2.Height);
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
		public static TxSizeI operator *(TxSizeI ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width * ope2.Width);
			dst.Height = (int)(ope1.Height * ope2.Height);
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
		public static TxSizeI operator *(TxSizeI ope1, TxPointD ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width * ope2.X);
			dst.Height = (int)(ope1.Height * ope2.Y);
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
		public static TxSizeI operator *(TxSizeI ope1, int ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width * ope2);
			dst.Height = (int)(ope1.Height * ope2);
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
		public static TxSizeI operator *(int ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1 * ope2.Width);
			dst.Height = (int)(ope1 * ope2.Height);
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
		public static TxSizeI operator *(TxSizeI ope1, Size ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width * ope2.Width);
			dst.Height = (int)(ope1.Height * ope2.Height);
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
		public static TxSizeI operator *(Size ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width * ope2.Width);
			dst.Height = (int)(ope1.Height * ope2.Height);
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
		public static TxSizeI operator *(TxSizeI ope1, SizeF ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width * ope2.Width);
			dst.Height = (int)(ope1.Height * ope2.Height);
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
		public static TxSizeI operator *(SizeF ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width * ope2.Width);
			dst.Height = (int)(ope1.Height * ope2.Height);
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
		public static TxSizeI operator *(TxSizeI ope1, Point ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width * ope2.X);
			dst.Height = (int)(ope1.Height * ope2.Y);
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
		public static TxSizeI operator *(Point ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.X * ope2.Width);
			dst.Height = (int)(ope1.Y * ope2.Height);
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
		public static TxSizeI operator *(TxSizeI ope1, PointF ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width * ope2.X);
			dst.Height = (int)(ope1.Height * ope2.Y);
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
		public static TxSizeI operator *(PointF ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.X * ope2.Width);
			dst.Height = (int)(ope1.Y * ope2.Height);
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
		public static TxSizeI operator /(TxSizeI ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width / ope2.Width);
			dst.Height = (int)(ope1.Height / ope2.Height);
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
		public static TxSizeI operator /(TxSizeI ope1, TxPointD ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width / ope2.X);
			dst.Height = (int)(ope1.Height / ope2.Y);
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
		public static TxSizeI operator /(TxSizeI ope1, int ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width / ope2);
			dst.Height = (int)(ope1.Height / ope2);
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
		public static TxSizeI operator /(int ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1 / ope2.Width);
			dst.Height = (int)(ope1 / ope2.Height);
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
		public static TxSizeI operator /(TxSizeI ope1, Size ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width / ope2.Width);
			dst.Height = (int)(ope1.Height / ope2.Height);
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
		public static TxSizeI operator /(Size ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width / ope2.Width);
			dst.Height = (int)(ope1.Height / ope2.Height);
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
		public static TxSizeI operator /(TxSizeI ope1, SizeF ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width / ope2.Width);
			dst.Height = (int)(ope1.Height / ope2.Height);
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
		public static TxSizeI operator /(SizeF ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width / ope2.Width);
			dst.Height = (int)(ope1.Height / ope2.Height);
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
		public static TxSizeI operator /(TxSizeI ope1, Point ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width / ope2.X);
			dst.Height = (int)(ope1.Height / ope2.Y);
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
		public static TxSizeI operator /(Point ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.X / ope2.Width);
			dst.Height = (int)(ope1.Y / ope2.Height);
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
		public static TxSizeI operator /(TxSizeI ope1, PointF ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.Width / ope2.X);
			dst.Height = (int)(ope1.Height / ope2.Y);
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
		public static TxSizeI operator /(PointF ope1, TxSizeI ope2)
		{
			TxSizeI dst = new TxSizeI();
			dst.Width = (int)(ope1.X / ope2.Width);
			dst.Height = (int)(ope1.Y / ope2.Height);
			return dst;
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxSizeD ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxSizeD(TxSizeI src)
		{
			return new TxSizeD(src.Width, src.Height);
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
		public static implicit operator Size(TxSizeI src)
		{
			return new Size(src.Width, src.Height);
		}

		/// <summary>
		/// 暗黙的な型変換 (SizeF ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator SizeF(TxSizeI src)
		{
			return new SizeF(src.Width, src.Height);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← Size)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxSizeI(Size src)
		{
			return new TxSizeI(src.Width, src.Height);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← SizeF)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxSizeI(SizeF src)
		{
			return new TxSizeI(
				(int)System.Math.Round(src.Width),
				(int)System.Math.Round(src.Height)
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
				if (destinationType == typeof(TxSizeI)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxSizeI)
				{
					var _value = (TxSizeI)value;
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
					var dst = new TxSizeI();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.Width = Convert.ToInt32(values[i]); break;
							case 1: dst.Height = Convert.ToInt32(values[i]); break;
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
