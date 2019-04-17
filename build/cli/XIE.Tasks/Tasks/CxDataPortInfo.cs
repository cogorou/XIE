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
	/// データ入出力ポート情報
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public partial class CxDataPortInfo : System.Object
		, ICloneable
		, IxEquatable
		, IxFileAccess
	{
		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxDataPortInfo()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="assemblyName">アセンブリ名 (アセンブリファイルまたは厳密名を指定してください。)</param>
		/// <param name="className">クラス名 (指定したアセンブリ内のクラスの名称を指定してください。)</param>
		/// <param name="configFile">構成ファイル</param>
		public CxDataPortInfo(string assemblyName, string className, string configFile)
		{
			AssemblyName = assemblyName;
			ClassName = className;
			ConfigFile = configFile;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// プロパティ:
		//

		#region プロパティ: (Parameters)

		/// <summary>
		/// アセンブリ名 (アセンブリファイルまたは厳密名を指定してください。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxDataPortInfo.AssemblyName")]
		public virtual string AssemblyName
		{
			get { return m_AssemblyName; }
			set { m_AssemblyName = value; }
		}
		private string m_AssemblyName = "";

		/// <summary>
		/// クラス名 (指定したアセンブリ内のクラスの名称を指定してください。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxDataPortInfo.ClassName")]
		public virtual string ClassName
		{
			get { return m_ClassName; }
			set { m_ClassName = value; }
		}
		private string m_ClassName = "";

		/// <summary>
		/// 構成ファイル
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxDataPortInfo.ConfigFile")]
		public virtual string ConfigFile
		{
			get { return m_ConfigFile; }
			set { m_ConfigFile = value; }
		}
		private string m_ConfigFile = "";

		#endregion

		// ////////////////////////////////////////////////////////////
		// 関数.
		//

		#region インスタンス生成:

		/// <summary>
		/// 現在のアセンブリ名とクラス名からインスタンスを生成します。
		/// </summary>
		/// <returns>
		///		生成されたインスタンスを返します。
		/// </returns>
		public object CreateInstance()
		{
			var assemblyName = this.AssemblyName;
			var className = this.ClassName;
			var configFile = this.ConfigFile;

			if (string.IsNullOrWhiteSpace(assemblyName))
			{
				#region 現在のアセンブリから生成する.
				Assembly asm = Assembly.GetCallingAssembly();
				Type type = asm.GetType(this.ClassName);
				return Activator.CreateInstance(type, new object[] { configFile });
				#endregion
			}
			else
			{
				#region 指定されたアセンブリから生成する.
				Assembly asm = null;
				if (System.IO.File.Exists(assemblyName))
					asm = Assembly.LoadFile(assemblyName);
				else
					asm = Assembly.Load(assemblyName);
				Type type = asm.GetType(className);
				return Activator.CreateInstance(type, new object[] { configFile });
				#endregion
			}
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

			var _src = (CxDataPortInfo)src;

			this.AssemblyName = _src.AssemblyName;
			this.ClassName = _src.ClassName;
			this.ConfigFile = _src.ConfigFile;

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
				var _src = (CxDataPortInfo)src;

				if (this.AssemblyName != _src.AssemblyName) return false;
				if (this.ClassName != _src.ClassName) return false;
				if (this.ConfigFile != _src.ConfigFile) return false;

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
		public static CxDataPortInfo LoadFrom(string filename, params object[] options)
		{
			return (CxDataPortInfo)XIE.Axi.ReadAsXml(filename, typeof(CxDataPortInfo));
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
				if (destinationType == typeof(CxDataPortInfo))
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
				if (destinationType == typeof(string) && value is CxDataPortInfo)
				{
					var _value = (CxDataPortInfo)value;
					var name = (string.IsNullOrEmpty(_value.ClassName)) ? "" : _value.ClassName.Trim();
					return string.Format("{0}", name);
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
}