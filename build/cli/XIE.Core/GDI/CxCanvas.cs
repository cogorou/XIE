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
	/// 画像描画クラス
	/// </summary>
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxCanvas : System.Object
		, IDisposable
		, ICloneable
		, IxEquatable
	{
		#region フィールド:

		/// <summary>
		/// 描画先のビットマップ
		/// </summary>
		[NonSerialized]
		private Bitmap m_Bitmap;

		/// <summary>
		/// 描画先のグラフィクス
		/// </summary>
		[NonSerialized]
		private Graphics m_Graphics;

		/// <summary>
		/// アタッチ状態
		/// </summary>
		[NonSerialized]
		private bool m_IsAttached;

		/// <summary>
		/// 幅 [範囲:1~]
		/// </summary>
		[NonSerialized]
		private int m_Width;

		/// <summary>
		/// 高さ [範囲:1~]
		/// </summary>
		[NonSerialized]
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
			m_Bitmap = null;
			m_Graphics = null;
			m_IsAttached = false;
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
		public CxCanvas()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ (アタッチ)
		/// </summary>
		/// <param name="graphics">描画先</param>
		/// <param name="width">描画先の幅 [1~]</param>
		/// <param name="height">描画先の高さ [1~]</param>
		public CxCanvas(Graphics graphics, int width, int height)
		{
			_Constructor();
			Attach(graphics, width, height);
		}

		/// <summary>
		/// コンストラクタ (アタッチ)
		/// </summary>
		/// <param name="graphics">描画先</param>
		/// <param name="canvas_info">キャンバス情報</param>
		public CxCanvas(Graphics graphics, CxCanvasInfo canvas_info)
		{
			_Constructor();
			Attach(graphics, canvas_info);
		}

		/// <summary>
		/// コンストラクタ (領域確保)
		/// </summary>
		/// <param name="width">幅 [1~]</param>
		/// <param name="height">高さ [1~]</param>
		public CxCanvas(int width, int height)
		{
			_Constructor();
			Resize(width, height);
		}

		/// <summary>
		/// コンストラクタ (領域確保)
		/// </summary>
		/// <param name="size">幅と高さ [1~]</param>
		public CxCanvas(TxSizeI size)
		{
			_Constructor();
			Resize(size);
		}

		/// <summary>
		/// コンストラクタ (領域確保)
		/// </summary>
		/// <param name="canvas_info">キャンバス情報</param>
		public CxCanvas(CxCanvasInfo canvas_info)
		{
			_Constructor();
			Resize(canvas_info);
		}

		#endregion

		#region IDisposable の実装: (解放)

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			if (m_IsAttached == false)
			{
				if (m_Graphics != null)
					m_Graphics.Dispose();
			}
			m_Graphics = null;

			if (m_Bitmap != null)
				m_Bitmap.Dispose();
			m_Bitmap = null;
			
			m_Width = 0;
			m_Height = 0;
		}

		/// <summary>
		/// 有効性 [初期値:false]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		public virtual bool IsValid
		{
			get
			{
				if (m_Graphics == null) return false;
				if (m_Width <= 0) return false;
				if (m_Height <= 0) return false;
				return true;
			}
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
			var clone = new CxCanvas();
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
			if (src is CxCanvas)
			{
				var _dst = this;
				var _src = (CxCanvas)src;
				if (_dst.Width != _src.Width ||
					_dst.Height != _src.Height)
				{
					_dst.Resize(_src.Width, _src.Height);
				}
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
			return (ReferenceEquals(src, this));
		}

		#endregion

		#region メソッド: (アタッチ)

		/// <summary>
		/// アタッチ
		/// </summary>
		/// <param name="graphics">描画先</param>
		/// <param name="width">描画先の幅 [1~]</param>
		/// <param name="height">描画先の高さ [1~]</param>
		public virtual void Attach(Graphics graphics, int width, int height)
		{
			this.Dispose();
			if (graphics != null)
			{
				this.m_Graphics = graphics;
				this.m_IsAttached = true;
			}
			this.m_Width = width;
			this.m_Height = height;
		}

		/// <summary>
		/// アタッチ
		/// </summary>
		/// <param name="graphics">描画先のグラフィクス</param>
		/// <param name="canvas_info">キャンバス情報</param>
		public virtual void Attach(Graphics graphics, CxCanvasInfo canvas_info)
		{
			this.Dispose();
			if (graphics != null)
			{
				this.m_Graphics = graphics;
				this.m_IsAttached = true;
			}
			this.m_Width = canvas_info.Width;
			this.m_Height = canvas_info.Height;

			this.ImageSize = canvas_info.ImageSize;
			this.BackgroundBrush = canvas_info.BackgroundBrush;
			this.Magnification = canvas_info.Magnification;
			this.ViewPoint = canvas_info.ViewPoint;
			this.ChannelNo = canvas_info.ChannelNo;
			this.Unpack = canvas_info.Unpack;
			this.Halftone = canvas_info.Halftone;
		}

		#endregion

		#region メソッド: (領域確保)

		/// <summary>
		/// 描画領域の確保
		/// </summary>
		/// <param name="width">幅 [1~]</param>
		/// <param name="height">高さ [1~]</param>
		public virtual void Resize(int width, int height)
		{
			this.Dispose();

			if (width > 0 && height > 0)
			{

				this.m_Bitmap = new Bitmap(width, height);
				this.m_Graphics = Graphics.FromImage(this.m_Bitmap);
				this.m_Width = width;
				this.m_Height = height;
			}
		}

		/// <summary>
		/// 描画領域の確保
		/// </summary>
		/// <param name="size">幅と高さ [1~]</param>
		public virtual void Resize(TxSizeI size)
		{
			this.Resize(size.Width, size.Height);
		}

		/// <summary>
		/// 描画領域の確保
		/// </summary>
		/// <param name="canvas_info">キャンバス情報</param>
		public virtual void Resize(CxCanvasInfo canvas_info)
		{
			this.Resize(canvas_info.Width, canvas_info.Height);

			this.ImageSize = canvas_info.ImageSize;
			this.BackgroundBrush = canvas_info.BackgroundBrush;
			this.Magnification = canvas_info.Magnification;
			this.ViewPoint = canvas_info.ViewPoint;
			this.ChannelNo = canvas_info.ChannelNo;
			this.Unpack = canvas_info.Unpack;
			this.Halftone = canvas_info.Halftone;
		}

		#endregion

		#region メソッド: (描画)

		/// <summary>
		/// 背景を指定色で塗り潰します。
		/// </summary>
		public virtual void Clear()
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);

			// 背景描画:
			using (var brush = this.BackgroundBrush.ToBrush(this.DisplayRect()))
			{
				this.Graphics.FillRectangle(brush, this.DisplayRect());
			}
		}

		/// <summary>
		/// 背景画像を描画します。
		/// </summary>
		/// <param name="image">背景画像</param>
		public virtual void DrawImage(object image)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);

			#region 背景の塗り潰し:
			if (this.BackgroundBrush.Style != ExGdiBrushStyle.None)
			{
				Clear();
			}
			#endregion

			#region 表示対象画像の描画:
			switch (System.Environment.OSVersion.Platform)
			{
				case PlatformID.Unix:
					// Linux では CLI を使用する.
					{
						#region use CLI
						try
						{
							using (var src = new CxImage())
							using (var buf = new CxImage())
							{
								if (image is CxImage)
									src.Attach(image);
								else
									src.CopyFrom(image);

								if (src.IsValid)
								{
									#region 濃度補間:
									if (this.Halftone)
									{
										this.Graphics.SmoothingMode = SmoothingMode.HighQuality;
										if (this.Magnification < 1.0)
											this.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;	// HighQualityBilinear/HighQualityBicubic
										else
											this.Graphics.InterpolationMode = InterpolationMode.Bilinear;	// Bilinear/Bicubic
									}
									else
									{
										this.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
										this.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
									}
									#endregion

									#region 切り出し:
									{
										var hdst = ((IxModule)buf).GetHandle();
										var hsrc = ((IxModule)src).GetHandle();
										var info = this.ToTxCanvas();
										xie_high.fnXIE_GDI_Canvas_Api_Extract(hdst, hsrc, info);
									}
									#endregion

									#region 描画:
									if (this.Magnification <= 1.0)
									{
										//
										// 倍率が 1.0 未満の時、既に縮小処理が行われています.
										// 
										var mag = this.Magnification;
										var eff = EffectiveRect();

										using (var bitmap = buf.ToBitmap())
										{
											this.Graphics.DrawImage(bitmap, eff.X, eff.Y);
										}
									}
									else
									{
										//
										// 倍率が 1.0 以上の時、可視範囲に倍率を加味ます.
										//

										var mag = this.Magnification;
										var eff = EffectiveRect();
										var vis = VisibleRect();
										var clip = VisibleRectI(true);
										using (var bitmap = buf.ToBitmap())
										{
											var gs = this.Graphics.Save();
											try
											{
												var tx = (float)((clip.X - vis.X) * mag);
												var ty = (float)((clip.Y - vis.Y) * mag);

												this.Graphics.Transform.Reset();
												this.Graphics.TranslateTransform(tx, ty);
												this.Graphics.ScaleTransform((float)mag, (float)mag);

												this.Graphics.DrawImageUnscaled(bitmap, eff.X, eff.Y);
											}
											finally
											{
												this.Graphics.Restore(gs);
											}
										}
									}
									#endregion
								}
							}
						}
						catch (System.Exception)
						{
						}
						#endregion
					}
					break;
				default:
					// Windows では Native の GDI を使用する.
					{
						#region use Native
						try
						{
							var hdc = this.Graphics.GetHdc();
							if (xie_high.fnXIE_GDI_Graphics_CheckValidity(hdc) == ExBoolean.True)
							{
								using (var src = new CxImage())
								{
									if (image is CxImage)
										src.Attach(image);
									else
										src.CopyFrom(image);
									this.ImageSize = src.Size;

									var hsrc = ((IxModule)src).GetHandle();
									var tag = ToTxCanvas();

									var watch = new CxStopwatch();
									watch.Start();
									ExStatus status = xie_high.fnXIE_GDI_Graphics_DrawImage(hdc, hsrc, tag);
									watch.Stop();
									//XIE.Log.Api.Trace("fnXIE_GDI_Graphics_DrawImage: {0} {1:F3} msec", status, watch.Lap);
								}
								return;
							}
						}
						finally
						{
							this.Graphics.ReleaseHdc();
						}
						#endregion
					}
					break;
			}
			#endregion
		}

		/// <summary>
		/// 図形を描画します。
		/// </summary>
		/// <param name="figure">描画対象の図形</param>
		/// <param name="mode">伸縮モード</param>
		public virtual void DrawOverlay(object figure, ExGdiScalingMode mode)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);

			if (figure is IxGdi2dRendering)
			{
				var figures = new IxGdi2dRendering[] { (IxGdi2dRendering)figure };
				DrawOverlay(figures, mode);
			}
		}

		/// <summary>
		/// 図形を描画します。
		/// </summary>
		/// <param name="figures">描画対象の図形</param>
		/// <param name="mode">伸縮モード</param>
		public virtual void DrawOverlay(IEnumerable figures, ExGdiScalingMode mode)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);

			var gs = this.Graphics.Save();
			try
			{
				var graphics = this.Graphics;
				var info = this.GetCanvasInfo();
				var mag = this.Magnification;
				var eff = this.EffectiveRect();
				var vis = this.VisibleRect();

				switch (mode)
				{
					default:
					case ExGdiScalingMode.None:
						break;
					case ExGdiScalingMode.TopLeft:
						{
							var tx = (float)(eff.X - (vis.X * mag));
							var ty = (float)(eff.Y - (vis.Y * mag));
							graphics.TranslateTransform(tx, ty);
						}
						break;
					case ExGdiScalingMode.Center:
						{
							var tx = (float)(eff.X - ((vis.X - 0.5) * mag));
							var ty = (float)(eff.Y - ((vis.Y - 0.5) * mag));
							graphics.TranslateTransform(tx, ty);
						}
						break;
				}

				foreach (var item in figures)
				{
					if (item is IxGdi2dRendering)
					{
						var figure = (IxGdi2dRendering)item;
						figure.Render(graphics, info, mode);
					}
				}
			}
			finally
			{
				this.Graphics.Restore(gs);
			}
		}

		#endregion

		#region メソッド: (表示イメージの取得)

		/// <summary>
		/// 表示イメージの取得
		/// </summary>
		/// <returns>
		///		描画結果を保存した画像オブジェクトを返します。
		/// </returns>
		public virtual CxImage Snapshot()
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (this.m_Bitmap == null)
				throw new CxException(ExStatus.InvalidObject);

			var dst = new CxImage();
			dst.CopyFrom(this.m_Bitmap);
			return dst;
		}

		#endregion

		#region プロパティ: (Information)

		/// <summary>
		/// 描画先のグラフィクス
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvas.Graphics")]
		public virtual Graphics Graphics
		{
			get { return m_Graphics; }
		}

		/// <summary>
		/// アタッチ状態
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvas.IsAttached")]
		public virtual bool IsAttached
		{
			get { return m_IsAttached; }
		}

		/// <summary>
		/// 幅と高さ (pixels) [範囲:0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvas.Size")]
		public virtual TxSizeI Size
		{
			get
			{
				return new TxSizeI(this.m_Width, this.m_Height);
			}
		}

		/// <summary>
		/// 幅 (pixels) [範囲:0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvas.Width")]
		public virtual int Width
		{
			get { return this.m_Width; }
		}

		/// <summary>
		/// 高さ (pixels) [範囲:0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxCanvas.Height")]
		public virtual int Height
		{
			get { return this.m_Height; }
		}

		#endregion

		#region プロパティ: (Parameter)

		/// <summary>
		/// 描画対象画像サイズ (pixels)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxCanvas.ImageSize")]
		public virtual TxSizeI ImageSize
		{
			get { return m_ImageSize; }
			set { m_ImageSize = value; }
		}

		/// <summary>
		/// 背景塗り潰し用ブラシ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxCanvas.BackgroundBrush")]
		public TxGdiBrush BackgroundBrush
		{
			get { return m_Background; }
			set { m_Background = value; }
		}

		/// <summary>
		/// 表示倍率 [範囲:0.0 より大きい値]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxCanvas.Magnification")]
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
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxCanvas.ViewPoint")]
		public virtual TxPointD ViewPoint
		{
			get { return m_ViewPoint; }
			set { m_ViewPoint = value; }
		}

		/// <summary>
		/// 表示対象チャネル指標 [範囲:0~3]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxCanvas.ChannelNo")]
		public virtual int ChannelNo
		{
			get { return m_ChannelNo; }
			set { m_ChannelNo = value; }
		}

		/// <summary>
		/// アンパック表示の指示
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxCanvas.Unpack")]
		public virtual bool Unpack
		{
			get { return m_Unpack; }
			set { m_Unpack = value; }
		}

		/// <summary>
		/// ハーフトーン
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxCanvas.Halftone")]
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
			var	display_rect = this.DisplayRect();
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
			var	display_rect = this.DisplayRect();
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

		#region メソッド: (キャンバス情報の取得と設定)

		/// <summary>
		/// キャンバス情報の取得
		/// </summary>
		/// <returns>
		///		現在の情報を CxCanvasInfo オブジェクトに設定して返します。
		/// </returns>
		public virtual CxCanvasInfo GetCanvasInfo()
		{
			var args = new CxCanvasInfo();
			args.Width = this.Width;
			args.Height = this.Height;
			args.ImageSize = this.ImageSize;
			args.BackgroundBrush = this.BackgroundBrush;
			args.Magnification = this.Magnification;
			args.ViewPoint = this.ViewPoint;
			args.ChannelNo = this.ChannelNo;
			args.Unpack = this.Unpack;
			args.Halftone = this.Halftone;
			return args;
		}

		/// <summary>
		/// キャンバス構造体への変換
		/// </summary>
		/// <returns>
		///		現在の情報を TxCanvas 構造体に設定して返します。
		/// </returns>
		public virtual TxCanvas ToTxCanvas()
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
			return CxCanvasInfo.EffectiveRect(display_rect, image_size, mag);
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
			return CxCanvasInfo.VisibleRect(display_rect, image_size, mag, view_point);
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
			return CxCanvasInfo.VisibleRectI(display_rect, image_size, mag, view_point, includePartialPixel);
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
			return CxCanvasInfo.DPtoIP(display_rect, image_size, mag, view_point, dp, mode);
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
			return CxCanvasInfo.IPtoDP(display_rect, image_size, mag, view_point, ip, mode);
		}

		#endregion
	}
}
