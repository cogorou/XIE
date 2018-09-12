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

namespace XIEcapture
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
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 音声入力デバイス
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIEcapture.CxAppSettings.AudioInputInfos")]
		public virtual XIE.Media.CxDeviceParam AudioInputInfo
		{
			get { return m_AudioInputInfo; }
			set { m_AudioInputInfo = value; }
		}
		private XIE.Media.CxDeviceParam m_AudioInputInfo = new XIE.Media.CxDeviceParam();

		/// <summary>
		/// フレームレート指標 [0:1fps, 1:5fps, 2:10fps, 3:15fps, 4:30fps]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIEcapture.CxAppSettings.FrameRate")]
		public virtual int FrameRate
		{
			get { return m_FrameRate; }
			set { m_FrameRate = value; }
		}
		private int m_FrameRate = 2;

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

			if (this.AudioInputInfo == null)
				this.AudioInputInfo = (XIE.Media.CxDeviceParam)_src.AudioInputInfo.Clone();
			else
				this.AudioInputInfo.CopyFrom(_src.AudioInputInfo);

			this.FrameRate = _src.FrameRate;

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

				if (this.AudioInputInfo == null && _src.AudioInputInfo != null) return false;
				if (this.AudioInputInfo != null && _src.AudioInputInfo == null) return false;
				if (this.AudioInputInfo != null && _src.AudioInputInfo != null)
				{
					if (this.AudioInputInfo.ContentEquals(_src.AudioInputInfo) == false) return false;
				}

				if (this.FrameRate != _src.FrameRate) return false;

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
