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

namespace XIE.GDI
{
	/// <summary>
	/// フォント情報構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	public struct TxGdiFont :
		IEquatable<TxGdiFont>

	{
		#region フィールド:

		private string m_Name;

		private float m_Size;

		private FontStyle m_Style;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 名称
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdiFont.Name")]
		[TypeConverter(typeof(NameConverter))]
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		#region NameConverter

		class NameConverter : StringConverter
		{
			public NameConverter()
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
				try
				{
					var result = new List<string>();

					{
						var collection = new System.Drawing.Text.InstalledFontCollection();
						var families = collection.Families;

						foreach (var family in families)
						{
							result.Add(family.Name);
						}
					}

					return new StandardValuesCollection(result);
				}
				catch (System.Exception)
				{
					return null;
				}
			}
		}

		#endregion

		/// <summary>
		/// サイズ
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdiFont.Size")]
		public float Size
		{
			get { return m_Size; }
			set { m_Size = value; }
		}

		/// <summary>
		/// スタイル
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.GDI.TxGdiFont.Style")]
		public FontStyle Style
		{
			get { return m_Style; }
			set { m_Style = value; }
		}

		#endregion

		#region 既定値:

		/// <summary>
		/// 既定値
		/// </summary>
		public static TxGdiFont Default
		{
			get
			{
				var result = new TxGdiFont();
				result.Name = "Consolas";
				result.Size = 14;
				result.Style = FontStyle.Regular;
				return result;
			}
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="size">サイズ</param>
		/// <param name="style">スタイル</param>
		public TxGdiFont(string name, float size, FontStyle style)
		{
			m_Name = name;
			m_Size = size;
			m_Style = style;
		}

		#endregion

		#region 比較:

		/// <summary>
		/// IEquatable の実装: 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="other">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public bool Equals(TxGdiFont other)
		{
			if (this.Name != other.Name) return false;
			if (this.Size != other.Size) return false;
			if (this.Style != other.Style) return false;
			return true;
		}

		/// <summary>
		/// 自身の内容と指定されたオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="obj">比較するオブジェクト。</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null)) return false;
			if (!(obj is TxGdiFont)) return false;
			return this.Equals((TxGdiFont)obj);
		}

		/// <summary>
		/// ハッシュ値の取得
		/// </summary>
		/// <returns>
		///		常に 0 を返します。
		/// </returns>
		public override int GetHashCode()
		{
			return 0;
		}

		#endregion

		#region 比較オペレータ:

		/// <summary>
		/// ２つのオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		内容が等しい場合は true、それ以外は false を返します。
		/// </returns>
		public static bool operator ==(TxGdiFont left, TxGdiFont right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// ２つのオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		内容が等しい場合は false、それ以外は true を返します。
		/// </returns>
		public static bool operator !=(TxGdiFont left, TxGdiFont right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 変換系:

		/// <summary>
		/// フォントの生成
		/// </summary>
		/// <returns>
		///		フォントオブジェクトを新しく生成して返します。
		/// </returns>
		public Font ToFont()
		{
			return new Font(this.Name, this.Size, this.Style);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxGdiFont)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxGdiFont)
				{
					var _value = (TxGdiFont)value;
					return string.Format("{0},{1},{2}",
						_value.Name,
						_value.Size,
						_value.Style
						);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				if (sourceType == typeof(string)) return true;
				return base.CanConvertFrom(context, sourceType);
			}

			public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
			{
				if (value is string)
				{
					var values = value.ToString().Split(new char[] { ',' });
					var dst = new TxGdiFont();
					var name = "";
					var size = 0f;
					var style = FontStyle.Regular;
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: name = values[i]; break;
							case 1: size = Convert.ToSingle(values[i]); break;
							case 2: Enum.TryParse<FontStyle>(values[i], out style); break;
						}
					}
					dst.Name = name;
					dst.Size = size;
					dst.Style = style;
					return dst;
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}
}
