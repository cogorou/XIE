/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.CodeDom;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using XIE.Ptr;

namespace XIE.GDI
{
	/// <summary>
	/// 画像ビュー
	/// </summary>
	[Serializable]
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(CxImageView))]
	[Description("ImageView")]
	public class CxImageView : System.Windows.Forms.UserControl
	{
		#region フィールド:

		private TxGdiBrush m_Background;
		private double m_Magnification;
		private TxPointD m_ViewPoint;
		private int m_ChannelNo;
		private bool m_Unpack;
		private bool m_Halftone;

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			this.DoubleBuffered = true;
			this.ResizeRedraw = true;

			m_Background = new TxGdiBrush(new TxRGB8x4(0x00, 0x00, 0x00));
			m_Magnification = 1.0;
			m_ViewPoint = new TxPointD();
			m_ChannelNo = 0;
			m_Unpack = false;
			m_Halftone = false;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImageView()
		{
			_Constructor();

			this.MouseDown += Control_MouseDown;
			this.MouseUp += Control_MouseUp;
			this.MouseMove += Control_MouseMove;
			this.MouseHover += Control_MouseHover;
			this.MouseEnter += Control_MouseEnter;
			this.MouseLeave += Control_MouseLeave;
			this.MouseWheel += Control_MouseWheel;
			this.PreviewKeyDown += Control_PreviewKeyDown;
		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxImageView()
		{
			this.MouseDown -= Control_MouseDown;
			this.MouseUp -= Control_MouseUp;
			this.MouseMove -= Control_MouseMove;
			this.MouseHover -= Control_MouseHover;
			this.MouseEnter -= Control_MouseEnter;
			this.MouseLeave -= Control_MouseLeave;
			this.MouseWheel -= Control_MouseWheel;
			this.PreviewKeyDown -= Control_PreviewKeyDown;

			this.Dispose();
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// リソースの解放
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (this.Buffer != null)
				this.Buffer.Dispose();
			this.Buffer = null;

			base.Dispose(disposing);
		}

		#endregion

		#region プロパティ: (Parameters)

		/// <summary>
		/// 表示対象画像 [初期値:null]
		/// </summary>
		[XmlIgnore]
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxImageView.Image")]
		public CxImage Image
		{
			get { return m_Image; }
			set { m_Image = value; }
		}
		[NonSerialized]
		private CxImage m_Image = null;

		/// <summary>
		/// 表示対象画像の描画用バッファ
		/// </summary>
		[NonSerialized]
		private CxImage Buffer = null;

		/// <summary>
		/// 背景塗り潰し用ブラシ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxImageView.BackgroundBrush")]
		public TxGdiBrush BackgroundBrush
		{
			get { return m_Background; }
			set { m_Background = value; }
		}

		/// <summary>
		/// 表示倍率 [範囲:0.0 より大きい値]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxImageView.Magnification")]
		public virtual double Magnification
		{
			get { return m_Magnification; }
			set
			{
				if (value <= 0)
					return;
				m_Magnification = value;
			}
		}

		/// <summary>
		/// 視点 (pixels)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxImageView.ViewPoint")]
		public virtual TxPointD ViewPoint
		{
			get { return m_ViewPoint; }
			set { m_ViewPoint = value; }
		}

		/// <summary>
		/// 表示対象チャネル指標 [範囲:0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxImageView.ChannelNo")]
		public virtual int ChannelNo
		{
			get { return m_ChannelNo; }
			set { m_ChannelNo = value; }
		}

		/// <summary>
		/// アンパック表示の指示
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxImageView.Unpack")]
		public virtual bool Unpack
		{
			get { return m_Unpack; }
			set { m_Unpack = value; }
		}

		/// <summary>
		/// ハーフトーン
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxImageView.Halftone")]
		public virtual bool Halftone
		{
			get { return m_Halftone; }
			set { m_Halftone = value; }
		}

		#endregion

		#region プロパティ: (Options)

		/// <summary>
		/// マウス押下で背景画像を掴みスクロールする機能の有効属性
		/// </summary>
		[CxCategory("Options")]
		[CxDescription("P:XIE.GDI.CxImageView.EnableMouseGrip")]
		public virtual bool EnableMouseGrip
		{
			get { return m_EnableMouseGrip; }
			set { m_EnableMouseGrip = value; }
		}
		private bool m_EnableMouseGrip = true;

		/// <summary>
		/// マウスホイールで背景画像をスクロールする機能の有効属性
		/// </summary>
		[CxCategory("Options")]
		[CxDescription("P:XIE.GDI.CxImageView.EnableWheelScroll")]
		public virtual bool EnableWheelScroll
		{
			get { return m_EnableWheelScroll; }
			set { m_EnableWheelScroll = value; }
		}
		private bool m_EnableWheelScroll = true;

		/// <summary>
		/// マウスホイールで背景画像の表示倍率を変更する機能の有効属性
		/// </summary>
		[CxCategory("Options")]
		[CxDescription("P:XIE.GDI.CxImageView.EnableWheelScaling")]
		public virtual bool EnableWheelScaling
		{
			get { return m_EnableWheelScaling; }
			set { m_EnableWheelScaling = value; }
		}
		private bool m_EnableWheelScaling = true;

		#endregion

		#region メソッド: (Information)

		/// <summary>
		/// 表示領域のサイズを計算します。
		/// </summary>
		/// <returns>
		///		表示領域のサイズを返します。
		///		計算できない場合はサイズ 0,0 を返します。
		/// </returns>
		public virtual TxSizeI DisplaySize()
		{
			return new TxSizeI(this.Width, this.Height);
		}

		/// <summary>
		/// 表示領域の範囲を計算します。
		/// </summary>
		/// <returns>
		///		表示領域の範囲を返します。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </returns>
		public virtual TxRectangleI DisplayRect()
		{
			return new TxRectangleI(0, 0, this.Width, this.Height);
		}

		/// <summary>
		/// 有効範囲を計算します。
		/// </summary>
		/// <returns>
		///		画像が表示される範囲を返します。
		///		表示倍率を乗算した画像サイズが表示範囲より小さい場合はセンタリングされます。
		///		それ以外は、表示範囲と同一です。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </returns>
		public virtual TxRectangleI EffectiveRect()
		{
			if (this.Image == null)
				return new TxRectangleD();

			var display_rect = this.DisplayRect();
			var bg_size = this.Image.Size;
			var mag = this.Magnification;

			return CxCanvas.EffectiveRect(display_rect, bg_size, mag);
		}

		/// <summary>
		/// 可視範囲を計算します。
		/// </summary>
		/// <returns>
		///		指定された範囲内に表示される画像の範囲を返します。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </returns>
		public virtual TxRectangleD VisibleRect()
		{
			if (this.Image == null)
				return new TxRectangleD();

			var display_rect = this.DisplayRect();
			var bg_size = this.Image.Size;
			var mag = this.Magnification;
			var view_point = this.ViewPoint;

			return CxCanvas.VisibleRect(display_rect, bg_size, mag, view_point);
		}

		/// <summary>
		/// 可視範囲を計算します。
		/// </summary>
		/// <param name="includePartialPixel">画面淵の端数画素を含むか否か [true=切り上げ、false=切り捨て]</param>
		/// <returns>
		///		指定された範囲内に表示される画像の範囲を返します。
		///		計算できない場合は位置・サイズともに 0 の矩形を返します。
		/// </returns>
		public virtual TxRectangleI VisibleRectI(bool includePartialPixel)
		{
			if (this.Image == null)
				return new TxRectangleD();

			var display_rect = this.DisplayRect();
			var bg_size = this.Image.Size;
			var mag = this.Magnification;
			var view_point = this.ViewPoint;

			return CxCanvas.VisibleRectI(display_rect, bg_size, mag, view_point, includePartialPixel);
		}

		/// <summary>
		/// ディスプレイ座標を画像座標に変換します。
		/// </summary>
		/// <param name="dp">ディスプレイ座標</param>
		/// <param name="mode">スケーリングモード</param>
		/// <returns>
		///		変換後の座標を返します。
		///		計算できない場合は 0,0 を返します。
		/// </returns>
		public virtual TxPointD DPtoIP(TxPointD dp, ExGdiScalingMode mode)
		{
			if (this.Image == null)
				return new TxPointD();

			return CxCanvas.DPtoIP(DisplayRect(), this.Image.Size, Magnification, ViewPoint, dp, mode);
		}

		/// <summary>
		/// 画像座標をディスプレイ座標に変換します。
		/// </summary>
		/// <param name="ip">画像座標</param>
		/// <param name="mode">スケーリングモード</param>
		/// <returns>
		///		変換後の座標を返します。
		///		計算できない場合は 0,0 を返します。
		/// </returns>
		public virtual TxPointD IPtoDP(TxPointD ip, ExGdiScalingMode mode)
		{
			if (this.Image == null)
				return new TxPointD();

			return CxCanvas.IPtoDP(DisplayRect(), this.Image.Size, Magnification, ViewPoint, ip, mode);
		}

		#endregion

		#region メソッド: (キャンバス情報の取得)

		/// <summary>
		/// キャンバス情報の取得
		/// </summary>
		/// <returns>
		///		現在の情報を CxCanvasInfo オブジェクトに設定して返します。
		/// </returns>
		public virtual CxCanvasInfo GetCanvasInfo()
		{
			var args = new CxCanvasInfo();
			args.Width = this.Width;
			args.Height = this.Height;
			args.ImageSize = (this.Image != null && this.Image.IsValid)
				? this.Image.Size
				: this.DisplaySize();
			args.BackgroundBrush = this.BackgroundBrush;
			args.Magnification = this.Magnification;
			args.ViewPoint = this.ViewPoint;
			args.ChannelNo = this.ChannelNo;
			args.Unpack = this.Unpack;
			args.Halftone = this.Halftone;
			return args;
		}

		/// <summary>
		/// キャンバス構造体への変換
		/// </summary>
		/// <returns>
		///		現在の情報を TxCanvas 構造体に設定して返します。
		/// </returns>
		public TxCanvas ToTxCanvas()
		{
			var tag = new TxCanvas();
			tag.Width = this.Width;
			tag.Height = this.Height;
			tag.BgSize = (this.Image != null && this.Image.IsValid)
				? this.Image.Size
				: this.DisplaySize();
			tag.BkColor = this.BackgroundBrush.Color;
			tag.BkEnable = (this.BackgroundBrush.Style != ExGdiBrushStyle.None) ? ExBoolean.True : ExBoolean.False;
			tag.Magnification = this.Magnification;
			tag.ViewPoint = this.ViewPoint;
			tag.ChannelNo = this.ChannelNo;
			tag.Unpack = this.Unpack ? ExBoolean.True : ExBoolean.False;
			tag.Halftone = this.Halftone ? ExBoolean.True : ExBoolean.False;
			return tag;
		}

		#endregion

		#region メソッド: (表示倍率調整)

		/// <summary>
		/// 表示倍率調整。(画像の幅または高さが画像ビューに収まるように調整します。)
		/// </summary>
		/// <param name="mode">動作モード [0:幅または高さの何れかに合わせる。1:幅に合わせる。2:高さに合わせる。]</param>
		/// <param name="ratio">適用率 (%) [1~100] ※画像ビューより小さめに表示する場合は、この値を 1~99 の間で調整してください。</param>
		public virtual void FitImageSize(int mode, int ratio = 100)
		{
			if (this.Image == null) return;
			if (this.Image.IsValid == false) return;

			var client_size = (TxSizeI)this.ClientSize;
			if (client_size.Width <= 0) return;
			if (client_size.Height <= 0) return;

			Size image_size = this.Image.Size;
			if (image_size.Width <= 0) return;
			if (image_size.Height <= 0) return;

			var mag_x = (double)client_size.Width / image_size.Width;
			var mag_y = (double)client_size.Height / image_size.Height;

			double mag = 0;
			switch (mode)
			{
				default:
				case 0: mag = System.Math.Min(mag_x, mag_y); break;
				case 1: mag = mag_x; break;
				case 2: mag = mag_y; break;
			}
			if (1 <= ratio && ratio <= 100)
				mag = mag * ratio / 100;
			if (mag < 0.001)
				mag = 0.001;
			this.Magnification = mag;
		}

		/// <summary>
		/// 表示倍率調整。(縮小または拡大します。)
		/// </summary>
		/// <param name="mode">動作モード [0:原寸、-1:縮小、+1:拡大]</param>
		public virtual void AdjustScale(int mode)
		{
			int scale = (int)System.Math.Round(this.Magnification * 100);
			switch (mode)
			{
				default:
				case 0:
					scale = 100;
					break;
				case -1:
					if (scale <= 1)
						scale = 1;
					else if (scale <= 10)
						scale = scale - 1;
					else if (scale <= 40)
						scale = ((scale + 4) * 2 / 10 * 10 / 2) - 5;
					else if (scale <= 100)
						scale = ((scale + 9) / 10 * 10) - 10;
					else if (scale <= 400)
						scale = ((scale + 49) * 2 / 100 * 100 / 2) - 50;
					else
						scale = ((scale + 99) / 100 * 100) - 100;
					break;
				case +1:
					if (scale >= 400)
						scale = ((scale) / 100 * 100) + 100;
					else if (scale >= 100)
						scale = ((scale) * 2 / 100 * 100 / 2) + 50;
					else if (scale >= 40)
						scale = ((scale) / 10 * 10) + 10;
					else if (scale >= 10)
						scale = ((scale) * 2 / 10 * 10 / 2) + 5;
					else
						scale = scale + 1;
					break;
			}
			this.Magnification = scale * 0.01;
		}

		#endregion

		#region メソッド: (表示イメージの取得)

		/// <summary>
		/// 表示イメージの取得
		/// </summary>
		/// <returns>
		///		描画結果を保存した画像オブジェクトを返します。
		/// </returns>
		public virtual CxImage Snapshot()
		{
			return Snapshot(this.Size, this.ViewPoint, this.Magnification);
		}

		/// <summary>
		/// 表示イメージの取得
		/// </summary>
		/// <param name="display_size">表示サイズ。出力先のサイズを示します。既定(0,0)の場合、ディスプレイサイズと同サイズになります。</param>
		/// <param name="view_point">視点。※表示対象画像の座標を示します。</param>
		/// <param name="magnification">表示倍率 [0 より大きい値] ※ 1.0 が等倍を示します。</param>
		/// <returns>
		///		描画結果を保存した画像オブジェクトを返します。
		/// </returns>
		public virtual CxImage Snapshot(TxSizeI display_size, TxPointD view_point, double magnification)
		{
			CxImage dst;
			using (var canvas = new CxCanvas())
			{
				if (display_size == new TxSizeI())
					display_size = this.Size;

				canvas.Resize(display_size.Width, display_size.Height);
				canvas.ViewPoint = view_point;
				canvas.Magnification = magnification;
				canvas.BackgroundBrush = this.BackgroundBrush;

				canvas.Clear();
				canvas.DrawImage(this.Image);

				if (this.Rendering != null)
					this.Rendering.Invoke(this, new CxRenderingEventArgs(canvas, this.Image));

				dst = canvas.Snapshot();

				if (this.Rendered != null)
					this.Rendered.Invoke(this, new CxRenderingEventArgs(canvas, this.Image));
			}
			return dst;
		}

		#endregion

		#region イベント:

		/// <summary>
		/// 描画イベント
		/// </summary>
		public virtual event CxRenderingEventHandler Rendering;

		/// <summary>
		/// 描画完了イベント
		/// </summary>
		public virtual event CxRenderingEventHandler Rendered;

		/// <summary>
		/// 操作イベント
		/// </summary>
		public virtual event CxHandlingEventHandler Handling;

		#endregion

		#region メッセージハンドラ: (描画処理)

		/// <summary>
		/// 描画処理
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				if (DesignMode)	// when design.
				{
					#region 描画:
					{
						var dsp = new TxRectangleI(0, 0, this.Width, this.Height);

						#region 無効領域の模様/バージョン表示.
						{
							var fg = Color.FromArgb(255, 255, 255);
							var bg = Color.FromArgb(0, 0, 0);
							using (var brush = new SolidBrush(bg))
							{
								e.Graphics.FillRectangle(brush, (Rectangle)dsp);
							}

							using (var pen = new Pen(fg))
							using (var font = new Font("Consolas", 14))
							{
								e.Graphics.DrawString(ToString(), font, pen.Brush, 0, 0);
								e.Graphics.DrawString("v" + ProductVersion.ToString(), font, pen.Brush, 0, 8 + 4);
							}
						}
						#endregion
					}
					#endregion
				}
				else
				{
					#region 描画:
					lock (this)
					{
						using (var canvas = new CxCanvas(e.Graphics, this.GetCanvasInfo()))
						{
							// 表示対象画像の描画:
							if (this.Image != null && this.Image.IsValid)
							{
								canvas.DrawImage(this.Image);
							}
							else
							{
								canvas.Clear();
							}

							// オーバレイ図形描画:
							if (this.Rendering != null)
								this.Rendering.Invoke(this, new CxRenderingEventArgs(canvas, this.Image));

							base.OnPaint(e);

							// 描画完了通知:
							if (this.Rendered != null)
								this.Rendered.Invoke(this, new CxRenderingEventArgs(canvas, this.Image));
						}
					}
					#endregion
				}
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// Windows メッセージを処理します。
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case WM_SIZE:
					break;
				case WM_PAINT:
					// メッセージを中断する. (OnPaint は無効化される)
					//this.DefWndProc(ref m);
					// 既定の処理:
					base.WndProc(ref m);
					return;

				case WM_ERASEBKGND:
					return;
			}
			base.WndProc(ref m);
		}

		const int WM_SIZE = 0x0003;
		const int WM_MOVE = 0x0005;
		const int WM_PAINT = 0x000F;
		const int WM_ERASEBKGND = 0x0014;
		const int WM_MOUSEFIRST = 0x0200;
		const int WM_MOUSEMOVE = 0x0200;
		const int WM_LBUTTONDOWN = 0x0201;
		const int WM_LBUTTONUP = 0x0202;
		const int WM_LBUTTONDBLCLK = 0x0203;
		const int WM_RBUTTONDOWN = 0x0204;
		const int WM_RBUTTONUP = 0x0205;
		const int WM_RBUTTONDBLCLK = 0x0206;
		const int WM_MBUTTONDOWN = 0x0207;
		const int WM_MBUTTONUP = 0x0208;
		const int WM_MBUTTONDBLCLK = 0x0209;
		const int WM_MOUSEWHEEL = 0x020A;
		const int WM_XBUTTONDOWN = 0x020B;
		const int WM_XBUTTONUP = 0x020C;
		const int WM_XBUTTONDBLCLK = 0x020D;
		const int WM_MOUSEHWHEEL = 0x020E;

		#endregion

		#region コントロールイベント: (マウス操作)

		private CxHandlingInfo HandlingInfo = new CxHandlingInfo();

		/// <summary>
		/// マウスが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				var args = new CxHandlingEventArgs(
					this.GetCanvasInfo(),
					this.Image,
					ExHandlingEventReason.MouseDown,
					e,
					Control.ModifierKeys
				);

				if (this.Handling != null)
					this.Handling.Invoke(this, args);

				if (args.IsGripped == false)
				{
					#region 背景画像の操作.
					if (EnableMouseGrip)
					{
						this.HandlingInfo.IsGripped = true;
						this.HandlingInfo.Location = e.Location;
						TxRectangleD vis = this.VisibleRect();
						this.HandlingInfo.ViewPoint = vis.Location + vis.Size / 2;
					}
					#endregion
				}

				this.Cursor = args.Cursor;
				if (args.IsUpdated)
					this.Refresh();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// マウスの押下が解除されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				var args = new CxHandlingEventArgs(
					this.GetCanvasInfo(),
					this.Image,
					ExHandlingEventReason.MouseUp,
					e,
					Control.ModifierKeys
				);

				if (this.Handling != null)
					this.Handling.Invoke(this, args);

				if (args.IsGripped == false)
				{
					#region マウス操作解除.
					this.HandlingInfo.IsGripped = false;
					this.HandlingInfo.Location = e.Location;
					TxRectangleD vis = this.VisibleRect();
					this.HandlingInfo.ViewPoint = vis.Location + vis.Size / 2;
					#endregion
				}

				this.Cursor = args.Cursor;
				if (args.IsUpdated)
					this.Refresh();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// マウスが移動したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				var args = new CxHandlingEventArgs(
					this.GetCanvasInfo(),
					this.Image,
					ExHandlingEventReason.MouseMove,
					e,
					Control.ModifierKeys
				);

				if (this.Handling != null)
					this.Handling.Invoke(this, args);

				if (args.IsGripped == false)
				{
					#region 背景画像の操作.
					if (EnableMouseGrip && this.HandlingInfo.IsGripped && e.Button == MouseButtons.Left)
					{
						var bg_size = (this.Image == null) ? new TxSizeI() : this.Image.Size;
						var mag = this.Magnification;
						if (mag <= 0) return;

						// マウスの移動量.(グリップ位置 - 現在位置)
						var dp = new XIE.TxPointD(
							this.HandlingInfo.Location.X - e.Location.X,
							this.HandlingInfo.Location.Y - e.Location.Y
							);

						// 画像座標に変換する.
						var ip = this.HandlingInfo.ViewPoint + (dp / mag);

						#region 上下限.
						if (ip.X > bg_size.Width - 1)
							ip.X = bg_size.Width - 1;
						if (ip.Y > bg_size.Height - 1)
							ip.Y = bg_size.Height - 1;
						if (ip.X < 0)
							ip.X = 0;
						if (ip.Y < 0)
							ip.Y = 0;
						#endregion

						#region 中心座標の調整.
						var visible_rect = this.VisibleRect();
						var st = visible_rect.Location;
						var ed = visible_rect.Location + visible_rect.Size;
						var ct = visible_rect.Location + visible_rect.Size * 0.5;
						if (dp.X < 0)		// →
						{
							if (ed.X == bg_size.Width &&
								ip.X > ct.X)
							{
								ip.X = ct.X;
								this.HandlingInfo.ViewPoint.X = ip.X;
							}
						}
						else if (dp.X > 0)	// ←
						{
							if (st.X == 0 &&
								ip.X < ct.X)
							{
								ip.X = ct.X;
								this.HandlingInfo.ViewPoint.X = ip.X;
							}
						}
						if (dp.Y < 0)		// ↓
						{
							if (ed.Y == bg_size.Height &&
								ip.Y > ct.Y)
							{
								ip.Y = ct.Y;
								this.HandlingInfo.ViewPoint.Y = ip.Y;
							}
						}
						else if (dp.Y > 0)	// ↑
						{
							if (st.Y == 0 &&
								ip.Y < ct.Y)
							{
								ip.Y = ct.Y;
								this.HandlingInfo.ViewPoint.Y = ip.Y;
							}
						}
						#endregion

						this.ViewPoint = ip;

						args.Cursor = Cursors.Hand;
						args.IsUpdated = true;
					}
					#endregion
				}

				this.Cursor = args.Cursor;
				if (args.IsUpdated)
					this.Refresh();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// マウスがコントロール上で一定期間停止したとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseHover(object sender, EventArgs e)
		{
			try
			{
				var mouse = this.PointToClient(Control.MousePosition);
				var args = new CxHandlingEventArgs(
					this.GetCanvasInfo(),
					this.Image,
					ExHandlingEventReason.MouseHover,
					new MouseEventArgs(Control.MouseButtons, 0, mouse.X, mouse.Y, 0),
					Control.ModifierKeys
				);

				if (this.Handling != null)
					this.Handling.Invoke(this, args);

				this.Cursor = args.Cursor;
				if (args.IsUpdated)
					this.Refresh();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// マウスがコントロール内に入ったとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseEnter(object sender, EventArgs e)
		{
			try
			{
				var mouse = this.PointToClient(Control.MousePosition);
				var args = new CxHandlingEventArgs(
					this.GetCanvasInfo(),
					this.Image,
					ExHandlingEventReason.MouseEnter,
					new MouseEventArgs(Control.MouseButtons, 0, mouse.X, mouse.Y, 0),
					Control.ModifierKeys
				);

				if (this.Handling != null)
					this.Handling.Invoke(this, args);

				this.Cursor = args.Cursor;
				if (args.IsUpdated)
					this.Refresh();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// マウスがコントロール外に出たとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseLeave(object sender, EventArgs e)
		{
			try
			{
				var mouse = this.PointToClient(Control.MousePosition);
				var args = new CxHandlingEventArgs(
					this.GetCanvasInfo(),
					this.Image,
					ExHandlingEventReason.MouseLeave,
					new MouseEventArgs(Control.MouseButtons, 0, mouse.X, mouse.Y, 0),
					Control.ModifierKeys
				);

				if (this.Handling != null)
					this.Handling.Invoke(this, args);

				this.Cursor = args.Cursor;
				if (args.IsUpdated)
					this.Refresh();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// マウスホイールが操作されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseWheel(object sender, MouseEventArgs e)
		{
			try
			{
				var args = new CxHandlingEventArgs(
					this.GetCanvasInfo(),
					this.Image,
					ExHandlingEventReason.MouseWheel,
					e,
					Control.ModifierKeys
				);

				if (this.Handling != null)
					this.Handling.Invoke(this, args);

				if (args.IsGripped == false)
				{
					#region 背景画像の操作.
					{
						Keys keys = args.Keys;
						bool ctrl = (keys & Keys.Control) == Keys.Control;
						bool shift = (keys & Keys.Shift) == Keys.Shift;
						bool alt = (keys & Keys.Alt) == Keys.Alt;

						int direction = -(e.Delta) / SystemInformation.MouseWheelScrollDelta;
						int lines = direction * SystemInformation.MouseWheelScrollLines * 2;

						if (ctrl && !shift && !alt)
						{
							if (EnableWheelScaling)
							{
								AdjustScale((e.Delta < 0) ? -1 : +1);
								this.Refresh();
							}
						}
						else if (!ctrl && shift && !alt)
						{
							if (EnableWheelScroll)
							{
								// 水平スクロール
								var vis = this.VisibleRect();
								var view_point = vis.Location + vis.Size / 2;
								view_point.X += lines;
								this.ViewPoint = view_point;
								this.Refresh();
							}
						}
						else if (!ctrl && !shift && !alt)
						{
							if (EnableWheelScroll)
							{
								// 垂直スクロール
								var vis = this.VisibleRect();
								var view_point = vis.Location + vis.Size / 2;
								view_point.Y += lines;
								this.ViewPoint = view_point;
								this.Refresh();
							}
						}
					}
					#endregion
				}

				this.Cursor = args.Cursor;
				if (args.IsUpdated)
					this.Refresh();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// キーボードが押下されたとき
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				var mouse = this.PointToClient(Control.MousePosition);
				var args = new CxHandlingEventArgs(
					this.GetCanvasInfo(),
					this.Image,
					ExHandlingEventReason.PreviewKeyDown,
					new MouseEventArgs(Control.MouseButtons, 0, mouse.X, mouse.Y, 0),
					Control.ModifierKeys
				);

				if (this.Handling != null)
					this.Handling.Invoke(this, args);

				if (args.IsGripped == false)
				{
				}

				this.Cursor = args.Cursor;
				if (args.IsUpdated)
					this.Refresh();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		#endregion
	}
}
