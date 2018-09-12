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

namespace XIE
{
	/// <summary>
	/// 画像サイズ構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxImageSize :
		IEquatable<TxImageSize>
	{
		#region フィールド:

		int m_Width;
		int m_Height;
		TxModel m_Model;
		int m_Channels;
		int m_Depth;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 幅 [範囲:1~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxImageSize.Width")]
		public int Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}

		/// <summary>
		/// 高さ [範囲:1~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxImageSize.Height")]
		public int Height
		{
			get { return m_Height; }
			set { m_Height = value; }
		}

		/// <summary>
		/// 要素の型
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxImageSize.Model")]
		public TxModel Model
		{
			get { return m_Model; }
			set { m_Model = value; }
		}

		/// <summary>
		/// チャネル数 [範囲:1~16]
		/// </summary>
		[TypeConverter(typeof(ChannelsConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxImageSize.Channels")]
		public int Channels
		{
			get { return m_Channels; }
			set { m_Channels = value; }
		}

		#region ChannelsConverter

		class ChannelsConverter : Int32Converter
		{
			public ChannelsConverter()
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
					var items = new List<int>();
					for (int i = 0; i < XIE.Defs.XIE_IMAGE_CHANNELS_MAX; i++)
						items.Add(i + 1);
					return new StandardValuesCollection(items);
				}
				catch (System.Exception)
				{
					return null;
				}
			}
		}

		#endregion

		/// <summary>
		/// ビット深度 [範囲:0=最大値、1~=指定値]
		/// </summary>
		[TypeConverter(typeof(DepthConverter))]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxImageSize.Depth")]
		public int Depth
		{
			get { return m_Depth; }
			set { m_Depth = value; }
		}

		#region DepthConverter

		class DepthConverter : Int32Converter
		{
			public DepthConverter()
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
					var items = new List<int>();
					for (int i = 0; i <= 64; i++)
						items.Add(i);
					return new StandardValuesCollection(items);
				}
				catch (System.Exception)
				{
					return null;
				}
			}
		}

		#endregion

		#endregion

		#region プロパティ:(拡張)

		/// <summary>
		/// 幅と高さ
		/// </summary>
		[XmlIgnore]
		[CxCategory("Parameters.2")]
		[CxDescription("P:XIE.TxImageSize.Size")]
		public TxSizeI Size
		{
			get
			{
				return new TxSizeI(this.Width, this.Height);
			}
			set
			{
				this.Width = value.Width;
				this.Height = value.Height;
			}
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="width">幅 [範囲:1~]</param>
		/// <param name="height">高さ [範囲:1~]</param>
		/// <param name="model">要素の型</param>
		/// <param name="channels">チャネル数 [範囲:1~16]</param>
		/// <param name="depth">ビット深度 (bits) [範囲:0=最大値、1~=指定値]</param>
		public TxImageSize(int width, int height, TxModel model, int channels, int depth = 0)
		{
			m_Width = width;
			m_Height = height;
			m_Model = model;
			m_Channels = channels;
			m_Depth = depth;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="size">幅と高さ [範囲:1~]</param>
		/// <param name="model">要素の型</param>
		/// <param name="channels">チャネル数 [範囲:1~16]</param>
		/// <param name="depth">ビット深度 (bits) [範囲:0=最大値、1~=指定値]</param>
		public TxImageSize(TxSizeI size, TxModel model, int channels, int depth = 0)
		{
			m_Width = size.Width;
			m_Height = size.Height;
			m_Model = model;
			m_Channels = channels;
			m_Depth = depth;
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
		public bool Equals(TxImageSize other)
		{
			if (this.Width != other.Width) return false;
			if (this.Height != other.Height) return false;
			if (this.Model != other.Model) return false;
			if (this.Channels != other.Channels) return false;
		//	if (this.Depth != other.Depth) return false;	// ignore
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
			if (!(obj is TxImageSize)) return false;
			return this.Equals((TxImageSize)obj);
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
		public static bool operator ==(TxImageSize left, TxImageSize right)
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
		public static bool operator !=(TxImageSize left, TxImageSize right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxImageSize)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxImageSize)
				{
					TxImageSize _value = (TxImageSize)value;
					return string.Format("{0},{1},{2},{3},{4},{5}",
						_value.Width,
						_value.Height,
						_value.Model.Type,
						_value.Model.Pack,
						_value.Channels,
						_value.Depth
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
					var dst = new TxImageSize();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.Width = Convert.ToInt32(values[i]); break;
							case 1: dst.Height = Convert.ToInt32(values[i]); break;
							case 3:
								{
									var type = (ExType)Enum.Parse(typeof(ExType), values[2]);
									int pack = Convert.ToInt32(values[3]);
									dst.Model = new TxModel(type, pack);
								}
								break;
							case 4: dst.Channels = Convert.ToInt32(values[i]); break;
							case 5: dst.Depth = Convert.ToInt32(values[i]); break;
						}
					}
					return dst;
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion
	}
}
