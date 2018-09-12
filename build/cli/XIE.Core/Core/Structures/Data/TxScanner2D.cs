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
using System.Linq;

namespace XIE
{
	/// <summary>
	/// ２次元配列走査構造体
	/// </summary>
	public unsafe struct TxScanner2D :
		IEquatable<TxScanner2D>
	{
		#region プロパティ:

		/// <summary>
		/// 領域の先頭アドレス
		/// </summary>
		public IntPtr Address;

		/// <summary>
		/// 幅
		/// </summary>
		public int Width;

		/// <summary>
		/// 高さ
		/// </summary>
		public int Height;

		/// <summary>
		/// 要素モデル
		/// </summary>
		public TxModel Model;

		/// <summary>
		/// 水平方向サイズ (bytes)
		/// </summary>
		public int Stride;

		/// <summary>
		/// 有効性の検査
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		public bool IsValid
		{
			get
			{
				if (this.Address == IntPtr.Zero) return false;
				if (this.Width <= 0) return false;
				if (this.Height <= 0) return false;
				if (this.Model.Size <= 0) return false;
				if (this.Stride < this.Width * this.Model.Size) return false;
				return true;
			}
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addr">配列の先頭アドレス</param>
		/// <param name="width">幅 [0~]</param>
		/// <param name="height">高さ [0~]</param>
		/// <param name="model">要素モデル</param>
		/// <param name="stride">水平方向サイズ (bytes)</param>
		public TxScanner2D(IntPtr addr, int width, int height, TxModel model, int stride)
		{
			Address = addr;
			Width = width;
			Height = height;
			Model = model;
			Stride = stride;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="src">参照先</param>
		/// <param name="ch">チャネル指標 [0~(src.Channels-1)]</param>
		public TxScanner2D(TxImage src, int ch = 0)
		{
			Address = src.Layer[ch];
			Width = src.Width;
			Height = src.Height;
			Model = src.Model;
			Stride = src.Stride;
		}

		#endregion

		#region インデクサ:

		/// <summary>
		/// 要素のアドレス
		/// </summary>
		/// <param name="y">Y座標</param>
		/// <param name="x">X座標</param>
		/// <returns>
		///		指定位置の要素のアドレスを返します。
		/// </returns>
		public unsafe IntPtr this[int y, int x]
		{
			get
			{
				byte* addr = (byte*)this.Address.ToPointer();
				return new IntPtr(addr + (y * this.Stride) + (x * this.Model.Size));
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
		public bool Equals(TxScanner2D other)
		{
			if (this.Address != other.Address) return false;
			if (this.Width != other.Width) return false;
			if (this.Height != other.Height) return false;
			if (this.Model != other.Model) return false;
			if (this.Stride != other.Stride) return false;
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
			if (!(obj is TxScanner2D)) return false;
			return this.Equals((TxScanner2D)obj);
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
		public static bool operator ==(TxScanner2D left, TxScanner2D right)
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
		public static bool operator !=(TxScanner2D left, TxScanner2D right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region 要素のコピー:

		/// <summary>
		/// 要素のコピー
		/// </summary>
		/// <param name="src">複製元</param>
		public void Copy(object src)
		{
			if (src is TxScanner2D)
			{
				using (var _dst = CxImage.FromTag((TxImage)this))
				using (var _src = CxImage.FromTag((TxImage)(TxScanner2D)src))
				{
					_dst.Filter().Copy(_src);
				}
				return;
			}
			if (src is IEnumerable)
			{
				using (var tmp = CxArray.From((IEnumerable)src))
				{
					if (tmp.Length != (this.Width * this.Height))
						throw new CxException(ExStatus.InvalidParam, "src length does not match (width * height).");
					using (var _src = CxImage.FromTag(new TxImage(tmp.Address(), this.Width, this.Height, tmp.Model, this.Width * tmp.Model.Size, 0)))
					using (var _dst = CxImage.FromTag((TxImage)this))
					{
						_dst.Filter().Copy(_src);
					}
				}
				return;
			}
			throw new CxException(ExStatus.Unsupported);
		}

		#endregion

		#region メソッド: (ForEach)

		/// <summary>
		/// 各要素に対する実行
		/// </summary>
		/// <param name="func">関数 (int y, int x, IntPtr addr1)</param>
		public unsafe void ForEach(Scan2D1 func)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			TxScanner src1_scan = new TxScanner(this.Address, this.Model.Size, this.Stride);
			int height = this.Height;
			int width = this.Width;
			for (int y = 0; y < height; y++)
			{
				byte* src1_adrs = src1_scan.Address;
				for (int x = 0; x < width; x++)
				{
					func(y, x, (IntPtr)src1_adrs);
					src1_adrs += src1_scan.ModelSize;
				}
				src1_scan.Address += src1_scan.Stride;
			}
		}

		/// <summary>
		/// 各要素に対する実行
		/// </summary>
		/// <param name="obj2">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="func">関数 (int y, int x, IntPtr addr1, IntPtr addr2)</param>
		public unsafe void ForEach(TxScanner2D obj2, Scan2D2 func)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj2.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj2.Width || this.Height != obj2.Height)
				throw new CxException(ExStatus.InvalidObject);
			TxScanner src1_scan = new TxScanner(this.Address, this.Model.Size, this.Stride);
			TxScanner src2_scan = new TxScanner(obj2.Address, obj2.Model.Size, obj2.Stride);
			int height = this.Height;
			int width = this.Width;
			for (int y = 0; y < height; y++)
			{
				byte* src1_adrs = src1_scan.Address;
				byte* src2_adrs = src2_scan.Address;
				for (int x = 0; x < width; x++)
				{
					func(y, x, (IntPtr)src1_adrs, (IntPtr)src2_adrs);
					src1_adrs += src1_scan.ModelSize;
					src2_adrs += src2_scan.ModelSize;
				}
				src1_scan.Address += src1_scan.Stride;
				src2_scan.Address += src2_scan.Stride;
			}
		}

		/// <summary>
		/// 各要素に対する実行
		/// </summary>
		/// <param name="obj2">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="obj3">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="func">関数 (int y, int x, IntPtr addr1, IntPtr addr2, IntPtr addr3)</param>
		public unsafe void ForEach(TxScanner2D obj2, TxScanner2D obj3, Scan2D3 func)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj2.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj3.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj2.Width || this.Height != obj2.Height)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj3.Width || this.Height != obj3.Height)
				throw new CxException(ExStatus.InvalidObject);
			TxScanner src1_scan = new TxScanner(this.Address, this.Model.Size, this.Stride);
			TxScanner src2_scan = new TxScanner(obj2.Address, obj2.Model.Size, obj2.Stride);
			TxScanner src3_scan = new TxScanner(obj3.Address, obj3.Model.Size, obj3.Stride);
			int height = this.Height;
			int width = this.Width;
			for (int y = 0; y < height; y++)
			{
				byte* src1_adrs = src1_scan.Address;
				byte* src2_adrs = src2_scan.Address;
				byte* src3_adrs = src3_scan.Address;
				for (int x = 0; x < width; x++)
				{
					func(y, x, (IntPtr)src1_adrs, (IntPtr)src2_adrs, (IntPtr)src3_adrs);
					src1_adrs += src1_scan.ModelSize;
					src2_adrs += src2_scan.ModelSize;
					src3_adrs += src3_scan.ModelSize;
				}
				src1_scan.Address += src1_scan.Stride;
				src2_scan.Address += src2_scan.Stride;
				src3_scan.Address += src3_scan.Stride;
			}
		}

		/// <summary>
		/// 各要素に対する実行
		/// </summary>
		/// <param name="obj2">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="obj3">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="obj4">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="func">関数 (int y, int x, IntPtr addr1, IntPtr addr2, IntPtr addr3, IntPtr addr4)</param>
		public unsafe void ForEach(TxScanner2D obj2, TxScanner2D obj3, TxScanner2D obj4, Scan2D4 func)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj2.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj3.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj4.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj2.Width || this.Height != obj2.Height)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj3.Width || this.Height != obj3.Height)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj4.Width || this.Height != obj4.Height)
				throw new CxException(ExStatus.InvalidObject);
			TxScanner src1_scan = new TxScanner(this.Address, this.Model.Size, this.Stride);
			TxScanner src2_scan = new TxScanner(obj2.Address, obj2.Model.Size, obj2.Stride);
			TxScanner src3_scan = new TxScanner(obj3.Address, obj3.Model.Size, obj3.Stride);
			TxScanner src4_scan = new TxScanner(obj4.Address, obj4.Model.Size, obj4.Stride);
			int height = this.Height;
			int width = this.Width;
			for (int y = 0; y < height; y++)
			{
				byte* src1_adrs = src1_scan.Address;
				byte* src2_adrs = src2_scan.Address;
				byte* src3_adrs = src3_scan.Address;
				byte* src4_adrs = src4_scan.Address;
				for (int x = 0; x < width; x++)
				{
					func(y, x, (IntPtr)src1_adrs, (IntPtr)src2_adrs, (IntPtr)src3_adrs, (IntPtr)src4_adrs);
					src1_adrs += src1_scan.ModelSize;
					src2_adrs += src2_scan.ModelSize;
					src3_adrs += src3_scan.ModelSize;
					src4_adrs += src4_scan.ModelSize;
				}
				src1_scan.Address += src1_scan.Stride;
				src2_scan.Address += src2_scan.Stride;
				src3_scan.Address += src3_scan.Stride;
				src4_scan.Address += src4_scan.Stride;
			}
		}

		/// <summary>
		/// 各要素に対する実行
		/// </summary>
		/// <param name="obj2">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="obj3">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="obj4">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="obj5">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="func">関数 (int y, int x, IntPtr addr1, IntPtr addr2, IntPtr addr3, IntPtr addr4, IntPtr addr5)</param>
		public unsafe void ForEach(TxScanner2D obj2, TxScanner2D obj3, TxScanner2D obj4, TxScanner2D obj5, Scan2D5 func)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj2.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj3.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj4.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj5.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj2.Width || this.Height != obj2.Height)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj3.Width || this.Height != obj3.Height)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj4.Width || this.Height != obj4.Height)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj5.Width || this.Height != obj5.Height)
				throw new CxException(ExStatus.InvalidObject);
			TxScanner src1_scan = new TxScanner(this.Address, this.Model.Size, this.Stride);
			TxScanner src2_scan = new TxScanner(obj2.Address, obj2.Model.Size, obj2.Stride);
			TxScanner src3_scan = new TxScanner(obj3.Address, obj3.Model.Size, obj3.Stride);
			TxScanner src4_scan = new TxScanner(obj4.Address, obj4.Model.Size, obj4.Stride);
			TxScanner src5_scan = new TxScanner(obj5.Address, obj5.Model.Size, obj5.Stride);
			int height = this.Height;
			int width = this.Width;
			for (int y = 0; y < height; y++)
			{
				byte* src1_adrs = src1_scan.Address;
				byte* src2_adrs = src2_scan.Address;
				byte* src3_adrs = src3_scan.Address;
				byte* src4_adrs = src4_scan.Address;
				byte* src5_adrs = src5_scan.Address;
				for (int x = 0; x < width; x++)
				{
					func(y, x, (IntPtr)src1_adrs, (IntPtr)src2_adrs, (IntPtr)src3_adrs, (IntPtr)src4_adrs, (IntPtr)src5_adrs);
					src1_adrs += src1_scan.ModelSize;
					src2_adrs += src2_scan.ModelSize;
					src3_adrs += src3_scan.ModelSize;
					src4_adrs += src4_scan.ModelSize;
					src5_adrs += src5_scan.ModelSize;
				}
				src1_scan.Address += src1_scan.Stride;
				src2_scan.Address += src2_scan.Stride;
				src3_scan.Address += src3_scan.Stride;
				src4_scan.Address += src4_scan.Stride;
				src5_scan.Address += src5_scan.Stride;
			}
		}

		/// <summary>
		/// 各要素に対する実行
		/// </summary>
		/// <param name="obj2">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="obj3">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="obj4">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="obj5">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="obj6">同時に走査するオブジェクト (自身の Size と一致している必要があります。)</param>
		/// <param name="func">関数 (int y, int x, IntPtr addr1, IntPtr addr2, IntPtr addr3, IntPtr addr4, IntPtr addr5, IntPtr addr6)</param>
		public unsafe void ForEach(TxScanner2D obj2, TxScanner2D obj3, TxScanner2D obj4, TxScanner2D obj5, TxScanner2D obj6, Scan2D6 func)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj2.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj3.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj4.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj5.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj6.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj2.Width || this.Height != obj2.Height)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj3.Width || this.Height != obj3.Height)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj4.Width || this.Height != obj4.Height)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj5.Width || this.Height != obj5.Height)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Width != obj6.Width || this.Height != obj6.Height)
				throw new CxException(ExStatus.InvalidObject);
			TxScanner src1_scan = new TxScanner(this.Address, this.Model.Size, this.Stride);
			TxScanner src2_scan = new TxScanner(obj2.Address, obj2.Model.Size, obj2.Stride);
			TxScanner src3_scan = new TxScanner(obj3.Address, obj3.Model.Size, obj3.Stride);
			TxScanner src4_scan = new TxScanner(obj4.Address, obj4.Model.Size, obj4.Stride);
			TxScanner src5_scan = new TxScanner(obj5.Address, obj5.Model.Size, obj5.Stride);
			TxScanner src6_scan = new TxScanner(obj6.Address, obj6.Model.Size, obj6.Stride);
			int height = this.Height;
			int width = this.Width;
			for (int y = 0; y < height; y++)
			{
				byte* src1_adrs = src1_scan.Address;
				byte* src2_adrs = src2_scan.Address;
				byte* src3_adrs = src3_scan.Address;
				byte* src4_adrs = src4_scan.Address;
				byte* src5_adrs = src5_scan.Address;
				byte* src6_adrs = src6_scan.Address;
				for (int x = 0; x < width; x++)
				{
					func(y, x, (IntPtr)src1_adrs, (IntPtr)src2_adrs, (IntPtr)src3_adrs, (IntPtr)src4_adrs, (IntPtr)src5_adrs, (IntPtr)src6_adrs);
					src1_adrs += src1_scan.ModelSize;
					src2_adrs += src2_scan.ModelSize;
					src3_adrs += src3_scan.ModelSize;
					src4_adrs += src4_scan.ModelSize;
					src5_adrs += src5_scan.ModelSize;
					src6_adrs += src6_scan.ModelSize;
				}
				src1_scan.Address += src1_scan.Stride;
				src2_scan.Address += src2_scan.Stride;
				src3_scan.Address += src3_scan.Stride;
				src4_scan.Address += src4_scan.Stride;
				src5_scan.Address += src5_scan.Stride;
				src6_scan.Address += src6_scan.Stride;
			}
		}

		#endregion
	}
}
