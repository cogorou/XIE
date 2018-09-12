/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using XIE;

namespace XIEprompt
{
	/// <summary>
	/// アプリケーション設定
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public class CxAppSettings : System.Object
		, ICloneable
		, IxEquatable
		, IxFileAccess
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxAppSettings()
		{
			this.References = ApiHelper.CreateReferences();
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// SetDllDirectory に指定するパス (x64 環境)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIEprompt.CxAppSettings.DllDirectoryX64")]
		public virtual string DllDirectoryX64
		{
			get { return m_DllDirectoryX64; }
			set { m_DllDirectoryX64 = value; }
		}
		private string m_DllDirectoryX64 = "";

		/// <summary>
		/// SetDllDirectory に指定するパス (x86 環境)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIEprompt.CxAppSettings.DllDirectoryX86")]
		public virtual string DllDirectoryX86
		{
			get { return m_DllDirectoryX86; }
			set { m_DllDirectoryX86 = value; }
		}
		private string m_DllDirectoryX86 = "";

		/// <summary>
		/// 参照設定
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIEprompt.CxAppSettings.References")]
		[XmlArrayItem(typeof(XIE.Tasks.CxReferencedAssembly))]
		public virtual XIE.Tasks.CxReferencedAssembly[] References
		{
			get { return m_References; }
			set { m_References = value; }
		}
		private XIE.Tasks.CxReferencedAssembly[] m_References = new XIE.Tasks.CxReferencedAssembly[0];

		#endregion

		#region プロパティ: (Runtime)

		/// <summary>
		/// スクリプトファイルディレクトリ
		/// </summary>
		[CxCategory("Runtime")]
		[CxDescription("P:XIEprompt.CxAppSettings.ScriptFileDirectory")]
		public virtual string ScriptFileDirectory
		{
			get { return m_ScriptFileDirectory; }
			set { m_ScriptFileDirectory = value; }
		}
		private string m_ScriptFileDirectory = "";

		#endregion

		#region 初期化:

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Setup()
		{
			#region 文字列の正規化:
			{
				DllDirectoryX64 = ValidateDirectory(DllDirectoryX64);
				DllDirectoryX86 = ValidateDirectory(DllDirectoryX86);
			}
			#endregion

			#region SetDllDirectory
			{
#if LINUX
#else
				string dir = "";
				if (Environment.Is64BitProcess)
					dir = this.DllDirectoryX64;
				else
					dir = this.DllDirectoryX86;
				if (dir != "" && System.IO.Directory.Exists(dir))
				{
					SetDllDirectory(dir);
				}
#endif
			}
			#endregion
		}

		/// <summary>
		/// ディレクトリ名を正規化します。
		/// </summary>
		/// <param name="dir"></param>
		/// <returns>
		///		存在する場合はフルパスに変換します。
		///		それ以外は空白に変換します。
		/// </returns>
		private string ValidateDirectory(string dir)
		{
			if (string.IsNullOrWhiteSpace(dir))
				return "";
			if (System.IO.Directory.Exists(dir))
				return System.IO.Path.GetFullPath(dir);
			return "";
		}

#if LINUX
#else
		/// <summary>
		/// プロービングするパスを追加します。
		/// </summary>
		/// <param name="path">パス</param>
		/// <returns>
		///		正常の場合は true 、異常の場合は false を返します。
		/// </returns>
		/// <remarks>
		/// https://msdn.microsoft.com/ja-jp/library/windows/desktop/ms686203%28v=vs.85%29.aspx
		/// </remarks>
		[DllImport("kernel32")]
		static extern bool SetDllDirectory(string path);
#endif

		#endregion

		// ////////////////////////////////////////////////////////////
		// インターフェースの実装.
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
			var clone = new CxAppSettings();
			clone.CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの複製 (XIE.IxEquatable)
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(this, src)) return;

			CxAppSettings _src = (CxAppSettings)src;

			this.DllDirectoryX64 = _src.DllDirectoryX64;
			this.DllDirectoryX86 = _src.DllDirectoryX86;

			this.References = new XIE.Tasks.CxReferencedAssembly[_src.References.Length];
			for (int i = 0; i < this.References.Length; i++)
				this.References[i] = (XIE.Tasks.CxReferencedAssembly)_src.References[i].Clone();

			this.ScriptFileDirectory = _src.ScriptFileDirectory;

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
				CxAppSettings _src = (CxAppSettings)src;

				if (this.DllDirectoryX64 != _src.DllDirectoryX64) return false;
				if (this.DllDirectoryX86 != _src.DllDirectoryX86) return false;

				if (this.References == null && _src.References != null) return false;
				if (this.References != null && _src.References == null) return false;
				if (this.References != null && _src.References != null)
				{
					if (this.References.Length != _src.References.Length) return false;
					for (int i = 0; i < this.References.Length; i++)
					{
						if (this.References[i] == null && _src.References[i] != null) return false;
						if (this.References[i] != null && _src.References[i] == null) return false;
						if (this.References[i] != null && _src.References[i] != null)
						{
							if (this.References[i].ContentEquals(_src.References[i]) == false) return false;
						}
					}
				}

				if (this.ScriptFileDirectory != _src.ScriptFileDirectory) return false;

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
		public static CxAppSettings LoadFrom(string filename, params object[] options)
		{
			return (CxAppSettings)XIE.Axi.ReadAsXml(filename, typeof(CxAppSettings));
		}

		/// <summary>
		/// ファイル読み込み
		/// </summary>
		/// <param name="filename">XML ファイル名。</param>
		/// <param name="options">オプション。</param>
		public virtual void Load(string filename, params object[] options)
		{
			this.CopyFrom(XIE.Axi.ReadAsXml(filename, typeof(CxAppSettings)));
		}

		/// <summary>
		/// ファイル保存
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
		internal class SelfConverter : ExpandableObjectConverter
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
			///		オブジェクトの内容を文字列に変換して返します。
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
