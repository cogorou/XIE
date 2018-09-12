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
	/// オーバレイ図形クラス (真円の円弧)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct CxGdiCircleArc
		: IEquatable<CxGdiCircleArc>
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
		public TxCircleArcD Shape;

		/// <summary>
		/// 端点の開閉
		/// </summary>
		private bool m_Closed;

		#endregion

		#region 既定値:

		/// <summary>
		/// 既定値
		/// </summary>
		public static CxGdiCircleArc Default
		{
			get
			{
				var result = new CxGdiCircleArc();
				result.m_Visible = true;
				result.m_Angle = 0;
				result.m_Axis = new TxPointD(0, 0);
				result.Pen = TxGdiPen.Default;
				result.Brush = TxGdiBrush.Default;
				result.Shape = new TxCircleArcD(0, 0, 0, 0, 0);
				result.Closed = false;
				return result;
			}
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="x">中心(X 座標)</param>
		/// <param name="y">中心(Y 座標)</param>
		/// <param name="radius">半径</param>
		/// <param name="start_angle">開始角 (度) [0~360]</param>
		/// <param name="sweep_angle">円弧範囲 (度) [0~±360]</param>
		/// <param name="closed">端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）] [初期値:false]</param>
		public CxGdiCircleArc(double x, double y, double radius, double start_angle, double sweep_angle, bool closed = false)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = new TxCircleArcD(x, y, radius, start_angle, sweep_angle);
			m_Closed = closed;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="center">中心</param>
		/// <param name="radius">半径</param>
		/// <param name="start_angle">開始角 (度) [0~360]</param>
		/// <param name="sweep_angle">円弧範囲 (度) [0~±360]</param>
		/// <param name="closed">端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）] [初期値:false]</param>
		public CxGdiCircleArc(TxPointD center, double radius, double start_angle, double sweep_angle, bool closed = false)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = new TxCircleArcD(center, radius, start_angle, sweep_angle);
			m_Closed = closed;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="shape">図形の形状</param>
		/// <param name="closed">端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）] [初期値:false]</param>
		public CxGdiCircleArc(TxCircleArcD shape, bool closed = false)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = shape;
			m_Closed = closed;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="shape">図形の形状</param>
		/// <param name="closed">端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）] [初期値:false]</param>
		public CxGdiCircleArc(TxCircleArcI shape, bool closed = false)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = shape;
			m_Closed = closed;
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
		public bool Equals(CxGdiCircleArc other)
		{
			if (this.Angle != other.Angle) return false;
			if (this.Axis != other.Axis) return false;
			if (this.Pen != other.Pen) return false;
			if (this.Brush != other.Brush) return false;
			if (this.Shape != other.Shape) return false;
			if (this.Closed != other.Closed) return false;
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
			if (!(obj is CxGdiCircleArc)) return false;
			return this.Equals((CxGdiCircleArc)obj);
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
		public static bool operator ==(CxGdiCircleArc left, CxGdiCircleArc right)
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
		public static bool operator !=(CxGdiCircleArc left, CxGdiCircleArc right)
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
			var clone = new CxGdiCircleArc();
			clone = this;
			return clone;
		}

		#endregion

		#region メソッド: (変換系)

		/// <summary>
		/// TxCircleArcD への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator TxCircleArcD(CxGdiCircleArc src)
		{
			return src.Shape;
		}

		/// <summary>
		/// TxCircleArcI への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator TxCircleArcI(CxGdiCircleArc src)
		{
			return (TxCircleArcI)src.Shape;
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
			// 外接矩形:
			var bounds = this.Bounds;
			switch (mode)
			{
				default:
				case ExGdiScalingMode.None:
					break;
				case ExGdiScalingMode.TopLeft:
				case ExGdiScalingMode.Center:
					{
						bounds.Location *= info.Magnification;
						bounds.Size *= info.Magnification;
					}
					break;
			}
			var shape = (Rectangle)bounds;

			// 塗り潰し:
			if (this.Brush.Style != ExGdiBrushStyle.None)
			{
				if (this.Closed)
				{
					using (var brush = this.Brush.ToBrush(bounds))
					{
						graphics.FillPie(brush, shape, (float)StartAngle, (float)SweepAngle);
					}
				}
			}

			// 輪郭:
			if (this.Pen.Style != ExGdiPenStyle.None)
			{
				if (this.Closed)
				{
					using (var pen = this.Pen.ToPen(info.Magnification))
					{
						graphics.DrawPie(pen, shape, (float)StartAngle, (float)SweepAngle);
					}
				}
				else
				{
					using (var pen = this.Pen.ToPen(info.Magnification))
					{
						graphics.DrawArc(pen, shape, (float)StartAngle, (float)SweepAngle);
					}
				}
			}
		}

		#endregion

		#region IxGdi2dHandling の実装: (操作)

		/// <summary>
		/// 基準位置
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.Location")]
		public TxPointD Location
		{
			get
			{
				return Shape.Center;
			}
			set
			{
				Shape.Center = value;
			}
		}

		/// <summary>
		/// 外接矩形
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.Bounds")]
		public TxRectangleD Bounds
		{
			get
			{
				var rect = new TxRectangleD(
					Shape.X - Shape.Radius,
					Shape.Y - Shape.Radius,
					Shape.Radius * 2,
					Shape.Radius * 2
					);

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
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.Angle")]
		public double Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}

		/// <summary>
		/// 回転の機軸 (図形の基準位置からの相対値(±))
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.Axis")]
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

			var dx = p0.X - Shape.X;
			var dy = p0.Y - Shape.Y;
			var distance = System.Math.Sqrt(dx * dx + dy * dy);

			if (Shape.Radius - margin <= distance && distance <= Shape.Radius + margin)
				return new TxHitPosition(2, 0, +1);

			if (0 <= distance && distance <= Shape.Radius - margin)
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
			if (prev_figure is CxGdiCircleArc)
			{
				var figure = (CxGdiCircleArc)prev_figure;
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
							var _base_position = XIE.Axi.Rotate(figure.Location, figure.Location + figure.Axis, -figure.Angle);
							var _prev_position = XIE.Axi.Rotate(prev_position, figure.Location + figure.Axis, -figure.Angle);
							var _move_position = XIE.Axi.Rotate(move_position, figure.Location + figure.Axis, -figure.Angle);
							var prev_diff = _prev_position - _base_position;
							var move_diff = _move_position - _base_position;
							var prev_radius = System.Math.Sqrt(prev_diff.X * prev_diff.X + prev_diff.Y * prev_diff.Y);
							var move_radius = System.Math.Sqrt(move_diff.X * move_diff.X + move_diff.Y * move_diff.Y);
							var mv = move_radius - prev_radius;
							this.Radius = figure.Radius + mv;
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
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.PenColor")]
		public TxRGB8x4 PenColor
		{
			get { return Pen.Color; }
			set { Pen.Color = value; }
		}

		/// <summary>
		/// ペン幅
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.PenWidth")]
		public float PenWidth
		{
			get { return Pen.Width; }
			set { Pen.Width = value; }
		}

		/// <summary>
		/// ペン形状
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.PenStyle")]
		public ExGdiPenStyle PenStyle
		{
			get { return Pen.Style; }
			set { Pen.Style = value; }
		}

		/// <summary>
		/// ブラシ前景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.BrushColor")]
		public TxRGB8x4 BrushColor
		{
			get { return Brush.Color; }
			set { Brush.Color = value; }
		}

		/// <summary>
		/// ブラシ背景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.BrushShadow")]
		public TxRGB8x4 BrushShadow
		{
			get { return Brush.Shadow; }
			set { Brush.Shadow = value; }
		}

		/// <summary>
		/// ブラシスタイル
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.BrushStyle")]
		public ExGdiBrushStyle BrushStyle
		{
			get { return Brush.Style; }
			set { Brush.Style = value; }
		}

		/// <summary>
		/// ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.HatchStyle")]
		public System.Drawing.Drawing2D.HatchStyle HatchStyle
		{
			get { return Brush.HatchStyle; }
			set { Brush.HatchStyle = value; }
		}

		/// <summary>
		/// 線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.LinearGradientMode")]
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
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.X")]
		public double X
		{
			get { return this.Shape.X; }
			set { this.Shape.X = value; }
		}

		/// <summary>
		/// 中心(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.Y")]
		public double Y
		{
			get { return this.Shape.Y; }
			set { this.Shape.Y = value; }
		}

		/// <summary>
		/// 半径
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.Radius")]
		public double Radius
		{
			get { return this.Shape.Radius; }
			set { this.Shape.Radius = value; }
		}

		/// <summary>
		/// 開始角 (度) [0~360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.StartAngle")]
		public double StartAngle
		{
			get { return Shape.StartAngle; }
			set { Shape.StartAngle = value; }
		}

		/// <summary>
		/// 円弧範囲 (度) [0~±360]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.SweepAngle")]
		public double SweepAngle
		{
			get { return Shape.SweepAngle; }
			set { Shape.SweepAngle = value; }
		}

		/// <summary>
		/// 端点の開閉 [true：扇型（円周の端点と中心が接続された状態） false:円弧（円周のみ描画された状態）] [初期値:false]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiCircleArc.Closed")]
		[DefaultValue(false)]
		public bool Closed
		{
			get { return this.m_Closed; }
			set { this.m_Closed = value; }
		}

		#endregion
	}
}
