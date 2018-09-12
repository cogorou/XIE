/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using XIE.Ptr;

namespace XIE
{
	/// <summary>
	/// 文字列オブジェクトクラス (ASCII)
	/// </summary>
	public unsafe class CxStringA : System.Object
		, IxModule
		, IDisposable
		, ICloneable
		, IxEquatable
	{
		#region IxModule の実装: (ハンドル)

		[NonSerialized]
		private HxModule m_Handle = IntPtr.Zero;
		[NonSerialized]
		private bool m_IsDisposable = false;

		/// <summary>
		/// ハンドルの取得
		/// </summary>
		/// <returns>
		///		保有しているハンドルを返します。
		/// </returns>
		HxModule IxModule.GetHandle()
		{
			return m_Handle;
		}

		/// <summary>
		/// ハンドルの設定
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		void IxModule.SetHandle(HxModule handle, bool disposable)
		{
			m_Handle = handle;
			m_IsDisposable = disposable;

			if (m_Handle == IntPtr.Zero)
				m_Tag = null;
			else
				m_Tag = (TxStringA*)m_Handle.TagPtr().ToPointer();
		}

		/// <summary>
		/// 破棄
		/// </summary>
		void IxModule.Destroy()
		{
			if (m_IsDisposable)
				m_Handle.Destroy();
			m_Handle = IntPtr.Zero;
		}

		#endregion

		#region 構造体.

		/// <summary>
		/// 情報構造体へのポインタ
		/// </summary>
		[NonSerialized]
		private unsafe TxStringA* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxStringA Tag()
		{
			if (this.m_Tag == null)
				return new TxStringA();
			return *this.m_Tag;
		}

		#endregion

		#region 生成関数:

		/// <summary>
		/// 配列オブジェクトハンドルからの生成
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public unsafe static CxStringA FromHandle(HxModule handle, bool disposable)
		{
			return new CxStringA(handle, disposable);
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_core.fnXIE_Core_Module_Create("CxStringA");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ (ハンドル指定)
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		private CxStringA(HxModule handle, bool disposable)
		{
			((IxModule)this).SetHandle(handle, disposable);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxStringA()
		{
			_Constructor();
		}

		/// <summary>
		/// コンストラクタ (バイト配列からの変換)
		/// </summary>
		/// <param name="src">複製元</param>
		public CxStringA(byte[] src)
		{
			_Constructor();
			CopyFrom(src);
		}

		/// <summary>
		/// コンストラクタ (文字列からの変換)
		/// </summary>
		/// <param name="src">複製元</param>
		public CxStringA(string src)
		{
			_Constructor();
			CopyFrom(src, System.Text.Encoding.Default);
		}

		/// <summary>
		/// コンストラクタ (文字列からの変換)
		/// </summary>
		/// <param name="src">複製元</param>
		/// <param name="encoding">エンコード</param>
		public CxStringA(string src, System.Text.Encoding encoding)
		{
			_Constructor();
			CopyFrom(src, encoding);
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxStringA()
		{
			((IxModule)this).Destroy();
		}

		#endregion

		#region IDisposable の実装: (解放)

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			((IxModule)this).GetHandle().Dispose();
		}

		/// <summary>
		/// 有効性 [初期値:false]
		/// </summary>
		/// <returns>
		///		現在のオブジェクトの内部リソースが有効な場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool IsValid
		{
			get { return ((IxModule)this).GetHandle().IsValid(); }
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// オブジェクトのクローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			var clone = new CxStringA();
			clone.CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;
			if (ReferenceEquals(src, null)) return;
			if (src is IxModule)
			{
				((IxModule)this).GetHandle().CopyFrom(((IxModule)src).GetHandle());
				return;
			}
			if (src is string)
			{
				var _src = (string)src;
				this.CopyFrom(_src, System.Text.Encoding.Default);
				return;
			}
			if (src is IEnumerable<byte>)
			{
				var _src = (IEnumerable<byte>)src;
				this.Resize(_src.Count());
				this.Reset();
				var _dst = (byte*)this.Address();
				int index = 0;
				foreach (var item in _src)
				{
					_dst[index] = item;
					index++;
				}
				return;
			}
			if (src is IxConvertible)
			{
				((IxConvertible)src).CopyTo(this);
				return;
			}
			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public virtual bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			try
			{
				var _src = (CxStringA)src;
				return ((IxModule)this).GetHandle().ContentEquals(((IxModule)_src).GetHandle());
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 文字列の先頭アドレス
		/// </summary>
		/// <returns>
		///		文字列の先頭アドレスを返します。
		/// </returns>
		public virtual unsafe IntPtr Address()
		{
			if (this.m_Tag == null)
				return IntPtr.Zero;
			return this.m_Tag->Address;
		}

		/// <summary>
		/// 文字列長 [初期値:0、範囲:0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxStringA.Length")]
		public virtual unsafe int Length
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Length;
			}
		}

		#endregion

		#region メソッド: (Resize)

		/// <summary>
		/// 領域の確保
		/// </summary>
		/// <param name="length">要素数 [0~] ※要素数が 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		public virtual void Resize(int length)
		{
			if (((IxModule)this).GetHandle() == IntPtr.Zero)
				_Constructor();

			ExStatus status = xie_core.fnXIE_Core_String_Resize(((IxModule)this).GetHandle(), length);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// リセット (全ての要素を 0 初期化します。)
		/// </summary>
		public virtual void Reset()
		{
			ExStatus status = xie_core.fnXIE_Core_String_Reset(((IxModule)this).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (変換系)

		/// <summary>
		/// 文字列からの変換
		/// </summary>
		/// <param name="src">複製元</param>
		/// <param name="encoding">エンコード</param>
		public virtual void CopyFrom(string src, System.Text.Encoding encoding)
		{
			var _src_array = encoding.GetBytes(src);
			CopyFrom(_src_array);
		}

		/// <summary>
		/// バイト配列への変換
		/// </summary>
		/// <returns>
		///		バイト配列へ変換して返します。
		/// </returns>
		public virtual byte[] ToArray()
		{
			byte[] dst = new byte[this.Length];
			if (this.Length > 0)
				Marshal.Copy(this.Address(), dst, 0, this.Length);
			return dst;
		}

		/// <summary>
		/// 文字列への変換 (System.Text.Encoding.Default)
		/// </summary>
		/// <returns>
		///		文字列に変換して返します。
		/// </returns>
		public override string ToString()
		{
			return ToString(System.Text.Encoding.Default);
		}

		/// <summary>
		/// 文字列への変換
		/// </summary>
		/// <param name="encoding">エンコード</param>
		/// <returns>
		///		文字列に変換して返します。
		/// </returns>
		public virtual string ToString(System.Text.Encoding encoding)
		{
			return encoding.GetString(this.ToArray());
		}

		#endregion
	}
}
