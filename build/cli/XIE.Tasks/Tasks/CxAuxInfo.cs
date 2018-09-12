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
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace XIE.Tasks
{
	/// <summary>
	/// 外部機器情報
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public partial class CxAuxInfo : System.Object
		, IDisposable
		, IXmlSerializable
		, ICloneable
		, IxEquatable
		, IxFileAccess
		, IxAuxImageList16
		, IxAuxInfoAudioInputs		// Aux インターフェース.
		, IxAuxInfoAudioOutputs		// Aux インターフェース.
		, IxAuxInfoCameras			// Aux インターフェース.
		, IxAuxInfoGrabbers			// Aux インターフェース.
		, IxAuxInfoDataPorts		// Aux インターフェース.
		, IxAuxInfoSerialPorts		// Aux インターフェース.
		, IxAuxInfoTcpServers		// Aux インターフェース.
		, IxAuxInfoTcpClients		// Aux インターフェース.
		, IxAuxInfoMedias			// Aux インターフェース.
		, IxAuxInfoImages			// Aux インターフェース.
		, IxAuxInfoTasks			// Aux インターフェース.
	{
		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAuxInfo()
		{
		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxAuxInfo()
		{
			Dispose();
		}

		#endregion

		#region 初期化:

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Setup()
		{
			if (m_Body != null)
			{
				Dispose();
				IsSetuped = true;
				m_Body.Setup();
			}
		}

		/// <summary>
		/// 初期化状態
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual bool IsSetuped
		{
			get { return m_IsSetuped; }
			protected set { m_IsSetuped = value; }
		}
		[NonSerialized]
		private bool m_IsSetuped = false;

		#endregion

		#region 浄化:

		/// <summary>
		/// 浄化
		/// </summary>
		/// <remarks>
		///		外部ファイルを参照する要素(Medias,Images,Tasks)の整合性を確認し、
		///		不正な要素があれば配列から除去します。
		///		不正か否かの判定は外部ファイルの存在の有無によって行います。
		///		但し、ファイル情報に対となるオブジェクトが存在するものは対象外です。
		/// </remarks>
		public virtual void Purge()
		{
			if (m_Body != null)
				m_Body.Purge();
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 基準ディレクトリ
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual string ProjectDir
		{
			get { return m_ProjectDir; }
			set { m_ProjectDir = value; }
		}
		[NonSerialized]
		private string m_ProjectDir = "";

		/// <summary>
		/// デバッグモード
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual bool DebugMode
		{
			get { return m_DebugMode; }
			set { m_DebugMode = value; }
		}
		[NonSerialized]
		private bool m_DebugMode = false;

		/// <summary>
		/// 実体
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		public virtual object Body
		{
			get { return m_Body; }
		}
		[NonSerialized]
		private CxAuxInfo_Body m_Body = new CxAuxInfo_Body();
		
		#endregion

		#region イベント:

		/// <summary>
		/// 変更要求イベント
		/// </summary>
		public event CxAuxNotifyEventHandler Requested;

		/// <summary>
		/// 変更要求イベント送信
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">引数</param>
		public virtual void SendRequested(object sender, CxAuxNotifyEventArgs e)
		{
			if (Requested != null)
				Requested.Invoke(sender, e);
		}

		/// <summary>
		/// 更新通知イベント
		/// </summary>
		public event CxAuxNotifyEventHandler Updated;

		/// <summary>
		/// 更新通知イベント送信
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="e">引数</param>
		public virtual void SendUpdated(object sender, CxAuxNotifyEventArgs e)
		{
			if (Updated != null)
				Updated.Invoke(sender, e);
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// インターフェースの実装:
		//

		#region IDisposable の実装:

		/// <summary>
		/// IDisposable の実装: リソースの解放
		/// </summary>
		public virtual void Dispose()
		{
			IsSetuped = false;

			if (m_Body != null)
				m_Body.Dispose();
		}

		#endregion

		#region IXmlSerializable の実装:

		/// <summary>
		/// IXmlSerializable の実装: オブジェクトの XML 表現からオブジェクトを生成します。
		/// </summary>
		/// <param name="reader">オブジェクトの逆シリアル化元である XmlReader ストリーム。</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			reader.ReadStartElement();

			#region 一時読み込み用コレクション
			var AudioInputInfos = new List<XIE.Media.CxDeviceParam>();
			var AudioOutputInfos = new List<XIE.Media.CxDeviceParam>();
			var CameraInfos = new List<XIE.Media.CxDeviceParam>();
			var SerialPortInfos = new List<CxSerialPortInfo>();
			var TcpServerInfos = new List<CxTcpServerInfo>();
			var TcpClientInfos = new List<CxTcpClientInfo>();
			var MediaInfos = new List<CxMediaInfo>();
			var ImageInfos = new List<CxImageInfo>();
			var TaskUnitInfos = new List<CxTaskUnitInfo>();
			#endregion

			#region Device
			if (reader.Name == "AudioInputInfos")
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement(reader.Name);
				}
				else
				{
					reader.ReadStartElement(reader.Name);
					while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
						AudioInputInfos.Add(XIE.AxiXml.GetValue<XIE.Media.CxDeviceParam>(reader));
					reader.ReadEndElement();
				}
			}

			if (reader.Name == "AudioOutputInfos")
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement(reader.Name);
				}
				else
				{
					reader.ReadStartElement(reader.Name);
					while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
						AudioOutputInfos.Add(XIE.AxiXml.GetValue<XIE.Media.CxDeviceParam>(reader));
					reader.ReadEndElement();
				}
			}

			if (reader.Name == "CameraInfos")
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement(reader.Name);
				}
				else
				{
					reader.ReadStartElement(reader.Name);
					while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
						CameraInfos.Add(XIE.AxiXml.GetValue<XIE.Media.CxDeviceParam>(reader));
					reader.ReadEndElement();
				}
			}

			if (reader.Name == "SerialPortInfos")
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement(reader.Name);
				}
				else
				{
					reader.ReadStartElement(reader.Name);
					while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
						SerialPortInfos.Add(XIE.AxiXml.GetValue<CxSerialPortInfo>(reader));
					reader.ReadEndElement();
				}
			}

			if (reader.Name == "TcpServerInfos")
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement(reader.Name);
				}
				else
				{
					reader.ReadStartElement(reader.Name);
					while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
						TcpServerInfos.Add(XIE.AxiXml.GetValue<CxTcpServerInfo>(reader));
					reader.ReadEndElement();
				}
			}

			if (reader.Name == "TcpClientInfos")
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement(reader.Name);
				}
				else
				{
					reader.ReadStartElement(reader.Name);
					while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
						TcpClientInfos.Add(XIE.AxiXml.GetValue<CxTcpClientInfo>(reader));
					reader.ReadEndElement();
				}
			}
			#endregion

			#region Data
			if (reader.Name == "MediaInfos")
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement(reader.Name);
				}
				else
				{
					reader.ReadStartElement(reader.Name);
					while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
						MediaInfos.Add(XIE.AxiXml.GetValue<CxMediaInfo>(reader));
					reader.ReadEndElement();
				}
			}

			if (reader.Name == "ImageInfos")
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement(reader.Name);
				}
				else
				{
					reader.ReadStartElement(reader.Name);
					while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
						ImageInfos.Add(XIE.AxiXml.GetValue<CxImageInfo>(reader));
					reader.ReadEndElement();
				}
			}
			#endregion

			#region Tasks
			if (reader.Name == "TaskUnitInfos")
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement(reader.Name);
				}
				else
				{
					reader.ReadStartElement(reader.Name);
					while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
						TaskUnitInfos.Add(XIE.AxiXml.GetValue<CxTaskUnitInfo>(reader));
					reader.ReadEndElement();
				}
			}

			#endregion

			#region コレクションから復元する.
			this.m_Body.AudioInputInfos = AudioInputInfos.ToArray();
			this.m_Body.AudioOutputInfos = AudioOutputInfos.ToArray();
			this.m_Body.CameraInfos = CameraInfos.ToArray();
			this.m_Body.SerialPortInfos = SerialPortInfos.ToArray();
			this.m_Body.TcpServerInfos = TcpServerInfos.ToArray();
			this.m_Body.TcpClientInfos = TcpClientInfos.ToArray();
			this.m_Body.MediaInfos = MediaInfos.ToArray();
			this.m_Body.ImageInfos = ImageInfos.ToArray();
			this.m_Body.TaskUnitInfos = TaskUnitInfos.ToArray();
			#endregion

			reader.ReadEndElement();
		}

		/// <summary>
		/// IXmlSerializable の実装: オブジェクトを XML 表現に変換します。
		/// </summary>
		/// <param name="writer">オブジェクトのシリアル化先の XmlWriter ストリーム。</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			#region Device
			// --
			{
				writer.WriteStartElement("AudioInputInfos");
				foreach (var item in this.m_Body.AudioInputInfos)
					XIE.AxiXml.AddValue(writer, item, null, null);
				writer.WriteEndElement();
			}
			// --
			{
				writer.WriteStartElement("AudioOutputInfos");
				foreach (var item in this.m_Body.AudioOutputInfos)
					XIE.AxiXml.AddValue(writer, item, null, null);
				writer.WriteEndElement();
			}
			// --
			{
				writer.WriteStartElement("CameraInfos");
				foreach (var item in this.m_Body.CameraInfos)
					XIE.AxiXml.AddValue(writer, item, null, null);
				writer.WriteEndElement();
			}
			// --
			{
				writer.WriteStartElement("SerialPortInfos");
				foreach (var item in this.m_Body.SerialPortInfos)
					XIE.AxiXml.AddValue(writer, item, null, null);
				writer.WriteEndElement();
			}
			// --
			{
				writer.WriteStartElement("TcpServerInfos");
				foreach (var item in this.m_Body.TcpServerInfos)
					XIE.AxiXml.AddValue(writer, item, null, null);
				writer.WriteEndElement();
			}
			// --
			{
				writer.WriteStartElement("TcpClientInfos");
				foreach (var item in this.m_Body.TcpClientInfos)
					XIE.AxiXml.AddValue(writer, item, null, null);
				writer.WriteEndElement();
			}
			#endregion

			#region Data
			// --
			{
				writer.WriteStartElement("MediaInfos");
				foreach (var item in this.m_Body.MediaInfos)
					XIE.AxiXml.AddValue(writer, item, null, null);
				writer.WriteEndElement();
			}
			// --
			{
				writer.WriteStartElement("ImageInfos");
				foreach (var item in this.m_Body.ImageInfos)
					XIE.AxiXml.AddValue(writer, item, null, null);
				writer.WriteEndElement();
			}
			#endregion

			#region Tasks
			// --
			{
				writer.WriteStartElement("TaskUnitInfos");
				foreach (var item in this.m_Body.TaskUnitInfos)
					XIE.AxiXml.AddValue(writer, item, null, null);
				writer.WriteEndElement();
			}
			#endregion
		}

		/// <summary>
		/// IXmlSerializable の実装: オブジェクトの XML 表現を記述する XmlSchema
		/// </summary>
		/// <returns>
		///		常に null を返します。
		/// </returns>
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// 自身のクローン生成 (ICloneable)
		/// </summary>
		/// <returns>
		///		自身のクローンを返します。
		/// </returns>
		public virtual object Clone()
		{
			var type = this.GetType();
			var asm = Assembly.GetAssembly(type);
			var clone = asm.CreateInstance(type.FullName);
			((IxEquatable)clone).CopyFrom(this);
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
			if (ReferenceEquals(this, src)) return;

			var _src = (CxAuxInfo)src;

			this.ProjectDir = _src.ProjectDir;
			this.DebugMode = _src.DebugMode;
			this.m_Body.CopyFrom(_src.m_Body);

			return;
		}

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
			if (!(src is CxAuxInfo)) return false;			// [x] クラスが異なる.

			try
			{
				var _src = (CxAuxInfo)src;

				if (this.ProjectDir != _src.ProjectDir) return false;
				if (this.DebugMode != _src.DebugMode) return false;
				if (this.m_Body.ContentEquals(_src.m_Body) == false) return false;

				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region IxFileAccess の実装:

		/// <summary>
		/// ファイル読み込み
		/// </summary>
		/// <param name="filename">XML ファイル名。</param>
		/// <param name="options">オプション。</param>
		public static CxAuxInfo LoadFrom(string filename, params object[] options)
		{
			return (CxAuxInfo)XIE.Axi.ReadAsXml(filename, typeof(CxAuxInfo));
		}

		/// <summary>
		/// IxFileAccess の実装: ファイル読み込み
		/// </summary>
		/// <param name="filename">XML ファイル名。</param>
		/// <param name="options">オプション。(未使用)</param>
		/// <remarks>
		///		追加したプロパティを Load 関数で読み込めない場合は、IxEquatable.CopyFrom の処理をご確認ください.
		/// </remarks>
		public virtual void Load(string filename, params object[] options)
		{
			object result = XIE.Axi.ReadAsXml(filename, this.GetType());
			this.CopyFrom(result);
		}

		/// <summary>
		/// IxFileAccess の実装: ファイル保存
		/// </summary>
		/// <param name="filename">パラメータファイル(XML)</param>
		/// <param name="options">オプション。(未使用)</param>
		public virtual void Save(string filename, params object[] options)
		{
			XIE.Axi.WriteAsXml(filename, this);
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// インターフェースの実装: (イメージリストインターフェース)
		//

		#region IxAuxImageList16 インターフェースの実装:

		/// <summary>
		/// イメージリスト
		/// </summary>
		ImageList IxAuxImageList16.ImageList
		{
			get { return this.m_Body.Imagelist16; }
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// インターフェースの実装: (Aux インターフェース)
		//

		#region IxAuxInfoAudioInputs インターフェースの実装:

		/// <summary>
		/// 音声入力デバイス: デバイス情報コレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		XIE.Media.CxDeviceParam[] IxAuxInfoAudioInputs.Infos
		{
			get { return m_Body.AudioInputInfos; }
		}

		/// <summary>
		/// 音声入力デバイス: 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		void IxAuxInfoAudioInputs.Add(XIE.Media.CxDeviceParam info)
		{
			m_Body.AudioInputInfos = ArrayTool.Add(m_Body.AudioInputInfos, info).ToArray();
		}

		/// <summary>
		/// 音声入力デバイス: 全て削除.
		/// </summary>
		void IxAuxInfoAudioInputs.RemoveAll()
		{
			m_Body.AudioInputInfos = new XIE.Media.CxDeviceParam[0];
		}

		/// <summary>
		/// 音声入力デバイス: 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void IxAuxInfoAudioInputs.RemoveAt(int index)
		{
			m_Body.AudioInputInfos = ArrayTool.RemoveAt(m_Body.AudioInputInfos, index).ToArray();
		}

		/// <summary>
		/// 音声入力デバイス: 削除.
		/// </summary>
		/// <param name="src">デバイス情報</param>
		void IxAuxInfoAudioInputs.Remove(object src)
		{
			if (src is XIE.Media.CxDeviceParam)
			{
				int index = ArrayTool.Find(m_Body.AudioInputInfos, (XIE.Media.CxDeviceParam)src);
				if (0 <= index)
					((IxAuxInfoAudioInputs)this).RemoveAt(index);
				return;
			}
			throw new System.NotSupportedException();
		}

		/// <summary>
		/// 音声入力デバイス: 検索.
		/// </summary>
		/// <param name="src">デバイス情報</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int IxAuxInfoAudioInputs.Find(object src)
		{
			if (src is XIE.Media.CxDeviceParam)
			{
				return ArrayTool.Find(m_Body.AudioInputInfos, (XIE.Media.CxDeviceParam)src);
			}
			return -1;
		}

		#endregion

		#region IxAuxInfoAudioOutputs インターフェースの実装:

		/// <summary>
		/// 音声出力デバイス: デバイス情報コレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		XIE.Media.CxDeviceParam[] IxAuxInfoAudioOutputs.Infos
		{
			get { return m_Body.AudioOutputInfos; }
		}

		/// <summary>
		/// 音声出力デバイス: 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		void IxAuxInfoAudioOutputs.Add(XIE.Media.CxDeviceParam info)
		{
			m_Body.AudioOutputInfos = ArrayTool.Add(m_Body.AudioOutputInfos, info).ToArray();
		}

		/// <summary>
		/// 音声出力デバイス: 全て削除.
		/// </summary>
		void IxAuxInfoAudioOutputs.RemoveAll()
		{
			m_Body.AudioOutputInfos = new XIE.Media.CxDeviceParam[0];
		}

		/// <summary>
		/// 音声出力デバイス: 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void IxAuxInfoAudioOutputs.RemoveAt(int index)
		{
			m_Body.AudioOutputInfos = ArrayTool.RemoveAt(m_Body.AudioOutputInfos, index).ToArray();
		}

		/// <summary>
		/// 音声出力デバイス: 削除.
		/// </summary>
		/// <param name="src">デバイス情報</param>
		void IxAuxInfoAudioOutputs.Remove(object src)
		{
			if (src is XIE.Media.CxDeviceParam)
			{
				int index = ArrayTool.Find(m_Body.AudioOutputInfos, (XIE.Media.CxDeviceParam)src);
				if (0 <= index)
					((IxAuxInfoAudioOutputs)this).RemoveAt(index);
				return;
			}
			throw new System.NotSupportedException();
		}

		/// <summary>
		/// 音声出力デバイス: 検索.
		/// </summary>
		/// <param name="src">デバイス情報</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int IxAuxInfoAudioOutputs.Find(object src)
		{
			if (src is XIE.Media.CxDeviceParam)
			{
				return ArrayTool.Find(m_Body.AudioOutputInfos, (XIE.Media.CxDeviceParam)src);
			}
			return -1;
		}

		#endregion

		#region IxAuxInfoCameras インターフェースの実装:

		/// <summary>
		/// カメラデバイス: デバイス情報コレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		XIE.Media.CxDeviceParam[] IxAuxInfoCameras.Infos
		{
			get { return m_Body.CameraInfos; }
		}

		/// <summary>
		/// カメラデバイス: コントローラコレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		XIE.Media.CxCamera[] IxAuxInfoCameras.Controllers
		{
			get { return m_Body.CameraControllers; }
		}

		/// <summary>
		/// カメラデバイス: 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		void IxAuxInfoCameras.Add(XIE.Media.CxDeviceParam info, XIE.Media.CxCamera controller)
		{
			m_Body.CameraInfos = ArrayTool.Add(m_Body.CameraInfos, info).ToArray();
			m_Body.CameraControllers = ArrayTool.Add(m_Body.CameraControllers, controller).ToArray();
		}

		/// <summary>
		/// カメラデバイス: 全て削除.
		/// </summary>
		void IxAuxInfoCameras.RemoveAll()
		{
			for (int i = 0; i < m_Body.CameraControllers.Length; i++)
			{
				if (m_Body.CameraControllers[i] != null)
					m_Body.CameraControllers[i].Dispose();
				m_Body.CameraControllers[i] = null;
			}

			m_Body.CameraInfos = new XIE.Media.CxDeviceParam[0];
			m_Body.CameraControllers = new XIE.Media.CxCamera[0];
		}

		/// <summary>
		/// カメラデバイス: 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void IxAuxInfoCameras.RemoveAt(int index)
		{
			if (0 <= index && index < m_Body.CameraControllers.Length)
			{
				if (m_Body.CameraControllers[index] != null)
					m_Body.CameraControllers[index].Dispose();
				m_Body.CameraControllers[index] = null;
			}

			m_Body.CameraInfos = ArrayTool.RemoveAt(m_Body.CameraInfos, index).ToArray();
			m_Body.CameraControllers = ArrayTool.RemoveAt(m_Body.CameraControllers, index).ToArray();
		}

		/// <summary>
		/// カメラデバイス: 削除.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		void IxAuxInfoCameras.Remove(object src)
		{
			if (src is XIE.Media.CxDeviceParam)
			{
				int index = ArrayTool.Find(m_Body.CameraInfos, (XIE.Media.CxDeviceParam)src);
				if (0 <= index)
					((IxAuxInfoCameras)this).RemoveAt(index);
				return;
			}
			if (src is XIE.Media.CxCamera)
			{
				int index = ArrayTool.Find(m_Body.CameraControllers, (XIE.Media.CxCamera)src);
				if (0 <= index)
					((IxAuxInfoCameras)this).RemoveAt(index);
				return;
			}
			throw new System.NotSupportedException();
		}

		/// <summary>
		/// カメラデバイス: 検索.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int IxAuxInfoCameras.Find(object src)
		{
			if (src is XIE.Media.CxDeviceParam)
			{
				return ArrayTool.Find(m_Body.CameraInfos, (XIE.Media.CxDeviceParam)src);
			}
			if (src is XIE.Media.CxCamera)
			{
				return ArrayTool.Find(m_Body.CameraControllers, (XIE.Media.CxCamera)src);
			}
			return -1;
		}

		#endregion

		#region IxAuxInfoGrabbers インターフェースの実装:

		/// <summary>
		/// イメージグラバー: プラグイン情報コレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		CxGrabberInfo[] IxAuxInfoGrabbers.Infos
		{
			get { return m_Body.GrabberInfos; }
		}

		/// <summary>
		/// イメージグラバー: コントローラコレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		CxGrabberThread[] IxAuxInfoGrabbers.Controllers
		{
			get { return m_Body.Grabbers; }
		}

		/// <summary>
		/// イメージグラバー: 追加.(ポートのオープンは行いません。)
		/// </summary>
		/// <param name="info">プラグイン情報</param>
		/// <param name="controller">コントローラ</param>
		void IxAuxInfoGrabbers.Add(CxGrabberInfo info, CxGrabberThread controller)
		{
			m_Body.GrabberInfos = ArrayTool.Add(m_Body.GrabberInfos, info).ToArray();
			m_Body.Grabbers = ArrayTool.Add(m_Body.Grabbers, controller).ToArray();
		}

		/// <summary>
		/// イメージグラバー: 全て削除.
		/// </summary>
		void IxAuxInfoGrabbers.RemoveAll()
		{
			for (int i = 0; i < m_Body.Grabbers.Length; i++)
			{
				if (m_Body.Grabbers[i] != null)
					m_Body.Grabbers[i].Dispose();
				m_Body.Grabbers[i] = null;
			}

			m_Body.GrabberInfos = new CxGrabberInfo[0];
			m_Body.Grabbers = new CxGrabberThread[0];
		}

		/// <summary>
		/// イメージグラバー: 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void IxAuxInfoGrabbers.RemoveAt(int index)
		{
			if (0 <= index && index < m_Body.Grabbers.Length)
			{
				if (m_Body.Grabbers[index] != null)
					m_Body.Grabbers[index].Dispose();
				m_Body.Grabbers[index] = null;
			}

			m_Body.GrabberInfos = ArrayTool.RemoveAt(m_Body.GrabberInfos, index).ToArray();
			m_Body.Grabbers = ArrayTool.RemoveAt(m_Body.Grabbers, index).ToArray();
		}

		/// <summary>
		/// イメージグラバー: 削除.
		/// </summary>
		/// <param name="src">プラグイン情報またはコントローラ</param>
		void IxAuxInfoGrabbers.Remove(object src)
		{
			if (src is CxGrabberInfo)
			{
				int index = ArrayTool.Find(m_Body.GrabberInfos, (CxGrabberInfo)src);
				if (0 <= index)
					((IxAuxInfoGrabbers)this).RemoveAt(index);
				return;
			}
			if (src is CxGrabberThread)
			{
				int index = ArrayTool.Find(m_Body.Grabbers, (CxGrabberThread)src);
				if (0 <= index)
					((IxAuxInfoGrabbers)this).RemoveAt(index);
				return;
			}
			throw new NotSupportedException();
		}

		/// <summary>
		/// イメージグラバー: 検索.
		/// </summary>
		/// <param name="src">プラグイン情報またはコントローラ</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int IxAuxInfoGrabbers.Find(object src)
		{
			if (src is CxGrabberInfo)
			{
				return ArrayTool.Find(m_Body.GrabberInfos, (CxGrabberInfo)src);
			}
			if (src is CxGrabberThread)
			{
				return ArrayTool.Find(m_Body.Grabbers, (CxGrabberThread)src);
			}
			throw new NotSupportedException();
		}

		#endregion

		#region IxAuxInfoDataPorts インターフェースの実装:

		/// <summary>
		/// データ入出力ポート: プラグイン情報コレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		CxDataPortInfo[] IxAuxInfoDataPorts.Infos
		{
			get { return m_Body.DataPortInfos; }
		}

		/// <summary>
		/// データ入出力ポート: コントローラコレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		CxDataPortThread[] IxAuxInfoDataPorts.Controllers
		{
			get { return m_Body.DataPorts; }
		}

		/// <summary>
		/// データ入出力ポート: 追加.(ポートのオープンは行いません。)
		/// </summary>
		/// <param name="info">プラグイン情報</param>
		/// <param name="controller">コントローラ</param>
		void IxAuxInfoDataPorts.Add(CxDataPortInfo info, CxDataPortThread controller)
		{
			m_Body.DataPortInfos = ArrayTool.Add(m_Body.DataPortInfos, info).ToArray();
			m_Body.DataPorts = ArrayTool.Add(m_Body.DataPorts, controller).ToArray();
		}

		/// <summary>
		/// データ入出力ポート: 全て削除.
		/// </summary>
		void IxAuxInfoDataPorts.RemoveAll()
		{
			for (int i = 0; i < m_Body.DataPorts.Length; i++)
			{
				if (m_Body.DataPorts[i] != null)
					m_Body.DataPorts[i].Dispose();
				m_Body.DataPorts[i] = null;
			}

			m_Body.DataPortInfos = new CxDataPortInfo[0];
			m_Body.DataPorts = new CxDataPortThread[0];
		}

		/// <summary>
		/// データ入出力ポート: 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void IxAuxInfoDataPorts.RemoveAt(int index)
		{
			if (0 <= index && index < m_Body.DataPorts.Length)
			{
				if (m_Body.DataPorts[index] != null)
					m_Body.DataPorts[index].Dispose();
				m_Body.DataPorts[index] = null;
			}

			m_Body.DataPortInfos = ArrayTool.RemoveAt(m_Body.DataPortInfos, index).ToArray();
			m_Body.DataPorts = ArrayTool.RemoveAt(m_Body.DataPorts, index).ToArray();
		}

		/// <summary>
		/// データ入出力ポート: 削除.
		/// </summary>
		/// <param name="src">プラグイン情報またはコントローラ</param>
		void IxAuxInfoDataPorts.Remove(object src)
		{
			if (src is CxDataPortInfo)
			{
				int index = ArrayTool.Find(m_Body.DataPortInfos, (CxDataPortInfo)src);
				if (0 <= index)
					((IxAuxInfoDataPorts)this).RemoveAt(index);
				return;
			}
			if (src is CxDataPortThread)
			{
				int index = ArrayTool.Find(m_Body.DataPorts, (CxDataPortThread)src);
				if (0 <= index)
					((IxAuxInfoDataPorts)this).RemoveAt(index);
				return;
			}
			throw new NotSupportedException();
		}

		/// <summary>
		/// データ入出力ポート: 検索.
		/// </summary>
		/// <param name="src">プラグイン情報またはコントローラ</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int IxAuxInfoDataPorts.Find(object src)
		{
			if (src is CxDataPortInfo)
			{
				return ArrayTool.Find(m_Body.DataPortInfos, (CxDataPortInfo)src);
			}
			if (src is CxDataPortThread)
			{
				return ArrayTool.Find(m_Body.DataPorts, (CxDataPortThread)src);
			}
			throw new NotSupportedException();
		}

		#endregion

		#region IxAuxInfoSerialPorts インターフェースの実装:

		/// <summary>
		/// シリアル通信: デバイス情報コレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		CxSerialPortInfo[] IxAuxInfoSerialPorts.Infos
		{
			get { return m_Body.SerialPortInfos; }
		}

		/// <summary>
		/// シリアル通信: コントローラコレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		XIE.IO.CxSerialPort[] IxAuxInfoSerialPorts.Controllers
		{
			get { return m_Body.SerialPorts; }
		}

		/// <summary>
		/// シリアル通信: 追加.(ポートのオープンは行いません。)
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		void IxAuxInfoSerialPorts.Add(CxSerialPortInfo info, XIE.IO.CxSerialPort controller)
		{
			m_Body.SerialPortInfos = ArrayTool.Add(m_Body.SerialPortInfos, info).ToArray();
			m_Body.SerialPorts = ArrayTool.Add(m_Body.SerialPorts, controller).ToArray();
		}

		/// <summary>
		/// シリアル通信: 全て削除.
		/// </summary>
		void IxAuxInfoSerialPorts.RemoveAll()
		{
			for (int i = 0; i < m_Body.SerialPorts.Length; i++)
			{
				if (m_Body.SerialPorts[i] != null)
					m_Body.SerialPorts[i].Dispose();
				m_Body.SerialPorts[i] = null;
			}

			m_Body.SerialPortInfos = new CxSerialPortInfo[0];
			m_Body.SerialPorts = new XIE.IO.CxSerialPort[0];
		}

		/// <summary>
		/// シリアル通信: 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void IxAuxInfoSerialPorts.RemoveAt(int index)
		{
			if (0 <= index && index < m_Body.SerialPorts.Length)
			{
				if (m_Body.SerialPorts[index] != null)
					m_Body.SerialPorts[index].Dispose();
				m_Body.SerialPorts[index] = null;
			}

			m_Body.SerialPortInfos = ArrayTool.RemoveAt(m_Body.SerialPortInfos, index).ToArray();
			m_Body.SerialPorts = ArrayTool.RemoveAt(m_Body.SerialPorts, index).ToArray();
		}

		/// <summary>
		/// シリアル通信: 削除.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		void IxAuxInfoSerialPorts.Remove(object src)
		{
			if (src is CxSerialPortInfo)
			{
				int index = ArrayTool.Find(m_Body.SerialPortInfos, (CxSerialPortInfo)src);
				if (0 <= index)
					((IxAuxInfoSerialPorts)this).RemoveAt(index);
				return;
			}
			if (src is XIE.IO.CxSerialPort)
			{
				int index = ArrayTool.Find(m_Body.SerialPorts, (XIE.IO.CxSerialPort)src);
				if (0 <= index)
					((IxAuxInfoSerialPorts)this).RemoveAt(index);
				return;
			}
			throw new NotSupportedException();
		}

		/// <summary>
		/// シリアル通信: 検索.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int IxAuxInfoSerialPorts.Find(object src)
		{
			if (src is CxSerialPortInfo)
			{
				return ArrayTool.Find(m_Body.SerialPortInfos, (CxSerialPortInfo)src);
			}
			if (src is XIE.IO.CxSerialPort)
			{
				return ArrayTool.Find(m_Body.SerialPorts, (XIE.IO.CxSerialPort)src);
			}
			throw new NotSupportedException();
		}

		#endregion

		#region IxAuxInfoTcpServers インターフェースの実装:

		/// <summary>
		/// TCP/IP Server: デバイス情報コレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		CxTcpServerInfo[] IxAuxInfoTcpServers.Infos
		{
			get { return m_Body.TcpServerInfos; }
		}

		/// <summary>
		/// TCP/IP Server: コントローラコレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		XIE.Net.CxTcpServer[] IxAuxInfoTcpServers.Controllers
		{
			get { return m_Body.TcpServers; }
		}

		/// <summary>
		/// TCP/IP Server: 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		void IxAuxInfoTcpServers.Add(CxTcpServerInfo info, XIE.Net.CxTcpServer controller)
		{
			m_Body.TcpServerInfos = ArrayTool.Add(m_Body.TcpServerInfos, info).ToArray();
			m_Body.TcpServers = ArrayTool.Add(m_Body.TcpServers, controller).ToArray();
		}

		/// <summary>
		/// TCP/IP Server: 全て削除.
		/// </summary>
		void IxAuxInfoTcpServers.RemoveAll()
		{
			for (int i = 0; i < m_Body.TcpServers.Length; i++)
			{
				if (m_Body.TcpServers[i] != null)
					m_Body.TcpServers[i].Dispose();
				m_Body.TcpServers[i] = null;
			}

			m_Body.TcpServerInfos = new CxTcpServerInfo[0];
			m_Body.TcpServers = new XIE.Net.CxTcpServer[0];
		}

		/// <summary>
		/// TCP/IP Server: 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void IxAuxInfoTcpServers.RemoveAt(int index)
		{
			if (0 <= index && index < m_Body.TcpServers.Length)
			{
				if (m_Body.TcpServers[index] != null)
					m_Body.TcpServers[index].Dispose();
				m_Body.TcpServers[index] = null;
			}

			m_Body.TcpServerInfos = ArrayTool.RemoveAt(m_Body.TcpServerInfos, index).ToArray();
			m_Body.TcpServers = ArrayTool.RemoveAt(m_Body.TcpServers, index).ToArray();
		}

		/// <summary>
		/// TCP/IP Server: 削除.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		void IxAuxInfoTcpServers.Remove(object src)
		{
			if (src is CxTcpServerInfo)
			{
				int index = ArrayTool.Find(m_Body.TcpServerInfos, (CxTcpServerInfo)src);
				if (0 <= index)
					((IxAuxInfoTcpServers)this).RemoveAt(index);
				return;
			}
			if (src is XIE.Net.CxTcpServer)
			{
				int index = ArrayTool.Find(m_Body.TcpServers, (XIE.Net.CxTcpServer)src);
				if (0 <= index)
					((IxAuxInfoTcpServers)this).RemoveAt(index);
				return;
			}
			throw new NotSupportedException();
		}

		/// <summary>
		/// TCP/IP Server: 検索.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int IxAuxInfoTcpServers.Find(object src)
		{
			if (src is CxTcpServerInfo)
			{
				return ArrayTool.Find(m_Body.TcpServerInfos, (CxTcpServerInfo)src);
			}
			if (src is XIE.Net.CxTcpServer)
			{
				return ArrayTool.Find(m_Body.TcpServers, (XIE.Net.CxTcpServer)src);
			}
			throw new NotSupportedException();
		}

		#endregion

		#region IxAuxInfoTcpClients インターフェースの実装:

		/// <summary>
		/// TCP/IP Client: デバイス情報コレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		CxTcpClientInfo[] IxAuxInfoTcpClients.Infos
		{
			get { return m_Body.TcpClientInfos; }
		}

		/// <summary>
		/// TCP/IP Client: コントローラコレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		XIE.Net.CxTcpClient[] IxAuxInfoTcpClients.Controllers
		{
			get { return m_Body.TcpClients; }
		}

		/// <summary>
		/// TCP/IP Client: 追加.
		/// </summary>
		/// <param name="info">デバイス情報</param>
		/// <param name="controller">コントローラ</param>
		void IxAuxInfoTcpClients.Add(CxTcpClientInfo info, XIE.Net.CxTcpClient controller)
		{
			m_Body.TcpClientInfos = ArrayTool.Add(m_Body.TcpClientInfos, info).ToArray();
			m_Body.TcpClients = ArrayTool.Add(m_Body.TcpClients, controller).ToArray();
		}

		/// <summary>
		/// TCP/IP Client: 全て削除.
		/// </summary>
		void IxAuxInfoTcpClients.RemoveAll()
		{
			for (int i = 0; i < m_Body.TcpClients.Length; i++)
			{
				if (m_Body.TcpClients[i] != null)
					m_Body.TcpClients[i].Dispose();
				m_Body.TcpClients[i] = null;
			}

			m_Body.TcpClientInfos = new CxTcpClientInfo[0];
			m_Body.TcpClients = new XIE.Net.CxTcpClient[0];
		}

		/// <summary>
		/// TCP/IP Client: 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void IxAuxInfoTcpClients.RemoveAt(int index)
		{
			if (0 <= index && index < m_Body.TcpClients.Length)
			{
				if (m_Body.TcpClients[index] != null)
					m_Body.TcpClients[index].Dispose();
				m_Body.TcpClients[index] = null;
			}

			m_Body.TcpClientInfos = ArrayTool.RemoveAt(m_Body.TcpClientInfos, index).ToArray();
			m_Body.TcpClients = ArrayTool.RemoveAt(m_Body.TcpClients, index).ToArray();
		}

		/// <summary>
		/// TCP/IP Client: 削除.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		void IxAuxInfoTcpClients.Remove(object src)
		{
			if (src is CxTcpClientInfo)
			{
				int index = ArrayTool.Find(m_Body.TcpClientInfos, (CxTcpClientInfo)src);
				if (0 <= index)
					((IxAuxInfoTcpClients)this).RemoveAt(index);
				return;
			}
			if (src is XIE.Net.CxTcpClient)
			{
				int index = ArrayTool.Find(m_Body.TcpClients, (XIE.Net.CxTcpClient)src);
				if (0 <= index)
					((IxAuxInfoTcpClients)this).RemoveAt(index);
				return;
			}
			throw new NotSupportedException();
		}

		/// <summary>
		/// TCP/IP Client: 検索.
		/// </summary>
		/// <param name="src">デバイス情報またはコントローラ</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int IxAuxInfoTcpClients.Find(object src)
		{
			if (src is CxTcpClientInfo)
			{
				return ArrayTool.Find(m_Body.TcpClientInfos, (CxTcpClientInfo)src);
			}
			if (src is XIE.Net.CxTcpClient)
			{
				return ArrayTool.Find(m_Body.TcpClients, (XIE.Net.CxTcpClient)src);
			}
			throw new NotSupportedException();
		}

		#endregion

		#region IxAuxInfoMedias インターフェースの実装:

		/// <summary>
		/// メディアプレイヤー: 動画ファイル情報コレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		CxMediaInfo[] IxAuxInfoMedias.Infos
		{
			get { return m_Body.MediaInfos; }
		}

		/// <summary>
		/// メディアプレイヤー: メディアプレイヤーコレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		XIE.Media.CxMediaPlayer[] IxAuxInfoMedias.Players
		{
			get { return m_Body.MediaPlayers; }
		}

		/// <summary>
		/// メディアプレイヤー: 追加.
		/// </summary>
		/// <param name="info">動画ファイル情報</param>
		/// <param name="player">メディアプレイヤー</param>
		void IxAuxInfoMedias.Add(CxMediaInfo info, XIE.Media.CxMediaPlayer player)
		{
			m_Body.MediaInfos = ArrayTool.Add(m_Body.MediaInfos, info).ToArray();
			m_Body.MediaPlayers = ArrayTool.Add(m_Body.MediaPlayers, player).ToArray();
		}

		/// <summary>
		/// メディアプレイヤー: 全て削除.
		/// </summary>
		void IxAuxInfoMedias.RemoveAll()
		{
			for (int i = 0; i < m_Body.MediaPlayers.Length; i++)
			{
				if (m_Body.MediaPlayers[i] != null)
					m_Body.MediaPlayers[i].Dispose();
				m_Body.MediaPlayers[i] = null;
			}

			m_Body.MediaInfos = new CxMediaInfo[0];
			m_Body.MediaPlayers = new XIE.Media.CxMediaPlayer[0];
		}

		/// <summary>
		/// メディアプレイヤー: 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void IxAuxInfoMedias.RemoveAt(int index)
		{
			if (0 <= index && index < m_Body.MediaPlayers.Length)
			{
				if (m_Body.MediaPlayers[index] != null)
					m_Body.MediaPlayers[index].Dispose();
				m_Body.MediaPlayers[index] = null;
			}

			m_Body.MediaInfos = ArrayTool.RemoveAt(m_Body.MediaInfos, index).ToArray();
			m_Body.MediaPlayers = ArrayTool.RemoveAt(m_Body.MediaPlayers, index).ToArray();
		}

		/// <summary>
		/// メディアプレイヤー: 削除.
		/// </summary>
		/// <param name="src">動画ファイル情報またはメディアプレイヤー</param>
		void IxAuxInfoMedias.Remove(object src)
		{
			if (src is CxMediaInfo)
			{
				int index = ArrayTool.Find(m_Body.MediaInfos, (CxMediaInfo)src);
				if (0 <= index)
					((IxAuxInfoMedias)this).RemoveAt(index);
				return;
			}
			if (src is XIE.Media.CxMediaPlayer)
			{
				int index = ArrayTool.Find(m_Body.MediaPlayers, (XIE.Media.CxMediaPlayer)src);
				if (0 <= index)
					((IxAuxInfoMedias)this).RemoveAt(index);
				return;
			}
			throw new NotSupportedException();
		}

		/// <summary>
		/// メディアプレイヤー: 検索.
		/// </summary>
		/// <param name="src">動画ファイル情報またはメディアプレイヤー</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int IxAuxInfoMedias.Find(object src)
		{
			if (src is CxMediaInfo)
			{
				return ArrayTool.Find(m_Body.MediaInfos, (CxMediaInfo)src);
			}
			if (src is XIE.Media.CxMediaPlayer)
			{
				return ArrayTool.Find(m_Body.MediaPlayers, (XIE.Media.CxMediaPlayer)src);
			}
			throw new NotSupportedException();
		}

		#endregion

		#region IxAuxInfoImages インターフェースの実装:

		/// <summary>
		/// 画像オブジェクト: データ情報コレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		CxImageInfo[] IxAuxInfoImages.Infos
		{
			get { return m_Body.ImageInfos; }
		}

		/// <summary>
		/// 画像オブジェクト: データオブジェクトコレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		XIE.CxImage[] IxAuxInfoImages.Datas
		{
			get { return m_Body.Images; }
		}

		/// <summary>
		/// 画像オブジェクト: 追加.
		/// </summary>
		/// <param name="info">データ情報</param>
		/// <param name="data">データオブジェクト</param>
		void IxAuxInfoImages.Add(CxImageInfo info, XIE.CxImage data)
		{
			m_Body.ImageInfos = ArrayTool.Add(m_Body.ImageInfos, info).ToArray();
			m_Body.Images = ArrayTool.Add(m_Body.Images, data).ToArray();
		}

		/// <summary>
		/// 画像オブジェクト: 全て削除.
		/// </summary>
		void IxAuxInfoImages.RemoveAll()
		{
			for (int i = 0; i < m_Body.Images.Length; i++)
			{
				if (m_Body.Images[i] != null)
					m_Body.Images[i].Dispose();
				m_Body.Images[i] = null;
			}

			m_Body.ImageInfos = new CxImageInfo[0];
			m_Body.Images = new XIE.CxImage[0];
		}

		/// <summary>
		/// 画像オブジェクト: 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void IxAuxInfoImages.RemoveAt(int index)
		{
			if (0 <= index && index < m_Body.Images.Length)
			{
				if (m_Body.Images[index] != null)
					m_Body.Images[index].Dispose();
				m_Body.Images[index] = null;
			}

			m_Body.ImageInfos = ArrayTool.RemoveAt(m_Body.ImageInfos, index).ToArray();
			m_Body.Images = ArrayTool.RemoveAt(m_Body.Images, index).ToArray();
		}

		/// <summary>
		/// 画像オブジェクト: 削除.
		/// </summary>
		/// <param name="src">データ情報またはデータオブジェクト</param>
		void IxAuxInfoImages.Remove(object src)
		{
			if (src is CxImageInfo)
			{
				int index = ArrayTool.Find(m_Body.ImageInfos, (CxImageInfo)src);
				if (0 <= index)
					((IxAuxInfoImages)this).RemoveAt(index);
				return;
			}
			if (src is XIE.CxImage)
			{
				int index = ArrayTool.Find(m_Body.Images, (XIE.CxImage)src);
				if (0 <= index)
					((IxAuxInfoImages)this).RemoveAt(index);
				return;
			}
			throw new NotSupportedException();
		}

		/// <summary>
		/// 画像オブジェクト: 検索.
		/// </summary>
		/// <param name="src">データ情報またはデータオブジェクト</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int IxAuxInfoImages.Find(object src)
		{
			if (src is CxImageInfo)
			{
				return ArrayTool.Find(m_Body.ImageInfos, (CxImageInfo)src);
			}
			if (src is XIE.CxImage)
			{
				return ArrayTool.Find(m_Body.Images, (XIE.CxImage)src);
			}
			throw new NotSupportedException();
		}

		#endregion

		#region IxAuxInfoTasks インターフェースの実装:

		/// <summary>
		/// タスクユニット: タスクユニット情報コレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		CxTaskUnitInfo[] IxAuxInfoTasks.Infos
		{
			get { return m_Body.TaskUnitInfos; }
		}

		/// <summary>
		/// タスクユニット: タスクユニットコレクション
		/// </summary>
		[Browsable(false)]
		[XmlIgnore]
		XIE.Tasks.CxTaskUnit[] IxAuxInfoTasks.Tasks
		{
			get { return m_Body.TaskUnits; }
		}

		/// <summary>
		/// タスクユニット: 追加.
		/// </summary>
		/// <param name="info">タスクユニット情報</param>
		/// <param name="task">タスクユニット</param>
		void IxAuxInfoTasks.Add(CxTaskUnitInfo info, XIE.Tasks.CxTaskUnit task)
		{
			m_Body.TaskUnitInfos = ArrayTool.Add(m_Body.TaskUnitInfos, info).ToArray();
			m_Body.TaskUnits = ArrayTool.Add(m_Body.TaskUnits, task).ToArray();
		}

		/// <summary>
		/// タスクユニット: 全て削除.
		/// </summary>
		void IxAuxInfoTasks.RemoveAll()
		{
			for (int i = 0; i < m_Body.TaskUnits.Length; i++)
			{
				if (m_Body.TaskUnits[i] != null)
					m_Body.TaskUnits[i].Dispose();
				m_Body.TaskUnits[i] = null;
			}

			m_Body.TaskUnitInfos = new CxTaskUnitInfo[0];
			m_Body.TaskUnits = new XIE.Tasks.CxTaskUnit[0];
		}

		/// <summary>
		/// タスクユニット: 削除.
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		void IxAuxInfoTasks.RemoveAt(int index)
		{
			if (0 <= index && index < m_Body.TaskUnits.Length)
			{
				if (m_Body.TaskUnits[index] != null)
					m_Body.TaskUnits[index].Dispose();
				m_Body.TaskUnits[index] = null;
			}

			m_Body.TaskUnitInfos = ArrayTool.RemoveAt(m_Body.TaskUnitInfos, index).ToArray();
			m_Body.TaskUnits = ArrayTool.RemoveAt(m_Body.TaskUnits, index).ToArray();
		}

		/// <summary>
		/// タスクユニット: 削除.
		/// </summary>
		/// <param name="src">タスクユニット情報またはタスクユニット</param>
		void IxAuxInfoTasks.Remove(object src)
		{
			if (src is CxTaskUnitInfo)
			{
				int index = ArrayTool.Find(m_Body.TaskUnitInfos, (CxTaskUnitInfo)src);
				if (0 <= index)
					((IxAuxInfoTasks)this).RemoveAt(index);
				return;
			}
			if (src is XIE.Tasks.CxTaskUnit)
			{
				int index = ArrayTool.Find(m_Body.TaskUnits, (XIE.Tasks.CxTaskUnit)src);
				if (0 <= index)
					((IxAuxInfoTasks)this).RemoveAt(index);
				return;
			}
			throw new NotSupportedException();
		}

		/// <summary>
		/// タスクユニット: 検索.
		/// </summary>
		/// <param name="src">タスクユニット情報またはタスクユニット</param>
		/// <returns>
		///		該当する要素の指標(0 以上)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		int IxAuxInfoTasks.Find(object src)
		{
			if (src is CxTaskUnitInfo)
			{
				return ArrayTool.Find(m_Body.TaskUnitInfos, (CxTaskUnitInfo)src);
			}
			if (src is XIE.Tasks.CxTaskUnit)
			{
				return ArrayTool.Find(m_Body.TaskUnits, (XIE.Tasks.CxTaskUnit)src);
			}
			throw new NotSupportedException();
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// 型変換.
		//

		#region SelfConverter

		/// <summary>
		/// 型変換クラス
		/// </summary>
		internal class SelfConverter : CxSortingConverter
		{
			/// <summary>
			/// コンバータがオブジェクトを指定した型に変換できるか否かを示します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="destinationType"></param>
			/// <returns>
			///		変換可能な場合は true を返します。
			/// </returns>
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return base.CanConvertTo(context, destinationType);
			}

			/// <summary>
			/// 指定されたオブジェクトを指定した型に変換します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture"></param>
			/// <param name="value"></param>
			/// <param name="destinationType"></param>
			/// <returns>
			///		インスタンスの内容を文字列に変換して返します。
			/// </returns>
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}

			/// <summary>
			/// コンバータが指定した型のオブジェクトから自身の型に変換できるか否かを示します。
			/// </summary>
			/// <param name="context"></param>
			/// <param name="sourceType"></param>
			/// <returns>
			///		変換可能な場合は true を返します。
			/// </returns>
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return base.CanConvertFrom(context, sourceType);
			}

			/// <summary>
			/// 指定された型のオブジェクトから自身の型への変換
			/// </summary>
			/// <param name="context"></param>
			/// <param name="culture"></param>
			/// <param name="value"></param>
			/// <returns>
			///		変換後のインスタンスを返します。
			/// </returns>
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}

	#region ArrayTool

	/// <summary>
	/// 配列操作
	/// </summary>
	static class ArrayTool
	{
		/// <summary>
		/// 配列要素の検索
		/// </summary>
		/// <typeparam name="TYPE">要素の型</typeparam>
		/// <param name="src">配列</param>
		/// <param name="item">要素</param>
		/// <returns>
		///		該当位置の配列指標(0~)を返します。
		///		見つからなければ -1 を返します。
		/// </returns>
		public static int Find<TYPE>(IEnumerable<TYPE> src, TYPE item)
		{
			int index = 0;
			foreach (var src_item in src)
			{
				if (ReferenceEquals(src_item, item)) return index;
				index++;
			}
			return -1;
		}

		/// <summary>
		/// 配列要素の追加
		/// </summary>
		/// <typeparam name="TYPE">要素の型</typeparam>
		/// <param name="src">配列</param>
		/// <param name="item">要素</param>
		/// <returns>
		///		すべての要素を配列からコレクションへ移し、最後尾に指定要素を追加して返します。
		/// </returns>
		public static List<TYPE> Add<TYPE>(IEnumerable<TYPE> src, TYPE item)
		{
			List<TYPE> results = new List<TYPE>(src);
			results.Add(item);
			return results;
		}

		/// <summary>
		/// 配列要素の削除
		/// </summary>
		/// <typeparam name="TYPE">要素の型</typeparam>
		/// <param name="src">配列</param>
		/// <param name="item">要素</param>
		/// <returns>
		///		該当する要素を除くその他の要素を配列からコレクションへ移して返します。
		/// </returns>
		public static List<TYPE> Remove<TYPE>(IEnumerable<TYPE> src, TYPE item)
		{
			List<TYPE> results = new List<TYPE>();
			foreach (TYPE src_item in src)
			{
				if (ReferenceEquals(src_item, item) == false)
					results.Add(src_item);
			}
			return results;
		}

		/// <summary>
		/// 配列要素の削除
		/// </summary>
		/// <typeparam name="TYPE">要素の型</typeparam>
		/// <param name="src">配列</param>
		/// <param name="index">要素の指標</param>
		/// <returns>
		///		該当する要素を除くその他の要素を配列からコレクションへ移して返します。
		/// </returns>
		public static List<TYPE> RemoveAt<TYPE>(IEnumerable<TYPE> src, int index)
		{
			List<TYPE> results = new List<TYPE>();
			int src_index = 0;
			foreach (var src_item in src)
			{
				if (src_index != index)
					results.Add(src_item);
				src_index++;
			}
			return results;
		}
	}

	#endregion
}
