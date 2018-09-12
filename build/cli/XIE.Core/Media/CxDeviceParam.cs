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
	/// デバイスパラメータクラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxDeviceParam : System.Object
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
				m_Tag = (TxDeviceParam*)m_Handle.TagPtr().ToPointer();
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
		private unsafe TxDeviceParam* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxDeviceParam Tag()
		{
			if (this.m_Tag == null)
				return new TxDeviceParam();
			return *this.m_Tag;
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_high.fnXIE_Media_Module_Create("CxDeviceParam");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDeviceParam()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">デバイス名称</param>
		/// <param name="index">デバイス指標</param>
		public CxDeviceParam(string name, int index)
		{
			_Constructor();
			this.Name = name;
			this.Index = index;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">デバイス名称</param>
		/// <param name="index">デバイス指標</param>
		/// <param name="pin">ピン指標</param>
		/// <param name="size">フレームサイズ</param>
		public CxDeviceParam(string name, int index, int pin, TxSizeI size)
		{
			_Constructor();
			this.Name = name;
			this.Index = index;
			this.Pin = pin;
			this.Size = size;
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxDeviceParam()
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
			var clone = new CxDeviceParam();
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
			if (src is CxDeviceParam)
			{
				var _src = (CxDeviceParam)src;
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
				var _src = (CxDeviceParam)src;
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
		/// デバイス名称
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxDeviceParam.Name")]
		public virtual string Name
		{
			get
			{
				return Marshal.PtrToStringAnsi(this.m_Tag->Name);
			}
			set
			{
				xie_high.fnXIE_Media_DeviceParam_Name_Set(((IxModule)this).GetHandle(), value);
			}
		}

		/// <summary>
		/// デバイス指標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxDeviceParam.Index")]
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
		/// ピン指標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxDeviceParam.Pin")]
		public virtual int Pin
		{
			get
			{
				return this.m_Tag->Pin;
			}
			set
			{
				this.m_Tag->Pin = value;
			}
		}

		/// <summary>
		/// フレームサイズ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxDeviceParam.Size")]
		public virtual TxSizeI Size
		{
			get
			{
				return new TxSizeI(this.m_Tag->Width, this.m_Tag->Height);
			}
			set
			{
				this.m_Tag->Width = value.Width;
				this.m_Tag->Height = value.Height;
			}
		}

		#endregion
	}
}
