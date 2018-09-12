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
	/// 制御プロパティクラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxControlProperty : System.Object
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
			HxModule handle = xie_high.fnXIE_Media_Module_Create("CxControlProperty");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxControlProperty()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="controller">コントローラ</param>
		/// <param name="name">プロパティ名称</param>
		public CxControlProperty(IxModule controller, string name)
		{
			_Constructor();
			Controller = controller;
			Name = name;
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxControlProperty()
		{
			this.Dispose();
			((IxModule)this).Destroy();
		}

		#endregion

		#region IDisposable の実装: (解放)

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			Controller = null;
			Name = null;
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
			var clone = new CxControlProperty();
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
			if (src is CxControlProperty)
			{
				var _src = (CxControlProperty)src;
				((IxModule)this).GetHandle().CopyFrom(((IxModule)_src).GetHandle());
				this.Controller = _src.Controller;
				this.Name = _src.Name;
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
				var _src = (CxControlProperty)src;
				if (((IxModule)this).GetHandle().ContentEquals(((IxModule)_src).GetHandle()) == false) return false;
				if (this.Controller != _src.Controller) return false;
				if (this.Name != _src.Name) return false;
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// コントローラ
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxControlProperty.Controller")]
		public virtual IxModule Controller
		{
			get
			{
				return m_Controller;
			}
			set
			{
				m_Controller = value;

				HxModule hController = (m_Controller == null) ? IntPtr.Zero : value.GetHandle();
				ExStatus status = xie_high.fnXIE_Media_ControlProperty_Controller_Set(((IxModule)this).GetHandle(), hController);
				if (status != ExStatus.Success)
					throw new CxException(status);
			}
		}
		private IxModule m_Controller = null;

		/// <summary>
		/// プロパティ名称
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Media.CxControlProperty.Name")]
		public virtual string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;

				ExStatus status = xie_high.fnXIE_Media_ControlProperty_Name_Set(((IxModule)this).GetHandle(), m_Name);
				if (status != ExStatus.Success)
					throw new CxException(status);
			}
		}
		private string m_Name = null;

		#endregion

		#region 操作:

		/// <summary>
		/// サポート状態の取得
		/// </summary>
		public virtual bool IsSupported()
		{
			ExBoolean value = ExBoolean.False;
			ExStatus status = xie_high.fnXIE_Media_ControlProperty_IsSupported(((IxModule)this).GetHandle(), out value);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return (value == ExBoolean.True);
		}

		/// <summary>
		/// 設定範囲の取得
		/// </summary>
		/// <returns>
		///		設定範囲を返します。
		/// </returns>
		public virtual TxRangeI GetRange()
		{
			TxRangeI value = new TxRangeI();
			ExStatus status = xie_high.fnXIE_Media_ControlProperty_GetRange(((IxModule)this).GetHandle(), out value);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return value;
		}

		/// <summary>
		/// ステップサイズの取得
		/// </summary>
		/// <returns>
		///		ステップサイズを返します。
		/// </returns>
		public virtual int GetStep()
		{
			int value = 0;
			ExStatus status = xie_high.fnXIE_Media_ControlProperty_GetStep(((IxModule)this).GetHandle(), out value);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return value;
		}

		/// <summary>
		/// 既定値の取得
		/// </summary>
		/// <returns>
		///		既定値を返します。
		/// </returns>
		public virtual int GetDefault()
		{
			int value = 0;
			ExStatus status = xie_high.fnXIE_Media_ControlProperty_GetDefault(((IxModule)this).GetHandle(), out value);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return value;
		}

		/// <summary>
		/// 制御フラグの取得
		/// </summary>
		/// <returns>
		///		制御フラグ [1:自動、2:手動] を返します。
		/// </returns>
		public virtual int GetFlags()
		{
			int value = 0;
			ExStatus status = xie_high.fnXIE_Media_ControlProperty_GetFlags(((IxModule)this).GetHandle(), out value);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return value;
		}

		/// <summary>
		/// 制御フラグの設定
		/// </summary>
		/// <param name="value">設定値 [1:自動、2:手動]</param>
		public virtual void SetFlags(int value)
		{
			ExStatus status = xie_high.fnXIE_Media_ControlProperty_SetFlags(((IxModule)this).GetHandle(), value);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 現在値の取得
		/// </summary>
		/// <returns>
		///		現在値を返します。
		/// </returns>
		public virtual int GetValue()
		{
			int value = 0;
			ExStatus status = xie_high.fnXIE_Media_ControlProperty_GetValue(((IxModule)this).GetHandle(), out value);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return value;
		}

		/// <summary>
		/// 現在値の設定
		/// </summary>
		/// <param name="value">設定値</param>
		/// <param name="relative">設定値が相対値であるか否か</param>
		public virtual void SetValue(int value, bool relative = false)
		{
			ExBoolean _relative = relative ? ExBoolean.True : ExBoolean.False;
			ExStatus status = xie_high.fnXIE_Media_ControlProperty_SetValue(((IxModule)this).GetHandle(), value, _relative);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion
	}
}
