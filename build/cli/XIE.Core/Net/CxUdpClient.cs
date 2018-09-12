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
	/// UDP通信クラス
	/// </summary>
	/// <example>
	/// <code lang="C#" source="examples/Net/CxUdpClient/CxUdpClient_01.cs"/>
	/// </example>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxUdpClient : System.Object
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
				m_Tag = (TxUdpClient*)m_Handle.TagPtr().ToPointer();
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
		private unsafe TxUdpClient* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxUdpClient Tag()
		{
			if (this.m_Tag == null)
				return new TxUdpClient();
			return *this.m_Tag;
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_high.fnXIE_Net_Module_Create("CxUdpClient");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxUdpClient()
		{
			_Constructor();
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxUdpClient()
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

		#region メソッド:

		/// <summary>
		/// 読み込み可能か否かを検査します。
		/// </summary>
		/// <param name="timeout">タイムアウト (msec) [-1,0,1~]</param>
		/// <returns>
		///		読み込み可能なデータが到達している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public bool Readable(int timeout)
		{
			ExBoolean result = ExBoolean.False;
			ExStatus status = xie_high.fnXIE_Net_UdpClient_Readable(((IxModule)this).GetHandle(), timeout, out result);
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
		/// <param name="remoteEP">受信対象 (実際の受信元のIPアドレスで更新されます。)</param>
		/// <returns>
		///		実際に読み込まれたバイト数を返します。<br/>
		///		データが無ければ 0 を返します。<br/>
		///		異常があれば例外を発行します。
		/// </returns>
		public int Read(byte[] buffer, int length, int timeout, ref TxIPEndPoint remoteEP)
		{
			int result = 0;
			ExStatus status = xie_high.fnXIE_Net_UdpClient_Read(((IxModule)this).GetHandle(), buffer, length, timeout, ref remoteEP, out result);
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
			ExStatus status = xie_high.fnXIE_Net_UdpClient_Writeable(((IxModule)this).GetHandle(), timeout, out result);
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
		/// <param name="remoteEP">送信先</param>
		/// <returns>
		///		実際に書き込まれたバイト数を返します。<br/>
		///		書き込めなければ 0 を返します。<br/>
		///		異常があれば例外を発行します。
		/// </returns>
		public int Write(byte[] buffer, int length, int timeout, TxIPEndPoint remoteEP)
		{
			int result = 0;
			ExStatus status = xie_high.fnXIE_Net_UdpClient_Write(((IxModule)this).GetHandle(), buffer, length, timeout, ref remoteEP, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// エンドポイント
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Net.CxUdpClient.EndPoint")]
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
		[CxDescription("P:XIE.Net.CxUdpClient.IPAddress")]
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
		[CxDescription("P:XIE.Net.CxUdpClient.Port")]
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
