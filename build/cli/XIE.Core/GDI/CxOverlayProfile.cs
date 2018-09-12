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
	/// オーバレイクラス (プロファイル表示)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxOverlayProfile : System.Object
		, IxModule
		, IDisposable
	{
		#region IxModule の実装: (ハンドル)

		[NonSerialized]
		private HxModule m_Handle = IntPtr.Zero;
		[NonSerialized]
		private bool m_IsDisposable = false;

		/// <summary>
		/// ハンドルの取得
		/// </summary>
		/// <returns>
		///		保有しているハンドルを返します。
		/// </returns>
		HxModule IxModule.GetHandle()
		{
			return m_Handle;
		}

		/// <summary>
		/// ハンドルの設定
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		void IxModule.SetHandle(HxModule handle, bool disposable)
		{
			m_Handle = handle;
			m_IsDisposable = disposable;
		}

		/// <summary>
		/// 破棄
		/// </summary>
		void IxModule.Destroy()
		{
			if (m_IsDisposable)
				m_Handle.Destroy();
			m_Handle = IntPtr.Zero;
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_high.fnXIE_GDI_Module_Create("CxOverlayProfile");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ (ハンドル指定)
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		private CxOverlayProfile(HxModule handle, bool disposable)
		{
			((IxModule)this).SetHandle(handle, disposable);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxOverlayProfile()
		{
			_Constructor();
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxOverlayProfile()
		{
			((IxModule)this).Destroy();
		}

		#endregion

		#region IDisposable の実装: (解放)

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			((IxModule)this).GetHandle().Dispose();
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
			get { return ((IxModule)this).GetHandle().IsValid(); }
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 可視属性
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxOverlayProfile.Visible")]
		public virtual bool Visible
		{
			get { return m_Visible; }
			set { m_Visible = value; }
		}
		private bool m_Visible = false;

		/// <summary>
		/// カーソル固定属性
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxOverlayProfile.IsFixed")]
		public bool IsFixed
		{
			get { return m_IsFixed; }
			set { m_IsFixed = value; }
		}
		private bool m_IsFixed = false;

		/// <summary>
		/// カーソル位置 (画像座標)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxOverlayProfile.CursorPosition")]
		public TxPointD CursorPosition
		{
			get { return m_CursorPosition; }
			set { m_CursorPosition = value; }
		}
		private TxPointD m_CursorPosition = new TxPointD();

		#endregion

		#region 描画:

		/// <summary>
		/// 描画処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Rendering(object sender, XIE.GDI.CxRenderingEventArgs e)
		{
			if (this.Visible == false) return;
			if (e.Image == null) return;
			if (e.Image.IsValid == false) return;

			try
			{
				var mag = e.Canvas.Magnification;
				var dsp = e.Canvas.DisplayRect();
				var vis = e.Canvas.VisibleRect();
				var eff = e.Canvas.EffectiveRect();

				var size = e.Image.ImageSize;
				var elements = size.Model.Pack * size.Channels;

				var channelNo = e.Canvas.ChannelNo;
				if (channelNo < 0)
					channelNo = 0;
				if (channelNo > elements - 1)
					channelNo = elements - 1;

				var unpack = e.Canvas.Unpack;
				if (unpack)
					elements = 1;

				var pos_x = (int)System.Math.Round(m_CursorPosition.X);
				var pos_y = (int)System.Math.Round(m_CursorPosition.Y);
				if (pos_x < 0)
					pos_x = 0;
				if (pos_y < 0)
					pos_y = 0;
				if (pos_x > size.Width - 1)
					pos_x = size.Width - 1;
				if (pos_y > size.Height - 1)
					pos_y = size.Height - 1;

				// ----------------------------------------------------------------------
				// Visualization
				/*
					RulerX         Cursor[0]
					 L M H           X
					 | | |           |
					-----------------+-------- Y: Cursor[1]
					 | | |           |
					 | | |           |
					-+-+-+-----------|-------- H: RulerY
					-+-+-+-----------|-------- M:
					-+-+-+-----------|-------- L:
				*/
				var colors = new TxRGB8x3[]
				{
					new TxRGB8x3(0xFF, 0x00, 0x00),	// red
					new TxRGB8x3(0x00, 0xFF, 0x00),	// green
					new TxRGB8x3(0x00, 0x00, 0xFF),	// blue
					new TxRGB8x3(0xFF, 0x00, 0xFF),	// magenta
					new TxRGB8x3(0xFF, 0xFF, 0x00),	// yellow
					new TxRGB8x3(0x00, 0xFF, 0xFF),	// cyan
				};

				var origin = new TxPointD(eff.X, eff.Y + eff.Height);

				var rulerL = new TxPointD(origin.X +  25.0, origin.Y -  25.0);
				var rulerM = new TxPointD(origin.X +  75.0, origin.Y -  75.0);
				var rulerH = new TxPointD(origin.X + 125.0, origin.Y - 125.0);

				var w_enough = (rulerH.X < (eff.X + eff.Width - 25.0));
				var h_enough = (rulerH.Y > (eff.Y + 25.0));

				var range = XIE.Axi.CalcRange(size.Model.Type, size.Depth);
				var range_diff = range.Upper - range.Lower;

				#region RulerX
				if (w_enough)
				{
					var figures = new CxGdiLineSegment[]
					{
						new CxGdiLineSegment(rulerL.X, eff.Y, rulerL.X, eff.Y + eff.Height),
						new CxGdiLineSegment(rulerM.X, eff.Y, rulerM.X, eff.Y + eff.Height),
						new CxGdiLineSegment(rulerH.X, eff.Y, rulerH.X, eff.Y + eff.Height),
					};
					var color = new TxRGB8x3(0x00, 0xFF, 0xFF);	// cyan

					for(int i=0 ; i<figures.Length ; i++)
					{
						figures[i].PenColor = color;
						figures[i].PenStyle = ExGdiPenStyle.Dot;
					}

					e.Canvas.DrawOverlay(figures, ExGdiScalingMode.None);
				}
				#endregion

				#region RulerY
				if (h_enough)
				{
					var figures = new CxGdiLineSegment[]
					{
						new CxGdiLineSegment(eff.X, rulerL.Y, eff.X + eff.Width, rulerL.Y),
						new CxGdiLineSegment(eff.X, rulerM.Y, eff.X + eff.Width, rulerM.Y),
						new CxGdiLineSegment(eff.X, rulerH.Y, eff.X + eff.Width, rulerH.Y),
					};
					var color = new TxRGB8x3(0x00, 0xFF, 0xFF);	// cyan

					for(int i=0 ; i<figures.Length ; i++)
					{
						figures[i].PenColor = color;
						figures[i].PenStyle = ExGdiPenStyle.Dot;
					}

					e.Canvas.DrawOverlay(figures, ExGdiScalingMode.None);
				}
				#endregion

				#region Profile
				if (w_enough && h_enough && range_diff > 0 && elements > 0)
				{
					var graphs_x = new CxArray[elements];
					var graphs_y = new CxArray[elements];
					var values = new double[elements];

					try
					{
						for (int i = 0; i < elements; i++)
						{
							graphs_x[i] = new CxArray();
							graphs_y[i] = new CxArray();
						}

						var hsrc = ((IxModule)e.Image).GetHandle();
						var canvas = e.Canvas.ToTxCanvas();
						var position = m_CursorPosition;
						ExStatus status = ExStatus.Impossible;
						using (var hgraphs_x = new CxArray(elements, TxModel.Ptr(1)))
						using (var hgraphs_y = new CxArray(elements, TxModel.Ptr(1)))
						using (var hvalues = new CxArray(elements, TxModel.F64(1)))
						{
							unsafe
							{
								var pgraphs_x = (void**)hgraphs_x.Address();
								var pgraphs_y = (void**)hgraphs_y.Address();
								var pvalues = (double*)hvalues.Address();

								for (int i = 0; i < elements; i++)
								{
									pgraphs_x[i] = ((IxModule)graphs_x[i]).GetHandle().ToPointer();
									pgraphs_y[i] = ((IxModule)graphs_y[i]).GetHandle().ToPointer();
									pvalues[i] = 0;
								}

								status = xie_high.fnXIE_GDI_Profile_MakeGraph(hsrc, canvas, position, hgraphs_x.Address(), hgraphs_y.Address(), hvalues.Address());

								for (int i = 0; i < elements; i++)
								{
									values[i] = pvalues[i];
								}
							}
						}
						if (status == ExStatus.Success)
						{
							#region Graph:
							{
								var figures_x = new List<CxGdiPolyline>();
								var figures_y = new List<CxGdiPolyline>();
								try
								{
									for (int i = 0; i < elements; i++)
									{
										var figure_x = new CxGdiPolyline(graphs_x[i]);
										var figure_y = new CxGdiPolyline(graphs_y[i]);
										{
											var color = (unpack)
												? colors[channelNo % colors.Length]
												: colors[i % colors.Length];
											figure_x.PenColor = color;
											figure_y.PenColor = color;
											figures_x.Add(figure_x);
											figures_y.Add(figure_y);
										}
									}
									e.Canvas.DrawOverlay(figures_x, ExGdiScalingMode.TopLeft);
									e.Canvas.DrawOverlay(figures_y, ExGdiScalingMode.TopLeft);
								}
								finally
								{
									#region 解放:
									foreach (var item in figures_x)
									{
										if (item is IDisposable)
											((IDisposable)item).Dispose();
									}
									foreach (var item in figures_y)
									{
										if (item is IDisposable)
											((IDisposable)item).Dispose();
									}
									#endregion
								}
							}
							#endregion

							#region Text
							{
								var figures = new CxGdiString[elements + 1];
								var color = new TxRGB8x3(0x00, 0xFF, 0x00);	// green
								double max_w = 0;
								double sum_h = 0;

								#region coordinate and the value of each element
								for (int i = 0; i < figures.Length; i++)
								{
									var figure = new CxGdiString();
									figures[i] = figure;

#if LINUX
									figure.Font = new TxGdiFont("", 12, FontStyle.Regular);
#else
									figure.Font = new TxGdiFont("Consolas", 9, FontStyle.Regular);
#endif

									if (i == 0)
									{
										figure.Text = string.Format(" X,Y = {0:F2}, {1:F2} ", m_CursorPosition.X, m_CursorPosition.Y);
									}
									else
									{
										var index = i - 1;
										var value = values[index];

										switch (size.Model.Type)
										{
											default:
												break;
											case ExType.U8:
											case ExType.U16:
											case ExType.U32:
											case ExType.U64:
											case ExType.S8:
											case ExType.S16:
											case ExType.S32:
											case ExType.S64:
												figure.Text = string.Format(" [{0}] {1:F0} ", index, value);
												break;
											case ExType.F32:
											case ExType.F64:
												figure.Text = string.Format(" [{0}] {1:F6} ", index, value);
												break;
										}
									}

									var bounds = figure.Bounds;
									if (max_w < bounds.Width)
										max_w = bounds.Width;
									sum_h += bounds.Height;
								}
								#endregion

								// background
								var dp = e.Canvas.IPtoDP(m_CursorPosition, ExGdiScalingMode.Center);
								var ave_h = sum_h / figures.Length;
								var background = new TxRectangleD(dp + 16, new TxSizeD(max_w + 16, sum_h + ave_h * 2));

								if (background.X + background.Width > dsp.X + dsp.Width)
									background.X = dp.X - 16 - background.Width;

								if (background.Y + background.Height > dsp.Y + dsp.Height)
									background.Y = dp.Y - 16 - background.Height;

								// text position
								for (int i = 0; i < figures.Length; i++)
								{
									figures[i].PenColor = color;
									figures[i].Align = ExGdiTextAlign.TopLeft;
									figures[i].Location = new TxPointD(background.X, background.Y + ave_h * (i + 1));
								}

								// draw background
								{
									var figure = new CxGdiRectangle();
									var bg_color = new TxRGB8x4(32, 32, 32, 192);	// black (alpha=75%)
									figure.Shape = background;
									figure.PenColor = bg_color;
									figure.Brush = new TxGdiBrush(bg_color);

									e.Canvas.DrawOverlay(figure, ExGdiScalingMode.None);
								}

								// draw texts
								e.Canvas.DrawOverlay(figures, ExGdiScalingMode.None);

								foreach (var item in figures)
								{
									if (item is IDisposable)
										((IDisposable)item).Dispose();
								}
							}
							#endregion
						}
					}
					finally
					{
						#region 解放:
						foreach (var item in graphs_x)
						{
							if (item is IDisposable)
								((IDisposable)item).Dispose();
						}
						foreach (var item in graphs_y)
						{
							if (item is IDisposable)
								((IDisposable)item).Dispose();
						}
						#endregion
					}
				}
				#endregion

				#region Cursor
				{
					var dp = e.Canvas.IPtoDP(m_CursorPosition, ExGdiScalingMode.Center);
					var figures = new CxGdiLineSegment[] 
					{
						new CxGdiLineSegment(dp.X, eff.Y, dp.X, eff.Y + eff.Height),
						new CxGdiLineSegment(eff.X, dp.Y, eff.X + eff.Width, dp.Y),
					};
					var color = new TxRGB8x3(0xFF, 0xFF, 0x00);	// yellow

					for(int i=0 ; i<figures.Length ; i++)
					{
						figures[i].PenColor = color;
					}

					e.Canvas.DrawOverlay(figures, ExGdiScalingMode.None);
				}
				#endregion
			}
			catch (System.Exception)
			{
			}
		}

		#endregion

		#region 操作:

		/// <summary>
		/// 図形操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Handling(object sender, XIE.GDI.CxHandlingEventArgs e)
		{
			if (this.Visible == false) return;
			if (this.IsFixed == true) return;

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
						break;
					case XIE.GDI.ExHandlingEventReason.MouseLeave:
						break;
					case XIE.GDI.ExHandlingEventReason.MouseUp:
						break;
					case XIE.GDI.ExHandlingEventReason.MouseDown:
						break;
					case XIE.GDI.ExHandlingEventReason.MouseMove:
						if (sender is Control)
						{
							var control = (Control)sender;
							TxPointD dp = control.PointToClient(Control.MousePosition);
							this.CursorPosition = e.Canvas.DPtoIP(dp, ExGdiScalingMode.Center);
							e.IsUpdated = true;
						}
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

		#endregion
	}
}
