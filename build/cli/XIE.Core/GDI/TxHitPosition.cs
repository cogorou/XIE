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
	/// ヒット位置構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxHitPosition :
		IEquatable<TxHitPosition>
	{
		#region フィールド:

		private int m_Mode;
		private int m_Index;
		private int m_Site;

		#endregion

		#region プロパティ:

		/// <summary>
		/// モード [0:無効、1:移動、2:形状変更]
		/// </summary>
		public int Mode
		{
			get { return m_Mode; }
			set { m_Mode = value; }
		}

		/// <summary>
		/// 配列指標 [0~]
		/// </summary>
		public int Index
		{
			get { return m_Index; }
			set { m_Index = value; }
		}

		/// <summary>
		/// 部位 [0:無効、+1~:頂点、-1~:辺]
		/// </summary>
		public int Site
		{
			get { return m_Site; }
			set { m_Site = value; }
		}

		#endregion

		#region 既定値:

		/// <summary>
		/// 既定値
		/// </summary>
		public static readonly TxHitPosition Default = default(TxHitPosition);

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="mode">モード [0:無効、1:移動、2:形状変更]</param>
		/// <param name="index">配列指標 [0~]</param>
		/// <param name="site">部位 [0:無効、+1~:頂点、-1~:辺]</param>
		public TxHitPosition(int mode, int index, int site)
		{
			m_Mode = mode;
			m_Index = index;
			m_Site = site;
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
		public bool Equals(TxHitPosition other)
		{
			if (this.Mode != other.Mode) return false;
			if (this.Index != other.Index) return false;
			if (this.Site != other.Site) return false;
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
			if (!(obj is TxHitPosition)) return false;
			return this.Equals((TxHitPosition)obj);
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
		public static bool operator ==(TxHitPosition left, TxHitPosition right)
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
		public static bool operator !=(TxHitPosition left, TxHitPosition right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxHitPosition)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxHitPosition)
				{
					var _value = (TxHitPosition)value;
					return string.Format("{0},{1},{2}",
						_value.Mode,
						_value.Index,
						_value.Site
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
					var dst = new TxHitPosition();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.Mode = Convert.ToInt32(values[i]); break;
							case 1: dst.Index = Convert.ToInt32(values[i]); break;
							case 2: dst.Site = Convert.ToInt32(values[i]); break;
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
