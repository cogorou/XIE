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
	/// スクリーンキャプチャクラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxScreenCapture : System.Object
		, IxModule
		, IDisposable
		, IxMediaControl
		, IxParam
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
			HxModule handle = xie_high.fnXIE_Media_Module_Create("CxScreenCapture");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);

			ExStatus status;

			status = xie_high.fnXIE_Media_ScreenCapture_CreateGrabber(handle, ExMediaType.Audio, ((IxModule)AudioGrabber).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);

			status = xie_high.fnXIE_Media_ScreenCapture_CreateGrabber(handle, ExMediaType.Video, ((IxModule)VideoGrabber).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);

			this.AudioGrabber.Start();
			this.VideoGrabber.Start();
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxScreenCapture()
		{
			_Constructor();
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxScreenCapture()
		{
			this.AudioGrabber.Dispose();
			this.VideoGrabber.Dispose();
			((IxModule)this.AudioGrabber).Destroy();
			((IxModule)this.VideoGrabber).Destroy();
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

		#region IxMediaControl の実装:

		/// <summary>
		/// リセット
		/// </summary>
		public void Reset()
		{
			ExStatus status = xie_high.fnXIE_Media_MediaControl_Reset(((IxModule)this).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 開始
		/// </summary>
		public void Start()
		{
			ExStatus status = xie_high.fnXIE_Media_MediaControl_Start(((IxModule)this).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 停止
		/// </summary>
		public void Stop()
		{
			ExStatus status = xie_high.fnXIE_Media_MediaControl_Stop(((IxModule)this).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 中断
		/// </summary>
		public void Abort()
		{
			ExStatus status = xie_high.fnXIE_Media_MediaControl_Abort(((IxModule)this).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 一時停止
		/// </summary>
		public void Pause()
		{
			ExStatus status = xie_high.fnXIE_Media_MediaControl_Pause(((IxModule)this).GetHandle());
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
			ExStatus status = xie_high.fnXIE_Media_MediaControl_Wait(((IxModule)this).GetHandle(), timeout, out result);
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
				xie_high.fnXIE_Media_MediaControl_IsRunning(((IxModule)this).GetHandle(), out result);
				return (result == ExBoolean.True);
			}
		}

		/// <summary>
		/// 一時停止状態
		/// </summary>
		/// <returns>
		///		非同期処理が一時停止中の場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool IsPaused
		{
			get
			{
				ExBoolean result = ExBoolean.False;
				xie_high.fnXIE_Media_MediaControl_IsPaused(((IxModule)this).GetHandle(), out result);
				return (result == ExBoolean.True);
			}
		}

		#endregion

		#region IxParam の実装:

		/// <summary>
		/// パラメータの取得
		/// </summary>
		/// <param name="name">パラメータ名称</param>
		/// <returns>
		///		取得した値を返します。
		///	</returns>
		public object GetParam(string name)
		{
			switch (name)
			{
				case "Timeout":
					{
						int _value = 0;
						var model = ModelOf.From(_value.GetType());
						ExStatus status = xie_core.fnXIE_Core_Param_GetParam(((IxModule)this).GetHandle(), name, new IntPtr(&_value), model);
						if (status != ExStatus.Success)
							throw new CxException(status);
						return _value;
					}
				case "FrameRate":
					{
						int _value = 0;
						var model = ModelOf.From(_value.GetType());
						ExStatus status = xie_core.fnXIE_Core_Param_GetParam(((IxModule)this).GetHandle(), name, new IntPtr(&_value), model);
						if (status != ExStatus.Success)
							throw new CxException(status);
						return _value;
					}
				case "FrameSize":
					{
						var _value = new TxImageSize();
						var model = ModelOf.From(_value.GetType());
						ExStatus status = xie_core.fnXIE_Core_Param_GetParam(((IxModule)this).GetHandle(), name, new IntPtr(&_value), model);
						if (status != ExStatus.Success)
							throw new CxException(status);
						return _value;
					}
				case "Video.Connected":
					{
						bool _value = false;
						var model = ModelOf.From(_value.GetType());
						ExStatus status = xie_core.fnXIE_Core_Param_GetParam(((IxModule)this).GetHandle(), name, new IntPtr(&_value), model);
						if (status != ExStatus.Success)
							throw new CxException(status);
						return _value;
					}
				case "Audio.Connected":
					{
						bool _value = false;
						var model = ModelOf.From(_value.GetType());
						ExStatus status = xie_core.fnXIE_Core_Param_GetParam(((IxModule)this).GetHandle(), name, new IntPtr(&_value), model);
						if (status != ExStatus.Success)
							throw new CxException(status);
						return _value;
					}
				case "Audio.DeviceName":
				case "Audio.ProductName":
				case "Output.DeviceName":
				case "Output.ProductName":
				case "Output.FileName":
					{
						var _value = new CxStringW();
						var model = TxModel.Ptr(1);
						ExStatus status = xie_core.fnXIE_Core_Param_GetParam(((IxModule)this).GetHandle(), name, (IntPtr)((IxModule)_value).GetHandle(), model);
						if (status != ExStatus.Success)
							throw new CxException(status);
						return _value.ToString();
					}
			}
			throw new NotSupportedException("Specified name is not supported.");
		}

		/// <summary>
		/// パラメータの設定
		/// </summary>
		/// <param name="name">パラメータ名称</param>
		/// <param name="value">設定値</param>
		public void SetParam(string name, object value)
		{
			switch (name)
			{
				case "Timeout":
					{
						if (!(value is int))
							throw new System.ArgumentException("value type must be int.");
						int _value = (int)value;
						var model = ModelOf.From(value.GetType());
						ExStatus status = xie_core.fnXIE_Core_Param_SetParam(((IxModule)this).GetHandle(), name, new IntPtr(&_value), model);
						if (status != ExStatus.Success)
							throw new CxException(status);
						return;
					}
				case "FrameRate":
					{
						if (!(value is int))
							throw new System.ArgumentException("value type must be int.");
						int _value = (int)value;
						var model = ModelOf.From(value.GetType());
						ExStatus status = xie_core.fnXIE_Core_Param_SetParam(((IxModule)this).GetHandle(), name, new IntPtr(&_value), model);
						if (status != ExStatus.Success)
							throw new CxException(status);
						return;
					}
				case "SaveGraphFile":
					{
						if (!(value is string))
							throw new System.ArgumentException("value type must be string.");
						var _value = new CxStringA((string)value, System.Text.Encoding.Default);
						var model = TxModel.Ptr(1);
						ExStatus status = xie_core.fnXIE_Core_Param_SetParam(((IxModule)this).GetHandle(), name, (IntPtr)((IxModule)_value).GetHandle(), model);
						if (status != ExStatus.Success)
							throw new CxException(status);
						return;
					}
			}
			throw new NotSupportedException("Specified name is not supported.");
		}

		#endregion

		#region 初期化:

		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="window">キャプチャ対象のウィンドウ情報 [CxScreenListItem]</param>
		/// <param name="audio">オーディオ入力 (省略時は null を指定してください。)</param>
		/// <param name="output">出力 (省略時は null を指定してください。)</param>
		public virtual void Setup(IxModule window, IxModule audio = null, IxModule output = null)
		{
			this.Dispose();

			HxModule hThis = ((IxModule)this).GetHandle();
			HxModule hWindow = (window == null) ? IntPtr.Zero : window.GetHandle();
			HxModule hAudio = (audio == null) ? IntPtr.Zero : audio.GetHandle();
			HxModule hOutput = (output == null) ? IntPtr.Zero : output.GetHandle();
			ExStatus status = xie_high.fnXIE_Media_ScreenCapture_Setup(hThis, hWindow, hAudio, hOutput);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region イベントレシーバ 関連:

		/// <summary>
		/// 音声捕獲用イベントレシーバオブジェクト
		/// </summary>
		private CxGrabberInternal AudioGrabber = new CxGrabberInternal();

		/// <summary>
		/// 映像捕獲用イベントレシーバオブジェクト
		/// </summary>
		private CxGrabberInternal VideoGrabber = new CxGrabberInternal();

		/// <summary>
		/// メディア捕獲用イベントレシーバの生成
		/// </summary>
		/// <param name="type">メディア種別</param>
		/// <returns>
		///		指定されたメディア種別のイベントレシーバを返します。
		/// </returns>
		public virtual CxGrabber CreateGrabber(ExMediaType type = ExMediaType.Video)
		{
			switch (type)
			{
				case ExMediaType.Audio:
					return new CxGrabber(AudioGrabber);
				case ExMediaType.Video:
					return new CxGrabber(VideoGrabber);
			}
			throw new System.NotSupportedException();
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// タイムアウト (msec) [-1,0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.Media.CxScreenCapture.Timeout")]
		public virtual int Timeout
		{
			get
			{
				return (int)GetParam("Timeout");
			}
			set
			{
				SetParam("Timeout", value);
			}
		}

		/// <summary>
		/// フレームレート (fps) [1~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.Media.CxScreenCapture.FrameRate")]
		public virtual int FrameRate
		{
			get
			{
				return (int)GetParam("FrameRate");
			}
			set
			{
				SetParam("FrameRate", value);
			}
		}

		#endregion

		#region メソッド:

		/// <summary>
		/// 現在設定されているフレームサイズを取得します。
		/// </summary>
		/// <returns>
		///		フレームサイズを返します。
		/// </returns>
		public virtual TxImageSize GetFrameSize()
		{
			return (TxImageSize)GetParam("FrameSize");
		}

		#endregion
	}
}
