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
	/// レイヤー構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxLayer :
		IEquatable<TxLayer>
	{
		#region プロパティ:

		/// <summary>
		/// 領域の先頭アドレス (0 チャネル目)
		/// </summary>
		public IntPtr Address0;
		/// <summary>
		/// 領域の先頭アドレス (1 チャネル目)
		/// </summary>
		public IntPtr Address1;
		/// <summary>
		/// 領域の先頭アドレス (2 チャネル目)
		/// </summary>
		public IntPtr Address2;
		/// <summary>
		/// 領域の先頭アドレス (3 チャネル目)
		/// </summary>
		public IntPtr Address3;
		/// <summary>
		/// 領域の先頭アドレス (4 チャネル目)
		/// </summary>
		public IntPtr Address4;
		/// <summary>
		/// 領域の先頭アドレス (5 チャネル目)
		/// </summary>
		public IntPtr Address5;
		/// <summary>
		/// 領域の先頭アドレス (6 チャネル目)
		/// </summary>
		public IntPtr Address6;
		/// <summary>
		/// 領域の先頭アドレス (7 チャネル目)
		/// </summary>
		public IntPtr Address7;
		/// <summary>
		/// 領域の先頭アドレス (8 チャネル目)
		/// </summary>
		public IntPtr Address8;
		/// <summary>
		/// 領域の先頭アドレス (9 チャネル目)
		/// </summary>
		public IntPtr Address9;
		/// <summary>
		/// 領域の先頭アドレス (10 チャネル目)
		/// </summary>
		public IntPtr Address10;
		/// <summary>
		/// 領域の先頭アドレス (11 チャネル目)
		/// </summary>
		public IntPtr Address11;
		/// <summary>
		/// 領域の先頭アドレス (12 チャネル目)
		/// </summary>
		public IntPtr Address12;
		/// <summary>
		/// 領域の先頭アドレス (13 チャネル目)
		/// </summary>
		public IntPtr Address13;
		/// <summary>
		/// 領域の先頭アドレス (14 チャネル目)
		/// </summary>
		public IntPtr Address14;
		/// <summary>
		/// 領域の先頭アドレス (15 チャネル目)
		/// </summary>
		public IntPtr Address15;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addrs">0 チャネル目の領域の先頭アドレス</param>
		public TxLayer(IntPtr addrs)
		{
			Address0 = addrs;
			Address1 = IntPtr.Zero;
			Address2 = IntPtr.Zero;
			Address3 = IntPtr.Zero;
			Address4 = IntPtr.Zero;
			Address5 = IntPtr.Zero;
			Address6 = IntPtr.Zero;
			Address7 = IntPtr.Zero;
			Address8 = IntPtr.Zero;
			Address9 = IntPtr.Zero;
			Address10 = IntPtr.Zero;
			Address11 = IntPtr.Zero;
			Address12 = IntPtr.Zero;
			Address13 = IntPtr.Zero;
			Address14 = IntPtr.Zero;
			Address15 = IntPtr.Zero;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addrs">各領域の先頭アドレスが格納された配列 [要素数=0~XIE.Defs.XIE_IMAGE_CHANNELS_MAX]</param>
		public TxLayer(IntPtr[] addrs)
		{
			Address0 = IntPtr.Zero;
			Address1 = IntPtr.Zero;
			Address2 = IntPtr.Zero;
			Address3 = IntPtr.Zero;
			Address4 = IntPtr.Zero;
			Address5 = IntPtr.Zero;
			Address6 = IntPtr.Zero;
			Address7 = IntPtr.Zero;
			Address8 = IntPtr.Zero;
			Address9 = IntPtr.Zero;
			Address10 = IntPtr.Zero;
			Address11 = IntPtr.Zero;
			Address12 = IntPtr.Zero;
			Address13 = IntPtr.Zero;
			Address14 = IntPtr.Zero;
			Address15 = IntPtr.Zero;

			for (int i = 0; i < addrs.Length; i++)
			{
				this[i] = addrs[i];
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
		public bool Equals(TxLayer other)
		{
			for(int i=0 ; i<XIE.Defs.XIE_IMAGE_CHANNELS_MAX ; i++)
				if (this[i] != other[i]) return false;
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
			if (!(obj is TxLayer)) return false;
			return this.Equals((TxLayer)obj);
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
		public static bool operator ==(TxLayer left, TxLayer right)
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
		public static bool operator !=(TxLayer left, TxLayer right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region インデクサ:

		/// <summary>
		/// インデクサ
		/// </summary>
		/// <param name="index">チャネル指標 [0~XIE.Defs.XIE_IMAGE_CHANNELS_MAX-1]</param>
		/// <returns>
		///		指定されたチャネルのアドレスを返します。
		/// </returns>
		public IntPtr this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return Address0;
					case 1: return Address1;
					case 2: return Address2;
					case 3: return Address3;
					case 4: return Address4;
					case 5: return Address5;
					case 6: return Address6;
					case 7: return Address7;
					case 8: return Address8;
					case 9: return Address9;
					case 10: return Address10;
					case 11: return Address11;
					case 12: return Address12;
					case 13: return Address13;
					case 14: return Address14;
					case 15: return Address15;
					default:
						throw new System.ArgumentException("The index have to be between 0 to XIE.Defs.XIE_IMAGE_CHANNELS_MAX-1.");
				}
			}
			set
			{
				switch (index)
				{
					case 0: Address0 = value; break;
					case 1: Address1 = value; break;
					case 2: Address2 = value; break;
					case 3: Address3 = value; break;
					case 4: Address4 = value; break;
					case 5: Address5 = value; break;
					case 6: Address6 = value; break;
					case 7: Address7 = value; break;
					case 8: Address8 = value; break;
					case 9: Address9 = value; break;
					case 10: Address10 = value; break;
					case 11: Address11 = value; break;
					case 12: Address12 = value; break;
					case 13: Address13 = value; break;
					case 14: Address14 = value; break;
					case 15: Address15 = value; break;
					default:
						throw new System.ArgumentException("The index have to be between 0 to XIE.Defs.XIE_IMAGE_CHANNELS_MAX-1.");
				}
			}
		}

		#endregion

		#region 変換系:

		/// <summary>
		/// 配列への変換
		/// </summary>
		/// <param name="index">開始位置 [0~(XIE.Defs.XIE_IMAGE_CHANNELS_MAX-1)]</param>
		/// <param name="count">個数 [0=末端まで、1~=指定個数]</param>
		/// <returns>
		///		アドレスを配列に格納して返します。
		/// </returns>
		public IntPtr[] ToArray(int index = 0, int count = 0)
		{
			if (!(0 <= index && index <= (XIE.Defs.XIE_IMAGE_CHANNELS_MAX - 1)))
				throw new CxException(ExStatus.InvalidParam);
			if (count == 0)
				count = XIE.Defs.XIE_IMAGE_CHANNELS_MAX - index;
			if (count <= 0)
				throw new CxException(ExStatus.InvalidParam);
			if ((index + count) > XIE.Defs.XIE_IMAGE_CHANNELS_MAX)
				throw new CxException(ExStatus.InvalidParam);

			var dst = new IntPtr[count];
			for (int i = 0; i < count; i++)
				dst[i] = this[i];

			return dst;
		}

		#endregion
	}
}
