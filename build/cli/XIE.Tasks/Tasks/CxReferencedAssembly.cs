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
	/// 参照するアセンブリの情報
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public partial class CxReferencedAssembly : System.Object
		, ICloneable
		, IxEquatable
		, IxFileAccess
	{
		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxReferencedAssembly()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="fullname">アセンブリ名 (アセンブリの厳密名を指定してください。)</param>
		/// <param name="hintpath">アセンブリファイルの格納場所</param>
		public CxReferencedAssembly(string fullname, string hintpath)
		{
			FullName = fullname;
			HintPath = hintpath;
		}

		#endregion

		// ////////////////////////////////////////////////////////////
		// プロパティ:
		//

		#region プロパティ:

		/// <summary>
		/// アセンブリ名 (アセンブリの厳密名を指定してください。)
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxReferencedAssembly.FullName")]
		public virtual string FullName
		{
			get { return m_FullName; }
			set { m_FullName = value; }
		}
		private string m_FullName = "";

		/// <summary>
		/// アセンブリファイルの格納場所
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.Tasks.CxReferencedAssembly.HintPath")]
		public virtual string HintPath
		{
			get { return m_HintPath; }
			set { m_HintPath = value; }
		}
		private string m_HintPath = "";

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

			var _src = (CxReferencedAssembly)src;

			this.FullName = _src.FullName;
			this.HintPath = _src.HintPath;

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
				var _src = (CxReferencedAssembly)src;

				if (this.FullName != _src.FullName) return false;
				if (this.HintPath != _src.HintPath) return false;

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
		public static CxReferencedAssembly LoadFrom(string filename, params object[] options)
		{
			return (CxReferencedAssembly)XIE.Axi.ReadAsXml(filename, typeof(CxReferencedAssembly));
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
				if (destinationType == typeof(CxReferencedAssembly))
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