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
	/// デバイスリスト項目クラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxDeviceListItem : System.Object
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
				m_Tag = (TxDeviceListItem*)m_Handle.TagPtr().ToPointer();
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
		private unsafe TxDeviceListItem* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxDeviceListItem Tag()
		{
			if (this.m_Tag == null)
				return new TxDeviceListItem();
			return *this.m_Tag;
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_high.fnXIE_Media_Module_Create("CxDeviceListItem");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDeviceListItem()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">メディア種別</param>
		/// <param name="dir">メディア方向</param>
		/// <param name="name">デバイス名称</param>
		/// <param name="index">デバイス指標</param>
		public CxDeviceListItem(ExMediaType type, ExMediaDir dir, string name, int index)
		{
			_Constructor();
			this.MediaType = type;
			this.MediaDir = dir;
			this.Name = name;
			this.Index = index;
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxDeviceListItem()
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
			var clone = new CxDeviceListItem();
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
			if (src is CxDeviceListItem)
			{
				var _src = (CxDeviceListItem)src;
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
				var _src = (CxDeviceListItem)src;
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
		/// メディア種別
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxDeviceListItem.MediaType")]
		public virtual ExMediaType MediaType
		{
			get
			{
				return this.m_Tag->MediaType;
			}
			set
			{
				this.m_Tag->MediaType = value;
			}
		}

		/// <summary>
		/// メディア方向
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxDeviceListItem.MediaDir")]
		public virtual ExMediaDir MediaDir
		{
			get
			{
				return this.m_Tag->MediaDir;
			}
			set
			{
				this.m_Tag->MediaDir = value;
			}
		}

		/// <summary>
		/// デバイス名称
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxDeviceListItem.Name")]
		public virtual string Name
		{
			get
			{
				return Marshal.PtrToStringAnsi(this.m_Tag->Name);
			}
			set
			{
				xie_high.fnXIE_Media_DeviceListItem_Name_Set(((IxModule)this).GetHandle(), value);
			}
		}

		/// <summary>
		/// デバイス指標
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxDeviceListItem.Index")]
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

		#endregion

		#region メソッド:

		/// <summary>
		/// プロダクト名の取得
		/// </summary>
		/// <returns>
		///		プロダクト名を返します。
		/// </returns>
		public virtual string GetProductName()
		{
			var result = new CxStringA();
			HxModule hThis = ((IxModule)this).GetHandle();
			HxModule hResult = ((IxModule)result).GetHandle();
			ExStatus status = xie_high.fnXIE_Media_DeviceListItem_GetProductName(hThis, hResult);
			if (status != ExStatus.Success)
				throw new CxException(status);
			var result_string = result.ToString();
			return result_string;
		}

		/// <summary>
		/// ピン名称一覧の取得
		/// </summary>
		/// <returns>
		///		ピン名称一覧を返します。
		/// </returns>
		public virtual unsafe string[] GetPinNames()
		{
			var result = new CxArray();
			HxModule hThis = ((IxModule)this).GetHandle();
			HxModule hResult = ((IxModule)result).GetHandle();
			ExStatus status = xie_high.fnXIE_Media_DeviceListItem_GetPinNames(hThis, hResult);
			if (status != ExStatus.Success)
				throw new CxException(status);
			var result_array = new string[result.Length];
			result.Scanner().ForEach(
				delegate(int i, IntPtr addr)
				{
					// 各要素はヒープに確保された C++ 側の CxString のポインタですので
					// 明示的に開放する必要があります.
					void** pstr = (void**)addr.ToPointer();
					HxModule module = new HxModule(*pstr);
					using (CxStringA item = CxStringA.FromHandle(module, true))	// true=解放する.
					{
						result_array[i] = item.ToString();
					}
				});
			return result_array;
		}

		/// <summary>
		/// フレームサイズ一覧の取得
		/// </summary>
		/// <param name="pin">ピン番号 [0~]</param>
		/// <returns>
		///		フレームサイズ一覧を返します。
		/// </returns>
		public virtual unsafe TxSizeI[] GetFrameSizes(int pin = 0)
		{
			var result = new CxArray();
			HxModule hThis = ((IxModule)this).GetHandle();
			HxModule hResult = ((IxModule)result).GetHandle();
			ExStatus status = xie_high.fnXIE_Media_DeviceListItem_GetFrameSizes(hThis, hResult, pin);
			if (status != ExStatus.Success)
				throw new CxException(status);
			var result_array = new TxSizeI[result.Length];
			result.Scanner().ForEach(
				delegate(int i, IntPtr addr)
				{
					result_array[i] = *((TxSizeI*)addr.ToPointer());
				});
			return result_array;
		}

		#endregion
	}
}
