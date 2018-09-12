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
	/// オーバレイクラス (ROI 表示/操作)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxOverlayROI : System.Object
		, IDisposable
	{
		#region フィールド:

		/// <summary>
		/// ペン
		/// </summary>
		public TxGdiPen Pen;

		/// <summary>
		/// ブラシ
		/// </summary>
		public TxGdiBrush Brush;

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxOverlayROI()
		{
			this.Pen = new TxGdiPen(new TxRGB8x4(128, 128, 255), 0, ExGdiPenStyle.Solid);
			this.Brush = new TxGdiBrush(new TxRGB8x4(128, 128, 255, 32));
		}

		#endregion

		#region IDisposable の実装: (解放)

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
		}

		/// <summary>
		/// 有効性 [初期値:false]
		/// </summary>
		/// <returns>
		///		現在のオブジェクトの内部リソースが有効な場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool IsValid
		{
			get
			{
				return true;
			}
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 可視属性
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxOverlayROI.Visible")]
		public virtual bool Visible
		{
			get { return m_Visible; }
			set { m_Visible = value; }
		}
		private bool m_Visible = false;

		/// <summary>
		/// 固定属性
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxOverlayROI.IsFixed")]
		public bool IsFixed
		{
			get { return m_IsFixed; }
			set { m_IsFixed = value; }
		}
		private bool m_IsFixed = false;

		/// <summary>
		/// オーバレイ図形の形状
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxGdiRectangle.Shape")]
		public virtual TxRectangleD Shape
		{
			get { return m_Shape; }
			set { m_Shape = value; }
		}
		private TxRectangleD m_Shape = new TxRectangleD();

		#endregion

		#region プロパティ: (描画色)

		/// <summary>
		/// ペン色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxOverlayROI.PenColor")]
		public TxRGB8x4 PenColor
		{
			get { return Pen.Color; }
			set { Pen.Color = value; }
		}

		/// <summary>
		/// ペン幅
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxOverlayROI.PenWidth")]
		public float PenWidth
		{
			get { return Pen.Width; }
			set { Pen.Width = value; }
		}

		/// <summary>
		/// ペン形状
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxOverlayROI.PenStyle")]
		public ExGdiPenStyle PenStyle
		{
			get { return Pen.Style; }
			set { Pen.Style = value; }
		}

		/// <summary>
		/// ブラシ前景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxOverlayROI.BrushColor")]
		public TxRGB8x4 BrushColor
		{
			get { return Brush.Color; }
			set { Brush.Color = value; }
		}

		/// <summary>
		/// ブラシ背景色
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxOverlayROI.BrushShadow")]
		public TxRGB8x4 BrushShadow
		{
			get { return Brush.Shadow; }
			set { Brush.Shadow = value; }
		}

		/// <summary>
		/// ブラシスタイル
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxOverlayROI.BrushStyle")]
		public ExGdiBrushStyle BrushStyle
		{
			get { return Brush.Style; }
			set { Brush.Style = value; }
		}

		/// <summary>
		/// ハッチスタイル (Style が Hatch の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxOverlayROI.HatchStyle")]
		public System.Drawing.Drawing2D.HatchStyle HatchStyle
		{
			get { return Brush.HatchStyle; }
			set { Brush.HatchStyle = value; }
		}

		/// <summary>
		/// 線形グラデーションモード (Style が LinearGradient の時に使用します。それ以外は無視します。)
		/// </summary>
		[CxCategory("Common")]
		[CxDescription("P:XIE.GDI.CxOverlayROI.LinearGradientMode")]
		public System.Drawing.Drawing2D.LinearGradientMode LinearGradientMode
		{
			get { return Brush.LinearGradientMode; }
			set { Brush.LinearGradientMode = value; }
		}

		#endregion

		#region プロパティ: (Options)

		/// <summary>
		/// グリッド表示属性
		/// </summary>
		[CxCategory("Options")]
		[CxDescription("P:XIE.GDI.CxOverlayROI.Grid")]
		public virtual bool Grid
		{
			get { return m_Grid; }
			set { m_Grid = value; }
		}
		private bool m_Grid = false;

		/// <summary>
		/// グリッド種別 [0:３分割、1:４分割]
		/// </summary>
		[CxCategory("Options")]
		[CxDescription("P:XIE.GDI.CxOverlayROI.GridType")]
		public virtual int GridType
		{
			get { return m_GridType; }
			set { m_GridType = value; }
		}
		private int m_GridType = 0;

		#endregion

		#region 関数:

		/// <summary>
		/// 有効性検査
		/// </summary>
		public virtual bool CheckValidity(XIE.CxImage image)
		{
			if (image == null) return false;
			var roi = (XIE.TxRectangleI)this.Shape;
			var st = (XIE.TxPointI)roi.Location;
			var ed = (XIE.TxPointI)(roi.Location + roi.Size);

			if (!(0 <= st.X && st.X < image.Width)) return false;
			if (!(0 <= st.Y && st.Y < image.Height)) return false;
			if (!(0 < ed.X && ed.X <= image.Width)) return false;
			if (!(0 < ed.Y && ed.Y <= image.Height)) return false;
			if (!(roi.Width > 0)) return false;
			if (!(roi.Height > 0)) return false;
			return true;
		}

		#endregion

		#region 描画:

		/// <summary>
		/// 表示更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			if (this.Visible == false) return;
			if (e.Image == null) return;

			try
			{
				var figures = new List<XIE.GDI.IxGdi2d>();

				// 枠:
				var frame = new CxGdiRectangle(this.Shape);
				{
					frame.Pen = this.Pen;
					frame.Brush = this.Brush;
				}
				figures.Add(frame);

				#region グリッド表示:
				if (this.Grid)
				{
					switch (this.GridType)
					{
						default:
						case 0:
							// ３分割.
							{
								var rect = frame.Shape;
								var xs = rect.X;
								var xe = rect.X + rect.Width;
								var ys = rect.Y;
								var ye = rect.Y + rect.Height;
								var gw = rect.Size.Width / 3;
								var gh = rect.Size.Height / 3;

								var grid = new XIE.GDI.CxGdiLineSegment[4]
								{
									new XIE.GDI.CxGdiLineSegment(xs + gw * 1, ys, xs + gw * 1, ye),
									new XIE.GDI.CxGdiLineSegment(xs + gw * 2, ys, xs + gw * 2, ye),
									new XIE.GDI.CxGdiLineSegment(xs, ys + gh * 1, xe, ys + gh * 1),
									new XIE.GDI.CxGdiLineSegment(xs, ys + gh * 2, xe, ys + gh * 2),
								};
								for (int i = 0; i < grid.Length; i++)
								{
									grid[i].Pen = frame.Pen;
								}

								foreach(var item in grid)
									figures.Add(item);
							}
							break;
						case 1:
							// ４分割.
							{
								var rect = frame.Shape;
								var xs = rect.X;
								var xe = rect.X + rect.Width;
								var ys = rect.Y;
								var ye = rect.Y + rect.Height;
								var gw = rect.Size.Width / 4;
								var gh = rect.Size.Height / 4;

								var grid = new XIE.GDI.CxGdiLineSegment[6]
								{
									new XIE.GDI.CxGdiLineSegment(xs + gw * 1, ys, xs + gw * 1, ye),
									new XIE.GDI.CxGdiLineSegment(xs + gw * 2, ys, xs + gw * 2, ye),
									new XIE.GDI.CxGdiLineSegment(xs + gw * 3, ys, xs + gw * 3, ye),
									new XIE.GDI.CxGdiLineSegment(xs, ys + gh * 1, xe, ys + gh * 1),
									new XIE.GDI.CxGdiLineSegment(xs, ys + gh * 2, xe, ys + gh * 2),
									new XIE.GDI.CxGdiLineSegment(xs, ys + gh * 3, xe, ys + gh * 3),
								};
								for (int i = 0; i < grid.Length; i++)
								{
									grid[i].Pen = frame.Pen;
								}

								foreach (var item in grid)
									figures.Add(item);
							}
							break;
					}
				}
				#endregion

				// Draw
				e.Canvas.DrawOverlay(figures, XIE.GDI.ExGdiScalingMode.TopLeft);
			}
			catch (System.Exception)
			{
			}
		}

		#endregion

		#region 操作:

		/// <summary>
		/// 掴んだ位置 (矩形上の位置)
		/// </summary>
		private XIE.GDI.TxHitPosition ROI_GripStatus = XIE.GDI.TxHitPosition.Default;

		/// <summary>
		/// 掴んだ位置 (画像座標)
		/// </summary>
		private XIE.TxPointI ROI_GripPoint = new XIE.TxPointI();

		/// <summary>
		/// 掴んだ際の矩形 (画像座標)
		/// </summary>
		private XIE.TxRectangleD ROI_GripRect = new XIE.TxRectangleD();

		/// <summary>
		/// 図形操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Handling(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			if (this.Visible == false) return;
			if (this.IsFixed == true) return;
			if (e.IsGripped) return;	// 既に誰かがマウス操作している.

			try
			{
				#region 分岐:
				switch (e.Reason)
				{
					default:
					case XIE.GDI.ExHandlingEventReason.None:
						break;
					case XIE.GDI.ExHandlingEventReason.PreviewKeyDown:
						break;
					case XIE.GDI.ExHandlingEventReason.MouseEnter:
						ROI_GripStatus = XIE.GDI.TxHitPosition.Default;
						break;
					case XIE.GDI.ExHandlingEventReason.MouseLeave:
						ROI_GripStatus = XIE.GDI.TxHitPosition.Default;
						break;
					case XIE.GDI.ExHandlingEventReason.MouseUp:
						ROI_GripStatus = XIE.GDI.TxHitPosition.Default;
						break;
					case XIE.GDI.ExHandlingEventReason.MouseDown:
						ROI_Handling_MouseDown(sender, e);
						break;
					case XIE.GDI.ExHandlingEventReason.MouseMove:
						ROI_Handling_MouseMove(sender, e);
						break;
					case XIE.GDI.ExHandlingEventReason.MouseHover:
						break;
					case XIE.GDI.ExHandlingEventReason.MouseWheel:
						break;
				}
				#endregion
			}
			catch (System.Exception)
			{
			}
		}

		/// <summary>
		/// 図形操作 (MouseDown)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ROI_Handling_MouseDown(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			var view = (XIE.GDI.CxImageView)sender;
			XIE.TxPointI dp = e.MouseEventArgs.Location;
			XIE.TxPointI ip = view.DPtoIP(dp, XIE.GDI.ExGdiScalingMode.TopLeft);

			ROI_GripStatus = ROI_CheckRectPosition(sender, e);
			ROI_GripPoint = ip;
			ROI_GripRect = this.Shape;

			e.IsGripped = (ROI_GripStatus.Mode != 0);
		}

		/// <summary>
		/// 図形操作 (MouseMove)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ROI_Handling_MouseMove(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			if (e.IsGripped) return;

			var view = (XIE.GDI.CxImageView)sender;
			XIE.TxPointI dp = e.MouseEventArgs.Location;
			XIE.TxPointI ip = view.DPtoIP(dp, XIE.GDI.ExGdiScalingMode.TopLeft);

			#region 移動またはサイズ変更:
			switch (ROI_GripStatus.Mode)
			{
				default:
				case 0:
					{
						var hit = ROI_CheckRectPosition(sender, e);
						e.IsGripped = (hit.Mode != 0);
					}
					break;
				case 1:
					{
						#region 移動:
						XIE.TxPointD st = ROI_GripRect.Location + (TxPointD)(ip - ROI_GripPoint);
						XIE.TxPointD ed = st + ROI_GripRect.Size;
						#endregion

						#region 上下限:
						double limit_ex = view.Image.Width;
						double limit_ey = view.Image.Height;
						if (ed.X > limit_ex)
						{
							st.X -= (ed.X - limit_ex);
							ed.X = limit_ex;
						}
						if (ed.Y > limit_ey)
						{
							st.Y -= (ed.Y - limit_ey);
							ed.Y = limit_ey;
						}
						if (st.X < 0)
						{
							ed.X -= st.X;
							st.X = 0;
						}
						if (st.Y < 0)
						{
							ed.Y -= st.Y;
							st.Y = 0;
						}
						if (ed.X > limit_ex)
						{
							ed.X = limit_ex;
						}
						if (ed.Y > limit_ey)
						{
							ed.Y = limit_ey;
						}
						#endregion

						#region 反映:
						this.Shape = new TxRectangleD(st.X, st.Y, (ed.X - st.X), (ed.Y - st.Y));
						e.IsGripped = true;
						e.Cursor = Cursors.SizeAll;
						view.Refresh();
						#endregion
					}
					break;
				case 2:
					switch (ROI_GripStatus.Site)
					{
						case -4:
							{
								#region サイズ変更:
								XIE.TxPointD st = ROI_GripRect.Location;
								XIE.TxPointD ed = ROI_GripRect.Location + ROI_GripRect.Size;
								st.X += (double)(ip.X - ROI_GripPoint.X);
								#endregion

								#region 上下限:
								if (st.X < 0)
									st.X = 0;
								if (st.X > ed.X - 2)
									st.X = ed.X - 2;
								#endregion

								#region 反映:
								this.Shape = new TxRectangleD(st.X, st.Y, (ed.X - st.X), (ed.Y - st.Y));
								e.IsGripped = true;
								e.Cursor = Cursors.SizeWE;
								view.Refresh();
								#endregion
							}
							break;
						case -1:
							{
								#region サイズ変更:
								XIE.TxPointD st = ROI_GripRect.Location;
								XIE.TxPointD ed = ROI_GripRect.Location + ROI_GripRect.Size;
								st.Y += (double)(ip.Y - ROI_GripPoint.Y);
								#endregion

								#region 上下限:
								if (st.Y < 0)
									st.Y = 0;
								if (st.Y > ed.Y - 2)
									st.Y = ed.Y - 2;
								#endregion

								#region 反映:
								this.Shape = new TxRectangleD(st.X, st.Y, (ed.X - st.X), (ed.Y - st.Y));
								e.IsGripped = true;
								e.Cursor = Cursors.SizeNS;
								view.Refresh();
								#endregion
							}
							break;
						case -2:
							{
								#region サイズ変更:
								XIE.TxPointD st = ROI_GripRect.Location;
								XIE.TxPointD ed = ROI_GripRect.Location + ROI_GripRect.Size;
								ed.X += (double)(ip.X - ROI_GripPoint.X);
								#endregion

								#region 上下限:
								if (ed.X < st.X + 2)
									ed.X = st.X + 2;
								if (ed.X > view.Image.Width)
									ed.X = view.Image.Width;
								#endregion

								#region 反映:
								this.Shape = new TxRectangleD(st.X, st.Y, (ed.X - st.X), (ed.Y - st.Y));
								e.IsGripped = true;
								e.Cursor = Cursors.SizeWE;
								view.Refresh();
								#endregion
							}
							break;
						case -3:
							{
								#region サイズ変更:
								XIE.TxPointD st = ROI_GripRect.Location;
								XIE.TxPointD ed = ROI_GripRect.Location + ROI_GripRect.Size;
								ed.Y += (double)(ip.Y - ROI_GripPoint.Y);
								#endregion

								#region 上下限:
								if (ed.Y < st.Y + 2)
									ed.Y = st.Y + 2;
								if (ed.Y > view.Image.Height)
									ed.Y = view.Image.Height;
								#endregion

								#region 反映:
								this.Shape = new TxRectangleD(st.X, st.Y, (ed.X - st.X), (ed.Y - st.Y));
								e.IsGripped = true;
								e.Cursor = Cursors.SizeNS;
								view.Refresh();
								#endregion
							}
							break;
						case +1:
							{
								#region サイズ変更:
								XIE.TxPointD st = ROI_GripRect.Location + (XIE.TxPointD)(ip - ROI_GripPoint);
								XIE.TxPointD ed = ROI_GripRect.Location + ROI_GripRect.Size;
								#endregion

								#region 上下限:
								if (st.X < 0)
									st.X = 0;
								if (st.Y < 0)
									st.Y = 0;
								if (st.X > ed.X - 2)
									st.X = ed.X - 2;
								if (st.Y > ed.Y - 2)
									st.Y = ed.Y - 2;
								#endregion

								#region 反映:
								this.Shape = new TxRectangleD(st.X, st.Y, (ed.X - st.X), (ed.Y - st.Y));
								e.IsGripped = true;
								e.Cursor = Cursors.SizeNWSE;
								view.Refresh();
								#endregion
							}
							break;
						case +2:
							{
								#region サイズ変更:
								XIE.TxPointD st = ROI_GripRect.Location;
								XIE.TxPointD ed = ROI_GripRect.Location + ROI_GripRect.Size;
								st.Y += (double)(ip.Y - ROI_GripPoint.Y);
								ed.X += (double)(ip.X - ROI_GripPoint.X);
								#endregion

								#region 上下限:
								if (st.Y < 0)
									st.Y = 0;
								if (st.Y > ed.Y - 2)
									st.Y = ed.Y - 2;
								if (ed.X < st.X + 2)
									ed.X = st.X + 2;
								if (ed.X > view.Image.Width)
									ed.X = view.Image.Width;
								#endregion

								#region 反映:
								this.Shape = new TxRectangleD(st.X, st.Y, (ed.X - st.X), (ed.Y - st.Y));
								e.IsGripped = true;
								e.Cursor = Cursors.SizeNESW;
								view.Refresh();
								#endregion
							}
							break;
						case +3:
							{
								#region サイズ変更:
								XIE.TxPointD st = ROI_GripRect.Location;
								XIE.TxPointD ed = ROI_GripRect.Location + ROI_GripRect.Size + (XIE.TxPointD)(ip - ROI_GripPoint);
								#endregion

								#region 上下限:
								if (ed.X < st.X + 2)
									ed.X = st.X + 2;
								if (ed.Y < st.Y + 2)
									ed.Y = st.Y + 2;
								if (ed.X > view.Image.Width)
									ed.X = view.Image.Width;
								if (ed.Y > view.Image.Height)
									ed.Y = view.Image.Height;
								#endregion

								#region 反映:
								this.Shape = new TxRectangleD(st.X, st.Y, (ed.X - st.X), (ed.Y - st.Y));
								e.IsGripped = true;
								e.Cursor = Cursors.SizeNWSE;
								view.Refresh();
								#endregion
							}
							break;
						case +4:
							{
								#region サイズ変更:
								XIE.TxPointD st = ROI_GripRect.Location;
								XIE.TxPointD ed = ROI_GripRect.Location + ROI_GripRect.Size;
								st.X += (double)(ip.X - ROI_GripPoint.X);
								ed.Y += (double)(ip.Y - ROI_GripPoint.Y);
								#endregion

								#region 上下限:
								if (st.X < 0)
									st.X = 0;
								if (st.X > ed.X - 2)
									st.X = ed.X - 2;
								if (ed.Y < st.Y + 2)
									ed.Y = st.Y + 2;
								if (ed.Y > view.Image.Height)
									ed.Y = view.Image.Height;
								#endregion

								#region 反映:
								this.Shape = new TxRectangleD(st.X, st.Y, (ed.X - st.X), (ed.Y - st.Y));
								e.IsGripped = true;
								e.Cursor = Cursors.SizeNESW;
								view.Refresh();
								#endregion
							}
							break;
					}
					break;
			}
			#endregion
		}

		/// <summary>
		/// 図形操作判定
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <returns>
		///		マウスが ROI の範囲外であれば、TxHitPosition.Default を返します。
		///		マウスが ROI の範囲内であれば、それ以外を返します。
		/// </returns>
		private XIE.GDI.TxHitPosition ROI_CheckRectPosition(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			var view = (XIE.GDI.CxImageView)sender;
			XIE.TxPointI dp = e.MouseEventArgs.Location;
			XIE.TxPointD ip = view.DPtoIP(dp, XIE.GDI.ExGdiScalingMode.TopLeft);
			double margin = 8 / view.Magnification;

			var hit = CxGdiRectangle.HitTestInternal(ip, margin, this.Shape);

			#region カーソル変更:
			switch (hit.Mode)
			{
				default:
				case 0:
					break;
				case 1:
					e.Cursor = Cursors.SizeAll;
					break;
				case 2:
					switch (hit.Site)
					{
						case -1: e.Cursor = Cursors.SizeNS; break;
						case -2: e.Cursor = Cursors.SizeWE; break;
						case -3: e.Cursor = Cursors.SizeNS; break;
						case -4: e.Cursor = Cursors.SizeWE; break;
						case +1: e.Cursor = Cursors.SizeNWSE; break;
						case +2: e.Cursor = Cursors.SizeNESW; break;
						case +3: e.Cursor = Cursors.SizeNWSE; break;
						case +4: e.Cursor = Cursors.SizeNESW; break;
					}
					break;
			}
			#endregion

			return hit;
		}

		#endregion
	}
}
