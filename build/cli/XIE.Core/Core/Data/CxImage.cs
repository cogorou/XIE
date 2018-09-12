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

namespace XIE
{
	/// <summary>
	/// 画像オブジェクトクラス
	/// </summary>
	/// <example>
	/// <code lang="C#" source="examples/Core/CxImage/CxImage_01.cs"/>
	/// </example>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxImage : System.Object
		, IxModule
		, IDisposable
		, ICloneable
		, IxEquatable
		, IxFileAccess
		, IxAttachable
		, IxLockable
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
				m_Tag = (TxImage*)m_Handle.TagPtr().ToPointer();
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
		private unsafe TxImage* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxImage Tag()
		{
			if (this.m_Tag == null)
				return new TxImage();
			return *this.m_Tag;
		}

		#endregion

		#region 生成関数:

		/// <summary>
		/// 画像オブジェクトハンドルからの生成
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public unsafe static CxImage FromHandle(HxModule handle, bool disposable)
		{
			return new CxImage(handle, disposable);
		}

		/// <summary>
		/// アタッチ (情報構造体指定)
		/// </summary>
		/// <param name="tag">アタッチ先</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static CxImage FromTag(TxImage tag)
		{
			var dst = new CxImage();
			dst.Attach(tag);
			return dst;
		}

		/// <summary>
		/// IEnumerable からの変換
		/// </summary>
		/// <param name="width">幅 [1~]</param>
		/// <param name="height">高さ [1~]</param>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static CxImage From(int width, int height, IEnumerable src)
		{
			int src_count = Axi.GetElementCount((IEnumerable)src);
			Type src_type = src.GetType().GetElementType();
			var model = ModelOf.From(src_type);
			if (model.Type == ExType.None || model.Pack == 0)
				throw new CxException(ExStatus.InvalidParam, "Model for Element was not found.");
			var dst = new CxImage(width, height, model, 1, 1);
			if (src_count > 0)
				dst.Scanner(0).Copy(src);
			return dst;
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_core.fnXIE_Core_Module_Create("CxImage");
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
		private CxImage(HxModule handle, bool disposable)
		{
			((IxModule)this).SetHandle(handle, disposable);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxImage()
		{
			_Constructor();
		}

		/// <summary>
		/// 領域の確保
		/// </summary>
		/// <param name="size">画像サイズ</param>
		/// <param name="packing_size">水平方向サイズのパッキングサイズ (bytes) [1,2,4,8,16] 既定値:[4]</param>
		/// <exception cref="T:XIE.CxException"/>
		public CxImage(TxImageSize size, int packing_size = XIE.Defs.XIE_IMAGE_PACKING_SIZE)
		{
			_Constructor();
			this.Resize(size, packing_size);
		}

		/// <summary>
		/// 領域の確保
		/// </summary>
		/// <param name="width">幅 [1~] ※幅/高さ共に 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		/// <param name="height">高さ [1~] ※幅/高さ共に 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		/// <param name="model">要素の型</param>
		/// <param name="channels">チャネル数 [1~16]</param>
		/// <param name="packing_size">水平方向サイズのパッキングサイズ (bytes) [1,2,4,8,16] 既定値:[4]</param>
		/// <exception cref="T:XIE.CxException"/>
		public CxImage(int width, int height, TxModel model, int channels, int packing_size = XIE.Defs.XIE_IMAGE_PACKING_SIZE)
		{
			_Constructor();
			this.Resize(width, height, model, channels, packing_size);
		}

		/// <summary>
		/// ファイル読み込み
		/// </summary>
		/// <param name="filename">ファイル名。[拡張子: bmp,png,jpg,jpeg,tif,tiff,raw]</param>
		/// <param name="options">オプション。[true=Unpacking, false=Packing] 既定では false です。</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public CxImage(string filename, params object[] options)
		{
			_Constructor();
			Load(filename, options);
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxImage()
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
			var clone = new CxImage();
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
			else if (src is Bitmap)
			{
				#region Bitmap からの変換.
				using (CxImage tmp = new CxImage())
				{
					Bitmap _src = (Bitmap)src;
					TxModel model;
					switch (_src.PixelFormat)
					{
						case PixelFormat.Format8bppIndexed:
							model = ModelOf.From(typeof(byte));
							break;
						case PixelFormat.Format24bppRgb:
							model = ModelOf.From(typeof(TxBGR8x3));
							break;
						case PixelFormat.Format32bppRgb:
						case PixelFormat.Format32bppArgb:
							model = ModelOf.From(typeof(TxBGR8x4));
							break;
						default:
							throw new CxException(ExStatus.Unsupported);
					}
					tmp.Resize(_src.Width, _src.Height, model, 1);

					BitmapData bmpData = null;
					try
					{
						bmpData = new BitmapData();
						bmpData.Width = tmp.Width;
						bmpData.Height = tmp.Height;
						bmpData.Stride = tmp.Stride;
						bmpData.PixelFormat = _src.PixelFormat;
						bmpData.Scan0 = tmp[0, 0, 0];

						_src.LockBits(
							new Rectangle(0, 0, _src.Width, _src.Height),
							ImageLockMode.ReadOnly | ImageLockMode.UserInputBuffer,
							_src.PixelFormat,
							bmpData
							);
					}
					finally
					{
						_src.UnlockBits(bmpData);
					}

					if (this.ImageSize != tmp.ImageSize)
						this.Resize(tmp.ImageSize);
					this.Filter().RgbToBgr(tmp);

					return;
				}
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
				var _src = (CxImage)src;
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
		/// <param name="filename">ファイル名。[拡張子: bmp,png,jpg,jpeg,tif,tiff,raw]</param>
		/// <param name="options">オプション。[true=Unpacking, false=Packing] 既定では false です。</param>
		public virtual void Load(string filename, params object[] options)
		{
			if (string.IsNullOrEmpty(filename))
				throw new CxException(ExStatus.InvalidParam);
			filename = System.IO.Path.GetFullPath(filename);
			if (System.IO.File.Exists(filename) == false)
				    throw new FileNotFoundException();

			#region 引数解析.
			bool unpack = false;
			if (options != null)
			{
				if (options.Length > 0 && options[0] is bool)
					unpack = (bool)options[0];
			}
			#endregion

			#region 読み込み:
			if (options != null && options.Length > 0)
			{
				if (options[0] is bool)
				{
					var option = (bool)options[0];
					var pOption = new IntPtr(&option);
					var model = ModelOf.From(option.GetType());
					ExStatus status = xie_core.fnXIE_Core_FileAccess_Load(((IxModule)this).GetHandle(), filename, pOption, model);
					if (status != ExStatus.Success)
						throw new CxException(status);
				}
			}
			else
			{
				var pOption = IntPtr.Zero;
				var model = TxModel.Default;
				ExStatus status = xie_core.fnXIE_Core_FileAccess_Load(((IxModule)this).GetHandle(), filename, pOption, model);
				if (status != ExStatus.Success)
					throw new CxException(status);
			}
			#endregion
		}

		/// <summary>
		/// IxFileAccess の実装: ファイル保存
		/// </summary>
		/// <param name="filename">ファイル名。(bmp,png,jpg,jpeg,tif,tiff,raw)</param>
		/// <param name="options">オプション。
		///		圧縮レベルや圧縮品質を Int32 (32bit 符号付き整数) で指定できます。bmp,raw では使用しません。
		///		png: [-1, 0, 1~9] 圧縮レベル。-1 は既定値、0 は非圧縮、1 は低圧縮、9 は高圧縮。(未指定の場合は -1 と等価です。)
		///		jpeg: [0~100] 圧縮品質。0 は低品質、100 は高品質。(未指定の場合は 100 と等価です。)
		///		tiff: [-1, 0, 1~9] 圧縮レベル。-1 は既定値、0 は非圧縮、1 は低圧縮、9 は高圧縮。(未指定の場合は非圧縮です。)
		/// </param>
		public virtual void Save(string filename, params object[] options)
		{
			if (string.IsNullOrEmpty(filename))
				throw new CxException(ExStatus.InvalidParam);

			#region 保存:
			if (options != null && options.Length > 0)
			{
				if (options[0] is int)
				{
					var option = (int)options[0];
					var pOption = new IntPtr(&option);
					var model = ModelOf.From(option.GetType());
					ExStatus status = xie_core.fnXIE_Core_FileAccess_Save(((IxModule)this).GetHandle(), filename, pOption, model);
					if (status != ExStatus.Success)
						throw new CxException(status);
				}
			}
			else
			{
				var pOption = IntPtr.Zero;
				var model = TxModel.Default;
				ExStatus status = xie_core.fnXIE_Core_FileAccess_Save(((IxModule)this).GetHandle(), filename, pOption, model);
				if (status != ExStatus.Success)
					throw new CxException(status);
			}
			#endregion
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

			if (src is TxImage)
			{
				ExStatus status = xie_core.fnXIE_Core_Image_Attach(((IxModule)this).GetHandle(), (TxImage)src);
				if (status != ExStatus.Success)
					throw new CxException(status);
				return;
			}

			throw new CxException(ExStatus.Unsupported);
		}

		#endregion

		#region IxLockable の実装:

		/// <summary>
		/// ロック
		/// </summary>
		/// <exception cref="T:XIE.CxException"/>
		public virtual void Lock()
		{
			((IxModule)this).GetHandle().Lock();
		}

		/// <summary>
		/// ロック解除
		/// </summary>
		/// <exception cref="T:XIE.CxException"/>
		public virtual void Unlock()
		{
			((IxModule)this).GetHandle().Unlock();
		}

		/// <summary>
		/// ロック状態 [初期値:false]
		/// </summary>
		/// <returns>
		///		現在のオブジェクトがメモリをロックしている場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		public virtual bool IsLocked
		{
			get
			{
				return ((IxModule)this).GetHandle().IsLocked();
			}
		}

		#endregion

		#region インデクサ:

		/// <summary>
		/// 領域の先頭アドレス
		/// </summary>
		/// <param name="ch">チャネル指標 [0~(Channels-1)]</param>
		/// <returns>
		///		指定チャネルの領域の先頭アドレスを返します。<br/>
		///		未確保の場合は IntPtr.Zero を返します。<br/>
		/// </returns>
		public virtual unsafe IntPtr Address(int ch)
		{
			if (this.m_Tag == null)
				return IntPtr.Zero;
			return this.m_Tag->Layer[ch];
		}

		/// <summary>
		/// 領域の先頭アドレス
		/// </summary>
		/// <param name="ch">チャネル指標 [0~(Channels-1)]</param>
		/// <returns>
		///		指定チャネルの領域の先頭アドレスを返します。<br/>
		///		未確保の場合は IntPtr.Zero を返します。<br/>
		/// </returns>
		public virtual unsafe IntPtr this[int ch]
		{
			get
			{
				return Address(ch);
			}
		}

		/// <summary>
		/// 要素のアドレス
		/// </summary>
		/// <param name="ch">チャネル指標 [0~(Channels-1)]</param>
		/// <param name="y">Y座標 [0~(Height-1)]</param>
		/// <param name="x">X座標 [0~(Width-1)]</param>
		/// <returns>
		///		指定位置の要素のアドレスを返します。
		///		未確保の場合は例外を発行します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe IntPtr this[int ch, int y, int x]
		{
			get
			{
				TxImage* tag = this.m_Tag;
				if (tag == null)
					throw new CxException(ExStatus.InvalidObject);

				byte* addr = (byte*)tag->Layer[ch];
				if (addr == null)
					throw new CxException(ExStatus.InvalidObject);

				if (!(0 <= ch && ch < tag->Channels))
					throw new CxException(ExStatus.InvalidParam);
				if (x < 0 || y < 0)
					throw new CxException(ExStatus.InvalidParam);

				if (!(y < tag->Height))
					throw new CxException(ExStatus.InvalidParam);
				if (!(x < tag->Width))
					throw new CxException(ExStatus.InvalidParam);

				return new IntPtr(addr + (y * (SIZE_T)tag->Stride) + (x * (SIZE_T)tag->Model.Size));
			}
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 幅 (pixels) [範囲:0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxImage.Width")]
		public virtual unsafe int Width
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Width;
			}
		}

		/// <summary>
		/// 高さ (pixels) [範囲:0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxImage.Height")]
		public virtual unsafe int Height
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Height;
			}
		}

		/// <summary>
		/// チャネル数 [範囲:1~16]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxImage.Channels")]
		public virtual unsafe int Channels
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Channels;
			}
		}

		/// <summary>
		/// 要素の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxImage.Model")]
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
		/// 水平方向サイズ (bytes) [範囲:0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxImage.Stride")]
		public virtual unsafe int Stride
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Stride;
			}
		}

		/// <summary>
		/// ビット深度 [範囲:0=最大値、1~64:指定値]
		/// </summary>
		[XmlIgnore]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxImage.Depth")]
		public virtual unsafe int Depth
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Depth;
			}
			set
			{
				if (this.m_Tag == null)
					return;
				this.m_Tag->Depth = value;
			}
		}

		#endregion

		#region プロパティ: (拡張)

		/// <summary>
		/// 要素のサイズ (bits) [範囲:0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxImage.Bpp")]
		public virtual unsafe int Bpp
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Model.Size * 8;
			}
		}

		/// <summary>
		/// 画像のサイズ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxImage.Size")]
		public virtual unsafe TxSizeI Size
		{
			get
			{
				if (this.m_Tag == null)
					return default(TxSizeI);
				return new TxSizeI(this.m_Tag->Width, this.m_Tag->Height);
			}
		}

		/// <summary>
		/// 画像のサイズ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxImage.ImageSize")]
		public virtual unsafe TxImageSize ImageSize
		{
			get
			{
				if (this.m_Tag == null)
					return default(TxImageSize);
				return new TxImageSize(this.m_Tag->Width, this.m_Tag->Height, this.m_Tag->Model, this.m_Tag->Channels, this.m_Tag->Depth);
			}
		}

		#endregion

		#region メソッド: (Exif)

		/// <summary>
		/// Exif の取得
		/// </summary>
		/// <returns>
		///		Exif が格納された領域を示す配列情報を返します。
		///		内部領域を参照していますので解放しないでください。
		/// </returns>
		public virtual TxExif Exif()
		{
			var handle = ((IxModule)this).GetHandle();
			var value = new TxExif();
			ExStatus status = xie_core.fnXIE_Core_Image_Exif_Get(handle, out value);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return value;
		}

		/// <summary>
		/// Exif の設定
		/// </summary>
		/// <param name="value">Exif が格納された領域を示す配列情報 (※内容を複製するので処理後は解放して構いません。)</param>
		public virtual void Exif(TxExif value)
		{
			var handle = ((IxModule)this).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Exif_Set(handle, value);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// Exif の加工・複製
		/// </summary>
		/// <param name="exif">複製元の Exif 構造体</param>
		/// <param name="ltc">複製後の Exif のタイムゾーン [true=ローカル時刻、false=協定世界時]</param>
		public virtual void ExifCopy(TxExif exif, bool ltc = true)
		{
			var handle = ((IxModule)this).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_ExifCopy(handle, exif, (ltc ? ExBoolean.True : ExBoolean.False));
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Resize)

		/// <summary>
		/// 領域の確保
		/// </summary>
		/// <param name="size">画像サイズ</param>
		/// <param name="packing_size">水平方向サイズのパッキングサイズ (bytes) [1,2,4,8,16] 既定値:[4]</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual void Resize(TxImageSize size, int packing_size = XIE.Defs.XIE_IMAGE_PACKING_SIZE)
		{
			this.Resize(size.Width, size.Height, size.Model, size.Channels, packing_size);
		}

		/// <summary>
		/// 領域の確保
		/// </summary>
		/// <param name="width">幅 [1~] ※幅/高さ共に 0 の場合は解放します。</param>
		/// <param name="height">高さ [1~] ※幅/高さ共に 0 の場合は解放します。</param>
		/// <param name="model">要素の型</param>
		/// <param name="channels">チャネル数 [1~16]</param>
		/// <param name="packing_size">水平方向サイズのパッキングサイズ (bytes) [1,2,4,8,16] 既定値:[4]</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual void Resize(int width, int height, TxModel model, int channels, int packing_size = XIE.Defs.XIE_IMAGE_PACKING_SIZE)
		{
			if (((IxModule)this).GetHandle() == IntPtr.Zero)
				_Constructor();

			ExStatus status = xie_core.fnXIE_Core_Image_Resize(((IxModule)this).GetHandle(), width, height, model, channels, packing_size);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// リセット (全ての要素を 0 初期化します。)
		/// </summary>
		public virtual void Reset()
		{
			ExStatus status = xie_core.fnXIE_Core_Image_Reset(((IxModule)this).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Attach)

		/// <summary>
		/// アタッチ (範囲指定)
		/// </summary>
		/// <param name="src">アタッチ先</param>
		/// <param name="bounds">始点とサイズ (幅,高さが 0 の場合はそれぞれ最大値に正規化します。)</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe void Attach(CxImage src, TxRectangleI bounds)
		{
			Attach(src, -1, bounds);
		}

		/// <summary>
		/// アタッチ (単一チャネル)
		/// </summary>
		/// <param name="src">アタッチ先</param>
		/// <param name="ch">チャネル指標 [範囲:-1=全チャネル、0~:指定チャネル]</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe void Attach(CxImage src, int ch)
		{
			TxImage src_tag = src.Tag();
			TxImage dst_tag = new TxImage();

			// ----- channel
			if (ch == -1)
				dst_tag.Layer = src_tag.Layer;
			else if (0 <= ch && ch < src_tag.Channels)
				dst_tag.Layer[0] = src_tag.Layer[ch];
			else
				throw new System.ArgumentException("The ch have to be between 0 to (src.Channels-1).");

			// ----- tag
			dst_tag.Width = src_tag.Width;
			dst_tag.Height = src_tag.Height;
			dst_tag.Model = src_tag.Model;
			dst_tag.Channels = (ch == -1) ? src_tag.Channels : 1;
			dst_tag.Depth = src_tag.Depth;
			dst_tag.Stride = src_tag.Stride;

			ExStatus status = xie_core.fnXIE_Core_Image_Attach(((IxModule)this).GetHandle(), dst_tag);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// アタッチ (単一チャネル + 範囲指定)
		/// </summary>
		/// <param name="src">アタッチ先</param>
		/// <param name="ch">チャネル指標 [範囲:-1=全チャネル、0~:指定チャネル]</param>
		/// <param name="bounds">始点とサイズ (幅,高さが 0 の場合はそれぞれ最大値に正規化します。)</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe void Attach(CxImage src, int ch, TxRectangleI bounds)
		{
			TxImage src_tag = src.Tag();
			TxImage dst_tag = new TxImage();

			// ----- bounds
			if (bounds.X < 0 ||
				bounds.Y < 0)
				throw new System.ArgumentException("The bounds have to be {0,0} or more.");
			int x = bounds.X;
			int y = bounds.Y;
			if (!(0 <= x && 0 <= y))
				throw new System.ArgumentException("The (offset + bounds) have to be {0,0} or more.");

			int width = bounds.Width;
			int height = bounds.Height;
			if (width <= 0)
				width = src_tag.Width - x;
			if (height <= 0)
				height = src_tag.Height - y;
			if (!(x + width <= src_tag.Width))
				throw new CxException(ExStatus.InvalidParam);
			if (!(y + height <= src_tag.Height))
				throw new CxException(ExStatus.InvalidParam);

			// ----- channel
			if (ch == -1)
			{
				for (int i = 0; i < src_tag.Channels; i++)
				{
					if (src_tag.Layer[i] == IntPtr.Zero)
						throw new CxException(ExStatus.InvalidParam);
					dst_tag.Layer[i] = new IntPtr(
							((byte*)src_tag.Layer[i])
							+ (y * (SIZE_T)src_tag.Stride)
							+ (x * (SIZE_T)src_tag.Model.Size)
						);
				}
			}
			else if (0 <= ch && ch < src_tag.Channels)
			{
				if (src_tag.Layer[ch] == IntPtr.Zero)
					throw new CxException(ExStatus.InvalidParam);
				dst_tag.Layer[0] = new IntPtr(
						((byte*)src_tag.Layer[ch])
						+ (y * (SIZE_T)src_tag.Stride)
						+ (x * (SIZE_T)src_tag.Model.Size)
					);
			}
			else
			{
				throw new System.ArgumentException("The ch have to be between 0 to (src.Channels-1).");
			}

			// ----- tag
			dst_tag.Width = width;
			dst_tag.Height = height;
			dst_tag.Model = src_tag.Model;
			dst_tag.Channels = (ch == -1) ? src_tag.Channels : 1;
			dst_tag.Depth = src_tag.Depth;
			dst_tag.Stride = src_tag.Stride;

			ExStatus status = xie_core.fnXIE_Core_Image_Attach(((IxModule)this).GetHandle(), dst_tag);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Child)

		/// <summary>
		/// チャイルド画像の生成
		/// </summary>
		/// <returns>
		///		現在のオブジェクトの画像領域にアタッチした新しいオブジェクトを生成して返します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe CxImage Child()
		{
			CxImage child = new CxImage();
			child.Attach(this);
			return child;
		}

		/// <summary>
		/// チャイルド画像の生成 (範囲指定)
		/// </summary>
		/// <param name="bounds">始点とサイズ (幅,高さが 0 の場合はそれぞれ最大値に正規化します。)</param>
		/// <returns>
		///		現在のオブジェクトの画像領域にアタッチした新しいオブジェクトを生成して返します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe CxImage Child(TxRectangleI bounds)
		{
			CxImage child = new CxImage();
			child.Attach(this, bounds);
			return child;
		}

		/// <summary>
		/// チャイルド画像の生成 (単一チャネル)
		/// </summary>
		/// <param name="ch">チャネル指標 [範囲:-1=全チャネル、0~:指定チャネル]</param>
		/// <returns>
		///		現在のオブジェクトの画像領域にアタッチした新しいオブジェクトを生成して返します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe CxImage Child(int ch)
		{
			CxImage child = new CxImage();
			child.Attach(this, ch);
			return child;
		}

		/// <summary>
		/// チャイルド画像の生成 (単一チャネル＋範囲指定)
		/// </summary>
		/// <param name="ch">チャネル指標 [範囲:-1=全チャネル、0~:指定チャネル]</param>
		/// <param name="bounds">始点とサイズ (幅,高さが 0 の場合はそれぞれ最大値に正規化します。)</param>
		/// <returns>
		///		現在のオブジェクトの画像領域にアタッチした新しいオブジェクトを生成して返します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe CxImage Child(int ch, TxRectangleI bounds)
		{
			CxImage child = new CxImage();
			child.Attach(this, ch, bounds);
			return child;
		}

		#endregion

		#region メソッド: (Clone)

		/// <summary>
		/// オブジェクトのクローンの生成 (型変換)
		/// </summary>
		/// <param name="model">複製先の型 [Type: None=同一型、その他=指定型] [Pack: 0=同一パック数、1~=指定パック数]</param>
		/// <param name="channels">複製先のチャネル数 [範囲:0=同一チャネル数、1~16=指定チャネル数]</param>
		/// <param name="scale">スケーリングする際の倍率 [0=元の値をそのまま複製する、その他=元の画素値に指定値を乗算する]</param>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual CxImage Clone(TxModel model, int channels = 0, double scale = 0)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);

			TxModel dst_model = new TxModel();
			dst_model.Type = (model.Type != ExType.None) ? model.Type : this.Model.Type;
			dst_model.Pack = (model.Pack != 0) ? model.Pack : this.Model.Pack;

			int dst_channels = (channels <= 0) ? this.Channels : channels;

			CxImage clone = new CxImage(
				this.Width,
				this.Height,
				dst_model,
				dst_channels
				);

			clone.Filter().Copy(this, scale);

			return clone;
		}

		#endregion

		#region メソッド: (Clear)

		/// <summary>
		/// 要素のクリア
		/// </summary>
		/// <param name="value">塗りつぶす濃度。</param>
		/// <param name="mask">マスク (省略する場合は null を指定してください。)</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe void Clear(object value, IxModule mask = null)
		{
			if (value == null)
				throw new CxException(ExStatus.InvalidParam);
			HxModule hdst = ((IxModule)this).GetHandle();
			HxModule hmask = (mask == null) ? HxModule.Zero : ((IxModule)mask).GetHandle();

			ExStatus status = ExStatus.Unsupported;
			TxModel model = ModelOf.From(value.GetType());
			switch (model.Type)
			{
				case ExType.None:
					if (value is Color)
					{
						var rgb_value = (TxRGB8x4)((Color)value);
						var rgb_model = ModelOf.From(rgb_value.GetType());
						status = xie_core.fnXIE_Core_Image_Clear(hdst, hmask, new IntPtr(&rgb_value), rgb_model);
					}
					break;
				case ExType.Ptr:
					break;
				default:
					using (CxArray temp = new CxArray(1, model))
					{
						IntPtr addr = temp.Address();
						System.Runtime.InteropServices.Marshal.StructureToPtr(value, addr, false);
						status = xie_core.fnXIE_Core_Image_Clear(hdst, hmask, addr, model);
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
		/// <param name="mask">マスク (省略する場合は null を指定してください。)</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual void ClearEx(object value, int index, int count, IxModule mask = null)
		{
			if (value == null)
				throw new CxException(ExStatus.InvalidParam);
			HxModule hdst = ((IxModule)this).GetHandle();
			HxModule hmask = (mask == null) ? HxModule.Zero : ((IxModule)mask).GetHandle();

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
						status = xie_core.fnXIE_Core_Image_ClearEx(hdst, hmask, addr, model, index, count);
					}
					break;
			}
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Measure)

		/// <summary>
		/// ビット深度 (bits) の計算
		/// </summary>
		/// <param name="ch">チャネル指標 [範囲:-1=すべてのチャネル、0~:指定チャネル]</param>
		/// <param name="mask">マスク (省略する場合は null を指定してください。)</param>
		/// <returns>
		///		現在の濃度値を表すことができる最小のビット深度 (bits) を計算します。<br/>
		///		未確保の場合は例外を発行します。<br/>
		///		正常に計算できた場合は 1 以上の値を返します。
		///		すべての濃度が 0 の時は 0 を返します。
		/// </returns>
		public int CalcDepth(int ch, IxModule mask = null)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);
			int depth = 0;
			HxModule hsrc = ((IxModule)this).GetHandle();
			HxModule hmask = (mask == null) ? HxModule.Zero : ((IxModule)mask).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_CalcDepth(hsrc, hmask, ch, out depth);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return depth;
		}

		/// <summary>
		/// 統計
		/// </summary>
		/// <param name="ch">チャネル指標 [範囲:0~]</param>
		/// <param name="mask">マスク (省略する場合は null を指定してください。)</param>
		/// <returns>
		///		指定されたチャネルの統計データを返します。
		///		未確保の場合は例外を発行します。<br/>
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public TxStatistics Statistics(int ch, IxModule mask = null)
		{
			var result = new TxStatistics();
			HxModule hsrc = ((IxModule)this).GetHandle();
			HxModule hmask = (mask == null) ? HxModule.Zero : ((IxModule)mask).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Statistics(hsrc, hmask, ch, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		/// <summary>
		/// 抽出
		/// </summary>
		/// <param name="ch">チャネル指標 [範囲:0~]</param>
		/// <param name="sy">抽出位置(Y) [0~Height-1]</param>
		/// <param name="sx">抽出位置(X) [0~Width-1]</param>
		/// <param name="length">抽出する長さ [範囲:0~]</param>
		/// <param name="dir">走査方向 [X=行抽出、Y=列抽出]</param>
		/// <param name="mask">マスク (省略する場合は null を指定してください。)</param>
		/// <returns>
		///		指定された範囲の要素を返します。
		///		配列の要素は自身と同一型です。要素数は抽出する長さと同一です。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public CxArray Extract(int ch, int sy, int sx, int length, ExScanDir dir, IxModule mask = null)
		{
			CxArray result = new CxArray();
			HxModule hsrc = ((IxModule)this).GetHandle();
			HxModule hresult = ((IxModule)result).GetHandle();
			HxModule hmask = (mask == null) ? HxModule.Zero : ((IxModule)mask).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Image_Extract(hsrc, hmask, ch, sy, sx, length, dir, hresult);
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
		///		画像オブジェクトフィルタを返します。
		/// </returns>
		public virtual CxImageFilter Filter()
		{
			return new CxImageFilter(this);
		}

		/// <summary>
		/// フィルタの取得 (マスク画像指定)
		/// </summary>
		/// <param name="mask">マスク画像</param>
		/// <returns>
		///		画像フィルタを返します。
		/// </returns>
		public virtual CxImageFilter Filter(IxModule mask)
		{
			return new CxImageFilter(this, mask);
		}

		#endregion

		#region メソッド: (Scanner)

		/// <summary>
		/// 配列走査構造体の取得
		/// </summary>
		/// <param name="ch">チャネル指標 [0~(Channels-1)]</param>
		/// <returns>
		///		２次元配列走査構造体を返します。
		/// </returns>
		public virtual TxScanner2D Scanner(int ch)
		{
			return new TxScanner2D(this[ch], this.Width, this.Height, this.Model, this.Stride);
		}

		/// <summary>
		/// 配列走査構造体の取得
		/// </summary>
		/// <param name="ch">チャネル指標 [0~(Channels-1)]</param>
		/// <param name="bounds">始点とサイズ</param>
		/// <returns>
		///		２次元配列走査構造体を返します。
		/// </returns>
		public virtual TxScanner2D Scanner(int ch, TxRectangleI bounds)
		{
			if (!(0 <= ch && ch < this.Channels))
				throw new CxException(ExStatus.InvalidParam);
			if (bounds.X < 0 || bounds.Y < 0)
				throw new CxException(ExStatus.InvalidParam);
			if (!(bounds.X + bounds.Width <= this.Width))
				throw new CxException(ExStatus.InvalidParam);
			if (!(bounds.Y + bounds.Height <= this.Height))
				throw new CxException(ExStatus.InvalidParam);
			return new TxScanner2D(this[ch, bounds.Y, bounds.X], bounds.Width, bounds.Height, this.Model, this.Stride);
		}

		#endregion

		#region メソッド: (変換系)

		/// <summary>
		/// Bitmap からの変換
		/// </summary>
		/// <param name="src">変換元 [8bpp,24bpp,32bpp]</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator CxImage(Bitmap src)
		{
			CxImage dst = new CxImage();
			dst.CopyFrom(src);
			return dst;
		}

		/// <summary>
		/// Bitmap への変換
		/// </summary>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static explicit operator Bitmap(CxImage src)
		{
			return src.ToBitmap();
		}

		/// <summary>
		/// Bitmap への変換
		/// </summary>
		/// <param name="use_alpha">32bpp の場合、アルファフィールドを使うか否か</param>
		/// <returns>
		///		Bitmap に変換して返します。
		/// </returns>
		public virtual Bitmap ToBitmap(bool use_alpha = false)
		{
			using (CxImage act = this.Child())
			{
				int width = act.Width;
				int height = act.Height;
				int channels = act.Channels;
				TxModel model = act.Model;
				PixelFormat format;

				#region 変換.
				if (model.Type == ExType.U8)
				{
					if (model.Pack == 1)
					{
						if (channels == 1)
						{
							if (act.Stride != Axi.CalcStride(act.Model, act.Width, 4))
							{
								act.Resize(width, height, ModelOf.From(typeof(byte)), 1);
								act.Filter().Copy(this);
							}
							format = PixelFormat.Format8bppIndexed;
						}
						else if (channels == 3)
						{
							act.Resize(width, height, ModelOf.From(typeof(TxBGR8x3)), 1);
							act.Filter().RgbToBgr(this);
							format = PixelFormat.Format24bppRgb;
						}
						else if (channels == 4)
						{
							act.Resize(width, height, ModelOf.From(typeof(TxBGR8x4)), 1);
							if (use_alpha)
							{
								act.Filter().Copy(this);
								act.Filter().RgbToBgr(this);
								format = PixelFormat.Format32bppArgb;
							}
							else
							{
								act.Filter().RgbToBgr(this);
								format = PixelFormat.Format32bppRgb;
							}
						}
						else
						{
							throw new CxException(ExStatus.Unsupported);
						}
					}
					else if (model.Pack == 3)
					{
						if (channels != 1)
							throw new CxException(ExStatus.Unsupported);
						else
						{
							act.Resize(width, height, ModelOf.From(typeof(TxBGR8x3)), 1);
							act.Filter().RgbToBgr(this);
							format = PixelFormat.Format24bppRgb;
						}
					}
					else if (model.Pack == 4)
					{
						if (channels != 1)
							throw new CxException(ExStatus.Unsupported);
						else
						{
							act.Resize(width, height, ModelOf.From(typeof(TxBGR8x4)), 1);
							if (use_alpha)
							{
								act.Filter().Copy(this);
								act.Filter().RgbToBgr(this);
								format = PixelFormat.Format32bppArgb;
							}
							else
							{
								act.Filter().RgbToBgr(this);
								format = PixelFormat.Format32bppRgb;
							}
						}
					}
					else
					{
						throw new CxException(ExStatus.Unsupported);
					}
				}
				else
				{
					throw new CxException(ExStatus.Unsupported);
				}
				#endregion

				Bitmap dst = new Bitmap(act.Width, act.Height, format);

				if (format == PixelFormat.Format8bppIndexed)
				{
					ColorPalette palette = dst.Palette;
					int length = palette.Entries.Length;
					for (int i = 0; i < length; i++)
						palette.Entries[i] = Color.FromArgb(i, i, i);
					dst.Palette = palette;
				}

				#region コピー.
				BitmapData bmpData = null;
				try
				{
					bmpData = new BitmapData();
					bmpData.Width = act.Width;
					bmpData.Height = act.Height;
					bmpData.Stride = act.Stride;
					bmpData.PixelFormat = format;
					bmpData.Scan0 = act[0, 0, 0];

					dst.LockBits(
						new Rectangle(0, 0, dst.Width, dst.Height),
						ImageLockMode.WriteOnly | ImageLockMode.UserInputBuffer,
						dst.PixelFormat,
						bmpData
						);
				}
				finally
				{
					dst.UnlockBits(bmpData);
				}
				#endregion

				return dst;
			}
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// ISerializable の実装: シグネチャコンストラクタ
		/// </summary>
		/// <param name="info">データ読み込み先の SerializationInfo</param>
		/// <param name="context">シリアル化先</param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public unsafe CxImage(SerializationInfo info, StreamingContext context)
		{
			_Constructor();

			TxModel model = TxModel.Default;
			int channels = 0;
			int width = 0;
			int height = 0;

			// --------------------------------------------------
			foreach (SerializationEntry entry in info)
			{
				switch (entry.Name)
				{
					case "Width":
						width = (int)info.GetValue(entry.Name, typeof(int));
						break;
					case "Height":
						height = (int)info.GetValue(entry.Name, typeof(int));
						break;
					case "Model":
						model = (TxModel)info.GetValue(entry.Name, typeof(TxModel));
						break;
					case "Channels":
						channels = (int)info.GetValue(entry.Name, typeof(int));
						break;
				}
			}

			if (channels > 0 && width > 0 && height > 0)
			{
				this.Resize(width, height, model, channels);

				int pixel_size = this.Model.Size;
				int stride = pixel_size * width;

				foreach (SerializationEntry entry in info)
				{
					if (entry.Name.StartsWith("Data") == false) continue;
					if (entry.Name.Length <= 4) continue;

					int ch = Convert.ToInt32(entry.Name.Substring(4));
					if (!(0 <= ch && ch < channels)) continue;

					byte[] data = (byte[])info.GetValue(entry.Name, typeof(byte[]));
					if (data.Length != stride * height) continue;

					// --------------------------------------------------
					int index = 0;
					{
						for (int y = 0; y < height; y++)
						{
							byte* addr = (byte*)this[ch, y, 0];
							for (int x = 0; x < stride; x++)
								addr[x] = data[index++];
						}
					}
				}
			}
		}

		/// <summary>
		/// ISerializable の実装: SerializationInfo に、オブジェクトをシリアル化するために必要なデータを設定します。
		/// </summary>
		/// <param name="info">データ読み込み先の SerializationInfo</param>
		/// <param name="context">シリアル化先</param>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		unsafe void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			int width = this.Width;
			int height = this.Height;
			TxModel model = this.Model;
			int channels = this.Channels;

			info.AddValue("Width", width);
			info.AddValue("Height", height);
			info.AddValue("Model", model);
			info.AddValue("Channels", channels);

			if (this.IsValid)
			{
				int pixel_size = this.Model.Size;
				int stride = pixel_size * width;

				for (int ch = 0; ch < channels; ch++)
				{
					byte[] data = new byte[stride * height];
					long index = 0;
					for (int y = 0; y < height; y++)
					{
						byte* addr = (byte*)this[ch, y, 0];
						for (int x = 0; x < stride; x++)
							data[index++] = addr[x];
					}

					info.AddValue(string.Format("Data{0}", ch), data);
				}
			}
		}

		#endregion

		#region IXmlSerializable の実装:

		/// <summary>
		/// IXmlSerializable の実装: オブジェクトの XML 表現からオブジェクトを生成します。
		/// </summary>
		/// <param name="reader">オブジェクトの逆シリアル化元である XmlReader ストリーム。</param>
		unsafe void IXmlSerializable.ReadXml(XmlReader reader)
		{
			reader.ReadStartElement();
			// --------------------------------------------------
			int width = 0;
			int height = 0;
			int channels = 0;
			TxModel model = TxModel.Default;
			if (reader.Name == "Width")
				width = XIE.AxiXml.GetValue<int>(reader, reader.Name);
			if (reader.Name == "Height")
				height = XIE.AxiXml.GetValue<int>(reader, reader.Name);
			if (reader.Name == "Model")
				model = XIE.AxiXml.GetValue<TxModel>(reader, reader.Name);
			if (reader.Name == "Channels")
				channels = XIE.AxiXml.GetValue<int>(reader, reader.Name);
			// --------------------------------------------------
			if (width > 0 && height > 0)
			{
				Resize(width, height, model, channels);
			}
			reader.ReadEndElement();
		}

		/// <summary>
		/// IXmlSerializable の実装: オブジェクトを XML 表現に変換します。
		/// </summary>
		/// <param name="writer">オブジェクトのシリアル化先の XmlWriter ストリーム。</param>
		unsafe void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			XIE.AxiXml.AddValue(writer, "Width", this.Width);
			XIE.AxiXml.AddValue(writer, "Height", this.Height);
			XIE.AxiXml.AddValue(writer, "Model", this.Model);
			XIE.AxiXml.AddValue(writer, "Channels", this.Channels);
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
