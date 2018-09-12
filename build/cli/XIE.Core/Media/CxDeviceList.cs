/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
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
using System.Linq;

namespace XIE.Media
{
	/// <summary>
	/// デバイスリストクラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxDeviceList : System.Object
		, IxModule
		, IDisposable
		, ICloneable
		, IxEquatable
		, IEnumerable<CxDeviceListItem>
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
			HxModule handle = xie_high.fnXIE_Media_Module_Create("CxDeviceList");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDeviceList()
		{
			_Constructor();
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxDeviceList()
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
			var clone = new CxDeviceList();
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
			if (src is CxDeviceList)
			{
				var _src = (CxDeviceList)src;
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
				var _src = (CxDeviceList)src;
				return ((IxModule)this).GetHandle().ContentEquals(((IxModule)_src).GetHandle());
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region 初期化:

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="type">メディア種別</param>
		/// <param name="dir">メディア方向</param>
		public virtual void Setup(ExMediaType type, ExMediaDir dir)
		{
			ExStatus status = xie_high.fnXIE_Media_DeviceList_Setup(((IxModule)this).GetHandle(), type, dir);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region インデクサ:

		/// <summary>
		/// 項目
		/// </summary>
		/// <param name="index">指標 [0~(Length-1)]</param>
		/// <returns>
		///		指定位置の項目を返します。
		/// </returns>
		public virtual CxDeviceListItem this[int index]
		{
			get
			{
				var value = new CxDeviceListItem();
				ExStatus status = xie_high.fnXIE_Media_DeviceList_Item_Get(((IxModule)this).GetHandle(), index, ((IxModule)value).GetHandle());
				if (status != ExStatus.Success)
					throw new CxException(status);
				return value;
			}
			set
			{
				ExStatus status = xie_high.fnXIE_Media_DeviceList_Item_Set(((IxModule)this).GetHandle(), index, ((IxModule)value).GetHandle());
				if (status != ExStatus.Success)
					throw new CxException(status);
			}
		}

		/// <summary>
		/// 項目数 [初期値:0、範囲:0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.Media.CxDeviceList.Length")]
		public virtual int Length
		{
			get
			{
				int length = 0;
				ExStatus status = xie_high.fnXIE_Media_DeviceList_Length(((IxModule)this).GetHandle(), ref length);
				if (status != ExStatus.Success)
					throw new CxException(status);
				return length;
			}
		}

		#endregion

		#region IEnumerable の実装:

		/// <summary>
		/// 列挙クラスの取得
		/// </summary>
		/// <returns>
		///		列挙クラスを返します。
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// 列挙クラスの取得
		/// </summary>
		/// <returns>
		///		列挙クラスを返します。
		/// </returns>
		IEnumerator<CxDeviceListItem> IEnumerable<CxDeviceListItem>.GetEnumerator()
		{
			return new Enumerator(this);
		}

		#endregion

		#region Enumerator

		/// <summary>
		/// 列挙クラス
		/// </summary>
		struct Enumerator : IEnumerator, IEnumerator<CxDeviceListItem>
		{
			CxDeviceList m_Items;
			CxDeviceListItem m_Current;
			int m_Index;

			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <param name="src">列挙対象</param>
			public Enumerator(CxDeviceList src)
			{
				m_Items = src;
				m_Current = null;
				m_Index = 0;
			}

			/// <summary>
			/// 解放
			/// </summary>
			public void Dispose()
			{
				m_Current = null;
				m_Index = 0;
			}

			/// <summary>
			/// 次へ進む
			/// </summary>
			/// <returns>
			///		正常の場合は true を返します。
			///		終端に達している場合は false を返します。
			/// </returns>
			public bool MoveNext()
			{
				if (m_Index < m_Items.Length)
				{
					m_Current = m_Items[m_Index];
					m_Index++;
					return true;
				}
				else
				{
					m_Current = null;
					m_Index = m_Items.Length;
					return false;
				}
			}

			/// <summary>
			/// リセット
			/// </summary>
			void IEnumerator.Reset()
			{
				m_Current = null;
				m_Index = 0;
			}

			/// <summary>
			/// 現在の要素
			/// </summary>
			object IEnumerator.Current
			{
				get { return m_Current; }
			}

			/// <summary>
			/// 現在の要素
			/// </summary>
			public CxDeviceListItem Current
			{
				get { return m_Current; }
			}
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// デバイス名称とデバイス指標が一致する項目を探します。
		/// </summary>
		/// <param name="name">デバイス名称</param>
		/// <param name="index">デバイス指標</param>
		/// <returns>
		///		見つかった項目の指標を返します。
		///		該当する項目が無ければ -1 を返します。
		/// </returns>
		public virtual int FindIndex(string name, int index)
		{
			if (this.IsValid)
			{
				if (string.IsNullOrEmpty(name))
				{
					if (0 <= index && index < this.Length)
						return index;
				}
				else
				{
					int length = this.Length;
					for (int i = 0; i < length; i++)
					{
						var item = this[i];
						if (string.Compare(item.Name, name, true) == 0)
						{
							if (index == 0)
								return i;
							index--;
						}
					}
				}
			}
			return -1;
		}

		#endregion
	}
}
