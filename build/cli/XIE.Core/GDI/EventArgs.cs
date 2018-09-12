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
	/// 描画イベントのデリゲート
	/// </summary>
	/// <param name="sender">イベントの発行元</param>
	/// <param name="e">イベントの内容</param>
	public delegate void CxRenderingEventHandler(object sender, CxRenderingEventArgs e);

	/// <summary>
	/// 描画イベント引数クラス
	/// </summary>
	[TypeConverter(typeof(CxSortingConverter))]
	public partial class CxRenderingEventArgs : EventArgs
		, ICloneable
		, IxEquatable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxRenderingEventArgs()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="canvas">描画先のキャンバス</param>
		/// <param name="image">背景画像</param>
		public CxRenderingEventArgs(CxCanvas canvas, XIE.CxImage image)
		{
			this.Canvas = canvas;
			this.Image = image;
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// オブジェクトのクローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			var clone = new CxRenderingEventArgs();
			clone.CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;
			if (ReferenceEquals(src, null)) return;

			if (src is CxRenderingEventArgs)
			{
				var _dst = this;
				var _src = (CxRenderingEventArgs)src;
				_dst.Canvas = _src.Canvas;
				_dst.Image = _src.Image;
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
			if (ReferenceEquals(src, this)) return true;
			if (ReferenceEquals(src, null)) return false;

			if (src is CxRenderingEventArgs)
			{
				var _dst = this;
				var _src = (CxRenderingEventArgs)src;
				if (_dst.Canvas != _src.Canvas) return false;
				if (_dst.Image != _src.Image) return false;
				return true;
			}
			return false;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 描画先のキャンバス [初期値:null] ※描画処理完了後は使用できませんのでご注意ください。
		/// </summary>
		[XmlIgnore]
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxRenderingEventArgs.Canvas")]
		public CxCanvas Canvas
		{
			get { return m_Canvas; }
			set { m_Canvas = value; }
		}
		private CxCanvas m_Canvas = null;

		/// <summary>
		/// 背景画像 [初期値:null]
		/// </summary>
		[XmlIgnore]
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxRenderingEventArgs.Image")]
		public virtual XIE.CxImage Image
		{
			get { return m_Image; }
			set { m_Image = value; }
		}
		private XIE.CxImage m_Image = null;

		#endregion
	}

	/// <summary>
	/// 操作イベントのデリゲート
	/// </summary>
	/// <param name="sender">イベントの発行元</param>
	/// <param name="e">イベントの内容</param>
	public delegate void CxHandlingEventHandler(object sender, CxHandlingEventArgs e);

	/// <summary>
	/// 操作イベント引数クラス
	/// </summary>
	[Serializable]
	public partial class CxHandlingEventArgs : EventArgs
		, ICloneable
		, IxEquatable
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxHandlingEventArgs()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="canvas">キャンバス情報</param>
		/// <param name="image">背景画像</param>
		/// <param name="reason">発生した理由</param>
		/// <param name="mouse">発生したマウスイベントの引数</param>
		/// <param name="keys">マウス操作時のキーボード押下状態</param>
		public CxHandlingEventArgs(CxCanvasInfo canvas, XIE.CxImage image, ExHandlingEventReason reason, MouseEventArgs mouse, Keys keys)
		{
			this.Canvas = canvas;
			this.Image = image;
			this.Reason = reason;
			this.MouseEventArgs = mouse;
			this.Keys = keys;
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// オブジェクトのクローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			var clone = new CxHandlingEventArgs();
			clone.CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;
			if (ReferenceEquals(src, null)) return;

			if (src is CxHandlingEventArgs)
			{
				var _dst = this;
				var _src = (CxHandlingEventArgs)src;
				_dst.Canvas = _src.Canvas;
				_dst.Image = _src.Image;
				_dst.Reason = _src.Reason;
				_dst.MouseEventArgs = _src.MouseEventArgs;
				_dst.Keys = _src.Keys;
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
			if (ReferenceEquals(src, this)) return true;
			if (ReferenceEquals(src, null)) return false;

			if (src is CxHandlingEventArgs)
			{
				var _dst = this;
				var _src = (CxHandlingEventArgs)src;
				if (_dst.Canvas != _src.Canvas) return false;
				if (_dst.Image != _src.Image) return false;
				if (_dst.Reason != _src.Reason) return false;
				if (_dst.MouseEventArgs != _src.MouseEventArgs) return false;
				if (_dst.Keys != _src.Keys) return false;
				return true;
			}
			return false;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// キャンバス情報
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxHandlingEventArgs.Canvas")]
		public virtual CxCanvasInfo Canvas
		{
			get { return m_Canvas; }
			set { m_Canvas = value; }
		}
		private CxCanvasInfo m_Canvas = null;

		/// <summary>
		/// 背景画像 [初期値:null]
		/// </summary>
		[XmlIgnore]
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxHandlingEventArgs.Image")]
		public virtual XIE.CxImage Image
		{
			get { return m_Image; }
			set { m_Image = value; }
		}
		private XIE.CxImage m_Image = null;

		/// <summary>
		/// 発生した理由 [初期値:None]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxHandlingEventArgs.Reason")]
		public virtual ExHandlingEventReason Reason
		{
			get { return m_Reason; }
			set { m_Reason = value; }
		}
		private ExHandlingEventReason m_Reason = ExHandlingEventReason.None;

		/// <summary>
		/// 発生したマウスイベントの引数 [初期値:null]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxHandlingEventArgs.MouseEventArgs")]
		public virtual MouseEventArgs MouseEventArgs
		{
			get { return m_MouseEventArgs; }
			set { m_MouseEventArgs = value; }
		}
		private MouseEventArgs m_MouseEventArgs = null;

		/// <summary>
		/// マウス操作時のキーボード押下状態 [初期値:None]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.CxHandlingEventArgs.Keys")]
		public virtual Keys Keys
		{
			get { return m_Keys; }
			set { m_Keys = value; }
		}
		private Keys m_Keys = Keys.None;

		#endregion

		#region プロパティ: (応答)

		/// <summary>
		/// グリップ状態 [初期値:false] ※操作を開始した場合は true に変更してください。
		/// </summary>
		[CxCategory("Response")]
		[CxDescription("P:XIE.GDI.CxHandlingEventArgs.IsGripped")]
		public virtual bool IsGripped
		{
			get { return m_IsGripped; }
			set { m_IsGripped = value; }
		}
		private bool m_IsGripped = false;

		/// <summary>
		/// カーソル形状
		/// </summary>
		[CxCategory("Response")]
		[CxDescription("P:XIE.GDI.CxHandlingEventArgs.Cursor")]
		public virtual System.Windows.Forms.Cursor Cursor
		{
			get { return m_Cursor; }
			set { m_Cursor = value; }
		}
		private System.Windows.Forms.Cursor m_Cursor = System.Windows.Forms.Cursors.Default;

		/// <summary>
		/// 更新されている為、再描画が必要な旨を通知します。
		/// </summary>
		[CxCategory("Response")]
		[CxDescription("P:XIE.GDI.CxHandlingEventArgs.IsUpdated")]
		public virtual bool IsUpdated
		{
			get { return m_IsUpdated; }
			set { m_IsUpdated = value; }
		}
		private bool m_IsUpdated = false;

		#endregion
	}

	/// <summary>
	/// 操作イベントの発生理由
	/// </summary>
	public enum ExHandlingEventReason
	{
		None,
		MouseDown,
		MouseUp,
		MouseMove,
		MouseHover,
		MouseEnter,
		MouseLeave,
		MouseWheel,
		PreviewKeyDown,
	}

	/// <summary>
	/// 操作イベント付加情報
	/// </summary>
	public class CxHandlingInfo
	{
		/// <summary>
		/// グリップ状態
		/// </summary>
		public bool IsGripped;

		/// <summary>
		/// グリップ位置 (クライアント座標)
		/// </summary>
		public Point Location;

		/// <summary>
		/// グリップ位置 (画像座標)
		/// </summary>
		public TxPointD ViewPoint;
	}
}
