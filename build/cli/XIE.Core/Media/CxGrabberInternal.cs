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
	/// グラバークラス (内部用)
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	internal unsafe class CxGrabberInternal : System.Object
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
			HxModule handle = xie_high.fnXIE_Media_Module_Create("CxGrabber");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxGrabberInternal()
		{
			_Constructor();
			Receive_Setup();
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxGrabberInternal()
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
			Notify = null;
			Receive_Dispose();
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
		public virtual bool IsRunning
		{
			get
			{
				ExBoolean result = ExBoolean.False;
				xie_core.fnXIE_Core_Runnable_IsRunning(((IxModule)this).GetHandle(), out result);
				return (result == ExBoolean.True);
			}
		}

		#endregion

		#region イベント:

		/// <summary>
		/// 通知イベント  
		/// </summary>
		public virtual event CxGrabberHandler Notify;

		/// <summary>
		/// イベント受信関数のデリゲート
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private delegate void ReceiveDelegate(IntPtr sender, HxModule e);

		/// <summary>
		/// イベント受信関数のデリゲートのインスタンス
		/// </summary>
		private ReceiveDelegate ReceiveHandler = null;

		/// <summary>
		/// イベント受信関数のポインタ
		/// </summary>
		private IntPtr ReceivePointer = IntPtr.Zero;

		/// <summary>
		/// イベント受信関数のポインタの解放を防ぐ機構
		/// </summary>
		private GCHandle ReceiveGCH = new GCHandle();

		/// <summary>
		/// イベント受信関数
		/// </summary>
		/// <param name="sender">送信元の this ポインタ</param>
		/// <param name="e">CxGrabberArgs の this ポインタ (関数を抜けると無効になります。保管や解放は行わないでください)</param>
		/// <remarks>
		///		引数 e は呼び出し元の関数内のスタックに生成された C++ クラスの this ポインタです。
		///		この関数内の args は .NET のクラスです。引数 e (C++ this ポインタ) の複製です。
		///		前者は、この関数を抜けると自動的に解放され無効になります。
		/// </remarks>
		private void Receive(IntPtr sender, HxModule e)
		{
			if (Notify != null)
			{
				var args = new CxGrabberArgs();
				var args_handle = ((IxModule)args).GetHandle();

				// 引数 e の内容を新規に生成した args へ複製する.
				xie_core.fnXIE_Core_Equatable_CopyFrom(args_handle, e);

				// ユーザーが登録したイベントハンドラを呼び出す.
				Notify(this, args);
			}
		}

		/// <summary>
		/// イベント受信関数の初期化
		/// </summary>
		/// <remarks>
		///		C# 側の Receive 関数のポインタを C++ 側の Notify に設定します.
		/// </remarks>
		private void Receive_Setup()
		{
			HxModule handle = ((IxModule)this).GetHandle();
			this.ReceiveHandler = new ReceiveDelegate(Receive);
			this.ReceivePointer = Marshal.GetFunctionPointerForDelegate(this.ReceiveHandler);
			this.ReceiveGCH = GCHandle.Alloc(this.ReceivePointer);
			ExStatus status = xie_high.fnXIE_Media_Grabber_Notify_Set(handle, this.ReceivePointer);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// イベント受信関数の解放
		/// </summary>
		private void Receive_Dispose()
		{
			HxModule handle = ((IxModule)this).GetHandle();
			if (handle != IntPtr.Zero)
			{
				ExStatus status = xie_high.fnXIE_Media_Grabber_Notify_Set(handle, IntPtr.Zero);
				if (status != ExStatus.Success)
					throw new CxException(status);
			}
			if (this.ReceiveHandler != null)
			{
				this.ReceiveGCH.Free();
				this.ReceivePointer = IntPtr.Zero;
				this.ReceiveHandler = null;
			}
		}

		#endregion
	}
}
