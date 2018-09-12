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

namespace XIE
{
	/// <summary>
	/// XIE モジュールハンドル
	/// </summary>
	public struct HxModule
	{
		/// <summary>
		/// アンマネージリソース (ネイティブ C++ クラスの this ポインタ)
		/// </summary>
		private IntPtr m_Address;

		/// <summary>
		/// 無効なアドレス(NULL)を示す定数
		/// </summary>
		public static readonly HxModule Zero = IntPtr.Zero;

		/// <summary>
		/// ポインタのサイズ
		/// </summary>
		/// <returns>
		///		動作環境が x86 の場合 4 を返します。<br/>
		///		動作環境が x64 の場合 8 を返します。<br/>
		/// </returns>
		public static int Size
		{
			get { return IntPtr.Size; }
		}

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ptr">XIE モジュールのハンドル</param>
		public HxModule(Int32 ptr)
		{
			m_Address = new IntPtr(ptr);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ptr">XIE モジュールのハンドル</param>
		public HxModule(Int64 ptr)
		{
			m_Address = new IntPtr(ptr);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ptr">XIE モジュールのハンドル</param>
		public HxModule(IntPtr ptr)
		{
			m_Address = ptr;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="ptr">XIE モジュールのハンドル</param>
		public unsafe HxModule(void* ptr)
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
			if (!(src is HxModule)) return false;
			return (this.m_Address == ((HxModule)src).m_Address);
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
		public static bool operator ==(HxModule ope1, HxModule ope2)
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
		public static bool operator !=(HxModule ope1, HxModule ope2)
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
		public static bool operator ==(HxModule ope1, System.IntPtr ope2)
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
		public static bool operator !=(HxModule ope1, System.IntPtr ope2)
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
		public static bool operator ==(System.IntPtr ope1, HxModule ope2)
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
		public static bool operator !=(System.IntPtr ope1, HxModule ope2)
		{
			return (ope1 != ope2.m_Address);
		}

		#endregion

		#region 型変換メソッド:

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

		#region 型変換演算子:

		/// <summary>
		/// 型変換演算子 (IntPtr←HxModule)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		IntPtr に変換して返します。
		///	</returns>
		public static explicit operator IntPtr(HxModule src)
		{
			return src.m_Address;
		}

		/// <summary>
		/// 型変換演算子 (HxModule←IntPtr) (暗黙的)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		HxModule 構造体に代入して返します。
		///	</returns>
		public static implicit operator HxModule(IntPtr src)
		{
			return new HxModule(src);
		}

		/// <summary>
		/// 型変換演算子 (Int32←HxModule)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		Int32 に変換して返します。
		///	</returns>
		public static explicit operator Int32(HxModule src)
		{
			return src.m_Address.ToInt32();
		}

		/// <summary>
		/// 型変換演算子 (HxModule←Int32)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		HxModule 構造体に代入して返します。
		///	</returns>
		public static explicit operator HxModule(Int32 src)
		{
			return new HxModule(src);
		}

		/// <summary>
		/// 型変換演算子 (Int64←HxModule)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		Int64 に変換して返します。
		///	</returns>
		public static explicit operator Int64(HxModule src)
		{
			return src.m_Address.ToInt64();
		}

		/// <summary>
		/// 型変換演算子 (HxModule←Int64)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		HxModule 構造体に代入して返します。
		///	</returns>
		public static explicit operator HxModule(Int64 src)
		{
			return new HxModule(src);
		}

		/// <summary>
		/// 型変換演算子 (void*←HxModule)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		void* に変換して返します。
		///	</returns>
		public static unsafe explicit operator void*(HxModule src)
		{
			return src.m_Address.ToPointer();
		}

		/// <summary>
		/// 型変換演算子 (HxModule←void*)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		HxModule 構造体に代入して返します。
		///	</returns>
		public static unsafe explicit operator HxModule(void* src)
		{
			return new HxModule(src);
		}

		#endregion

		#region HxModule

		/// <summary>
		/// オブジェクトの破棄。
		/// </summary>
		/// <exception cref="T:XIE.CxException"/>
		public void Destroy()
		{
			if (m_Address == IntPtr.Zero) return;
			ExStatus status = xie_core.fnXIE_Core_Module_Destroy(this);
			if (status != ExStatus.Success)
				throw new CxException(status);
			m_Address = IntPtr.Zero;
		}

		/// <summary>
		/// オブジェクトのデータ領域の解放。(※ オブジェクト自体は解放されません。)
		/// </summary>
		/// <exception cref="T:XIE.CxException"/>
		public void Dispose()
		{
			if (m_Address == IntPtr.Zero) return;
			ExStatus status = xie_core.fnXIE_Core_Module_Dispose(this);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// オブジェクトの有効性の検査
		/// </summary>
		/// <returns>
		///		指定のオブジェクトのデータ領域が有効か否かを検査します。
		///		有効の場合は true を、無効の場合は false を返します。
		/// </returns>
		public bool IsValid()
		{
			return xie_core.fnXIE_Core_Module_IsValid(this) == ExBoolean.True;
		}

		#endregion

		#region IxTagPtr

		/// <summary>
		/// オブジェクトの情報構造体へのポインタの取得
		/// </summary>
		/// <returns>
		///		指定のオブジェクトのデータ領域の情報が格納された構造体へのポインタを返します。
		///		このオブジェクトが解放されるか、データ領域が解放された場合は無効になります。
		/// </returns>
		/// <seealso cref="M:XIE.Axi.Destroy(XIE.HxModule)"/>
		/// <seealso cref="M:XIE.Axi.Dispose(XIE.HxModule)"/>
		public IntPtr TagPtr()
		{
			return xie_core.fnXIE_Core_TagPtr(this);
		}

		#endregion

		#region IxEquatable

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		/// <returns>
		///		オブジェクトの内容を複製します。
		///		型の異なるオブジェクトには対応していません。
		///		異常があれば例外を発行します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public void CopyFrom(HxModule src)
		{
			ExStatus status = xie_core.fnXIE_Core_Equatable_CopyFrom(this, src);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// オブジェクトの比較
		/// </summary>
		/// <param name="cmp">比較対象</param>
		/// <returns>
		///		オブジェクトの内容を比較します。
		///		型の異なるオブジェクトには対応していません。
		///		一致する場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public bool ContentEquals(HxModule cmp)
		{
			return xie_core.fnXIE_Core_Equatable_ContentEquals(this, cmp) == ExBoolean.True;
		}

		#endregion

		#region IxAttachable

		/// <summary>
		/// オブジェクトのアタッチ
		/// </summary>
		/// <param name="src">アタッチ先</param>
		/// <returns>
		///		外部リソースにアタッチします。
		///		異常があれば例外を発行します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public void Attach(HxModule src)
		{
			ExStatus status = xie_core.fnXIE_Core_Attachable_Attach(this, src);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// オブジェクトのアタッチ状態の検査
		/// </summary>
		/// <returns>
		///		指定のオブジェクトが他のオブジェクトにアタッチしているか否かを検査します。
		///		アタッチしている場合は true を、アタッチしていない場合は false を返します。
		/// </returns>
		public bool IsAttached()
		{
			return xie_core.fnXIE_Core_Attachable_IsAttached(this) == ExBoolean.True;
		}

		#endregion

		#region IxLockable

		/// <summary>
		/// メモリのロック
		/// </summary>
		/// <returns>
		///		メモリをロックします。
		///		異常があれば例外を発行します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public void Lock()
		{
			ExStatus status = xie_core.fnXIE_Core_Lockable_Lock(this);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// メモリのロック解除
		/// </summary>
		/// <returns>
		///		メモリをロック解除します。
		///		異常があれば例外を発行します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public void Unlock()
		{
			ExStatus status = xie_core.fnXIE_Core_Lockable_Unlock(this);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// メモリのロック状態の検査
		/// </summary>
		/// <returns>
		///		指定のオブジェクトがメモリをロックしているか否かを検査します。
		///		ロックしている場合は true を、ロックしていない場合は false を返します。
		/// </returns>
		public bool IsLocked()
		{
			return xie_core.fnXIE_Core_Lockable_IsLocked(this) == ExBoolean.True;
		}

		#endregion
	}
}
