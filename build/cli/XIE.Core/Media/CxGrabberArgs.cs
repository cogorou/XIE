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

namespace XIE.Media
{
	/// <summary>
	/// グラバーデリゲート
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param> 
	public delegate void CxGrabberHandler(object sender, CxGrabberArgs e);

	/// <summary>
	/// グラバー引数クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxGrabberArgs : System.EventArgs
		, IxModule
		, IDisposable
		, ICloneable
		, IxConvertible
		, IxEquatable
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

			if (m_Handle == IntPtr.Zero)
				m_Tag = null;
			else
				m_Tag = (TxGrabberArgs*)m_Handle.TagPtr().ToPointer();
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

		#region 構造体.

		/// <summary>
		/// 情報構造体へのポインタ
		/// </summary>
		[NonSerialized]
		private unsafe TxGrabberArgs* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxGrabberArgs Tag()
		{
			if (this.m_Tag == null)
				return new TxGrabberArgs();
			return *this.m_Tag;
		}

		#endregion

		#region 生成関数:

		/// <summary>
		/// グラバー引数クラスのオブジェクトハンドルからの生成
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public unsafe static CxGrabberArgs FromHandle(HxModule handle, bool disposable)
		{
			return new CxGrabberArgs(handle, disposable);
		}

		/// <summary>
		/// コンストラクタ (ハンドル指定)
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		private CxGrabberArgs(HxModule handle, bool disposable)
		{
			((IxModule)this).SetHandle(handle, disposable);
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_high.fnXIE_Media_Module_Create("CxGrabberArgs");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGrabberArgs()
		{
			_Constructor();
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxGrabberArgs()
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
		[XmlIgnore]
		[ReadOnly(true)]
		public virtual bool IsValid
		{
			get
			{
				return true;
			}
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
			var clone = new CxGrabberArgs();
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
			if (src is CxGrabberArgs)
			{
				var _src = (CxGrabberArgs)src;
				((IxModule)this).GetHandle().CopyFrom(((IxModule)_src).GetHandle());
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
				var _src = (CxGrabberArgs)src;
				return ((IxModule)this).GetHandle().ContentEquals(((IxModule)_src).GetHandle());
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
			if (dst is CxImage)
			{
				var _dst = (CxImage)dst;
				((IxModule)_dst).GetHandle().CopyFrom(((IxModule)this).GetHandle());
			}
			else
			{
				throw new CxException(ExStatus.Unsupported);
			}
		}

		/// <summary>
		/// CxImage への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator CxImage(CxGrabberArgs src)
		{
			var dst = new CxImage();
			((IxModule)dst).GetHandle().CopyFrom(((IxModule)src).GetHandle());
			return dst;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// タイムスタンプ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxGrabberArgs.TimeStamp")]
		public virtual ulong TimeStamp
		{
			get
			{
				return this.m_Tag->TimeStamp;
			}
			set
			{
				this.m_Tag->TimeStamp = value;
			}
		}

		/// <summary>
		/// フレームサイズ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxGrabberArgs.FrameSize")]
		public virtual TxImageSize FrameSize
		{
			get
			{
				return this.m_Tag->FrameSize;
			}
			set
			{
				this.m_Tag->FrameSize = value;
			}
		}

		/// <summary>
		/// 処理経過
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxGrabberArgs.Progress")]
		public virtual double Progress
		{
			get
			{
				return this.m_Tag->Progress;
			}
			set
			{
				this.m_Tag->Progress = value;
			}
		}

		/// <summary>
		/// データ領域の先頭アドレス
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxGrabberArgs.Address")]
		public virtual IntPtr Address
		{
			get
			{
				return this.m_Tag->Address;
			}
			set
			{
				this.m_Tag->Address = value;
			}
		}

		/// <summary>
		/// データ長 (bytes)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxGrabberArgs.Length")]
		public virtual int Length
		{
			get
			{
				return this.m_Tag->Length;
			}
			set
			{
				this.m_Tag->Length = value;
			}
		}

		/// <summary>
		/// フレーム指標 [-1,0~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxGrabberArgs.Index")]
		public virtual int Index
		{
			get
			{
				return this.m_Tag->Index;
			}
			set
			{
				this.m_Tag->Index = value;
			}
		}

		/// <summary>
		/// 処理中断の応答
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxGrabberArgs.Cancellation")]
		public virtual bool Cancellation
		{
			get
			{
				return (this.m_Tag->Cancellation == ExBoolean.True);
			}
			set
			{
				this.m_Tag->Cancellation = value ? ExBoolean.True : ExBoolean.False;
			}
		}

		#endregion
	}
}
