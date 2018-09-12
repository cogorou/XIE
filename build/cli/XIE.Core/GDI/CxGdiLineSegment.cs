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
	/// オーバレイ図形クラス (線分)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct CxGdiLineSegment
		: IEquatable<CxGdiLineSegment>
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
		public TxLineSegmentD Shape;

		#endregion

		#region 既定値:

		/// <summary>
		/// 既定値
		/// </summary>
		public static CxGdiLineSegment Default
		{
			get
			{
				var result = new CxGdiLineSegment();
				result.Angle = 0;
				result.Axis = new TxPointD(0, 0);
				result.Pen = TxGdiPen.Default;
				result.Brush = TxGdiBrush.Default;
				result.Shape = new TxLineSegmentD(0, 0, 0, 0);
				return result;
			}
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="x1">始点(X 座標)</param>
		/// <param name="y1">始点(Y 座標)</param>
		/// <param name="x2">終点(X 座標)</param>
		/// <param name="y2">終点(Y 座標)</param>
		public CxGdiLineSegment(double x1, double y1, double x2, double y2)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = new TxLineSegmentD(x1, y1, x2, y2);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="p1">始点</param>
		/// <param name="p2">終点</param>
		public CxGdiLineSegment(TxPointD p1, TxPointD p2)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = new TxLineSegmentD(p1, p2);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="shape">図形の形状</param>
		public CxGdiLineSegment(TxLineSegmentD shape)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = shape;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="shape">図形の形状</param>
		public CxGdiLineSegment(TxLineSegmentI shape)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = shape;
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
		public bool Equals(CxGdiLineSegment other)
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
			if (!(obj is CxGdiLineSegment)) return false;
			return this.Equals((CxGdiLineSegment)obj);
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
		public static bool operator ==(CxGdiLineSegment left, CxGdiLineSegment right)
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
		public static bool operator !=(CxGdiLineSegment left, CxGdiLineSegment right)
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
			var clone = new CxGdiLineSegment();
			clone = this;
			return clone;
		}

		#endregion

		#region メソッド: (変換系)

		/// <summary>
		/// TxLineSegmentD への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator TxLineSegmentD(CxGdiLineSegment src)
		{
			return src.Shape;
		}

		/// <summary>
		/// TxLineSegmentI への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator TxLineSegmentI(CxGdiLineSegment src)
		{
			return (TxLineSegmentI)src.Shape;
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
			// 始点と終点:
			TxPointD st;
			TxPointD ed;
			switch (mode)
			{
				default:
				case ExGdiScalingMode.None:
					{
						st = Shape.Point1;
						ed = Shape.Point2;
					}
					break;
				case ExGdiScalingMode.TopLeft:
				case ExGdiScalingMode.Center:
					{
						st = Shape.Point1 * info.Magnification;
						ed = Shape.Point2 * info.Magnification;
					}
					break;
			}

			// 輪郭:
			if (this.Pen.Style != ExGdiPenStyle.None)
			{
				using (var pen = this.Pen.ToPen(info.Magnification))
				{
					graphics.DrawLine(pen, (float)st.X, (float)st.Y, (float)ed.X, (float)ed.Y);
				}
			}
		}

		#endregion

		#region IxGdi2dHandling の実装: (操作)

		/// <summary>
		/// 基準位置
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.Location")]
		public TxPointD Location
		{
			get
			{
				return this.Point1;
			}
			set
			{
				var diff = value - this.Point1;

				Shape.X1 += diff.X;
				Shape.X2 += diff.X;

				Shape.Y1 += diff.Y;
				Shape.Y2 += diff.Y;
			}
		}

		/// <summary>
		/// 外接矩形
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.Bounds")]
		public TxRectangleD Bounds
		{
			get
			{
				var stat_x = new TxStatistics();
				var stat_y = new TxStatistics();

				stat_x.Reset();
				stat_y.Reset();

				stat_x += Shape.X1;
				stat_x += Shape.X2;

				stat_y += Shape.Y1;
				stat_y += Shape.Y2;

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
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.Angle")]
		public double Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}

		/// <summary>
		/// 回転の機軸 (図形の基準位置からの相対値(±))
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.Axis")]
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
			var hit = HitTestInternal(p0, margin, this.Shape);
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
		internal static TxHitPosition HitTestInternal(TxPointD position, double margin, TxLineSegmentD figure)
		{
			// 始点.
			if (figure.X1-margin <= position.X && position.X <= figure.X1+margin &&
				figure.Y1-margin <= position.Y && position.Y <= figure.Y1+margin)
			{
				return new TxHitPosition(2, 0, 1);
			}
			// 終点.
			if (figure.X2-margin <= position.X && position.X <= figure.X2+margin &&
				figure.Y2-margin <= position.Y && position.Y <= figure.Y2+margin)
			{
				return new TxHitPosition(2, 0, 2);
			}

			// 線分上.
			double	xL = figure.X2 - figure.X1;
			double	yL = figure.Y2 - figure.Y1;
			if( !(xL == 0 && yL == 0) )
			{
				// 始点を中心にしたときの終点の角度を算出する.
				double degree = XIE.Axi.RadToDeg( System.Math.Atan2( yL, xL ) );

				// 各点を始点を中心に逆方向に回転する.
				TxPointD st = figure.Point1;
				TxPointD ed = XIE.Axi.Rotate(figure.Point2, st, -degree);
				TxPointD p0 = XIE.Axi.Rotate(position, st, -degree);

				// マウス位置を水平線上で判定する.
				if( st.X-margin <= p0.X && p0.X <= ed.X+margin &&
					st.Y-margin <= p0.Y && p0.Y <= ed.Y+margin )
				{
					return new TxHitPosition(1, 0, 0);
				}
			}

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
			if (prev_figure is CxGdiLineSegment)
			{
				var figure = (CxGdiLineSegment)prev_figure;
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
								case +1: this.Point1 = figure.Point1 + mv; break;
								case +2: this.Point2 = figure.Point2 + mv; break;
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
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.PenColor")]
		public TxRGB8x4 PenColor
		{
			get { return Pen.Color; }
			set { Pen.Color = value; }
		}

		/// <summary>
		/// ペン幅
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.PenWidth")]
		public float PenWidth
		{
			get { return Pen.Width; }
			set { Pen.Width = value; }
		}

		/// <summary>
		/// ペン形状
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.PenStyle")]
		public ExGdiPenStyle PenStyle
		{
			get { return Pen.Style; }
			set { Pen.Style = value; }
		}

		/// <summary>
		/// ブラシ前景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.BrushColor")]
		public TxRGB8x4 BrushColor
		{
			get { return Brush.Color; }
			set { Brush.Color = value; }
		}

		/// <summary>
		/// ブラシ背景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.BrushShadow")]
		public TxRGB8x4 BrushShadow
		{
			get { return Brush.Shadow; }
			set { Brush.Shadow = value; }
		}

		/// <summary>
		/// ブラシスタイル
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.BrushStyle")]
		public ExGdiBrushStyle BrushStyle
		{
			get { return Brush.Style; }
			set { Brush.Style = value; }
		}

		/// <summary>
		/// ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.HatchStyle")]
		public System.Drawing.Drawing2D.HatchStyle HatchStyle
		{
			get { return Brush.HatchStyle; }
			set { Brush.HatchStyle = value; }
		}

		/// <summary>
		/// 線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.LinearGradientMode")]
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
		/// 始点(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.X1")]
		public double X1
		{
			get { return this.Shape.X1; }
			set { this.Shape.X1 = value; }
		}

		/// <summary>
		/// 始点(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.Y1")]
		public double Y1
		{
			get { return this.Shape.Y1; }
			set { this.Shape.Y1 = value; }
		}

		/// <summary>
		/// 終点(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.X2")]
		public double X2
		{
			get { return this.Shape.X2; }
			set { this.Shape.X2 = value; }
		}

		/// <summary>
		/// 終点(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.Y2")]
		public double Y2
		{
			get { return this.Shape.Y2; }
			set { this.Shape.Y2 = value; }
		}

		#endregion

		#region プロパティ:(補助)

		/// <summary>
		/// 始点
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.Point1")]
		public TxPointD Point1
		{
			get { return new TxPointD(this.X1, this.Y1); }
			set
			{
				X1 = value.X;
				Y1 = value.Y;
			}
		}

		/// <summary>
		/// 終点
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiLineSegment.Point2")]
		public TxPointD Point2
		{
			get { return new TxPointD(this.X2, this.Y2); }
			set
			{
				X2 = value.X;
				Y2 = value.Y;
			}
		}

		#endregion
	}
}
