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
	/// オーバレイ図形クラス (台形)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct CxGdiTrapezoid
		: IEquatable<CxGdiTrapezoid>
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
		public TxTrapezoidD Shape;

		#endregion

		#region 既定値:

		/// <summary>
		/// 既定値
		/// </summary>
		public static CxGdiTrapezoid Default
		{
			get
			{
				var result = new CxGdiTrapezoid();
				result.Angle = 0;
				result.Axis = new TxPointD(0, 0);
				result.Pen = TxGdiPen.Default;
				result.Brush = TxGdiBrush.Default;
				result.Shape = new TxTrapezoidD(0, 0, 0, 0, 0, 0, 0, 0);
				return result;
			}
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="x1">左上(X 座標)</param>
		/// <param name="y1">左上(Y 座標)</param>
		/// <param name="x2">右上(X 座標)</param>
		/// <param name="y2">右上(Y 座標)</param>
		/// <param name="x3">右下(X 座標)</param>
		/// <param name="y3">右下(Y 座標)</param>
		/// <param name="x4">左下(X 座標)</param>
		/// <param name="y4">左下(Y 座標)</param>
		public CxGdiTrapezoid(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = new TxTrapezoidD(x1, y1, x2, y2, x3, y3, x4, y4);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="p1">左上</param>
		/// <param name="p2">右上</param>
		/// <param name="p3">右下</param>
		/// <param name="p4">左下</param>
		public CxGdiTrapezoid(TxPointD p1, TxPointD p2, TxPointD p3, TxPointD p4)
		{
			m_Visible = true;
			m_Angle = 0;
			m_Axis = new TxPointD(0, 0);
			Pen = TxGdiPen.Default;
			Brush = TxGdiBrush.Default;
			Shape = new TxTrapezoidD(p1, p2, p3, p4);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="shape">図形の形状</param>
		public CxGdiTrapezoid(TxTrapezoidD shape)
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
		public CxGdiTrapezoid(TxTrapezoidI shape)
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
		public bool Equals(CxGdiTrapezoid other)
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
			if (!(obj is CxGdiTrapezoid)) return false;
			return this.Equals((CxGdiTrapezoid)obj);
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
		public static bool operator ==(CxGdiTrapezoid left, CxGdiTrapezoid right)
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
		public static bool operator !=(CxGdiTrapezoid left, CxGdiTrapezoid right)
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
			var clone = new CxGdiTrapezoid();
			clone = this;
			return clone;
		}

		#endregion

		#region メソッド: (変換系)

		/// <summary>
		/// TxTrapezoidD への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator TxTrapezoidD(CxGdiTrapezoid src)
		{
			return src.Shape;
		}

		/// <summary>
		/// TxTrapezoidI への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator TxTrapezoidI(CxGdiTrapezoid src)
		{
			return (TxTrapezoidI)src.Shape;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// 描画処理
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
			Point[] shape;
			switch (mode)
			{
				default:
				case ExGdiScalingMode.None:
					{
						bounds = this.Bounds;
						shape = new Point[]
						{
							Vertex1,
							Vertex2,
							Vertex3,
							Vertex4,
						};
					}
					break;
				case ExGdiScalingMode.TopLeft:
				case ExGdiScalingMode.Center:
					{
						bounds = this.Bounds;
						bounds.Location *= info.Magnification;
						bounds.Size *= info.Magnification;

						shape = new Point[]
						{
							Vertex1 * info.Magnification,
							Vertex2 * info.Magnification,
							Vertex3 * info.Magnification,
							Vertex4 * info.Magnification,
						};
					}
					break;
			}

			// 塗り潰し:
			if (this.Brush.Style != ExGdiBrushStyle.None)
			{
				using (var brush = this.Brush.ToBrush(bounds))
				{
					graphics.FillPolygon(brush, shape);
				}
			}

			// 輪郭:
			if (this.Pen.Style != ExGdiPenStyle.None)
			{
				using (var pen = this.Pen.ToPen(info.Magnification))
				{
					graphics.DrawPolygon(pen, shape);
				}
			}
		}

		#endregion

		#region IxGdi2dHandling の実装: (操作)

		/// <summary>
		/// 基準位置
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.Location")]
		public TxPointD Location
		{
			get
			{
				return this.Vertex1;
			}
			set
			{
				var diff = value - this.Vertex1;

				Shape.X1 += diff.X;
				Shape.X2 += diff.X;
				Shape.X3 += diff.X;
				Shape.X4 += diff.X;

				Shape.Y1 += diff.Y;
				Shape.Y2 += diff.Y;
				Shape.Y3 += diff.Y;
				Shape.Y4 += diff.Y;
			}
		}

		/// <summary>
		/// 外接矩形
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.Bounds")]
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
				stat_x += Shape.X3;
				stat_x += Shape.X4;

				stat_y += Shape.Y1;
				stat_y += Shape.Y2;
				stat_y += Shape.Y3;
				stat_y += Shape.Y4;

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
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.Angle")]
		public double Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}

		/// <summary>
		/// 回転の機軸 (図形の基準位置からの相対値(±))
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.Axis")]
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

			var vertex = new TxPointD[]
			{
				this.Vertex1,
				this.Vertex2,
				this.Vertex3,
				this.Vertex4,
				this.Vertex1,
			};

			// 頂点と辺の判定:
			{
				for(int j=0 ; j<4 ; j++)
				{
					var ls = new TxLineSegmentD(
								vertex[j+0].X,
								vertex[j+0].Y,
								vertex[j+1].X,
								vertex[j+1].Y
							);
					var hit = CxGdiLineSegment.HitTestInternal(p0, margin, ls);
					switch(hit.Mode)
					{
					case 2:		// 端点.
						if (hit.Site == 1)	// 始点.
							return new TxHitPosition(2, 0, +(j+1));
						break;
					case 1:		// 辺上.
						return new TxHitPosition(2, 0, -(j+1));
					}
				}
			}

			// 内外判定:
			{
				var bounds = this.Bounds;
				var hit = CxGdiRectangle.HitTestInternal(p0, margin, bounds);
				if (hit.Mode != 0)
					return new TxHitPosition(1, 0, 0);
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
			if (prev_figure is CxGdiTrapezoid)
			{
				var figure = (CxGdiTrapezoid)prev_figure;
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
									this.X1 = figure.X1 + mv.X;
									this.X4 = figure.X4 + mv.X;
									break;
								case -1:
									this.Y1 = figure.Y1 + mv.Y;
									this.Y2 = figure.Y2 + mv.Y;
									break;
								case -2:
									this.X2 = figure.X2 + mv.X;
									this.X3 = figure.X3 + mv.X;
									break;
								case -3:
									this.Y3 = figure.Y3 + mv.Y;
									this.Y4 = figure.Y4 + mv.Y;
									break;
								case +1:
									this.X1 = figure.X1 + mv.X;
									this.Y1 = figure.Y1 + mv.Y;
									break;
								case +2:
									this.X2 = figure.X2 + mv.X;
									this.Y2 = figure.Y2 + mv.Y;
									break;
								case +3:
									this.X3 = figure.X3 + mv.X;
									this.Y3 = figure.Y3 + mv.Y;
									break;
								case +4:
									this.X4 = figure.X4 + mv.X;
									this.Y4 = figure.Y4 + mv.Y;
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
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.PenColor")]
		public TxRGB8x4 PenColor
		{
			get { return Pen.Color; }
			set { Pen.Color = value; }
		}

		/// <summary>
		/// ペン幅
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.PenWidth")]
		public float PenWidth
		{
			get { return Pen.Width; }
			set { Pen.Width = value; }
		}

		/// <summary>
		/// ペン形状
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.PenStyle")]
		public ExGdiPenStyle PenStyle
		{
			get { return Pen.Style; }
			set { Pen.Style = value; }
		}

		/// <summary>
		/// ブラシ前景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.BrushColor")]
		public TxRGB8x4 BrushColor
		{
			get { return Brush.Color; }
			set { Brush.Color = value; }
		}

		/// <summary>
		/// ブラシ背景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.BrushShadow")]
		public TxRGB8x4 BrushShadow
		{
			get { return Brush.Shadow; }
			set { Brush.Shadow = value; }
		}

		/// <summary>
		/// ブラシスタイル
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.BrushStyle")]
		public ExGdiBrushStyle BrushStyle
		{
			get { return Brush.Style; }
			set { Brush.Style = value; }
		}

		/// <summary>
		/// ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.HatchStyle")]
		public System.Drawing.Drawing2D.HatchStyle HatchStyle
		{
			get { return Brush.HatchStyle; }
			set { Brush.HatchStyle = value; }
		}

		/// <summary>
		/// 線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.LinearGradientMode")]
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
		/// 左上(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.X1")]
		public double X1
		{
			get { return this.Shape.X1; }
			set { this.Shape.X1 = value; }
		}

		/// <summary>
		/// 左上(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.Y1")]
		public double Y1
		{
			get { return this.Shape.Y1; }
			set { this.Shape.Y1 = value; }
		}

		/// <summary>
		/// 右上(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.X2")]
		public double X2
		{
			get { return this.Shape.X2; }
			set { this.Shape.X2 = value; }
		}

		/// <summary>
		/// 右上(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.Y2")]
		public double Y2
		{
			get { return this.Shape.Y2; }
			set { this.Shape.Y2 = value; }
		}

		/// <summary>
		/// 右下(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.X3")]
		public double X3
		{
			get { return this.Shape.X3; }
			set { this.Shape.X3 = value; }
		}

		/// <summary>
		/// 右下(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.Y3")]
		public double Y3
		{
			get { return this.Shape.Y3; }
			set { this.Shape.Y3 = value; }
		}

		/// <summary>
		/// 左下(X 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.X4")]
		public double X4
		{
			get { return this.Shape.X4; }
			set { this.Shape.X4 = value; }
		}

		/// <summary>
		/// 左下(Y 座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.Y4")]
		public double Y4
		{
			get { return this.Shape.Y4; }
			set { this.Shape.Y4 = value; }
		}

		#endregion

		#region プロパティ:(補助)

		/// <summary>
		/// 頂点1
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.Vertex1")]
		public TxPointD Vertex1
		{
			get { return new TxPointD(this.X1, this.Y1); }
			set
			{
				X1 = value.X;
				Y1 = value.Y;
			}
		}

		/// <summary>
		/// 頂点2
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.Vertex2")]
		public TxPointD Vertex2
		{
			get { return new TxPointD(this.X2, this.Y2); }
			set
			{
				X2 = value.X;
				Y2 = value.Y;
			}
		}

		/// <summary>
		/// 頂点3
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.Vertex3")]
		public TxPointD Vertex3
		{
			get { return new TxPointD(this.X3, this.Y3); }
			set
			{
				X3 = value.X;
				Y3 = value.Y;
			}
		}

		/// <summary>
		/// 頂点4
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiTrapezoid.Vertex4")]
		public TxPointD Vertex4
		{
			get { return new TxPointD(this.X4, this.Y4); }
			set
			{
				X4 = value.X;
				Y4 = value.Y;
			}
		}

		#endregion
	}
}
