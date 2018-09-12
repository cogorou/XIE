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
using XIE.Ptr;

namespace XIE
{
	/// <summary>
	/// 配列オブジェクトクラス
	/// </summary>
	/// <example>
	/// <code lang="C#" source="examples/Core/CxArray/CxArray_01.cs"/>
	/// </example>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxArray : System.Object
		, IxModule
		, IDisposable
		, ICloneable
		, IxEquatable
		, IxFileAccess
		, IxAttachable
		, ISerializable
		, IXmlSerializable
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
				m_Tag = (TxArray*)m_Handle.TagPtr().ToPointer();
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
		private unsafe TxArray* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxArray Tag()
		{
			if (this.m_Tag == null)
				return new TxArray();
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
		public unsafe static CxArray FromHandle(HxModule handle, bool disposable)
		{
			return new CxArray(handle, disposable);
		}

		/// <summary>
		/// アタッチ (情報構造体指定)
		/// </summary>
		/// <param name="tag">アタッチ先</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static CxArray FromTag(TxArray tag)
		{
			var dst = new CxArray();
			dst.Attach(tag);
			return dst;
		}

		/// <summary>
		/// IEnumerable からの変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static CxArray From(IEnumerable src)
		{
			var dst = new CxArray();
			dst.CopyFrom(src);
			return dst;
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_core.fnXIE_Core_Module_Create("CxArray");
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
		private CxArray(HxModule handle, bool disposable)
		{
			((IxModule)this).SetHandle(handle, disposable);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxArray()
		{
			_Constructor();
		}

		/// <summary>
		/// 領域の確保
		/// </summary>
		/// <param name="length">要素数 [0~] ※要素数が 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		/// <param name="model">要素の型</param>
		public CxArray(int length, TxModel model)
		{
			_Constructor();
			this.Resize(length, model);
		}

		/// <summary>
		/// ファイル読み込み
		/// </summary>
		/// <param name="filename">ファイル名 [拡張子: raw]</param>
		/// <param name="options">オプション。(未使用)</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public CxArray(string filename, params object[] options)
		{
			_Constructor();
			Load(filename, options);
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxArray()
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
			var clone = new CxArray();
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
			else if (src is IEnumerable<HxModule>)
			{
				#region データ変換.
				List<HxModule> _src = new List<HxModule>((IEnumerable<HxModule>)src);
				this.Resize(_src.Count, ModelOf.From(typeof(HxModule)));
				void** _dst = (void**)this.Address().ToPointer();
				for (int i = 0; i < _src.Count; i++)
					_dst[i] = _src[i].ToPointer();
				return;
				#endregion
			}
			else if (src is IEnumerable)
			{
				#region データ変換:
				int src_count = Axi.GetElementCount((IEnumerable)src);
				if (src_count == 0)
				{
					this.Dispose();
				}
				else
				{
					var src_enum = ((IEnumerable)src).GetEnumerator();
					src_enum.MoveNext();
					var src_item = src_enum.Current;
					if (src_item == null)
						throw new CxException(ExStatus.Unsupported, "Item is null.");
					Type src_type = src_item.GetType();
					Type ptr_type = Axi.PointerOf(src_type);
					if (ptr_type == null)
						throw new CxException(ExStatus.Unsupported, "There is no pointer for Element type.");
					TxModel model = ModelOf.From(src_type);
					this.Resize(src_count, model);
					var ptr = (IxEquatable)Activator.CreateInstance(ptr_type, new object[] { this.Address() });
					ptr.CopyFrom(src);
				}
				return;
				#endregion
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
				var _src = (CxArray)src;
				return ((IxModule)this).GetHandle().ContentEquals( ((IxModule)_src).GetHandle());
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region IxFileAccess の実装: (ファイルアクセス)

		/// <summary>
		/// IxFileAccess の実装: ファイル読み込み
		/// </summary>
		/// <param name="filename">ファイル名。(raw)</param>
		/// <param name="options">オプション。(未使用)</param>
		public virtual void Load(string filename, params object[] options)
		{
			if (string.IsNullOrEmpty(filename))
				throw new CxException(ExStatus.InvalidParam);
			filename = System.IO.Path.GetFullPath(filename);
			if (System.IO.File.Exists(filename) == false)
				throw new System.IO.FileNotFoundException();

			ExStatus status = xie_core.fnXIE_Core_FileAccess_Load(((IxModule)this).GetHandle(), filename, IntPtr.Zero, TxModel.Default);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// IxFileAccess の実装: ファイル保存
		/// </summary>
		/// <param name="filename">ファイル名。(raw)</param>
		/// <param name="options">オプション。(未使用)</param>
		public virtual void Save(string filename, params object[] options)
		{
			if (string.IsNullOrEmpty(filename))
				throw new CxException(ExStatus.InvalidParam);

			ExStatus status = xie_core.fnXIE_Core_FileAccess_Save(((IxModule)this).GetHandle(), filename, IntPtr.Zero, TxModel.Default);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region IxAttachable の実装:

		/// <summary>
		/// アタッチ状態 [初期値:false] (true の場合 Dispose で Handle を解放しません。)
		/// </summary>
		/// <returns>
		///		現在のオブジェクトが外部リソースを参照している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool IsAttached
		{
			get { return ((IxModule)this).GetHandle().IsAttached(); }
		}

		/// <summary>
		/// アタッチ
		/// </summary>
		/// <param name="src">アタッチ先</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe void Attach(object src)
		{
			if (src is IxModule)
			{
				ExStatus status = xie_core.fnXIE_Core_Attachable_Attach(((IxModule)this).GetHandle(), ((IxModule)src).GetHandle());
				if (status != ExStatus.Success)
					throw new CxException(status);
				return;
			}

			if (src is TxArray)
			{
				ExStatus status = xie_core.fnXIE_Core_Array_Attach(((IxModule)this).GetHandle(), (TxArray)src);
				if (status != ExStatus.Success)
					throw new CxException(status);
				return;
			}

			throw new CxException(ExStatus.Unsupported);
		}

		#endregion

		#region インデクサ:

		/// <summary>
		/// 配列の先頭アドレス
		/// </summary>
		/// <returns>
		///		配列の先頭アドレスを返します。
		/// </returns>
		public virtual unsafe IntPtr Address()
		{
			if (this.m_Tag == null)
				return IntPtr.Zero;
			return this.m_Tag->Address;
		}

		/// <summary>
		/// 要素のアドレス
		/// </summary>
		/// <param name="index">配列指標 [0~(Length-1)]</param>
		/// <returns>
		///		指定位置の要素のアドレスを返します。
		/// </returns>
		public virtual unsafe IntPtr this[int index]
		{
			get
			{
				if (this.m_Tag == null ||
					this.m_Tag->Address == IntPtr.Zero)
					return IntPtr.Zero;

				if (!(0 <= index && index < this.Length))
					throw new CxException(ExStatus.InvalidParam);

				byte* addr = (byte*)this.m_Tag->Address.ToPointer();
				return new IntPtr(addr + (index * this.Model.Size));
			}
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 要素の型 [初期値:TxModel.Default]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxArray.Model")]
		public virtual unsafe TxModel Model
		{
			get
			{
				if (this.m_Tag == null)
					return TxModel.Default;
				return this.m_Tag->Model;
			}
		}

		/// <summary>
		/// 要素数 [初期値:0、範囲:0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxArray.Length")]
		public virtual unsafe int Length
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Length;
			}
		}

		/// <summary>
		/// 配列のサイズ (bytes)
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxArray.Size")]
		public virtual SIZE_T Size
		{
			get { return this.Model.Size * this.Length; }
		}

		#endregion

		#region メソッド: (Resize)

		/// <summary>
		/// 領域の確保
		/// </summary>
		/// <param name="model">要素の型</param>
		/// <param name="length">要素数 [0~] ※要素数が 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		public virtual void Resize(int length, TxModel model)
		{
			if (((IxModule)this).GetHandle() == IntPtr.Zero)
				_Constructor();

			ExStatus status = xie_core.fnXIE_Core_Array_Resize(((IxModule)this).GetHandle(), length, model);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// リセット (全ての要素を 0 初期化します。)
		/// </summary>
		public virtual void Reset()
		{
			ExStatus status = xie_core.fnXIE_Core_Array_Reset(((IxModule)this).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Attach)

		/// <summary>
		/// アタッチ (範囲指定)
		/// </summary>
		/// <param name="src">アタッチ先</param>
		/// <param name="index">始点 [0~]</param>
		/// <param name="length">長さ [1~]</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe void Attach(CxArray src, int index, int length)
		{
			this.Dispose();

			TxArray src_tag = src.Tag();
			if (src_tag.Address == IntPtr.Zero)
				throw new CxException(ExStatus.InvalidObject);
			if (index > src_tag.Length - 1)
				throw new CxException(ExStatus.InvalidParam);
			if (index + length > src_tag.Length)
				throw new CxException(ExStatus.InvalidParam);

			TxArray dst_tag = new TxArray(src[index], length, src.Model);
			this.Attach(dst_tag);
		}

		#endregion

		#region メソッド: (Child)

		/// <summary>
		/// チャイルド配列の生成
		/// </summary>
		/// <returns>
		///		現在のオブジェクトの内部領域にアタッチした新しいオブジェクトを生成して返します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe CxArray Child()
		{
			CxArray child = new CxArray();
			child.Attach(this);
			return child;
		}

		/// <summary>
		/// チャイルド配列の生成 (範囲指定)
		/// </summary>
		/// <param name="index">始点 [0~]</param>
		/// <param name="length">長さ [1~]</param>
		/// <returns>
		///		現在のオブジェクトの内部領域にアタッチした新しいオブジェクトを生成して返します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe CxArray Child(int index, int length)
		{
			CxArray child = new CxArray();
			child.Attach(this, index, length);
			return child;
		}

		#endregion

		#region メソッド: (Clone)

		/// <summary>
		/// オブジェクトのクローンの生成 (型変換)
		/// </summary>
		/// <param name="model">出力配列の型</param>
		/// <param name="scale">スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]</param>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual CxArray Clone(TxModel model, double scale = 0)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);

			TxModel dst_model = new TxModel();
			dst_model.Type = (model.Type != ExType.None) ? model.Type : this.Model.Type;
			dst_model.Pack = (model.Pack != 0) ? model.Pack : this.Model.Pack;
			CxArray clone = new CxArray(this.Length, dst_model);
			clone.Filter().Copy(this, scale);
			return clone;
		}

		#endregion

		#region メソッド: (Clear)

		/// <summary>
		/// 要素のクリア
		/// </summary>
		/// <param name="value">塗りつぶす濃度。</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe void Clear(object value)
		{
			if (value == null)
				throw new CxException(ExStatus.InvalidParam);

			HxModule hdst = ((IxModule)this).GetHandle();
			ExStatus status = ExStatus.Unsupported;
			TxModel model = ModelOf.From(value.GetType());
			switch (model.Type)
			{
				case ExType.None:
					if (value is Color)
					{
						var rgb_value = (TxRGB8x4)((Color)value);
						var rgb_model = ModelOf.From(rgb_value.GetType());
						status = xie_core.fnXIE_Core_Array_Clear(hdst, new IntPtr(&rgb_value), rgb_model);
					}
					break;
				case ExType.Ptr:
					break;
				default:
					using (CxArray temp = new CxArray(1, model))
					{
						IntPtr addr = temp.Address();
						System.Runtime.InteropServices.Marshal.StructureToPtr(value, addr, false);
						status = xie_core.fnXIE_Core_Array_Clear(hdst, addr, model);
					}
					break;
			}
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 要素のクリア (フィールド指定)
		/// </summary>
		/// <param name="value">塗りつぶす濃度。</param>
		/// <param name="index">フィールド指標 [0~]</param>
		/// <param name="count">フィールド数 [1~]</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual void ClearEx(object value, int index, int count)
		{
			if (value == null)
				throw new CxException(ExStatus.InvalidParam);
			HxModule hdst = ((IxModule)this).GetHandle();

			ExStatus status = ExStatus.Unsupported;
			TxModel model = ModelOf.From(value.GetType());
			switch (model.Type)
			{
				case ExType.None:
					break;
				case ExType.Ptr:
					break;
				default:
					using (CxArray temp = new CxArray(1, model))
					{
						IntPtr addr = temp.Address();
						System.Runtime.InteropServices.Marshal.StructureToPtr(value, addr, false);
						status = xie_core.fnXIE_Core_Array_ClearEx(hdst, addr, model, index, count);
					}
					break;
			}
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Measure)

		/// <summary>
		/// 統計
		/// </summary>
		/// <param name="ch">フィールド指標 [0~]</param>
		/// <returns>
		///		統計データを返します。
		///		未確保の場合は例外を発行します。<br/>
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public TxStatistics Statistics(int ch = 0)
		{
			var result = new TxStatistics();
			HxModule hsrc = ((IxModule)this).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Array_Statistics(hsrc, ch, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		/// <summary>
		/// 抽出
		/// </summary>
		/// <param name="index">抽出位置 [0~Length-1]</param>
		/// <param name="length">抽出する長さ [範囲:0~]</param>
		/// <returns>
		///		指定された範囲の要素を格納して返します。
		///		配列の要素は自身と同一型です。要素数は抽出する長さと同一です。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public CxArray Extract(int index, int length)
		{
			CxArray result = new CxArray();
			HxModule hresult = ((IxModule)result).GetHandle();
			HxModule hsrc = ((IxModule)this).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Array_Extract(hsrc, index, length, hresult);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		#endregion

		#region メソッド: (Filter)

		/// <summary>
		/// フィルタの取得
		/// </summary>
		/// <returns>
		///		配列オブジェクトフィルタを返します。
		/// </returns>
		public virtual CxArrayFilter Filter()
		{
			return new CxArrayFilter(this);
		}

		#endregion

		#region メソッド: (Scanner)

		/// <summary>
		/// 配列走査構造体の取得
		/// </summary>
		/// <returns>
		///		１次元配列走査構造体を返します。
		/// </returns>
		public virtual TxScanner1D Scanner()
		{
			return new TxScanner1D(this.Address(), this.Length, this.Model);
		}

		/// <summary>
		/// 配列走査構造体の取得
		/// </summary>
		/// <param name="index">始点 [0~]</param>
		/// <param name="length">長さ [1~]</param>
		/// <returns>
		///		１次元配列走査構造体を返します。
		/// </returns>
		public virtual TxScanner1D Scanner(int index, int length)
		{
			if (index < 0)
				throw new CxException(ExStatus.InvalidParam);
			if (!(index + length <= this.Length))
				throw new CxException(ExStatus.InvalidParam);
			return new TxScanner1D(this[index], length, this.Model);
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// ISerializable の実装: シグネチャコンストラクタ
		/// </summary>
		/// <param name="info">データ読み込み先の SerializationInfo</param>
		/// <param name="context">シリアル化先</param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public CxArray(SerializationInfo info, StreamingContext context)
		{
			_Constructor();

			TxModel model = TxModel.Default;
			int length = 0;
			byte[] items = null;
 
			foreach (SerializationEntry entry in info)
			{
				try
				{
					switch (entry.Name)
					{
						case "Model":
							model = (TxModel)info.GetValue(entry.Name, typeof(TxModel));
							break;
						case "Length":
							length = (int)info.GetValue(entry.Name, typeof(int));
							break;
						case "Items":
							items = (byte[])info.GetValue(entry.Name, typeof(byte[]));
							break;
					}
				}
				catch (System.Exception)
				{
				}
			}

			if (items != null && length > 0)
			{
				Resize(length, model);
				Marshal.Copy(items, 0, this.Address(), items.Length);
			}
		}

		/// <summary>
		/// ISerializable の実装: SerializationInfo に、オブジェクトをシリアル化するために必要なデータを設定します。
		/// </summary>
		/// <param name="info">データ読み込み先の SerializationInfo</param>
		/// <param name="context">シリアル化先</param>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Model", this.Model);
			info.AddValue("Length", this.Length);

			byte[] buf = new byte[this.Size];
			if (this.Address() != IntPtr.Zero)
			{
				Marshal.Copy(this.Address(), buf, 0, buf.Length);
			}
			info.AddValue("Items", buf);
		}

		#endregion

		#region IXmlSerializable の実装:

		/// <summary>
		/// IXmlSerializable の実装: オブジェクトの XML 表現からオブジェクトを生成します。
		/// </summary>
		/// <param name="reader">オブジェクトの逆シリアル化元である XmlReader ストリーム。</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			reader.ReadStartElement();
			// --------------------------------------------------
			TxModel model = TxModel.Default;
			int length = 0;
			if (reader.Name == "Model")
				model = XIE.AxiXml.GetValue<TxModel>(reader, reader.Name);
			if (reader.Name == "Length")
				length = XIE.AxiXml.GetValue<int>(reader, reader.Name);
			// --------------------------------------------------
			if (model.Type != ExType.None && length > 0)
				this.Resize(length, model);
			else
				this.Dispose();
			// --------------------------------------------------
			if (reader.Name == "Items")
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement(reader.Name);
				}
				else
				{
					reader.ReadStartElement(reader.Name);
					if (this.Address() != IntPtr.Zero)
					{
						Type item_type = Axi.TypeOf(model.Type);
						Type ptr_type = Axi.PointerOf(model.Type);
						if (item_type != null && ptr_type != null)
						{
							XmlSerializer xs = new XmlSerializer(item_type.MakeArrayType());
							object items = xs.Deserialize(reader);
							object ptr = Activator.CreateInstance(ptr_type, new object[] { this.Address() });
							ptr_type.GetMethod("CopyFrom").Invoke(ptr, new object[] { items });
						}
					}
					reader.ReadEndElement();
				}
			}
			// --------------------------------------------------
			reader.ReadEndElement();
		}

		/// <summary>
		/// IXmlSerializable の実装: オブジェクトを XML 表現に変換します。
		/// </summary>
		/// <param name="writer">オブジェクトのシリアル化先の XmlWriter ストリーム。</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", "");	// 各項目にプレフィックスと名前空間を入れない.

			XIE.AxiXml.AddValue(writer, "Model", this.Model);
			XIE.AxiXml.AddValue(writer, "Length", this.Length);

			int count = this.Length * this.Model.Pack;
			Type ptr_type = Axi.PointerOf(this.Model.Type);
			if (ptr_type != null && count > 0)
			{
				writer.WriteStartElement("Items");

				object ptr = Activator.CreateInstance(ptr_type, new object[] { this.Address() });
				object buf = ptr_type.GetMethod("ToArray").Invoke(ptr, new object[] { count });
				XmlSerializer xs = new XmlSerializer(buf.GetType());
				xs.Serialize(writer, buf, ns);

				writer.WriteEndElement();
			}
		}

		/// <summary>
		/// IXmlSerializable の実装: 処理対象オブジェクトの XML 表現を記述する XmlSchema
		/// </summary>
		/// <returns>
		///		常に null を返します。
		/// </returns>
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		#endregion
	}
}
