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

namespace XIE.Net
{
	/// <summary>
	/// TCP/IP通信クライアントクラス
	/// </summary>
	/// <example>
	/// <code lang="C#" source="examples/Net/CxTcpClient/CxTcpClient_01.cs"/>
	/// </example>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxTcpClient : System.Object
		, IxModule
		, IDisposable
		, IxRunnable
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
				m_Tag = (TxTcpClient*)m_Handle.TagPtr().ToPointer();
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
		private unsafe TxTcpClient* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxTcpClient Tag()
		{
			if (this.m_Tag == null)
				return new TxTcpClient();
			return *this.m_Tag;
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_high.fnXIE_Net_Module_Create("CxTcpClient");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTcpClient()
		{
			_Constructor();
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxTcpClient()
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
			ExStatus status = xie_high.fnXIE_Net_Module_Setup(((IxModule)this).GetHandle());
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

		#region IxRunnable の実装:

		/// <summary>
		/// リセット
		/// </summary>
		public void Reset()
		{
			ExStatus status = xie_core.fnXIE_Core_Runnable_Reset(((IxModule)this).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 開始
		/// </summary>
		public void Start()
		{
			ExStatus status = xie_core.fnXIE_Core_Runnable_Start(((IxModule)this).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 停止
		/// </summary>
		public void Stop()
		{
			ExStatus status = xie_core.fnXIE_Core_Runnable_Stop(((IxModule)this).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 待機
		/// </summary>
		/// <param name="timeout">タイムアウト (msec) [-1,0~]</param>
		/// <returns>
		///		停止を検知すると true を返します。
		///		タイムアウトすると false を返します。
		/// </returns>
		public virtual bool Wait(int timeout)
		{
			ExBoolean result = ExBoolean.False;
			ExStatus status = xie_core.fnXIE_Core_Runnable_Wait(((IxModule)this).GetHandle(), timeout, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return (result == ExBoolean.True);
		}

		/// <summary>
		/// 動作状態
		/// </summary>
		/// <returns>
		///		非同期処理が動作中の場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public bool IsRunning
		{
			get
			{
				ExBoolean result = ExBoolean.False;
				xie_core.fnXIE_Core_Runnable_IsRunning(((IxModule)this).GetHandle(), out result);
				return (result == ExBoolean.True);
			}
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// ストリームを取得します。
		/// </summary>
		/// <returns>
		///		ストリームを返します。
		/// </returns>
		public virtual TxSocketStream Stream()
		{
			if (m_Tag == null)
				throw new CxException(ExStatus.InvalidObject);
			TxSocketStream result = new TxSocketStream(m_Tag->Socket, new TxIPEndPoint(), new TxIPEndPoint(m_Tag->IPAddress, m_Tag->Port));
			return result;
		}

		/// <summary>
		/// 接続状態を検査します。
		/// </summary>
		/// <returns>
		///		接続している場合は true 、それ以外は false を返します。
		/// </returns>
		public virtual bool Connected()
		{
			ExBoolean result = ExBoolean.False;
			ExStatus status = xie_high.fnXIE_Net_TcpClient_Connected(((IxModule)this).GetHandle(), out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return (result == ExBoolean.True);
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 接続先
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Net.CxTcpClient.EndPoint")]
		public virtual TxIPEndPoint EndPoint
		{
			get
			{
				if (this.m_Tag == null)
					return new TxIPEndPoint();
				return new TxIPEndPoint(this.m_Tag->IPAddress, this.m_Tag->Port);
			}
			set
			{
				if (this.m_Tag == null)
					return;
				this.m_Tag->IPAddress = value.IPAddress;
				this.m_Tag->Port = value.Port;
			}
		}

		/// <summary>
		/// 接続先の IP アドレス
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Net.CxTcpClient.IPAddress")]
		public virtual TxIPAddress IPAddress
		{
			get
			{
				if (this.m_Tag == null)
					return new TxIPAddress();
				return this.m_Tag->IPAddress;
			}
			set
			{
				if (this.m_Tag == null)
					return;
				this.m_Tag->IPAddress = value;
			}
		}

		/// <summary>
		/// 接続先のポート番号
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Net.CxTcpClient.Port")]
		public virtual int Port
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Port;
			}
			set
			{
				if (this.m_Tag == null)
					return;
				this.m_Tag->Port = value;
			}
		}

		#endregion
	}
}
