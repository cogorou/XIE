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

namespace XIE.IO
{
	/// <summary>
	/// シリアル通信クラス
	/// </summary>
	/// <example>
	/// <code lang="C#" source="examples/IO/CxSerialPort/CxSerialPort_01.cs"/>
	/// </example>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxSerialPort : System.Object
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

			if (m_Handle == IntPtr.Zero)
				m_Tag = null;
			else
				m_Tag = (TxSerialPort*)m_Handle.TagPtr().ToPointer();
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
		private unsafe TxSerialPort* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxSerialPort Tag()
		{
			if (this.m_Tag == null)
				return new TxSerialPort();
			return *this.m_Tag;
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_high.fnXIE_IO_Module_Create("CxSerialPort");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxSerialPort()
		{
			_Constructor();
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxSerialPort()
		{
			((IxModule)this).Destroy();
		}

		#endregion

		#region 初期化:

		/// <summary>
		/// 
		/// </summary>
		public virtual void Setup()
		{
			ExStatus status = xie_high.fnXIE_IO_Module_Setup(((IxModule)this).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
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

		#region メソッド:

		/// <summary>
		/// 読み込み可能か否かを検査します。
		/// </summary>
		/// <param name="timeout">タイムアウト (msec) [-1,0,1~]</param>
		/// <returns>
		/// </returns>
		public bool Readable(int timeout)
		{
			ExBoolean result = ExBoolean.False;
			ExStatus status = xie_high.fnXIE_IO_SerialPort_Readable(((IxModule)this).GetHandle(), timeout, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return (result == ExBoolean.True);
		}

		/// <summary>
		/// データを読み込みます。
		/// </summary>
		/// <param name="buffer">読み込むデータを格納するバッファ</param>
		/// <param name="length">読み込むバイト数の上限値</param>
		/// <param name="timeout">タイムアウト (msec) [-1,0,1~]</param>
		/// <returns>
		///		実際に読み込まれたバイト数を返します。<br/>
		///		データが無ければ 0 を返します。<br/>
		///		異常があれば例外を発行します。
		/// </returns>
		public int Read(byte[] buffer, int length, int timeout)
		{
			int result = 0;
			ExStatus status = xie_high.fnXIE_IO_SerialPort_Read(((IxModule)this).GetHandle(), buffer, length, timeout, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		/// <summary>
		/// 書き込み可能か否かを検査します。
		/// </summary>
		/// <param name="timeout">タイムアウト (msec) [-1,0,1~]</param>
		/// <returns>
		/// </returns>
		public bool Writeable(int timeout)
		{
			ExBoolean result = ExBoolean.False;
			ExStatus status = xie_high.fnXIE_IO_SerialPort_Writeable(((IxModule)this).GetHandle(), timeout, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return (result == ExBoolean.True);
		}

		/// <summary>
		/// データを書き込みます。
		/// </summary>
		/// <param name="buffer">書き込むデータが格納されたバッファ</param>
		/// <param name="length">書き込むバイト数</param>
		/// <param name="timeout">タイムアウト (msec) [-1,0,1~]</param>
		/// <returns>
		///		実際に書き込まれたバイト数を返します。<br/>
		///		書き込めなければ 0 を返します。<br/>
		///		異常があれば例外を発行します。
		/// </returns>
		public int Write(byte[] buffer, int length, int timeout)
		{
			int result = 0;
			ExStatus status = xie_high.fnXIE_IO_SerialPort_Write(((IxModule)this).GetHandle(), buffer, length, timeout, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		#endregion

		#region プロパティ:

		[CxCategory("Parameters")]
		[CxDescription("P:XIE.IO.CxSerialPort.PortName")]
		public virtual string PortName
		{
			get
			{
				IntPtr addr = xie_high.fnXIE_IO_SerialPort_PortName_Get(((IxModule)this).GetHandle());
				if (addr == IntPtr.Zero)
					return "";
				return Marshal.PtrToStringAnsi(addr);
			}
			set
			{
				if (this.m_Tag == null)
					return;
				xie_high.fnXIE_IO_SerialPort_PortName_Set(((IxModule)this).GetHandle(), value);
			}
		}

		[CxCategory("Parameters")]
		[CxDescription("P:XIE.IO.CxSerialPort.Param")]
		public virtual TxSerialPort Param
		{
			get
			{
				if (this.m_Tag == null)
					return new TxSerialPort();
				return *this.m_Tag;
			}
			set
			{
				if (this.m_Tag == null)
					return;
				*this.m_Tag = value;
			}
		}

		#endregion
	}
}
