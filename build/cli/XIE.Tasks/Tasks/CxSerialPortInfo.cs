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
using System.Xml.Serialization;
using System.IO.Ports;

namespace XIE.Tasks
{
	/// <summary>
	/// シリアル通信ポート情報
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public partial class CxSerialPortInfo : System.Object
		, ICloneable
		, IxEquatable
		, IxFileAccess
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxSerialPortInfo()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="portName">使用するポート (COM1 など)。</param>
		/// <param name="baudRate">ボーレート(bps) [初期値:9600、範囲:0 より大きい値]</param>
		/// <param name="parity">パリティ。[初期値:None]</param>
		/// <param name="dataBits">データビット(bit) [初期値:8、範囲:5~8]</param>
		/// <param name="stopBits">ストップビット。[初期値:One]</param>
		/// <param name="handshake">ハンドシェイク [初期値:None]</param>
		public CxSerialPortInfo(string portName, int baudRate, XIE.IO.ExParity parity, int dataBits, XIE.IO.ExStopBits stopBits, XIE.IO.ExHandshake handshake)
		{
			this.PortName = portName;
			this.BaudRate = baudRate;
			this.Parity = parity;
			this.DataBits = dataBits;
			this.StopBits = stopBits;
			this.Handshake = handshake;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="portName">使用するポート (COM1 など)。</param>
		/// <param name="param">パラメータ</param>
		public CxSerialPortInfo(string portName, XIE.IO.TxSerialPort param)
		{
			this.PortName = portName;
			this.BaudRate = param.BaudRate;
			this.Parity = param.Parity;
			this.DataBits = param.DataBits;
			this.StopBits = param.StopBits;
			this.Handshake = param.Handshake;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// プロパティ:
		//

		#region プロパティ: 1

		/// <summary>
		/// 使用するポート (COM1 など)。
		/// </summary>
		[CxCategory("Parameter.1")]
		[CxDescription("P:XIE.Tasks.CxSerialPortInfo.PortName")]
		[TypeConverter(typeof(SerialPortNameConverter))]
		public virtual string PortName
		{
			get { return m_PortName; }
			set { m_PortName = value; }
		}
#if LINUX
		private string m_PortName = "/dev/ttyS0";
#else
		private string m_PortName = "COM1";
#endif

		#endregion

		#region プロパティ: 2

		/// <summary>
		/// ボーレート(bps) [初期値:9600、範囲:0 より大きい値]
		/// </summary>
		[CxCategory("Parameter.2")]
		[CxDescription("P:XIE.Tasks.CxSerialPortInfo.BaudRate")]
		[TypeConverter(typeof(SerialBaudRateConverter))]
		public virtual int BaudRate
		{
			get { return m_BaudRate; }
			set { m_BaudRate = value; }
		}
		private int m_BaudRate = 9600;

		/// <summary>
		/// パリティ。[初期値:None]
		/// </summary>
		[CxCategory("Parameter.2")]
		[CxDescription("P:XIE.Tasks.CxSerialPortInfo.Parity")]
		public virtual XIE.IO.ExParity Parity
		{
			get { return m_Parity; }
			set { m_Parity = value; }
		}
		private XIE.IO.ExParity m_Parity = XIE.IO.ExParity.None;

		/// <summary>
		/// データビット(bit) [初期値:8、範囲:5~8]
		/// </summary>
		[CxCategory("Parameter.2")]
		[CxDescription("P:XIE.Tasks.CxSerialPortInfo.DataBits")]
		[TypeConverter(typeof(SerialDataBitsConverter))]
		public virtual int DataBits
		{
			get { return m_DataBits; }
			set { m_DataBits = value; }
		}
		private int m_DataBits = 8;

		/// <summary>
		/// ストップビット。[初期値:One]
		/// </summary>
		[CxCategory("Parameter.2")]
		[CxDescription("P:XIE.Tasks.CxSerialPortInfo.StopBits")]
		public virtual XIE.IO.ExStopBits StopBits
		{
			get { return m_StopBits; }
			set { m_StopBits = value; }
		}
		private XIE.IO.ExStopBits m_StopBits = XIE.IO.ExStopBits.One;

		/// <summary>
		/// データのシリアルポート伝送のハンドシェイクプロトコル [初期値:None]
		/// </summary>
		[CxCategory("Parameter.2")]
		[CxDescription("P:XIE.Tasks.CxSerialPortInfo.Handshake")]
		public virtual XIE.IO.ExHandshake Handshake
		{
			get { return m_Handshake; }
			set { m_Handshake = value; }
		}
		private XIE.IO.ExHandshake m_Handshake = XIE.IO.ExHandshake.None;

		#endregion

		#region プロパティ: 3

		/// <summary>
		/// 行末に付加する改行コード [初期値:LF]
		/// </summary>
		[CxCategory("Parameter.3")]
		[CxDescription("P:XIE.Tasks.CxSerialPortInfo.NewLine")]
		public virtual XIE.IO.ExNewLine NewLine
		{
			get { return m_NewLine; }
			set { m_NewLine = value; }
		}
		private XIE.IO.ExNewLine m_NewLine = XIE.IO.ExNewLine.LF;

		#endregion

		// ////////////////////////////////////////////////////////////
		// 関数.
		//

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

			if (src is CxSerialPortInfo)
			{
				#region 同一型.
				var _src = (CxSerialPortInfo)src;
				var _dst = this;
				_dst.PortName = _src.PortName;
				_dst.BaudRate = _src.BaudRate;
				_dst.Parity = _src.Parity;
				_dst.DataBits = _src.DataBits;
				_dst.StopBits = _src.StopBits;
				_dst.Handshake = _src.Handshake;
				_dst.NewLine = _src.NewLine;
				#endregion
			}
			else if (src is XIE.IO.CxSerialPort)
			{
				#region XIE.IO.CxSerialPort
				var _src = (XIE.IO.CxSerialPort)src;
				var _dst = this;
				_dst.PortName = _src.PortName;
				_dst.BaudRate = _src.Param.BaudRate;
				_dst.Parity = _src.Param.Parity;
				_dst.DataBits = _src.Param.DataBits;
				_dst.StopBits = _src.Param.StopBits;
				_dst.Handshake = _src.Param.Handshake;
				#endregion
			}

			return;
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
				var _src = (CxSerialPortInfo)src;

				if (this.PortName != _src.PortName) return false;
				if (this.BaudRate != _src.BaudRate) return false;
				if (this.Parity != _src.Parity) return false;
				if (this.DataBits != _src.DataBits) return false;
				if (this.StopBits != _src.StopBits) return false;
				if (this.Handshake != _src.Handshake) return false;
				if (this.NewLine != _src.NewLine) return false;

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
		public static CxSerialPortInfo LoadFrom(string filename, params object[] options)
		{
			return (CxSerialPortInfo)XIE.Axi.ReadAsXml(filename, typeof(CxSerialPortInfo));
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
		/// <param name="filename">XML ファイル名。</param>
		/// <param name="options">オプション。</param>
		public virtual void Save(string filename, params object[] options)
		{
			XIE.Axi.WriteAsXml(filename, this);
		}

		#endregion

		#region 変換:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="dst">複製先</param>
		public void CopyTo(XIE.IO.CxSerialPort dst)
		{
			dst.PortName = this.PortName;
			dst.Param = new XIE.IO.TxSerialPort(this.BaudRate, this.Parity, this.DataBits, this.StopBits, this.Handshake);
		}

		/// <summary>
		/// コントローラの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public XIE.IO.CxSerialPort CreateController()
		{
			XIE.IO.CxSerialPort result = new XIE.IO.CxSerialPort();
			this.CopyTo(result);
			return result;
		}

		#endregion

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
				if (destinationType == typeof(CxSerialPortInfo))
					return true;
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
			///		オブジェクトの内容を文字列に変換して返します。
			/// </returns>
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is CxSerialPortInfo)
				{
					CxSerialPortInfo _value = (CxSerialPortInfo)value;
					return string.Format("{0},{1},{2},{3},{4}",
						_value.PortName,
						_value.BaudRate,
						_value.Parity,
						_value.DataBits,
						_value.StopBits
						);
				}
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
			///		変換後のオブジェクトを返します。
			/// </returns>
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}

	#region SerialPortNameConverter

	/// <summary>
	/// シリアル通信ポート名称選択 (プロパティグリッド用)
	/// </summary>
	/// <remarks>
	///		see: http://msdn.microsoft.com/ja-jp/library/aa302326.aspx
	/// </remarks>
	class SerialPortNameConverter : StringConverter
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SerialPortNameConverter()
		{
		}

		/// <summary>
		/// true を返すと、このオブジェクトで一覧から選択可能な標準セットの値をサポートすることを示します。
		/// </summary>
		/// <param name="context"></param>
		/// <returns>
		///		true を返します。
		/// </returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// ドロップダウン リストにない値をユーザーが入力できるようにするには、false を返します。
		/// これにより、ドロップダウン リスト形式からコンボ ボックス形式に変わります。
		/// </summary>
		/// <param name="context"></param>
		/// <returns>
		///		false を返します。
		/// </returns>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		/// <summary>
		/// 標準値が格納された StandardValuesCollection を返します。
		/// StandardValuesCollection を作成する 1 つの方法として、コンストラクタで値の配列を指定します。
		/// オプション ウィンドウ アプリケーションでは、提示する既定のファイル名が格納された文字列配列を使用できます。
		/// </summary>
		/// <param name="context"></param>
		/// <returns>
		///		シリアル通信ポート名称のコレクションを返します。
		///		無ければ null を返します。
		/// </returns>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			try
			{
				return new StandardValuesCollection(SerialPort.GetPortNames());
			}
			catch (System.Exception)
			{
				return null;
			}
		}
	}

	#endregion

	#region SerialBaudRateConverter

	/// <summary>
	/// ボーレート選択 (プロパティグリッド用)
	/// </summary>
	class SerialBaudRateConverter : Int32Converter
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SerialBaudRateConverter()
		{
		}

		/// <summary>
		/// true を返すと、このオブジェクトで一覧から選択可能な標準セットの値をサポートすることを示します。
		/// </summary>
		/// <param name="context"></param>
		/// <returns>
		///		true を返します。
		/// </returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// ドロップダウン リストにない値をユーザーが入力できるようにするには、false を返します。
		/// これにより、ドロップダウン リスト形式からコンボ ボックス形式に変わります。
		/// </summary>
		/// <param name="context"></param>
		/// <returns>
		///		false を返します。
		/// </returns>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		/// <summary>
		/// 標準値が格納された StandardValuesCollection を返します。
		/// StandardValuesCollection を作成する 1 つの方法として、コンストラクタで値の配列を指定します。
		/// オプション ウィンドウ アプリケーションでは、提示する既定のファイル名が格納された文字列配列を使用できます。
		/// </summary>
		/// <param name="context"></param>
		/// <returns>
		///		ボーレートのコレクションを返します。
		///		無ければ null を返します。
		/// </returns>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			try
			{
				return new StandardValuesCollection(
					new int[]
					{
						9600,
						19200,
						38400,
						57600,
						115200,
					}
					);
			}
			catch (System.Exception)
			{
				return null;
			}
		}
	}

	#endregion

	#region SerialDataBitsConverter

	/// <summary>
	/// データビット選択 (プロパティグリッド用)
	/// </summary>
	class SerialDataBitsConverter : Int32Converter
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SerialDataBitsConverter()
		{
		}

		/// <summary>
		/// true を返すと、このオブジェクトで一覧から選択可能な標準セットの値をサポートすることを示します。
		/// </summary>
		/// <param name="context"></param>
		/// <returns>
		///		true を返します。
		/// </returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// ドロップダウン リストにない値をユーザーが入力できるようにするには、false を返します。
		/// これにより、ドロップダウン リスト形式からコンボ ボックス形式に変わります。
		/// </summary>
		/// <param name="context"></param>
		/// <returns>
		///		false を返します。
		/// </returns>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		/// <summary>
		/// 標準値が格納された StandardValuesCollection を返します。
		/// StandardValuesCollection を作成する 1 つの方法として、コンストラクタで値の配列を指定します。
		/// オプション ウィンドウ アプリケーションでは、提示する既定のファイル名が格納された文字列配列を使用できます。
		/// </summary>
		/// <param name="context"></param>
		/// <returns>
		///		ボーレートのコレクションを返します。
		///		無ければ null を返します。
		/// </returns>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			try
			{
				return new StandardValuesCollection(new int[] { 5, 6, 7, 8 });
			}
			catch (System.Exception)
			{
				return null;
			}
		}
	}

	#endregion
}