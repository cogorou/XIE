/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
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
	/// 要素モデル構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxModel :
		IEquatable<TxModel>
	{
		#region フィールド:

		ExType m_Type;
		int m_Pack;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 型
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxModel.Type")]
		public ExType Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}

		/// <summary>
		/// パック数 [範囲:0,1~]
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxModel.Pack")]
		public int Pack
		{
			get { return m_Pack; }
			set { m_Pack = value; }
		}

		#endregion

		#region プロパティ: (拡張)

		/// <summary>
		/// 要素のサイズ (bytes) [初期値:0、範囲:0~]
		/// </summary>
		/// <returns>
		///		現在の型とパック数からサイズを計算して返します。
		///		無効な場合は 0 を返します。
		/// </returns>
		[XmlIgnore]
		[ReadOnly(true)]
		[Browsable(false)]
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxModel.Size")]
		public int Size
		{
			get
			{
				if (this.Pack <= 0)
					return 0;
				else
					return TxModel.SizeOf(this.Type) * this.Pack;
			}
		}

		/// <summary>
		/// 型のサイズ (bytes)
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>
		///		指定された型のサイズを返します。
		/// </returns>
		public static int SizeOf(ExType type)
		{
			switch (type)
			{
				default: return 0;
				case ExType.Ptr: return IntPtr.Size;
				case ExType.U8: return 1;
				case ExType.U16: return 2;
				case ExType.U32: return 4;
				case ExType.U64: return 8;
				case ExType.S8: return 1;
				case ExType.S16: return 2;
				case ExType.S32: return 4;
				case ExType.S64: return 8;
				case ExType.F32: return 4;
				case ExType.F64: return 8;
			}
		}

		#endregion

		#region 既定値:

		/// <summary>
		/// 既定値
		/// </summary>
		public static readonly TxModel Default = default(TxModel);

		#endregion

		#region 生成関数:

		/// <summary>
		/// TxModel(ExType.Ptr, pack) を返します。
		/// </summary>
		/// <param name="pack">パック数 [範囲:1~]</param>
		public static TxModel Ptr(int pack) { return new TxModel(ExType.Ptr, pack); }
		/// <summary>
		/// TxModel(ExType.U8, pack) を返します。
		/// </summary>
		/// <param name="pack">パック数 [範囲:1~]</param>
		public static TxModel U8(int pack) { return new TxModel(ExType.U8, pack); }
		/// <summary>
		/// TxModel(ExType.U16, pack) を返します。
		/// </summary>
		/// <param name="pack">パック数 [範囲:1~]</param>
		public static TxModel U16(int pack) { return new TxModel(ExType.U16, pack); }
		/// <summary>
		/// TxModel(ExType.U32, pack) を返します。
		/// </summary>
		/// <param name="pack">パック数 [範囲:1~]</param>
		public static TxModel U32(int pack) { return new TxModel(ExType.U32, pack); }
		/// <summary>
		/// TxModel(ExType.U64, pack) を返します。
		/// </summary>
		/// <param name="pack">パック数 [範囲:1~]</param>
		public static TxModel U64(int pack) { return new TxModel(ExType.U64, pack); }
		/// <summary>
		/// TxModel(ExType.S8, pack) を返します。
		/// </summary>
		/// <param name="pack">パック数 [範囲:1~]</param>
		public static TxModel S8(int pack) { return new TxModel(ExType.S8, pack); }
		/// <summary>
		/// TxModel(ExType.S16, pack) を返します。
		/// </summary>
		/// <param name="pack">パック数 [範囲:1~]</param>
		public static TxModel S16(int pack) { return new TxModel(ExType.S16, pack); }
		/// <summary>
		/// TxModel(ExType.S32, pack) を返します。
		/// </summary>
		/// <param name="pack">パック数 [範囲:1~]</param>
		public static TxModel S32(int pack) { return new TxModel(ExType.S32, pack); }
		/// <summary>
		/// TxModel(ExType.S64, pack) を返します。
		/// </summary>
		/// <param name="pack">パック数 [範囲:1~]</param>
		public static TxModel S64(int pack) { return new TxModel(ExType.S64, pack); }
		/// <summary>
		/// TxModel(ExType.F32, pack) を返します。
		/// </summary>
		/// <param name="pack">パック数 [範囲:1~]</param>
		public static TxModel F32(int pack) { return new TxModel(ExType.F32, pack); }
		/// <summary>
		/// TxModel(ExType.F64, pack) を返します。
		/// </summary>
		/// <param name="pack">パック数 [範囲:1~]</param>
		public static TxModel F64(int pack) { return new TxModel(ExType.F64, pack); }

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="pack">パック数 [範囲:0,1~]</param>
		public TxModel(ExType type = ExType.None, int pack = 0)
		{
			m_Type = type;
			m_Pack = pack;
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
		public bool Equals(TxModel other)
		{
			if (this.Type != other.Type) return false;
			if (this.Pack != other.Pack) return false;
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
			if (!(obj is TxModel)) return false;
			return this.Equals((TxModel)obj);
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
		public static bool operator ==(TxModel left, TxModel right)
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
		public static bool operator !=(TxModel left, TxModel right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region オペレータ.(加算)

		/// <summary>
		/// 加算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		加算結果を返します。
		/// </returns>
		public static TxModel operator +(TxModel ope1, int ope2)
		{
			TxModel dst = new TxModel();
			dst.Type = ope1.Type;
			dst.Pack = ope1.Pack + ope2;
			return dst;
		}

		#endregion

		#region オペレータ.(減算)

		/// <summary>
		/// 減算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		減算結果を返します。
		/// </returns>
		public static TxModel operator -(TxModel ope1, int ope2)
		{
			TxModel dst = new TxModel();
			dst.Type = ope1.Type;
			dst.Pack = ope1.Pack - ope2;
			return dst;
		}

		#endregion

		#region オペレータ.(乗算)

		/// <summary>
		/// 乗算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		乗算結果を返します。
		/// </returns>
		public static TxModel operator *(TxModel ope1, int ope2)
		{
			TxModel dst = new TxModel();
			dst.Type = ope1.Type;
			dst.Pack = ope1.Pack * ope2;
			return dst;
		}

		#endregion

		#region オペレータ.(除算)

		/// <summary>
		/// 除算
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		除算結果を返します。
		/// </returns>
		public static TxModel operator /(TxModel ope1, int ope2)
		{
			TxModel dst = new TxModel();
			dst.Type = ope1.Type;
			dst.Pack = (ope2 == 0) ? ope1.Pack : ope1.Pack / ope2;
			return dst;
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxModel)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxModel)
				{
					TxModel _value = (TxModel)value;
					return string.Format("{0},{1}",
						_value.Type,
						_value.Pack
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
					var dst = new TxModel();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.Type = (ExType)Enum.Parse(typeof(ExType), values[i]); break;
							case 1: dst.Pack = Convert.ToInt32(values[i]); break;
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
