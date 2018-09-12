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
	/// 点構造体 (整数版)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.S32, Pack = 2)]
	public struct TxPointI :
		IEquatable<TxPointI>
	{
		#region フィールド:

		int m_X;
		int m_Y;

		#endregion

		#region プロパティ:

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxPointI.X")]
		public int X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxPointI.Y")]
		public int Y
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
		public TxPointI(int x, int y)
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
		public bool Equals(TxPointI other)
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
			if (!(obj is TxPointI)) return false;
			return this.Equals((TxPointI)obj);
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
		public static bool operator ==(TxPointI left, TxPointI right)
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
		public static bool operator !=(TxPointI left, TxPointI right)
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
		public static TxPointI operator +(TxPointI ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X + ope2.X);
			dst.Y = (int)(ope1.Y + ope2.Y);
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
		public static TxPointI operator +(TxPointI ope1, TxSizeI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X + ope2.Width);
			dst.Y = (int)(ope1.Y + ope2.Height);
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
		public static TxPointI operator +(TxPointI ope1, int ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X + ope2);
			dst.Y = (int)(ope1.Y + ope2);
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
		public static TxPointI operator +(int ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1 + ope2.X);
			dst.Y = (int)(ope1 + ope2.Y);
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
		public static TxPointI operator +(TxPointI ope1, Point ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X + ope2.X);
			dst.Y = (int)(ope1.Y + ope2.Y);
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
		public static TxPointI operator +(Point ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X + ope2.X);
			dst.Y = (int)(ope1.Y + ope2.Y);
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
		public static TxPointI operator +(TxPointI ope1, PointF ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X + ope2.X);
			dst.Y = (int)(ope1.Y + ope2.Y);
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
		public static TxPointI operator +(PointF ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X + ope2.X);
			dst.Y = (int)(ope1.Y + ope2.Y);
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
		public static TxPointI operator +(TxPointI ope1, Size ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X + ope2.Width);
			dst.Y = (int)(ope1.Y + ope2.Height);
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
		public static TxPointI operator +(Size ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.Width + ope2.X);
			dst.Y = (int)(ope1.Height + ope2.Y);
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
		public static TxPointI operator +(TxPointI ope1, SizeF ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X + ope2.Width);
			dst.Y = (int)(ope1.Y + ope2.Height);
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
		public static TxPointI operator +(SizeF ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.Width + ope2.X);
			dst.Y = (int)(ope1.Height + ope2.Y);
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
		public static TxPointI operator -(TxPointI ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X - ope2.X);
			dst.Y = (int)(ope1.Y - ope2.Y);
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
		public static TxPointI operator -(TxPointI ope1, TxSizeI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X - ope2.Width);
			dst.Y = (int)(ope1.Y - ope2.Height);
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
		public static TxPointI operator -(TxPointI ope1, int ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X - ope2);
			dst.Y = (int)(ope1.Y - ope2);
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
		public static TxPointI operator -(int ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1 - ope2.X);
			dst.Y = (int)(ope1 - ope2.Y);
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
		public static TxPointI operator -(TxPointI ope1, Point ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X - ope2.X);
			dst.Y = (int)(ope1.Y - ope2.Y);
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
		public static TxPointI operator -(Point ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X - ope2.X);
			dst.Y = (int)(ope1.Y - ope2.Y);
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
		public static TxPointI operator -(TxPointI ope1, PointF ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X - ope2.X);
			dst.Y = (int)(ope1.Y - ope2.Y);
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
		public static TxPointI operator -(PointF ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X - ope2.X);
			dst.Y = (int)(ope1.Y - ope2.Y);
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
		public static TxPointI operator -(TxPointI ope1, Size ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X - ope2.Width);
			dst.Y = (int)(ope1.Y - ope2.Height);
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
		public static TxPointI operator -(Size ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.Width - ope2.X);
			dst.Y = (int)(ope1.Height - ope2.Y);
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
		public static TxPointI operator -(TxPointI ope1, SizeF ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X - ope2.Width);
			dst.Y = (int)(ope1.Y - ope2.Height);
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
		public static TxPointI operator -(SizeF ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.Width - ope2.X);
			dst.Y = (int)(ope1.Height - ope2.Y);
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
		public static TxPointI operator *(TxPointI ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X * ope2.X);
			dst.Y = (int)(ope1.Y * ope2.Y);
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
		public static TxPointI operator *(TxPointI ope1, TxSizeI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X * ope2.Width);
			dst.Y = (int)(ope1.Y * ope2.Height);
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
		public static TxPointI operator *(TxPointI ope1, int ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X * ope2);
			dst.Y = (int)(ope1.Y * ope2);
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
		public static TxPointI operator *(int ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1 * ope2.X);
			dst.Y = (int)(ope1 * ope2.Y);
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
		public static TxPointI operator *(TxPointI ope1, Point ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X * ope2.X);
			dst.Y = (int)(ope1.Y * ope2.Y);
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
		public static TxPointI operator *(Point ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X * ope2.X);
			dst.Y = (int)(ope1.Y * ope2.Y);
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
		public static TxPointI operator *(TxPointI ope1, PointF ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X * ope2.X);
			dst.Y = (int)(ope1.Y * ope2.Y);
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
		public static TxPointI operator *(PointF ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X * ope2.X);
			dst.Y = (int)(ope1.Y * ope2.Y);
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
		public static TxPointI operator *(TxPointI ope1, Size ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X * ope2.Width);
			dst.Y = (int)(ope1.Y * ope2.Height);
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
		public static TxPointI operator *(Size ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.Width * ope2.X);
			dst.Y = (int)(ope1.Height * ope2.Y);
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
		public static TxPointI operator *(TxPointI ope1, SizeF ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X * ope2.Width);
			dst.Y = (int)(ope1.Y * ope2.Height);
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
		public static TxPointI operator *(SizeF ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.Width * ope2.X);
			dst.Y = (int)(ope1.Height * ope2.Y);
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
		public static TxPointI operator /(TxPointI ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X / ope2.X);
			dst.Y = (int)(ope1.Y / ope2.Y);
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
		public static TxPointI operator /(TxPointI ope1, TxSizeI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X / ope2.Width);
			dst.Y = (int)(ope1.Y / ope2.Height);
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
		public static TxPointI operator /(TxPointI ope1, int ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X / ope2);
			dst.Y = (int)(ope1.Y / ope2);
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
		public static TxPointI operator /(int ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1 / ope2.X);
			dst.Y = (int)(ope1 / ope2.Y);
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
		public static TxPointI operator /(TxPointI ope1, Point ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X / ope2.X);
			dst.Y = (int)(ope1.Y / ope2.Y);
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
		public static TxPointI operator /(Point ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X / ope2.X);
			dst.Y = (int)(ope1.Y / ope2.Y);
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
		public static TxPointI operator /(TxPointI ope1, PointF ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X / ope2.X);
			dst.Y = (int)(ope1.Y / ope2.Y);
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
		public static TxPointI operator /(PointF ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X / ope2.X);
			dst.Y = (int)(ope1.Y / ope2.Y);
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
		public static TxPointI operator /(TxPointI ope1, Size ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X / ope2.Width);
			dst.Y = (int)(ope1.Y / ope2.Height);
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
		public static TxPointI operator /(Size ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.Width / ope2.X);
			dst.Y = (int)(ope1.Height / ope2.Y);
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
		public static TxPointI operator /(TxPointI ope1, SizeF ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.X / ope2.Width);
			dst.Y = (int)(ope1.Y / ope2.Height);
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
		public static TxPointI operator /(SizeF ope1, TxPointI ope2)
		{
			TxPointI dst = new TxPointI();
			dst.X = (int)(ope1.Width / ope2.X);
			dst.Y = (int)(ope1.Height / ope2.Y);
			return dst;
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxPointD ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxPointD(TxPointI src)
		{
			return new TxPointD(src.X, src.Y);
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
		public static implicit operator Point(TxPointI src)
		{
			return new Point(src.X, src.Y);
		}

		/// <summary>
		/// 暗黙的な型変換 (PointF ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator PointF(TxPointI src)
		{
			return new PointF(src.X, src.Y);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← Point)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxPointI(Point src)
		{
			return new TxPointI(src.X, src.Y);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← PointF)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxPointI(PointF src)
		{
			return new TxPointI(
				(int)System.Math.Round(src.X),
				(int)System.Math.Round(src.Y)
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
				if (destinationType == typeof(TxPointI)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxPointI)
				{
					var _value = (TxPointI)value;
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
					var dst = new TxPointI();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.X = Convert.ToInt32(values[i]); break;
							case 1: dst.Y = Convert.ToInt32(values[i]); break;
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
