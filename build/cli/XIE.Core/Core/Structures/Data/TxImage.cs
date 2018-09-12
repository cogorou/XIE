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
	/// 画像オブジェクト構造体
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	[StructLayout(LayoutKind.Sequential, Pack = XIE.Defs.XIE_PACKING_SIZE)]
	public struct TxImage :
		IEquatable<TxImage>
	{
		#region プロパティ:

		/// <summary>
		/// 画像レイヤー
		/// </summary>
		public TxLayer Layer;

		/// <summary>
		/// 幅 [範囲:1~]
		/// </summary>
		public int Width;

		/// <summary>
		/// 高さ [範囲:1~]
		/// </summary>
		public int Height;

		/// <summary>
		/// 要素の型
		/// </summary>
		public TxModel Model;

		/// <summary>
		/// チャネル数 [範囲:1~16]
		/// </summary>
		public int Channels;

		/// <summary>
		/// 領域の水平方向サイズ (bytes)
		/// </summary>
		public int Stride;

		/// <summary>
		/// ビット深度 [範囲:0=最大値、1~=指定値]
		/// </summary>
		public int Depth;

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addr">画像アドレス</param>
		/// <param name="width">幅 [範囲:1~]</param>
		/// <param name="height">高さ [範囲:1~]</param>
		/// <param name="model">要素の型</param>
		/// <param name="stride">領域の水平方向サイズ (bytes)</param>
		/// <param name="depth">ビット深度 (bits) [範囲:0=最大値、1~=指定値]</param>
		public TxImage(IntPtr addr, int width, int height, TxModel model, int stride, int depth)
		{
			Layer = new TxLayer(new IntPtr[] {addr});
			Width = width;
			Height = height;
			Model = model;
			Channels = 1;
			Stride = stride;
			Depth = depth;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="layer">画像レイヤー</param>
		/// <param name="width">幅 [範囲:1~]</param>
		/// <param name="height">高さ [範囲:1~]</param>
		/// <param name="model">要素の型</param>
		/// <param name="channels">チャネル数 [範囲:1~16]</param>
		/// <param name="stride">領域の水平方向サイズ (bytes)</param>
		/// <param name="depth">ビット深度 (bits) [範囲:0=最大値、1~=指定値]</param>
		public TxImage(TxLayer layer, int width, int height, TxModel model, int channels, int stride, int depth)
		{
			Layer = layer;
			Width = width;
			Height = height;
			Model = model;
			Channels = channels;
			Stride = stride;
			Depth = depth;
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
		public bool Equals(TxImage other)
		{
			for(int i=0 ; i<XIE.Defs.XIE_IMAGE_CHANNELS_MAX ; i++)
				if (this.Layer[i] != other.Layer[i]) return false;
			if (this.Width != other.Width) return false;
			if (this.Height != other.Height) return false;
			if (this.Model != other.Model) return false;
			if (this.Channels != other.Channels) return false;
			if (this.Stride != other.Stride) return false;
			if (this.Depth != other.Depth) return false;
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
			if (!(obj is TxImage)) return false;
			return this.Equals((TxImage)obj);
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
		public static bool operator ==(TxImage left, TxImage right)
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
		public static bool operator !=(TxImage left, TxImage right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 暗黙的な型変換:

		/// <summary>
		/// 暗黙的な型変換 (自身 ← TxScanner2D)
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		キャスト結果を返します。
		/// </returns>
		public static implicit operator TxImage(TxScanner2D src)
		{
			return new TxImage(src.Address, src.Width, src.Height, src.Model, src.Stride, 0);
		}

		#endregion
	}
}
