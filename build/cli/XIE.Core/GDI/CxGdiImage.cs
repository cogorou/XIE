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
using System.Windows.Forms;

namespace XIE.GDI
{
	/// <summary>
	/// オーバレイ図形クラス (画像)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxGdiImage : System.Object
		, IxGdi2d
		, IDisposable
		, ICloneable
		, IxConvertible
		, IxEquatable
	{
		#region フィールド:

		private double m_Angle;
		private TxPointD m_Axis;

		/// <summary>
		/// ペン
		/// </summary>
		public TxGdiPen Pen;

		/// <summary>
		/// ブラシ
		/// </summary>
		public TxGdiBrush Brush;

		/// <summary>
		/// ビットマップ
		/// </summary>
		public Bitmap Bitmap;

		/// <summary>
		/// X座標
		/// </summary>
		private double m_X;

		/// <summary>
		/// Y座標
		/// </summary>
		private double m_Y;

		/// <summary>
		/// 水平倍率 [範囲:0.0 より大きい値] ※ 1.0 を等倍とします。
		/// </summary>
		private double m_MagX;

		/// <summary>
		/// 垂直倍率 [範囲:0.0 より大きい値] ※ 1.0 を等倍とします。
		/// </summary>
		private double m_MagY;

		/// <summary>
		/// 不透過率 [範囲:0.0=透過、1.0=不透過]
		/// </summary>
		private double m_Alpha;

		/// <summary>
		/// 画素ごとの透過の有効属性
		/// </summary>
		private bool m_AlphaFormat;

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Bitmap = null;
			m_X = 0;
			m_Y = 0;
			m_MagX = 1;
			m_MagY = 1;
			m_Alpha = 1;
			m_AlphaFormat = false;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGdiImage()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="src">複製元</param>
		public CxGdiImage(CxImage src)
		{
			_Constructor();
			CopyFrom(src);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="src">複製元</param>
		public CxGdiImage(Bitmap src)
		{
			_Constructor();
			CopyFrom(src);
		}

		#endregion

		#region IDisposable の実装: (解放)

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			if (Bitmap != null)
				Bitmap.Dispose();
			Bitmap = null;
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
				if (Bitmap == null) return false;
				return true;
			}
		}

		#endregion

		#region ICloneable の実装: (複製)

		/// <summary>
		/// オブジェクトのクローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			var clone = new CxGdiImage();
			clone.CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装: (複製)

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;
			if (ReferenceEquals(src, null)) return;
			if (src is CxGdiImage)
			{
				var _src = (CxGdiImage)src;
				this.Dispose();
				if (_src.Bitmap != null)
					this.Bitmap = (Bitmap)_src.Bitmap.Clone();
				this.Angle = _src.Angle;
				this.Axis = _src.Axis;
				this.Pen = _src.Pen;
				this.Brush = _src.Brush;
				this.X = _src.X;
				this.Y = _src.Y;
				this.MagX = _src.MagX;
				this.MagY = _src.MagY;
				this.Alpha = _src.Alpha;
				this.AlphaFormat = _src.AlphaFormat;
				return;
			}
			if (src is CxImage)
			{
				var _src = (CxImage)src;
				this.Dispose();
				if (_src.IsValid)
					this.Bitmap = _src.ToBitmap();
				return;
			}
			if (src is Bitmap)
			{
				var _src = (Bitmap)src;
				this.Dispose();
				this.Bitmap = (Bitmap)_src.Clone();
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
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			try
			{
				var _src = (CxGdiImage)src;
				if (this.Angle != _src.Angle) return false;
				if (this.Axis != _src.Axis) return false;
				if (this.Pen != _src.Pen) return false;
				if (this.Brush != _src.Brush) return false;
				if (this.X != _src.X) return false;
				if (this.Y != _src.Y) return false;
				if (this.MagX != _src.MagX) return false;
				if (this.MagY != _src.MagY) return false;
				if (this.Alpha != _src.Alpha) return false;
				if (this.AlphaFormat != _src.AlphaFormat) return false;
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region IxConvertible の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="dst">複製先</param>
		public virtual void CopyTo(object dst)
		{
			if (dst is CxImage)
			{
				CxImage _dst = (CxImage)dst;
			}
			else
			{
				throw new CxException(ExStatus.Unsupported);
			}
		}

		/// <summary>
		/// CxImage からの変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator CxGdiImage(CxImage src)
		{
			CxGdiImage dst = new CxGdiImage();
			dst.CopyFrom(src);
			return dst;
		}

		/// <summary>
		/// CxImage への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator CxImage(CxGdiImage src)
		{
			CxImage dst = new CxImage();
			return dst;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// 描画処理:
		//

		#region IxGdi2dRendering の実装: (描画)

		/// <summary>
		/// 図形をグラフィクスに描画します。
		/// </summary>
		/// <param name="graphics">描画先のグラフィクス</param>
		/// <param name="info">キャンバス情報</param>
		/// <param name="mode">スケーリングモード</param>
		public void Render(Graphics graphics, CxCanvasInfo info, ExGdiScalingMode mode)
		{
			// 回転:
			if (this.Angle == 0)
			{
				RenderInternal(graphics, info, mode);
			}
			else
			{
				var gs = graphics.Save();
				try
				{
					var angle = (float)this.Angle;
					var axis = (mode == ExGdiScalingMode.None)
						? (PointF)(this.Location + this.Axis)
						: (PointF)((this.Location + this.Axis) * info.Magnification);
					graphics.TranslateTransform(+axis.X, +axis.Y);
					graphics.RotateTransform(angle);
					graphics.TranslateTransform(-axis.X, -axis.Y);

					RenderInternal(graphics, info, mode);
				}
				finally
				{
					graphics.Restore(gs);
				}
			}
		}

		/// <summary>
		/// 図形をグラフィクスに描画します。
		/// </summary>
		/// <param name="graphics">描画先のグラフィクス</param>
		/// <param name="info">キャンバス情報</param>
		/// <param name="mode">スケーリングモード</param>
		private void RenderInternal(Graphics graphics, CxCanvasInfo info, ExGdiScalingMode mode)
		{
			if (this.IsValid == false) return;

			// 外接矩形:
			TxRectangleD bounds;
			switch (mode)
			{
				default:
				case ExGdiScalingMode.None:
					{
						bounds = new TxRectangleD(this.Location, this.VisualSize);
					}
					break;
				case ExGdiScalingMode.TopLeft:
				case ExGdiScalingMode.Center:
					{
						bounds = new TxRectangleD(
							this.Location * info.Magnification,
							this.VisualSize * info.Magnification
							);
					}
					break;
			}

			if (1 <= this.Alpha)
			{
				graphics.DrawImage(this.Bitmap, (RectangleF)bounds);
			}
			else if (0 < this.Alpha)
			{
				using (var attr = new System.Drawing.Imaging.ImageAttributes())
				{
					var matrix = new System.Drawing.Imaging.ColorMatrix(new float[][]
					{
						new float[] {1,0,0,0,0},
						new float[] {0,1,0,0,0},
						new float[] {0,0,1,0,0},
						new float[] {0,0,0,(float)this.Alpha,0},
						new float[] {0,0,0,0,1},
					});
					attr.SetColorMatrix(matrix);
					graphics.DrawImage(
						this.Bitmap,
						(Rectangle)bounds,
						0, 0,
						this.Bitmap.Width,
						this.Bitmap.Height,
						GraphicsUnit.Pixel,
						attr);
				}
			}
		}

		#endregion

		#region IxGdi2dHandling の実装: (操作)

		/// <summary>
		/// 基準位置
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiImage.Location")]
		public virtual TxPointD Location
		{
			get
			{
				return new TxPointD(m_X, m_Y);
			}
			set
			{
				m_X = value.X;
				m_Y = value.Y;
			}
		}

		/// <summary>
		/// 外接矩形
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiImage.Bounds")]
		public virtual TxRectangleD Bounds
		{
			get
			{
				var rect = new TxRectangleD(this.Location, this.VisualSize);

				var trapezoid = rect.ToTrapezoid();
				var stat_x = new TxStatistics();
				var stat_y = new TxStatistics();

				stat_x.Reset();
				stat_y.Reset();

				stat_x += trapezoid.X1;
				stat_x += trapezoid.X2;
				stat_x += trapezoid.X3;
				stat_x += trapezoid.X4;

				stat_y += trapezoid.Y1;
				stat_y += trapezoid.Y2;
				stat_y += trapezoid.Y3;
				stat_y += trapezoid.Y4;

				var sx = stat_x.Min;
				var sy = stat_y.Min;
				var ex = stat_x.Max;
				var ey = stat_y.Max;

				return new TxRectangleD(sx, sy, (ex - sx), (ey - sy));
			}
		}

		/// <summary>
		/// 回転角 (degree) [±180]
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiImage.Angle")]
		public double Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}

		/// <summary>
		/// 回転の機軸 (図形の基準位置からの相対値(±))
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiImage.Axis")]
		public TxPointD Axis
		{
			get { return m_Axis; }
			set { m_Axis = value; }
		}

		/// <summary>
		/// 指定座標が図形のどの位置にあるかを判定します。
		/// </summary>
		/// <param name="position">指定座標</param>
		/// <param name="margin">判定の許容範囲 [0~] ※±margin の範囲内で判定します。</param>
		/// <returns>
		///		指定座標が図形外にあれば 0 を返します。
		///		指定座標が図形内にあれば 0 以外の値を返します。
		///		返す値の範囲は図形によって異なります。
		/// </returns>
		public virtual TxHitPosition HitTest(TxPointD position, double margin)
		{
			if (this.IsValid == false)
				return TxHitPosition.Default;

			var p0 = XIE.Axi.Rotate(position, this.Location + this.Axis, -this.Angle);
			var bounds = new TxRectangleD(this.Location, this.VisualSize);
			var hit = CxGdiRectangle.HitTestInternal(p0, margin, bounds);
			return hit;
		}

		/// <summary>
		/// 図形の編集（位置移動または形状変更）を行います。
		/// </summary>
		/// <param name="prev_figure">編集前の図形</param>
		/// <param name="prev_position">移動前の座標</param>
		/// <param name="move_position">移動後の座標</param>
		/// <param name="margin">判定の許容範囲 [0~] ※±margin の範囲内で判定します。</param>
		public void Modify(object prev_figure, TxPointD prev_position, TxPointD move_position, double margin)
		{
			if (prev_figure is CxGdiImage)
			{
				var figure = (CxGdiImage)prev_figure;
				if (this.IsValid && figure.IsValid)
				{
					var hit = figure.HitTest(prev_position, margin);

					switch (hit.Mode)
					{
						case 1:
							{
								var mv = move_position - prev_position;
								this.Location = figure.Location + mv;
							}
							break;
						case 2:
							{
								var _prev_position = XIE.Axi.Rotate(prev_position, figure.Location + figure.Axis, -figure.Angle);
								var _move_position = XIE.Axi.Rotate(move_position, figure.Location + figure.Axis, -figure.Angle);
								var mv = _move_position - _prev_position;

								var bounds = new TxRectangleD(figure.Location, figure.VisualSize);
								var dummy1 = new CxGdiRectangle(bounds) { Angle = figure.Angle, Axis = figure.Axis };
								var dummy2 = dummy1;
								dummy2.Modify(dummy1, prev_position, move_position, margin);
								this.MagX = dummy2.Width / figure.Bitmap.Width;
								this.MagY = dummy2.Height / figure.Bitmap.Height;

								var _this_location = this.Location;
								switch (hit.Site)
								{
									case -4:
										this.X = figure.X + mv.X;
										break;
									case -1:
										this.Y = figure.Y + mv.Y;
										break;
									case +1:
										this.X = figure.X + mv.X;
										this.Y = figure.Y + mv.Y;
										break;
									case +2:
										this.Y = figure.Y + mv.Y;
										break;
									case +4:
										this.X = figure.X + mv.X;
										break;
								}
								if (figure.Angle != 0)
								{
									if (_this_location.X != this.Location.X)
										this.m_Axis.X = figure.m_Axis.X - mv.X;
									if (_this_location.Y != this.Location.Y)
										this.m_Axis.Y = figure.m_Axis.Y - mv.Y;
								}
							}
							break;
					}
				}
				return;
			}
			throw new XIE.CxException(ExStatus.Unsupported);
		}

		#endregion

		#region IxGdi2dVisualizing の実装: (描画色)

		/// <summary>
		/// ペン色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiImage.PenColor")]
		public TxRGB8x4 PenColor
		{
			get { return Pen.Color; }
			set { Pen.Color = value; }
		}

		/// <summary>
		/// ペン幅
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiImage.PenWidth")]
		public float PenWidth
		{
			get { return Pen.Width; }
			set { Pen.Width = value; }
		}

		/// <summary>
		/// ペン形状
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiImage.PenStyle")]
		public ExGdiPenStyle PenStyle
		{
			get { return Pen.Style; }
			set { Pen.Style = value; }
		}

		/// <summary>
		/// ブラシ前景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiImage.BrushColor")]
		public TxRGB8x4 BrushColor
		{
			get { return Brush.Color; }
			set { Brush.Color = value; }
		}

		/// <summary>
		/// ブラシ背景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiImage.BrushShadow")]
		public TxRGB8x4 BrushShadow
		{
			get { return Brush.Shadow; }
			set { Brush.Shadow = value; }
		}

		/// <summary>
		/// ブラシスタイル
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiImage.BrushStyle")]
		public ExGdiBrushStyle BrushStyle
		{
			get { return Brush.Style; }
			set { Brush.Style = value; }
		}

		/// <summary>
		/// ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiImage.HatchStyle")]
		public System.Drawing.Drawing2D.HatchStyle HatchStyle
		{
			get { return Brush.HatchStyle; }
			set { Brush.HatchStyle = value; }
		}

		/// <summary>
		/// 線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiImage.LinearGradientMode")]
		public System.Drawing.Drawing2D.LinearGradientMode LinearGradientMode
		{
			get { return Brush.LinearGradientMode; }
			set { Brush.LinearGradientMode = value; }
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// 固有の処理:
		//

		#region プロパティ: (Parameter)

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiImage.X")]
		public virtual unsafe double X
		{
			get
			{
				return this.m_X;
			}
			set
			{
				this.m_X = value;
			}
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiImage.Y")]
		public virtual unsafe double Y
		{
			get
			{
				return this.m_Y;
			}
			set
			{
				this.m_Y = value;
			}
		}

		/// <summary>
		/// 視覚的なサイズの取得
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiImage.VisualSize")]
		public virtual TxSizeD VisualSize
		{
			get
			{
				if (Bitmap == null)
					return new TxSizeD(0, 0);
				else
					return new TxSizeD(Bitmap.Width * m_MagX, Bitmap.Height * m_MagY);
			}
		}

		/// <summary>
		/// 水平倍率 [初期値:1.0、範囲:0.0 より大きい値] ※ 1.0 を等倍とします。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiImage.MagX")]
		[DefaultValue(1.0)]
		public virtual unsafe double MagX
		{
			get
			{
				return this.m_MagX;
			}
			set
			{
				this.m_MagX = value;
			}
		}

		/// <summary>
		/// 垂直倍率 [初期値:1.0、範囲:0.0 より大きい値] ※ 1.0 を等倍とします。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiImage.MagY")]
		[DefaultValue(1.0)]
		public virtual unsafe double MagY
		{
			get
			{
				return this.m_MagY;
			}
			set
			{
				this.m_MagY = value;
			}
		}

		/// <summary>
		/// 不透過率 [初期値:1.0、範囲:0.0~1.0] ※ 0.0=透過、1.0=不透過
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiImage.Alpha")]
		[DefaultValue(1.0)]
		public virtual unsafe double Alpha
		{
			get
			{
				return this.m_Alpha;
			}
			set
			{
				this.m_Alpha = value;
			}
		}

		/// <summary>
		/// 画素ごとの透過の有効属性 [初期値:false] 
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiImage.AlphaFormat")]
		[DefaultValue(false)]
		public virtual unsafe bool AlphaFormat
		{
			get
			{
				return this.m_AlphaFormat;
			}
			set
			{
				this.m_AlphaFormat = value;
			}
		}

		#endregion
	}
}
