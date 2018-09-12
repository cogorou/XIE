/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
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
using System.Linq;
using XIE.Ptr;

namespace XIE.GDI
{
	/// <summary>
	/// オーバレイ図形クラス (点)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct CxGdiPoint
		: IEquatable<CxGdiPoint>
		, ICloneable
		, IxGdi2d
	{
		#region フィールド:

		private bool m_Visible;
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
		/// 図形の形状
		/// </summary>
		public TxPointD Shape;

		/// <summary>
		/// アンカーサイズ
		/// </summary>
		private ExGdiAnchorStyle m_AnchorStyle;

		/// <summary>
		/// アンカー形状
		/// </summary>
		private TxSizeD m_AnchorSize;

		#endregion

		#region 既定値:

		/// <summary>
		/// 既定値
		/// </summary>
		public static CxGdiPoint Default
		{
			get
			{
				var result = new CxGdiPoint();
				result.Angle = 0;
				result.Axis = new TxPointD(0, 0);
				result.Pen = TxGdiPen.Default;
				result.Brush = TxGdiBrush.Default;
				result.Shape = new TxPointD(0, 0);
				return result;
			}
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="x">X 座標</param>
		/// <param name="y">Y 座標</param>
		public CxGdiPoint(double x, double y)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = new TxPointD(x, y);
			m_AnchorSize = new TxSizeD(0.5, 0.5);
			m_AnchorStyle = ExGdiAnchorStyle.Cross;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="shape">図形の形状</param>
		/// <param name="style">アンカー形状</param>
		/// <param name="size_x">アンカーサイズ (X)</param>
		/// <param name="size_y">アンカーサイズ (Y)</param>
		public CxGdiPoint(TxPointD shape, ExGdiAnchorStyle style = ExGdiAnchorStyle.Cross, double size_x = 0.5, double size_y = 0.5)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = shape;
			m_AnchorSize = new TxSizeD(size_x, size_y);
			m_AnchorStyle = style;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="shape">図形の形状</param>
		/// <param name="style">アンカー形状</param>
		/// <param name="size_x">アンカーサイズ (X)</param>
		/// <param name="size_y">アンカーサイズ (Y)</param>
		public CxGdiPoint(TxPointI shape, ExGdiAnchorStyle style = ExGdiAnchorStyle.Cross, double size_x = 0.5, double size_y = 0.5)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = shape;
			m_AnchorSize = new TxSizeD(size_x, size_y);
			m_AnchorStyle = style;
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
		public bool Equals(CxGdiPoint other)
		{
			if (this.Angle != other.Angle) return false;
			if (this.Axis != other.Axis) return false;
			if (this.Pen != other.Pen) return false;
			if (this.Brush != other.Brush) return false;
			if (this.Shape != other.Shape) return false;
			if (this.AnchorSize != other.AnchorSize) return false;
			if (this.AnchorStyle != other.AnchorStyle) return false;
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
			if (!(obj is CxGdiPoint)) return false;
			return this.Equals((CxGdiPoint)obj);
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
		public static bool operator ==(CxGdiPoint left, CxGdiPoint right)
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
		public static bool operator !=(CxGdiPoint left, CxGdiPoint right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region ICloneable の実装: (複製)

		/// <summary>
		/// オブジェクトのクローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		object ICloneable.Clone()
		{
			var clone = new CxGdiPoint();
			clone = this;
			return clone;
		}

		#endregion

		#region メソッド: (変換系)

		/// <summary>
		/// TxPointD への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator TxPointD(CxGdiPoint src)
		{
			return src.Shape;
		}

		/// <summary>
		/// TxPointI への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator TxPointI(CxGdiPoint src)
		{
			return (TxPointI)src.Shape;
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
			// 外接矩形と頂点:
			TxRectangleD bounds;
			TxPointD shape;
			TxSizeD size;
			switch (mode)
			{
				default:
				case ExGdiScalingMode.None:
					{
						bounds = this.Bounds;
						shape = this.Shape;
						size = this.AnchorSize;
					}
					break;
				case ExGdiScalingMode.TopLeft:
				case ExGdiScalingMode.Center:
					{
						bounds = this.Bounds;
						bounds.Location *= info.Magnification;
						bounds.Size *= info.Magnification;
						shape = this.Shape * info.Magnification;
						size = this.AnchorSize * info.Magnification;
					}
					break;
			}

			#region 描画:
			switch (this.AnchorStyle)
			{
				default:
				case ExGdiAnchorStyle.None:
					{
					}
					break;
				case ExGdiAnchorStyle.Cross:
					{
						var p1 = new TxPointD(shape.X - size.Width, shape.Y);
						var p2 = new TxPointD(shape.X + size.Width, shape.Y);
						var p3 = new TxPointD(shape.X, shape.Y - size.Height);
						var p4 = new TxPointD(shape.X, shape.Y + size.Height);

						// 輪郭:
						if (this.Pen.Style != ExGdiPenStyle.None)
						{
							using (var pen = this.Pen.ToPen(info.Magnification))
							{
								graphics.DrawLine(pen, (PointF)p1, (PointF)p2);
								graphics.DrawLine(pen, (PointF)p3, (PointF)p4);
							}
						}
					}
					break;
				case ExGdiAnchorStyle.Diamond:
					{
						var p1 = new TxPointD(shape.X - size.Width, shape.Y);
						var p2 = new TxPointD(shape.X + size.Width, shape.Y);
						var p3 = new TxPointD(shape.X, shape.Y - size.Height);
						var p4 = new TxPointD(shape.X, shape.Y + size.Height);
						var vertex = new PointF[] { p1, p3, p2, p4, };

						// 塗り潰し:
						if (this.Brush.Style != ExGdiBrushStyle.None)
						{
							using (var brush = this.Brush.ToBrush(bounds))
							{
								graphics.FillPolygon(brush, vertex);
							}
						}

						// 輪郭:
						if (this.Pen.Style != ExGdiPenStyle.None)
						{
							using (var pen = this.Pen.ToPen(info.Magnification))
							{
								graphics.DrawPolygon(pen, vertex);
							}
						}
					}
					break;
				case ExGdiAnchorStyle.Arrow:
					{
						var p1 = new TxPointD(shape.X - size.Width, shape.Y - size.Height);
						var p2 = new TxPointD(shape.X, shape.Y);
						var p3 = new TxPointD(shape.X - size.Width, shape.Y + size.Height);

						// 輪郭:
						if (this.Pen.Style != ExGdiPenStyle.None)
						{
							using (var pen = this.Pen.ToPen(info.Magnification))
							{
								graphics.DrawLine(pen, (PointF)p1, (PointF)p2);
								graphics.DrawLine(pen, (PointF)p3, (PointF)p2);
							}
						}
					}
					break;
				case ExGdiAnchorStyle.Triangle:
					{
						var p1 = new TxPointD(shape.X - size.Width, shape.Y - size.Height);
						var p2 = new TxPointD(shape.X, shape.Y);
						var p3 = new TxPointD(shape.X - size.Width, shape.Y + size.Height);
						var vertex = new PointF[] { p1, p2, p3, };

						// 塗り潰し:
						if (this.Brush.Style != ExGdiBrushStyle.None)
						{
							using (var brush = this.Brush.ToBrush(bounds))
							{
								graphics.FillPolygon(brush, vertex);
							}
						}

						// 輪郭:
						if (this.Pen.Style != ExGdiPenStyle.None)
						{
							using (var pen = this.Pen.ToPen(info.Magnification))
							{
								graphics.DrawPolygon(pen, vertex);
							}
						}
					}
					break;
				case ExGdiAnchorStyle.Diagcross:
					{
						var p1 = new TxPointD(shape.X - size.Width, shape.Y - size.Height);
						var p2 = new TxPointD(shape.X + size.Width, shape.Y + size.Height);
						var p3 = new TxPointD(shape.X + size.Width, shape.Y - size.Height);
						var p4 = new TxPointD(shape.X - size.Width, shape.Y + size.Height);

						// 輪郭:
						if (this.Pen.Style != ExGdiPenStyle.None)
						{
							using (var pen = this.Pen.ToPen(info.Magnification))
							{
								graphics.DrawLine(pen, (PointF)p1, (PointF)p2);
								graphics.DrawLine(pen, (PointF)p3, (PointF)p4);
							}
						}
					}
					break;
				case ExGdiAnchorStyle.Rectangle:
					{
						var p1 = new TxPointD(shape.X - size.Width, shape.Y - size.Height);
						var p2 = new TxPointD(shape.X + size.Width, shape.Y + size.Height);
						var p3 = new TxPointD(shape.X + size.Width, shape.Y - size.Height);
						var p4 = new TxPointD(shape.X - size.Width, shape.Y + size.Height);
						var vertex = new PointF[] { p1, p3, p2, p4, };

						// 塗り潰し:
						if (this.Brush.Style != ExGdiBrushStyle.None)
						{
							using (var brush = this.Brush.ToBrush(bounds))
							{
								graphics.FillPolygon(brush, vertex);
							}
						}

						// 輪郭:
						if (this.Pen.Style != ExGdiPenStyle.None)
						{
							using (var pen = this.Pen.ToPen(info.Magnification))
							{
								graphics.DrawPolygon(pen, vertex);
							}
						}
					}
					break;
			}
			#endregion
		}

		#endregion

		#region IxGdi2dHandling の実装: (操作)

		/// <summary>
		/// 基準位置
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.Location")]
		public TxPointD Location
		{
			get
			{
				return Shape;
			}
			set
			{
				Shape = value;
			}
		}

		/// <summary>
		/// 外接矩形
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.Bounds")]
		public TxRectangleD Bounds
		{
			get
			{
				double sx = Shape.X - this.AnchorSize.Width;
				double sy = Shape.Y - this.AnchorSize.Height;
				double ex = Shape.X + this.AnchorSize.Width;
				double ey = Shape.Y + this.AnchorSize.Height;

				var rect = new TxRectangleD(sx, sy, (ex - sx), (ey - sy));

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

				sx = stat_x.Min;
				sy = stat_y.Min;
				ex = stat_x.Max;
				ey = stat_y.Max;

				return new TxRectangleD(sx, sy, (ex - sx), (ey - sy));
			}
		}

		/// <summary>
		/// 回転角 (degree) [±180]
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.Angle")]
		public double Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}

		/// <summary>
		/// 回転の機軸 (図形の基準位置からの相対値(±))
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.Axis")]
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
		public TxHitPosition HitTest(TxPointD position, double margin)
		{
			var p0 = XIE.Axi.Rotate(position, this.Location + this.Axis, -this.Angle);
			var bounds = this.Bounds;
			var hit = CxGdiRectangle.HitTestInternal(p0, margin, bounds);
			if (hit.Mode != 0)
				return new TxHitPosition(1, 0, 0);
			return TxHitPosition.Default;
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
			if (prev_figure is CxGdiPoint)
			{
				var figure = (CxGdiPoint)prev_figure;
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
						break;
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
		[CxDescription("P:XIE.GDI.CxGdiPoint.PenColor")]
		public TxRGB8x4 PenColor
		{
			get { return Pen.Color; }
			set { Pen.Color = value; }
		}

		/// <summary>
		/// ペン幅
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.PenWidth")]
		public float PenWidth
		{
			get { return Pen.Width; }
			set { Pen.Width = value; }
		}

		/// <summary>
		/// ペン形状
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.PenStyle")]
		public ExGdiPenStyle PenStyle
		{
			get { return Pen.Style; }
			set { Pen.Style = value; }
		}

		/// <summary>
		/// ブラシ前景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.BrushColor")]
		public TxRGB8x4 BrushColor
		{
			get { return Brush.Color; }
			set { Brush.Color = value; }
		}

		/// <summary>
		/// ブラシ背景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.BrushShadow")]
		public TxRGB8x4 BrushShadow
		{
			get { return Brush.Shadow; }
			set { Brush.Shadow = value; }
		}

		/// <summary>
		/// ブラシスタイル
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.BrushStyle")]
		public ExGdiBrushStyle BrushStyle
		{
			get { return Brush.Style; }
			set { Brush.Style = value; }
		}

		/// <summary>
		/// ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.HatchStyle")]
		public System.Drawing.Drawing2D.HatchStyle HatchStyle
		{
			get { return Brush.HatchStyle; }
			set { Brush.HatchStyle = value; }
		}

		/// <summary>
		/// 線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.LinearGradientMode")]
		public System.Drawing.Drawing2D.LinearGradientMode LinearGradientMode
		{
			get { return Brush.LinearGradientMode; }
			set { Brush.LinearGradientMode = value; }
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// 固有の処理:
		//

		#region プロパティ:

		/// <summary>
		/// X 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.X")]
		public double X
		{
			get { return this.Shape.X; }
			set { this.Shape.X = value; }
		}

		/// <summary>
		/// Y 座標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.Y")]
		public double Y
		{
			get { return this.Shape.Y; }
			set { this.Shape.Y = value; }
		}

		#endregion

		#region プロパティ: (付加情報)

		/// <summary>
		/// アンカー形状
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.AnchorStyle")]
		public ExGdiAnchorStyle AnchorStyle
		{
			get { return m_AnchorStyle; }
			set { m_AnchorStyle = value; }
		}

		/// <summary>
		/// アンカーサイズ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiPoint.AnchorSize")]
		public TxSizeD AnchorSize
		{
			get { return m_AnchorSize; }
			set { m_AnchorSize = value; }
		}

		#endregion
	}
}
