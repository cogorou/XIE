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
	/// １次元配列走査構造体
	/// </summary>
	public unsafe struct TxScanner1D :
		IEquatable<TxScanner1D>
	{
		#region プロパティ:

		/// <summary>
		/// 領域の先頭アドレス
		/// </summary>
		public IntPtr Address;

		/// <summary>
		/// 要素数
		/// </summary>
		public int Length;

		/// <summary>
		/// 要素モデル
		/// </summary>
		public TxModel Model;

		/// <summary>
		/// 有効性の検査
		/// </summary>
		/// <returns>
		///		現在設定されている領域の情報が有効な場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public bool IsValid
		{
			get
			{
				if (this.Address == IntPtr.Zero) return false;
				if (this.Length <= 0) return false;
				if (this.Model.Size <= 0) return false;
				return true;
			}
		}

		#endregion

		#region コンストラクタ: 

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="addr">配列の先頭アドレス</param>
		/// <param name="length">要素数 [0~]</param>
		/// <param name="model">要素モデル</param>
		public TxScanner1D(IntPtr addr, int length, TxModel model)
		{
			Address = addr;
			Length = length;
			Model = model;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="src">参照先</param>
		public TxScanner1D(TxArray src)
		{
			Address = src.Address;
			Length = src.Length;
			Model = src.Model;
		}

		#endregion

		#region インデクサ:

		/// <summary>
		/// 要素のアドレス
		/// </summary>
		/// <param name="index">配列指標</param>
		/// <returns>
		///		指定位置の要素のアドレスを返します。
		/// </returns>
		public unsafe IntPtr this[int index]
		{
			get
			{
				byte* addr = (byte*)this.Address.ToPointer();
				return new IntPtr(addr + (index * this.Model.Size));
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
		public bool Equals(TxScanner1D other)
		{
			if (this.Address != other.Address) return false;
			if (this.Length != other.Length) return false;
			if (this.Model != other.Model) return false;
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
			if (!(obj is TxScanner1D)) return false;
			return this.Equals((TxScanner1D)obj);
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
		public static bool operator ==(TxScanner1D left, TxScanner1D right)
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
		public static bool operator !=(TxScanner1D left, TxScanner1D right)
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
			if (src is TxScanner1D)
			{
				using (var _dst = CxArray.FromTag((TxArray)this))
				using (var _src = CxArray.FromTag((TxArray)(TxScanner1D)src))
				{
					_dst.Filter().Copy(_src);
				}
				return;
			}
			if (src is IEnumerable)
			{
				using (var _src = CxArray.From((IEnumerable)src))
				using (var _dst = CxArray.FromTag((TxArray)this))
				{
					if (_dst.Length != _src.Length)
						throw new CxException(ExStatus.InvalidParam, "Length does not match.");
					_dst.Filter().Copy(_src);
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
		/// <param name="func">関数 (int x, IntPtr addr1)</param>
		public void ForEach(Scan1D1 func)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			int length = this.Length;
			byte* obj1_adrs = (byte*)this.Address.ToPointer();
			for (int x = 0; x < length; x++)
			{
				func(x, (IntPtr)obj1_adrs);
				obj1_adrs += this.Model.Size;
			}
		}

		/// <summary>
		/// 各要素に対する実行
		/// </summary>
		/// <param name="obj2">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="func">関数 (int x, IntPtr addr1, IntPtr addr2)</param>
		public void ForEach(TxScanner1D obj2, Scan1D2 func)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj2.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj2.Length)
				throw new CxException(ExStatus.InvalidObject);
			int length = this.Length;
			byte* obj1_adrs = (byte*)this.Address.ToPointer();
			byte* obj2_adrs = (byte*)obj2.Address.ToPointer();
			for (int x = 0; x < length; x++)
			{
				func(x, (IntPtr)obj1_adrs, (IntPtr)obj2_adrs);
				obj1_adrs += this.Model.Size;
				obj2_adrs += obj2.Model.Size;
			}
		}

		/// <summary>
		/// 各要素に対する実行
		/// </summary>
		/// <param name="obj2">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="obj3">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="func">関数 (int x, IntPtr addr1, IntPtr addr2, IntPtr addr3)</param>
		public void ForEach(TxScanner1D obj2, TxScanner1D obj3, Scan1D3 func)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj2.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj3.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj2.Length)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj3.Length)
				throw new CxException(ExStatus.InvalidObject);
			int length = this.Length;
			byte* obj1_adrs = (byte*)this.Address.ToPointer();
			byte* obj2_adrs = (byte*)obj2.Address.ToPointer();
			byte* obj3_adrs = (byte*)obj3.Address.ToPointer();
			for (int x = 0; x < length; x++)
			{
				func(x, (IntPtr)obj1_adrs, (IntPtr)obj2_adrs, (IntPtr)obj3_adrs);
				obj1_adrs += this.Model.Size;
				obj2_adrs += obj2.Model.Size;
				obj3_adrs += obj3.Model.Size;
			}
		}

		/// <summary>
		/// 各要素に対する実行
		/// </summary>
		/// <param name="obj2">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="obj3">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="obj4">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="func">関数 (int x, IntPtr addr1, IntPtr addr2, IntPtr addr3, IntPtr addr4)</param>
		public void ForEach(TxScanner1D obj2, TxScanner1D obj3, TxScanner1D obj4, Scan1D4 func)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj2.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj3.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (obj4.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj2.Length)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj3.Length)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj4.Length)
				throw new CxException(ExStatus.InvalidObject);
			int length = this.Length;
			byte* obj1_adrs = (byte*)this.Address.ToPointer();
			byte* obj2_adrs = (byte*)obj2.Address.ToPointer();
			byte* obj3_adrs = (byte*)obj3.Address.ToPointer();
			byte* obj4_adrs = (byte*)obj4.Address.ToPointer();
			for (int x = 0; x < length; x++)
			{
				func(x, (IntPtr)obj1_adrs, (IntPtr)obj2_adrs, (IntPtr)obj3_adrs, (IntPtr)obj4_adrs);
				obj1_adrs += this.Model.Size;
				obj2_adrs += obj2.Model.Size;
				obj3_adrs += obj3.Model.Size;
				obj4_adrs += obj4.Model.Size;
			}
		}

		/// <summary>
		/// 各要素に対する実行
		/// </summary>
		/// <param name="obj2">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="obj3">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="obj4">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="obj5">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="func">関数 (int x, IntPtr addr1, IntPtr addr2, IntPtr addr3, IntPtr addr4, IntPtr addr5)</param>
		public void ForEach(TxScanner1D obj2, TxScanner1D obj3, TxScanner1D obj4, TxScanner1D obj5, Scan1D5 func)
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
			if (this.Length != obj2.Length)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj3.Length)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj4.Length)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj5.Length)
				throw new CxException(ExStatus.InvalidObject);
			int length = this.Length;
			byte* obj1_adrs = (byte*)this.Address.ToPointer();
			byte* obj2_adrs = (byte*)obj2.Address.ToPointer();
			byte* obj3_adrs = (byte*)obj3.Address.ToPointer();
			byte* obj4_adrs = (byte*)obj4.Address.ToPointer();
			byte* obj5_adrs = (byte*)obj5.Address.ToPointer();
			for (int x = 0; x < length; x++)
			{
				func(x, (IntPtr)obj1_adrs, (IntPtr)obj2_adrs, (IntPtr)obj3_adrs, (IntPtr)obj4_adrs, (IntPtr)obj5_adrs);
				obj1_adrs += this.Model.Size;
				obj2_adrs += obj2.Model.Size;
				obj3_adrs += obj3.Model.Size;
				obj4_adrs += obj4.Model.Size;
				obj5_adrs += obj5.Model.Size;
			}
		}

		/// <summary>
		/// 各要素に対する実行
		/// </summary>
		/// <param name="obj2">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="obj3">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="obj4">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="obj5">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="obj6">同時に走査するオブジェクト (自身の Length と一致している必要があります。)</param>
		/// <param name="func">関数 (int x, IntPtr addr1, IntPtr addr2, IntPtr addr3, IntPtr addr4, IntPtr addr5, IntPtr addr6)</param>
		public void ForEach(TxScanner1D obj2, TxScanner1D obj3, TxScanner1D obj4, TxScanner1D obj5, TxScanner1D obj6, Scan1D6 func)
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
			if (this.Length != obj2.Length)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj3.Length)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj4.Length)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj5.Length)
				throw new CxException(ExStatus.InvalidObject);
			if (this.Length != obj6.Length)
				throw new CxException(ExStatus.InvalidObject);
			int length = this.Length;
			byte* obj1_adrs = (byte*)this.Address.ToPointer();
			byte* obj2_adrs = (byte*)obj2.Address.ToPointer();
			byte* obj3_adrs = (byte*)obj3.Address.ToPointer();
			byte* obj4_adrs = (byte*)obj4.Address.ToPointer();
			byte* obj5_adrs = (byte*)obj5.Address.ToPointer();
			byte* obj6_adrs = (byte*)obj6.Address.ToPointer();
			for (int x = 0; x < length; x++)
			{
				func(x, (IntPtr)obj1_adrs, (IntPtr)obj2_adrs, (IntPtr)obj3_adrs, (IntPtr)obj4_adrs, (IntPtr)obj5_adrs, (IntPtr)obj6_adrs);
				obj1_adrs += this.Model.Size;
				obj2_adrs += obj2.Model.Size;
				obj3_adrs += obj3.Model.Size;
				obj4_adrs += obj4.Model.Size;
				obj5_adrs += obj5.Model.Size;
				obj6_adrs += obj6.Model.Size;
			}
		}

		#endregion
	}
}
