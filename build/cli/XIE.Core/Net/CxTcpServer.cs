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
	/// TCP/IP通信サーバークラス
	/// </summary>
	/// <example>
	/// <code lang="C#" source="examples/Net/CxTcpServer/CxTcpServer_01.cs"/>
	/// </example>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxTcpServer : System.Object
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
				m_Tag = (TxTcpServer*)m_Handle.TagPtr().ToPointer();
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
		private unsafe TxTcpServer* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxTcpServer Tag()
		{
			if (this.m_Tag == null)
				return new TxTcpServer();
			return *this.m_Tag;
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_high.fnXIE_Net_Module_Create("CxTcpServer");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTcpServer()
		{
			_Constructor();
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxTcpServer()
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
		/// ソケットストリームの取得
		/// </summary>
		/// <param name="index">クライアント指標 [0~(Connections() - 1)]</param>
		/// <returns>
		///		指定されたクライアントの情報を格納したソケットストリームを返します。
		/// </returns>
		public virtual TxSocketStream Stream(int index)
		{
			if (m_Tag == null)
				throw new CxException(ExStatus.InvalidObject);

			var streams = (TxSocketStream*)m_Tag->Clients;
			var length = m_Tag->Connections;

			if (!(0 <= index && index < length))
				throw new CxException(ExStatus.InvalidParam);

			var result = streams[index];
			return result;
		}

		/// <summary>
		/// 接続数の取得
		/// </summary>
		/// <returns>
		///		接続しているクライアントの数を返します。
		/// </returns>
		public virtual int Connections()
		{
			if (m_Tag == null)
				throw new CxException(ExStatus.InvalidObject);
			return m_Tag->Connections;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// エンドポイント
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Net.CxTcpServer.EndPoint")]
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
		/// IP アドレス
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Net.CxTcpServer.IPAddress")]
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
		/// ポート番号
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Net.CxTcpServer.Port")]
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

		/// <summary>
		/// 最大接続数
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Net.CxTcpServer.Backlog")]
		public virtual int Backlog
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Backlog;
			}
			set
			{
				if (this.m_Tag == null)
					return;
				this.m_Tag->Backlog = value;
			}
		}

		#endregion
	}
}
