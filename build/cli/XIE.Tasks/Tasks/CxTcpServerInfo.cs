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

namespace XIE.Tasks
{
	/// <summary>
	/// TcpServer 情報
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public partial class CxTcpServerInfo : System.Object
		, ICloneable
		, IxEquatable
		, IxFileAccess
	{
		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxTcpServerInfo()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="hostname">ホスト名</param>
		/// <param name="port">ポート</param>
		public CxTcpServerInfo(string hostname, int port)
		{
			Hostname = hostname;
			Port = port;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="resolve">IPアドレスを自動取得するか否か</param>
		/// <param name="index">自動取得したIPアドレスのコレクションの指標 [0~]</param>
		public CxTcpServerInfo(bool resolve, int index)
		{
			IPResolve = resolve;
			IPIndex = index;
		}

		#endregion

		#region プロパティ: (Parameter)

		/// <summary>
		/// ホスト名 (127.0.0.1 のようなアドレス)
		/// </summary>
		[Browsable(true)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxTcpServerInfo.Hostname")]
		public virtual string Hostname
		{
			get { return m_Hostname; }
			set { m_Hostname = value; }
		}
		private string m_Hostname = "127.0.0.1";

		/// <summary>
		/// ポート番号 [0-1023:Well known, 1024-49151:Registered, 49152-65535:Dynamic]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxTcpServerInfo.Port")]
		public virtual int Port
		{
			get { return m_Port; }
			set { m_Port = value; }
		}
		private int m_Port = 5000;

		/// <summary>
		/// 最大接続数 [初期値:5] [範囲:1~]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxTcpServerInfo.Backlog")]
		public virtual int Backlog
		{
			get { return m_Backlog; }
			set { m_Backlog = value; }
		}
		private int m_Backlog = 5;

		#endregion

		#region プロパティ: (Parameter2)

		/// <summary>
		/// IPアドレスを自動取得するか否か
		/// </summary>
		[Browsable(true)]
		[CxCategory("Parameter2")]
		[CxDescription("P:XIE.Tasks.CxTcpServerInfo.IPResolve")]
		public virtual bool IPResolve
		{
			get { return m_IPAutoResolve; }
			set { m_IPAutoResolve = value; }
		}
		private bool m_IPAutoResolve = false;

		/// <summary>
		/// 自動取得したIPアドレスのコレクションの指標 [0~]
		/// </summary>
		[Browsable(true)]
		[CxCategory("Parameter2")]
		[CxDescription("P:XIE.Tasks.CxTcpServerInfo.IPIndex")]
		public virtual int IPIndex
		{
			get { return m_IPIndex; }
			set { m_IPIndex = value; }
		}
		private int m_IPIndex = 0;

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

			var _src = (CxTcpServerInfo)src;
			this.Hostname = _src.Hostname;
			this.Port = _src.Port;
			this.IPResolve = _src.IPResolve;
			this.IPIndex = _src.IPIndex;
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
				var _src = (CxTcpServerInfo)src;
				if (this.Hostname != _src.Hostname) return false;
				if (this.Port != _src.Port) return false;
				if (this.IPResolve != _src.IPResolve) return false;
				if (this.IPIndex != _src.IPIndex) return false;
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
		public static CxTcpServerInfo LoadFrom(string filename, params object[] options)
		{
			return (CxTcpServerInfo)XIE.Axi.ReadAsXml(filename, typeof(CxTcpServerInfo));
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
		public void CopyTo(XIE.Net.CxTcpServer dst)
		{
			System.Net.IPAddress ip_addr = null;
			if (this.IPResolve)
			{
				#region 自動取得.
				string hostname = System.Net.Dns.GetHostName();
				int index = this.IPIndex;
				System.Net.IPAddress[] ip_addrs = System.Net.Dns.GetHostAddresses(hostname);
				if (index < ip_addrs.Length)
					ip_addr = ip_addrs[index];
				#endregion
			}
			else
			{
				#region 固定値.
				ip_addr = System.Net.IPAddress.Parse(this.Hostname);
				#endregion
			}

			dst.IPAddress = ip_addr;
			dst.Port = this.Port;
			dst.Backlog = this.Backlog;
		}

		/// <summary>
		/// コントローラの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public XIE.Net.CxTcpServer CreateController()
		{
			XIE.Net.CxTcpServer result = new XIE.Net.CxTcpServer();
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
				if (destinationType == typeof(CxTcpServerInfo))
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
				if (destinationType == typeof(string) && value is CxTcpServerInfo)
				{
					CxTcpServerInfo _value = (CxTcpServerInfo)value;
					return string.Format("{0},{1},{2},{3}",
						_value.Hostname,
						_value.Port,
						_value.IPResolve,
						_value.IPIndex
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
				if (sourceType == typeof(string))
					return true;
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
				if (value is string)
				{
					var values = value.ToString().Split(new char[] { ',' });
					CxTcpServerInfo _dst = new CxTcpServerInfo();
					if (values.Length > 0)
						_dst.Hostname = values[0].Trim();
					if (values.Length > 1)
						_dst.Port = int.Parse(values[1]);
					if (values.Length > 2)
						_dst.IPResolve = bool.Parse(values[2]);
					if (values.Length > 3)
						_dst.IPIndex = int.Parse(values[3]);
					return _dst;
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}
}
