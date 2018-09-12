/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace XIE.Ptr
{
	using ITEM_TYPE = TxCircleD;

	/// <summary>
	/// TxCircleD 構造体のポインタ
	/// </summary>
	public struct TxCircleDPtr : IxPointer<ITEM_TYPE>, IxEquatable
	{
		/// <summary>
		/// アンマネージリソース (配列の先頭アドレス)
		/// </summary>
		private IntPtr m_Address;

		/// <summary>
		/// 配列要素１つ分のサイズ (byte)
		/// </summary>
		/// <returns>
		///		配列要素１つ分のバイトサイズを返します。
		/// </returns>
		public static int SizeOfItem
		{
			get { return Marshal.SizeOf(typeof(ITEM_TYPE)); }
		}

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ptr">配列の先頭アドレス</param>
		public TxCircleDPtr(Int32 ptr)
		{
			m_Address = new IntPtr(ptr);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ptr">配列の先頭アドレス</param>
		public TxCircleDPtr(Int64 ptr)
		{
			m_Address = new IntPtr(ptr);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ptr">配列の先頭アドレス</param>
		public TxCircleDPtr(IntPtr ptr)
		{
			m_Address = ptr;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ptr">配列の先頭アドレス</param>
		public unsafe TxCircleDPtr(void* ptr)
		{
			m_Address = new IntPtr(ptr);
		}

		#endregion

		#region 継承メンバ

		/// <summary>
		/// 比較メソッド (等価)
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		等価の場合 true、不等価の場合 false を返します。
		///	</returns>
		public override bool Equals (object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (!(src is TxCircleDPtr)) return false;
			return (this.m_Address == ((TxCircleDPtr)src).m_Address);
		}

		/// <summary>
		/// ハッシュコードの取得
		/// </summary>
		/// <returns>
		///		常に 0 を返します。
		///	</returns>
		public override int GetHashCode()
		{
			return 0;
		}

		#endregion

		#region 比較オペレータ

		/// <summary>
		/// 比較オペレータ (等価)
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		一致する場合は true、一致しない場合は false を返します。
		/// </returns>
		public static bool operator ==(TxCircleDPtr ope1, TxCircleDPtr ope2)
		{
			if (ReferenceEquals(ope1, null))
				return ReferenceEquals(ope2, null);
			else
				return ope1.Equals(ope2);
		}

		/// <summary>
		/// 比較オペレータ (不等価)
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		一致しない場合は true、一致する場合は false を返します。
		/// </returns>
		public static bool operator !=(TxCircleDPtr ope1, TxCircleDPtr ope2)
		{
			return !(ope1 == ope2);
		}

		#endregion

		#region 比較オペレータ.(暗黙的)

		/// <summary>
		/// 比較オペレータ (等価)
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		一致する場合は true、一致しない場合は false を返します。
		/// </returns>
		public static bool operator ==(TxCircleDPtr ope1, System.IntPtr ope2)
		{
			return (ope1.m_Address == ope2);
		}

		/// <summary>
		/// 比較オペレータ (不等価)
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		一致しない場合は true、一致する場合は false を返します。
		/// </returns>
		public static bool operator !=(TxCircleDPtr ope1, System.IntPtr ope2)
		{
			return (ope1.m_Address != ope2);
		}

		/// <summary>
		/// 比較オペレータ (等価)
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		一致する場合は true、一致しない場合は false を返します。
		/// </returns>
		public static bool operator ==(System.IntPtr ope1, TxCircleDPtr ope2)
		{
			return (ope1 == ope2.m_Address);
		}

		/// <summary>
		/// 比較オペレータ (不等価)
		/// </summary>
		/// <param name="ope1">左辺値</param>
		/// <param name="ope2">右辺値</param>
		/// <returns>
		///		一致しない場合は true、一致する場合は false を返します。
		/// </returns>
		public static bool operator !=(System.IntPtr ope1, TxCircleDPtr ope2)
		{
			return (ope1 != ope2.m_Address);
		}

		#endregion

		#region 型変換

		/// <summary>
		/// 型変換
		/// </summary>
		/// <returns>
		///		このオブジェクトの値を Int32 に変換して返します。
		/// </returns>
		public Int32 ToInt32()
		{
			return m_Address.ToInt32();
		}

		/// <summary>
		/// 型変換
		/// </summary>
		/// <returns>
		///		このオブジェクトの値を Int64 に変換して返します。
		/// </returns>
		public Int64 ToInt64()
		{
			return m_Address.ToInt64();
		}

		/// <summary>
		/// 型変換
		/// </summary>
		/// <returns>
		///		このオブジェクトの値を void* に変換して返します。
		/// </returns>
		public unsafe void* ToPointer()
		{
			return m_Address.ToPointer();
		}

		/// <summary>
		/// 型変換
		/// </summary>
		/// <returns>
		///		このオブジェクトの値を文字列に変換して返します。
		/// </returns>
		public override string ToString()
		{
			return m_Address.ToString();
		}

		/// <summary>
		/// 型変換
		/// </summary>
		/// <returns>
		///		このオブジェクトの値を文字列に変換して返します。
		/// </returns>
		public string ToString(string format)
		{
			return m_Address.ToString(format);
		}

		#endregion

		#region 明示的な型変換演算子 (Any←／→TxCircleDPtr)

		/// <summary>
		/// 明示的な型変換演算子 (Int32←TxCircleDPtr)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		Int32 に変換して返します。
		///	</returns>
		public static explicit operator Int32(TxCircleDPtr src)
		{
			return src.m_Address.ToInt32();
		}

		/// <summary>
		/// 明示的な型変換演算子 (TxCircleDPtr←Int32)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		TxCircleDPtr 構造体に代入して返します。
		///	</returns>
		public static explicit operator TxCircleDPtr(Int32 src)
		{
			return new TxCircleDPtr(src);
		}

		/// <summary>
		/// 明示的な型変換演算子 (Int64←TxCircleDPtr)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		Int64 に変換して返します。
		///	</returns>
		public static explicit operator Int64(TxCircleDPtr src)
		{
			return src.m_Address.ToInt64();
		}

		/// <summary>
		/// 明示的な型変換演算子 (TxCircleDPtr←Int64)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		TxCircleDPtr 構造体に代入して返します。
		///	</returns>
		public static explicit operator TxCircleDPtr(Int64 src)
		{
			return new TxCircleDPtr(src);
		}

		/// <summary>
		/// 明示的な型変換演算子 (void*←TxCircleDPtr)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		void* に変換して返します。
		///	</returns>
		public static unsafe explicit operator void*(TxCircleDPtr src)
		{
			return src.m_Address.ToPointer();
		}

		/// <summary>
		/// 明示的な型変換演算子 (TxCircleDPtr←void*)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		TxCircleDPtr 構造体に代入して返します。
		///	</returns>
		public static unsafe explicit operator TxCircleDPtr(void* src)
		{
			return new TxCircleDPtr(src);
		}

		#endregion

		#region 暗黙的な型変換演算子 (IntPtr←／→TxCircleDPtr)

		/// <summary>
		/// 暗黙的な型変換演算子 (IntPtr←TxCircleDPtr)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		IntPtr に変換して返します。
		///	</returns>
		public static implicit operator IntPtr(TxCircleDPtr src)
		{
			return src.m_Address;
		}

		/// <summary>
		/// 暗黙的な型変換演算子 (TxCircleDPtr←IntPtr)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		TxCircleDPtr 構造体に代入して返します。
		///	</returns>
		public static implicit operator TxCircleDPtr(IntPtr src)
		{
			return new TxCircleDPtr(src);
		}

		#endregion

		#region インクリメント.

		/// <summary>
		/// インクリメント演算子
		/// </summary>
		/// <param name="src">対象</param>
		/// <param name="value">加算する値</param>
		/// <returns>
		///		指定された値をアドレスに加算して返します。
		/// </returns>
		public static unsafe TxCircleDPtr operator +(TxCircleDPtr src, Int32 value)
		{
			return new TxCircleDPtr(((ITEM_TYPE*)src.m_Address) + value);
		}

		/// <summary>
		/// インクリメント演算子
		/// </summary>
		/// <param name="src">対象</param>
		/// <param name="value">加算する値</param>
		/// <returns>
		///		指定された値をアドレスに加算して返します。
		/// </returns>
		public static unsafe TxCircleDPtr operator +(TxCircleDPtr src, UInt32 value)
		{
			return new TxCircleDPtr(((ITEM_TYPE*)src.m_Address) + value);
		}

		/// <summary>
		/// インクリメント演算子
		/// </summary>
		/// <param name="src">対象</param>
		/// <param name="value">加算する値</param>
		/// <returns>
		///		指定された値をアドレスに加算して返します。
		/// </returns>
		public static unsafe TxCircleDPtr operator +(TxCircleDPtr src, Int64 value)
		{
			return new TxCircleDPtr(((ITEM_TYPE*)src.m_Address) + value);
		}

		/// <summary>
		/// インクリメント演算子
		/// </summary>
		/// <param name="src">対象</param>
		/// <param name="value">加算する値</param>
		/// <returns>
		///		指定された値をアドレスに加算して返します。
		/// </returns>
		public static unsafe TxCircleDPtr operator +(TxCircleDPtr src, UInt64 value)
		{
			return new TxCircleDPtr(((ITEM_TYPE*)src.m_Address) + value);
		}

		/// <summary>
		/// インクリメント演算子
		/// </summary>
		/// <param name="src">対象</param>
		/// <param name="value">加算する値</param>
		/// <returns>
		///		指定された値をアドレスに加算して返します。
		/// </returns>
		public static unsafe TxCircleDPtr operator +(TxCircleDPtr src, SIZE_T value)
		{
			return new TxCircleDPtr(((ITEM_TYPE*)src.m_Address) + value);
		}

		#endregion

		#region デクリメント.

		/// <summary>
		/// デクリメント演算子
		/// </summary>
		/// <param name="src">対象</param>
		/// <param name="value">加算する値</param>
		/// <returns>
		///		指定された値をアドレスに加算して返します。
		/// </returns>
		public static unsafe TxCircleDPtr operator -(TxCircleDPtr src, Int32 value)
		{
			return new TxCircleDPtr(((ITEM_TYPE*)src.m_Address) - value);
		}

		/// <summary>
		/// デクリメント演算子
		/// </summary>
		/// <param name="src">対象</param>
		/// <param name="value">加算する値</param>
		/// <returns>
		///		指定された値をアドレスに加算して返します。
		/// </returns>
		public static unsafe TxCircleDPtr operator -(TxCircleDPtr src, UInt32 value)
		{
			return new TxCircleDPtr(((ITEM_TYPE*)src.m_Address) - value);
		}

		/// <summary>
		/// デクリメント演算子
		/// </summary>
		/// <param name="src">対象</param>
		/// <param name="value">加算する値</param>
		/// <returns>
		///		指定された値をアドレスに加算して返します。
		/// </returns>
		public static unsafe TxCircleDPtr operator -(TxCircleDPtr src, Int64 value)
		{
			return new TxCircleDPtr(((ITEM_TYPE*)src.m_Address) - value);
		}

		/// <summary>
		/// デクリメント演算子
		/// </summary>
		/// <param name="src">対象</param>
		/// <param name="value">加算する値</param>
		/// <returns>
		///		指定された値をアドレスに加算して返します。
		/// </returns>
		public static unsafe TxCircleDPtr operator -(TxCircleDPtr src, UInt64 value)
		{
			return new TxCircleDPtr(((ITEM_TYPE*)src.m_Address) - value);
		}

		/// <summary>
		/// デクリメント演算子
		/// </summary>
		/// <param name="src">対象</param>
		/// <param name="value">加算する値</param>
		/// <returns>
		///		指定された値をアドレスに加算して返します。
		/// </returns>
		public static unsafe TxCircleDPtr operator -(TxCircleDPtr src, SIZE_T value)
		{
			return new TxCircleDPtr(((ITEM_TYPE*)src.m_Address) - value);
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		void IxEquatable.CopyFrom(object src)
		{
			this.CopyFrom((IEnumerable<ITEM_TYPE>)src);
			return;
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		bool IxEquatable.ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;
			return (this.Address == ((IxPointer<ITEM_TYPE>)src).Address);
		}

		#endregion

		#region アンマネージリソースの操作 (2)

		/// <summary>
		/// アンマネージリソースの確保
		/// </summary>
		/// <param name="num">要素数 [1~]</param>
		/// <returns>
		///		確保された配列の先頭アドレスを返します。
		/// </returns>
		public static IntPtr Allocate(int num)
		{
			int size = Marshal.SizeOf(typeof(ITEM_TYPE));
			IntPtr ptr = Axi.MemoryAlloc(size * num);
			return ptr;
		}

		/// <summary>
		/// IxPointer の実装: アンマネージリソースの確保
		/// </summary>
		/// <param name="num">要素数 [1~]</param>
		/// <returns>
		///		確保された配列の先頭アドレスを返します。
		/// </returns>
		IntPtr XIE.IxPointer<ITEM_TYPE>.Allocate(int num)
		{
			return Allocate(num);
		}

		/// <summary>
		/// 配列の先頭アドレス
		/// </summary>
		/// <returns>
		///		配列の先頭アドレスを返します。
		/// </returns>
		public IntPtr Address
		{
			get { return this.m_Address; }
			set { this.m_Address = value; }
		}

		/// <summary>
		/// 配列要素の値
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		/// <returns>
		///		指定された要素を返します。
		/// </returns>
		public unsafe ITEM_TYPE this[int index]
		{
			get { return ((ITEM_TYPE*)m_Address)[index]; }
			set { ((ITEM_TYPE*)m_Address)[index] = value; }
		}

		/// <summary>
		/// マネージ配列の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public void CopyFrom(IEnumerable<ITEM_TYPE> src)
		{
			if (this.m_Address == IntPtr.Zero)
				throw new System.AccessViolationException("Address is null");
			int index = 0;
			foreach (var item in src)
				this[index++] = item;
		}

		/// <summary>
		/// マネージ配列への変換
		/// </summary>
		/// <param name="length">要素数 [0~]</param>
		/// <returns>
		///		各要素を格納したマネージ配列を返します。
		/// </returns>
		public ITEM_TYPE[] ToArray(int length)
		{
			if (this.m_Address == IntPtr.Zero)
				throw new System.AccessViolationException("Address is null");
			ITEM_TYPE[] dst = new ITEM_TYPE[length];
			for (int i = 0; i < length; i++)
				dst[i] = this[i];
			return dst;
		}

		/// <summary>
		/// 配列要素の参照
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		/// <returns>
		///		指定された位置のアドレスを自身と同じクラスに格納して返します。
		/// </returns>
		public unsafe TxCircleDPtr At(int index)
		{
			return new TxCircleDPtr(((ITEM_TYPE*)m_Address) + index);
		}

		/// <summary>
		/// 配列要素のアドレス
		/// </summary>
		/// <param name="index">配列指標 [0~]</param>
		/// <returns>
		///		指定された位置のアドレスを返します。
		/// </returns>
		public unsafe ITEM_TYPE* Pt(int index)
		{
			return ((ITEM_TYPE*)m_Address) + index;
		}

		#endregion
	}
}
