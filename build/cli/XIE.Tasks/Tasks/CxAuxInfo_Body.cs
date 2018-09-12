/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace XIE.Tasks
{
	/// <summary>
	/// 外部機器情報プロパティセット (非公開クラス)
	/// </summary>
	/// <remarks>
	///		クラス自体は非公開ですがプロパティはアプリケーションの
	///		プロパティグリッドに表示できるように設計する必要があります。
	/// </remarks>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	partial class CxAuxInfo_Body : System.Object
		, IDisposable
		, ICloneable
		, IxEquatable
	{
		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAuxInfo_Body()
		{
		}

		#endregion

		#region 初期化と解放.

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Setup()
		{
			Dispose();

			IsSetuped = true;

			this.m_Icons = new CxAuxInfo_Icons();

			// 下位.
			this.AudioInput_Setup();
			this.AudioOutput_Setup();
			this.Camera_Setup();
			this.Grabber_Setup();
			this.DataPort_Setup();
			this.SerialPort_Setup();
			this.TcpServer_Setup();
			this.TcpClient_Setup();

			// 中位.
			this.Media_Setup();	// 注:AudioOutput に依存する.
			this.Image_Setup();

			// 上位.
			this.Task_Setup();
		}

		/// <summary>
		/// IDispose: リソースの解放
		/// </summary>
		public virtual void Dispose()
		{
			IsSetuped = false;

			// 上位.
			this.Task_Dispose();

			// 下位.
			this.AudioInput_Dispose();
			this.AudioOutput_Dispose();
			this.Camera_Dispose();
			this.Grabber_Dispose();
			this.DataPort_Dispose();
			this.SerialPort_Dispose();
			this.TcpServer_Dispose();
			this.TcpClient_Dispose();
			this.Media_Dispose();
			this.Image_Dispose();
		}

		/// <summary>
		/// 浄化
		/// </summary>
		public virtual void Purge()
		{
			this.Media_Purge();
			this.Image_Purge();
			this.Task_Purge();
		}

		/// <summary>
		/// 初期化状態
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual bool IsSetuped
		{
			get { return m_IsSetuped; }
			set { m_IsSetuped = value; }
		}
		[NonSerialized]
		private bool m_IsSetuped = false;

		#endregion

		#region 複製.

		/// <summary>
		/// 自身のクローン生成 (ICloneable)
		/// </summary>
		/// <returns>
		///		自身のクローンを返します。
		/// </returns>
		public virtual object Clone()
		{
			var clone = new CxAuxInfo_Body();
			clone.CopyFrom(this);
			return clone;
		}

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			this.AudioInput_CopyFrom(src);
			this.AudioOutput_CopyFrom(src);
			this.Camera_CopyFrom(src);
			this.Grabber_CopyFrom(src);
			this.DataPort_CopyFrom(src);
			this.SerialPort_CopyFrom(src);
			this.TcpServer_CopyFrom(src);
			this.TcpClient_CopyFrom(src);
			this.Media_CopyFrom(src);
			this.Image_CopyFrom(src);
			this.Task_CopyFrom(src);

			return;
		}

		#endregion

		#region 比較.

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return false;	// [x] null 参照.
			if (!(src is CxAuxInfo_Body)) return false;		// [x] クラスが異なる.

			try
			{
				var _src = (CxAuxInfo_Body)src;

				if (AudioInput_ContentEquals(_src) == false) return false;
				if (AudioOutput_ContentEquals(_src) == false) return false;
				if (Camera_ContentEquals(_src) == false) return false;
				if (Grabber_ContentEquals(_src) == false) return false;
				if (DataPort_ContentEquals(_src) == false) return false;
				if (SerialPort_ContentEquals(_src) == false) return false;
				if (TcpServer_ContentEquals(_src) == false) return false;
				if (TcpClient_ContentEquals(_src) == false) return false;
				if (Media_ContentEquals(_src) == false) return false;
				if (Image_ContentEquals(_src) == false) return false;
				if (Task_ContentEquals(_src) == false) return false;

				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// ハッシュコードの取得
		/// </summary>
		/// <returns>
		///		常に 0 を返します。
		/// </returns>
		public override int GetHashCode()
		{
			return 0;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// イメージリスト
		//

		#region イメージリスト:

		[XmlIgnore]
		[ReadOnly(true)]
		[Browsable(false)]
		[CxCategory("ImageList")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Body.Imagelist16")]
		public virtual ImageList Imagelist16
		{
			get
			{
				if (m_Icons == null)
					return null;
				return m_Icons.imageList16;
			}
		}
		private CxAuxInfo_Icons m_Icons = null;

		#endregion

		// ////////////////////////////////////////////////////////////
		// 音声入力デバイス (Device:AudioInput)
		//

		#region 音声入力デバイス: (プロパティ)

		/// <summary>
		/// 音声入力デバイス: デバイス情報コレクション
		/// </summary>
		[CxCategory("Device")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Body.AudioInputInfos")]
		[XmlArrayItem(typeof(XIE.Media.CxDeviceParam))]
		public virtual XIE.Media.CxDeviceParam[] AudioInputInfos
		{
			get { return m_AudioInputInfos; }
			set { m_AudioInputInfos = value; }
		}
		private XIE.Media.CxDeviceParam[] m_AudioInputInfos = new XIE.Media.CxDeviceParam[0];

		#endregion

		#region 音声入力デバイス: (初期化と解放)

		/// <summary>
		/// 音声入力デバイス: 初期化.
		/// </summary>
		public virtual void AudioInput_Setup()
		{
		}

		/// <summary>
		/// 音声入力デバイス: 解放.
		/// </summary>
		public virtual void AudioInput_Dispose()
		{
		}

		#endregion

		#region 音声入力デバイス: (複製)

		/// <summary>
		/// 音声入力デバイス: 複製.
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void AudioInput_CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;
			// ---
			this.AudioInputInfos = new XIE.Media.CxDeviceParam[_src.AudioInputInfos.Length];
			for (int i = 0; i < this.AudioInputInfos.Length; i++)
				if (_src.AudioInputInfos[i] != null)
					this.AudioInputInfos[i] = (XIE.Media.CxDeviceParam)_src.AudioInputInfos[i].Clone();
		}

		#endregion

		#region 音声入力デバイス: (比較)

		/// <summary>
		/// 音声入力デバイス: 複製.
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool AudioInput_ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return false;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;

			if (this.AudioInputInfos.Length != _src.AudioInputInfos.Length) return false;
			for (int i = 0; i < this.AudioInputInfos.Length; i++)
				if (this.AudioInputInfos[i].ContentEquals(_src.AudioInputInfos[i]) == false) return false;

			return true;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// 音声出力デバイス (Device:AudioOutput)
		//

		#region 音声出力デバイス: (プロパティ)

		/// <summary>
		/// 音声出力デバイス: デバイス情報コレクション
		/// </summary>
		[CxCategory("Device")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Body.AudioOutputInfos")]
		[XmlArrayItem(typeof(XIE.Media.CxDeviceParam))]
		public virtual XIE.Media.CxDeviceParam[] AudioOutputInfos
		{
			get { return m_AudioOutputInfos; }
			set { m_AudioOutputInfos = value; }
		}
		private XIE.Media.CxDeviceParam[] m_AudioOutputInfos = new XIE.Media.CxDeviceParam[0];

		#endregion

		#region 音声出力デバイス: (初期化と解放)

		/// <summary>
		/// 音声出力デバイス: 初期化.
		/// </summary>
		public virtual void AudioOutput_Setup()
		{
		}

		/// <summary>
		/// 音声出力デバイス: 解放.
		/// </summary>
		public virtual void AudioOutput_Dispose()
		{
		}

		#endregion

		#region 音声出力デバイス: (複製)

		/// <summary>
		/// 音声出力デバイス: 複製.
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void AudioOutput_CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;
			// ---
			this.AudioOutputInfos = new XIE.Media.CxDeviceParam[_src.AudioOutputInfos.Length];
			for (int i = 0; i < this.AudioOutputInfos.Length; i++)
				if (_src.AudioOutputInfos[i] != null)
					this.AudioOutputInfos[i] = (XIE.Media.CxDeviceParam)_src.AudioOutputInfos[i].Clone();
		}

		#endregion

		#region 音声出力デバイス: (比較)

		/// <summary>
		/// 音声出力デバイス: 複製.
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool AudioOutput_ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return false;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;

			if (this.AudioOutputInfos.Length != _src.AudioOutputInfos.Length) return false;
			for (int i = 0; i < this.AudioOutputInfos.Length; i++)
				if (this.AudioOutputInfos[i].ContentEquals(_src.AudioOutputInfos[i]) == false) return false;

			return true;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// カメラデバイス (Device:Camera)
		//

		#region カメラデバイス: (プロパティ)

		/// <summary>
		/// カメラデバイス: デバイス情報コレクション
		/// </summary>
		[CxCategory("Device")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Body.CameraInfos")]
		[XmlArrayItem(typeof(XIE.Media.CxDeviceParam))]
		public virtual XIE.Media.CxDeviceParam[] CameraInfos
		{
			get { return m_CameraInfos; }
			set { m_CameraInfos = value; }
		}
		private XIE.Media.CxDeviceParam[] m_CameraInfos = new XIE.Media.CxDeviceParam[0];

		/// <summary>
		/// カメラデバイス: コントローラ
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual XIE.Media.CxCamera[] CameraControllers
		{
			get { return m_CameraControllers; }
			set { m_CameraControllers = value; }
		}
		[NonSerialized]
		private XIE.Media.CxCamera[] m_CameraControllers = new XIE.Media.CxCamera[0];

		#endregion

		#region カメラデバイス: (初期化と解放)

		/// <summary>
		/// カメラデバイス: 初期化.
		/// </summary>
		public virtual void Camera_Setup()
		{
			Camera_Dispose();

			int length = this.CameraInfos.Length;
			this.CameraControllers = new XIE.Media.CxCamera[length];

			#region 生成.
			for (int i = 0; i < this.CameraInfos.Length; i++)
			{
				try
				{
					var info = this.CameraInfos[i];
					var controller = new XIE.Media.CxCamera();
					controller.Setup(info, null, null);
					this.CameraControllers[i] = controller;
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		/// <summary>
		/// カメラデバイス: 解放.
		/// </summary>
		public virtual void Camera_Dispose()
		{
			#region 解放.
			try
			{
				for (int i = 0; i < this.CameraControllers.Length; i++)
				{
					if (this.CameraControllers[i] is IDisposable)
						((IDisposable)this.CameraControllers[i]).Dispose();
					this.CameraControllers[i] = null;
				}
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
			}
			#endregion
		}

		#endregion

		#region カメラデバイス: (複製)

		/// <summary>
		/// カメラデバイス: 複製.
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void Camera_CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;
			// ---
			this.CameraInfos = new XIE.Media.CxDeviceParam[_src.CameraInfos.Length];
			for (int i = 0; i < this.CameraInfos.Length; i++)
				if (_src.CameraInfos[i] != null)
					this.CameraInfos[i] = (XIE.Media.CxDeviceParam)_src.CameraInfos[i].Clone();
		}

		#endregion

		#region カメラデバイス: (比較)

		/// <summary>
		/// カメラデバイス: 複製.
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool Camera_ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return false;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;

			if (this.CameraInfos.Length != _src.CameraInfos.Length) return false;
			for (int i = 0; i < this.CameraInfos.Length; i++)
				if (this.CameraInfos[i].ContentEquals(_src.CameraInfos[i]) == false) return false;

			return true;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// イメージグラバー (Device:Grabber)
		//

		#region イメージグラバー: (プロパティ)

		/// <summary>
		/// イメージグラバー: デバイス情報
		/// </summary>
		[CxCategory("Device")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Body.GrabberInfos")]
		[XmlArrayItem(typeof(CxGrabberInfo))]
		public virtual CxGrabberInfo[] GrabberInfos
		{
			get { return m_GrabberInfos; }
			set { m_GrabberInfos = value; }
		}
		private CxGrabberInfo[] m_GrabberInfos = new CxGrabberInfo[0];

		/// <summary>
		/// イメージグラバー: コントローラオブジェクト
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual CxGrabberThread[] Grabbers
		{
			get { return m_Grabbers; }
			set { m_Grabbers = value; }
		}
		[NonSerialized]
		private CxGrabberThread[] m_Grabbers = new CxGrabberThread[0];

		#endregion

		#region イメージグラバー: (初期化と解放)

		/// <summary>
		/// イメージグラバー: 初期化.
		/// </summary>
		public virtual void Grabber_Setup()
		{
			Grabber_Dispose();

			int length = this.GrabberInfos.Length;
			this.Grabbers = new CxGrabberThread[length];

			#region 生成.
			for (int i = 0; i < this.GrabberInfos.Length; i++)
			{
				try
				{
					var item = this.GrabberInfos[i];
					var controller = (CxGrabberThread)item.CreateInstance();
					controller.Setup();
					this.Grabbers[i] = controller;
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		/// <summary>
		/// イメージグラバー: 解放.
		/// </summary>
		public virtual void Grabber_Dispose()
		{
			#region 解放.
			for (int i = 0; i < Grabbers.Length; i++)
			{
				try
				{
					if (Grabbers[i] != null)
						Grabbers[i].Dispose();
					Grabbers[i] = null;
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		#endregion

		#region イメージグラバー: (複製)

		/// <summary>
		/// イメージグラバー: 複製.
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void Grabber_CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;
			// ---
			this.GrabberInfos = new CxGrabberInfo[_src.GrabberInfos.Length];
			for (int i = 0; i < this.GrabberInfos.Length; i++)
				if (_src.GrabberInfos[i] != null)
					this.GrabberInfos[i] = (CxGrabberInfo)_src.GrabberInfos[i].Clone();
		}

		#endregion

		#region イメージグラバー: (比較)

		/// <summary>
		/// イメージグラバー: 比較.
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool Grabber_ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return false;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;

			if (this.GrabberInfos.Length != _src.GrabberInfos.Length) return false;
			for (int i = 0; i < this.GrabberInfos.Length; i++)
				if (this.GrabberInfos[i].ContentEquals(_src.GrabberInfos[i]) == false) return false;

			return true;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// データ入出力ポート (Device:DataPort)
		//

		#region データ入出力ポート: (プロパティ)

		/// <summary>
		/// データ入出力ポート: デバイス情報
		/// </summary>
		[CxCategory("Device")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Body.DataPortInfos")]
		[XmlArrayItem(typeof(CxDataPortInfo))]
		public virtual CxDataPortInfo[] DataPortInfos
		{
			get { return m_DataPortInfos; }
			set { m_DataPortInfos = value; }
		}
		private CxDataPortInfo[] m_DataPortInfos = new CxDataPortInfo[0];

		/// <summary>
		/// データ入出力ポート: コントローラオブジェクト
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual CxDataPortThread[] DataPorts
		{
			get { return m_DataPorts; }
			set { m_DataPorts = value; }
		}
		[NonSerialized]
		private CxDataPortThread[] m_DataPorts = new CxDataPortThread[0];

		#endregion

		#region データ入出力ポート: (初期化と解放)

		/// <summary>
		/// データ入出力ポート: 初期化.
		/// </summary>
		public virtual void DataPort_Setup()
		{
			DataPort_Dispose();

			int length = this.DataPortInfos.Length;
			this.DataPorts = new CxDataPortThread[length];

			#region 生成.
			for (int i = 0; i < this.DataPortInfos.Length; i++)
			{
				try
				{
					var item = this.DataPortInfos[i];
					var controller = (CxDataPortThread)item.CreateInstance();
					controller.Setup();
					this.DataPorts[i] = controller;
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		/// <summary>
		/// データ入出力ポート: 解放.
		/// </summary>
		public virtual void DataPort_Dispose()
		{
			#region 解放.
			for (int i = 0; i < DataPorts.Length; i++)
			{
				try
				{
					if (DataPorts[i] != null)
						DataPorts[i].Dispose();
					DataPorts[i] = null;
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		#endregion

		#region データ入出力ポート: (複製)

		/// <summary>
		/// データ入出力ポート: 複製.
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void DataPort_CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;
			// ---
			this.DataPortInfos = new CxDataPortInfo[_src.DataPortInfos.Length];
			for (int i = 0; i < this.DataPortInfos.Length; i++)
				if (_src.DataPortInfos[i] != null)
					this.DataPortInfos[i] = (CxDataPortInfo)_src.DataPortInfos[i].Clone();
		}

		#endregion

		#region データ入出力ポート: (比較)

		/// <summary>
		/// データ入出力ポート: 比較.
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool DataPort_ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return false;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;

			if (this.DataPortInfos.Length != _src.DataPortInfos.Length) return false;
			for (int i = 0; i < this.DataPortInfos.Length; i++)
				if (this.DataPortInfos[i].ContentEquals(_src.DataPortInfos[i]) == false) return false;

			return true;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// シリアル通信 (Device:SerialPort)
		//

		#region シリアル通信: (プロパティ)

		/// <summary>
		/// シリアル通信: デバイス情報
		/// </summary>
		[CxCategory("Device")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Body.SerialPortInfos")]
		[XmlArrayItem(typeof(CxSerialPortInfo))]
		public virtual CxSerialPortInfo[] SerialPortInfos
		{
			get { return m_SerialPortInfos; }
			set { m_SerialPortInfos = value; }
		}
		private CxSerialPortInfo[] m_SerialPortInfos = new CxSerialPortInfo[0];

		/// <summary>
		/// シリアル通信: 通信オブジェクト
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual XIE.IO.CxSerialPort[] SerialPorts
		{
			get { return m_SerialPorts; }
			set { m_SerialPorts = value; }
		}
		[NonSerialized]
		private XIE.IO.CxSerialPort[] m_SerialPorts = new XIE.IO.CxSerialPort[0];

		#endregion

		#region シリアル通信: (初期化と解放)

		/// <summary>
		/// シリアル通信: 初期化.
		/// </summary>
		public virtual void SerialPort_Setup()
		{
			SerialPort_Dispose();

			int length = this.SerialPortInfos.Length;
			this.SerialPorts = new XIE.IO.CxSerialPort[length];

			#region 生成.
			for (int i = 0; i < this.SerialPortInfos.Length; i++)
			{
				try
				{
					CxSerialPortInfo item = this.SerialPortInfos[i];
					XIE.IO.CxSerialPort controller = item.CreateController();
					controller.Setup();
					this.SerialPorts[i] = controller;
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		/// <summary>
		/// シリアル通信: 解放.
		/// </summary>
		public virtual void SerialPort_Dispose()
		{
			#region 解放.
			for (int i=0 ; i<SerialPorts.Length ; i++)
			{
				try
				{
					if (SerialPorts[i] != null)
						SerialPorts[i].Dispose();
					SerialPorts[i] = null;
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		#endregion

		#region シリアル通信: (複製)

		/// <summary>
		/// シリアル通信: 複製.
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void SerialPort_CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;
			// ---
			this.SerialPortInfos = new CxSerialPortInfo[_src.SerialPortInfos.Length];
			for (int i = 0; i < this.SerialPortInfos.Length; i++)
				if (_src.SerialPortInfos[i] != null)
					this.SerialPortInfos[i] = (CxSerialPortInfo)_src.SerialPortInfos[i].Clone();
		}

		#endregion

		#region シリアル通信: (比較)

		/// <summary>
		/// シリアル通信: 比較.
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool SerialPort_ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return false;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;

			if (this.SerialPortInfos.Length != _src.SerialPortInfos.Length) return false;
			for (int i = 0; i < this.SerialPortInfos.Length; i++)
				if (this.SerialPortInfos[i].ContentEquals(_src.SerialPortInfos[i]) == false) return false;

			return true;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// TCP/IP 通信 (Device:TCP/IP Server)
		//

		#region TCP/IP Server: (プロパティ)

		/// <summary>
		/// TCP/IP Server: デバイス情報
		/// </summary>
		[CxCategory("Device")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Body.TcpServerInfos")]
		[XmlArrayItem(typeof(CxTcpServerInfo))]
		public virtual CxTcpServerInfo[] TcpServerInfos
		{
			get { return m_TcpServerInfos; }
			set { m_TcpServerInfos = value; }
		}
		private CxTcpServerInfo[] m_TcpServerInfos = new CxTcpServerInfo[0];

		/// <summary>
		/// TCP/IP Server: 通信オブジェクト
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual XIE.Net.CxTcpServer[] TcpServers
		{
			get { return m_TcpServers; }
			set { m_TcpServers = value; }
		}
		[NonSerialized]
		private XIE.Net.CxTcpServer[] m_TcpServers = new XIE.Net.CxTcpServer[0];

		#endregion

		#region TCP/IP Server: (初期化と解放)

		/// <summary>
		/// TCP/IP Server: 初期化.
		/// </summary>
		public virtual void TcpServer_Setup()
		{
			TcpServer_Dispose();

			int length = this.TcpServerInfos.Length;
			this.TcpServers = new XIE.Net.CxTcpServer[length];

			#region 生成.
			for (int i = 0; i < TcpServerInfos.Length; i++)
			{
				try
				{
					CxTcpServerInfo info = TcpServerInfos[i];
					XIE.Net.CxTcpServer controller = info.CreateController();
					controller.Setup();
					controller.Start();
					this.TcpServers[i] = controller;
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		/// <summary>
		/// TCP/IP Server: 解放.
		/// </summary>
		public virtual void TcpServer_Dispose()
		{
			#region 解放.
			for (int i = 0; i < TcpServers.Length; i++)
			{
				try
				{
					if (TcpServers[i] != null)
						TcpServers[i].Dispose();
					TcpServers[i] = null;
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		#endregion

		#region TCP/IP Server: (複製)

		/// <summary>
		/// TCP/IP Server: 複製.
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void TcpServer_CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;
			// ---
			this.TcpServerInfos = new CxTcpServerInfo[_src.TcpServerInfos.Length];
			for (int i = 0; i < this.TcpServerInfos.Length; i++)
				if (_src.TcpServerInfos[i] != null)
					this.TcpServerInfos[i] = (CxTcpServerInfo)_src.TcpServerInfos[i].Clone();
		}

		#endregion

		#region TCP/IP Server: (比較)

		/// <summary>
		/// TCP/IP Server: 比較.
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool TcpServer_ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return false;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;

			if (this.TcpServerInfos.Length != _src.TcpServerInfos.Length) return false;
			for (int i = 0; i < this.TcpServerInfos.Length; i++)
				if (this.TcpServerInfos[i].ContentEquals(_src.TcpServerInfos[i]) == false) return false;

			return true;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// TCP/IP 通信 (Device:TCP/IP Client)
		//

		#region TCP/IP Client: (プロパティ)

		/// <summary>
		/// TCP/IP Client: デバイス情報
		/// </summary>
		[CxCategory("Device")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Body.TcpClientInfos")]
		[XmlArrayItem(typeof(CxTcpClientInfo))]
		public virtual CxTcpClientInfo[] TcpClientInfos
		{
			get { return m_TcpClientInfos; }
			set { m_TcpClientInfos = value; }
		}
		private CxTcpClientInfo[] m_TcpClientInfos = new CxTcpClientInfo[0];

		/// <summary>
		/// TCP/IP Client: 通信オブジェクト
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual XIE.Net.CxTcpClient[] TcpClients
		{
			get { return m_TcpClients; }
			set { m_TcpClients = value; }
		}
		[NonSerialized]
		private XIE.Net.CxTcpClient[] m_TcpClients = new XIE.Net.CxTcpClient[0];

		#endregion

		#region TCP/IP Client: (初期化と解放)

		/// <summary>
		/// TCP/IP Client: 初期化.
		/// </summary>
		public virtual void TcpClient_Setup()
		{
			TcpClient_Dispose();

			int length = this.TcpClientInfos.Length;
			this.TcpClients = new XIE.Net.CxTcpClient[length];

			#region 生成.
			for (int i=0 ; i<TcpClientInfos.Length ; i++)
			{
				try
				{
					CxTcpClientInfo info = TcpClientInfos[i];
					XIE.Net.CxTcpClient controller = info.CreateController();
					controller.Setup();
					controller.Start();
					this.TcpClients[i] = controller;
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		/// <summary>
		/// TCP/IP Client: 解放.
		/// </summary>
		public virtual void TcpClient_Dispose()
		{
			#region 解放.
			for (int i = 0; i < TcpClients.Length; i++)
			{
				try
				{
					if (TcpClients[i] != null)
						TcpClients[i].Dispose();
					TcpClients[i] = null;
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		#endregion

		#region TCP/IP Client: (複製)

		/// <summary>
		/// TCP/IP Client: 複製.
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void TcpClient_CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;
			// ---
			this.TcpClientInfos = new CxTcpClientInfo[_src.TcpClientInfos.Length];
			for (int i = 0; i < this.TcpClientInfos.Length; i++)
				if (_src.TcpClientInfos[i] != null)
					this.TcpClientInfos[i] = (CxTcpClientInfo)_src.TcpClientInfos[i].Clone();
		}

		#endregion

		#region TCP/IP Client: (比較)

		/// <summary>
		/// TCP/IP Client: 比較.
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool TcpClient_ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return false;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;

			if (this.TcpClientInfos.Length != _src.TcpClientInfos.Length) return false;
			for (int i = 0; i < this.TcpClientInfos.Length; i++)
				if (this.TcpClientInfos[i].ContentEquals(_src.TcpClientInfos[i]) == false) return false;

			return true;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// メディアプレイヤー (Data:Media)
		//

		#region メディアプレイヤー: (プロパティ)

		/// <summary>
		/// メディアプレイヤー: 動画ファイル情報コレクション
		/// </summary>
		[CxCategory("Data")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Body.MediaInfos")]
		[XmlArrayItem(typeof(CxMediaInfo))]
		public virtual CxMediaInfo[] MediaInfos
		{
			get { return m_MediaInfos; }
			set { m_MediaInfos = value; }
		}
		private CxMediaInfo[] m_MediaInfos = new CxMediaInfo[0];

		/// <summary>
		/// メディアプレイヤー: メディアプレイヤーコレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual XIE.Media.CxMediaPlayer[] MediaPlayers
		{
			get { return m_MediaPlayers; }
			set { m_MediaPlayers = value; }
		}
		[NonSerialized]
		private XIE.Media.CxMediaPlayer[] m_MediaPlayers = new XIE.Media.CxMediaPlayer[0];

		#endregion

		#region メディアプレイヤー: (初期化と解放)

		/// <summary>
		/// メディアプレイヤー: 初期化.
		/// </summary>
		public virtual void Media_Setup()
		{
			Media_Dispose();

			int length = this.MediaInfos.Length;
			this.MediaPlayers = new XIE.Media.CxMediaPlayer[length];

			#region 生成.
			for (int i = 0; i < length; i++)
			{
				try
				{
					XIE.Media.CxDeviceParam audioOutput = null;
					if (AudioOutputInfos.Length != 0)
						audioOutput = AudioOutputInfos[0];
					this.MediaPlayers[i] = this.MediaInfos[i].CreateMedia(audioOutput);
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		/// <summary>
		/// メディアプレイヤー: 解放.
		/// </summary>
		public virtual void Media_Dispose()
		{
			#region 解放.
			try
			{
				for (int i = 0; i < this.MediaPlayers.Length; i++)
				{
					if (this.MediaPlayers[i] != null)
						this.MediaPlayers[i].Dispose();
					this.MediaPlayers[i] = null;
				}
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
			}
			#endregion
		}

		/// <summary>
		/// メディアプレイヤー: 浄化
		/// </summary>
		public virtual void Media_Purge()
		{
			var pairs = new Dictionary<CxMediaInfo, XIE.Media.CxMediaPlayer>();
			for (int i = 0; i < this.MediaInfos.Length; i++)
				pairs[this.MediaInfos[i]] = this.MediaPlayers[i];

			#region 不正な要素の除去.
			foreach (var item in pairs)
			{
				if (item.Value == null)
				{
					if (string.IsNullOrEmpty(item.Key.FileName) ||
						!System.IO.File.Exists(item.Key.FileName))
					{
						this.MediaInfos = ArrayTool.Remove(this.MediaInfos, item.Key).ToArray();
					}
				}
			}
			#endregion
		}

		#endregion

		#region メディアプレイヤー: (複製)

		/// <summary>
		/// メディアプレイヤー: 複製.
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void Media_CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;
			// ---
			this.MediaInfos = new CxMediaInfo[_src.MediaInfos.Length];
			for (int i = 0; i < this.MediaInfos.Length; i++)
				if (_src.MediaInfos[i] != null)
					this.MediaInfos[i] = (CxMediaInfo)_src.MediaInfos[i].Clone();
		}

		#endregion

		#region メディアプレイヤー: (比較)

		/// <summary>
		/// メディアプレイヤー: 複製.
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool Media_ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return false;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;

			if (this.MediaInfos.Length != _src.MediaInfos.Length) return false;
			for (int i = 0; i < this.MediaInfos.Length; i++)
				if (this.MediaInfos[i].ContentEquals(_src.MediaInfos[i]) == false) return false;

			return true;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// 画像オブジェクト (Data:Image)
		//

		#region 画像オブジェクト: (プロパティ)

		/// <summary>
		/// 画像オブジェクト: データ情報コレクション
		/// </summary>
		[CxCategory("Data")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Body.ImageInfos")]
		[XmlArrayItem(typeof(CxImageInfo))]
		public virtual CxImageInfo[] ImageInfos
		{
			get { return m_ImageInfos; }
			set { m_ImageInfos = value; }
		}
		private CxImageInfo[] m_ImageInfos = new CxImageInfo[0];

		/// <summary>
		/// 画像オブジェクト: データコレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual XIE.CxImage[] Images
		{
			get { return m_Images; }
			set { m_Images = value; }
		}
		[NonSerialized]
		private XIE.CxImage[] m_Images = new XIE.CxImage[0];

		#endregion

		#region 画像オブジェクト: (初期化と解放)

		/// <summary>
		/// 画像オブジェクト: 初期化.
		/// </summary>
		public virtual void Image_Setup()
		{
			Image_Dispose();

			int length = this.ImageInfos.Length;
			this.Images = new XIE.CxImage[length];

			#region 生成.
			for (int i = 0; i < length; i++)
			{
				try
				{
					var info = this.ImageInfos[i];
					if (!string.IsNullOrEmpty(info.FileName) &&
						System.IO.File.Exists(info.FileName))
					{
						this.Images[i] = new XIE.CxImage(info.FileName);
					}
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		/// <summary>
		/// 画像オブジェクト: 解放.
		/// </summary>
		public virtual void Image_Dispose()
		{
			#region 解放.
			try
			{
				for (int i = 0; i < this.Images.Length; i++)
				{
					if (this.Images[i] != null)
						this.Images[i].Dispose();
					this.Images[i] = null;
				}
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
			}
			#endregion
		}

		/// <summary>
		/// 画像オブジェクト: 浄化
		/// </summary>
		public virtual void Image_Purge()
		{
			var pairs = new Dictionary<CxImageInfo, XIE.CxImage>();
			for (int i = 0; i < this.ImageInfos.Length; i++)
				pairs[this.ImageInfos[i]] = this.Images[i];

			#region 不正な要素の除去.
			foreach (var item in pairs)
			{
				if (item.Value == null)
				{
					if (string.IsNullOrEmpty(item.Key.FileName) ||
						!System.IO.File.Exists(item.Key.FileName))
					{
						this.ImageInfos = ArrayTool.Remove(this.ImageInfos, item.Key).ToArray();
					}
				}
			}
			#endregion
		}

		#endregion

		#region 画像オブジェクト: (複製)

		/// <summary>
		/// 画像オブジェクト: 複製.
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void Image_CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;
			// ---
			this.ImageInfos = new CxImageInfo[_src.ImageInfos.Length];
			for (int i = 0; i < this.ImageInfos.Length; i++)
				if (_src.ImageInfos[i] != null)
					this.ImageInfos[i] = (CxImageInfo)_src.ImageInfos[i].Clone();
		}

		#endregion

		#region 画像オブジェクト: (比較)

		/// <summary>
		/// 画像オブジェクト: 複製.
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool Image_ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return false;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;

			if (this.ImageInfos.Length != _src.ImageInfos.Length) return false;
			for (int i = 0; i < this.ImageInfos.Length; i++)
				if (this.ImageInfos[i].ContentEquals(_src.ImageInfos[i]) == false) return false;

			return true;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// タスクユニット (Tasks:Task)
		//

		#region タスクユニット: (プロパティ)

		/// <summary>
		/// タスクユニット: タスクユニット情報コレクション
		/// </summary>
		[CxCategory("Tasks")]
		[CxDescription("P:XIE.Tasks.CxAuxInfo_Body.TaskUnitInfos")]
		[XmlArrayItem(typeof(CxTaskUnitInfo))]
		public virtual CxTaskUnitInfo[] TaskUnitInfos
		{
			get { return m_TaskInfos; }
			set { m_TaskInfos = value; }
		}
		private CxTaskUnitInfo[] m_TaskInfos = new CxTaskUnitInfo[0];

		/// <summary>
		/// タスクユニット: タスクユニットコレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual XIE.Tasks.CxTaskUnit[] TaskUnits
		{
			get { return m_TaskUnits; }
			set { m_TaskUnits = value; }
		}
		[NonSerialized]
		private XIE.Tasks.CxTaskUnit[] m_TaskUnits = new XIE.Tasks.CxTaskUnit[0];

		#endregion

		#region タスクユニット: (初期化と解放)

		/// <summary>
		/// タスクユニット: 初期化.
		/// </summary>
		public virtual void Task_Setup()
		{
			Task_Dispose();

			int length = this.TaskUnitInfos.Length;
			this.TaskUnits = new XIE.Tasks.CxTaskUnit[length];

			#region 生成.
			for (int i = 0; i < length; i++)
			{
				try
				{
					this.TaskUnits[i] = this.TaskUnitInfos[i].CreateTaskUnit();
				}
				catch (System.Exception ex)
				{
					XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
				}
			}
			#endregion
		}

		/// <summary>
		/// タスクユニット: 解放.
		/// </summary>
		public virtual void Task_Dispose()
		{
			#region 解放.
			try
			{
				for (int i = 0; i < this.TaskUnits.Length; i++)
				{
					if (this.TaskUnits[i] != null)
						this.TaskUnits[i].Dispose();
					this.TaskUnits[i] = null;
				}
			}
			catch (System.Exception ex)
			{
				XIE.Log.Api.Error(ex.Message + "\n" + ex.StackTrace);
			}
			#endregion
		}

		/// <summary>
		/// タスクユニット: 浄化
		/// </summary>
		public virtual void Task_Purge()
		{
			var pairs = new Dictionary<CxTaskUnitInfo, XIE.Tasks.CxTaskUnit>();
			for (int i = 0; i < this.TaskUnitInfos.Length; i++)
				pairs[this.TaskUnitInfos[i]] = this.TaskUnits[i];

			#region 不正な要素の除去.
			foreach (var item in pairs)
			{
				if (item.Value == null)
				{
					if (string.IsNullOrEmpty(item.Key.FileName) ||
						!System.IO.File.Exists(item.Key.FileName))
					{
						this.TaskUnitInfos = ArrayTool.Remove(this.TaskUnitInfos, item.Key).ToArray();
					}
				}
			}
			#endregion
		}

		#endregion

		#region タスクユニット: (複製)

		/// <summary>
		/// タスクユニット: 複製.
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void Task_CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;
			// ---
			this.TaskUnitInfos = new CxTaskUnitInfo[_src.TaskUnitInfos.Length];
			for (int i = 0; i < this.TaskUnitInfos.Length; i++)
				if (_src.TaskUnitInfos[i] != null)
					this.TaskUnitInfos[i] = (CxTaskUnitInfo)_src.TaskUnitInfos[i].Clone();
		}

		#endregion

		#region タスクユニット: (比較)

		/// <summary>
		/// タスクユニット: 複製.
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool Task_ContentEquals(object src)
		{
			if (ReferenceEquals(src, this)) return true;	// [o] 同一インスタンス.
			if (ReferenceEquals(src, null)) return false;	// [x] null 参照.

			var _src = (CxAuxInfo_Body)src;

			if (this.TaskUnitInfos.Length != _src.TaskUnitInfos.Length) return false;
			for (int i = 0; i < this.TaskUnitInfos.Length; i++)
				if (this.TaskUnitInfos[i].ContentEquals(_src.TaskUnitInfos[i]) == false) return false;

			return true;
		}

		#endregion
	}
}