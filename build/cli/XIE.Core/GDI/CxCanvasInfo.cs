/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Text;
using System.Reflection;
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
	/// キャンバス情報クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class CxCanvasInfo : System.Object
		, ICloneable
		, IxEquatable
	{
		#region フィールド:

		/// <summary>
		/// 幅 [範囲:1~]
		/// </summary>
		private int m_Width;

		/// <summary>
		/// 高さ [範囲:1~]
		/// </summary>
		private int m_Height;

		/// <summary>
		/// 描画対象画像サイズ (pixels)
		/// </summary>
		private TxSizeI m_ImageSize;

		/// <summary>
		/// 背景塗り潰し用ブラシ
		/// </summary>
		private TxGdiBrush m_Background;

		/// <summary>
		/// 表示倍率 [範囲:0.0 より大きい値]
		/// </summary>
		private double m_Magnification;

		/// <summary>
		/// 視点 (pixels)
		/// </summary>
		private TxPointD m_ViewPoint;

		/// <summary>
		/// 表示対象チャネル指標 [範囲:0~3]
		/// </summary>
		private int m_ChannelNo;

		/// <summary>
		/// アンパック表示の指示
		/// </summary>
		private bool m_Unpack;

		/// <summary>
		/// ハーフトーン
		/// </summary>
		private bool m_Halftone;

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			m_Width = 0;
			m_Height = 0;
			m_ImageSize = new TxSizeI();
			m_Background = new TxGdiBrush(new TxRGB8x4(0x00, 0x00, 0x00));
			m_Magnification = 1;
			m_ViewPoint = new TxPointD();
			m_ChannelNo = 0;
			m_Unpack = false;
			m_Halftone = false;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxCanvasInfo()
		{
			_Constructor();
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// オブジェクトのクローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			var clone = new CxCanvasInfo();
			clone.CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;
			if (ReferenceEquals(src, null)) return;
			if (src is CxCanvasInfo)
			{
				var _dst = this;
				var _src = (CxCanvasInfo)src;
				_dst.Width = _src.Width;
				_dst.Height = _src.Height;
				_dst.ImageSize = _src.ImageSize;
				_dst.BackgroundBrush = _src.BackgroundBrush;
				_dst.Magnification = _src.Magnification;
				_dst.ViewPoint = _src.ViewPoint;
				_dst.ChannelNo = _src.ChannelNo;
				_dst.Unpack = _src.Unpack;
				_dst.Halftone = _src.Halftone;
				return;
			}
			if (src is IxConvertible)
			{
				((IxConvertible)src).CopyTo(this);
				return;
			}
			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public virtual bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;
			if (ReferenceEquals(src, null)) return false;
			if (src is CxCanvasInfo)
			{
				var _dst = this;
				var _src = (CxCanvasInfo)src;
				if (_dst.Width != _src.Width) return false;
				if (_dst.Height != _src.Height) return false;
				if (_dst.ImageSize != _src.ImageSize) return false;
				if (_dst.BackgroundBrush != _src.BackgroundBrush) return false;
				if (_dst.Magnification != _src.Magnification) return false;
				if (_dst.ViewPoint != _src.ViewPoint) return false;
				if (_dst.ChannelNo != _src.ChannelNo) return false;
				if (_dst.Unpack != _src.Unpack) return false;
				if (_dst.Halftone != _src.Halftone) return false;
				return true;
			}
			return false;
		}

		#endregion

		#region プロパティ: (Information)

		/// <summary>
		/// 幅 (pixels) [範囲:0~]
		/// </summary>
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvasInfo.Width")]
		public virtual int Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}

		/// <summary>
		/// 高さ (pixels) [範囲:0~]
		/// </summary>
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvasInfo.Height")]
		public virtual int Height
		{
			get { return m_Height; }
			set { m_Height = value; }
		}

		/// <summary>
		/// 描画対象画像サイズ (pixels)
		/// </summary>
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvasInfo.ImageSize")]
		public virtual TxSizeI ImageSize
		{
			get { return m_ImageSize; }
			set { m_ImageSize = value; }
		}

		/// <summary>
		/// 背景塗り潰し用ブラシ
		/// </summary>
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvasInfo.BackgroundBrush")]
		public TxGdiBrush BackgroundBrush
		{
			get { return m_Background; }
			set { m_Background = value; }
		}

		/// <summary>
		/// 表示倍率 [範囲:0.0 より大きい値]
		/// </summary>
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvasInfo.Magnification")]
		public virtual double Magnification
		{
			get { return m_Magnification; }
			set
			{
				if (value <= 0)
					return;
				m_Magnification = value;
			}
		}

		/// <summary>
		/// 視点 (pixels)
		/// </summary>
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvasInfo.ViewPoint")]
		public virtual TxPointD ViewPoint
		{
			get { return m_ViewPoint; }
			set { m_ViewPoint = value; }
		}

		/// <summary>
		/// 表示対象チャネル指標 [範囲:0~3]
		/// </summary>
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvasInfo.ChannelNo")]
		public virtual int ChannelNo
		{
			get { return m_ChannelNo; }
			set { m_ChannelNo = value; }
		}

		/// <summary>
		/// アンパック表示の指示
		/// </summary>
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvasInfo.Unpack")]
		public virtual bool Unpack
		{
			get { return m_Unpack; }
			set { m_Unpack = value; }
		}

		/// <summary>
		/// ハーフトーン
		/// </summary>
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvasInfo.Halftone")]
		public virtual bool Halftone
		{
			get { return m_Halftone; }
			set { m_Halftone = value; }
		}

		#endregion

		#region メソッド: (Information)

		/// <summary>
		/// 表示領域のサイズを計算します。
		/// </summary>
		/// <returns>
		///		表示領域のサイズを返します。
		///		計算できない場合はサイズ 0,0 を返します。
		/// </returns>
		public virtual TxSizeI DisplaySize()
		{
			return new TxSizeI(this.Width, this.Height);
		}

		/// <summary>
		/// 表示領域の範囲を計算します。
		/// </summary>
		/// <returns>
		///		表示領域の範囲を返します。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </returns>
		public virtual TxRectangleI DisplayRect()
		{
			return new TxRectangleI(0, 0, this.Width, this.Height);
		}

		/// <summary>
		/// 有効範囲を計算します。
		/// </summary>
		/// <returns>
		///		画像が表示される範囲を返します。
		///		表示倍率を乗算した画像サイズが表示範囲より小さい場合はセンタリングされます。
		///		それ以外は、表示範囲と同一です。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </returns>
		public virtual TxRectangleI EffectiveRect()
		{
			var display_rect = this.DisplayRect();
			var image_size = this.ImageSize;
			var mag = this.Magnification;

			return EffectiveRect(display_rect, image_size, mag);
		}

		/// <summary>
		/// 可視範囲を計算します。
		/// </summary>
		/// <returns>
		///		指定された範囲内に表示される画像の範囲を返します。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </returns>
		public virtual TxRectangleD VisibleRect()
		{
			var display_rect = this.DisplayRect();
			var image_size = this.ImageSize;
			var mag = this.Magnification;
			var view_point = this.ViewPoint;

			return VisibleRect(display_rect, image_size, mag, view_point);
		}

		/// <summary>
		/// 可視範囲を計算します。
		/// </summary>
		/// <param name="includePartialPixel">画面淵の端数画素を含むか否か [true=切り上げ、false=切り捨て]</param>
		/// <returns>
		///		指定された範囲内に表示される画像の範囲を返します。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </returns>
		public virtual TxRectangleI VisibleRectI(bool includePartialPixel)
		{
			var display_rect = this.DisplayRect();
			var image_size = this.ImageSize;
			var mag = this.Magnification;
			var view_point = this.ViewPoint;

			return VisibleRectI(display_rect, image_size, mag, view_point, includePartialPixel);
		}

		/// <summary>
		/// ディスプレイ座標を画像座標に変換します。
		/// </summary>
		/// <param name="dp">ディスプレイ座標</param>
		/// <param name="mode">スケーリングモード</param>
		/// <returns>
		///		変換後の座標を返します。
		///		計算できない場合は 0,0 を返します。
		/// </returns>
		public virtual TxPointD DPtoIP(TxPointD dp, ExGdiScalingMode mode)
		{
			return DPtoIP(DisplayRect(), ImageSize, Magnification, ViewPoint, dp, mode);
		}

		/// <summary>
		/// 画像座標をディスプレイ座標に変換します。
		/// </summary>
		/// <param name="ip">画像座標</param>
		/// <param name="mode">スケーリングモード</param>
		/// <returns>
		///		変換後の座標を返します。
		///		計算できない場合は 0,0 を返します。
		/// </returns>
		public virtual TxPointD IPtoDP(TxPointD ip, ExGdiScalingMode mode)
		{
			return IPtoDP(DisplayRect(), ImageSize, Magnification, ViewPoint, ip, mode);
		}

		#endregion

		#region メソッド: (キャンバス情報の取得)

		/// <summary>
		/// キャンバス構造体への変換
		/// </summary>
		/// <returns>
		///		現在の情報を TxCanvas 構造体に設定して返します。
		/// </returns>
		public TxCanvas ToTxCanvas()
		{
			var tag = new TxCanvas();
			tag.Width = this.Width;
			tag.Height = this.Height;
			tag.BgSize = this.ImageSize;
			tag.BkColor = this.BackgroundBrush.Color;
			tag.BkEnable = (this.BackgroundBrush.Style != ExGdiBrushStyle.None) ? ExBoolean.True : ExBoolean.False;
			tag.Magnification = this.Magnification;
			tag.ViewPoint = this.ViewPoint;
			tag.ChannelNo = this.ChannelNo;
			tag.Unpack = this.Unpack ? ExBoolean.True : ExBoolean.False;
			tag.Halftone = this.Halftone ? ExBoolean.True : ExBoolean.False;
			return tag;
		}

		#endregion

		#region スタティック関数:

		/// <summary>
		/// 有効範囲を計算します。
		/// </summary>
		/// <param name="display_rect">表示範囲</param>
		/// <param name="image_size">描画対象画像サイズ</param>
		/// <param name="mag">表示倍率 [0.0 より大きい値] ※ 1.0 を等倍とします。</param>
		/// <returns>
		///		画像が表示される範囲を返します。
		///		表示倍率を乗算した画像サイズが表示範囲より小さい場合はセンタリングされます。
		///		それ以外は、表示範囲と同一です。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </returns>
		public static TxRectangleD EffectiveRect(TxRectangleI display_rect, TxSizeI image_size, double mag)
		{
			if (mag <= 0)
				return new TxRectangleI();

			var result = new TxRectangleI(display_rect.X, display_rect.Y, display_rect.Width, display_rect.Height);

			int xoi = (int)(image_size.Width * mag);		// X on image
			int yoi = (int)(image_size.Height * mag);		// Y on image

			int xod = (int)(display_rect.Width);		// X on display
			int yod = (int)(display_rect.Height);		// Y on display

			// HORZ
			if (xoi < xod)
			{
				result.X = (int)(display_rect.X + (int)System.Math.Round((xod - xoi) / 2.0));
				result.Width = (int)xoi;
			}
			// VERT
			if (yoi < yod)
			{
				result.Y = (int)(display_rect.Y + (int)System.Math.Round((yod - yoi) / 2.0));
				result.Height = (int)yoi;
			}

			return result;
		}

		/// <summary>
		/// 可視範囲を計算します。
		/// </summary>
		/// <param name="display_rect">表示範囲</param>
		/// <param name="image_size">描画対象画像サイズ</param>
		/// <param name="mag">表示倍率 [0.0 より大きい値] ※ 1.0 を等倍とします。</param>
		/// <param name="view_point">視点 (画像座標)</param>
		/// <returns>
		///		指定された範囲内に表示される画像の範囲を返します。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </returns>
		public static TxRectangleD VisibleRect(TxRectangleI display_rect, TxSizeI image_size, double mag, TxPointD view_point)
		{
			if (mag <= 0)
				return new TxRectangleD();

			var result = new TxRectangleD(0, 0, image_size.Width, image_size.Height);

			double xod = display_rect.Width / mag;	// X on display
			double yod = display_rect.Height / mag;	// Y on display

			// HORZ
			if (xod < image_size.Width)
			{
				double sx = (view_point.X - xod / 2.0);
				if (sx < 0)
					sx = 0;

				double ex = (sx + xod);
				if (ex > image_size.Width)
				{
					ex = image_size.Width;
					sx = image_size.Width - xod;
				}

				result.X = sx;
				result.Width = (ex - sx);
			}
			// VERT
			if (yod < image_size.Height)
			{
				double sy = (view_point.Y - yod / 2.0);
				if (sy < 0)
					sy = 0;

				double ey = (sy + yod);
				if (ey > image_size.Height)
				{
					ey = image_size.Height;
					sy = image_size.Height - yod;
				}

				result.Y = sy;
				result.Height = (ey - sy);
			}

			return result;
		}

		/// <summary>
		/// 可視範囲を計算します。
		/// </summary>
		/// <param name="display_rect">表示範囲</param>
		/// <param name="image_size">描画対象画像サイズ</param>
		/// <param name="mag">表示倍率 [0.0 より大きい値] ※ 1.0 を等倍とします。</param>
		/// <param name="view_point">視点 (画像座標)</param>
		/// <param name="includePartialPixel">画面淵の端数画素を含むか否か [true=切り上げ、false=切り捨て]</param>
		/// <returns>
		///		指定された範囲内に表示される画像の範囲を返します。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </returns>
		public static TxRectangleD VisibleRectI(TxRectangleI display_rect, TxSizeI image_size, double mag, TxPointD view_point, bool includePartialPixel)
		{
			var src = VisibleRect(display_rect, image_size, mag, view_point);

			int sx, sy, ex, ey;
			if (includePartialPixel)
			{
				sx = (int)System.Math.Floor(src.X);
				sy = (int)System.Math.Floor(src.Y);
				ex = (int)System.Math.Ceiling(src.X + src.Width);
				ey = (int)System.Math.Ceiling(src.Y + src.Height);
			}
			else
			{
				sx = (int)System.Math.Ceiling(src.X);
				sy = (int)System.Math.Ceiling(src.Y);
				ex = (int)System.Math.Floor(src.X + src.Width);
				ey = (int)System.Math.Floor(src.Y + src.Height);
			}

			if (ex > image_size.Width)
				ex = image_size.Width;
			if (ey > image_size.Height)
				ey = image_size.Height;

			var dst = new TxRectangleI(sx, sy, (ex - sx), (ey - sy));
			return dst;
		}

		/// <summary>
		/// ディスプレイ座標を画像座標に変換します。
		/// </summary>
		/// <param name="display_rect">表示範囲</param>
		/// <param name="image_size">描画対象画像サイズ</param>
		/// <param name="mag">表示倍率 [0.0 より大きい値] ※ 1.0 を等倍とします。</param>
		/// <param name="view_point">視点 (画像座標)</param>
		/// <param name="dp">ディスプレイ座標</param>
		/// <param name="mode">スケーリングモード</param>
		/// <returns>
		///		変換後の座標を返します。
		///		計算できない場合は 0,0 を返します。
		/// </returns>
		public static TxPointD DPtoIP(TxRectangleI display_rect, TxSizeI image_size, double mag, TxPointD view_point, TxPointD dp, XIE.GDI.ExGdiScalingMode mode)
		{
			if (mag <= 0)
				return new TxPointD();

			TxRectangleD eff = EffectiveRect(display_rect, image_size, mag);
			TxRectangleD vis = VisibleRect(display_rect, image_size, mag, view_point);

			var ip = new TxPointD();
			switch (mode)
			{
				default:
				case ExGdiScalingMode.TopLeft:
					ip.X = (dp.X - eff.X) / mag + vis.X;
					ip.Y = (dp.Y - eff.Y) / mag + vis.Y;
					break;
				case ExGdiScalingMode.Center:
					ip.X = (dp.X - eff.X) / mag + vis.X - 0.5;
					ip.Y = (dp.Y - eff.Y) / mag + vis.Y - 0.5;
					break;
			}
			return ip;
		}

		/// <summary>
		/// 画像座標をディスプレイ座標に変換します。
		/// </summary>
		/// <param name="display_rect">表示範囲</param>
		/// <param name="image_size">描画対象画像サイズ</param>
		/// <param name="mag">表示倍率 [0.0 より大きい値] ※ 1.0 を等倍とします。</param>
		/// <param name="view_point">視点 (画像座標)</param>
		/// <param name="ip">画像座標</param>
		/// <param name="mode">スケーリングモード</param>
		/// <returns>
		///		変換後の座標を返します。
		///		計算できない場合は 0,0 を返します。
		/// </returns>
		public static TxPointD IPtoDP(TxRectangleI display_rect, TxSizeI image_size, double mag, TxPointD view_point, TxPointD ip, XIE.GDI.ExGdiScalingMode mode)
		{
			if (mag <= 0)
				return new TxPointD();

			TxRectangleD eff = EffectiveRect(display_rect, image_size, mag);
			TxRectangleD vis = VisibleRect(display_rect, image_size, mag, view_point);

			var dp = new TxPointD();
			switch (mode)
			{
				default:
				case ExGdiScalingMode.TopLeft:
					dp.X = (ip.X - vis.X) * mag + eff.X;
					dp.Y = (ip.Y - vis.Y) * mag + eff.Y;
					break;
				case ExGdiScalingMode.Center:
					dp.X = (ip.X - vis.X + 0.5) * mag + eff.X;
					dp.Y = (ip.Y - vis.Y + 0.5) * mag + eff.Y;
					break;
			}

			return dp;
		}

		#endregion
	}
}
