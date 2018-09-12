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
using System.Text;

namespace XIE
{
	/// <summary>
	/// Raw フォーマットファイルヘッダー構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxRawFileHeader :
		IEquatable<TxRawFileHeader>
	{
		#region プロパティ:

		/// <summary>
		/// 識別子 [Axi.XIE_MODULE_ID]
		/// </summary>
		public int Signature;

		/// <summary>
		/// バージョン [Axi.XIE_VER]
		/// </summary>
		public int Version;

		/// <summary>
		/// リビジョン [0]
		/// </summary>
		public int Revision;

		/// <summary>
		/// クラス名
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] ClassName;

		/// <summary>
		/// 終端 (常に 0 です)
		/// </summary>
		public int Terminal;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// 既定値
		/// </summary>
		public static TxRawFileHeader Reset()
		{
			TxRawFileHeader result = new TxRawFileHeader();
			result.Signature = XIE.Defs.XIE_MODULE_ID;
			result.Version = XIE.Defs.XIE_VER;
			result.Revision = 0;
			result.ClassName = new byte[256];
			result.Terminal = 0;
			return result;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">クラス名</param>
		public TxRawFileHeader(string name)
		{
			Signature = XIE.Defs.XIE_MODULE_ID;
			Version = XIE.Defs.XIE_VER;
			Revision = 0;
			ClassName = new byte[256];
			Terminal = 0;

			if (string.IsNullOrEmpty(name) == false)
			{
				byte[] src = Encoding.ASCII.GetBytes(name);
				byte[] dst = ClassName;
				int length = src.Length;
				if (length > 256)
					length = 256;
				for (int i = 0; i < length; i++)
					dst[i] = src[i];
			}
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
		public bool Equals(TxRawFileHeader other)
		{
			if (this.Signature != other.Signature) return false;
			if (this.Version != other.Version) return false;
			if (this.Revision != other.Revision) return false;
			if (this.Terminal != other.Terminal) return false;
			if (this.ClassName.Length != other.ClassName.Length) return false;
			string src = Encoding.ASCII.GetString(this.ClassName);
			string cmp = Encoding.ASCII.GetString(other.ClassName);
			if (src != cmp) return false;
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
			if (!(obj is TxRawFileHeader)) return false;
			return this.Equals((TxRawFileHeader)obj);
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
		public static bool operator ==(TxRawFileHeader left, TxRawFileHeader right)
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
		public static bool operator !=(TxRawFileHeader left, TxRawFileHeader right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}
