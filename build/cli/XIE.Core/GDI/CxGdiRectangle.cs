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
	/// オーバレイ図形クラス (矩形)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct CxGdiRectangle
		: IEquatable<CxGdiRectangle>
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
		public TxRectangleD Shape;

		/// <summary>
		/// 角の丸みのサイズ [0~]
		/// </summary>
		private TxSizeD m_RoundSize;

		#endregion

		#region 既定値:

		/// <summary>
		/// 既定値
		/// </summary>
		public static CxGdiRectangle Default
		{
			get
			{
				var result = new CxGdiRectangle();
				result.Angle = 0;
				result.Axis = new TxPointD(0, 0);
				result.Pen = TxGdiPen.Default;
				result.Brush = TxGdiBrush.Default;
				result.Shape = new TxRectangleD(0, 0, 0, 0);
				result.RoundSize = new TxSizeD(0, 0);
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
		/// <param name="width">幅</param>
		/// <param name="height">高さ</param>
		public CxGdiRectangle(double x, double y, double width, double height)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = new TxRectangleD(x, y, width, height);
			m_RoundSize = new TxSizeD(0, 0);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="location">位置</param>
		/// <param name="size">サイズ</param>
		public CxGdiRectangle(TxPointD location, TxSizeD size)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = new TxRectangleD(location, size);
			m_RoundSize = new TxSizeD(0, 0);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="shape">図形の形状</param>
		public CxGdiRectangle(TxRectangleD shape)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = shape;
			m_RoundSize = new TxSizeD(0, 0);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="shape">図形の形状</param>
		public CxGdiRectangle(TxRectangleI shape)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = shape;
			m_RoundSize = new TxSizeD(0, 0);
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
		public bool Equals(CxGdiRectangle other)
		{
			if (this.Angle != other.Angle) return false;
			if (this.Axis != other.Axis) return false;
			if (this.Pen != other.Pen) return false;
			if (this.Brush != other.Brush) return false;
			if (this.Shape != other.Shape) return false;
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
			if (!(obj is CxGdiRectangle)) return false;
			return this.Equals((CxGdiRectangle)obj);
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
		public static bool operator ==(CxGdiRectangle left, CxGdiRectangle right)
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
		public static bool operator !=(CxGdiRectangle left, CxGdiRectangle right)
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
			var clone = new CxGdiRectangle();
			clone = this;
			return clone;
		}

		#endregion

		#region メソッド: (変換系)

		/// <summary>
		/// TxRectangleD への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator TxRectangleD(CxGdiRectangle src)
		{
			return src.Shape;
		}

		/// <summary>
		/// TxRectangleI への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator TxRectangleI(CxGdiRectangle src)
		{
			return (TxRectangleI)src.Shape;
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
			if (RoundSize.Width == 0 || RoundSize.Height == 0)
			{
				#region 通常の矩形:
				// 外接矩形:
				TxRectangleD bounds;
				Rectangle shape;
				switch (mode)
				{
					default:
					case ExGdiScalingMode.None:
						{
							bounds = this.Bounds;
							shape = this.Shape;
						}
						break;
					case ExGdiScalingMode.TopLeft:
					case ExGdiScalingMode.Center:
						{
							bounds = this.Bounds;
							bounds.Location *= info.Magnification;
							bounds.Size *= info.Magnification;
							shape = bounds;
						}
						break;
				}

				// 塗り潰し:
				if (this.Brush.Style != ExGdiBrushStyle.None)
				{
					using (var brush = this.Brush.ToBrush(bounds))
					{
						graphics.FillRectangle(brush, shape);
					}
				}

				// 輪郭:
				if (this.Pen.Style != ExGdiPenStyle.None)
				{
					using (var pen = this.Pen.ToPen(info.Magnification))
					{
						graphics.DrawRectangle(pen, shape);
					}
				}
				#endregion
			}
			else
			{
				#region 角に丸みがある矩形:
				// 外接矩形:
				TxRectangleD bounds;
				double mag;
				switch (mode)
				{
					default:
					case ExGdiScalingMode.None:
						{
							bounds = this.Bounds;
							mag = 1;
						}
						break;
					case ExGdiScalingMode.TopLeft:
					case ExGdiScalingMode.Center:
						{
							bounds = this.Bounds;
							bounds.Location *= info.Magnification;
							bounds.Size *= info.Magnification;
							mag = info.Magnification;
						}
						break;
				}

				/*	
				 *   1        2
				 *	┌─┬──────┬─┐
				 *	├─┼──────┼─┤
				 *	│ │      │ │
				 *	│ │      │ │
				 *	├─┼──────┼─┤
				 *	└─┴──────┴─┘
				 *   4        3
				 */
				{
					double rw = System.Math.Abs(RoundSize.Width) * mag;
					double rh = System.Math.Abs(RoundSize.Height) * mag;
					var inside = new TxRectangleD(
							bounds.Location + new TxSizeD(rw, rh),
							bounds.Size - (new TxSizeD(rw, rh) * 2)
						);
					var vo = bounds.ToTrapezoid();
					var vi = inside.ToTrapezoid();

					// 塗り潰し:
					if (this.Brush.Style != ExGdiBrushStyle.None)
					{
						using (var brush = this.Brush.ToBrush(bounds))
						{
							graphics.FillPie(brush, (float)(vi.X1 - rw), (float)(vi.Y1 - rh), (float)rw * 2, (float)rh * 2, 180, 90);
							graphics.FillPie(brush, (float)(vi.X2 - rw), (float)(vi.Y2 - rh), (float)rw * 2, (float)rh * 2, 270, 90);
							graphics.FillPie(brush, (float)(vi.X3 - rw), (float)(vi.Y3 - rh), (float)rw * 2, (float)rh * 2, 0, 90);
							graphics.FillPie(brush, (float)(vi.X4 - rw), (float)(vi.Y4 - rh), (float)rw * 2, (float)rh * 2, 90, 90);

							graphics.FillRectangle(brush, (RectangleF)inside);
							graphics.FillRectangle(brush, new RectangleF((float)vi.X1, (float)vo.Y1, (float)inside.Width, (float)rh));
							graphics.FillRectangle(brush, new RectangleF((float)vi.X4, (float)vi.Y4, (float)inside.Width, (float)rh));
							graphics.FillRectangle(brush, new RectangleF((float)vo.X1, (float)vi.Y1, (float)rw, (float)inside.Height));
							graphics.FillRectangle(brush, new RectangleF((float)vi.X2, (float)vi.Y2, (float)rw, (float)inside.Height));
						}
					}

					// 輪郭:
					if (this.Pen.Style != ExGdiPenStyle.None)
					{
						using (var pen = this.Pen.ToPen(info.Magnification))
						{
							graphics.DrawArc(pen, (float)(vi.X1 - rw), (float)(vi.Y1 - rh), (float)rw * 2, (float)rh * 2, 180, 90);
							graphics.DrawArc(pen, (float)(vi.X2 - rw), (float)(vi.Y2 - rh), (float)rw * 2, (float)rh * 2, 270, 90);
							graphics.DrawArc(pen, (float)(vi.X3 - rw), (float)(vi.Y3 - rh), (float)rw * 2, (float)rh * 2, 0, 90);
							graphics.DrawArc(pen, (float)(vi.X4 - rw), (float)(vi.Y4 - rh), (float)rw * 2, (float)rh * 2, 90, 90);

							graphics.DrawLine(pen, (float)vi.X1, (float)vo.Y1, (float)vi.X2, (float)vo.Y2);
							graphics.DrawLine(pen, (float)vi.X4, (float)vo.Y4, (float)vi.X3, (float)vo.Y3);
							graphics.DrawLine(pen, (float)vo.X1, (float)vi.Y1, (float)vo.X4, (float)vi.Y4);
							graphics.DrawLine(pen, (float)vo.X2, (float)vi.Y2, (float)vo.X3, (float)vi.Y3);
						}
					}
				}
				#endregion
			}
		}

		#endregion

		#region IxGdi2dHandling の実装: (操作)

		/// <summary>
		/// 基準位置
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.Location")]
		public TxPointD Location
		{
			get
			{
				return Shape.Location;
			}
			set
			{
				Shape.Location = value;
			}
		}

		/// <summary>
		/// 外接矩形
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.Bounds")]
		public TxRectangleD Bounds
		{
			get
			{
				var trapezoid = Shape.ToTrapezoid();
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
		[CxDescription("P:XIE.GDI.CxGdiRectangle.Angle")]
		public double Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}

		/// <summary>
		/// 回転の機軸 (図形の基準位置からの相対値(±))
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.Axis")]
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
			var hit = CxGdiRectangle.HitTestInternal(p0, margin, this.Shape);
			return hit;
		}

		/// <summary>
		/// 指定座標が図形のどの位置にあるかを判定します。
		/// </summary>
		/// <param name="position">指定座標</param>
		/// <param name="margin">判定の許容範囲 [0~] ※±margin の範囲内で判定します。</param>
		/// <param name="figure">図形</param>
		/// <returns>
		///		指定座標が図形外にあれば 0 を返します。
		///		指定座標が図形内にあれば 0 以外の値を返します。
		///		返す値の範囲は図形によって異なります。
		/// </returns>
		internal static TxHitPosition HitTestInternal(TxPointD position, double margin, TxRectangleD figure)
		{
			// 矩形の頂点.
			double	sx = figure.X;
			double	sy = figure.Y;
			double	ex = figure.X + figure.Width;
			double	ey = figure.Y + figure.Height;

			// 矩形の基準点との差.
			var diff = position - figure.Location;

			// 範囲:
			bool L = (sx-margin <= position.X && position.X <= sx+margin);
			bool T = (sy-margin <= position.Y && position.Y <= sy+margin);
			bool R = (ex-margin <= position.X && position.X <= ex+margin);
			bool B = (ey-margin <= position.Y && position.Y <= ey+margin);

			bool W = (figure.Width < 0)
				? (figure.Width <= diff.X && diff.X <= 0)
				: (0 <= diff.X && diff.X <= figure.Width);

			bool H = (figure.Height < 0)
				? (figure.Height <= diff.Y && diff.Y <= 0)
				: (0 <= diff.Y && diff.Y <= figure.Height);

			// 頂点.
			if (T && L) return new TxHitPosition(2, 0, 1);
			if (T && R) return new TxHitPosition(2, 0, 2);
			if (B && R) return new TxHitPosition(2, 0, 3);
			if (B && L) return new TxHitPosition(2, 0, 4);

			// 辺.
			if (T && W) return new TxHitPosition(2, 0, -1);
			if (R && H) return new TxHitPosition(2, 0, -2);
			if (B && W) return new TxHitPosition(2, 0, -3);
			if (L && H) return new TxHitPosition(2, 0, -4);

			// 面.
			if (W && H) return new TxHitPosition(1, 0, 0);

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
			if (prev_figure is CxGdiRectangle)
			{
				var figure = (CxGdiRectangle)prev_figure;
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

							var _this_location = this.Location;
							switch (hit.Site)
							{
								case -4:
									this.X = figure.X + mv.X;
									this.Width = figure.Width - mv.X;
									break;
								case -1:
									this.Y = figure.Y + mv.Y;
									this.Height = figure.Height - mv.Y;
									break;
								case -2:
									this.Width = figure.Width + mv.X;
									break;
								case -3:
									this.Height = figure.Height + mv.Y;
									break;
								case +1:
									this.X = figure.X + mv.X;
									this.Width = figure.Width - mv.X;
									this.Y = figure.Y + mv.Y;
									this.Height = figure.Height - mv.Y;
									break;
								case +2:
									this.Width = figure.Width + mv.X;
									this.Y = figure.Y + mv.Y;
									this.Height = figure.Height - mv.Y;
									break;
								case +3:
									this.Width = figure.Width + mv.X;
									this.Height = figure.Height + mv.Y;
									break;
								case +4:
									this.X = figure.X + mv.X;
									this.Width = figure.Width - mv.X;
									this.Height = figure.Height + mv.Y;
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
		[CxDescription("P:XIE.GDI.CxGdiRectangle.PenColor")]
		public TxRGB8x4 PenColor
		{
			get { return Pen.Color; }
			set { Pen.Color = value; }
		}

		/// <summary>
		/// ペン幅
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.PenWidth")]
		public float PenWidth
		{
			get { return Pen.Width; }
			set { Pen.Width = value; }
		}

		/// <summary>
		/// ペン形状
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.PenStyle")]
		public ExGdiPenStyle PenStyle
		{
			get { return Pen.Style; }
			set { Pen.Style = value; }
		}

		/// <summary>
		/// ブラシ前景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.BrushColor")]
		public TxRGB8x4 BrushColor
		{
			get { return Brush.Color; }
			set { Brush.Color = value; }
		}

		/// <summary>
		/// ブラシ背景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.BrushShadow")]
		public TxRGB8x4 BrushShadow
		{
			get { return Brush.Shadow; }
			set { Brush.Shadow = value; }
		}

		/// <summary>
		/// ブラシスタイル
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.BrushStyle")]
		public ExGdiBrushStyle BrushStyle
		{
			get { return Brush.Style; }
			set { Brush.Style = value; }
		}

		/// <summary>
		/// ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.HatchStyle")]
		public System.Drawing.Drawing2D.HatchStyle HatchStyle
		{
			get { return Brush.HatchStyle; }
			set { Brush.HatchStyle = value; }
		}

		/// <summary>
		/// 線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.LinearGradientMode")]
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
		/// 中心(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.X")]
		public double X
		{
			get { return this.Shape.X; }
			set { this.Shape.X = value; }
		}

		/// <summary>
		/// 中心(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.Y")]
		public double Y
		{
			get { return this.Shape.Y; }
			set { this.Shape.Y = value; }
		}

		/// <summary>
		/// 幅
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.Width")]
		public double Width
		{
			get { return this.Shape.Width; }
			set { this.Shape.Width = value; }
		}

		/// <summary>
		/// 高さ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.Height")]
		public double Height
		{
			get { return this.Shape.Height; }
			set { this.Shape.Height = value; }
		}

		#endregion

		#region プロパティ:(補助)

		/// <summary>
		/// サイズ
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.Size")]
		public TxSizeD Size
		{
			get { return Shape.Size; }
			set { Shape.Size = value; }
		}

		#endregion

		#region プロパティ:(付加機能)

		/// <summary>
		/// 角の丸みのサイズ [0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.RoundSize")]
		public TxSizeD RoundSize
		{
			get { return m_RoundSize; }
			set { m_RoundSize = value; }
		}

		#endregion
	}
}
