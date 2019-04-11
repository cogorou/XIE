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

namespace XIEstudio
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
		/// ウィンドウの表示方法
		/// </summary>
		[XIE.CxCategory("Basic")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.WindowState")]
		public virtual FormWindowState WindowState
		{
			get { return m_WindowState; }
			set { m_WindowState = value; }
		}
		private FormWindowState m_WindowState = FormWindowState.Normal;

		/// <summary>
		/// 画像ファイル読み込み用ディレクトリ
		/// </summary>
		[XIE.CxCategory("Basic")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.ImageFileDirectory")]
		public virtual string ImageFileDirectory
		{
			get { return m_ImageFileDirectory; }
			set { m_ImageFileDirectory = value; }
		}
		private string m_ImageFileDirectory = "";

		/// <summary>
		/// タスクフローファイルディレクトリ
		/// </summary>
		[XIE.CxCategory("Basic")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.TaskflowFileDirectory")]
		public virtual string TaskflowFileDirectory
		{
			get { return m_TaskflowFileDirectory; }
			set { m_TaskflowFileDirectory = value; }
		}
		private string m_TaskflowFileDirectory = "";

		/// <summary>
		/// データファイル保存用ディレクトリ
		/// </summary>
		[XIE.CxCategory("Basic")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.SaveDataDirectory")]
		public virtual string SaveDataDirectory
		{
			get { return m_SaveDataDirectory; }
			set { m_SaveDataDirectory = value; }
		}
		private string m_SaveDataDirectory = "";

		#endregion

		#region プロパティ: (Tasks)

		/// <summary>
		/// SetDllDirectory に指定するパス (x64 環境)
		/// </summary>
		[XIE.CxCategory("Tasks")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.DllDirectoryX64")]
		public virtual string DllDirectoryX64
		{
			get { return m_DllDirectoryX64; }
			set { m_DllDirectoryX64 = value; }
		}
		private string m_DllDirectoryX64 = "";

		/// <summary>
		/// SetDllDirectory に指定するパス (x86 環境)
		/// </summary>
		[XIE.CxCategory("Tasks")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.DllDirectoryX86")]
		public virtual string DllDirectoryX86
		{
			get { return m_DllDirectoryX86; }
			set { m_DllDirectoryX86 = value; }
		}
		private string m_DllDirectoryX86 = "";

		/// <summary>
		/// 参照設定
		/// </summary>
		[XIE.CxCategory("Tasks")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.References")]
		[XmlArrayItem(typeof(XIE.Tasks.CxReferencedAssembly))]
		public virtual XIE.Tasks.CxReferencedAssembly[] References
		{
			get { return m_References; }
			set { m_References = value; }
		}
		private XIE.Tasks.CxReferencedAssembly[] m_References = new XIE.Tasks.CxReferencedAssembly[0];

		/// <summary>
		/// デバッグモード
		/// </summary>
		[XIE.CxCategory("Tasks")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.DebugMode")]
		public virtual bool DebugMode
		{
			get { return m_DebugMode; }
			set { m_DebugMode = value; }
		}
		private bool m_DebugMode = false;

		/// <summary>
		/// エントリポイントの生成
		/// </summary>
		[XIE.CxCategory("Tasks")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.GenerateEntryPoint")]
		public virtual bool GenerateEntryPoint
		{
			get { return m_GenerateEntryPoint; }
			set { m_GenerateEntryPoint = value; }
		}
		private bool m_GenerateEntryPoint = true;

		/// <summary>
		/// コード生成レベル [0:最小限、1:詳細]
		/// </summary>
		[TypeConverter(typeof(CodeGenerationLevelConverter))]
		[CxCategory("Tasks")]
		[CxDescription("P:XIEstudio.CxAppSettings.CodeGenerationLevel")]
		public virtual int CodeGenerationLevel
		{
			get { return m_CodeGenerationLevel; }
			set { m_CodeGenerationLevel = value; }
		}
		private int m_CodeGenerationLevel = 0;

		#region IndexConverter

		class CodeGenerationLevelConverter : Int32Converter
		{
			public CodeGenerationLevelConverter()
			{
			}
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}
			public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				return new StandardValuesCollection(new int[] { 0, 1 });
			}
		}

		#endregion

		#endregion

		#region プロパティ: (Media)

		/// <summary>
		/// 保存する際の動画の種別
		/// </summary>
		[XIE.CxCategory("Media")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.VideoFileType")]
		public virtual XIE.Tasks.ExVideoFileType VideoFileType
		{
			get { return m_VideoFileType; }
			set { m_VideoFileType = value; }
		}
		private XIE.Tasks.ExVideoFileType m_VideoFileType = XIE.Tasks.ExVideoFileType.Auto;

		/// <summary>
		/// Exif の項目名の言語モード [0:英語、1:日本語]
		/// </summary>
		[XIE.CxCategory("Media")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.ExifItemNameMode")]
		public virtual int ExifItemNameMode
		{
			get { return m_ExifItemNameMode; }
			set { m_ExifItemNameMode = value; }
		}
		private int m_ExifItemNameMode = 1;

		#endregion

		#region プロパティ: (Visualization)

		/// <summary>
		/// 画像ビューの背景の塗り潰し色
		/// </summary>
		[XmlIgnore]
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.BkColor")]
		public virtual TxRGB8x3 BkColor
		{
			get { return m_BkColor; }
			set { m_BkColor = value; }
		}
		private TxRGB8x3 m_BkColor = new TxRGB8x3(64, 64, 64);

		/// <summary>
		/// ROI のグリッド表示の有効属性
		/// </summary>
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.ROIGrid")]
		public virtual bool ROIGrid
		{
			get { return m_ROIGrid; }
			set { m_ROIGrid = value; }
		}
		private bool m_ROIGrid = true;

		/// <summary>
		/// ROI のグリッド種別 [0:３分割、1:４分割]
		/// </summary>
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.ROIGridType")]
		public virtual int ROIGridType
		{
			get { return m_ROIGridType; }
			set { m_ROIGridType = value; }
		}
		private int m_ROIGridType = 0;

		/// <summary>
		/// ROI の前景色
		/// </summary>
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.ROIPenColor")]
		public virtual XIE.TxRGB8x4 ROIPenColor
		{
			get { return m_ROIPenColor; }
			set { m_ROIPenColor = value; }
		}
		private XIE.TxRGB8x4 m_ROIPenColor = new XIE.TxRGB8x4(255, 255, 255);

		/// <summary>
		/// ROI の背景色
		/// </summary>
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.ROIBkColor")]
		public virtual XIE.TxRGB8x4 ROIBkColor
		{
			get { return m_ROIBkColor; }
			set { m_ROIBkColor = value; }
		}
		private XIE.TxRGB8x4 m_ROIBkColor = new XIE.TxRGB8x4(0, 0, 0, 64);

		/// <summary>
		/// ROI の背景の塗り潰しの有効属性
		/// </summary>
		[XIE.CxCategory("Visualization")]
		[XIE.CxDescription("P:XIEstudio.CxAppSettings.ROIBkEnable")]
		public virtual bool ROIBkEnable
		{
			get { return m_ROIBkEnable; }
			set { m_ROIBkEnable = value; }
		}
		private bool m_ROIBkEnable = false;

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
				ImageFileDirectory = ValidateDirectory(ImageFileDirectory);
				TaskflowFileDirectory = ValidateDirectory(TaskflowFileDirectory);
				SaveDataDirectory = ValidateDirectory(SaveDataDirectory);
			}
			#endregion

			#region SetDllDirectory
			{
#if LINUX
#else
				string dir = "";
				if (Environment.Is64BitProcess)
					dir = CxAuxInfoForm.AppSettings.DllDirectoryX64;
				else
					dir = CxAuxInfoForm.AppSettings.DllDirectoryX86;
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

			this.WindowState = _src.WindowState;
			this.ImageFileDirectory = _src.ImageFileDirectory;
			this.TaskflowFileDirectory = _src.TaskflowFileDirectory;
			this.SaveDataDirectory = _src.SaveDataDirectory;

			// Tasks
			this.DllDirectoryX64 = _src.DllDirectoryX64;
			this.DllDirectoryX86 = _src.DllDirectoryX86;

			this.References = new XIE.Tasks.CxReferencedAssembly[_src.References.Length];
			for (int i = 0; i < this.References.Length; i++)
				this.References[i] = (XIE.Tasks.CxReferencedAssembly)_src.References[i].Clone();

			this.DebugMode = _src.DebugMode;
			this.GenerateEntryPoint = _src.GenerateEntryPoint;
			this.CodeGenerationLevel = _src.CodeGenerationLevel;

			// Media
			this.VideoFileType = _src.VideoFileType;
			this.ExifItemNameMode = _src.ExifItemNameMode; 

			// Visualization
			this.BkColor = _src.BkColor;
			this.ROIGrid = _src.ROIGrid;
			this.ROIGridType = _src.ROIGridType;
			this.ROIPenColor = _src.ROIPenColor;
			this.ROIBkColor = _src.ROIBkColor;
			this.ROIBkEnable = _src.ROIBkEnable;

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

				if (this.WindowState != _src.WindowState) return false;
				if (this.ImageFileDirectory != _src.ImageFileDirectory) return false;
				if (this.TaskflowFileDirectory != _src.TaskflowFileDirectory) return false;
				if (this.SaveDataDirectory != _src.SaveDataDirectory) return false;

				// Tasks
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

				if (this.DebugMode != _src.DebugMode) return false;
				if (this.GenerateEntryPoint != _src.GenerateEntryPoint) return false;
				if (this.CodeGenerationLevel != _src.CodeGenerationLevel) return false;

				// Media
				if (this.VideoFileType != _src.VideoFileType) return false;
				if (this.ExifItemNameMode != _src.ExifItemNameMode) return false;

				// Visualization
				if (this.BkColor != _src.BkColor) return false;
				if (this.ROIGrid != _src.ROIGrid) return false;
				if (this.ROIGridType != _src.ROIGridType) return false;
				if (this.ROIPenColor != _src.ROIPenColor) return false;
				if (this.ROIBkColor != _src.ROIBkColor) return false;
				if (this.ROIBkEnable != _src.ROIBkEnable) return false;

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
