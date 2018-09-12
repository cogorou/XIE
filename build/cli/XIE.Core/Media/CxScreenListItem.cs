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
	/// スクリーンリスト項目クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxScreenListItem : System.Object
		, IxModule
		, IDisposable
		, ICloneable
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
				m_Tag = (TxScreenListItem*)m_Handle.TagPtr().ToPointer();
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
		private unsafe TxScreenListItem* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxScreenListItem Tag()
		{
			if (this.m_Tag == null)
				return new TxScreenListItem();
			return *this.m_Tag;
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_high.fnXIE_Media_Module_Create("CxScreenListItem");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxScreenListItem()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="handle">ウィンドウハンドル</param>
		/// <param name="name">ウィンドウ名称</param>
		/// <param name="bounds">ウィンドウの位置とサイズ</param>
		public CxScreenListItem(IntPtr handle, string name, TxRectangleI bounds)
		{
			_Constructor();
			this.Handle = handle;
			this.Name = name;
			this.Bounds = bounds;
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxScreenListItem()
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
				return ((IxModule)this).GetHandle().IsValid();
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
			var clone = new CxScreenListItem();
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
			if (src is CxScreenListItem)
			{
				var _src = (CxScreenListItem)src;
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
				var _src = (CxScreenListItem)src;
				return ((IxModule)this).GetHandle().ContentEquals(((IxModule)_src).GetHandle());
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// ウィンドウハンドル
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxScreenListItem.Handle")]
		public virtual IntPtr Handle
		{
			get
			{
				return this.m_Tag->Handle;
			}
			set
			{
				this.m_Tag->Handle = value;
			}
		}

		/// <summary>
		/// ウィンドウ名称
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxScreenListItem.Name")]
		public virtual string Name
		{
			get
			{
				return Marshal.PtrToStringAnsi(this.m_Tag->Name);
			}
			set
			{
				xie_high.fnXIE_Media_ScreenListItem_Name_Set(((IxModule)this).GetHandle(), value);
			}
		}

		/// <summary>
		/// ウィンドウの位置とサイズ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxScreenListItem.Bounds")]
		public virtual TxRectangleI Bounds
		{
			get
			{
				return new TxRectangleI(m_Tag->X, m_Tag->Y, m_Tag->Width, m_Tag->Height);
			}
			set
			{
				this.m_Tag->X = value.X;
				this.m_Tag->Y = value.Y;
				this.m_Tag->Width = value.Width;
				this.m_Tag->Height = value.Height;
			}
		}

		#endregion
	}
}
