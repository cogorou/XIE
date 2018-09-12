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
	/// オーバレイクラス (グリッド表示)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public class CxOverlayGrid : System.Object
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
			HxModule handle = xie_high.fnXIE_GDI_Module_Create("CxOverlayGrid");
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
		private CxOverlayGrid(HxModule handle, bool disposable)
		{
			((IxModule)this).SetHandle(handle, disposable);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxOverlayGrid()
		{
			_Constructor();
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxOverlayGrid()
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
		[CxDescription("P:XIE.GDI.CxOverlayGrid.Visible")]
		public virtual bool Visible
		{
			get { return m_Visible; }
			set { m_Visible = value; }
		}
		private bool m_Visible = false;

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

			try
			{
				if (e.Canvas.Magnification >= 4.0)
				{
					var figures = new List<IxGdi2d>();
					var vis = e.Canvas.VisibleRectI(true);
					var black = new TxRGB8x3(0x00, 0x00, 0x00);
					var gray = new TxRGB8x3(0x3F, 0x3F, 0x3F);

					// Vertical Line (Black)
					for (int x = 0; x < vis.Width; x++)
					{
						var figure = new CxGdiLineSegment(vis.X + x, vis.Y, vis.X + x, vis.Y + vis.Height);
						figure.PenColor = black;
						figures.Add(figure);
					}
					// Vertical Line (Gray)
					for (int x = 0; x < vis.Width; x++)
					{
						var figure = new CxGdiLineSegment(vis.X + x, vis.Y, vis.X + x, vis.Y + vis.Height);
						figure.PenColor = gray;
						figure.PenStyle = ExGdiPenStyle.Dot;
						figures.Add(figure);
					}
					// Horizontal Line (Black)
					for (int y = 0; y < vis.Height; y++)
					{
						var figure = new CxGdiLineSegment(vis.X, vis.Y + y, vis.X + vis.Width, vis.Y + y);
						figure.PenColor = black;
						figures.Add(figure);
					}
					// Horizontal Line (Gray)
					for (int y = 0; y < vis.Height; y++)
					{
						var figure = new CxGdiLineSegment(vis.X, vis.Y + y, vis.X + vis.Width, vis.Y + y);
						figure.PenColor = gray;
						figure.PenStyle = ExGdiPenStyle.Dot;
						figures.Add(figure);
					}

					e.Canvas.DrawOverlay(figures, ExGdiScalingMode.TopLeft);
				}
			}
			catch (System.Exception)
			{
			}
		}

		#endregion
	}
}
