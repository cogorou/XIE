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
	/// Exif オブジェクトクラス
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxExif : System.Object
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
				m_Tag = (TxExif*)m_Handle.TagPtr().ToPointer();
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
		private unsafe TxExif* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxExif Tag()
		{
			if (this.m_Tag == null)
				return new TxExif();
			return *this.m_Tag;
		}

		#endregion

		#region 生成関数:

		/// <summary>
		/// Exif オブジェクトハンドルからの生成
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public unsafe static CxExif FromHandle(HxModule handle, bool disposable)
		{
			return new CxExif(handle, disposable);
		}

		/// <summary>
		/// アタッチ (情報構造体指定)
		/// </summary>
		/// <param name="tag">アタッチ先</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static CxExif FromTag(TxExif tag)
		{
			var dst = new CxExif();
			dst.Attach(tag);
			return dst;
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_core.fnXIE_Core_Module_Create("CxExif");
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
		private CxExif(HxModule handle, bool disposable)
		{
			((IxModule)this).SetHandle(handle, disposable);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxExif()
		{
			_Constructor();
		}

		/// <summary>
		/// 領域の確保
		/// </summary>
		/// <param name="length">要素数 [0~] ※要素数が 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		public CxExif(int length)
		{
			_Constructor();
			this.Resize(length);
		}

		/// <summary>
		/// ファイル読み込み
		/// </summary>
		/// <param name="filename">ファイル名 [拡張子: raw]</param>
		/// <param name="options">オプション。(未使用)</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public CxExif(string filename, params object[] options)
		{
			_Constructor();
			Load(filename, options);
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxExif()
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
			var clone = new CxExif();
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
				var _src = (CxExif)src;
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

			if (src is TxExif)
			{
				ExStatus status = xie_core.fnXIE_Core_Exif_Attach(((IxModule)this).GetHandle(), (TxExif)src);
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
		[CxDescription("P:XIE.CxExif.Model")]
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
		[CxDescription("P:XIE.CxExif.Length")]
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
		[CxDescription("P:XIE.CxExif.Size")]
		public virtual SIZE_T Size
		{
			get { return this.Model.Size * this.Length; }
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

			ExStatus status = xie_core.fnXIE_Core_Exif_Resize(((IxModule)this).GetHandle(), length);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// リセット (全ての要素を 0 初期化します。)
		/// </summary>
		public virtual void Reset()
		{
			ExStatus status = xie_core.fnXIE_Core_Exif_Reset(((IxModule)this).GetHandle());
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
		public virtual unsafe void Attach(CxExif src, int index, int length)
		{
			this.Dispose();

			TxExif src_tag = src.Tag();
			if (src_tag.Address == IntPtr.Zero)
				throw new CxException(ExStatus.InvalidObject);
			if (index > src_tag.Length - 1)
				throw new CxException(ExStatus.InvalidParam);
			if (index + length > src_tag.Length)
				throw new CxException(ExStatus.InvalidParam);

			TxExif dst_tag = new TxExif(src[index], length, src.Model);
			this.Attach(dst_tag);
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

		#region メソッド: (Get/Set)

		/// <summary>
		/// エンディアンタイプを取得します。
		/// </summary>
		/// <returns>
		///		取得したエンディアンタイプを返します。
		///		保有する Exif が無効な場合は None を返します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual ExEndianType EndianType()
		{
			var result = ExEndianType.None;
			var status = xie_core.fnXIE_Core_Exif_EndianType(((IxModule)this).GetHandle(), ref result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		/// <summary>
		/// 識別可能な項目をすべて取得します。
		/// </summary>
		/// <returns>
		///		取得した項目のコレクションを返します。
		///		保有する Exif が無効な場合は要素数 0 のコレクションを返します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual List<TxExifItem> GetItems()
		{
			var result = new List<TxExifItem>();
			using (var buf = new CxArray())
			{
				var status = xie_core.fnXIE_Core_Exif_GetItems(((IxModule)this).GetHandle(), ((IxModule)buf).GetHandle());
				if (status != ExStatus.Success)
					throw new CxException(status);

				var scan = buf.Scanner();
				if (scan.IsValid)
				{
					scan.ForEach((int x, IntPtr addr) =>
						{
							var _src = (XIE.Ptr.Int32Ptr)addr;
							var _dst = new TxExifItem();
							_dst.Offset = (int)_src[0];
							_dst.EndianType = (ExEndianType)_src[1];
							_dst.ID = (ushort)_src[2];
							_dst.Type = (short)_src[3];
							_dst.Count = (int)_src[4];
							_dst.ValueOrIndex = (int)_src[5];
							result.Add(_dst);
						});
				}
			}
			return result;
		}

		/// <summary>
		/// 識別可能な項目と値を新しい領域に複製します。対象外の領域は 0 初期化されています。
		/// </summary>
		/// <returns>
		///		複製後のオブジェクトを返します。
		///		保有する Exif が無効な場合は未確保のオブジェクトを返します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual CxExif GetPurgedExif()
		{
			var result = new CxExif();
			var status = xie_core.fnXIE_Core_Exif_GetPurgedExif(((IxModule)this).GetHandle(), ((IxModule)result).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		/// <summary>
		/// 指定された項目の値を取得します。
		/// </summary>
		/// <param name="item">対象の項目</param>
		/// <param name="value">取得する値を格納するオブジェクト [CxArray, item.Type=2 の場合は CxStringA も可能です。]</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual void GetValue(TxExifItem item, IxModule value)
		{
			var status = xie_core.fnXIE_Core_Exif_GetValue(((IxModule)this).GetHandle(), item, value.GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 指定された項目の値を設定します。
		/// </summary>
		/// <param name="item">対象の項目</param>
		/// <param name="value">設定する値が格納されたオブジェクト [CxArray, item.Type=2 の場合は CxStringA も可能です。]</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual void SetValue(TxExifItem item, IxModule value)
		{
			var status = xie_core.fnXIE_Core_Exif_SetValue(((IxModule)this).GetHandle(), item, value.GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// ISerializable の実装: シグネチャコンストラクタ
		/// </summary>
		/// <param name="info">データ読み込み先の SerializationInfo</param>
		/// <param name="context">シリアル化先</param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public CxExif(SerializationInfo info, StreamingContext context)
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

			if (items != null && length > 0 && model.Type == ExType.U8 && model.Pack == 1)
			{
				Resize(length);
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
				this.Resize(length);
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
