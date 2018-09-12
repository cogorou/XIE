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
using System.Drawing.Design;
using System.Windows.Forms.Design;
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
	/// カラー構造体 (RGBA) 8bit
	/// </summary>
	[Serializable]
	[Editor(typeof(SelfEditor), typeof(System.Drawing.Design.UITypeEditor))]
	[TypeConverter(typeof(SelfConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	[CxModelOf(Type = ExType.U8, Pack = 4)]
	public struct TxRGB8x4 :
		IEquatable<TxRGB8x4>
	{
		#region フィールド:

		byte m_R;
		byte m_G;
		byte m_B;
		byte m_A;

		#endregion

		#region プロパティ:

		/// <summary>
		/// 赤成分
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRGB8x4.R")]
		public byte R
		{
			get { return m_R; }
			set { m_R = value; }
		}

		/// <summary>
		/// 緑成分
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRGB8x4.G")]
		public byte G
		{
			get { return m_G; }
			set { m_G = value; }
		}

		/// <summary>
		/// 青成分
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRGB8x4.B")]
		public byte B
		{
			get { return m_B; }
			set { m_B = value; }
		}

		/// <summary>
		/// アルファ値
		/// </summary>
		[CxCategory("Parameters")]
		[CxDescription("P:XIE.TxRGB8x4.A")]
		public byte A
		{
			get { return m_A; }
			set { m_A = value; }
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="red">赤成分</param>
		/// <param name="green">緑成分</param>
		/// <param name="blue">青成分</param>
		/// <param name="alpha">アルファ値</param>
		public TxRGB8x4(byte red, byte green, byte blue, byte alpha = 0xFF)
		{
			m_R = red;
			m_G = green;
			m_B = blue;
			m_A = alpha;
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
		public bool Equals(TxRGB8x4 other)
		{
			if (this.R != other.R) return false;
			if (this.G != other.G) return false;
			if (this.B != other.B) return false;
			if (this.A != other.A) return false;
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
			if (!(obj is TxRGB8x4)) return false;
			return this.Equals((TxRGB8x4)obj);
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
		public static bool operator ==(TxRGB8x4 left, TxRGB8x4 right)
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
		public static bool operator !=(TxRGB8x4 left, TxRGB8x4 right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換: (他の型 ← 自身)

		/// <summary>
		/// 暗黙的な型変換 (TxBGR8x3 ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxBGR8x3(TxRGB8x4 src)
		{
			return new TxBGR8x3(src.R, src.G, src.B);
		}

		/// <summary>
		/// 暗黙的な型変換 (TxBGR8x4 ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxBGR8x4(TxRGB8x4 src)
		{
			return new TxBGR8x4(src.R, src.G, src.B, src.A);
		}

		/// <summary>
		/// 暗黙的な型変換 (TxRGB8x3 ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxRGB8x3(TxRGB8x4 src)
		{
			return new TxRGB8x3(src.R, src.G, src.B);
		}

		/// <summary>
		/// 暗黙的な型変換 (自身 ← Color)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxRGB8x4(Color src)
		{
			return new TxRGB8x4(src.R, src.G, src.B, src.A);
		}

		/// <summary>
		/// 暗黙的な型変換 (Color ← 自身)
		/// </summary>
		/// <param name="src">右辺値</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator Color(TxRGB8x4 src)
		{
			return Color.FromArgb(src.A, src.R, src.G, src.B);
		}

		#endregion

		#region 変換系: (文字列変換)

		/// <summary>
		/// 文字列変換
		/// </summary>
		/// <returns>
		///		現在の値を文字列に変換して返します。
		/// </returns>
		public override string ToString()
		{
			return string.Format("{0},{1},{2},{3}",
				this.R,
				this.G,
				this.B,
				this.A
				);
		}

		#endregion

		#region SelfConverter:

		private class SelfConverter : CxSortingConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(TxRGB8x4)) return true;
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && value is TxRGB8x4)
				{
					var _value = (TxRGB8x4)value;
					return string.Format("{0},{1},{2},{3}",
						_value.R,
						_value.G,
						_value.B,
						_value.A
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
					var dst = new TxRGB8x4();
					for (int i = 0; i < values.Length; i++)
					{
						switch (i)
						{
							case 0: dst.R = Convert.ToByte(values[i]); break;
							case 1: dst.G = Convert.ToByte(values[i]); break;
							case 2: dst.B = Convert.ToByte(values[i]); break;
							case 3: dst.A = Convert.ToByte(values[i]); break;
						}
					}
					return dst;
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		#endregion

		#region SelfEditor

		/// <summary>
		/// カラー構造体 (RGBA) 8bit プロパティグリッド用エディタ
		/// </summary>
		private class SelfEditor : System.Drawing.Design.ColorEditor
		{
			/// <summary>
			/// コンストラクタ
			/// </summary>
			public SelfEditor()
			{
			}

			/// <summary>
			/// 編集スタイルの取得
			/// </summary>
			/// <param name="context">コンテキスト情報</param>
			/// <returns>
			///		UITypeEditorEditStyle.Modal を返します。
			/// </returns>
			public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
			{
				return UITypeEditorEditStyle.Modal;
			}

			/// <summary>
			/// オブジェクトの値の編集
			/// </summary>
			/// <param name="context">コンテキスト情報</param>
			/// <param name="provider">サービスを取得する為に使用するプロバイダ</param>
			/// <param name="value">編集対象のオブジェクト</param>
			/// <returns>
			///		編集後の値を返します。
			/// </returns>
			public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				var color = (Color)(TxRGB8x4)value;
				var result = base.EditValue(context, provider, color);
				return (TxRGB8x4)(Color)result;
			}

			/// <summary>
			/// オブジェクトの内部表現の描画
			/// </summary>
			/// <param name="e">描画対象と描画位置</param>
			public override void PaintValue(PaintValueEventArgs e)
			{
				if (e.Value is TxRGB8x4)
				{
					var color = (Color)(TxRGB8x4)e.Value;
					var args = new PaintValueEventArgs(e.Context, color, e.Graphics, e.Bounds);
					base.PaintValue(args);
				}
				else
				{
					base.PaintValue(e);
				}
			}
		}

		#endregion
	}
}
