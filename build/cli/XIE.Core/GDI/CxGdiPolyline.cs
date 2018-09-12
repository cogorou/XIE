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

using TE = XIE.TxPointD;
using TPTR = XIE.Ptr.TxPointDPtr;

namespace XIE.GDI
{
	/// <summary>
	/// オーバレイ図形クラス (折れ線)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxGdiPolyline : System.Object
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
		/// 図形の形状。折れ線（または多角形）を構成する点列です。
		/// </summary>
		private TxPointD[] m_Shape;

		/// <summary>
		/// 端点の開閉 [true: 閉じる（多角形）、false: 閉じない（折れ線）]
		/// </summary>
		private bool m_Closed;

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
			m_Shape = new TxPointD[0];
			m_Closed = false;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGdiPolyline()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="src">複製元</param>
		public CxGdiPolyline(CxArray src)
		{
			_Constructor();
			CopyFrom(src);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="src">複製元</param>
		public CxGdiPolyline(IEnumerable src)
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
			m_Shape = new TxPointD[0];
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
				if (m_Shape == null) return false;
				if (m_Shape.Length == 0) return false;
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
			var clone = new CxGdiPolyline();
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
			if (src is CxGdiPolyline)
			{
				var _src = (CxGdiPolyline)src;
				this.Angle = _src.Angle;
				this.Axis = _src.Axis;
				this.Pen = _src.Pen;
				this.Brush = _src.Brush;
				this.Shape = _src.Shape;
				this.Closed = _src.Closed;
				return;
			}
			if (src is CxArray)
			{
				var _src = (CxArray)src;

				#region TxPointD:
				if (_src.Model == ModelOf.From<TxPointD>())
				{
					var src_addr = (XIE.Ptr.TxPointDPtr)_src.Address();
					this.Shape = new TxPointD[_src.Length];
					for (int i = 0; i < _src.Length; i++)
						this.Shape[i] = src_addr[i];
					return;
				}
				#endregion

				#region TxPointI:
				if (_src.Model == ModelOf.From<TxPointI>())
				{
					var src_addr = (XIE.Ptr.TxPointIPtr)_src.Address();
					this.Shape = new TxPointD[_src.Length];
					for (int i = 0; i < _src.Length; i++)
						this.Shape[i] = src_addr[i];
					return;
				}
				#endregion
			}
			if (src is IEnumerable<TxPointI>)
			{
				#region データ変換.
				{
					var _src = new List<TxPointI>((IEnumerable<TxPointI>)src);
					this.Shape = new TxPointD[_src.Count];
					for (int i = 0; i < _src.Count; i++)
						this.Shape[i] = _src[i];
					return;
				}
				#endregion
			}
			if (src is IEnumerable<TxPointD>)
			{
				#region データ変換.
				{
					var _src = new List<TxPointD>((IEnumerable<TxPointD>)src);
					this.Shape = new TxPointD[_src.Count];
					for (int i = 0; i < _src.Count; i++)
						this.Shape[i] = _src[i];
					return;
				}
				#endregion
			}
			if (src is IEnumerable<Point>)
			{
				#region データ変換.
				{
					var _src = new List<Point>((IEnumerable<Point>)src);
					this.Shape = new TxPointD[_src.Count];
					for (int i = 0; i < _src.Count; i++)
						this.Shape[i] = _src[i];
					return;
				}
				#endregion
			}
			if (src is IEnumerable<PointF>)
			{
				#region データ変換.
				{
					var _src = new List<PointF>((IEnumerable<PointF>)src);
					this.Shape = new TxPointD[_src.Count];
					for (int i = 0; i < _src.Count; i++)
						this.Shape[i] = _src[i];
					return;
				}
				#endregion
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
				var _src = (CxGdiPolyline)src;
				if (this.Angle != _src.Angle) return false;
				if (this.Axis != _src.Axis) return false;
				if (this.Pen != _src.Pen) return false;
				if (this.Brush != _src.Brush) return false;
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
			if (dst is CxArray)
			{
				var _dst = (CxArray)dst;
				_dst.CopyFrom(this.Shape);
			}
			else
			{
				throw new CxException(ExStatus.Unsupported);
			}
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

			// 外接矩形と頂点:
			TxRectangleD bounds;
			Point[] shape;
			switch (mode)
			{
				default:
				case ExGdiScalingMode.None:
					{
						bounds = this.Bounds;
						shape = new Point[this.Shape.Length];
						for(int i=0 ; i<this.Shape.Length ; i++)
						{
							shape[i] = this.Shape[i];
						}
					}
					break;
				case ExGdiScalingMode.TopLeft:
				case ExGdiScalingMode.Center:
					{
						bounds = this.Bounds;
						bounds.Location *= info.Magnification;
						bounds.Size *= info.Magnification;

						shape = new Point[this.Shape.Length];
						for (int i = 0; i < this.Shape.Length; i++)
						{
							shape[i] = this.Shape[i] * info.Magnification;
						}
					}
					break;
			}

			// 塗り潰し:
			if (this.Brush.Style != ExGdiBrushStyle.None)
			{
				if (this.Closed)
				{
					using (var brush = this.Brush.ToBrush(bounds))
					{
						graphics.FillPolygon(brush, shape);
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
						graphics.DrawPolygon(pen, shape);
					}
				}
				else
				{
					using (var pen = this.Pen.ToPen(info.Magnification))
					{
						graphics.DrawLines(pen, shape);
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
		[CxDescription("P:XIE.GDI.CxGdiPolyline.Location")]
		public virtual TxPointD Location
		{
			get
			{
				if (this.IsValid)
					return this.m_Shape[0];
				else
					return new TxPointD(0, 0);
			}
			set
			{
				if (this.IsValid)
				{
					var diff = value - this.m_Shape[0];
					for (int i = 0; i < this.m_Shape.Length; i++)
					{
						m_Shape[i] = m_Shape[i] + diff;
					}
				}
			}
		}

		/// <summary>
		/// 外接矩形
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPolyline.Bounds")]
		public virtual TxRectangleD Bounds
		{
			get
			{
				var result = new TxRectangleD(0, 0, 0, 0);
				if (this.IsValid == false)
					return result;

				int length = m_Shape.Length;

				var stat_x = new TxStatistics();
				var stat_y = new TxStatistics();

				stat_x.Reset();
				stat_y.Reset();

				foreach (var item in m_Shape)
				{
					stat_x += item.X;
					stat_y += item.Y;
				}

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
		[CxDescription("P:XIE.GDI.CxGdiPolyline.Angle")]
		public double Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}

		/// <summary>
		/// 回転の機軸 (図形の基準位置からの相対値(±))
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPolyline.Axis")]
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

			// 各頂点.
			for(int i=0 ; i<m_Shape.Length ; i++)
			{
				var item = m_Shape[i];
				if (item.X - margin <= p0.X && p0.X <= item.X + margin &&
					item.Y - margin <= p0.Y && p0.Y <= item.Y + margin)
				{
					return new TxHitPosition(2, i,  +1);
				}
			}

			// 各辺(頂点間).
			for(int i=0 ; i<m_Shape.Length-1 ; i++)
			{
				var ls = new TxLineSegmentD(m_Shape[i], m_Shape[i+1]);
				var hit = CxGdiLineSegment.HitTestInternal(p0, margin, ls);
				if (hit.Mode == 1)
					return new TxHitPosition(2, i, -1);
			}

			// 終点と始点の間.
			if (this.Closed && m_Shape.Length >= 3)
			{
				var ls = new TxLineSegmentD(m_Shape[m_Shape.Length - 1], m_Shape[0]);
				var hit1 = CxGdiLineSegment.HitTestInternal(p0, margin, ls);
				if (hit1.Mode == 1)
					return new TxHitPosition(2, m_Shape.Length - 1, -1);

				var bounds = this.Bounds;
				var hit2 = CxGdiRectangle.HitTestInternal(p0, margin, bounds);
				if (hit2.Mode != 0)
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
			if (prev_figure is CxGdiPolyline)
			{
				var figure = (CxGdiPolyline)prev_figure;
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

								var _this_location = this.Location;
								var min_length = System.Math.Min(this.Shape.Length, figure.Shape.Length);
								if (0 <= hit.Index && hit.Index < min_length)
								{
									// 頂点.
									if (hit.Site > 0)
									{
										this.Shape[hit.Index] = figure.Shape[hit.Index] + mv;
									}
									// 辺.
									else if (hit.Site < 0)
									{
										int index0 = hit.Index;
										int index1 = (hit.Index == (min_length - 1)) ? 0 : hit.Index + 1;
										this.Shape[index0] = figure.Shape[index0] + mv;
										this.Shape[index1] = figure.Shape[index1] + mv;
									}
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
		[CxDescription("P:XIE.GDI.CxGdiPolyline.PenColor")]
		public TxRGB8x4 PenColor
		{
			get { return Pen.Color; }
			set { Pen.Color = value; }
		}

		/// <summary>
		/// ペン幅
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPolyline.PenWidth")]
		public float PenWidth
		{
			get { return Pen.Width; }
			set { Pen.Width = value; }
		}

		/// <summary>
		/// ペン形状
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPolyline.PenStyle")]
		public ExGdiPenStyle PenStyle
		{
			get { return Pen.Style; }
			set { Pen.Style = value; }
		}

		/// <summary>
		/// ブラシ前景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPolyline.BrushColor")]
		public TxRGB8x4 BrushColor
		{
			get { return Brush.Color; }
			set { Brush.Color = value; }
		}

		/// <summary>
		/// ブラシ背景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPolyline.BrushShadow")]
		public TxRGB8x4 BrushShadow
		{
			get { return Brush.Shadow; }
			set { Brush.Shadow = value; }
		}

		/// <summary>
		/// ブラシスタイル
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPolyline.BrushStyle")]
		public ExGdiBrushStyle BrushStyle
		{
			get { return Brush.Style; }
			set { Brush.Style = value; }
		}

		/// <summary>
		/// ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPolyline.HatchStyle")]
		public System.Drawing.Drawing2D.HatchStyle HatchStyle
		{
			get { return Brush.HatchStyle; }
			set { Brush.HatchStyle = value; }
		}

		/// <summary>
		/// 線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiPolyline.LinearGradientMode")]
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
		/// 図形の形状。折れ線（または多角形）を構成する点列です。
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiPolyline.Shape")]
		public virtual TxPointD[] Shape
		{
			get
			{
				return this.m_Shape;
			}
			set
			{
				if (value == null)
					this.m_Shape = new TxPointD[0];
				else
				{
					this.m_Shape = new TxPointD[value.Length];
					for (int i = 0; i < this.m_Shape.Length; i++)
						this.m_Shape[i] = value[i];
				}
			}
		}

		/// <summary>
		/// 端点の開閉 [true: 閉じる（多角形）、false: 閉じない（折れ線）] [初期値:false]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiPolyline.Closed")]
		[DefaultValue(false)]
		public virtual bool Closed
		{
			get { return this.m_Closed; }
			set { this.m_Closed = value; }
		}

		#endregion
	}
}
