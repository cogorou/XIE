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
	/// オーバレイ図形クラス (文字列)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxGdiString : System.Object
		, IxGdi2d
		, IDisposable
		, ICloneable
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
		/// X 座標
		/// </summary>
		private double m_X;

		/// <summary>
		/// Y 座標
		/// </summary>
		private double m_Y;

		/// <summary>
		/// フォント
		/// </summary>
		private TxGdiFont m_Font;

		/// <summary>
		/// テキストの配置 [初期値:TopLeft]
		/// </summary>
		private ExGdiTextAlign m_Align;

		/// <summary>
		/// テキスト
		/// </summary>
		private string m_Text;

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
			m_X = 0;
			m_Y = 0;
			m_Font = TxGdiFont.Default;
			m_Align = ExGdiTextAlign.TopLeft;
			m_Text = "";
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGdiString()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="src">複製元</param>
		public CxGdiString(string src)
		{
			_Constructor();
			CopyFrom(src);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="src">複製元</param>
		public CxGdiString(CxStringA src)
		{
			_Constructor();
			CopyFrom(src);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="src">複製元</param>
		public CxGdiString(CxStringW src)
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
			m_Text = "";
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
				if (string.IsNullOrEmpty(m_Text)) return false;
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
			var clone = new CxGdiString();
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
			if (src is CxGdiString)
			{
				var _src = (CxGdiString)src;
				this.Angle = _src.Angle;
				this.Axis = _src.Axis;
				this.Pen = _src.Pen;
				this.Brush = _src.Brush;
				this.X = _src.X;
				this.Y = _src.Y;
				this.Font = _src.Font;
				this.Align = _src.Align;
				this.Text = _src.Text;
				return;
			}
			if (src is string)
			{
				this.Text = (string)src;
				return;
			}
			if (src is CxStringA)
			{
				var _src = (CxStringA)src;
				this.Text = _src.ToString();
				return;
			}
			if (src is CxStringW)
			{
				var _src = (CxStringW)src;
				this.Text = _src.ToString();
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
				var _src = (CxGdiString)src;
				if (this.Angle != _src.Angle) return false;
				if (this.Axis != _src.Axis) return false;
				if (this.Pen != _src.Pen) return false;
				if (this.Brush != _src.Brush) return false;
				if (this.X != _src.X) return false;
				if (this.Y != _src.Y) return false;
				if (this.Font != _src.Font) return false;
				if (this.Align != _src.Align) return false;
				if (this.Text != _src.Text) return false;
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
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
			var gs = graphics.Save();
			try
			{
				// 伸縮:
				if (mode != ExGdiScalingMode.None)
				{
					if (0.0 < info.Magnification && info.Magnification != 1.0)
					{
						graphics.ScaleTransform((float)info.Magnification, (float)info.Magnification);
					}
				}

				// 回転:
				if (this.Angle == 0)
				{
					RenderInternal(graphics, info, mode);
				}
				else
				{
					var angle = (float)this.Angle;
					var axis = (PointF)(this.Location + this.Axis);
					//var axis = (mode == ExGdiScalingMode.None)
					//	? (PointF)(this.Location + this.Axis)
					//	: (PointF)((this.Location + this.Axis) * info.Magnification);
					graphics.TranslateTransform(+axis.X, +axis.Y);
					graphics.RotateTransform(angle);
					graphics.TranslateTransform(-axis.X, -axis.Y);

					RenderInternal(graphics, info, mode);
				}
			}
			finally
			{
				graphics.Restore(gs);
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

			using (var font = this.Font.ToFont())
			{
				// 外接矩形:
				TxRectangleD bounds;
				switch (mode)
				{
					default:
					case ExGdiScalingMode.None:
						{
							bounds = this.GetBounds(graphics, font);
						}
						break;
					case ExGdiScalingMode.TopLeft:
					case ExGdiScalingMode.Center:
						{
							bounds = this.GetBounds(graphics, font);
							//bounds.X *= info.Magnification;
							//bounds.Y *= info.Magnification;
							//bounds.Width *= info.Magnification;
							//bounds.Height *= info.Magnification;
						}
						break;
				}

				// 塗り潰し:
				if (this.Brush.Style != ExGdiBrushStyle.None)
				{
					using (var brush = this.Brush.ToBrush(bounds))
					{
						graphics.FillRectangle(brush, bounds);
					}
				}

				// 描画:
				if (this.Pen.Style != ExGdiPenStyle.None)
				{
					using (var brush = new SolidBrush(this.PenColor))
					{
						graphics.DrawString(this.Text, font, brush, (PointF)bounds.Location);
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
		[CxDescription("P:XIE.GDI.CxGdiString.Location")]
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
		[CxDescription("P:XIE.GDI.CxGdiString.Bounds")]
		public virtual TxRectangleD Bounds
		{
			get
			{
				var result = new TxRectangleD(m_X, m_Y, 0, 0);
				if (this.Text != null && this.Text.Length > 0 && Font.Size > 0)
				{
					int height = (int)System.Math.Ceiling(Font.Size);
					int width = this.Text.Length * height;
					using (var bitmap = new Bitmap(width, height))
					using (var graphics = Graphics.FromImage(bitmap))
					using (var font = new Font(Font.Name, Font.Size, Font.Style))
					{
						result = this.GetBounds(graphics, font);
					}
				}
				return result;
			}
		}

		/// <summary>
		/// 外接矩形の取得
		/// </summary>
		/// <param name="graphics">グラフィクス</param>
		/// <param name="font">フォント</param>
		/// <returns>
		///		外接矩形を計算して返します。
		/// </returns>
		public TxRectangleD GetBounds(Graphics graphics, Font font)
		{
			var result = new TxRectangleD(m_X, m_Y, 0, 0);
			if (this.Text != null && this.Text.Length > 0 && Font.Size > 0)
			{
				int height = (int)System.Math.Ceiling(Font.Size);
				int width = this.Text.Length * height;
				SizeF size = graphics.MeasureString(this.Text, font);
				result.Size = size;

				#region Align
				switch (this.Align)
				{
					case ExGdiTextAlign.TopLeft:
						break;
					case ExGdiTextAlign.TopRight:
						result.X = result.X - size.Width;
						break;
					case ExGdiTextAlign.TopCenter:
						result.X = result.X - size.Width / 2;
						break;
					case ExGdiTextAlign.BottomLeft:
						result.Y = result.Y - size.Height;		// Bottom
						break;
					case ExGdiTextAlign.BottomRight:
						result.Y = result.Y - size.Height;		// Bottom
						result.X = result.X - size.Width;
						break;
					case ExGdiTextAlign.BottomCenter:
						result.Y = result.Y - size.Height;		// Bottom
						result.X = result.X - size.Width / 2;
						break;
					case ExGdiTextAlign.BaselineLeft:
						result.Y = result.Y - size.Height / 2;	// Center
						break;
					case ExGdiTextAlign.BaselineRight:
						result.Y = result.Y - size.Height / 2;	// Center
						result.X = result.X - size.Width;
						break;
					case ExGdiTextAlign.BaselineCenter:
						result.Y = result.Y - size.Height / 2;	// Center
						result.X = result.X - size.Width / 2;
						break;
				}
				#endregion
			}
			return result;
		}

		/// <summary>
		/// 回転角 (degree) [±180]
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiString.Angle")]
		public double Angle
		{
			get { return m_Angle; }
			set { m_Angle = value; }
		}

		/// <summary>
		/// 回転の機軸 (図形の基準位置からの相対値(±))
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiString.Axis")]
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
			var bounds = this.Bounds;
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
			if (prev_figure is CxGdiString)
			{
				var figure = (CxGdiString)prev_figure;
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
		[CxDescription("P:XIE.GDI.CxGdiString.PenColor")]
		public TxRGB8x4 PenColor
		{
			get { return Pen.Color; }
			set { Pen.Color = value; }
		}

		/// <summary>
		/// ペン幅
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiString.PenWidth")]
		public float PenWidth
		{
			get { return Pen.Width; }
			set { Pen.Width = value; }
		}

		/// <summary>
		/// ペン形状
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiString.PenStyle")]
		public ExGdiPenStyle PenStyle
		{
			get { return Pen.Style; }
			set { Pen.Style = value; }
		}

		/// <summary>
		/// ブラシ前景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiString.BrushColor")]
		public TxRGB8x4 BrushColor
		{
			get { return Brush.Color; }
			set { Brush.Color = value; }
		}

		/// <summary>
		/// ブラシ背景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiString.BrushShadow")]
		public TxRGB8x4 BrushShadow
		{
			get { return Brush.Shadow; }
			set { Brush.Shadow = value; }
		}

		/// <summary>
		/// ブラシスタイル
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiString.BrushStyle")]
		public ExGdiBrushStyle BrushStyle
		{
			get { return Brush.Style; }
			set { Brush.Style = value; }
		}

		/// <summary>
		/// ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiString.HatchStyle")]
		public System.Drawing.Drawing2D.HatchStyle HatchStyle
		{
			get { return Brush.HatchStyle; }
			set { Brush.HatchStyle = value; }
		}

		/// <summary>
		/// 線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxGdiString.LinearGradientMode")]
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
		[CxDescription("P:XIE.GDI.CxGdiString.X")]
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
		[CxDescription("P:XIE.GDI.CxGdiString.Y")]
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
		/// フォント
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiString.Font")]
		public virtual TxGdiFont Font
		{
			get
			{
				return m_Font;
			}
			set
			{
				m_Font = value;
			}
		}

		/// <summary>
		/// テキストの配置 [初期値:TopLeft]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiString.Align")]
		[DefaultValue(typeof(ExGdiTextAlign), "TopLeft")]
		public virtual unsafe ExGdiTextAlign Align
		{
			get
			{
				return this.m_Align;
			}
			set
			{
				this.m_Align = value;
			}
		}

		/// <summary>
		/// テキスト
		/// </summary>
		[CxCategory("Information")]
		[CxDescription("P:XIE.GDI.CxGdiString.Text")]
		public virtual string Text
		{
			get
			{
				return m_Text;
			}
			set
			{
				if (value == null)
					m_Text = "";
				else
					m_Text = value;
			}
		}
	
		#endregion
	}
}
